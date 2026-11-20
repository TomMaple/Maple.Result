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

namespace Maple.Results.Analyzers.Tests.Unit.NoDiagnosticTests;

/// <summary>
///     Tests that no analyzer reports the call shapes that are correct, or that keep a task or an <c>Error</c> as a value on
///     purpose: LINQ queries and operators.
/// </summary>
public class NoDiagnosticQueryUnitTests
{
    private const string Members = """
        static Task<Result<int>> GetOkAsync() => Task.FromResult(Result.FromValue(1));
        static ValueTask<Result<int>> GetOkValueTask() => new(Result.FromValue(1));
        static ValueTask SaveValueTask() => default;
        """;

    [Theory]
    [InlineData("Result<Task> r = ok.Select(x => Result.FromValue(SaveAsync()));")]
    [InlineData("Task<Result<Task<int>>> r = GetOkAsync().Select(x => Result.FromValue(GetIntAsync()));")]
    [InlineData("ValueTask<Result<Task<int>>> r = GetOkValueTask().Select(x => Result.FromValue(GetIntAsync()));")]
    public void Analyze_QueryKeepingTheValueOnPurpose_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
