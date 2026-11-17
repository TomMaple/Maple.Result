// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Results.Extensions;

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

        return result.Then(selector);
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
    /// <param name="selector">
    ///     A function that takes the value of the initial result and returns the <see cref="Error" /> of the failed result.
    /// </param>
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

        return result.Then(x =>
        {
            var selectedCollection = collectionSelector(x);
            var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

            return collectionResult.Then(y => resultSelector(x, y));
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

        return result.Then(x =>
        {
            var selectedCollection = collectionSelector(x);
            var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

            return collectionResult.Then(y =>
            {
                var selected = resultSelector(x, y);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        });
    }

    #region Not supported: selectors returning a task

    // These overloads turn a query whose select returns a task into a compile error (CS0619). Without them, the query binds to the
    // generic overload, which wraps the task in a successful result without awaiting it (issue #126). They are generic and constrained
    // to the task type (#251): a selector that only throws has no return type to infer, so it does not reach them, and a selector that
    // returns another value fails the constraint. The constraint to a Task also covers a Task<T>; IEquatable<ValueTask> is implemented
    // by ValueTask only, and a ValueTask<T> has an overload of its own, which is more specific than the generic overload. The unused
    // optional parameter only makes the signatures differ from the generic overloads. The overloads constrained to a Task or a
    // ValueTask have priority 1, so that they win over the generic overloads, also with explicit type arguments (method syntax); a task
    // is kept on purpose by selecting it in a result (e.g., select Result.FromValue(SaveAsync(x))). An async lambda (method syntax)
    // gives a Task, which fails the ValueTask constraint, so the two are never ambiguous.

    private const string SelectorTaskNotAwaitedMessage =
        "The selector returns a task, which the query does not await. " +
        "Use ThenAsync (or IfSuccessAsync for a task without a value), or a from clause whose function returns a Task<Result<T>>, " +
        "or select the task in a result to keep it (e.g., select Result.FromValue(SaveAsync(x))).";

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Select<T, TTask>(this Result<T> result, Func<T, TTask> selector, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Select<T, TValueTask>(this Result<T> result, Func<T, TValueTask> selector, ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result Select<T, TIgnored>(this Result<T> result, Func<T, ValueTask<TIgnored>> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result SelectMany<T, TMiddle, TTask>(
        this Result<T> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, TTask> resultSelector,
        Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result SelectMany<T, TMiddle, TValueTask>(
        this Result<T> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, TValueTask> resultSelector,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result SelectMany<T, TMiddle, TIgnored>(
        this Result<T> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, ValueTask<TIgnored>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    #endregion

    #region Non-generic results and selected results

    // See issue #217.

    /// <summary>
    ///     Projects a successful result (the <c>select</c> clause of a query); a failed result is returned unchanged.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A result that the selector returns is not nested (as by <c>Then</c>): a <see cref="Result" /> or a <see cref="Result{T}" /> is
    ///         returned as it is. Specify the type arguments to keep it nested on purpose.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the source result.</typeparam>
    /// <param name="result">The result to project. Must not be <see langword="null" />.</param>
    /// <param name="selector">The function that receives the value and returns the projection. Must not be <see langword="null" />.</param>
    /// <returns>The result that the selector returns (not nested), or the failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" /> or the <paramref name="selector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="selector" /> returns <see langword="null" />.
    /// </exception>
    public static Result Select<T>(this Result<T> result, Func<T, Result> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        if (result.IsError)
            return result.Error;

        var selected = selector(result.Value);

        return Guard.EnsureNotNullReturned(selected, nameof(selector));
    }

    /// <summary>
    ///     Projects a successful result (the <c>select</c> clause of a query); a failed result is returned unchanged.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A result that the selector returns is not nested (as by <c>Then</c>): a <see cref="Result" /> or a <see cref="Result{T}" /> is
    ///         returned as it is. Specify the type arguments to keep it nested on purpose.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the source result.</typeparam>
    /// <typeparam name="TNext">The type of the value of the returned result.</typeparam>
    /// <param name="result">The result to project. Must not be <see langword="null" />.</param>
    /// <param name="selector">The function that receives the value and returns the projection. Must not be <see langword="null" />.</param>
    /// <returns>The result that the selector returns (not nested), or the failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" /> or the <paramref name="selector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="selector" /> returns <see langword="null" />.
    /// </exception>
    public static Result<TNext> Select<T, TNext>(this Result<T> result, Func<T, Result<TNext>> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        if (result.IsError)
            return result.Error;

        var selected = selector(result.Value);

        return Guard.EnsureNotNullReturned(selected, nameof(selector));
    }

    /// <summary>
    ///     Projects a successful result (the <c>select</c> clause of a query); a failed result is returned unchanged.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    /// </remarks>
    /// <typeparam name="TNext">The type of the value of the returned result.</typeparam>
    /// <param name="result">The result to project. Must not be <see langword="null" />.</param>
    /// <param name="selector">
    ///     The function that receives the successful result and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>A successful result with the value of the selector, or the failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" /> or the <paramref name="selector" /> is <see langword="null" />.
    /// </exception>
    public static Result<TNext> Select<TNext>(this Result result, Func<Result, TNext> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        return result.Then(() => selector(result));
    }

    /// <summary>
    ///     Projects a successful result (the <c>select</c> clause of a query); a failed result is returned unchanged.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    /// </remarks>
    /// <param name="result">The result to project. Must not be <see langword="null" />.</param>
    /// <param name="selector">
    ///     The function that receives the successful result and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>A failed result with the error of the selector, or the failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" /> or the <paramref name="selector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="selector" /> returns <see langword="null" />.
    /// </exception>
    public static Result Select(this Result result, Func<Result, Error> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        if (result.IsError)
            return result.Error;

        var selected = selector(result);

        return Guard.EnsureNotNullReturned(selected, nameof(selector));
    }

    /// <summary>
    ///     Projects a successful result (the <c>select</c> clause of a query); a failed result is returned unchanged.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    ///     <para>
    ///         A result that the selector returns is not nested (as by <c>Then</c>): a <see cref="Result" /> or a <see cref="Result{T}" /> is
    ///         returned as it is. Specify the type arguments to keep it nested on purpose.
    ///     </para>
    /// </remarks>
    /// <param name="result">The result to project. Must not be <see langword="null" />.</param>
    /// <param name="selector">
    ///     The function that receives the successful result and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the selector returns (not nested), or the failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" /> or the <paramref name="selector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="selector" /> returns <see langword="null" />.
    /// </exception>
    public static Result Select(this Result result, Func<Result, Result> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        if (result.IsError)
            return result.Error;

        var selected = selector(result);

        return Guard.EnsureNotNullReturned(selected, nameof(selector));
    }

    /// <summary>
    ///     Projects a successful result (the <c>select</c> clause of a query); a failed result is returned unchanged.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    ///     <para>
    ///         A result that the selector returns is not nested (as by <c>Then</c>): a <see cref="Result" /> or a <see cref="Result{T}" /> is
    ///         returned as it is. Specify the type arguments to keep it nested on purpose.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TNext">The type of the value of the returned result.</typeparam>
    /// <param name="result">The result to project. Must not be <see langword="null" />.</param>
    /// <param name="selector">
    ///     The function that receives the successful result and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the selector returns (not nested), or the failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" /> or the <paramref name="selector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="selector" /> returns <see langword="null" />.
    /// </exception>
    public static Result<TNext> Select<TNext>(this Result result, Func<Result, Result<TNext>> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        if (result.IsError)
            return result.Error;

        var selected = selector(result);

        return Guard.EnsureNotNullReturned(selected, nameof(selector));
    }

    /// <summary>
    ///     Chains a <c>from</c> clause of a query: if the source result is successful, gets the next result from the collection
    ///     selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed
    ///     result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A result that the result selector returns is not nested (as by <c>Then</c>): a <see cref="Result" /> or a
    ///         <see cref="Result{T}" /> is returned as it is. Specify the type arguments to keep it nested on purpose.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the source result.</typeparam>
    /// <typeparam name="TMiddle">The type of the value of the result that the collection selector returns.</typeparam>
    /// <param name="result">The source result. Must not be <see langword="null" />.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the value of the next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" />, the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static Result SelectMany<T, TMiddle>(
        this Result<T> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        if (result.IsError)
            return result.Error;

        var selectedCollection = collectionSelector(result.Value);
        var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

        return collectionResult.Then(middle =>
        {
            var selected = resultSelector(result.Value, middle);
            return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
        });
    }

    /// <summary>
    ///     Chains a <c>from</c> clause of a query: if the source result is successful, gets the next result from the collection
    ///     selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed
    ///     result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A result that the result selector returns is not nested (as by <c>Then</c>): a <see cref="Result" /> or a
    ///         <see cref="Result{T}" /> is returned as it is. Specify the type arguments to keep it nested on purpose.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the source result.</typeparam>
    /// <typeparam name="TMiddle">The type of the value of the result that the collection selector returns.</typeparam>
    /// <typeparam name="TNext">The type of the value of the returned result.</typeparam>
    /// <param name="result">The source result. Must not be <see langword="null" />.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the value of the next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" />, the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static Result<TNext> SelectMany<T, TMiddle, TNext>(
        this Result<T> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        if (result.IsError)
            return result.Error;

        var selectedCollection = collectionSelector(result.Value);
        var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

        return collectionResult.Then(middle =>
        {
            var selected = resultSelector(result.Value, middle);
            return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
        });
    }

    /// <summary>
    ///     Chains a <c>from</c> clause of a query: if the source result is successful, gets the next result from the collection
    ///     selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed
    ///     result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the source result.</typeparam>
    /// <typeparam name="TNext">The type of the value of the returned result.</typeparam>
    /// <param name="result">The source result. Must not be <see langword="null" />.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the successful next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>A successful result with the value of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" />, the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" />.
    /// </exception>
    public static Result<TNext> SelectMany<T, TNext>(this Result<T> result, Func<T, Result> collectionSelector, Func<T, Result, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        if (result.IsError)
            return result.Error;

        var selectedCollection = collectionSelector(result.Value);
        var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

        return collectionResult.Then(() => resultSelector(result.Value, collectionResult));
    }

    /// <summary>
    ///     Chains a <c>from</c> clause of a query: if the source result is successful, gets the next result from the collection
    ///     selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed
    ///     result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the source result.</typeparam>
    /// <param name="result">The source result. Must not be <see langword="null" />.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the successful next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>A failed result with the error of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" />, the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static Result SelectMany<T>(this Result<T> result, Func<T, Result> collectionSelector, Func<T, Result, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        if (result.IsError)
            return result.Error;

        var selectedCollection = collectionSelector(result.Value);
        var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

        return collectionResult.Then(() =>
        {
            var selected = resultSelector(result.Value, collectionResult);
            return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
        });
    }

    /// <summary>
    ///     Chains a <c>from</c> clause of a query: if the source result is successful, gets the next result from the collection
    ///     selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed
    ///     result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    ///     <para>
    ///         A result that the result selector returns is not nested (as by <c>Then</c>): a <see cref="Result" /> or a
    ///         <see cref="Result{T}" /> is returned as it is. Specify the type arguments to keep it nested on purpose.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the source result.</typeparam>
    /// <param name="result">The source result. Must not be <see langword="null" />.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the successful next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" />, the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static Result SelectMany<T>(this Result<T> result, Func<T, Result> collectionSelector, Func<T, Result, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        if (result.IsError)
            return result.Error;

        var selectedCollection = collectionSelector(result.Value);
        var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

        return collectionResult.Then(() =>
        {
            var selected = resultSelector(result.Value, collectionResult);
            return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
        });
    }

    /// <summary>
    ///     Chains a <c>from</c> clause of a query: if the source result is successful, gets the next result from the collection
    ///     selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed
    ///     result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    ///     <para>
    ///         A result that the result selector returns is not nested (as by <c>Then</c>): a <see cref="Result" /> or a
    ///         <see cref="Result{T}" /> is returned as it is. Specify the type arguments to keep it nested on purpose.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the source result.</typeparam>
    /// <typeparam name="TNext">The type of the value of the returned result.</typeparam>
    /// <param name="result">The source result. Must not be <see langword="null" />.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the successful next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" />, the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static Result<TNext> SelectMany<T, TNext>(
        this Result<T> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        if (result.IsError)
            return result.Error;

        var selectedCollection = collectionSelector(result.Value);
        var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

        return collectionResult.Then(() =>
        {
            var selected = resultSelector(result.Value, collectionResult);
            return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
        });
    }

    /// <summary>
    ///     Chains a <c>from</c> clause of a query: if the source result is successful, gets the next result from the collection
    ///     selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed
    ///     result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    /// </remarks>
    /// <typeparam name="TMiddle">The type of the value of the result that the collection selector returns.</typeparam>
    /// <typeparam name="TNext">The type of the value of the returned result.</typeparam>
    /// <param name="result">The source result. Must not be <see langword="null" />.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the value of the next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>A successful result with the value of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" />, the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" />.
    /// </exception>
    public static Result<TNext> SelectMany<TMiddle, TNext>(
        this Result result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        if (result.IsError)
            return result.Error;

        var selectedCollection = collectionSelector(result);
        var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

        return collectionResult.Then(middle => resultSelector(result, middle));
    }

    /// <summary>
    ///     Chains a <c>from</c> clause of a query: if the source result is successful, gets the next result from the collection
    ///     selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed
    ///     result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    /// </remarks>
    /// <typeparam name="TMiddle">The type of the value of the result that the collection selector returns.</typeparam>
    /// <param name="result">The source result. Must not be <see langword="null" />.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the value of the next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>A failed result with the error of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" />, the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static Result SelectMany<TMiddle>(
        this Result result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        if (result.IsError)
            return result.Error;

        var selectedCollection = collectionSelector(result);
        var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

        return collectionResult.Then(middle =>
        {
            var selected = resultSelector(result, middle);
            return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
        });
    }

    /// <summary>
    ///     Chains a <c>from</c> clause of a query: if the source result is successful, gets the next result from the collection
    ///     selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed
    ///     result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    ///     <para>
    ///         A result that the result selector returns is not nested (as by <c>Then</c>): a <see cref="Result" /> or a
    ///         <see cref="Result{T}" /> is returned as it is. Specify the type arguments to keep it nested on purpose.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TMiddle">The type of the value of the result that the collection selector returns.</typeparam>
    /// <param name="result">The source result. Must not be <see langword="null" />.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the value of the next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" />, the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static Result SelectMany<TMiddle>(
        this Result result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        if (result.IsError)
            return result.Error;

        var selectedCollection = collectionSelector(result);
        var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

        return collectionResult.Then(middle =>
        {
            var selected = resultSelector(result, middle);
            return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
        });
    }

    /// <summary>
    ///     Chains a <c>from</c> clause of a query: if the source result is successful, gets the next result from the collection
    ///     selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed
    ///     result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    ///     <para>
    ///         A result that the result selector returns is not nested (as by <c>Then</c>): a <see cref="Result" /> or a
    ///         <see cref="Result{T}" /> is returned as it is. Specify the type arguments to keep it nested on purpose.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TMiddle">The type of the value of the result that the collection selector returns.</typeparam>
    /// <typeparam name="TNext">The type of the value of the returned result.</typeparam>
    /// <param name="result">The source result. Must not be <see langword="null" />.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the value of the next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" />, the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static Result<TNext> SelectMany<TMiddle, TNext>(
        this Result result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        if (result.IsError)
            return result.Error;

        var selectedCollection = collectionSelector(result);
        var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

        return collectionResult.Then(middle =>
        {
            var selected = resultSelector(result, middle);
            return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
        });
    }

    /// <summary>
    ///     Chains a <c>from</c> clause of a query: if the source result is successful, gets the next result from the collection
    ///     selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed
    ///     result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    /// </remarks>
    /// <typeparam name="TNext">The type of the value of the returned result.</typeparam>
    /// <param name="result">The source result. Must not be <see langword="null" />.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the successful next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>A successful result with the value of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" />, the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" />.
    /// </exception>
    public static Result<TNext> SelectMany<TNext>(
        this Result result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        if (result.IsError)
            return result.Error;

        var selectedCollection = collectionSelector(result);
        var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

        return collectionResult.Then(() => resultSelector(result, collectionResult));
    }

    /// <summary>
    ///     Chains a <c>from</c> clause of a query: if the source result is successful, gets the next result from the collection
    ///     selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed
    ///     result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    /// </remarks>
    /// <param name="result">The source result. Must not be <see langword="null" />.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the successful next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>A failed result with the error of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" />, the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static Result SelectMany(this Result result, Func<Result, Result> collectionSelector, Func<Result, Result, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        if (result.IsError)
            return result.Error;

        var selectedCollection = collectionSelector(result);
        var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

        return collectionResult.Then(() =>
        {
            var selected = resultSelector(result, collectionResult);
            return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
        });
    }

    /// <summary>
    ///     Chains a <c>from</c> clause of a query: if the source result is successful, gets the next result from the collection
    ///     selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed
    ///     result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    ///     <para>
    ///         A result that the result selector returns is not nested (as by <c>Then</c>): a <see cref="Result" /> or a
    ///         <see cref="Result{T}" /> is returned as it is. Specify the type arguments to keep it nested on purpose.
    ///     </para>
    /// </remarks>
    /// <param name="result">The source result. Must not be <see langword="null" />.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the successful next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" />, the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static Result SelectMany(this Result result, Func<Result, Result> collectionSelector, Func<Result, Result, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        if (result.IsError)
            return result.Error;

        var selectedCollection = collectionSelector(result);
        var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

        return collectionResult.Then(() =>
        {
            var selected = resultSelector(result, collectionResult);
            return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
        });
    }

    /// <summary>
    ///     Chains a <c>from</c> clause of a query: if the source result is successful, gets the next result from the collection
    ///     selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed
    ///     result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    ///     <para>
    ///         A result that the result selector returns is not nested (as by <c>Then</c>): a <see cref="Result" /> or a
    ///         <see cref="Result{T}" /> is returned as it is. Specify the type arguments to keep it nested on purpose.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TNext">The type of the value of the returned result.</typeparam>
    /// <param name="result">The source result. Must not be <see langword="null" />.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the successful next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" />, the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static Result<TNext> SelectMany<TNext>(
        this Result result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        if (result.IsError)
            return result.Error;

        var selectedCollection = collectionSelector(result);
        var collectionResult = Guard.EnsureNotNullReturned(selectedCollection, nameof(collectionSelector));

        return collectionResult.Then(() =>
        {
            var selected = resultSelector(result, collectionResult);
            return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
        });
    }

    #endregion

    #region Not supported: selectors returning a task, for the non-generic results

    // The traps of the region above, for the new source and collection shapes (#217): a result selector that returns a task
    // would bind to the overload with a TNext and wrap the task in the result.

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Select<TTask>(this Result result, Func<Result, TTask> selector, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Select<TValueTask>(this Result result, Func<Result, TValueTask> selector, ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result Select<TIgnored>(this Result result, Func<Result, ValueTask<TIgnored>> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result SelectMany<T, TTask>(
        this Result<T> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, TTask> resultSelector,
        Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result SelectMany<T, TValueTask>(
        this Result<T> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, TValueTask> resultSelector,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result SelectMany<T, TIgnored>(
        this Result<T> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, ValueTask<TIgnored>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result SelectMany<TMiddle, TTask>(
        this Result result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, TTask> resultSelector,
        Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result SelectMany<TMiddle, TValueTask>(
        this Result result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, TValueTask> resultSelector,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result SelectMany<TMiddle, TIgnored>(
        this Result result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, ValueTask<TIgnored>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result SelectMany<TTask>(
        this Result result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, TTask> resultSelector,
        Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result SelectMany<TValueTask>(
        this Result result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, TValueTask> resultSelector,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result SelectMany<TIgnored>(
        this Result result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, ValueTask<TIgnored>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    #endregion
}
