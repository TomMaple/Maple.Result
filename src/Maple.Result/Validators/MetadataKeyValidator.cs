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
using System.Collections.Frozen;
using System.Collections.Generic;

namespace Maple.Results.Validators;

/// <summary>
///     Validates the keys of the <see cref="Error.Metadata" /> entries.
/// </summary>
/// <remarks>
///     <para>
///         Metadata entries correspond to the <i>RFC 9457</i> extension members, which share the top-level object with the standard members.
///         A key that matches a top-level member name of the format (<see cref="ProblemDetailsMembers" />) would produce a duplicate
///         or ambiguous member once the metadata is flattened into a problem details response, so such keys are rejected.
///     </para>
///     <para>
///         The comparison ignores case: clients often deserialize JSON case-insensitively,
///         and naming policies can change the case of a key during serialization.
///         For the same reason, two keys of one error must not differ only in case (<see cref="ValidateCase(string, Dictionary{string, object?}, string)" />).
///     </para>
///     <para>
///         The naming recommendations of <i>RFC 9457</i> (section 4: letters, digits and <c>_</c>, starting with a letter, at least 3 characters)
///         are not enforced, because they are not mandatory and problem details received from other services must be accepted as they are.
///     </para>
/// </remarks>
internal static class MetadataKeyValidator
{
    private static readonly FrozenSet<string> ReservedKeys = new[]
    {
        ProblemDetailsMembers.Type,
        ProblemDetailsMembers.Title,
        ProblemDetailsMembers.Status,
        ProblemDetailsMembers.Detail,
        ProblemDetailsMembers.Instance,
        ProblemDetailsMembers.Errors,
        ProblemDetailsMembers.DetailTemplated
    }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    ///     Determines whether the key matches a standard problem details member name (case-insensitive).
    /// </summary>
    internal static bool IsReserved(string key)
    {
        return ReservedKeys.Contains(key);
    }

    /// <summary>
    ///     Throws if the key is null, empty, whitespace-only or reserved.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the <paramref name="key" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="key" /> is empty, whitespace-only or matches a standard problem details member name.
    /// </exception>
    internal static void Validate(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        if (IsReserved(key))
        {
            throw new ArgumentException(
                $"The metadata key ‘{key}’ is reserved. Keys must not match (case-insensitively) the standard " +
                "problem details members: ‘type’, ‘title’, ‘status’, ‘detail’, ‘instance’, ‘errors’, ‘detailTemplated’.",
                nameof(key));
        }
    }

    /// <summary>
    ///     Throws if the key differs only in case from a key of the existing metadata; the same key is allowed (it replaces the value).
    /// </summary>
    /// <remarks>
    ///     The existing metadata never holds two keys that differ only in case, so an exact match is the only key the new key can equal.
    /// </remarks>
    /// <exception cref="ArgumentException">If the <paramref name="key" /> differs only in case from an existing key.</exception>
    internal static void ValidateCase(string key, Dictionary<string, object?> existing, string paramName)
    {
        if (existing.ContainsKey(key))
            return;

        foreach (var existingKey in existing.Keys)
        {
            if (string.Equals(existingKey, key, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    $"The metadata key ‘{key}’ differs only in case from the existing key ‘{existingKey}’. " +
                    "Keys that differ only in case would be the same member of a problem details object; " +
                    $"use ‘{existingKey}’ to replace the value, or remove it first.",
                    paramName);
            }
        }
    }

    /// <summary>
    ///     Throws if two keys of the same call differ only in case; the same key repeated is allowed (the last value wins).
    /// </summary>
    /// <exception cref="ArgumentException">If the <paramref name="earlierKey" /> and the <paramref name="key" /> differ only in case.</exception>
    internal static void ValidateCase(string earlierKey, string key, string paramName)
    {
        var differOnlyInCase = string.Equals(earlierKey, key, StringComparison.OrdinalIgnoreCase)
                               && !string.Equals(earlierKey, key, StringComparison.Ordinal);

        if (differOnlyInCase)
        {
            throw new ArgumentException(
                $"The metadata keys ‘{earlierKey}’ and ‘{key}’ differ only in case. " +
                "Keys that differ only in case would be the same member of a problem details object.",
                paramName);
        }
    }
}
