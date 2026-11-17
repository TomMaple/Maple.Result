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
///     The collection of extension methods that run a side effect when an <see cref="IOperationResult" /> is failed: an action receives
///     the <see cref="Error" />, and the result is returned unchanged.
/// </summary>
/// <remarks>
///     A successful result is returned unchanged, and the action is not called. To replace a failed result (e.g., recover with a
///     value, or return another error), use <c>Else</c>; a function that returns a value is a compile error here (CS0619), so that a failure
///     is not turned into a success by mistake.
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

        if (result.IsError)
            ifErrorAction(result.Error);

        return result;
    }

    #region Not supported: error actions returning a task

    // These overloads turn a call with an error action that returns a task into a compile error (CS0619). Without them, a lambda
    // that returns a task converts to Action<Error>, and the task is never awaited (an async lambda becomes async void, issue #123).
    // They are generic and constrained to the task type (#251): a lambda that only throws has no return type to infer, so it does not
    // reach them and binds to the Action<Error> overload, and a lambda that returns another value fails the constraint. The constraint
    // to a Task also covers a Task<T>; IEquatable<ValueTask> is implemented by ValueTask only, and a ValueTask<T> has an overload of its
    // own, whose type argument cannot be inferred from a lambda that only throws either. The unused optional parameter only makes the
    // signatures differ. The overloads constrained to a Task or a ValueTask have priority 1, so that they win over the value trap below;
    // an async lambda gives a Task, which fails the ValueTask constraint, so the two are never ambiguous.

    private const string ErrorActionTaskNotAwaitedMessage =
        "The error action returns a task, which IfError does not await. Use IfErrorAsync, or discard the task explicitly in a " +
        "statement lambda (e.g., e => { _ = LogAsync(e); }).";

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>IfError</c> does not await; use <c>IfErrorAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static TResult IfError<TResult, TTask>([DisallowNull] this TResult result, Func<Error, TTask> ifErrorAction, Task? unused = null)
        where TResult : IOperationResult
        where TTask : Task
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>IfError</c> does not await; use <c>IfErrorAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static TResult IfError<TResult, TValueTask>([DisallowNull] this TResult result, Func<Error, TValueTask> ifErrorAction,
        ValueTask unused = default)
        where TResult : IOperationResult
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>IfError</c> does not await; use <c>IfErrorAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static TResult IfError<TResult, TIgnored>([DisallowNull] this TResult result, Func<Error, ValueTask<TIgnored>> ifErrorAction)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    #endregion

    #region Not supported: functions returning a value

    // A function that returns a value is a transformation, not a side effect (#197): these overloads turn the call into a compile
    // error (CS0619) that names the method to use, instead of discarding the value. A statement lambda (e.g., x => { list.Add(x); })
    // or a method that returns nothing is a side effect and binds to the action overload.

    private const string ValueDiscardedMessage =
        "The function returns a value, which IfError discards; an assignment or an increment (e.g., e => last = e or e => count++) has a value too. " +
        "Use Else to replace the result with the value, or a statement lambda (e.g., e => { titles.Add(e.Title); } or e => { count++; }) for a side effect.";

    /// <summary>
    ///     Not supported: the function returns a value (an assignment or an increment has one too), which <c>IfError</c> discards;
    ///     use <c>Else</c>, or a statement lambda.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ValueDiscardedMessage, error: true)]
    public static TResult IfError<TResult, TValue>([DisallowNull] this TResult result, Func<Error, TValue> ifErrorAction)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ValueDiscardedMessage);
    }

    #endregion
}
