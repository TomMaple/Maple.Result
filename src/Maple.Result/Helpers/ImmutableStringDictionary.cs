// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Maple.Results.Helpers;

/// <summary>
///     An immutable dictionary of string-keyed values, with keys compared ordinally: a read-only wrapper of a private copy that no other code
///     holds, so the content never changes and an instance can be shared.
/// </summary>
/// <remarks>
///     Used for <see cref="TemplatedMessage.Parameters" /> and <see cref="Error.Metadata" />. The copy is exposed internally as <see cref="Items" />,
///     so that equality, hashing and the text output can enumerate it without allocating.
/// </remarks>
internal sealed class ImmutableStringDictionary : ReadOnlyDictionary<string, object?>
{
    /// <summary>
    ///     Wraps the <paramref name="copy" />, which must use the ordinal comparer and must not be referenced by any other code.
    /// </summary>
    internal ImmutableStringDictionary(Dictionary<string, object?> copy)
        : base(copy)
    {
        Items = copy;
    }

    /// <summary>
    ///     The wrapped copy; it must not be changed.
    /// </summary>
    internal Dictionary<string, object?> Items { get; }
}
