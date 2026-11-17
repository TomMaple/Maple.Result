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
    //
    // A trap applies only to a call that uses the names of its parameters, and Try has two names for its first one: action, of
    // Try(Action), and function, of Try<T>(Func<T>). A call that names the argument reaches only the traps with that name (#273):
    // - function: the traps that are generic and constrained to the task type, as those of Then (#251), catch a Task, a Task<T> and a
    //   ValueTask, which would bind to Try<T> with T = the task. Their priority makes them win over it, also with explicit type
    //   arguments. A ValueTask<T> has the Func<ValueTask<TTaskResult>> function traps.
    // - action: the Func<Task> and Func<ValueTask> traps, and a Func<ValueTask<TTaskResult>> action trap for a ValueTask<T>, which
    //   converts to neither of them and would bind to Try(Action), which discards it.
    // The unused optional parameter only makes a signature differ from the overload with the same parameter types. A task is kept on
    // purpose by returning it in a result (e.g., () => Result.FromValue(SaveAsync())).

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
    public static Result Try<TTask>(Func<TTask> function, Func<Exception, Error?> mapException, Task? unused = null)
        where TTask : Task
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Try<TValueTask>(Func<TValueTask> function, Func<Exception, Error?> mapException, ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    public static Result Try<TTaskResult>(Func<ValueTask<TTaskResult>> action, Func<Exception, Error?> mapException, ValueTask unused = default)
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

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Try<TTask, TException>(Func<TTask> function, Func<TException, Error?> mapException, Task? unused = null)
        where TTask : Task
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
    public static Result Try<TValueTask, TException>(Func<TValueTask> function, Func<TException, Error?> mapException,
        ValueTask unused = default)
        where TValueTask : struct, IEquatable<ValueTask>
        where TException : Exception
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    public static Result Try<TTaskResult, TException>(Func<ValueTask<TTaskResult>> action, Func<TException, Error?> mapException,
        ValueTask unused = default)
        where TException : Exception
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    #endregion
}
