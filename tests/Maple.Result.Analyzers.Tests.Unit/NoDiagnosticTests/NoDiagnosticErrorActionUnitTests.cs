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
///     Tests that no analyzer reports the call shapes that are correct, or that keep a task or an <c>Error</c> as a value on
///     purpose: error actions of IfError.
/// </summary>
public class NoDiagnosticErrorActionUnitTests
{
    private const string Members = """
        static Task<int> LogIntAsync(Error error) => Task.FromResult(1);
        static ValueTask LogValueTask(Error error) => default;
        static ValueTask<int> LogIntValueTask(Error error) => new(1);
        static Task LogAsync(Error error) => Task.CompletedTask;
        """;

    [Theory]
    [InlineData("Task t = ok.Match(x => SaveAsync(), e => LogAsync(e));")]
    [InlineData("Task t = GetResult().Match(() => SaveAsync(), e => LogAsync(e));")]
    [InlineData("ok.Switch(x => { }, e => { _ = LogAsync(e); });")]
    [InlineData("Result r = GetResult().IfError(e => { _ = LogValueTask(e); });")]
    public void Analyze_ErrorActionThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
