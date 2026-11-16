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

internal static partial class UriTagValidator
{
    private const string Message =
        "The URI tag is not valid. Check https://datatracker.ietf.org/doc/html/rfc4151#section-2.1 for details.";

    // RFC 4151: tag = "tag:" taggingEntity ":" specific [ "#" fragment ]
    //   taggingEntity = authorityName "," date
    //   authorityName = DNSname / emailAddress
    //   date          = year ["-" month ["-" day]]
    // The date is validated partially: the year is 1xxx or 2xxx, the month is 01-12 and the day is 01-31, each as two digits per the RFC's 2DIGIT grammar.
    // This rejects impossible values such as month 13 or day 32 without asserting a real calendar date (for example, it still accepts 02-30).
    //   specific = *( pchar / "/" / "?" ), fragment = *( pchar / "/" / "?" ), where pchar allows "%" only as pct-encoded = "%" HEXDIG HEXDIG.
    // The pattern ends with \z, not $, which would also match before a trailing newline.
    // The scheme is case-insensitive (RFC 3986 section 3.1; the "tag:" literal of the ABNF matches any case); the value is stored as passed,
    // and tags are compared character by character (RFC 4151 section 2.4), so "TAG:…" and "tag:…" are different tags.
    // NonBacktracking: the value can come from another service (ErrorUri.Parse and TryParse), and this engine runs in linear time
    // whatever the pattern (#189). The source generator does not generate code for this option; it creates the Regex instead.
    [GeneratedRegex(
        "^(?i:tag):" +
        // authorityName = DNSname / emailAddress
        @"(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?(?:\.[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?)*" +
        @"|[A-Za-z0-9._-]+@[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?(?:\.[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?)*)" +
        // "," date
        @",[12][0-9]{3}(?:-(?:0[1-9]|1[0-2])(?:-(?:0[1-9]|[12][0-9]|3[01]))?)?" +
        // ":" specific [ "#" fragment ]
        @":(?:[A-Za-z0-9\-._~!$&'()*+,;=:@/?]|%[0-9A-Fa-f]{2})*(?:#(?:[A-Za-z0-9\-._~!$&'()*+,;=:@/?]|%[0-9A-Fa-f]{2})*)?\z",
        RegexOptions.CultureInvariant | RegexOptions.NonBacktracking)]
    private static partial Regex TagPattern();

    /// <summary>
    ///     Whether the value matches the <i>RFC 4151</i> grammar. The pattern is the full grammar, including the <c>tag:</c> scheme, so the value
    ///     is not parsed with <see cref="Uri" /> as well.
    /// </summary>
    internal static bool IsValid(string uriTag)
    {
        return TagPattern().IsMatch(uriTag);
    }

    internal static void Validate(string uriTag)
    {
        if (!IsValid(uriTag))
            throw new UriFormatException(Message);
    }
}
