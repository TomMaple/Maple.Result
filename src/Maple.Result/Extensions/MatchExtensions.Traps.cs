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

namespace Maple.Results.Extensions;

// The overload traps of MatchExtensions.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public static partial class MatchExtensions
{
    #region Not supported: two actions

    // Match is a fold: two functions that return one type. Without these overloads, two actions would report that no overload takes them;
    // these turn such a call into a compile error (CS0619) that names Switch, so that code written for the Match with two actions of 2.x gets
    // a message that says what to do. They return what that Match returned, so that its call reports this error only (e.g., in
    // var r = result.Match(…)), not one more for the type. A call that the fold accepts binds to the fold: a lambda with a value converts better to a function
    // than to an action. Two lambdas that only throw have no type to infer, so they bind here too.
    // See issue #255.

    private const string TwoActionsMessage =
        "Match takes two functions that return a value. Use Switch for two side effects, or Then(…).IfError(…) to transform the value and " +
        "handle the error.";

    /// <summary>
    ///     Not supported: <c>Match</c> takes two functions that return a value; use <c>Switch</c> for two actions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TwoActionsMessage, error: true)]
    public static Result Match(this Result result, Action ifSuccessAction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(TwoActionsMessage);
    }

    /// <summary>
    ///     Not supported: <c>Match</c> takes two functions that return a value; use <c>Switch</c> for two actions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TwoActionsMessage, error: true)]
    public static Result<T> Match<T>(this Result<T> result, Action<T> ifSuccessAction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(TwoActionsMessage);
    }

    #endregion
}
