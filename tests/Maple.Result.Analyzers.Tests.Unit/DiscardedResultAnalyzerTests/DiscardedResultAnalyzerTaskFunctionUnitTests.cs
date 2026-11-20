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

namespace Maple.Results.Analyzers.Tests.Unit.DiscardedResultAnalyzerTests;

/// <summary>
///     Tests that the <see cref="DiscardedResultAnalyzer" /> (MR0006) reports a result that a statement discards,
///     on call shapes that compile without a diagnostic of the compiler: functions that return a task, and Result.Try.
/// </summary>
public class DiscardedResultAnalyzerTaskFunctionUnitTests
{
    private const string Members = "";

    [Theory]
    [InlineData("Result.Try(async () => { await Task.Yield(); return 5; }, (IOException e) => err);")]
    [InlineData("Result.Try(() => GetIntValueTask(), (IOException e) => err);")]
    [InlineData("Result.Try(GetIntValueTask, (IOException e) => err);")]
    [InlineData("Result.Try(async () => await GetIntAsync(), (IOException e) => err);")]
    [InlineData("Result.Try(() => GetIntAsync(), (IOException e) => err);")]
    [InlineData("Result.Try(() => SaveAsync(), (IOException e) => err);")]
    [InlineData("Result.Try(() => GetIntValueTask(), e => err);")]
    [InlineData("Result.Try(() => GetIntAsync(), e => err);")]
    [InlineData("Result.Try(() => 5, (IOException e) => err);")]
    [InlineData("Result.Try(() => { }, (IOException e) => err);")]
    [InlineData("Result.Try<int, IOException>(() => 5, e => err);")]
    [InlineData("Result.Try(() => Compute(), (IOException e) => err);")]
    [InlineData("Result.Try(Compute, (IOException e) => err);")]
    [InlineData("Result.Try(() => GetResult(), e => err);")]
    [InlineData("Result.Try(() => GetResultOfInt(), e => err);")]
    [InlineData("Result.Try(() => GetError(), e => err);")]
    [InlineData("ok.Then(x => x + 1);")]
    [InlineData("ok.Else(e => 0);")]
    public void Analyze_FunctionReturningTask_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<DiscardedResultAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.DiscardedResultId);
    }
}
