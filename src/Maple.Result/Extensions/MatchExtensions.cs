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
///     The collection of extension methods for executing actions or functions based on the state of a <see cref="Result" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of operations by specifying operations that will be executed depending on the status of the <see cref="Result" />
///     instance. These helpers centralize success/error handling logic by accepting delegates for each branch,
///     enabling concise match-like patterns across synchronous result workflows.
/// </remarks>
public static class MatchExtensions
{
    #region Result

    /// <summary>
    ///     Invokes one of the provided actions depending on whether the <paramref name="result" /> is successful.
    /// </summary>
    /// <remarks>
    ///     Use this overload when the desired outcome is a side effect, and you want to keep the existing <see cref="Result" /> instance for further chaining.
    /// </remarks>
    /// <param name="result">
    ///     The <see cref="Result" /> whose state determines which action is invoked. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessAction">
    ///     The action to execute if the <paramref name="result" /> indicates success. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action to execute if the <paramref name="result" /> indicates failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The original <paramref name="result" />, enabling fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result Match(this Result result, Action ifSuccessAction, Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess)
            ifSuccessAction();
        else
            ifErrorAction(result.Error);

        return result;
    }

    /// <summary>
    ///     Invokes one of the provided functions depending on whether the <paramref name="result" /> is successful and
    ///     returns the produced <see cref="Result" />.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> whose outcome is inspected. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function invoked when the <paramref name="result" /> represents success. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The function invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Result" /> returned by either <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns <see langword="null" />, or <paramref name="ifErrorFunction" /> returns <see langword="null" />.
    /// </exception>
    [OverloadResolutionPriority(2)]
    public static Result Match(this Result result, Func<Result> ifSuccessFunction, Func<Error, Result> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        if (result.IsSuccess)
        {
            var successResult = ifSuccessFunction();
            return Guard.EnsureNotNullReturned(successResult, nameof(ifSuccessFunction));
        }

        var errorResult = ifErrorFunction(result.Error);

        return Guard.EnsureNotNullReturned(errorResult, nameof(ifErrorFunction));
    }

    /// <summary>
    ///     Invokes one of the provided functions depending on whether the <paramref name="result" /> is successful and returns their value as
    ///     a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <typeparam name="T">The return type produced by either branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result" /> whose outcome dictates which function is invoked. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The function invoked when the <paramref name="result" /> represents success. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The function invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The value of the <typeparamref name="T"/> produced by either <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    [OverloadResolutionPriority(2)]
    public static T Match<T>(this Result result, Func<T> ifSuccessFunction, Func<Error, T> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess
            ? ifSuccessFunction()
            : ifErrorFunction(result.Error);
    }

    /// <summary>
    ///     Invokes one of the provided functions depending on whether the <paramref name="result" /> is successful and returns their output as
    ///     a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> returned by the matching branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result" /> whose state determines which function is invoked. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The function invoked when the <paramref name="result" /> represents success. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The function invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Result{T}" /> produced by either <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns <see langword="null" />, or <paramref name="ifErrorFunction" /> returns <see langword="null" />.
    /// </exception>
    [OverloadResolutionPriority(2)]
    public static Result<T> Match<T>(this Result result, Func<Result<T>> ifSuccessFunction, Func<Error, Result<T>> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        if (result.IsSuccess)
        {
            var successResult = ifSuccessFunction();
            return Guard.EnsureNotNullReturned(successResult, nameof(ifSuccessFunction));
        }

        var errorResult = ifErrorFunction(result.Error);

        return Guard.EnsureNotNullReturned(errorResult, nameof(ifErrorFunction));
    }

    #endregion

    #region Result<T>

    /// <summary>
    ///     Invokes one of the provided actions depending on whether the <paramref name="result" /> is successful.
    /// </summary>
    /// <remarks>
    ///     Use this overload when the desired outcome is a side effect,
    ///     and you want to keep the existing <see cref="Result{T}" /> instance for further chaining.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the <see cref="Result{T}" />.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome determines which action is invoked. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessAction">
    ///     The action to execute when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action to execute when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The original <paramref name="result" /> for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<T> Match<T>(this Result<T> result, Action<T> ifSuccessAction, Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess)
            ifSuccessAction(result.Value);
        else
            ifErrorAction(result.Error);

        return result;
    }

    /// <summary>
    ///     Invokes one of the provided functions depending on whether the <paramref name="result" /> is successful and returns their output as
    ///     a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type produced by either branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the function to invoke. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The function executed when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The function executed when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The value of the <typeparamref name="TNext"/> type returned by either <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    [OverloadResolutionPriority(2)]
    public static TNext Match<T, TNext>(this Result<T> result, Func<T, TNext> ifSuccessFunction, Func<Error, TNext> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess
            ? ifSuccessFunction(result.Value)
            : ifErrorFunction(result.Error);
    }

    /// <summary>
    ///     Executes one of the provided functions depending on whether the <paramref name="result" /> is successful and returns their output as
    ///     a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type of the <see cref="Result{TNext}" /> returned by the invoked function.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the function to invoke. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The function executed when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The function executed when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Result{TNext}" /> returned by either <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns <see langword="null" />, or <paramref name="ifErrorFunction" /> returns <see langword="null" />.
    /// </exception>
    [OverloadResolutionPriority(2)]
    public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, Result<TNext>> ifSuccessFunction, Func<Error, Result<TNext>> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        if (result.IsSuccess)
        {
            var successResult = ifSuccessFunction(result.Value);
            return Guard.EnsureNotNullReturned(successResult, nameof(ifSuccessFunction));
        }

        var errorResult = ifErrorFunction(result.Error);

        return Guard.EnsureNotNullReturned(errorResult, nameof(ifErrorFunction));
    }

    #endregion

    #region Not supported: functions returning a task

    // These overloads turn a call with a function that returns a task into a compile error (CS0619). Without them, the call binds to the
    // generic overload, which wraps the task in a successful result without awaiting it (issue #119). They are more specific than the
    // generic overloads, so they are chosen first; an explicit type argument excludes them and selects the generic overload on purpose.
    // The Task overloads have a higher priority than the ValueTask ones: an async lambda converts to both, which would be ambiguous (CS0121,
    // issue #123). The overloads in which both functions return a value have a higher priority still, so that
    // Match(() => SaveAsync(), e => LogAsync(e)) keeps returning the task.

    private const string TaskNotAwaitedMessage =
        "The function returns a task, which Match does not await. Use MatchAsync, or specify the type arguments " +
        "(e.g., Match<Task>(…) or Match<T, Task>(…)) to keep the task.";

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Match(this Result result, Func<Task> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Match<TNext>(this Result result, Func<Task<TNext>> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    public static Result Match(this Result result, Func<ValueTask> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    public static Result Match<TNext>(this Result result, Func<ValueTask<TNext>> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Match<T>(this Result<T> result, Func<T, Task> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Match<T, TNext>(this Result<T> result, Func<T, Task<TNext>> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    public static Result Match<T>(this Result<T> result, Func<T, ValueTask> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    public static Result Match<T, TNext>(this Result<T> result, Func<T, ValueTask<TNext>> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    #endregion

    #region Not supported: error actions returning a task

    // These overloads turn a call with an error action that returns a task into a compile error (CS0619). Without them, a lambda
    // that returns a task converts to Action<Error>, and the task is never awaited (an async lambda becomes async void, issue #123).
    // A lambda whose body has a value prefers a delegate with a return type, so these overloads win over the Action<Error> ones.
    // A Task<T> converts to Task, so the Task overloads also cover functions returning Task<T>. The priorities are explained above.

    private const string ErrorActionTaskNotAwaitedMessage =
        "The error action returns a task, which Match does not await. Use MatchAsync, or discard the task explicitly in a statement " +
        "lambda (e.g., e => { _ = LogAsync(e); }).";

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Match(this Result result, Action ifSuccessAction, Func<Error, Task> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result Match(this Result result, Action ifSuccessAction, Func<Error, ValueTask> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result Match<TTaskResult>(this Result result, Action ifSuccessAction, Func<Error, ValueTask<TTaskResult>> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Match(this Result result, Func<Result> ifSuccessFunction, Func<Error, Task> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result Match(this Result result, Func<Result> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result Match<TTaskResult>(this Result result, Func<Result> ifSuccessFunction, Func<Error, ValueTask<TTaskResult>> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<T> Match<T>(this Result result, Func<T> ifSuccessFunction, Func<Error, Task> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result<T> Match<T>(this Result result, Func<T> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result<T> Match<T, TTaskResult>(this Result result, Func<T> ifSuccessFunction, Func<Error, ValueTask<TTaskResult>> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Match(this Result result, Func<Error> ifSuccessFunction, Func<Error, Task> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result Match(this Result result, Func<Error> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result Match<TTaskResult>(this Result result, Func<Error> ifSuccessFunction, Func<Error, ValueTask<TTaskResult>> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<T> Match<T>(this Result result, Func<Result<T>> ifSuccessFunction, Func<Error, Task> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result<T> Match<T>(this Result result, Func<Result<T>> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result<T> Match<T, TTaskResult>(this Result result, Func<Result<T>> ifSuccessFunction, Func<Error, ValueTask<TTaskResult>> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<T> Match<T>(this Result<T> result, Action<T> ifSuccessAction, Func<Error, Task> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result<T> Match<T>(this Result<T> result, Action<T> ifSuccessAction, Func<Error, ValueTask> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result<T> Match<T, TTaskResult>(this Result<T> result, Action<T> ifSuccessAction, Func<Error, ValueTask<TTaskResult>> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, TNext> ifSuccessFunction, Func<Error, Task> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, TNext> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result<TNext> Match<T, TNext, TTaskResult>(
        this Result<T> result,
        Func<T, TNext> ifSuccessFunction,
        Func<Error, ValueTask<TTaskResult>> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Match<T>(this Result<T> result, Func<T, Error> ifSuccessFunction, Func<Error, Task> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result Match<T>(this Result<T> result, Func<T, Error> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result Match<T, TTaskResult>(this Result<T> result, Func<T, Error> ifSuccessFunction, Func<Error, ValueTask<TTaskResult>> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, Result<TNext>> ifSuccessFunction, Func<Error, Task> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, Result<TNext>> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the error action returns a task, which <c>Match</c> does not await; use <c>MatchAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ErrorActionTaskNotAwaitedMessage, error: true)]
    public static Result<TNext> Match<T, TNext, TTaskResult>(
        this Result<T> result,
        Func<T, Result<TNext>> ifSuccessFunction,
        Func<Error, ValueTask<TTaskResult>> ifErrorAction)
    {
        throw new NotSupportedException(ErrorActionTaskNotAwaitedMessage);
    }

    #endregion

    #region Not supported: a success function and an error action (#227)

    // A success function with an error action transforms the result, as Then(…).IfError(…) does, and the same call with a
    // success action taps it: which one runs depended on the return type of the lambda (e.g., x => set.Add(x) mapped to a
    // Result<bool>), as for IfSuccess before #197. These overloads keep the signatures and the priorities of the overloads
    // they replace, so no other call changes its binding, and turn such a call into a compile error (CS0619) that names
    // Then(…).IfError(…). Match keeps two meanings: two functions (a value) and two actions (a side effect).

    private const string SuccessFunctionAndErrorActionMessage =
        "Match with a success function and an error action transforms the result, as Then does. Use Then(…).IfError(…) " +
        "to transform it, a statement lambda (e.g., x => { list.Add(x); }) for a side effect, or two functions that " +
        "return the same type for a value.";

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>Then(…).IfError(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static Result Match(this Result result, Func<Result> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>Then(…).IfError(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static Result<T> Match<T>(this Result result, Func<T> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>Then(…).IfError(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static Result Match(this Result result, Func<Error> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>Then(…).IfError(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static Result<T> Match<T>(this Result result, Func<Result<T>> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>Then(…).IfError(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, TNext> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>Then(…).IfError(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static Result Match<T>(this Result<T> result, Func<T, Error> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>Then(…).IfError(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, Result<TNext>> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    #endregion
}
