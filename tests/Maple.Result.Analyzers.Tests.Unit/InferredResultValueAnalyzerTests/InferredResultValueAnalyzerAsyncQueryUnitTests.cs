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
///     on call shapes that compile without a diagnostic of the compiler: asynchronous LINQ queries and operators.
/// </summary>
public class InferredResultValueAnalyzerAsyncQueryUnitTests
{
    private const string Members = """
        static readonly Result done = Result.Success();
        static Task<Result<int>> GetTask() => Task.FromResult<Result<int>>(1);
        static ValueTask<Result<int>> GetValueTask() => new(Result.FromValue(1));
        static Task<Result> GetDoneTask() => Task.FromResult(Result.Success());
        static ValueTask<Result> GetDoneValueTask() => new(Result.Success());
        static Result<int> GetOther(int x) => x;
        static Task<Result<int>> GetOtherAsync(int x) => Task.FromResult<Result<int>>(x);
        static ValueTask<Result<int>> GetOtherValueAsync(int x) => new(Result.FromValue(x));
        static Result Check(int x) => Result.Success();
        static Task<Result> CheckAsync(int x) => Task.FromResult(Result.Success());
        static ValueTask<Result> CheckValueAsync(int x) => new(Result.Success());
        static Task<int> GetIntAsync(int x) => Task.FromResult(x);
        static ValueTask<int> GetIntValueAsync(int x) => new(x);
        static Task SaveAsync(int x) => Task.CompletedTask;
        static ValueTask SaveValueAsync(int x) => default;
        static Exception Fail() => new InvalidOperationException();
        """;

    [Theory]
    [InlineData("var r = GetTask().Select(x => GetIntAsync(x));")]
    [InlineData("var r = GetTask().Select(async x => { await Task.Yield(); return 1; });")]
    [InlineData("var r = GetTask().Select(GetIntAsync);")]
    [InlineData("var r = GetTask().Select(x => GetIntValueAsync(x));")]
    [InlineData("var r = GetTask().Select(x => SaveValueAsync(x));")]
    [InlineData("var r = GetTask().Select(x => SaveAsync(x));")]
    [InlineData("var r = from x in GetTask() select GetIntAsync(x);")]
    [InlineData("var r = GetValueTask().Select(x => GetIntAsync(x));")]
    [InlineData("var r = GetValueTask().Select(async x => { await Task.Yield(); return 1; });")]
    [InlineData("var r = GetValueTask().Select(x => GetIntValueAsync(x));")]
    [InlineData("var r = from x in GetTask() from y in GetOther(x) select GetIntAsync(x + y);")]
    [InlineData("var r = from x in GetTask() from y in GetOtherAsync(x) select GetIntAsync(x + y);")]
    [InlineData("var r = GetTask().SelectMany(async x => await GetOtherAsync(x), async (x, y) => await GetIntAsync(x + y));")]
    [InlineData("var r = from x in GetTask() from y in GetOtherAsync(x) select GetIntValueAsync(x + y);")]
    [InlineData("var r = from x in GetTask() from y in GetOtherAsync(x) select SaveValueAsync(x + y);")]
    [InlineData("var r = from x in GetValueTask() from y in GetOtherValueAsync(x) select GetIntAsync(x + y);")]
    [InlineData("var r = from x in GetTask() from y in GetOtherAsync(x) from z in GetOther(y) select GetIntAsync(z);")]
    [InlineData("var r = from x in ok from y in GetOtherAsync(x) select GetIntAsync(x + y);")]
    [InlineData("var r = ok.SelectMany(async x => await GetOtherAsync(x), async (x, y) => await GetIntAsync(x + y));")]
    [InlineData("var r = ok.SelectMany(async x => await GetOtherAsync(x), (x, y) => GetIntValueAsync(x + y));")]
    [InlineData("var r = from x in ok from y in GetOtherValueAsync(x) select GetIntAsync(x + y);")]
    [InlineData("var r = from d in GetDoneTask() select GetIntAsync(1);")]
    [InlineData("var r = from d in GetDoneValueTask() select SaveAsync(1);")]
    [InlineData("var r = from x in GetTask() from d in CheckAsync(x) select GetIntAsync(x);")]
    [InlineData("var r = from x in GetTask() from d in Check(x) select SaveValueAsync(x);")]
    [InlineData("var r = from d in GetDoneTask() from y in GetOther(1) select GetIntValueAsync(y);")]
    [InlineData("var r = from d in GetDoneTask() from e in CheckAsync(1) select SaveAsync(1);")]
    [InlineData("var r = from d in done from y in GetOtherAsync(1) select GetIntAsync(y);")]
    [InlineData("var r = from d in done from e in CheckAsync(1) select SaveAsync(1);")]
    [InlineData("var r = from d in GetDoneValueTask() from e in CheckValueAsync(1) select SaveValueAsync(1);")]
    [InlineData("var r = from x in GetValueTask() select GetIntAsync(x);")]
    [InlineData("var r = from x in ok select GetIntAsync(x);")]
    public void Analyze_AsyncQuery_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<InferredResultValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.InferredResultValueId);
    }
}
