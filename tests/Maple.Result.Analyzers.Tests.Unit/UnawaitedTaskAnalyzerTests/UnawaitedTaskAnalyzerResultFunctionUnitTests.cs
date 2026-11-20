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

namespace Maple.Results.Analyzers.Tests.Unit.UnawaitedTaskAnalyzerTests;

/// <summary>
///     Tests that an <c>async</c> lambda or a function that returns a task, passed to <c>IfSuccess</c> or <c>IfError</c>, is reported once,
///     by the <see cref="UnawaitedTaskAnalyzer" /> (MR0002), and that it names the method that keeps what the task returns: <c>ThenAsync</c>
///     or <c>ElseAsync</c> for a result, an <c>Error</c>, or the type of the result’s value when the result of the call is used.
/// </summary>
public class UnawaitedTaskAnalyzerResultFunctionUnitTests
{
    private const string Members = """
        static Task<Result> ValidateAsync(int x) => Task.FromResult(Result.Success());
        static Task<Result<int>> RecoverAsync(Error e) => Task.FromResult(Result.FromValue(1));
        static Task<Result<string>> LoadOtherAsync(Error e) => Task.FromResult(Result.FromValue("a"));
        static Task<Error> ToErrorAsync(int x) => Task.FromResult(err);
        static Task<int> NormalizeAsync(int x) => Task.FromResult(x);
        static Task<int> DefaultAsync(Error e) => Task.FromResult(0);
        static Task<Error> WrapAsync(Error e) => Task.FromResult(e);
        """;

    [Theory]
    [InlineData("ok.IfSuccess(async x => await ValidateAsync(x));", "ThenAsync")]
    [InlineData("ok.IfSuccess(async x => await ToErrorAsync(x));", "ThenAsync")]
    [InlineData("ok.IfSuccess(x => ValidateAsync(x));", "ThenAsync")]
    [InlineData("var r = ok.IfSuccess(async x => await NormalizeAsync(x));", "ThenAsync")]
    [InlineData("var r = ok.IfSuccess(x => NormalizeAsync(x));", "ThenAsync")]
    [InlineData("ok.IfError(async e => await RecoverAsync(e));", "ElseAsync")]
    [InlineData("ok.IfError(async e => await WrapAsync(e));", "ElseAsync")]
    [InlineData("var r = ok.IfError(async e => await DefaultAsync(e));", "ElseAsync")]
    [InlineData("ok.IfError(async e => await LoadOtherAsync(e));", "IfErrorAsync")]
    [InlineData("ok.IfSuccess(async x => await NormalizeAsync(x));", "IfSuccessAsync")]
    [InlineData("_ = ok.IfSuccess(async x => await NormalizeAsync(x));", "IfSuccessAsync")]
    [InlineData("_ = ok.IfError(e => DefaultAsync(e));", "IfErrorAsync")]
    [InlineData("ok.IfSuccess(async x => await SaveAsync());", "IfSuccessAsync")]
    public void Analyze_FunctionThatReturnsATask_ReportsOnlyTheRuleAndNamesTheMethod(string statements, string newName)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Id.ShouldBe(Rules.UnawaitedTaskId);
        diagnostic.Properties[Rules.RenameToProperty].ShouldBe(newName);
        diagnostic.GetMessage().ShouldEndWith("Use " + newName + ".");
    }
}
