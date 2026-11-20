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
using System.Linq;
using System.Reflection;

namespace Maple.Results.Tests.Unit;

/// <summary>
///     The library has no public type or member marked <see cref="ObsoleteAttribute" />. Its overload traps (hidden overloads marked
///     obsolete as an error, which turned an unsupported call into a compile error) were replaced by the analyzer (#223); a trap that
///     comes back fails this test.
/// </summary>
public class ObsoleteMemberUnitTests
{
    [Fact]
    public void PublicTypesAndMembers_OfTheLibrary_AreNotMarkedObsolete()
    {
        // Arrange
        const BindingFlags PublicMembers = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        var types = typeof(Result).Assembly.GetExportedTypes();

        // Act
        var result = types
            .Concat(types.SelectMany(t => t.GetMembers(PublicMembers)))
            .Where(m => m.IsDefined(typeof(ObsoleteAttribute), inherit: false))
            .Select(m => m is Type ? m.ToString() : $"{m.DeclaringType}: {m}")
            .ToList();

        // Assert
        result.ShouldBeEmpty();
    }
}
