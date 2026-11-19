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
///     on call shapes that compile without a diagnostic of the compiler: calls on a ValueTask of a result.
/// </summary>
public class UnawaitedTaskAnalyzerValueTaskResultUnitTests
{
    private const string Members = """
        public record User(int Id, string Name);
        static readonly User Guest = new(0, "guest");
        static readonly HashSet<int> set = [];
        static readonly HashSet<string> titles = [];
        static ValueTask<Result<User>> GetUser() => ValueTask.FromResult(Result<User>.FromValue(Guest));
        static ValueTask<Result> GetOk() => ValueTask.FromResult(Result.Success());
        static string GetName(User u) => u.Name;
        static ValueTask<string> GetNameAsync(User u) => ValueTask.FromResult(u.Name);
        static Task<string> GetNameOtherAsync(User u) => Task.FromResult(u.Name);
        static ValueTask<int> GetLengthAsync(string s) => ValueTask.FromResult(s.Length);
        static ValueTask<Result<string>> GetResultAsync(User u) => ValueTask.FromResult(Result<string>.FromValue(u.Name));
        static ValueTask<Result> SaveResultAsync(User u) => ValueTask.FromResult(Result.Success());
        static ValueTask SaveAsync(User u) => ValueTask.CompletedTask;
        static Task SaveOtherAsync(User u) => Task.CompletedTask;
        static ValueTask LogAsync(Error e) => ValueTask.CompletedTask;
        static Task LogOtherAsync(Error e) => Task.CompletedTask;
        static void Log(User u) { }
        static void LogError(Error e) { }
        """;

    [Theory]
    [InlineData("_ = GetUser().IfSuccessAsync(u => GetNameAsync(u));")]
    [InlineData("_ = GetUser().IfErrorAsync(e => GetNameAsync(null!));")]
    public void Analyze_CallOnValueTaskResult_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<UnawaitedTaskAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.UnawaitedTaskId);
    }
}
