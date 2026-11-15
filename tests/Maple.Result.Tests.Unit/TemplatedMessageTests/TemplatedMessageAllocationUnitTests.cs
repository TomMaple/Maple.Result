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
using System.Collections.Generic;

namespace Maple.Result.Tests.Unit.TemplatedMessageTests;

public class TemplatedMessageAllocationUnitTests
{
    private static readonly Dictionary<string, object?> EmptyDictionary = new();

    #region no parameters (issue #128)

    [Fact]
    public void Ctor_EmptyTupleArray_AllocatesOnlyTheMessage()
    {
        // Arrange
        var expected = AllocationHelper.AllocatedBytes(() => _ = new TemplatedMessage("errors.length"));

        // Act
        var allocated = AllocationHelper.AllocatedBytes(() => _ = new TemplatedMessage("errors.length", Array.Empty<(string Key, object? Value)>()));

        // Assert
        allocated.ShouldBe(expected);
    }

    [Fact]
    public void Ctor_EmptyDictionary_AllocatesOnlyTheMessage()
    {
        // Arrange
        var expected = AllocationHelper.AllocatedBytes(() => _ = new TemplatedMessage("errors.length"));

        // Act
        var allocated = AllocationHelper.AllocatedBytes(() => _ = new TemplatedMessage("errors.length", EmptyDictionary));

        // Assert
        allocated.ShouldBe(expected);
    }

    #endregion
}
