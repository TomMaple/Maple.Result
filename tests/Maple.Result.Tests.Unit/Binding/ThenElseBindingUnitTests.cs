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
///     (#197): a function that returns a value passed to a side effect, or a task that would not be awaited, is a compile error (CS0619)
///     that names the method to use; an action passed to a transformation is a compile error that the compiler reports by itself (#256);
///     the rest binds as intended.
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

    [Theory]
    [InlineData("_ = Ok().IfSuccess(x => Set.Add(x));")]
    [InlineData("_ = Ok().IfSuccess(x => Validate(x));")]
    [InlineData("_ = Ok().IfSuccess(x => GetError());")]
    [InlineData("_ = Ok().IfSuccess(Set.Add);")]
    [InlineData("_ = R().IfSuccess(() => 5);")]
    [InlineData("_ = Ok().IfSuccess(x => SaveAsync());")]
    [InlineData("_ = Ok().IfSuccess(async x => await SaveAsync());")]
    [InlineData("_ = Ok().IfSuccess(x => GetIntVt());")]
    [InlineData("_ = Ok().Then(x => SaveAsync());")]
    [InlineData("_ = Ok().Then(async x => await GetIntAsync());")]
    [InlineData("_ = Failed().IfError(e => Interlocked.Increment(ref Failures));")]
    [InlineData("_ = FailedBool().IfError(e => Titles.Add(e.Title));")]
    [InlineData("_ = FailedR().IfError(e => Audit(e));")]
    [InlineData("_ = Failed().IfError(e => 0);")]
    [InlineData("_ = Failed().IfError(e => e.WithMetadata(\"k\", 1));")]
    [InlineData("_ = Failed().IfError(e => LogAsync(e));")]
    [InlineData("_ = Failed().Else(e => LogAsync(e));")]
    [InlineData("_ = Ok().IfSuccessAsync(x => SaveChangesAsync());")]
    [InlineData("_ = Ok().IfSuccessAsync(async x => await GetIntAsync());")]
    [InlineData("_ = Task.FromResult(Ok()).IfSuccessAsync(x => ValidateAsync(x));")]
    [InlineData("_ = Failed().IfErrorAsync(async e => await FallbackAsync());")]
    [InlineData("_ = Task.FromResult(Failed()).ElseAsync(e => LogAsync(e));")]
    [InlineData("_ = FailedObjectTask().ElseAsync(e => LogAsync(e));")]
    [InlineData("_ = FailedObjectTask().ElseAsync(async e => { await LogAsync(e); });")]
    public void TaskBased_DelegateOfTheWrongKind_ReportsOnlyCS0619(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("_ = Ok().IfSuccessAsync(x => GetIntVt());")]
    [InlineData("_ = Failed().IfErrorAsync(e => FallbackVt());")]
    [InlineData("_ = R().IfSuccessAsync(() => GetIntVt());")]
    [InlineData("_ = FailedObjectVt().ElseAsync(e => LogVt(e));")]
    [InlineData("_ = FailedObjectVt().ElseAsync(async e => { await LogVt(e); });")]
    public void ValueTaskBased_DelegateOfTheWrongKind_ReportsOnlyCS0619(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.ValueTask, Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }

    // An action, or a function that returns a task without a value, passed to Then, Else, ThenAsync or ElseAsync on a result: the
    // compiler reports it by itself, so there is no overload trap for it (#256).
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
    [InlineData("Ok().ThenAsync(x => SaveAsync());", "CS0266,CS1662")]
    [InlineData("Ok().ThenAsync(async x => { await SaveAsync(); });", "CS1643")]
    [InlineData("R().ThenAsync(() => SaveAsync());", "CS0266,CS1662")]
    [InlineData("Failed().ElseAsync(e => LogAsync(e));", "CS0266,CS1662")]
    [InlineData("Failed().ElseAsync(async e => { await LogAsync(e); });", "CS1643")]
    [InlineData("FailedObject().ElseAsync(e => LogAsync(e));", "CS0266,CS1662")]
    public void TaskBased_ActionInATransformation_ReportsTheCompilerErrors(string statements, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

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
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.ValueTask, Members);

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
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

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
    public void TaskBased_SideEffectOrTransformation_CompilesToExpectedType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Task, Members);

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
    public void ValueTaskBased_SideEffectOrTransformation_CompilesToExpectedType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.ValueTask, Members);

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
        var snippet = SnippetCompiler.Compile(statements, SnippetImports.Both, Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }
}
