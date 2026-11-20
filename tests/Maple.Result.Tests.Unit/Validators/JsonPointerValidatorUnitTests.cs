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
using System;

namespace Maple.Results.Tests.Unit.Validators;

public class JsonPointerValidatorUnitTests
{
    #region IsValid

    [Theory]
    [InlineData("")]                    // the whole input
    [InlineData("/")]                   // a member with an empty name
    [InlineData("/email")]
    [InlineData("/items/0/name")]
    [InlineData("/a~1b")]               // "a/b"
    [InlineData("/m~0n")]               // "m~n"
    [InlineData("/~01")]                // "~1"
    [InlineData("/first name")]
    [InlineData("/naïve")]
    [InlineData("/c%d")]                // '%' is an ordinary character in the string form
    [InlineData("/Idempotency-Key")]
    public void IsValid_StringForm_ReturnsTrue(string pointer)
    {
        // Act
        var result = JsonPointerValidator.IsValid(pointer);

        // Assert
        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData("#")]                   // the whole input
    [InlineData("#/")]                  // a member with an empty name
    [InlineData("#/email")]
    [InlineData("#/profile/color")]
    [InlineData("#/a~1b")]
    [InlineData("#/first%20name")]
    [InlineData("#/first name")]        // the characters of a fragment are not checked
    [InlineData("#/c%25d")]             // "/c%d"
    [InlineData("#/a%7E0b")]            // "/a~0b"
    [InlineData("#/%2Femail")]          // "//email"
    [InlineData("#%2Femail")]           // "/email": the fragment is checked after it is decoded
    public void IsValid_UriFragmentForm_ReturnsTrue(string pointer)
    {
        // Act
        var result = JsonPointerValidator.IsValid(pointer);

        // Assert
        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData("email")]               // no leading '/'
    [InlineData("Email")]
    [InlineData("a/b")]
    [InlineData("items[0].name")]       // other path syntaxes
    [InlineData("Items[0].Name")]
    [InlineData("$.email")]
    [InlineData(" /email")]             // a leading space
    [InlineData(" ")]
    [InlineData("/a~")]                 // a '~' not followed by '0' or '1'
    [InlineData("/a~2b")]
    [InlineData("/~/a")]
    public void IsValid_InvalidStringForm_ReturnsFalse(string pointer)
    {
        // Act
        var result = JsonPointerValidator.IsValid(pointer);

        // Assert
        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData("#email")]              // no leading '/' after '#'
    [InlineData("##/a")]
    [InlineData("#/a~")]
    [InlineData("#/a%2")]               // a '%' not followed by two hexadecimal digits
    [InlineData("#/a%zz")]
    [InlineData("#/a%")]
    [InlineData("#/a%7E2")]             // "/a~2"
    [InlineData("#/a%7E")]              // "/a~"
    public void IsValid_InvalidUriFragmentForm_ReturnsFalse(string pointer)
    {
        // Act
        var result = JsonPointerValidator.IsValid(pointer);

        // Assert
        result.ShouldBeFalse();
    }

    #endregion

    #region Validate

    [Fact]
    public void Validate_Null_ReturnsNull()
    {
        // Act
        var result = JsonPointerValidator.Validate(null, "pointer");

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void Validate_ValidPointer_ReturnsSameInstance()
    {
        // Arrange
        var pointer = "#/email";

        // Act
        var result = JsonPointerValidator.Validate(pointer, "pointer");

        // Assert
        result.ShouldBeSameAs(pointer);
    }

    [Fact]
    public void Validate_InvalidPointer_ThrowsArgumentExceptionWithParameterName()
    {
        // Act
        var exception = Record.Exception(() => JsonPointerValidator.Validate("Items[0].Name", "pointer"));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        var argumentException = exception as ArgumentException;
        argumentException.ShouldNotBeNull();
        argumentException.ParamName.ShouldBe("pointer");
    }

    [Fact]
    public void Validate_InvalidPointer_MessageShowsValueAndExamples()
    {
        // Act
        var exception = Record.Exception(() => JsonPointerValidator.Validate("Items[0].Name", "pointer"));

        // Assert
        exception.ShouldNotBeNull();
        exception.Message.ShouldContain("‘Items[0].Name’");
        exception.Message.ShouldContain("‘/email’");
        exception.Message.ShouldContain("‘/items/0/name’");
    }

    #endregion
}
