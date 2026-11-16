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

namespace Maple.Results.Tests.Unit;

public class ErrorDetailUnitTests
{
    #region ctor

    [Fact]
    public void Equals_DetailWithTemplateIdOnlyVsDetailAddedToError_AreEqual()
    {
        // Arrange
        var detail = new ErrorDetail("#/email", "The email is required.", new TemplatedMessage("errors.email.required"));
        var added = Error.Validation(ErrorUri.None, "Test title")
            .WithErrorDetail("#/email", "The email is required.", "errors.email.required")
            .ErrorDetails[0];

        // Act & Assert
        detail.Equals(added).ShouldBeTrue();
        detail.GetHashCode().ShouldBe(added.GetHashCode());
    }

    [Fact]
    public void Ctor_AllMembers_SetsThem()
    {
        // Arrange
        var detailTemplated = new TemplatedMessage("message-id");

        // Act
        var sut = new ErrorDetail("#/name", "The name is required.", detailTemplated);

        // Assert
        sut.PropertyPointer.ShouldBe("#/name");
        sut.Detail.ShouldBe("The name is required.");
        sut.DetailTemplated.ShouldBeSameAs(detailTemplated);
    }

    [Fact]
    public void Ctor_NullDetail_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => new ErrorDetail("#/name", null!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        ((ArgumentNullException)exception).ParamName.ShouldBe("Detail");
    }

    [Fact]
    public void Ctor_NullPropertyPointerAndTemplate_IsAllowed()
    {
        // Act
        var sut = new ErrorDetail(null, "The request is invalid.");

        // Assert
        sut.PropertyPointer.ShouldBeNull();
        sut.DetailTemplated.ShouldBeNull();
    }

    [Theory]
    [InlineData("email")]
    [InlineData("Items[0].Name")]
    [InlineData("/a~2b")]
    [InlineData("#email")]
    public void Ctor_InvalidPropertyPointer_ThrowsArgumentException(string propertyPointer)
    {
        // Act
        var exception = Record.Exception(() => new ErrorDetail(propertyPointer, "The value is invalid."));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        ((ArgumentException)exception).ParamName.ShouldBe("PropertyPointer");
    }

    [Fact]
    public void Ctor_InvalidPropertyPointer_MessageShowsValue()
    {
        // Act
        var exception = Record.Exception(() => new ErrorDetail("Email", "The email address is required."));

        // Assert
        exception.ShouldNotBeNull();
        exception.Message.ShouldContain("‘Email’");
    }

    [Theory]
    [InlineData("")]
    [InlineData("/email")]
    [InlineData("#/email")]
    [InlineData("#")]
    public void Ctor_ValidPropertyPointer_StoresItUnchanged(string propertyPointer)
    {
        // Act
        var sut = new ErrorDetail(propertyPointer, "The value is invalid.");

        // Assert
        sut.PropertyPointer.ShouldBe(propertyPointer);
    }

    [Fact]
    public void Equals_StringFormVsUriFragmentForm_AreNotEqual()
    {
        // Arrange
        var stringForm = new ErrorDetail("/email", "The email address is required.");
        var uriFragmentForm = new ErrorDetail("#/email", "The email address is required.");

        // Act
        var result = stringForm.Equals(uriFragmentForm);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void Deconstruct_Always_ReturnsAllMembers()
    {
        // Arrange
        var sut = new ErrorDetail("#/name", "The name is required.");

        // Act
        var (propertyPointer, detail, detailTemplated) = sut;

        // Assert
        propertyPointer.ShouldBe("#/name");
        detail.ShouldBe("The name is required.");
        detailTemplated.ShouldBeNull();
    }

    #endregion

    #region empty texts (issue #203)

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\n")]
    public void Ctor_EmptyOrWhitespaceDetail_ThrowsArgumentException(string detail)
    {
        // Act
        var exception = Record.Exception(() => new ErrorDetail("/name", detail));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        ((ArgumentException)exception).ParamName.ShouldBe("Detail");
    }

    [Fact]
    public void Ctor_DetailWithSurroundingWhiteSpace_StoresItUnchanged()
    {
        // Act
        var sut = new ErrorDetail("/name", " Too short. ");

        // Assert
        sut.Detail.ShouldBe(" Too short. ");
    }

    #endregion
}
