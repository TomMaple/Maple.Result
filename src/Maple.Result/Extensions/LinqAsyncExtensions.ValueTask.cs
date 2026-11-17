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

// The ValueTask-based methods of LinqAsyncExtensions (#248); the class is documented in LinqAsyncExtensions.Task.cs.
public static partial class LinqAsyncExtensions
{
    /// <summary>
    ///     Projects the value of a successful <see cref="Result{T}" /> produced by the awaited <paramref name="result" /> into a new
    ///     <see cref="Result{TNext}" /> by applying the provided <paramref name="selector" /> function;
    ///     or returns a new <see cref="Result{TNext}" /> with the same error, otherwise.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed <paramref name="selector" /> function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The output type of the passed <paramref name="selector" /> function and the type of the <see cref="Result{TNext}" /> value to return.
    /// </typeparam>
    /// <param name="result">
    ///     The <see cref="ValueTask{TResult}" /> that represents the initial result to transform if it is successful.
    /// </param>
    /// <param name="selector">A function that maps the value of the initial result to the final value.</param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a successful <see cref="Result{TNext}" /> containing the value produced by
    ///     <paramref name="selector" /> if the <paramref name="result" /> is successful;
    ///     otherwise, a <see cref="ValueTask{TResult}" /> that represents a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="selector" /> parameter is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     This method enables the single-clause LINQ query syntax (a <c>from … select …</c> query without an intermediate <c>from</c>) on
    ///     a <see cref="ValueTask{TResult}" /> of a <see cref="Result{T}" />, projecting a successful value while propagating an error unchanged.
    /// </remarks>
    public static ValueTask<Result<TNext>> Select<T, TNext>(this ValueTask<Result<T>> result, Func<T, TNext> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return Core(result, selector);

        static async ValueTask<Result<TNext>> Core(ValueTask<Result<T>> result, Func<T, TNext> selector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.Then(selector);
        }
    }

    /// <summary>
    ///     Applies the provided <paramref name="selector" /> to the value of a successful <see cref="Result{T}" /> produced by the awaited
    ///     <paramref name="result" /> and returns a failed <see cref="Result" /> with the <see cref="Error" /> it returns;
    ///     otherwise, returns a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed <paramref name="selector" /> function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <param name="result">The <see cref="ValueTask{TResult}" /> that represents the initial result.</param>
    /// <param name="selector">
    ///     A function that takes the value of the initial result and returns the <see cref="Error" /> of the failed result.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="selector" /> if the <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="selector" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />,
    ///     or <paramref name="selector" /> returns <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method makes a query that selects an <see cref="Error" />, e.g., <c>from user in GetUserAsync(id) select Error.Conflict(…)</c>,
    ///         return a failed <see cref="Result" />.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a query would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///     </para>
    /// </remarks>
    public static ValueTask<Result> Select<T>(this ValueTask<Result<T>> result, Func<T, Error> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return Core(result, selector);

        static async ValueTask<Result> Core(ValueTask<Result<T>> result, Func<T, Error> selector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var returnedError = selector(resultValue.Value);
            var error = Guard.EnsureNotNullReturned(returnedError, nameof(selector));

            return Result.FromError(error);
        }
    }

    /// <summary>
    ///     Returns a new instance of the <see cref="ValueTask{TResult}" /> that represents the result of applying the provided collection selector and
    ///     a result selector functions if the current <see cref="Result{T}" /> is successful,
    ///     or the new <see cref="ValueTask{TResult}" /> that represents a new <see cref="Result{TNext}" /> instance with the same error otherwise.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed <paramref name="collectionSelector" /> and
    ///     <paramref name="resultSelector" /> functions. If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TMiddle">
    ///     The type of the <see cref="Result{TMiddle}" /> output type of the passed <paramref name="collectionSelector" /> function and the type of one of
    ///     the parameters passed to the <paramref name="resultSelector" /> function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The output type of the <see cref="Result{TNext}" /> value to return.
    /// </typeparam>
    /// <param name="result">
    ///     The <see cref="ValueTask{TResult}" /> that represents the initial result to transform if it is successful.
    /// </param>
    /// <param name="collectionSelector">
    ///     A function that takes the value of the initial result and returns a new result representing an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that combines the value from the initial result and the intermediate value to produce the final value.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />,
    ///     or <paramref name="collectionSelector" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the result of applying the provided collection selector and a result selector functions
    ///     if the current <paramref name="result" /> is successful; otherwise, a <see cref="ValueTask{TResult}" /> that represents a failed result.
    /// </returns>
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> result,
            Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<T, TMiddle, TNext> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            return await resultValue.ThenAsync(async ValueTask<Result<TNext>> (x) =>
            {
                var collectionTask = collectionSelector(x);
                var collectionResult = await Guard.EnsureNotNullResult(
                    collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

                return collectionResult.Then(y => resultSelector(x, y));
            }).ConfigureAwait(ResultConfiguration.ContinueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Applies the synchronous <paramref name="collectionSelector" /> and the <paramref name="resultSelector" /> to the value of a successful
    ///     <see cref="Result{T}" /> produced by the awaited <paramref name="result" />;
    ///     otherwise, returns a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables LINQ query syntax that mixes an asynchronous <c>from</c> clause followed by a synchronous one,
    ///     e.g., <c>from user in GetUserAsync(id) from address in GetAddress(user) select …</c>.
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.
    /// </typeparam>
    /// <typeparam name="TMiddle">The type of the value of the intermediate result returned by <paramref name="collectionSelector" />.</typeparam>
    /// <typeparam name="TNext">The type of the value of the final result, produced by <paramref name="resultSelector" />.</typeparam>
    /// <param name="result">The <see cref="ValueTask{TResult}" /> that represents the initial result.</param>
    /// <param name="collectionSelector">
    ///     A synchronous function that takes the value of the initial result and returns a result with an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that combines the value of the initial result and the intermediate value to produce the final value.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the result of applying the selectors if both the initial and the intermediate results are
    ///     successful; otherwise, a failed result with the first <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />,
    ///     <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />,
    ///     or <paramref name="collectionSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> result,
            Func<T, Result<TMiddle>> collectionSelector,
            Func<T, TMiddle, TNext> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
    }

    /// <summary>
    ///     Applies the asynchronous <paramref name="collectionSelector" /> and the <paramref name="resultSelector" /> to the value of a successful
    ///     <paramref name="result" />; otherwise, returns a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables LINQ query syntax that mixes a synchronous <c>from</c> clause followed by an asynchronous one,
    ///     e.g., <c>from user in GetUser(id) from orders in GetOrdersAsync(user) select …</c>.
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.
    /// </typeparam>
    /// <typeparam name="TMiddle">The type of the value of the intermediate result returned by <paramref name="collectionSelector" />.</typeparam>
    /// <typeparam name="TNext">The type of the value of the final result, produced by <paramref name="resultSelector" />.</typeparam>
    /// <param name="result">The initial result.</param>
    /// <param name="collectionSelector">
    ///     An asynchronous function that takes the value of the initial result and returns a result with an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that combines the value of the initial result and the intermediate value to produce the final value.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the result of applying the selectors if both the initial and the intermediate results are
    ///     successful; otherwise, a failed result with the first <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />,
    ///     <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="collectionSelector" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            Result<T> result,
            Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<T, TMiddle, TNext> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result.Value);
            var collectionResult = await Guard.EnsureNotNullResult(collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(y => resultSelector(result.Value, y));
        }
    }

    /// <summary>
    ///     Applies the asynchronous <paramref name="collectionSelector" /> and the <paramref name="resultSelector" /> to the value of a successful
    ///     <see cref="Result{T}" /> produced by the awaited <paramref name="result" /> and returns a failed <see cref="Result" /> with
    ///     the <see cref="Error" /> returned by <paramref name="resultSelector" />; otherwise, returns a failed <see cref="Result" /> with
    ///     the first <see cref="Error" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.
    /// </typeparam>
    /// <typeparam name="TMiddle">The type of the value of the intermediate result returned by <paramref name="collectionSelector" />.</typeparam>
    /// <param name="result">The <see cref="ValueTask{TResult}" /> that represents the initial result.</param>
    /// <param name="collectionSelector">
    ///     An asynchronous function that takes the value of the initial result and returns a result with an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that takes the value of the initial result and the intermediate value, and returns the <see cref="Error" /> of the failed result.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="resultSelector" /> if both the initial and the intermediate results are successful;
    ///     otherwise, a failed <see cref="Result" /> with the first <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />,
    ///     <paramref name="collectionSelector" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method makes a query with more than one <c>from</c> clause that selects an <see cref="Error" />,
    ///         e.g., <c>from user in GetUserAsync(id) from order in GetOrderAsync(user) select Error.Conflict(…)</c>, return a failed <see cref="Result" />.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a query would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///     </para>
    /// </remarks>
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            ValueTask<Result<T>> result,
            Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<T, TMiddle, Error> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue.Value);
            var collectionResult = await Guard.EnsureNotNullResult(collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(y =>
            {
                var selected = resultSelector(resultValue.Value, y);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
    }

    /// <summary>
    ///     Applies the synchronous <paramref name="collectionSelector" /> and the <paramref name="resultSelector" /> to the value of a successful
    ///     <see cref="Result{T}" /> produced by the awaited <paramref name="result" /> and returns a failed <see cref="Result" /> with
    ///     the <see cref="Error" /> returned by <paramref name="resultSelector" />; otherwise, returns a failed <see cref="Result" /> with
    ///     the first <see cref="Error" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.
    /// </typeparam>
    /// <typeparam name="TMiddle">The type of the value of the intermediate result returned by <paramref name="collectionSelector" />.</typeparam>
    /// <param name="result">The <see cref="ValueTask{TResult}" /> that represents the initial result.</param>
    /// <param name="collectionSelector">
    ///     A synchronous function that takes the value of the initial result and returns a result with an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that takes the value of the initial result and the intermediate value, and returns the <see cref="Error" /> of the failed result.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="resultSelector" /> if both the initial and the intermediate results are successful;
    ///     otherwise, a failed <see cref="Result" /> with the first <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />,
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />, or <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method makes a query with more than one <c>from</c> clause that selects an <see cref="Error" />,
    ///         e.g., <c>from user in GetUserAsync(id) from order in GetOrder(user) select Error.Conflict(…)</c>, return a failed <see cref="Result" />.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a query would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///     </para>
    /// </remarks>
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this ValueTask<Result<T>> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(ValueTask<Result<T>> result, Func<T, Result<TMiddle>> collectionSelector, Func<T, TMiddle, Error> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
    }

    /// <summary>
    ///     Applies the asynchronous <paramref name="collectionSelector" /> and the <paramref name="resultSelector" /> to the value of a successful
    ///     <paramref name="result" /> and returns a failed <see cref="Result" /> with the <see cref="Error" /> returned by <paramref name="resultSelector" />;
    ///     otherwise, returns a failed <see cref="Result" /> with the first <see cref="Error" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.
    /// </typeparam>
    /// <typeparam name="TMiddle">The type of the value of the intermediate result returned by <paramref name="collectionSelector" />.</typeparam>
    /// <param name="result">The initial result.</param>
    /// <param name="collectionSelector">
    ///     An asynchronous function that takes the value of the initial result and returns a result with an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that takes the value of the initial result and the intermediate value, and returns the <see cref="Error" /> of the failed result.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="resultSelector" /> if both the initial and the intermediate results are successful;
    ///     otherwise, a failed <see cref="Result" /> with the first <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are <see
    ///     langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="collectionSelector" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method makes a query with more than one <c>from</c> clause that selects an <see cref="Error" />,
    ///         e.g., <c>from user in GetUser(id) from order in GetOrderAsync(user) select Error.Conflict(…)</c>, return a failed <see cref="Result" />.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a query would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///     </para>
    /// </remarks>
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this Result<T> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(Result<T> result, Func<T, ValueTask<Result<TMiddle>>> collectionSelector, Func<T, TMiddle, Error> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result.Value);
            var collectionResult = await Guard.EnsureNotNullResult(collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(y =>
            {
                var selected = resultSelector(result.Value, y);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
    }

    #region Not supported: selectors returning a task

    // These overloads turn a query whose select returns a task into a compile error (CS0619). Without them, the query binds to the
    // generic overload, which wraps the task in a successful result without awaiting it (issue #126). They are more specific than the
    // generic overloads, so they are chosen first; explicit type arguments (method syntax) exclude them and keep the task on purpose.
    // The Task overloads have a higher priority than the ValueTask ones: an async lambda (possible in method syntax) converts to both,
    // which would be ambiguous (CS0121).

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> Select<T>(this ValueTask<Result<T>> result, Func<T, Task> selector)
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
    public static ValueTask<Result<TNext>> Select<T, TNext>(this ValueTask<Result<T>> result, Func<T, Task<TNext>> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> Select<T>(this ValueTask<Result<T>> result, Func<T, ValueTask> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> Select<T, TNext>(this ValueTask<Result<T>> result, Func<T, ValueTask<TNext>> selector)
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
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Task> resultSelector)
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
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, ValueTask<TNext>> resultSelector)
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
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this ValueTask<Result<T>> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, Task> resultSelector)
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
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this ValueTask<Result<T>> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, ValueTask<TNext>> resultSelector)
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
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this Result<T> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Task> resultSelector)
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
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this Result<T> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }
    #endregion

    #region Non-generic results and selected results

    // See issue #217.

    /// <summary>
    ///     Awaits the result and projects it if it is successful (the <c>select</c> clause of a query); a failed result is
    ///     returned unchanged.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A result that the selector returns is not nested (as by <c>Then</c>): a <see cref="Result" /> or a <see cref="Result{T}" /> is
    ///         returned as it is. Specify the type arguments to keep it nested on purpose.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the source result.</typeparam>
    /// <param name="result">The asynchronous operation that returns the result.</param>
    /// <param name="selector">The function that receives the value and returns the projection. Must not be <see langword="null" />.</param>
    /// <returns>The result that the selector returns (not nested), or the failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="selector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="selector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> Select<T>(this ValueTask<Result<T>> result, Func<T, Result> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return Core(result, selector);

        static async ValueTask<Result> Core(ValueTask<Result<T>> result, Func<T, Result> selector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.Select(selector);
        }
    }

    /// <summary>
    ///     Awaits the result and projects it if it is successful (the <c>select</c> clause of a query); a failed result is
    ///     returned unchanged.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A result that the selector returns is not nested (as by <c>Then</c>): a <see cref="Result" /> or a <see cref="Result{T}" /> is
    ///         returned as it is. Specify the type arguments to keep it nested on purpose.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the source result.</typeparam>
    /// <typeparam name="TNext">The type of the value of the returned result.</typeparam>
    /// <param name="result">The asynchronous operation that returns the result.</param>
    /// <param name="selector">The function that receives the value and returns the projection. Must not be <see langword="null" />.</param>
    /// <returns>The result that the selector returns (not nested), or the failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="selector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="selector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> Select<T, TNext>(this ValueTask<Result<T>> result, Func<T, Result<TNext>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return Core(result, selector);

        static async ValueTask<Result<TNext>> Core(ValueTask<Result<T>> result, Func<T, Result<TNext>> selector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.Select(selector);
        }
    }

    /// <summary>
    ///     Awaits the result and projects it if it is successful (the <c>select</c> clause of a query); a failed result is
    ///     returned unchanged.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    /// </remarks>
    /// <typeparam name="TNext">The type of the value of the returned result.</typeparam>
    /// <param name="result">The asynchronous operation that returns the result.</param>
    /// <param name="selector">
    ///     The function that receives the successful result and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>A successful result with the value of the selector, or the failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="selector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> Select<TNext>(this ValueTask<Result> result, Func<Result, TNext> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return Core(result, selector);

        static async ValueTask<Result<TNext>> Core(ValueTask<Result> result, Func<Result, TNext> selector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.Select(selector);
        }
    }

    /// <summary>
    ///     Awaits the result and projects it if it is successful (the <c>select</c> clause of a query); a failed result is
    ///     returned unchanged.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         A non-generic <see cref="Result" /> clause has no value: its range variable is the successful <see cref="Result" /> itself
    ///         (e.g., <c>from _ in Validate(input)</c>). A query cannot declare a range variable twice, so a second such clause needs another
    ///         name (e.g., <c>from active in …</c>).
    ///     </para>
    /// </remarks>
    /// <param name="result">The asynchronous operation that returns the result.</param>
    /// <param name="selector">
    ///     The function that receives the successful result and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>A failed result with the error of the selector, or the failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="selector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="selector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> Select(this ValueTask<Result> result, Func<Result, Error> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return Core(result, selector);

        static async ValueTask<Result> Core(ValueTask<Result> result, Func<Result, Error> selector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.Select(selector);
        }
    }

    /// <summary>
    ///     Awaits the result and projects it if it is successful (the <c>select</c> clause of a query); a failed result is
    ///     returned unchanged.
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
    /// <param name="result">The asynchronous operation that returns the result.</param>
    /// <param name="selector">
    ///     The function that receives the successful result and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the selector returns (not nested), or the failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="selector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="selector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> Select(this ValueTask<Result> result, Func<Result, Result> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return Core(result, selector);

        static async ValueTask<Result> Core(ValueTask<Result> result, Func<Result, Result> selector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.Select(selector);
        }
    }

    /// <summary>
    ///     Awaits the result and projects it if it is successful (the <c>select</c> clause of a query); a failed result is
    ///     returned unchanged.
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
    /// <param name="result">The asynchronous operation that returns the result.</param>
    /// <param name="selector">
    ///     The function that receives the successful result and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the selector returns (not nested), or the failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="selector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="selector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> Select<TNext>(this ValueTask<Result> result, Func<Result, Result<TNext>> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return Core(result, selector);

        static async ValueTask<Result<TNext>> Core(ValueTask<Result> result, Func<Result, Result<TNext>> selector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.Select(selector);
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the value of the next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or the
    ///     <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            ValueTask<Result<T>> result,
            Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<T, TMiddle, Result> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue.Value);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(middle =>
            {
                var selected = resultSelector(resultValue.Value, middle);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the value of the next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or the
    ///     <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> result,
            Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<T, TMiddle, Result<TNext>> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue.Value);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(middle =>
            {
                var selected = resultSelector(resultValue.Value, middle);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the successful next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>A successful result with the value of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> result,
            Func<T, ValueTask<Result>> collectionSelector,
            Func<T, Result, TNext> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue.Value);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() => resultSelector(resultValue.Value, collectionResult));
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the successful next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>A failed result with the error of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or the
    ///     <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<T>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            ValueTask<Result<T>> result,
            Func<T, ValueTask<Result>> collectionSelector,
            Func<T, Result, Error> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue.Value);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() =>
            {
                var selected = resultSelector(resultValue.Value, collectionResult);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the successful next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or the
    ///     <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<T>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            ValueTask<Result<T>> result,
            Func<T, ValueTask<Result>> collectionSelector,
            Func<T, Result, Result> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue.Value);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() =>
            {
                var selected = resultSelector(resultValue.Value, collectionResult);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the successful next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or the
    ///     <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> result,
            Func<T, ValueTask<Result>> collectionSelector,
            Func<T, Result, Result<TNext>> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue.Value);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() =>
            {
                var selected = resultSelector(resultValue.Value, collectionResult);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the value of the next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>A successful result with the value of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result> result,
            Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<Result, TMiddle, TNext> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(middle => resultSelector(resultValue, middle));
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the value of the next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>A failed result with the error of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or the
    ///     <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<TMiddle>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            ValueTask<Result> result,
            Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<Result, TMiddle, Error> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(middle =>
            {
                var selected = resultSelector(resultValue, middle);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the value of the next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or the
    ///     <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<TMiddle>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            ValueTask<Result> result,
            Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<Result, TMiddle, Result> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(middle =>
            {
                var selected = resultSelector(resultValue, middle);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the value of the next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or the
    ///     <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result> result,
            Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<Result, TMiddle, Result<TNext>> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(middle =>
            {
                var selected = resultSelector(resultValue, middle);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the successful next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>A successful result with the value of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result> result,
            Func<Result, ValueTask<Result>> collectionSelector,
            Func<Result, Result, TNext> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() => resultSelector(resultValue, collectionResult));
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the successful next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>A failed result with the error of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or the
    ///     <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            ValueTask<Result> result,
            Func<Result, ValueTask<Result>> collectionSelector,
            Func<Result, Result, Error> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() =>
            {
                var selected = resultSelector(resultValue, collectionResult);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the successful next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or the
    ///     <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            ValueTask<Result> result,
            Func<Result, ValueTask<Result>> collectionSelector,
            Func<Result, Result, Result> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() =>
            {
                var selected = resultSelector(resultValue, collectionResult);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the successful next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or the
    ///     <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result> result,
            Func<Result, ValueTask<Result>> collectionSelector,
            Func<Result, Result, Result<TNext>> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue.IsError)
                return resultValue.Error;

            var collectionTask = collectionSelector(resultValue);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() =>
            {
                var selected = resultSelector(resultValue, collectionResult);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the value of the next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this ValueTask<Result<T>> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            ValueTask<Result<T>> result,
            Func<T, Result<TMiddle>> collectionSelector,
            Func<T, TMiddle, Result> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the value of the next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> result,
            Func<T, Result<TMiddle>> collectionSelector,
            Func<T, TMiddle, Result<TNext>> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the successful next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>A successful result with the value of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> result,
            Func<T, Result> collectionSelector,
            Func<T, Result, TNext> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the successful next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>A failed result with the error of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<T>(
        this ValueTask<Result<T>> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(ValueTask<Result<T>> result, Func<T, Result> collectionSelector, Func<T, Result, Error> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the successful next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<T>(
        this ValueTask<Result<T>> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(ValueTask<Result<T>> result, Func<T, Result> collectionSelector, Func<T, Result, Result> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the value and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the value and the successful next result, and returns the projection. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> result,
            Func<T, Result> collectionSelector,
            Func<T, Result, Result<TNext>> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the value of the next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>A successful result with the value of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this ValueTask<Result> result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result> result,
            Func<Result, Result<TMiddle>> collectionSelector,
            Func<Result, TMiddle, TNext> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the value of the next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>A failed result with the error of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<TMiddle>(
        this ValueTask<Result> result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            ValueTask<Result> result,
            Func<Result, Result<TMiddle>> collectionSelector,
            Func<Result, TMiddle, Error> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the value of the next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<TMiddle>(
        this ValueTask<Result> result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            ValueTask<Result> result,
            Func<Result, Result<TMiddle>> collectionSelector,
            Func<Result, TMiddle, Result> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the value of the next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this ValueTask<Result> result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result> result,
            Func<Result, Result<TMiddle>> collectionSelector,
            Func<Result, TMiddle, Result<TNext>> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the successful next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>A successful result with the value of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this ValueTask<Result> result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result> result,
            Func<Result, Result> collectionSelector,
            Func<Result, Result, TNext> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the successful next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>A failed result with the error of the result selector, or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany(
        this ValueTask<Result> result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            ValueTask<Result> result,
            Func<Result, Result> collectionSelector,
            Func<Result, Result, Error> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the successful next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany(
        this ValueTask<Result> result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            ValueTask<Result> result,
            Func<Result, Result> collectionSelector,
            Func<Result, Result, Result> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
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
    /// <param name="result">The asynchronous operation that returns the source result.</param>
    /// <param name="collectionSelector">
    ///     The function that receives the successful source result and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="resultSelector">
    ///     The function that receives the successful source result and the successful next result, and returns the projection. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <returns>The result that the result selector returns (not nested), or the first failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="collectionSelector" /> or the <paramref name="resultSelector" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />, or the
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />, or the <paramref name="resultSelector" /> returns
    ///     <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this ValueTask<Result> result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result> result,
            Func<Result, Result> collectionSelector,
            Func<Result, Result, Result<TNext>> resultSelector)
        {
            var resultValue = await Guard.EnsureNotNullAwaited(result, nameof(result), ResultConfiguration.ContinueOnCapturedContext);
            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
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
    ///     The function that receives the value and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
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
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or
    ///     the <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this Result<T> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            Result<T> result,
            Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<T, TMiddle, Result> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result.Value);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(middle =>
            {
                var selected = resultSelector(result.Value, middle);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    ///     The function that receives the value and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
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
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or
    ///     the <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            Result<T> result,
            Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<T, TMiddle, Result<TNext>> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result.Value);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(middle =>
            {
                var selected = resultSelector(result.Value, middle);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    ///     The function that receives the value and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
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
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            Result<T> result,
            Func<T, ValueTask<Result>> collectionSelector,
            Func<T, Result, TNext> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result.Value);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() => resultSelector(result.Value, collectionResult));
        }
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
    ///     The function that receives the value and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
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
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or
    ///     the <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<T>(
        this Result<T> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(Result<T> result, Func<T, ValueTask<Result>> collectionSelector, Func<T, Result, Error> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result.Value);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() =>
            {
                var selected = resultSelector(result.Value, collectionResult);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    ///     The function that receives the value and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
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
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or
    ///     the <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<T>(
        this Result<T> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(Result<T> result, Func<T, ValueTask<Result>> collectionSelector, Func<T, Result, Result> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result.Value);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() =>
            {
                var selected = resultSelector(result.Value, collectionResult);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    ///     The function that receives the value and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
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
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or
    ///     the <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            Result<T> result,
            Func<T, ValueTask<Result>> collectionSelector,
            Func<T, Result, Result<TNext>> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result.Value);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() =>
            {
                var selected = resultSelector(result.Value, collectionResult);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
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
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this Result result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            Result result,
            Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<Result, TMiddle, TNext> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(middle => resultSelector(result, middle));
        }
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
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
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
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or
    ///     the <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<TMiddle>(
        this Result result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            Result result,
            Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<Result, TMiddle, Error> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(middle =>
            {
                var selected = resultSelector(result, middle);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
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
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or
    ///     the <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany<TMiddle>(
        this Result result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            Result result,
            Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<Result, TMiddle, Result> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(middle =>
            {
                var selected = resultSelector(result, middle);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
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
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or
    ///     the <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this Result result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            Result result,
            Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<Result, TMiddle, Result<TNext>> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(middle =>
            {
                var selected = resultSelector(result, middle);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
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
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this Result result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            Result result,
            Func<Result, ValueTask<Result>> collectionSelector,
            Func<Result, Result, TNext> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() => resultSelector(result, collectionResult));
        }
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
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
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
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or
    ///     the <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany(
        this Result result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            Result result,
            Func<Result, ValueTask<Result>> collectionSelector,
            Func<Result, Result, Error> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() =>
            {
                var selected = resultSelector(result, collectionResult);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
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
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or
    ///     the <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> SelectMany(
        this Result result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, Result> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            Result result,
            Func<Result, ValueTask<Result>> collectionSelector,
            Func<Result, Result, Result> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() =>
            {
                var selected = resultSelector(result, collectionResult);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    ///     The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be
    ///     <see langword="null" />.
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
    ///     If the <paramref name="collectionSelector" /> returns <see langword="null" /> or a task with a <see langword="null" /> result, or
    ///     the <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this Result result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, Result<TNext>> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            Result result,
            Func<Result, ValueTask<Result>> collectionSelector,
            Func<Result, Result, Result<TNext>> resultSelector)
        {
            if (result.IsError)
                return result.Error;

            var collectionTask = collectionSelector(result);
            var collectionResult = await Guard.EnsureNotNullResult(
                collectionTask, nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);

            return collectionResult.Then(() =>
            {
                var selected = resultSelector(result, collectionResult);
                return Guard.EnsureNotNullReturned(selected, nameof(resultSelector));
            });
        }
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
    public static ValueTask<Result> Select(this ValueTask<Result> result, Func<Result, Task> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> Select<TNext>(this ValueTask<Result> result, Func<Result, Task<TNext>> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> Select(this ValueTask<Result> result, Func<Result, ValueTask> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> Select<TNext>(this ValueTask<Result> result, Func<Result, ValueTask<TNext>> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany<T>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<T>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany<TMiddle>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<TMiddle>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany<T>(
        this ValueTask<Result<T>> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<T>(
        this ValueTask<Result<T>> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany<TMiddle>(
        this ValueTask<Result> result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this ValueTask<Result> result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<TMiddle>(
        this ValueTask<Result> result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this ValueTask<Result> result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany(
        this ValueTask<Result> result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this ValueTask<Result> result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany(
        this ValueTask<Result> result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this ValueTask<Result> result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany<T>(
        this Result<T> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<T>(
        this Result<T> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany<TMiddle>(
        this Result result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this Result result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<TMiddle>(
        this Result result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this Result result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany(
        this Result result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this Result result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany(
        this Result result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this Result result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }
    #endregion
}
