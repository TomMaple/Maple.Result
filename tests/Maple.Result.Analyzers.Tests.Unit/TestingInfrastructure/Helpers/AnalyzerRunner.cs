// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Maple.Results.Analyzers.Tests.Unit.TestingInfrastructure.Helpers;

/// <summary>
///     Compiles a snippet against the library assembly and runs analyzers on it, so that a test can assert what a rule reports for a call
///     shape. The snippet shell (usings and members) is the one of the binding tests of the library, so that their rows can be used as they
///     are.
/// </summary>
internal static class AnalyzerRunner
{
    private const string Usings = """
        using System;
        using System.Collections.Generic;
        using System.IO;
        using System.Linq;
        using System.Text;
        using System.Threading;
        using System.Threading.Tasks;
        using Maple.Results;
        using Maple.Results.Extensions;

        """;

    private const string ClassStart = """

        public static class Snippet
        {
            static readonly Error err = Error.Failure(ErrorUri.None, "e");
            static int Compute() => 5;
            static Task<int> GetIntAsync() => Task.FromResult(5);
            static ValueTask<int> GetIntValueTask() => new(5);
            static Task SaveAsync() => Task.CompletedTask;
            static Result GetResult() => Result.Success();
            static Result<int> GetResultOfInt() => Result.FromValue(1);
            static Error GetError() => err;
            static Result<int> ok = Result.FromValue(1);

        """;

    private const string Postlude = """

            }
        }
        """;

    private static readonly ImmutableArray<MetadataReference> References = BuildReferences();

    /// <summary>
    ///     Every analyzer of the analyzer assembly, for the tests that assert that nothing is reported.
    /// </summary>
    public static ImmutableArray<DiagnosticAnalyzer> AllAnalyzers { get; } =
    [
        new ConvertedResultValueAnalyzer(),
        new DiscardedResultAnalyzer(),
        new DroppedResultAnalyzer(),
        new IgnoredValueAnalyzer(),
        new InferredResultValueAnalyzer(),
        new UnawaitedTaskAnalyzer(),
    ];

    /// <summary>
    ///     Compiles the <paramref name="statements" /> as the body of a method of a class with the <paramref name="members" />, and runs the
    ///     <typeparamref name="TAnalyzer" /> on it.
    /// </summary>
    public static AnalysisResult Run<TAnalyzer>(string statements, string members = "")
        where TAnalyzer : DiagnosticAnalyzer, new() =>
        Run(statements, members, [new TAnalyzer()]);

    /// <summary>
    ///     Compiles the <paramref name="statements" /> as the body of a method of a class with the <paramref name="members" />, and runs the
    ///     <paramref name="analyzers" /> on it. The method is asynchronous when the statements await something.
    /// </summary>
    public static AnalysisResult Run(string statements, string members, ImmutableArray<DiagnosticAnalyzer> analyzers)
    {
        var runStart = statements.Contains("await ", StringComparison.Ordinal)
            ? "\n    static async Task Run()\n    {\n"
            : "\n    static void Run()\n    {\n";
        var source = Usings + ClassStart + members + runStart + statements + Postlude;

        return RunSource(source, analyzers);
    }

    /// <summary>
    ///     Compiles the <paramref name="source" /> as it is and runs the <paramref name="analyzers" /> on it.
    /// </summary>
    /// <param name="source">A whole source file.</param>
    /// <param name="analyzers">The analyzers to run.</param>
    /// <param name="referenceLibrary"><see langword="false" /> to compile without a reference to the library.</param>
    /// <param name="suppressedIds">The identifiers that the compilation suppresses, as <c>NoWarn</c> of a project does.</param>
    public static AnalysisResult RunSource(
        string source,
        ImmutableArray<DiagnosticAnalyzer> analyzers,
        bool referenceLibrary = true,
        params string[] suppressedIds)
    {
        var tree = CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Latest));
        var references = referenceLibrary
            ? References
            : References.RemoveAt(References.Length - 1);
        var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable)
            .WithSpecificDiagnosticOptions(suppressedIds.ToDictionary(id => id, _ => ReportDiagnostic.Suppress));

        var compilation = CSharpCompilation.Create("Snippet", [tree], references, options);

        var errors = compilation.GetDiagnostics()
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ToArray();

        var diagnostics = compilation.WithAnalyzers(analyzers)
            .GetAnalyzerDiagnosticsAsync()
            .GetAwaiter()
            .GetResult()
            .OrderBy(d => d.Location.SourceSpan.Start)
            .ToArray();

        return new AnalysisResult(tree, errors, diagnostics);
    }

    private static ImmutableArray<MetadataReference> BuildReferences()
    {
        var platform = (string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!;
        var references = platform
            .Split(Path.PathSeparator)
            .Select(path => (MetadataReference)MetadataReference.CreateFromFile(path))
            .ToList();

        references.Add(MetadataReference.CreateFromFile(typeof(Result).Assembly.Location));

        return [.. references];
    }
}

/// <summary>
///     What the compiler and the analyzers reported for a snippet.
/// </summary>
internal sealed class AnalysisResult(SyntaxTree tree, Diagnostic[] compilerErrors, Diagnostic[] diagnostics)
{
    /// <summary>The identifiers of the errors of the compiler; a snippet that a rule is tested on has none.</summary>
    public IReadOnlyList<string> CompilerErrorIds => [.. compilerErrors.Select(d => d.Id)];

    /// <summary>The errors of the compiler, for the message of a failed assertion.</summary>
    public string CompilerErrors => string.Join(Environment.NewLine, compilerErrors.Select(d => d.ToString()));

    /// <summary>The identifiers of the diagnostics of the analyzers, in the order of their positions.</summary>
    public IReadOnlyList<string> DiagnosticIds => [.. diagnostics.Select(d => d.Id)];

    /// <summary>The diagnostics of the analyzers, in the order of their positions.</summary>
    public IReadOnlyList<Diagnostic> Diagnostics => diagnostics;

    /// <summary>The text that the <paramref name="diagnostic" /> marks.</summary>
    public string MarkedText(Diagnostic diagnostic) => TextAt(diagnostic.Location);

    /// <summary>The text of the snippet at the <paramref name="location" />.</summary>
    public string TextAt(Location location) => tree.GetText().ToString(location.SourceSpan);
}
