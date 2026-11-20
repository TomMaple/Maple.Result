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

namespace Maple.Results.Tests.Unit.Binding;

/// <summary>
///     A function that returns a <c>ValueTask&lt;TValue&gt;</c> passed to <c>IfSuccessAsync</c> or <c>IfErrorAsync</c> (#289): the
///     call binds to the overload that awaits the task and ignores its value, on a result and on a task of a result, and the receiver
///     decides the kind of the returned task (a <c>ValueTask</c> for a result). A <c>ValueTask&lt;TValue&gt;</c> does not convert to a
///     <c>ValueTask</c>, so without these overloads the call was a compile error on a result, and on a task of a result it bound to
///     the synchronous action, which does not await the task.
/// </summary>
public class ValueTaskValueFunctionBindingUnitTests
{
    private const string Members = """
        static readonly Error Err = Error.Failure(ErrorUri.None, "x");
        static Result done = Result.Success();
        static Task<Result<int>> okTask = Task.FromResult(ok);
        static Task<Result> doneTask = Task.FromResult(done);
        static ValueTask<Result<int>> okVt = new(ok);
        static ValueTask<Result> doneVt = new(done);
        static ValueTask<bool> SetVt(int x) => new(true);
        static ValueTask<bool> FlushVt() => new(true);
        static ValueTask<int> CountVt(Error e) => new(1);
        static Task<bool> SetAsync(int x) => Task.FromResult(true);
        """;

    [Theory]
    [InlineData("ValueTask<Result<int>> r = ok.IfSuccessAsync(x => SetVt(x));",
        "Result<int>.IfSuccessAsync<int, bool>(Func<int, ValueTask<bool>>, bool?)")]
    [InlineData("ValueTask<Result> r = done.IfSuccessAsync(() => FlushVt());",
        "Result.IfSuccessAsync<bool>(Func<ValueTask<bool>>, bool?)")]
    [InlineData("Task<Result<int>> r = okTask.IfSuccessAsync(x => SetVt(x));",
        "Task<Result<int>>.IfSuccessAsync<int, bool>(Func<int, ValueTask<bool>>, bool?)")]
    [InlineData("Task<Result> r = doneTask.IfSuccessAsync(() => FlushVt());",
        "Task<Result>.IfSuccessAsync<bool>(Func<ValueTask<bool>>, bool?)")]
    [InlineData("ValueTask<Result<int>> r = okVt.IfSuccessAsync(x => SetVt(x));",
        "ValueTask<Result<int>>.IfSuccessAsync<int, bool>(Func<int, ValueTask<bool>>, bool?)")]
    [InlineData("ValueTask<Result> r = doneVt.IfSuccessAsync(() => FlushVt());",
        "ValueTask<Result>.IfSuccessAsync<bool>(Func<ValueTask<bool>>, bool?)")]
    // A method group, and an anonymous function with an explicit return type.
    [InlineData("ValueTask<Result<int>> r = ok.IfSuccessAsync(SetVt);",
        "Result<int>.IfSuccessAsync<int, bool>(Func<int, ValueTask<bool>>, bool?)")]
    [InlineData("ValueTask<Result<int>> r = ok.IfSuccessAsync(async ValueTask<bool> (int x) => await SetVt(x));",
        "Result<int>.IfSuccessAsync<int, bool>(Func<int, ValueTask<bool>>, bool?)")]
    public void IfSuccessAsync_FunctionReturningAValueTaskWithAValue_BindsToTheOverloadThatAwaitsIt(string statements, string expectedSignature)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
        snippet.BoundMethod("IfSuccessAsync").ShouldBe(expectedSignature);
    }

    [Theory]
    [InlineData("ValueTask<Result<int>> r = ok.IfErrorAsync(e => CountVt(e));",
        "Result<int>.IfErrorAsync<Result<int>, int>(Func<Error, ValueTask<int>>, bool?)")]
    [InlineData("ValueTask<Result> r = done.IfErrorAsync(e => CountVt(e));",
        "Result.IfErrorAsync<Result, int>(Func<Error, ValueTask<int>>, bool?)")]
    [InlineData("Task<Result<int>> r = okTask.IfErrorAsync(e => CountVt(e));",
        "Task<Result<int>>.IfErrorAsync<Result<int>, int>(Func<Error, ValueTask<int>>, bool?)")]
    [InlineData("Task<Result> r = doneTask.IfErrorAsync(e => CountVt(e));",
        "Task<Result>.IfErrorAsync<Result, int>(Func<Error, ValueTask<int>>, bool?)")]
    [InlineData("ValueTask<Result<int>> r = okVt.IfErrorAsync(e => CountVt(e));",
        "ValueTask<Result<int>>.IfErrorAsync<Result<int>, int>(Func<Error, ValueTask<int>>, bool?)")]
    [InlineData("ValueTask<Result> r = doneVt.IfErrorAsync(e => CountVt(e));",
        "ValueTask<Result>.IfErrorAsync<Result, int>(Func<Error, ValueTask<int>>, bool?)")]
    [InlineData("ValueTask<Result<int>> r = ok.IfErrorAsync(CountVt);",
        "Result<int>.IfErrorAsync<Result<int>, int>(Func<Error, ValueTask<int>>, bool?)")]
    public void IfErrorAsync_FunctionReturningAValueTaskWithAValue_BindsToTheOverloadThatAwaitsIt(string statements, string expectedSignature)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
        snippet.BoundMethod("IfErrorAsync").ShouldBe(expectedSignature);
    }

    // An async lambda with an expression body converts to a function that returns a Task, and that overload has the priority, so the
    // new overloads change nothing for it; neither do they for a function that returns a Task with a value.
    [Theory]
    [InlineData("Task<Result<int>> r = ok.IfSuccessAsync(async x => await SetVt(x));", "IfSuccessAsync",
        "Result<int>.IfSuccessAsync<int>(Func<int, Task>, bool?)")]
    [InlineData("Task<Result<int>> r = ok.IfSuccessAsync(x => SetAsync(x));", "IfSuccessAsync",
        "Result<int>.IfSuccessAsync<int>(Func<int, Task>, bool?)")]
    [InlineData("ValueTask<Result<int>> r = okVt.IfSuccessAsync(async x => await SetVt(x));", "IfSuccessAsync",
        "ValueTask<Result<int>>.IfSuccessAsync<int>(Func<int, Task>, bool?)")]
    [InlineData("Task<Result<int>> r = ok.IfErrorAsync(async e => await CountVt(e));", "IfErrorAsync",
        "Result<int>.IfErrorAsync<Result<int>>(Func<Error, Task>, bool?)")]
    public void AsyncLambdaOrTaskFunction_StillBindsToTheOverloadForATaskFunction(string statements, string method, string expectedSignature)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
        snippet.BoundMethod(method).ShouldBe(expectedSignature);
    }

    // An async lambda with a block body that returns a value does not convert to a function that returns a Task without a value
    // (CS8031 before these overloads); it converts to a function that returns a ValueTask with a value.
    [Theory]
    [InlineData("ValueTask<Result<int>> r = ok.IfSuccessAsync(async x => { await Task.Yield(); return 1; });", "IfSuccessAsync",
        "Result<int>.IfSuccessAsync<int, int>(Func<int, ValueTask<int>>, bool?)")]
    [InlineData("Task<Result<int>> r = okTask.IfSuccessAsync(async x => { await Task.Yield(); return 1; });", "IfSuccessAsync",
        "Task<Result<int>>.IfSuccessAsync<int, int>(Func<int, ValueTask<int>>, bool?)")]
    [InlineData("ValueTask<Result<int>> r = ok.IfErrorAsync(async e => { await Task.Yield(); return 1; });", "IfErrorAsync",
        "Result<int>.IfErrorAsync<Result<int>, int>(Func<Error, ValueTask<int>>, bool?)")]
    public void AsyncLambdaWithABlockBodyThatReturnsAValue_BindsToTheOverloadForAValueTaskFunction(string statements, string method,
        string expectedSignature)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
        snippet.BoundMethod(method).ShouldBe(expectedSignature);
    }
}
