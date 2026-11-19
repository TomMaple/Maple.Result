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
///     purpose: calls of Match and MatchAsync.
/// </summary>
public class NoDiagnosticMatchUnitTests
{
    private const string Members = """
        static Result done = Result.Success();
        static Task<Result<int>> okTask = Task.FromResult(Result.FromValue(1));
        static Task<Result> doneTask = Task.FromResult(Result.Success());
        static ValueTask<Result<int>> okVt => new(Result.FromValue(1));
        static ValueTask<Result> doneVt => new(Result.Success());
        static int Compute(int x) => x;
        static void Show(int x) { }
        static void Log(Error e) { }
        static Task<int> GetIntAsync(int x) => Task.FromResult(x);
        static ValueTask<int> GetIntVt(int x) => new(x);
        static Task SaveAsync(int x) => Task.CompletedTask;
        static ValueTask SaveVt(int x) => default;
        static Task LogAsync(Error e) => Task.CompletedTask;
        static ValueTask LogVt(Error e) => default;
        static ValueTask<int> LogVtInt(Error e) => default;
        static Result<int> Validate(int x) => x;
        static Result Check(int x) => Result.Success();
        static Task<Result<int>> ValidateAsync(int x) => Task.FromResult(Result.FromValue(x));
        static ValueTask<Result<int>> ValidateVt(int x) => new(Result.FromValue(x));
        static System.Text.StringBuilder sb = new();
        static HashSet<int> set = new();
        static int last;
        """;

    [Theory]
    [InlineData("string r = ok.Match(x => x.ToString(), e => e.Title);")]
    [InlineData("int r = done.Match(() => 1, e => 0);")]
    [InlineData("int r = ok.Match(x => x, e => throw new InvalidOperationException(e.Title));")]
    [InlineData("int r = ok.Match(x => Compute(x), e => throw new InvalidOperationException(e.Title));")]
    [InlineData("Result<int> r = ok.Match(x => Validate(x), e => e);")]
    [InlineData("Result<int> r = ok.Match(x => Validate(x), e => Validate(0));")]
    [InlineData("Result r = ok.Match(x => Check(x), e => e);")]
    [InlineData("Result r = done.Match(() => Check(1), e => e);")]
    [InlineData("Result<int> r = done.Match(() => Validate(1), e => e);")]
    [InlineData("Result<int> r = ok.Match<int, Result<int>>(x => x + 1, e => e);")]
    [InlineData("Task<int> r = ok.Match(x => GetIntAsync(x), e => GetIntAsync(0));")]
    [InlineData("Task r = ok.Match(x => SaveAsync(x), e => LogAsync(e));")]
    [InlineData("Task<int> r = ok.Match(async x => await GetIntAsync(x), async e => { await LogAsync(e); return 0; });")]
    [InlineData("ValueTask<int> r = ok.Match(x => GetIntVt(x), e => GetIntVt(0));")]
    [InlineData("Task<Result<int>> r = ok.Match(x => ValidateAsync(x), e => ValidateAsync(0));")]
    [InlineData("ValueTask r = done.Match(() => SaveVt(1), e => LogVt(e));")]
    [InlineData("bool r = ok.Match(x => set.Add(x), e => false);")]
    [InlineData("Task<string> r = okTask.MatchAsync(x => x.ToString(), e => e.Title);")]
    [InlineData("Task<int> r = okTask.MatchAsync(x => x, e => throw new InvalidOperationException(e.Title));")]
    [InlineData("Task<Result<int>> r = okTask.MatchAsync(x => Validate(x), e => e);")]
    [InlineData("Task<int> r = okTask.MatchAsync(x => GetIntAsync(x), e => GetIntAsync(0));")]
    [InlineData("Task<int> r = okTask.MatchAsync(async x => await GetIntAsync(x), async e => { await LogAsync(e); return 0; });")]
    [InlineData("Task<int> r = okTask.MatchAsync(x => GetIntAsync(x), e => throw new InvalidOperationException(e.Title));")]
    [InlineData("Task r = okTask.MatchAsync(x => SaveAsync(x), e => LogAsync(e));")]
    [InlineData("Task r = okTask.MatchAsync(async x => await SaveAsync(x), async e => await LogAsync(e));")]
    [InlineData("Task<Result<int>> r = okTask.MatchAsync(x => ValidateAsync(x), e => ValidateAsync(0));")]
    [InlineData("Task<int> r = doneTask.MatchAsync(() => 1, e => 0);")]
    [InlineData("Task<int> r = doneTask.MatchAsync(() => GetIntAsync(1), e => GetIntAsync(0));")]
    [InlineData("Task r = doneTask.MatchAsync(() => SaveAsync(1), e => LogAsync(e));")]
    [InlineData("Task<Result> r = doneTask.MatchAsync(() => Check(1), e => e);")]
    [InlineData("ValueTask<string> r = okVt.MatchAsync(x => x.ToString(), e => e.Title);")]
    [InlineData("ValueTask<int> r = okVt.MatchAsync(x => GetIntVt(x), e => GetIntVt(0));")]
    [InlineData("ValueTask<int> r = okVt.MatchAsync(async x => await GetIntAsync(x), async e => { await LogAsync(e); return 0; });")]
    [InlineData("ValueTask r = okVt.MatchAsync(x => SaveVt(x), e => LogVt(e));")]
    [InlineData("ValueTask r = okVt.MatchAsync(async x => await SaveAsync(x), async e => await LogAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = okVt.MatchAsync(x => ValidateVt(x), e => ValidateVt(0));")]
    [InlineData("ValueTask<int> r = doneVt.MatchAsync(() => 1, e => 0);")]
    [InlineData("ValueTask<int> r = doneVt.MatchAsync(() => GetIntVt(1), e => GetIntVt(0));")]
    [InlineData("ValueTask r = doneVt.MatchAsync(() => SaveVt(1), e => LogVt(e));")]
    [InlineData("Task<int> r = okTask.MatchAsync(x => GetIntVt(x), e => GetIntVt(0));")]
    [InlineData("Task r = okTask.MatchAsync(x => SaveVt(x), e => LogVt(e));")]
    [InlineData("Task r = doneTask.MatchAsync(() => SaveVt(1), e => LogVt(e));")]
    [InlineData("ValueTask<int> r = okVt.MatchAsync(x => GetIntAsync(x), e => GetIntAsync(0));")]
    [InlineData("ValueTask r = okVt.MatchAsync(x => SaveAsync(x), e => LogAsync(e));")]
    [InlineData("ValueTask r = doneVt.MatchAsync(() => SaveAsync(1), e => LogAsync(e));")]
    public void Analyze_MatchCallThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
