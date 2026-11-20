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
///     on call shapes that compile without a diagnostic of the compiler: functions that return a value.
/// </summary>
public class IgnoredValueAnalyzerValueFunctionUnitTests
{
    private const string Members = """
        static readonly Error Err = Error.Failure(ErrorUri.None, "x");
        static int Last;
        static int Count;
        static Error? LastError;
        static Result<int> Ok() => 1;
        static Result<int> Failed() => Err;
        static Task<Error> GetErrorAsync() => Task.FromResult(Err);
        static ValueTask<int> GetIntVt() => new(1);
        static ValueTask<Error> GetErrorVt() => new(Err);
        """;

    [Theory]
    [InlineData("var r = Ok().IfSuccess(x => Count++);")]
    [InlineData("var r = Ok().IfSuccess(x => ++Count);")]
    [InlineData("var r = Failed().IfError(e => Count++);")]
    [InlineData("var r = Task.FromResult(Ok()).IfSuccessAsync(x => Count++);")]
    [InlineData("var r = new ValueTask<Result<int>>(Failed()).IfErrorAsync(e => Count++);")]
    public void Analyze_FunctionReturningValue_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<IgnoredValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.IgnoredValueId);
    }
}
