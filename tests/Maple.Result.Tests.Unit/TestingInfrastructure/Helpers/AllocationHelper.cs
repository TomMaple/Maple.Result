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

namespace Maple.Result.Tests.Unit.TestingInfrastructure.Helpers;

/// <summary>
///     Measures the memory that code allocates.
/// </summary>
internal static class AllocationHelper
{
    private const int Runs = 5;

    /// <summary>
    ///     Returns the number of bytes the <paramref name="action" /> allocates on the current thread: the smallest of several runs, after two
    ///     warm-up runs, so that one-time allocations (e.g., static initialization) are not counted.
    /// </summary>
    internal static long AllocatedBytes(Action action)
    {
        action();
        action();

        var smallest = long.MaxValue;
        for (var run = 0; run < Runs; run++)
        {
            var before = GC.GetAllocatedBytesForCurrentThread();
            action();
            var allocated = GC.GetAllocatedBytesForCurrentThread() - before;

            smallest = Math.Min(smallest, allocated);
        }

        return smallest;
    }
}
