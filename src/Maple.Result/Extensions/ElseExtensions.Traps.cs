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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Results.Extensions;

// The overload traps of ElseExtensions.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public static partial class ElseExtensions
{
    #region Not supported: functions returning a task

    // These overloads turn a call with a function that returns a task into a compile error (CS0619): Else does not await it. Without
    // them, the task would replace the result (the function returns the replacement) and nobody would await it. They are generic and
    // constrained to the task type (#251): a lambda that only throws has no return type to infer, so it does not reach them and binds
    // to the overload that runs, and a lambda that returns another value fails the constraint. The constraint to a Task also covers a
    // Task<T>; IEquatable<ValueTask> is implemented by ValueTask only, and a ValueTask<T> has an overload of its own, whose type argument
    // cannot be inferred from a lambda that only throws either. The unused optional parameter only makes the signatures differ. The
    // overloads constrained to a Task or a ValueTask have priority 1; an async lambda gives a Task, which fails the ValueTask
    // constraint, so the two are never ambiguous. The parameter has the name of the overload that runs (ifErrorFunction): a trap
    // applies only to a call that uses its names, so a call that names the argument reaches the trap too (#273).

    private const string ElseTaskNotAwaitedMessage =
        "The function returns a task, which Else does not await. Use ElseAsync.";

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Else</c> does not await; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ElseTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static TResult Else<TResult, TTask>([DisallowNull] this TResult result, Func<Error, TTask> ifErrorFunction, Task? unused = null)
        where TResult : IOperationResult
        where TTask : Task
    {
        throw new NotSupportedException(ElseTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Else</c> does not await; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ElseTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static TResult Else<TResult, TValueTask>([DisallowNull] this TResult result, Func<Error, TValueTask> ifErrorFunction,
        ValueTask unused = default)
        where TResult : IOperationResult
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ElseTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Else</c> does not await; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ElseTaskNotAwaitedMessage, error: true)]
    public static TResult Else<TResult, TIgnored>([DisallowNull] this TResult result, Func<Error, ValueTask<TIgnored>> ifErrorFunction)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ElseTaskNotAwaitedMessage);
    }

    #endregion
}
