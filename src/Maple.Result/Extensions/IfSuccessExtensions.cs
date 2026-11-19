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

namespace Maple.Results.Extensions;

/// <summary>
///     The collection of extension methods that run a side effect when a <see cref="Result" /> or <see cref="Result{T}" /> is successful: an action
///     receives the value (if
///     any), and the result is returned unchanged.
/// </summary>
/// <remarks>
///     A failed result is returned unchanged, and the action is not called. To transform or chain the result with a function that
///     returns a value, a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" />, use <c>Then</c>: here it only runs
///     as a side effect, and what it returns is ignored. The analyzer of the package reports the likely mistakes (MR0004, MR0005).
/// </remarks>
public static partial class IfSuccessExtensions
{
    #region Result extensions

    /// <summary>
    ///     Invokes the specified action if the <paramref name="result" /> represents a successful outcome.
    /// </summary>
    /// <remarks>
    ///     This method enables fluent handling of successful <see cref="Result" />s by executing the provided action only when the <paramref name="result" />
    ///     indicates success. The action is not invoked if the <paramref name="result" /> is not successful.
    /// </remarks>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The action to execute if the <paramref name="result" /> is successful. Cannot be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The original <paramref name="result" />, allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result IfSuccess(this Result result, Action ifSuccessAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        if (result.IsSuccess)
            ifSuccessAction();

        return result;
    }

    #endregion

    #region Result<T> extensions

    /// <summary>
    ///     Invokes the specified action if the <paramref name="result" /> represents a successful outcome.
    /// </summary>
    /// <remarks>
    ///     This method enables fluent handling of successful <see cref="Result{T}" />s by executing the provided action only when
    ///     the <paramref name="result" /> indicates success. The action is not invoked if the <paramref name="result" /> is not successful.
    /// </remarks>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The action to execute if the <paramref name="result" /> is successful.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The original <paramref name="result" />, allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<T> IfSuccess<T>(this Result<T> result, Action<T> ifSuccessAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        if (result.IsSuccess)
            ifSuccessAction(result.Value);

        return result;
    }

    #endregion
}
