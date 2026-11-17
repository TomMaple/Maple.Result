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

// The overload traps of ElseAsyncExtensions.ValueTask.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public static partial class ElseAsyncExtensions
{
    #region Not supported: functions returning a task without a value, on task results

    // A function that returns a task without a value is a side effect, not a transformation (#197). On a task result of a
    // Result<object>, it would bind to the overload with a synchronous function and give a successful result that holds the task, which
    // nobody awaits: these overloads turn the call into a compile error (CS0619) that names the method to use, for both kinds of task. On a result, and for a
    // synchronous action, the compiler reports the call by itself (e.g., CS0029, CS0266 or CS1643), so there are no overloads for them (#256).

    /// <summary>
    ///     Not supported: the function returns a task without a value; use <c>IfErrorAsync</c> for a side effect.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionMessage, error: true)]
    public static ValueTask<TResult> ElseAsync<TResult>(this ValueTask<TResult> resultTask, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ActionMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task without a value; use <c>IfErrorAsync</c> for a side effect.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<TResult> ElseAsync<TResult>(this ValueTask<TResult> resultTask,
        Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ActionMessage);
    }

    #endregion
}
