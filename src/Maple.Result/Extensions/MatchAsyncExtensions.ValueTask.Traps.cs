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
using System.Threading.Tasks;

namespace Maple.Results.Extensions;

// The overload traps of MatchAsyncExtensions.ValueTask.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public static partial class MatchAsyncExtensions
{
    #region Not supported: two actions

    // MatchAsync is a fold: two functions that return one type. These overloads turn a call with two actions into a compile error (CS0619)
    // that names SwitchAsync. They return what the MatchAsync with two actions of 2.x returned, so that its call reports this error only.
    // A call that the fold accepts binds to the fold: a lambda with a value converts better to a function than to an action, and the
    // overloads with functions that return a task have priority 1.
    // See issue #255.

    /// <summary>
    ///     Not supported: <c>MatchAsync</c> takes two functions that return a value; use <c>SwitchAsync</c> for two actions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TwoActionsMessage, error: true)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask, Action ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(TwoActionsMessage);
    }

    /// <summary>
    ///     Not supported: <c>MatchAsync</c> takes two functions that return a value; use <c>SwitchAsync</c> for two actions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TwoActionsMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result<T>> resultTask, Action<T> ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(TwoActionsMessage);
    }

    #endregion
}
