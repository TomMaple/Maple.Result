// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Validators;
using System;

namespace Maple.Results.Tests.Unit.Validators;

public class MetadataKeyValidatorUnitTests
{
    [Theory]
    [InlineData("type")]
    [InlineData("title")]
    [InlineData("status")]
    [InlineData("detail")]
    [InlineData("instance")]
    [InlineData("errors")]
    [InlineData("detailTemplated")]
    [InlineData("TYPE")]
    [InlineData("Instance")]
    [InlineData("detailtemplated")]
    public void IsReserved_StandardMemberName_ReturnsTrue(string key)
    {
        // Act
        var result = MetadataKeyValidator.IsReserved(key);

        // Assert
        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData("metadata")]
    [InlineData("traceId")]
    [InlineData("limit")]
    [InlineData("detailTemplatedX")]
    [InlineData("typ")]
    public void IsReserved_OtherName_ReturnsFalse(string key)
    {
        // Act
        var result = MetadataKeyValidator.IsReserved(key);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void Validate_ValidKey_DoesNotThrow()
    {
        // Act & Assert
        Should.NotThrow(() => MetadataKeyValidator.Validate("retryAfterSeconds"));
    }

    [Fact]
    public void Validate_ReservedKey_ThrowsArgumentExceptionWithQuotedKey()
    {
        // Act
        var exception = Should.Throw<ArgumentException>(() => MetadataKeyValidator.Validate("Title"));

        // Assert
        exception.Message.ShouldStartWith("The metadata key ‘Title’ is reserved.");
        exception.Message.ShouldContain("‘detailTemplated’");
    }

    [Fact]
    public void Validate_NullKey_ThrowsArgumentNullException()
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() => MetadataKeyValidator.Validate(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("errors")]
    [InlineData("Errors")]
    public void Validate_InvalidKey_ThrowsArgumentException(string key)
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => MetadataKeyValidator.Validate(key));
    }

    #region padded keys (issue #203)

    [Theory]
    [InlineData(" type")]
    [InlineData("type ")]
    [InlineData("\tlimit")]
    [InlineData(" limit ")]
    public void Validate_PaddedKey_ThrowsArgumentException(string key)
    {
        // Act
        var exception = Record.Exception(() => MetadataKeyValidator.Validate(key));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        ((ArgumentException)exception).ParamName.ShouldBe("key");
    }

    [Fact]
    public void Validate_PaddedKey_MessageShowsKey()
    {
        // Act
        var exception = Record.Exception(() => MetadataKeyValidator.Validate(" type"));

        // Assert
        exception.ShouldNotBeNull();
        exception.Message.ShouldContain("‘ type’");
    }

    [Fact]
    public void Validate_KeyWithWhiteSpaceInside_DoesNotThrow()
    {
        // Act & Assert
        Should.NotThrow(() => MetadataKeyValidator.Validate("retry after"));
    }

    #endregion
}
