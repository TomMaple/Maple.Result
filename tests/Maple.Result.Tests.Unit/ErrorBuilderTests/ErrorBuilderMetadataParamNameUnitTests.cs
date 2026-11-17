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

namespace Maple.Results.Tests.Unit.ErrorBuilderTests;

// An ArgumentException carries the name of a parameter of the called method (#203): the batch overloads of WithMetadata report
// "metadata" for every invalid key, as they do for two keys that differ only in case; the single-key overload reports "key" (#250).
public class ErrorBuilderMetadataParamNameUnitTests
{
    private static ErrorBuilder CreateSut()
    {
        return ErrorBuilder.NotFound(ErrorUri.None, "Missing");
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
        exception.ShouldBeAssignableTo<ArgumentException>()!.ParamName.ShouldBe("metadata");
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
        exception.ShouldBeAssignableTo<ArgumentException>()!.ParamName.ShouldBe("metadata");
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
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("metadata");
    }

    [Fact]
    public void WithMetadataTuples_NullKey_ThrowsArgumentNullExceptionThatReportsMetadata()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(("remaining", 0), (null!, 1)));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("metadata");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(" limit")]
    [InlineData("type")]
    public void WithMetadata_InvalidKey_ReportsKey(string key)
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(key, 1));

        // Assert
        exception.ShouldBeAssignableTo<ArgumentException>()!.ParamName.ShouldBe("key");
    }

    [Fact]
    public void WithMetadata_NullKey_ThrowsArgumentNullExceptionThatReportsKey()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(null!, 1));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("key");
    }
}
