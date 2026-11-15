// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Result.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Result.Extensions;

/// <summary>
///     The collection of LINQ extension methods for a <see cref="Result" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of operations and using LINQ query syntax.
///     They help simplify error handling and reduce boilerplate code when working with <see cref="Result{T}" />-based workflows.
/// </remarks>
public static class LinqExtensions
{
    /// <summary>
    ///     Projects the value of a successful <see cref="Result{T}" /> into a new <see cref="Result{TNext}" /> by applying the provided
    ///     <paramref name="selector" /> function; or returns a new <see cref="Result{TNext}" /> with the same error, otherwise.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed <paramref name="selector" /> function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The output type of the passed <paramref name="selector" /> function and the type of the <see cref="Result{TNext}" /> value to return.
    /// </typeparam>
    /// <param name="result">The initial result to transform if it is successful.</param>
    /// <param name="selector">A function that maps the value of the initial result to the final value.</param>
    /// <returns>
    ///     A successful <see cref="Result{TNext}" /> containing the value produced by <paramref name="selector" /> if the <paramref name="result" /> is
    ///     successful; otherwise, a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="selector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     This method enables the single-clause LINQ query syntax (a <c>from … select …</c> query without an intermediate <c>from</c>) on
    ///     a <see cref="Result{T}" />, projecting a successful value while propagating an error unchanged.
    /// </remarks>
    public static Result<TNext> Select<T, TNext>(this Result<T> result, Func<T, TNext> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        return result.IfSuccess(selector);
    }

    /// <summary>
    ///     Applies the provided <paramref name="selector" /> to the value of a successful <see cref="Result{T}" /> and returns a failed
    ///     <see cref="Result" /> with the <see cref="Error" /> it returns; otherwise, returns a failed <see cref="Result" /> with the original
    ///     <see cref="Error" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed <paramref name="selector" /> function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <param name="result">The initial result.</param>
    /// <param name="selector">A function that takes the value of the initial result and returns the <see cref="Error" /> of the failed result.</param>
    /// <returns>
    ///     A failed <see cref="Result" /> with the <see cref="Error" /> returned by <paramref name="selector" /> if the <paramref name="result" /> is
    ///     successful; otherwise, a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="selector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="selector" /> returns <see langword="null" />.</exception>
    /// <remarks>
    ///     <para>
    ///         This method makes a query that selects an <see cref="Error" />, e.g., <c>from user in GetUser(id) select Error.Conflict(…)</c>,
    ///         return a failed <see cref="Result" />.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a query would bind to <see cref="Select{T, TNext}(Result{T}, Func{T, TNext})" /> and return
    ///         a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///     </para>
    /// </remarks>
    public static Result Select<T>(this Result<T> result, Func<T, Error> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        if (result.IsError)
            return result.Error;

        var returnedError = selector(result.Value);
        var error = Guard.EnsureNotNullReturned(returnedError, nameof(selector));

        return Result.FromError(error);
    }

    /// <summary>
    ///     Returns a new <see cref="Result{TNext}" /> instance by applying the provided collection selector and a result selector functions,
    ///     if the current <see cref="Result{T}" /> is successful; or a new <see cref="Result{TNext}" /> instance with the same error, otherwise.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed <paramref name="collectionSelector" /> and
    ///     <paramref name="resultSelector" /> functions. If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TMiddle">
    ///     The type of the <see cref="Result{TMiddle}" /> output type of the passed <paramref name="collectionSelector" /> function and
    ///     the type of one of the parameters passed to the <paramref name="resultSelector" /> function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The type of the <see cref="Result{TNext}" /> value to return.
    /// </typeparam>
    /// <param name="result">The initial result to transform if it is successful.</param>
    /// <param name="collectionSelector">
    ///     A function that takes the value of the initial result and returns a new result representing an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that combines the value from the initial result and the intermediate value to produce the final value.
    /// </param>
    /// <returns>
    ///     A result containing the value produced by the result selector if both the initial result and the intermediate result are successful;
    ///     otherwise, a failed result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="collectionSelector" /> returns <see langword="null" />.</exception>
    /// <remarks>
    ///     This method enables chaining of operations on successful results while propagating errors in LINQ query syntax without additional boilerplate code.
    /// </remarks>
    public static Result<TNext> SelectMany<T, TMiddle, TNext>(
        this Result<T> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return result.IfSuccess(x =>
        {
            var selectedCollection = collectionSelector(x);
            var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

            return collectionResult.IfSuccess(y => resultSelector(x, y));
        });
    }

    /// <summary>
    ///     Applies the provided <paramref name="collectionSelector" /> and <paramref name="resultSelector" /> functions to the value of a successful
    ///     <see cref="Result{T}" /> and returns a failed <see cref="Result" /> with the <see cref="Error" /> returned by <paramref name="resultSelector" />;
    ///     otherwise, returns a failed <see cref="Result" /> with the first <see cref="Error" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.
    /// </typeparam>
    /// <typeparam name="TMiddle">The type of the value of the intermediate result returned by <paramref name="collectionSelector" />.</typeparam>
    /// <param name="result">The initial result.</param>
    /// <param name="collectionSelector">
    ///     A function that takes the value of the initial result and returns a result with an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that takes the value of the initial result and the intermediate value, and returns the <see cref="Error" /> of the failed result.
    /// </param>
    /// <returns>
    ///     A failed <see cref="Result" /> with the <see cref="Error" /> returned by <paramref name="resultSelector" /> if both the initial and
    ///     the intermediate results are successful; otherwise, a failed <see cref="Result" /> with the first <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are <see
    ///     langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="collectionSelector" /> returns <see langword="null" />, or <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method makes a query with more than one <c>from</c> clause that selects an <see cref="Error" />,
    ///         e.g., <c>from user in GetUser(id) from order in GetOrder(user) select Error.Conflict(…)</c>, return a failed <see cref="Result" />.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a query would bind to
    ///         <see cref="SelectMany{T, TMiddle, TNext}(Result{T}, Func{T, Result{TMiddle}}, Func{T, TMiddle, TNext})" /> and return
    ///         a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///     </para>
    /// </remarks>
    public static Result SelectMany<T, TMiddle>(
        this Result<T> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return result.IfSuccess(x =>
        {
            var selectedCollection = collectionSelector(x);
            var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

            return collectionResult.IfSuccess(y =>
            {
                var selected = resultSelector(x, y);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        });
    }

    #region Not supported: selectors returning a task

    // These overloads turn a query whose select returns a task into a compile error (CS0619). Without them, the query binds to the
    // generic overload, which wraps the task in a successful result without awaiting it (issue #126). They are more specific than the
    // generic overloads, so they are chosen first; explicit type arguments (method syntax) exclude them and keep the task on purpose.
    // The Task overloads have a higher priority than the ValueTask ones: an async lambda (possible in method syntax) converts to both,
    // which would be ambiguous (CS0121).

    private const string SelectorTaskNotAwaitedMessage =
        "The selector returns a task, which the query does not await. Use IfSuccessAsync, or a from clause whose function returns " +
        "a Task<Result<T>>, or specify the type arguments (e.g., Select<T, Task>(…)) to keep the task.";

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Select<T>(this Result<T> result, Func<T, Task> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TNext> Select<T, TNext>(this Result<T> result, Func<T, Task<TNext>> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result Select<T>(this Result<T> result, Func<T, ValueTask> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result<TNext> Select<T, TNext>(this Result<T> result, Func<T, ValueTask<TNext>> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result SelectMany<T, TMiddle>(this Result<T> result, Func<T, Result<TMiddle>> collectionSelector, Func<T, TMiddle, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TNext> SelectMany<T, TMiddle, TNext>(
        this Result<T> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result SelectMany<T, TMiddle>(this Result<T> result, Func<T, Result<TMiddle>> collectionSelector, Func<T, TMiddle, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result<TNext> SelectMany<T, TMiddle, TNext>(
        this Result<T> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    #endregion
}
