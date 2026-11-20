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
///     The <c>Task</c>-based and the <c>ValueTask</c>-based methods in one class per verb, in <c>Maple.Results.Extensions</c> (#248):
///     on a synchronous result the function decides the kind of task (an <c>async</c> lambda gives a <c>Task</c>, an explicit return type
///     a <c>ValueTask</c>), on a task result the receiver decides it (a function of the other kind is awaited, #226), and a shape that
///     no overload takes reports the errors of the compiler, never an ambiguity (CS0121). The shapes that compile into something wrong
///     (e.g., a task without a value in <c>ThenAsync</c> on a task result) are reported by the analyzer (rules MR0001 to MR0006 of
///     <c>Maple.Result.Analyzers</c>) and tested with it (#223).
/// </summary>
public class MergedAsyncClassesBindingUnitTests
{
    private const string Members = """
        static readonly Result done = Result.Success();
        static Task<Result<int>> GetTask() => Task.FromResult(Result.FromValue(1));
        static ValueTask<Result<int>> GetValueTask() => ValueTask.FromResult(Result.FromValue(1));
        static Task<int> DoubleAsync(int x) => Task.FromResult(x * 2);
        static ValueTask<int> DoubleValueAsync(int x) => ValueTask.FromResult(x * 2);
        static Task<Result<int>> NextAsync(int x) => Task.FromResult(Result.FromValue(x + 1));
        static ValueTask<Result<int>> NextValueAsync(int x) => ValueTask.FromResult(Result.FromValue(x + 1));
        static Task StoreAsync(int x) => Task.CompletedTask;
        static ValueTask StoreValueAsync(int x) => ValueTask.CompletedTask;
        static Task LogAsync(Error e) => Task.CompletedTask;
        static ValueTask LogValueAsync(Error e) => ValueTask.CompletedTask;
        static Task<int> RecoverAsync(Error e) => Task.FromResult(0);
        static ValueTask<int> RecoverValueAsync(Error e) => ValueTask.FromResult(0);
        static Task<Result> GetDoneTask() => Task.FromResult(Result.Success());
        static ValueTask<Result> GetDoneValueTask() => ValueTask.FromResult(Result.Success());
        static Task<Error> TranslateAsync(Error e) => Task.FromResult(e);
        static ValueTask<Error> TranslateValueAsync(Error e) => ValueTask.FromResult(e);
        static Task<Result<int>> FallbackAsync(Error e) => Task.FromResult(Result.FromValue(0));
        static ValueTask<Result<int>> FallbackValueAsync(Error e) => ValueTask.FromResult(Result.FromValue(0));
        static Task<Result> CheckAsync(int x) => Task.FromResult(Result.Success());
        static ValueTask<Result> CheckValueAsync(int x) => ValueTask.FromResult(Result.Success());
        static Task<Error> RejectAsync(int x) => Task.FromResult(Error.Failure(ErrorUri.None, "x"));
        static ValueTask<Error> RejectValueAsync(int x) => ValueTask.FromResult(Error.Failure(ErrorUri.None, "x"));
        """;

    [Theory]
    // A synchronous result: the function decides the kind of task.
    [InlineData("Task<Result<int>> r = ok.ThenAsync(x => DoubleAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = ok.ThenAsync(x => DoubleValueAsync(x));")]
    [InlineData("Task<Result<int>> r = ok.ThenAsync(DoubleAsync);")]
    [InlineData("ValueTask<Result<int>> r = ok.ThenAsync(DoubleValueAsync);")]
    [InlineData("Task<Result<int>> r = ok.ThenAsync(x => NextAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = ok.ThenAsync(x => NextValueAsync(x));")]
    [InlineData("Task<Result<int>> r = done.ThenAsync(() => GetIntAsync());")]
    [InlineData("ValueTask<Result<int>> r = done.ThenAsync(() => GetIntValueTask());")]
    [InlineData("Task<Result<int>> r = ok.IfSuccessAsync(x => StoreAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = ok.IfSuccessAsync(x => StoreValueAsync(x));")]
    [InlineData("Task<Result<int>> r = ok.IfErrorAsync(e => LogAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = ok.IfErrorAsync(e => LogValueAsync(e));")]
    [InlineData("Task<Result<int>> r = ok.ElseAsync(e => RecoverAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = ok.ElseAsync(e => RecoverValueAsync(e));")]
    // A synchronous result: an async lambda converts to both kinds and gives a Task; an explicit return type gives a ValueTask.
    [InlineData("Task<Result<int>> r = ok.ThenAsync(async x => { await Task.Yield(); return x + 1; });")]
    [InlineData("Task<Result<int>> r = ok.ThenAsync(async x => await DoubleValueAsync(x));")]
    [InlineData("Task<Result<int>> r = done.ThenAsync(async () => { await Task.Yield(); return 1; });")]
    [InlineData("Task<Result<int>> r = ok.IfSuccessAsync(async x => { await Task.Yield(); });")]
    [InlineData("Task<Result<int>> r = ok.IfErrorAsync(async e => { await Task.Yield(); });")]
    [InlineData("Task<Result<int>> r = ok.ElseAsync(async e => { await Task.Yield(); return 0; });")]
    [InlineData("ValueTask<Result<int>> r = ok.ThenAsync(async ValueTask<int> (int x) => { await Task.Yield(); return x + 1; });")]
    [InlineData("ValueTask<Result<int>> r = ok.IfSuccessAsync(async ValueTask (int x) => { await Task.Yield(); });")]
    [InlineData("ValueTask<Result<int>> r = ok.ElseAsync(async ValueTask<int> (Error e) => { await Task.Yield(); return 0; });")]
    // A task result: the receiver decides the kind of task.
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(x => x + 1);")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(x => x + 1);")]
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(x => DoubleAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(x => DoubleValueAsync(x));")]
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(async x => { await Task.Yield(); return x + 1; });")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(async x => { await Task.Yield(); return x + 1; });")]
    [InlineData("Task<Result<int>> r = GetTask().IfErrorAsync(async e => { await Task.Yield(); });")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().IfErrorAsync(async e => { await Task.Yield(); });")]
    [InlineData("Task<int> r = GetTask().MatchAsync(x => x, e => 0);")]
    [InlineData("ValueTask<int> r = GetValueTask().MatchAsync(x => x, e => 0);")]
    [InlineData("Task<int> r = GetTask().MatchAsync(x => DoubleAsync(x), e => RecoverAsync(e));")]
    [InlineData("ValueTask<int> r = GetValueTask().MatchAsync(x => DoubleValueAsync(x), e => RecoverValueAsync(e));")]
    [InlineData("Task r = GetTask().SwitchAsync(x => { }, e => { });")]
    [InlineData("ValueTask r = GetValueTask().SwitchAsync(x => { }, e => { });")]
    [InlineData("Task<Result> r = GetTask().ToResultAsync();")]
    [InlineData("ValueTask<Result> r = GetValueTask().ToResultAsync();")]
    // A task result with a function that returns the other kind of task: it is awaited, and the receiver decides the kind (#226).
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(x => DoubleValueAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(x => DoubleAsync(x));")]
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(DoubleValueAsync);")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(DoubleAsync);")]
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(x => NextValueAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(x => NextAsync(x));")]
    [InlineData("Task<Result> r = GetTask().ThenAsync(x => CheckValueAsync(x));")]
    [InlineData("ValueTask<Result> r = GetValueTask().ThenAsync(x => CheckAsync(x));")]
    [InlineData("Task<Result> r = GetTask().ThenAsync(x => RejectValueAsync(x));")]
    [InlineData("ValueTask<Result> r = GetValueTask().ThenAsync(x => RejectAsync(x));")]
    [InlineData("Task<Result<int>> r = GetDoneTask().ThenAsync(() => GetIntValueTask());")]
    [InlineData("ValueTask<Result<int>> r = GetDoneValueTask().ThenAsync(() => GetIntAsync());")]
    [InlineData("Task<Result<int>> r = GetDoneTask().ThenAsync(() => NextValueAsync(1));")]
    [InlineData("ValueTask<Result<int>> r = GetDoneValueTask().ThenAsync(() => NextAsync(1));")]
    [InlineData("Task<Result> r = GetDoneTask().ThenAsync(() => CheckValueAsync(1));")]
    [InlineData("ValueTask<Result> r = GetDoneValueTask().ThenAsync(() => CheckAsync(1));")]
    [InlineData("Task<Result> r = GetDoneTask().ThenAsync(() => RejectValueAsync(1));")]
    [InlineData("ValueTask<Result> r = GetDoneValueTask().ThenAsync(() => RejectAsync(1));")]
    [InlineData("Task<Result<int>> r = GetTask().ElseAsync(e => FallbackValueAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ElseAsync(e => FallbackAsync(e));")]
    [InlineData("Task<Result<int>> r = GetTask().ElseAsync(e => RecoverValueAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ElseAsync(e => RecoverAsync(e));")]
    [InlineData("Task<Result<int>> r = GetTask().ElseAsync(e => TranslateValueAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ElseAsync(e => TranslateAsync(e));")]
    [InlineData("Task<Result> r = GetDoneTask().ElseAsync(e => TranslateValueAsync(e));")]
    [InlineData("ValueTask<Result> r = GetDoneValueTask().ElseAsync(e => TranslateAsync(e));")]
    [InlineData("Task<Result<int>> r = GetTask().IfSuccessAsync(x => StoreValueAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().IfSuccessAsync(x => StoreAsync(x));")]
    [InlineData("Task<Result> r = GetDoneTask().IfSuccessAsync(() => StoreValueAsync(1));")]
    [InlineData("ValueTask<Result> r = GetDoneValueTask().IfSuccessAsync(() => StoreAsync(1));")]
    [InlineData("Task<Result<int>> r = GetTask().IfErrorAsync(e => LogValueAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().IfErrorAsync(e => LogAsync(e));")]
    [InlineData("Task<int> r = GetTask().MatchAsync(x => DoubleValueAsync(x), e => RecoverValueAsync(e));")]
    [InlineData("ValueTask<int> r = GetValueTask().MatchAsync(x => DoubleAsync(x), e => RecoverAsync(e));")]
    [InlineData("Task r = GetTask().MatchAsync(x => StoreValueAsync(x), e => LogValueAsync(e));")]
    [InlineData("ValueTask r = GetValueTask().MatchAsync(x => StoreAsync(x), e => LogAsync(e));")]
    [InlineData("Task<int> r = GetDoneTask().MatchAsync(() => GetIntValueTask(), e => RecoverValueAsync(e));")]
    [InlineData("ValueTask<int> r = GetDoneValueTask().MatchAsync(() => GetIntAsync(), e => RecoverAsync(e));")]
    [InlineData("Task r = GetDoneTask().MatchAsync(() => StoreValueAsync(1), e => LogValueAsync(e));")]
    [InlineData("ValueTask r = GetDoneValueTask().MatchAsync(() => StoreAsync(1), e => LogAsync(e));")]
    // An async lambda on a task result gives the kind of the receiver.
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(async x => await DoubleAsync(x));")]
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(async x => await DoubleValueAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ElseAsync(async e => await RecoverAsync(e));")]
    [InlineData("ValueTask<int> r = GetValueTask().MatchAsync(async x => await DoubleAsync(x), async e => await RecoverAsync(e));")]
    // Chains.
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(x => DoubleValueAsync(x)).ThenAsync(DoubleAsync).IfErrorAsync(LogValueAsync);")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(DoubleAsync).ThenAsync(DoubleValueAsync).IfErrorAsync(LogAsync);")]
    [InlineData("ValueTask<Result<int>> r = ok.ThenAsync(x => DoubleValueAsync(x)).ThenAsync(x => x + 1).IfErrorAsync(e => LogValueAsync(e));")]
    [InlineData("Task<Result<int>> r = ok.ThenAsync(async x => { await Task.Yield(); return x; }).ThenAsync(DoubleAsync).IfErrorAsync(LogAsync);")]
    // Queries.
    [InlineData("Task<Result<int>> r = from x in ok from y in NextAsync(x) select x + y;")]
    [InlineData("ValueTask<Result<int>> r = from x in ok from y in NextValueAsync(x) select x + y;")]
    [InlineData("Task<Result<int>> r = from x in GetTask() from y in ok select x + y;")]
    [InlineData("ValueTask<Result<int>> r = from x in GetValueTask() from y in ok select x + y;")]
    [InlineData("Task<Result<int>> r = from x in GetTask() from y in NextAsync(x) select x + y;")]
    [InlineData("ValueTask<Result<int>> r = from x in GetValueTask() from y in NextValueAsync(x) select x + y;")]
    [InlineData("Task<Result<int>> r = from x in GetTask() select x + 1;")]
    [InlineData("ValueTask<Result<int>> r = from x in GetValueTask() select x + 1;")]
    // The synchronous methods and the ValueTask-based methods with one using.
    [InlineData("Result<int> r = ok.Then(x => x + 1).IfError(e => { });")]
    [InlineData("Result<int> r = from x in ok select x + 1;")]
    // A Task with a value in IfSuccessAsync or IfErrorAsync converts to a task without a value: it is awaited, and its value is ignored.
    [InlineData("Task<Result<int>> r = ok.IfSuccessAsync(x => DoubleAsync(x));")]
    [InlineData("Task<Result<int>> r = ok.IfErrorAsync(e => RecoverAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().IfSuccessAsync(x => DoubleAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().IfErrorAsync(e => RecoverAsync(e));")]
    // A ValueTask with a value does not convert to a ValueTask without one: an overload of its own awaits it and ignores the value (#289).
    // An async lambda with a block body that returns a value binds to that overload too.
    [InlineData("ValueTask<Result<int>> r = ok.IfSuccessAsync(x => DoubleValueAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = ok.IfErrorAsync(e => RecoverValueAsync(e));")]
    [InlineData("Task<Result<int>> r = GetTask().IfSuccessAsync(x => DoubleValueAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().IfErrorAsync(e => RecoverValueAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = ok.IfSuccessAsync(async x => { await Task.Yield(); return 1; });")]
    [InlineData("ValueTask<Result<int>> r = ok.IfErrorAsync(async e => { await Task.Yield(); return 1; });")]
    public void Call_OneNamespace_CompilesToExpectedType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    // A synchronous result: a task without a value in ThenAsync or ElseAsync; the compiler reports it (#256).
    [InlineData("_ = ok.ThenAsync(async x => { await Task.Yield(); });", "CS1643")]
    [InlineData("_ = done.ThenAsync(async () => { await Task.Yield(); });", "CS1643")]
    [InlineData("_ = ok.ElseAsync(async e => { await Task.Yield(); });", "CS1643")]
    [InlineData("_ = ok.ThenAsync(x => StoreAsync(x));", "CS0029,CS0266,CS1662")]
    [InlineData("_ = ok.ThenAsync(x => StoreValueAsync(x));", "CS0029,CS1662")]
    [InlineData("_ = ok.ElseAsync(e => LogAsync(e));", "CS0029,CS0266,CS1662")]
    [InlineData("_ = ok.ElseAsync(e => LogValueAsync(e));", "CS0029,CS1662")]
    // A task result with a function that returns the other kind of task: a task without a value in ElseAsync (#226).
    [InlineData("_ = GetTask().ElseAsync(e => LogValueAsync(e));", "CS0029,CS1662")]
    [InlineData("_ = GetValueTask().ElseAsync(e => LogAsync(e));", "CS0029,CS0266,CS1662")]
    // Still rejected: one function of each kind, and a query that mixes the kinds of task.
    [InlineData("_ = GetTask().MatchAsync(x => DoubleAsync(x), e => RecoverValueAsync(e));", "CS0029,CS1662")]
    [InlineData("_ = from x in GetTask() from y in NextValueAsync(x) select x + y;", "CS0019,CS0029,CS1503")]
    public void Call_OneNamespace_ReportsOnlyTheStatedErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }

    // An async lambda binds to the overload for a function that returns a Task, on every receiver (#226): on a ValueTask result, the
    // lambda produces a Task, which the method awaits, and the call returns a ValueTask.
    [Theory]
    [InlineData("var r = GetValueTask().ThenAsync(async x => { await Task.Yield(); return x + 1; });", "ThenAsync",
        "ValueTask<Result<int>>.ThenAsync<int, int>(Func<int, Task<int>>, bool?)")]
    [InlineData("var r = GetTask().ThenAsync(async x => { await Task.Yield(); return x + 1; });", "ThenAsync",
        "Task<Result<int>>.ThenAsync<int, int>(Func<int, Task<int>>, bool?)")]
    [InlineData("var r = GetValueTask().IfSuccessAsync(async x => { await Task.Yield(); });", "IfSuccessAsync",
        "ValueTask<Result<int>>.IfSuccessAsync<int>(Func<int, Task>, bool?)")]
    [InlineData("var r = GetValueTask().ElseAsync(async e => { await Task.Yield(); return 0; });", "ElseAsync",
        "ValueTask<Result<int>>.ElseAsync<int>(Func<Error, Task<int>>, bool?)")]
    public void AsyncLambdaOnATaskOfAResult_BindsToTheOverloadForATaskFunction(string statements, string method, string expectedSignature)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
        snippet.BoundMethod(method).ShouldBe(expectedSignature);
    }
}
