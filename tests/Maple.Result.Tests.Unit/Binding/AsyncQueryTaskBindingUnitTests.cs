// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Linq;

namespace Maple.Results.Tests.Unit.Binding;

/// <summary>
///     The asynchronous LINQ operators and a task (#270): a selector that only throws binds to an operator that runs, and a task is
///     kept on purpose with <c>Result.FromValue(task)</c> or with explicit type arguments, on every source. A selector that returns a
///     task compiles too, and nobody awaits the task: the library has no overload trap for it (#223), so the analyzer reports it
///     (rules MR0001 to MR0006 of <c>Maple.Result.Analyzers</c>), and those shapes are tested with the analyzer.
/// </summary>
public class AsyncQueryTaskBindingUnitTests
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
        static Exception Fail() => new InvalidOperationException();
        """;

    [Theory]
    // A selector that only throws (it is not a task) binds to an operator that runs.
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
    // The same for the non-generic results.
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
    // A task is kept on purpose in a result, on every source.
    [InlineData("Task<Result<Task<int>>> r = GetTask().Select(x => Result.FromValue(GetIntAsync(x)));")]
    [InlineData("ValueTask<Result<Task<int>>> r = GetValueTask().Select(x => Result.FromValue(GetIntAsync(x)));")]
    [InlineData("Task<Result<Task<int>>> r = from x in GetTask() select Result.FromValue(GetIntAsync(x));")]
    [InlineData("Task<Result<Task<int>>> r = from d in GetDoneTask() select Result.FromValue(GetIntAsync(1));")]
    // The shapes that ran before still run, with the same types.
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
    public void Query_ShapeThatRuns_CompilesToExpectedType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    // Explicit type arguments for a task keep the task on purpose, on every source: the call says that the value of the result is
    // the task, so the analyzer does not report it.
    [Theory]
    [InlineData("Task<Result<Task<int>>> r = GetTask().Select<int, Task<int>>(x => GetIntAsync(x));")]
    [InlineData("ValueTask<Result<Task<int>>> r = GetValueTask().Select<int, Task<int>>(x => GetIntAsync(x));")]
    [InlineData("Task<Result<Task<int>>> r = GetTask().SelectMany<int, int, Task<int>>(x => GetOther(x), (x, y) => GetIntAsync(x + y));")]
    [InlineData("Task<Result<Task<int>>> r = ok.SelectMany<int, int, Task<int>>(x => GetOtherAsync(x), (x, y) => GetIntAsync(x + y));")]
    [InlineData("Result<Task<int>> r = ok.Select<int, Task<int>>(x => GetIntAsync(x));")]
    [InlineData("Task<Result<Task<int>>> r = GetDoneTask().Select<Task<int>>(d => GetIntAsync(1));")]
    [InlineData("ValueTask<Result<Task<int>>> r = GetDoneValueTask().Select<Task<int>>(d => GetIntAsync(1));")]
    [InlineData("Task<Result<Task<int>>> r = GetTask().SelectMany<int, Task<int>>(x => Check(x), (x, d) => GetIntAsync(x));")]
    [InlineData("Task<Result<Task<int>>> r = done.SelectMany<int, Task<int>>(d => GetOtherAsync(1), (d, y) => GetIntAsync(y));")]
    public void Query_ExplicitTypeArgumentsForATask_KeepsTheTask(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    // An async lambda whose expression only throws: no LINQ operator takes an action, so the compiler rejects it (it does not
    // become async void, as it does in IfError since #251).
    [Fact]
    public void Select_AsyncLambdaWhoseExpressionOnlyThrows_IsACompilerError()
    {
        // Act
        var snippet = SnippetCompiler.Compile("var r = GetTask().Select(async x => throw Fail());", members: Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe("CS4010", string.Join(Environment.NewLine, snippet.Errors));
    }
}
