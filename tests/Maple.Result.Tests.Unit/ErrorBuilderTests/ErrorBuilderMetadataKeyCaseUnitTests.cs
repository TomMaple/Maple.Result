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

// Keys that differ only in case would be the same member of a problem details object (clients often read JSON
// case-insensitively, and naming policies change the case), so the builder rejects them; the same key replaces the value.
public class ErrorBuilderMetadataKeyCaseUnitTests
{
    private static ErrorBuilder CreateSut()
    {
        return ErrorBuilder.NotFound(ErrorUri.None(), "Missing")
            .WithMetadata("RetryAfter", 30);
    }

    #region WithMetadata(string, object?)

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromExistingKey_ThrowsArgumentException()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata("retryAfter", 60));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("key");
    }

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromExistingKey_MessageNamesBothKeys()
    {
        // Arrange
        const string ExpectedMessage = "The metadata key ‘retryAfter’ differs only in case from the existing key ‘RetryAfter’. "
            + "Keys that differ only in case would be the same member of a problem details object; use ‘RetryAfter’ to replace "
            + "the value, or remove it first. (Parameter 'key')";

        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata("retryAfter", 60));

        // Assert
        exception.ShouldNotBeNull().Message.ShouldBe(ExpectedMessage);
    }

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromExistingKey_LeavesTheBuilderUnchanged()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        _ = Record.Exception(() => sut.WithMetadata("retryAfter", 60));

        // Assert
        var result = sut.Build().Metadata;
        result.Count.ShouldBe(1);
        result["RetryAfter"].ShouldBe(30);
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
    public void WithMetadata_KeyDifferingOnlyInCaseFromKeyOfTheSourceError_ThrowsArgumentException()
    {
        // Arrange
        var sut = CreateSut().Build().ToBuilder();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata("RETRYAFTER", 60));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("key");
    }

    #endregion

    #region WithMetadata(params ReadOnlySpan<(string, object?)>)

    [Fact]
    public void WithMetadataTuples_KeyDifferingOnlyInCaseFromExistingKey_ThrowsArgumentException()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(("limit", 100), ("retryAfter", 60)));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("metadata");
    }

    [Fact]
    public void WithMetadataTuples_KeysDifferingOnlyInCaseInOneCall_ThrowsArgumentException()
    {
        // Arrange
        var sut = ErrorBuilder.NotFound(ErrorUri.None(), "Missing");

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(("Limit", 100), ("remaining", 0), ("limit", 200)));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("metadata");
    }

    [Fact]
    public void WithMetadataTuples_KeysDifferingOnlyInCaseInOneCall_MessageNamesBothKeys()
    {
        // Arrange
        const string ExpectedMessage = "The metadata keys ‘Limit’ and ‘limit’ differ only in case. "
            + "Keys that differ only in case would be the same member of a problem details object. (Parameter 'metadata')";

        var sut = ErrorBuilder.NotFound(ErrorUri.None(), "Missing");

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(("Limit", 100), ("remaining", 0), ("limit", 200)));

        // Assert
        exception.ShouldNotBeNull().Message.ShouldBe(ExpectedMessage);
    }

    [Fact]
    public void WithMetadataTuples_KeyDifferingOnlyInCase_LeavesTheBuilderUnchanged()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        _ = Record.Exception(() => sut.WithMetadata(("limit", 100), ("retryAfter", 60)));

        // Assert
        var result = sut.Build().Metadata;
        result.Count.ShouldBe(1);
        result["RetryAfter"].ShouldBe(30);
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
    public void WithMetadataEnumerable_KeyDifferingOnlyInCaseFromExistingKey_ThrowsArgumentException()
    {
        // Arrange
        var metadata = new Dictionary<string, object?> { ["limit"] = 100, ["retryAfter"] = 60 };
        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(metadata));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("metadata");
    }

    [Fact]
    public void WithMetadataEnumerable_KeysDifferingOnlyInCaseInOneCall_ThrowsArgumentException()
    {
        // Arrange
        var metadata = new List<KeyValuePair<string, object?>>
        {
            new("Limit", 100),
            new("limit", 200)
        };

        var sut = ErrorBuilder.NotFound(ErrorUri.None(), "Missing");

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(metadata));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("metadata");
    }

    [Fact]
    public void WithMetadataEnumerable_KeyDifferingOnlyInCase_LeavesTheBuilderUnchanged()
    {
        // Arrange
        var metadata = new Dictionary<string, object?> { ["limit"] = 100, ["retryAfter"] = 60 };
        var sut = CreateSut();

        // Act
        _ = Record.Exception(() => sut.WithMetadata(metadata));

        // Assert
        var result = sut.Build().Metadata;
        result.Count.ShouldBe(1);
        result["RetryAfter"].ShouldBe(30);
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
}
