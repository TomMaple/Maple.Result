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
///     The collection of extension methods for executing actions or functions conditionally based on the success state of a <see cref="Result" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of operations that should only be performed if a preceding <see cref="Result" /> indicates success.
///     They help simplify error handling and reduce boilerplate code when working with <see cref="Result" />-based workflows.
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

    // These overloads turn a call with a function that returns a task into a compile error (CS0619). Without them, the call binds to the
    // generic overload, which wraps the task in a successful result without awaiting it (issue #119). They are more specific than the
    // generic overloads, so they are chosen first; an explicit type argument excludes them and selects the generic overload on purpose.
    // The Task overloads have a higher priority than the ValueTask ones: an async lambda converts to both, which would be ambiguous (CS0121).

    private const string TaskNotAwaitedMessage =
        "The function returns a task, which IfSuccess does not await. Use IfSuccessAsync, or specify the type arguments " +
        "(e.g., IfSuccess<Task>(…) or IfSuccess<T, Task>(…)) to keep the task.";

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
}
