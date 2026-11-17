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

// The overload traps of LinqExtensions.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public static partial class LinqExtensions
{
    #region Not supported: selectors returning a task

    // These overloads turn a query whose select returns a task into a compile error (CS0619). Without them, the query binds to the
    // generic overload, which wraps the task in a successful result without awaiting it (issue #126). They are generic and constrained
    // to the task type (#251): a selector that only throws has no return type to infer, so it does not reach them, and a selector that
    // returns another value fails the constraint. The constraint to a Task also covers a Task<T>; IEquatable<ValueTask> is implemented
    // by ValueTask only, and a ValueTask<T> has an overload of its own, which is more specific than the generic overload. The unused
    // optional parameter only makes the signatures differ from the generic overloads. The overloads constrained to a Task or a
    // ValueTask have priority 1, so that they win over the generic overloads, also with explicit type arguments (method syntax); a task
    // is kept on purpose by selecting it in a result (e.g., select Result.FromValue(SaveAsync(x))). An async lambda (method syntax)
    // gives a Task, which fails the ValueTask constraint, so the two are never ambiguous.
    // They return a result of the task, the type that the query would have without them, so that a query that continues with the
    // result (e.g., (from x in ok select GetIntAsync(x)).Value) reports this error only (#274).

    private const string SelectorTaskNotAwaitedMessage =
        "The selector returns a task, which the query does not await. " +
        "Use ThenAsync (or IfSuccessAsync for a task without a value), or a from clause whose function returns a Task<Result<T>>, " +
        "or select the task in a result to keep it (e.g., select Result.FromValue(SaveAsync(x))).";

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TTask> Select<T, TTask>(this Result<T> result, Func<T, TTask> selector, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TValueTask> Select<T, TValueTask>(this Result<T> result, Func<T, TValueTask> selector, ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result<ValueTask<TIgnored>> Select<T, TIgnored>(this Result<T> result, Func<T, ValueTask<TIgnored>> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TTask> SelectMany<T, TMiddle, TTask>(
        this Result<T> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, TTask> resultSelector,
        Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TValueTask> SelectMany<T, TMiddle, TValueTask>(
        this Result<T> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, TValueTask> resultSelector,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result<ValueTask<TIgnored>> SelectMany<T, TMiddle, TIgnored>(
        this Result<T> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, ValueTask<TIgnored>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    #endregion

    #region Not supported: selectors returning a task, for the non-generic results

    // The traps of the region above, for the new source and collection shapes (#217): a result selector that returns a task
    // would bind to the overload with a TNext and wrap the task in the result.

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TTask> Select<TTask>(this Result result, Func<Result, TTask> selector, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TValueTask> Select<TValueTask>(this Result result, Func<Result, TValueTask> selector, ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result<ValueTask<TIgnored>> Select<TIgnored>(this Result result, Func<Result, ValueTask<TIgnored>> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TTask> SelectMany<T, TTask>(
        this Result<T> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, TTask> resultSelector,
        Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TValueTask> SelectMany<T, TValueTask>(
        this Result<T> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, TValueTask> resultSelector,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result<ValueTask<TIgnored>> SelectMany<T, TIgnored>(
        this Result<T> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, ValueTask<TIgnored>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TTask> SelectMany<TMiddle, TTask>(
        this Result result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, TTask> resultSelector,
        Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TValueTask> SelectMany<TMiddle, TValueTask>(
        this Result result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, TValueTask> resultSelector,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result<ValueTask<TIgnored>> SelectMany<TMiddle, TIgnored>(
        this Result result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, ValueTask<TIgnored>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TTask> SelectMany<TTask>(
        this Result result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, TTask> resultSelector,
        Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TValueTask> SelectMany<TValueTask>(
        this Result result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, TValueTask> resultSelector,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static Result<ValueTask<TIgnored>> SelectMany<TIgnored>(
        this Result result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, ValueTask<TIgnored>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    #endregion
}
