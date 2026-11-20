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
///     purpose: calls through IOperationResult.
/// </summary>
public class NoDiagnosticClosedInterfaceUnitTests
{
    private const string Members = "";

    [Theory]
    [InlineData("IOperationResult r = GetResult(); bool failed = r.IsError;")]
    [InlineData("IOperationResult r = ok; bool failed = r.TryGetError(out var error);")]
    [InlineData("IOperationResult r = GetResult(); IOperationResult r2 = r.IfError(e => { });")]
    [InlineData("var list = new List<IOperationResult> { GetResult(), ok };")]
    [InlineData("Task<IOperationResult> task = Task.FromResult<IOperationResult>(GetResult());")]
    public void Analyze_ClosedInterfaceThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
