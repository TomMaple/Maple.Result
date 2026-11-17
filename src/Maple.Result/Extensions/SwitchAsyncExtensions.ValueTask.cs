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

    #region Not supported: actions returning a task

    // A lambda that returns a task converts to an action, and the task would never be awaited (an async lambda becomes async void). These
    // overloads turn such a call into a compile error (CS0619) that names MatchAsync; they follow the traps of the synchronous Switch (see
    // SwitchExtensions), with the continueOnCapturedContext parameter before the unused one.
    // See issue #255.

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask SwitchAsync<TTask>(this ValueTask<Result> resultTask, Func<TTask> ifSuccessAction, Action<Error> ifErrorAction,
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
    public static ValueTask SwitchAsync<TIgnored>(this ValueTask<Result> resultTask, Func<ValueTask<TIgnored>> ifSuccessAction, Action<Error> ifErrorAction,
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
    public static ValueTask SwitchAsync<TValueTask>(this ValueTask<Result> resultTask, Func<TValueTask> ifSuccessAction, Action<Error> ifErrorAction,
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
    public static ValueTask SwitchAsync<TTask>(this ValueTask<Result> resultTask, Action ifSuccessAction, Func<Error, TTask> ifErrorAction,
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
    public static ValueTask SwitchAsync<TIgnored>(this ValueTask<Result> resultTask, Action ifSuccessAction, Func<Error, ValueTask<TIgnored>> ifErrorAction,
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
    public static ValueTask SwitchAsync<TValueTask>(this ValueTask<Result> resultTask, Action ifSuccessAction, Func<Error, TValueTask> ifErrorAction,
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
    public static ValueTask SwitchAsync<TTask1, TTask2>(this ValueTask<Result> resultTask, Func<TTask1> ifSuccessAction, Func<Error, TTask2> ifErrorAction,
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
    public static ValueTask SwitchAsync<T, TTask>(this ValueTask<Result<T>> resultTask, Func<T, TTask> ifSuccessAction, Action<Error> ifErrorAction,
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
    public static ValueTask SwitchAsync<T, TIgnored>(this ValueTask<Result<T>> resultTask, Func<T, ValueTask<TIgnored>> ifSuccessAction,
        Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask SwitchAsync<T, TValueTask>(this ValueTask<Result<T>> resultTask, Func<T, TValueTask> ifSuccessAction, Action<Error> ifErrorAction,
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
    public static ValueTask SwitchAsync<T, TTask>(this ValueTask<Result<T>> resultTask, Action<T> ifSuccessAction, Func<Error, TTask> ifErrorAction,
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
    public static ValueTask SwitchAsync<T, TIgnored>(this ValueTask<Result<T>> resultTask, Action<T> ifSuccessAction,
        Func<Error, ValueTask<TIgnored>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>SwitchAsync</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask SwitchAsync<T, TValueTask>(this ValueTask<Result<T>> resultTask, Action<T> ifSuccessAction, Func<Error, TValueTask> ifErrorAction,
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
    public static ValueTask SwitchAsync<T, TTask1, TTask2>(this ValueTask<Result<T>> resultTask, Func<T, TTask1> ifSuccessAction,
        Func<Error, TTask2> ifErrorAction,
        bool? continueOnCapturedContext = null, Task? unused = null)
        where TTask1 : Task
        where TTask2 : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }
    #endregion
}
