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
///     The collection of extension methods that run one of two actions depending on whether a <see cref="Result" /> or <see cref="Result{T}" />
///     is successful, and return nothing.
/// </summary>
/// <remarks>
///     <c>Switch</c> ends a chain for side effects, as <c>Match</c> ends it for a value. To run a side effect and keep chaining, use
///     <c>IfSuccess(…).IfError(…)</c>, which return the result. An action that returns a value is a side effect (e.g.,
///     <c>x => set.Add(x)</c>); a function that returns a task is not awaited, and the analyzer of the package reports it (MR0002).
/// </remarks>
public static partial class SwitchExtensions
{
    #region Result

    /// <summary>
    ///     Invokes one of the provided actions depending on whether the <paramref name="result" /> is successful.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> whose state selects the action. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The action invoked if the <paramref name="result" /> is successful. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action invoked if the <paramref name="result" /> is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static void Switch(this Result result, Action ifSuccessAction, Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess)
            ifSuccessAction();
        else
            ifErrorAction(result.Error);
    }

    #endregion

    #region Result<T>

    /// <summary>
    ///     Invokes one of the provided actions depending on whether the <paramref name="result" /> is successful.
    /// </summary>
    /// <typeparam name="T">The type of the value of the <paramref name="result" />.</typeparam>
    /// <param name="result">The <see cref="Result{T}" /> whose state selects the action. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The action invoked if the <paramref name="result" /> is successful, with its value. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action invoked if the <paramref name="result" /> is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static void Switch<T>(this Result<T> result, Action<T> ifSuccessAction, Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess)
            ifSuccessAction(result.Value);
        else
            ifErrorAction(result.Error);
    }

    #endregion
}
