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
using System.Diagnostics;
using System.Linq;

namespace Maple.Results.Tests.Unit.ErrorBuilderTests;

// The rule of ErrorBuilderMetadataKeyCaseUnitTests with many keys (#234): the extension members of a response from another service
// become metadata, so the number of keys is not trusted, and the check of a key that differs only in case must not scan the other keys.
public class ErrorBuilderMetadataKeyCaseManyKeysUnitTests
{
    private const int ManyKeys = 100;

    // About 6 seconds with a scan of the existing keys for each key, a few milliseconds with a lookup;
    // the limit is generous for slow CI machines.
    private const int HostileKeyCount = 40_000;
    private static readonly TimeSpan Maximum = TimeSpan.FromSeconds(2);

    private static ErrorBuilder CreateSut(int keyCount = ManyKeys)
    {
        var sut = ErrorBuilder.NotFound(ErrorUri.None, "Missing");
        for (var i = 0; i < keyCount; i++)
            sut.WithMetadata($"key{i}", i);

        return sut;
    }

    private static (string Key, object? Value)[] CreateTuples(int keyCount)
    {
        return Enumerable.Range(0, keyCount)
            .Select(i => ($"key{i}", (object?)i))
            .ToArray();
    }

    private static List<KeyValuePair<string, object?>> CreatePairs(int keyCount)
    {
        return Enumerable.Range(0, keyCount)
            .Select(i => new KeyValuePair<string, object?>($"key{i}", i))
            .ToList();
    }

    #region linear time

    [Fact]
    public void WithMetadata_ManyKeysOneByOne_AreAddedInLinearTime()
    {
        // Arrange
        var sut = ErrorBuilder.NotFound(ErrorUri.None, "Missing");
        var stopwatch = Stopwatch.StartNew();

        // Act
        for (var i = 0; i < HostileKeyCount; i++)
            sut.WithMetadata($"key{i}", i);

        // Assert
        stopwatch.Elapsed.ShouldBeLessThan(Maximum);
        sut.Build().Metadata.Count.ShouldBe(HostileKeyCount);
    }

    [Fact]
    public void WithMetadataTuples_ManyKeysInOneCall_AreAddedInLinearTime()
    {
        // Arrange
        var metadata = CreateTuples(HostileKeyCount);
        var sut = ErrorBuilder.NotFound(ErrorUri.None, "Missing");
        var stopwatch = Stopwatch.StartNew();

        // Act
        sut.WithMetadata(metadata);

        // Assert
        stopwatch.Elapsed.ShouldBeLessThan(Maximum);
        sut.Build().Metadata.Count.ShouldBe(HostileKeyCount);
    }

    [Fact]
    public void WithMetadataEnumerable_ManyKeysInOneCall_AreAddedInLinearTime()
    {
        // Arrange
        var metadata = CreatePairs(HostileKeyCount);
        var sut = ErrorBuilder.NotFound(ErrorUri.None, "Missing");
        var stopwatch = Stopwatch.StartNew();

        // Act
        sut.WithMetadata(metadata);

        // Assert
        stopwatch.Elapsed.ShouldBeLessThan(Maximum);
        sut.Build().Metadata.Count.ShouldBe(HostileKeyCount);
    }

    #endregion

    #region a key that differs only in case from one of many keys

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromOneOfManyKeys_ThrowsArgumentException()
    {
        // Arrange
        const string ExpectedMessage = "The metadata key ‘KEY50’ differs only in case from the existing key ‘key50’. "
            + "Keys that differ only in case would be the same member of a problem details object; use ‘key50’ to replace "
            + "the value, or remove it first. (Parameter 'key')";

        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata("KEY50", 0));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().Message.ShouldBe(ExpectedMessage);
    }

    [Fact]
    public void WithMetadataTuples_KeyDifferingOnlyInCaseFromOneOfManyKeys_ThrowsArgumentException()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(("other", 1), ("KEY50", 0)));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("metadata");
    }

    [Fact]
    public void WithMetadataEnumerable_KeyDifferingOnlyInCaseFromOneOfManyKeys_ThrowsArgumentException()
    {
        // Arrange
        var metadata = new List<KeyValuePair<string, object?>> { new("other", 1), new("KEY50", 0) };
        var sut = CreateSut();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(metadata));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("metadata");
    }

    [Fact]
    public void WithMetadata_SameKeyAsOneOfManyKeys_ReplacesTheValue()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.WithMetadata("key50", -1).Build().Metadata;

        // Assert
        result.Count.ShouldBe(ManyKeys);
        result["key50"].ShouldBe(-1);
    }

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromOneOfManyKeys_LeavesTheBuilderUnchanged()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        _ = Record.Exception(() => sut.WithMetadata("KEY50", 0));

        // Assert
        var result = sut.Build().Metadata;
        result.Count.ShouldBe(ManyKeys);
        result.ContainsKey("KEY50").ShouldBeFalse();
    }

    #endregion

    #region keys that differ only in case among many keys of one call

    [Fact]
    public void WithMetadataTuples_KeysDifferingOnlyInCaseAmongManyKeysOfOneCall_ThrowsArgumentException()
    {
        // Arrange
        const string ExpectedMessage = "The metadata keys ‘key50’ and ‘KEY50’ differ only in case. "
            + "Keys that differ only in case would be the same member of a problem details object. (Parameter 'metadata')";

        var metadata = CreateTuples(ManyKeys).Append(("KEY50", 0)).ToArray();
        var sut = ErrorBuilder.NotFound(ErrorUri.None, "Missing");

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(metadata));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().Message.ShouldBe(ExpectedMessage);
    }

    [Fact]
    public void WithMetadataEnumerable_KeysDifferingOnlyInCaseAmongManyKeysOfOneCall_ThrowsArgumentException()
    {
        // Arrange
        const string ExpectedMessage = "The metadata keys ‘key50’ and ‘KEY50’ differ only in case. "
            + "Keys that differ only in case would be the same member of a problem details object. (Parameter 'metadata')";

        var metadata = CreatePairs(ManyKeys);
        metadata.Add(new KeyValuePair<string, object?>("KEY50", 0));
        var sut = ErrorBuilder.NotFound(ErrorUri.None, "Missing");

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(metadata));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().Message.ShouldBe(ExpectedMessage);
    }

    [Fact]
    public void WithMetadataTuples_SameKeyRepeatedAmongManyKeysOfOneCall_TakesTheLastValue()
    {
        // Arrange
        var metadata = CreateTuples(ManyKeys).Append(("key50", -1)).ToArray();
        var sut = ErrorBuilder.NotFound(ErrorUri.None, "Missing");

        // Act
        var result = sut.WithMetadata(metadata).Build().Metadata;

        // Assert
        result.Count.ShouldBe(ManyKeys);
        result["key50"].ShouldBe(-1);
    }

    #endregion

    #region removed keys

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromRemovedKey_AddsTheEntry()
    {
        // Arrange
        var sut = CreateSut().WithoutMetadata("key50");

        // Act
        var result = sut.WithMetadata("KEY50", 0).Build().Metadata;

        // Assert
        result.Count.ShouldBe(ManyKeys);
        result["KEY50"].ShouldBe(0);
    }

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseAfterRemovingItInThatCase_ThrowsArgumentException()
    {
        // Arrange: the removal is ordinal, so "KEY50" removes nothing and "key50" stays.
        var sut = CreateSut().WithoutMetadata("KEY50");

        // Act
        var exception = Record.Exception(() => sut.WithMetadata("KEY50", 0));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("key");
    }

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseAfterRemovingAllKeys_AddsTheEntry()
    {
        // Arrange
        var sut = CreateSut().WithoutMetadata();

        // Act
        var result = sut.WithMetadata("KEY50", 0).Build().Metadata;

        // Assert
        result.Count.ShouldBe(1);
        result["KEY50"].ShouldBe(0);
    }

    #endregion

    #region built errors

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromOneOfManyKeysOfTheSourceError_ThrowsArgumentException()
    {
        // Arrange
        var sut = CreateSut().Build().ToBuilder();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata("KEY50", 0));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("key");
    }

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseAfterBuild_ThrowsArgumentException()
    {
        // Arrange
        var sut = CreateSut();
        _ = sut.Build();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata("KEY50", 0));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("key");
    }

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromKeyAddedAfterBuild_ThrowsArgumentException()
    {
        // Arrange
        var sut = CreateSut();
        _ = sut.Build();
        sut.WithMetadata("added", 1);

        // Act
        var exception = Record.Exception(() => sut.WithMetadata("ADDED", 0));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("key");
    }

    [Fact]
    public void ErrorWithMetadata_KeyDifferingOnlyInCaseFromOneOfManyKeys_ThrowsArgumentException()
    {
        // Arrange
        var sut = CreateSut().Build();

        // Act
        var exception = Record.Exception(() => sut.WithMetadata("KEY50", 0));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("key");
    }

    #endregion
}
