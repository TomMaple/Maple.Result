// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Http;
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
///         For the same reason, keys that differ only in case are one key of the metadata
///         (<see cref="Helpers.ImmutableStringDictionary.KeyComparer" />): a key in another case replaces the entry (#349).
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
    /// <param name="key">The metadata key to validate.</param>
    /// <param name="paramName">
    ///     The name of the parameter of the called method that holds the key: <c>key</c> for a single entry, <c>metadata</c> for a batch
    ///     (#250).
    /// </param>
    /// <exception cref="ArgumentNullException">If the <paramref name="key" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="key" /> is empty, whitespace-only, starts or ends with white space, or matches a standard problem details member
    ///     name.
    /// </exception>
    internal static void Validate(string key, string paramName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key, paramName);

        if (!NameValidator.IsValid(key))
        {
            throw new ArgumentException(
                $"The metadata key ‘{key}’ starts or ends with white space. A key is not trimmed; remove the white space.",
                paramName);
        }

        if (IsReserved(key))
        {
            throw new ArgumentException(
                $"The metadata key ‘{key}’ is reserved. Keys must not match (case-insensitively) the standard " +
                "problem details members: ‘type’, ‘title’, ‘status’, ‘detail’, ‘instance’, ‘errors’, ‘detailTemplated’.",
                paramName);
        }
    }
}
