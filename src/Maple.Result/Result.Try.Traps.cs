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

namespace Maple.Results;

// The overload traps of Result.Try.cs: overloads that turn an unsupported call into a compile error (CS0619) whose
// message names what to use instead. They never run (#247).
public sealed partial record Result
{
    #region Not supported: functions returning a task

    // These overloads turn a call with a function that returns a task into a compile error (CS0619). Without them, an async lambda
    // converts to Action (async void: its exceptions escape Try), and a lambda returning a task binds to Try<T> with T = Task (a result
    // holding a task that is never awaited). A Task<T> converts to Task, so the Func<Task> overloads also cover most functions returning
    // Task<T>; their priority makes them win over Try(Action) and Try<T>(Func<T>). A lambda that always throws converts to any delegate
    // type, so it binds to these overloads too: pass the work as a method that can complete, or use TryAsync.
    //
    // Both mapper families need a trap for each shape. An async lambda with a block body that returns a value does not convert to
    // Func<Task>, and a ValueTask<T> does not convert to ValueTask: the untyped family catches both with Func<ValueTask<TTaskResult>>,
    // and the typed family, whose calls have two type parameters, needs Func<Task<TTaskResult>> and Func<ValueTask<TTaskResult>> of its
    // own (#191).

    private const string FunctionReturnsTaskMessage =
        "The function returns a task, which Try does not await (an async lambda would also become async void). Use TryAsync.";

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Try(Func<Task> action, Func<Exception, Error?> mapException)
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    public static Result Try(Func<ValueTask> action, Func<Exception, Error?> mapException)
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    public static Result Try<TTaskResult>(Func<ValueTask<TTaskResult>> function, Func<Exception, Error?> mapException)
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Try<TException>(Func<Task> action, Func<TException, Error?> mapException)
        where TException : Exception
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    public static Result Try<TException>(Func<ValueTask> action, Func<TException, Error?> mapException)
        where TException : Exception
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Try<TTaskResult, TException>(Func<Task<TTaskResult>> function, Func<TException, Error?> mapException)
        where TException : Exception
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    public static Result Try<TTaskResult, TException>(Func<ValueTask<TTaskResult>> function, Func<TException, Error?> mapException)
        where TException : Exception
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    #endregion
}
