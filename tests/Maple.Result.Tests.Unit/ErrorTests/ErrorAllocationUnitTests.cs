// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Tests.Unit.TestingInfrastructure.Helpers;
using Sut = Maple.Results.Error;

namespace Maple.Results.Tests.Unit.ErrorTests;

public class ErrorAllocationUnitTests
{
    #region metadata equality (issue #128)

    [Fact]
    public void Equals_ErrorsWithMetadata_AllocatesNoMoreThanWithoutMetadata()
    {
        // Arrange
        var first = CreateError().WithMetadata(("limit", 100), ("remaining", 0));
        var second = CreateError().WithMetadata(("limit", 100), ("remaining", 0));
        var firstWithoutMetadata = CreateError();
        var secondWithoutMetadata = CreateError();
        var expected = AllocationHelper.AllocatedBytes(() => _ = firstWithoutMetadata.Equals(secondWithoutMetadata));

        // Act
        var allocated = AllocationHelper.AllocatedBytes(() => _ = first.Equals(second));

        // Assert
        allocated.ShouldBe(expected);
    }

    [Fact]
    public void GetHashCode_ErrorWithMetadata_AllocatesNoMoreThanWithoutMetadata()
    {
        // Arrange
        var sut = CreateError().WithMetadata(("limit", 100), ("remaining", 0));
        var withoutMetadata = CreateError();
        var expected = AllocationHelper.AllocatedBytes(() => _ = withoutMetadata.GetHashCode());

        // Act
        var allocated = AllocationHelper.AllocatedBytes(() => _ = sut.GetHashCode());

        // Assert
        allocated.ShouldBe(expected);
    }

    #endregion

    #region helper methods

    private static Sut CreateError()
    {
        return Sut.Validation(ErrorUri.None(), "Test title");
    }

    #endregion
}
