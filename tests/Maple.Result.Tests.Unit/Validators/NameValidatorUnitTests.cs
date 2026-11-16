// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Validators;

namespace Maple.Results.Tests.Unit.Validators;

public class NameValidatorUnitTests
{
    [Theory]
    [InlineData("type")]
    [InlineData("retry after")]
    [InlineData("a")]
    [InlineData("x-y")]
    [InlineData("_")]
    public void IsValid_Name_ReturnsTrue(string name)
    {
        // Act
        var result = NameValidator.IsValid(name);

        // Assert
        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" type")]
    [InlineData("type ")]
    [InlineData("\ttype")]
    [InlineData("type\n")]
    [InlineData("\u00A0x")]
    public void IsValid_EmptyOrPaddedName_ReturnsFalse(string name)
    {
        // Act
        var result = NameValidator.IsValid(name);

        // Assert
        result.ShouldBeFalse();
    }
}
