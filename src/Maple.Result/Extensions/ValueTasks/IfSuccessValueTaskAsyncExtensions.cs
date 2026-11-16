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
using System.Threading.Tasks;

namespace Maple.Results.Extensions.ValueTasks;

/// <summary>
///     The collection of extension methods for executing asynchronous actions or functions, represented by <see cref="ValueTask" />,
///     conditionally based on the success state of a <see cref="Result" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of operations that should only be performed if a preceding <see cref="Result" /> indicates success.
///     They help simplify error handling and reduce boilerplate code when working with <see cref="Result" />-based workflows.
///     <para>
///         These methods are declared in the <c>Maple.Results.Extensions.ValueTasks</c> namespace,
///         separately from their <see cref="System.Threading.Tasks.Task" />-based counterparts in <c>Maple.Results.Extensions</c>,
///         because both sets declare the same method names and an asynchronous lambda is convertible to a <c>Func&lt;Task&gt;
///         </c> as well as to a <c>Func&lt;ValueTask&gt;</c> delegate. Importing only one of these namespaces keeps such calls unambiguous.
///     </para>
/// </remarks>
public static class IfSuccessValueTaskAsyncExtensions
{
    #region Result extensions

    /// <returns>
    ///     A <see cref="ValueTask{Result}" /> that represents the original <paramref name="result" />, allowing for method chaining.
    /// </returns>
    /// <inheritdoc cref="IfSuccessTaskAsyncExtensions.IfSuccessAsync(Result, Func{Task}, bool?)" />
    public static ValueTask<Result> IfSuccessAsync(this Result result, Func<ValueTask> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        return Core(result, ifSuccessAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(Result result, Func<ValueTask> ifSuccessAction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
                await ifSuccessAction().ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    #endregion

    #region Result<T> extensions

    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the original <paramref name="result" />, allowing for method chaining.
    /// </returns>
    /// <inheritdoc cref="IfSuccessTaskAsyncExtensions.IfSuccessAsync{T}(Result{T}, Func{T, Task}, bool?)" />
    public static ValueTask<Result<T>> IfSuccessAsync<T>(this Result<T> result, Func<T, ValueTask> ifSuccessAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        return Core(result, ifSuccessAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(Result<T> result, Func<T, ValueTask> ifSuccessAction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
                await ifSuccessAction(result.Value).ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    #endregion

    #region ValueTask<Result> extensions

    /// <summary>
    ///     Invokes the specified asynchronous action as an asynchronous operation,
    ///     if the asynchronous <paramref name="resultTask" /> operation represents a successful outcome.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method enables fluent handling of asynchronous operations that return a <see cref="Result" /> by executing the provided asynchronous action
    ///         only when the <paramref name="resultTask" /> indicates a successful asynchronous operation.
    ///         The action is not invoked if the outcome of the <paramref name="resultTask" /> is not successful.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    ///     </para>
    /// </remarks>
    /// <param name="resultTask">The asynchronous operation to be executed and which result is to be inspected.</param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> is a successful asynchronous operation. Cannot be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the outcome of the original <paramref name="resultTask" /> asynchronous operation,
    ///     allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="ifSuccessAction" /> parameter is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> IfSuccessAsync(this ValueTask<Result> resultTask, Func<ValueTask> ifSuccessAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        return Core(resultTask, ifSuccessAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(ValueTask<Result> resultTask, Func<ValueTask> ifSuccessAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
                await ifSuccessAction().ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    #endregion

    #region ValueTask<Result<T>> extensions

    /// <summary>
    ///     Invokes the specified asynchronous action as an asynchronous operation,
    ///     if the asynchronous <paramref name="resultTask" /> operation represents a successful outcome.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method enables fluent handling of asynchronous operations that return a <see cref="Result{T}" /> by executing
    ///         the provided asynchronous action only when the <paramref name="resultTask" /> indicates a successful asynchronous operation.
    ///         The action is not invoked if the outcome of the <paramref name="resultTask" /> is not successful.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed function. If successful, this is also the type
    ///     of the parameter passed to that function. Also, the type of the <see cref="Result{T}" /> value to return if the operation is successful.
    /// </typeparam>
    /// <param name="resultTask">The asynchronous operation to be executed and which result is to be inspected.</param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> is a successful asynchronous operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the outcome of the original <paramref name="resultTask" /> asynchronous operation,
    ///     allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="ifSuccessAction" /> parameter is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<T>> IfSuccessAsync<T>(this ValueTask<Result<T>> resultTask,
        Func<T, ValueTask> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        return Core(resultTask, ifSuccessAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(ValueTask<Result<T>> resultTask, Func<T, ValueTask> ifSuccessAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
                await ifSuccessAction(result.Value).ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    #endregion
}
