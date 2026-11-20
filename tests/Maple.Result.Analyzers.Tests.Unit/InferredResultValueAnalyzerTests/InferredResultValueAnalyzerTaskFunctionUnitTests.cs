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
///     on call shapes that compile without a diagnostic of the compiler: functions that return a task, and Result.Try.
/// </summary>
public class InferredResultValueAnalyzerTaskFunctionUnitTests
{
    private const string Members = "";

    [Theory]
    [InlineData("Result.Try(async () => { await Task.Yield(); return 5; }, (IOException e) => err);")]
    [InlineData("Result.Try(() => GetIntValueTask(), (IOException e) => err);")]
    [InlineData("Result.Try(GetIntValueTask, (IOException e) => err);")]
    [InlineData("Result.Try(async () => await GetIntAsync(), (IOException e) => err);")]
    [InlineData("Result.Try(() => GetIntAsync(), (IOException e) => err);")]
    [InlineData("Result.Try(() => SaveAsync(), (IOException e) => err);")]
    [InlineData("Result.Try(() => GetIntValueTask(), e => err);")]
    [InlineData("Result.Try(() => GetIntAsync(), e => err);")]
    [InlineData("var q = from x in ok select GetIntAsync();")]
    [InlineData("var q = from x in ok select SaveAsync();")]
    public void Analyze_FunctionReturningTask_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<InferredResultValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.InferredResultValueId);
    }
}
