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
///     purpose: functions that return a task, and Result.Try.
/// </summary>
public class NoDiagnosticTaskFunctionUnitTests
{
    private const string Members = "";

    [Theory]
    [InlineData("ok.IfSuccess(x => { });")]
    [InlineData("ok.IfError(e => { _ = SaveAsync(); });")]
    [InlineData("ok.IfSuccess(x => _ = SaveAsync());")]
    [InlineData("ok.IfError(e => _ = SaveAsync());")]
    [InlineData("Task? last = null; ok.IfSuccess(x => last = SaveAsync());")]
    [InlineData("var q = from x in ok select x + 1;")]
    [InlineData("var r = Result.Try(() => GetResultOfInt(), e => err);")]
    [InlineData("var r = Result.Try(() => GetResult(), e => err);")]
    [InlineData("var r = Result.Try(() => GetError(), e => err);")]
    [InlineData("var r = Result.Try(() => Compute(), e => err);")]
    public void Analyze_FunctionReturningTaskThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
