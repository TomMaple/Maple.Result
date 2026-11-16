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

namespace Maple.Results.Extensions.ValueTasks;

/// <summary>
///     The collection of extension methods for executing asynchronous actions or functions based on the state of a <see cref="Result" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of operations by specifying operations that will be executed depending on the status of the <see cref="Result" />
///     instance. These helpers centralize success/error handling logic by accepting delegates for each branch,
///     enabling concise match-like patterns across synchronous result workflows.
///     <para>
///         These methods are declared in the <c>Maple.Results.Extensions.ValueTasks</c> namespace, separately from their
///         <see cref="System.Threading.Tasks.Task" />-based counterparts in <c>Maple.Results.Extensions</c>, because both sets declare the same
///         method names and
///         an asynchronous lambda is convertible to a <c>Func&lt;Task&gt;</c> as well as to a <c>Func&lt;ValueTask&gt;</c> delegate.
///         Importing only one of these namespaces keeps such calls unambiguous.
///     </para>
/// </remarks>
public static class MatchValueTaskAsyncExtensions
{
    #region Result

    /// <returns>
    ///     A <see cref="ValueTask{Result}" /> that represents the original <paramref name="result" />, enabling fluent chaining.
    /// </returns>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync(Result, Func{Task}, Func{Error, Task}, bool?)" />
    public static ValueTask<Result> MatchAsync(this Result result, Func<ValueTask> ifSuccessAction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            Result result,
            Func<ValueTask> ifSuccessAction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
                await ifSuccessAction().ConfigureAwait(continueOnCapturedContext);
            else
                await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    /// <returns>
    ///     The <see cref="ValueTask{Result}" /> which represents the <see cref="Result" /> returned by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync(Result, Func{Task{Result}}, Func{Error, Task{Result}}, bool?)" />
    public static ValueTask<Result> MatchAsync(this Result result, Func<ValueTask<Result>> ifSuccessFunction,
        Func<Error, ValueTask<Result>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            Result result,
            Func<ValueTask<Result>> ifSuccessFunction,
            Func<Error, ValueTask<Result>> ifErrorFunction,
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

    /// <returns>
    ///     The <see cref="ValueTask{T}" /> that represents the value of type <typeparamref name="T" /> produced by either <paramref name="ifSuccessFunction" />
    ///     or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T}(Result, Func{Task{T}}, Func{Error, Task{T}}, bool?)" />
    public static ValueTask<T> MatchAsync<T>(this Result result, Func<ValueTask<T>> ifSuccessFunction,
        Func<Error, ValueTask<T>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<T> Core(
            Result result,
            Func<ValueTask<T>> ifSuccessFunction,
            Func<Error, ValueTask<T>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            return result.IsSuccess
                ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
                : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
        }
    }

    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{T}" /> produced by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T}(Result, Func{Task{Result{T}}}, Func{Error, Task{Result{T}}}, bool?)" />
    public static ValueTask<Result<T>> MatchAsync<T>(this Result result, Func<ValueTask<Result<T>>> ifSuccessFunction,
        Func<Error, ValueTask<Result<T>>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(
            Result result,
            Func<ValueTask<Result<T>>> ifSuccessFunction,
            Func<Error, ValueTask<Result<T>>> ifErrorFunction,
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

    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the original <paramref name="result" /> for fluent chaining.
    /// </returns>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T}(Result{T}, Func{T, Task}, Func{Error, Task}, bool?)" />
    public static ValueTask<Result<T>> MatchAsync<T>(this Result<T> result, Func<T, ValueTask> ifSuccessAction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(
            Result<T> result,
            Func<T, ValueTask> ifSuccessAction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
                await ifSuccessAction(result.Value).ConfigureAwait(continueOnCapturedContext);
            else
                await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    /// <returns>
    ///     The <see cref="ValueTask{TNext}" /> that represents the value of the <typeparamref name="TNext" /> type returned by either
    ///     <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T, TNext}(Result{T}, Func{T, Task{TNext}}, Func{Error, Task{TNext}}, bool?)" />
    public static ValueTask<TNext> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<TNext>> ifSuccessFunction,
        Func<Error, ValueTask<TNext>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<TNext> Core(
            Result<T> result,
            Func<T, ValueTask<TNext>> ifSuccessFunction,
            Func<Error, ValueTask<TNext>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            return result.IsSuccess
                ? await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext)
                : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
        }
    }

    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{TNext}" /> returned by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T, TNext}(Result{T}, Func{T, Task{Result{TNext}}}, Func{Error, Task{Result{TNext}}}, bool?)" />
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
        Func<Error, ValueTask<Result<TNext>>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<TNext>> Core(
            Result<T> result,
            Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
            Func<Error, ValueTask<Result<TNext>>> ifErrorFunction,
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

    #region ValueTask<Result>

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
    ///     The asynchronous operation to be executed and which result is to be inspected.
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
    ///     A <see cref="ValueTask{Result}" /> that represents the original <paramref name="resultTask" />, enabling fluent chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessAction" />, or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask, Func<ValueTask> ifSuccessAction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            ValueTask<Result> resultTask,
            Func<ValueTask> ifSuccessAction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
                await ifSuccessAction().ConfigureAwait(continueOnCapturedContext);
            else
                await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

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
    ///     The asynchronous operation to be executed and which result is to be inspected.
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
    ///     The <see cref="ValueTask{Result}" /> which represents the <see cref="Result" /> returned by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask, Func<ValueTask<Result>> ifSuccessFunction,
        Func<Error, ValueTask<Result>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            ValueTask<Result> resultTask,
            Func<ValueTask<Result>> ifSuccessFunction,
            Func<Error, ValueTask<Result>> ifErrorFunction,
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
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and returns
    ///     the value that the invoked function returns, as an asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The return type produced by either branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
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
    ///     The <see cref="ValueTask{T}" /> that represents the value of type <typeparamref name="T" /> produced by either <paramref name="ifSuccessFunction" />
    ///     or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static ValueTask<T> MatchAsync<T>(this ValueTask<Result> resultTask, Func<ValueTask<T>> ifSuccessFunction,
        Func<Error, ValueTask<T>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<T> Core(
            ValueTask<Result> resultTask,
            Func<ValueTask<T>> ifSuccessFunction,
            Func<Error, ValueTask<T>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            return result.IsSuccess
                ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
                : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and returns
    ///     their output as a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> returned by the matching branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
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
    ///     The <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{T}" /> produced by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<T>> MatchAsync<T>(
        this ValueTask<Result> resultTask, Func<ValueTask<Result<T>>> ifSuccessFunction,
        Func<Error, ValueTask<Result<T>>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(
            ValueTask<Result> resultTask,
            Func<ValueTask<Result<T>>> ifSuccessFunction,
            Func<Error, ValueTask<Result<T>>> ifErrorFunction,
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

    #region ValueTask<Result<T>>

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
    ///     to be executed and which result is to be inspected to select which function to invoke.
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
    ///     The <see cref="ValueTask{TResult}" /> that represents the outcome of the <paramref name="resultTask" /> for fluent chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessAction" />, or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result<T>> resultTask, Func<T, ValueTask> ifSuccessAction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(
            ValueTask<Result<T>> resultTask,
            Func<T, ValueTask> ifSuccessAction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
                await ifSuccessAction(result.Value).ConfigureAwait(continueOnCapturedContext);
            else
                await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and returns
    ///     the value that the invoked function returns, as an asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type produced by either branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation, which returns <see cref="Result{T}" />,
    ///     to be executed and which result is to be inspected to select which function to invoke.
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
    ///     The <see cref="ValueTask{TNext}" /> that represents the value of the <typeparamref name="TNext" /> type returned by either
    ///     <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static ValueTask<TNext> MatchAsync<T, TNext>(
        this ValueTask<Result<T>> resultTask,
        Func<T, ValueTask<TNext>> ifSuccessFunction,
        Func<Error, ValueTask<TNext>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<TNext> Core(
            ValueTask<Result<T>> resultTask,
            Func<T, ValueTask<TNext>> ifSuccessFunction,
            Func<Error, ValueTask<TNext>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            return result.IsSuccess
                ? await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext)
                : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Executes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and returns
    ///     their output as a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type of the <see cref="Result{TNext}" /> returned by the invoked function.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
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
    ///     The <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{TNext}" /> returned by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(
        this ValueTask<Result<T>> resultTask,
        Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
        Func<Error, ValueTask<Result<TNext>>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> resultTask,
            Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
            Func<Error, ValueTask<Result<TNext>>> ifErrorFunction,
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

    #region ValueTask results with synchronous delegates (#214)

    /// <summary>
    ///     Awaits the <paramref name="resultTask" /> and runs one of the synchronous delegates: the first one if the result is
    ///     successful, the second one with the <see cref="Error" /> if it is failed.
    /// </summary>
    /// <remarks>
    ///     The delegates are synchronous: they run after the <paramref name="resultTask" /> completes. Pass two asynchronous delegates
    ///     for asynchronous work; one synchronous and one asynchronous delegate are not supported.
    /// </remarks>
    /// <param name="resultTask">The asynchronous operation that returns the result to match.</param>
    /// <param name="ifSuccessAction">The synchronous action to run on a successful result. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorAction">The synchronous action to run on a failed result, with its error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the match, as the synchronous <c>Match</c> returns it.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="ifSuccessAction" /> or the <paramref name="ifErrorAction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Action ifSuccessAction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(ValueTask<Result> resultTask, Action ifSuccessAction, Action<Error> ifErrorAction, bool continueOnCapturedContext)
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
    /// <param name="resultTask">The asynchronous operation that returns the result to match.</param>
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
    ///     If the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorFunction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Func<Result> ifSuccessFunction, Func<Error, Result> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            ValueTask<Result> resultTask,
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
    /// <param name="resultTask">The asynchronous operation that returns the result to match.</param>
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
    ///     If the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorFunction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static ValueTask<T> MatchAsync<T>(this ValueTask<Result> resultTask,
        Func<T> ifSuccessFunction, Func<Error, T> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<T> Core(ValueTask<Result> resultTask, Func<T> ifSuccessFunction, Func<Error, T> ifErrorFunction, bool continueOnCapturedContext)
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
    /// <param name="resultTask">The asynchronous operation that returns the result to match.</param>
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
    ///     If the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorFunction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask,
        Func<Result<T>> ifSuccessFunction, Func<Error, Result<T>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(
            ValueTask<Result> resultTask,
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
    /// <param name="resultTask">The asynchronous operation that returns the result to match.</param>
    /// <param name="ifSuccessAction">The synchronous action to run on a successful result, with its value. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorAction">The synchronous action to run on a failed result, with its error. Must not be <see langword="null" />.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegates, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the match, as the synchronous <c>Match</c> returns it.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="ifSuccessAction" /> or the <paramref name="ifErrorAction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result<T>> resultTask,
        Action<T> ifSuccessAction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(
            ValueTask<Result<T>> resultTask,
            Action<T> ifSuccessAction,
            Action<Error> ifErrorAction,
            bool continueOnCapturedContext)
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
    /// <param name="resultTask">The asynchronous operation that returns the result to match.</param>
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
    ///     If the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorFunction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static ValueTask<TNext> MatchAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
        Func<T, TNext> ifSuccessFunction, Func<Error, TNext> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<TNext> Core(
            ValueTask<Result<T>> resultTask,
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
    /// <param name="resultTask">The asynchronous operation that returns the result to match.</param>
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
    ///     If the <paramref name="ifSuccessFunction" /> or the <paramref name="ifErrorFunction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or a function
    ///     returns <see langword="null" /> where a result or an error is required.
    /// </exception>
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
        Func<T, Result<TNext>> ifSuccessFunction, Func<Error, Result<TNext>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> resultTask,
            Func<T, Result<TNext>> ifSuccessFunction,
            Func<Error, Result<TNext>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Match(ifSuccessFunction, ifErrorFunction);
        }
    }

    #endregion

    #region Not supported: one synchronous and one asynchronous delegate, and delegates returning a Task (#214)

    // One synchronous and one asynchronous delegate would bind to the overloads with synchronous delegates, which would drop the task
    // (an async lambda as an action becomes async void) or wrap it in the result (#214). These overloads, the traps of the synchronous
    // Match moved to the task receivers, turn such a call into a compile error (CS0619). The second group covers two delegates that
    // return a Task, which would otherwise be ambiguous (CS0121) between the first group. The overloads whose delegates return a
    // ValueTask have priority 1: an async lambda converts to a Task and to a ValueTask.

    private const string MixedSuccessTaskMessage =
        "The success delegate returns a task, but the error delegate is synchronous. MatchAsync takes two synchronous " +
        "or two asynchronous delegates: make the error delegate asynchronous too, or await the result first and call " +
        "Match.";

    private const string MixedErrorTaskMessage =
        "The error delegate returns a task, but the success delegate is synchronous. MatchAsync takes two synchronous " +
        "or two asynchronous delegates: make the success delegate asynchronous too, or await the result first and call " +
        "Match.";

    private const string OtherTaskMessage =
        "The delegates return a Task, which the ValueTask-based MatchAsync does not await. Import " +
        "Maple.Results.Extensions instead of Maple.Results.Extensions.ValueTasks, or wrap them in a ValueTask (e.g., " +
        "new ValueTask<T>(task)).";

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Func<ValueTask> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> MatchAsync<TNext>(this ValueTask<Result> resultTask,
        Func<ValueTask<TNext>> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Func<Task> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync<TNext>(this ValueTask<Result> resultTask,
        Func<Task<TNext>> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> MatchAsync<T>(this ValueTask<Result<T>> resultTask,
        Func<T, ValueTask> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> MatchAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
        Func<T, ValueTask<TNext>> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync<T>(this ValueTask<Result<T>> resultTask,
        Func<T, Task> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the success delegate returns a task and the error delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedSuccessTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
        Func<T, Task<TNext>> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedSuccessTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Action ifSuccessAction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Action ifSuccessAction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync<TTaskResult>(this ValueTask<Result> resultTask,
        Action ifSuccessAction, Func<Error, Task<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Func<Result> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Func<Result> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync<TTaskResult>(this ValueTask<Result> resultTask,
        Func<Result> ifSuccessFunction, Func<Error, Task<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask,
        Func<T> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask,
        Func<T> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T, TTaskResult>(this ValueTask<Result> resultTask,
        Func<T> ifSuccessFunction, Func<Error, Task<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Func<Error> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Func<Error> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync<TTaskResult>(this ValueTask<Result> resultTask,
        Func<Error> ifSuccessFunction, Func<Error, Task<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask,
        Func<Result<T>> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask,
        Func<Result<T>> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T, TTaskResult>(this ValueTask<Result> resultTask,
        Func<Result<T>> ifSuccessFunction, Func<Error, Task<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result<T>> resultTask,
        Action<T> ifSuccessAction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result<T>> resultTask,
        Action<T> ifSuccessAction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T, TTaskResult>(this ValueTask<Result<T>> resultTask,
        Action<T> ifSuccessAction, Func<Error, Task<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
        Func<T, TNext> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
        Func<T, TNext> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext, TTaskResult>(this ValueTask<Result<T>> resultTask,
        Func<T, TNext> ifSuccessFunction, Func<Error, Task<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> MatchAsync<T>(this ValueTask<Result<T>> resultTask,
        Func<T, Error> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync<T>(this ValueTask<Result<T>> resultTask,
        Func<T, Error> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync<T, TTaskResult>(this ValueTask<Result<T>> resultTask,
        Func<T, Error> ifSuccessFunction, Func<Error, Task<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
        Func<T, Result<TNext>> ifSuccessFunction, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
        Func<T, Result<TNext>> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the error delegate returns a task and the success delegate is synchronous.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(MixedErrorTaskMessage, error: true)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext, TTaskResult>(this ValueTask<Result<T>> resultTask,
        Func<T, Result<TNext>> ifSuccessFunction, Func<Error, Task<TTaskResult>> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(MixedErrorTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>Task</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Func<Task> ifSuccessAction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>Task</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Func<Task<Result>> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>Task</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Func<Task<Result>> ifSuccessFunction, Func<Error, Task<Result>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>Task</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask,
        Func<Task<T>> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>Task</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Func<Task<Error>> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>Task</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static ValueTask<T> MatchAsync<T>(this ValueTask<Result> resultTask,
        Func<Task<T>> ifSuccessFunction, Func<Error, Task<T>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>Task</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask,
        Func<Task<Result<T>>> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>Task</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask,
        Func<Task<Result<T>>> ifSuccessFunction, Func<Error, Task<Result<T>>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>Task</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result<T>> resultTask,
        Func<T, Task> ifSuccessAction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>Task</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
        Func<T, Task<TNext>> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>Task</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static ValueTask<Result> MatchAsync<T>(this ValueTask<Result<T>> resultTask,
        Func<T, Task<Error>> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>Task</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static ValueTask<TNext> MatchAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
        Func<T, Task<TNext>> ifSuccessFunction, Func<Error, Task<TNext>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>Task</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
        Func<T, Task<Result<TNext>>> ifSuccessFunction, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    /// <summary>
    ///     Not supported: the delegates return a <c>Task</c>, which this method does not await.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(OtherTaskMessage, error: true)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
        Func<T, Task<Result<TNext>>> ifSuccessFunction, Func<Error, Task<Result<TNext>>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(OtherTaskMessage);
    }

    #endregion

    #region Not supported: a success function and an error action (#227)

    // A success function with an error action transforms the result, as ThenAsync(…).IfErrorAsync(…) does, and the same call with a
    // success action taps it: which one runs depended on the return type of the lambda (e.g., x => set.Add(x) mapped to a
    // Result<bool>), as for IfSuccess before #197. These overloads keep the signatures and the priorities of the overloads
    // they replace, so no other call changes its binding, and turn such a call into a compile error (CS0619) that names
    // ThenAsync(…).IfErrorAsync(…). MatchAsync keeps two meanings: two functions (a value) and two actions (a side effect).

    private const string SuccessFunctionAndErrorActionMessage =
        "MatchAsync with a success function and an error action transforms the result, as ThenAsync does; an assignment or an increment " +
        "(e.g., x => last = x) is a function too. " +
        "Use ThenAsync(…).IfErrorAsync(…) to transform it, a success delegate without a value (e.g., x => { last = x; }) for a side effect, or " +
        "two functions that return the same type for a value.";

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static ValueTask<Result> MatchAsync(this Result result, Func<ValueTask<Result>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T>(this Result result, Func<ValueTask<T>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static ValueTask<Result> MatchAsync(this Result result, Func<ValueTask<Error>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T>(this Result result, Func<ValueTask<Result<T>>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<TNext>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static ValueTask<Result> MatchAsync<T>(
        this Result<T> result,
        Func<T, ValueTask<Error>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask, Func<ValueTask<Result>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask, Func<ValueTask<T>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask, Func<ValueTask<Error>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<T>> MatchAsync<T>(
        this ValueTask<Result> resultTask, Func<ValueTask<Result<T>>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(
        this ValueTask<Result<T>> resultTask,
        Func<T, ValueTask<TNext>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result> MatchAsync<T>(
        this ValueTask<Result<T>> resultTask,
        Func<T, ValueTask<Error>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(
        this ValueTask<Result<T>> resultTask,
        Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Func<Result> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask,
        Func<T> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask,
        Func<Error> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask,
        Func<Result<T>> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
        Func<T, TNext> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static ValueTask<Result> MatchAsync<T>(this ValueTask<Result<T>> resultTask,
        Func<T, Error> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    /// <summary>
    ///     Not supported: a success function with an error action transforms the result; use
    ///     <c>ThenAsync(…).IfErrorAsync(…)</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(SuccessFunctionAndErrorActionMessage, error: true)]
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
        Func<T, Result<TNext>> ifSuccessFunction, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        throw new NotSupportedException(SuccessFunctionAndErrorActionMessage);
    }

    #endregion
}
