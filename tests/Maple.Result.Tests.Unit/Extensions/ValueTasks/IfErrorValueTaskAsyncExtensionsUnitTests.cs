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
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Extensions.ValueTasks;

public class IfErrorValueTaskAsyncExtensionsUnitTests
{
    #region IfErrorAsync (Result, Func<Error, ValueTask>)

    [Fact]
    public async Task IfErrorAsync_NoResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfErrorAsync(_ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? AsyncAction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? AsyncAction = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, ValueTask>>();

        var sut = Result.Success();

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = await sut.IfErrorAsync(_ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, ValueTask>>();

        var sut = GetErrorResult();

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.IfErrorAsync(_ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfErrorAsync (Result<T>, Func<Error, ValueTask>)

    [Fact]
    public async Task IfErrorAsync_NoGenericResultWithAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfErrorAsync(_ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultWithNoAction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 24;
        const Func<Error, ValueTask>? AsyncAction = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? AsyncAction = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultWithAction_DoesNotCallAction()
    {
        // Arrange
        const int InitialValue = 29;
        var actionMock = new Mock<Func<Error, ValueTask>>();

        Result<int> sut = InitialValue;

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultWithAction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 38;

        Result<int> sut = InitialValue;

        // Act
        var result = await sut.IfErrorAsync(_ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, ValueTask>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.IfErrorAsync(_ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfErrorAsync (ValueTask<Result>, Func<Error, ValueTask>)

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultValueTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? AsyncAction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultValueTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? AsyncAction = null;

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultValueTaskWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, ValueTask>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultValueTaskWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var successResult = Result.Success();
        var sut = ValueTask.FromResult(successResult);

        // Act
        var result = await sut.IfErrorAsync(_ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successResult);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultValueTaskWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, ValueTask>>();

        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultValueTaskWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.IfErrorAsync(_ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(errorResult);
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    #endregion

    #region IfErrorAsync (ValueTask<Result<T>>, Func<Error, ValueTask>)

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultValueTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 24;
        const Func<Error, ValueTask>? AsyncAction = null;

        var sut = ValueTask.FromResult(Result.FromValue(InitialValue));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultValueTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? AsyncAction = null;

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultValueTaskWithAction_DoesNotCallAction()
    {
        // Arrange
        const int InitialValue = 29;
        var actionMock = new Mock<Func<Error, ValueTask>>();

        var sut = ValueTask.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultValueTaskWithAction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 38;

        var resultValue = Result.FromValue(InitialValue);
        var sut = ValueTask.FromResult(resultValue);

        // Act
        var result = await sut.IfErrorAsync(_ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(resultValue);
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultValueTaskWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, ValueTask>>();

        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultValueTaskWithAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.IfErrorAsync(_ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(errorResult);
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    #endregion

    #region helper methods

    private static Result GetErrorResult()
    {
        return Error.Failure(ErrorUri.None, "4b3612e7-d22a-45f2-9a5c-4a8c14d39141", "Error title 1");
    }

    private static Result<T> GetErrorResult<T>()
    {
        return Error.Failure(ErrorUri.None, "ff7711a7-c224-4280-816c-1722864177cf", "Error title 2");
    }

    #endregion

    #region ValueTask results with synchronous delegates

    // See issue #214.

    private static readonly Error SyncTestError = Error.Failure(ErrorUri.None, "Synchronous delegate test error");

    [Fact]
    public async Task IfErrorAsync_FailedResultOfTTaskWithSyncAction_PassesErrorAndReturnsResult()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(SyncTestError));
        Error? received = null;

        // Act
        var result = await sut.IfErrorAsync(error => { received = error; });

        // Assert
        received.ShouldBeSameAs(SyncTestError);
        result.Error.ShouldBeSameAs(SyncTestError);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultOfTTaskWithSyncAction_DoesNotCallAction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(5));
        var called = false;

        // Act
        var result = await sut.IfErrorAsync(_ => { called = true; });

        // Assert
        called.ShouldBeFalse();
        result.Value.ShouldBe(5);
    }

    [Fact]
    public async Task IfErrorAsync_IncompleteTaskWithSynchronousDelegate_RunsDelegateAfterCompletion()
    {
        // Arrange
        var source = new TaskCompletionSource<Result<int>>();
        var called = false;
        var sut = new ValueTask<Result<int>>(source.Task);

        // Act
        var task = sut.IfErrorAsync(_ => { called = true; });
        var calledBeforeCompletion = called;
        source.SetResult(Result<int>.FromError(SyncTestError));
        await task;

        // Assert
        calledBeforeCompletion.ShouldBeFalse();
        called.ShouldBeTrue();
    }

    // A function that returns a Task on a ValueTask of a result is awaited by an overload of its own, which returns a ValueTask (#226).
    [Fact]
    public void IfErrorAsync_FunctionReturningTaskOnValueTaskResult_IsAnOverloadWithPriorityOne()
    {
        // Act
        var overloads = OverloadReflectionHelper.ValueTaskPartMethods(typeof(IfErrorAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && m.GetParameters().Where(p => p.ParameterType.Name.StartsWith("Func`", StringComparison.Ordinal))
                            .All(p => OverloadReflectionHelper.IsTaskFunction(p.ParameterType))
                        && m.GetParameters().Any(p => OverloadReflectionHelper.IsTaskFunction(p.ParameterType)))
            .ToList();

        // Assert
        overloads.Count.ShouldBe(1);
        overloads.ShouldAllBe(m => m.Name == "IfErrorAsync" && OverloadReflectionHelper.Priority(m) == 1);
    }

    // An async lambda binds to the overloads whose delegates return a Task, on every receiver (#226): on a ValueTask of a
    // result, they have priority 1, and those whose delegates return a ValueTask priority 0.
    [Fact]
    public void IfErrorAsync_OverloadsWithTaskDelegatesOnValueTaskResults_HavePriorityOne()
    {
        // Act
        var priorities = OverloadReflectionHelper.ValueTaskPartMethods(typeof(IfErrorAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && OverloadReflectionHelper.IsTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadReflectionHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(1);
        priorities.ShouldAllBe(p => p == 1);
    }

    [Fact]
    public void IfErrorAsync_OverloadsWithValueTaskDelegatesOnValueTaskResults_HavePriorityZero()
    {
        // Act
        var priorities = OverloadReflectionHelper.ValueTaskPartMethods(typeof(IfErrorAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && OverloadReflectionHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadReflectionHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(2);
        priorities.ShouldAllBe(p => p == 0);
    }

    [Fact]
    public void IfErrorAsync_SynchronousOverloadsOnValueTaskResults_HavePriorityZero()
    {
        // Act
        var priorities = OverloadReflectionHelper.ValueTaskPartMethods(typeof(IfErrorAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && !OverloadReflectionHelper.IsTaskFunction(m.GetParameters()[1].ParameterType)
                        && !OverloadReflectionHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadReflectionHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(1);
        priorities.ShouldAllBe(p => p == 0);
    }

    #endregion

    #region ValueTask results with functions returning a Task

    // A function that returns the other kind of task is awaited, and the receiver decides the kind (#226).

    private static readonly Error OtherKindError = Error.Failure(ErrorUri.None, "Other kind of task test error");

    [Fact]
    public async Task IfErrorAsync_FailedResultOfTValueTaskWithTaskAction_CallsActionAndReturnsResultUnchanged()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(OtherKindError));
        Error? passedError = null;

        // Act
        var result = await sut.IfErrorAsync(e => { passedError = e; return Task.CompletedTask; });

        // Assert
        result.Error.ShouldBeSameAs(OtherKindError);
        passedError.ShouldBeSameAs(OtherKindError);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultOfTValueTaskWithTaskAction_DoesNotCallAction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(5));
        Error? passedError = null;

        // Act
        var result = await sut.IfErrorAsync(e => { passedError = e; return Task.CompletedTask; });

        // Assert
        result.Value.ShouldBe(5);
        passedError.ShouldBeNull();
    }

    [Fact]
    public async Task IfErrorAsync_TaskFunctionOnIncompleteValueTask_RunsAfterTheResultIsAvailable()
    {
        // Arrange
        var source = new TaskCompletionSource<Result<int>>();
        var called = false;

        // Act
        var task = Run();
        var calledBeforeCompletion = called;
        source.SetResult(Result<int>.FromError(OtherKindError));
        await task;

        // Assert
        calledBeforeCompletion.ShouldBeFalse();
        called.ShouldBeTrue();

        async Task Run()
        {
            var sut = new ValueTask<Result<int>>(source.Task);
            _ = await sut.IfErrorAsync(e => { called = true; return Task.CompletedTask; });
        }
    }

    #endregion

    #region Functions returning a ValueTask with a value

    // See issue #289. A function that returns a ValueTask<TValue> is awaited, and its value is ignored.

    private static readonly Error ValueFunctionError = Error.Failure(ErrorUri.None, "Value function test error");

    [Fact]
    public async Task IfErrorAsync_FailedResultOfTWithValueFunction_PassesErrorAndReturnsResult()
    {
        // Arrange
        var sut = Result<int>.FromError(ValueFunctionError);
        Error? passedError = null;

        // Act
        var result = await sut.IfErrorAsync(e => { passedError = e; return new ValueTask<string>("ignored"); });

        // Assert
        result.ShouldBeSameAs(sut);
        passedError.ShouldBeSameAs(ValueFunctionError);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultOfTWithValueFunction_DoesNotCallFunction()
    {
        // Arrange
        var sut = Result.FromValue(5);
        var calls = 0;

        // Act
        var result = await sut.IfErrorAsync(e => new ValueTask<int>(++calls));

        // Assert
        result.ShouldBeSameAs(sut);
        calls.ShouldBe(0);
    }

    [Fact]
    public async Task IfErrorAsync_FailedResultWithValueFunction_PassesErrorAndReturnsResult()
    {
        // Arrange
        var sut = Result.FromError(ValueFunctionError);
        Error? passedError = null;

        // Act
        var result = await sut.IfErrorAsync(e => { passedError = e; return new ValueTask<int>(1); });

        // Assert
        result.ShouldBeSameAs(sut);
        passedError.ShouldBeSameAs(ValueFunctionError);
    }

    [Fact]
    public async Task IfErrorAsync_ResultOfTWithIncompleteValueFunction_CompletesAfterTheFunction()
    {
        // Arrange
        var sut = Result<int>.FromError(ValueFunctionError);
        var gate = new TaskCompletionSource<int>();

        // Act
        var task = sut.IfErrorAsync(e => new ValueTask<int>(gate.Task)).AsTask();
        var completedBeforeTheFunction = task.IsCompleted;
        gate.SetResult(7);
        var result = await task;

        // Assert
        completedBeforeTheFunction.ShouldBeFalse();
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public async Task IfErrorAsync_ResultOfTWithFailingValueFunction_ThrowsTheException()
    {
        // Arrange
        var sut = Result<int>.FromError(ValueFunctionError);

        // Act
        var exception = await Record.ExceptionAsync(()
            => sut.IfErrorAsync(e => ValueTask.FromException<int>(new FormatException("Value function failed"))).AsTask());

        // Assert
        exception.ShouldBeOfType<FormatException>();
        var formatException = exception as FormatException;
        formatException.ShouldNotBeNull();
        formatException.Message.ShouldBe("Value function failed");
    }

    [Fact]
    public async Task IfErrorAsync_FailedResultOfTValueTaskWithValueFunction_PassesErrorAndReturnsResult()
    {
        // Arrange
        var source = Result<int>.FromError(ValueFunctionError);
        var sut = ValueTask.FromResult(source);
        Error? passedError = null;

        // Act
        var result = await sut.IfErrorAsync(e => { passedError = e; return new ValueTask<string>("ignored"); });

        // Assert
        result.ShouldBeSameAs(source);
        passedError.ShouldBeSameAs(ValueFunctionError);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultOfTValueTaskWithValueFunction_DoesNotCallFunction()
    {
        // Arrange
        var source = Result.FromValue(5);
        var sut = ValueTask.FromResult(source);
        var calls = 0;

        // Act
        var result = await sut.IfErrorAsync(e => new ValueTask<int>(++calls));

        // Assert
        result.ShouldBeSameAs(source);
        calls.ShouldBe(0);
    }

    [Fact]
    public async Task IfErrorAsync_ResultOfTValueTaskWithIncompleteValueFunction_CompletesAfterTheFunction()
    {
        // Arrange
        var source = Result<int>.FromError(ValueFunctionError);
        var sut = ValueTask.FromResult(source);
        var gate = new TaskCompletionSource<int>();

        // Act
        var task = sut.IfErrorAsync(e => new ValueTask<int>(gate.Task)).AsTask();
        var completedBeforeTheFunction = task.IsCompleted;
        gate.SetResult(7);
        var result = await task;

        // Assert
        completedBeforeTheFunction.ShouldBeFalse();
        result.ShouldBeSameAs(source);
    }

    [Fact]
    public async Task IfErrorAsync_ResultOfTValueTaskWithFailingValueFunction_ThrowsTheException()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(ValueFunctionError));

        // Act
        var exception = await Record.ExceptionAsync(()
            => sut.IfErrorAsync(e => ValueTask.FromException<int>(new FormatException("Value function failed"))).AsTask());

        // Assert
        exception.ShouldBeOfType<FormatException>();
        var formatException = exception as FormatException;
        formatException.ShouldNotBeNull();
        formatException.Message.ShouldBe("Value function failed");
    }

    #endregion
}
