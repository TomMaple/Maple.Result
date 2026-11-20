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

namespace Maple.Results.Analyzers.Tests.Unit.IgnoredValueAnalyzerTests;

/// <summary>
///     Tests of the <see cref="IgnoredValueAnalyzer" /> (MR0005): <c>IfSuccess</c> or <c>IfError</c> with a function that returns the type
///     of the result’s value, in a call whose result is used, is what version 2 code wrote to transform the result, and is reported as a
///     warning; a side effect that returns another type, or a call whose result is not used, is not.
/// </summary>
public class IgnoredValueAnalyzerUnitTests
{
    private const string Members = """
        static readonly StringBuilder builder = new();
        static readonly HashSet<int> set = new();
        static int last;
        static Task<Result<int>> okTask = Task.FromResult(ok);
        static int Normalize(int x) => x;
        static Task<int> NormalizeAsync(int x) => Task.FromResult(x);
        static ValueTask<int> NormalizeVt(int x) => new(x);
        static ValueTask<bool> FlushVt() => new(true);
        static int Fallback(Error e) => 0;
        static Result<int>? maybe = ok;
        """;

    [Theory]
    [InlineData("var r = ok.IfSuccess(x => Normalize(x));", "Normalize(x)",
        "IfSuccess runs this function as a side effect and keeps the original result; in version 2 it transformed the result. "
        + "Use Then to continue with the returned value.")]
    [InlineData("var r = await ok.IfSuccessAsync(x => NormalizeAsync(x));", "NormalizeAsync(x)",
        "IfSuccessAsync runs this function as a side effect and keeps the original result; in version 2 it transformed the result. "
        + "Use ThenAsync to continue with the returned value.")]
    [InlineData("var r = await ok.IfSuccessAsync(async x => await NormalizeAsync(x));", "await NormalizeAsync(x)",
        "IfSuccessAsync runs this function as a side effect and keeps the original result; in version 2 it transformed the result. "
        + "Use ThenAsync to continue with the returned value.")]
    [InlineData("var r = await ok.IfSuccessAsync(NormalizeAsync);", "NormalizeAsync",
        "IfSuccessAsync runs this function as a side effect and keeps the original result; in version 2 it transformed the result. "
        + "Use ThenAsync to continue with the returned value.")]
    [InlineData("var r = await okTask.IfSuccessAsync(x => Normalize(x));", "Normalize(x)",
        "IfSuccessAsync runs this function as a side effect and keeps the original result; in version 2 it transformed the result. "
        + "Use ThenAsync to continue with the returned value.")]
    // A function that returns a ValueTask with a value, and an async lambda with a block body that returns one: the overload for a
    // ValueTask with a value awaits it and ignores the value (#289).
    [InlineData("var r = await ok.IfSuccessAsync(x => NormalizeVt(x));", "NormalizeVt(x)",
        "IfSuccessAsync runs this function as a side effect and keeps the original result; in version 2 it transformed the result. "
        + "Use ThenAsync to continue with the returned value.")]
    [InlineData("var r = await okTask.IfSuccessAsync(x => NormalizeVt(x));", "NormalizeVt(x)",
        "IfSuccessAsync runs this function as a side effect and keeps the original result; in version 2 it transformed the result. "
        + "Use ThenAsync to continue with the returned value.")]
    [InlineData("var r = await ok.IfSuccessAsync(async x => { await Task.Yield(); return x + 1; });", "async x => { await Task.Yield(); return x + 1; }",
        "IfSuccessAsync runs this function as a side effect and keeps the original result; in version 2 it transformed the result. "
        + "Use ThenAsync to continue with the returned value.")]
    [InlineData("var r = ok.IfError(e => Fallback(e));", "Fallback(e)",
        "IfError runs this function as a side effect and keeps the original result; in version 2 it transformed the result. "
        + "Use Else to continue with the returned value.")]
    [InlineData("return ok.IfSuccess(x => Normalize(x)).IsSuccess ? 1 : 0;", "Normalize(x)",
        "IfSuccess runs this function as a side effect and keeps the original result; in version 2 it transformed the result. "
        + "Use Then to continue with the returned value.")]
    [InlineData("var s = ok.IfSuccess(x => Normalize(x))?.IsSuccess;", "Normalize(x)",
        "IfSuccess runs this function as a side effect and keeps the original result; in version 2 it transformed the result. "
        + "Use Then to continue with the returned value.")]
    public void Analyze_FunctionReturningTheValueTypeInAUsedCall_WarnsAndNamesTheTransformingMethod(string statements, string markedText, string message)
    {
        // Act
        var result = AnalyzerRunner.Run<IgnoredValueAnalyzer>(statements, Members);

        // Assert
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Id.ShouldBe(Rules.IgnoredValueId);
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Warning);
        result.MarkedText(diagnostic).ShouldBe(markedText);
        diagnostic.GetMessage().ShouldBe(message);
    }

    [Theory]
    [InlineData("var r = ok.IfSuccess(x => Normalize(x));", "IfSuccess", "Then")]
    [InlineData("var r = ok.IfSuccessAsync(x => NormalizeAsync(x));", "IfSuccessAsync", "ThenAsync")]
    [InlineData("var r = ok.IfError(e => Fallback(e));", "IfError", "Else")]
    [InlineData("var r = okTask.IfErrorAsync(e => Fallback(e));", "IfErrorAsync", "ElseAsync")]
    [InlineData("var r = ok.IfSuccessAsync(x => NormalizeVt(x));", "IfSuccessAsync", "ThenAsync")]
    public void Analyze_FunctionReturningTheValueType_OffersTheTransformingMethodAtTheMethodName(string statements, string methodName, string newName)
    {
        // Act
        var result = AnalyzerRunner.Run<IgnoredValueAnalyzer>(statements, Members);

        // Assert
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Properties[Rules.RenameToProperty].ShouldBe(newName);
        var nameLocation = diagnostic.AdditionalLocations.ShouldHaveSingleItem();
        result.TextAt(nameLocation).ShouldBe(methodName);
    }

    [Theory]
    [InlineData("ok.IfSuccess(x => Normalize(x));")]
    [InlineData("await ok.IfSuccessAsync(x => NormalizeAsync(x));")]
    [InlineData("await ok.IfSuccessAsync(x => NormalizeVt(x));")]
    [InlineData("maybe?.IfSuccess(x => Normalize(x));")]
    [InlineData("await okTask.IfSuccessAsync(x => Normalize(x)).ConfigureAwait(false);")]
    [InlineData("await ok.IfSuccessAsync(x => NormalizeAsync(x)).ConfigureAwait(false);")]
    [InlineData("_ = ok.IfSuccess(x => Normalize(x));")]
    [InlineData("_ = await ok.IfSuccessAsync(x => NormalizeAsync(x));")]
    [InlineData("_ = ok.IfError(e => Fallback(e));")]
    [InlineData("var r = ok.IfSuccessAsync(x => FlushVt());")]
    [InlineData("var r = ok.IfSuccessAsync(async x => { await Task.Yield(); return true; });")]
    [InlineData("var r = ok.IfSuccess(x => builder.Append(x));")]
    [InlineData("var r = ok.IfSuccess(x => set.Add(x));")]
    [InlineData("var r = ok.IfSuccess(x => last = x);")]
    [InlineData("var r = ok.IfSuccess(x => { Normalize(x); });")]
    [InlineData("var r = ok.IfSuccess(x => Console.WriteLine(x));")]
    [InlineData("var r = GetResult().IfSuccess(() => Compute());")]
    [InlineData("var r = ok.Then(x => Normalize(x));")]
    public void Analyze_SideEffectOrUnusedCall_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<IgnoredValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
