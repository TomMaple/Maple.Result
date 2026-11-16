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
using System.Globalization;
using System.Linq;

namespace Maple.Results.Tests.Unit.Binding;

/// <summary>
///     <c>Match</c> and <c>MatchAsync</c> with a success function and an error action transformed the result, as
///     <c>Then(…).IfError(…)</c>, and tapped it when the same lambda had no value (#227). Such a call is a compile error (CS0619)
///     that names the replacement; <c>Match</c> keeps two functions (a value) and two actions (a side effect).
/// </summary>
public class MatchSuccessFunctionErrorActionBindingUnitTests
{
    private const string Members = """
        static readonly HashSet<int> Set = new();
        static Task<Result<int>> GetOkAsync() => Task.FromResult(Result.FromValue(1));
        static ValueTask<Result<int>> GetOkValueTask() => new(Result.FromValue(1));
        static ValueTask SaveValueTask() => default;
        static Error ToError(int value) => err;
        static void LogError(Error error) { }
        """;

    [Theory]
    [InlineData("_ = GetResult().Match(() => GetResult(), e => { });")]
    [InlineData("_ = GetResult().Match(() => 5, e => { });")]
    [InlineData("_ = GetResult().Match(() => GetError(), e => { });")]
    [InlineData("_ = GetResult().Match(() => GetResultOfInt(), e => { });")]
    [InlineData("_ = ok.Match(x => x + 1, e => { });")]
    [InlineData("_ = ok.Match(x => GetError(), e => { });")]
    [InlineData("_ = ok.Match(x => GetResultOfInt(), e => { });")]
    [InlineData("_ = ok.Match(x => Set.Add(x), e => { });")]
    [InlineData("_ = ok.Match(ToError, LogError);")]
    [InlineData("_ = ok.Match<int, Error>(x => GetError(), e => { });")]
    [InlineData("_ = ok.Match<int, string>(x => x.ToString(), e => { });")]
    public void Match_SuccessFunctionAndErrorAction_ReportsOnlyCS0619(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("_ = GetResult().MatchAsync(() => GetIntAsync(), e => Task.CompletedTask);")]
    [InlineData("_ = GetResult().MatchAsync(() => Task.FromResult(GetError()), e => Task.CompletedTask);")]
    [InlineData("_ = ok.MatchAsync(x => GetIntAsync(), e => Task.CompletedTask);")]
    [InlineData("_ = ok.MatchAsync(async x => await GetIntAsync(), async e => await SaveAsync());")]
    [InlineData("_ = GetOkAsync().MatchAsync(x => GetIntAsync(), e => Task.CompletedTask);")]
    [InlineData("_ = GetOkAsync().MatchAsync(x => Task.FromResult(GetResultOfInt()), e => Task.CompletedTask);")]
    [InlineData("_ = GetOkAsync().MatchAsync(x => x + 1, e => { });")]
    [InlineData("_ = GetOkAsync().MatchAsync(x => Set.Add(x), e => LogError(e));")]
    public void TaskBasedMatchAsync_SuccessFunctionAndErrorAction_ReportsOnlyCS0619(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("_ = ok.MatchAsync(x => GetIntValueTask(), e => SaveValueTask());")]
    [InlineData("_ = GetOkValueTask().MatchAsync(x => GetIntValueTask(), e => SaveValueTask());")]
    [InlineData("_ = GetOkValueTask().MatchAsync(x => x + 1, e => { });")]
    public void ValueTaskBasedMatchAsync_SuccessFunctionAndErrorAction_ReportsOnlyCS0619(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.ValueTask, Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("_ = ok.Match(x => Set.Add(x), e => { });")]
    public void Match_SuccessFunctionAndErrorAction_MessageNamesThenAndIfError(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        var message = snippet.Errors.Single(d => d.Id == "CS0619").GetMessage(CultureInfo.InvariantCulture);
        message.ShouldContain("Use Then(…).IfError(…) to transform it");
    }

    [Theory]
    [InlineData("_ = GetOkAsync().MatchAsync(x => x + 1, e => { });")]
    [InlineData("_ = ok.MatchAsync(x => GetIntAsync(), e => Task.CompletedTask);")]
    public void MatchAsync_SuccessFunctionAndErrorAction_MessageNamesThenAsyncAndIfErrorAsync(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        var message = snippet.Errors.Single(d => d.Id == "CS0619").GetMessage(CultureInfo.InvariantCulture);
        message.ShouldContain("Use ThenAsync(…).IfErrorAsync(…) to transform it");
    }

    [Theory]
    [InlineData("Result<int> r = ok.Match(x => { Set.Add(x); }, e => { });")]
    [InlineData("Result r = GetResult().Match(() => { }, e => { });")]
    [InlineData("bool r = ok.Match(x => Set.Add(x), e => false);")]
    [InlineData("int r = ok.Match(x => x, e => 0);")]
    [InlineData("Result r = ok.Match(x => GetError(), e => GetResult());")]
    [InlineData("Result<int> r = ok.Then(x => x + 1).IfError(e => { });")]
    [InlineData("Result r = ok.Then(x => GetError()).IfError(LogError);")]
    public void Match_TwoActionsTwoFunctionsOrThenAndIfError_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("Task<Result<int>> r = ok.MatchAsync(x => SaveAsync(), e => Task.CompletedTask);")]
    [InlineData("Task<int> r = ok.MatchAsync(x => GetIntAsync(), e => Task.FromResult(0));")]
    [InlineData("Task<Result<int>> r = GetOkAsync().MatchAsync(x => { Set.Add(x); }, e => { });")]
    [InlineData("Task<int> r = GetOkAsync().MatchAsync(x => x, e => 0);")]
    [InlineData("Task<Result<int>> r = ok.ThenAsync(x => GetIntAsync()).IfErrorAsync(e => Task.CompletedTask);")]
    [InlineData("Task<Result<int>> r = GetOkAsync().ThenAsync(x => x + 1).IfErrorAsync(e => LogError(e));")]
    public void TaskBasedMatchAsync_TwoActionsTwoFunctionsOrThenAndIfError_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }
}
