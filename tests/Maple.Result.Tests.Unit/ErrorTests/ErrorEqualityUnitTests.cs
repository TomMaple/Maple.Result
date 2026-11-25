// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Sut = Maple.Results.Error;

namespace Maple.Results.Tests.Unit.ErrorTests;

public class ErrorEqualityUnitTests
{
    #region value equality

    [Fact]
    public void Equals_FactoryAndBuilderWithTemplateIdOnly_AreEqual()
    {
        // Arrange
        var fromFactory = Sut.Validation(ErrorUri.None, "Test title", detailTemplated: new TemplatedMessage("message-id"));
        var fromBuilder = ErrorBuilder.Validation(ErrorUri.None, "Test title")
            .WithDetailTemplated("message-id")
            .Build();

        // Act & Assert
        fromFactory.Equals(fromBuilder).ShouldBeTrue();
        fromFactory.GetHashCode().ShouldBe(fromBuilder.GetHashCode());
    }

    [Fact]
    public void Equals_TwoIdenticallyBuiltErrorsWithoutDetails_AreEqual()
    {
        // Arrange
        var first = CreateError();
        var second = CreateError();

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_TwoIdenticallyBuiltErrorsWithDetails_AreEqual()
    {
        // Arrange
        var first = CreateError()
            .WithErrorDetail("#/first", "First detail")
            .WithErrorDetail("#/second", "Second detail");

        var second = CreateError()
            .WithErrorDetail("#/first", "First detail")
            .WithErrorDetail("#/second", "Second detail");

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_TwoIdenticallyBuiltErrorsWithTemplatedDetails_AreEqual()
    {
        // Arrange
        var first = CreateError()
            .WithErrorDetail("#/email", "The email is required.", "errors.email.required", ("minLength", 3));

        var second = CreateError()
            .WithErrorDetail("#/email", "The email is required.", "errors.email.required", ("minLength", 3));

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_ErrorsWithTemplatedDetailParametersInDifferentOrder_AreEqual()
    {
        // Arrange
        var first = CreateError()
            .WithErrorDetail("#/email", "The email is invalid.", "errors.email.length", ("minLength", 3), ("maxLength", 10));

        var second = CreateError()
            .WithErrorDetail("#/email", "The email is invalid.", "errors.email.length", ("maxLength", 10), ("minLength", 3));

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_ErrorsWithTemplatedDetailDifferentParamValues_AreNotEqual()
    {
        // Arrange
        var first = CreateError()
            .WithErrorDetail("#/email", "The email is required.", "errors.email.required", ("minLength", 3));

        var second = CreateError()
            .WithErrorDetail("#/email", "The email is required.", "errors.email.required", ("minLength", 5));

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_ErrorsWithTemplatedDetailDifferentParamKeys_AreNotEqual()
    {
        // Arrange
        var first = CreateError()
            .WithErrorDetail("#/email", "The email is required.", "errors.email.required", ("minLength", 3));

        var second = CreateError()
            .WithErrorDetail("#/email", "The email is required.", "errors.email.required", ("maxLength", 3));

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_ErrorsWithDifferentDetailContent_AreNotEqual()
    {
        // Arrange
        var first = CreateError().WithErrorDetail("#/property", "First detail");
        var second = CreateError().WithErrorDetail("#/property", "Different detail");

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_ErrorsWithDifferentNumberOfDetails_AreNotEqual()
    {
        // Arrange
        var first = CreateError().WithErrorDetail("#/property", "Detail");
        var second = CreateError()
            .WithErrorDetail("#/property", "Detail")
            .WithErrorDetail("#/other", "Detail");

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_ErrorsWithDifferentScalarProperties_AreNotEqual()
    {
        // Arrange
        var first = Sut.Validation(ErrorUri.Tag("tag:test.com,2024:test"), "Title");
        var second = Sut.Validation(ErrorUri.Tag("tag:test.com,2024:test"), "Different title");

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_UrisFromFactoryAndFromParse_AreEqual()
    {
        // Arrange
        var fromFactory = Sut.Validation(ErrorUri.Tag("tag:test.com,2024:test"), "Title",
            instanceUri: ErrorUri.Relative("/account/12345/msgs/abc"));
        var fromParse = Sut.Validation(ErrorUri.Parse("tag:test.com,2024:test"), "Title",
            instanceUri: ErrorUri.Parse("/account/12345/msgs/abc"));

        // Act & Assert
        fromFactory.Equals(fromParse).ShouldBeTrue();
        fromFactory.GetHashCode().ShouldBe(fromParse.GetHashCode());
    }

    [Fact]
    public void Equals_ErrorsWithDifferentTypeUri_AreNotEqual()
    {
        // Arrange
        var first = Sut.Validation(ErrorUri.Tag("tag:test.com,2024:test"), "Title");
        var second = Sut.Validation(ErrorUri.Tag("tag:test.com,2024:other"), "Title");

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Theory]
    [InlineData("/account/12345/msgs/other")]
    [InlineData(null)]
    public void Equals_ErrorsWithDifferentInstanceUri_AreNotEqual(string? otherInstanceUri)
    {
        // Arrange
        var first = Sut.Validation(ErrorUri.None, "Title", instanceUri: ErrorUri.Relative("/account/12345/msgs/abc"));
        var second = Sut.Validation(ErrorUri.None, "Title",
            instanceUri: otherInstanceUri is null ? null : ErrorUri.Relative(otherInstanceUri));

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    #endregion

    #region metadata equality

    [Fact]
    public void Equals_ErrorsWithSameMetadata_AreEqual()
    {
        // Arrange
        var first = CreateError().WithMetadata("retryAfterSeconds", 30);
        var second = CreateError().WithMetadata("retryAfterSeconds", 30);

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_ErrorsWithMetadataInDifferentOrder_AreEqual()
    {
        // Arrange
        var first = CreateError().WithMetadata("limit", 100).WithMetadata("remaining", 0);
        var second = CreateError().WithMetadata("remaining", 0).WithMetadata("limit", 100);

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_ErrorsWithDifferentMetadataValues_AreNotEqual()
    {
        // Arrange
        var first = CreateError().WithMetadata("limit", 100);
        var second = CreateError().WithMetadata("limit", 200);

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
        first.GetHashCode().ShouldNotBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_ErrorsWithDifferentMetadataKeys_AreNotEqual()
    {
        // Arrange
        var first = CreateError().WithMetadata("limit", 100);
        var second = CreateError().WithMetadata("quota", 100);

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    // Lookups ignore the case, but the spelling is serialized and shown by ToString, so it counts for equality (#349).
    [Fact]
    public void Equals_ErrorsWithMetadataKeysDifferingOnlyInCase_AreNotEqual()
    {
        // Arrange
        var first = CreateError().WithMetadata("traceId", 1);
        var second = CreateError().WithMetadata("TraceId", 1);

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_ErrorWithAndWithoutMetadata_AreNotEqual()
    {
        // Arrange
        var first = CreateError().WithMetadata("limit", 100);
        var second = CreateError();

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_ErrorsWithSameNullMetadataValue_AreEqual()
    {
        // Arrange
        var first = CreateError().WithMetadata("traceId", null);
        var second = CreateError().WithMetadata("traceId", null);

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_ErrorsWithNullAndNonNullMetadataValue_AreNotEqual()
    {
        // Arrange
        var first = CreateError().WithMetadata("traceId", null);
        var second = CreateError().WithMetadata("traceId", "abc");

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        second.Equals(first).ShouldBeFalse();
    }

    #endregion

    #region copies

    [Fact]
    public void ToBuilder_DetailAddedToCopy_DoesNotChangeOriginal()
    {
        // Arrange
        var original = CreateError().WithErrorDetail("#/original", "Original detail");

        // Act
        var copy = original.ToBuilder()
            .WithTitle("Changed title")
            .AddErrorDetail("#/added", "Added to the copy only")
            .Build();

        // Assert
        original.ErrorDetails.Count.ShouldBe(1);
        original.Title.ShouldBe("Test title");
        copy.ErrorDetails.Count.ShouldBe(2);
        copy.Title.ShouldBe("Changed title");
    }

    [Fact]
    public void WithErrorDetail_DetailAddedToOriginal_DoesNotChangeEarlierCopy()
    {
        // Arrange
        var original = CreateError().WithErrorDetail("#/original", "Original detail");
        var copy = original.ToBuilder().WithTitle("Changed title").Build();

        // Act
        var changed = original.WithErrorDetail("#/added", "Added to the original only");

        // Assert
        changed.ErrorDetails.Count.ShouldBe(2);
        original.ErrorDetails.Count.ShouldBe(1);
        copy.ErrorDetails.Count.ShouldBe(1);
    }

    [Fact]
    public void With_EmptyWithExpression_ReturnsEqualCopy()
    {
        // Arrange
        var original = CreateError().WithErrorDetail("#/original", "Original detail");

        // Act
        var copy = original with { };

        // Assert
        copy.ErrorDetails.Count.ShouldBe(1);
        (copy == original).ShouldBeTrue();
    }

    #endregion

    #region helper methods

    private static Sut CreateError()
    {
        return Sut.Validation(
            ErrorUri.Tag("tag:test.com,2024:test"),
            "Test title",
            "Test detail");
    }

    #endregion
}
