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

namespace Maple.Results.Analyzers.Tests.Unit.InferredResultValueAnalyzerTests;

/// <summary>
///     Tests that the <see cref="InferredResultValueAnalyzer" /> (MR0001) reports a task inferred as the value of a result,
///     on call shapes that compile without a diagnostic of the compiler: calls that mix Task and ValueTask.
/// </summary>
public class InferredResultValueAnalyzerMixedTaskKindsUnitTests
{
    private const string Members = """
        static readonly Result done = Result.Success();
        static Task<Result<int>> GetTask() => Task.FromResult(Result.FromValue(1));
        static ValueTask<Result<int>> GetValueTask() => ValueTask.FromResult(Result.FromValue(1));
        static Task<int> DoubleAsync(int x) => Task.FromResult(x * 2);
        static ValueTask<int> DoubleValueAsync(int x) => ValueTask.FromResult(x * 2);
        static Task<Result<int>> NextAsync(int x) => Task.FromResult(Result.FromValue(x + 1));
        static ValueTask<Result<int>> NextValueAsync(int x) => ValueTask.FromResult(Result.FromValue(x + 1));
        static Task StoreAsync(int x) => Task.CompletedTask;
        static ValueTask StoreValueAsync(int x) => ValueTask.CompletedTask;
        static Task LogAsync(Error e) => Task.CompletedTask;
        static ValueTask LogValueAsync(Error e) => ValueTask.CompletedTask;
        static Task<int> RecoverAsync(Error e) => Task.FromResult(0);
        static ValueTask<int> RecoverValueAsync(Error e) => ValueTask.FromResult(0);
        static Task<Result> GetDoneTask() => Task.FromResult(Result.Success());
        static ValueTask<Result> GetDoneValueTask() => ValueTask.FromResult(Result.Success());
        static Task<Error> TranslateAsync(Error e) => Task.FromResult(e);
        static ValueTask<Error> TranslateValueAsync(Error e) => ValueTask.FromResult(e);
        static Task<Result<int>> FallbackAsync(Error e) => Task.FromResult(Result.FromValue(0));
        static ValueTask<Result<int>> FallbackValueAsync(Error e) => ValueTask.FromResult(Result.FromValue(0));
        static Task<Result> CheckAsync(int x) => Task.FromResult(Result.Success());
        static ValueTask<Result> CheckValueAsync(int x) => ValueTask.FromResult(Result.Success());
        static Task<Error> RejectAsync(int x) => Task.FromResult(Error.Failure(ErrorUri.None, "x"));
        static ValueTask<Error> RejectValueAsync(int x) => ValueTask.FromResult(Error.Failure(ErrorUri.None, "x"));
        """;

    [Theory]
    [InlineData("_ = GetTask().ThenAsync(x => StoreAsync(x));")]
    [InlineData("_ = GetValueTask().ThenAsync(x => StoreValueAsync(x));")]
    [InlineData("_ = GetTask().ThenAsync(x => StoreValueAsync(x));")]
    [InlineData("_ = GetValueTask().ThenAsync(x => StoreAsync(x));")]
    [InlineData("_ = GetDoneTask().ThenAsync(() => StoreValueAsync(1));")]
    [InlineData("_ = GetDoneValueTask().ThenAsync(() => StoreAsync(1));")]
    public void Analyze_CallMixingTaskKinds_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<InferredResultValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.InferredResultValueId);
    }
}
