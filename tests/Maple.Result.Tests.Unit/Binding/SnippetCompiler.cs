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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Maple.Results.Tests.Unit.Binding;

/// <summary>
/// Compiles a snippet against the library assembly, so that a test can assert the diagnostic (e.g., CS0619 of an
/// overload trap) or the method that a call binds to, which a reflection test of the attributes cannot show.
/// </summary>
internal static class SnippetCompiler
{
    private const string Usings = """
        using System;
        using System.Collections.Generic;
        using System.IO;
        using System.Linq;
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

    private const string RunStart = """

            static void Run()
            {
        """;

    private const string Postlude = """

            }
        }
        """;

    private static readonly ImmutableArray<MetadataReference> References = BuildReferences();

    /// <summary>
    ///     Compiles the statements as the body of a method, with the namespace of the extension methods imported (it holds the
    ///     <c>Task</c>-based and the <c>ValueTask</c>-based methods, #248), and returns the semantic model and the error diagnostics.
    /// </summary>
    /// <param name="statements">The statements of the method body.</param>
    /// <param name="members">Members of the snippet class (e.g., helper methods, fields or a nested record), placed before the method.</param>
    public static SnippetResult Compile(string statements, string members = "")
    {
        var source = Usings + ClassStart + members + RunStart + statements + Postlude;
        var tree = CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Latest));

        var compilation = CSharpCompilation.Create(
            "Snippet",
            [tree],
            References,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));

        var errors = compilation.GetDiagnostics()
            .Where(d => d.Severity == DiagnosticSeverity.Error)
            .ToArray();

        return new SnippetResult(compilation, tree, errors);
    }

    private static ImmutableArray<MetadataReference> BuildReferences()
    {
        var platform = (string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!;
        var references = platform
            .Split(Path.PathSeparator)
            .Where(path => Path.GetFileName(path) is "System.Runtime.dll" or "System.Private.CoreLib.dll" or "System.Linq.dll"
                or "System.Collections.dll" or "System.Threading.Tasks.dll" or "System.Runtime.InteropServices.dll"
                or "System.ComponentModel.dll" or "System.Memory.dll" or "netstandard.dll" or "System.Console.dll")
            .Select(path => (MetadataReference)MetadataReference.CreateFromFile(path))
            .ToList();

        references.Add(MetadataReference.CreateFromFile(typeof(Result).Assembly.Location));
        return [.. references];
    }
}

internal sealed class SnippetResult(CSharpCompilation compilation, SyntaxTree tree, Diagnostic[] errors)
{
    /// <summary>The ids of the error diagnostics, e.g., <c>CS0619</c>.</summary>
    public IReadOnlyList<string> ErrorIds => [.. errors.Select(d => d.Id)];

    public IReadOnlyList<Diagnostic> Errors => errors;

    /// <summary>The signature of the method that the first call of <paramref name="methodName"/> binds to.</summary>
    public string BoundMethod(string methodName)
    {
        var model = compilation.GetSemanticModel(tree);
        var invocation = tree.GetRoot().DescendantNodes().OfType<InvocationExpressionSyntax>()
            .First(i => i.Expression.ToString().EndsWith(methodName, StringComparison.Ordinal)
                        || i.Expression.ToString().Contains(methodName + "<", StringComparison.Ordinal));

        var symbol = model.GetSymbolInfo(invocation).Symbol as IMethodSymbol
                     ?? throw new InvalidOperationException($"The call of {methodName} did not bind to a method.");

        return symbol.ToDisplayString(SymbolDisplayFormat.CSharpShortErrorMessageFormat);
    }
}
