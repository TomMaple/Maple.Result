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
///     An error action that returns a task is a compile error (CS0619) in <c>IfError</c>, not an ambiguity (CS0121) (#124); <c>Match</c>
///     and <c>Switch</c> are covered by their own tests (#255). Two functions of <c>Match</c> that return a task still give the task, and
///     a statement lambda that discards the task still compiles.
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
    [InlineData("_ = GetResult().IfError(e => LogIntAsync(e));")]
    [InlineData("_ = GetResult().IfError(e => LogValueTask(e));")]
    [InlineData("_ = GetResult().IfError(e => LogIntValueTask(e));")]
    [InlineData("_ = GetResult().IfError(async e => await LogAsync(e));")]
    [InlineData("_ = GetResult().IfError(LogAsync);")]
    [InlineData("_ = ok.IfError(e => LogAsync(e));")]
    [InlineData("_ = ok.IfError(e => LogIntAsync(e));")]
    [InlineData("_ = ok.IfError(e => LogValueTask(e));")]
    [InlineData("_ = ok.IfError(e => LogIntValueTask(e));")]
    [InlineData("_ = ok.IfError(async e => await LogAsync(e));")]
    [InlineData("_ = ok.IfError(LogAsync);")]
    public void IfError_ErrorActionReturningTask_ReportsOnlyCS0619(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("Task t = ok.Match(x => SaveAsync(), e => LogAsync(e));")]
    [InlineData("Task t = GetResult().Match(() => SaveAsync(), e => LogAsync(e));")]
    [InlineData("ok.Switch(x => { }, e => { _ = LogAsync(e); });")]
    [InlineData("Result r = GetResult().IfError(e => { _ = LogValueTask(e); });")]
    public void MatchOrSwitch_TwoFunctionsReturningTaskOrDiscardedTask_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }
}
