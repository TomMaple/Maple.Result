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
using System.ComponentModel;

namespace Maple.Results.Extensions;

/// <summary>
///     The collection of extension methods that fold a <see cref="Result" /> or <see cref="Result{T}" /> into one value: a function for the
///     success and a function for the error, which return the same type.
/// </summary>
/// <remarks>
///     <c>Match</c> returns what the function that ran returns, and does not look at it: a value, a <see cref="Result" />, a
///     <see cref="Result{T}" />, or a task, which the caller awaits (e.g., <c>await result.Match(x => CountAsync(x), e => CountAsync(0))</c>).
///     For two side effects, use <c>Switch</c>; two actions are a compile error here (CS0619).
/// </remarks>
public static class MatchExtensions
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

    #region Not supported: two actions

    // Match is a fold: two functions that return one type. Without these overloads, two actions would report that no overload takes them;
    // these turn such a call into a compile error (CS0619) that names Switch, so that code written for the Match with two actions of 2.x gets
    // a message that says what to do. They return what that Match returned, so that its call reports this error only (e.g., in
    // var r = result.Match(…)), not one more for the type. A call that the fold accepts binds to the fold: a lambda with a value converts better to a function
    // than to an action. Two lambdas that only throw have no type to infer, so they bind here too.
    // See issue #255.

    private const string TwoActionsMessage =
        "Match takes two functions that return a value. Use Switch for two side effects, or Then(…).IfError(…) to transform the value and " +
        "handle the error.";

    /// <summary>
    ///     Not supported: <c>Match</c> takes two functions that return a value; use <c>Switch</c> for two actions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TwoActionsMessage, error: true)]
    public static Result Match(this Result result, Action ifSuccessAction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(TwoActionsMessage);
    }

    /// <summary>
    ///     Not supported: <c>Match</c> takes two functions that return a value; use <c>Switch</c> for two actions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TwoActionsMessage, error: true)]
    public static Result<T> Match<T>(this Result<T> result, Action<T> ifSuccessAction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(TwoActionsMessage);
    }

    #endregion
}
