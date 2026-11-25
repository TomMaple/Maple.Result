// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Collections.Generic;
using System.Linq;
using Sut = Maple.Results.Error;

namespace Maple.Results.Tests.Unit.ErrorTests;

// Error.WithMetadata goes through ErrorBuilder; these tests check the public behaviour of Error (ErrorBuilderMetadataKeyCaseUnitTests
// covers each case in detail). Keys are compared ignoring case; a key in another case replaces the entry and its spelling (#349).
public class ErrorMetadataKeyCaseUnitTests
{
    private static Sut CreateSut()
    {
        return Sut.NotFound(ErrorUri.None, "Missing")
            .WithMetadata("RetryAfter", 30);
    }

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromExistingKey_ReplacesTheEntryAndItsSpelling()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.WithMetadata("retryAfter", 60);

        // Assert
        result.Metadata.Keys.ShouldBe(["retryAfter"]);
        result.Metadata["retryAfter"].ShouldBe(60);
    }

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromExistingKey_KeepsTheSourceError()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        _ = sut.WithMetadata("retryAfter", 60);

        // Assert
        sut.Metadata.Keys.ShouldBe(["RetryAfter"]);
        sut.Metadata["RetryAfter"].ShouldBe(30);
    }

    [Fact]
    public void WithMetadataTuples_KeysDifferingOnlyInCaseInOneCall_KeepsTheLastEntry()
    {
        // Arrange
        var sut = Sut.NotFound(ErrorUri.None, "Missing");

        // Act
        var result = sut.WithMetadata(("Limit", 100), ("limit", 200));

        // Assert
        result.Metadata.Keys.ShouldBe(["limit"]);
        result.Metadata["limit"].ShouldBe(200);
    }

    [Fact]
    public void WithMetadataEnumerable_KeyDifferingOnlyInCaseFromExistingKey_ReplacesTheEntryAndItsSpelling()
    {
        // Arrange
        var metadata = new Dictionary<string, object?> { ["retryAfter"] = 60 };
        var sut = CreateSut();

        // Act
        var result = sut.WithMetadata(metadata);

        // Assert
        result.Metadata.Keys.ShouldBe(["retryAfter"]);
        result.Metadata["retryAfter"].ShouldBe(60);
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

    [Fact]
    public void TryGetMetadata_KeyInDifferentCase_ReturnsTrueAndTheValue()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.TryGetMetadata<int>("retryAfter", out var value);

        // Assert
        result.ShouldBeTrue();
        value.ShouldBe(30);
    }

    [Fact]
    public void MetadataIndexer_KeyInDifferentCase_ReturnsTheValue()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Metadata["RETRYAFTER"];

        // Assert
        result.ShouldBe(30);
    }

    [Fact]
    public void MetadataContainsKey_KeyInDifferentCase_ReturnsTrue()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Metadata.ContainsKey("retryafter");

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void MetadataTryGetValue_KeyInDifferentCase_ReturnsTrueAndTheValue()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Metadata.TryGetValue("retryAfter", out var value);

        // Assert
        result.ShouldBeTrue();
        value.ShouldBe(30);
    }

    [Fact]
    public void MetadataKeysContains_KeyInDifferentCase_ReturnsTrue()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.Metadata.Keys.Contains("retryAfter");

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void WithoutMetadata_KeyInDifferentCase_RemovesTheEntry()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.WithoutMetadata("retryAfter");

        // Assert
        result.Metadata.ShouldBeEmpty();
    }
}
