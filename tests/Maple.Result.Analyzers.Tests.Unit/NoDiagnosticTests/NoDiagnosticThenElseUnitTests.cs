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
///     purpose: calls of Then, Else, IfSuccess and IfError.
/// </summary>
public class NoDiagnosticThenElseUnitTests
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
    [InlineData("_ = Ok().IfSuccess(x => Set.Add(x));")]
    [InlineData("Result<int> r = Ok().IfSuccess(x => Console.WriteLine(x));")]
    [InlineData("Result<int> r = Ok().IfSuccess(x => { Set.Add(x); });")]
    [InlineData("Result<int> r = Ok().IfSuccess(Print);")]
    [InlineData("Result r = R().IfSuccess(() => Console.WriteLine());")]
    [InlineData("Result<int> r = Failed().IfError(e => Console.WriteLine(e.Title));")]
    [InlineData("Result<int> r = Failed().IfError(Log);")]
    [InlineData("Result<int> r = Failed().IfError(e => { Interlocked.Increment(ref Failures); });")]
    [InlineData("IOperationResult io = Failed(); IOperationResult r2 = io.IfError(e => Log(e));")]
    [InlineData("Result<bool> r = Ok().Then(x => Set.Add(x));")]
    [InlineData("Result<string> r = Ok().Then(x => x.ToString());")]
    [InlineData("Result<int> r = Ok().Then(x => Validate(x));")]
    [InlineData("Result<int> r = Ok().Then(Validate);")]
    [InlineData("Result r = Ok().Then(x => GetError());")]
    [InlineData("Result r = Ok().Then(x => Result.Success());")]
    [InlineData("Result<int> r = R().Then(() => 5);")]
    [InlineData("Result r = R().Then(() => GetError());")]
    [InlineData("Result<int> r = Failed().Else(e => 0);")]
    [InlineData("Result<int> r = Failed().Else(e => e.WithMetadata(\"k\", 1));")]
    [InlineData("Result r = FailedR().Else(e => Audit(e));")]
    [InlineData("Result<int> r = Failed().Else(e => Interlocked.Increment(ref Failures));")]
    [InlineData("Task<Result<int>> r = Ok().IfSuccessAsync(async x => { await SaveAsync(); });")]
    [InlineData("Task<Result<int>> r = Ok().IfSuccessAsync(x => SaveAsync());")]
    [InlineData("Task<Result<int>> r = Task.FromResult(Ok()).IfSuccessAsync(x => SaveAsync());")]
    [InlineData("Task<Result> r = R().IfSuccessAsync(() => SaveAsync());")]
    [InlineData("Task<Result<int>> r = Ok().ThenAsync(async x => await GetIntAsync());")]
    [InlineData("Task<Result<int>> r = Ok().ThenAsync(x => ValidateAsync(x));")]
    [InlineData("Task<Result<int>> r = Task.FromResult(Ok()).ThenAsync(x => ValidateAsync(x));")]
    [InlineData("Task<Result<int>> r = Ok().ThenAsync(ValidateAsync);")]
    [InlineData("Task<Result<int>> r = Failed().IfErrorAsync(e => LogAsync(e));")]
    [InlineData("Task<Result<int>> r = Failed().ElseAsync(async e => await FallbackAsync());")]
    [InlineData("Task<Result<int>> r = Task.FromResult(Failed()).ElseAsync(e => FallbackAsync());")]
    [InlineData("Task<Result<int>> r = Task.FromResult(Failed()).IfErrorAsync(e => LogAsync(e));")]
    [InlineData("Result<int> r = from x in Ok() select x + 1;")]
    [InlineData("Task<Result<int>> r = (from x in Task.FromResult(Ok()) from y in ValidateAsync(x) select x + y);")]
    [InlineData("int r = Ok().Match(x => x, e => 0);")]
    [InlineData("ValueTask<Result<int>> r = Ok().IfSuccessAsync(x => SaveVt());")]
    [InlineData("ValueTask<Result<int>> r = Ok().ThenAsync(x => ValidateVt(x));")]
    [InlineData("ValueTask<Result<int>> r = Ok().ThenAsync(x => GetIntVt());")]
    [InlineData("ValueTask<Result<int>> r = new ValueTask<Result<int>>(Ok()).ThenAsync(x => ValidateVt(x));")]
    [InlineData("ValueTask<Result<int>> r = Failed().IfErrorAsync(e => LogVt(e));")]
    [InlineData("ValueTask<Result<int>> r = Failed().ElseAsync(e => FallbackVt());")]
    [InlineData("ValueTask<Result> r = R().IfSuccessAsync(() => SaveVt());")]
    [InlineData("ValueTask<Result<int>> r = R().ThenAsync(() => GetIntVt());")]
    [InlineData("Result<int> r = Failed().IfError(e => Log(e));")]
    public void Analyze_ThenOrElseCallThatIsCorrect_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("Result<Task<int>> r = Ok().Then(x => Result.FromValue(Task.FromResult(x)));")]
    public void Analyze_ThenOrElseCallKeepingTheValueOnPurpose_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run(statements, Members, AnalyzerRunner.AllAnalyzers);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
