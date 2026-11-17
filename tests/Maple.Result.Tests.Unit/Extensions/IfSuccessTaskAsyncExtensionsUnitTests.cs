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

namespace Maple.Results.Tests.Unit.Extensions;

public class IfSuccessTaskAsyncExtensionsUnitTests
{
    #region IfSuccessAsync (Result, Func<Task>)

    [Fact]
    public async Task IfSuccessAsync_NoResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(() => new Task(() => { })));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Task>? ActionAsync = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Task>? ActionAsync = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Task>>();

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
        var result = await sut.IfSuccessAsync(() => Task.CompletedTask);

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
        var actionMock = new Mock<Func<Task>>();

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
        var result = await sut.IfSuccessAsync(() => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultWithGenericAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(_ => Task.CompletedTask));

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
        const Func<int, Task>? ActionAsync = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithNoGenericAction_ThrowsException()
    {
        // Arrange
        const Func<int, Task>? ActionAsync = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

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
        var functionMock = new Mock<Func<int, Task>>();

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
        var result = await sut.IfSuccessAsync(_ => Task.CompletedTask);

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
        var functionMock = new Mock<Func<int, Task>>();

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
        var result = await sut.IfSuccessAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result>, Func<Task>)

    [Fact]
    public async Task IfSuccessAsync_NoResultTaskWithAction_ThrowsException()
    {
        // Arrange
        const Task<Result>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(() => new Task(() => { })));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Task>? ActionAsync = null;

        var sut = Task.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Task>? ActionAsync = null;

        var sut = Task.FromResult(GetErrorResult());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Task>>();

        var sut = Task.FromResult(Result.Success());

        // Act
        await sut.IfSuccessAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var initialResult = Result.Success();
        var sut = Task.FromResult(initialResult);

        // Act
        var result = await sut.IfSuccessAsync(() => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBeSameAs(initialResult);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Task>>();

        var sut = Task.FromResult(GetErrorResult());

        // Act
        await sut.IfSuccessAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfSuccessAsync(() => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result<T>>, Func<T, Task>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultTaskWithGenericAction_ThrowsException()
    {
        // Arrange
        const Task<Result<int>>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(_ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithNoGenericAction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 35;
        const Func<int, Task>? ActionAsync = null;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithNoGenericAction_ThrowsException()
    {
        // Arrange
        const Func<int, Task>? ActionAsync = null;

        var sut = Task.FromResult(GetErrorResult<int>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithGenericAction_CallsAction()
    {
        // Arrange
        const int InitialValue = 35;
        var functionMock = new Mock<Func<int, Task>>();

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithGenericAction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 35;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithGenericAction_DoesNotCallAction()
    {
        // Arrange
        var functionMock = new Mock<Func<int, Task>>();

        var sut = Task.FromResult(GetErrorResult<int>());

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithGenericAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.CompletedTask);

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

    #region IfSuccessAsync with a function returning a value (overload trap)

    // A function that returns a value is a transformation: the side-effect method reports it as a compile error (CS0619) that
    // names the transforming method, instead of discarding the value (#197). The bindings are checked with a consumer project.

    [Fact]
    public void IfSuccessAsync_FunctionReturningValueOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = OverloadTrapHelper.TaskPartMethods(typeof(IfSuccessAsyncExtensions))
            .Where(m => OverloadTrapHelper.IsTrap(m, "The function returns a value in a task, which IfSuccessAsync discards"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(4);
        traps.ShouldAllBe(m => m.Name == "IfSuccessAsync"
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    #endregion

    #region Task results with synchronous delegates

    // See issue #214.

    private static readonly Error SyncTestError = Error.Failure(ErrorUri.None, "Synchronous delegate test error");
    private static readonly Error SyncOtherError = Error.Conflict(ErrorUri.None, "Synchronous delegate other error");

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithSyncAction_CallsActionAndReturnsResult()
    {
        // Arrange
        var sut = Task.FromResult(Result.Success());
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
        var sut = Task.FromResult(Result.FromError(SyncTestError));
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
        var sut = Task.FromResult(Result.FromValue(5));
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
        var sut = Task.FromResult(Result<int>.FromError(SyncTestError));
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
        var sut = source.Task;

        // Act
        var task = sut.IfSuccessAsync(_ => { called = true; });
        var calledBeforeCompletion = called;
        source.SetResult(Result.FromValue(5));
        await task;

        // Assert
        calledBeforeCompletion.ShouldBeFalse();
        called.ShouldBeTrue();
    }

    [Fact]
    public void IfSuccessAsync_SyncFunctionReturningValueOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = OverloadTrapHelper.TaskPartMethods(typeof(IfSuccessAsyncExtensions))
            .Where(m => OverloadTrapHelper.IsTrap(m, "The function returns a value, which IfSuccessAsync discards"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(2);
        traps.ShouldAllBe(m => m.Name == "IfSuccessAsync"
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    [Fact]
    public void IfSuccessAsync_FunctionReturningValueTaskOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = OverloadTrapHelper.TaskPartMethods(typeof(IfSuccessAsyncExtensions))
            .Where(m => OverloadTrapHelper.IsTrap(m, "The function returns a ValueTask, which IfSuccessAsync on a Task of a result does not await"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(4);
        traps.ShouldAllBe(m => m.Name == "IfSuccessAsync"
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    [Fact]
    public void IfSuccessAsync_OverloadsWithTaskDelegatesOnTaskResults_HavePriorityOne()
    {
        // Act
        var priorities = OverloadTrapHelper.TaskPartMethods(typeof(IfSuccessAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(Task<>)
                        && OverloadTrapHelper.IsTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadTrapHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(4);
        priorities.ShouldAllBe(p => p == 1);
    }

    [Fact]
    public void IfSuccessAsync_SynchronousOverloadsOnTaskResults_HavePriorityZero()
    {
        // Act
        var priorities = OverloadTrapHelper.TaskPartMethods(typeof(IfSuccessAsyncExtensions))
            .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is null
                        && m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(Task<>)
                        && !OverloadTrapHelper.IsTaskFunction(m.GetParameters()[1].ParameterType)
                        && !OverloadTrapHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadTrapHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(2);
        priorities.ShouldAllBe(p => p == 0);
    }

    #endregion
}
