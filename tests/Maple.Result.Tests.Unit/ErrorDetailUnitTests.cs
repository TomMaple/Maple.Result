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

    #region Pointer (issue #230)

    [Fact]
    public void Pointer_NamesAndIndex_JoinsThemWithSlashes()
    {
        // Act
        var pointer = ErrorDetail.Pointer("items", 0, "name");

        // Assert
        pointer.ShouldBe("/items/0/name");
    }

    [Fact]
    public void Pointer_NoSegments_ReturnsEmptyPointer()
    {
        ErrorDetail.Pointer().ShouldBe(string.Empty);
    }

    [Fact]
    public void Pointer_EmptyName_IsAValidSegment()
    {
        ErrorDetail.Pointer("").ShouldBe("/");
    }

    [Theory]
    [InlineData("a/b", "/a~1b")]
    [InlineData("a~b", "/a~0b")]
    [InlineData("~1", "/~01")]
    [InlineData("~/", "/~0~1")]
    [InlineData("a b", "/a b")]
    public void Pointer_NameWithSpecialCharacters_EscapesTildeBeforeSlash(string name, string expected)
    {
        ErrorDetail.Pointer(name).ShouldBe(expected);
    }

    [Fact]
    public void Pointer_LongIndex_IsWrittenInDecimal()
    {
        ErrorDetail.Pointer("ids", 3_000_000_000L).ShouldBe("/ids/3000000000");
    }

    [Theory]
    [InlineData("a/b")]
    [InlineData("a~b")]
    [InlineData("~1/~0")]
    public void Pointer_Result_IsAcceptedByTheConstructor(string name)
    {
        // Act
        var sut = new ErrorDetail(ErrorDetail.Pointer("items", 1, name), "Invalid.");

        // Assert
        sut.PropertyPointer.ShouldNotBeNull();
    }

    [Fact]
    public void Pointer_NullSegment_ThrowsArgumentNullException()
    {
        var exception = Record.Exception(() => ErrorDetail.Pointer("items", null!));

        exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Pointer_NegativeIndex_ThrowsArgumentOutOfRangeException(int index)
    {
        var exception = Record.Exception(() => ErrorDetail.Pointer("items", index));

        exception.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Pointer_NegativeLongIndex_ThrowsArgumentOutOfRangeException()
    {
        var exception = Record.Exception(() => ErrorDetail.Pointer("items", -1L));

        exception.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Pointer_SegmentOfAnotherType_ThrowsArgumentException()
    {
        var exception = Record.Exception(() => ErrorDetail.Pointer("items", 1.5));

        exception.ShouldBeOfType<ArgumentException>();
    }

    #endregion

    #region Pointer with an index of any integer type (issue #236)

    [Fact]
    public void Pointer_SByteIndex_IsWrittenInDecimal()
    {
        ErrorDetail.Pointer("items", sbyte.MaxValue).ShouldBe("/items/127");
    }

    [Fact]
    public void Pointer_ByteIndex_IsWrittenInDecimal()
    {
        ErrorDetail.Pointer("items", byte.MaxValue).ShouldBe("/items/255");
    }

    [Fact]
    public void Pointer_Int16Index_IsWrittenInDecimal()
    {
        ErrorDetail.Pointer("items", short.MaxValue).ShouldBe("/items/32767");
    }

    [Fact]
    public void Pointer_UInt16Index_IsWrittenInDecimal()
    {
        ErrorDetail.Pointer("items", ushort.MaxValue).ShouldBe("/items/65535");
    }

    [Fact]
    public void Pointer_UInt32Index_IsWrittenInDecimal()
    {
        ErrorDetail.Pointer("items", uint.MaxValue).ShouldBe("/items/4294967295");
    }

    [Fact]
    public void Pointer_UInt64Index_IsWrittenInDecimal()
    {
        ErrorDetail.Pointer("items", ulong.MaxValue).ShouldBe("/items/18446744073709551615");
    }

    [Fact]
    public void Pointer_NIntIndex_IsWrittenInDecimal()
    {
        ErrorDetail.Pointer("items", (nint)42).ShouldBe("/items/42");
    }

    [Fact]
    public void Pointer_NUIntIndex_IsWrittenInDecimal()
    {
        ErrorDetail.Pointer("items", (nuint)42).ShouldBe("/items/42");
    }

    [Fact]
    public void Pointer_NegativeSByteIndex_ThrowsArgumentOutOfRangeException()
    {
        var exception = Record.Exception(() => ErrorDetail.Pointer("items", (sbyte)-1));

        exception.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Pointer_NegativeInt16Index_ThrowsArgumentOutOfRangeException()
    {
        var exception = Record.Exception(() => ErrorDetail.Pointer("items", (short)-1));

        exception.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Pointer_NegativeNIntIndex_ThrowsArgumentOutOfRangeException()
    {
        var exception = Record.Exception(() => ErrorDetail.Pointer("items", (nint)(-1)));

        exception.ShouldBeOfType<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Pointer_CharSegment_ThrowsArgumentExceptionThatNamesTheType()
    {
        // Arrange
        const string ExpectedMessage = "A segment of a JSON Pointer must be a string or an integer (e.g., an int or a long), not ‘System.Char’. "
            + "(Parameter 'segments')";

        // Act
        var exception = Record.Exception(() => ErrorDetail.Pointer("items", '1'));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().Message.ShouldBe(ExpectedMessage);
    }

    [Fact]
    public void Pointer_EnumSegment_ThrowsArgumentException()
    {
        var exception = Record.Exception(() => ErrorDetail.Pointer("items", ErrorCategory.Validation));

        exception.ShouldBeOfType<ArgumentException>();
    }

    [Fact]
    public void Pointer_Int128Segment_ThrowsArgumentException()
    {
        var exception = Record.Exception(() => ErrorDetail.Pointer("items", (Int128)1));

        exception.ShouldBeOfType<ArgumentException>();
    }

    #endregion
}
