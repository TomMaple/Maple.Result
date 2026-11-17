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
using System.Globalization;
using System.Linq;

namespace Maple.Results.Tests.Unit.Binding;

/// <summary>
///     <c>Switch</c> and <c>SwitchAsync</c> take two actions and return nothing (#255). An action that returns a value is a side effect, and
///     a lambda that only throws is fine in either position. An action that returns a task is a compile error (CS0619) that names
///     <c>Match</c>, except for two actions that return a <c>ValueTask</c>, or one a <c>Task</c> and the other a <c>ValueTask</c>, which
///     are ambiguous (CS0121).
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
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("ok.Switch(x => SaveAsync(x), e => Log(e));", "CS0619")]
    [InlineData("ok.Switch(x => Show(x), e => LogAsync(e));", "CS0619")]
    [InlineData("ok.Switch(x => SaveAsync(x), e => LogAsync(e));", "CS0619")]
    [InlineData("ok.Switch(x => GetIntAsync(x), e => Log(e));", "CS0619")]
    [InlineData("ok.Switch(async x => await SaveAsync(x), e => Log(e));", "CS0619")]
    [InlineData("ok.Switch(x => Show(x), async e => await LogAsync(e));", "CS0619")]
    [InlineData("ok.Switch(async x => await SaveAsync(x), async e => await LogAsync(e));", "CS0619")]
    [InlineData("ok.Switch(x => SaveVt(x), e => Log(e));", "CS0619")]
    [InlineData("ok.Switch(x => Show(x), e => LogVt(e));", "CS0619")]
    [InlineData("ok.Switch(x => GetIntVt(x), e => Log(e));", "CS0619")]
    [InlineData("ok.Switch(x => Show(x), e => LogVtInt(e));", "CS0619")]
    [InlineData("ok.Switch(x => SaveVt(x), e => LogVt(e));", "CS0121")]
    [InlineData("ok.Switch(x => GetIntVt(x), e => LogVtInt(e));", "CS0121")]
    [InlineData("ok.Switch(x => SaveAsync(x), e => LogVt(e));", "CS0121")]
    [InlineData("ok.Switch(x => SaveVt(x), e => LogAsync(e));", "CS0121")]
    [InlineData("ok.Switch(x => GetIntVt(x), e => LogAsync(e));", "CS0619")]
    [InlineData("ok.Switch(SaveAsync, LogAsync);", "CS0619")]
    [InlineData("ok.Switch(x => SaveAsync(x), e => throw new InvalidOperationException(e.Title));", "CS0619")]
    [InlineData("done.Switch(() => SaveAsync(1), e => Log(e));", "CS0619")]
    [InlineData("done.Switch(() => Show(1), e => LogVt(e));", "CS0619")]
    [InlineData("done.Switch(async () => await SaveAsync(1), async e => await LogAsync(e));", "CS0619")]
    [InlineData("okTask.SwitchAsync(x => SaveAsync(x), e => Log(e));", "CS0619")]
    [InlineData("okTask.SwitchAsync(x => Show(x), e => LogAsync(e));", "CS0619")]
    [InlineData("okTask.SwitchAsync(x => SaveAsync(x), e => LogAsync(e));", "CS0619")]
    [InlineData("okTask.SwitchAsync(x => Show(x), async e => await LogAsync(e));", "CS0619")]
    [InlineData("okTask.SwitchAsync(x => SaveVt(x), e => Log(e));", "CS0619")]
    [InlineData("okTask.SwitchAsync(x => SaveVt(x), e => LogVt(e));", "CS0121")]
    [InlineData("doneTask.SwitchAsync(() => Show(1), e => LogAsync(e));", "CS0619")]
    [InlineData("okVt.SwitchAsync(x => SaveAsync(x), e => Log(e));", "CS0619")]
    [InlineData("okVt.SwitchAsync(x => Show(x), e => LogVt(e));", "CS0619")]
    [InlineData("okVt.SwitchAsync(async x => await SaveAsync(x), async e => await LogAsync(e));", "CS0619")]
    [InlineData("doneVt.SwitchAsync(() => SaveVt(1), e => Log(e));", "CS0619")]
    public void Switch_ActionReturningTask_ReportsOnlyTheStatedErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("ok.Switch(x => SaveAsync(x), e => Log(e));")]
    [InlineData("ok.Switch(x => Show(x), e => LogVt(e));")]
    [InlineData("done.Switch(async () => await SaveAsync(1), async e => await LogAsync(e));")]
    public void Switch_ActionReturningTask_MessageNamesMatch(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        var message = snippet.Errors.Single(d => d.Id == "CS0619").GetMessage(CultureInfo.InvariantCulture);
        message.ShouldEndWith("is obsolete: 'An action returns a task, which Switch does not await. Use Match with two functions that return "
                              + "a task.'");
    }

    [Theory]
    [InlineData("okTask.SwitchAsync(x => SaveAsync(x), e => Log(e));")]
    [InlineData("doneTask.SwitchAsync(() => Show(1), e => LogAsync(e));")]
    [InlineData("okVt.SwitchAsync(x => Show(x), e => LogVt(e));")]
    [InlineData("doneVt.SwitchAsync(() => SaveVt(1), e => Log(e));")]
    public void SwitchAsync_ActionReturningTask_MessageNamesMatchAsync(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        var message = snippet.Errors.Single(d => d.Id == "CS0619").GetMessage(CultureInfo.InvariantCulture);
        message.ShouldEndWith("is obsolete: 'An action returns a task, which SwitchAsync does not await. Use MatchAsync with two functions that "
                              + "return a task.'");
    }
}
