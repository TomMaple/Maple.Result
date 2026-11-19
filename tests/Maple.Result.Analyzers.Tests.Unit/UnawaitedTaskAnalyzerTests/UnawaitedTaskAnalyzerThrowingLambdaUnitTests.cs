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

namespace Maple.Results.Analyzers.Tests.Unit.UnawaitedTaskAnalyzerTests;

/// <summary>
///     Tests that the <see cref="UnawaitedTaskAnalyzer" /> (MR0002) reports a task that a side effect method does not await,
///     on call shapes that compile without a diagnostic of the compiler: calls next to a lambda that only throws.
/// </summary>
public class UnawaitedTaskAnalyzerThrowingLambdaUnitTests
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
    [InlineData("var r = ok.IfError(e => LogAsync(e));")]
    [InlineData("var r = ok.IfError(async e => { await LogAsync(e); });")]
    [InlineData("var r = ok.IfError(e => LogValueAsync(e));")]
    [InlineData("var r = ok.IfError(e => GetCountAsync(e));")]
    [InlineData("var r = ok.IfError(e => GetCountValueAsync(e));")]
    [InlineData("var r = ok.IfError(async e => { throw Fail(); });")]
    [InlineData("var r = ok.IfSuccess(x => SaveAsync(x));")]
    [InlineData("var r = ok.IfSuccess(async x => await SaveAsync(x));")]
    [InlineData("var r = ok.IfSuccess(x => GetIntAsync());")]
    [InlineData("var r = ok.IfSuccess(x => GetIntValueTask());")]
    [InlineData("var r = done.IfSuccess(() => SaveAsync());")]
    [InlineData("var r = done.IfSuccess(() => GetIntValueTask());")]
    [InlineData("ok.Switch(x => { }, e => LogAsync(e));")]
    public void Analyze_CallNextToThrowingLambda_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<UnawaitedTaskAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.UnawaitedTaskId);
    }
}
