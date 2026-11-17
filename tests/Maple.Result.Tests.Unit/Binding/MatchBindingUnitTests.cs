// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Tests.Unit.TestingInfrastructure.TestData;
using System;
using System.Globalization;
using System.Linq;

namespace Maple.Results.Tests.Unit.Binding;

/// <summary>
///     <c>Match</c> and <c>MatchAsync</c> are a fold (#255): two functions and one return type, which the call returns (a task too).
///     Two actions are a compile error (CS0619) that names <c>Switch</c>; a mixed call (a value and an <see cref="Error" />, or one
///     synchronous and one asynchronous function) has no common return type, so the compiler rejects it by itself. There is no
///     <c>MatchAsync</c> on a result: <c>Match</c> returns the task of the function that ran. Each supported shape is assigned to a
///     variable of the expected type.
/// </summary>
public class MatchBindingUnitTests
{
    private const string Members = MatchSwitchSnippetMembers.Members;

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
    public void Match_TwoFunctions_CompilesToTheirType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
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
    public void MatchAsync_TwoFunctions_CompilesToATaskOfTheirType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("ok.Match(x => Show(x), e => Log(e));", "CS0619")]
    [InlineData("done.Match(() => Show(1), e => Log(e));", "CS0619")]
    [InlineData("_ = ok.Match(x => Compute(x), e => Log(e));", "CS0619")]
    [InlineData("ok.Match(x => Show(x), e => LogAsync(e));", "CS0619")]
    [InlineData("ok.Match(x => throw new InvalidOperationException(), e => throw new InvalidOperationException());", "CS0619")]
    [InlineData("ok.Match(Show, Log);", "CS0619")]
    [InlineData("ok.Match(async x => await SaveAsync(x), e => Log(e));", "CS0619")]
    [InlineData("_ = ok.Match(x => x + 1, e => e);", "CS0201")]
    [InlineData("_ = ok.Match(x => GetIntAsync(x), e => 0);", "CS0201")]
    [InlineData("okTask.MatchAsync(x => Show(x), e => Log(e));", "CS0619")]
    [InlineData("okTask.MatchAsync(x => Show(x), e => LogAsync(e));", "CS0619")]
    [InlineData("doneTask.MatchAsync(() => Show(1), e => Log(e));", "CS0619")]
    [InlineData("okVt.MatchAsync(x => Show(x), e => Log(e));", "CS0619")]
    [InlineData("doneVt.MatchAsync(() => Show(1), e => Log(e));", "CS0619")]
    [InlineData("_ = okTask.MatchAsync(x => x, e => GetIntAsync(0));", "CS0029,CS0201,CS1662")]
    [InlineData("_ = okTask.MatchAsync(x => GetIntAsync(x), e => 0);", "CS0029,CS0201,CS1662")]
    [InlineData("_ = okTask.MatchAsync(x => GetIntAsync(x), e => GetIntVt(0));", "CS0619")]
    [InlineData("_ = okTask.MatchAsync(x => GetIntVt(x), e => GetIntVt(0));", "CS0619")]
    [InlineData("_ = okTask.MatchAsync(x => SaveVt(x), e => LogVt(e));", "CS0619")]
    [InlineData("_ = okVt.MatchAsync(x => GetIntAsync(x), e => GetIntAsync(0));", "CS0619")]
    [InlineData("_ = okVt.MatchAsync(x => SaveAsync(x), e => LogAsync(e));", "CS0619")]
    [InlineData("_ = ok.MatchAsync(x => GetIntAsync(x), e => GetIntAsync(0));", "CS1929")]
    [InlineData("_ = done.MatchAsync(() => GetIntVt(1), e => GetIntVt(0));", "CS1929")]
    public void Match_UnsupportedShape_ReportsOnlyTheStatedErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("ok.Match(x => Show(x), e => Log(e));")]
    [InlineData("done.Match(() => Show(1), e => Log(e));")]
    [InlineData("_ = ok.Match(x => Compute(x), e => Log(e));")]
    public void Match_TwoActions_MessageNamesSwitchAndThenIfError(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        var message = snippet.Errors.Single(d => d.Id == "CS0619").GetMessage(CultureInfo.InvariantCulture);
        message.ShouldEndWith("is obsolete: 'Match takes two functions that return a value. Use Switch for two side effects, or "
                              + "Then(…).IfError(…) to transform the value and handle the error.'");
    }

    [Theory]
    [InlineData("okTask.MatchAsync(x => Show(x), e => Log(e));")]
    [InlineData("doneTask.MatchAsync(() => Show(1), e => Log(e));")]
    [InlineData("okVt.MatchAsync(x => Show(x), e => Log(e));")]
    [InlineData("doneVt.MatchAsync(() => Show(1), e => Log(e));")]
    public void MatchAsync_TwoActions_MessageNamesSwitchAsyncAndThenAsyncIfErrorAsync(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        var message = snippet.Errors.Single(d => d.Id == "CS0619").GetMessage(CultureInfo.InvariantCulture);
        message.ShouldEndWith("is obsolete: 'MatchAsync takes two functions that return a value. Use SwitchAsync for two side effects, or "
                              + "ThenAsync(…).IfErrorAsync(…) to transform the value and handle the error.'");
    }

    [Theory]
    [InlineData("_ = okTask.MatchAsync(x => GetIntVt(x), e => GetIntVt(0));", "ValueTask")]
    [InlineData("_ = doneTask.MatchAsync(() => SaveVt(1), e => LogVt(e));", "ValueTask")]
    [InlineData("_ = okVt.MatchAsync(x => GetIntAsync(x), e => GetIntAsync(0));", "Task")]
    [InlineData("_ = doneVt.MatchAsync(() => SaveAsync(1), e => LogAsync(e));", "Task")]
    public void MatchAsync_FunctionsReturningTheOtherKindOfTask_MessageNamesIt(string statements, string otherKind)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        var message = snippet.Errors.Single(d => d.Id == "CS0619").GetMessage(CultureInfo.InvariantCulture);
        message.ShouldContain($"The delegates return a {otherKind}, which the ");
    }
}
