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

/// <summary>
///     The collection of extension methods that run a side effect when an <see cref="IOperationResult" /> is failed: an asynchronous action (a function
///     that returns a <see cref="Task" />) receives
///     the <see cref="Error" />, and the result is returned unchanged.
/// </summary>
/// <remarks>
///     A successful result is returned unchanged, and the action is not called. To replace a failed result (e.g., recover with a
///     value, or return another error), use <c>ElseAsync</c>; a function that returns a value is a compile error here (CS0619), so that a failure
///     is not turned into a success by mistake.
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
    [OverloadResolutionPriority(1)]
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

    #region Not supported: functions returning a value

    // A function that returns a value is a transformation, not a side effect (#197): these overloads turn the call into a compile
    // error (CS0619) that names the method to use, instead of discarding the value. A statement lambda (e.g., x => { list.Add(x); })
    // or a method that returns nothing is a side effect and binds to the action overload.

    private const string ValueDiscardedMessage =
        "The function returns a value in a task, which IfErrorAsync discards; an assignment or an increment in an async lambda " +
        "(e.g., async e => last = await LoadAsync(e)) has a value too. " +
        "Use ElseAsync to replace the result with the value, or an async statement lambda (e.g., async e => { await LogAsync(e); }) for a side effect.";

    /// <summary>
    ///     Not supported: the function returns a value in a task (an assignment or an increment in an async lambda has one too), which
    ///     <c>IfErrorAsync</c> discards; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static Task<TResult> IfErrorAsync<TResult, TValue>([DisallowNull] this TResult result, Func<Error, Task<TValue>> ifErrorAction, bool? continueOnCapturedContext = null)
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
    [OverloadResolutionPriority(1)]
    public static Task<TResult> IfErrorAsync<TResult, TValue>(this Task<TResult> resultTask, Func<Error, Task<TValue>> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    #endregion

    #region Task results with synchronous delegates

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
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorAction">The synchronous action that receives the error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run it on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result that the <paramref name="resultTask" /> returns.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" /> or the <paramref name="ifErrorAction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task<TResult> IfErrorAsync<TResult>(this Task<TResult> resultTask,
        Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TResult> Core(Task<TResult> resultTask, Action<Error> ifErrorAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.IfError(ifErrorAction);
        }
    }

    #endregion

    #region Not supported: synchronous delegates of the wrong kind, and functions returning a ValueTask

    // A synchronous action passed to ThenAsync or ElseAsync, or a synchronous function that returns a value passed to
    // IfSuccessAsync or IfErrorAsync, is a compile error (CS0619) that names the method to use, as in #197.
    // A function that returns a ValueTask would bind to the overloads with a synchronous delegate and give a result holding a task
    // that nobody awaits (#214): these overloads turn the call into a compile error (CS0619) that names the namespace to import.
    // The overloads whose delegate returns a Task have priority 1 (#214): an async lambda converts to a Task and to a ValueTask,
    // so without it, a call with an async lambda would be ambiguous (CS0121) between them and the traps for the other kind of task.

    private const string SyncValueDiscardedMessage =
        "The function returns a value, which IfErrorAsync discards; an assignment or an increment (e.g., e => last = e or e => count++) " +
        "has a value too. " +
        "Use ElseAsync to replace the result with the value, or a statement lambda (e.g., e => { titles.Add(e.Title); } or e => { count++; }) " +
        "for a side effect.";

    private const string OtherTaskMessage =
        "The function returns a ValueTask, which the Task-based IfErrorAsync does not await. Import " +
        "Maple.Results.Extensions.ValueTasks instead of Maple.Results.Extensions, or convert it with AsTask().";

    /// <summary>
    ///     Not supported: the function returns a value (an assignment or an increment has one too), which <c>IfErrorAsync</c> discards;
    ///     use <c>ElseAsync</c>, or a statement lambda.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SyncValueDiscardedMessage, error: true)]
    public static Task<TResult> IfErrorAsync<TResult, TValue>(this Task<TResult> resultTask,
        Func<Error, TValue> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(SyncValueDiscardedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<TResult> IfErrorAsync<TResult>(this Task<TResult> resultTask,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<TResult> IfErrorAsync<TResult, TTaskResult>(this Task<TResult> resultTask,
        Func<Error, ValueTask<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    #endregion
}
