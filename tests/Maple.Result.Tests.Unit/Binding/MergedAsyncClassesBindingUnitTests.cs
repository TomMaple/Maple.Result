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
///     a <c>ValueTask</c>), on a task result the receiver decides it, and a trapped shape reports only CS0619, never an ambiguity (CS0121).
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
    // Chains.
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
    public void Call_OneNamespace_CompilesToExpectedType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    // A synchronous result: the value traps of both kinds apply to an async lambda (#257).
    [InlineData("_ = ok.IfSuccessAsync(async x => { await Task.Yield(); return 1; });", "CS0619")]
    [InlineData("_ = ok.IfErrorAsync(async e => { await Task.Yield(); return 1; });", "CS0619")]
    [InlineData("_ = ok.IfSuccessAsync(x => DoubleAsync(x));", "CS0619")]
    [InlineData("_ = ok.IfSuccessAsync(x => DoubleValueAsync(x));", "CS0619")]
    [InlineData("_ = ok.IfErrorAsync(e => RecoverAsync(e));", "CS0619")]
    [InlineData("_ = ok.IfErrorAsync(e => RecoverValueAsync(e));", "CS0619")]
    // A synchronous result: a task without a value in ThenAsync or ElseAsync; the compiler reports it (#256).
    [InlineData("_ = ok.ThenAsync(async x => { await Task.Yield(); });", "CS1643")]
    [InlineData("_ = done.ThenAsync(async () => { await Task.Yield(); });", "CS1643")]
    [InlineData("_ = ok.ElseAsync(async e => { await Task.Yield(); });", "CS1643")]
    [InlineData("_ = ok.ThenAsync(x => StoreAsync(x));", "CS0029,CS0266,CS1662")]
    [InlineData("_ = ok.ThenAsync(x => StoreValueAsync(x));", "CS0029,CS1662")]
    [InlineData("_ = ok.ElseAsync(e => LogAsync(e));", "CS0029,CS0266,CS1662")]
    [InlineData("_ = ok.ElseAsync(e => LogValueAsync(e));", "CS0029,CS1662")]
    // A task result: a task without a value in ThenAsync.
    [InlineData("_ = GetTask().ThenAsync(x => StoreAsync(x));", "CS0619")]
    [InlineData("_ = GetValueTask().ThenAsync(x => StoreValueAsync(x));", "CS0619")]
    // A task result with a function that returns the other kind of task.
    [InlineData("_ = GetTask().ThenAsync(x => DoubleValueAsync(x));", "CS0619")]
    [InlineData("_ = GetValueTask().ThenAsync(x => DoubleAsync(x));", "CS0619")]
    [InlineData("_ = GetTask().IfErrorAsync(e => LogValueAsync(e));", "CS0619")]
    [InlineData("_ = GetValueTask().IfErrorAsync(e => LogAsync(e));", "CS0619")]
    // Still rejected: one function of each kind, and a query that mixes the kinds of task.
    [InlineData("_ = GetTask().MatchAsync(x => DoubleAsync(x), e => RecoverValueAsync(e));", "CS0619")]
    [InlineData("_ = from x in GetTask() from y in NextValueAsync(x) select x + y;", "CS0019,CS0029,CS1503")]
    public void Call_OneNamespace_ReportsOnlyTheStatedErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }
}
