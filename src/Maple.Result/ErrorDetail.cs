// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Validators;
using System;

namespace Maple.Results;

/// <summary>
///     The structure for an individual error occurrence that contains a problem detail.
/// </summary>
/// <remarks>
///     <para>The record is immutable: its properties are get-only.</para>
///     <para>
///         The <see cref="PropertyPointer" /> is a JSON Pointer (<i>RFC 6901</i>) to the value in the input that was validated, as the client sent
///         it, whatever the transport: a JSON body (<c>/address/street</c>), the query string (<c>/page</c>, <c>/ids/1</c> for the second
///         <c>ids</c> parameter), a message (<c>/items/1/sku</c>) or a header (<c>/Idempotency-Key</c>). It is <see langword="null" /> for a
///         rule across several values.
///     </para>
///     <para>
///         A name taken from the input has to be escaped: <c>~</c> as <c>~0</c> first, then <c>/</c> as <c>~1</c>. Otherwise, a name with a
///         <c>~</c> makes the constructor throw, and a name with a <c>/</c> gives a wrong pointer.
///     </para>
///     <para>
///         For more details about JSON Pointers, see: <seealso href="https://datatracker.ietf.org/doc/html/rfc6901"/>.
///     </para>
/// </remarks>
/// <param name="PropertyPointer">
///     The JSON Pointer which identifies the invalid value in the input data: <see langword="null" />, empty (the whole input), a pointer that
///     starts with <c>/</c> (e.g., <c>/email</c>, <c>/items/0/name</c>), or its URI fragment form (e.g., <c>#/email</c>). It is stored as it is
///     given, so <c>/email</c> and <c>#/email</c> are different values.
/// </param>
/// <param name="Detail">The human-readable explanation specific to this individual error occurrence.</param>
/// <param name="DetailTemplated">The message template with the human-readable explanation.</param>
/// <exception cref="ArgumentNullException">If the <paramref name="Detail" /> is <see langword="null" />.</exception>
/// <exception cref="ArgumentException">
///     If the <paramref name="PropertyPointer" /> is not a JSON Pointer (e.g., <c>Email</c> or <c>Items[0].Name</c>), or contains a <c>~</c>
///     that is not followed by <c>0</c> or <c>1</c>.
/// </exception>
public sealed record ErrorDetail(string? PropertyPointer, string Detail, TemplatedMessage? DetailTemplated = null)
{
    /// <summary>
    ///     The JSON Pointer which identifies the invalid value in the input data: <see langword="null" />, or a JSON Pointer in the string form
    ///     (e.g., <c>/email</c>) or in the URI fragment form (e.g., <c>#/email</c>), as it was given.
    /// </summary>
    public string? PropertyPointer { get; } = JsonPointerValidator.Validate(PropertyPointer, nameof(PropertyPointer));

    /// <summary>
    ///     The human-readable explanation specific to this individual error occurrence (never <see langword="null" />).
    /// </summary>
    public string Detail { get; } = Detail ?? throw new ArgumentNullException(nameof(Detail));

    /// <summary>
    ///     The message template with the human-readable explanation.
    /// </summary>
    public TemplatedMessage? DetailTemplated { get; } = DetailTemplated;
}
