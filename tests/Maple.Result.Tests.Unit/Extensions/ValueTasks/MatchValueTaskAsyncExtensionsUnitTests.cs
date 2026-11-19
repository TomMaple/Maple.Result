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
using System.Linq;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Extensions.ValueTasks;

public class MatchValueTaskAsyncExtensionsUnitTests
{
    private static readonly Error SourceError = Error.Failure(ErrorUri.None, "50f1c1fb-560e-4bb3-84da-034072df7ca2", "Error title 6");

    #region MatchAsync (ValueTask<Result>, Func<TOut>, Func<Error, TOut>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithFunctions_ReturnsValueOfSuccessFunction()
    {
        // Arrange
        var errorCalled = false;
        var sut = ValueTask.FromResult(Result.Success());

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
    public async Task MatchAsync_FailedResultValueTaskWithFunctions_ReturnsValueOfErrorFunctionCalledWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = ValueTask.FromResult(Result.FromError(SourceError));

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
    public async Task MatchAsync_ResultValueTaskReturningNull_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = ValueTask.FromResult<Result>(null!);

        // Act
        var exception = await Record.ExceptionAsync(async () => await sut.MatchAsync(() => 1, _ => 0));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task MatchAsync_ResultValueTaskWithFunctionReturningNullResult_ReturnsNull()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => (Result)null!, e => e);

        // Assert
        result.ShouldBeNull();
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask<TOut>>, Func<Error, ValueTask<TOut>>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithValueTaskFunctions_ReturnsValueOfSuccessFunction()
    {
        // Arrange
        var errorCalled = false;
        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(async () =>
        {
            await Task.Yield();

            return "success";
        }, _ =>
        {
            errorCalled = true;

            return ValueTask.FromResult("error");
        });

        // Assert
        result.ShouldBe("success");
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultValueTaskWithValueTaskFunctions_ReturnsValueOfErrorFunctionCalledWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = ValueTask.FromResult(Result.FromError(SourceError));

        // Act
        var result = await sut.MatchAsync(() =>
        {
            successCalled = true;

            return ValueTask.FromResult("success");
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
    public async Task MatchAsync_ResultValueTaskWithValueTaskFunctionReturningNullResult_ReturnsNull()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult<Result>(null!), e => ValueTask.FromResult<Result>(e));

        // Assert
        result.ShouldBeNull();
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithValueTaskActions_AwaitsOnlySuccessFunction()
    {
        // Arrange
        var successCompleted = false;
        var errorCalled = false;
        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(async () =>
        {
            await Task.Yield();
            successCompleted = true;
        }, _ =>
        {
            errorCalled = true;

            return ValueTask.CompletedTask;
        });

        // Assert
        successCompleted.ShouldBeTrue();
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultValueTaskWithValueTaskActions_AwaitsOnlyErrorFunctionWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = ValueTask.FromResult(Result.FromError(SourceError));

        // Act
        await sut.MatchAsync(() =>
        {
            successCalled = true;

            return ValueTask.CompletedTask;
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

    #region MatchAsync (ValueTask<Result<T>>, Func<T, TOut>, Func<Error, TOut>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTValueTaskWithFunctions_ReturnsValueOfSuccessFunctionCalledWithTheValue()
    {
        // Arrange
        var errorCalled = false;
        var sut = ValueTask.FromResult(Result.FromValue(5));

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
    public async Task MatchAsync_FailedResultOfTValueTaskWithFunctions_ReturnsValueOfErrorFunctionCalledWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = ValueTask.FromResult(Result<int>.FromError(SourceError));

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
    public async Task MatchAsync_FailedResultOfTValueTaskWithErrorFunctionReturningTheError_ReturnsFailedResult()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(SourceError));

        // Act
        var result = await sut.MatchAsync(x => Result.FromValue(x.ToString()), e => e);

        // Assert
        result.Error.ShouldBeSameAs(SourceError);
    }

    [Fact]
    public async Task MatchAsync_ResultOfTValueTaskReturningNull_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = ValueTask.FromResult<Result<int>>(null!);

        // Act
        var exception = await Record.ExceptionAsync(async () => await sut.MatchAsync(x => x, _ => 0));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    #endregion

    #region MatchAsync (ValueTask<Result<T>>, Func<T, ValueTask<TOut>>, Func<Error, ValueTask<TOut>>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTValueTaskWithValueTaskFunctions_ReturnsValueOfSuccessFunctionCalledWithTheValue()
    {
        // Arrange
        var errorCalled = false;
        var sut = ValueTask.FromResult(Result.FromValue(5));

        // Act
        var result = await sut.MatchAsync(async x =>
        {
            await Task.Yield();

            return $"value {x}";
        }, _ =>
        {
            errorCalled = true;

            return ValueTask.FromResult("error");
        });

        // Assert
        result.ShouldBe("value 5");
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTValueTaskWithValueTaskFunctions_ReturnsValueOfErrorFunctionCalledWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = ValueTask.FromResult(Result<int>.FromError(SourceError));

        // Act
        var result = await sut.MatchAsync(x =>
        {
            successCalled = true;

            return ValueTask.FromResult($"value {x}");
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
    public async Task MatchAsync_ResultOfTValueTaskWithValueTaskFunctionThrowing_PropagatesTheException()
    {
        // Arrange
        var thrown = new InvalidOperationException("success function");
        var sut = ValueTask.FromResult(Result.FromValue(5));

        // Act
        var exception = await Record.ExceptionAsync(async () => await sut.MatchAsync<int, int>(_ => throw thrown, _ => ValueTask.FromResult(0)));

        // Assert
        exception.ShouldBeSameAs(thrown);
    }

    #endregion

    #region MatchAsync (ValueTask<Result<T>>, Func<T, ValueTask>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTValueTaskWithValueTaskActions_AwaitsOnlySuccessFunctionWithTheValue()
    {
        // Arrange
        int? passedValue = null;
        var errorCalled = false;
        var sut = ValueTask.FromResult(Result.FromValue(5));

        // Act
        await sut.MatchAsync(async x =>
        {
            await Task.Yield();
            passedValue = x;
        }, _ =>
        {
            errorCalled = true;

            return ValueTask.CompletedTask;
        });

        // Assert
        passedValue.ShouldBe(5);
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTValueTaskWithValueTaskActions_AwaitsOnlyErrorFunctionWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = ValueTask.FromResult(Result<int>.FromError(SourceError));

        // Act
        await sut.MatchAsync(_ =>
        {
            successCalled = true;

            return ValueTask.CompletedTask;
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

    #region MatchAsync overloads and their priority

    // A function that returns a Task on a ValueTask of a result is awaited by an overload of its own, which returns a ValueTask (#226).
    [Fact]
    public void MatchAsync_FunctionReturningTaskOnValueTaskResult_IsAnOverloadWithPriorityOne()
    {
        // Act
        var overloads = OverloadReflectionHelper.ValueTaskPartMethods(typeof(MatchAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && m.GetParameters().Where(p => p.ParameterType.Name.StartsWith("Func`", StringComparison.Ordinal))
                            .All(p => OverloadReflectionHelper.IsTaskFunction(p.ParameterType))
                        && m.GetParameters().Any(p => OverloadReflectionHelper.IsTaskFunction(p.ParameterType)))
            .ToList();

        // Assert
        overloads.Count.ShouldBe(4);
        overloads.ShouldAllBe(m => m.Name == "MatchAsync" && OverloadReflectionHelper.Priority(m) == 1);
    }

    [Fact]
    public void MatchAsync_AllMethods_AreOverloadsOnAValueTask()
    {
        // Act
        var methods = OverloadReflectionHelper.ValueTaskPartMethods(typeof(MatchAsyncExtensions)).ToList();

        // Assert
        // The overloads, and those for the other kind of task (#226).
        methods.Count.ShouldBe(6 + 4);
        methods.ShouldAllBe(m => m.Name == "MatchAsync" && m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(ValueTask<>));
    }

    // An async lambda binds to the overloads whose delegates return a Task, on every receiver (#226): on a ValueTask of a
    // result, they have priority 1, and those whose delegates return a ValueTask priority 0.
    [Fact]
    public void MatchAsync_OverloadsWithTaskFunctions_HavePriorityOne()
    {
        // Act
        var overloads = OverloadReflectionHelper.ValueTaskPartMethods(typeof(MatchAsyncExtensions))
            .Where(m => OverloadReflectionHelper.IsTaskFunction(m.GetParameters()[1].ParameterType))
            .ToList();

        // Assert
        overloads.Count.ShouldBe(4);
        overloads.ShouldAllBe(m => OverloadReflectionHelper.Priority(m) == 1);
    }

    [Fact]
    public void MatchAsync_OverloadsWithValueTaskFunctions_HavePriorityZero()
    {
        // Act
        var overloads = OverloadReflectionHelper.ValueTaskPartMethods(typeof(MatchAsyncExtensions))
            .Where(m => OverloadReflectionHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .ToList();

        // Assert
        overloads.Count.ShouldBe(4);
        overloads.ShouldAllBe(m => OverloadReflectionHelper.Priority(m) == 0);
    }

    #endregion

    #region ValueTask results with functions returning a Task

    // A function that returns the other kind of task is awaited, and the receiver decides the kind (#226).

    private static readonly Error OtherKindError = Error.Failure(ErrorUri.None, "Other kind of task test error");

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithTaskFunctions_RunsTheSuccessFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => Task.FromResult(1), e => Task.FromResult(0));

        // Assert
        result.ShouldBe(1);
    }

    [Fact]
    public async Task MatchAsync_FailedResultValueTaskWithTaskFunctions_RunsTheErrorFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromError(OtherKindError));

        // Act
        var result = await sut.MatchAsync(() => Task.FromResult(1), e => Task.FromResult(0));

        // Assert
        result.ShouldBe(0);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithTaskActions_RunsTheSuccessFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.Success());
        var log = "";

        // Act
        await sut.MatchAsync(() => { log += "success"; return Task.CompletedTask; }, e => { log += "error"; return Task.CompletedTask; });

        // Assert
        log.ShouldBe("success");
    }

    [Fact]
    public async Task MatchAsync_FailedResultValueTaskWithTaskActions_RunsTheErrorFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromError(OtherKindError));
        var log = "";

        // Act
        await sut.MatchAsync(() => { log += "success"; return Task.CompletedTask; }, e => { log += "error"; return Task.CompletedTask; });

        // Assert
        log.ShouldBe("error");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTValueTaskWithTaskFunctions_RunsTheSuccessFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(5));

        // Act
        var result = await sut.MatchAsync(x => Task.FromResult(x * 2), e => Task.FromResult(0));

        // Assert
        result.ShouldBe(10);
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTValueTaskWithTaskFunctions_RunsTheErrorFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(OtherKindError));

        // Act
        var result = await sut.MatchAsync(x => Task.FromResult(x * 2), e => Task.FromResult(0));

        // Assert
        result.ShouldBe(0);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTValueTaskWithTaskActions_RunsTheSuccessFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(5));
        var log = "";

        // Act
        await sut.MatchAsync(x => { log += x; return Task.CompletedTask; }, e => { log += "error"; return Task.CompletedTask; });

        // Assert
        log.ShouldBe("5");
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTValueTaskWithTaskActions_RunsTheErrorFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(OtherKindError));
        var log = "";

        // Act
        await sut.MatchAsync(x => { log += x; return Task.CompletedTask; }, e => { log += "error"; return Task.CompletedTask; });

        // Assert
        log.ShouldBe("error");
    }

    [Fact]
    public async Task MatchAsync_TaskFunctionOnIncompleteValueTask_RunsAfterTheResultIsAvailable()
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
            var sut = new ValueTask<Result<int>>(source.Task);
            _ = await sut.MatchAsync(x => { called = true; return Task.FromResult(x); }, e => Task.FromResult(0));
        }
    }

    #endregion
}
