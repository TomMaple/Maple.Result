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
using System.Collections.Generic;

namespace Maple.Results.Validators;

/// <summary>
///     Validates the names chosen by the code that creates an error: the metadata keys, the template identifiers and the parameter names of
///     templated messages.
/// </summary>
/// <remarks>
///     <para>
///         A name is not empty and does not start or end with white space (as <see cref="char.IsWhiteSpace(char)" /> defines it); white space
///         inside a name is allowed (<c>retry after</c>). An empty or padded name is always a mistake, e.g., a name split from a text and not
///         trimmed. A name is not trimmed: a padded name is rejected.
///     </para>
///     <para>
///         This is a rule of this library, for consistency with the title and the metadata keys, not of an RFC: <i>RFC 8259</i> (section 4)
///         allows any string as a member name, and the naming recommendations of <i>RFC 9457</i> (section 4) are not mandatory.
///     </para>
/// </remarks>
internal static class NameValidator
{
    /// <summary>
    ///     The number of names from which a name that differs only in case is found with a case-insensitive index instead of a scan (#234):
    ///     the usual collection with a few names allocates nothing more, and a large one is checked in linear time.
    /// </summary>
    internal const int CaseIndexThreshold = 8;

    /// <summary>
    ///     Determines whether the name is not empty and does not start or end with white space.
    /// </summary>
    internal static bool IsValid(string name)
    {
        return name.Length > 0
               && !char.IsWhiteSpace(name[0])
               && !char.IsWhiteSpace(name[^1]);
    }

    /// <summary>
    ///     Returns the name of the <paramref name="dictionary" /> that differs only in case from the <paramref name="name" />, which is not in
    ///     the dictionary; <see langword="null" /> if there is none.
    /// </summary>
    /// <remarks>
    ///     The dictionary never holds two names that differ only in case, so there is at most one. The <paramref name="index" /> (the names,
    ///     compared case-insensitively, with their stored spelling) is created once the dictionary has <see cref="CaseIndexThreshold" />
    ///     names; the caller keeps it in step with the later changes of the dictionary.
    /// </remarks>
    internal static string? FindNameDifferingOnlyInCase(string name, Dictionary<string, object?> dictionary, ref Dictionary<string, string>? index)
    {
        if (index is null && dictionary.Count >= CaseIndexThreshold)
        {
            index = new Dictionary<string, string>(dictionary.Count, StringComparer.OrdinalIgnoreCase);
            foreach (var existingName in dictionary.Keys)
                index.Add(existingName, existingName);
        }

        if (index is not null)
            return index.GetValueOrDefault(name);

        foreach (var existingName in dictionary.Keys)
        {
            if (string.Equals(existingName, name, StringComparison.OrdinalIgnoreCase))
                return existingName;
        }

        return null;
    }
}
