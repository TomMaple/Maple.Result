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
///     purpose: functions that return an Error.
/// </summary>
public class NoDiagnosticErrorFunctionUnitTests
{
    private const string Members = """
        static Task<Result<int>> GetOkAsync() => Task.FromResult(Result.FromValue(1));
        static ValueTask<Result<int>> GetOkValueTask() => new(Result.FromValue(1));
        static Error ToError(int value) => err;
        static void LogError(Error error) { }
        """;

    [Theory]
    [InlineData("Result r = ok.Then(x => GetError());")]
    [InlineData("Result r = GetResult().Then(() => GetError());")]
    [InlineData("Result r = ok.Then(ToError);")]
    [InlineData("Result r = from x in ok from y in GetResultOfInt() select GetError();")]
    [InlineData("Task<Result> r = ok.ThenAsync(x => Task.FromResult(GetError()));")]
    [InlineData("Task<Result> r = ok.ThenAsync(async x => { await Task.Yield(); return GetError(); });")]
    [InlineData("Task<Result> r = GetOkAsync().ThenAsync(x => Task.FromResult(GetError()));")]
    [InlineData("Task<Result> r = from x in GetOkAsync() select GetError();")]
    [InlineData("Task<Result> r = from x in ok from y in GetOkAsync() select GetError();")]
    [InlineData("Task<Result> r = from x in GetOkAsync() from y in GetResultOfInt() select GetError();")]
    [InlineData("ValueTask<Result> r = ok.ThenAsync(x => new ValueTask<Error>(GetError()));")]
    [InlineData("ValueTask<Result> r = from x in GetOkValueTask() select GetError();")]
    [InlineData("ValueTask<Result> r = from x in GetOkValueTask() from y in GetResultOfInt() select GetError();")]
    public void Analyze_FunctionReturningErrorThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("Result<Error> r = GetResult().Then<Error>(() => GetError());")]
    [InlineData("Result<Error> r = ok.Then<int, Error>(x => GetError());")]
    [InlineData("Result<Error> r = ok.Select<int, Error>(x => GetError());")]
    public void Analyze_FunctionReturningErrorKeepingTheValueOnPurpose_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
