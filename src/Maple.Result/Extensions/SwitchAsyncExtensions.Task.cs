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

namespace Maple.Results.Extensions;

/// <summary>
///     The collection of extension methods that await a <see cref="Task{TResult}" /> of a <see cref="Result" /> or <see cref="Result{T}" />
///     and run one of two actions depending on whether the result is successful.
/// </summary>
/// <remarks>
///     <c>SwitchAsync</c> ends a chain for side effects, as <c>MatchAsync</c> ends it for a value; the returned task completes when the action
///     has run. The actions are synchronous: they run after the task completes. A function that returns a task is not awaited by
///     <c>SwitchAsync</c>, and the analyzer of the package reports it (MR0002); use <c>MatchAsync</c> with two functions that return a task.
///     <para>
///         The methods for a <see cref="System.Threading.Tasks.Task" /> and for a <see cref="System.Threading.Tasks.ValueTask" /> of a result
///         are in this one class; the kind of the task is kept.
///     </para>
/// </remarks>
public static partial class SwitchAsyncExtensions
{
    #region Task<Result>

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and invokes one of the provided actions depending on whether the result is successful.
    /// </summary>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The action invoked if the result is successful. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action invoked if the result is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed actions, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The task that completes when the invoked action has run.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task SwitchAsync(this Task<Result> resultTask, Action ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task Core(Task<Result> resultTask, Action ifSuccessAction, Action<Error> ifErrorAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            result.Switch(ifSuccessAction, ifErrorAction);
        }
    }

    #endregion

    #region Task<Result<T>>

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and invokes one of the provided actions depending on whether the result is successful.
    /// </summary>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The action invoked if the result is successful, with its value. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action invoked if the result is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed actions, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The task that completes when the invoked action has run.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task SwitchAsync<T>(this Task<Result<T>> resultTask, Action<T> ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task Core(Task<Result<T>> resultTask, Action<T> ifSuccessAction, Action<Error> ifErrorAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            result.Switch(ifSuccessAction, ifErrorAction);
        }
    }

    #endregion
}
