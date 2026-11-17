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

// The overload traps of IfErrorExtensions.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public static partial class IfErrorExtensions
{
    #region Not supported: error actions returning a task

    // These overloads turn a call with an error action that returns a task into a compile error (CS0619). Without them, a lambda
    // that returns a task converts to Action<Error>, and the task is never awaited (an async lambda becomes async void, issue #123).
    // They are generic and constrained to the task type (#251): a lambda that only throws has no return type to infer, so it does not
    // reach them and binds to the Action<Error> overload, and a lambda that returns another value fails the constraint. The constraint
    // to a Task also covers a Task<T>; IEquatable<ValueTask> is implemented by ValueTask only, and a ValueTask<T> has an overload of its
    // own, whose type argument cannot be inferred from a lambda that only throws either. The unused optional parameter only makes the
    // signatures differ. The overloads constrained to a Task or a ValueTask have priority 1, so that they win over the value trap below;
    // an async lambda gives a Task, which fails the ValueTask constraint, so the two are never ambiguous.

    private const string ErrorActionTaskNotAwaitedMessage =
        "The error action returns a task, which IfError does not await. Use IfErrorAsync, or discard the task explicitly in a " +
        "statement lambda (e.g., e => { _ = LogAsync(e); }).";

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>IfError</c> does not await; use <c>IfErrorAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static TResult IfError<TResult, TTask>([DisallowNull] this TResult result, Func<Error, TTask> ifErrorAction, Task? unused = null)
        where TResult : IOperationResult
        where TTask : Task
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>IfError</c> does not await; use <c>IfErrorAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static TResult IfError<TResult, TValueTask>([DisallowNull] this TResult result, Func<Error, TValueTask> ifErrorAction,
        ValueTask unused = default)
        where TResult : IOperationResult
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>IfError</c> does not await; use <c>IfErrorAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static TResult IfError<TResult, TIgnored>([DisallowNull] this TResult result, Func<Error, ValueTask<TIgnored>> ifErrorAction)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    #endregion

    #region Not supported: functions returning a value

    // A function that returns a value is a transformation, not a side effect (#197): these overloads turn the call into a compile
    // error (CS0619) that names the method to use, instead of discarding the value. A statement lambda (e.g., x => { list.Add(x); })
    // or a method that returns nothing is a side effect and binds to the action overload.

    private const string ValueDiscardedMessage =
        "The function returns a value, which IfError discards; an assignment or an increment (e.g., e => last = e or e => count++) has a value too. " +
        "Use Else to replace the result with the value, or a statement lambda (e.g., e => { titles.Add(e.Title); } or e => { count++; }) for a side effect.";

    /// <summary>
    ///     Not supported: the function returns a value (an assignment or an increment has one too), which <c>IfError</c> discards;
    ///     use <c>Else</c>, or a statement lambda.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static TResult IfError<TResult, TValue>([DisallowNull] this TResult result, Func<Error, TValue> ifErrorAction)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    #endregion
}
