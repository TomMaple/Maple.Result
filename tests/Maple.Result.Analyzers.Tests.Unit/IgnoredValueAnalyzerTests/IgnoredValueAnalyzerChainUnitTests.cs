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
///     on call shapes that compile without a diagnostic of the compiler: chained calls.
/// </summary>
public class IgnoredValueAnalyzerChainUnitTests
{
    private const string Members = """
        static Task<Result<int>> okTask = Task.FromResult(Result.FromValue(1));
        static ValueTask<Result<int>> okVt = new(Result.FromValue(1));
        static Result done = Result.Success();
        static Task<Result> doneTask = Task.FromResult(Result.Success());
        static ValueTask<Result> doneVt = new(Result.Success());
        static ValueTask SaveValueTask() => default;
        """;

    [Theory]
    [InlineData("Result<int> r = ok.IfSuccess(x => GetIntValueTask());")]
    [InlineData("var r = ok.IfSuccess(x => GetIntAsync()).Value;")]
    public void Analyze_ChainedCall_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<IgnoredValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.IgnoredValueId);
    }
}
