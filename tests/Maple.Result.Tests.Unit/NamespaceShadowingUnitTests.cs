// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results;

// A namespace under Maple.* but outside Maple.Results.*, like the code of an application or of another Maple package.
// With the former namespace Maple.Result, `Result` resolved to the namespace here, and `Result.Success()` did not compile
// (CS0234). This file is the compile-time check that it does not happen with Maple.Results.
namespace Maple.NamespaceShadowing.Tests.Unit;

public class NamespaceShadowingUnitTests
{
    [Fact]
    public void Success_CalledFromAnotherMapleNamespace_ReturnsSuccessResult()
    {
        // Act
        var result = Result.Success();

        // Assert
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void FromValue_CalledFromAnotherMapleNamespace_ReturnsSuccessResult()
    {
        // Act
        var result = Result.FromValue(1);

        // Assert
        result.Value.ShouldBe(1);
    }

    [Fact]
    public void FromError_CalledFromAnotherMapleNamespace_ReturnsFailedResult()
    {
        // Arrange
        var error = Error.NotFound(ErrorUri.None, "Missing");

        // Act
        var result = Result.FromError(error);

        // Assert
        result.Error.ShouldBe(error);
    }
}
