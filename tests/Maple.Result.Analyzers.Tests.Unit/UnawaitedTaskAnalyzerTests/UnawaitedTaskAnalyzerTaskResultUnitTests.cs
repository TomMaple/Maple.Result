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
///     Tests that the <see cref="UnawaitedTaskAnalyzer" /> (MR0002) reports a task that a side effect method does not await,
///     on call shapes that compile without a diagnostic of the compiler: calls on a Task of a result.
/// </summary>
public class UnawaitedTaskAnalyzerTaskResultUnitTests
{
    private const string Members = """
        public record User(int Id, string Name);
        static readonly User Guest = new(0, "guest");
        static readonly HashSet<int> set = [];
        static readonly HashSet<string> titles = [];
        static Task<Result<User>> GetUser() => Task.FromResult(Result<User>.FromValue(Guest));
        static Task<Result> GetOk() => Task.FromResult(Result.Success());
        static string GetName(User u) => u.Name;
        static Task<string> GetNameAsync(User u) => Task.FromResult(u.Name);
        static ValueTask<string> GetNameOtherAsync(User u) => ValueTask.FromResult(u.Name);
        static Task<int> GetLengthAsync(string s) => Task.FromResult(s.Length);
        static Task<Result<string>> GetResultAsync(User u) => Task.FromResult(Result<string>.FromValue(u.Name));
        static Task<Result> SaveResultAsync(User u) => Task.FromResult(Result.Success());
        static Task SaveAsync(User u) => Task.CompletedTask;
        static ValueTask SaveOtherAsync(User u) => ValueTask.CompletedTask;
        static Task LogAsync(Error e) => Task.CompletedTask;
        static ValueTask LogOtherAsync(Error e) => ValueTask.CompletedTask;
        static void Log(User u) { }
        static void LogError(Error e) { }
        """;

    [Theory]
    [InlineData("_ = GetUser().IfSuccessAsync(u => GetNameOtherAsync(u));")]
    public void Analyze_CallOnTaskResult_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<UnawaitedTaskAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.UnawaitedTaskId);
    }
}
