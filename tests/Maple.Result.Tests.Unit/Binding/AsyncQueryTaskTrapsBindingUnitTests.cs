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
using System.Globalization;
using System.Linq;

namespace Maple.Results.Tests.Unit.Binding;

/// <summary>
///     The task traps of the asynchronous LINQ operators as generic methods constrained to the task type, as the synchronous ones
///     since #251 (#270): a selector that only throws binds to an operator that runs, a selector that returns a task still reports
///     the task trap (CS0619), and explicit type arguments no longer keep a task; <c>Result.FromValue(task)</c> does, on every source.
///     The shapes A to D are those of the issue; N are the places with a non-generic <see cref="Result" />.
/// </summary>
public class AsyncQueryTaskTrapsBindingUnitTests
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

    [Theory]
    // A selector that returns a task of either kind, in method and in query syntax.
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
    // Explicit type arguments for a task reach the trap too, on every source (#251 for a result).
    [InlineData("var r = GetTask().Select<int, Task<int>>(x => GetIntAsync(x));")]
    [InlineData("var r = GetValueTask().Select<int, Task<int>>(x => GetIntAsync(x));")]
    [InlineData("var r = GetTask().SelectMany<int, int, Task<int>>(x => GetOther(x), (x, y) => GetIntAsync(x + y));")]
    [InlineData("var r = ok.SelectMany<int, int, Task<int>>(x => GetOtherAsync(x), (x, y) => GetIntAsync(x + y));")]
    [InlineData("var r = ok.Select<int, Task<int>>(x => GetIntAsync(x));")]
    [InlineData("var r = GetDoneTask().Select<Task<int>>(d => GetIntAsync(1));")]
    [InlineData("var r = GetDoneValueTask().Select<Task<int>>(d => GetIntAsync(1));")]
    [InlineData("var r = GetTask().SelectMany<int, Task<int>>(x => Check(x), (x, d) => GetIntAsync(x));")]
    [InlineData("var r = done.SelectMany<int, Task<int>>(d => GetOtherAsync(1), (d, y) => GetIntAsync(y));")]
    public void Query_SelectingTask_ReportsOnlyTheTaskTrap(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
        snippet.Errors.First().GetMessage(CultureInfo.InvariantCulture).ShouldContain("The selector returns a task, which the query does not await.");
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

    // The message names the way to keep a task on purpose: a result that holds it, not the type arguments.
    [Theory]
    [InlineData("var r = from x in GetTask() select GetIntAsync(x);")]
    [InlineData("var r = from x in GetValueTask() select GetIntAsync(x);")]
    [InlineData("var r = from x in ok select GetIntAsync(x);")]
    public void Query_SelectingTask_MessageNamesResultFromValue(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var message = snippet.Errors.Single(d => d.Id == "CS0619").GetMessage(CultureInfo.InvariantCulture);
        message.ShouldContain("or select the task in a result to keep it (e.g., select Result.FromValue(SaveAsync(x)))");
        message.ShouldNotContain("type arguments");
    }
}
