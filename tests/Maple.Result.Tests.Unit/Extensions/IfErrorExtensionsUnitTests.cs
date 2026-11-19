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
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Extensions;

public class IfErrorExtensionsUnitTests
{
    #region IfError (Result, Action<Error>)

    [Fact]
    public void IfError_NoResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.IfError(_ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessfulResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Action<Error>? Action = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.IfError(Action!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_ErrorResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Action<Error>? Action = null;

        var sut = GetErrorResult();

        // Act
        var exception = Record.Exception(() => sut.IfError(Action!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessfulResultWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Action<Error>>();

        var sut = Result.Success();

        // Act
        sut.IfError(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void IfError_SuccessfulResultWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = sut.IfError(_ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void IfError_ErrorResultWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Action<Error>>();

        var sut = GetErrorResult();

        // Act
        sut.IfError(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void IfError_ErrorResultWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.IfError(_ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfError (Result<T>, Action<Error>)

    [Fact]
    public void IfError_NoGenericResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.IfError(_ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessfulGenericResultWithNoAction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 24;
        const Action<Error>? Action = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = Record.Exception(() => sut.IfError(Action!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_ErrorGenericResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Action<Error>? Action = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = Record.Exception(() => sut.IfError(Action!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessfulGenericResultWithAction_DoesNotCallAction()
    {
        // Arrange
        const int InitialValue = 29;
        var actionMock = new Mock<Action<Error>>();

        Result<int> sut = InitialValue;

        // Act
        sut.IfError(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void IfError_SuccessfulGenericResultWithAction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 38;

        Result<int> sut = InitialValue;

        // Act
        var result = sut.IfError(_ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public void IfError_ErrorGenericResultWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Action<Error>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.IfError(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void IfError_ErrorGenericResultWithAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = sut.IfError(_ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfError with an error action discarding a task

    [Fact]
    public void IfError_FailedResultWithStatementLambdaDiscardingTask_CallsErrorAction()
    {
        // Arrange
        var sut = GetErrorResult();
        var taskSource = new TaskCompletionSource();
        Error? passedError = null;

        // Act
        var result = sut.IfError(e =>
        {
            passedError = e;
            _ = taskSource.Task;
        });

        // Assert
        result.ShouldBeSameAs(sut);
        passedError.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region helper methods

    private static Result GetErrorResult()
    {
        return Error.Failure(ErrorUri.None, "d10e75ee-2687-49c2-9641-fd8b86e07718", "Error title 1");
    }

    private static Result<T> GetErrorResult<T>()
    {
        return Error.Failure(ErrorUri.None, "f2082bda-b481-4f4b-974d-1faccec44be4", "Error title 2");
    }

    private static Error GetReplacementError()
    {
        return Error.Failure(ErrorUri.None, "a73d2604-e540-4dd3-8db8-3e6a70f89d72", "Replacement error");
    }

    #endregion
}
