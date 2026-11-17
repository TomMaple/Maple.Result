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

// The overload traps of ThenExtensions.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public static partial class ThenExtensions
{
    #region Not supported: functions returning a task

    // These overloads turn a call with a function that returns a task into a compile error (CS0619). Without them, the call binds to the
    // generic overload, which wraps the task in a successful result without awaiting it (issue #119). They are generic and constrained
    // to the task type (#251): a lambda that only throws has no return type to infer, so it does not reach them and binds to an
    // overload that runs, and a lambda that returns another value fails the constraint. The constraint to a Task also covers a Task<T>;
    // IEquatable<ValueTask> is implemented by ValueTask only, and a ValueTask<T> has an overload of its own, which is more specific than
    // the generic overload. The unused optional parameter only makes the signatures differ from the generic overloads. The overloads
    // constrained to a Task or a ValueTask have priority 1, so that they win over the generic overloads, also with explicit type
    // arguments; a task is kept on purpose by returning it in a result (e.g., x => Result.FromValue(SaveAsync(x))). An async lambda
    // gives a Task, which fails the ValueTask constraint, so the two are never ambiguous. The parameter has the name of the generic
    // overload (ifSuccessFunction): a trap applies only to a call that uses its names, so a call that names the argument reaches the
    // trap too (#273).

    private const string ThenTaskNotAwaitedMessage =
        "The function returns a task, which Then does not await. Use ThenAsync, or return the task in a result to keep it " +
        "(e.g., x => Result.FromValue(SaveAsync(x))).";

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Then</c> does not await; use <c>ThenAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ThenTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Then<TTask>(this Result result, Func<TTask> ifSuccessFunction, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(ThenTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Then</c> does not await; use <c>ThenAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ThenTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Then<TValueTask>(this Result result, Func<TValueTask> ifSuccessFunction, ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ThenTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Then</c> does not await; use <c>ThenAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ThenTaskNotAwaitedMessage, error: true)]
    public static Result Then<TIgnored>(this Result result, Func<ValueTask<TIgnored>> ifSuccessFunction)
    {
        throw new NotSupportedException(ThenTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Then</c> does not await; use <c>ThenAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ThenTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Then<T, TTask>(this Result<T> result, Func<T, TTask> ifSuccessFunction, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(ThenTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Then</c> does not await; use <c>ThenAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ThenTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Then<T, TValueTask>(this Result<T> result, Func<T, TValueTask> ifSuccessFunction, ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ThenTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Then</c> does not await; use <c>ThenAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ThenTaskNotAwaitedMessage, error: true)]
    public static Result Then<T, TIgnored>(this Result<T> result, Func<T, ValueTask<TIgnored>> ifSuccessFunction)
    {
        throw new NotSupportedException(ThenTaskNotAwaitedMessage);
    }

    #endregion
}
