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

namespace Maple.Result.Helpers;

/// <summary>
///     A read-only dictionary over a private copy of string-keyed values, with keys compared ordinally. No other code holds the copy, so the
///     content never changes.
/// </summary>
/// <remarks>
///     Used for <see cref="TemplatedMessage.Params" /> and <see cref="Error.Metadata" />. The copy is exposed internally, so that equality, hashing
///     and the text output can enumerate it without allocating.
/// </remarks>
internal sealed class ReadOnlyCopy : ReadOnlyDictionary<string, object?>
{
    /// <summary>
    ///     Wraps the <paramref name="copy" />, which must use the ordinal comparer and must not be referenced by any other code.
    /// </summary>
    internal ReadOnlyCopy(Dictionary<string, object?> copy)
        : base(copy)
    {
        Copy = copy;
    }

    /// <summary>
    ///     The wrapped copy.
    /// </summary>
    internal Dictionary<string, object?> Copy { get; }
}
