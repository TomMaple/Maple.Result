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
///         For the same reason, two keys of one error must not differ only in case (<see cref="KeyDiffersOnlyInCase" /> and
///         <see cref="ValidateCase(string, string, string)" />).
///     </para>
///     <para>
///         A key does not start or end with white space (<see cref="NameValidator" />): <c>" type"</c> would get past the reserved-name check
///         and be written next to <c>type</c>. White space inside a key is allowed.
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
    ///     Throws if the key is null, empty, whitespace-only, starts or ends with white space, or is reserved.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the <paramref name="key" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="key" /> is empty, whitespace-only, starts or ends with white space, or matches a standard problem details member
    ///     name.
    /// </exception>
    internal static void Validate(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        if (!NameValidator.IsValid(key))
        {
            throw new ArgumentException(
                $"The metadata key ‘{key}’ starts or ends with white space. A key is not trimmed; remove the white space.",
                nameof(key));
        }

        if (IsReserved(key))
        {
            throw new ArgumentException(
                $"The metadata key ‘{key}’ is reserved. Keys must not match (case-insensitively) the standard " +
                "problem details members: ‘type’, ‘title’, ‘status’, ‘detail’, ‘instance’, ‘errors’, ‘detailTemplated’.",
                nameof(key));
        }
    }

    /// <summary>
    ///     Creates the exception for a key that differs only in case from a key of the existing metadata.
    /// </summary>
    /// <remarks>
    ///     The caller finds the <paramref name="existingKey" />; the existing metadata never holds two keys that differ only in case,
    ///     so there is at most one.
    /// </remarks>
    internal static ArgumentException KeyDiffersOnlyInCase(string key, string existingKey, string paramName)
    {
        return new ArgumentException(
            $"The metadata key ‘{key}’ differs only in case from the existing key ‘{existingKey}’. " +
            "Keys that differ only in case would be the same member of a problem details object; " +
            $"use ‘{existingKey}’ to replace the value, or remove it first.",
            paramName);
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
