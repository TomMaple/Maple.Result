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
///     A query whose <c>select</c> returns a task (#127): a selector that returns a result that holds the task keeps it on purpose, in a
///     result, a task of a result and a value task of a result. A selector that returns the task itself compiles too, and nobody awaits
///     the task: the library has no overload trap for it (#223), so the analyzer reports it (rules MR0001 to MR0006 of
///     <c>Maple.Result.Analyzers</c>), and those shapes are tested with the analyzer.
/// </summary>
public class QuerySelectingTaskBindingUnitTests
{
    private const string Members = """
        static Task<Result<int>> GetOkAsync() => Task.FromResult(Result.FromValue(1));
        static ValueTask<Result<int>> GetOkValueTask() => new(Result.FromValue(1));
        """;

    // A selector that returns a result is flattened, so the task in it is the value of the result of the query: nothing wraps it by
    // accident, and the analyzer does not report it.
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
}
