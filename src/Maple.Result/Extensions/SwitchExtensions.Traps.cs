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

// The overload traps of SwitchExtensions.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public static partial class SwitchExtensions
{
    #region Not supported: actions returning a task

    // A lambda that returns a task converts to an action, and the task would never be awaited (an async lambda becomes async void). These
    // overloads turn such a call into a compile error (CS0619) that names Match, in the success position, in the error position and in both.
    // They are generic and constrained to the task type (#251): a lambda that only throws has no return type to infer, so it does not
    // reach them, and a lambda that returns another value fails the constraint. The unused optional parameter only makes the signatures
    // differ. The overloads constrained to a Task or a ValueTask have priority 1: an async lambda converts to both a Task and a ValueTask,
    // and an action that returns a task in both positions would otherwise be ambiguous (CS0121) between the overloads for one position.
    // See issue #255.

    private const string ActionReturningTaskMessage =
        "An action returns a task, which Switch does not await. Use Match with two functions that return a task.";

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<TTask>(this Result result, Func<TTask> ifSuccessAction, Action<Error> ifErrorAction, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    public static void Switch<TIgnored>(this Result result, Func<ValueTask<TIgnored>> ifSuccessAction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<TValueTask>(this Result result, Func<TValueTask> ifSuccessAction, Action<Error> ifErrorAction,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<TTask>(this Result result, Action ifSuccessAction, Func<Error, TTask> ifErrorAction, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    public static void Switch<TIgnored>(this Result result, Action ifSuccessAction, Func<Error, ValueTask<TIgnored>> ifErrorAction)
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<TValueTask>(this Result result, Action ifSuccessAction, Func<Error, TValueTask> ifErrorAction,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: both actions return a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<TTask1, TTask2>(this Result result, Func<TTask1> ifSuccessAction, Func<Error, TTask2> ifErrorAction,
        Task? unused = null)
        where TTask1 : Task
        where TTask2 : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<T, TTask>(this Result<T> result, Func<T, TTask> ifSuccessAction, Action<Error> ifErrorAction, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    public static void Switch<T, TIgnored>(this Result<T> result, Func<T, ValueTask<TIgnored>> ifSuccessAction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<T, TValueTask>(this Result<T> result, Func<T, TValueTask> ifSuccessAction, Action<Error> ifErrorAction,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<T, TTask>(this Result<T> result, Action<T> ifSuccessAction, Func<Error, TTask> ifErrorAction, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    public static void Switch<T, TIgnored>(this Result<T> result, Action<T> ifSuccessAction, Func<Error, ValueTask<TIgnored>> ifErrorAction)
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<T, TValueTask>(this Result<T> result, Action<T> ifSuccessAction, Func<Error, TValueTask> ifErrorAction,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: both actions return a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<T, TTask1, TTask2>(this Result<T> result, Func<T, TTask1> ifSuccessAction, Func<Error, TTask2> ifErrorAction,
        Task? unused = null)
        where TTask1 : Task
        where TTask2 : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    #endregion
}
