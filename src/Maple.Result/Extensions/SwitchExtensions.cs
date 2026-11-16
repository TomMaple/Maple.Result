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

/// <summary>
///     The collection of extension methods that run one of two actions depending on whether a <see cref="Result" /> or <see cref="Result{T}" />
///     is successful, and return nothing.
/// </summary>
/// <remarks>
///     <c>Switch</c> ends a chain for side effects, as <c>Match</c> ends it for a value. To run a side effect and keep chaining, use
///     <c>IfSuccess(…).IfError(…)</c>, which return the result. An action that returns a value is a side effect (e.g.,
///     <c>x => set.Add(x)</c>); an action that returns a task is a compile error (CS0619), because <c>Switch</c> would not await it.
/// </remarks>
public static class SwitchExtensions
{
    #region Result

    /// <summary>
    ///     Invokes one of the provided actions depending on whether the <paramref name="result" /> is successful.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> whose state selects the action. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The action invoked if the <paramref name="result" /> is successful. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action invoked if the <paramref name="result" /> is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static void Switch(this Result result, Action ifSuccessAction, Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess)
            ifSuccessAction();
        else
            ifErrorAction(result.Error);
    }

    #endregion

    #region Result<T>

    /// <summary>
    ///     Invokes one of the provided actions depending on whether the <paramref name="result" /> is successful.
    /// </summary>
    /// <typeparam name="T">The type of the value of the <paramref name="result" />.</typeparam>
    /// <param name="result">The <see cref="Result{T}" /> whose state selects the action. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The action invoked if the <paramref name="result" /> is successful, with its value. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action invoked if the <paramref name="result" /> is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static void Switch<T>(this Result<T> result, Action<T> ifSuccessAction, Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess)
            ifSuccessAction(result.Value);
        else
            ifErrorAction(result.Error);
    }

    #endregion

    #region Not supported: actions returning a task

    // A lambda that returns a task converts to an action, and the task would never be awaited (an async lambda becomes async void). These
    // overloads turn such a call into a compile error (CS0619) that names Match, in the success position, in the error position and in both.
    // They are generic and constrained to the task type (#251): a lambda that only throws has no return type to infer, so it does not
    // reach them, and a lambda that returns another value fails the constraint. The unused optional parameter only makes the signatures
    // differ. The overloads constrained to a Task or a ValueTask have priority 1: an async lambda converts to both a Task and a ValueTask,
    // and an action that returns a task in both positions would otherwise be ambiguous (CS0121) between the overloads for one position.
    // See issue #255.

    private const string ActionReturningTaskMessage =
        "An action returns a task, which Switch does not await. Use Match with two functions that return a task.";

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<TTask>(this Result result, Func<TTask> ifSuccessAction, Action<Error> ifErrorAction, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    public static void Switch<TIgnored>(this Result result, Func<ValueTask<TIgnored>> ifSuccessAction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<TValueTask>(this Result result, Func<TValueTask> ifSuccessAction, Action<Error> ifErrorAction,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<TTask>(this Result result, Action ifSuccessAction, Func<Error, TTask> ifErrorAction, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    public static void Switch<TIgnored>(this Result result, Action ifSuccessAction, Func<Error, ValueTask<TIgnored>> ifErrorAction)
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<TValueTask>(this Result result, Action ifSuccessAction, Func<Error, TValueTask> ifErrorAction,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: both actions return a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<TTask1, TTask2>(this Result result, Func<TTask1> ifSuccessAction, Func<Error, TTask2> ifErrorAction,
        Task? unused = null)
        where TTask1 : Task
        where TTask2 : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<T, TTask>(this Result<T> result, Func<T, TTask> ifSuccessAction, Action<Error> ifErrorAction, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    public static void Switch<T, TIgnored>(this Result<T> result, Func<T, ValueTask<TIgnored>> ifSuccessAction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<T, TValueTask>(this Result<T> result, Func<T, TValueTask> ifSuccessAction, Action<Error> ifErrorAction,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<T, TTask>(this Result<T> result, Action<T> ifSuccessAction, Func<Error, TTask> ifErrorAction, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    public static void Switch<T, TIgnored>(this Result<T> result, Action<T> ifSuccessAction, Func<Error, ValueTask<TIgnored>> ifErrorAction)
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: an action returns a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<T, TValueTask>(this Result<T> result, Action<T> ifSuccessAction, Func<Error, TValueTask> ifErrorAction,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    /// <summary>
    ///     Not supported: both actions return a task, which <c>Switch</c> does not await; use <c>Match</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionReturningTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static void Switch<T, TTask1, TTask2>(this Result<T> result, Func<T, TTask1> ifSuccessAction, Func<Error, TTask2> ifErrorAction,
        Task? unused = null)
        where TTask1 : Task
        where TTask2 : Task
    {
        throw new NotSupportedException(ActionReturningTaskMessage);
    }

    #endregion
}
