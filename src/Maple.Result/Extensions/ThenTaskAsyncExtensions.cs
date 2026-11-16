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
///     The collection of extension methods that transform or chain a successful <see cref="Result" /> or <see cref="Result{T}" />: a function run
///     asynchronously that returns a value, a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> gives the next result.
/// </summary>
/// <remarks>
///     A failed result is returned unchanged, and the function is not called. A returned <see cref="Result" /> or <see cref="Result{T}" /> is
///     flattened, and a returned <see cref="Error" /> gives a failed result. For a side effect that keeps the result unchanged, use
///     <c>IfSuccessAsync</c>.
/// </remarks>
public static class ThenTaskAsyncExtensions
{
    #region Result extensions

    /// <summary>
    ///     Invokes the specified asynchronous function as an asynchronous operation, if the <paramref name="result" /> represents a successful outcome;
    ///     otherwise, returns an error <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional asynchronous operations that should only execute if the previous <see cref="Result" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     An asynchronous function to execute if <paramref name="result" /> is successful.
    ///     The function must return a <see cref="Task{Result}" /> and must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, a <see cref="Task{Result}" /> that represents the original error <paramref name="result" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    public static Task<Result> ThenAsync(this Result result, Func<Task<Result>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(result, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Result result, Func<Task<Result>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            return result;
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the <paramref name="result" /> represents a successful outcome and returns a failed
    ///     <see cref="Result" /> with the <see cref="Error" /> it returns; otherwise, returns the original <paramref name="result" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use it to turn a successful result into a failure, e.g., <c>result.ThenAsync(async () => Error.Conflict(…))</c>.
    ///         The returned <see cref="Result" /> is always failed.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a call would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///         To get that on purpose, specify the type arguments: <c>ThenAsync&lt;Error&gt;(…)</c>.
    ///     </para>
    /// </remarks>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     An asynchronous function to execute if <paramref name="result" /> is successful.
    ///     It returns the <see cref="Error" /> of the failed <see cref="Result" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful; otherwise, the original <paramref name="result" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    public static Task<Result> ThenAsync(this Result result, Func<Task<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(result, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Result result, Func<Task<Error>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                var error = await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);

                return Result.FromError(error);
            }

            return result;
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function as an asynchronous operation, and returns its output as a <see cref="Task{TResult}" />,
    ///     if the operation represented by the <paramref name="result" /> is successful;
    ///     otherwise, returns a failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional asynchronous operations that should only execute if the previous <see cref="Result" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> value to return if the operation is successful.</typeparam>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     An asynchronous function to execute if the <paramref name="result" /> is successful.
    ///     The function’s return value is used as the successful <see cref="Result{T}" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the successful <see cref="Result{T}" /> containing the value returned by
    ///     <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, a <see cref="Task{TResult}" /> that represents the failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task.</exception>
    public static Task<Result<T>> ThenAsync<T>(this Result result, Func<Task<T>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(result, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Result result, Func<Task<T>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullTask(successTask, nameof(ifSuccessFunction)).ConfigureAwait(continueOnCapturedContext);
            }

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function as an asynchronous operation, and returns a <see cref="Result{T}" /> with its output if the operation
    ///     represented by the <paramref name="result" /> is successful;
    ///     otherwise, returns a failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional asynchronous operations that should only execute if the previous <see cref="Result" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> value to return if the operation is successful.</typeparam>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     An asynchronous function to execute if the <paramref name="result" /> is successful.
    ///     The function’s return value is used as the successful <see cref="Result{T}" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents a successful <see cref="Result{T}" /> returned by <paramref name="ifSuccessFunction" />
    ///     if <paramref name="result" /> is successful;
    ///     otherwise, a <see cref="Task{TResult}" /> that represents the failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    public static Task<Result<T>> ThenAsync<T>(this Result result, Func<Task<Result<T>>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(result, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Result result, Func<Task<Result<T>>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            return result.Error;
        }
    }

    #endregion

    #region Result<T> extensions

    /// <summary>
    ///     Invokes the specified asynchronous function if the <paramref name="result" /> represents a successful outcome;
    ///     otherwise, returns an error <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional asynchronous operations that should only execute if the previous <see cref="Result" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided asynchronous function is not invoked.
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function to execute if <paramref name="result" /> is successful. The value of the successful <see cref="Result{T}" /> is passed as
    ///     a parameter. The function must return a <see cref="Task{Result}" /> and must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that is returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    public static Task<Result> ThenAsync<T>(this Result<T> result, Func<T, Task<Result>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(result, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Result<T> result, Func<T, Task<Result>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the <paramref name="result" /> represents a successful outcome and returns a failed
    ///     <see cref="Result" /> with the <see cref="Error" /> it returns;
    ///     otherwise, returns a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use it to turn a successful result into a failure, e.g., <c>result.ThenAsync(async user => Error.Conflict(…))</c>.
    ///         The returned <see cref="Result" /> is always failed.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a call would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///         To get that on purpose, specify the type arguments: <c>ThenAsync&lt;T, Error&gt;(…)</c>.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the <paramref name="result" />.</typeparam>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function to execute if <paramref name="result" /> is successful. The value of the successful <see cref="Result{T}" /> is passed as
    ///     a parameter. It returns the <see cref="Error" /> of the failed <see cref="Result" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    public static Task<Result> ThenAsync<T>(this Result<T> result, Func<T, Task<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(result, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Result<T> result, Func<T, Task<Error>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                var error = await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);

                return Result.FromError(error);
            }

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function and returns its output as a <see cref="Result{TNext}" /> if the operation represented by
    ///     the <paramref name="result" /> is successful; otherwise, returns a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional asynchronous operations that should only execute if the previous <see cref="Result{T}" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided asynchronous function is not invoked.
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The output type of the passed function and the type of the <see cref="Result{TNext}" /> value to return.
    /// </typeparam>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     An asynchronous function to execute if the <paramref name="result" /> is successful. The value of the successful <see cref="Result{T}" /> is passed
    ///     as a parameter. The function’s return value is used as the successful <see cref="Result{TNext}" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the successful <see cref="Result{TNext}" /> containing the value returned by
    ///     <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task.</exception>
    public static Task<Result<TNext>> ThenAsync<T, TNext>(this Result<T> result,
        Func<T, Task<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(result, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(Result<T> result, Func<T, Task<TNext>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                var nextTask = Guard.EnsureNotNullTask(successTask, nameof(ifSuccessFunction));

                return await nextTask.ConfigureAwait(continueOnCapturedContext);
            }

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function and returns a <see cref="Result{TNext}" /> with its output if the operation represented by
    ///     the <paramref name="result" /> is successful; otherwise, returns a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional asynchronous operations that should only execute if the previous <see cref="Result{T}" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The type of the <see cref="Result{TNext}" /> value to return and of the output of the passed function.
    /// </typeparam>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     An asynchronous function to execute if the <paramref name="result" /> is successful. The value of the successful <see cref="Result{T}" /> is passed
    ///     as a parameter. The function’s must return <see cref="Task{Result}" /> that represents <see cref="Result{TNext}" />.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the successful <see cref="Result{TNext}" /> returned by <paramref name="ifSuccessFunction" />
    ///     if <paramref name="result" /> is successful; otherwise, a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    public static Task<Result<TNext>> ThenAsync<T, TNext>(this Result<T> result,
        Func<T, Task<Result<TNext>>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(result, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(Result<T> result, Func<T, Task<Result<TNext>>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            return result.Error;
        }
    }

    #endregion

    #region Task<Result> extensions

    /// <summary>
    ///     Invokes the specified asynchronous function as an asynchronous operation, if the asynchronous <paramref name="resultTask" /> operation represents
    ///     a successful outcome; otherwise, returns an asynchronous operation that represents the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method enables chaining additional asynchronous operations that should only execute if the previous asynchronous operation was successful;
    ///         otherwise the provided function is not invoked.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation.
    ///     </para>
    /// </remarks>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     An asynchronous function to execute if <paramref name="resultTask" /> is successful asynchronous operation.
    ///     The function must return a <see cref="Task{TResult}" /> and must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> returned by <paramref name="ifSuccessFunction" /> if a <paramref name="resultTask" /> operation is successful;
    ///     otherwise, a <see cref="Task{TResult}" /> that represents the original error.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> ThenAsync(this Task<Result> resultTask, Func<Task<Result>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result> resultTask, Func<Task<Result>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            return result;
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the asynchronous <paramref name="resultTask" /> operation represents a successful outcome and
    ///     returns a failed <see cref="Result" /> with the <see cref="Error" /> it returns; otherwise, returns the original result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use it to turn a successful result into a failure, e.g., <c>resultTask.ThenAsync(async () => Error.Conflict(…))</c>.
    ///         The returned <see cref="Result" /> is always failed.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a call would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///         To get that on purpose, specify the type arguments: <c>ThenAsync&lt;Error&gt;(…)</c>.
    ///     </para>
    /// </remarks>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     An asynchronous function to execute if the <paramref name="resultTask" /> operation is successful.
    ///     It returns the <see cref="Error" /> of the failed <see cref="Result" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="ifSuccessFunction" /> if the <paramref name="resultTask" /> operation is successful; otherwise, the original result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> ThenAsync(this Task<Result> resultTask, Func<Task<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result> resultTask, Func<Task<Error>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                var error = await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);

                return Result.FromError(error);
            }

            return result;
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function as an asynchronous operation, and returns its output as a <see cref="Task{TResult}" />,
    ///     if the operation represented by the <paramref name="resultTask" /> is successful;
    ///     otherwise, returns a failed <see cref="Task{TResult}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method enables chaining additional asynchronous operations that should only execute if the previous was successful.
    ///         If <paramref name="resultTask" /> is not a successful operation, the provided function is not invoked.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> value to return if the operation is successful.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     An asynchronous function to execute if <paramref name="resultTask" /> is successful asynchronous operation.
    ///     The function must return a <see cref="Task{TResult}" /> and must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the successful <see cref="Result{T}" /> containing the value returned by
    ///     <paramref name="ifSuccessFunction" /> if <paramref name="resultTask" /> operation is successful;
    ///     otherwise, a <see cref="Task{TResult}" /> that represents the failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> ThenAsync<T>(this Task<Result> resultTask, Func<Task<T>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Task<Result> resultTask, Func<Task<T>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullTask(successTask, nameof(ifSuccessFunction)).ConfigureAwait(continueOnCapturedContext);
            }

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function as an asynchronous operation, and returns its output as a <see cref="Task{TResult}" />,
    ///     if the operation represented by the <paramref name="resultTask" /> is successful;
    ///     otherwise, returns a failed <see cref="Task{TResult}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method enables chaining additional asynchronous operations that should only execute if the previous was successful.
    ///         If <paramref name="resultTask" /> is not successful operation, the provided function is not invoked.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> value to return if the operation is successful.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     An asynchronous function to execute if <paramref name="resultTask" /> is successful asynchronous operation.
    ///     The function must return a <see cref="Task{TResult}" /> and must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the successful <see cref="Result{T}" /> containing the value returned by
    ///     <paramref name="ifSuccessFunction" /> if <paramref name="resultTask" /> operation is successful;
    ///     otherwise, a <see cref="Task{TResult}" /> that represents the failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> ThenAsync<T>(this Task<Result> resultTask,
        Func<Task<Result<T>>> ifSuccessFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Task<Result> resultTask, Func<Task<Result<T>>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction();
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            return result.Error;
        }
    }

    #endregion

    #region Task<Result<T>> extensions

    /// <summary>
    ///     Invokes the specified asynchronous function as an asynchronous operation, if the <paramref name="resultTask" /> represents a successful outcome;
    ///     otherwise, returns an asynchronous operation that represents the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method enables chaining additional asynchronous operations that should only execute if the previous asynchronous operation was successful;
    ///         otherwise the provided function is not invoked.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function to execute if <paramref name="resultTask" /> is successful asynchronous operation. The value of the successful
    ///     <see cref="Result{T}" /> is passed as a parameter. The function must return a <see cref="Task{Result}" /> and must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that is returned by <paramref name="ifSuccessFunction" /> if <paramref name="resultTask" /> is successful;
    ///     otherwise, a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> ThenAsync<T>(this Task<Result<T>> resultTask,
        Func<T, Task<Result>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result<T>> resultTask, Func<T, Task<Result>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the asynchronous <paramref name="resultTask" /> operation represents a successful outcome and
    ///     returns a failed <see cref="Result" /> with the <see cref="Error" /> it returns;
    ///     otherwise, returns a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use it to turn a successful result into a failure, e.g., <c>resultTask.ThenAsync(async user => Error.Conflict(…))</c>.
    ///         The returned <see cref="Result" /> is always failed.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a call would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///         To get that on purpose, specify the type arguments: <c>ThenAsync&lt;T, Error&gt;(…)</c>.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result of the <paramref name="resultTask" /> operation.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function to execute if the <paramref name="resultTask" /> operation is successful. The value of the successful
    ///     <see cref="Result{T}" /> is passed as a parameter. It returns the <see cref="Error" /> of the failed <see cref="Result" />.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="ifSuccessFunction" /> if the <paramref name="resultTask" /> operation is successful;
    ///     otherwise, a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> ThenAsync<T>(this Task<Result<T>> resultTask,
        Func<T, Task<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result<T>> resultTask, Func<T, Task<Error>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                var error = await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);

                return Result.FromError(error);
            }

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function as an asynchronous operation and returns its output as an asynchronous operation that returns
    ///     <see cref="Result{TNext}" />, if the asynchronous <paramref name="resultTask" /> operation represents a successful outcome;
    ///     otherwise, returns an asynchronous operation that represents the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method enables chaining additional asynchronous operations that should only execute if the previous asynchronous operation was successful;
    ///         otherwise the provided function is not invoked.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The output type of the passed function and the type of the <see cref="Result{TNext}" /> value to return.
    /// </typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function to execute if <paramref name="resultTask" /> is successful asynchronous operation. The value of the successful
    ///     <see cref="Result{T}" /> is passed as a parameter. The function must return a <see cref="Task{Result}" /> and must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the successful <see cref="Result{TNext}" /> containing the value returned by
    ///     <paramref name="ifSuccessFunction" /> if <paramref name="resultTask" /> is successful;
    ///     otherwise, a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<TNext>> ThenAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, Task<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(Task<Result<T>> resultTask, Func<T, Task<TNext>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                var nextTask = Guard.EnsureNotNullTask(successTask, nameof(ifSuccessFunction));

                return await nextTask.ConfigureAwait(continueOnCapturedContext);
            }

            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function as an asynchronous operation and returns its output as an asynchronous operation that returns
    ///     <see cref="Result{TNext}" />, if the asynchronous <paramref name="resultTask" /> operation represents a successful outcome;
    ///     otherwise, returns an asynchronous operation that represents the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This method enables chaining additional asynchronous operations that should only execute if the previous asynchronous operation was successful;
    ///         otherwise the provided function is not invoked.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The type of the <see cref="Result{TNext}" /> value to return and of the output of the passed function.
    /// </typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     An asynchronous function to execute if <paramref name="resultTask" /> is successful asynchronous operation. The value of the successful
    ///     <see cref="Result{T}" /> is passed as a parameter.
    ///     The function’s must return <see cref="Task{Result}" /> that represents <see cref="Result{TNext}" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the successful <see cref="Result{TNext}" /> returned by <paramref name="ifSuccessFunction" />
    ///     if <paramref name="resultTask" /> is successful; otherwise, a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<TNext>> ThenAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, Task<Result<TNext>>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(Task<Result<T>> resultTask, Func<T, Task<Result<TNext>>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
            {
                var successTask = ifSuccessFunction(result.Value);
                return await Guard.EnsureNotNullResult(successTask, nameof(ifSuccessFunction), continueOnCapturedContext);
            }

            return result.Error;
        }
    }

    #endregion

    #region Not supported: actions

    // A function that returns nothing is a side effect, not a transformation (#197): these overloads turn the call into a compile
    // error (CS0619) that names the method to use, instead of the conversion errors CS0029 and CS1662.

    private const string ActionMessage =
        "The function returns a task without a value. ThenAsync needs a function whose task returns a value, a result " +
        "or an error; use IfSuccessAsync for a side effect.";

    /// <summary>
    ///     Not supported: the function returns a task without a value; use <c>IfSuccessAsync</c> for a side effect.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionMessage, error: true)]
    public static Task<Result> ThenAsync(this Result result, Func<Task> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ActionMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task without a value; use <c>IfSuccessAsync</c> for a side effect.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionMessage, error: true)]
    public static Task<Result<T>> ThenAsync<T>(this Result<T> result, Func<T, Task> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ActionMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task without a value; use <c>IfSuccessAsync</c> for a side effect.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result> ThenAsync(this Task<Result> resultTask, Func<Task> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ActionMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task without a value; use <c>IfSuccessAsync</c> for a side effect.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> ThenAsync<T>(this Task<Result<T>> resultTask, Func<T, Task> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(ActionMessage);
    }

    #endregion

    #region Task results with synchronous delegates

    // See issue #214.

    /// <summary>
    ///     Runs the synchronous function if the result that the <paramref name="resultTask" /> returns is successful, and returns a result with its value.
    /// </summary>
    /// <remarks>
    ///     The function is synchronous: it runs after the <paramref name="resultTask" /> completes, and it is not called for a
    ///     failed result, whose <see cref="Error" /> is returned. Use the overloads with an asynchronous function for
    ///     asynchronous work.
    /// </remarks>
    /// <typeparam name="T">The type of the value that the function returns.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">The synchronous function that returns the value of the next result. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run it on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The successful result with the value of the function, or the failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" /> or the <paramref name="ifSuccessFunction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task<Result<T>> ThenAsync<T>(this Task<Result> resultTask,
        Func<T> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Task<Result> resultTask, Func<T> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Then(ifSuccessFunction);
        }
    }

    /// <summary>
    ///     Runs the synchronous function if the result that the <paramref name="resultTask" /> returns is successful, and returns the result that it returns.
    /// </summary>
    /// <remarks>
    ///     The function is synchronous: it runs after the <paramref name="resultTask" /> completes, and it is not called for a
    ///     failed result, whose <see cref="Error" /> is returned. Use the overloads with an asynchronous function for
    ///     asynchronous work.
    /// </remarks>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">The synchronous function that returns the next result. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run it on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the function, or the failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" /> or the <paramref name="ifSuccessFunction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or the
    ///     <paramref name="ifSuccessFunction" /> returns <see langword="null" />.
    /// </exception>
    public static Task<Result> ThenAsync(this Task<Result> resultTask,
        Func<Result> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result> resultTask, Func<Result> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Then(ifSuccessFunction);
        }
    }

    /// <summary>
    ///     Runs the synchronous function if the result that the <paramref name="resultTask" /> returns is successful, and returns the result that it returns.
    /// </summary>
    /// <remarks>
    ///     The function is synchronous: it runs after the <paramref name="resultTask" /> completes, and it is not called for a
    ///     failed result, whose <see cref="Error" /> is returned. Use the overloads with an asynchronous function for
    ///     asynchronous work.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the next result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">The synchronous function that returns the next result. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run it on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the function, or a failed result with the original error.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" /> or the <paramref name="ifSuccessFunction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or the
    ///     <paramref name="ifSuccessFunction" /> returns <see langword="null" />.
    /// </exception>
    public static Task<Result<T>> ThenAsync<T>(this Task<Result> resultTask,
        Func<Result<T>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Task<Result> resultTask, Func<Result<T>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Then(ifSuccessFunction);
        }
    }

    /// <summary>
    ///     Runs the synchronous function if the result that the <paramref name="resultTask" /> returns is successful, and returns a failed result
    ///     with the error that it returns.
    /// </summary>
    /// <remarks>
    ///     The function is synchronous: it runs after the <paramref name="resultTask" /> completes, and it is not called for a
    ///     failed result, whose <see cref="Error" /> is returned. Use the overloads with an asynchronous function for
    ///     asynchronous work.
    /// </remarks>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">The synchronous function that returns the error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run it on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>A failed result with the error of the function, or the original failed result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" /> or the <paramref name="ifSuccessFunction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or the
    ///     <paramref name="ifSuccessFunction" /> returns <see langword="null" />.
    /// </exception>
    public static Task<Result> ThenAsync(this Task<Result> resultTask,
        Func<Error> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result> resultTask, Func<Error> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Then(ifSuccessFunction);
        }
    }

    /// <summary>
    ///     Runs the synchronous function on the value if the result that the <paramref name="resultTask" /> returns is successful, and returns a
    ///     result with the value that it returns.
    /// </summary>
    /// <remarks>
    ///     The function is synchronous: it runs after the <paramref name="resultTask" /> completes, and it is not called for a
    ///     failed result, whose <see cref="Error" /> is returned. Use the overloads with an asynchronous function for
    ///     asynchronous work.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <typeparam name="TNext">The type of the value that the function returns.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function that receives the value and returns the next value. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run it on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The successful result with the value of the function, or a failed result with the original error.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" /> or the <paramref name="ifSuccessFunction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task<Result<TNext>> ThenAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, TNext> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(Task<Result<T>> resultTask, Func<T, TNext> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Then(ifSuccessFunction);
        }
    }

    /// <summary>
    ///     Runs the synchronous function on the value if the result that the <paramref name="resultTask" /> returns is successful, and returns the
    ///     result that it returns.
    /// </summary>
    /// <remarks>
    ///     The function is synchronous: it runs after the <paramref name="resultTask" /> completes, and it is not called for a
    ///     failed result, whose <see cref="Error" /> is returned. Use the overloads with an asynchronous function for
    ///     asynchronous work.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function that receives the value and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run it on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the function, or a failed result with the original error.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" /> or the <paramref name="ifSuccessFunction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or the
    ///     <paramref name="ifSuccessFunction" /> returns <see langword="null" />.
    /// </exception>
    public static Task<Result> ThenAsync<T>(this Task<Result<T>> resultTask,
        Func<T, Result> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result<T>> resultTask, Func<T, Result> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Then(ifSuccessFunction);
        }
    }

    /// <summary>
    ///     Runs the synchronous function on the value if the result that the <paramref name="resultTask" /> returns is successful, and returns the
    ///     result that it returns.
    /// </summary>
    /// <remarks>
    ///     The function is synchronous: it runs after the <paramref name="resultTask" /> completes, and it is not called for a
    ///     failed result, whose <see cref="Error" /> is returned. Use the overloads with an asynchronous function for
    ///     asynchronous work.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <typeparam name="TNext">The type of the value of the next result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The synchronous function that receives the value and returns the next result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run it on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the function, or a failed result with the original error.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" /> or the <paramref name="ifSuccessFunction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or the
    ///     <paramref name="ifSuccessFunction" /> returns <see langword="null" />.
    /// </exception>
    public static Task<Result<TNext>> ThenAsync<T, TNext>(this Task<Result<T>> resultTask,
        Func<T, Result<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<TNext>> Core(Task<Result<T>> resultTask, Func<T, Result<TNext>> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Then(ifSuccessFunction);
        }
    }

    /// <summary>
    ///     Runs the synchronous function on the value if the result that the <paramref name="resultTask" /> returns is successful, and returns a
    ///     failed result with the error that it returns.
    /// </summary>
    /// <remarks>
    ///     The function is synchronous: it runs after the <paramref name="resultTask" /> completes, and it is not called for a
    ///     failed result, whose <see cref="Error" /> is returned. Use the overloads with an asynchronous function for
    ///     asynchronous work.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">The synchronous function that receives the value and returns the error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run it on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>A failed result with the error of the function, or a failed result with the original error.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" /> or the <paramref name="ifSuccessFunction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or the
    ///     <paramref name="ifSuccessFunction" /> returns <see langword="null" />.
    /// </exception>
    public static Task<Result> ThenAsync<T>(this Task<Result<T>> resultTask,
        Func<T, Error> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        return Core(resultTask, ifSuccessFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result<T>> resultTask, Func<T, Error> ifSuccessFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Then(ifSuccessFunction);
        }
    }

    #endregion

    #region Not supported: synchronous delegates of the wrong kind, and functions returning a ValueTask

    // A synchronous action passed to ThenAsync or ElseAsync, or a synchronous function that returns a value passed to
    // IfSuccessAsync or IfErrorAsync, is a compile error (CS0619) that names the method to use, as in #197.
    // A function that returns a ValueTask would bind to the overloads with a synchronous delegate and give a result holding a task
    // that nobody awaits (#214): these overloads turn the call into a compile error (CS0619) that names the namespace to import.
    // The overloads whose delegate returns a Task have priority 1 (#214): an async lambda converts to a Task and to a ValueTask,
    // so without it, a call with an async lambda would be ambiguous (CS0121) between them and the traps for the other kind of task.

    private const string SyncActionMessage =
        "The action returns nothing. ThenAsync needs a function that returns a value, a result or an error; use " +
        "IfSuccessAsync for a side effect.";

    private const string OtherTaskMessage =
        "The function returns a ValueTask, which the Task-based ThenAsync does not await. Import " +
        "Maple.Results.Extensions.ValueTasks instead of Maple.Results.Extensions, or convert it with AsTask().";

    /// <summary>
    ///     Not supported: the action returns nothing; use <c>IfSuccessAsync</c> for a side effect.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SyncActionMessage, error: true)]
    public static Task<Result> ThenAsync(this Task<Result> resultTask, Action ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SyncActionMessage);
    }

    /// <summary>
    ///     Not supported: the action returns nothing; use <c>IfSuccessAsync</c> for a side effect.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SyncActionMessage, error: true)]
    public static Task<Result<T>> ThenAsync<T>(this Task<Result<T>> resultTask, Action<T> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SyncActionMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result> ThenAsync(this Task<Result> resultTask, Func<ValueTask> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result> ThenAsync<TTaskResult>(this Task<Result> resultTask,
        Func<ValueTask<TTaskResult>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result<T>> ThenAsync<T>(this Task<Result<T>> resultTask, Func<T, ValueTask> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a <c>ValueTask</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static Task<Result<T>> ThenAsync<T, TTaskResult>(this Task<Result<T>> resultTask,
        Func<T, ValueTask<TTaskResult>> ifSuccessFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    #endregion
}
