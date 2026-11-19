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
///     A call that names its arguments (#273): the shapes that run bind to the overloads that run, with the names of their parameters,
///     and a function that no overload takes is a compile error that the compiler reports by itself. A named function that returns a
///     task which nobody awaits compiles (the task becomes the value of the result, or the value of <c>MatchAsync</c>): the library has
///     no overload trap for it (#223), so the analyzer reports it (rules MR0001 to MR0006 of <c>Maple.Result.Analyzers</c>), and those
///     shapes are tested with the analyzer.
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

    // No shape of Else and ElseAsync compiles by accident: the compiler reports the types that it cannot infer or convert.
    [Theory]
    [InlineData("var r = ok.Else(ifErrorFunction: e => RecoverAsync(e));", "CS0411")]
    [InlineData("var r = ok.Else(ifErrorFunction: e => LogAsync(e));", "CS0411")]
    [InlineData("var r = ok.Else(ifErrorFunction: e => LogVt(e));", "CS0411")]
    [InlineData("var r = ok.Else(ifErrorFunction: async e => { await LogAsync(e); });", "CS0411")]
    [InlineData("var r = plain.Else(ifErrorFunction: e => LogAsync(e));", "CS0411")]
    [InlineData("var r = plain.Else(ifErrorFunction: e => LogVt(e));", "CS0411")]
    [InlineData("var r = okTask.ElseAsync(ifErrorFunction: e => LogAsync(e));", "CS0029,CS0266,CS1662")]
    [InlineData("var r = okTask.ElseAsync(ifErrorFunction: e => LogVt(e));", "CS0029,CS1662")]
    [InlineData("var r = okTask.ElseAsync(ifErrorFunction: async e => { await LogAsync(e); });", "CS1643")]
    [InlineData("var r = plainTask.ElseAsync(ifErrorFunction: e => LogAsync(e));", "CS0029,CS0266,CS1662")]
    [InlineData("var r = okVt.ElseAsync(ifErrorFunction: e => LogAsync(e));", "CS0029,CS0266,CS1662")]
    [InlineData("var r = okVt.ElseAsync(ifErrorFunction: e => LogVt(e));", "CS0029,CS1662")]
    [InlineData("var r = plainVt.ElseAsync(ifErrorFunction: e => LogVt(e));", "CS0029,CS1662")]
    public void ElseAndElseAsync_NamedFunctionReturningTaskThatIsNotAwaited_ReportsTheCompilerErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
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

    // MatchAsync awaits two functions that return a task without a value as actions (ifSuccessAction, ifErrorAction), without names or
    // with their own names; the functions that return a task with a value, the synchronous functions, and the functions of the other
    // kind of task (#226) bind to the fold, with or without names.
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

    // A known limit: with the names of the functions (ifSuccessFunction, ifErrorFunction), two functions that return a task without a
    // value bind to the fold of two synchronous functions, and the call returns a task in a task. The analyzer reports the shapes
    // whose functions return a task; a success function that only throws has no return type, and the row keeps the limit visible.
    [Fact]
    public void MatchAsync_NamedSuccessFunctionThatOnlyThrowsWithErrorFunctionReturningTask_ReturnsATaskInATask()
    {
        // Act
        var snippet = SnippetCompiler.Compile(
            "Task<Task> r = okTask.MatchAsync(ifSuccessFunction: x => throw new InvalidOperationException(), ifErrorFunction: e => LogAsync(e));",
            Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    // The synchronous Match is a fold whose value may be a task: the caller gets it and awaits it.
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

    // Try has two names for its first parameter: action (Try(Action)) and function (Try<T>(Func<T>)). A function or an action that
    // returns a task compiles with either name and without names, and Try does not await the task; the analyzer reports those shapes.
    // Explicit type arguments for a task keep the task on purpose, as a result that holds the task does.
    [Theory]
    [InlineData("Result<Task<int>> r = Result.Try<Task<int>>(() => GetIntAsync(), e => err);")]
    [InlineData("Result<Task<int>> r = Result.Try<Task<int>, InvalidOperationException>(() => GetIntAsync(), e => err);")]
    public void Try_ExplicitTaskTypeArgument_KeepsTheTask(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
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
