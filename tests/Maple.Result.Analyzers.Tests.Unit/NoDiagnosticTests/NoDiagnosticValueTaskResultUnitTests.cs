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
///     purpose: calls on a ValueTask of a result.
/// </summary>
public class NoDiagnosticValueTaskResultUnitTests
{
    private const string Members = """
        public record User(int Id, string Name);
        static readonly User Guest = new(0, "guest");
        static readonly HashSet<int> set = [];
        static readonly HashSet<string> titles = [];
        static ValueTask<Result<User>> GetUser() => ValueTask.FromResult(Result<User>.FromValue(Guest));
        static ValueTask<Result> GetOk() => ValueTask.FromResult(Result.Success());
        static string GetName(User u) => u.Name;
        static ValueTask<string> GetNameAsync(User u) => ValueTask.FromResult(u.Name);
        static Task<string> GetNameOtherAsync(User u) => Task.FromResult(u.Name);
        static ValueTask<int> GetLengthAsync(string s) => ValueTask.FromResult(s.Length);
        static ValueTask<Result<string>> GetResultAsync(User u) => ValueTask.FromResult(Result<string>.FromValue(u.Name));
        static ValueTask<Result> SaveResultAsync(User u) => ValueTask.FromResult(Result.Success());
        static ValueTask SaveAsync(User u) => ValueTask.CompletedTask;
        static Task SaveOtherAsync(User u) => Task.CompletedTask;
        static ValueTask LogAsync(Error e) => ValueTask.CompletedTask;
        static Task LogOtherAsync(Error e) => Task.CompletedTask;
        static void Log(User u) { }
        static void LogError(Error e) { }
        """;

    [Theory]
    [InlineData("ValueTask<Result<string>> r = GetUser().ThenAsync(u => u.Name);")]
    [InlineData("ValueTask<Result<string>> r = GetUser().ThenAsync(u => Result<string>.FromValue(u.Name));")]
    [InlineData("ValueTask<Result> r = GetUser().ThenAsync(u => Error.NotFound(ErrorUri.None, \"x\"));")]
    [InlineData("ValueTask<Result> r = GetUser().ThenAsync(u => Result.Success());")]
    [InlineData("ValueTask<Result<string>> r = GetUser().ThenAsync(GetName);")]
    [InlineData("ValueTask<Result<bool>> r = GetUser().ThenAsync(u => set.Add(u.Id));")]
    [InlineData("ValueTask<Result<string>> r = GetUser().ThenAsync<User, string>(u => u.Name);")]
    [InlineData("ValueTask<Result<int>> r = GetUser().ThenAsync(u => u.Id, continueOnCapturedContext: true);")]
    [InlineData("ValueTask<Result<string>> r = GetUser().ThenAsync(u => GetNameAsync(u));")]
    [InlineData("ValueTask<Result<string>> r = GetUser().ThenAsync(async u => await GetNameAsync(u));")]
    [InlineData("ValueTask<Result<string>> r = GetUser().ThenAsync(u => GetResultAsync(u));")]
    [InlineData("ValueTask<Result<string>> r = GetUser().ThenAsync(GetNameAsync);")]
    [InlineData("ValueTask<Result<int>> r = GetUser().ThenAsync(async u => { await SaveAsync(u); return 1; });")]
    [InlineData("ValueTask<Result> r = GetUser().ThenAsync(u => SaveResultAsync(u));")]
    [InlineData("ValueTask<Result<string>> r = GetUser().ThenAsync(async u => await GetNameOtherAsync(u));")]
    [InlineData("ValueTask<Result<int>> r = GetOk().ThenAsync(() => 5);")]
    [InlineData("ValueTask<Result> r = GetOk().ThenAsync(() => Result.Success());")]
    [InlineData("ValueTask<Result> r = GetOk().ThenAsync(() => Error.NotFound(ErrorUri.None, \"x\"));")]
    [InlineData("ValueTask<Result<int>> r = GetOk().ThenAsync(() => Result<int>.FromValue(1));")]
    [InlineData("ValueTask<Result<int>> r = GetOk().ThenAsync(() => ValueTask.FromResult(5));")]
    [InlineData("ValueTask<Result<int>> r = GetOk().ThenAsync(async () => { await SaveAsync(null!); return 5; });")]
    [InlineData("ValueTask<Result<User>> r = GetUser().ElseAsync(e => Result<User>.FromValue(Guest));")]
    [InlineData("ValueTask<Result<User>> r = GetUser().ElseAsync(e => (Result<User>)Guest);")]
    [InlineData("ValueTask<Result> r = GetOk().ElseAsync(e => Result.Success());")]
    [InlineData("ValueTask<Result<User>> r = GetUser().ElseAsync(e => ValueTask.FromResult(Result<User>.FromValue(Guest)));")]
    [InlineData("ValueTask<Result<User>> r = GetUser().ElseAsync(async e => { await LogAsync(e); return Result<User>.FromValue(Guest); });")]
    [InlineData("ValueTask<Result<User>> r = GetUser().IfSuccessAsync(u => Log(u));")]
    [InlineData("ValueTask<Result<User>> r = GetUser().IfSuccessAsync(u => { set.Add(u.Id); });")]
    [InlineData("ValueTask<Result<User>> r = GetUser().IfSuccessAsync(Log);")]
    [InlineData("ValueTask<Result<User>> r = GetUser().IfSuccessAsync(u => SaveAsync(u));")]
    [InlineData("ValueTask<Result<User>> r = GetUser().IfSuccessAsync(async u => await SaveAsync(u));")]
    [InlineData("ValueTask<Result<User>> r = GetUser().IfSuccessAsync(SaveAsync);")]
    [InlineData("ValueTask<Result> r = GetOk().IfSuccessAsync(() => Log(null!));")]
    [InlineData("ValueTask<Result> r = GetOk().IfSuccessAsync(() => ValueTask.CompletedTask);")]
    [InlineData("ValueTask<Result<User>> r = GetUser().IfErrorAsync(e => LogError(e));")]
    [InlineData("ValueTask<Result<User>> r = GetUser().IfErrorAsync(LogError);")]
    [InlineData("ValueTask<Result<User>> r = GetUser().IfErrorAsync(e => LogAsync(e));")]
    [InlineData("ValueTask<Result<User>> r = GetUser().IfErrorAsync(async e => await LogAsync(e));")]
    [InlineData("ValueTask<Result> r = GetOk().IfErrorAsync(e => { titles.Add(e.Title); });")]
    [InlineData("ValueTask<Result<int>> r = GetUser().ThenAsync(u => u.Name).ThenAsync(n => GetLengthAsync(n)).IfErrorAsync(e => LogError(e));")]
    [InlineData("ValueTask<string> r = GetUser().MatchAsync(u => u.Name, e => \"unknown\");")]
    [InlineData("ValueTask r = GetUser().SwitchAsync(u => Log(u), e => LogError(e));")]
    [InlineData("ValueTask<Result<string>> r = GetUser().MatchAsync(u => Result<string>.FromValue(u.Name), e => Result<string>.FromValue(\"x\"));")]
    [InlineData("ValueTask r = GetUser().SwitchAsync(Log, LogError);")]
    [InlineData("ValueTask r = GetOk().SwitchAsync(() => Log(null!), e => LogError(e));")]
    [InlineData("ValueTask<int> r = GetOk().MatchAsync(() => 5, e => 0);")]
    [InlineData("ValueTask<Result> r = GetOk().MatchAsync(() => Result.Success(), e => Result.Success());")]
    [InlineData("ValueTask r = GetUser().MatchAsync(u => SaveAsync(u), e => LogAsync(e));")]
    [InlineData("ValueTask<string> r = GetUser().MatchAsync(u => GetNameAsync(u), e => ValueTask.FromResult(\"x\"));")]
    [InlineData("ValueTask r = GetOk().MatchAsync(() => SaveAsync(null!), e => LogAsync(e));")]
    [InlineData("ValueTask<Result<string>> r = from u in GetUser() select u.Name;")]
    [InlineData("ValueTask<Result<string>> r = GetUser().ThenAsync(u => GetNameOtherAsync(u));")]
    [InlineData("ValueTask<Result<int>> r = GetOk().ThenAsync(() => Task.FromResult(5));")]
    [InlineData("ValueTask<Result<string>> r = GetUser().ThenAsync(GetNameOtherAsync);")]
    [InlineData("ValueTask<Result<User>> r = GetUser().IfSuccessAsync(u => SaveOtherAsync(u));")]
    [InlineData("ValueTask<Result> r = GetOk().IfSuccessAsync(() => Task.CompletedTask);")]
    [InlineData("ValueTask<Result<User>> r = GetUser().IfErrorAsync(e => LogOtherAsync(e));")]
    [InlineData("ValueTask r = GetUser().MatchAsync(u => SaveOtherAsync(u), e => LogOtherAsync(e));")]
    [InlineData("ValueTask r = GetOk().MatchAsync(() => SaveOtherAsync(null!), e => LogOtherAsync(e));")]
    [InlineData("ValueTask<string> r = GetUser().MatchAsync(u => GetNameOtherAsync(u), e => Task.FromResult(\"x\"));")]
    [InlineData("ValueTask<Result<User>> r = GetUser().ElseAsync(e => Task.FromResult(Result<User>.FromValue(Guest)));")]
    [InlineData("ValueTask r = GetUser().MatchAsync(u => GetNameOtherAsync(u), e => LogOtherAsync(e));")]
    [InlineData("_ = GetUser().IfSuccessAsync(u => set.Add(u.Id));")]
    [InlineData("_ = GetUser().IfSuccessAsync(u => GetNameOtherAsync(u));")]
    [InlineData("_ = GetUser().IfErrorAsync(e => titles.Add(e.Title));")]
    [InlineData("_ = GetUser().IfSuccessAsync(u => GetNameAsync(u));")]
    [InlineData("_ = GetUser().IfErrorAsync(e => GetNameAsync(null!));")]
    public void Analyze_CallOnValueTaskResultThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("ValueTask<Result<ValueTask<string>>> r = GetUser().ThenAsync<User, ValueTask<string>>(u => GetNameAsync(u));")]
    public void Analyze_CallOnValueTaskResultKeepingTheValueOnPurpose_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
