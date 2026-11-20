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

internal static class UriUrnValidator
{
    private const string Message = "The URN is not valid. Check https://datatracker.ietf.org/doc/html/rfc8141#section-2 for details.";

    // RFC 3986: pchar = unreserved / pct-encoded / sub-delims / ":" / "@"
    private const string PChar = @"(?:[A-Za-z0-9\-._~!$&'()*+,;=:@]|%[0-9A-Fa-f]{2})";

    // pchar without a literal "=" (a percent-encoded "%3D" stays allowed); used to end the r-component at "?=".
    private const string PCharWithoutEquals = @"(?:[A-Za-z0-9\-._~!$&'()*+,;:@]|%[0-9A-Fa-f]{2})";

    // RFC 8141: namestring    = assigned-name [ rq-components ] [ "#" f-component ]
    //           assigned-name = "urn" ":" NID ":" NSS
    //           NID           = (alphanum) 0*30(ldh) (alphanum)
    //           NSS           = pchar *(pchar / "/")
    //           rq-components = [ "?+" r-component ] [ "?=" q-component ]
    //           r-component / q-component = pchar *( pchar / "/" / "?" )
    //           f-component   = fragment = *( pchar / "/" / "?" )
    // The "urn" scheme is case-insensitive.
    // "The r-component ends with a "?=" sequence (which begins a q-component) or a "#" character" (RFC 8141, section 2.3.1), so
    // the r-component cannot contain "?=". It is written without a lookahead, which NonBacktracking does not support: a run of
    // "?" is followed by a character that is neither "?" nor "=", or ends the r-component.
    // NonBacktracking: the value can come from another service (ErrorUri.Parse and TryParse), and this engine runs in linear time
    // whatever the pattern (#189). [GeneratedRegex] cannot be used with NonBacktracking (SYSLIB1044); revisit in .NET 12.
    private static readonly Regex UrnPattern = new(
        "^(?i:urn):" +
        // NID
        "[A-Za-z0-9][A-Za-z0-9-]{0,30}[A-Za-z0-9]:" +
        // NSS
        PChar + "(?:" + PChar + "|/)*" +
        // rq-components and f-component
        @"(?:\?\+" + PChar + "(?:" + PCharWithoutEquals + "|/|=|" + @"\?+(?:" + PCharWithoutEquals + @"|/))*\?*)?" +
        @"(?:\?=" + PChar + "(?:" + PChar + "|[/?])*)?" +
        "(?:#(?:" + PChar + @"|[/?])*)?\z",
        RegexOptions.CultureInvariant | RegexOptions.NonBacktracking);

    /// <summary>
    ///     Whether the value matches the <i>RFC 8141</i> grammar.
    /// </summary>
    internal static bool IsValid(string urn)
    {
        return UrnPattern.IsMatch(urn);
    }

    internal static void Validate(string urn)
    {
        if (!IsValid(urn))
            throw new UriFormatException(Message);
    }
}
