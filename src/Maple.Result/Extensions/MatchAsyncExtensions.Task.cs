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
///     The collection of extension methods that await a task of a <see cref="Result" /> or <see cref="Result{T}" /> and fold it into one value:
///     a function for the success and a function for the error, which return the same type.
/// </summary>
/// <remarks>
///     The functions are both synchronous, or both return a <see cref="Task" /> or a <see cref="Task{TResult}" />, which is awaited.
///     <c>MatchAsync</c> returns what the function that ran returns, and does not look at it. There is no <c>MatchAsync</c> on a result:
///     <c>Match</c> returns the task of the function that ran. For two side effects, use <c>SwitchAsync</c>; two actions are a compile error
///     here (CS0619).
///     <para>
///         The methods for a <see cref="System.Threading.Tasks.Task" /> and for a <see cref="System.Threading.Tasks.ValueTask" /> of a result
///         are in this one class; the kind of the task is kept, and functions that return the other kind of task are awaited.
///     </para>
/// </remarks>
public static partial class MatchAsyncExtensions
{
    #region Task<Result>

    /// <summary>
    ///     Awaits the <paramref name="resultTask" />, invokes one of the provided functions depending on whether the result is successful and
    ///     returns the value that the invoked function returns.
    /// </summary>
    /// <remarks>
    ///     The functions are synchronous: they run after the <paramref name="resultTask" /> completes. The value is returned as the function
    ///     returns it, <see langword="null" /> included.
    /// </remarks>
    /// <typeparam name="TOut">The type that both functions return.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to fold. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function invoked if the result is successful. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The function invoked if the result is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed functions, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The task of the value that <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" /> returns.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task<TOut> MatchAsync<TOut>(this Task<Result> resultTask, Func<TOut> ifSuccessFunction, Func<Error, TOut> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TOut> Core(Task<Result> resultTask, Func<TOut> ifSuccessFunction, Func<Error, TOut> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorFunction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" />, invokes one of the provided asynchronous functions depending on whether the result is
    ///     successful, awaits it and returns the value that it returns.
    /// </summary>
    /// <remarks>
    ///     The value is returned as the function returns it, <see langword="null" /> included.
    /// </remarks>
    /// <typeparam name="TOut">The type of the value of the tasks that both functions return.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to fold. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked if the result is successful. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the result is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed functions when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The task of the value that <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" /> returns.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task,
    ///     or <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<TOut> MatchAsync<TOut>(this Task<Result> resultTask, Func<Task<TOut>> ifSuccessFunction,
        Func<Error, Task<TOut>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TOut> Core(Task<Result> resultTask, Func<Task<TOut>> ifSuccessFunction, Func<Error, Task<TOut>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullTask(successTask, nameof(ifSuccessFunction)).ConfigureAwait(continueOnCapturedContext);
            }

            var errorTask = ifErrorFunction(result.Error);

            return await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorFunction)).ConfigureAwait(continueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" />, invokes one of the provided asynchronous actions depending on whether the result is
    ///     successful, and awaits it.
    /// </summary>
    /// <param name="resultTask">The asynchronous operation that returns the result to fold. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action invoked if the result is successful. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked if the result is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed actions when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The task that completes when the invoked action completes.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessAction" /> returns a <see langword="null" /> task,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task MatchAsync(this Task<Result> resultTask, Func<Task> ifSuccessAction, Func<Error, Task> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task Core(Task<Result> resultTask, Func<Task> ifSuccessAction, Func<Error, Task> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessAction();
                await Guard.EnsureNotNullTask(successTask, nameof(ifSuccessAction)).ConfigureAwait(continueOnCapturedContext);
            }
            else
            {
                var errorTask = ifErrorAction(result.Error);
                await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);
            }
        }
    }

    #endregion

    #region Task<Result<T>>

    /// <summary>
    ///     Awaits the <paramref name="resultTask" />, invokes one of the provided functions depending on whether the result is successful and
    ///     returns the value that the invoked function returns.
    /// </summary>
    /// <remarks>
    ///     The functions are synchronous: they run after the <paramref name="resultTask" /> completes. The value is returned as the function
    ///     returns it, <see langword="null" /> included.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <typeparam name="TOut">The type that both functions return.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to fold. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function invoked if the result is successful, with its value. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The function invoked if the result is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed functions, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The task of the value that <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" /> returns.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task<TOut> MatchAsync<T, TOut>(this Task<Result<T>> resultTask, Func<T, TOut> ifSuccessFunction,
        Func<Error, TOut> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TOut> Core(Task<Result<T>> resultTask, Func<T, TOut> ifSuccessFunction, Func<Error, TOut> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorFunction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" />, invokes one of the provided asynchronous functions depending on whether the result is
    ///     successful, awaits it and returns the value that it returns.
    /// </summary>
    /// <remarks>
    ///     The value is returned as the function returns it, <see langword="null" /> included.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <typeparam name="TOut">The type of the value of the tasks that both functions return.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to fold. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked if the result is successful, with its value. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the result is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed functions when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The task of the value that <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" /> returns.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task,
    ///     or <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<TOut> MatchAsync<T, TOut>(this Task<Result<T>> resultTask, Func<T, Task<TOut>> ifSuccessFunction,
        Func<Error, Task<TOut>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TOut> Core(Task<Result<T>> resultTask, Func<T, Task<TOut>> ifSuccessFunction, Func<Error, Task<TOut>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                return await Guard.EnsureNotNullTask(successTask, nameof(ifSuccessFunction)).ConfigureAwait(continueOnCapturedContext);
            }

            var errorTask = ifErrorFunction(result.Error);

            return await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorFunction)).ConfigureAwait(continueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" />, invokes one of the provided asynchronous actions depending on whether the result is
    ///     successful, and awaits it.
    /// </summary>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to fold. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action invoked if the result is successful, with its value. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked if the result is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed actions when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The task that completes when the invoked action completes.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessAction" /> returns a <see langword="null" /> task,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task MatchAsync<T>(this Task<Result<T>> resultTask, Func<T, Task> ifSuccessAction, Func<Error, Task> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task Core(Task<Result<T>> resultTask, Func<T, Task> ifSuccessAction, Func<Error, Task> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessAction(result.Value);
                await Guard.EnsureNotNullTask(successTask, nameof(ifSuccessAction)).ConfigureAwait(continueOnCapturedContext);
            }
            else
            {
                var errorTask = ifErrorAction(result.Error);
                await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);
            }
        }
    }

    #endregion

    #region Not supported: two actions

    // MatchAsync is a fold: two functions that return one type. These overloads turn a call with two actions into a compile error (CS0619)
    // that names SwitchAsync. They return what the MatchAsync with two actions of 2.x returned, so that its call reports this error only.
    // A call that the fold accepts binds to the fold: a lambda with a value converts better to a function than to an action, and the
    // overloads with functions that return a task have priority 1.
    // See issue #255.

    private const string TwoActionsMessage =
        "MatchAsync takes two functions that return a value. Use SwitchAsync for two side effects, or ThenAsync(…).IfErrorAsync(…) to " +
        "transform the value and handle the error.";

    /// <summary>
    ///     Not supported: <c>MatchAsync</c> takes two functions that return a value; use <c>SwitchAsync</c> for two actions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TwoActionsMessage, error: true)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask, Action ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(TwoActionsMessage);
    }

    /// <summary>
    ///     Not supported: <c>MatchAsync</c> takes two functions that return a value; use <c>SwitchAsync</c> for two actions.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TwoActionsMessage, error: true)]
    public static Task<Result<T>> MatchAsync<T>(this Task<Result<T>> resultTask, Action<T> ifSuccessAction, Action<Error> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(TwoActionsMessage);
    }

    #endregion

    #region Task results with functions returning a ValueTask

    // See issue #226.

    /// <summary>
    ///     Awaits the <paramref name="resultTask" />, invokes one of the provided asynchronous functions depending on whether the result is
    ///     successful, awaits it and returns the value that it returns.
    /// </summary>
    /// <remarks>
    ///     The value is returned as the function returns it, <see langword="null" /> included.
    ///     <para>
    ///         The function returns a <see cref="ValueTask" />, the other kind of task: it is awaited, and a <see cref="Task" />
    ///         is returned, as for a function that returns a <see cref="Task" />.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TOut">The type of the value of the tasks that both functions return.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to fold. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked if the result is successful. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the result is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed functions when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The task of the value that <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" /> returns.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task<TOut> MatchAsync<TOut>(this Task<Result> resultTask, Func<ValueTask<TOut>> ifSuccessFunction,
        Func<Error, ValueTask<TOut>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TOut> Core(Task<Result> resultTask, Func<ValueTask<TOut>> ifSuccessFunction, Func<Error, ValueTask<TOut>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await successTask.ConfigureAwait(continueOnCapturedContext);
            }

            var errorTask = ifErrorFunction(result.Error);

            return await errorTask.ConfigureAwait(continueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" />, invokes one of the provided asynchronous actions depending on whether the result is
    ///     successful, and awaits it.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The function returns a <see cref="ValueTask" />, the other kind of task: it is awaited, and a <see cref="Task" />
    ///         is returned, as for a function that returns a <see cref="Task" />.
    ///     </para>
    /// </remarks>
    /// <param name="resultTask">The asynchronous operation that returns the result to fold. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action invoked if the result is successful. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked if the result is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed actions when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The task that completes when the invoked action completes.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task MatchAsync(this Task<Result> resultTask, Func<ValueTask> ifSuccessAction, Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task Core(Task<Result> resultTask, Func<ValueTask> ifSuccessAction, Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessAction();
                await successTask.ConfigureAwait(continueOnCapturedContext);
            }
            else
            {
                var errorTask = ifErrorAction(result.Error);
                await errorTask.ConfigureAwait(continueOnCapturedContext);
            }
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" />, invokes one of the provided asynchronous functions depending on whether the result is
    ///     successful, awaits it and returns the value that it returns.
    /// </summary>
    /// <remarks>
    ///     The value is returned as the function returns it, <see langword="null" /> included.
    ///     <para>
    ///         The function returns a <see cref="ValueTask" />, the other kind of task: it is awaited, and a <see cref="Task" />
    ///         is returned, as for a function that returns a <see cref="Task" />.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <typeparam name="TOut">The type of the value of the tasks that both functions return.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to fold. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked if the result is successful, with its value. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the result is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed functions when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The task of the value that <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" /> returns.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task<TOut> MatchAsync<T, TOut>(this Task<Result<T>> resultTask, Func<T, ValueTask<TOut>> ifSuccessFunction,
        Func<Error, ValueTask<TOut>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TOut> Core(Task<Result<T>> resultTask, Func<T, ValueTask<TOut>> ifSuccessFunction, Func<Error, ValueTask<TOut>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                return await successTask.ConfigureAwait(continueOnCapturedContext);
            }

            var errorTask = ifErrorFunction(result.Error);

            return await errorTask.ConfigureAwait(continueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" />, invokes one of the provided asynchronous actions depending on whether the result is
    ///     successful, and awaits it.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The function returns a <see cref="ValueTask" />, the other kind of task: it is awaited, and a <see cref="Task" />
    ///         is returned, as for a function that returns a <see cref="Task" />.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to fold. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action invoked if the result is successful, with its value. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked if the result is failed, with its <see cref="Error" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed actions when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The task that completes when the invoked action completes.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task MatchAsync<T>(this Task<Result<T>> resultTask, Func<T, ValueTask> ifSuccessAction, Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task Core(Task<Result<T>> resultTask, Func<T, ValueTask> ifSuccessAction, Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessAction(result.Value);
                await successTask.ConfigureAwait(continueOnCapturedContext);
            }
            else
            {
                var errorTask = ifErrorAction(result.Error);
                await errorTask.ConfigureAwait(continueOnCapturedContext);
            }
        }
    }

    #endregion

}
