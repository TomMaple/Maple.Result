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

// The overload traps of LinqAsyncExtensions.ValueTask.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public static partial class LinqAsyncExtensions
{
    #region Not supported: selectors returning a task

    // These overloads turn a query whose select returns a task into a compile error (CS0619). Without them, the query binds to the
    // generic overload, which wraps the task in a successful result without awaiting it (issue #126). They are more specific than the
    // generic overloads, so they are chosen first; explicit type arguments (method syntax) exclude them and keep the task on purpose.
    // The Task overloads have a higher priority than the ValueTask ones: an async lambda (possible in method syntax) converts to both,
    // which would be ambiguous (CS0121).

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> Select<T>(this ValueTask<Result<T>> result, Func<T, Task> selector)
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
    public static ValueTask<Result<TNext>> Select<T, TNext>(this ValueTask<Result<T>> result, Func<T, Task<TNext>> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> Select<T>(this ValueTask<Result<T>> result, Func<T, ValueTask> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> Select<T, TNext>(this ValueTask<Result<T>> result, Func<T, ValueTask<TNext>> selector)
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
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Task> resultSelector)
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
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, ValueTask<TNext>> resultSelector)
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
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this ValueTask<Result<T>> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, Task> resultSelector)
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
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this ValueTask<Result<T>> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, ValueTask<TNext>> resultSelector)
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
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this Result<T> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Task> resultSelector)
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
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this Result<T> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await;
    ///     use <c>ThenAsync</c> (or <c>IfSuccessAsync</c> for a task without a value).
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, ValueTask<TNext>> resultSelector)
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
    public static ValueTask<Result> Select(this ValueTask<Result> result, Func<Result, Task> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> Select<TNext>(this ValueTask<Result> result, Func<Result, Task<TNext>> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> Select(this ValueTask<Result> result, Func<Result, ValueTask> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> Select<TNext>(this ValueTask<Result> result, Func<Result, ValueTask<TNext>> selector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany<T>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<T>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany<TMiddle>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<TMiddle>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this ValueTask<Result> result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany<T>(
        this ValueTask<Result<T>> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<T>(
        this ValueTask<Result<T>> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, Result> collectionSelector,
        Func<T, Result, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany<TMiddle>(
        this ValueTask<Result> result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this ValueTask<Result> result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<TMiddle>(
        this ValueTask<Result> result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this ValueTask<Result> result,
        Func<Result, Result<TMiddle>> collectionSelector,
        Func<Result, TMiddle, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany(
        this ValueTask<Result> result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this ValueTask<Result> result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany(
        this ValueTask<Result> result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this ValueTask<Result> result,
        Func<Result, Result> collectionSelector,
        Func<Result, Result, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany<T>(
        this Result<T> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<T>(
        this Result<T> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result>> collectionSelector,
        Func<T, Result, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany<TMiddle>(
        this Result result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this Result result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany<TMiddle>(
        this Result result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(
        this Result result,
        Func<Result, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<Result, TMiddle, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> SelectMany(
        this Result result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, Task> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this Result result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, Task<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result> SelectMany(
        this Result result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, ValueTask> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the selector returns a task, which the query does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SelectorTaskNotAwaitedMessage, error: true)]
    public static ValueTask<Result<TNext>> SelectMany<TNext>(
        this Result result,
        Func<Result, ValueTask<Result>> collectionSelector,
        Func<Result, Result, ValueTask<TNext>> resultSelector)
    {
        throw new NotSupportedException(SelectorTaskNotAwaitedMessage);
    }

    #endregion
}
