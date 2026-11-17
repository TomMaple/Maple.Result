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
///     A query whose <c>select</c> returns a task is a compile error (CS0619) for every source, <c>from</c> order and the method syntax
///     with an <c>async</c> lambda (#127), also with explicit type arguments (#251, #270); a result that holds the task keeps it on purpose.
/// </summary>
public class QuerySelectingTaskBindingUnitTests
{
    private const string Members = """
        static Task<Result<int>> GetOkAsync() => Task.FromResult(Result.FromValue(1));
        static ValueTask<Result<int>> GetOkValueTask() => new(Result.FromValue(1));
        static ValueTask SaveValueTask() => default;
        """;

    [Theory]
    [InlineData("var q = from x in ok select SaveValueTask();")]
    [InlineData("var q = from x in ok select GetIntValueTask();")]
    [InlineData("var q = from x in ok from y in ok select SaveAsync();")]
    [InlineData("var q = from x in ok from y in ok select GetIntAsync();")]
    [InlineData("var q = from x in ok from y in ok select SaveValueTask();")]
    [InlineData("var q = from x in ok from y in ok select GetIntValueTask();")]
    [InlineData("var q = from x in GetOkAsync() select SaveAsync();")]
    [InlineData("var q = from x in GetOkAsync() select GetIntAsync();")]
    [InlineData("var q = from x in GetOkAsync() select SaveValueTask();")]
    [InlineData("var q = from x in GetOkAsync() select GetIntValueTask();")]
    [InlineData("var q = from x in GetOkAsync() from y in GetOkAsync() select SaveAsync();")]
    [InlineData("var q = from x in GetOkAsync() from y in GetOkAsync() select GetIntAsync();")]
    [InlineData("var q = from x in GetOkAsync() from y in GetOkAsync() select SaveValueTask();")]
    [InlineData("var q = from x in GetOkAsync() from y in GetOkAsync() select GetIntValueTask();")]
    [InlineData("var q = from x in GetOkValueTask() select SaveAsync();")]
    [InlineData("var q = from x in GetOkValueTask() select GetIntAsync();")]
    [InlineData("var q = from x in GetOkValueTask() select SaveValueTask();")]
    [InlineData("var q = from x in GetOkValueTask() select GetIntValueTask();")]
    [InlineData("var q = from x in GetOkValueTask() from y in GetOkValueTask() select SaveAsync();")]
    [InlineData("var q = from x in GetOkValueTask() from y in GetOkValueTask() select GetIntAsync();")]
    [InlineData("var q = from x in GetOkValueTask() from y in GetOkValueTask() select SaveValueTask();")]
    [InlineData("var q = from x in GetOkValueTask() from y in GetOkValueTask() select GetIntValueTask();")]
    [InlineData("var q = from x in ok from y in GetOkAsync() select SaveAsync();")]
    [InlineData("var q = from x in GetOkAsync() from y in ok select GetIntValueTask();")]
    [InlineData("var q = from x in ok from y in GetOkValueTask() select SaveAsync();")]
    [InlineData("var q = from x in GetOkValueTask() from y in ok select GetIntAsync();")]
    [InlineData("var q = ok.Select(async x => await SaveAsync());")]
    [InlineData("var q = GetOkAsync().Select(async x => await SaveAsync());")]
    [InlineData("var q = GetOkValueTask().Select(async x => await SaveAsync());")]
    public void Query_SelectingTask_ReportsOnlyCS0619(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    // The traps are constrained to the task type on a result (#251) and on a task of a result (#270), so explicit type arguments reach
    // them, and a task is kept in a result instead.
    [Theory]
    [InlineData("Result<Task> r = ok.Select(x => Result.FromValue(SaveAsync()));")]
    [InlineData("Task<Result<Task<int>>> r = GetOkAsync().Select(x => Result.FromValue(GetIntAsync()));")]
    [InlineData("ValueTask<Result<Task<int>>> r = GetOkValueTask().Select(x => Result.FromValue(GetIntAsync()));")]
    public void Select_TaskKeptOnPurpose_KeepsTheTask(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    // Since #197, IfSuccessAsync rejects a function whose task returns a value, so the message names ThenAsync first (#224).
    [Theory]
    [InlineData("var q = from x in ok select GetIntAsync();")]
    [InlineData("var q = from x in GetOkAsync() select GetIntAsync();")]
    [InlineData("var q = from x in GetOkValueTask() select GetIntValueTask();")]
    [InlineData("var q = from x in ok from y in GetOkAsync() select GetIntAsync();")]
    public void Query_SelectingTask_MessageNamesThenAsync(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var message = snippet.Errors.Single(d => d.Id == "CS0619").GetMessage(CultureInfo.InvariantCulture);
        message.ShouldContain("Use ThenAsync (or IfSuccessAsync for a task without a value)");
    }
}
