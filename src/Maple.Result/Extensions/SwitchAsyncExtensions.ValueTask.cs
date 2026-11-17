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

// The ValueTask-based methods of SwitchAsyncExtensions (#248); the class is documented in SwitchAsyncExtensions.Task.cs.
public static partial class SwitchAsyncExtensions
{
    #region ValueTask<Result>

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and invokes one of the provided actions depending on whether the result is successful.
    /// </summary>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect.</param>
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
    ///     If any of the <paramref name="ifSuccessAction" /> or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask SwitchAsync(this ValueTask<Result> resultTask, Action ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask Core(ValueTask<Result> resultTask, Action ifSuccessAction, Action<Error> ifErrorAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            result.Switch(ifSuccessAction, ifErrorAction);
        }
    }

    #endregion

    #region ValueTask<Result<T>>

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and invokes one of the provided actions depending on whether the result is successful.
    /// </summary>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect.</param>
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
    ///     If any of the <paramref name="ifSuccessAction" /> or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask SwitchAsync<T>(this ValueTask<Result<T>> resultTask, Action<T> ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask Core(ValueTask<Result<T>> resultTask, Action<T> ifSuccessAction, Action<Error> ifErrorAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            result.Switch(ifSuccessAction, ifErrorAction);
        }
    }

    #endregion
}
