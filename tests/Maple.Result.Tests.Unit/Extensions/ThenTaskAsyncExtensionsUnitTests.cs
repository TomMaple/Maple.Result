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

public class ThenTaskAsyncExtensionsUnitTests
{
    #region IfSuccessAsync (Result, Func<Task<Result>>)

    [Fact]
    public async Task IfSuccessAsync_NoResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ThenAsync(() => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result>>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = Result.Success();

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithSuccessfulResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var funcResult = Result.Success();

        var sut = Result.Success();

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(funcResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBeSameAs(funcResult);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithErrorResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var funcError = GetErrorResult();

        var sut = Result.Success();

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(funcError));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result>>>();

        var sut = GetErrorResult();

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithResultFunction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result, Func<Task<T>>)

    [Fact]
    public async Task IfSuccessAsync_NoResultWithGenericFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ThenAsync(() => Task.FromResult(456)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<int>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<int>>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithGenericFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<int>>>();

        var sut = Result.Success();

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithGenericFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int FuncValue = 2463;

        var sut = Result.Success();

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(FuncValue));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithGenericFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<int>>>();

        var sut = GetErrorResult();

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithGenericFunction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(234));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result, Func<Task<Result<T>>>)

    [Fact]
    public async Task IfSuccessAsync_NoResultWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception =
            await Record.ExceptionAsync(() => Sut!.ThenAsync(() => Task.FromResult(Result.FromValue(123))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result<int>>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result<int>>>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithGenericResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result<int>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = Result.Success();

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithSuccessfulGenericResultFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int FuncValue = 2463;

        var sut = Result.Success();

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(Result.FromValue(FuncValue)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithErrorGenericResultFunction_ReturnsFunctionValueResultWithError()
    {
        // Arrange
        var funcError = GetError();

        var sut = Result.Success();

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(Result<int>.FromError(funcError)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithGenericResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result<int>>>>();

        var sut = GetErrorResult();

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithGenericResultFunction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(Result.FromValue(678)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task<Result>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result<string>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ThenAsync(_ => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const string InitialValue = "Start";
        const Func<string, Task<Result>>? Function = null;

        Result<string> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<string, Task<Result>>? Function = null;

        var sut = GetErrorResult<string>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithResultFunction_CallsFunction()
    {
        // Arrange
        const string InitialValue = "Start";
        var functionMock = new Mock<Func<string, Task<Result>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        Result<string> sut = InitialValue;

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithSuccessResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        const string InitialValue = "Start";

        Result<string> sut = InitialValue;

        // Act
        var result = await sut.ThenAsync(_ => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithErrorResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        const string InitialValue = "Start";
        var errorResult = GetErrorResult();

        Result<string> sut = InitialValue;

        // Act
        var result = await sut.ThenAsync(_ => Task.FromResult(errorResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<string, Task<Result>>>();

        var sut = GetErrorResult<string>();

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithResultFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<string>();

        // Act
        var result = await sut.ThenAsync(_ => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task<TNext>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultWithGenericFunction_ThrowsException()
    {
        // Arrange
        const Result<double>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ThenAsync(_ => Task.FromResult(12.34)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const double InitialValue = 12.34;
        const Func<double, Task<int>>? Function = null;

        Result<double> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const Func<double, Task<int>>? Function = null;

        var sut = GetErrorResult<double>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithGenericFunction_CallsFunction()
    {
        // Arrange
        const double InitialValue = 12.34;
        var functionMock = new Mock<Func<double, Task<int>>>();

        Result<double> sut = InitialValue;

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithGenericFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int ExpectedValue = 13;
        const double InitialValue = 12.34;

        Result<double> sut = InitialValue;

        // Act
        var result = await sut.ThenAsync(x => Task.FromResult((int)x + 1));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithGenericFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<double, Task<int>>>();

        var sut = GetErrorResult<double>();

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<double>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithGenericFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<double>();

        // Act
        var result = await sut.ThenAsync(_ => Task.FromResult(9827));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task<Result<TNext>>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.ThenAsync(x => Task.FromResult(Result.FromValue(x.ToString()))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 39;
        const Func<int, Task<Result<string>>>? Function = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<int, Task<Result<string>>>? Function = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithGenericResultFunction_CallsFunction()
    {
        // Arrange
        const int InitialValue = 39;
        var functionMock = new Mock<Func<int, Task<Result<string>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        Result<int> sut = InitialValue;

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithSuccessfulGenericResultFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int InitialValue = 49;
        const string FuncValue = "50";

        Result<int> sut = InitialValue;

        // Act
        var result = await sut.ThenAsync(x => Task.FromResult(Result.FromValue((x + 1).ToString())));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithErrorGenericResultFunction_ReturnsFunctionValueResultWithError()
    {
        // Arrange
        const int InitialValue = 49;
        var funcError = GetErrorResult<string>();

        Result<int> sut = InitialValue;

        // Act
        var result = await sut.ThenAsync(_ => Task.FromResult(funcError));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithGenericResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<int, Task<Result<string>>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithGenericResultFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.ThenAsync(x => Task.FromResult(Result.FromValue(x.ToString())));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result>, Func<Task<Result>>)

    [Fact]
    public async Task IfSuccessAsync_NoResultTaskWithResultFunction_ThrowsException()
    {
        // Arrange
        const Task<Result>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ThenAsync(() => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result>>? Function = null;

        var sut = Task.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result>>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = Task.FromResult(Result.Success());

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithSuccessfulResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var funcResult = Result.Success();

        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(funcResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeTrue();
        result.ShouldBeSameAs(funcResult);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithErrorResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var funcError = GetErrorResult();

        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(funcError));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result>>>();

        var sut = Task.FromResult(GetErrorResult());

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithResultFunction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result>, Func<Task<T>>)

    [Fact]
    public async Task IfSuccessAsync_NoResultTaskWithGenericFunction_ThrowsException()
    {
        // Arrange
        const Task<Result>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ThenAsync(() => Task.FromResult(456)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<int>>? Function = null;

        var sut = Task.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<int>>? Function = null;

        var sut = Task.FromResult(GetErrorResult());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithGenericFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<int>>>();

        var sut = Task.FromResult(Result.Success());

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithGenericFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int FuncValue = 2463;

        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(FuncValue));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithGenericFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<int>>>();

        var sut = Task.FromResult(GetErrorResult());

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithGenericFunction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(234));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result>, Func<Task<Result<T>>>)

    [Fact]
    public async Task IfSuccessAsync_NoResultTaskWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Task<Result>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ThenAsync(() => Task.FromResult(Result.FromValue(123))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result<int>>>? Function = null;

        var sut = Task.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result<int>>>? Function = null;

        var sut = Task.FromResult(GetErrorResult());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithGenericResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result<int>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = Task.FromResult(Result.Success());

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithSuccessfulGenericResultFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int FuncValue = 2463;

        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(Result.FromValue(FuncValue)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithErrorGenericResultFunction_ReturnsFunctionValueResultWithError()
    {
        // Arrange
        var funcError = GetError();

        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(Result<int>.FromError(funcError)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithGenericResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result<int>>>>();

        var sut = Task.FromResult(GetErrorResult());

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithGenericResultFunction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.ThenAsync(() => Task.FromResult(Result.FromValue(678)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result<T>>, Func<T, Task<Result>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultTaskWithResultFunction_ThrowsException()
    {
        // Arrange
        const Task<Result<string>>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ThenAsync(_ => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const string InitialValue = "Start";
        const Func<string, Task<Result>>? Function = null;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<string, Task<Result>>? Function = null;

        var sut = Task.FromResult(GetErrorResult<string>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithResultFunction_CallsFunction()
    {
        // Arrange
        const string InitialValue = "Start";
        var functionMock = new Mock<Func<string, Task<Result>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithSuccessResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        const string InitialValue = "Start";

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.ThenAsync(_ => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithErrorResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        const string InitialValue = "Start";
        var errorResult = GetErrorResult();

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.ThenAsync(_ => Task.FromResult(errorResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<string, Task<Result>>>();

        var sut = Task.FromResult(GetErrorResult<string>());

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithResultFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<string>();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.ThenAsync(_ => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result<T>>, Func<T, Task<TNext>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultTaskWithGenericFunction_ThrowsException()
    {
        // Arrange
        const Task<Result<double>>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ThenAsync(_ => Task.FromResult(12.34)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const double InitialValue = 12.34;
        const Func<double, Task<int>>? Function = null;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const Func<double, Task<int>>? Function = null;

        var sut = Task.FromResult(GetErrorResult<double>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithGenericFunction_CallsFunction()
    {
        // Arrange
        const double InitialValue = 12.34;
        var functionMock = new Mock<Func<double, Task<int>>>();

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithGenericFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int ExpectedValue = 13;
        const double InitialValue = 12.34;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.ThenAsync(x => Task.FromResult((int)x + 1));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithGenericFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<double, Task<int>>>();

        var sut = Task.FromResult(GetErrorResult<double>());

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<double>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithGenericFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<double>();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.ThenAsync(_ => Task.FromResult(9827));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result<T>>, Func<T, Task<Result<TNext>>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultTaskWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Task<Result<int>>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.ThenAsync(x => Task.FromResult(Result.FromValue(x.ToString()))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 39;
        const Func<int, Task<Result<string>>>? Function = null;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<int, Task<Result<string>>>? Function = null;

        var sut = Task.FromResult(GetErrorResult<int>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ThenAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithGenericResultFunction_CallsFunction()
    {
        // Arrange
        const int InitialValue = 39;
        var functionMock = new Mock<Func<int, Task<Result<string>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithSuccessfulGenericResultFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int InitialValue = 49;
        const string FuncValue = "50";

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.ThenAsync(x => Task.FromResult(Result.FromValue((x + 1).ToString())));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithErrorGenericResultFunction_ReturnsFunctionValueResultWithError()
    {
        // Arrange
        const int InitialValue = 49;
        var funcError = GetErrorResult<string>();

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.ThenAsync(_ => Task.FromResult(funcError));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithGenericResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<int, Task<Result<string>>>>();

        var sut = Task.FromResult(GetErrorResult<int>());

        // Act
        await sut.ThenAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithGenericResultFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.ThenAsync(x => Task.FromResult(Result.FromValue(x.ToString())));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync with a function returning an Error (overload trap)

    // See issue #119.

    private static readonly Error TrapError = Error.Conflict(ErrorUri.Tag("tag:test.com,2024:Trap"), "Trap title");

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithErrorFunction_ReturnsFailedResultWithReturnedError()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        Result result = await sut.ThenAsync(() => Task.FromResult(TrapError));

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(TrapError);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithErrorFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var sut = Result.FromError(GetError());
        var called = false;

        // Act
        var result = await sut.ThenAsync(() =>
        {
            called = true;
            return Task.FromResult(TrapError);
        });

        // Assert
        result.ShouldBeSameAs(sut);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithErrorFunctionReturningNull_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(async () => await sut.ThenAsync(() => Task.FromResult((Error)null!)));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>().Message.ShouldBe("The function ‘ifSuccessFunction’ returned null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultOfTWithErrorFunction_ReturnsFailedResultWithReturnedError()
    {
        // Arrange
        var sut = Result<int>.FromValue(5);

        // Act
        Result result = await sut.ThenAsync(_ => Task.FromResult(TrapError));

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(TrapError);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultOfTWithErrorFunction_ReturnsFailedResultWithOriginalError()
    {
        // Arrange
        var error = GetError();
        var sut = Result<int>.FromError(error);

        // Act
        Result result = await sut.ThenAsync(_ => Task.FromResult(TrapError));

        // Assert
        result.Error.ShouldBeSameAs(error);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultOfTWithErrorFunctionAndExplicitTypeArguments_ReturnsSuccessfulResultOfError()
    {
        // Arrange
        var sut = Result<int>.FromValue(5);

        // Act
        Result<Error> result = await sut.ThenAsync<int, Error>(_ => Task.FromResult(TrapError));

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(TrapError);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithErrorFunction_ReturnsFailedResultWithReturnedError()
    {
        // Arrange
        var sut = Task.FromResult(Result.Success());

        // Act
        Result result = await sut.ThenAsync(() => Task.FromResult(TrapError));

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(TrapError);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithErrorFunction_ReturnsFailedResultWithOriginalError()
    {
        // Arrange
        var error = GetError();
        var sut = Task.FromResult(Result.FromError(error));

        // Act
        Result result = await sut.ThenAsync(() => Task.FromResult(TrapError));

        // Assert
        result.Error.ShouldBeSameAs(error);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultOfTTaskWithErrorFunction_ReturnsFailedResultWithReturnedError()
    {
        // Arrange
        var sut = Task.FromResult(Result<int>.FromValue(5));

        // Act
        Result result = await sut.ThenAsync(_ => Task.FromResult(TrapError));

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(TrapError);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultOfTTaskWithErrorFunction_ReturnsFailedResultWithOriginalError()
    {
        // Arrange
        var error = GetError();
        var sut = Task.FromResult(Result<int>.FromError(error));

        // Act
        Result result = await sut.ThenAsync(_ => Task.FromResult(TrapError));

        // Assert
        result.Error.ShouldBeSameAs(error);
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

    #region ThenAsync with a function returning a task without a value (overload trap)

    // A function that returns a task without a value is a side effect: on a task result, the transforming method reports it as a
    // compile error (CS0619) that names the side-effect method (#197). On a result, the compiler reports it by itself, so there is no
    // overload trap for it (#256). The bindings are checked in Binding/ThenElseBindingUnitTests.

    [Fact]
    public void ThenAsync_FunctionReturningTaskWithoutValueOverloads_AreHiddenCompileErrorsOnTaskResults()
    {
        // Act
        var traps = OverloadTrapHelper.TaskPartMethods(typeof(ThenAsyncExtensions))
            .Where(m => OverloadTrapHelper.IsTrap(m, "The function returns a task without a value. ThenAsync needs"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(2);
        traps.ShouldAllBe(m => m.Name == "ThenAsync"
                               && m.GetParameters()[0].ParameterType.IsGenericType
                               && m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(Task<>)
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    #endregion

    #region Task results with synchronous delegates

    // See issue #214.

    private static readonly Error SyncTestError = Error.Failure(ErrorUri.None, "Synchronous delegate test error");
    private static readonly Error SyncOtherError = Error.Conflict(ErrorUri.None, "Synchronous delegate other error");

    private static int ThrowingFunction(int value)
    {
        throw new FormatException("Synchronous delegate test exception.");
    }

    [Fact]
    public async Task ThenAsync_SuccessfulResultTaskWithSyncValueFunction_ReturnsResultWithValue()
    {
        // Arrange
        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.ThenAsync(() => 7);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(7);
    }

    [Fact]
    public async Task ThenAsync_FailedResultTaskWithSyncValueFunction_ReturnsOriginalErrorAndDoesNotCallFunction()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromError(SyncTestError));
        var called = false;

        // Act
        var result = await sut.ThenAsync(() => { called = true; return 7; });

        // Assert
        result.Error.ShouldBeSameAs(SyncTestError);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ThenAsync_SuccessfulResultTaskWithSyncResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.ThenAsync(() => Result.FromError(SyncOtherError));

        // Assert
        result.Error.ShouldBeSameAs(SyncOtherError);
    }

    [Fact]
    public async Task ThenAsync_FailedResultTaskWithSyncResultFunction_ReturnsOriginalError()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromError(SyncTestError));

        // Act
        var result = await sut.ThenAsync(() => Result.Success());

        // Assert
        result.Error.ShouldBeSameAs(SyncTestError);
    }

    [Fact]
    public async Task ThenAsync_SuccessfulResultTaskWithSyncResultOfTFunction_ReturnsFunctionResult()
    {
        // Arrange
        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.ThenAsync(() => Result.FromValue("next"));

        // Assert
        result.Value.ShouldBe("next");
    }

    [Fact]
    public async Task ThenAsync_FailedResultTaskWithSyncResultOfTFunction_ReturnsOriginalError()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromError(SyncTestError));

        // Act
        var result = await sut.ThenAsync(() => Result.FromValue("next"));

        // Assert
        result.Error.ShouldBeSameAs(SyncTestError);
    }

    [Fact]
    public async Task ThenAsync_SuccessfulResultTaskWithSyncErrorFunction_ReturnsFailedResultWithFunctionError()
    {
        // Arrange
        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.ThenAsync(() => SyncOtherError);

        // Assert
        result.Error.ShouldBeSameAs(SyncOtherError);
    }

    [Fact]
    public async Task ThenAsync_FailedResultTaskWithSyncErrorFunction_ReturnsOriginalError()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromError(SyncTestError));

        // Act
        var result = await sut.ThenAsync(() => SyncOtherError);

        // Assert
        result.Error.ShouldBeSameAs(SyncTestError);
    }

    [Fact]
    public async Task ThenAsync_SuccessfulResultOfTTaskWithSyncValueFunction_ReturnsResultWithFunctionValue()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        var result = await sut.ThenAsync(value => value * 2);

        // Assert
        result.Value.ShouldBe(10);
    }

    [Fact]
    public async Task ThenAsync_FailedResultOfTTaskWithSyncValueFunction_ReturnsOriginalErrorAndDoesNotCallFunction()
    {
        // Arrange
        var sut = Task.FromResult(Result<int>.FromError(SyncTestError));
        var called = false;

        // Act
        var result = await sut.ThenAsync(value => { called = true; return value * 2; });

        // Assert
        result.Error.ShouldBeSameAs(SyncTestError);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ThenAsync_SuccessfulResultOfTTaskWithSyncResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        var result = await sut.ThenAsync(_ => Result.FromError(SyncOtherError));

        // Assert
        result.Error.ShouldBeSameAs(SyncOtherError);
    }

    [Fact]
    public async Task ThenAsync_FailedResultOfTTaskWithSyncResultFunction_ReturnsOriginalError()
    {
        // Arrange
        var sut = Task.FromResult(Result<int>.FromError(SyncTestError));

        // Act
        var result = await sut.ThenAsync(_ => Result.Success());

        // Assert
        result.Error.ShouldBeSameAs(SyncTestError);
    }

    [Fact]
    public async Task ThenAsync_SuccessfulResultOfTTaskWithSyncResultOfTFunction_ReturnsFunctionResult()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        var result = await sut.ThenAsync(value => Result.FromValue(value.ToString()));

        // Assert
        result.Value.ShouldBe("5");
    }

    [Fact]
    public async Task ThenAsync_FailedResultOfTTaskWithSyncResultOfTFunction_ReturnsOriginalError()
    {
        // Arrange
        var sut = Task.FromResult(Result<int>.FromError(SyncTestError));

        // Act
        var result = await sut.ThenAsync(value => Result.FromValue(value.ToString()));

        // Assert
        result.Error.ShouldBeSameAs(SyncTestError);
    }

    [Fact]
    public async Task ThenAsync_SuccessfulResultOfTTaskWithSyncErrorFunction_ReturnsFailedResultWithFunctionError()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        var result = await sut.ThenAsync(_ => SyncOtherError);

        // Assert
        result.Error.ShouldBeSameAs(SyncOtherError);
    }

    [Fact]
    public async Task ThenAsync_FailedResultOfTTaskWithSyncErrorFunction_ReturnsOriginalError()
    {
        // Arrange
        var sut = Task.FromResult(Result<int>.FromError(SyncTestError));

        // Act
        var result = await sut.ThenAsync(_ => SyncOtherError);

        // Assert
        result.Error.ShouldBeSameAs(SyncTestError);
    }

    [Fact]
    public async Task ThenAsync_SuccessfulResultOfTTaskWithSyncFunctionReturningNullResult_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        var exception = await Record.ExceptionAsync(async () => await sut.ThenAsync(_ => (Result<string>)null!));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task ThenAsync_SyncFunctionThrows_FaultsReturnedTaskInsteadOfThrowingFromCall()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        var call = Record.Exception(() => { _ = sut.ThenAsync(ThrowingFunction); });
        var awaited = await Record.ExceptionAsync(async () => await sut.ThenAsync(ThrowingFunction));

        // Assert
        call.ShouldBeNull();
        awaited.ShouldBeOfType<FormatException>();
    }

    [Fact]
    public async Task ThenAsync_IncompleteTaskWithSynchronousDelegate_RunsDelegateAfterCompletion()
    {
        // Arrange
        var source = new TaskCompletionSource<Result<int>>();
        var called = false;
        var sut = source.Task;

        // Act
        var task = sut.ThenAsync(value => { called = true; return value; });
        var calledBeforeCompletion = called;
        source.SetResult(Result.FromValue(5));
        await task;

        // Assert
        calledBeforeCompletion.ShouldBeFalse();
        called.ShouldBeTrue();
    }

    [Fact]
    public void ThenAsync_NoOverloadTakesASynchronousAction()
    {
        // Act
        var overloads = OverloadTrapHelper.TaskPartMethods(typeof(ThenAsyncExtensions))
            .Where(m => m.Name == "ThenAsync" && m.GetParameters().Any(p => OverloadTrapHelper.IsAction(p.ParameterType)))
            .ToList();

        // Assert
        overloads.ShouldBeEmpty();
    }

    [Fact]
    public void ThenAsync_FunctionReturningValueTaskOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = OverloadTrapHelper.TaskPartMethods(typeof(ThenAsyncExtensions))
            .Where(m => OverloadTrapHelper.IsTrap(m, "The function returns a ValueTask, which ThenAsync on a Task of a result does not await"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(4);
        traps.ShouldAllBe(m => m.Name == "ThenAsync"
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    [Fact]
    public void ThenAsync_OverloadsWithTaskDelegatesOnTaskResults_HavePriorityOne()
    {
        // Act
        var priorities = OverloadTrapHelper.TaskPartMethods(typeof(ThenAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(Task<>)
                        && OverloadTrapHelper.IsTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadTrapHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(10);
        priorities.ShouldAllBe(p => p == 1);
    }

    [Fact]
    public void ThenAsync_SynchronousOverloadsOnTaskResults_HavePriorityZero()
    {
        // Act
        var priorities = OverloadTrapHelper.TaskPartMethods(typeof(ThenAsyncExtensions))
            .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is null
                        && m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(Task<>)
                        && !OverloadTrapHelper.IsTaskFunction(m.GetParameters()[1].ParameterType)
                        && !OverloadTrapHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadTrapHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(8);
        priorities.ShouldAllBe(p => p == 0);
    }

    #endregion
}
