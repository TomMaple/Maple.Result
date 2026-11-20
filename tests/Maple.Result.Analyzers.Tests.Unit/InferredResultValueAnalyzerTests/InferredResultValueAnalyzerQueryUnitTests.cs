// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Analyzers.Tests.Unit.TestingInfrastructure.Helpers;

namespace Maple.Results.Analyzers.Tests.Unit.InferredResultValueAnalyzerTests;

/// <summary>
///     Tests that the <see cref="InferredResultValueAnalyzer" /> (MR0001) reports a task inferred as the value of a result,
///     on call shapes that compile without a diagnostic of the compiler: LINQ queries and operators.
/// </summary>
public class InferredResultValueAnalyzerQueryUnitTests
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
    [InlineData("var q = from x in ok select GetIntAsync();")]
    [InlineData("var q = from x in ok from y in GetOkAsync() select GetIntAsync();")]
    public void Analyze_Query_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<InferredResultValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.InferredResultValueId);
    }
}
