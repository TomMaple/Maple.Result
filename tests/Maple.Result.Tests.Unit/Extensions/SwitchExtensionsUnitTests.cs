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
using System;
using System.Linq;
using System.Reflection;

namespace Maple.Results.Tests.Unit.Extensions;

public class SwitchExtensionsUnitTests
{
    private static readonly Error SourceError = Error.Failure(ErrorUri.None, "8c0f5d0e-7d4b-4f0e-9d0a-2d3f8e6c1b7a", "Switch error");

    #region Switch (Result, Action, Action<Error>)

    [Fact]
    public void Switch_NullResult_ThrowsArgumentNullException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Switch(() => { }, _ => { }));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("result");
    }

    [Fact]
    public void Switch_NullSuccessAction_ThrowsArgumentNullException()
    {
        // Arrange
        const Action? SuccessAction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Switch(SuccessAction!, _ => { }));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("ifSuccessAction");
    }

    [Fact]
    public void Switch_NullErrorAction_ThrowsArgumentNullException()
    {
        // Arrange
        const Action<Error>? ErrorAction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Switch(() => { }, ErrorAction!));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("ifErrorAction");
    }

    [Fact]
    public void Switch_SuccessfulResult_RunsOnlySuccessAction()
    {
        // Arrange
        var successCalls = 0;
        var errorCalls = 0;
        var sut = Result.Success();

        // Act
        sut.Switch(() => successCalls++, _ => errorCalls++);

        // Assert
        successCalls.ShouldBe(1);
        errorCalls.ShouldBe(0);
    }

    [Fact]
    public void Switch_FailedResult_RunsOnlyErrorActionWithTheError()
    {
        // Arrange
        var successCalls = 0;
        Error? passedError = null;
        var sut = Result.FromError(SourceError);

        // Act
        sut.Switch(() => successCalls++, e => passedError = e);

        // Assert
        successCalls.ShouldBe(0);
        passedError.ShouldBeSameAs(SourceError);
    }

    [Fact]
    public void Switch_SuccessActionThrows_PropagatesTheException()
    {
        // Arrange
        var thrown = new InvalidOperationException("success action");
        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Switch(() => throw thrown, _ => { }));

        // Assert
        exception.ShouldBeSameAs(thrown);
    }

    #endregion

    #region Switch (Result<T>, Action<T>, Action<Error>)

    [Fact]
    public void Switch_NullResultOfT_ThrowsArgumentNullException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Switch(_ => { }, _ => { }));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("result");
    }

    [Fact]
    public void Switch_ResultOfTWithNullSuccessAction_ThrowsArgumentNullException()
    {
        // Arrange
        const Action<int>? SuccessAction = null;

        var sut = Result.FromValue(5);

        // Act
        var exception = Record.Exception(() => sut.Switch(SuccessAction!, _ => { }));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("ifSuccessAction");
    }

    [Fact]
    public void Switch_ResultOfTWithNullErrorAction_ThrowsArgumentNullException()
    {
        // Arrange
        const Action<Error>? ErrorAction = null;

        var sut = Result.FromValue(5);

        // Act
        var exception = Record.Exception(() => sut.Switch(_ => { }, ErrorAction!));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("ifErrorAction");
    }

    [Fact]
    public void Switch_SuccessfulResultOfT_RunsOnlySuccessActionWithTheValue()
    {
        // Arrange
        int? passedValue = null;
        var errorCalls = 0;
        var sut = Result.FromValue(5);

        // Act
        sut.Switch(x => passedValue = x, _ => errorCalls++);

        // Assert
        passedValue.ShouldBe(5);
        errorCalls.ShouldBe(0);
    }

    [Fact]
    public void Switch_FailedResultOfT_RunsOnlyErrorActionWithTheError()
    {
        // Arrange
        var successCalls = 0;
        Error? passedError = null;
        var sut = Result<int>.FromError(SourceError);

        // Act
        sut.Switch(_ => successCalls++, e => passedError = e);

        // Assert
        successCalls.ShouldBe(0);
        passedError.ShouldBeSameAs(SourceError);
    }

    [Fact]
    public void Switch_FailedResultOfTWithErrorActionThrowing_PropagatesTheException()
    {
        // Arrange
        var thrown = new InvalidOperationException("error action");
        var sut = Result<int>.FromError(SourceError);

        // Act
        var exception = Record.Exception(() => sut.Switch(_ => { }, _ => throw thrown));

        // Assert
        exception.ShouldBeSameAs(thrown);
    }

    #endregion

    #region Switch overloads and their priority

    [Fact]
    public void Switch_Overloads_HaveDefaultPriority()
    {
        // Act
        var overloads = typeof(SwitchExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .ToList();

        // Assert
        overloads.Count.ShouldBe(2);
        overloads.ShouldAllBe(m => m.Name == "Switch" && m.ReturnType == typeof(void) && OverloadReflectionHelper.Priority(m) == 0);
    }

    #endregion
}
