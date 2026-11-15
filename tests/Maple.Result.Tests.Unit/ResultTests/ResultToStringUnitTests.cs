// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Maple.Results.Tests.Unit.ResultTests;

public class ResultToStringUnitTests
{
    private const string ExpectedError = "Error { Category = NotFound, TypeUri = about:blank, Title = Missing, Detail = , "
        + "DetailTemplated = , InstanceUri = , ErrorDetails = [], Metadata = {} }";

    #region Result

    [Fact]
    public void ToString_ResultSuccess_WritesNoMembers()
    {
        // Arrange
        const string Expected = "Result { }";
        var sut = Result.Success();

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe(Expected);
    }

    [Fact]
    public void ToString_ResultFailure_WritesOnlyTheError()
    {
        // Arrange
        const string Expected = "Result { Error = " + ExpectedError + " }";
        var sut = Result.FromError(Error.NotFound(ErrorUri.None, "Missing"));

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe(Expected);
    }

    #endregion

    #region Result<T>

    [Fact]
    public void ToString_ResultTSuccess_WritesOnlyTheValue()
    {
        // Arrange
        const string Expected = "Result { Value = 42 }";
        var sut = Result.FromValue(42);

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe(Expected);
    }

    [Fact]
    public void ToString_ResultTSuccessWithNullValue_WritesAnEmptyValue()
    {
        // Arrange
        const string Expected = "Result { Value =  }";
        var sut = Result.FromValue<string?>(null);

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe(Expected);
    }

    [Fact]
    public void ToString_ResultTValueTypeFailure_WritesOnlyTheError()
    {
        // Arrange
        const string Expected = "Result { Error = " + ExpectedError + " }";
        var sut = Result<int>.FromError(Error.NotFound(ErrorUri.None, "Missing"));

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe(Expected);
    }

    [Fact]
    public void ToString_ResultTReferenceTypeFailure_WritesOnlyTheError()
    {
        // Arrange
        const string Expected = "Result { Error = " + ExpectedError + " }";
        var sut = Result<string>.FromError(Error.NotFound(ErrorUri.None, "Missing"));

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe(Expected);
    }

    #endregion
}
