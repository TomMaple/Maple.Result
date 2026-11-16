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
///     The collection of extension methods for executing asynchronous actions or functions based on the state of a <see cref="Result" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of operations by specifying operations that will be executed depending on the status of the <see cref="Result" />
///     instance. These helpers centralize success/error handling logic by accepting delegates for each branch,
///     enabling concise match-like patterns across synchronous result workflows.
/// </remarks>
public static class MatchTaskAsyncExtensions
{
    #region Result

    /// <summary>
    ///     Invokes one of the provided asynchronous actions depending on whether the <paramref name="result" /> is successful.
    /// </summary>
    /// <remarks>
    ///     Use this overload when the desired outcome is a side effect, and you want to keep the existing <see cref="Result" /> instance for further chaining.
    /// </remarks>
    /// <param name="result">
    ///     The <see cref="Result" /> whose state determines which action is invoked. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="result" /> indicates success. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action to execute if the <paramref name="result" /> indicates failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that represents the original <paramref name="result" />, enabling fluent chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessAction" /> returns a <see langword="null" /> task,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    public static Task<Result> MatchAsync(this Result result, Func<Task> ifSuccessAction,
        Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Result result, Func<Task> ifSuccessAction, Func<Error, Task> ifErrorAction, bool continueOnCapturedContext)
        {
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

            return result;
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its <see cref="Result" /> output if the <paramref name="result" /> is successful,
    ///     or the provided asynchronous action and returns <see cref="Result" /> with the original error.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> whose outcome is inspected. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents success. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{Result}" /> that represents the <see cref="Result" /> returned by either <paramref name="ifSuccessFunction" />
    ///     if <paramref name="result" /> is successful, or the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    public static Task<Result> MatchAsync(this Result result, Func<Task<Result>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Result result, Func<Task<Result>> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            var errorTask = ifErrorAction(result.Error);
            await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="result" /> is successful and
    ///     returns the produced <see cref="Result" />.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> whose outcome is inspected. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents success. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{Result}" /> which represents the <see cref="Result" /> returned by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    public static Task<Result> MatchAsync(this Result result, Func<Task<Result>> ifSuccessFunction,
        Func<Error, Task<Result>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(
            Result result,
            Func<Task<Result>> ifSuccessFunction,
            Func<Error, Task<Result>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            var errorTask = ifErrorFunction(result.Error);

            return await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as a <see cref="Result{T}" /> instance if the <paramref name="result" /> is
    ///     successful, or the provided asynchronous action and returns <see cref="Result{T}" /> with the original error.
    /// </summary>
    /// <typeparam name="T">The return type produced by the successful branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result" /> whose outcome dictates which function is invoked. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents success. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{Result}" /> that represents the <see cref="Result{T}" /> with the value returned by either
    ///     the <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful,
    ///     or the <see cref="Result{T}" /> with the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    public static Task<Result<T>> MatchAsync<T>(this Result result, Func<Task<T>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Result result, Func<Task<T>> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullTask(successTask, nameof(ifSuccessFunction)).ConfigureAwait(continueOnCapturedContext);
            }

            var errorTask = ifErrorAction(result.Error);
            await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function if the <paramref name="result" /> is successful and returns a failed <see cref="Result" /> with
    ///     the <see cref="Error" /> it returns; otherwise, invokes the provided asynchronous action and returns the original <paramref name="result" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use it to turn a successful <see cref="Result" /> into a failure, e.g.,
    ///         <c>result.MatchAsync(async () => Error.Conflict(…), e => LogAsync(e))</c>. The returned <see cref="Result" /> is always failed.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a call would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///         To get that on purpose, specify the type argument: <c>MatchAsync&lt;Error&gt;(…)</c>.
    ///     </para>
    /// </remarks>
    /// <param name="result">
    ///     The <see cref="Result" /> whose outcome dictates which delegate is invoked. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents success.
    ///     It returns the <see cref="Error" /> of the failed <see cref="Result" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorAction" /> parameters are
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    public static Task<Result> MatchAsync(this Result result, Func<Task<Error>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Result result, Func<Task<Error>> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                var error = await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);

                return Result.FromError(error);
            }

            var errorTask = ifErrorAction(result.Error);
            await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="result" /> is successful and returns their value as
    ///     a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <typeparam name="T">The return type produced by either branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result" /> whose outcome dictates which function is invoked. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents success. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{T}" /> that represents the value of type<typeparamref name="T" /> produced by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task,
    ///     or <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task.
    /// </exception>
    public static Task<T> MatchAsync<T>(this Result result, Func<Task<T>> ifSuccessFunction,
        Func<Error, Task<T>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<T> Core(Result result, Func<Task<T>> ifSuccessFunction, Func<Error, Task<T>> ifErrorFunction, bool continueOnCapturedContext)
        {
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
    ///     Invokes the provided asynchronous function and returns its <see cref="Result{T}" /> output if the <paramref name="result" /> is successful,
    ///     or the provided asynchronous action and returns <see cref="Result{T}" /> with the original error.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> returned by the matching branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result" /> whose state determines which function is invoked. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents success. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{T}" /> returned by the <paramref name="ifSuccessFunction" />
    ///     if <paramref name="result" /> is successful,
    ///     or the <see cref="Result{T}" /> with the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    public static Task<Result<T>> MatchAsync<T>(this Result result, Func<Task<Result<T>>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(
            Result result,
            Func<Task<Result<T>>> ifSuccessFunction,
            Func<Error, Task> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            var errorTask = ifErrorAction(result.Error);
            await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="result" /> is successful and returns their output as
    ///     a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> returned by the matching branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result" /> whose state determines which function is invoked. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents success. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{T}" /> produced by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    public static Task<Result<T>> MatchAsync<T>(this Result result, Func<Task<Result<T>>> ifSuccessFunction,
        Func<Error, Task<Result<T>>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(
            Result result,
            Func<Task<Result<T>>> ifSuccessFunction,
            Func<Error, Task<Result<T>>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            var errorTask = ifErrorFunction(result.Error);

            return await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    #endregion

    #region Result<T>

    /// <summary>
    ///     Invokes one of the provided asynchronous actions depending on whether the <paramref name="result" /> is successful.
    /// </summary>
    /// <remarks>
    ///     Use this overload when the desired outcome is a side effect,
    ///     and you want to keep the existing <see cref="Result{T}" /> instance for further chaining.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the <see cref="Result{T}" />.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome determines which asynchronous action is invoked. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action to execute when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the original <paramref name="result" /> for fluent chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessAction" /> returns a <see langword="null" /> task,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    public static Task<Result<T>> MatchAsync<T>(this Result<T> result, Func<T, Task> ifSuccessAction,
        Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Result<T> result, Func<T, Task> ifSuccessAction, Func<Error, Task> ifErrorAction, bool continueOnCapturedContext)
        {
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

            return result;
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as a <see cref="Result{TNext}" /> instance if the <paramref name="result" /> is
    ///     successful, or the provided asynchronous action and returns <see cref="Result{TNext}" /> with the original error.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type produced by either branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the function to invoke. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function executed when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{TNext}" /> with the value returned by <paramref name="ifSuccessFunction" />
    ///     if <paramref name="result" /> is successful,
    ///     or the <see cref="Result{TNext}" /> with the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    public static Task<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, Task<TNext>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(
            Result<T> result,
            Func<T, Task<TNext>> ifSuccessFunction,
            Func<Error, Task> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                var nextTask = Guard.EnsureNotNullTask(successTask, nameof(ifSuccessFunction));

                return await nextTask.ConfigureAwait(continueOnCapturedContext);
            }

            var errorTask = ifErrorAction(result.Error);
            await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function if the <paramref name="result" /> is successful and returns a failed <see cref="Result" /> with
    ///     the <see cref="Error" /> it returns; otherwise, invokes the provided asynchronous action and returns a failed <see cref="Result" /> with
    ///     the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use it to turn a successful <see cref="Result{T}" /> into a failure, e.g.,
    ///         <c>result.MatchAsync(async user => Error.Conflict(…), e => LogAsync(e))</c>. The returned <see cref="Result" /> is always failed.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a call would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///         To get that on purpose, specify the type arguments: <c>MatchAsync&lt;T, Error&gt;(…)</c>.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the delegate to invoke. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function executed when the <paramref name="result" /> represents success. The value of the successful
    ///     <see cref="Result{T}" /> is passed as a parameter. It returns the <see cref="Error" /> of the failed <see cref="Result" />.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result" /> with the original <see cref="Error" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorAction" /> parameters are
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    public static Task<Result> MatchAsync<T>(
        this Result<T> result,
        Func<T, Task<Error>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(
            Result<T> result,
            Func<T, Task<Error>> ifSuccessFunction,
            Func<Error, Task> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                var error = await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);

                return Result.FromError(error);
            }

            var errorTask = ifErrorAction(result.Error);
            await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="result" /> is successful and returns their output as
    ///     a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type produced by either branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the function to invoke. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function executed when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function executed when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TNext}" /> that represents the value of the <typeparamref name="TNext" /> type returned by either
    ///     <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task,
    ///     or <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task.
    /// </exception>
    public static Task<TNext> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, Task<TNext>> ifSuccessFunction,
        Func<Error, Task<TNext>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TNext> Core(
            Result<T> result,
            Func<T, Task<TNext>> ifSuccessFunction,
            Func<Error, Task<TNext>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
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
    ///     Invokes the provided asynchronous function and returns its <see cref="Result{TNext}" /> output if the <paramref name="result" /> is successful,
    ///     or the provided asynchronous action and returns <see cref="Result{TNext}" /> with the original error.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type of the <see cref="Result{TNext}" /> returned by the invoked function.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the function to invoke. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function executed when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{TNext}" /> returned by <paramref name="ifSuccessFunction" />
    ///     if the <paramref name="result" /> is successful,
    ///     or the <see cref="Result{TNext}" /> with the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    public static Task<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, Task<Result<TNext>>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(
            Result<T> result,
            Func<T, Task<Result<TNext>>> ifSuccessFunction,
            Func<Error, Task> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            var errorTask = ifErrorAction(result.Error);
            await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);

            return result.Error;
        }
    }

    /// <summary>
    ///     Executes one of the provided asynchronous functions depending on whether the <paramref name="result" /> is successful and returns their output as
    ///     a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type of the <see cref="Result{TNext}" /> returned by the invoked function.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the function to invoke. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function executed when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function executed when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{TNext}" /> returned by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    public static Task<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, Task<Result<TNext>>> ifSuccessFunction,
        Func<Error, Task<Result<TNext>>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(
            Result<T> result,
            Func<T, Task<Result<TNext>>> ifSuccessFunction,
            Func<Error, Task<Result<TNext>>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            var errorTask = ifErrorFunction(result.Error);

            return await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    #endregion

    #region Task<Result>

    /// <summary>
    ///     Invokes one of the provided asynchronous actions depending on whether the <paramref name="resultTask" /> represents a successful operation.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use this overload when the desired outcome is a side effect,
    ///         and you want to keep the existing <see cref="Result" /> instance for further chaining.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    ///     </para>
    /// </remarks>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> indicates a successful operation. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> indicates a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that represents the original <paramref name="resultTask" />, enabling fluent chaining.
    /// </returns>
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
    public static Task<Result> MatchAsync(this Task<Result> resultTask, Func<Task> ifSuccessAction,
        Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result> resultTask, Func<Task> ifSuccessAction, Func<Error, Task> ifErrorAction, bool continueOnCapturedContext)
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

            return result;
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its <see cref="Result" /> output as an asynchronous operation,
    ///     if the <paramref name="resultTask" /> represents a successful operation,
    ///     or the provided asynchronous action and returns <see cref="Result" /> with the original error as an asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    /// </remarks>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{Result}" /> that represents the <see cref="Result" /> returned by either <paramref name="ifSuccessFunction" />
    ///     if <paramref name="resultTask" /> is a successful operation, or the original <see cref="Result" />,
    ///     the outcome of the <paramref name="resultTask" /> operation, after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask, Func<Task<Result>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(
            Task<Result> resultTask,
            Func<Task<Result>> ifSuccessFunction,
            Func<Error, Task> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            var errorTask = ifErrorAction(result.Error);
            await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and
    ///     returns the produced <see cref="Result" />.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a successful operation. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a failed operation. The <see cref="Error" /> associated with
    ///     the <paramref name="resultTask" /> operation outcome is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{Result}" /> which represents the <see cref="Result" /> returned by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask, Func<Task<Result>> ifSuccessFunction,
        Func<Error, Task<Result>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(
            Task<Result> resultTask,
            Func<Task<Result>> ifSuccessFunction,
            Func<Error, Task<Result>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            var errorTask = ifErrorFunction(result.Error);

            return await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as an asynchronous operation that returns a <see cref="Result{T}" /> instance
    ///     if the <paramref name="resultTask" /> is a successful operation,
    ///     or the provided asynchronous action and returns <see cref="Result{T}" /> with the original error as an asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The return type produced by the successful branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{Result}" /> that represents the <see cref="Result{T}" /> with the value returned by either
    ///     the <paramref name="ifSuccessFunction" /> if <paramref name="resultTask" /> is a successful operation,
    ///     or the original <see cref="Result{T}" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> MatchAsync<T>(this Task<Result> resultTask, Func<Task<T>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(
            Task<Result> resultTask,
            Func<Task<T>> ifSuccessFunction,
            Func<Error, Task> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullTask(successTask, nameof(ifSuccessFunction)).ConfigureAwait(continueOnCapturedContext);
            }

            var errorTask = ifErrorAction(result.Error);
            await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function if the <paramref name="resultTask" /> is a successful operation and returns a failed
    ///     <see cref="Result" /> with the <see cref="Error" /> it returns; otherwise, invokes the provided asynchronous action and returns
    ///     the original result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use it to turn a successful result into a failure, e.g.,
    ///         <c>resultTask.MatchAsync(async () => Error.Conflict(…), e => LogAsync(e))</c>. The returned <see cref="Result" /> is always failed.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a call would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///         To get that on purpose, specify the type argument: <c>MatchAsync&lt;Error&gt;(…)</c>.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation.
    ///     </para>
    /// </remarks>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     It returns the <see cref="Error" /> of the failed <see cref="Result" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="ifSuccessFunction" /> if the <paramref name="resultTask" /> operation is successful;
    ///     otherwise, the original result after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorAction" /> parameters are
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask, Func<Task<Error>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(
            Task<Result> resultTask,
            Func<Task<Error>> ifSuccessFunction,
            Func<Error, Task> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                var error = await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);

                return Result.FromError(error);
            }

            var errorTask = ifErrorAction(result.Error);
            await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and
    ///     returns their value as an asynchronous operation that returns a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The return type produced by either branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a failed operation. The <see cref="Error" /> associated with
    ///     the <paramref name="resultTask" /> operation outcome is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{T}" /> that represents the value of type<typeparamref name="T" /> produced by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
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
    public static Task<T> MatchAsync<T>(this Task<Result> resultTask, Func<Task<T>> ifSuccessFunction,
        Func<Error, Task<T>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<T> Core(
            Task<Result> resultTask,
            Func<Task<T>> ifSuccessFunction,
            Func<Error, Task<T>> ifErrorFunction,
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
    ///     Invokes the provided asynchronous function and returns its <see cref="Result{T}" /> output as an asynchronous operation
    ///     if the <paramref name="resultTask" /> is a successful operation,
    ///     or the provided asynchronous action and returns <see cref="Result{T}" /> with the original error as an asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> returned by the matching branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{T}" /> returned by the <paramref name="ifSuccessFunction" />
    ///     if <paramref name="resultTask" /> is a successful operation, or the <see cref="Result{T}" /> with the original outcome of
    ///     the <paramref name="resultTask" /> operation after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> MatchAsync<T>(
        this Task<Result> resultTask, Func<Task<Result<T>>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(
            Task<Result> resultTask,
            Func<Task<Result<T>>> ifSuccessFunction,
            Func<Error, Task> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            var errorTask = ifErrorAction(result.Error);
            await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and
    ///     returns their output as a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> returned by the matching branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a failed operation. The <see cref="Error" /> associated with
    ///     the <paramref name="resultTask" /> operation outcome is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{T}" /> produced by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> MatchAsync<T>(
        this Task<Result> resultTask, Func<Task<Result<T>>> ifSuccessFunction,
        Func<Error, Task<Result<T>>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(
            Task<Result> resultTask,
            Func<Task<Result<T>>> ifSuccessFunction,
            Func<Error, Task<Result<T>>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            var errorTask = ifErrorFunction(result.Error);

            return await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    #endregion

    #region Task<Result<T>>

    /// <summary>
    ///     Invokes one of the provided asynchronous actions depending on whether the <paramref name="resultTask" /> is a successful operation.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use this overload when the desired outcome is a side effect,
    ///         and you want to keep the existing <see cref="Result{T}" /> instance for further chaining.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the <see cref="Result{T}" />.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation, which returns <see cref="Result{T}" />,
    ///     to be executed and which result is to be inspected to select which function to invoke. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> indicates a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> indicates a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the outcome of the <paramref name="resultTask" /> for fluent chaining.
    /// </returns>
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
    public static Task<Result<T>> MatchAsync<T>(this Task<Result<T>> resultTask, Func<T, Task> ifSuccessAction,
        Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(
            Task<Result<T>> resultTask,
            Func<T, Task> ifSuccessAction,
            Func<Error, Task> ifErrorAction,
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

            return result;
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as a <see cref="Result{TNext}" /> instance if the <paramref name="resultTask" /> is
    ///     a successful operation, or the provided asynchronous action and returns <see cref="Result{TNext}" /> with the original error.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type produced by either branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation, which returns <see cref="Result{T}" />,
    ///     to be executed and which result is to be inspected to select which function to invoke. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{TNext}" /> with the value returned by <paramref name="ifSuccessFunction" />
    ///     if <paramref name="resultTask" /> is a successful operation, or the <see cref="Result{TNext}" /> with the outcome of the original
    ///     <paramref name="resultTask" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<TNext>> MatchAsync<T, TNext>(
        this Task<Result<T>> resultTask,
        Func<T, Task<TNext>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(
            Task<Result<T>> resultTask,
            Func<T, Task<TNext>> ifSuccessFunction,
            Func<Error, Task> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                var nextTask = Guard.EnsureNotNullTask(successTask, nameof(ifSuccessFunction));

                return await nextTask.ConfigureAwait(continueOnCapturedContext);
            }

            var errorTask = ifErrorAction(result.Error);
            await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function if the <paramref name="resultTask" /> is a successful operation and returns a failed
    ///     <see cref="Result" /> with the <see cref="Error" /> it returns; otherwise, invokes the provided asynchronous action and returns
    ///     a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use it to turn a successful result into a failure, e.g.,
    ///         <c>resultTask.MatchAsync(async user => Error.Conflict(…), e => LogAsync(e))</c>. The returned <see cref="Result" /> is always failed.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a call would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///         To get that on purpose, specify the type arguments: <c>MatchAsync&lt;T, Error&gt;(…)</c>.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation. The value of
    ///     the successful <see cref="Result{T}" /> is passed as a parameter. It returns the <see cref="Error" /> of the failed <see cref="Result" />.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="ifSuccessFunction" /> if the <paramref name="resultTask" /> operation is successful;
    ///     otherwise, a failed <see cref="Result" /> with the original <see cref="Error" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorAction" /> parameters are
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> MatchAsync<T>(
        this Task<Result<T>> resultTask,
        Func<T, Task<Error>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(
            Task<Result<T>> resultTask,
            Func<T, Task<Error>> ifSuccessFunction,
            Func<Error, Task> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                var error = await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);

                return Result.FromError(error);
            }

            var errorTask = ifErrorAction(result.Error);
            await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and
    ///     returns their output as an asynchronous operation that represents a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type produced by either branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation, which returns <see cref="Result{T}" />,
    ///     to be executed and which result is to be inspected to select which function to invoke. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a failed operation. The <see cref="Error" /> associated with
    ///     the <paramref name="resultTask" /> operation outcome is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TNext}" /> that represents the value of the <typeparamref name="TNext" /> type returned by either
    ///     <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
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
    public static Task<TNext> MatchAsync<T, TNext>(
        this Task<Result<T>> resultTask,
        Func<T, Task<TNext>> ifSuccessFunction,
        Func<Error, Task<TNext>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TNext> Core(
            Task<Result<T>> resultTask,
            Func<T, Task<TNext>> ifSuccessFunction,
            Func<Error, Task<TNext>> ifErrorFunction,
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
    ///     Invokes the provided asynchronous function and returns its value as an asynchronous operation that returns a <see cref="Result{TNext}" /> instance
    ///     if the <paramref name="resultTask" /> is a successful operation,
    ///     or the provided asynchronous action and returns <see cref="Result{TNext}" /> with the original error as an asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type of the <see cref="Result{TNext}" /> returned by the invoked function.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{TNext}" /> value returned by <paramref name="ifSuccessFunction" />
    ///     if the <paramref name="resultTask" /> is a successful operation, or the <see cref="Result{TNext}" /> with the original outcome of
    ///     the <paramref name="resultTask" /> operation after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorAction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<TNext>> MatchAsync<T, TNext>(
        this Task<Result<T>> resultTask,
        Func<T, Task<Result<TNext>>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(
            Task<Result<T>> resultTask,
            Func<T, Task<Result<TNext>>> ifSuccessFunction,
            Func<Error, Task> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            var errorTask = ifErrorAction(result.Error);
            await Guard.EnsureNotNullTask(errorTask, nameof(ifErrorAction)).ConfigureAwait(continueOnCapturedContext);

            return result.Error;
        }
    }

    /// <summary>
    ///     Executes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and
    ///     returns their output as a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type of the <see cref="Result{TNext}" /> returned by the invoked function.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{TNext}" /> returned by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<TNext>> MatchAsync<T, TNext>(
        this Task<Result<T>> resultTask,
        Func<T, Task<Result<TNext>>> ifSuccessFunction,
        Func<Error, Task<Result<TNext>>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(
            Task<Result<T>> resultTask,
            Func<T, Task<Result<TNext>>> ifSuccessFunction,
            Func<Error, Task<Result<TNext>>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            var errorTask = ifErrorFunction(result.Error);

            return await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    #endregion

    #region Task results with synchronous delegates (#214)

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <param name="resultTask">The asynchronous operation that returns the result to match. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">The synchronous action to run on a successful result. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorAction">The synchronous action to run on a failed result, with its error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the match, as the synchronous <c>Match</c> returns it.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" />, the <paramref name="ifSuccessAction" /> or the <paramref name="ifErrorAction" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Action ifSuccessAction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result> resultTask, Action ifSuccessAction, Action<Error> ifErrorAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessAction, ifErrorAction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <param name="resultTask">The asynchronous operation that returns the result to match. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function to run on a successful result; it returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">The synchronous action to run on a failed result, with its error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the match, as the synchronous <c>Match</c> returns it.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" />, the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorAction" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Func<Result> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result> resultTask, Func<Result> ifSuccessFunction, Action<Error> ifErrorAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorAction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <param name="resultTask">The asynchronous operation that returns the result to match. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function to run on a successful result; it returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The synchronous function to run on a failed result, with its error; it returns the value of the match. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the match, as the synchronous <c>Match</c> returns it.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" />, the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorFunction" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Func<Result> ifSuccessFunction, Func<Error, Result> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(
            Task<Result> resultTask,
            Func<Result> ifSuccessFunction,
            Func<Error, Result> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorFunction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result, or of the value that the delegates return.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to match. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function to run on a successful result; it returns the value. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">The synchronous action to run on a failed result, with its error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the match, as the synchronous <c>Match</c> returns it.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" />, the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorAction" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static Task<Result<T>> MatchAsync<T>(this Task<Result> resultTask,
        Func<T> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Task<Result> resultTask, Func<T> ifSuccessFunction, Action<Error> ifErrorAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorAction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <param name="resultTask">The asynchronous operation that returns the result to match. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function to run on a successful result; it returns the error of a failed result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">The synchronous action to run on a failed result, with its error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the match, as the synchronous <c>Match</c> returns it.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" />, the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorAction" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Func<Error> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result> resultTask, Func<Error> ifSuccessFunction, Action<Error> ifErrorAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorAction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result, or of the value that the delegates return.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to match. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function to run on a successful result; it returns the value. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The synchronous function to run on a failed result, with its error; it returns the value of the match. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The value that the delegate returns.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" />, the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorFunction" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static Task<T> MatchAsync<T>(this Task<Result> resultTask,
        Func<T> ifSuccessFunction, Func<Error, T> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<T> Core(Task<Result> resultTask, Func<T> ifSuccessFunction, Func<Error, T> ifErrorFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorFunction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result, or of the value that the delegates return.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to match. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function to run on a successful result; it returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">The synchronous action to run on a failed result, with its error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the match, as the synchronous <c>Match</c> returns it.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" />, the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorAction" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static Task<Result<T>> MatchAsync<T>(this Task<Result> resultTask,
        Func<Result<T>> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(
            Task<Result> resultTask,
            Func<Result<T>> ifSuccessFunction,
            Action<Error> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorAction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result, or of the value that the delegates return.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to match. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function to run on a successful result; it returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The synchronous function to run on a failed result, with its error; it returns the value of the match. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the match, as the synchronous <c>Match</c> returns it.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" />, the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorFunction" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static Task<Result<T>> MatchAsync<T>(this Task<Result> resultTask,
        Func<Result<T>> ifSuccessFunction, Func<Error, Result<T>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(
            Task<Result> resultTask,
            Func<Result<T>> ifSuccessFunction,
            Func<Error, Result<T>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorFunction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to match. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">The synchronous action to run on a successful result, with its value. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorAction">The synchronous action to run on a failed result, with its error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the match, as the synchronous <c>Match</c> returns it.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" />, the <paramref name="ifSuccessAction" /> or the <paramref name="ifErrorAction" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static Task<Result<T>> MatchAsync<T>(this Task<Result<T>> resultTask,
        Action<T> ifSuccessAction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Task<Result<T>> resultTask, Action<T> ifSuccessAction, Action<Error> ifErrorAction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessAction, ifErrorAction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <typeparam name="TNext">The type of the value that the delegates return.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to match. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function to run on a successful result, with its value; it returns the value. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">The synchronous action to run on a failed result, with its error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the match, as the synchronous <c>Match</c> returns it.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" />, the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorAction" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static Task<Result<TNext>> MatchAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, TNext> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(
            Task<Result<T>> resultTask,
            Func<T, TNext> ifSuccessFunction,
            Action<Error> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorAction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to match. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function to run on a successful result, with its value; it returns the error of a failed result. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">The synchronous action to run on a failed result, with its error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the match, as the synchronous <c>Match</c> returns it.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" />, the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorAction" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static Task<Result> MatchAsync<T>(this Task<Result<T>> resultTask,
        Func<T, Error> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(
            Task<Result<T>> resultTask,
            Func<T, Error> ifSuccessFunction,
            Action<Error> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorAction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <typeparam name="TNext">The type of the value that the delegates return.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to match. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function to run on a successful result, with its value; it returns the value. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The synchronous function to run on a failed result, with its error; it returns the value of the match. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The value that the delegate returns.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" />, the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorFunction" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static Task<TNext> MatchAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, TNext> ifSuccessFunction, Func<Error, TNext> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TNext> Core(
            Task<Result<T>> resultTask,
            Func<T, TNext> ifSuccessFunction,
            Func<Error, TNext> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorFunction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <typeparam name="TNext">The type of the value that the delegates return.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to match. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function to run on a successful result, with its value; it returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">The synchronous action to run on a failed result, with its error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the match, as the synchronous <c>Match</c> returns it.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" />, the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorAction" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static Task<Result<TNext>> MatchAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, Result<TNext>> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(
            Task<Result<T>> resultTask,
            Func<T, Result<TNext>> ifSuccessFunction,
            Action<Error> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorAction);
        }
    }

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <typeparam name="TNext">The type of the value that the delegates return.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to match. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function to run on a successful result, with its value; it returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The synchronous function to run on a failed result, with its error; it returns the value of the match. Must not be
    ///     <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the match, as the synchronous <c>Match</c> returns it.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" />, the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorFunction" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static Task<Result<TNext>> MatchAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, Result<TNext>> ifSuccessFunction, Func<Error, Result<TNext>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(
            Task<Result<T>> resultTask,
            Func<T, Result<TNext>> ifSuccessFunction,
            Func<Error, Result<TNext>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorFunction);
        }
    }

    #endregion

    #region Not supported: one synchronous and one asynchronous delegate, and delegates returning a ValueTask (#214)

    // One synchronous and one asynchronous delegate would bind to the overloads with synchronous delegates, which would drop the task
    // (an async lambda as an action becomes async void) or wrap it in the result (#214). These overloads, the traps of the synchronous
    // Match moved to the task receivers, turn such a call into a compile error (CS0619). The second group covers two delegates that
    // return a ValueTask, which would otherwise be ambiguous (CS0121) between the first group. The overloads whose delegates return a
    // Task have priority 1: an async lambda converts to a Task and to a ValueTask.

    private const string MixedSuccessTaskMessage =
        "The success delegate returns a task, but the error delegate is synchronous. MatchAsync takes two synchronous " +
        "or two asynchronous delegates: make the error delegate asynchronous too, or await the result first and call " +
        "Match.";

    private const string MixedErrorTaskMessage =
        "The error delegate returns a task, but the success delegate is synchronous. MatchAsync takes two synchronous " +
        "or two asynchronous delegates: make the success delegate asynchronous too, or await the result first and call " +
        "Match.";

    private const string OtherTaskMessage =
        "The delegates return a ValueTask, which the Task-based MatchAsync does not await. Import " +
        "Maple.Results.Extensions.ValueTasks instead of Maple.Results.Extensions, or convert them with AsTask().";

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Func<Task> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result> MatchAsync<TNext>(this Task<Result> resultTask,
        Func<Task<TNext>> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Func<ValueTask> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    public static Task<Result> MatchAsync<TNext>(this Task<Result> resultTask,
        Func<ValueTask<TNext>> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result> MatchAsync<T>(this Task<Result<T>> resultTask,
        Func<T, Task> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result> MatchAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, Task<TNext>> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    public static Task<Result> MatchAsync<T>(this Task<Result<T>> resultTask,
        Func<T, ValueTask> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    public static Task<Result> MatchAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, ValueTask<TNext>> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Action ifSuccessAction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Action ifSuccessAction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result> MatchAsync<TTaskResult>(this Task<Result> resultTask,
        Action ifSuccessAction, Func<Error, ValueTask<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Func<Result> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Func<Result> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result> MatchAsync<TTaskResult>(this Task<Result> resultTask,
        Func<Result> ifSuccessFunction, Func<Error, ValueTask<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> MatchAsync<T>(this Task<Result> resultTask,
        Func<T> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result<T>> MatchAsync<T>(this Task<Result> resultTask,
        Func<T> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result<T>> MatchAsync<T, TTaskResult>(this Task<Result> resultTask,
        Func<T> ifSuccessFunction, Func<Error, ValueTask<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Func<Error> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Func<Error> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result> MatchAsync<TTaskResult>(this Task<Result> resultTask,
        Func<Error> ifSuccessFunction, Func<Error, ValueTask<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> MatchAsync<T>(this Task<Result> resultTask,
        Func<Result<T>> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result<T>> MatchAsync<T>(this Task<Result> resultTask,
        Func<Result<T>> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result<T>> MatchAsync<T, TTaskResult>(this Task<Result> resultTask,
        Func<Result<T>> ifSuccessFunction, Func<Error, ValueTask<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> MatchAsync<T>(this Task<Result<T>> resultTask,
        Action<T> ifSuccessAction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result<T>> MatchAsync<T>(this Task<Result<T>> resultTask,
        Action<T> ifSuccessAction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result<T>> MatchAsync<T, TTaskResult>(this Task<Result<T>> resultTask,
        Action<T> ifSuccessAction, Func<Error, ValueTask<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result<TNext>> MatchAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, TNext> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result<TNext>> MatchAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, TNext> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result<TNext>> MatchAsync<T, TNext, TTaskResult>(this Task<Result<T>> resultTask,
        Func<T, TNext> ifSuccessFunction, Func<Error, ValueTask<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result> MatchAsync<T>(this Task<Result<T>> resultTask,
        Func<T, Error> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result> MatchAsync<T>(this Task<Result<T>> resultTask,
        Func<T, Error> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result> MatchAsync<T, TTaskResult>(this Task<Result<T>> resultTask,
        Func<T, Error> ifSuccessFunction, Func<Error, ValueTask<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result<TNext>> MatchAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, Result<TNext>> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result<TNext>> MatchAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, Result<TNext>> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static Task<Result<TNext>> MatchAsync<T, TNext, TTaskResult>(this Task<Result<T>> resultTask,
        Func<T, Result<TNext>> ifSuccessFunction, Func<Error, ValueTask<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Func<ValueTask> ifSuccessAction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Func<ValueTask<Result>> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Func<ValueTask<Result>> ifSuccessFunction, Func<Error, ValueTask<Result>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result<T>> MatchAsync<T>(this Task<Result> resultTask,
        Func<ValueTask<T>> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result> MatchAsync(this Task<Result> resultTask,
        Func<ValueTask<Error>> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<T> MatchAsync<T>(this Task<Result> resultTask,
        Func<ValueTask<T>> ifSuccessFunction, Func<Error, ValueTask<T>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result<T>> MatchAsync<T>(this Task<Result> resultTask,
        Func<ValueTask<Result<T>>> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result<T>> MatchAsync<T>(this Task<Result> resultTask,
        Func<ValueTask<Result<T>>> ifSuccessFunction, Func<Error, ValueTask<Result<T>>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result<T>> MatchAsync<T>(this Task<Result<T>> resultTask,
        Func<T, ValueTask> ifSuccessAction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result<TNext>> MatchAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, ValueTask<TNext>> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result> MatchAsync<T>(this Task<Result<T>> resultTask,
        Func<T, ValueTask<Error>> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<TNext> MatchAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, ValueTask<TNext>> ifSuccessFunction, Func<Error, ValueTask<TNext>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result<TNext>> MatchAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, ValueTask<Result<TNext>>> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result<TNext>> MatchAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, ValueTask<Result<TNext>>> ifSuccessFunction, Func<Error, ValueTask<Result<TNext>>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    #endregion
}
