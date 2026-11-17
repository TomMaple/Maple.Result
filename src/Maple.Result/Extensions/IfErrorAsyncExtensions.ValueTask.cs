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

namespace Maple.Results.Extensions;

// The ValueTask-based methods of IfErrorAsyncExtensions (#248); the class is documented in IfErrorAsyncExtensions.Task.cs.
public static partial class IfErrorAsyncExtensions
{
    /// <returns>
    ///     A <see cref="ValueTask{IOperationResult}" /> that represents the original <paramref name="result" /> instance,
    ///     regardless of whether the action was invoked.
    /// </returns>
    /// <inheritdoc cref="IfErrorAsyncExtensions.IfErrorAsync{TResult}(TResult, Func{Error, Task}, bool?)" />
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

    #region Not supported: functions returning a value

    // A function that returns a value is a transformation, not a side effect (#197): these overloads turn the call into a compile
    // error (CS0619) that names the method to use, instead of discarding the value. A statement lambda (e.g., x => { list.Add(x); })
    // or a method that returns nothing is a side effect and binds to the action overload.

    /// <summary>
    ///     Not supported: the function returns a value in a task (an assignment or an increment in an async lambda has one too), which
    ///     <c>IfErrorAsync</c> discards; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static ValueTask<TResult> IfErrorAsync<TResult, TValue>([DisallowNull] this TResult result, Func<Error, ValueTask<TValue>> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a value in a task (an assignment or an increment in an async lambda has one too), which
    ///     <c>IfErrorAsync</c> discards; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static ValueTask<TResult> IfErrorAsync<TResult, TValue>(this ValueTask<TResult> resultTask, Func<Error, ValueTask<TValue>> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a value in a task, which <c>IfErrorAsync</c> discards; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<TResult> IfErrorAsync<TResult, TTaskResult>(this ValueTask<TResult> resultTask,
        Func<Error, Task<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }
    #endregion

    #region ValueTask results with synchronous delegates

    // See issue #214.

    /// <summary>
    ///     Runs the synchronous action if the result that the <paramref name="resultTask" /> returns is failed, and returns
    ///     the result unchanged.
    /// </summary>
    /// <remarks>
    ///     The action is synchronous: it runs after the <paramref name="resultTask" /> completes, and it is not called for a
    ///     successful result. The result is returned unchanged.
    /// </remarks>
    /// <typeparam name="TResult">The type of the result: <see cref="Result" /> or <see cref="Result{T}" />.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect.</param>
    /// <param name="ifErrorAction">The synchronous action that receives the error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run it on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result that the <paramref name="resultTask" /> returns.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="ifErrorAction" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<TResult> IfErrorAsync<TResult>(this ValueTask<TResult> resultTask,
        Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<TResult> Core(ValueTask<TResult> resultTask, Action<Error> ifErrorAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.IfError(ifErrorAction);
        }
    }
    #endregion

    #region ValueTask results with functions returning a Task

    // See issue #226.

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
    ///     <para>
    ///         The function returns a <see cref="Task" />, the other kind of task: it is awaited, and a <see cref="ValueTask" />
    ///         is returned, as for a function that returns a <see cref="ValueTask" />.
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
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static ValueTask<TResult> IfErrorAsync<TResult>(
        this ValueTask<TResult> resultTask,
        Func<Error, Task> ifErrorAction,
        bool? continueOnCapturedContext = null
    )
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<TResult> Core(ValueTask<TResult> resultTask, Func<Error, Task> ifErrorAction, bool continueOnCapturedContext)
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
    #endregion

    #region Not supported: synchronous functions returning a value

    // A synchronous function that returns a value, passed to IfSuccessAsync or IfErrorAsync, is a compile error (CS0619) that names
    // the method to use, as in #197.

    /// <summary>
    ///     Not supported: the function returns a value (an assignment or an increment has one too), which <c>IfErrorAsync</c> discards;
    ///     use <c>ElseAsync</c>, or a statement lambda.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SyncValueDiscardedMessage, error: true)]
    public static ValueTask<TResult> IfErrorAsync<TResult, TValue>(this ValueTask<TResult> resultTask,
        Func<Error, TValue> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(SyncValueDiscardedMessage);
    }
    #endregion
}
