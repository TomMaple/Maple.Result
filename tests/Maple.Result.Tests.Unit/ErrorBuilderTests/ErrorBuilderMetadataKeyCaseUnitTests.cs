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

namespace Maple.Results.Tests.Unit.ErrorBuilderTests;

// Keys that differ only in case are the same member of a problem details object (clients often read JSON case-insensitively, and naming
// policies change the case), so they are one entry: a key in another case replaces the entry, and the spelling of the call is stored (#349).
public class ErrorBuilderMetadataKeyCaseUnitTests
{
    private static ErrorBuilder CreateSut()
    {
        return ErrorBuilder.NotFound(ErrorUri.None, "Missing")
            .WithMetadata("RetryAfter", 30);
    }

    #region WithMetadata(string, object?)

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromExistingKey_ReplacesTheEntryAndItsSpelling()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.WithMetadata("retryAfter", 60).Build().Metadata;

        // Assert
        result.Keys.ShouldBe(["retryAfter"]);
        result["retryAfter"].ShouldBe(60);
    }

    [Fact]
    public void WithMetadata_SameKey_ReplacesTheValue()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.WithMetadata("RetryAfter", 60).Build().Metadata;

        // Assert
        result.Count.ShouldBe(1);
        result["RetryAfter"].ShouldBe(60);
    }

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromRemovedKey_AddsTheEntry()
    {
        // Arrange
        var sut = CreateSut().WithoutMetadata("RetryAfter");

        // Act
        var result = sut.WithMetadata("retryAfter", 60).Build().Metadata;

        // Assert
        result.Count.ShouldBe(1);
        result["retryAfter"].ShouldBe(60);
    }

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromKeyOfTheSourceError_ReplacesTheEntryAndItsSpelling()
    {
        // Arrange
        var sut = CreateSut().Build().ToBuilder();

        // Act
        var result = sut.WithMetadata("RETRYAFTER", 60).Build().Metadata;

        // Assert
        result.Keys.ShouldBe(["RETRYAFTER"]);
        result["RETRYAFTER"].ShouldBe(60);
    }

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCase_KeepsAnErrorBuiltBefore()
    {
        // Arrange
        var sut = CreateSut();
        var builtBefore = sut.Build();

        // Act
        _ = sut.WithMetadata("retryAfter", 60);

        // Assert
        builtBefore.Metadata.Keys.ShouldBe(["RetryAfter"]);
        builtBefore.Metadata["RetryAfter"].ShouldBe(30);
    }

    #endregion

    #region WithMetadata(params ReadOnlySpan<(string, object?)>)

    [Fact]
    public void WithMetadataTuples_KeyDifferingOnlyInCaseFromExistingKey_ReplacesTheEntryAndItsSpelling()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.WithMetadata(("limit", 100), ("retryAfter", 60)).Build().Metadata;

        // Assert
        result.Keys.ShouldBe(["limit", "retryAfter"], ignoreOrder: true);
        result["retryAfter"].ShouldBe(60);
    }

    [Fact]
    public void WithMetadataTuples_KeysDifferingOnlyInCaseInOneCall_KeepsTheLastEntry()
    {
        // Arrange
        var sut = ErrorBuilder.NotFound(ErrorUri.None, "Missing");

        // Act
        var result = sut.WithMetadata(("Limit", 100), ("remaining", 0), ("limit", 200)).Build().Metadata;

        // Assert
        result.Keys.ShouldBe(["limit", "remaining"], ignoreOrder: true);
        result["limit"].ShouldBe(200);
    }

    [Fact]
    public void WithMetadataTuples_SameKeyRepeatedInOneCall_TakesTheLastValue()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.WithMetadata(("RetryAfter", 60), ("RetryAfter", 90)).Build().Metadata;

        // Assert
        result.Count.ShouldBe(1);
        result["RetryAfter"].ShouldBe(90);
    }

    #endregion

    #region WithMetadata(IEnumerable<KeyValuePair<string, object?>>)

    [Fact]
    public void WithMetadataEnumerable_KeyDifferingOnlyInCaseFromExistingKey_ReplacesTheEntryAndItsSpelling()
    {
        // Arrange
        var metadata = new Dictionary<string, object?> { ["limit"] = 100, ["retryAfter"] = 60 };
        var sut = CreateSut();

        // Act
        var result = sut.WithMetadata(metadata).Build().Metadata;

        // Assert
        result.Keys.ShouldBe(["limit", "retryAfter"], ignoreOrder: true);
        result["retryAfter"].ShouldBe(60);
    }

    [Fact]
    public void WithMetadataEnumerable_KeysDifferingOnlyInCaseInOneCall_KeepsTheLastEntry()
    {
        // Arrange
        var metadata = new List<KeyValuePair<string, object?>>
        {
            new("Limit", 100),
            new("limit", 200)
        };

        var sut = ErrorBuilder.NotFound(ErrorUri.None, "Missing");

        // Act
        var result = sut.WithMetadata(metadata).Build().Metadata;

        // Assert
        result.Keys.ShouldBe(["limit"]);
        result["limit"].ShouldBe(200);
    }

    [Fact]
    public void WithMetadataEnumerable_SameKeyRepeatedInOneCall_TakesTheLastValue()
    {
        // Arrange
        var metadata = new List<KeyValuePair<string, object?>>
        {
            new("RetryAfter", 60),
            new("RetryAfter", 90)
        };

        var sut = CreateSut();

        // Act
        var result = sut.WithMetadata(metadata).Build().Metadata;

        // Assert
        result.Count.ShouldBe(1);
        result["RetryAfter"].ShouldBe(90);
    }

    #endregion

    #region WithoutMetadata(string)

    [Fact]
    public void WithoutMetadata_KeyInDifferentCase_RemovesTheEntry()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.WithoutMetadata("retryAfter").Build().Metadata;

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public void WithoutMetadata_KeyInDifferentCaseOfTheSourceError_RemovesTheEntry()
    {
        // Arrange
        var sut = CreateSut().Build().ToBuilder();

        // Act
        var result = sut.WithoutMetadata("RETRYAFTER").Build().Metadata;

        // Assert
        result.ShouldBeEmpty();
    }

    #endregion
}
