// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Result.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Result.Extensions;

/// <summary>
///     The collection of extension methods for executing actions or functions conditionally based on the error state of a <see cref="IOperationResult" />
///     instance.
/// </summary>
/// <remarks>
///     These methods enable fluent handling of error scenarios by allowing to specify actions or functions that should be executed only when the result
///     indicates an error.
/// </remarks>
public static class IfErrorExtensions
{
    /// <summary>
    ///     Invokes the specified action if the <paramref name="result" /> represents an error, and returns the original <paramref name="result" />.
    /// </summary>
    /// <remarks>
    ///     Use this method to perform side effects, such as logging or error handling, when a <paramref name="result" /> indicates an error,
    ///     without altering the <paramref name="result" /> itself.
    ///     The method does not modify the <paramref name="result" /> or handle the <see cref="Error" /> beyond invoking the specified action.
    /// </remarks>
    /// <typeparam name="TResult">The type of the result object, which must implement the <see cref="IOperationResult" /> interface.</typeparam>
    /// <param name="result">The <paramref name="result" /> to inspect for an error. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorAction">
    ///     The action to execute if the <paramref name="result" /> is an error.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The original <paramref name="result" /> instance, regardless of whether the action was invoked.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static TResult IfError<TResult>([DisallowNull] this TResult result, Action<Error> ifErrorAction)
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (!result.IsSuccess())
            ifErrorAction(result.Error);

        return result;
    }

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
    public static TResult IfError<TResult>([DisallowNull] this TResult result, Func<Error, TResult> ifErrorFunction)
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        if (result.IsSuccess())
            return result;

        var errorResult = ifErrorFunction(result.Error);

        return Guard.EnsureNotNullReturned(errorResult, nameof(ifErrorFunction));
    }

    #region Not supported: error actions returning a task

    // These overloads turn a call with an error action that returns a task into a compile error (CS0619). Without them, a lambda
    // that returns a task converts to Action<Error>, and the task is never awaited (an async lambda becomes async void, issue #123).
    // A lambda whose body has a value prefers a delegate with a return type, so these overloads win over the Action<Error> one.
    // A Task<T> converts to Task, so the Task overload also covers functions returning Task<T>. It has a higher priority than the
    // ValueTask ones: an async lambda converts to both, which would be ambiguous (CS0121).

    private const string ErrorActionTaskNotAwaitedMessage =
        "The error action returns a task, which IfError does not await. Use IfErrorAsync, or discard the task explicitly in a " +
        "statement lambda (e.g., e => { _ = LogAsync(e); }).";

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>IfError</c> does not await; use <c>IfErrorAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static TResult IfError<TResult>([DisallowNull] this TResult result, Func<Error, Task> ifErrorAction)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>IfError</c> does not await; use <c>IfErrorAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static TResult IfError<TResult>([DisallowNull] this TResult result, Func<Error, ValueTask> ifErrorAction)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>IfError</c> does not await; use <c>IfErrorAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static TResult IfError<TResult, TTaskResult>([DisallowNull] this TResult result, Func<Error, ValueTask<TTaskResult>> ifErrorAction)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    #endregion
}
