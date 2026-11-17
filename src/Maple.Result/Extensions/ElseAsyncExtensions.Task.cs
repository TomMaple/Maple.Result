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
///     The collection of extension methods that replace a failed <see cref="IOperationResult" />: a function run asynchronously that receives the
///     <see cref="Error" /> returns the replacement result, e.g., a recovered value or another error.
/// </summary>
/// <remarks>
///     A successful result is returned unchanged, and the function is not called. For a side effect that keeps the result unchanged, use
///     <c>IfErrorAsync</c>.
///     <para>
///         The methods for a <see cref="System.Threading.Tasks.Task" /> and for a <see cref="System.Threading.Tasks.ValueTask" /> are in this
///         one class. On a task of a result, the kind of the task is kept, and a function that returns the other kind of task is awaited.
///         On a result, the function decides it: a function that returns a <c>ValueTask</c> gives a <c>ValueTask</c>, and an
///         <see langword="async" /> lambda gives a <c>Task</c> (with an explicit return type, e.g.,
///         <c>async ValueTask&lt;int&gt; (int x) =&gt; …</c>, it gives a <c>ValueTask</c>).
///     </para>
/// </remarks>
public static partial class ElseAsyncExtensions
{
    #region None

    /// <summary>
    ///     Invokes the specified asynchronous function if the <paramref name="result" /> represents an error and returns its outcome;
    ///     otherwise, returns the original <paramref name="result" />.
    /// </summary>
    /// <remarks>
    ///     Use this method to handle error cases in a fluent, asynchronous manner without affecting successful results.
    ///     The <paramref name="ifErrorFunction" /> is only invoked if the <paramref name="result" /> indicates an error.
    /// </remarks>
    /// <typeparam name="TResult">
    ///     The type of the <paramref name="result" />, which must implement the <see cref="IOperationResult" /> interface.
    /// </typeparam>
    /// <param name="result">The result to evaluate for success or error. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorFunction">
    ///     The function to invoke asynchronously if the <paramref name="result" /> represents an error.
    ///     The function receives the error and returns a new result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{IOperationResult}" /> that represents the <see cref="IOperationResult" /> outcome of the asynchronous operation.
    ///     The <see cref="Task{IOperationResult}" /> is either the original <paramref name="result" /> if it indicates success,
    ///     or the result returned by the <paramref name="ifErrorFunction" /> if an <see cref="Error" /> is present.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<TResult> ElseAsync<TResult>(
        [DisallowNull] this TResult result,
        Func<Error, Task<TResult>> ifErrorFunction,
        bool? continueOnCapturedContext = null
    )
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TResult> Core(TResult result, Func<Error, Task<TResult>> ifErrorFunction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
                return result;

            var errorTask = ifErrorFunction(result.Error);

            return await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the <paramref name="resultTask" /> represents an error operation and returns its outcome;
    ///     otherwise, returns the outcome of the <paramref name="resultTask" /> asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use this method to handle error cases in a fluent, asynchronous manner without affecting successful results.
    ///         The <paramref name="ifErrorFunction" /> is only invoked if the outcome of the <paramref name="resultTask" /> asynchronous operation indicates
    ///         an error.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TResult">
    ///     The type of the operation result object, which must implement the <see cref="IOperationResult" /> interface.
    /// </typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function to invoke asynchronously if the <paramref name="resultTask" /> returns an error.
    ///     The function receives the error and returns a new result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{IOperationResult}" /> that represents the <see cref="IOperationResult" /> outcome of the asynchronous operation.
    ///     The <see cref="Task{IOperationResult}" /> is either the outcome of the original <paramref name="resultTask" /> if it indicates success,
    ///     or the result returned by the <paramref name="ifErrorFunction" /> if an <see cref="Error" /> is present.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<TResult> ElseAsync<TResult>(
        this Task<TResult> resultTask,
        Func<Error, Task<TResult>> ifErrorFunction,
        bool? continueOnCapturedContext = null
    )
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TResult> Core(Task<TResult> resultTask, Func<Error, Task<TResult>> ifErrorFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
                return result;

            var errorTask = ifErrorFunction(result.Error);

            return await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }
    #endregion

    #region A value or an Error in a task

    // Else takes a function that returns a value or an Error through the implicit conversions to the result type. A task blocks
    // them: Task<int> does not convert to Task<Result<int>>, and the type argument of the task contradicts the receiver in the type
    // inference, so these overloads give ElseAsync the same functions (#237). The overloads on a task receiver have priority 1, as
    // the generic one: an async lambda converts to a Task and to a ValueTask, and the traps for a ValueTask would be ambiguous.

    /// <summary>
    ///     Invokes the specified asynchronous function if the <paramref name="result" /> is failed and returns a successful
    ///     <see cref="Result{T}" /> with the value that it returns; otherwise, returns the original <paramref name="result" />.
    /// </summary>
    /// <remarks>
    ///     Use it to recover from an error with a value, e.g., <c>result.ElseAsync(async error =&gt; await LoadDefaultAsync())</c>, as
    ///     <c>Else(error =&gt; 0)</c> does. A <see langword="null" /> value is a valid value.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the <paramref name="result" />.</typeparam>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for an error. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function to invoke if the <paramref name="result" /> is failed. It receives the <see cref="Error" /> and returns
    ///     the value of the successful <see cref="Result{T}" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the original <paramref name="result" /> if it is successful; otherwise, a successful
    ///     <see cref="Result{T}" /> with the value returned by the <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task.</exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> ElseAsync<T>(this Result<T> result, Func<Error, Task<T>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Result<T> result, Func<Error, Task<T>> ifErrorFunction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
                return result;

            var recoveryTask = ifErrorFunction(result.Error);
            var checkedTask = Guard.EnsureNotNullTask(recoveryTask, nameof(ifErrorFunction));
            var value = await checkedTask.ConfigureAwait(continueOnCapturedContext);

            return Result<T>.FromValue(value);
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the <paramref name="result" /> is failed and returns a failed
    ///     <see cref="Result{T}" /> with the <see cref="Error" /> that it returns; otherwise, returns the original <paramref name="result" />.
    /// </summary>
    /// <remarks>
    ///     Use it to replace an error with another one, e.g., <c>result.ElseAsync(async error =&gt; await TranslateAsync(error))</c>, as
    ///     <c>Else(error =&gt; error.WithMetadata(…))</c> does. When <typeparamref name="T" /> is <see cref="Error" />, a returned
    ///     <see cref="Error" /> is the error of a failed result too; return <c>Result&lt;Error&gt;.FromValue(…)</c> for a successful one.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the <paramref name="result" />.</typeparam>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for an error. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function to invoke if the <paramref name="result" /> is failed. It receives the <see cref="Error" /> and returns
    ///     the <see cref="Error" /> of the failed <see cref="Result{T}" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the original <paramref name="result" /> if it is successful; otherwise, a failed
    ///     <see cref="Result{T}" /> with the <see cref="Error" /> returned by the <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> ElseAsync<T>(this Result<T> result, Func<Error, Task<Error>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Result<T> result, Func<Error, Task<Error>> ifErrorFunction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
                return result;

            var errorTask = ifErrorFunction(result.Error);
            var error = await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);

            return Result<T>.FromError(error);
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the <paramref name="result" /> is failed and returns a failed
    ///     <see cref="Result" /> with the <see cref="Error" /> that it returns; otherwise, returns the original <paramref name="result" />.
    /// </summary>
    /// <remarks>
    ///     Use it to replace an error with another one, e.g., <c>result.ElseAsync(async error =&gt; await TranslateAsync(error))</c>, as
    ///     <c>Else(error =&gt; error.WithMetadata(…))</c> does.
    /// </remarks>
    /// <param name="result">The <see cref="Result" /> to evaluate for an error. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function to invoke if the <paramref name="result" /> is failed. It receives the <see cref="Error" /> and returns
    ///     the <see cref="Error" /> of the failed <see cref="Result" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the original <paramref name="result" /> if it is successful; otherwise, a failed
    ///     <see cref="Result" /> with the <see cref="Error" /> returned by the <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> ElseAsync(this Result result, Func<Error, Task<Error>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Result result, Func<Error, Task<Error>> ifErrorFunction, bool continueOnCapturedContext)
        {
            if (result.IsSuccess)
                return result;

            var errorTask = ifErrorFunction(result.Error);
            var error = await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);

            return Result.FromError(error);
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the result of the <paramref name="resultTask" /> is failed and returns a successful
    ///     <see cref="Result{T}" /> with the value that it returns; otherwise, returns the result of the <paramref name="resultTask" />.
    /// </summary>
    /// <remarks>
    ///     Use it to recover from an error with a value, e.g., <c>resultTask.ElseAsync(async error =&gt; await LoadDefaultAsync())</c>, as
    ///     <c>Else(error =&gt; 0)</c> does. A <see langword="null" /> value is a valid value.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function to invoke if the result is failed. It receives the <see cref="Error" /> and returns the value of the
    ///     successful <see cref="Result{T}" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the result of the <paramref name="resultTask" /> if it is successful; otherwise, a
    ///     successful <see cref="Result{T}" /> with the value returned by the <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> ElseAsync<T>(this Task<Result<T>> resultTask, Func<Error, Task<T>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Task<Result<T>> resultTask, Func<Error, Task<T>> ifErrorFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
                return result;

            var recoveryTask = ifErrorFunction(result.Error);
            var checkedTask = Guard.EnsureNotNullTask(recoveryTask, nameof(ifErrorFunction));
            var value = await checkedTask.ConfigureAwait(continueOnCapturedContext);

            return Result<T>.FromValue(value);
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the result of the <paramref name="resultTask" /> is failed and returns a failed
    ///     <see cref="Result{T}" /> with the <see cref="Error" /> that it returns; otherwise, returns the result of the
    ///     <paramref name="resultTask" />.
    /// </summary>
    /// <remarks>
    ///     Use it to replace an error with another one, e.g., <c>resultTask.ElseAsync(async error =&gt; await TranslateAsync(error))</c>, as
    ///     <c>Else(error =&gt; error.WithMetadata(…))</c> does. When <typeparamref name="T" /> is <see cref="Error" />, a returned
    ///     <see cref="Error" /> is the error of a failed result too; return <c>Result&lt;Error&gt;.FromValue(…)</c> for a successful one.
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function to invoke if the result is failed. It receives the <see cref="Error" /> and returns the
    ///     <see cref="Error" /> of the failed <see cref="Result{T}" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the result of the <paramref name="resultTask" /> if it is successful; otherwise, a
    ///     failed <see cref="Result{T}" /> with the <see cref="Error" /> returned by the <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> ElseAsync<T>(this Task<Result<T>> resultTask, Func<Error, Task<Error>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Task<Result<T>> resultTask, Func<Error, Task<Error>> ifErrorFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
                return result;

            var errorTask = ifErrorFunction(result.Error);
            var error = await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);

            return Result<T>.FromError(error);
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the result of the <paramref name="resultTask" /> is failed and returns a failed
    ///     <see cref="Result" /> with the <see cref="Error" /> that it returns; otherwise, returns the result of the <paramref name="resultTask" />.
    /// </summary>
    /// <remarks>
    ///     Use it to replace an error with another one, e.g., <c>resultTask.ElseAsync(async error =&gt; await TranslateAsync(error))</c>, as
    ///     <c>Else(error =&gt; error.WithMetadata(…))</c> does.
    /// </remarks>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function to invoke if the result is failed. It receives the <see cref="Error" /> and returns the
    ///     <see cref="Error" /> of the failed <see cref="Result" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the result of the <paramref name="resultTask" /> if it is successful; otherwise, a
    ///     failed <see cref="Result" /> with the <see cref="Error" /> returned by the <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifErrorFunction" /> returns a <see langword="null" /> task or a task with a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> ElseAsync(this Task<Result> resultTask, Func<Error, Task<Error>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result> resultTask, Func<Error, Task<Error>> ifErrorFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
                return result;

            var errorTask = ifErrorFunction(result.Error);
            var error = await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);

            return Result.FromError(error);
        }
    }
    #endregion

    #region Not supported: functions returning a task without a value, on task results

    // A function that returns a task without a value is a side effect, not a transformation (#197). On a task result of a
    // Result<object>, it would bind to the overload with a synchronous function and give a successful result that holds the task, which
    // nobody awaits: these overloads turn the call into a compile error (CS0619) that names the method to use, for both kinds of task. On a result, and for a
    // synchronous action, the compiler reports the call by itself (e.g., CS0029, CS0266 or CS1643), so there are no overloads for them (#256).

    private const string ActionMessage =
        "The function returns a task without a value. ElseAsync needs a function whose task returns the replacement " +
        "result; use IfErrorAsync for a side effect.";

    /// <summary>
    ///     Not supported: the function returns a task without a value; use <c>IfErrorAsync</c> for a side effect.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Task<TResult> ElseAsync<TResult>(this Task<TResult> resultTask, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ActionMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task without a value; use <c>IfErrorAsync</c> for a side effect.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionMessage, error: true)]
    public static Task<TResult> ElseAsync<TResult>(this Task<TResult> resultTask,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ActionMessage);
    }
    #endregion

    #region Task results with synchronous delegates

    // See issue #214.

    /// <summary>
    ///     Runs the synchronous function if the result that the <paramref name="resultTask" /> returns is failed, and returns
    ///     the result that the function returns instead (e.g., a recovered value or another error).
    /// </summary>
    /// <remarks>
    ///     The function is synchronous: it runs after the <paramref name="resultTask" /> completes, and it is not called for a
    ///     successful result, which is returned unchanged.
    /// </remarks>
    /// <typeparam name="TResult">The type of the result: <see cref="Result" /> or <see cref="Result{T}" />.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorFunction">
    ///     The synchronous function that receives the error and returns the replacement result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread);
    ///     <see langword="false" /> to run it on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>The result of the function, or the original successful result.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" /> or the <paramref name="ifErrorFunction" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />, or the
    ///     <paramref name="ifErrorFunction" /> returns <see langword="null" />.
    /// </exception>
    public static Task<TResult> ElseAsync<TResult>(this Task<TResult> resultTask,
        Func<Error, TResult> ifErrorFunction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TResult> Core(Task<TResult> resultTask, Func<Error, TResult> ifErrorFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);
            return result.Else(ifErrorFunction);
        }
    }
    #endregion

    #region Task results with functions returning a ValueTask

    // See issue #226.

    /// <summary>
    ///     Invokes the specified asynchronous function if the <paramref name="resultTask" /> represents an error operation and returns its outcome;
    ///     otherwise, returns the outcome of the <paramref name="resultTask" /> asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use this method to handle error cases in a fluent, asynchronous manner without affecting successful results.
    ///         The <paramref name="ifErrorFunction" /> is only invoked if the outcome of the <paramref name="resultTask" /> asynchronous operation indicates
    ///         an error.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    ///     </para>
    ///     <para>
    ///         The function returns a <see cref="ValueTask" />, the other kind of task: it is awaited, and a <see cref="Task" />
    ///         is returned, as for a function that returns a <see cref="Task" />.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TResult">
    ///     The type of the operation result object, which must implement the <see cref="IOperationResult" /> interface.
    /// </typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function to invoke asynchronously if the <paramref name="resultTask" /> returns an error.
    ///     The function receives the error and returns a new result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{IOperationResult}" /> that represents the <see cref="IOperationResult" /> outcome of the asynchronous operation.
    ///     The <see cref="Task{IOperationResult}" /> is either the outcome of the original <paramref name="resultTask" /> if it indicates success,
    ///     or the result returned by the <paramref name="ifErrorFunction" /> if an <see cref="Error" /> is present.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    public static Task<TResult> ElseAsync<TResult>(
        this Task<TResult> resultTask,
        Func<Error, ValueTask<TResult>> ifErrorFunction,
        bool? continueOnCapturedContext = null
    )
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<TResult> Core(Task<TResult> resultTask, Func<Error, ValueTask<TResult>> ifErrorFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
                return result;

            var errorTask = ifErrorFunction(result.Error);

            return await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the result of the <paramref name="resultTask" /> is failed and returns a successful
    ///     <see cref="Result{T}" /> with the value that it returns; otherwise, returns the result of the <paramref name="resultTask" />.
    /// </summary>
    /// <remarks>
    ///     Use it to recover from an error with a value, e.g., <c>resultTask.ElseAsync(async error =&gt; await LoadDefaultAsync())</c>, as
    ///     <c>Else(error =&gt; 0)</c> does. A <see langword="null" /> value is a valid value.
    ///     <para>
    ///         The function returns a <see cref="ValueTask" />, the other kind of task: it is awaited, and a <see cref="Task" />
    ///         is returned, as for a function that returns a <see cref="Task" />.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function to invoke if the result is failed. It receives the <see cref="Error" /> and returns the value of the
    ///     successful <see cref="Result{T}" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the result of the <paramref name="resultTask" /> if it is successful; otherwise, a
    ///     successful <see cref="Result{T}" /> with the value returned by the <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task<Result<T>> ElseAsync<T>(this Task<Result<T>> resultTask, Func<Error, ValueTask<T>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Task<Result<T>> resultTask, Func<Error, ValueTask<T>> ifErrorFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
                return result;

            var recoveryTask = ifErrorFunction(result.Error);
            var checkedTask = recoveryTask;
            var value = await checkedTask.ConfigureAwait(continueOnCapturedContext);

            return Result<T>.FromValue(value);
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the result of the <paramref name="resultTask" /> is failed and returns a failed
    ///     <see cref="Result{T}" /> with the <see cref="Error" /> that it returns; otherwise, returns the result of the
    ///     <paramref name="resultTask" />.
    /// </summary>
    /// <remarks>
    ///     Use it to replace an error with another one, e.g., <c>resultTask.ElseAsync(async error =&gt; await TranslateAsync(error))</c>, as
    ///     <c>Else(error =&gt; error.WithMetadata(…))</c> does. When <typeparamref name="T" /> is <see cref="Error" />, a returned
    ///     <see cref="Error" /> is the error of a failed result too; return <c>Result&lt;Error&gt;.FromValue(…)</c> for a successful one.
    ///     <para>
    ///         The function returns a <see cref="ValueTask" />, the other kind of task: it is awaited, and a <see cref="Task" />
    ///         is returned, as for a function that returns a <see cref="Task" />.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the result.</typeparam>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function to invoke if the result is failed. It receives the <see cref="Error" /> and returns the
    ///     <see cref="Error" /> of the failed <see cref="Result{T}" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the result of the <paramref name="resultTask" /> if it is successful; otherwise, a
    ///     failed <see cref="Result{T}" /> with the <see cref="Error" /> returned by the <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    public static Task<Result<T>> ElseAsync<T>(this Task<Result<T>> resultTask, Func<Error, ValueTask<Error>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Task<Result<T>> resultTask, Func<Error, ValueTask<Error>> ifErrorFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
                return result;

            var errorTask = ifErrorFunction(result.Error);
            var error = await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);

            return Result<T>.FromError(error);
        }
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the result of the <paramref name="resultTask" /> is failed and returns a failed
    ///     <see cref="Result" /> with the <see cref="Error" /> that it returns; otherwise, returns the result of the <paramref name="resultTask" />.
    /// </summary>
    /// <remarks>
    ///     Use it to replace an error with another one, e.g., <c>resultTask.ElseAsync(async error =&gt; await TranslateAsync(error))</c>, as
    ///     <c>Else(error =&gt; error.WithMetadata(…))</c> does.
    ///     <para>
    ///         The function returns a <see cref="ValueTask" />, the other kind of task: it is awaited, and a <see cref="Task" />
    ///         is returned, as for a function that returns a <see cref="Task" />.
    ///     </para>
    /// </remarks>
    /// <param name="resultTask">The asynchronous operation that returns the result to inspect. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function to invoke if the result is failed. It receives the <see cref="Error" /> and returns the
    ///     <see cref="Error" /> of the failed <see cref="Result" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the result of the <paramref name="resultTask" /> if it is successful; otherwise, a
    ///     failed <see cref="Result" /> with the <see cref="Error" /> returned by the <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    public static Task<Result> ElseAsync(this Task<Result> resultTask, Func<Error, ValueTask<Error>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result> resultTask, Func<Error, ValueTask<Error>> ifErrorFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
                return result;

            var errorTask = ifErrorFunction(result.Error);
            var error = await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);

            return Result.FromError(error);
        }
    }
    #endregion

}
