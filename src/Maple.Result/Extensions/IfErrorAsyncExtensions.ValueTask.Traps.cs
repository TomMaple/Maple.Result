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

// The overload traps of IfErrorAsyncExtensions.ValueTask.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public static partial class IfErrorAsyncExtensions
{
    #region Not supported: functions returning a value

    // A function that returns a value is a transformation, not a side effect (#197): these overloads turn the call into a compile
    // error (CS0619) that names the method to use, instead of discarding the value. A statement lambda (e.g., x => { list.Add(x); })
    // or a method that returns nothing is a side effect and binds to the action overload.

    /// <summary>
    ///     Not supported: the function returns a value in a task (an assignment or an increment in an async lambda has one too), which
    ///     <c>IfErrorAsync</c> discards; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static ValueTask<TResult> IfErrorAsync<TResult, TValue>([DisallowNull] this TResult result, Func<Error, ValueTask<TValue>> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a value in a task (an assignment or an increment in an async lambda has one too), which
    ///     <c>IfErrorAsync</c> discards; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static ValueTask<TResult> IfErrorAsync<TResult, TValue>(this ValueTask<TResult> resultTask, Func<Error, ValueTask<TValue>> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a value in a task, which <c>IfErrorAsync</c> discards; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<TResult> IfErrorAsync<TResult, TTaskResult>(this ValueTask<TResult> resultTask,
        Func<Error, Task<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    #endregion

    #region Not supported: synchronous functions returning a value

    // A synchronous function that returns a value, passed to IfSuccessAsync or IfErrorAsync, is a compile error (CS0619) that names
    // the method to use, as in #197.

    /// <summary>
    ///     Not supported: the function returns a value (an assignment or an increment has one too), which <c>IfErrorAsync</c> discards;
    ///     use <c>ElseAsync</c>, or a statement lambda.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SyncValueDiscardedMessage, error: true)]
    public static ValueTask<TResult> IfErrorAsync<TResult, TValue>(this ValueTask<TResult> resultTask,
        Func<Error, TValue> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(SyncValueDiscardedMessage);
    }

    #endregion
}
