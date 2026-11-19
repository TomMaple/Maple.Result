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

public class IfSuccessValueTaskAsyncExtensionsUnitTests
{
    #region IfSuccessAsync (Result, Func<ValueTask>)

    [Fact]
    public async Task IfSuccessAsync_NoResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(() => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask>? ActionAsync = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask>? ActionAsync = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<ValueTask>>();

        var sut = Result.Success();

        // Act
        await sut.IfSuccessAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = await sut.IfSuccessAsync(() => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Func<ValueTask>>();

        var sut = GetErrorResult();

        // Act
        await sut.IfSuccessAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.IfSuccessAsync(() => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, ValueTask>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultWithGenericAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(_ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithNoGenericAction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 35;
        const Func<int, ValueTask>? ActionAsync = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithNoGenericAction_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask>? ActionAsync = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithGenericAction_CallsAction()
    {
        // Arrange
        const int InitialValue = 35;
        var functionMock = new Mock<Func<int, ValueTask>>();

        Result<int> sut = InitialValue;

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithGenericAction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 35;

        Result<int> sut = InitialValue;

        // Act
        var result = await sut.IfSuccessAsync(_ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithGenericAction_DoesNotCallAction()
    {
        // Arrange
        var functionMock = new Mock<Func<int, ValueTask>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithGenericAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.IfSuccessAsync(_ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (ValueTask<Result>, Func<ValueTask>)

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultValueTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask>? ActionAsync = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultValueTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask>? ActionAsync = null;

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultValueTaskWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<ValueTask>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.IfSuccessAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultValueTaskWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var initialResult = Result.Success();
        var sut = ValueTask.FromResult(initialResult);

        // Act
        var result = await sut.IfSuccessAsync(() => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBeSameAs(initialResult);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultValueTaskWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Func<ValueTask>>();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        await sut.IfSuccessAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultValueTaskWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.IfSuccessAsync(() => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync (ValueTask<Result<T>>, Func<T, ValueTask>)

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultValueTaskWithNoGenericAction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 35;
        const Func<int, ValueTask>? ActionAsync = null;

        var sut = ValueTask.FromResult(Result.FromValue(InitialValue));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultValueTaskWithNoGenericAction_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask>? ActionAsync = null;

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultValueTaskWithGenericAction_CallsAction()
    {
        // Arrange
        const int InitialValue = 35;
        var functionMock = new Mock<Func<int, ValueTask>>();

        var sut = ValueTask.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultValueTaskWithGenericAction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 35;

        var sut = ValueTask.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.IfSuccessAsync(_ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultValueTaskWithGenericAction_DoesNotCallAction()
    {
        // Arrange
        var functionMock = new Mock<Func<int, ValueTask>>();

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultValueTaskWithGenericAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.IfSuccessAsync(_ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region helper methods

    private static Error GetError()
    {
        return Error.Failure(ErrorUri.None, "e06d75de-115c-46ec-a5f6-f6373007ec61", "Error title 1");
    }

    private static Result GetErrorResult()
    {
        return Error.Failure(ErrorUri.None, "636d213a-2dc4-4f63-adaf-bbfb80d888ab", "Error title 3");
    }

    private static Result<T> GetErrorResult<T>()
    {
        return Error.Failure(ErrorUri.None, "a3ef9d52-c18d-4707-8ef3-f7e25072224f", "Error title 4");
    }

    #endregion

    #region ValueTask results with synchronous delegates

    // See issue #214.

    private static readonly Error SyncTestError = Error.Failure(ErrorUri.None, "Synchronous delegate test error");
    private static readonly Error SyncOtherError = Error.Conflict(ErrorUri.None, "Synchronous delegate other error");

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithSyncAction_CallsActionAndReturnsResult()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.Success());
        var called = false;

        // Act
        var result = await sut.IfSuccessAsync(() => { called = true; });

        // Assert
        called.ShouldBeTrue();
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task IfSuccessAsync_FailedResultTaskWithSyncAction_DoesNotCallAction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromError(SyncTestError));
        var called = false;

        // Act
        var result = await sut.IfSuccessAsync(() => { called = true; });

        // Assert
        called.ShouldBeFalse();
        result.Error.ShouldBeSameAs(SyncTestError);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultOfTTaskWithSyncAction_PassesValueAndReturnsResult()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(5));
        var received = 0;

        // Act
        var result = await sut.IfSuccessAsync(value => { received = value; });

        // Assert
        received.ShouldBe(5);
        result.Value.ShouldBe(5);
    }

    [Fact]
    public async Task IfSuccessAsync_FailedResultOfTTaskWithSyncAction_DoesNotCallAction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(SyncTestError));
        var called = false;

        // Act
        var result = await sut.IfSuccessAsync(_ => { called = true; });

        // Assert
        called.ShouldBeFalse();
        result.Error.ShouldBeSameAs(SyncTestError);
    }

    [Fact]
    public async Task IfSuccessAsync_IncompleteTaskWithSynchronousDelegate_RunsDelegateAfterCompletion()
    {
        // Arrange
        var source = new TaskCompletionSource<Result<int>>();
        var called = false;
        var sut = new ValueTask<Result<int>>(source.Task);

        // Act
        var task = sut.IfSuccessAsync(_ => { called = true; });
        var calledBeforeCompletion = called;
        source.SetResult(Result.FromValue(5));
        await task;

        // Assert
        calledBeforeCompletion.ShouldBeFalse();
        called.ShouldBeTrue();
    }

    // A function that returns a Task on a ValueTask of a result is awaited by an overload of its own, which returns a ValueTask (#226).
    [Fact]
    public void IfSuccessAsync_FunctionReturningTaskOnValueTaskResult_IsAnOverloadWithPriorityOne()
    {
        // Act
        var overloads = OverloadReflectionHelper.ValueTaskPartMethods(typeof(IfSuccessAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && m.GetParameters().Where(p => p.ParameterType.Name.StartsWith("Func`", StringComparison.Ordinal))
                            .All(p => OverloadReflectionHelper.IsTaskFunction(p.ParameterType))
                        && m.GetParameters().Any(p => OverloadReflectionHelper.IsTaskFunction(p.ParameterType)))
            .ToList();

        // Assert
        overloads.Count.ShouldBe(2);
        overloads.ShouldAllBe(m => m.Name == "IfSuccessAsync" && OverloadReflectionHelper.Priority(m) == 1);
    }

    // An async lambda binds to the overloads whose delegates return a Task, on every receiver (#226): on a ValueTask of a
    // result, they have priority 1, and those whose delegates return a ValueTask priority 0.
    [Fact]
    public void IfSuccessAsync_OverloadsWithTaskDelegatesOnValueTaskResults_HavePriorityOne()
    {
        // Act
        var priorities = OverloadReflectionHelper.ValueTaskPartMethods(typeof(IfSuccessAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && OverloadReflectionHelper.IsTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadReflectionHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(2);
        priorities.ShouldAllBe(p => p == 1);
    }

    [Fact]
    public void IfSuccessAsync_OverloadsWithValueTaskDelegatesOnValueTaskResults_HavePriorityZero()
    {
        // Act
        var priorities = OverloadReflectionHelper.ValueTaskPartMethods(typeof(IfSuccessAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && OverloadReflectionHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadReflectionHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(2);
        priorities.ShouldAllBe(p => p == 0);
    }

    [Fact]
    public void IfSuccessAsync_SynchronousOverloadsOnValueTaskResults_HavePriorityZero()
    {
        // Act
        var priorities = OverloadReflectionHelper.ValueTaskPartMethods(typeof(IfSuccessAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && !OverloadReflectionHelper.IsTaskFunction(m.GetParameters()[1].ParameterType)
                        && !OverloadReflectionHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadReflectionHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(2);
        priorities.ShouldAllBe(p => p == 0);
    }

    #endregion

    #region ValueTask results with functions returning a Task

    // A function that returns the other kind of task is awaited, and the receiver decides the kind (#226).

    private static readonly Error OtherKindError = Error.Failure(ErrorUri.None, "Other kind of task test error");

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultValueTaskWithTaskAction_CallsActionAndReturnsResultUnchanged()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.Success());
        var called = false;

        // Act
        var result = await sut.IfSuccessAsync(() => { called = true; return Task.CompletedTask; });

        // Assert
        result.IsSuccess.ShouldBeTrue();
        called.ShouldBeTrue();
    }

    [Fact]
    public async Task IfSuccessAsync_FailedResultValueTaskWithTaskAction_DoesNotCallAction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromError(OtherKindError));
        var called = false;

        // Act
        var result = await sut.IfSuccessAsync(() => { called = true; return Task.CompletedTask; });

        // Assert
        result.Error.ShouldBeSameAs(OtherKindError);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultOfTValueTaskWithTaskAction_CallsActionAndReturnsResultUnchanged()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(5));
        var passed = 0;

        // Act
        var result = await sut.IfSuccessAsync(x => { passed = x; return Task.CompletedTask; });

        // Assert
        result.Value.ShouldBe(5);
        passed.ShouldBe(5);
    }

    [Fact]
    public async Task IfSuccessAsync_FailedResultOfTValueTaskWithTaskAction_DoesNotCallAction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(OtherKindError));
        var passed = 0;

        // Act
        var result = await sut.IfSuccessAsync(x => { passed = x; return Task.CompletedTask; });

        // Assert
        result.Error.ShouldBeSameAs(OtherKindError);
        passed.ShouldBe(0);
    }

    [Fact]
    public async Task IfSuccessAsync_TaskFunctionOnIncompleteValueTask_RunsAfterTheResultIsAvailable()
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
            _ = await sut.IfSuccessAsync(x => { called = true; return Task.CompletedTask; });
        }
    }

    #endregion
}
