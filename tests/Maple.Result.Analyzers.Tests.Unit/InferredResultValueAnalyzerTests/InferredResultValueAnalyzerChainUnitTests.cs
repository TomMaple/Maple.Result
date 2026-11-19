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

namespace Maple.Results.Analyzers.Tests.Unit.InferredResultValueAnalyzerTests;

/// <summary>
///     Tests that the <see cref="InferredResultValueAnalyzer" /> (MR0001) reports a task inferred as the value of a result,
///     on call shapes that compile without a diagnostic of the compiler: chained calls.
/// </summary>
public class InferredResultValueAnalyzerChainUnitTests
{
    private const string Members = """
        static Task<Result<int>> okTask = Task.FromResult(Result.FromValue(1));
        static ValueTask<Result<int>> okVt = new(Result.FromValue(1));
        static Result done = Result.Success();
        static Task<Result> doneTask = Task.FromResult(Result.Success());
        static ValueTask<Result> doneVt = new(Result.Success());
        static ValueTask SaveValueTask() => default;
        """;

    [Theory]
    [InlineData("var r = ok.Then(x => GetIntAsync()).Then(t => 1);")]
    [InlineData("var r = ok.Then(x => GetIntAsync()).IfSuccess(t => { });")]
    [InlineData("var r = ok.Then(x => GetIntAsync()).Value;")]
    [InlineData("var r = ok.Then(x => GetIntAsync()).Match(t => 1, e => 0);")]
    [InlineData("var r = ok.Then(x => SaveAsync()).Then(t => 1);")]
    [InlineData("var r = ok.Then(x => SaveValueTask()).Then(t => 1);")]
    [InlineData("var r = ok.Then(x => GetIntValueTask()).Then(t => 1);")]
    [InlineData("Result<Task<int>> r = ok.Then(x => GetIntAsync());")]
    [InlineData("var r = done.Then(() => GetIntAsync()).Then(t => 1);")]
    [InlineData("var r = done.Then(() => SaveValueTask()).Value;")]
    [InlineData("Result<ValueTask<int>> r = done.Then(() => GetIntValueTask());")]
    [InlineData("var r = (from x in ok select GetIntAsync()).Value;")]
    [InlineData("var r = (from x in ok select SaveValueTask()).Then(t => 1);")]
    [InlineData("var r = (from x in ok select GetIntValueTask()).Value;")]
    [InlineData("var r = (from x in done select GetIntAsync()).Value;")]
    [InlineData("async Task L() { var r = (await (from x in okTask select GetIntAsync())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in okVt select SaveValueTask())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in doneTask select GetIntValueTask())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in doneVt select GetIntAsync())).Value; } _ = L();")]
    [InlineData("var r = (from x in ok from y in ok select GetIntAsync()).Value;")]
    [InlineData("var r = (from x in ok from y in done select SaveValueTask()).Value;")]
    [InlineData("var r = (from x in done from y in ok select GetIntValueTask()).Value;")]
    [InlineData("var r = (from x in done from y in done select GetIntAsync()).Value;")]
    [InlineData("async Task L() { var r = (await (from x in okTask from y in okTask select GetIntAsync())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in okTask from y in ok select SaveValueTask())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in ok from y in okTask select GetIntAsync())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in okVt from y in okVt select GetIntValueTask())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in ok from y in okVt select GetIntAsync())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in doneTask from y in doneTask select GetIntAsync())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in doneVt from y in done select SaveAsync())).Value; } _ = L();")]
    public void Analyze_ChainedCall_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<InferredResultValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.InferredResultValueId);
    }
}
