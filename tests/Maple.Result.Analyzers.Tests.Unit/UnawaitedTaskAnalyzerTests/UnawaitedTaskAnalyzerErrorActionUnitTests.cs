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
///     Tests that the <see cref="UnawaitedTaskAnalyzer" /> (MR0002) reports a task that a side-effect method does not await,
///     on call shapes that compile without a diagnostic of the compiler: error actions of IfError.
/// </summary>
public class UnawaitedTaskAnalyzerErrorActionUnitTests
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
    [InlineData("_ = ok.IfError(e => LogAsync(e));")]
    [InlineData("_ = ok.IfError(e => LogIntAsync(e));")]
    [InlineData("_ = ok.IfError(e => LogValueTask(e));")]
    [InlineData("_ = ok.IfError(e => LogIntValueTask(e));")]
    [InlineData("_ = ok.IfError(async e => await LogAsync(e));")]
    public void Analyze_ErrorAction_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<UnawaitedTaskAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.UnawaitedTaskId);
    }
}
