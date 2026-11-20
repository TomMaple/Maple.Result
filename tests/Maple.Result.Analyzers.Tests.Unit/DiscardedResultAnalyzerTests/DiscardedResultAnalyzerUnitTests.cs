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

namespace Maple.Results.Analyzers.Tests.Unit.DiscardedResultAnalyzerTests;

/// <summary>
///     Tests of the <see cref="DiscardedResultAnalyzer" /> (MR0006): a statement that is a call whose result, or task of a result, is not
///     used is reported; a result that is used, discarded explicitly or whose error was handled is not.
/// </summary>
public class DiscardedResultAnalyzerUnitTests
{
    private const string Members = """
        sealed class Order { public Result Save() => Result.Success(); }
        static Order? order = new();
        static Task<Result<int>> okTask = Task.FromResult(ok);
        static Result Save() => Result.Success();
        static Result<int> Load() => 1;
        static Task<Result> SaveResultAsync() => Task.FromResult(Result.Success());
        static ValueTask<Result<int>> LoadValueTask() => new(Result.FromValue(1));
        static void Log(Error e) { }
        """;

    [Theory]
    [InlineData("Save();", "Save()")]
    [InlineData("Load();", "Load()")]
    [InlineData("await SaveResultAsync();", "await SaveResultAsync()")]
    [InlineData("SaveResultAsync();", "SaveResultAsync()")]
    [InlineData("await LoadValueTask();", "await LoadValueTask()")]
    [InlineData("order?.Save();", "order?.Save()")]
    [InlineData("ok.Then(x => x + 1);", "ok.Then(x => x + 1)")]
    [InlineData("Load().IfSuccess(x => Console.WriteLine(x));", "Load().IfSuccess(x => Console.WriteLine(x))")]
    [InlineData("ok.Then(x => x + 1).IfSuccess(x => Console.WriteLine(x));", "ok.Then(x => x + 1).IfSuccess(x => Console.WriteLine(x))")]
    [InlineData("await LoadValueTask().IfSuccessAsync(x => Console.WriteLine(x));", "await LoadValueTask().IfSuccessAsync(x => Console.WriteLine(x))")]
    [InlineData("ok.Else(e => 0);", "ok.Else(e => 0)")]
    [InlineData("await okTask.ThenAsync(x => x + 1);", "await okTask.ThenAsync(x => x + 1)")]
    [InlineData("new List<int> { 1 }.ForEach(x => Load());", "Load()")]
    public void Analyze_ResultThatIsNotUsed_ReportsTheStatement(string statements, string markedText)
    {
        // Act
        var result = AnalyzerRunner.Run<DiscardedResultAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Id.ShouldBe(Rules.DiscardedResultId);
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        result.MarkedText(diagnostic).ShouldBe(markedText);
    }

    [Theory]
    [InlineData("_ = Save();")]
    [InlineData("var saved = Save();")]
    [InlineData("_ = await SaveResultAsync();")]
    [InlineData("var isSaved = Save().IsSuccess;")]
    [InlineData("Console.WriteLine(Load());")]
    [InlineData("Compute();")]
    [InlineData("await SaveAsync();")]
    [InlineData("ok.IfError(e => Log(e));")]
    [InlineData("ok.IfSuccess(x => Console.WriteLine(x));")]
    [InlineData("await okTask.IfSuccessAsync(x => Console.WriteLine(x));")]
    [InlineData("ok.IfSuccess(x => Console.WriteLine(x)).IfSuccess(x => Console.WriteLine(x));")]
    [InlineData("Load().IfError(e => Log(e)).IfSuccess(x => Console.WriteLine(x));")]
    [InlineData("ok.Then(x => x + 1).IfError(e => Log(e));")]
    [InlineData("await okTask.IfErrorAsync(e => Log(e));")]
    [InlineData("ok.Match(x => Load(), e => e);")]
    [InlineData("ok.Switch(x => Console.WriteLine(x), e => Log(e));")]
    [InlineData("_ = ok.IfSuccess(x => Load());")]
    public void Analyze_ResultThatIsUsedDiscardedOrHandled_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<DiscardedResultAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("Save();", "The Result returned by Save is not used, so a failure is lost. Check it, return it, or discard it with ‘_ =’.")]
    [InlineData(
        "await LoadValueTask();",
        "The Result<int> returned by LoadValueTask is not used, so a failure is lost. Check it, return it, or discard it with ‘_ =’.")]
    public void Analyze_ResultThatIsNotUsed_NamesTheResultAndTheMethod(string statements, string message)
    {
        // Act
        var result = AnalyzerRunner.Run<DiscardedResultAnalyzer>(statements, Members);

        // Assert
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.GetMessage().ShouldBe(message);
    }
}
