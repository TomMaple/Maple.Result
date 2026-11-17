// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Tests.Unit.TestingInfrastructure.Helpers;
using System;
using System.Diagnostics;
using System.Linq;

namespace Maple.Results.Tests.Unit.ErrorUriTests;

public class ErrorUriParseUnitTests
{
    #region test data

    /// <summary>Valid values and the error URI that the factory of their kind creates.</summary>
    public static readonly TheoryData<string, ErrorUri> ValidValues = new()
    {
        { "about:blank", ErrorUri.None },
        // "about:blank" is recognized in any case (#275); None always stores "about:blank".
        { "ABOUT:blank", ErrorUri.None },
        { "About:blank", ErrorUri.None },
        { "about:BLANK", ErrorUri.None },
        { "About:Blank", ErrorUri.None },
        { "ABOUT:BLANK", ErrorUri.None },
        { "/account/12345/msgs/abc", ErrorUri.Relative("/account/12345/msgs/abc") },
        { "tag:example.com,2026:errors:out-of-credit", ErrorUri.Tag("tag:example.com,2026:errors:out-of-credit") },
        { "TAG:example.com,2026:x", ErrorUri.Tag("TAG:example.com,2026:x") },
        { "urn:example:errors:out-of-credit", ErrorUri.Urn("urn:example:errors:out-of-credit") },
        { "URN:example:x", ErrorUri.Urn("URN:example:x") },
        { "https://example.com/probs/out-of-credit", ErrorUri.Locator("https://example.com/probs/out-of-credit") },
        { "http://example.com/x", ErrorUri.Locator("http://example.com/x") },
        { "HTTPS://example.com/x", ErrorUri.Locator("HTTPS://example.com/x") },
        // Any other scheme falls back to ErrorUri.Other (RFC 9457 allows any URI reference).
        { "mailto:support@example.com", ErrorUri.Other("mailto:support@example.com") },
        { "ftp://example.com/x", ErrorUri.Other("ftp://example.com/x") },
        { "file:///x", ErrorUri.Other("file:///x") },
        { "MAILTO:support@example.com", ErrorUri.Other("MAILTO:support@example.com") },
    };

    /// <summary>Values that are neither one of the supported kinds nor an absolute URI with another scheme.</summary>
    public static readonly TheoryData<string> UnknownKinds =
    [
        "about",                    // no colon: a relative path, not the "about" scheme
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
        "mailto:a b",
        "about:a b",
        "about:blank ",             // a trailing space: not "about:blank", and not a valid URI
    ];

    /// <summary>Values of a known kind that its factory rejects, and the beginning of the factory's message.</summary>
    public static readonly TheoryData<string, string> InvalidValuesOfKnownKindWithMessages = new()
    {
        { "tag:example.com:x", "The URI tag is not valid." },
        { "urn:x", "The URN is not valid." },
        { "https://exa mple.com/x", "The URI locator is not valid." },
        { "//example.com/x", "The relative URI reference is not valid." },
        { "mailto:a b", "The URI is not a valid absolute URI." },
        { "about:a b", "The URI is not a valid absolute URI." },
        { "about:blank ", "The URI is not a valid absolute URI." },
    };

    /// <summary>The <c>about</c> URIs other than <c>about:blank</c>, which are kept as they are, like any other scheme (#275).</summary>
    public static readonly TheoryData<string> OtherAboutUris =
    [
        "about:invalid",            // a registered token (the IANA registry of "about" URI tokens)
        "about:config",             // a token of an application
        "ABOUT:config",             // the scheme in any case, stored as passed
        "about:blank#x",            // a fragment: no longer "about:blank"
        "about:blank?x",            // a query
        "about:BLANK#x",
        "about:blankx",
        "about:",                   // an empty token (RFC 6694: about-token = *pchar)
    ];

    /// <summary>Error URIs of every kind, for the round-trip through their value.</summary>
    public static readonly TheoryData<ErrorUri> ErrorUris =
    [
        ErrorUri.None,
        default(ErrorUri),
        ErrorUri.Relative("/account/12345/msgs/abc"),
        ErrorUri.Tag("tag:example.com,2026:errors:out-of-credit"),
        ErrorUri.Urn("urn:example:errors:out-of-credit"),
        ErrorUri.Locator("https://example.com/probs/out-of-credit"),
        ErrorUri.Other("mailto:support@example.com"),
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
    [MemberData(nameof(OtherAboutUris))]
    public void Parse_AboutUriOtherThanAboutBlank_ReturnsErrorUriWithTheValueAsPassed(string value)
    {
        // Act
        var result = ErrorUri.Parse(value);

        // Assert
        result.Value.ShouldBe(value);
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
    [MemberData(nameof(OtherAboutUris))]
    public void TryParse_AboutUriOtherThanAboutBlank_ReturnsTrueAndErrorUriWithTheValueAsPassed(string value)
    {
        // Act
        var parsed = ErrorUri.TryParse(value, out var result);

        // Assert
        parsed.ShouldBeTrue();
        result.Value.ShouldBe(value);
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

    // ReDoS regression (#189): TryParse is the entry point for values received from other services.
    [Fact]
    public void TryParse_HostileUrn_ReturnsFalseInLinearTime()
    {
        // Arrange
        var maximum = TimeSpan.FromSeconds(2);
        var hostile = "urn:a1:x?+" + string.Concat(Enumerable.Repeat("a?=", 32_000)) + " ";
        var stopwatch = Stopwatch.StartNew();

        // Act
        var result = ErrorUri.TryParse(hostile, out _);

        // Assert
        stopwatch.Elapsed.ShouldBeLessThan(maximum);
        result.ShouldBeFalse();
    }

    // The r-component ends at "?=" (RFC 8141, section 2.3.1), so the q-component that follows must be valid.
    [Fact]
    public void TryParse_UrnWithInvalidComponentAfterQuestionMarkEquals_ReturnsFalse()
    {
        // Act
        var result = ErrorUri.TryParse("urn:example:a?+a?=/b", out _);

        // Assert
        result.ShouldBeFalse();
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
    [InlineData("mailto:a b")]
    [InlineData("out-of-credit")]
    public void TryParse_RejectedValue_AllocatesNothing(string value)
    {
        // Act
        var allocated = AllocationHelper.AllocatedBytes(() => _ = ErrorUri.TryParse(value, out _));

        // Assert
        allocated.ShouldBe(0);
    }

    #endregion

    #region IParsable<ErrorUri>

    [Theory]
    [MemberData(nameof(ValidValues))]
    public void IParsableParse_ValidValue_ReturnsTheErrorUriOfItsKind(string value, ErrorUri expected)
    {
        // Act
        var result = ParseAs<ErrorUri>(value);

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [MemberData(nameof(OtherAboutUris))]
    public void IParsableParse_AboutUriOtherThanAboutBlank_ReturnsErrorUriWithTheValueAsPassed(string value)
    {
        // Act
        var result = ParseAs<ErrorUri>(value);

        // Assert
        result.Value.ShouldBe(value);
    }

    [Theory]
    [MemberData(nameof(UnknownKinds))]
    public void IParsableParse_UnknownKind_ThrowsUriFormatException(string value)
    {
        // Act
        var exception = Record.Exception(() => ParseAs<ErrorUri>(value));

        // Assert
        exception.ShouldBeOfType<UriFormatException>();
    }

    [Theory]
    [MemberData(nameof(ValidValues))]
    public void IParsableTryParse_ValidValue_ReturnsTrueAndTheErrorUriOfItsKind(string value, ErrorUri expected)
    {
        // Act
        var parsed = TryParseAs<ErrorUri>(value, out var result);

        // Assert
        parsed.ShouldBeTrue();
        result.ShouldBe(expected);
    }

    [Theory]
    [MemberData(nameof(OtherAboutUris))]
    public void IParsableTryParse_AboutUriOtherThanAboutBlank_ReturnsTrueAndErrorUriWithTheValueAsPassed(string value)
    {
        // Act
        var parsed = TryParseAs<ErrorUri>(value, out var result);

        // Assert
        parsed.ShouldBeTrue();
        result.Value.ShouldBe(value);
    }

    [Theory]
    [MemberData(nameof(UnknownKinds))]
    public void IParsableTryParse_UnknownKind_ReturnsFalse(string value)
    {
        // Act
        var parsed = TryParseAs<ErrorUri>(value, out _);

        // Assert
        parsed.ShouldBeFalse();
    }

    #endregion

    #region helper methods

    /// <summary>Parses through the static members of <see cref="IParsable{TSelf}" />, as generic code (e.g., ASP.NET Core binding) does.</summary>
    private static T ParseAs<T>(string value)
        where T : IParsable<T>
    {
        return T.Parse(value, null);
    }

    /// <inheritdoc cref="ParseAs{T}" />
    private static bool TryParseAs<T>(string? value, out T result)
        where T : IParsable<T>
    {
        return T.TryParse(value, null, out result!);
    }

    #endregion
}
