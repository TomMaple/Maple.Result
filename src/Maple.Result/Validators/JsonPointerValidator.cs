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

namespace Maple.Results.Validators;

/// <summary>
///     Validates the syntax of a JSON Pointer (<i>RFC 6901</i>), e.g., the <see cref="ErrorDetail.PropertyPointer" />.
/// </summary>
/// <remarks>
///     <para>
///         Two forms are valid: the string form (section 5), which is empty (the whole input) or starts with <c>/</c>, and the URI fragment
///         form (section 6), which is <c>#</c> followed by the percent-encoded string form. In both, a <c>~</c> is followed by <c>0</c> or <c>1</c>.
///     </para>
///     <para>
///         Not checked: the characters that <i>RFC 3986</i> allows in a fragment (<c>#/first name</c> is valid), and whether the pointer
///         leads to anything in the input. The value is not normalized: <c>/email</c> and <c>#/email</c> stay different values.
///     </para>
///     <para>The check is one pass over the characters (no regular expression), so it is linear for untrusted input.</para>
/// </remarks>
internal static class JsonPointerValidator
{
    /// <summary>
    ///     Returns the pointer if it is <see langword="null" /> or a valid JSON Pointer.
    /// </summary>
    /// <exception cref="ArgumentException">If the <paramref name="pointer" /> is not a valid JSON Pointer.</exception>
    internal static string? Validate(string? pointer, string paramName)
    {
        if (pointer is null || IsValid(pointer))
            return pointer;

        throw new ArgumentException(
            $"The property pointer ‘{pointer}’ is not a JSON Pointer (RFC 6901). A pointer is empty (the whole input) or starts with ‘/’, " +
            "e.g., ‘/email’ or ‘/items/0/name’; in a name, ‘~’ is written as ‘~0’ and ‘/’ as ‘~1’. " +
            "The URI fragment form starts with ‘#’, e.g., ‘#/email’.",
            paramName);
    }

    /// <summary>
    ///     Determines whether the pointer is a JSON Pointer in the string form or in the URI fragment form.
    /// </summary>
    internal static bool IsValid(string pointer)
    {
        if (!pointer.StartsWith('#'))
            return IsValidPath(pointer);

        var fragment = pointer.AsSpan(1);
        if (!IsPercentEncoded(fragment))
            return false;

        var path = Uri.UnescapeDataString(fragment);

        return IsValidPath(path);
    }

    // json-pointer = *( "/" reference-token ); a '~' only as "~0" or "~1"
    private static bool IsValidPath(ReadOnlySpan<char> path)
    {
        if (path.IsEmpty)
            return true;

        if (path[0] != '/')
            return false;

        for (var i = 1; i < path.Length; i++)
        {
            if (path[i] != '~')
                continue;

            var isEscape = i + 1 < path.Length && path[i + 1] is '0' or '1';
            if (!isEscape)
                return false;

            i++;
        }

        return true;
    }

    // every '%' is followed by two hexadecimal digits
    private static bool IsPercentEncoded(ReadOnlySpan<char> fragment)
    {
        for (var i = 0; i < fragment.Length; i++)
        {
            if (fragment[i] != '%')
                continue;

            var isEscape = i + 2 < fragment.Length && char.IsAsciiHexDigit(fragment[i + 1]) && char.IsAsciiHexDigit(fragment[i + 2]);
            if (!isEscape)
                return false;

            i += 2;
        }

        return true;
    }
}
