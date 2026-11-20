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
///     on call shapes that compile without a diagnostic of the compiler: functions that return a task, and Result.Try.
/// </summary>
public class UnawaitedTaskAnalyzerTaskFunctionUnitTests
{
    private const string Members = "";

    [Theory]
    [InlineData("ok.IfError(e => SaveAsync());")]
    public void Analyze_FunctionReturningTask_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<UnawaitedTaskAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.UnawaitedTaskId);
    }
}
