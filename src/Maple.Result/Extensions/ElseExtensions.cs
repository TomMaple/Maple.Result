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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Results.Extensions;

/// <summary>
///     The collection of extension methods that replace a failed <see cref="IOperationResult" />: a function that receives the <see cref="Error" />
///     returns the replacement result, e.g., a recovered value or another error.
/// </summary>
/// <remarks>
///     A successful result is returned unchanged, and the function is not called. For a side effect that keeps the result unchanged, use
///     <c>IfError</c>.
/// </remarks>
public static class ElseExtensions
{
    #region None

    /// <summary>
    ///     Invokes the specified function if the <paramref name="result" /> represents an error and returns its outcome;
    ///     otherwise, returns the original <paramref name="result" />.
    /// </summary>
    /// <typeparam name="TResult">The type of the result object, which must implement the <see cref="IOperationResult" /> interface.</typeparam>
    /// <param name="result">The <paramref name="result" /> to inspect for an error. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorFunction">
    ///     The function to execute if the <paramref name="result" /> is an error. The <see cref="Error" /> associated with the <paramref name="result" /> is
    ///     passed to this function, and the function’s return value is used as the outcome.
    ///     The function must return a value of the <typeparamref name="TResult" /> type. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The original <paramref name="result" /> if it represents success; otherwise, the outcome of the <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifErrorFunction" /> returns <see langword="null" />.</exception>
    public static TResult Else<TResult>([DisallowNull] this TResult result, Func<Error, TResult> ifErrorFunction)
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        if (result.IsSuccess)
            return result;

        var errorResult = ifErrorFunction(result.Error);

        return Guard.EnsureNotNullReturned(errorResult, nameof(ifErrorFunction));
    }

    #endregion

    #region Not supported: functions returning a task

    // These overloads turn a call with a function that returns a task into a compile error (CS0619): Else does not await it. Without
    // them, the task would replace the result (the function returns the replacement) and nobody would await it. A Task<T> converts
    // to Task, so the Task overload also covers functions returning Task<T>. It has a higher priority than the ValueTask ones: an
    // async lambda converts to both, which would be ambiguous (CS0121).

    private const string ElseTaskNotAwaitedMessage =
        "The function returns a task, which Else does not await. Use ElseAsync.";

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Else</c> does not await; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ElseTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static TResult Else<TResult>([DisallowNull] this TResult result, Func<Error, Task> ifErrorFunction)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ElseTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Else</c> does not await; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ElseTaskNotAwaitedMessage, error: true)]
    public static TResult Else<TResult>([DisallowNull] this TResult result, Func<Error, ValueTask> ifErrorFunction)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ElseTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Else</c> does not await; use <c>ElseAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ElseTaskNotAwaitedMessage, error: true)]
    public static TResult Else<TResult, TTaskResult>([DisallowNull] this TResult result, Func<Error, ValueTask<TTaskResult>> ifErrorFunction)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ElseTaskNotAwaitedMessage);
    }

    #endregion

    #region Not supported: actions

    // A function that returns nothing is a side effect, not a transformation (#197): these overloads turn the call into a compile
    // error (CS0619) that names the method to use, instead of the conversion errors CS0029 and CS1662.

    private const string ActionMessage =
        "The action returns nothing. Else needs a function that returns the replacement result (a value, a result or an " +
        "error); use IfError for a side effect.";

    /// <summary>
    ///     Not supported: the action returns nothing; use <c>IfError</c> for a side effect.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionMessage, error: true)]
    public static TResult Else<TResult>([DisallowNull] this TResult result, Action<Error> ifErrorFunction)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ActionMessage);
    }

    #endregion
}
