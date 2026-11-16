// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Results.Extensions;

/// <summary>
///     The collection of extension methods that run a side effect when a <see cref="Result" /> or <see cref="Result{T}" /> is successful: an action
///     receives the value (if
///     any), and the result is returned unchanged.
/// </summary>
/// <remarks>
///     A failed result is returned unchanged, and the action is not called. To transform or chain the result with a function that
///     returns a value, a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" />, use <c>Then</c>; a function that returns
///     a value is a compile error here (CS0619), so that the value is not discarded by mistake.
/// </remarks>
public static class IfSuccessExtensions
{
    #region Result extensions

    /// <summary>
    ///     Invokes the specified action if the <paramref name="result" /> represents a successful outcome.
    /// </summary>
    /// <remarks>
    ///     This method enables fluent handling of successful <see cref="Result" />s by executing the provided action only when the <paramref name="result" />
    ///     indicates success. The action is not invoked if the <paramref name="result" /> is not successful.
    /// </remarks>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The action to execute if the <paramref name="result" /> is successful. Cannot be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The original <paramref name="result" />, allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result IfSuccess(this Result result, Action ifSuccessAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        if (result.IsSuccess)
            ifSuccessAction();

        return result;
    }

    #endregion

    #region Result<T> extensions

    /// <summary>
    ///     Invokes the specified action if the <paramref name="result" /> represents a successful outcome.
    /// </summary>
    /// <remarks>
    ///     This method enables fluent handling of successful <see cref="Result{T}" />s by executing the provided action only when
    ///     the <paramref name="result" /> indicates success. The action is not invoked if the <paramref name="result" /> is not successful.
    /// </remarks>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The action to execute if the <paramref name="result" /> is successful.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The original <paramref name="result" />, allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<T> IfSuccess<T>(this Result<T> result, Action<T> ifSuccessAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        if (result.IsSuccess)
            ifSuccessAction(result.Value);

        return result;
    }

    #endregion

    #region Not supported: functions returning a task

    // These overloads turn a call with a function that returns a task into a compile error (CS0619) that points to IfSuccessAsync.
    // Without them, the value traps below would report the call as a discarded value. They are more specific than the value traps,
    // so they are chosen first. The Task overloads have a higher priority than the ValueTask ones: an async lambda converts to both,
    // which would be ambiguous (CS0121).

    private const string TaskNotAwaitedMessage =
        "The function returns a task, which IfSuccess does not await. Use IfSuccessAsync, or discard the task " +
        "explicitly in a statement lambda (e.g., x => { _ = SaveAsync(x); }).";

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result IfSuccess(this Result result, Func<Task> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result IfSuccess<TNext>(this Result result, Func<Task<TNext>> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    public static Result IfSuccess(this Result result, Func<ValueTask> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    public static Result IfSuccess<TNext>(this Result result, Func<ValueTask<TNext>> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result IfSuccess<T>(this Result<T> result, Func<T, Task> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result IfSuccess<T, TNext>(this Result<T> result, Func<T, Task<TNext>> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    public static Result IfSuccess<T>(this Result<T> result, Func<T, ValueTask> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    public static Result IfSuccess<T, TNext>(this Result<T> result, Func<T, ValueTask<TNext>> ifSuccessFunction)
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
