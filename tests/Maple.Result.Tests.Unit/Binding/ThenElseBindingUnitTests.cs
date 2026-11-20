// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Linq;

namespace Maple.Results.Tests.Unit.Binding;

/// <summary>
///     The side effects (<c>IfSuccess</c>, <c>IfError</c>) and the transformations (<c>Then</c>, <c>Else</c>) and their asynchronous forms
///     (#197): a side effect takes a function that returns a value and ignores the value, and awaits a task with a value; a delegate
///     that fits no overload (e.g., an action passed to a transformation, #256) is a compile error that the compiler reports by itself;
///     the rest binds as intended. The library has no overload traps (#223): a task that would not be awaited compiles, and the
///     analyzer (rules MR0001 to MR0006 of <c>Maple.Result.Analyzers</c>) reports it; those shapes are tested with the analyzer.
/// </summary>
public class ThenElseBindingUnitTests
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

    // A function that returns a value passed to a side effect as a method group or as a value that is not a statement, and a task
    // without a value passed to Else or ElseAsync on a task of a result: the compiler reports them by itself.
    [Theory]
    [InlineData("_ = Ok().IfSuccess(Set.Add);", "CS1929")]
    [InlineData("_ = R().IfSuccess(() => 5);", "CS0201")]
    [InlineData("_ = Failed().IfError(e => 0);", "CS0201")]
    [InlineData("_ = Failed().Else(e => LogAsync(e));", "CS0411")]
    [InlineData("_ = Task.FromResult(Failed()).ElseAsync(e => LogAsync(e));", "CS0029,CS0266,CS1662")]
    [InlineData("_ = FailedObjectTask().ElseAsync(async e => { await LogAsync(e); });", "CS1643")]
    public void TaskBased_DelegateOfTheWrongKind_ReportsTheCompilerErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("_ = FailedObjectVt().ElseAsync(async e => { await LogVt(e); });", "CS1643")]
    public void ValueTaskBased_DelegateOfTheWrongKind_ReportsTheCompilerErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }

    // An action, or a function that returns a task without a value, passed to Then, Else, ThenAsync or ElseAsync on a result: the
    // compiler reports it by itself (#256).
    [Theory]
    [InlineData("Ok().Then(x => Console.WriteLine(x));", "CS0029,CS1662")]
    [InlineData("Ok().Then(x => Print(x));", "CS0029,CS1662")]
    [InlineData("Ok().Then(Print);", "CS1929")]
    [InlineData("Ok().Then(x => { });", "CS1643")]
    [InlineData("R().Then(() => Console.WriteLine());", "CS0029,CS1662")]
    [InlineData("Failed().Else(e => Console.WriteLine(e));", "CS0029,CS1662")]
    [InlineData("Failed().Else(Log);", "CS0407")]
    [InlineData("Failed().Else(e => { });", "CS1643")]
    [InlineData("FailedObject().Else(e => Log(e));", "CS0029,CS1662")]
    [InlineData("Ok().ThenAsync(x => SaveAsync());", "CS0029,CS0266,CS1662")]
    [InlineData("Ok().ThenAsync(async x => { await SaveAsync(); });", "CS1643")]
    [InlineData("R().ThenAsync(() => SaveAsync());", "CS0029,CS0266,CS1662")]
    [InlineData("Failed().ElseAsync(e => LogAsync(e));", "CS0029,CS0266,CS1662")]
    [InlineData("Failed().ElseAsync(async e => { await LogAsync(e); });", "CS1643")]
    [InlineData("FailedObject().ElseAsync(e => LogAsync(e));", "CS0029,CS0266,CS1662")]
    public void TaskBased_ActionInATransformation_ReportsTheCompilerErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("Ok().ThenAsync(x => SaveVt());", "CS0029,CS1662")]
    [InlineData("Ok().ThenAsync(async x => { await SaveVt(); });", "CS1643")]
    [InlineData("R().ThenAsync(() => SaveVt());", "CS0029,CS1662")]
    [InlineData("Failed().ElseAsync(e => LogVt(e));", "CS0029,CS1662")]
    [InlineData("Failed().ElseAsync(async e => { await LogVt(e); });", "CS1643")]
    [InlineData("FailedObject().ElseAsync(e => LogVt(e));", "CS0029,CS1662")]
    public void ValueTaskBased_ActionInATransformation_ReportsTheCompilerErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("Ok().Then(x => Print(x));", "CS0029,CS1662")]
    [InlineData("Failed().Else(e => Log(e));", "CS0029,CS1662")]
    [InlineData("Ok().ThenAsync(x => SaveAsync());", "CS0029,CS0266,CS1662")]
    [InlineData("Ok().ThenAsync(x => SaveVt());", "CS0029,CS1662")]
    [InlineData("Failed().ElseAsync(e => LogAsync(e));", "CS0029,CS0266,CS1662")]
    [InlineData("Failed().ElseAsync(e => LogVt(e));", "CS0029,CS1662")]
    public void BothNamespaces_ActionInATransformation_ReportsTheCompilerErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
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
    [InlineData("Result<Task<int>> r = Ok().Then(x => Result.FromValue(Task.FromResult(x)));")]
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
    // A side effect whose function returns a value, or a task with a value: the value is ignored, and the task is awaited.
    [InlineData("Result<int> r = Ok().IfSuccess(x => Set.Add(x));")]
    [InlineData("Result<int> r = Failed().IfError(e => Interlocked.Increment(ref Failures));")]
    [InlineData("Result<bool> r = FailedBool().IfError(e => Titles.Add(e.Title));")]
    [InlineData("Task<Result<int>> r = Ok().IfSuccessAsync(x => SaveChangesAsync());")]
    [InlineData("Task<Result<int>> r = Ok().IfSuccessAsync(async x => await GetIntAsync());")]
    public void TaskBased_SideEffectOrTransformation_CompilesToExpectedType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("ValueTask<Result<int>> r = Ok().IfSuccessAsync(x => SaveVt());")]
    [InlineData("ValueTask<Result<int>> r = Ok().ThenAsync(x => ValidateVt(x));")]
    [InlineData("ValueTask<Result<int>> r = Ok().ThenAsync(x => GetIntVt());")]
    [InlineData("ValueTask<Result<int>> r = new ValueTask<Result<int>>(Ok()).ThenAsync(x => ValidateVt(x));")]
    [InlineData("ValueTask<Result<int>> r = Failed().IfErrorAsync(e => LogVt(e));")]
    [InlineData("ValueTask<Result<int>> r = Failed().ElseAsync(e => FallbackVt());")]
    [InlineData("ValueTask<Result> r = R().IfSuccessAsync(() => SaveVt());")]
    [InlineData("ValueTask<Result<int>> r = R().ThenAsync(() => GetIntVt());")]
    // A side effect whose function returns a ValueTask with a value: the task is awaited, and the value is ignored (#289).
    [InlineData("ValueTask<Result<int>> r = Ok().IfSuccessAsync(x => GetIntVt());")]
    [InlineData("ValueTask<Result<int>> r = Failed().IfErrorAsync(e => FallbackVt());")]
    [InlineData("ValueTask<Result> r = R().IfSuccessAsync(() => GetIntVt());")]
    public void ValueTaskBased_SideEffectOrTransformation_CompilesToExpectedType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("Result<int> r = Ok().IfSuccess(x => Console.WriteLine(x));")]
    [InlineData("Result<bool> r = Ok().Then(x => Set.Add(x));")]
    [InlineData("Result<int> r = Failed().IfError(e => Log(e));")]
    [InlineData("Result<int> r = Failed().Else(e => 0);")]
    [InlineData("Result<int> r = from x in Ok() select x + 1;")]
    public void BothNamespaces_SynchronousCall_CompilesToExpectedType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }
}
