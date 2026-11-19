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
///     purpose: calls of Switch and SwitchAsync.
/// </summary>
public class NoDiagnosticSwitchUnitTests
{
    private const string Members = """
        static Result done = Result.Success();
        static Task<Result<int>> okTask = Task.FromResult(Result.FromValue(1));
        static Task<Result> doneTask = Task.FromResult(Result.Success());
        static ValueTask<Result<int>> okVt => new(Result.FromValue(1));
        static ValueTask<Result> doneVt => new(Result.Success());
        static int Compute(int x) => x;
        static void Show(int x) { }
        static void Log(Error e) { }
        static Task<int> GetIntAsync(int x) => Task.FromResult(x);
        static ValueTask<int> GetIntVt(int x) => new(x);
        static Task SaveAsync(int x) => Task.CompletedTask;
        static ValueTask SaveVt(int x) => default;
        static Task LogAsync(Error e) => Task.CompletedTask;
        static ValueTask LogVt(Error e) => default;
        static ValueTask<int> LogVtInt(Error e) => default;
        static Result<int> Validate(int x) => x;
        static Result Check(int x) => Result.Success();
        static Task<Result<int>> ValidateAsync(int x) => Task.FromResult(Result.FromValue(x));
        static ValueTask<Result<int>> ValidateVt(int x) => new(Result.FromValue(x));
        static System.Text.StringBuilder sb = new();
        static HashSet<int> set = new();
        static int last;
        """;

    [Theory]
    [InlineData("ok.Switch(x => Show(x), e => Log(e));")]
    [InlineData("ok.Switch(Show, Log);")]
    [InlineData("ok.Switch(x => { Show(x); }, e => { Log(e); });")]
    [InlineData("ok.Switch(x => sb.Append(x), e => sb.Append(e.Title));")]
    [InlineData("ok.Switch(x => set.Add(x), e => last = 0);")]
    [InlineData("ok.Switch(x => last = x, e => last++);")]
    [InlineData("ok.Switch(x => Show(x), e => throw new InvalidOperationException(e.Title));")]
    [InlineData("ok.Switch(x => throw new InvalidOperationException(), e => Log(e));")]
    [InlineData("ok.Switch(x => throw new InvalidOperationException(), e => throw new InvalidOperationException(e.Title));")]
    [InlineData("ok.Switch(x => { _ = SaveAsync(x); }, e => Log(e));")]
    [InlineData("done.Switch(() => Show(1), e => Log(e));")]
    [InlineData("done.Switch(() => sb.Append(1), e => throw new InvalidOperationException(e.Title));")]
    [InlineData("Task r = okTask.SwitchAsync(x => Show(x), e => Log(e));")]
    [InlineData("Task r = okTask.SwitchAsync(x => Show(x), e => throw new InvalidOperationException(e.Title));")]
    [InlineData("Task r = okTask.SwitchAsync(x => sb.Append(x), e => Log(e));")]
    [InlineData("Task r = doneTask.SwitchAsync(() => Show(1), e => Log(e));")]
    [InlineData("ValueTask r = okVt.SwitchAsync(x => Show(x), e => Log(e));")]
    [InlineData("ValueTask r = okVt.SwitchAsync(x => sb.Append(x), e => throw new InvalidOperationException(e.Title));")]
    [InlineData("ValueTask r = doneVt.SwitchAsync(() => Show(1), e => Log(e));")]
    public void Analyze_SwitchCallThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
