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
///     Synchronous delegates on <c>Task</c> results (#214), with only the <c>Task</c>-based namespace imported: the calls bind
///     as intended (each is assigned to a variable of the expected type); a delegate that returns the other kind of task is awaited
///     (#226); and a delegate that no overload takes (e.g., a function with an action in <c>MatchAsync</c>, #255, or a synchronous action
///     passed to <c>ThenAsync</c> or <c>ElseAsync</c>, #256) is a compile error that the compiler reports by itself. The shapes that
///     compile into something wrong (e.g., a task without a value in <c>ThenAsync</c>) are reported by the analyzer (rules MR0001 to
///     MR0006 of <c>Maple.Result.Analyzers</c>) and tested with it (#223).
/// </summary>
public class SyncDelegatesOnTaskResultsBindingUnitTests
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
    [InlineData("Task<Result<Task<string>>> r = GetUser().ThenAsync<User, Task<string>>(u => GetNameAsync(u));")]
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
    // A function that returns the other kind of task is awaited, and the receiver decides the kind (#226).
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
    // A side effect whose function returns a value, or a task with a value: the value is ignored, and the task is awaited.
    [InlineData("Task<Result<User>> r = GetUser().IfSuccessAsync(u => set.Add(u.Id));")]
    [InlineData("Task<Result<User>> r = GetUser().IfErrorAsync(e => titles.Add(e.Title));")]
    [InlineData("Task<Result<User>> r = GetUser().IfSuccessAsync(u => GetNameAsync(u));")]
    [InlineData("Task<Result<User>> r = GetUser().IfErrorAsync(e => GetNameAsync(null!));")]
    public void Call_SupportedShape_CompilesToExpectedType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    // A task without a value in ElseAsync, a value that is not a statement or a method group that returns a value in IfSuccessAsync,
    // and a function with an action in MatchAsync (SwitchAsync takes two actions, #255): the compiler reports them by itself.
    [Theory]
    [InlineData("_ = GetUser().ElseAsync(e => LogAsync(e));", "CS0029,CS0266,CS1662")]
    [InlineData("_ = GetUser().ElseAsync(async e => { await LogAsync(e); });", "CS1643")]
    [InlineData("_ = GetUser().ElseAsync(e => LogOtherAsync(e));", "CS0029,CS1662")]
    [InlineData("_ = GetOk().IfSuccessAsync(() => 5);", "CS0029,CS0201,CS1662")]
    [InlineData("_ = GetUser().IfSuccessAsync(GetName);", "CS1929")]
    [InlineData("_ = GetUser().MatchAsync(u => GetNameAsync(u), e => LogError(e));", "CS0029,CS1662")]
    [InlineData("_ = GetUser().MatchAsync(u => SaveAsync(u), e => LogError(e));", "CS0029,CS1662")]
    [InlineData("_ = GetUser().MatchAsync(async u => await SaveAsync(u), e => LogError(e));", "CS0029,CS1662")]
    [InlineData("_ = GetOk().MatchAsync(() => SaveAsync(null!), e => LogError(e));", "CS0029,CS1662")]
    [InlineData("_ = GetUser().MatchAsync(u => Log(u), e => LogAsync(e));", "CS0029,CS1662")]
    [InlineData("_ = GetUser().MatchAsync(u => GetNameOtherAsync(u), e => LogOtherAsync(e));", "CS0029,CS1662")]
    [InlineData("_ = GetUser().MatchAsync(u => SaveOtherAsync(u), e => LogError(e));", "CS0029,CS1662")]
    [InlineData("_ = GetUser().MatchAsync(u => Log(u), e => LogOtherAsync(e));", "CS0029,CS1662")]
    public void Call_UnsupportedShape_ReportsTheCompilerErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }

    // A synchronous action passed to ThenAsync or ElseAsync: the compiler reports it by itself (#256).
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
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }
}
