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
using System.Text;

namespace Maple.Result.Helpers;

/// <summary>
///     Content-based (order-independent) equality and hashing for string-keyed dictionaries, and their text representation.
/// </summary>
/// <remarks>
///     The methods enumerate <see cref="ImmutableStringDictionary.Items" /> directly: its struct enumerator allocates nothing, while an
///     enumerator obtained through an interface is allocated on the heap on every call.
/// </remarks>
internal static class DictionaryHelper
{
    /// <summary>
    ///     Appends the entries of the dictionary to the <paramref name="builder" /> as <c>{ key = value, … }</c> (<c>{}</c> when empty),
    ///     in the style of the compiler-generated <c>ToString</c> of a record.
    /// </summary>
    internal static void Append(StringBuilder builder, ImmutableStringDictionary dictionary)
    {
        if (dictionary.Count == 0)
        {
            builder.Append("{}");
            return;
        }

        builder.Append("{ ");

        var first = true;
        foreach (var pair in dictionary.Items)
        {
            if (!first)
                builder.Append(", ");

            builder.Append(pair.Key).Append(" = ").Append(pair.Value);
            first = false;
        }

        builder.Append(" }");
    }

    /// <summary>
    ///     Determines whether two dictionaries contain the same keys with equal values, regardless of order.
    /// </summary>
    internal static bool Equal(ImmutableStringDictionary? left, ImmutableStringDictionary? right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null || left.Count != right.Count)
            return false;

        foreach (var pair in left.Items)
        {
            if (!right.Items.TryGetValue(pair.Key, out var value) || !Equals(pair.Value, value))
                return false;
        }

        return true;
    }

    /// <summary>
    ///     Returns an order-independent hash code of the dictionary entries, consistent with <see cref="Equal" />.
    /// </summary>
    internal static int GetHashCode(ImmutableStringDictionary dictionary)
    {
        // Combine the entries in an order-independent way, since a dictionary is unordered.
        var hashCode = 0;
        foreach (var pair in dictionary.Items)
            hashCode ^= HashCode.Combine(pair.Key, pair.Value);

        return hashCode;
    }
}
