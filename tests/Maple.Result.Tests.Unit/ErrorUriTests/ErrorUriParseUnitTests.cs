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

namespace Maple.Result.Tests.Unit.ErrorUriTests;

public class ErrorUriParseUnitTests
{
    #region test data

    /// <summary>Valid values and the error URI that the factory of their kind creates.</summary>
    public static readonly TheoryData<string, ErrorUri> ValidValues = new()
    {
        { "about:blank", ErrorUri.None() },
        // RFC 3986 section 3.1: the scheme is case-insensitive; None() always stores "about:blank".
        { "ABOUT:blank", ErrorUri.None() },
        { "About:blank", ErrorUri.None() },
        { "/account/12345/msgs/abc", ErrorUri.Relative("/account/12345/msgs/abc") },
        { "tag:example.com,2026:errors:out-of-credit", ErrorUri.Tag("tag:example.com,2026:errors:out-of-credit") },
        { "TAG:example.com,2026:x", ErrorUri.Tag("TAG:example.com,2026:x") },
        { "urn:example:errors:out-of-credit", ErrorUri.Urn("urn:example:errors:out-of-credit") },
        { "URN:example:x", ErrorUri.Urn("URN:example:x") },
        { "https://example.com/probs/out-of-credit", ErrorUri.Locator("https://example.com/probs/out-of-credit") },
        { "http://example.com/x", ErrorUri.Locator("http://example.com/x") },
        { "HTTPS://example.com/x", ErrorUri.Locator("HTTPS://example.com/x") },
    };

    /// <summary>Values whose scheme (or form) is not one of the kinds of an error URI.</summary>
    public static readonly TheoryData<string> UnknownKinds =
    [
        "about:BLANK",              // the "blank" token is case-sensitive
        "about:blank#x",            // a fragment
        "about:blank?x",            // a query
        "about:srcdoc",             // another about token
        "mailto:support@example.com",
        "ftp://example.com/x",
        "file:///x",
        "out-of-credit",            // a relative path (RFC 9457 allows it; the factories do not)
        "../probs/x",
        "",
        " ",
        " https://example.com/x",   // a leading space
    ];

    /// <summary>Values of a known kind that its factory rejects.</summary>
    public static readonly TheoryData<string> InvalidValuesOfKnownKind =
    [
        "tag:example.com:x",
        "urn:x",
        "https://exa mple.com/x",
        "//example.com/x",
    ];

    /// <summary>Values of a known kind that its factory rejects, and the beginning of the factory's message.</summary>
    public static readonly TheoryData<string, string> InvalidValuesOfKnownKindWithMessages = new()
    {
        { "tag:example.com:x", "The URI tag is not valid." },
        { "urn:x", "The URN is not valid." },
        { "https://exa mple.com/x", "The URI locator is not valid." },
        { "//example.com/x", "The relative URI reference is not valid." },
    };

    /// <summary>Error URIs of every kind, for the round-trip through their value.</summary>
    public static readonly TheoryData<ErrorUri> ErrorUris =
    [
        ErrorUri.None(),
        default(ErrorUri),
        ErrorUri.Relative("/account/12345/msgs/abc"),
        ErrorUri.Tag("tag:example.com,2026:errors:out-of-credit"),
        ErrorUri.Urn("urn:example:errors:out-of-credit"),
        ErrorUri.Locator("https://example.com/probs/out-of-credit"),
    ];

    #endregion

    #region Parse

    [Theory]
    [MemberData(nameof(ValidValues))]
    public void Parse_ValidValue_ReturnsTheErrorUriOfItsKind(string value, ErrorUri expected)
    {
        // Act
        var result = ErrorUri.Parse(value);

        // Assert
        result.ShouldBe(expected);
        result.Value.ShouldBe(expected.Value);
    }

    [Theory]
    [MemberData(nameof(UnknownKinds))]
    public void Parse_UnknownKind_ThrowsUriFormatException(string value)
    {
        // Act
        var exception = Record.Exception(() => ErrorUri.Parse(value));

        // Assert
        exception.ShouldBeOfType<UriFormatException>().Message.ShouldStartWith("The error URI is not valid.");
    }

    [Theory]
    [MemberData(nameof(InvalidValuesOfKnownKindWithMessages))]
    public void Parse_InvalidValueOfKnownKind_ThrowsTheMessageOfItsFactory(string value, string expectedMessageStart)
    {
        // Act
        var exception = Record.Exception(() => ErrorUri.Parse(value));

        // Assert
        exception.ShouldBeOfType<UriFormatException>().Message.ShouldStartWith(expectedMessageStart);
    }

    [Fact]
    public void Parse_Null_ThrowsArgumentNullExceptionNamingTheParameter()
    {
        // Act
        var exception = Record.Exception(() => ErrorUri.Parse(null!));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("value");
    }

    [Theory]
    [MemberData(nameof(ErrorUris))]
    public void Parse_ValueOfAnErrorUri_ReturnsAnEqualErrorUri(ErrorUri errorUri)
    {
        // Act
        var result = ErrorUri.Parse(errorUri.Value);

        // Assert
        result.ShouldBe(errorUri);
    }

    #endregion

    #region TryParse

    [Theory]
    [MemberData(nameof(ValidValues))]
    public void TryParse_ValidValue_ReturnsTrueAndTheErrorUriOfItsKind(string value, ErrorUri expected)
    {
        // Act
        var parsed = ErrorUri.TryParse(value, out var result);

        // Assert
        parsed.ShouldBeTrue();
        result.ShouldBe(expected);
        result.Value.ShouldBe(expected.Value);
    }

    [Theory]
    [MemberData(nameof(UnknownKinds))]
    public void TryParse_UnknownKind_ReturnsFalse(string value)
    {
        // Act
        var parsed = ErrorUri.TryParse(value, out _);

        // Assert
        parsed.ShouldBeFalse();
    }

    [Theory]
    [MemberData(nameof(InvalidValuesOfKnownKind))]
    public void TryParse_InvalidValueOfKnownKind_ReturnsFalse(string value)
    {
        // Act
        var parsed = ErrorUri.TryParse(value, out _);

        // Assert
        parsed.ShouldBeFalse();
    }

    [Fact]
    public void TryParse_Null_ReturnsFalse()
    {
        // Act
        var parsed = ErrorUri.TryParse(null, out _);

        // Assert
        parsed.ShouldBeFalse();
    }

    [Theory]
    [MemberData(nameof(ErrorUris))]
    public void TryParse_ValueOfAnErrorUri_ReturnsAnEqualErrorUri(ErrorUri errorUri)
    {
        // Act
        var parsed = ErrorUri.TryParse(errorUri.Value, out var result);

        // Assert
        parsed.ShouldBeTrue();
        result.ShouldBe(errorUri);
    }

    // An exception would allocate; the values are checked by patterns, so a rejected value allocates nothing.
    [Theory]
    [InlineData("tag:example.com:x")]
    [InlineData("urn:x")]
    [InlineData("//example.com/x")]
    [InlineData("mailto:support@example.com")]
    public void TryParse_RejectedValue_AllocatesNothing(string value)
    {
        // Act
        var allocated = AllocationHelper.AllocatedBytes(() => _ = ErrorUri.TryParse(value, out _));

        // Assert
        allocated.ShouldBe(0);
    }

    #endregion
}
