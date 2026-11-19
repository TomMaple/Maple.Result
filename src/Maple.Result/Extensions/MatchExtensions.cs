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
///     The collection of extension methods that fold a <see cref="Result" /> or <see cref="Result{T}" /> into one value: a function for the
///     success and a function for the error, which return the same type.
/// </summary>
/// <remarks>
///     <c>Match</c> returns what the function that ran returns, and does not look at it: a value, a <see cref="Result" />, a
///     <see cref="Result{T}" />, or a task, which the caller awaits (e.g., <c>await result.Match(x => CountAsync(x), e => CountAsync(0))</c>).
///     For two side effects, use <c>Switch</c>: two actions return no value, so they do not compile here.
/// </remarks>
public static partial class MatchExtensions
{
    #region Result

    /// <summary>
    ///     Invokes one of the provided functions depending on whether the <paramref name="result" /> is successful and returns the value that the
    ///     invoked function returns.
    /// </summary>
    /// <remarks>
    ///     The value is returned as the function returns it, <see langword="null" /> included. A function that returns a task gives that task,
    ///     not awaited; a function that returns a <see cref="Result" /> and an error function that returns the <see cref="Error" /> give a
    ///     <see cref="Result" /> (e.g., <c>result.Match(() => Validate(), e => e)</c>).
    /// </remarks>
    /// <typeparam name="TOut">The type that both functions return.</typeparam>
    /// <param name="result">The <see cref="Result" /> whose state selects the function. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function invoked if the <paramref name="result" /> is successful. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The function invoked if the <paramref name="result" /> is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The value that <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" /> returns.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static TOut Match<TOut>(this Result result, Func<TOut> ifSuccessFunction, Func<Error, TOut> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess
            ? ifSuccessFunction()
            : ifErrorFunction(result.Error);
    }

    #endregion

    #region Result<T>

    /// <summary>
    ///     Invokes one of the provided functions depending on whether the <paramref name="result" /> is successful and returns the value that the
    ///     invoked function returns.
    /// </summary>
    /// <remarks>
    ///     The value is returned as the function returns it, <see langword="null" /> included. A function that returns a task gives that task,
    ///     not awaited; a function that returns a <see cref="Result{T}" /> and an error function that returns the <see cref="Error" /> give a
    ///     <see cref="Result{T}" /> (e.g., <c>result.Match(x => Validate(x), e => e)</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the value of the <paramref name="result" />.</typeparam>
    /// <typeparam name="TOut">The type that both functions return.</typeparam>
    /// <param name="result">The <see cref="Result{T}" /> whose state selects the function. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function invoked if the <paramref name="result" /> is successful, with its value. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The function invoked if the <paramref name="result" /> is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The value that <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" /> returns.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static TOut Match<T, TOut>(this Result<T> result, Func<T, TOut> ifSuccessFunction, Func<Error, TOut> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess
            ? ifSuccessFunction(result.Value)
            : ifErrorFunction(result.Error);
    }

    #endregion
}
