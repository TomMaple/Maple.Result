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
///     on call shapes that compile without a diagnostic of the compiler: calls with named arguments.
/// </summary>
public class UnawaitedTaskAnalyzerNamedArgumentUnitTests
{
    private const string Members = """
        static Result plain = Result.Success();
        static Task<Result<int>> okTask = Task.FromResult(Result.FromValue(1));
        static Task<Result> plainTask = Task.FromResult(Result.Success());
        static ValueTask<Result<int>> okVt => new(Result.FromValue(1));
        static ValueTask<Result> plainVt => new(Result.Success());
        static Task SaveAsync(int x) => Task.CompletedTask;
        static ValueTask SaveVt() => default;
        static ValueTask SaveVt(int x) => default;
        static Task LogAsync(Error e) => Task.CompletedTask;
        static ValueTask LogVt(Error e) => default;
        static Task<int> LogIntAsync(Error e) => Task.FromResult(1);
        static ValueTask<int> LogIntVt(Error e) => new(1);
        static Task<Result<int>> RecoverAsync(Error e) => Task.FromResult(Result.FromValue(1));
        """;

    [Theory]
    [InlineData("var r = Result.Try(action: () => SaveAsync(), mapException: e => err);")]
    [InlineData("var r = Result.Try(action: () => GetIntAsync(), mapException: e => err);")]
    [InlineData("var r = Result.Try(action: () => SaveVt(), mapException: e => err);")]
    [InlineData("var r = Result.Try(action: () => GetIntValueTask(), mapException: e => err);")]
    [InlineData("var r = Result.Try(action: async () => { await SaveAsync(); }, mapException: e => err);")]
    [InlineData("var r = Result.Try(action: () => GetIntValueTask(), mapException: (InvalidOperationException e) => err);")]
    public void Analyze_CallWithNamedArguments_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<UnawaitedTaskAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.UnawaitedTaskId);
    }
}
