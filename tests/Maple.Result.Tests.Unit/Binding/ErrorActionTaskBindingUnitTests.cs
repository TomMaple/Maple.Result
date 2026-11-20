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
///     An error action that returns a task in <c>IfError</c> (#124): a method group is a compile error that the compiler reports by
///     itself; a lambda compiles and discards the task, which the analyzer reports (rules MR0001 to MR0006 of
///     <c>Maple.Result.Analyzers</c>), so those shapes are tested with the analyzer (#223). <c>Match</c> and <c>Switch</c> are covered by
///     their own tests (#255). Two functions of <c>Match</c> that return a task still give the task, and a statement lambda that
///     discards the task still compiles.
/// </summary>
public class ErrorActionTaskBindingUnitTests
{
    private const string Members = """
        static Task<int> LogIntAsync(Error error) => Task.FromResult(1);
        static ValueTask LogValueTask(Error error) => default;
        static ValueTask<int> LogIntValueTask(Error error) => new(1);
        static Task LogAsync(Error error) => Task.CompletedTask;
        """;

    // A method group does not convert to an action when the method returns a value, so the compiler reports it by itself.
    [Theory]
    [InlineData("_ = GetResult().IfError(LogAsync);", "CS0407")]
    [InlineData("_ = ok.IfError(LogAsync);", "CS0407")]
    public void IfError_MethodGroupReturningTask_ReportsTheCompilerErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
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
