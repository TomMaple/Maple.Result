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
using System.Diagnostics.CodeAnalysis;

namespace Maple.Results.Extensions;

/// <summary>
///     The collection of extension methods that run a side effect when an <see cref="IOperationResult" /> is failed: an action receives
///     the <see cref="Error" />, and the result is returned unchanged.
/// </summary>
/// <remarks>
///     A successful result is returned unchanged, and the action is not called. To replace a failed result (e.g., recover with a
///     value, or return another error), use <c>Else</c>: here a function that returns a value only runs as a side effect, and the
///     failed result stays failed. The analyzer of the package reports the calls that are probably a mistake (MR0004, MR0005).
/// </remarks>
public static partial class IfErrorExtensions
{
    /// <summary>
    ///     Invokes the specified action if the <paramref name="result" /> represents an error, and returns the original <paramref name="result" />.
    /// </summary>
    /// <remarks>
    ///     Use this method to perform side effects, such as logging or error handling, when a <paramref name="result" /> indicates an error,
    ///     without altering the <paramref name="result" /> itself.
    ///     The method does not modify the <paramref name="result" /> or handle the <see cref="Error" /> beyond invoking the specified action.
    /// </remarks>
    /// <typeparam name="TResult">The type of the result object, which must implement the <see cref="IOperationResult" /> interface.</typeparam>
    /// <param name="result">The <paramref name="result" /> to inspect for an error. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorAction">
    ///     The action to execute if the <paramref name="result" /> is an error.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The original <paramref name="result" /> instance, regardless of whether the action was invoked.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static TResult IfError<TResult>([DisallowNull] this TResult result, Action<Error> ifErrorAction)
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsError)
            ifErrorAction(result.Error);

        return result;
    }
}
