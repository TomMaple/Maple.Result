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

// The overload traps of MatchAsyncExtensions.Task.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public static partial class MatchAsyncExtensions
{
    #region Not supported: two actions

    // MatchAsync is a fold: two functions that return one type. These overloads turn a call with two actions into a compile error (CS0619)
    // that names SwitchAsync. They return what the MatchAsync with two actions of 2.x returned, so that its call reports this error only.
    // A call that the fold accepts binds to the fold: a lambda with a value converts better to a function than to an action, and the
    // overloads with functions that return a task have priority 1.
    // See issue #255.

    private const string TwoActionsMessage =
        "MatchAsync takes two functions that return a value. Use SwitchAsync for two side effects, or ThenAsync(…).IfErrorAsync(…) to " +
        "transform the value and handle the error.";

    /// <summary>
    ///     Not supported: <c>MatchAsync</c> takes two functions that return a value; use <c>SwitchAsync</c> for two actions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TwoActionsMessage, error: true)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask, Action ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(TwoActionsMessage);
    }

    /// <summary>
    ///     Not supported: <c>MatchAsync</c> takes two functions that return a value; use <c>SwitchAsync</c> for two actions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TwoActionsMessage, error: true)]
    public static Task<Result<T>> MatchAsync<T>(this Task<Result<T>> resultTask, Action<T> ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(TwoActionsMessage);
    }

    #endregion

    #region Not supported: functions returning a task without a value

    // MatchAsync awaits two functions that return a task without a value when they are passed as actions: the overloads that run name
    // them ifSuccessAction and ifErrorAction. A call that names them ifSuccessFunction and ifErrorFunction cannot bind to those overloads.
    // Without these traps it binds to the fold of two synchronous functions, whose value is then the task of one of the functions: the
    // caller gets a task in a task, and awaiting the outer one leaves the inner one running (#273).
    //
    // The type of the success function is generic and constrained to the task type, and the type of the error function is not. That
    // makes a trap more specific than the fold, so that it wins the call that names the functions, and less specific than the overload
    // with two asynchronous actions, so that a call without names still binds to that one. No priority and no unused parameter is
    // needed. A function that returns a task with a value is awaited by an overload of its own, which is more specific or has priority 1.
    // The traps return what the overloads with two asynchronous actions return, so that the call reports this error only.

    private const string FunctionsReturnTaskWithoutValueMessage =
        "The functions return a task without a value, which MatchAsync awaits only when they are passed as actions. Name them " +
        "ifSuccessAction and ifErrorAction, or pass them without names; as ifSuccessFunction and ifErrorFunction, their task " +
        "would be the value of MatchAsync, and nobody would await it.";

    /// <summary>
    ///     Not supported: the functions return a task without a value, which <c>MatchAsync</c> awaits only as actions; name them
    ///     <c>ifSuccessAction</c> and <c>ifErrorAction</c>, or pass them without names.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionsReturnTaskWithoutValueMessage, error: true)]
    public static Task MatchAsync<TTask>(this Task<Result> resultTask, Func<TTask> ifSuccessFunction,
        Func<Error, Task> ifErrorFunction, bool? continueOnCapturedContext = null)
        where TTask : Task
    {
        throw new NotSupportedException(FunctionsReturnTaskWithoutValueMessage);
    }

    /// <summary>
    ///     Not supported: the functions return a task without a value, which <c>MatchAsync</c> awaits only as actions; name them
    ///     <c>ifSuccessAction</c> and <c>ifErrorAction</c>, or pass them without names.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionsReturnTaskWithoutValueMessage, error: true)]
    public static Task MatchAsync<TValueTask>(this Task<Result> resultTask, Func<TValueTask> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorFunction, bool? continueOnCapturedContext = null)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(FunctionsReturnTaskWithoutValueMessage);
    }

    /// <summary>
    ///     Not supported: the functions return a task without a value, which <c>MatchAsync</c> awaits only as actions; name them
    ///     <c>ifSuccessAction</c> and <c>ifErrorAction</c>, or pass them without names.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionsReturnTaskWithoutValueMessage, error: true)]
    public static Task MatchAsync<T, TTask>(this Task<Result<T>> resultTask, Func<T, TTask> ifSuccessFunction,
        Func<Error, Task> ifErrorFunction, bool? continueOnCapturedContext = null)
        where TTask : Task
    {
        throw new NotSupportedException(FunctionsReturnTaskWithoutValueMessage);
    }

    /// <summary>
    ///     Not supported: the functions return a task without a value, which <c>MatchAsync</c> awaits only as actions; name them
    ///     <c>ifSuccessAction</c> and <c>ifErrorAction</c>, or pass them without names.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionsReturnTaskWithoutValueMessage, error: true)]
    public static Task MatchAsync<T, TValueTask>(this Task<Result<T>> resultTask, Func<T, TValueTask> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorFunction, bool? continueOnCapturedContext = null)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(FunctionsReturnTaskWithoutValueMessage);
    }

    #endregion
}
