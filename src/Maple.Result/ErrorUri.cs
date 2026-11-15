// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Result.Validators;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Maple.Result;

/// <summary>
///     Represents a <i>URI</i> that identifies or describes the type or category of an error in a standardized format.
/// </summary>
/// <remarks>
///     It can be
///     <list type="bullet">
///         <item>
///             <term>no value</term>
///             <description><c>about:blank</c></description>
///         </item>
///         <item>
///             <term>URI locator</term>
///             <description>the <i>URI</i> of the human-readable documentation for the type of problem,</description>
///         </item>
///         <item>
///             <term>URI tag</term>
///             <description>
///                 the non-dereferenceable, unique representation of the specific type of problem
///                 (e.g., <example><c>tag:example@example.org,2021-09-17:OutOfLuck</c></example>),
///             </description>
///         </item>
///         <item>
///             <term>URN</term>
///             <description>
///                 the non-dereferenceable, persistent name of the specific type of problem
///                 (e.g., <example><c>urn:example:errors:out-of-credit</c></example>),
///             </description>
///         </item>
///         <item>
///             <term>relative URI reference</term>
///             <description>
///                 the full path resolved against the base <i>URI</i> of the response (e.g., <example><c>/account/12345/msgs/abc</c></example>);
///                 <i>RFC 9457</i> recommends absolute <i>URI</i>s when possible.
///             </description>
///         </item>
///     </list>
///     For <i>URI Locator</i> and relative reference specification, see also: <seealso href="https://datatracker.ietf.org/doc/html/rfc3986" />.<br />
///     For <i>URI Tag</i> specification, see also: <seealso href="https://datatracker.ietf.org/doc/html/rfc4151" />.<br />
///     For <i>URN</i> specification, see also: <seealso href="https://datatracker.ietf.org/doc/html/rfc8141" />.
/// </remarks>
public readonly record struct ErrorUri
{
    private const string NoneValue = "about:blank";

    private const string UnknownKindMessage =
        "The error URI is not valid. Expected ‘about:blank’, a relative reference starting with ‘/’, " +
        "or a ‘tag:’, ‘urn:’, ‘http:’ or ‘https:’ URI.";

    private readonly string? _value;

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="ErrorUri" /> record with the provided value, without validating it.
    /// </summary>
    /// <remarks>
    ///     Used by the factory methods (<see cref="None" />, <see cref="Locator" />, <see cref="Tag" />, <see cref="Urn" />, <see cref="Relative" />)
    ///     after validation, and for values taken from an existing <see cref="Error" />, which were validated when it was created.
    /// </remarks>
    internal ErrorUri(string value)
    {
        _value = value;
    }

    #endregion

    /// <summary>
    ///     The <i>URI</i> value representing the error type or category.
    /// </summary>
    /// <remarks>
    ///     A default <see cref="ErrorUri" /> instance—which bypasses the constructor and therefore has no underlying value—reports <c>about:blank</c>,
    ///     the value that <i>RFC 9457</i> defines for an unspecified type.
    ///     This keeps the mandatory <see cref="Error.TypeUri" /> from ever being <see langword="null" />.
    /// </remarks>
    public string Value => _value ?? NoneValue;

    /// <summary>
    ///     Returns a new instance of the no-value <see cref="ErrorUri" />: <c>about:blank</c>.
    /// </summary>
    public static ErrorUri None()
    {
        return new ErrorUri(NoneValue);
    }

    /// <summary>
    ///     Returns a new instance of the <see cref="ErrorUri" /> with a valid <i>URI Locator</i>.
    /// </summary>
    /// <param name="uriLocator">
    ///     The absolute <c>http</c> or <c>https</c> <i>URI</i>, e.g., <c>https://example.com/probs/out-of-credit</c>;
    ///     non-ASCII characters must be percent-encoded, and an internationalized domain name must use its punycode (<c>xn--</c>) form.
    /// </param>
    /// <returns>A new instance of the <see cref="ErrorUri" /> with the specified <i>URI Locator</i>.</returns>
    /// <remarks>
    ///     The value is stored as passed, so it must already be a valid <i>URI</i> (<i>RFC 3986</i>): leading or trailing white space,
    ///     spaces and other characters that would need escaping,
    ///     invalid percent-encoding (e.g., <c>%zz</c>) and a second <c>#</c> are rejected rather than normalized.
    /// </remarks>
    /// <exception cref="ArgumentNullException">If the <paramref name="uriLocator" /> is <see langword="null" />.</exception>
    /// <exception cref="UriFormatException">If the <paramref name="uriLocator" /> is not a valid <i>URI Locator</i>.</exception>
    public static ErrorUri Locator(string uriLocator)
    {
        ArgumentNullException.ThrowIfNull(uriLocator);
        UriLocatorValidator.Validate(uriLocator);

        return new ErrorUri(uriLocator);
    }

    /// <summary>
    ///     Returns a new instance of the <see cref="ErrorUri" /> with a valid <i>URI Tag</i>.
    /// </summary>
    /// <param name="uriTag">
    ///     The <i>URI Tag</i> (<i>RFC 4151</i>), e.g., <c>tag:example.com,2026:errors:out-of-credit</c>;
    ///     non-ASCII characters must be percent-encoded, and an internationalized domain name must use its punycode (<c>xn--</c>) form.
    /// </param>
    /// <returns>A new instance of the <see cref="ErrorUri" /> with the specified <i>URI Tag</i>.</returns>
    /// <remarks>
    ///     The value is stored as passed, so it must match the <i>RFC 4151</i> syntax exactly: white space (including a trailing new line)
    ///     and invalid percent-encoding (e.g., <c>%zz</c>) are rejected.
    /// </remarks>
    /// <exception cref="ArgumentNullException">If the <paramref name="uriTag" /> is <see langword="null" />.</exception>
    /// <exception cref="UriFormatException">If the <paramref name="uriTag" /> is not a valid <i>URI Tag</i>.</exception>
    public static ErrorUri Tag(string uriTag)
    {
        ArgumentNullException.ThrowIfNull(uriTag);
        UriTagValidator.Validate(uriTag);

        return new ErrorUri(uriTag);
    }

    /// <summary>
    ///     Returns a new instance of the <see cref="ErrorUri" /> with a valid <i>URN</i> (<i>RFC 8141</i>).
    /// </summary>
    /// <param name="urn">
    ///     The <i>URN</i>, e.g., <c>urn:example:errors:out-of-credit</c>; non-ASCII characters must be percent-encoded.
    /// </param>
    /// <returns>A new instance of the <see cref="ErrorUri" /> with the specified <i>URN</i>.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="urn" /> is <see langword="null" />.</exception>
    /// <exception cref="UriFormatException">If the <paramref name="urn" /> is not a valid <i>URN</i>.</exception>
    public static ErrorUri Urn(string urn)
    {
        ArgumentNullException.ThrowIfNull(urn);
        UriUrnValidator.Validate(urn);

        return new ErrorUri(urn);
    }

    /// <summary>
    ///     Returns a new instance of the <see cref="ErrorUri" /> with a valid relative <i>URI</i> reference with the full path (<i>RFC 3986</i>, section 4.2).
    /// </summary>
    /// <param name="relativeReference">
    ///     The relative reference with the full path (e.g., <c>/account/12345/msgs/abc</c>), optionally followed by a query and a fragment;
    ///     non-ASCII characters must be percent-encoded.
    /// </param>
    /// <returns>A new instance of the <see cref="ErrorUri" /> with the specified relative reference.</returns>
    /// <remarks>
    ///     <para>
    ///         A relative reference is resolved against the base <i>URI</i> of the response.
    ///         <i>RFC 9457</i> (sections 3.1.1 and 3.1.5) allows it in both <see cref="Error.TypeUri" /> and <see cref="Error.InstanceUri" />,
    ///         but recommends absolute <i>URI</i>s when possible, because relative ones can cause confusion and might not be handled correctly by all
    ///         implementations. When a relative reference is used, it should include the full path.
    ///     </para>
    ///     <para>
    ///         Therefore, only a reference that starts with a single <c>/</c> is accepted: a relative path (<c>msgs/abc</c>),
    ///         a network-path reference (<c>//host/path</c>) and an empty reference are rejected.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">If the <paramref name="relativeReference" /> is <see langword="null" />.</exception>
    /// <exception cref="UriFormatException">If the <paramref name="relativeReference" /> is not a valid relative reference.</exception>
    public static ErrorUri Relative(string relativeReference)
    {
        ArgumentNullException.ThrowIfNull(relativeReference);
        UriRelativeValidator.Validate(relativeReference);

        return new ErrorUri(relativeReference);
    }

    #region parsing

    /// <summary>
    ///     Returns a new instance of the <see cref="ErrorUri" /> of the kind that the <paramref name="value" /> has, validated by the factory method of that
    ///     kind.
    /// </summary>
    /// <param name="value">
    ///     The <i>URI</i>: <c>about:blank</c>, a relative reference starting with <c>/</c>, or a <c>tag:</c>, <c>urn:</c>, <c>http:</c> or
    ///     <c>https:</c> <i>URI</i>.
    /// </param>
    /// <returns>A new instance of the <see cref="ErrorUri" /> with the specified value.</returns>
    /// <remarks>
    ///     <para>
    ///         The kind is detected from the value: <c>about:blank</c> returns <see cref="None" />; a value starting with <c>/</c> is passed to
    ///         <see cref="Relative" />; the scheme <c>tag</c> to <see cref="Tag" />, <c>urn</c> to <see cref="Urn" />, and <c>http</c> or
    ///         <c>https</c> to <see cref="Locator" />. Any other value is rejected.
    ///     </para>
    ///     <para>
    ///         Schemes are case-insensitive (<i>RFC 3986</i>, section 3.1), so <c>TAG:…</c> is a <i>URI Tag</i>; the value is stored as passed.
    ///         <c>about:blank</c> is the exception: its scheme may have any case, the <c>blank</c> token must be lowercase, and the result always has
    ///         the value <c>about:blank</c>. <c>ErrorUri.Parse(uri.Value)</c> returns an <see cref="ErrorUri" /> equal to <c>uri</c>.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">If the <paramref name="value" /> is <see langword="null" />.</exception>
    /// <exception cref="UriFormatException">
    ///     If the <paramref name="value" /> is not of a known kind, or not valid for its kind (with the message of the factory method of that kind).
    /// </exception>
    public static ErrorUri Parse(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        return DetectKind(value) switch
        {
            Kind.None => None(),
            Kind.Relative => Relative(value),
            Kind.Tag => Tag(value),
            Kind.Urn => Urn(value),
            Kind.Locator => Locator(value),
            _ => throw new UriFormatException(UnknownKindMessage)
        };
    }

    /// <summary>
    ///     Tries to create an <see cref="ErrorUri" /> of the kind that the <paramref name="value" /> has, with the rules of <see cref="Parse" />.
    /// </summary>
    /// <param name="value">The <i>URI</i>; see <see cref="Parse" />.</param>
    /// <param name="result">
    ///     The <see cref="ErrorUri" /> with the specified value, if it is valid; otherwise, the default <see cref="ErrorUri" />.
    /// </param>
    /// <returns><see langword="true" /> if the <paramref name="value" /> is a valid error <i>URI</i>; otherwise, <see langword="false" />.</returns>
    /// <remarks>This method never throws, and a rejected value is checked without throwing an exception internally.</remarks>
    public static bool TryParse([NotNullWhen(true)] string? value, out ErrorUri result)
    {
        result = default;

        if (value is null)
            return false;

        var kind = DetectKind(value);
        var isValid = kind switch
        {
            Kind.None => true,
            Kind.Relative => UriRelativeValidator.IsValid(value),
            Kind.Tag => UriTagValidator.IsValid(value),
            Kind.Urn => UriUrnValidator.IsValid(value),
            Kind.Locator => UriLocatorValidator.IsValid(value),
            _ => false
        };

        if (!isValid)
            return false;

        result = kind == Kind.None ? None() : new ErrorUri(value);
        return true;
    }

    #endregion

    /// <summary>
    ///     Returns the <i>URI</i> <see cref="Value" />.
    /// </summary>
    /// <returns>The <i>URI</i> value, e.g., <c>about:blank</c>.</returns>
    public override string ToString()
    {
        return Value;
    }

    #region equality

    /// <summary>
    ///     Determines whether the specified <see cref="ErrorUri" /> is equal to the current one,
    ///     comparing the <see cref="Value" /> rather than the underlying field, so that a default instance is equal to the one returned by <see cref="None" />.
    /// </summary>
    /// <param name="other">The <see cref="ErrorUri" /> to compare with the current instance.</param>
    /// <returns>
    ///     <see langword="true" /> if the specified <see cref="ErrorUri" /> is equal to the current one; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(ErrorUri other)
    {
        return Value == other.Value;
    }

    /// <summary>
    ///     Returns a hash code that is consistent with <see cref="Equals(ErrorUri)" />.
    /// </summary>
    /// <returns>A hash code for the current <see cref="ErrorUri" />.</returns>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    #endregion

    #region helper methods

    /// <summary>
    ///     Detects the kind of an error <i>URI</i> from its value, without validating it.
    /// </summary>
    private static Kind DetectKind(string value)
    {
        var span = value.AsSpan();

        // "about:blank" with the scheme in any case; the "blank" token is case-sensitive.
        if (span.Length == NoneValue.Length
            && span[..6].Equals("about:", StringComparison.OrdinalIgnoreCase)
            && span[6..].SequenceEqual("blank"))
        {
            return Kind.None;
        }

        if (span.StartsWith('/'))
            return Kind.Relative;

        var colon = span.IndexOf(':');
        if (colon <= 0)
            return Kind.Unknown;

        var scheme = span[..colon];

        if (scheme.Equals("tag", StringComparison.OrdinalIgnoreCase))
            return Kind.Tag;

        if (scheme.Equals("urn", StringComparison.OrdinalIgnoreCase))
            return Kind.Urn;

        if (scheme.Equals("http", StringComparison.OrdinalIgnoreCase) || scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
            return Kind.Locator;

        return Kind.Unknown;
    }

    private enum Kind
    {
        Unknown,
        None,
        Relative,
        Tag,
        Urn,
        Locator
    }

    #endregion
}
