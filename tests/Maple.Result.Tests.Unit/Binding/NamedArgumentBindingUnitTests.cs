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
///     A call that names its arguments reaches the task traps too (#273). A trap applies only to a call that uses the names of its
///     parameters, so a trap with another name than the overload that the call would bind to let the call through: the task became
///     the value of the result, or the value of <c>MatchAsync</c>, and nobody awaited it. The shapes that run keep binding to the
///     overloads that run.
/// </summary>
public class NamedArgumentBindingUnitTests
{
    private const string Members = """
        static Result plain = Result.Success();
        static Task<Result<int>> okTask = Task.FromResult(Result.FromValue(1));
        static Task<Result> plainTask = Task.FromResult(Result.Success());
        static ValueTask<Result<int>> okVt => new(Result.FromValue(1));
        static ValueTask<Result> plainVt => new(Result.Success());
        static Task SaveAsync(int x) => Task.CompletedTask;
        static ValueTask SaveVt() => default;
        static ValueTask SaveVt(int x) => default;
        static Task LogAsync(Error e) => Task.CompletedTask;
        static ValueTask LogVt(Error e) => default;
        static Task<int> LogIntAsync(Error e) => Task.FromResult(1);
        static ValueTask<int> LogIntVt(Error e) => new(1);
        static Task<Result<int>> RecoverAsync(Error e) => Task.FromResult(Result.FromValue(1));
        """;

    #region Then and ThenAsync

    [Theory]
    [InlineData("var r = ok.Then(ifSuccessFunction: x => SaveAsync(x));")]
    [InlineData("var r = ok.Then(ifSuccessFunction: x => GetIntAsync());")]
    [InlineData("var r = ok.Then(ifSuccessFunction: x => SaveVt(x));")]
    [InlineData("var r = ok.Then(ifSuccessFunction: x => GetIntValueTask());")]
    [InlineData("var r = ok.Then(ifSuccessFunction: async x => { await SaveAsync(x); });")]
    [InlineData("var r = ok.Then(ifSuccessFunction: async x => { await SaveAsync(x); return 1; });")]
    [InlineData("var r = plain.Then(ifSuccessFunction: () => SaveAsync());")]
    [InlineData("var r = plain.Then(ifSuccessFunction: () => GetIntAsync());")]
    [InlineData("var r = plain.Then(ifSuccessFunction: () => SaveVt());")]
    [InlineData("var r = plain.Then(ifSuccessFunction: () => GetIntValueTask());")]
    [InlineData("var r = plain.Then(ifSuccessFunction: async () => { await SaveAsync(); });")]
    [InlineData("var r = plain.Then(ifSuccessFunction: async () => { await SaveAsync(); return 1; });")]
    public void Then_NamedFunctionReturningTask_ReportsOnlyTheTaskTrap(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, Members);

        // Assert
        string.Join(",", snippet.ErrorIds).ShouldBe("CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    // A task with a value is awaited by ThenAsync; a task without one is the case of IfSuccessAsync, and a compile error here.
    [Theory]
    [InlineData("var r = okTask.ThenAsync(ifSuccessFunction: x => SaveAsync(x));")]
    [InlineData("var r = okTask.ThenAsync(ifSuccessFunction: x => SaveVt(x));")]
    [InlineData("var r = okTask.ThenAsync(ifSuccessFunction: async x => { await SaveAsync(x); });")]
    [InlineData("var r = plainTask.ThenAsync(ifSuccessFunction: () => SaveAsync());")]
    [InlineData("var r = plainTask.ThenAsync(ifSuccessFunction: () => SaveVt());")]
    [InlineData("var r = plainTask.ThenAsync(ifSuccessFunction: async () => { await SaveAsync(); });")]
    [InlineData("var r = okVt.ThenAsync(ifSuccessFunction: x => SaveAsync(x));")]
    [InlineData("var r = okVt.ThenAsync(ifSuccessFunction: x => SaveVt(x));")]
    [InlineData("var r = okVt.ThenAsync(ifSuccessFunction: async x => { await SaveAsync(x); });")]
    [InlineData("var r = plainVt.ThenAsync(ifSuccessFunction: () => SaveAsync());")]
    [InlineData("var r = plainVt.ThenAsync(ifSuccessFunction: () => SaveVt());")]
    [InlineData("var r = plainVt.ThenAsync(ifSuccessFunction: async () => { await SaveAsync(); });")]
    public void ThenAsync_NamedFunctionReturningTaskWithoutValue_ReportsOnlyTheTaskTrap(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, Members);

        // Assert
        string.Join(",", snippet.ErrorIds).ShouldBe("CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("Result<int> r = ok.Then(ifSuccessFunction: x => x + 1);")]
    [InlineData("Result<int> r = plain.Then(ifSuccessFunction: () => Compute());")]
    [InlineData("Result<Task<int>> r = ok.Then(ifSuccessFunction: x => Result.FromValue(GetIntAsync()));")]
    [InlineData("Task<Result<int>> r = ok.ThenAsync(ifSuccessFunction: x => GetIntAsync());")]
    [InlineData("Task<Result<int>> r = okTask.ThenAsync(ifSuccessFunction: x => x + 1);")]
    [InlineData("Task<Result<int>> r = okTask.ThenAsync(ifSuccessFunction: x => GetIntAsync());")]
    [InlineData("Task<Result<int>> r = okTask.ThenAsync(ifSuccessFunction: x => GetIntValueTask());")]
    [InlineData("Task<Result<int>> r = okTask.ThenAsync(ifSuccessFunction: async x => { await SaveAsync(x); return 1; });")]
    [InlineData("Task<Result<int>> r = plainTask.ThenAsync(ifSuccessFunction: () => GetIntAsync());")]
    [InlineData("ValueTask<Result<int>> r = okVt.ThenAsync(ifSuccessFunction: x => GetIntValueTask());")]
    [InlineData("ValueTask<Result<int>> r = okVt.ThenAsync(ifSuccessFunction: x => GetIntAsync());")]
    [InlineData("ValueTask<Result<int>> r = plainVt.ThenAsync(ifSuccessFunction: () => GetIntValueTask());")]
    public void ThenAndThenAsync_NamedFunctionThatRuns_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    #endregion

    #region Else and ElseAsync

    // No shape of Else and ElseAsync compiled by accident; the call now reports the message of the trap instead of an error of the
    // compiler about types that it cannot infer or convert.
    [Theory]
    [InlineData("var r = ok.Else(ifErrorFunction: e => RecoverAsync(e));")]
    [InlineData("var r = ok.Else(ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = ok.Else(ifErrorFunction: e => LogVt(e));")]
    [InlineData("var r = ok.Else(ifErrorFunction: async e => { await LogAsync(e); });")]
    [InlineData("var r = plain.Else(ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = plain.Else(ifErrorFunction: e => LogVt(e));")]
    [InlineData("var r = okTask.ElseAsync(ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = okTask.ElseAsync(ifErrorFunction: e => LogVt(e));")]
    [InlineData("var r = okTask.ElseAsync(ifErrorFunction: async e => { await LogAsync(e); });")]
    [InlineData("var r = plainTask.ElseAsync(ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = okVt.ElseAsync(ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = okVt.ElseAsync(ifErrorFunction: e => LogVt(e));")]
    [InlineData("var r = plainVt.ElseAsync(ifErrorFunction: e => LogVt(e));")]
    public void ElseAndElseAsync_NamedFunctionReturningTaskThatIsNotAwaited_ReportsOnlyTheTaskTrap(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, Members);

        // Assert
        string.Join(",", snippet.ErrorIds).ShouldBe("CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("Result<int> r = ok.Else(ifErrorFunction: e => 0);")]
    [InlineData("Result<int> r = ok.Else(ifErrorFunction: e => GetError());")]
    [InlineData("Task<Result<int>> r = ok.ElseAsync(ifErrorFunction: e => RecoverAsync(e));")]
    [InlineData("Task<Result<int>> r = okTask.ElseAsync(ifErrorFunction: e => RecoverAsync(e));")]
    [InlineData("Task<Result<int>> r = okTask.ElseAsync(ifErrorFunction: e => GetResultOfInt());")]
    [InlineData("ValueTask<Result<int>> r = okVt.ElseAsync(ifErrorFunction: e => RecoverAsync(e));")]
    public void ElseAndElseAsync_NamedFunctionThatRuns_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    #endregion

    #region MatchAsync

    // MatchAsync awaits two functions that return a task without a value only as actions (ifSuccessAction, ifErrorAction). With the
    // names of the functions, the call bound to the fold of two synchronous functions and returned a task in a task.
    [Theory]
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: x => SaveAsync(x), ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: x => SaveVt(x), ifErrorFunction: e => LogVt(e));")]
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: async x => { await SaveAsync(x); }, ifErrorFunction: async e => { await LogAsync(e); });")]
    [InlineData("var r = plainTask.MatchAsync(ifSuccessFunction: () => SaveAsync(), ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = plainTask.MatchAsync(ifSuccessFunction: () => SaveVt(), ifErrorFunction: e => LogVt(e));")]
    [InlineData("var r = plainTask.MatchAsync(ifSuccessFunction: async () => { await SaveAsync(); }, ifErrorFunction: async e => { await LogAsync(e); });")]
    [InlineData("var r = okVt.MatchAsync(ifSuccessFunction: x => SaveAsync(x), ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = okVt.MatchAsync(ifSuccessFunction: x => SaveVt(x), ifErrorFunction: e => LogVt(e));")]
    [InlineData("var r = okVt.MatchAsync(ifSuccessFunction: async x => { await SaveAsync(x); }, ifErrorFunction: async e => { await LogAsync(e); });")]
    [InlineData("var r = plainVt.MatchAsync(ifSuccessFunction: () => SaveAsync(), ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = plainVt.MatchAsync(ifSuccessFunction: () => SaveVt(), ifErrorFunction: e => LogVt(e));")]
    [InlineData("var r = plainVt.MatchAsync(ifSuccessFunction: async () => { await SaveAsync(); }, ifErrorFunction: async e => { await LogAsync(e); });")]
    // One function with a value and one without: the common type of the fold would be the task without a value.
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: x => GetIntAsync(), ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: x => SaveAsync(x), ifErrorFunction: e => LogIntAsync(e));")]
    // Only one argument is named, or the names come with the configuration of the awaits.
    [InlineData("var r = okTask.MatchAsync(x => SaveAsync(x), ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: x => SaveAsync(x), ifErrorFunction: e => LogAsync(e), continueOnCapturedContext: false);")]
    [InlineData("var r = okVt.MatchAsync(ifSuccessFunction: x => SaveVt(x), ifErrorFunction: e => LogVt(e), continueOnCapturedContext: false);")]
    // An error function that only throws converts to any delegate, so the success function decides.
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: x => SaveAsync(x), ifErrorFunction: e => throw new InvalidOperationException());")]
    public void MatchAsync_NamedFunctionsReturningTaskWithoutValue_ReportOnlyTheTrap(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, Members);

        // Assert
        string.Join(",", snippet.ErrorIds).ShouldBe("CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    [Fact]
    public void MatchAsync_NamedFunctionsReturningTaskWithoutValue_MessageNamesTheParametersOfTheActions()
    {
        // Act
        var snippet = SnippetCompiler.Compile(
            "var r = okTask.MatchAsync(ifSuccessFunction: x => SaveAsync(x), ifErrorFunction: e => LogAsync(e));",
            Members);

        // Assert
        var message = snippet.Errors.Single().GetMessage();
        message.ShouldContain("ifSuccessAction and ifErrorAction");
        message.ShouldContain("without names");
    }

    // The traps must not take a call from an overload that runs: asynchronous actions without names or with their own names, functions
    // that return a task with a value, synchronous functions, and the functions of the other kind of task (#226).
    [Theory]
    [InlineData("Task r = okTask.MatchAsync(x => SaveAsync(x), e => LogAsync(e));")]
    [InlineData("Task r = okTask.MatchAsync(x => SaveAsync(x), e => LogAsync(e), false);")]
    [InlineData("Task r = okTask.MatchAsync(x => SaveVt(x), e => LogVt(e));")]
    [InlineData("Task r = okTask.MatchAsync(x => SaveVt(x), e => LogVt(e), false);")]
    [InlineData("Task r = okTask.MatchAsync(async x => await SaveAsync(x), async e => await LogAsync(e));")]
    [InlineData("Task r = okTask.MatchAsync(ifSuccessAction: x => SaveAsync(x), ifErrorAction: e => LogAsync(e));")]
    [InlineData("Task r = okTask.MatchAsync(ifSuccessAction: x => SaveVt(x), ifErrorAction: e => LogVt(e));")]
    [InlineData("Task r = plainTask.MatchAsync(() => SaveAsync(), e => LogAsync(e));")]
    [InlineData("Task r = plainTask.MatchAsync(() => SaveVt(), e => LogVt(e));")]
    [InlineData("ValueTask r = okVt.MatchAsync(x => SaveVt(x), e => LogVt(e));")]
    [InlineData("ValueTask r = okVt.MatchAsync(x => SaveVt(x), e => LogVt(e), false);")]
    [InlineData("ValueTask r = okVt.MatchAsync(x => SaveAsync(x), e => LogAsync(e));")]
    [InlineData("ValueTask r = okVt.MatchAsync(async x => await SaveAsync(x), async e => await LogAsync(e));")]
    [InlineData("ValueTask r = okVt.MatchAsync(ifSuccessAction: x => SaveVt(x), ifErrorAction: e => LogVt(e));")]
    [InlineData("ValueTask r = plainVt.MatchAsync(() => SaveVt(), e => LogVt(e));")]
    [InlineData("ValueTask r = plainVt.MatchAsync(() => SaveAsync(), e => LogAsync(e));")]
    [InlineData("Task<int> r = okTask.MatchAsync(ifSuccessFunction: x => GetIntAsync(), ifErrorFunction: e => LogIntAsync(e));")]
    [InlineData("Task<int> r = okTask.MatchAsync(ifSuccessFunction: x => GetIntValueTask(), ifErrorFunction: e => LogIntVt(e));")]
    [InlineData("Task<int> r = okTask.MatchAsync(ifSuccessFunction: x => x, ifErrorFunction: e => 0);")]
    [InlineData("Task<int> r = okTask.MatchAsync(ifSuccessFunction: async x => { await SaveAsync(x); return 1; }, ifErrorFunction: e => LogIntAsync(e));")]
    [InlineData("Task<int> r = plainTask.MatchAsync(ifSuccessFunction: () => GetIntAsync(), ifErrorFunction: e => LogIntAsync(e));")]
    [InlineData("ValueTask<int> r = okVt.MatchAsync(ifSuccessFunction: x => GetIntValueTask(), ifErrorFunction: e => LogIntVt(e));")]
    [InlineData("ValueTask<int> r = okVt.MatchAsync(ifSuccessFunction: x => GetIntAsync(), ifErrorFunction: e => LogIntAsync(e));")]
    [InlineData("ValueTask<int> r = okVt.MatchAsync(ifSuccessFunction: x => x, ifErrorFunction: e => 0);")]
    [InlineData("ValueTask<int> r = plainVt.MatchAsync(ifSuccessFunction: () => GetIntValueTask(), ifErrorFunction: e => LogIntVt(e));")]
    public void MatchAsync_ShapeThatRuns_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    // A known limit: a success function that only throws has no return type to infer, so the trap does not apply, and the call with
    // the names of the functions still binds to the fold. Such a function is not a fold; the row keeps the limit visible.
    [Fact]
    public void MatchAsync_NamedSuccessFunctionThatOnlyThrowsWithErrorFunctionReturningTask_StillReturnsATaskInATask()
    {
        // Act
        var snippet = SnippetCompiler.Compile(
            "Task<Task> r = okTask.MatchAsync(ifSuccessFunction: x => throw new InvalidOperationException(), ifErrorFunction: e => LogAsync(e));",
            Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    // The synchronous Match is a fold whose value may be a task: the caller gets it and awaits it. It is not a trap.
    [Theory]
    [InlineData("Task<int> r = ok.Match(ifSuccessFunction: x => GetIntAsync(), ifErrorFunction: e => LogIntAsync(e));")]
    [InlineData("Task<int> r = ok.Match(x => GetIntAsync(), e => LogIntAsync(e));")]
    public void Match_FunctionsReturningTask_ReturnsTheTaskToTheCaller(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    #endregion

    #region Result.Try

    // Try has two names for its first parameter: action (Try(Action)) and function (Try<T>(Func<T>)). A task reaches both.
    [Theory]
    [InlineData("var r = Result.Try(function: () => SaveAsync(), mapException: e => err);")]
    [InlineData("var r = Result.Try(function: () => GetIntAsync(), mapException: e => err);")]
    [InlineData("var r = Result.Try(function: () => SaveVt(), mapException: e => err);")]
    [InlineData("var r = Result.Try(function: () => GetIntValueTask(), mapException: e => err);")]
    [InlineData("var r = Result.Try(function: async () => { await SaveAsync(); }, mapException: e => err);")]
    [InlineData("var r = Result.Try(function: async () => { await SaveAsync(); return 1; }, mapException: e => err);")]
    [InlineData("var r = Result.Try(function: () => SaveAsync(), mapException: (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(function: () => GetIntAsync(), mapException: (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(function: () => SaveVt(), mapException: (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(function: () => GetIntValueTask(), mapException: (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(function: async () => { await SaveAsync(); }, mapException: (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(action: () => SaveAsync(), mapException: e => err);")]
    [InlineData("var r = Result.Try(action: () => GetIntAsync(), mapException: e => err);")]
    [InlineData("var r = Result.Try(action: () => SaveVt(), mapException: e => err);")]
    [InlineData("var r = Result.Try(action: () => GetIntValueTask(), mapException: e => err);")]
    [InlineData("var r = Result.Try(action: async () => { await SaveAsync(); }, mapException: e => err);")]
    [InlineData("var r = Result.Try(action: () => GetIntValueTask(), mapException: (InvalidOperationException e) => err);")]
    public void Try_NamedFunctionOrActionReturningTask_ReportsOnlyTheTaskTrap(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, Members);

        // Assert
        string.Join(",", snippet.ErrorIds).ShouldBe("CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    // Without names, each of these shapes was a compile error already; the new traps must not make one of them ambiguous (CS0121).
    [Theory]
    [InlineData("var r = Result.Try(() => SaveAsync(), e => err);")]
    [InlineData("var r = Result.Try(() => GetIntAsync(), e => err);")]
    [InlineData("var r = Result.Try(() => SaveVt(), e => err);")]
    [InlineData("var r = Result.Try(() => GetIntValueTask(), e => err);")]
    [InlineData("var r = Result.Try(async () => { await SaveAsync(); }, e => err);")]
    [InlineData("var r = Result.Try(async () => { await SaveAsync(); return 1; }, e => err);")]
    [InlineData("var r = Result.Try(() => SaveAsync(), (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(() => GetIntAsync(), (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(() => SaveVt(), (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(() => GetIntValueTask(), (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(async () => { await SaveAsync(); return 1; }, (InvalidOperationException e) => err);")]
    public void Try_FunctionReturningTaskWithoutNames_ReportsOnlyTheTaskTrap(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, Members);

        // Assert
        string.Join(",", snippet.ErrorIds).ShouldBe("CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    // Explicit type arguments no longer keep a task, as in Then and Select (#251): a result that holds the task does.
    [Theory]
    [InlineData("var r = Result.Try<Task<int>>(() => GetIntAsync(), e => err);")]
    [InlineData("var r = Result.Try<Task<int>, InvalidOperationException>(() => GetIntAsync(), e => err);")]
    public void Try_ExplicitTaskTypeArgument_ReportsOnlyTheTaskTrap(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, Members);

        // Assert
        string.Join(",", snippet.ErrorIds).ShouldBe("CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("Result<int> r = Result.Try(function: () => Compute(), mapException: e => err);")]
    [InlineData("Result<int> r = Result.Try(function: () => Compute(), mapException: (InvalidOperationException e) => err);")]
    [InlineData("Result<int> r = Result.Try<int, InvalidOperationException>(() => Compute(), e => err);")]
    [InlineData("Result r = Result.Try(action: () => Console.WriteLine(), mapException: e => err);")]
    [InlineData("Result r = Result.Try(function: () => GetResult(), mapException: e => err);")]
    [InlineData("Result<int> r = Result.Try(function: () => GetResultOfInt(), mapException: e => err);")]
    [InlineData("Result r = Result.Try(function: () => GetError(), mapException: e => err);")]
    [InlineData("Result<Task<int>> r = Result.Try(() => Result.FromValue(GetIntAsync()), e => err);")]
    [InlineData("Task<Result> r = Result.TryAsync(action: () => SaveAsync(), mapException: e => err);")]
    [InlineData("Task<Result<int>> r = Result.TryAsync(function: () => GetIntAsync(), mapException: e => err);")]
    [InlineData("ValueTask<Result> r = Result.TryAsync(action: () => SaveVt(), mapException: e => err);")]
    [InlineData("ValueTask<Result<int>> r = Result.TryAsync(function: () => GetIntValueTask(), mapException: e => err);")]
    public void TryAndTryAsync_ShapeThatRuns_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    #endregion
}
