// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Extensions;
using Maple.Results.Tests.Unit.TestingInfrastructure.Helpers;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Extensions;

public class MatchTaskAsyncExtensionsUnitTests
{
    private static readonly Error SourceError = Error.Failure(ErrorUri.None, "50f1c1fb-560e-4bb3-84da-034072df7ca2", "Error title 6");

    #region MatchAsync (Task<Result>, Func<TOut>, Func<Error, TOut>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultTaskWithFunctions_ReturnsValueOfSuccessFunction()
    {
        // Arrange
        var errorCalled = false;
        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => "success", _ =>
        {
            errorCalled = true;

            return "error";
        });

        // Assert
        result.ShouldBe("success");
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultTaskWithFunctions_ReturnsValueOfErrorFunctionCalledWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = Task.FromResult(Result.FromError(SourceError));

        // Act
        var result = await sut.MatchAsync(() =>
        {
            successCalled = true;

            return "success";
        }, e =>
        {
            passedError = e;

            return "error";
        });

        // Assert
        result.ShouldBe("error");
        passedError.ShouldBeSameAs(SourceError);
        successCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_ResultTaskReturningNull_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = Task.FromResult<Result>(null!);

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => 1, _ => 0));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task MatchAsync_ResultTaskWithFunctionReturningNullResult_ReturnsNull()
    {
        // Arrange
        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => (Result)null!, e => e);

        // Assert
        result.ShouldBeNull();
    }

    #endregion

    #region MatchAsync (Task<Result>, Func<Task<TOut>>, Func<Error, Task<TOut>>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultTaskWithTaskFunctions_ReturnsValueOfSuccessFunction()
    {
        // Arrange
        var errorCalled = false;
        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(async () =>
        {
            await Task.Yield();

            return "success";
        }, _ =>
        {
            errorCalled = true;

            return Task.FromResult("error");
        });

        // Assert
        result.ShouldBe("success");
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultTaskWithTaskFunctions_ReturnsValueOfErrorFunctionCalledWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = Task.FromResult(Result.FromError(SourceError));

        // Act
        var result = await sut.MatchAsync(() =>
        {
            successCalled = true;

            return Task.FromResult("success");
        }, async e =>
        {
            await Task.Yield();
            passedError = e;

            return "error";
        });

        // Assert
        result.ShouldBe("error");
        passedError.ShouldBeSameAs(SourceError);
        successCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_ResultTaskWithTaskFunctionReturningNullResult_ReturnsNull()
    {
        // Arrange
        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => Task.FromResult<Result>(null!), e => Task.FromResult<Result>(e));

        // Assert
        result.ShouldBeNull();
    }

    #endregion

    #region MatchAsync (Task<Result>, Func<Task>, Func<Error, Task>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultTaskWithTaskActions_AwaitsOnlySuccessFunction()
    {
        // Arrange
        var successCompleted = false;
        var errorCalled = false;
        var sut = Task.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(async () =>
        {
            await Task.Yield();
            successCompleted = true;
        }, _ =>
        {
            errorCalled = true;

            return Task.CompletedTask;
        });

        // Assert
        successCompleted.ShouldBeTrue();
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultTaskWithTaskActions_AwaitsOnlyErrorFunctionWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = Task.FromResult(Result.FromError(SourceError));

        // Act
        await sut.MatchAsync(() =>
        {
            successCalled = true;

            return Task.CompletedTask;
        }, async e =>
        {
            await Task.Yield();
            passedError = e;
        });

        // Assert
        passedError.ShouldBeSameAs(SourceError);
        successCalled.ShouldBeFalse();
    }

    #endregion

    #region MatchAsync (Task<Result<T>>, Func<T, TOut>, Func<Error, TOut>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTTaskWithFunctions_ReturnsValueOfSuccessFunctionCalledWithTheValue()
    {
        // Arrange
        var errorCalled = false;
        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        var result = await sut.MatchAsync(x => $"value {x}", _ =>
        {
            errorCalled = true;

            return "error";
        });

        // Assert
        result.ShouldBe("value 5");
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTTaskWithFunctions_ReturnsValueOfErrorFunctionCalledWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = Task.FromResult(Result<int>.FromError(SourceError));

        // Act
        var result = await sut.MatchAsync(x =>
        {
            successCalled = true;

            return $"value {x}";
        }, e =>
        {
            passedError = e;

            return "error";
        });

        // Assert
        result.ShouldBe("error");
        passedError.ShouldBeSameAs(SourceError);
        successCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTTaskWithErrorFunctionReturningTheError_ReturnsFailedResult()
    {
        // Arrange
        var sut = Task.FromResult(Result<int>.FromError(SourceError));

        // Act
        var result = await sut.MatchAsync(x => Result.FromValue(x.ToString()), e => e);

        // Assert
        result.Error.ShouldBeSameAs(SourceError);
    }

    [Fact]
    public async Task MatchAsync_ResultOfTTaskReturningNull_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = Task.FromResult<Result<int>>(null!);

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(x => x, _ => 0));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    #endregion

    #region MatchAsync (Task<Result<T>>, Func<T, Task<TOut>>, Func<Error, Task<TOut>>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTTaskWithTaskFunctions_ReturnsValueOfSuccessFunctionCalledWithTheValue()
    {
        // Arrange
        var errorCalled = false;
        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        var result = await sut.MatchAsync(async x =>
        {
            await Task.Yield();

            return $"value {x}";
        }, _ =>
        {
            errorCalled = true;

            return Task.FromResult("error");
        });

        // Assert
        result.ShouldBe("value 5");
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTTaskWithTaskFunctions_ReturnsValueOfErrorFunctionCalledWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = Task.FromResult(Result<int>.FromError(SourceError));

        // Act
        var result = await sut.MatchAsync(x =>
        {
            successCalled = true;

            return Task.FromResult($"value {x}");
        }, async e =>
        {
            await Task.Yield();
            passedError = e;

            return "error";
        });

        // Assert
        result.ShouldBe("error");
        passedError.ShouldBeSameAs(SourceError);
        successCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_ResultOfTTaskWithTaskFunctionThrowing_PropagatesTheException()
    {
        // Arrange
        var thrown = new InvalidOperationException("success function");
        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync<int, int>(_ => throw thrown, _ => Task.FromResult(0)));

        // Assert
        exception.ShouldBeSameAs(thrown);
    }

    #endregion

    #region MatchAsync (Task<Result<T>>, Func<T, Task>, Func<Error, Task>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTTaskWithTaskActions_AwaitsOnlySuccessFunctionWithTheValue()
    {
        // Arrange
        int? passedValue = null;
        var errorCalled = false;
        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        await sut.MatchAsync(async x =>
        {
            await Task.Yield();
            passedValue = x;
        }, _ =>
        {
            errorCalled = true;

            return Task.CompletedTask;
        });

        // Assert
        passedValue.ShouldBe(5);
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTTaskWithTaskActions_AwaitsOnlyErrorFunctionWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = Task.FromResult(Result<int>.FromError(SourceError));

        // Act
        await sut.MatchAsync(_ =>
        {
            successCalled = true;

            return Task.CompletedTask;
        }, async e =>
        {
            await Task.Yield();
            passedError = e;
        });

        // Assert
        passedError.ShouldBeSameAs(SourceError);
        successCalled.ShouldBeFalse();
    }

    #endregion

    #region overload traps

    // See issue #255.

    [Fact]
    public void MatchAsync_TwoActionsOverloads_AreHiddenCompileErrorsThatNameSwitchAsync()
    {
        // Act
        var traps = OverloadTrapHelper.TaskPartMethods(typeof(MatchAsyncExtensions))
            .Where(m => OverloadTrapHelper.IsTrap(m, "MatchAsync takes two functions that return a value. Use SwitchAsync"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(2);
        traps.ShouldAllBe(m => m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never
                               && m.GetParameters().Skip(1).Take(2).All(p => p.ParameterType.Name.StartsWith("Action", StringComparison.Ordinal)));
    }

    // A function that returns a ValueTask on a Task of a result is awaited by an overload of its own, which returns a Task (#226).
    [Fact]
    public void MatchAsync_FunctionReturningValueTaskOnTaskResult_IsAnOverloadWithPriorityZero()
    {
        // Act
        var overloads = OverloadTrapHelper.TaskPartMethods(typeof(MatchAsyncExtensions))
            .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is null
                        && m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(Task<>)
                        && m.GetParameters().Where(p => p.ParameterType.Name.StartsWith("Func`", StringComparison.Ordinal))
                            .All(p => OverloadTrapHelper.IsValueTaskFunction(p.ParameterType))
                        && m.GetParameters().Any(p => OverloadTrapHelper.IsValueTaskFunction(p.ParameterType)))
            .ToList();

        // Assert
        overloads.Count.ShouldBe(4);
        overloads.ShouldAllBe(m => m.Name == "MatchAsync" && OverloadTrapHelper.Priority(m) == 0);
    }

    [Fact]
    public void MatchAsync_AllMethods_AreOverloadsOnATaskOrTheTrapsForTwoActions()
    {
        // Act
        var methods = OverloadTrapHelper.TaskPartMethods(typeof(MatchAsyncExtensions)).ToList();

        // Assert
        methods.Count.ShouldBe(6 + 4 + 2); // the overloads, those for the other kind of task (#226), the traps for two actions
        methods.ShouldAllBe(m => m.Name == "MatchAsync" && m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(Task<>));
    }

    [Fact]
    public void MatchAsync_OverloadsWithTaskFunctions_HavePriorityOne()
    {
        // Act
        var overloads = OverloadTrapHelper.TaskPartMethods(typeof(MatchAsyncExtensions))
            .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is null && OverloadTrapHelper.IsTaskFunction(m.GetParameters()[1].ParameterType))
            .ToList();

        // Assert
        overloads.Count.ShouldBe(4);
        overloads.ShouldAllBe(m => OverloadTrapHelper.Priority(m) == 1);
    }

    #endregion

    #region Task results with functions returning a ValueTask

    // A function that returns the other kind of task is awaited, and the receiver decides the kind (#226).

    private static readonly Error OtherKindError = Error.Failure(ErrorUri.None, "Other kind of task test error");

    [Fact]
    public async Task MatchAsync_SuccessfulResultTaskWithValueTaskFunctions_RunsTheSuccessFunction()
    {
        // Arrange
        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(1), e => ValueTask.FromResult(0));

        // Assert
        result.ShouldBe(1);
    }

    [Fact]
    public async Task MatchAsync_FailedResultTaskWithValueTaskFunctions_RunsTheErrorFunction()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromError(OtherKindError));

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(1), e => ValueTask.FromResult(0));

        // Assert
        result.ShouldBe(0);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultTaskWithValueTaskActions_RunsTheSuccessFunction()
    {
        // Arrange
        var sut = Task.FromResult(Result.Success());
        var log = "";

        // Act
        await sut.MatchAsync(() => { log += "success"; return ValueTask.CompletedTask; }, e => { log += "error"; return ValueTask.CompletedTask; });

        // Assert
        log.ShouldBe("success");
    }

    [Fact]
    public async Task MatchAsync_FailedResultTaskWithValueTaskActions_RunsTheErrorFunction()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromError(OtherKindError));
        var log = "";

        // Act
        await sut.MatchAsync(() => { log += "success"; return ValueTask.CompletedTask; }, e => { log += "error"; return ValueTask.CompletedTask; });

        // Assert
        log.ShouldBe("error");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTTaskWithValueTaskFunctions_RunsTheSuccessFunction()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        var result = await sut.MatchAsync(x => ValueTask.FromResult(x * 2), e => ValueTask.FromResult(0));

        // Assert
        result.ShouldBe(10);
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTTaskWithValueTaskFunctions_RunsTheErrorFunction()
    {
        // Arrange
        var sut = Task.FromResult(Result<int>.FromError(OtherKindError));

        // Act
        var result = await sut.MatchAsync(x => ValueTask.FromResult(x * 2), e => ValueTask.FromResult(0));

        // Assert
        result.ShouldBe(0);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTTaskWithValueTaskActions_RunsTheSuccessFunction()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromValue(5));
        var log = "";

        // Act
        await sut.MatchAsync(x => { log += x; return ValueTask.CompletedTask; }, e => { log += "error"; return ValueTask.CompletedTask; });

        // Assert
        log.ShouldBe("5");
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTTaskWithValueTaskActions_RunsTheErrorFunction()
    {
        // Arrange
        var sut = Task.FromResult(Result<int>.FromError(OtherKindError));
        var log = "";

        // Act
        await sut.MatchAsync(x => { log += x; return ValueTask.CompletedTask; }, e => { log += "error"; return ValueTask.CompletedTask; });

        // Assert
        log.ShouldBe("error");
    }

    [Fact]
    public async Task MatchAsync_ValueTaskFunctionOnIncompleteTask_RunsAfterTheResultIsAvailable()
    {
        // Arrange
        var source = new TaskCompletionSource<Result<int>>();
        var called = false;

        // Act
        var task = Run();
        var calledBeforeCompletion = called;
        source.SetResult(Result.FromValue(5));
        await task;

        // Assert
        calledBeforeCompletion.ShouldBeFalse();
        called.ShouldBeTrue();

        async Task Run()
        {
            var sut = source.Task;
            _ = await sut.MatchAsync(x => { called = true; return ValueTask.FromResult(x); }, e => ValueTask.FromResult(0));
        }
    }

    #endregion
}
