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
using Sut = Maple.Results.Error;

namespace Maple.Results.Tests.Unit.ErrorTests;

// Error.WithMetadata goes through ErrorBuilder; these tests check the public behaviour of Error (ErrorBuilderMetadataKeyCaseUnitTests
// covers each case in detail).
public class ErrorMetadataKeyCaseUnitTests
{
    private static Sut CreateSut()
    {
        return Sut.NotFound(ErrorUri.None, "Missing")
            .WithMetadata("RetryAfter", 30);
    }

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromExistingKey_ThrowsArgumentException()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata("retryAfter", 60));

        // Assert
        exception.ShouldBeOfType<ArgumentException>();
        var argumentException = exception as ArgumentException;
        argumentException.ShouldNotBeNull();
        argumentException.ParamName.ShouldBe("key");
    }

    [Fact]
    public void WithMetadataTuples_KeysDifferingOnlyInCaseInOneCall_ThrowsArgumentException()
    {
        // Arrange
        var sut = Sut.NotFound(ErrorUri.None, "Missing");

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(("Limit", 100), ("limit", 200)));

        // Assert
        exception.ShouldBeOfType<ArgumentException>();
        var argumentException = exception as ArgumentException;
        argumentException.ShouldNotBeNull();
        argumentException.ParamName.ShouldBe("metadata");
    }

    [Fact]
    public void WithMetadataEnumerable_KeyDifferingOnlyInCaseFromExistingKey_ThrowsArgumentException()
    {
        // Arrange
        var metadata = new Dictionary<string, object?> { ["retryAfter"] = 60 };
        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(metadata));

        // Assert
        exception.ShouldBeOfType<ArgumentException>();
        var argumentException = exception as ArgumentException;
        argumentException.ShouldNotBeNull();
        argumentException.ParamName.ShouldBe("metadata");
    }

    [Fact]
    public void WithMetadata_SameKey_ReplacesTheValue()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.WithMetadata("RetryAfter", 60);

        // Assert
        result.Metadata.Count.ShouldBe(1);
        result.Metadata["RetryAfter"].ShouldBe(60);
    }

    // Lookups stay ordinal: with no keys that differ only in case, an exact key is unambiguous.
    [Fact]
    public void TryGetMetadata_KeyInDifferentCase_ReturnsFalse()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.TryGetMetadata<int>("retryAfter", out _);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void WithoutMetadata_KeyInDifferentCase_KeepsTheEntry()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.WithoutMetadata("retryAfter");

        // Assert
        result.Metadata.ShouldContainKey("RetryAfter");
    }
}
