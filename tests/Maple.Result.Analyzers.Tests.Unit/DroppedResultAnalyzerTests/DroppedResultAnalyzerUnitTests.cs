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

namespace Maple.Results.Analyzers.Tests.Unit.DroppedResultAnalyzerTests;

/// <summary>
///     Tests of the <see cref="DroppedResultAnalyzer" /> (MR0004): a function passed to a side effect method that returns a result or an
///     <c>Error</c> (or a task of one) is reported; a function that returns something else, or stores the result, is not.
/// </summary>
public class DroppedResultAnalyzerUnitTests
{
    private const string Members = """
        static Result<int>? last;
        static Task<Result<int>> okTask = Task.FromResult(ok);
        static Result<int> Validate(int x) => x;
        static Task<Result<int>> ValidateAsync(int x) => Task.FromResult(Result.FromValue(x));
        static Result Audit(Error e) => Result.Success();
        static void Log(Error e) { }
        """;

    [Theory]
    [InlineData("_ = ok.IfSuccess(x => Validate(x));", "x => Validate(x)",
        "IfSuccess ignores the Result<int> that this function returns, so a failure is lost. Use Then to continue with it.")]
    [InlineData("_ = ok.IfSuccess(x => GetError());", "x => GetError()",
        "IfSuccess ignores the Error that this function returns, so a failure is lost. Use Then to continue with it.")]
    [InlineData("_ = ok.IfSuccessAsync(x => ValidateAsync(x));", "x => ValidateAsync(x)",
        "IfSuccessAsync ignores the Task<Result<int>> that this function returns, so a failure is lost. Use ThenAsync to continue with it.")]
    [InlineData("_ = ok.IfSuccessAsync(ValidateAsync);", "ValidateAsync",
        "IfSuccessAsync ignores the Task<Result<int>> that this function returns, so a failure is lost. Use ThenAsync to continue with it.")]
    [InlineData("_ = ok.IfSuccessAsync(async x => await ValidateAsync(x));", "async x => await ValidateAsync(x)",
        "IfSuccessAsync ignores the Result<int> that this function returns, so a failure is lost. Use ThenAsync to continue with it.")]
    [InlineData("_ = okTask.IfSuccessAsync(x => Validate(x));", "x => Validate(x)",
        "IfSuccessAsync ignores the Result<int> that this function returns, so a failure is lost. Use ThenAsync to continue with it.")]
    [InlineData("_ = ok.IfError(e => Validate(0));", "e => Validate(0)",
        "IfError ignores the Result<int> that this function returns, so a failure is lost. Use Else to continue with it.")]
    [InlineData("_ = ok.IfError(e => GetError());", "e => GetError()",
        "IfError ignores the Error that this function returns, so a failure is lost. Use Else to continue with it.")]
    [InlineData("_ = ok.IfError(e => Audit(e));", "e => Audit(e)",
        "IfError ignores the Result that this function returns, so a failure is lost. Handle it inside the function.")]
    [InlineData("ok.Switch(x => Validate(x), e => Log(e));", "x => Validate(x)",
        "Switch ignores the Result<int> that this function returns, so a failure is lost. Handle it inside the function.")]
    public void Analyze_FunctionReturningAResult_MarksTheFunctionAndAdvisesTheFix(string statements, string markedText, string message)
    {
        // Act
        var result = AnalyzerRunner.Run<DroppedResultAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Id.ShouldBe(Rules.DroppedResultId);
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Error);
        result.MarkedText(diagnostic).ShouldBe(markedText);
        diagnostic.GetMessage().ShouldBe(message);
    }

    [Theory]
    [InlineData("_ = ok.IfSuccess(x => Validate(x));", "IfSuccess", "Then")]
    [InlineData("_ = ok.IfSuccessAsync(x => ValidateAsync(x));", "IfSuccessAsync", "ThenAsync")]
    [InlineData("_ = ok.IfError(e => Validate(0));", "IfError", "Else")]
    [InlineData("_ = ok.IfError(e => GetError());", "IfError", "Else")]
    [InlineData("_ = ok.IfErrorAsync(e => ValidateAsync(0));", "IfErrorAsync", "ElseAsync")]
    public void Analyze_ResultThatTheOtherMethodKeeps_OffersItAtTheMethodName(string statements, string methodName, string newName)
    {
        // Act
        var result = AnalyzerRunner.Run<DroppedResultAnalyzer>(statements, Members);

        // Assert
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Properties[Rules.RenameToProperty].ShouldBe(newName);
        var nameLocation = diagnostic.AdditionalLocations.ShouldHaveSingleItem();
        result.TextAt(nameLocation).ShouldBe(methodName);
    }

    [Fact]
    public void Analyze_ResultOfAnotherTypeDroppedByIfError_OffersNoMethodName()
    {
        // Act
        var result = AnalyzerRunner.Run<DroppedResultAnalyzer>("_ = ok.IfError(e => Audit(e));", Members);

        // Assert
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Properties.ShouldNotContainKey(Rules.RenameToProperty);
    }

    [Theory]
    [InlineData("_ = ok.IfSuccess(x => last = Validate(x));")]
    [InlineData("_ = ok.IfSuccess(x => { Validate(x); });")]
    [InlineData("_ = ok.IfSuccess(x => Console.WriteLine(x));")]
    [InlineData("_ = ok.IfSuccess(x => Compute());")]
    [InlineData("_ = ok.IfSuccessAsync(x => SaveAsync());")]
    [InlineData("_ = ok.IfSuccessAsync(x => GetIntAsync());")]
    [InlineData("_ = ok.Then(x => Validate(x));")]
    [InlineData("_ = ok.ThenAsync(x => ValidateAsync(x));")]
    [InlineData("new List<int> { 1 }.ForEach(x => Validate(x));")]
    public void Analyze_FunctionWhoseResultIsNotDropped_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<DroppedResultAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
