// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Tests.Unit.TestingInfrastructure.TestData;
using System;
using System.Linq;

namespace Maple.Results.Tests.Unit.Binding;

/// <summary>
///     <c>Switch</c> and <c>SwitchAsync</c> take two actions and return nothing (#255). An action that returns a value is a side effect, and
///     a lambda that only throws is fine in either position. A lambda that returns a task compiles too, and nobody awaits the task: the
///     library has no overload trap for it (#223), so the analyzer reports it (rules MR0001 to MR0006 of
///     <c>Maple.Result.Analyzers</c>), and those shapes are tested with the analyzer. Two method groups that return a task are a
///     compile error that the compiler reports by itself.
/// </summary>
public class SwitchBindingUnitTests
{
    private const string Members = MatchSwitchSnippetMembers.Members;

    [Theory]
    [InlineData("ok.Switch(x => Show(x), e => Log(e));")]
    [InlineData("ok.Switch(Show, Log);")]
    [InlineData("ok.Switch(x => { Show(x); }, e => { Log(e); });")]
    [InlineData("ok.Switch(x => sb.Append(x), e => sb.Append(e.Title));")]
    [InlineData("ok.Switch(x => set.Add(x), e => last = 0);")]
    [InlineData("ok.Switch(x => last = x, e => last++);")]
    [InlineData("ok.Switch(x => Show(x), e => throw new InvalidOperationException(e.Title));")]
    [InlineData("ok.Switch(x => throw new InvalidOperationException(), e => Log(e));")]
    [InlineData("ok.Switch(x => throw new InvalidOperationException(), e => throw new InvalidOperationException(e.Title));")]
    [InlineData("ok.Switch(x => { _ = SaveAsync(x); }, e => Log(e));")]
    [InlineData("done.Switch(() => Show(1), e => Log(e));")]
    [InlineData("done.Switch(() => sb.Append(1), e => throw new InvalidOperationException(e.Title));")]
    [InlineData("Task r = okTask.SwitchAsync(x => Show(x), e => Log(e));")]
    [InlineData("Task r = okTask.SwitchAsync(x => Show(x), e => throw new InvalidOperationException(e.Title));")]
    [InlineData("Task r = okTask.SwitchAsync(x => sb.Append(x), e => Log(e));")]
    [InlineData("Task r = doneTask.SwitchAsync(() => Show(1), e => Log(e));")]
    [InlineData("ValueTask r = okVt.SwitchAsync(x => Show(x), e => Log(e));")]
    [InlineData("ValueTask r = okVt.SwitchAsync(x => sb.Append(x), e => throw new InvalidOperationException(e.Title));")]
    [InlineData("ValueTask r = doneVt.SwitchAsync(() => Show(1), e => Log(e));")]
    public void Switch_TwoActions_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    // A method group does not convert to an action when the method returns a value, so the compiler reports it by itself.
    [Theory]
    [InlineData("ok.Switch(SaveAsync, LogAsync);", "CS1929")]
    public void Switch_MethodGroupsReturningTask_ReportsTheCompilerErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }
}
