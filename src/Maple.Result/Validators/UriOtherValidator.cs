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
using System.Text.RegularExpressions;

namespace Maple.Results.Validators;

internal static class UriOtherValidator
{
    private const string AboutBlankMessage =
        "The URI ‘about:blank’ has its own property. Use ErrorUri.None, or ErrorUri.Parse when the kind is not known.";

    private const string Message =
        "The URI is not a valid absolute URI. Check https://datatracker.ietf.org/doc/html/rfc3986#section-3 for details.";

    // RFC 3986: absolute-URI with an optional fragment = scheme ":" hier-part [ "?" query ] [ "#" fragment ]
    //           scheme = ALPHA *( ALPHA / DIGIT / "+" / "-" / "." )
    // The rest is checked at the character level, as for a URI locator: only the RFC 3986 characters (unreserved, reserved
    // and "%" as pct-encoded = "%" HEXDIG HEXDIG), and at most one "#", which starts the fragment. The value is stored as
    // passed, so white space and characters that would need escaping are rejected. The pattern ends with \z, not $, which
    // would also match before a trailing newline.
    // NonBacktracking: the value can come from another service (ErrorUri.Parse and TryParse), and this engine runs in linear time
    // whatever the pattern (#189). [GeneratedRegex] cannot be used with NonBacktracking (SYSLIB1044); revisit in .NET 12.
    private static readonly Regex AbsoluteUriPattern = new(
        @"^[A-Za-z][A-Za-z0-9+\-.]*:" +
        @"(?:[A-Za-z0-9\-._~:/?\[\]@!$&'()*+,;=]|%[0-9A-Fa-f]{2})*" +
        @"(?:#(?:[A-Za-z0-9\-._~:/?\[\]@!$&'()*+,;=]|%[0-9A-Fa-f]{2})*)?\z",
        RegexOptions.CultureInvariant | RegexOptions.NonBacktracking);

    /// <summary>
    ///     Whether the value is an absolute <i>URI</i> (<i>RFC 3986</i>) that is neither <c>about:blank</c> (in any case) nor of a scheme
    ///     that has a factory method of its own.
    /// </summary>
    internal static bool IsValid(string uri)
    {
        return AbsoluteUriPattern.IsMatch(uri) && !IsAboutBlank(uri) && SupportingFactory(uri) is null;
    }

    internal static void Validate(string uri)
    {
        if (!AbsoluteUriPattern.IsMatch(uri))
            throw new UriFormatException(Message);

        if (IsAboutBlank(uri))
            throw new UriFormatException(AboutBlankMessage);

        var factory = SupportingFactory(uri);
        if (factory is not null)
        {
            var colon = uri.IndexOf(':');
            var scheme = uri[..colon];

            throw new UriFormatException(
                $"The scheme ‘{scheme}’ has its own factory method. Use ErrorUri.{factory}, or ErrorUri.Parse when the kind is not known.");
        }
    }

    /// <summary>
    ///     Whether the value is <c>about:blank</c> in any case, which is <see cref="ErrorUri.None" />: as an other <i>URI</i>, its value
    ///     would not survive <see cref="ErrorUri.Parse" /> (#275). Any other <c>about</c> <i>URI</i> is an other <i>URI</i>.
    /// </summary>
    private static bool IsAboutBlank(string uri)
    {
        return uri.Equals(ErrorUri.NoneValue, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    ///     The name of the factory method that supports the scheme of the value (case-insensitive, <i>RFC 3986</i>, section 3.1),
    ///     or <see langword="null" /> for any other scheme.
    /// </summary>
    private static string? SupportingFactory(string uri)
    {
        var colon = uri.IndexOf(':');
        var scheme = uri.AsSpan(0, colon);

        if (scheme.Equals("tag", StringComparison.OrdinalIgnoreCase))
            return "Tag";

        if (scheme.Equals("urn", StringComparison.OrdinalIgnoreCase))
            return "Urn";

        if (scheme.Equals("http", StringComparison.OrdinalIgnoreCase) || scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
            return "Locator";

        return null;
    }
}
