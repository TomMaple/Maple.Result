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
using System.Linq;
using System.Reflection;

namespace Maple.Results.Tests.Unit.Extensions;

public class ElseExtensionsUnitTests
{
    #region IfError (Result, Func<Error, Result>)

    [Fact]
    public void IfError_NoResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Else(_ => Result.Success()));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessfulResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Result>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Else(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_ErrorResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Result>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = Record.Exception(() => sut.Else(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessfulResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Result>>();

        var sut = Result.Success();

        // Act
        var result = sut.Else(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public void IfError_SuccessfulResultWithResultFunction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = sut.Else(_ => Result.Success());

        // Assert
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public void IfError_ErrorResultWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Result>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = GetErrorResult();

        // Act
        sut.Else(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void IfError_ErrorResultWithSuccessResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var funcResult = Result.Success();

        var sut = GetErrorResult();

        // Act
        var result = sut.Else(_ => funcResult);

        // Assert
        result.ShouldBeSameAs(funcResult);
    }

    [Fact]
    public void IfError_ErrorResultWithErrorResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var funcError = GetReplacementError();

        var sut = GetErrorResult();

        // Act
        var result = sut.Else(_ => funcError);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(funcError);
    }

    #endregion

    #region IfError (Result<T>, Func<Error, Result<T>>)

    [Fact]
    public void IfError_NoGenericResultWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Else(_ => Result<int>.FromValue(1)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessfulGenericResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 24;
        const Func<Error, Result<int>>? Function = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = Record.Exception(() => sut.Else(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_ErrorGenericResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Result<int>>? Function = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = Record.Exception(() => sut.Else(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessfulGenericResultWithGenericResultFunction_DoesNotCallFunction()
    {
        // Arrange
        const int InitialValue = 42;
        var functionMock = new Mock<Func<Error, Result<int>>>();

        Result<int> sut = InitialValue;

        // Act
        var result = sut.Else(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
        result.ShouldBeSameAs(sut);
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public void IfError_SuccessfulGenericResultWithGenericResultFunction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 11;

        Result<int> sut = InitialValue;

        // Act
        var result = sut.Else(_ => Result<int>.FromValue(2));

        // Assert
        result.ShouldBeSameAs(sut);
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public void IfError_ErrorGenericResultWithGenericResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Result<int>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = GetErrorResult<int>();

        // Act
        sut.Else(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void IfError_ErrorGenericResultWithGenericResultFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        var funcResult = Result<int>.FromValue(58);

        var sut = GetErrorResult<int>();

        // Act
        var result = sut.Else(_ => funcResult);

        // Assert
        result.ShouldBeSameAs(funcResult);
        result.Value.ShouldBe(58);
    }

    [Fact]
    public void IfError_ErrorGenericResultWithGenericResultFunction_ReturnsFunctionValueResultWithError()
    {
        // Arrange
        var funcError = GetReplacementError();

        var sut = GetErrorResult<int>();

        // Act
        var result = sut.Else(_ => funcError);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(funcError);
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

    #region Else with an action

    // A function that returns nothing is a side effect, not a transformation (#197). The compiler reports it by itself (#256).
    // The bindings are checked in Binding/ThenElseBindingUnitTests.

    [Fact]
    public void Else_NoOverloadTakesAnAction()
    {
        // Act
        var overloads = typeof(ElseExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == "Else" && m.GetParameters().Any(p => OverloadReflectionHelper.IsAction(p.ParameterType)))
            .ToList();

        // Assert
        overloads.ShouldBeEmpty();
    }

    #endregion
}
