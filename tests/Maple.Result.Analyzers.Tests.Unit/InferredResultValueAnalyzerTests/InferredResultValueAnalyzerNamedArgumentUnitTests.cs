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
///     on call shapes that compile without a diagnostic of the compiler: calls with named arguments.
/// </summary>
public class InferredResultValueAnalyzerNamedArgumentUnitTests
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
    [InlineData("var r = ok.Then(ifSuccessFunction: x => SaveAsync(x));")]
    [InlineData("var r = ok.Then(ifSuccessFunction: x => GetIntAsync());")]
    [InlineData("var r = ok.Then(ifSuccessFunction: x => SaveVt(x));")]
    [InlineData("var r = ok.Then(ifSuccessFunction: x => GetIntValueTask());")]
    [InlineData("var r = ok.Then(ifSuccessFunction: async x => { await SaveAsync(x); });")]
    [InlineData("var r = ok.Then(ifSuccessFunction: async x => { await SaveAsync(x); return 1; });")]
    [InlineData("var r = plain.Then(ifSuccessFunction: () => SaveAsync());")]
    [InlineData("var r = plain.Then(ifSuccessFunction: () => GetIntAsync());")]
    [InlineData("var r = plain.Then(ifSuccessFunction: () => SaveVt());")]
    [InlineData("var r = plain.Then(ifSuccessFunction: () => GetIntValueTask());")]
    [InlineData("var r = plain.Then(ifSuccessFunction: async () => { await SaveAsync(); });")]
    [InlineData("var r = plain.Then(ifSuccessFunction: async () => { await SaveAsync(); return 1; });")]
    [InlineData("var r = okTask.ThenAsync(ifSuccessFunction: x => SaveAsync(x));")]
    [InlineData("var r = okTask.ThenAsync(ifSuccessFunction: x => SaveVt(x));")]
    [InlineData("var r = okTask.ThenAsync(ifSuccessFunction: async x => { await SaveAsync(x); });")]
    [InlineData("var r = plainTask.ThenAsync(ifSuccessFunction: () => SaveAsync());")]
    [InlineData("var r = plainTask.ThenAsync(ifSuccessFunction: () => SaveVt());")]
    [InlineData("var r = plainTask.ThenAsync(ifSuccessFunction: async () => { await SaveAsync(); });")]
    [InlineData("var r = okVt.ThenAsync(ifSuccessFunction: x => SaveAsync(x));")]
    [InlineData("var r = okVt.ThenAsync(ifSuccessFunction: x => SaveVt(x));")]
    [InlineData("var r = okVt.ThenAsync(ifSuccessFunction: async x => { await SaveAsync(x); });")]
    [InlineData("var r = plainVt.ThenAsync(ifSuccessFunction: () => SaveAsync());")]
    [InlineData("var r = plainVt.ThenAsync(ifSuccessFunction: () => SaveVt());")]
    [InlineData("var r = plainVt.ThenAsync(ifSuccessFunction: async () => { await SaveAsync(); });")]
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: x => SaveAsync(x), ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: x => SaveVt(x), ifErrorFunction: e => LogVt(e));")]
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: async x => { await SaveAsync(x); }, ifErrorFunction: async e => { await LogAsync(e); });")]
    [InlineData("var r = plainTask.MatchAsync(ifSuccessFunction: () => SaveAsync(), ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = plainTask.MatchAsync(ifSuccessFunction: () => SaveVt(), ifErrorFunction: e => LogVt(e));")]
    [InlineData("var r = plainTask.MatchAsync(ifSuccessFunction: async () => { await SaveAsync(); }, ifErrorFunction: async e => { await LogAsync(e); });")]
    [InlineData("var r = okVt.MatchAsync(ifSuccessFunction: x => SaveAsync(x), ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = okVt.MatchAsync(ifSuccessFunction: x => SaveVt(x), ifErrorFunction: e => LogVt(e));")]
    [InlineData("var r = okVt.MatchAsync(ifSuccessFunction: async x => { await SaveAsync(x); }, ifErrorFunction: async e => { await LogAsync(e); });")]
    [InlineData("var r = plainVt.MatchAsync(ifSuccessFunction: () => SaveAsync(), ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = plainVt.MatchAsync(ifSuccessFunction: () => SaveVt(), ifErrorFunction: e => LogVt(e));")]
    [InlineData("var r = plainVt.MatchAsync(ifSuccessFunction: async () => { await SaveAsync(); }, ifErrorFunction: async e => { await LogAsync(e); });")]
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: x => GetIntAsync(), ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: x => SaveAsync(x), ifErrorFunction: e => LogIntAsync(e));")]
    [InlineData("var r = okTask.MatchAsync(x => SaveAsync(x), ifErrorFunction: e => LogAsync(e));")]
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: x => SaveAsync(x), ifErrorFunction: e => LogAsync(e), continueOnCapturedContext: false);")]
    [InlineData("var r = okVt.MatchAsync(ifSuccessFunction: x => SaveVt(x), ifErrorFunction: e => LogVt(e), continueOnCapturedContext: false);")]
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: x => SaveAsync(x), ifErrorFunction: e => throw new InvalidOperationException());")]
    [InlineData("var r = Result.Try(function: () => SaveAsync(), mapException: e => err);")]
    [InlineData("var r = Result.Try(function: () => GetIntAsync(), mapException: e => err);")]
    [InlineData("var r = Result.Try(function: () => SaveVt(), mapException: e => err);")]
    [InlineData("var r = Result.Try(function: () => GetIntValueTask(), mapException: e => err);")]
    [InlineData("var r = Result.Try(function: async () => { await SaveAsync(); }, mapException: e => err);")]
    [InlineData("var r = Result.Try(function: async () => { await SaveAsync(); return 1; }, mapException: e => err);")]
    [InlineData("var r = Result.Try(function: () => SaveAsync(), mapException: (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(function: () => GetIntAsync(), mapException: (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(function: () => SaveVt(), mapException: (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(function: () => GetIntValueTask(), mapException: (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(function: async () => { await SaveAsync(); }, mapException: (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(() => SaveAsync(), e => err);")]
    [InlineData("var r = Result.Try(() => GetIntAsync(), e => err);")]
    [InlineData("var r = Result.Try(() => SaveVt(), e => err);")]
    [InlineData("var r = Result.Try(() => GetIntValueTask(), e => err);")]
    [InlineData("var r = Result.Try(async () => { await SaveAsync(); }, e => err);")]
    [InlineData("var r = Result.Try(async () => { await SaveAsync(); return 1; }, e => err);")]
    [InlineData("var r = Result.Try(() => SaveAsync(), (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(() => GetIntAsync(), (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(() => SaveVt(), (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(() => GetIntValueTask(), (InvalidOperationException e) => err);")]
    [InlineData("var r = Result.Try(async () => { await SaveAsync(); return 1; }, (InvalidOperationException e) => err);")]
    public void Analyze_CallWithNamedArguments_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<InferredResultValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.InferredResultValueId);
    }
}
