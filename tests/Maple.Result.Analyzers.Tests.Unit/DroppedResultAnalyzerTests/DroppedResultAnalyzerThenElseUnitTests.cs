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

namespace Maple.Results.Analyzers.Tests.Unit.DroppedResultAnalyzerTests;

/// <summary>
///     Tests that the <see cref="DroppedResultAnalyzer" /> (MR0004) reports a result that a side effect method drops,
///     on call shapes that compile without a diagnostic of the compiler: calls of Then, Else, IfSuccess and IfError.
/// </summary>
public class DroppedResultAnalyzerThenElseUnitTests
{
    private const string Members = """
        static readonly Error Err = Error.Failure(ErrorUri.None, "x");
        static readonly HashSet<int> Set = new();
        static readonly HashSet<string> Titles = new();
        static int Failures;
        static Result<int> Ok() => 1;
        static Result<int> Failed() => Err;
        static Result<bool> FailedBool() => Err;
        static Result FailedR() => Err;
        static Result R() => Result.Success();
        static Result<int> Validate(int x) => x;
        static Result Audit(Error e) => Result.Success();
        static void Print(int x) { }
        static void Log(Error e) { }
        static Task<int> SaveChangesAsync() => Task.FromResult(1);
        static Task<Result<int>> ValidateAsync(int x) => Task.FromResult<Result<int>>(x);
        static Task LogAsync(Error e) => Task.CompletedTask;
        static Task<Result<int>> FallbackAsync() => Task.FromResult<Result<int>>(0);
        static ValueTask SaveVt() => default;
        static ValueTask<int> GetIntVt() => new(1);
        static ValueTask<Result<int>> ValidateVt(int x) => new((Result<int>)x);
        static ValueTask LogVt(Error e) => default;
        static ValueTask<Result<int>> FallbackVt() => new((Result<int>)0);
        static Result<object> FailedObject() => Result<object>.FromError(Err);
        static Task<Result<object>> FailedObjectTask() => Task.FromResult(FailedObject());
        static ValueTask<Result<object>> FailedObjectVt() => new(FailedObject());
        """;

    [Theory]
    [InlineData("_ = Ok().IfSuccess(x => Validate(x));")]
    [InlineData("_ = Ok().IfSuccess(x => GetError());")]
    [InlineData("_ = FailedR().IfError(e => Audit(e));")]
    [InlineData("_ = Failed().IfError(e => e.WithMetadata(\"k\", 1));")]
    [InlineData("_ = Task.FromResult(Ok()).IfSuccessAsync(x => ValidateAsync(x));")]
    [InlineData("_ = Failed().IfErrorAsync(async e => await FallbackAsync());")]
    public void Analyze_ThenOrElseCall_ReportsTheRule(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<DroppedResultAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldNotBeEmpty();
        result.DiagnosticIds.ShouldAllBe(id => id == Rules.DroppedResultId);
    }
}
