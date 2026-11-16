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
///     An error action that returns a task is a compile error (CS0619) in <c>IfError</c> and in every synchronous <c>Match</c>, and
///     so is an <c>async</c> success lambda in the synchronous <c>Match</c>, not an ambiguity (CS0121) (#124). Two functions that
///     return a task still give the task, and a statement lambda that discards the task still compiles.
/// </summary>
public class ErrorActionTaskBindingUnitTests
{
    private const string Members = """
        static Task<int> LogIntAsync(Error error) => Task.FromResult(1);
        static ValueTask LogValueTask(Error error) => default;
        static ValueTask<int> LogIntValueTask(Error error) => new(1);
        static Task LogAsync(Error error) => Task.CompletedTask;
        """;

    [Theory]
    [InlineData("_ = GetResult().IfError(e => LogAsync(e));")]
    [InlineData("_ = GetResult().Match(() => { }, e => LogAsync(e));")]
    [InlineData("_ = GetResult().Match(() => 5, e => LogAsync(e));")]
    [InlineData("_ = GetResult().IfError(e => LogIntAsync(e));")]
    [InlineData("_ = GetResult().Match(() => { }, e => LogIntAsync(e));")]
    [InlineData("_ = GetResult().Match(() => 5, e => LogIntAsync(e));")]
    [InlineData("_ = GetResult().IfError(e => LogValueTask(e));")]
    [InlineData("_ = GetResult().Match(() => { }, e => LogValueTask(e));")]
    [InlineData("_ = GetResult().Match(() => 5, e => LogValueTask(e));")]
    [InlineData("_ = GetResult().IfError(e => LogIntValueTask(e));")]
    [InlineData("_ = GetResult().Match(() => { }, e => LogIntValueTask(e));")]
    [InlineData("_ = GetResult().Match(() => 5, e => LogIntValueTask(e));")]
    [InlineData("_ = GetResult().IfError(async e => await LogAsync(e));")]
    [InlineData("_ = GetResult().Match(() => { }, async e => await LogAsync(e));")]
    [InlineData("_ = GetResult().Match(() => 5, async e => await LogAsync(e));")]
    [InlineData("_ = GetResult().IfError(LogAsync);")]
    [InlineData("_ = GetResult().Match(() => { }, LogAsync);")]
    [InlineData("_ = GetResult().Match(() => 5, LogAsync);")]
    [InlineData("_ = ok.IfError(e => LogAsync(e));")]
    [InlineData("_ = ok.Match(x => { }, e => LogAsync(e));")]
    [InlineData("_ = ok.Match(x => x + 1, e => LogAsync(e));")]
    [InlineData("_ = ok.IfError(e => LogIntAsync(e));")]
    [InlineData("_ = ok.Match(x => { }, e => LogIntAsync(e));")]
    [InlineData("_ = ok.Match(x => x + 1, e => LogIntAsync(e));")]
    [InlineData("_ = ok.IfError(e => LogValueTask(e));")]
    [InlineData("_ = ok.Match(x => { }, e => LogValueTask(e));")]
    [InlineData("_ = ok.Match(x => x + 1, e => LogValueTask(e));")]
    [InlineData("_ = ok.IfError(e => LogIntValueTask(e));")]
    [InlineData("_ = ok.Match(x => { }, e => LogIntValueTask(e));")]
    [InlineData("_ = ok.Match(x => x + 1, e => LogIntValueTask(e));")]
    [InlineData("_ = ok.IfError(async e => await LogAsync(e));")]
    [InlineData("_ = ok.Match(x => { }, async e => await LogAsync(e));")]
    [InlineData("_ = ok.Match(x => x + 1, async e => await LogAsync(e));")]
    [InlineData("_ = ok.IfError(LogAsync);")]
    [InlineData("_ = ok.Match(x => { }, LogAsync);")]
    [InlineData("_ = ok.Match(x => x + 1, LogAsync);")]
    public void IfErrorOrMatch_ErrorActionReturningTask_ReportsOnlyCS0619(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("_ = GetResult().Match(async () => await SaveAsync(), e => { });")]
    [InlineData("_ = ok.Match(async x => await SaveAsync(), e => { });")]
    [InlineData("_ = ok.Match(async x => { await SaveAsync(); return 1; }, e => { });")]
    public void Match_AsyncSuccessLambda_ReportsOnlyCS0619(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("Task t = ok.Match(x => SaveAsync(), e => LogAsync(e));")]
    [InlineData("Task t = GetResult().Match(() => SaveAsync(), e => LogAsync(e));")]
    [InlineData("Result<int> r = ok.Match(x => { }, e => { _ = LogAsync(e); });")]
    [InlineData("Result r = GetResult().IfError(e => { _ = LogValueTask(e); });")]
    public void Match_TwoFunctionsReturningTaskOrDiscardedTask_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }
}
