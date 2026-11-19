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
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Maple.Results.Analyzers.CodeFixes.Tests.Unit.TestingInfrastructure.Helpers;

/// <summary>
///     Verifies a code fix with the test library of the Roslyn SDK: the analyzer must report exactly the marked diagnostics
///     (<c>{|MR0001:text|}</c>), and applying the fix, one diagnostic at a time and as “fix all”, must give the expected code, which must
///     compile.
/// </summary>
internal static class CodeFixVerifier<TAnalyzer, TCodeFix>
    where TAnalyzer : DiagnosticAnalyzer, new()
    where TCodeFix : CodeFixProvider, new()
{
    private const string Usings = """
        using System;
        using System.Collections.Generic;
        using System.Threading.Tasks;
        using Maple.Results;
        using Maple.Results.Extensions;

        """;

    private const string ClassStart = """

        public static class Snippet
        {
            static readonly Error err = Error.Failure(ErrorUri.None, "e");
            static Result<int> ok = Result.FromValue(1);
            static Task<Result<int>> okTask = Task.FromResult(ok);
            static int Compute() => 5;
            static Error GetError() => err;
            static Task<int> GetIntAsync() => Task.FromResult(5);
            static Task SaveAsync() => Task.CompletedTask;
            static Result GetResult() => Result.Success();
            static Result<int> GetResultOfInt() => Result.FromValue(1);
            static Task<Result> GetResultAsync() => Task.FromResult(Result.Success());
            static Task<Result<int>> GetResultOfIntAsync() => Task.FromResult(ok);
            static Task LogAsync(Error e) => Task.CompletedTask;

        """;

    private static readonly ImmutableArray<MetadataReference> References = BuildReferences();

    /// <summary>
    ///     Compiles the <paramref name="statements" /> as the body of a method, checks the marked diagnostics, applies the code fix and
    ///     compares the result with the <paramref name="fixedStatements" />. The method is asynchronous when the statements await something.
    /// </summary>
    public static Task VerifyAsync(string statements, string fixedStatements) =>
        VerifySourceAsync(Wrap(statements), Wrap(fixedStatements));

    /// <summary>
    ///     Compiles the <paramref name="members" /> as members of a class (methods, fields), checks the marked diagnostics, applies the code
    ///     fix and compares the result with the <paramref name="fixedMembers" />. Each line of the members is indented by four spaces.
    /// </summary>
    public static Task VerifyMembersAsync(string members, string fixedMembers) =>
        VerifySourceAsync(Usings + ClassStart + "\n" + members + "\n}\n", Usings + ClassStart + "\n" + fixedMembers + "\n}\n");

    /// <summary>
    ///     Compiles the <paramref name="statements" /> as the top-level statements of a program, checks the marked diagnostics, applies the
    ///     code fix and compares the result with the <paramref name="fixedStatements" />.
    /// </summary>
    public static Task VerifyProgramAsync(string statements, string fixedStatements) =>
        VerifySourceAsync(Usings + "\n" + statements + "\n", Usings + "\n" + fixedStatements + "\n", OutputKind.ConsoleApplication);

    private static Task VerifySourceAsync(string source, string fixedSource, OutputKind outputKind = OutputKind.DynamicallyLinkedLibrary)
    {
        var test = new CSharpCodeFixTest<TAnalyzer, TCodeFix, DefaultVerifier>
        {
            TestCode = source,
            FixedCode = fixedSource,
            TestState = { OutputKind = outputKind },
            FixedState = { OutputKind = outputKind },

            // The references of the running tests, so that nothing is downloaded: the platform assemblies and the library.
            ReferenceAssemblies = new ReferenceAssemblies("net10.0"),

            // The rules that replace a compile error cannot be suppressed and are reported in generated code, on purpose.
            TestBehaviors = TestBehaviors.SkipSuppressionCheck | TestBehaviors.SkipGeneratedCodeCheck,
        };

        test.TestState.AdditionalReferences.AddRange(References);
        test.FixedState.AdditionalReferences.AddRange(References);

        return test.RunAsync(TestContext.Current.CancellationToken);
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

    private static string Wrap(string statements)
    {
        var runStart = statements.Contains("await ", StringComparison.Ordinal)
            ? "\n    static async Task Run()\n    {\n        "
            : "\n    static void Run()\n    {\n        ";

        return Usings + ClassStart + runStart + statements + "\n    }\n}\n";
    }
}
