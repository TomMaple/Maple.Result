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
///     purpose: calls on a Task of a result.
/// </summary>
public class NoDiagnosticTaskResultUnitTests
{
    private const string Members = """
        public record User(int Id, string Name);
        static readonly User Guest = new(0, "guest");
        static readonly HashSet<int> set = [];
        static readonly HashSet<string> titles = [];
        static Task<Result<User>> GetUser() => Task.FromResult(Result<User>.FromValue(Guest));
        static Task<Result> GetOk() => Task.FromResult(Result.Success());
        static string GetName(User u) => u.Name;
        static Task<string> GetNameAsync(User u) => Task.FromResult(u.Name);
        static ValueTask<string> GetNameOtherAsync(User u) => ValueTask.FromResult(u.Name);
        static Task<int> GetLengthAsync(string s) => Task.FromResult(s.Length);
        static Task<Result<string>> GetResultAsync(User u) => Task.FromResult(Result<string>.FromValue(u.Name));
        static Task<Result> SaveResultAsync(User u) => Task.FromResult(Result.Success());
        static Task SaveAsync(User u) => Task.CompletedTask;
        static ValueTask SaveOtherAsync(User u) => ValueTask.CompletedTask;
        static Task LogAsync(Error e) => Task.CompletedTask;
        static ValueTask LogOtherAsync(Error e) => ValueTask.CompletedTask;
        static void Log(User u) { }
        static void LogError(Error e) { }
        """;

    [Theory]
    [InlineData("Task<Result<string>> r = GetUser().ThenAsync(u => u.Name);")]
    [InlineData("Task<Result<string>> r = GetUser().ThenAsync(u => Result<string>.FromValue(u.Name));")]
    [InlineData("Task<Result> r = GetUser().ThenAsync(u => Error.NotFound(ErrorUri.None, \"x\"));")]
    [InlineData("Task<Result> r = GetUser().ThenAsync(u => Result.Success());")]
    [InlineData("Task<Result<string>> r = GetUser().ThenAsync(GetName);")]
    [InlineData("Task<Result<bool>> r = GetUser().ThenAsync(u => set.Add(u.Id));")]
    [InlineData("Task<Result<string>> r = GetUser().ThenAsync<User, string>(u => u.Name);")]
    [InlineData("Task<Result<int>> r = GetUser().ThenAsync(u => u.Id, continueOnCapturedContext: true);")]
    [InlineData("Task<Result<string>> r = GetUser().ThenAsync(u => GetNameAsync(u));")]
    [InlineData("Task<Result<string>> r = GetUser().ThenAsync(async u => await GetNameAsync(u));")]
    [InlineData("Task<Result<string>> r = GetUser().ThenAsync(u => GetResultAsync(u));")]
    [InlineData("Task<Result<string>> r = GetUser().ThenAsync(GetNameAsync);")]
    [InlineData("Task<Result<int>> r = GetUser().ThenAsync(async u => { await SaveAsync(u); return 1; });")]
    [InlineData("Task<Result> r = GetUser().ThenAsync(u => SaveResultAsync(u));")]
    [InlineData("Task<Result<string>> r = GetUser().ThenAsync(async u => await GetNameOtherAsync(u));")]
    [InlineData("Task<Result<int>> r = GetOk().ThenAsync(() => 5);")]
    [InlineData("Task<Result> r = GetOk().ThenAsync(() => Result.Success());")]
    [InlineData("Task<Result> r = GetOk().ThenAsync(() => Error.NotFound(ErrorUri.None, \"x\"));")]
    [InlineData("Task<Result<int>> r = GetOk().ThenAsync(() => Result<int>.FromValue(1));")]
    [InlineData("Task<Result<int>> r = GetOk().ThenAsync(() => Task.FromResult(5));")]
    [InlineData("Task<Result<int>> r = GetOk().ThenAsync(async () => { await SaveAsync(null!); return 5; });")]
    [InlineData("Task<Result<User>> r = GetUser().ElseAsync(e => Result<User>.FromValue(Guest));")]
    [InlineData("Task<Result<User>> r = GetUser().ElseAsync(e => (Result<User>)Guest);")]
    [InlineData("Task<Result> r = GetOk().ElseAsync(e => Result.Success());")]
    [InlineData("Task<Result<User>> r = GetUser().ElseAsync(e => Task.FromResult(Result<User>.FromValue(Guest)));")]
    [InlineData("Task<Result<User>> r = GetUser().ElseAsync(async e => { await LogAsync(e); return Result<User>.FromValue(Guest); });")]
    [InlineData("Task<Result<User>> r = GetUser().IfSuccessAsync(u => Log(u));")]
    [InlineData("Task<Result<User>> r = GetUser().IfSuccessAsync(u => { set.Add(u.Id); });")]
    [InlineData("Task<Result<User>> r = GetUser().IfSuccessAsync(Log);")]
    [InlineData("Task<Result<User>> r = GetUser().IfSuccessAsync(u => SaveAsync(u));")]
    [InlineData("Task<Result<User>> r = GetUser().IfSuccessAsync(async u => await SaveAsync(u));")]
    [InlineData("Task<Result<User>> r = GetUser().IfSuccessAsync(SaveAsync);")]
    [InlineData("Task<Result> r = GetOk().IfSuccessAsync(() => Log(null!));")]
    [InlineData("Task<Result> r = GetOk().IfSuccessAsync(() => Task.CompletedTask);")]
    [InlineData("Task<Result<User>> r = GetUser().IfErrorAsync(e => LogError(e));")]
    [InlineData("Task<Result<User>> r = GetUser().IfErrorAsync(LogError);")]
    [InlineData("Task<Result<User>> r = GetUser().IfErrorAsync(e => LogAsync(e));")]
    [InlineData("Task<Result<User>> r = GetUser().IfErrorAsync(async e => await LogAsync(e));")]
    [InlineData("Task<Result> r = GetOk().IfErrorAsync(e => { titles.Add(e.Title); });")]
    [InlineData("Task<Result<int>> r = GetUser().ThenAsync(u => u.Name).ThenAsync(n => GetLengthAsync(n)).IfErrorAsync(e => LogError(e));")]
    [InlineData("Task<string> r = GetUser().MatchAsync(u => u.Name, e => \"unknown\");")]
    [InlineData("Task r = GetUser().SwitchAsync(u => Log(u), e => LogError(e));")]
    [InlineData("Task<Result<string>> r = GetUser().MatchAsync(u => Result<string>.FromValue(u.Name), e => Result<string>.FromValue(\"x\"));")]
    [InlineData("Task r = GetUser().SwitchAsync(Log, LogError);")]
    [InlineData("Task r = GetOk().SwitchAsync(() => Log(null!), e => LogError(e));")]
    [InlineData("Task<int> r = GetOk().MatchAsync(() => 5, e => 0);")]
    [InlineData("Task<Result> r = GetOk().MatchAsync(() => Result.Success(), e => Result.Success());")]
    [InlineData("Task r = GetUser().MatchAsync(u => SaveAsync(u), e => LogAsync(e));")]
    [InlineData("Task<string> r = GetUser().MatchAsync(u => GetNameAsync(u), e => Task.FromResult(\"x\"));")]
    [InlineData("Task r = GetOk().MatchAsync(() => SaveAsync(null!), e => LogAsync(e));")]
    [InlineData("Task<Result<string>> r = from u in GetUser() select u.Name;")]
    [InlineData("Task<Result<string>> r = GetUser().ThenAsync(u => GetNameOtherAsync(u));")]
    [InlineData("Task<Result<int>> r = GetOk().ThenAsync(() => ValueTask.FromResult(5));")]
    [InlineData("Task<Result<string>> r = GetUser().ThenAsync(GetNameOtherAsync);")]
    [InlineData("Task<Result<User>> r = GetUser().IfSuccessAsync(u => SaveOtherAsync(u));")]
    [InlineData("Task<Result> r = GetOk().IfSuccessAsync(() => ValueTask.CompletedTask);")]
    [InlineData("Task<Result<User>> r = GetUser().IfErrorAsync(e => LogOtherAsync(e));")]
    [InlineData("Task r = GetUser().MatchAsync(u => SaveOtherAsync(u), e => LogOtherAsync(e));")]
    [InlineData("Task r = GetOk().MatchAsync(() => SaveOtherAsync(null!), e => LogOtherAsync(e));")]
    [InlineData("Task<string> r = GetUser().MatchAsync(u => GetNameOtherAsync(u), e => ValueTask.FromResult(\"x\"));")]
    [InlineData("Task<Result<User>> r = GetUser().ElseAsync(e => ValueTask.FromResult(Result<User>.FromValue(Guest)));")]
    [InlineData("_ = GetUser().IfSuccessAsync(u => set.Add(u.Id));")]
    [InlineData("_ = GetUser().IfSuccessAsync(u => GetNameAsync(u));")]
    [InlineData("_ = GetUser().IfErrorAsync(e => titles.Add(e.Title));")]
    [InlineData("_ = GetUser().IfErrorAsync(e => GetNameAsync(null!));")]
    public void Analyze_CallOnTaskResultThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("Task<Result<Task<string>>> r = GetUser().ThenAsync<User, Task<string>>(u => GetNameAsync(u));")]
    public void Analyze_CallOnTaskResultKeepingTheValueOnPurpose_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
