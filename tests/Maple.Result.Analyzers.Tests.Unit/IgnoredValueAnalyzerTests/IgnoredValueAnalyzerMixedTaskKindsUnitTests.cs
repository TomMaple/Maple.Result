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
///     on call shapes that compile without a diagnostic of the compiler: calls that mix Task and ValueTask.
/// </summary>
public class IgnoredValueAnalyzerMixedTaskKindsUnitTests
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
    [InlineData("_ = ok.IfSuccessAsync(x => DoubleAsync(x));")]
    [InlineData("_ = ok.IfErrorAsync(e => RecoverAsync(e));")]
    [InlineData("_ = GetTask().IfSuccessAsync(x => DoubleValueAsync(x));")]
    [InlineData("_ = GetValueTask().IfSuccessAsync(x => DoubleAsync(x));")]
    [InlineData("_ = GetTask().IfErrorAsync(e => RecoverValueAsync(e));")]
    [InlineData("_ = GetValueTask().IfErrorAsync(e => RecoverAsync(e));")]
    [InlineData("_ = ok.IfSuccessAsync(x => DoubleValueAsync(x));")]
    [InlineData("_ = ok.IfErrorAsync(e => RecoverValueAsync(e));")]
    [InlineData("_ = GetValueTask().IfSuccessAsync(x => DoubleValueAsync(x));")]
    [InlineData("_ = GetValueTask().IfErrorAsync(e => RecoverValueAsync(e));")]
    public void Analyze_CallMixingTaskKinds_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<IgnoredValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.IgnoredValueId);
    }
}
