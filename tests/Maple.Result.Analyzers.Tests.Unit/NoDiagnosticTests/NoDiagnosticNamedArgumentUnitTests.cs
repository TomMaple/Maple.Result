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
///     purpose: calls with named arguments.
/// </summary>
public class NoDiagnosticNamedArgumentUnitTests
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
    [InlineData("Result<int> r = ok.Then(ifSuccessFunction: x => x + 1);")]
    [InlineData("Result<int> r = plain.Then(ifSuccessFunction: () => Compute());")]
    [InlineData("Task<Result<int>> r = ok.ThenAsync(ifSuccessFunction: x => GetIntAsync());")]
    [InlineData("Task<Result<int>> r = okTask.ThenAsync(ifSuccessFunction: x => x + 1);")]
    [InlineData("Task<Result<int>> r = okTask.ThenAsync(ifSuccessFunction: x => GetIntAsync());")]
    [InlineData("Task<Result<int>> r = okTask.ThenAsync(ifSuccessFunction: x => GetIntValueTask());")]
    [InlineData("Task<Result<int>> r = okTask.ThenAsync(ifSuccessFunction: async x => { await SaveAsync(x); return 1; });")]
    [InlineData("Task<Result<int>> r = plainTask.ThenAsync(ifSuccessFunction: () => GetIntAsync());")]
    [InlineData("ValueTask<Result<int>> r = okVt.ThenAsync(ifSuccessFunction: x => GetIntValueTask());")]
    [InlineData("ValueTask<Result<int>> r = okVt.ThenAsync(ifSuccessFunction: x => GetIntAsync());")]
    [InlineData("ValueTask<Result<int>> r = plainVt.ThenAsync(ifSuccessFunction: () => GetIntValueTask());")]
    [InlineData("Result<int> r = ok.Else(ifErrorFunction: e => 0);")]
    [InlineData("Result<int> r = ok.Else(ifErrorFunction: e => GetError());")]
    [InlineData("Task<Result<int>> r = ok.ElseAsync(ifErrorFunction: e => RecoverAsync(e));")]
    [InlineData("Task<Result<int>> r = okTask.ElseAsync(ifErrorFunction: e => RecoverAsync(e));")]
    [InlineData("Task<Result<int>> r = okTask.ElseAsync(ifErrorFunction: e => GetResultOfInt());")]
    [InlineData("ValueTask<Result<int>> r = okVt.ElseAsync(ifErrorFunction: e => RecoverAsync(e));")]
    [InlineData("Task r = okTask.MatchAsync(x => SaveAsync(x), e => LogAsync(e));")]
    [InlineData("Task r = okTask.MatchAsync(x => SaveAsync(x), e => LogAsync(e), false);")]
    [InlineData("Task r = okTask.MatchAsync(x => SaveVt(x), e => LogVt(e));")]
    [InlineData("Task r = okTask.MatchAsync(x => SaveVt(x), e => LogVt(e), false);")]
    [InlineData("Task r = okTask.MatchAsync(async x => await SaveAsync(x), async e => await LogAsync(e));")]
    [InlineData("Task r = okTask.MatchAsync(ifSuccessAction: x => SaveAsync(x), ifErrorAction: e => LogAsync(e));")]
    [InlineData("Task r = okTask.MatchAsync(ifSuccessAction: x => SaveVt(x), ifErrorAction: e => LogVt(e));")]
    [InlineData("Task r = plainTask.MatchAsync(() => SaveAsync(), e => LogAsync(e));")]
    [InlineData("Task r = plainTask.MatchAsync(() => SaveVt(), e => LogVt(e));")]
    [InlineData("ValueTask r = okVt.MatchAsync(x => SaveVt(x), e => LogVt(e));")]
    [InlineData("ValueTask r = okVt.MatchAsync(x => SaveVt(x), e => LogVt(e), false);")]
    [InlineData("ValueTask r = okVt.MatchAsync(x => SaveAsync(x), e => LogAsync(e));")]
    [InlineData("ValueTask r = okVt.MatchAsync(async x => await SaveAsync(x), async e => await LogAsync(e));")]
    [InlineData("ValueTask r = okVt.MatchAsync(ifSuccessAction: x => SaveVt(x), ifErrorAction: e => LogVt(e));")]
    [InlineData("ValueTask r = plainVt.MatchAsync(() => SaveVt(), e => LogVt(e));")]
    [InlineData("ValueTask r = plainVt.MatchAsync(() => SaveAsync(), e => LogAsync(e));")]
    [InlineData("Task<int> r = okTask.MatchAsync(ifSuccessFunction: x => GetIntAsync(), ifErrorFunction: e => LogIntAsync(e));")]
    [InlineData("Task<int> r = okTask.MatchAsync(ifSuccessFunction: x => GetIntValueTask(), ifErrorFunction: e => LogIntVt(e));")]
    [InlineData("Task<int> r = okTask.MatchAsync(ifSuccessFunction: x => x, ifErrorFunction: e => 0);")]
    [InlineData("Task<int> r = okTask.MatchAsync(ifSuccessFunction: async x => { await SaveAsync(x); return 1; }, ifErrorFunction: e => LogIntAsync(e));")]
    [InlineData("Task<int> r = plainTask.MatchAsync(ifSuccessFunction: () => GetIntAsync(), ifErrorFunction: e => LogIntAsync(e));")]
    [InlineData("ValueTask<int> r = okVt.MatchAsync(ifSuccessFunction: x => GetIntValueTask(), ifErrorFunction: e => LogIntVt(e));")]
    [InlineData("ValueTask<int> r = okVt.MatchAsync(ifSuccessFunction: x => GetIntAsync(), ifErrorFunction: e => LogIntAsync(e));")]
    [InlineData("ValueTask<int> r = okVt.MatchAsync(ifSuccessFunction: x => x, ifErrorFunction: e => 0);")]
    [InlineData("ValueTask<int> r = plainVt.MatchAsync(ifSuccessFunction: () => GetIntValueTask(), ifErrorFunction: e => LogIntVt(e));")]
    [InlineData("Task<int> r = ok.Match(ifSuccessFunction: x => GetIntAsync(), ifErrorFunction: e => LogIntAsync(e));")]
    [InlineData("Task<int> r = ok.Match(x => GetIntAsync(), e => LogIntAsync(e));")]
    [InlineData("Result<int> r = Result.Try(function: () => Compute(), mapException: e => err);")]
    [InlineData("Result<int> r = Result.Try(function: () => Compute(), mapException: (InvalidOperationException e) => err);")]
    [InlineData("Result<int> r = Result.Try<int, InvalidOperationException>(() => Compute(), e => err);")]
    [InlineData("Result r = Result.Try(action: () => Console.WriteLine(), mapException: e => err);")]
    [InlineData("Result r = Result.Try(function: () => GetResult(), mapException: e => err);")]
    [InlineData("Result<int> r = Result.Try(function: () => GetResultOfInt(), mapException: e => err);")]
    [InlineData("Result r = Result.Try(function: () => GetError(), mapException: e => err);")]
    [InlineData("Task<Result> r = Result.TryAsync(action: () => SaveAsync(), mapException: e => err);")]
    [InlineData("Task<Result<int>> r = Result.TryAsync(function: () => GetIntAsync(), mapException: e => err);")]
    [InlineData("ValueTask<Result> r = Result.TryAsync(action: () => SaveVt(), mapException: e => err);")]
    [InlineData("ValueTask<Result<int>> r = Result.TryAsync(function: () => GetIntValueTask(), mapException: e => err);")]
    public void Analyze_CallWithNamedArgumentsThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("Result<Task<int>> r = ok.Then(ifSuccessFunction: x => Result.FromValue(GetIntAsync()));")]
    [InlineData("var r = Result.Try<Task<int>>(() => GetIntAsync(), e => err);")]
    [InlineData("var r = Result.Try<Task<int>, InvalidOperationException>(() => GetIntAsync(), e => err);")]
    [InlineData("Result<Task<int>> r = Result.Try(() => Result.FromValue(GetIntAsync()), e => err);")]
    public void Analyze_CallWithNamedArgumentsKeepingTheValueOnPurpose_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
