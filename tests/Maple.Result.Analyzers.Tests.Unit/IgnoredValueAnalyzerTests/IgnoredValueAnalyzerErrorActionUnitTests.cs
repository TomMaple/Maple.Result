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

namespace Maple.Results.Analyzers.Tests.Unit.IgnoredValueAnalyzerTests;

/// <summary>
///     Tests that the <see cref="IgnoredValueAnalyzer" /> (MR0005) reports a value that <c>IfSuccess</c> or <c>IfError</c> ignores,
///     on call shapes that compile without a diagnostic of the compiler: error actions of IfError.
/// </summary>
public class IgnoredValueAnalyzerErrorActionUnitTests
{
    private const string Members = """
        static Task<int> LogIntAsync(Error error) => Task.FromResult(1);
        static ValueTask LogValueTask(Error error) => default;
        static ValueTask<int> LogIntValueTask(Error error) => new(1);
        static Task LogAsync(Error error) => Task.CompletedTask;
        """;

    [Theory]
    [InlineData("_ = ok.IfError(e => LogIntAsync(e));")]
    [InlineData("_ = ok.IfError(e => LogIntValueTask(e));")]
    public void Analyze_ErrorAction_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<IgnoredValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.IgnoredValueId);
    }
}
