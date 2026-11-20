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

namespace Maple.Results.Analyzers.CodeFixes.Tests.Unit;

/// <summary>
///     Tests of the <see cref="DiscardResultCodeFixProvider" />: the fix of MR0006 discards the result with <c>_ =</c>, keeps the rest of
///     the statement, and the fixed code compiles and is not reported again.
/// </summary>
public class DiscardResultCodeFixProviderUnitTests
{
    [Theory]
    [InlineData("{|MR0006:GetResult()|};", "_ = GetResult();")]
    [InlineData("{|MR0006:GetResultOfInt()|};", "_ = GetResultOfInt();")]
    [InlineData("{|MR0006:await GetResultAsync()|};", "_ = await GetResultAsync();")]
    [InlineData("{|MR0006:GetResultAsync()|};", "_ = GetResultAsync();")]
    [InlineData("{|MR0006:ok.Then(x => x + 1)|};", "_ = ok.Then(x => x + 1);")]
    [InlineData("new List<int> { 1 }.ForEach(x => {|MR0006:GetResult()|});", "new List<int> { 1 }.ForEach(x => _ = GetResult());")]
    public async Task RegisterCodeFixes_ResultThatIsNotUsed_DiscardsIt(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<DiscardedResultAnalyzer, DiscardResultCodeFixProvider>.VerifyAsync(statements, fixedStatements);
    }

    [Theory]
    [InlineData("{|MR0006:GetResult()|}; {|MR0006:GetResultOfInt()|};", "_ = GetResult(); _ = GetResultOfInt();")]
    public async Task RegisterCodeFixes_SeveralResultsThatAreNotUsed_DiscardsAll(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<DiscardedResultAnalyzer, DiscardResultCodeFixProvider>.VerifyAsync(statements, fixedStatements);
    }

    [Theory]
    [InlineData("/* saved */ {|MR0006:GetResult()|}; // done", "/* saved */ _ = GetResult(); // done")]
    public async Task RegisterCodeFixes_StatementWithComments_KeepsThem(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<DiscardedResultAnalyzer, DiscardResultCodeFixProvider>.VerifyAsync(statements, fixedStatements);
    }
}
