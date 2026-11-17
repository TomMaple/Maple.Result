// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Maple.Results.Tests.Unit.TestingInfrastructure.TestData;

/// <summary>
///     The members of the snippets that test the binding of <c>Match</c>, <c>MatchAsync</c>, <c>Switch</c> and <c>SwitchAsync</c>
///     (#255): the receivers and the helper methods of the call shapes in the issue.
/// </summary>
internal static class MatchSwitchSnippetMembers
{
    /// <summary>The members, placed in the snippet class next to <c>err</c> and <c>ok</c> of the snippet compiler.</summary>
    public const string Members = """
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
}
