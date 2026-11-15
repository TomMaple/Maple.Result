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
using Sut = Maple.Results.Error;

namespace Maple.Results.Tests.Unit.ErrorTests;

public class ErrorDebuggerDisplayUnitTests
{
    [Fact]
    public void DebuggerDisplayAttribute_Error_UsesTheDebuggerDisplayProperty()
    {
        // Act
        var attribute = typeof(Sut).GetCustomAttribute<DebuggerDisplayAttribute>();

        // Assert
        attribute.ShouldNotBeNull().Value.ShouldBe("{DebuggerDisplay,nq}");
    }

    [Fact]
    public void DebuggerDisplay_NoErrorDetails_ReturnsCategoryAndTitle()
    {
        // Arrange
        const string Expected = "NotFound \"Missing\"";
        var sut = Sut.NotFound(ErrorUri.None(), "Missing");

        // Act
        var result = sut.DebuggerDisplay;

        // Assert
        result.ShouldBe(Expected);
    }

    [Fact]
    public void DebuggerDisplay_OneErrorDetail_ReturnsCategoryTitleAndOneDetail()
    {
        // Arrange
        const string Expected = "Validation \"Invalid request\" (1 detail)";
        var sut = Sut.Validation(ErrorUri.None(), "Invalid request")
            .WithErrorDetail("/name", "The name is required.");

        // Act
        var result = sut.DebuggerDisplay;

        // Assert
        result.ShouldBe(Expected);
    }

    [Fact]
    public void DebuggerDisplay_SeveralErrorDetails_ReturnsCategoryTitleAndDetailCount()
    {
        // Arrange
        const string Expected = "Validation \"Invalid request\" (3 details)";
        var sut = Sut.Validation(ErrorUri.None(), "Invalid request")
            .WithErrorDetail("/name", "The name is required.")
            .WithErrorDetail("/email", "The email is required.")
            .WithErrorDetail("/age", "The age must be at least 18.");

        // Act
        var result = sut.DebuggerDisplay;

        // Assert
        result.ShouldBe(Expected);
    }

    [Fact]
    public void DebuggerDisplay_DetailAndMetadata_AreNotIncluded()
    {
        // Arrange
        const string Expected = "Conflict \"Already exists\"";
        var sut = Sut.Conflict(ErrorUri.None(), "Already exists", "The user already exists.")
            .WithMetadata("userId", 42);

        // Act
        var result = sut.DebuggerDisplay;

        // Assert
        result.ShouldBe(Expected);
    }
}
