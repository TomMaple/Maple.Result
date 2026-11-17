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

// The overload traps of IfSuccessExtensions.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public static partial class IfSuccessExtensions
{
    #region Not supported: functions returning a task

    // These overloads turn a call with a function that returns a task into a compile error (CS0619) that points to IfSuccessAsync.
    // Without them, the value traps below would report the call as a discarded value. They are generic and constrained to the task
    // type (#251): a lambda that only throws has no return type to infer, so it does not reach them and binds to the action overload,
    // and a lambda that returns another value fails the constraint and reaches the value trap. The constraint to a Task also covers a
    // Task<T>; IEquatable<ValueTask> is implemented by ValueTask only, and a ValueTask<T> has an overload of its own, whose type argument
    // cannot be inferred from a lambda that only throws either. The unused optional parameter only makes the signatures differ from
    // the value traps. The overloads constrained to a Task or a ValueTask have priority 1, so that they win over the value traps; an
    // async lambda gives a Task, which fails the ValueTask constraint, so the two are never ambiguous.

    private const string TaskNotAwaitedMessage =
        "The function returns a task, which IfSuccess does not await. Use IfSuccessAsync, or discard the task " +
        "explicitly in a statement lambda (e.g., x => { _ = SaveAsync(x); }).";

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result IfSuccess<TTask>(this Result result, Func<TTask> ifSuccessAction, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result IfSuccess<TValueTask>(this Result result, Func<TValueTask> ifSuccessAction, ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    public static Result IfSuccess<TIgnored>(this Result result, Func<ValueTask<TIgnored>> ifSuccessAction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result IfSuccess<T, TTask>(this Result<T> result, Func<T, TTask> ifSuccessAction, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result IfSuccess<T, TValueTask>(this Result<T> result, Func<T, TValueTask> ifSuccessAction, ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    public static Result IfSuccess<T, TIgnored>(this Result<T> result, Func<T, ValueTask<TIgnored>> ifSuccessAction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    #endregion

    #region Not supported: functions returning a value

    // A function that returns a value is a transformation, not a side effect (#197): these overloads turn the call into a compile
    // error (CS0619) that names the method to use, instead of discarding the value. A statement lambda (e.g., x => { list.Add(x); })
    // or a method that returns nothing is a side effect and binds to the action overload.

    private const string ValueDiscardedMessage =
        "The function returns a value, which IfSuccess discards; an assignment or an increment (e.g., x => last = x or x => count++) has a value too. " +
        "Use Then to keep the value, or a statement lambda (e.g., x => { list.Add(x); } or x => { count++; }) for a side effect.";

    /// <summary>
    ///     Not supported: the function returns a value (an assignment or an increment has one too), which <c>IfSuccess</c> discards;
    ///     use <c>Then</c>, or a statement lambda.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static Result IfSuccess<TValue>(this Result result, Func<TValue> ifSuccessAction)
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a value (an assignment or an increment has one too), which <c>IfSuccess</c> discards;
    ///     use <c>Then</c>, or a statement lambda.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static Result<T> IfSuccess<T, TValue>(this Result<T> result, Func<T, TValue> ifSuccessAction)
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    #endregion
}
