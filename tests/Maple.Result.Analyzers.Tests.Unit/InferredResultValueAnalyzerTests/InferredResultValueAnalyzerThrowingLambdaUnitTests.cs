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
///     on call shapes that compile without a diagnostic of the compiler: calls next to a lambda that only throws.
/// </summary>
public class InferredResultValueAnalyzerThrowingLambdaUnitTests
{
    private const string Members = """
        static readonly Result done = Result.Success();
        static readonly Task<Result<int>> okTask = Task.FromResult(Result.FromValue(1));
        static int count;
        static void Log(Error e) { }
        static Task LogAsync(Error e) => Task.CompletedTask;
        static ValueTask LogValueAsync(Error e) => ValueTask.CompletedTask;
        static Task<int> GetCountAsync(Error e) => Task.FromResult(1);
        static ValueTask<int> GetCountValueAsync(Error e) => new(1);
        static Task SaveAsync(int x) => Task.CompletedTask;
        static int Compute(int x) => x * 2;
        static Exception Fail() => new InvalidOperationException();
        static Exception Fail(Error e) => new InvalidOperationException(e.Title);
        """;

    [Theory]
    [InlineData("var r = ok.Then(x => GetIntAsync());")]
    [InlineData("var r = ok.Then(async x => await GetIntAsync());")]
    [InlineData("var r = ok.Then(x => SaveAsync(x));")]
    [InlineData("var r = ok.Then(x => GetIntValueTask());")]
    [InlineData("var r = ok.Then(x => ValueTask.CompletedTask);")]
    [InlineData("var r = ok.Then(SaveAsync);")]
    [InlineData("var r = done.Then(() => SaveAsync());")]
    [InlineData("var r = done.Then(() => GetIntValueTask());")]
    [InlineData("var r = from x in ok select GetIntAsync();")]
    [InlineData("var r = from x in ok select GetIntValueTask();")]
    [InlineData("var r = from x in ok select SaveAsync(x);")]
    [InlineData("var r = from x in ok from y in GetResultOfInt() select SaveAsync(x + y);")]
    [InlineData("var r = from x in GetResult() select SaveAsync(1);")]
    public void Analyze_CallNextToThrowingLambda_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<InferredResultValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.InferredResultValueId);
    }
}
