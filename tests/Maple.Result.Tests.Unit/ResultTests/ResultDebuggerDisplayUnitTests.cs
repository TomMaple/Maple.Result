// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Diagnostics;
using System.Reflection;

namespace Maple.Results.Tests.Unit.ResultTests;

public class ResultDebuggerDisplayUnitTests
{
    #region Result

    [Fact]
    public void DebuggerDisplayAttribute_Result_UsesTheDebuggerDisplayProperty()
    {
        // Act
        var attribute = typeof(Result).GetCustomAttribute<DebuggerDisplayAttribute>();

        // Assert
        attribute.ShouldNotBeNull().Value.ShouldBe("{DebuggerDisplay,nq}");
    }

    [Fact]
    public void DebuggerDisplay_ResultSuccess_ReturnsToString()
    {
        // Arrange
        const string Expected = "Result { }";
        var sut = Result.Success();

        // Act
        var result = sut.DebuggerDisplay;

        // Assert
        result.ShouldBe(Expected);
    }

    [Fact]
    public void DebuggerDisplay_ResultFailure_ReturnsTheShortErrorDisplay()
    {
        // Arrange
        const string Expected = "Error: NotFound \"Missing\"";
        var sut = Result.FromError(Error.NotFound(ErrorUri.None, "Missing"));

        // Act
        var result = sut.DebuggerDisplay;

        // Assert
        result.ShouldBe(Expected);
    }

    #endregion

    #region Result<T>

    [Fact]
    public void DebuggerDisplayAttribute_ResultT_UsesTheDebuggerDisplayProperty()
    {
        // Act
        var attribute = typeof(Result<>).GetCustomAttribute<DebuggerDisplayAttribute>();

        // Assert
        attribute.ShouldNotBeNull().Value.ShouldBe("{DebuggerDisplay,nq}");
    }

    [Fact]
    public void DebuggerDisplay_ResultTSuccess_ReturnsToString()
    {
        // Arrange
        const string Expected = "Result { Value = 42 }";
        var sut = Result.FromValue(42);

        // Act
        var result = sut.DebuggerDisplay;

        // Assert
        result.ShouldBe(Expected);
    }

    [Fact]
    public void DebuggerDisplay_ResultTFailure_ReturnsTheShortErrorDisplay()
    {
        // Arrange
        const string Expected = "Error: Validation \"Invalid request\" (2 details)";
        var error = Error.Validation(ErrorUri.None, "Invalid request")
            .WithErrorDetail("/name", "The name is required.")
            .WithErrorDetail("/email", "The email is required.");

        var sut = Result<int>.FromError(error);

        // Act
        var result = sut.DebuggerDisplay;

        // Assert
        result.ShouldBe(Expected);
    }

    #endregion
}
