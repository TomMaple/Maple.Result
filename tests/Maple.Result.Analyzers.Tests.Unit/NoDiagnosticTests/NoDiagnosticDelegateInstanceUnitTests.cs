// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Analyzers.Tests.Unit.TestingInfrastructure.Helpers;

namespace Maple.Results.Analyzers.Tests.Unit.NoDiagnosticTests;

/// <summary>
///     Tests that no analyzer reports a delegate that is passed as an instance (a variable, or a delegate created from one): the rules look
///     at a lambda or a method group, where the function is written at the call.
/// </summary>
public class NoDiagnosticDelegateInstanceUnitTests
{
    private const string Members = """
        static readonly Action<int> show = x => Console.WriteLine(x);
        static readonly Func<int, Task> save = x => Task.CompletedTask;
        static readonly Func<int, int> twice = x => x * 2;
        """;

    [Theory]
    [InlineData("var r = ok.IfSuccess(show);")]
    [InlineData("var r = ok.IfSuccess(new Action<int>(show));")]
    [InlineData("var r = ok.IfSuccessAsync(save);")]
    [InlineData("var r = ok.IfSuccessAsync(new Func<int, Task>(save));")]
    [InlineData("var r = ok.Then(twice);")]
    public void Analyze_DelegateInstance_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
