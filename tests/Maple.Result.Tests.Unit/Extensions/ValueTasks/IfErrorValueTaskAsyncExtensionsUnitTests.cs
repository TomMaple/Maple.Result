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
using Maple.Results.Tests.Unit.TestingInfrastructure.Fakes;
using Maple.Results.Tests.Unit.TestingInfrastructure.Helpers;
using Moq;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

    private static Error GetReplacementError()
    {
        return Error.Failure(ErrorUri.None, "53e1e02c-3e21-47c9-9e1a-93ba22f03ba8", "Replacement error");
    }

    #endregion

    #region IfErrorAsync with a function returning a value (overload trap)

    // A function that returns a value is a transformation: the side-effect method reports it as a compile error (CS0619) that
    // names the transforming method, instead of discarding the value (#197). The bindings are checked with a consumer project.

    [Fact]
    public void IfErrorAsync_FunctionReturningValueOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = OverloadTrapHelper.ValueTaskPartMethods(typeof(IfErrorAsyncExtensions))
            .Where(m => OverloadTrapHelper.IsTrap(m, "The function returns a value in a task, which IfErrorAsync discards"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(2);
        traps.ShouldAllBe(m => m.Name == "IfErrorAsync"
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    #endregion

    #region ValueTask results with synchronous delegates

    // See issue #214.

    private static readonly Error SyncTestError = Error.Failure(ErrorUri.None, "Synchronous delegate test error");
    private static readonly Error SyncOtherError = Error.Conflict(ErrorUri.None, "Synchronous delegate other error");

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

    [Fact]
    public void IfErrorAsync_SyncFunctionReturningValueOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = OverloadTrapHelper.ValueTaskPartMethods(typeof(IfErrorAsyncExtensions))
            .Where(m => OverloadTrapHelper.IsTrap(m, "The function returns a value, which IfErrorAsync discards"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(1);
        traps.ShouldAllBe(m => m.Name == "IfErrorAsync"
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    [Fact]
    public void IfErrorAsync_FunctionReturningTaskOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = OverloadTrapHelper.ValueTaskPartMethods(typeof(IfErrorAsyncExtensions))
            .Where(m => OverloadTrapHelper.IsTrap(m, "The function returns a Task, which IfErrorAsync on a ValueTask of a result does not await"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(2);
        traps.ShouldAllBe(m => m.Name == "IfErrorAsync"
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    [Fact]
    public void IfErrorAsync_OverloadsWithValueTaskDelegatesOnValueTaskResults_HavePriorityOne()
    {
        // Act
        var priorities = OverloadTrapHelper.ValueTaskPartMethods(typeof(IfErrorAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && OverloadTrapHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadTrapHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(2);
        priorities.ShouldAllBe(p => p == 1);
    }

    [Fact]
    public void IfErrorAsync_SynchronousOverloadsOnValueTaskResults_HavePriorityZero()
    {
        // Act
        var priorities = OverloadTrapHelper.ValueTaskPartMethods(typeof(IfErrorAsyncExtensions))
            .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is null
                        && m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && !OverloadTrapHelper.IsTaskFunction(m.GetParameters()[1].ParameterType)
                        && !OverloadTrapHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadTrapHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(1);
        priorities.ShouldAllBe(p => p == 0);
    }

    #endregion
}
