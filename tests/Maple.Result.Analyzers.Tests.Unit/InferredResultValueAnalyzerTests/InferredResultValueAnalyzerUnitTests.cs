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
using Microsoft.CodeAnalysis;

namespace Maple.Results.Analyzers.Tests.Unit.InferredResultValueAnalyzerTests;

/// <summary>
///     Tests of the <see cref="InferredResultValueAnalyzer" /> (MR0001): what it marks, what its message advises, the method name it offers
///     to a code fix, and the calls that keep a task, a result or an <c>Error</c> as a value on purpose, which it does not report.
/// </summary>
public class InferredResultValueAnalyzerUnitTests
{
    private const string Members = """
        static Task<Result<int>> okTask = Task.FromResult(ok);
        static Result<Task<int>> resultOfTask = Result.FromValue(Task.FromResult(1));
        static Error MapException(Exception e) => err;
        """;

    [Theory]
    [InlineData("var r = ok.Then(x => GetIntAsync());",
        "The function passed to Then returns a Task<int>, which becomes the value of the result. Use ThenAsync to await it.")]
    [InlineData("var r = ok.Then(x => GetIntValueTask());",
        "The function passed to Then returns a ValueTask<int>, which becomes the value of the result. Use ThenAsync to await it.")]
    [InlineData("var r = ok.Then(x => SaveAsync());",
        "The function passed to Then returns a Task, which becomes the value of the result. Use IfSuccessAsync for a side effect that returns no value.")]
    [InlineData("var r = Result.Try(() => GetIntAsync(), MapException);",
        "The function passed to Try returns a Task<int>, which becomes the value of the result. Use TryAsync to await it.")]
    [InlineData("var r = okTask.ThenAsync(x => SaveAsync());",
        "The function passed to ThenAsync returns a Task, which becomes the value of the result. Use IfSuccessAsync for a side effect that returns no value.")]
    [InlineData("var r = okTask.ThenAsync(async x => await SaveAsync());",
        "The function passed to ThenAsync returns a Task, which becomes the value of the result. Use IfSuccessAsync for a side effect that returns no value.")]
    [InlineData("var r = from x in ok select GetIntAsync();",
        "The function passed to Select returns a Task<int>, which becomes the value of the result. Await it in a from clause, or use ThenAsync.")]
    [InlineData("var r = okTask.MatchAsync(ifSuccessFunction: x => SaveAsync(), ifErrorFunction: e => SaveAsync());",
        "The function passed to MatchAsync returns a Task, so the call returns a task of a task. Pass functions that the method awaits.")]
    public void Analyze_TaskInferredAsValue_AdvisesTheFix(string statements, string message)
    {
        // Act
        var result = AnalyzerRunner.Run<InferredResultValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Id.ShouldBe(Rules.InferredResultValueId);
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Error);
        diagnostic.GetMessage().ShouldBe(message);
    }

    [Theory]
    [InlineData("var r = ok.Then(x => GetIntAsync());", "Then", "ThenAsync")]
    [InlineData("var r = GetResult().Then(() => SaveAsync());", "Then", "IfSuccessAsync")]
    [InlineData("var r = ok.Then(x => SaveAsync());", "Then", "IfSuccessAsync")]
    [InlineData("var r = Result.Try(() => GetIntAsync(), MapException);", "Try", "TryAsync")]
    public void Analyze_TaskInASynchronousMethod_MarksTheMethodNameAndOffersTheAsyncForm(string statements, string markedText, string newName)
    {
        // Act
        var result = AnalyzerRunner.Run<InferredResultValueAnalyzer>(statements, Members);

        // Assert
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        result.MarkedText(diagnostic).ShouldBe(markedText);
        diagnostic.Properties[Rules.RenameToProperty].ShouldBe(newName);
    }

    [Theory]
    [InlineData("var r = okTask.ThenAsync(x => SaveAsync());")]
    [InlineData("var r = from x in ok select GetIntAsync();")]
    [InlineData("var r = ThenExtensions.Then(ok, x => GetIntAsync());")]
    public void Analyze_TaskWhereNoRenameHelpsOrIsValid_OffersNoMethodName(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<InferredResultValueAnalyzer>(statements, Members);

        // Assert
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Properties.ShouldNotContainKey(Rules.RenameToProperty);
    }

    [Theory]
    [InlineData("var r = ok.Then<int, Task<int>>(x => GetIntAsync());")]
    [InlineData("var r = Result.Try<Task<int>>(() => GetIntAsync(), MapException);")]
    [InlineData("var r = ok.Then(x => Result.FromValue(GetIntAsync()));")]
    [InlineData("var r = Result.FromValue(GetIntAsync());")]
    [InlineData("var r = from x in ok select Result.FromValue(GetIntAsync());")]
    [InlineData("var r = resultOfTask.IfSuccess(t => Console.WriteLine(t.Id));")]
    [InlineData("var r = resultOfTask.Else(e => Task.FromResult(0));")]
    [InlineData("var t = ok.Match(x => GetIntAsync(), e => GetIntAsync());")]
    [InlineData("var r = ok.Then(x => x + 1);")]
    [InlineData("var r = ok.ThenAsync(x => GetIntAsync());")]
    public void Analyze_ValueKeptOnPurposeOrAwaited_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<InferredResultValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
