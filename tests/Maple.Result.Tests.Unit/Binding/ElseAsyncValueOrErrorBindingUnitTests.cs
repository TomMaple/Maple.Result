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
///     <c>ElseAsync</c> with a function that returns a value or an <see cref="Error" /> in a task (#237), as <c>Else</c> takes one through the
///     implicit conversions: the shapes of the issue compile and give the stated result type, the shapes that compiled before still do,
///     and the trapped shapes still report CS0619 on the same traps.
/// </summary>
public class ElseAsyncValueOrErrorBindingUnitTests
{
    private const string Members = """
        static readonly Error Err = Error.Failure(ErrorUri.None, "x");
        static Result<int> failed = Err;
        static Task<Result<int>> failedTask = Task.FromResult(failed);
        static ValueTask<Result<int>> failedVt = new(failed);
        static Result plain = Err;
        static Task<Result> plainTask = Task.FromResult(plain);
        static ValueTask<Result> plainVt = new(plain);
        static Task LogAsync(Error e) => Task.CompletedTask;
        static Task<Error> GetErrorAsync() => Task.FromResult(Err);
        static Task<int> IntFromErrorAsync(Error e) => Task.FromResult(0);
        static Task<Result<int>> FallbackAsync() => Task.FromResult<Result<int>>(0);
        static Task<Result<int>> FallbackFromErrorAsync(Error e) => Task.FromResult<Result<int>>(0);
        static ValueTask LogVt(Error e) => default;
        static ValueTask<Error> GetErrorVt() => new(Err);
        static ValueTask<int> IntFromErrorVt(Error e) => new(0);
        static ValueTask<Result<int>> FallbackVt() => new((Result<int>)0);
        static ValueTask<Result<int>> FallbackFromErrorVt(Error e) => new((Result<int>)0);
        """;

    #region Maple.Results.Extensions

    [Theory]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(async e => 0);")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(async e => { await LogAsync(e); return e; });")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(async e => e.WithMetadata(\"k\", 1));")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(async e => await GetIntAsync());")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(e => GetIntAsync());")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(e => GetErrorAsync());")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(IntFromErrorAsync);")]
    [InlineData("Task<Result<int>> r = failed.ElseAsync(e => GetIntAsync());")]
    [InlineData("Task<Result<int>> r = failed.ElseAsync(e => GetErrorAsync());")]
    [InlineData("Task<Result> r = plainTask.ElseAsync(async e => e.WithMetadata(\"k\", 1));")]
    [InlineData("Task<Result> r = plain.ElseAsync(e => GetErrorAsync());")]
    [InlineData("Task<Result<int>> r = failed.ElseAsync(async e => 0);")]
    [InlineData("Task<Result<int>> r = failed.ElseAsync(async e => { await LogAsync(e); return 0; });")]
    public void TaskBased_FunctionReturningAValueOrAnErrorInATask_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(e => FallbackAsync());")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(async e => await FallbackAsync());")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(FallbackFromErrorAsync);")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(async e => Result<int>.FromValue(0));")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(e => 0);")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(e => e);")]
    [InlineData("Task<Result> r = plainTask.ElseAsync(async e => Result.Success());")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(async e => e.Category == ErrorCategory.NotFound ? 0 : e);")]
    public void TaskBased_ShapeThatCompiledBefore_StillCompiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("_ = failedTask.ElseAsync(e => LogAsync(e));", "The function returns a task without a value.")]
    [InlineData("_ = failedTask.ElseAsync(e => LogVt(e));", "The function returns a ValueTask")]
    [InlineData("_ = failedTask.ElseAsync(e => IntFromErrorVt(e));", "The function returns a ValueTask")]
    [InlineData("_ = failedTask.ElseAsync(async e => { await LogAsync(e); });", "The function returns a task without a value.")]
    public void TaskBased_TrappedShape_StillReportsCS0619OnTheSameTrap(string statements, string message)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
        var error = snippet.Errors.First().GetMessage();
        error.ShouldContain(message);
    }

    #endregion

    #region Maple.Results.Extensions.ValueTasks

    [Theory]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(async e => 0);")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(async e => { await LogVt(e); return e; });")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(async e => e.WithMetadata(\"k\", 1));")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(async e => await IntFromErrorVt(e));")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(e => new ValueTask<int>(0));")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(e => GetErrorVt());")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(IntFromErrorVt);")]
    [InlineData("ValueTask<Result<int>> r = failed.ElseAsync(e => IntFromErrorVt(e));")]
    [InlineData("ValueTask<Result<int>> r = failed.ElseAsync(e => GetErrorVt());")]
    [InlineData("ValueTask<Result> r = plainVt.ElseAsync(async e => e.WithMetadata(\"k\", 1));")]
    [InlineData("ValueTask<Result> r = plain.ElseAsync(e => GetErrorVt());")]
    [InlineData("ValueTask<Result<int>> r = failed.ElseAsync(async e => 0);")]
    [InlineData("ValueTask<Result<int>> r = failed.ElseAsync(async e => { await LogVt(e); return 0; });")]
    public void ValueTaskBased_FunctionReturningAValueOrAnErrorInAValueTask_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.ValueTask, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(e => FallbackVt());")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(async e => await FallbackVt());")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(FallbackFromErrorVt);")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(async e => Result<int>.FromValue(0));")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(e => 0);")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(e => e);")]
    [InlineData("ValueTask<Result> r = plainVt.ElseAsync(async e => Result.Success());")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(async e => e.Category == ErrorCategory.NotFound ? 0 : e);")]
    public void ValueTaskBased_ShapeThatCompiledBefore_StillCompiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.ValueTask, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("_ = failedVt.ElseAsync(e => LogVt(e));", "The function returns a task without a value.")]
    [InlineData("_ = failedVt.ElseAsync(e => LogAsync(e));", "The function returns a Task")]
    [InlineData("_ = failedVt.ElseAsync(e => IntFromErrorAsync(e));", "The function returns a Task")]
    [InlineData("_ = failedVt.ElseAsync(async e => { await LogVt(e); });", "The function returns a task without a value.")]
    public void ValueTaskBased_TrappedShape_StillReportsCS0619OnTheSameTrap(string statements, string message)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.ValueTask, Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
        var error = snippet.Errors.First().GetMessage();
        error.ShouldContain(message);
    }

    #endregion
}
