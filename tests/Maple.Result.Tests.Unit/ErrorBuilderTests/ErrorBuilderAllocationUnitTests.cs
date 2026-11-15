// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Result.Tests.Unit.TestingInfrastructure.Helpers;
using System;
using System.Linq;
using Sut = Maple.Result.ErrorBuilder;

namespace Maple.Result.Tests.Unit.ErrorBuilderTests;

public class ErrorBuilderAllocationUnitTests
{
    private static readonly string[] Keys = Enumerable.Range(0, 10).Select(i => $"key{i}").ToArray();
    private static readonly object Value = "value";
    private static readonly ErrorDetail Detail = new("#/email", "The email is required.");

    #region Build (issue #142)

    [Fact]
    public void Build_PendingMetadata_AllocatesTheSameForOneAndTenEntries()
    {
        // Arrange
        var oneEntry = AllocatedBytesOfBuild(builder => builder.WithMetadata(Keys[0], Value));

        // Act
        var tenEntries = AllocatedBytesOfBuild(builder =>
        {
            foreach (var key in Keys)
                builder.WithMetadata(key, Value);
        });

        // Assert
        tenEntries.ShouldBe(oneEntry);
    }

    [Fact]
    public void Build_PendingErrorDetails_AllocatesTheSameForOneAndTenDetails()
    {
        // Arrange
        var oneDetail = AllocatedBytesOfBuild(builder => builder.AddErrorDetail(Detail));

        // Act
        var tenDetails = AllocatedBytesOfBuild(builder =>
        {
            for (var i = 0; i < 10; i++)
                builder.AddErrorDetail(Detail);
        });

        // Assert
        tenDetails.ShouldBe(oneDetail);
    }

    #endregion

    #region helper methods

    /// <summary>
    ///     The bytes that the first <see cref="Sut.Build" /> after the <paramref name="change" /> allocates: the allocations of creating and
    ///     changing a builder and building it, minus those of creating and changing it alone.
    /// </summary>
    private static long AllocatedBytesOfBuild(Action<Sut> change)
    {
        var withBuild = AllocationHelper.AllocatedBytes(() =>
        {
            var builder = CreateBuilder();
            change(builder);
            builder.Build();
        });

        var withoutBuild = AllocationHelper.AllocatedBytes(() =>
        {
            var builder = CreateBuilder();
            change(builder);
        });

        return withBuild - withoutBuild;
    }

    private static Sut CreateBuilder()
    {
        return Sut.Validation(ErrorUri.None(), "Test title");
    }

    #endregion
}
