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

namespace Maple.Results.Analyzers.Tests.Unit.NoDiagnosticTests;

/// <summary>
///     Tests that no analyzer reports the call shapes that are correct, or that keep a task or an <c>Error</c> as a value on
///     purpose: functions that return a value.
/// </summary>
public class NoDiagnosticValueFunctionUnitTests
{
    private const string Members = """
        static readonly Error Err = Error.Failure(ErrorUri.None, "x");
        static int Last;
        static int Count;
        static Error? LastError;
        static Result<int> Ok() => 1;
        static Result<int> Failed() => Err;
        static Task<Error> GetErrorAsync() => Task.FromResult(Err);
        static ValueTask<int> GetIntVt() => new(1);
        static ValueTask<Error> GetErrorVt() => new(Err);
        """;

    [Theory]
    [InlineData("_ = Ok().IfSuccess(x => Last = x);")]
    [InlineData("_ = Ok().IfSuccess(x => Count += x);")]
    [InlineData("_ = Failed().IfError(e => LastError = e);")]
    [InlineData("_ = Task.FromResult(Ok()).IfSuccessAsync(x => Last = x);")]
    [InlineData("_ = Task.FromResult(Failed()).IfErrorAsync(e => LastError = e);")]
    [InlineData("_ = Ok().IfSuccessAsync(async x => Last = await GetIntAsync());")]
    [InlineData("_ = Failed().IfErrorAsync(async e => LastError = await GetErrorAsync());")]
    [InlineData("_ = new ValueTask<Result<int>>(Ok()).IfSuccessAsync(x => Last = x);")]
    [InlineData("_ = Ok().IfSuccessAsync(async x => Last = await GetIntVt());")]
    [InlineData("_ = Failed().IfErrorAsync(async e => LastError = await GetErrorVt());")]
    [InlineData("_ = Ok().IfSuccess(x => { Last = x; });")]
    [InlineData("_ = Ok().IfSuccess(x => { Count++; });")]
    [InlineData("_ = Failed().IfError(e => { LastError = e; });")]
    [InlineData("Ok().Switch(x => { Last = x; }, e => { LastError = e; });")]
    [InlineData("_ = Task.FromResult(Ok()).IfSuccessAsync(x => { Last = x; });")]
    [InlineData("_ = Ok().IfSuccessAsync(async x => { Last = await GetIntAsync(); });")]
    [InlineData("_ = Task.FromResult(Ok()).SwitchAsync(x => { Last = x; }, e => { LastError = e; });")]
    [InlineData("_ = new ValueTask<Result<int>>(Failed()).IfErrorAsync(e => { Count++; });")]
    [InlineData("_ = Ok().IfSuccessAsync(async x => { Last = await GetIntVt(); });")]
    public void Analyze_FunctionReturningValueThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
