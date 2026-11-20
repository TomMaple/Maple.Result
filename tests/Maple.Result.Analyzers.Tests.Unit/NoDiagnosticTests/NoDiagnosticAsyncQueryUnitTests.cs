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
///     purpose: asynchronous LINQ queries and operators.
/// </summary>
public class NoDiagnosticAsyncQueryUnitTests
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
    [InlineData("Task<Result> r = GetTask().Select(x => throw Fail());")]
    [InlineData("Task<Result> r = GetTask().Select(x => { throw Fail(); });")]
    [InlineData("ValueTask<Result> r = GetValueTask().Select(x => throw Fail());")]
    [InlineData("Task<Result> r = GetTask().SelectMany(x => GetOther(x), (x, y) => throw Fail());")]
    [InlineData("Task<Result> r = GetTask().SelectMany(x => GetOtherAsync(x), (x, y) => throw Fail());")]
    [InlineData("ValueTask<Result> r = GetValueTask().SelectMany(x => GetOtherValueAsync(x), (x, y) => throw Fail());")]
    [InlineData("Task<Result> r = ok.SelectMany(x => GetOtherAsync(x), (x, y) => throw Fail());")]
    [InlineData("Task<Result> r = ok.SelectMany(async x => await GetOtherAsync(x), (x, y) => throw Fail());")]
    [InlineData("ValueTask<Result> r = ok.SelectMany(x => GetOtherValueAsync(x), (x, y) => throw Fail());")]
    [InlineData("Result r = ok.Select(x => throw Fail());")]
    [InlineData("Result r = ok.SelectMany(x => GetOther(x), (x, y) => throw Fail());")]
    [InlineData("Task<Result> r = GetDoneTask().Select(d => throw Fail());")]
    [InlineData("ValueTask<Result> r = GetDoneValueTask().Select(d => throw Fail());")]
    [InlineData("Task<Result> r = GetTask().SelectMany(x => Check(x), (x, d) => throw Fail());")]
    [InlineData("Task<Result> r = GetTask().SelectMany(x => CheckAsync(x), (x, d) => throw Fail());")]
    [InlineData("Task<Result> r = GetDoneTask().SelectMany(d => GetOther(1), (d, y) => throw Fail());")]
    [InlineData("Task<Result> r = GetDoneTask().SelectMany(d => GetOtherAsync(1), (d, y) => throw Fail());")]
    [InlineData("Task<Result> r = GetDoneTask().SelectMany(d => Check(1), (d, e) => throw Fail());")]
    [InlineData("Task<Result> r = GetDoneTask().SelectMany(d => CheckAsync(1), (d, e) => throw Fail());")]
    [InlineData("Task<Result> r = ok.SelectMany(x => CheckAsync(x), (x, d) => throw Fail());")]
    [InlineData("Task<Result> r = done.SelectMany(d => GetOtherAsync(1), (d, y) => throw Fail());")]
    [InlineData("Task<Result> r = done.SelectMany(d => CheckAsync(1), (d, e) => throw Fail());")]
    [InlineData("ValueTask<Result> r = ok.SelectMany(x => CheckValueAsync(x), (x, d) => throw Fail());")]
    [InlineData("ValueTask<Result> r = done.SelectMany(d => CheckValueAsync(1), (d, e) => throw Fail());")]
    [InlineData("ValueTask<Result> r = GetDoneValueTask().SelectMany(d => CheckValueAsync(1), (d, e) => throw Fail());")]
    [InlineData("Task<Result<int>> r = GetTask().Select(x => x + 1);")]
    [InlineData("Task<Result<int>> r = GetTask().Select(x => GetOther(x));")]
    [InlineData("Task<Result> r = GetTask().Select(x => err);")]
    [InlineData("Task<Result<int>> r = from x in GetTask() select x + 1;")]
    [InlineData("ValueTask<Result<int>> r = from x in GetValueTask() select x + 1;")]
    [InlineData("Task<Result<int>> r = from x in GetTask() from y in GetOtherAsync(x) select x + y;")]
    [InlineData("Task<Result<int>> r = GetTask().SelectMany(async x => await GetOtherAsync(x), (x, y) => x + y);")]
    [InlineData("ValueTask<Result<int>> r = from x in GetValueTask() from y in GetOtherValueAsync(x) select x + y;")]
    [InlineData("Task<Result<int>> r = from x in GetTask() from y in GetOtherAsync(x) from z in GetOther(y) select x + y + z;")]
    [InlineData("Task<Result<int>> r = from x in ok from y in GetOtherAsync(x) select x + y;")]
    [InlineData("Task<Result<int>> r = ok.SelectMany(async x => await GetOtherAsync(x), (x, y) => x + y);")]
    [InlineData("ValueTask<Result<int>> r = from x in ok from y in GetOtherValueAsync(x) select x + y;")]
    [InlineData("Task<Result<int>> r = from d in GetDoneTask() select 1;")]
    [InlineData("Task<Result<int>> r = from x in GetTask() from d in CheckAsync(x) select x;")]
    [InlineData("Task<Result<int>> r = from d in done from y in GetOtherAsync(1) select y;")]
    [InlineData("ValueTask<Result<int>> r = from d in GetDoneValueTask() from e in CheckValueAsync(1) select 1;")]
    public void Analyze_AsyncQueryThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("Task<Result<Task<int>>> r = GetTask().Select(x => Result.FromValue(GetIntAsync(x)));")]
    [InlineData("ValueTask<Result<Task<int>>> r = GetValueTask().Select(x => Result.FromValue(GetIntAsync(x)));")]
    [InlineData("Task<Result<Task<int>>> r = from x in GetTask() select Result.FromValue(GetIntAsync(x));")]
    [InlineData("Task<Result<Task<int>>> r = from d in GetDoneTask() select Result.FromValue(GetIntAsync(1));")]
    [InlineData("var r = GetTask().Select<int, Task<int>>(x => GetIntAsync(x));")]
    [InlineData("var r = GetValueTask().Select<int, Task<int>>(x => GetIntAsync(x));")]
    [InlineData("var r = GetTask().SelectMany<int, int, Task<int>>(x => GetOther(x), (x, y) => GetIntAsync(x + y));")]
    [InlineData("var r = ok.SelectMany<int, int, Task<int>>(x => GetOtherAsync(x), (x, y) => GetIntAsync(x + y));")]
    [InlineData("var r = ok.Select<int, Task<int>>(x => GetIntAsync(x));")]
    [InlineData("var r = GetDoneTask().Select<Task<int>>(d => GetIntAsync(1));")]
    [InlineData("var r = GetDoneValueTask().Select<Task<int>>(d => GetIntAsync(1));")]
    [InlineData("var r = GetTask().SelectMany<int, Task<int>>(x => Check(x), (x, d) => GetIntAsync(x));")]
    [InlineData("var r = done.SelectMany<int, Task<int>>(d => GetOtherAsync(1), (d, y) => GetIntAsync(y));")]
    public void Analyze_AsyncQueryKeepingTheValueOnPurpose_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
