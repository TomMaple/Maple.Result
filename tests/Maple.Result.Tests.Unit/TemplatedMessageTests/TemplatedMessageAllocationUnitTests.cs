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

    #region equality (issue #128)

    [Fact]
    public void Equals_MessagesWithParams_AllocatesNothing()
    {
        // Arrange
        var first = new TemplatedMessage("errors.length", ("min", 3), ("max", 10));
        var second = new TemplatedMessage("errors.length", ("min", 3), ("max", 10));

        // Act
        var allocated = AllocationHelper.AllocatedBytes(() => _ = first.Equals(second));

        // Assert
        allocated.ShouldBe(0);
    }

    [Fact]
    public void GetHashCode_MessageWithParams_AllocatesNothing()
    {
        // Arrange
        var sut = new TemplatedMessage("errors.length", ("min", 3), ("max", 10));

        // Act
        var allocated = AllocationHelper.AllocatedBytes(() => _ = sut.GetHashCode());

        // Assert
        allocated.ShouldBe(0);
    }

    #endregion

    #region parameters of another message (issue #128)

    [Fact]
    public void Ctor_ParamsOfAnotherMessage_SharesTheDictionary()
    {
        // Arrange
        var source = new TemplatedMessage("errors.length", ("min", 3), ("max", 10));

        // Act
        var sut = new TemplatedMessage("errors.range", source.Params);

        // Assert
        sut.Params.ShouldBeSameAs(source.Params);
    }

    [Fact]
    public void Ctor_ParamsOfAnotherMessage_AllocatesOnlyTheMessage()
    {
        // Arrange
        var source = new TemplatedMessage("errors.length", ("min", 3), ("max", 10));
        var expected = AllocationHelper.AllocatedBytes(() => _ = new TemplatedMessage("errors.range"));

        // Act
        var allocated = AllocationHelper.AllocatedBytes(() => _ = new TemplatedMessage("errors.range", source.Params));

        // Assert
        allocated.ShouldBe(expected);
    }

    [Fact]
    public void Ctor_DictionaryWithSameContentAsParams_CopiesIt()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { ["min"] = 3, ["max"] = 10 };

        // Act
        var sut = new TemplatedMessage("errors.length", parameters);

        // Assert
        sut.Params.ShouldNotBeSameAs(parameters);
        sut.Params.ShouldBe(parameters, ignoreOrder: true);
    }

    #endregion
}
