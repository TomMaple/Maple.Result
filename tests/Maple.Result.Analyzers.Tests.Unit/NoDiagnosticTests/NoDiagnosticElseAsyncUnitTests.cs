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
///     purpose: calls of ElseAsync.
/// </summary>
public class NoDiagnosticElseAsyncUnitTests
{
    private const string Members = """
        static readonly Error Err = Error.Failure(ErrorUri.None, "x");
        static Result<int> failed = Err;
        static Task<Result<int>> failedTask = Task.FromResult(failed);
        static ValueTask<Result<int>> failedVt = new(failed);
        static Result plain = Err;
        static Task<Result> plainTask = Task.FromResult(plain);
        static ValueTask<Result> plainVt = new(plain);
        static Task LogAsync(Error e) => Task.CompletedTask;
        static Task<Error> GetErrorAsync() => Task.FromResult(Err);
        static Task<int> IntFromErrorAsync(Error e) => Task.FromResult(0);
        static Task<Result<int>> FallbackAsync() => Task.FromResult<Result<int>>(0);
        static Task<Result<int>> FallbackFromErrorAsync(Error e) => Task.FromResult<Result<int>>(0);
        static ValueTask LogVt(Error e) => default;
        static ValueTask<Error> GetErrorVt() => new(Err);
        static ValueTask<int> IntFromErrorVt(Error e) => new(0);
        static ValueTask<Result<int>> FallbackVt() => new((Result<int>)0);
        static ValueTask<Result<int>> FallbackFromErrorVt(Error e) => new((Result<int>)0);
        """;

    [Theory]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(async e => 0);")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(async e => { await LogAsync(e); return e; });")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(async e => e.WithMetadata(\"k\", 1));")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(async e => await GetIntAsync());")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(e => GetIntAsync());")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(e => GetErrorAsync());")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(IntFromErrorAsync);")]
    [InlineData("Task<Result<int>> r = failed.ElseAsync(e => GetIntAsync());")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(e => IntFromErrorVt(e));")]
    [InlineData("Task<Result<int>> r = failed.ElseAsync(e => GetErrorAsync());")]
    [InlineData("Task<Result> r = plainTask.ElseAsync(async e => e.WithMetadata(\"k\", 1));")]
    [InlineData("Task<Result> r = plain.ElseAsync(e => GetErrorAsync());")]
    [InlineData("Task<Result<int>> r = failed.ElseAsync(async e => 0);")]
    [InlineData("Task<Result<int>> r = failed.ElseAsync(async e => { await LogAsync(e); return 0; });")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(e => FallbackAsync());")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(async e => await FallbackAsync());")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(FallbackFromErrorAsync);")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(async e => Result<int>.FromValue(0));")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(e => 0);")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(e => e);")]
    [InlineData("Task<Result> r = plainTask.ElseAsync(async e => Result.Success());")]
    [InlineData("Task<Result<int>> r = failedTask.ElseAsync(async e => e.Category == ErrorCategory.NotFound ? 0 : e);")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(async e => 0);")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(async e => { await LogVt(e); return e; });")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(async e => e.WithMetadata(\"k\", 1));")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(async e => await IntFromErrorVt(e));")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(e => new ValueTask<int>(0));")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(e => GetErrorVt());")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(IntFromErrorVt);")]
    [InlineData("ValueTask<Result<int>> r = failed.ElseAsync(e => IntFromErrorVt(e));")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(e => IntFromErrorAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = failed.ElseAsync(e => GetErrorVt());")]
    [InlineData("ValueTask<Result> r = plainVt.ElseAsync(async e => e.WithMetadata(\"k\", 1));")]
    [InlineData("ValueTask<Result> r = plain.ElseAsync(e => GetErrorVt());")]
    [InlineData("Task<Result<int>> r = failed.ElseAsync(async e => { await LogVt(e); return 0; });")]
    [InlineData("ValueTask<Result<int>> r = failed.ElseAsync(async ValueTask<int> (Error e) => 0);")]
    [InlineData("ValueTask<Result<int>> r = failed.ElseAsync(async ValueTask<int> (Error e) => { await LogVt(e); return 0; });")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(e => FallbackVt());")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(async e => await FallbackVt());")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(FallbackFromErrorVt);")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(async e => Result<int>.FromValue(0));")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(e => 0);")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(e => e);")]
    [InlineData("ValueTask<Result> r = plainVt.ElseAsync(async e => Result.Success());")]
    [InlineData("ValueTask<Result<int>> r = failedVt.ElseAsync(async e => e.Category == ErrorCategory.NotFound ? 0 : e);")]
    public void Analyze_ElseAsyncCallThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
