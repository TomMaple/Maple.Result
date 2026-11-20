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

namespace Maple.Results.Tests.Unit.Extensions;

public class IfErrorTaskAsyncExtensionsUnitTests
{
    #region IfErrorAsync (Result, Func<Error, Task>)

    [Fact]
    public async Task IfErrorAsync_NoResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfErrorAsync(_ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? AsyncAction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? AsyncAction = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, Task>>();

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
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, Task>>();

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
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfErrorAsync (Result<T>, Func<Error, Task>)

    [Fact]
    public async Task IfErrorAsync_NoGenericResultWithAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfErrorAsync(_ => Task.CompletedTask));

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
        const Func<Error, Task>? AsyncAction = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? AsyncAction = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

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
        var actionMock = new Mock<Func<Error, Task>>();

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
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

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
        var actionMock = new Mock<Func<Error, Task>>();

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
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfErrorAsync (Task<Result>, Func<Error, Task>)

    [Fact]
    public async Task IfErrorAsync_NoResultTaskWithAction_ThrowsException()
    {
        // Arrange
        const Task<Result>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfErrorAsync(_ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? AsyncAction = null;

        var sut = Task.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? AsyncAction = null;

        var sut = Task.FromResult(GetErrorResult());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultTaskWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, Task>>();

        var sut = Task.FromResult(Result.Success());

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultTaskWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var successResult = Result.Success();
        var sut = Task.FromResult(successResult);

        // Act
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successResult);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultTaskWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, Task>>();

        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultTaskWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(errorResult);
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    #endregion

    #region IfErrorAsync (Task<Result<T>>, Func<Error, Task>)

    [Fact]
    public async Task IfErrorAsync_NoGenericResultTaskWithAction_ThrowsException()
    {
        // Arrange
        const Task<Result<int>>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfErrorAsync(_ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 24;
        const Func<Error, Task>? AsyncAction = null;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? AsyncAction = null;

        var sut = Task.FromResult(GetErrorResult<int>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultTaskWithAction_DoesNotCallAction()
    {
        // Arrange
        const int InitialValue = 29;
        var actionMock = new Mock<Func<Error, Task>>();

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultTaskWithAction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 38;

        var resultValue = Result.FromValue(InitialValue);
        var sut = Task.FromResult(resultValue);

        // Act
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(resultValue);
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultTaskWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, Task>>();

        var errorResult = GetErrorResult<int>();
        var sut = Task.FromResult(errorResult);

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultTaskWithAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

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

    #region Task results with synchronous delegates

    // See issue #214.

    private static readonly Error SyncTestError = Error.Failure(ErrorUri.None, "Synchronous delegate test error");

    [Fact]
    public async Task IfErrorAsync_FailedResultOfTTaskWithSyncAction_PassesErrorAndReturnsResult()
    {
        // Arrange
        var sut = Task.FromResult(Result<int>.FromError(SyncTestError));
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
        var sut = Task.FromResult(Result.FromValue(5));
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
        var sut = source.Task;

        // Act
        var task = sut.IfErrorAsync(_ => { called = true; });
        var calledBeforeCompletion = called;
        source.SetResult(Result<int>.FromError(SyncTestError));
        await task;

        // Assert
        calledBeforeCompletion.ShouldBeFalse();
        called.ShouldBeTrue();
    }

    // A function that returns a ValueTask on a Task of a result is awaited by an overload of its own, which returns a Task (#226); so is one that
    // returns a ValueTask with a value (#289).
    [Fact]
    public void IfErrorAsync_FunctionReturningValueTaskOnTaskResult_IsAnOverloadWithPriorityZero()
    {
        // Act
        var overloads = OverloadReflectionHelper.TaskPartMethods(typeof(IfErrorAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(Task<>)
                        && m.GetParameters().Where(p => p.ParameterType.Name.StartsWith("Func`", StringComparison.Ordinal))
                            .All(p => OverloadReflectionHelper.IsValueTaskFunction(p.ParameterType))
                        && m.GetParameters().Any(p => OverloadReflectionHelper.IsValueTaskFunction(p.ParameterType)))
            .ToList();

        // Assert
        overloads.Count.ShouldBe(2);
        overloads.ShouldAllBe(m => m.Name == "IfErrorAsync" && OverloadReflectionHelper.Priority(m) == 0);
    }

    [Fact]
    public void IfErrorAsync_OverloadsWithTaskDelegatesOnTaskResults_HavePriorityOne()
    {
        // Act
        var priorities = OverloadReflectionHelper.TaskPartMethods(typeof(IfErrorAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(Task<>)
                        && OverloadReflectionHelper.IsTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadReflectionHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(1);
        priorities.ShouldAllBe(p => p == 1);
    }

    [Fact]
    public void IfErrorAsync_SynchronousOverloadsOnTaskResults_HavePriorityZero()
    {
        // Act
        var priorities = OverloadReflectionHelper.TaskPartMethods(typeof(IfErrorAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(Task<>)
                        && !OverloadReflectionHelper.IsTaskFunction(m.GetParameters()[1].ParameterType)
                        && !OverloadReflectionHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadReflectionHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(1);
        priorities.ShouldAllBe(p => p == 0);
    }

    #endregion

    #region Task results with functions returning a ValueTask

    // A function that returns the other kind of task is awaited, and the receiver decides the kind (#226).

    private static readonly Error OtherKindError = Error.Failure(ErrorUri.None, "Other kind of task test error");

    [Fact]
    public async Task IfErrorAsync_FailedResultOfTTaskWithValueTaskAction_CallsActionAndReturnsResultUnchanged()
    {
        // Arrange
        var sut = Task.FromResult(Result<int>.FromError(OtherKindError));
        Error? passedError = null;

        // Act
        var result = await sut.IfErrorAsync(e => { passedError = e; return ValueTask.CompletedTask; });

        // Assert
        result.Error.ShouldBeSameAs(OtherKindError);
        passedError.ShouldBeSameAs(OtherKindError);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultOfTTaskWithValueTaskAction_DoesNotCallAction()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromValue(5));
        Error? passedError = null;

        // Act
        var result = await sut.IfErrorAsync(e => { passedError = e; return ValueTask.CompletedTask; });

        // Assert
        result.Value.ShouldBe(5);
        passedError.ShouldBeNull();
    }

    [Fact]
    public async Task IfErrorAsync_ValueTaskFunctionOnIncompleteTask_RunsAfterTheResultIsAvailable()
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
            var sut = source.Task;
            _ = await sut.IfErrorAsync(e => { called = true; return ValueTask.CompletedTask; });
        }
    }

    #endregion

    #region Task results with functions returning a ValueTask with a value

    // See issue #289. A function that returns a ValueTask<TValue> is awaited, and its value is ignored.

    private static readonly Error ValueFunctionError = Error.Failure(ErrorUri.None, "Value function test error");

    [Fact]
    public async Task IfErrorAsync_FailedResultOfTTaskWithValueFunction_PassesErrorAndReturnsResult()
    {
        // Arrange
        var source = Result<int>.FromError(ValueFunctionError);
        var sut = Task.FromResult(source);
        Error? passedError = null;

        // Act
        var result = await sut.IfErrorAsync(e => { passedError = e; return new ValueTask<string>("ignored"); });

        // Assert
        result.ShouldBeSameAs(source);
        passedError.ShouldBeSameAs(ValueFunctionError);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultOfTTaskWithValueFunction_DoesNotCallFunction()
    {
        // Arrange
        var source = Result.FromValue(5);
        var sut = Task.FromResult(source);
        var calls = 0;

        // Act
        var result = await sut.IfErrorAsync(e => new ValueTask<int>(++calls));

        // Assert
        result.ShouldBeSameAs(source);
        calls.ShouldBe(0);
    }

    [Fact]
    public async Task IfErrorAsync_ResultOfTTaskWithIncompleteValueFunction_CompletesAfterTheFunction()
    {
        // Arrange
        var source = Result<int>.FromError(ValueFunctionError);
        var sut = Task.FromResult(source);
        var gate = new TaskCompletionSource<int>();

        // Act
        var task = sut.IfErrorAsync(e => new ValueTask<int>(gate.Task));
        var completedBeforeTheFunction = task.IsCompleted;
        gate.SetResult(7);
        var result = await task;

        // Assert
        completedBeforeTheFunction.ShouldBeFalse();
        result.ShouldBeSameAs(source);
    }

    [Fact]
    public async Task IfErrorAsync_ResultOfTTaskWithFailingValueFunction_ThrowsTheException()
    {
        // Arrange
        var sut = Task.FromResult(Result<int>.FromError(ValueFunctionError));

        // Act
        var exception = await Record.ExceptionAsync(()
            => sut.IfErrorAsync(e => ValueTask.FromException<int>(new FormatException("Value function failed"))));

        // Assert
        exception.ShouldBeOfType<FormatException>();
        var formatException = exception as FormatException;
        formatException.ShouldNotBeNull();
        formatException.Message.ShouldBe("Value function failed");
    }

    #endregion
}
