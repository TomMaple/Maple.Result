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

internal static partial class UriLocatorValidator
{
    private const string Message = "The URI locator is not valid. Check https://datatracker.ietf.org/doc/html/rfc3986#section-3.1 for details.";

    // new Uri trims surrounding whitespace and escapes characters such as spaces, invalid percent-encoding and non-ASCII characters,
    // but ErrorUri stores the string as passed. So the raw string must already be a valid URI: only the RFC 3986 characters
    // (unreserved, reserved and "%" as pct-encoded = "%" HEXDIG HEXDIG), and at most one "#", which starts the fragment.
    [GeneratedRegex(
        @"^(?:[A-Za-z0-9\-._~:/?\[\]@!$&'()*+,;=]|%[0-9A-Fa-f]{2})+" +
        @"(?:#(?:[A-Za-z0-9\-._~:/?\[\]@!$&'()*+,;=]|%[0-9A-Fa-f]{2})*)?\z",
        RegexOptions.CultureInvariant)]
    private static partial Regex RawUriPattern();

    /// <summary>
    ///     Whether the value is a valid absolute <c>http</c> or <c>https</c> <i>URI</i>, stored as passed.
    /// </summary>
    internal static bool IsValid(string uriLocator)
    {
        return Uri.TryCreate(uriLocator, UriKind.Absolute, out var uri)
               && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
               && RawUriPattern().IsMatch(uriLocator);
    }

    internal static void Validate(string uriLocator)
    {
        if (!IsValid(uriLocator))
            throw new UriFormatException(Message);
    }
}
