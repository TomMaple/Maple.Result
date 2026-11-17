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
///     The collection of extension methods that run a side effect when a <see cref="Result" /> or <see cref="Result{T}" /> is successful: an asynchronous
///     action (a function that returns a <see cref="Task" />) receives the value (if
///     any), and the result is returned unchanged.
/// </summary>
/// <remarks>
///     A failed result is returned unchanged, and the action is not called. To transform or chain the result with a function that
///     returns a value, a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" />, use <c>ThenAsync</c>; a function that returns
///     a value is a compile error here (CS0619), so that the value is not discarded by mistake.
/// </remarks>
public static class IfSuccessTaskAsyncExtensions
{
    #region Result extensions

    /// <summary>
    ///     Invokes the specified asynchronous action as an asynchronous operation, if the <paramref name="result" /> represents a successful outcome.
    /// </summary>
    /// <remarks>
    ///     This method enables fluent handling of successful <see cref="Result" />s by executing the provided asynchronous action only when
    ///     the <paramref name="result" /> indicates success. The action is not invoked if the <paramref name="result" /> is not successful.
    /// </remarks>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="result" /> is successful. Cannot be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that represents the original <paramref name="result" />, allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessAction" /> returns a <see langword="null" /> task.</exception>
    public static Task<Result> IfSuccessAsync(this Result result, Func<Task> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        return Core(result, ifSuccessAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Result result, Func<Task> ifSuccessAction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessAction();
                await Guard.EnsureNotNullTask(successTask, nameof(ifSuccessAction)).ConfigureAwait(continueOnCapturedContext);
            }

            return result;
        }
    }

    #endregion

    #region Result<T> extensions

    /// <summary>
    ///     Invokes the specified asynchronous action if the <paramref name="result" /> represents a successful outcome.
    /// </summary>
    /// <remarks>
    ///     This method enables fluent handling of successful <see cref="Result{T}" />s by executing the provided asynchronous action only when
    ///     the <paramref name="result" /> indicates success. The action is not invoked if the <paramref name="result" /> is not successful.
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed function. If successful, this is also the type of
    ///     the parameter passed to that function. Also, the type of the <see cref="Result{T}" /> value to return if the operation is successful.
    /// </typeparam>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="result" /> is successful.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the original <paramref name="result" />, allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessAction" /> returns a <see langword="null" /> task.</exception>
    public static Task<Result<T>> IfSuccessAsync<T>(this Result<T> result, Func<T, Task> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        return Core(result, ifSuccessAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Result<T> result, Func<T, Task> ifSuccessAction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessAction(result.Value);
                await Guard.EnsureNotNullTask(successTask, nameof(ifSuccessAction)).ConfigureAwait(continueOnCapturedContext);
            }

            return result;
        }
    }

    #endregion

    #region Task<Result> extensions

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
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> is a successful asynchronous operation. Cannot be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the outcome of the original <paramref name="resultTask" /> asynchronous operation,
    ///     allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifSuccessAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessAction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> IfSuccessAsync(this Task<Result> resultTask, Func<Task> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        return Core(resultTask, ifSuccessAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result> resultTask, Func<Task> ifSuccessAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessAction();
                await Guard.EnsureNotNullTask(successTask, nameof(ifSuccessAction)).ConfigureAwait(continueOnCapturedContext);
            }

            return result;
        }
    }

    #endregion

    #region Task<Result<T>> extensions

    /// <summary>
    ///     Invokes the specified asynchronous action as an asynchronous operation,
    ///     if the asynchronous <paramref name="resultTask" /> operation represents a successful outcome.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method enables fluent handling of asynchronous operations that return a <see cref="Result{T}" /> by executing the provided asynchronous
    ///         action only when the <paramref name="resultTask" /> indicates a successful asynchronous operation.
    ///         The action is not invoked if the outcome of the <paramref name="resultTask" /> is not successful.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed function. If successful, this is also the type of
    ///     the parameter passed to that function. Also, the type of the <see cref="Result{T}" /> value to return if the operation is successful.
    /// </typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
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
    ///     A <see cref="Task{TResult}" /> that represents the outcome of the original <paramref name="resultTask" /> asynchronous operation,
    ///     allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifSuccessAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessAction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> IfSuccessAsync<T>(this Task<Result<T>> resultTask,
        Func<T, Task> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        return Core(resultTask, ifSuccessAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Task<Result<T>> resultTask, Func<T, Task> ifSuccessAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessAction(result.Value);
                await Guard.EnsureNotNullTask(successTask, nameof(ifSuccessAction)).ConfigureAwait(continueOnCapturedContext);
            }

            return result;
        }
    }

    #endregion

    #region Not supported: functions returning a value

    // A function that returns a value is a transformation, not a side effect (#197): these overloads turn the call into a compile
    // error (CS0619) that names the method to use, instead of discarding the value. A statement lambda (e.g., x => { list.Add(x); })
    // or a method that returns nothing is a side effect and binds to the action overload.

    private const string ValueDiscardedMessage =
        "The function returns a value in a task, which IfSuccessAsync discards; an assignment or an increment in an async lambda " +
        "(e.g., async x => last = await LoadAsync(x)) has a value too. " +
        "Use ThenAsync to keep the value, or an async statement lambda (e.g., async x => { await SaveAsync(x); }) for a side effect.";

    /// <summary>
    ///     Not supported: the function returns a value in a task (an assignment or an increment in an async lambda has one too), which
    ///     <c>IfSuccessAsync</c> discards; use <c>ThenAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static Task<Result> IfSuccessAsync<TValue>(this Result result, Func<Task<TValue>> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a value in a task (an assignment or an increment in an async lambda has one too), which
    ///     <c>IfSuccessAsync</c> discards; use <c>ThenAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static Task<Result<T>> IfSuccessAsync<T, TValue>(this Result<T> result, Func<T, Task<TValue>> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a value in a task (an assignment or an increment in an async lambda has one too), which
    ///     <c>IfSuccessAsync</c> discards; use <c>ThenAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result> IfSuccessAsync<TValue>(this Task<Result> resultTask, Func<Task<TValue>> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a value in a task (an assignment or an increment in an async lambda has one too), which
    ///     <c>IfSuccessAsync</c> discards; use <c>ThenAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> IfSuccessAsync<T, TValue>(this Task<Result<T>> resultTask, Func<T, Task<TValue>> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    #endregion

    #region Task results with synchronous delegates

    // See issue #214.

    /// <summary>
    ///     Runs the synchronous action if the result that the <paramref name="resultTask" /> returns is successful, and returns
    ///     the result unchanged.
    /// </summary>
    /// <remarks>
    ///     The action is synchronous: it runs after the <paramref name="resultTask" /> completes, and it is not called for a
    ///     failed result. The result is returned unchanged.
    /// </remarks>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">The synchronous action to run. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run it on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result that the <paramref name="resultTask" /> returns.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" /> or the <paramref name="ifSuccessAction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task<Result> IfSuccessAsync(this Task<Result> resultTask,
        Action ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        return Core(resultTask, ifSuccessAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result> resultTask, Action ifSuccessAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.IfSuccess(ifSuccessAction);
        }
    }

    /// <summary>
    ///     Runs the synchronous action on the value if the result that the <paramref name="resultTask" /> returns is successful,
    ///     and returns the result unchanged.
    /// </summary>
    /// <remarks>
    ///     The action is synchronous: it runs after the <paramref name="resultTask" /> completes, and it is not called for a
    ///     failed result. The result is returned unchanged.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">The synchronous action that receives the value. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run it on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result that the <paramref name="resultTask" /> returns.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" /> or the <paramref name="ifSuccessAction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task<Result<T>> IfSuccessAsync<T>(this Task<Result<T>> resultTask,
        Action<T> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        return Core(resultTask, ifSuccessAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Task<Result<T>> resultTask, Action<T> ifSuccessAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.IfSuccess(ifSuccessAction);
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
        "The function returns a value, which IfSuccessAsync discards; an assignment or an increment (e.g., x => last = x or x => count++) " +
        "has a value too. " +
        "Use ThenAsync to keep the value, or a statement lambda (e.g., x => { list.Add(x); } or x => { count++; }) for a side effect.";

    private const string OtherTaskMessage =
        "The function returns a ValueTask, which the Task-based IfSuccessAsync does not await. Import " +
        "Maple.Results.Extensions.ValueTasks instead of Maple.Results.Extensions, or convert it with AsTask().";

    /// <summary>
    ///     Not supported: the function returns a value (an assignment or an increment has one too), which <c>IfSuccessAsync</c> discards;
    ///     use <c>ThenAsync</c>, or a statement lambda.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SyncValueDiscardedMessage, error: true)]
    public static Task<Result> IfSuccessAsync<TValue>(this Task<Result> resultTask, Func<TValue> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SyncValueDiscardedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a value (an assignment or an increment has one too), which <c>IfSuccessAsync</c> discards;
    ///     use <c>ThenAsync</c>, or a statement lambda.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SyncValueDiscardedMessage, error: true)]
    public static Task<Result<T>> IfSuccessAsync<T, TValue>(this Task<Result<T>> resultTask,
        Func<T, TValue> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SyncValueDiscardedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result> IfSuccessAsync(this Task<Result> resultTask, Func<ValueTask> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result> IfSuccessAsync<TTaskResult>(this Task<Result> resultTask,
        Func<ValueTask<TTaskResult>> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result<T>> IfSuccessAsync<T>(this Task<Result<T>> resultTask, Func<T, ValueTask> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result<T>> IfSuccessAsync<T, TTaskResult>(this Task<Result<T>> resultTask,
        Func<T, ValueTask<TTaskResult>> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    #endregion
}
