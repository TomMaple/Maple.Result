// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Linq;

namespace Maple.Results.Tests.Unit.Binding;

/// <summary>
///     Synchronous delegates on <c>ValueTask</c> results (#214), with only the <c>ValueTask</c>-based namespace imported: the calls bind
///     as intended (each is assigned to a variable of the expected type); a delegate of the wrong kind, two actions of <c>MatchAsync</c>
///     (<c>SwitchAsync</c> takes them, #255), or a delegate that returns the other kind of task is a compile error (CS0619); and a
///     synchronous action passed to <c>ThenAsync</c> or <c>ElseAsync</c> is a compile error that the compiler reports by itself (#256).
/// </summary>
public class SyncDelegatesOnValueTaskResultsBindingUnitTests
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
    [InlineData("ValueTask<Result<ValueTask<string>>> r = GetUser().ThenAsync<User, ValueTask<string>>(u => GetNameAsync(u));")]
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
    public void Call_SupportedShape_CompilesToExpectedType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.ValueTask, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("_ = GetUser().ThenAsync(u => SaveAsync(u));")]
    [InlineData("_ = GetUser().ThenAsync(async u => await SaveAsync(u));")]
    [InlineData("_ = GetOk().ThenAsync(() => SaveAsync(null!));")]
    [InlineData("_ = GetUser().ElseAsync(e => LogAsync(e));")]
    [InlineData("_ = GetUser().ElseAsync(async e => { await LogAsync(e); });")]
    [InlineData("_ = GetUser().ThenAsync(u => GetNameOtherAsync(u));")]
    [InlineData("_ = GetUser().ThenAsync(u => SaveOtherAsync(u));")]
    [InlineData("_ = GetOk().ThenAsync(() => SaveOtherAsync(null!));")]
    [InlineData("_ = GetOk().ThenAsync(() => Task.FromResult(5));")]
    [InlineData("_ = GetUser().ThenAsync(GetNameOtherAsync);")]
    [InlineData("_ = GetUser().ElseAsync(e => LogOtherAsync(e));")]
    [InlineData("_ = GetUser().ElseAsync(e => Task.FromResult(Result<User>.FromValue(Guest)));")]
    [InlineData("_ = GetUser().IfSuccessAsync(u => set.Add(u.Id));")]
    [InlineData("_ = GetOk().IfSuccessAsync(() => 5);")]
    [InlineData("_ = GetUser().IfSuccessAsync(GetName);")]
    [InlineData("_ = GetUser().IfSuccessAsync(u => GetNameAsync(u));")]
    [InlineData("_ = GetUser().IfSuccessAsync(u => SaveOtherAsync(u));")]
    [InlineData("_ = GetUser().IfSuccessAsync(u => GetNameOtherAsync(u));")]
    [InlineData("_ = GetOk().IfSuccessAsync(() => Task.CompletedTask);")]
    [InlineData("_ = GetUser().IfErrorAsync(e => titles.Add(e.Title));")]
    [InlineData("_ = GetUser().IfErrorAsync(e => LogOtherAsync(e));")]
    [InlineData("_ = GetUser().IfErrorAsync(e => GetNameAsync(null!));")]
    [InlineData("_ = GetUser().MatchAsync(u => GetNameAsync(u), e => LogError(e));")]
    [InlineData("_ = GetUser().MatchAsync(u => SaveAsync(u), e => LogError(e));")]
    [InlineData("_ = GetUser().MatchAsync(async u => await SaveAsync(u), e => LogError(e));")]
    [InlineData("_ = GetOk().MatchAsync(() => SaveAsync(null!), e => LogError(e));")]
    [InlineData("_ = GetUser().MatchAsync(u => Log(u), e => LogAsync(e));")]
    [InlineData("_ = GetUser().MatchAsync(u => SaveOtherAsync(u), e => LogOtherAsync(e));")]
    [InlineData("_ = GetUser().MatchAsync(u => GetNameOtherAsync(u), e => LogOtherAsync(e));")]
    [InlineData("_ = GetOk().MatchAsync(() => SaveOtherAsync(null!), e => LogOtherAsync(e));")]
    [InlineData("_ = GetUser().MatchAsync(u => SaveOtherAsync(u), e => LogError(e));")]
    [InlineData("_ = GetUser().MatchAsync(u => Log(u), e => LogOtherAsync(e));")]
    [InlineData("_ = GetUser().MatchAsync(u => GetNameOtherAsync(u), e => Task.FromResult(\"x\"));")]
    public void Call_UnsupportedShape_ReportsOnlyCS0619(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.ValueTask, Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    // A synchronous action passed to ThenAsync or ElseAsync: the compiler reports it by itself, so there is no overload trap for it (#256).
    [Theory]
    [InlineData("GetUser().ThenAsync(u => Log(u));", "CS0029,CS1662")]
    [InlineData("GetUser().ThenAsync(Log);", "CS1929")]
    [InlineData("GetUser().ThenAsync(u => { });", "CS1643")]
    [InlineData("GetOk().ThenAsync(() => Log(null!));", "CS0029,CS1662")]
    [InlineData("GetUser().ElseAsync(e => LogError(e));", "CS0029,CS1662")]
    [InlineData("GetUser().ElseAsync(LogError);", "CS0407")]
    [InlineData("GetOk().ElseAsync(e => { });", "CS1643")]
    public void Call_SynchronousActionInATransformation_ReportsTheCompilerErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.ValueTask, Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }
}
