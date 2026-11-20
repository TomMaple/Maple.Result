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
///     A lambda that only throws (#251) converts to every delegate type and has no return type to infer, so it binds to the overload
///     that runs, also in <c>Result.Try</c>. A function that returns a task or a value, which no overload takes, is a compile error that
///     the compiler reports by itself; a task that nobody awaits compiles, and the analyzer reports it (rules MR0001 to MR0006 of
///     <c>Maple.Result.Analyzers</c>), so those shapes are tested with the analyzer (#223). A task is kept on purpose in a result or
///     with explicit type arguments.
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
    [InlineData("Result r = Result.Try(() => throw new IOException(), ex => err);", "Try", "Result.Try(Func<Error>, Func<Exception, Error?>)")]
    public void LambdaThatOnlyThrows_BindsToTheOverloadThatRuns(string statements, string method, string expectedSignature)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

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
        var snippet = SnippetCompiler.Compile(statements, members: Members);

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
        var snippet = SnippetCompiler.Compile(statements, members: Members);

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
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
        snippet.BoundMethod(method).ShouldBe(expectedSignature);
    }

    // The shapes of the issue with a function that returns a task or a value (c01 to c12, without c09) that no overload takes: the
    // compiler reports them by itself. The shapes that compile and leave a task that nobody awaits are tested with the analyzer.
    [Theory]
    [InlineData("var r = ok.IfError(async e => { await Task.Yield(); return 1; });", "CS8030")]
    [InlineData("var r = ok.IfError(LogAsync);", "CS0407")]
    [InlineData("var r = ok.IfError(e => e.Title);", "CS0201")]
    [InlineData("var r = ok.IfSuccess(x => ValueTask.CompletedTask);", "CS0201")]
    [InlineData("var r = ok.IfSuccess(SaveAsync);", "CS1929")]
    [InlineData("var r = ok.Else(e => LogAsync(e));", "CS0411")]
    [InlineData("var r = ok.Else(async e => await GetCountAsync(e));", "CS0411")]
    [InlineData("var r = ok.Else(e => LogValueAsync(e));", "CS0411")]
    [InlineData("var r = ok.Else(e => GetCountValueAsync(e));", "CS0411")]
    [InlineData("var r = ok.Else(LogAsync);", "CS0411")]
    public void FunctionReturningATaskOrAValue_ReportsTheCompilerErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }

    // A side effect takes a function whose body is a call or an increment and ignores the value (#257).
    [Theory]
    [InlineData("var r = ok.IfError(e => count++);", "IfError", "Result<int>.IfError<Result<int>>(Action<Error>)")]
    [InlineData("var r = ok.IfSuccess(x => Compute(x));", "IfSuccess", "Result<int>.IfSuccess<int>(Action<int>)")]
    public void SideEffect_FunctionReturningAValue_BindsToTheActionOverload(string statements, string method, string expectedSignature)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
        snippet.BoundMethod(method).ShouldBe(expectedSignature);
    }

    // Limit 1 of the issue, accepted: an async lambda whose expression only throws has no return type to infer either, so it binds
    // to the action overload as async void, as its block form does; the analyzer reports both.
    [Fact]
    public void IfError_AsyncLambdaWhoseExpressionOnlyThrows_BindsToTheActionOverload()
    {
        // Act
        var snippet = SnippetCompiler.Compile("var r = ok.IfError(async e => throw Fail());", members: Members);

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
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe("CS0121", string.Join(Environment.NewLine, snippet.Errors));
    }

    // Limit 3 of the issue: explicit type arguments keep a task on purpose, as a result that holds the task does.
    [Theory]
    [InlineData("Result<Task<int>> r = ok.Then<int, Task<int>>(x => GetIntAsync());")]
    [InlineData("Result<Task> r = done.Then<Task>(() => SaveAsync(1));")]
    [InlineData("Result<Task> r = ok.Select<int, Task>(x => SaveAsync(x));")]
    public void ExplicitTypeArgumentsForATask_KeepTheTask(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("Result<Task<int>> r = ok.Then(x => Result.FromValue(GetIntAsync()));")]
    [InlineData("Result<Task> r = done.Then(() => Result.FromValue(SaveAsync(1)));")]
    [InlineData("Result<Task> r = from x in ok select Result.FromValue(SaveAsync(x));")]
    public void TaskInAResult_IsKept(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }
}
