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
///         Two metadata keys of one error, or two parameter names of one message, that differ only in case are one name: clients often read JSON
///         case-insensitively, and naming policies change the case of the keys, so they would be the same member. The dictionaries compare them
///         with <see cref="Helpers.ImmutableStringDictionary.KeyComparer" /> (#349).
///     </para>
///     <para>
///         These are rules of this library, for consistency with the title and the metadata keys, not of an RFC: <i>RFC 8259</i> (section 4)
///         allows any string as a member name, and the naming recommendations of <i>RFC 9457</i> (section 4) are not mandatory.
///     </para>
/// </remarks>
internal static class NameValidator
{
    /// <summary>
    ///     Determines whether the name is not empty and does not start or end with white space.
    /// </summary>
    internal static bool IsValid(string name)
    {
        return name.Length > 0
               && !char.IsWhiteSpace(name[0])
               && !char.IsWhiteSpace(name[^1]);
    }
}
