// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Analyzers.CodeFixes.Tests.Unit.TestingInfrastructure.Helpers;
using System.Threading.Tasks;

namespace Maple.Results.Analyzers.CodeFixes.Tests.Unit.RenameMethodCodeFixProviderTests;

/// <summary>
///     Tests of the <see cref="RenameMethodCodeFixProvider" />: the fix of MR0001, MR0002, MR0004 and MR0005 calls the method that the
///     diagnostic names, changes nothing else, and the fixed code compiles; a diagnostic that names no method gets no fix.
/// </summary>
public class RenameMethodCodeFixProviderUnitTests
{
    [Theory]
    [InlineData("var r = ok.{|MR0001:Then|}(x => GetIntAsync());", "var r = ok.ThenAsync(x => GetIntAsync());")]
    [InlineData("var r = GetResult().{|MR0001:Then|}(() => SaveAsync());", "var r = GetResult().IfSuccessAsync(() => SaveAsync());")]
    [InlineData("var r = Result.{|MR0001:Try|}(() => GetIntAsync(), e => err);", "var r = Result.TryAsync(() => GetIntAsync(), e => err);")]
    public async Task RegisterCodeFixes_TaskInferredAsValue_CallsTheAsyncForm(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<InferredResultValueAnalyzer, RenameMethodCodeFixProvider>.VerifyAsync(statements, fixedStatements);
    }

    [Theory]
    [InlineData("_ = ok.IfSuccess(x => {|MR0002:SaveAsync()|});", "_ = ok.IfSuccessAsync(x => SaveAsync());")]
    [InlineData("_ = ok.IfError(e => {|MR0002:LogAsync(e)|});", "_ = ok.IfErrorAsync(e => LogAsync(e));")]
    [InlineData("_ = ok.IfError((Error e) => {|MR0002:LogAsync(e)|});", "_ = ok.IfErrorAsync((Error e) => LogAsync(e));")]
    public async Task RegisterCodeFixes_TaskPassedAsAnAction_CallsTheAsyncForm(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<UnawaitedTaskAnalyzer, RenameMethodCodeFixProvider>.VerifyAsync(statements, fixedStatements);
    }

    [Theory]
    [InlineData("_ = ok.IfSuccess({|MR0004:x => GetResultOfInt()|});", "_ = ok.Then(x => GetResultOfInt());")]
    [InlineData("_ = ok.IfSuccessAsync({|MR0004:x => GetResultOfIntAsync()|});", "_ = ok.ThenAsync(x => GetResultOfIntAsync());")]
    [InlineData("_ = ok.IfError({|MR0004:e => GetResultOfInt()|});", "_ = ok.Else(e => GetResultOfInt());")]
    [InlineData("_ = ok.IfError({|MR0004:e => GetError()|});", "_ = ok.Else(e => GetError());")]
    [InlineData("_ = ok.IfErrorAsync({|MR0004:e => GetResultOfIntAsync()|});", "_ = ok.ElseAsync(e => GetResultOfIntAsync());")]
    public async Task RegisterCodeFixes_ResultDroppedByASideEffectMethod_CallsTheMethodThatKeepsIt(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<DroppedResultAnalyzer, RenameMethodCodeFixProvider>.VerifyAsync(statements, fixedStatements);
    }

    [Theory]
    [InlineData("var r = ok.IfSuccess(x => {|MR0005:Compute()|});", "var r = ok.Then(x => Compute());")]
    [InlineData("var r = ok.IfSuccessAsync(x => {|MR0005:GetIntAsync()|});", "var r = ok.ThenAsync(x => GetIntAsync());")]
    [InlineData("var r = ok.IfError(e => {|MR0005:Compute()|});", "var r = ok.Else(e => Compute());")]
    [InlineData("var r = okTask.IfErrorAsync(e => {|MR0005:Compute()|});", "var r = okTask.ElseAsync(e => Compute());")]
    public async Task RegisterCodeFixes_ValueIgnoredByASideEffectMethod_CallsTheTransformingMethod(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<IgnoredValueAnalyzer, RenameMethodCodeFixProvider>.VerifyAsync(statements, fixedStatements);
    }

    [Theory]
    [InlineData(
        "var a = ok.{|MR0001:Then|}(x => GetIntAsync()); var b = ok.{|MR0001:Then|}(x => GetIntAsync());",
        "var a = ok.ThenAsync(x => GetIntAsync()); var b = ok.ThenAsync(x => GetIntAsync());")]
    public async Task RegisterCodeFixes_SeveralDiagnosticsWithTheSameFix_FixesAll(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<InferredResultValueAnalyzer, RenameMethodCodeFixProvider>.VerifyAsync(statements, fixedStatements);
    }

    [Theory]
    [InlineData(
        "_ = ok.IfSuccess(x => {|MR0002:SaveAsync()|}); _ = ok.IfSuccess(x => {|MR0002:GetIntAsync()|});",
        "_ = ok.IfSuccessAsync(x => SaveAsync()); _ = ok.IfSuccessAsync(x => GetIntAsync());")]
    public async Task RegisterCodeFixes_SeveralDiagnosticsThatMarkTheFunction_FixesAllAtTheMethodNames(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<UnawaitedTaskAnalyzer, RenameMethodCodeFixProvider>.VerifyAsync(statements, fixedStatements);
    }

    [Theory]
    [InlineData("var r = from x in ok {|MR0001:select GetIntAsync()|};")]
    [InlineData("var r = ThenExtensions.{|MR0001:Then|}(ok, x => GetIntAsync());")]
    [InlineData("var r = okTask.{|MR0001:ThenAsync|}(x => SaveAsync());")]
    public async Task RegisterCodeFixes_TaskWhereNoRenameHelpsOrIsValid_OffersNoFix(string statements)
    {
        // Act & Assert
        await CodeFixVerifier<InferredResultValueAnalyzer, RenameMethodCodeFixProvider>.VerifyAsync(statements, statements);
    }

    [Theory]
    [InlineData("_ = ok.IfError({|MR0004:e => GetResult()|});")]
    public async Task RegisterCodeFixes_ResultOfAnotherTypeDroppedByIfError_OffersNoFix(string statements)
    {
        // Act & Assert
        await CodeFixVerifier<DroppedResultAnalyzer, RenameMethodCodeFixProvider>.VerifyAsync(statements, statements);
    }
}
