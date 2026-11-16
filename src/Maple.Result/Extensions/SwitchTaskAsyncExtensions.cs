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
///     The collection of extension methods that await a <see cref="Task{TResult}" /> of a <see cref="Result" /> or <see cref="Result{T}" />
///     and run one of two actions depending on whether the result is successful.
/// </summary>
/// <remarks>
///     <c>SwitchAsync</c> ends a chain for side effects, as <c>MatchAsync</c> ends it for a value; the returned task completes when the action
///     has run. The actions are synchronous: they run after the task completes. An action that returns a task is a compile error (CS0619),
///     because <c>SwitchAsync</c> would not await it; use <c>MatchAsync</c> with two functions that return a task.
/// </remarks>
public static class SwitchTaskAsyncExtensions
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

    #region Not supported: actions returning a task (#255)

    // A lambda that returns a task converts to an action, and the task would never be awaited (an async lambda becomes async void). These
    // overloads turn such a call into a compile error (CS0619) that names MatchAsync; they follow the traps of the synchronous Switch (see
    // SwitchExtensions), with the continueOnCapturedContext parameter before the unused one.

    private const string ActionReturningTaskMessage =
        "An action returns a task, which SwitchAsync does not await. Use MatchAsync with two functions that return a task.";

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task SwitchAsync<TTask>(this Task<Result> resultTask, Func<TTask> ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    public static Task SwitchAsync<TIgnored>(this Task<Result> resultTask, Func<ValueTask<TIgnored>> ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task SwitchAsync<TValueTask>(this Task<Result> resultTask, Func<TValueTask> ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null, ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task SwitchAsync<TTask>(this Task<Result> resultTask, Action ifSuccessAction, Func<Error, TTask> ifErrorAction,
        bool? continueOnCapturedContext = null, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    public static Task SwitchAsync<TIgnored>(this Task<Result> resultTask, Action ifSuccessAction, Func<Error, ValueTask<TIgnored>> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task SwitchAsync<TValueTask>(this Task<Result> resultTask, Action ifSuccessAction, Func<Error, TValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null, ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: both actions return a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task SwitchAsync<TTask1, TTask2>(this Task<Result> resultTask, Func<TTask1> ifSuccessAction, Func<Error, TTask2> ifErrorAction,
        bool? continueOnCapturedContext = null, Task? unused = null)
        where TTask1 : Task
        where TTask2 : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task SwitchAsync<T, TTask>(this Task<Result<T>> resultTask, Func<T, TTask> ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    public static Task SwitchAsync<T, TIgnored>(this Task<Result<T>> resultTask, Func<T, ValueTask<TIgnored>> ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task SwitchAsync<T, TValueTask>(this Task<Result<T>> resultTask, Func<T, TValueTask> ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null, ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task SwitchAsync<T, TTask>(this Task<Result<T>> resultTask, Action<T> ifSuccessAction, Func<Error, TTask> ifErrorAction,
        bool? continueOnCapturedContext = null, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    public static Task SwitchAsync<T, TIgnored>(this Task<Result<T>> resultTask, Action<T> ifSuccessAction, Func<Error, ValueTask<TIgnored>> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task SwitchAsync<T, TValueTask>(this Task<Result<T>> resultTask, Action<T> ifSuccessAction, Func<Error, TValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null, ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: both actions return a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task SwitchAsync<T, TTask1, TTask2>(this Task<Result<T>> resultTask, Func<T, TTask1> ifSuccessAction, Func<Error, TTask2> ifErrorAction,
        bool? continueOnCapturedContext = null, Task? unused = null)
        where TTask1 : Task
        where TTask2 : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    #endregion
}
