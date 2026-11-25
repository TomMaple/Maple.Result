// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections.Generic;

namespace Maple.Results.Tests.Unit.ErrorTests;

// The batch overloads of Error.WithMetadata report the same parameter name as those of ErrorBuilder: "metadata" for every invalid
// key; the single-key overload reports "key" (#250).
public class ErrorMetadataParamNameUnitTests
{
    private static Error CreateSut()
    {
        return Error.NotFound(ErrorUri.None, "Missing");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(" limit")]
    [InlineData("limit ")]
    [InlineData("type")]
    [InlineData("STATUS")]
    public void WithMetadataEnumerable_InvalidKey_ReportsMetadata(string key)
    {
        // Arrange
        var metadata = new List<KeyValuePair<string, object?>> { new("remaining", 0), new(key, 1) };
        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(metadata));

        // Assert
        exception.ShouldBeAssignableTo<ArgumentException>();
        var argumentException = exception as ArgumentException;
        argumentException.ShouldNotBeNull();
        argumentException.ParamName.ShouldBe("metadata");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(" limit")]
    [InlineData("limit ")]
    [InlineData("type")]
    [InlineData("STATUS")]
    public void WithMetadataTuples_InvalidKey_ReportsMetadata(string key)
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(("remaining", 0), (key, 1)));

        // Assert
        exception.ShouldBeAssignableTo<ArgumentException>();
        var argumentException = exception as ArgumentException;
        argumentException.ShouldNotBeNull();
        argumentException.ParamName.ShouldBe("metadata");
    }

    [Fact]
    public void WithMetadataEnumerable_NullKey_ThrowsArgumentNullExceptionThatReportsMetadata()
    {
        // Arrange
        var metadata = new List<KeyValuePair<string, object?>> { new("remaining", 0), new(null!, 1) };
        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(metadata));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>();
        var argumentNullException = exception as ArgumentNullException;
        argumentNullException.ShouldNotBeNull();
        argumentNullException.ParamName.ShouldBe("metadata");
    }

    [Fact]
    public void WithMetadataTuples_NullKey_ThrowsArgumentNullExceptionThatReportsMetadata()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(("remaining", 0), (null!, 1)));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>();
        var argumentNullException = exception as ArgumentNullException;
        argumentNullException.ShouldNotBeNull();
        argumentNullException.ParamName.ShouldBe("metadata");
    }
}
