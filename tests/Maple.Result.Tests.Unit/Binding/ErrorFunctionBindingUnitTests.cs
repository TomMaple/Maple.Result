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
///     A function that returns an <see cref="Error" /> gives a failed <see cref="Result" /> in <c>Then</c>, <c>Match</c>, their
///     asynchronous forms and a query (#120); explicit type arguments keep a successful <c>Result&lt;Error&gt;</c>. Each shape is
///     assigned to a variable of the expected type.
/// </summary>
public class ErrorFunctionBindingUnitTests
{
    private const string Members = """
        static Task<Result<int>> GetOkAsync() => Task.FromResult(Result.FromValue(1));
        static ValueTask<Result<int>> GetOkValueTask() => new(Result.FromValue(1));
        static Error ToError(int value) => err;
        static void LogError(Error error) { }
        """;

    [Theory]
    [InlineData("Result r = ok.Then(x => GetError());")]
    [InlineData("Result r = GetResult().Then(() => GetError());")]
    [InlineData("Result r = ok.Then(ToError);")]
    [InlineData("Result r = ok.Match(x => GetError(), e => { });")]
    [InlineData("Result r = GetResult().Match(() => GetError(), e => { });")]
    [InlineData("Result r = ok.Match(ToError, LogError);")]
    [InlineData("Result r = from x in ok from y in GetResultOfInt() select GetError();")]
    public void Synchronous_FunctionReturningError_GivesFailedResult(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("Task<Result> r = ok.ThenAsync(x => Task.FromResult(GetError()));")]
    [InlineData("Task<Result> r = ok.ThenAsync(async x => { await Task.Yield(); return GetError(); });")]
    [InlineData("Task<Result> r = GetOkAsync().ThenAsync(x => Task.FromResult(GetError()));")]
    [InlineData("Task<Result> r = ok.MatchAsync(x => Task.FromResult(GetError()), e => Task.CompletedTask);")]
    [InlineData("Task<Result> r = GetOkAsync().MatchAsync(x => Task.FromResult(GetError()), e => Task.CompletedTask);")]
    [InlineData("Task<Result> r = from x in GetOkAsync() select GetError();")]
    [InlineData("Task<Result> r = from x in ok from y in GetOkAsync() select GetError();")]
    [InlineData("Task<Result> r = from x in GetOkAsync() from y in GetResultOfInt() select GetError();")]
    public void TaskBased_FunctionReturningError_GivesFailedResult(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("ValueTask<Result> r = ok.ThenAsync(x => new ValueTask<Error>(GetError()));")]
    [InlineData("ValueTask<Result> r = ok.MatchAsync(x => new ValueTask<Error>(GetError()), e => ValueTask.CompletedTask);")]
    [InlineData("ValueTask<Result> r = from x in GetOkValueTask() select GetError();")]
    [InlineData("ValueTask<Result> r = from x in GetOkValueTask() from y in GetResultOfInt() select GetError();")]
    public void ValueTaskBased_FunctionReturningError_GivesFailedResult(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.ValueTask, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("Result<Error> r = GetResult().Then<Error>(() => GetError());")]
    [InlineData("Result<Error> r = ok.Then<int, Error>(x => GetError());")]
    [InlineData("Result<Error> r = ok.Match<int, Error>(x => GetError(), e => { });")]
    [InlineData("Result<Error> r = ok.Select<int, Error>(x => GetError());")]
    public void ExplicitTypeArguments_FunctionReturningError_KeepsSuccessfulResultOfError(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }
}
