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
///     purpose: calls next to a lambda that only throws.
/// </summary>
public class NoDiagnosticThrowingLambdaUnitTests
{
    private const string Members = """
        static readonly Result done = Result.Success();
        static readonly Task<Result<int>> okTask = Task.FromResult(Result.FromValue(1));
        static int count;
        static void Log(Error e) { }
        static Task LogAsync(Error e) => Task.CompletedTask;
        static ValueTask LogValueAsync(Error e) => ValueTask.CompletedTask;
        static Task<int> GetCountAsync(Error e) => Task.FromResult(1);
        static ValueTask<int> GetCountValueAsync(Error e) => new(1);
        static Task SaveAsync(int x) => Task.CompletedTask;
        static int Compute(int x) => x * 2;
        static Exception Fail() => new InvalidOperationException();
        static Exception Fail(Error e) => new InvalidOperationException(e.Title);
        """;

    [Theory]
    [InlineData("var r = ok.IfError(e => throw Fail(e));")]
    [InlineData("var r = ok.IfError(e => { throw Fail(e); });")]
    [InlineData("var r = ok.IfSuccess(_ => throw Fail());")]
    [InlineData("var r = done.IfSuccess(() => throw Fail());")]
    [InlineData("var r = ok.Else(e => throw Fail(e));")]
    [InlineData("var r = ok.Then(x => throw Fail());")]
    [InlineData("var r = done.Then(() => throw Fail());")]
    [InlineData("ok.Switch(x => { }, e => throw Fail(e));")]
    [InlineData("done.Switch(() => { }, e => throw Fail(e));")]
    [InlineData("ok.Switch(x => throw Fail(), e => { });")]
    [InlineData("ok.Switch(x => throw Fail(), e => throw Fail());")]
    [InlineData("int r = ok.Match(x => x, e => throw Fail(e));")]
    [InlineData("int r = ok.Match(x => Compute(x), e => throw Fail(e));")]
    [InlineData("Task<int> r = okTask.MatchAsync(x => x, e => throw Fail(e));")]
    [InlineData("Task r = okTask.SwitchAsync(x => { }, e => throw Fail(e));")]
    [InlineData("var r = ok.Select(x => throw Fail());")]
    [InlineData("Task<Result<int>> r = ok.IfErrorAsync(e => throw Fail(e));")]
    [InlineData("Task<Result<int>> r = okTask.IfErrorAsync(e => throw Fail(e));")]
    [InlineData("Task<Result<int>> r = ok.IfSuccessAsync(x => throw Fail());")]
    [InlineData("Task<Result<int>> r = okTask.IfSuccessAsync(x => throw Fail());")]
    [InlineData("var r = ok.ThenAsync(x => throw Fail());")]
    [InlineData("var r = okTask.ThenAsync(x => throw Fail());")]
    [InlineData("var r = ok.IfError(e => Log(e));")]
    [InlineData("var r = ok.IfError(e => { Log(e); });")]
    [InlineData("var r = ok.IfError(Log);")]
    [InlineData("var r = ok.IfError(e => { _ = LogAsync(e); });")]
    [InlineData("var r = ok.IfError(void (Error e) => throw Fail());")]
    [InlineData("var r = ok.Else(e => 0);")]
    [InlineData("ok.Switch(x => { }, e => { });")]
    public void Analyze_CallNextToThrowingLambdaThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("var r = ok.Then<int, Task<int>>(x => GetIntAsync());")]
    [InlineData("var r = done.Then<Task>(() => SaveAsync(1));")]
    [InlineData("var r = ok.Select<int, Task>(x => SaveAsync(x));")]
    [InlineData("Result<Task<int>> r = ok.Then(x => Result.FromValue(GetIntAsync()));")]
    [InlineData("Result<Task> r = done.Then(() => Result.FromValue(SaveAsync(1)));")]
    [InlineData("Result<Task> r = from x in ok select Result.FromValue(SaveAsync(x));")]
    public void Analyze_CallNextToThrowingLambdaKeepingTheValueOnPurpose_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
