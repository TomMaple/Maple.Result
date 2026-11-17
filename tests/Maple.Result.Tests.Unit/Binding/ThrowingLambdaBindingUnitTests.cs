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
///     A lambda that only throws (#251) converts to every delegate type, so it used to reach the traps for a function that returns a
///     task. The task traps are generic methods constrained to the task type: a lambda that only throws has no return type to infer,
///     so it binds to the overload that runs, and a function that returns a task still reports the task trap (CS0619).
/// </summary>
public class ThrowingLambdaBindingUnitTests
{
    private const string Members = """
        static readonly Result done = Result.Success();
        static readonly Task<Result<int>> okTask = Task.FromResult(Result.FromValue(1));
        static int count;
        static void Log(Error e) { }
        static Task LogAsync(Error e) => Task.CompletedTask;
        static ValueTask LogValueAsync(Error e) => ValueTask.CompletedTask;
        static Task<int> GetCountAsync(Error e) => Task.FromResult(1);
        static ValueTask<int> GetCountValueAsync(Error e) => new(1);
        static Task SaveAsync(int x) => Task.CompletedTask;
        static int Compute(int x) => x * 2;
        static Exception Fail() => new InvalidOperationException();
        static Exception Fail(Error e) => new InvalidOperationException(e.Title);
        """;

    // The shapes of the issue that threw at compile time (a01 to a11), with Switch for a Match with an action (#255).
    [Theory]
    [InlineData("var r = ok.IfError(e => throw Fail(e));", "IfError", "Result<int>.IfError<Result<int>>(Action<Error>)")]
    [InlineData("var r = ok.IfError(e => { throw Fail(e); });", "IfError", "Result<int>.IfError<Result<int>>(Action<Error>)")]
    [InlineData("var r = ok.IfSuccess(_ => throw Fail());", "IfSuccess", "Result<int>.IfSuccess<int>(Action<int>)")]
    [InlineData("var r = done.IfSuccess(() => throw Fail());", "IfSuccess", "Result.IfSuccess(Action)")]
    [InlineData("var r = ok.Else(e => throw Fail(e));", "Else", "Result<int>.Else<Result<int>>(Func<Error, Result<int>>)")]
    [InlineData("var r = ok.Then(x => throw Fail());", "Then", "Result<int>.Then<int>(Func<int, Error>)")]
    [InlineData("var r = done.Then(() => throw Fail());", "Then", "Result.Then(Func<Error>)")]
    [InlineData("ok.Switch(x => { }, e => throw Fail(e));", "Switch", "Result<int>.Switch<int>(Action<int>, Action<Error>)")]
    [InlineData("done.Switch(() => { }, e => throw Fail(e));", "Switch", "Result.Switch(Action, Action<Error>)")]
    [InlineData("ok.Switch(x => throw Fail(), e => { });", "Switch", "Result<int>.Switch<int>(Action<int>, Action<Error>)")]
    [InlineData("ok.Switch(x => throw Fail(), e => throw Fail());", "Switch", "Result<int>.Switch<int>(Action<int>, Action<Error>)")]
    [InlineData("int r = ok.Match(x => x, e => throw Fail(e));", "Match", "Result<int>.Match<int, int>(Func<int, int>, Func<Error, int>)")]
    [InlineData("int r = ok.Match(x => Compute(x), e => throw Fail(e));", "Match", "Result<int>.Match<int, int>(Func<int, int>, Func<Error, int>)")]
    [InlineData("Task<int> r = okTask.MatchAsync(x => x, e => throw Fail(e));", "MatchAsync",
        "Task<Result<int>>.MatchAsync<int, int>(Func<int, int>, Func<Error, int>, bool?)")]
    [InlineData("Task r = okTask.SwitchAsync(x => { }, e => throw Fail(e));", "SwitchAsync",
        "Task<Result<int>>.SwitchAsync<int>(Action<int>, Action<Error>, bool?)")]
    [InlineData("var r = ok.Select(x => throw Fail());", "Select", "Result<int>.Select<int>(Func<int, Error>)")]
    public void LambdaThatOnlyThrows_BindsToTheOverloadThatRuns(string statements, string method, string expectedSignature)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
        snippet.BoundMethod(method).ShouldBe(expectedSignature);
    }

    // The asynchronous methods took a lambda that only throws before; they still do.
    [Theory]
    [InlineData("Task<Result<int>> r = ok.IfErrorAsync(e => throw Fail(e));")]
    [InlineData("Task<Result<int>> r = okTask.IfErrorAsync(e => throw Fail(e));")]
    [InlineData("Task<Result<int>> r = ok.IfSuccessAsync(x => throw Fail());")]
    [InlineData("Task<Result<int>> r = okTask.IfSuccessAsync(x => throw Fail());")]
    [InlineData("var r = ok.ThenAsync(x => throw Fail());")]
    [InlineData("var r = okTask.ThenAsync(x => throw Fail());")]
    public void AsynchronousMethod_LambdaThatOnlyThrows_StillCompiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("var r = ok.IfError(e => throw Fail(e));")]
    [InlineData("var r = ok.IfSuccess(_ => throw Fail());")]
    [InlineData("var r = ok.Else(e => throw Fail(e));")]
    [InlineData("var r = ok.Then(x => throw Fail());")]
    [InlineData("ok.Switch(x => { }, e => throw Fail(e));")]
    public void BothNamespaces_LambdaThatOnlyThrows_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    // The shapes of the issue that ran before (b01 to b07), and must still run.
    [Theory]
    [InlineData("var r = ok.IfError(e => Log(e));", "IfError", "Result<int>.IfError<Result<int>>(Action<Error>)")]
    [InlineData("var r = ok.IfError(e => { Log(e); });", "IfError", "Result<int>.IfError<Result<int>>(Action<Error>)")]
    [InlineData("var r = ok.IfError(Log);", "IfError", "Result<int>.IfError<Result<int>>(Action<Error>)")]
    [InlineData("var r = ok.IfError(e => { _ = LogAsync(e); });", "IfError", "Result<int>.IfError<Result<int>>(Action<Error>)")]
    [InlineData("var r = ok.IfError(void (Error e) => throw Fail());", "IfError", "Result<int>.IfError<Result<int>>(Action<Error>)")]
    [InlineData("var r = ok.Else(e => 0);", "Else", "Result<int>.Else<Result<int>>(Func<Error, Result<int>>)")]
    [InlineData("ok.Switch(x => { }, e => { });", "Switch", "Result<int>.Switch<int>(Action<int>, Action<Error>)")]
    public void ShapeThatRanBefore_StillBindsToTheOverloadThatRuns(string statements, string method, string expectedSignature)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
        snippet.BoundMethod(method).ShouldBe(expectedSignature);
    }

    // The shapes of the issue that were traps (c01 to c12, without c09), and must still be: a function that returns a task reports
    // the task trap, a function that returns a value reports the value trap of IfSuccess and IfError (#257).
    [Theory]
    [InlineData("var r = ok.IfError(e => LogAsync(e));", "The error action returns a task, which IfError does not await.")]
    [InlineData("var r = ok.IfError(async e => { await LogAsync(e); });", "The error action returns a task, which IfError does not await.")]
    [InlineData("var r = ok.IfError(e => LogValueAsync(e));", "The error action returns a task, which IfError does not await.")]
    [InlineData("var r = ok.IfError(e => GetCountAsync(e));", "The error action returns a task, which IfError does not await.")]
    [InlineData("var r = ok.IfError(e => GetCountValueAsync(e));", "The error action returns a task, which IfError does not await.")]
    [InlineData("var r = ok.IfError(async e => { await Task.Yield(); return 1; });", "The error action returns a task, which IfError does not await.")]
    [InlineData("var r = ok.IfError(LogAsync);", "The error action returns a task, which IfError does not await.")]
    [InlineData("var r = ok.IfError(async e => { throw Fail(); });", "The error action returns a task, which IfError does not await.")]
    [InlineData("var r = ok.IfError(e => e.Title);", "The function returns a value, which IfError discards;")]
    [InlineData("var r = ok.IfError(e => count++);", "The function returns a value, which IfError discards;")]
    [InlineData("var r = ok.IfSuccess(x => SaveAsync(x));", "The function returns a task, which IfSuccess does not await.")]
    [InlineData("var r = ok.IfSuccess(async x => await SaveAsync(x));", "The function returns a task, which IfSuccess does not await.")]
    [InlineData("var r = ok.IfSuccess(x => GetIntAsync());", "The function returns a task, which IfSuccess does not await.")]
    [InlineData("var r = ok.IfSuccess(x => GetIntValueTask());", "The function returns a task, which IfSuccess does not await.")]
    [InlineData("var r = ok.IfSuccess(x => ValueTask.CompletedTask);", "The function returns a task, which IfSuccess does not await.")]
    [InlineData("var r = ok.IfSuccess(SaveAsync);", "The function returns a task, which IfSuccess does not await.")]
    [InlineData("var r = done.IfSuccess(() => SaveAsync());", "The function returns a task, which IfSuccess does not await.")]
    [InlineData("var r = done.IfSuccess(() => GetIntValueTask());", "The function returns a task, which IfSuccess does not await.")]
    [InlineData("var r = ok.IfSuccess(x => Compute(x));", "The function returns a value, which IfSuccess discards;")]
    [InlineData("var r = ok.Else(e => LogAsync(e));", "The function returns a task, which Else does not await.")]
    [InlineData("var r = ok.Else(async e => await GetCountAsync(e));", "The function returns a task, which Else does not await.")]
    [InlineData("var r = ok.Else(e => LogValueAsync(e));", "The function returns a task, which Else does not await.")]
    [InlineData("var r = ok.Else(e => GetCountValueAsync(e));", "The function returns a task, which Else does not await.")]
    [InlineData("var r = ok.Else(LogAsync);", "The function returns a task, which Else does not await.")]
    [InlineData("var r = ok.Then(x => GetIntAsync());", "The function returns a task, which Then does not await.")]
    [InlineData("var r = ok.Then(async x => await GetIntAsync());", "The function returns a task, which Then does not await.")]
    [InlineData("var r = ok.Then(x => SaveAsync(x));", "The function returns a task, which Then does not await.")]
    [InlineData("var r = ok.Then(x => GetIntValueTask());", "The function returns a task, which Then does not await.")]
    [InlineData("var r = ok.Then(x => ValueTask.CompletedTask);", "The function returns a task, which Then does not await.")]
    [InlineData("var r = ok.Then(SaveAsync);", "The function returns a task, which Then does not await.")]
    [InlineData("var r = done.Then(() => SaveAsync());", "The function returns a task, which Then does not await.")]
    [InlineData("var r = done.Then(() => GetIntValueTask());", "The function returns a task, which Then does not await.")]
    [InlineData("ok.Switch(x => { }, e => LogAsync(e));", "An action returns a task, which Switch does not await.")]
    [InlineData("var r = from x in ok select GetIntAsync();", "The selector returns a task, which the query does not await.")]
    [InlineData("var r = from x in ok select GetIntValueTask();", "The selector returns a task, which the query does not await.")]
    [InlineData("var r = from x in ok select SaveAsync(x);", "The selector returns a task, which the query does not await.")]
    [InlineData("var r = from x in ok from y in GetResultOfInt() select SaveAsync(x + y);", "The selector returns a task, which the query does not await.")]
    [InlineData("var r = from x in GetResult() select SaveAsync(1);", "The selector returns a task, which the query does not await.")]
    public void FunctionReturningATaskOrAValue_StillReportsItsTrap(string statements, string message)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
        snippet.Errors.First().GetMessage(CultureInfo.InvariantCulture).ShouldContain(message);
    }

    // Limit 1 of the issue, accepted: an async lambda whose expression only throws has no return type to infer either, so it binds
    // to the action overload as async void. Its block form is still a task trap (see above).
    [Fact]
    public void IfError_AsyncLambdaWhoseExpressionOnlyThrows_BindsToTheActionOverload()
    {
        // Act
        var snippet = SnippetCompiler.Compile("var r = ok.IfError(async e => throw Fail());", SnippetImports.Task, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
        snippet.BoundMethod("IfError").ShouldBe("Result<int>.IfError<Result<int>>(Action<Error>)");
    }

    // ElseAsync has several overloads with a function that returns a task (of the result, of a value, of an Error), and a lambda
    // that only throws converts to all of them; left as it is (the issue's To decide).
    [Theory]
    [InlineData("var r = ok.ElseAsync(e => throw Fail(e));")]
    [InlineData("var r = okTask.ElseAsync(e => throw Fail(e));")]
    public void ElseAsync_LambdaThatOnlyThrows_IsStillAmbiguous(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe("CS0121", string.Join(Environment.NewLine, snippet.Errors));
    }

    // Limit 3 of the issue: explicit type arguments no longer keep a task; a result that holds the task does.
    [Theory]
    [InlineData("var r = ok.Then<int, Task<int>>(x => GetIntAsync());", "The function returns a task, which Then does not await.")]
    [InlineData("var r = done.Then<Task>(() => SaveAsync(1));", "The function returns a task, which Then does not await.")]
    [InlineData("var r = ok.Select<int, Task>(x => SaveAsync(x));", "The selector returns a task, which the query does not await.")]
    public void ExplicitTypeArgumentsForATask_ReportTheTaskTrap(string statements, string message)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
        snippet.Errors.First().GetMessage(CultureInfo.InvariantCulture).ShouldContain(message);
    }

    [Theory]
    [InlineData("Result<Task<int>> r = ok.Then(x => Result.FromValue(GetIntAsync()));")]
    [InlineData("Result<Task> r = done.Then(() => Result.FromValue(SaveAsync(1)));")]
    [InlineData("Result<Task> r = from x in ok select Result.FromValue(SaveAsync(x));")]
    public void TaskInAResult_IsKept(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }
}
