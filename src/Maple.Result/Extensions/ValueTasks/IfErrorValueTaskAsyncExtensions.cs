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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Results.Extensions.ValueTasks;

/// <summary>
///     The collection of extension methods that run a side effect when an <see cref="IOperationResult" /> is failed: an asynchronous action (a function
///     that returns a <see cref="ValueTask" />) receives
///     the <see cref="Error" />, and the result is returned unchanged.
/// </summary>
/// <remarks>
///     A successful result is returned unchanged, and the action is not called. To replace a failed result (e.g., recover with a
///     value, or return another error), use <c>ElseAsync</c>; a function that returns a value is a compile error here (CS0619), so that a failure
///     is not turned into a success by mistake.
///     <para>
///         These methods are declared in the <c>Maple.Results.Extensions.ValueTasks</c> namespace, separately from their
///         <see cref="System.Threading.Tasks.Task" />-based counterparts in <c>Maple.Results.Extensions</c>, because both sets declare the same method
///     names and
///         an asynchronous lambda is convertible to a <c>Func&lt;Task&gt;</c> as well as to a <c>Func&lt;ValueTask&gt;</c> delegate.
///         Importing only one of these namespaces keeps such calls unambiguous.
///     </para>
/// </remarks>
public static class IfErrorValueTaskAsyncExtensions
{
    /// <returns>
    ///     A <see cref="ValueTask{IOperationResult}" /> that represents the original <paramref name="result" /> instance,
    ///     regardless of whether the action was invoked.
    /// </returns>
    /// <inheritdoc cref="IfErrorTaskAsyncExtensions.IfErrorAsync{TResult}(TResult, Func{Error, Task}, bool?)" />
    public static ValueTask<TResult> IfErrorAsync<TResult>(
        [DisallowNull] this TResult result,
        Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null
    )
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<TResult> Core(TResult result, Func<Error, ValueTask> ifErrorAction, bool continueOnCapturedContext)
        {
            if (result.IsError)
                await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

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
    /// <param name="resultTask">The asynchronous operation to be executed and which result is to be inspected.</param>
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
    ///     A <see cref="ValueTask{IOperationResult}" /> that represents the outcome of the <paramref name="resultTask" /> asynchronous operation,
    ///     regardless of whether the action was invoked.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="ifErrorAction" /> parameter is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<TResult> IfErrorAsync<TResult>(
        this ValueTask<TResult> resultTask,
        Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null
    )
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<TResult> Core(ValueTask<TResult> resultTask, Func<Error, ValueTask> ifErrorAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsError)
                await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    #region Not supported: functions returning a value (#197)

    // A function that returns a value is a transformation, not a side effect (#197): these overloads turn the call into a compile
    // error (CS0619) that names the method to use, instead of discarding the value. A statement lambda (e.g., x => { list.Add(x); })
    // or a method that returns nothing is a side effect and binds to the action overload.

    private const string ValueDiscardedMessage =
        "The function returns a value in a task, which IfErrorAsync discards. Use ElseAsync to replace the result with " +
        "it, or an async statement lambda for a side effect.";

    /// <summary>
    ///     Not supported: the function returns a value in a task, which <c>IfErrorAsync</c> discards; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static ValueTask<TResult> IfErrorAsync<TResult, TValue>([DisallowNull] this TResult result, Func<Error, ValueTask<TValue>> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a value in a task, which <c>IfErrorAsync</c> discards; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static ValueTask<TResult> IfErrorAsync<TResult, TValue>(this ValueTask<TResult> resultTask, Func<Error, ValueTask<TValue>> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    #endregion
}
