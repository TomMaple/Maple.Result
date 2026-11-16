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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Extensions;

public class IfSuccessExtensionsUnitTests
{
    #region IfSuccess (Result, Action)

    [Fact]
    public void IfSuccess_NoResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.IfSuccess(() => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Action? Action = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Action!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Action? Action = null;

        var sut = GetErrorResult();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Action!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Action>();

        var sut = Result.Success();

        // Act
        sut.IfSuccess(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = sut.IfSuccess(() => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Action>();

        var sut = GetErrorResult();

        // Act
        sut.IfSuccess(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.IfSuccess(() => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccess (Result<T>, Action<T>)

    [Fact]
    public void IfSuccess_NoGenericResultWithGenericAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.IfSuccess(_ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithNoGenericAction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 35;
        const Action<int>? Action = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Action!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithNoGenericAction_ThrowsException()
    {
        // Arrange
        const Action<int>? Action = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Action!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithGenericAction_CallsAction()
    {
        // Arrange
        const int InitialValue = 35;
        var actionMock = new Mock<Action<int>>();

        Result<int> sut = InitialValue;

        // Act
        sut.IfSuccess(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithGenericAction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 35;

        Result<int> sut = InitialValue;

        // Act
        var result = sut.IfSuccess(_ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithGenericAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Action<int>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.IfSuccess(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithGenericAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = sut.IfSuccess(_ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
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

    #region IfSuccess with a function returning a task (overload trap, issue #119)

    [Fact]
    public void IfSuccess_FunctionReturningTaskOverloads_AreHiddenCompileErrors()
    {
        // Arrange
        var overloads = typeof(IfSuccessExtensions).GetMethods()
            .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is not null)
            .ToArray();

        // Act & Assert
        overloads.Length.ShouldBe(8);
        overloads.ShouldAllBe(m => m.Name == "IfSuccess"
                                   && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                                   && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithTaskFunctionAndExplicitTypeArgument_ReturnsResultWithTaskNotAwaited()
    {
        // Arrange
        var sut = Result.Success();
        var taskSource = new TaskCompletionSource();

        // Act
        Result<Task> result = sut.IfSuccess<Task>(() => taskSource.Task);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(taskSource.Task);
        result.Value.IsCompleted.ShouldBeFalse();
    }

    [Fact]
    public void IfSuccess_SuccessfulResultOfTWithTaskFunctionAndExplicitTypeArguments_ReturnsResultWithTaskNotAwaited()
    {
        // Arrange
        var sut = Result<int>.FromValue(5);
        var taskSource = new TaskCompletionSource();

        // Act
        Result<Task> result = sut.IfSuccess<int, Task>(_ => taskSource.Task);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(taskSource.Task);
        result.Value.IsCompleted.ShouldBeFalse();
    }

    #endregion
}
