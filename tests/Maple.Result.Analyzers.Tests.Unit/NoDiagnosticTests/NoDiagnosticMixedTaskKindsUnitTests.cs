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
///     purpose: calls that mix Task and ValueTask.
/// </summary>
public class NoDiagnosticMixedTaskKindsUnitTests
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
    [InlineData("Task<Result<int>> r = ok.ThenAsync(x => DoubleAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = ok.ThenAsync(x => DoubleValueAsync(x));")]
    [InlineData("Task<Result<int>> r = ok.ThenAsync(DoubleAsync);")]
    [InlineData("ValueTask<Result<int>> r = ok.ThenAsync(DoubleValueAsync);")]
    [InlineData("Task<Result<int>> r = ok.ThenAsync(x => NextAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = ok.ThenAsync(x => NextValueAsync(x));")]
    [InlineData("Task<Result<int>> r = done.ThenAsync(() => GetIntAsync());")]
    [InlineData("ValueTask<Result<int>> r = done.ThenAsync(() => GetIntValueTask());")]
    [InlineData("Task<Result<int>> r = ok.IfSuccessAsync(x => StoreAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = ok.IfSuccessAsync(x => StoreValueAsync(x));")]
    [InlineData("Task<Result<int>> r = ok.IfErrorAsync(e => LogAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = ok.IfErrorAsync(e => LogValueAsync(e));")]
    [InlineData("Task<Result<int>> r = ok.ElseAsync(e => RecoverAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = ok.ElseAsync(e => RecoverValueAsync(e));")]
    [InlineData("Task<Result<int>> r = ok.ThenAsync(async x => { await Task.Yield(); return x + 1; });")]
    [InlineData("Task<Result<int>> r = ok.ThenAsync(async x => await DoubleValueAsync(x));")]
    [InlineData("Task<Result<int>> r = done.ThenAsync(async () => { await Task.Yield(); return 1; });")]
    [InlineData("Task<Result<int>> r = ok.IfSuccessAsync(async x => { await Task.Yield(); });")]
    [InlineData("Task<Result<int>> r = ok.IfErrorAsync(async e => { await Task.Yield(); });")]
    [InlineData("Task<Result<int>> r = ok.ElseAsync(async e => { await Task.Yield(); return 0; });")]
    [InlineData("ValueTask<Result<int>> r = ok.ThenAsync(async ValueTask<int> (int x) => { await Task.Yield(); return x + 1; });")]
    [InlineData("ValueTask<Result<int>> r = ok.IfSuccessAsync(async ValueTask (int x) => { await Task.Yield(); });")]
    [InlineData("ValueTask<Result<int>> r = ok.ElseAsync(async ValueTask<int> (Error e) => { await Task.Yield(); return 0; });")]
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(x => x + 1);")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(x => x + 1);")]
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(x => DoubleAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(x => DoubleValueAsync(x));")]
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(async x => { await Task.Yield(); return x + 1; });")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(async x => { await Task.Yield(); return x + 1; });")]
    [InlineData("Task<Result<int>> r = GetTask().IfErrorAsync(async e => { await Task.Yield(); });")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().IfErrorAsync(async e => { await Task.Yield(); });")]
    [InlineData("Task<int> r = GetTask().MatchAsync(x => x, e => 0);")]
    [InlineData("ValueTask<int> r = GetValueTask().MatchAsync(x => x, e => 0);")]
    [InlineData("Task<int> r = GetTask().MatchAsync(x => DoubleAsync(x), e => RecoverAsync(e));")]
    [InlineData("ValueTask<int> r = GetValueTask().MatchAsync(x => DoubleValueAsync(x), e => RecoverValueAsync(e));")]
    [InlineData("Task r = GetTask().SwitchAsync(x => { }, e => { });")]
    [InlineData("ValueTask r = GetValueTask().SwitchAsync(x => { }, e => { });")]
    [InlineData("Task<Result> r = GetTask().ToResultAsync();")]
    [InlineData("ValueTask<Result> r = GetValueTask().ToResultAsync();")]
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(x => DoubleValueAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(x => DoubleAsync(x));")]
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(DoubleValueAsync);")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(DoubleAsync);")]
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(x => NextValueAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(x => NextAsync(x));")]
    [InlineData("Task<Result> r = GetTask().ThenAsync(x => CheckValueAsync(x));")]
    [InlineData("ValueTask<Result> r = GetValueTask().ThenAsync(x => CheckAsync(x));")]
    [InlineData("Task<Result> r = GetTask().ThenAsync(x => RejectValueAsync(x));")]
    [InlineData("ValueTask<Result> r = GetValueTask().ThenAsync(x => RejectAsync(x));")]
    [InlineData("Task<Result<int>> r = GetDoneTask().ThenAsync(() => GetIntValueTask());")]
    [InlineData("ValueTask<Result<int>> r = GetDoneValueTask().ThenAsync(() => GetIntAsync());")]
    [InlineData("Task<Result<int>> r = GetDoneTask().ThenAsync(() => NextValueAsync(1));")]
    [InlineData("ValueTask<Result<int>> r = GetDoneValueTask().ThenAsync(() => NextAsync(1));")]
    [InlineData("Task<Result> r = GetDoneTask().ThenAsync(() => CheckValueAsync(1));")]
    [InlineData("ValueTask<Result> r = GetDoneValueTask().ThenAsync(() => CheckAsync(1));")]
    [InlineData("Task<Result> r = GetDoneTask().ThenAsync(() => RejectValueAsync(1));")]
    [InlineData("ValueTask<Result> r = GetDoneValueTask().ThenAsync(() => RejectAsync(1));")]
    [InlineData("Task<Result<int>> r = GetTask().ElseAsync(e => FallbackValueAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ElseAsync(e => FallbackAsync(e));")]
    [InlineData("Task<Result<int>> r = GetTask().ElseAsync(e => RecoverValueAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ElseAsync(e => RecoverAsync(e));")]
    [InlineData("Task<Result<int>> r = GetTask().ElseAsync(e => TranslateValueAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ElseAsync(e => TranslateAsync(e));")]
    [InlineData("Task<Result> r = GetDoneTask().ElseAsync(e => TranslateValueAsync(e));")]
    [InlineData("ValueTask<Result> r = GetDoneValueTask().ElseAsync(e => TranslateAsync(e));")]
    [InlineData("Task<Result<int>> r = GetTask().IfSuccessAsync(x => StoreValueAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().IfSuccessAsync(x => StoreAsync(x));")]
    [InlineData("Task<Result> r = GetDoneTask().IfSuccessAsync(() => StoreValueAsync(1));")]
    [InlineData("ValueTask<Result> r = GetDoneValueTask().IfSuccessAsync(() => StoreAsync(1));")]
    [InlineData("Task<Result<int>> r = GetTask().IfErrorAsync(e => LogValueAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().IfErrorAsync(e => LogAsync(e));")]
    [InlineData("Task<int> r = GetTask().MatchAsync(x => DoubleValueAsync(x), e => RecoverValueAsync(e));")]
    [InlineData("ValueTask<int> r = GetValueTask().MatchAsync(x => DoubleAsync(x), e => RecoverAsync(e));")]
    [InlineData("Task r = GetTask().MatchAsync(x => StoreValueAsync(x), e => LogValueAsync(e));")]
    [InlineData("ValueTask r = GetValueTask().MatchAsync(x => StoreAsync(x), e => LogAsync(e));")]
    [InlineData("Task<int> r = GetDoneTask().MatchAsync(() => GetIntValueTask(), e => RecoverValueAsync(e));")]
    [InlineData("ValueTask<int> r = GetDoneValueTask().MatchAsync(() => GetIntAsync(), e => RecoverAsync(e));")]
    [InlineData("Task r = GetDoneTask().MatchAsync(() => StoreValueAsync(1), e => LogValueAsync(e));")]
    [InlineData("ValueTask r = GetDoneValueTask().MatchAsync(() => StoreAsync(1), e => LogAsync(e));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(async x => await DoubleAsync(x));")]
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(async x => await DoubleValueAsync(x));")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ElseAsync(async e => await RecoverAsync(e));")]
    [InlineData("ValueTask<int> r = GetValueTask().MatchAsync(async x => await DoubleAsync(x), async e => await RecoverAsync(e));")]
    [InlineData("Task<Result<int>> r = GetTask().ThenAsync(x => DoubleValueAsync(x)).ThenAsync(DoubleAsync).IfErrorAsync(LogValueAsync);")]
    [InlineData("ValueTask<Result<int>> r = GetValueTask().ThenAsync(DoubleAsync).ThenAsync(DoubleValueAsync).IfErrorAsync(LogAsync);")]
    [InlineData("ValueTask<Result<int>> r = ok.ThenAsync(x => DoubleValueAsync(x)).ThenAsync(x => x + 1).IfErrorAsync(e => LogValueAsync(e));")]
    [InlineData("Task<Result<int>> r = ok.ThenAsync(async x => { await Task.Yield(); return x; }).ThenAsync(DoubleAsync).IfErrorAsync(LogAsync);")]
    [InlineData("Task<Result<int>> r = from x in ok from y in NextAsync(x) select x + y;")]
    [InlineData("ValueTask<Result<int>> r = from x in ok from y in NextValueAsync(x) select x + y;")]
    [InlineData("Task<Result<int>> r = from x in GetTask() from y in ok select x + y;")]
    [InlineData("ValueTask<Result<int>> r = from x in GetValueTask() from y in ok select x + y;")]
    [InlineData("Task<Result<int>> r = from x in GetTask() from y in NextAsync(x) select x + y;")]
    [InlineData("ValueTask<Result<int>> r = from x in GetValueTask() from y in NextValueAsync(x) select x + y;")]
    [InlineData("Task<Result<int>> r = from x in GetTask() select x + 1;")]
    [InlineData("ValueTask<Result<int>> r = from x in GetValueTask() select x + 1;")]
    [InlineData("Result<int> r = ok.Then(x => x + 1).IfError(e => { });")]
    [InlineData("Result<int> r = from x in ok select x + 1;")]
    [InlineData("var r = GetValueTask().ThenAsync(async x => { await Task.Yield(); return x + 1; });")]
    [InlineData("var r = GetTask().ThenAsync(async x => { await Task.Yield(); return x + 1; });")]
    [InlineData("var r = GetValueTask().IfSuccessAsync(async x => { await Task.Yield(); });")]
    [InlineData("var r = GetValueTask().ElseAsync(async e => { await Task.Yield(); return 0; });")]
    public void Analyze_CallMixingTaskKindsThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
