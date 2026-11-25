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

// The rule of ErrorBuilderMetadataKeyCaseUnitTests with many keys (#234, #349): the extension members of a response from another service
// become metadata, so the number of keys is not trusted, and finding the entry of a key in another case must not scan the other keys.
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
    public void WithMetadata_KeyDifferingOnlyInCaseFromOneOfManyKeys_ReplacesTheEntryAndItsSpelling()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.WithMetadata("KEY50", -1).Build().Metadata;

        // Assert
        result.Count.ShouldBe(ManyKeys);
        result.Keys.Single(IsKey50).ShouldBe("KEY50");
        result["KEY50"].ShouldBe(-1);
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
    public void WithoutMetadata_KeyDifferingOnlyInCaseFromOneOfManyKeys_RemovesTheEntry()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var result = sut.WithoutMetadata("KEY50").Build().Metadata;

        // Assert
        result.Count.ShouldBe(ManyKeys - 1);
        result.Keys.Any(IsKey50).ShouldBeFalse();
    }

    #endregion

    #region keys that differ only in case among many keys of one call

    [Fact]
    public void WithMetadataTuples_KeysDifferingOnlyInCaseAmongManyKeysOfOneCall_KeepsTheLastEntry()
    {
        // Arrange
        var metadata = CreateTuples(ManyKeys)
            .Append(("KEY50", -1))
            .ToArray();

        var sut = ErrorBuilder.NotFound(ErrorUri.None, "Missing");

        // Act
        var result = sut.WithMetadata(metadata).Build().Metadata;

        // Assert
        result.Count.ShouldBe(ManyKeys);
        result.Keys.Single(IsKey50).ShouldBe("KEY50");
        result["KEY50"].ShouldBe(-1);
    }

    [Fact]
    public void WithMetadataEnumerable_KeysDifferingOnlyInCaseAmongManyKeysOfOneCall_KeepsTheLastEntry()
    {
        // Arrange
        var metadata = CreatePairs(ManyKeys);
        metadata.Add(new KeyValuePair<string, object?>("KEY50", -1));

        var sut = ErrorBuilder.NotFound(ErrorUri.None, "Missing");

        // Act
        var result = sut.WithMetadata(metadata).Build().Metadata;

        // Assert
        result.Count.ShouldBe(ManyKeys);
        result.Keys.Single(IsKey50).ShouldBe("KEY50");
        result["KEY50"].ShouldBe(-1);
    }

    #endregion

    #region built errors

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseFromOneOfManyKeysOfTheSourceError_ReplacesTheEntryAndItsSpelling()
    {
        // Arrange
        var sut = CreateSut().Build().ToBuilder();

        // Act
        var result = sut.WithMetadata("KEY50", -1).Build().Metadata;

        // Assert
        result.Count.ShouldBe(ManyKeys);
        result.Keys.Single(IsKey50).ShouldBe("KEY50");
        result["KEY50"].ShouldBe(-1);
    }

    [Fact]
    public void WithMetadata_KeyDifferingOnlyInCaseAfterBuild_KeepsTheBuiltError()
    {
        // Arrange
        var sut = CreateSut();
        var result = sut.Build().Metadata;

        // Act
        _ = sut.WithMetadata("KEY50", -1);

        // Assert
        result.Count.ShouldBe(ManyKeys);
        result.Keys.Single(IsKey50).ShouldBe("key50");
        result["key50"].ShouldBe(50);
    }

    [Fact]
    public void ErrorWithMetadata_KeyDifferingOnlyInCaseFromOneOfManyKeys_ReplacesTheEntryAndItsSpelling()
    {
        // Arrange
        var sut = CreateSut().Build();

        // Act
        var result = sut.WithMetadata("KEY50", -1).Metadata;

        // Assert
        result.Count.ShouldBe(ManyKeys);
        result.Keys.Single(IsKey50).ShouldBe("KEY50");
        result["KEY50"].ShouldBe(-1);
    }

    #endregion

    private static bool IsKey50(string key)
    {
        return string.Equals(key, "key50", StringComparison.OrdinalIgnoreCase);
    }
}
