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

// The overload traps of IfSuccessAsyncExtensions.Task.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public static partial class IfSuccessAsyncExtensions
{
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
    [OverloadResolutionPriority(1)]
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
    [OverloadResolutionPriority(1)]
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

    /// <summary>
    ///     Not supported: the function returns a value in a task, which <c>IfSuccessAsync</c> discards; use <c>ThenAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static Task<Result> IfSuccessAsync<TTaskResult>(this Task<Result> resultTask,
        Func<ValueTask<TTaskResult>> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a value in a task, which <c>IfSuccessAsync</c> discards; use <c>ThenAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static Task<Result<T>> IfSuccessAsync<T, TTaskResult>(this Task<Result<T>> resultTask,
        Func<T, ValueTask<TTaskResult>> ifSuccessAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    #endregion

    #region Not supported: synchronous functions returning a value

    // A synchronous function that returns a value, passed to IfSuccessAsync or IfErrorAsync, is a compile error (CS0619) that names
    // the method to use, as in #197.

    private const string SyncValueDiscardedMessage =
        "The function returns a value, which IfSuccessAsync discards; an assignment or an increment (e.g., x => last = x or x => count++) " +
        "has a value too. " +
        "Use ThenAsync to keep the value, or a statement lambda (e.g., x => { list.Add(x); } or x => { count++; }) for a side effect.";

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

    #endregion
}
