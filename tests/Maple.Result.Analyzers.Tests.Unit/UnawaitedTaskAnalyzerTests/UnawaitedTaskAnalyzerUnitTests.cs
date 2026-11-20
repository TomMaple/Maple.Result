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

namespace Maple.Results.Analyzers.Tests.Unit.UnawaitedTaskAnalyzerTests;

/// <summary>
///     Tests of the <see cref="UnawaitedTaskAnalyzer" /> (MR0002): what it marks, what its message advises, the method name it offers to a
///     code fix, and the functions that it does not report.
/// </summary>
public class UnawaitedTaskAnalyzerUnitTests
{
    private const string Members = """
        static Task<Result<int>> okTask = Task.FromResult(ok);
        static Task LogAsync(Error e) => Task.CompletedTask;
        static void Log(Error e) { }
        """;

    [Theory]
    [InlineData("_ = ok.IfSuccess(x => SaveAsync());", "SaveAsync()",
        "IfSuccess takes an action here, so the task that this function returns is not awaited. Use IfSuccessAsync.")]
    [InlineData("_ = ok.IfError(e => LogAsync(e));", "LogAsync(e)",
        "IfError takes an action here, so the task that this function returns is not awaited. Use IfErrorAsync.")]
    [InlineData("_ = ok.IfError(async e => await LogAsync(e));", "async e => await LogAsync(e)",
        "IfError takes an action here, so this async lambda becomes async void and is not awaited. Use IfErrorAsync.")]
    [InlineData("ok.Switch(x => Console.WriteLine(x), e => LogAsync(e));", "LogAsync(e)",
        "Switch takes an action here, so the task that this function returns is not awaited. Use Match or MatchAsync with two functions that return a task.")]
    // With its type argument written, the call cannot bind to the overload for a ValueTask with a value (#289), and takes the action.
    [InlineData("_ = okTask.IfSuccessAsync<int>(x => GetIntValueTask());", "GetIntValueTask()",
        "IfSuccessAsync takes an action here, so the task that this function returns is not awaited. "
        + "Pass a function that returns a Task or a ValueTask without a value, e.g., an async lambda that awaits it.")]
    public void Analyze_TaskPassedAsAnAction_MarksTheFunctionAndAdvisesTheFix(string statements, string markedText, string message)
    {
        // Act
        var result = AnalyzerRunner.Run<UnawaitedTaskAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Id.ShouldBe(Rules.UnawaitedTaskId);
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Error);
        result.MarkedText(diagnostic).ShouldBe(markedText);
        diagnostic.GetMessage().ShouldBe(message);
    }

    [Theory]
    [InlineData("_ = ok.IfSuccess(x => SaveAsync());", "IfSuccess", "IfSuccessAsync")]
    [InlineData("_ = ok.IfError(async e => await LogAsync(e));", "IfError", "IfErrorAsync")]
    public void Analyze_TaskInASynchronousMethod_OffersTheAsyncFormAtTheMethodName(string statements, string methodName, string newName)
    {
        // Act
        var result = AnalyzerRunner.Run<UnawaitedTaskAnalyzer>(statements, Members);

        // Assert
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Properties[Rules.RenameToProperty].ShouldBe(newName);
        var nameLocation = diagnostic.AdditionalLocations.ShouldHaveSingleItem();
        result.TextAt(nameLocation).ShouldBe(methodName);
    }

    [Fact]
    public void Analyze_TwoTasksInSwitch_ReportsEachFunction()
    {
        // Act
        var result = AnalyzerRunner.Run<UnawaitedTaskAnalyzer>("ok.Switch(x => SaveAsync(), e => LogAsync(e));", Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBe([Rules.UnawaitedTaskId, Rules.UnawaitedTaskId]);
    }

    [Theory]
    [InlineData("_ = ok.IfSuccess(x => Console.WriteLine(x));")]
    [InlineData("_ = ok.IfError(e => { _ = LogAsync(e); });")]
    [InlineData("_ = ok.IfSuccessAsync(x => SaveAsync());")]
    [InlineData("_ = ok.IfErrorAsync(async e => await LogAsync(e));")]
    [InlineData("_ = okTask.IfErrorAsync(e => Log(e));")]
    // A ValueTask with a value is awaited by an overload of its own (#289).
    [InlineData("_ = okTask.IfSuccessAsync(x => GetIntValueTask());")]
    [InlineData("_ = ok.IfSuccessAsync(x => GetIntValueTask());")]
    [InlineData("_ = okTask.IfErrorAsync(e => GetIntValueTask());")]
    [InlineData("_ = ok.IfError(e => throw new InvalidOperationException(e.Title));")]
    [InlineData("new List<int> { 1 }.ForEach(x => SaveAsync());")]
    public void Analyze_ActionOrAwaitedFunction_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<UnawaitedTaskAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
