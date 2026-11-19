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
///     Tests that the <see cref="UnawaitedTaskAnalyzer" /> (MR0002) reports a task that a side-effect method does not await,
///     on call shapes that compile without a diagnostic of the compiler: calls of Switch and SwitchAsync.
/// </summary>
public class UnawaitedTaskAnalyzerSwitchUnitTests
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
    [InlineData("ok.Switch(x => SaveAsync(x), e => Log(e));")]
    [InlineData("ok.Switch(x => Show(x), e => LogAsync(e));")]
    [InlineData("ok.Switch(x => SaveAsync(x), e => LogAsync(e));")]
    [InlineData("ok.Switch(x => GetIntAsync(x), e => Log(e));")]
    [InlineData("ok.Switch(async x => await SaveAsync(x), e => Log(e));")]
    [InlineData("ok.Switch(x => Show(x), async e => await LogAsync(e));")]
    [InlineData("ok.Switch(async x => await SaveAsync(x), async e => await LogAsync(e));")]
    [InlineData("ok.Switch(x => SaveVt(x), e => Log(e));")]
    [InlineData("ok.Switch(x => Show(x), e => LogVt(e));")]
    [InlineData("ok.Switch(x => GetIntVt(x), e => Log(e));")]
    [InlineData("ok.Switch(x => Show(x), e => LogVtInt(e));")]
    [InlineData("ok.Switch(x => SaveVt(x), e => LogVt(e));")]
    [InlineData("ok.Switch(x => GetIntVt(x), e => LogVtInt(e));")]
    [InlineData("ok.Switch(x => SaveAsync(x), e => LogVt(e));")]
    [InlineData("ok.Switch(x => SaveVt(x), e => LogAsync(e));")]
    [InlineData("ok.Switch(x => GetIntVt(x), e => LogAsync(e));")]
    [InlineData("ok.Switch(x => SaveAsync(x), e => throw new InvalidOperationException(e.Title));")]
    [InlineData("done.Switch(() => SaveAsync(1), e => Log(e));")]
    [InlineData("done.Switch(() => Show(1), e => LogVt(e));")]
    [InlineData("done.Switch(async () => await SaveAsync(1), async e => await LogAsync(e));")]
    [InlineData("okTask.SwitchAsync(x => SaveAsync(x), e => Log(e));")]
    [InlineData("okTask.SwitchAsync(x => Show(x), e => LogAsync(e));")]
    [InlineData("okTask.SwitchAsync(x => SaveAsync(x), e => LogAsync(e));")]
    [InlineData("okTask.SwitchAsync(x => Show(x), async e => await LogAsync(e));")]
    [InlineData("okTask.SwitchAsync(x => SaveVt(x), e => Log(e));")]
    [InlineData("okTask.SwitchAsync(x => SaveVt(x), e => LogVt(e));")]
    [InlineData("doneTask.SwitchAsync(() => Show(1), e => LogAsync(e));")]
    [InlineData("okVt.SwitchAsync(x => SaveAsync(x), e => Log(e));")]
    [InlineData("okVt.SwitchAsync(x => Show(x), e => LogVt(e));")]
    [InlineData("okVt.SwitchAsync(async x => await SaveAsync(x), async e => await LogAsync(e));")]
    [InlineData("doneVt.SwitchAsync(() => SaveVt(1), e => Log(e));")]
    public void Analyze_SwitchCall_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<UnawaitedTaskAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.UnawaitedTaskId);
    }
}
