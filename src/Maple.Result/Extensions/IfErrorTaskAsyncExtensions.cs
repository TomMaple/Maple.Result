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
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Maple.Results.Extensions;

/// <summary>
///     The collection of extension methods for executing asynchronous actions or functions conditionally based on the error state of
///     a <see cref="IOperationResult" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent handling of error scenarios by allowing to specify actions or functions that should be executed only when the result
///     indicates an error.
/// </remarks>
public static class IfErrorTaskAsyncExtensions
{
    /// <summary>
    ///     Invokes the specified asynchronous action if the <paramref name="result" /> represents an error,
    ///     and returns the original <paramref name="result" />.
    /// </summary>
    /// <remarks>
    ///     Use this method to perform side effects, such as logging or error handling, when a <paramref name="result" /> indicates an error, without altering
    ///     the <paramref name="result" /> itself.
    ///     The method does not modify the <paramref name="result" /> or handle the <see cref="Error" /> beyond invoking the specified asynchronous action.
    /// </remarks>
    /// <typeparam name="TResult">The type of the result object, which must implement the <see cref="IOperationResult" /> interface.</typeparam>
    /// <param name="result">The <paramref name="result" /> to inspect for an error. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action to execute if the <paramref name="result" /> is an error.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this asynchronous action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{IOperationResult}" /> that represents the original <paramref name="result" /> instance,
    ///     regardless of whether the action was invoked.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.</exception>
    public static Task<TResult> IfErrorAsync<TResult>(
        [DisallowNull] this TResult result,
        Func<Error, Task> ifErrorAction,
        bool? continueOnCapturedContext = null
    )
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TResult> Core(TResult result, Func<Error, Task> ifErrorAction, bool continueOnCapturedContext)
        {
            if (result.IsError)
            {
                var errorTask = ifErrorAction(result.Error);
                await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);
            }

            return result;
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous action if the <paramref name="resultTask" /> represents an error operation,
    ///     and returns its <typeparamref name="TResult" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use this method to perform side effects, such as logging or error handling,
    ///         when a <paramref name="resultTask" /> indicates an asynchronous error operation, without altering the operation result.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TResult">
    ///     The type of the operation result object, which must implement the <see cref="IOperationResult" /> interface.
    /// </typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> returns an error. The <see cref="Error" /> associated with
    ///     the <paramref name="resultTask" /> result is passed to this asynchronous action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{IOperationResult}" /> that represents the outcome of the <paramref name="resultTask" /> asynchronous operation,
    ///     regardless of whether the action was invoked.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    public static Task<TResult> IfErrorAsync<TResult>(
        this Task<TResult> resultTask,
        Func<Error, Task> ifErrorAction,
        bool? continueOnCapturedContext = null
    )
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TResult> Core(Task<TResult> resultTask, Func<Error, Task> ifErrorAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsError)
            {
                var errorTask = ifErrorAction(result.Error);
                await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);
            }

            return result;
        }
    }
}
