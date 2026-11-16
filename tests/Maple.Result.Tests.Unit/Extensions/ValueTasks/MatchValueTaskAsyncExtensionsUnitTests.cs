// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Extensions.ValueTasks;
using Maple.Results.Tests.Unit.TestingInfrastructure.Fakes;
using Maple.Results.Tests.Unit.TestingInfrastructure.Helpers;
using Moq;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Extensions.ValueTasks;

public class MatchValueTaskAsyncExtensionsUnitTests
{
    #region MatchAsync (Result, Func<ValueTask>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_NoResultWithActions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception =
            await Record.ExceptionAsync(() => Sut!.MatchAsync(() => ValueTask.CompletedTask, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoSuccessActionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask>? SuccessAction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessAction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessActionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.CompletedTask, ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorActions_CallsSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<ValueTask>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorActions_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorActions_ReturnsOriginalResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = await sut.MatchAsync(() => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(sut);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorActions_DoesNotCallSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<ValueTask>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorActions_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorActions_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(() => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldBe(sut);
    }

    #endregion

    #region MatchAsync (Result, Func<ValueTask<Result>>, Func<Error, ValueTask<Result>>)

    [Fact]
    public async Task MatchAsync_NoResultWithSuccessAndErrorResultFunctions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(() => ValueTask.FromResult(Result.Success()), _ => ValueTask.FromResult(Result.Success())).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoSuccessResultFunctionAndErrorResultFunction_ThrowsException2()
    {
        // Arrange
        const Func<ValueTask<Result>>? SuccessFunction = null;

        var sut = Result.Success();

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.FromResult(Result.Success())).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessResultFunctionAndNoErrorResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result>>? ErrorFunction = null;

        var sut = Result.Success();

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorResultFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.Success()));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorResultFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorResultFunctions_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var successFunctionResult = Result.Success();

        var sut = Result.Success();

        // Act
        var result = await sut.MatchAsync(
            () => ValueTask.FromResult(successFunctionResult),
            _ => ValueTask.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successFunctionResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorResultFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.Success()));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorResultFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorResultFunctions_ReturnsErrorFunctionResult()
    {
        // Arrange
        var errorFunctionResult = Result.Success();

        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(
            () => ValueTask.FromResult(Result.Success()),
            _ => ValueTask.FromResult(errorFunctionResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(errorFunctionResult);
    }

    #endregion

    #region MatchAsync (Result, Func<ValueTask<T>>, Func<Error, ValueTask<T>>)

    [Fact]
    public async Task MatchAsync_NoResultWithGenericSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception =
            await Record.ExceptionAsync(() => Sut!.MatchAsync(() => ValueTask.FromResult(1), _ => ValueTask.FromResult(2)).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoGenericSuccessFunctionAndGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask<int>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(Function!, _ => ValueTask.FromResult(2)).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessFunctionAndNoGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<int>>? ErrorFunction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.FromResult(1), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<int>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(2));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<int>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(1), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessAndErrorFunctions_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int ExpectedValue = 58;

        var sut = Result.Success();

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(ExpectedValue), _ => ValueTask.FromResult(2));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<int>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(2));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<int>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(1), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericSuccessAndErrorFunctions_ReturnsErrorFunctionValue()
    {
        // Arrange
        const int ExpectedValue = 73;

        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(1), _ => ValueTask.FromResult(ExpectedValue));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    #endregion

    #region MatchAsync (Result, Func<ValueTask<Result<T>>>, Func<Error, ValueTask<Result<T>>>)

    [Fact]
    public async Task MatchAsync_NoResultWithGenericResultSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), _ => ValueTask.FromResult(Result.FromValue(2))).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoGenericResultSuccessFunctionAndGenericResultErrorFunction_ThrowsException3()
    {
        // Arrange
        const Func<ValueTask<Result<int>>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(Function!, _ => ValueTask.FromResult(Result.FromValue(2))).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessFunctionAndNoGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result<int>>>? ErrorFunction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result<int>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue(2)));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<int>>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessAndErrorFunctions_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var expectedResult = Result.FromValue(9);

        var sut = Result.Success();

        // Act
        var result = await sut.MatchAsync(
            () => ValueTask.FromResult(expectedResult),
            _ => ValueTask.FromResult(Result.FromValue(2)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBeSameAs(expectedResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result<int>>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue(2)));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericResultSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<int>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericResultSuccessAndErrorFunctions_ReturnsErrorFunctionValueResult()
    {
        // Arrange
        var expectedResult = Result.FromValue(15);

        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(
            () => ValueTask.FromResult(Result.FromValue(1)),
            _ => ValueTask.FromResult(expectedResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(expectedResult);
    }

    #endregion

    #region MatchAsync (Result<T>, Func<T, ValueTask>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_NoGenericResultWithSuccessAndErrorActions_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception =
            await Record.ExceptionAsync(() => Sut!.MatchAsync(_ => ValueTask.CompletedTask, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithNoSuccessActionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask>? SuccessAction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessAction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithSuccessActionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(_ => ValueTask.CompletedTask, ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithSuccessAndErrorActions_CallsSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<int, ValueTask>>();

        const int Value = 25;
        Result<int> sut = Value;

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithSuccessAndErrorActions_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        const int Value = 25;
        Result<int> sut = Value;

        // Act
        await sut.MatchAsync(_ => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithSuccessAndErrorActions_ReturnsOriginalValueResult()
    {
        // Arrange
        const int Value = 25;
        Result<int> sut = Value;

        // Act
        var result = await sut.MatchAsync(_ => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithSuccessAndErrorActions_DoesNotCallSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<int, ValueTask>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithSuccessAndErrorActions_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(_ => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithSuccessAndErrorActions_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.MatchAsync(_ => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.ShouldBeSameAs(sut);
    }

    #endregion

    #region MatchAsync (Result<T>, Func<T, ValueTask<TNext>>, Func<Error, ValueTask<TNext>>)

    [Fact]
    public async Task MatchAsync_NoGenericResultWithGenericSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(x => ValueTask.FromResult(x.ToString()), _ => ValueTask.FromResult("error")).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithNoGenericSuccessFunctionAndGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask<string>>? SuccessFunction = null;

        Result<int> sut = 1;

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.FromResult("error")).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessFunctionAndNoGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<string>>? ErrorFunction = null;

        Result<int> sut = 1;

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 5;
        var successFunctionMock = new Mock<Func<int, ValueTask<string>>>();
        successFunctionMock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync("success value");

        Result<int> sut = Value;

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult("error"));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<string>>>();

        Result<int> sut = 5;

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessAndErrorFunctions_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int Value = 5;
        const string ExpectedValue = "6";

        Result<int> sut = Value;

        // Act
        var result = await sut.MatchAsync(x => ValueTask.FromResult((x + 1).ToString()), _ => ValueTask.FromResult("error"));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithWithGenericSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, ValueTask<string>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult("error"));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<string>>>();
        errorFunctionMock
            .Setup(x => x.Invoke(It.IsAny<Error>()))
            .ReturnsAsync("error value");

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericSuccessAndErrorFunctions_ReturnsErrorFunctionValue()
    {
        // Arrange
        const string ExpectedValue = "Error function value";

        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), _ => ValueTask.FromResult(ExpectedValue));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    #endregion

    #region MatchAsync (Result<T>, Func<T, ValueTask<Result<TNext>>>, Func<Error, ValueTask<Result<TNext>>>)

    [Fact]
    public async Task MatchAsync_NoGenericResultWithGenericResultSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(
                x => ValueTask.FromResult(Result.FromValue(x.ToString())),
                _ => ValueTask.FromResult(Result.FromValue("error value"))).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithNoGenericResultSuccessFunctionAndGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask<Result<string>>>? SuccessFunction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(SuccessFunction!, _ => ValueTask.FromResult(Result.FromValue("error value"))).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessFunctionAndNoGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result<string>>>? ErrorFunction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 90;
        var successFunctionMock = new Mock<Func<int, ValueTask<Result<string>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        Result<int> sut = Value;

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue("error value")));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<string>>>>();

        Result<int> sut = 90;

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessAndErrorFunctions_ReturnsSuccessFunctionValueResult()
    {
        // Arrange
        const string ExpectedValue = "-90-";
        const int Value = 90;

        Result<int> sut = Value;

        // Act
        var result = await sut.MatchAsync(
            x => ValueTask.FromResult(Result.FromValue($"-{x}-")),
            _ => ValueTask.FromResult(Result.FromValue("error value")));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, ValueTask<Result<string>>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue("error value")));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericResultSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<string>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericResultSuccessAndErrorFunctions_ReturnsErrorFunctionValueResult()
    {
        // Arrange
        const string ExpectedValue = "error value";

        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.MatchAsync(
            x => ValueTask.FromResult(Result.FromValue(x.ToString())),
            _ => ValueTask.FromResult(Result.FromValue(ExpectedValue)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithNoSuccessActionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask>? SuccessAction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessAction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessActionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.CompletedTask, ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessAndErrorActions_CallsSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<ValueTask>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessAndErrorActions_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(() => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessAndErrorActions_ReturnsOriginalResult()
    {
        // Arrange
        var initialResult = Result.Success();
        var sut = ValueTask.FromResult(initialResult);

        // Act
        var result = await sut.MatchAsync(() => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(initialResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessAndErrorActions_DoesNotCallSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<ValueTask>>();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessAndErrorActions_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(() => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessAndErrorActions_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.MatchAsync(() => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldBe(errorResult);
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask<Result>>, Func<Error, ValueTask<Result>>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithNoSuccessResultFunctionAndErrorResultFunction_ThrowsException2()
    {
        // Arrange
        const Func<ValueTask<Result>>? SuccessFunction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.FromResult(Result.Success())).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessResultFunctionAndNoErrorResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result>>? ErrorFunction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessAndErrorResultFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.Success()));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessAndErrorResultFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result>>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessAndErrorResultFunctions_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var successFunctionResult = Result.Success();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(
            () => ValueTask.FromResult(successFunctionResult),
            _ => ValueTask.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successFunctionResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessAndErrorResultFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result>>>();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.Success()));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessAndErrorResultFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessAndErrorResultFunctions_ReturnsErrorFunctionResult()
    {
        // Arrange
        var errorFunctionResult = Result.Success();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        var result = await sut.MatchAsync(
            () => ValueTask.FromResult(Result.Success()),
            _ => ValueTask.FromResult(errorFunctionResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(errorFunctionResult);
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask<T>>, Func<Error, ValueTask<T>>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithNoGenericSuccessFunctionAndGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask<int>>? Function = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(Function!, _ => ValueTask.FromResult(2)).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericSuccessFunctionAndNoGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<int>>? ErrorFunction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.FromResult(1), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<int>>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(2));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<int>>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(1), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericSuccessAndErrorFunctions_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int ExpectedValue = 58;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(ExpectedValue), _ => ValueTask.FromResult(2));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<int>>>();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(2));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<int>>>();

        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(1), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericSuccessAndErrorFunctions_ReturnsErrorFunctionValue()
    {
        // Arrange
        const int ExpectedValue = 73;

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(1), _ => ValueTask.FromResult(ExpectedValue));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask<Result<T>>>, Func<Error, ValueTask<Result<T>>>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithNoGenericResultSuccessFunctionAndGenericResultErrorFunction_ThrowsException3()
    {
        // Arrange
        const Func<ValueTask<Result<int>>>? Function = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(Function!, _ => ValueTask.FromResult(Result.FromValue(2))).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericResultSuccessFunctionAndNoGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result<int>>>? ErrorFunction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericResultSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result<int>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue(2)));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericResultSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<int>>>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericResultSuccessAndErrorFunctions_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var expectedResult = Result.FromValue(9);

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(
            () => ValueTask.FromResult(expectedResult),
            _ => ValueTask.FromResult(Result.FromValue(2)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBeSameAs(expectedResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericResultSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result<int>>>>();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue(2)));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericResultSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<int>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericResultSuccessAndErrorFunctions_ReturnsErrorFunctionValueResult()
    {
        // Arrange
        var expectedResult = Result.FromValue(15);

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        var result = await sut.MatchAsync(
            () => ValueTask.FromResult(Result.FromValue(1)),
            _ => ValueTask.FromResult(expectedResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(expectedResult);
    }

    #endregion

    #region MatchAsync (ValueTask<Result<T>>, Func<T, ValueTask>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithNoSuccessActionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask>? SuccessAction = null;

        const int Value = 68;
        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessAction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithSuccessActionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        const int Value = 68;
        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(_ => ValueTask.CompletedTask, ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithSuccessAndErrorActions_CallsSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<int, ValueTask>>();

        const int Value = 25;
        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithSuccessAndErrorActions_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        const int Value = 25;
        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(_ => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithSuccessAndErrorActions_ReturnsOriginalValueResult()
    {
        // Arrange
        const int Value = 25;
        var initialResult = Result.FromValue(Value);
        var sut = ValueTask.FromResult(initialResult);

        // Act
        var result = await sut.MatchAsync(_ => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBeSameAs(initialResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithSuccessAndErrorActions_DoesNotCallSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<int, ValueTask>>();

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithSuccessAndErrorActions_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(_ => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithSuccessAndErrorActions_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.MatchAsync(_ => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.ShouldBeSameAs(errorResult);
    }

    #endregion

    #region MatchAsync (ValueTask<Result<T>>, Func<T, ValueTask<TNext>>, Func<Error, ValueTask<TNext>>)

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithNoGenericSuccessFunctionAndGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const int Value = 17;
        const Func<int, ValueTask<string>>? SuccessFunction = null;

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.FromResult("error")).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericSuccessFunctionAndNoGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const int Value = 17;
        const Func<Error, ValueTask<string>>? ErrorFunction = null;

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 17;
        var successFunctionMock = new Mock<Func<int, ValueTask<string>>>();
        successFunctionMock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync("success value");

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult("error"));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        const int Value = 17;
        var errorFunctionMock = new Mock<Func<Error, ValueTask<string>>>();

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericSuccessAndErrorFunctions_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int Value = 17;
        const string ExpectedValue = "18";

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var result = await sut.MatchAsync(x => ValueTask.FromResult((x + 1).ToString()), _ => ValueTask.FromResult("error"));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithWithGenericSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, ValueTask<string>>>();

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult("error"));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<string>>>();
        errorFunctionMock
            .Setup(x => x.Invoke(It.IsAny<Error>()))
            .ReturnsAsync("error value");

        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericSuccessAndErrorFunctions_ReturnsErrorFunctionValue()
    {
        // Arrange
        const string ExpectedValue = "Error function value";

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        var result = await sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), _ => ValueTask.FromResult(ExpectedValue));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    #endregion

    #region MatchAsync (ValueTask<Result<T>>, Func<T, ValueTask<Result<TNext>>>, Func<Error, ValueTask<Result<TNext>>>)

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithNoGenericResultSuccessFunctionAndGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const int Value = 90;
        const Func<int, ValueTask<Result<string>>>? SuccessFunction = null;

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(SuccessFunction!, _ => ValueTask.FromResult(Result.FromValue("error value"))).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericResultSuccessFunctionAndNoGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const int Value = 90;
        const Func<Error, ValueTask<Result<string>>>? ErrorFunction = null;

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericResultSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 90;
        var successFunctionMock = new Mock<Func<int, ValueTask<Result<string>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue("error value")));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericResultSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        const int Value = 90;
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<string>>>>();

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericResultSuccessAndErrorFunctions_ReturnsSuccessFunctionValueResult()
    {
        // Arrange
        const string ExpectedValue = "-90-";
        const int Value = 90;

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var result = await sut.MatchAsync(
            x => ValueTask.FromResult(Result.FromValue($"-{x}-")),
            _ => ValueTask.FromResult(Result.FromValue("error value")));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericResultSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, ValueTask<Result<string>>>>();

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue("error value")));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericResultSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<string>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericResultSuccessAndErrorFunctions_ReturnsErrorFunctionValueResult()
    {
        // Arrange
        const string ExpectedValue = "error value";

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        var result = await sut.MatchAsync(
            x => ValueTask.FromResult(Result.FromValue(x.ToString())),
            _ => ValueTask.FromResult(Result.FromValue(ExpectedValue)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    #endregion

    #region helper methods

    private static Result GetErrorResult()
    {
        return Error.Failure(ErrorUri.None, "50f1c1fb-560e-4bb3-84da-034072df7ca2", "Error title 6");
    }

    private static Result<T> GetErrorResult<T>()
    {
        return Error.Failure(ErrorUri.None, "3d7a81ec-801d-45f4-b4b0-8d58e93b1d89", "Error title 7");
    }

    #endregion

    #region MatchAsync with a function returning an Error (overload trap, issue #119)

    private static readonly Error TrapError = Error.Conflict(ErrorUri.Tag("tag:test.com,2024:Trap"), "Trap title");

    #endregion

    #region ValueTask results with synchronous delegates (issue #214)

    private static readonly Error SyncTestError = Error.Failure(ErrorUri.None, "Synchronous delegate test error");
    private static readonly Error SyncOtherError = Error.Conflict(ErrorUri.None, "Synchronous delegate other error");

    [Fact]
    public async Task MatchAsync_SuccessfulResultTaskWithActionAndErrorAction_RunsSuccessDelegate()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.Success());
        var successCalled = false;
        Error? received = null;

        // Act
        var result = await sut.MatchAsync(() => { successCalled = true; }, error => { received = error; });

        // Assert
        successCalled.ShouldBeTrue();
        received.ShouldBeNull();
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task MatchAsync_FailedResultTaskWithActionAndErrorAction_RunsErrorDelegate()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromError(SyncTestError));
        var successCalled = false;
        Error? received = null;

        // Act
        var result = await sut.MatchAsync(() => { successCalled = true; }, error => { received = error; });

        // Assert
        successCalled.ShouldBeFalse();
        received.ShouldBeSameAs(SyncTestError);
        result.Error.ShouldBeSameAs(SyncTestError);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultTaskWithResultFunctionAndErrorResultFunction_RunsSuccessDelegate()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.Success());
        var successCalled = false;
        Error? received = null;

        // Act
        var result = await sut.MatchAsync(() => { successCalled = true; return Result.FromError(SyncOtherError); }, error => { received = error; return Result.Success(); });

        // Assert
        successCalled.ShouldBeTrue();
        received.ShouldBeNull();
        result.Error.ShouldBeSameAs(SyncOtherError);
    }

    [Fact]
    public async Task MatchAsync_FailedResultTaskWithResultFunctionAndErrorResultFunction_RunsErrorDelegate()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromError(SyncTestError));
        var successCalled = false;
        Error? received = null;

        // Act
        var result = await sut.MatchAsync(() => { successCalled = true; return Result.FromError(SyncOtherError); }, error => { received = error; return Result.Success(); });

        // Assert
        successCalled.ShouldBeFalse();
        received.ShouldBeSameAs(SyncTestError);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultTaskWithValueFunctionAndErrorValueFunction_RunsSuccessDelegate()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.Success());
        var successCalled = false;
        Error? received = null;

        // Act
        var result = await sut.MatchAsync(() => { successCalled = true; return 7; }, error => { received = error; return -1; });

        // Assert
        successCalled.ShouldBeTrue();
        result.ShouldBe(7);
    }

    [Fact]
    public async Task MatchAsync_FailedResultTaskWithValueFunctionAndErrorValueFunction_RunsErrorDelegate()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromError(SyncTestError));
        var successCalled = false;
        Error? received = null;

        // Act
        var result = await sut.MatchAsync(() => { successCalled = true; return 7; }, error => { received = error; return -1; });

        // Assert
        successCalled.ShouldBeFalse();
        received.ShouldBeSameAs(SyncTestError);
        result.ShouldBe(-1);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTTaskWithResultOfTFunctionAndErrorResultOfTFunction_RunsSuccessDelegate()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.Success());
        var successCalled = false;
        Error? received = null;

        // Act
        var result = await sut.MatchAsync(() => { successCalled = true; return Result.FromValue(7); }, error => { received = error; return Result.FromValue(-1); });

        // Assert
        successCalled.ShouldBeTrue();
        result.Value.ShouldBe(7);
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTTaskWithResultOfTFunctionAndErrorResultOfTFunction_RunsErrorDelegate()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromError(SyncTestError));
        var successCalled = false;
        Error? received = null;

        // Act
        var result = await sut.MatchAsync(() => { successCalled = true; return Result.FromValue(7); }, error => { received = error; return Result.FromValue(-1); });

        // Assert
        successCalled.ShouldBeFalse();
        received.ShouldBeSameAs(SyncTestError);
        result.Value.ShouldBe(-1);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTTaskWithActionAndErrorAction_RunsSuccessDelegate()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(5));
        var successCalled = false;
        Error? received = null;

        // Act
        var result = await sut.MatchAsync(value => { successCalled = value == 5; }, error => { received = error; });

        // Assert
        successCalled.ShouldBeTrue();
        result.Value.ShouldBe(5);
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTTaskWithActionAndErrorAction_RunsErrorDelegate()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(SyncTestError));
        var successCalled = false;
        Error? received = null;

        // Act
        var result = await sut.MatchAsync(value => { successCalled = value == 5; }, error => { received = error; });

        // Assert
        successCalled.ShouldBeFalse();
        received.ShouldBeSameAs(SyncTestError);
        result.Error.ShouldBeSameAs(SyncTestError);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTTaskWithValueFunctionAndErrorValueFunction_RunsSuccessDelegate()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(5));
        var successCalled = false;
        Error? received = null;

        // Act
        var result = await sut.MatchAsync(value => { successCalled = true; return value.ToString(); }, error => { received = error; return "none"; });

        // Assert
        successCalled.ShouldBeTrue();
        result.ShouldBe("5");
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTTaskWithValueFunctionAndErrorValueFunction_RunsErrorDelegate()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(SyncTestError));
        var successCalled = false;
        Error? received = null;

        // Act
        var result = await sut.MatchAsync(value => { successCalled = true; return value.ToString(); }, error => { received = error; return "none"; });

        // Assert
        successCalled.ShouldBeFalse();
        received.ShouldBeSameAs(SyncTestError);
        result.ShouldBe("none");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTTaskWithResultFunctionAndErrorResultFunction_RunsSuccessDelegate()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(5));
        var successCalled = false;
        Error? received = null;

        // Act
        var result = await sut.MatchAsync(value => { successCalled = true; return Result.FromValue(value.ToString()); }, error => { received = error; return Result.FromValue("none"); });

        // Assert
        successCalled.ShouldBeTrue();
        result.Value.ShouldBe("5");
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTTaskWithResultFunctionAndErrorResultFunction_RunsErrorDelegate()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(SyncTestError));
        var successCalled = false;
        Error? received = null;

        // Act
        var result = await sut.MatchAsync(value => { successCalled = true; return Result.FromValue(value.ToString()); }, error => { received = error; return Result.FromValue("none"); });

        // Assert
        successCalled.ShouldBeFalse();
        received.ShouldBeSameAs(SyncTestError);
        result.Value.ShouldBe("none");
    }

    [Fact]
    public async Task MatchAsync_IncompleteTaskWithSynchronousDelegate_RunsDelegateAfterCompletion()
    {
        // Arrange
        var source = new TaskCompletionSource<Result<int>>();
        var called = false;
        var sut = new ValueTask<Result<int>>(source.Task);

        // Act
        var task = sut.MatchAsync(_ => { called = true; }, _ => { });
        var calledBeforeCompletion = called;
        source.SetResult(Result.FromValue(5));
        await task;

        // Assert
        calledBeforeCompletion.ShouldBeFalse();
        called.ShouldBeTrue();
    }

    [Fact]
    public void MatchAsync_AsyncSuccessAndSyncErrorDelegateOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = typeof(MatchValueTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => OverloadTrapHelper.IsTrap(m, "The success delegate returns a task, but the error delegate is synchronous"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(8);
        traps.ShouldAllBe(m => m.Name == "MatchAsync"
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    [Fact]
    public void MatchAsync_SyncSuccessAndAsyncErrorDelegateOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = typeof(MatchValueTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => OverloadTrapHelper.IsTrap(m, "The error delegate returns a task, but the success delegate is synchronous"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(27);
        traps.ShouldAllBe(m => m.Name == "MatchAsync"
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    [Fact]
    public void MatchAsync_DelegatesReturningTaskOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = typeof(MatchValueTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => OverloadTrapHelper.IsTrap(m, "The delegates return a Task, which the ValueTask-based MatchAsync does not await"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(14);
        traps.ShouldAllBe(m => m.Name == "MatchAsync"
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    [Fact]
    public void MatchAsync_OverloadsWithValueTaskDelegatesOnValueTaskResults_HavePriorityOne()
    {
        // Act
        var priorities = typeof(MatchValueTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && OverloadTrapHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadTrapHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(18);
        priorities.ShouldAllBe(p => p == 1);
    }

    [Fact]
    public void MatchAsync_SynchronousOverloadsOnValueTaskResults_HavePriorityZero()
    {
        // Act
        var priorities = typeof(MatchValueTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is null
                        && m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && !OverloadTrapHelper.IsTaskFunction(m.GetParameters()[1].ParameterType)
                        && !OverloadTrapHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadTrapHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(7);
        priorities.ShouldAllBe(p => p == 0);
    }

    #endregion

    #region MatchAsync with a success function and an error action (overload trap, issue #227)

    [Fact]
    public void MatchAsync_SuccessFunctionAndErrorActionOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = typeof(MatchValueTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => OverloadTrapHelper.IsTrap(m, "MatchAsync with a success function and an error action"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(21);
        traps.ShouldAllBe(m => m.Name == "MatchAsync"
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    #endregion
}
