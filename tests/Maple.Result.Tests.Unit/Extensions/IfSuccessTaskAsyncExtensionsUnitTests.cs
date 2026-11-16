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
using Moq;
using System;
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

}
