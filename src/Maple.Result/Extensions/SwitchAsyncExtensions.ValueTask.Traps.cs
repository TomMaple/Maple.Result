// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Results.Extensions;

// The overload traps of SwitchAsyncExtensions.ValueTask.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public static partial class SwitchAsyncExtensions
{
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
