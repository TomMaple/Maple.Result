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
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Maple.Results.Extensions;

/// <summary>
///     The collection of extension methods that replace a failed <see cref="IOperationResult" />: a function run asynchronously that receives the
///     <see cref="Error" /> returns the replacement result, e.g., a recovered value or another error.
/// </summary>
/// <remarks>
///     A successful result is returned unchanged, and the function is not called. For a side effect that keeps the result unchanged, use
///     <c>IfErrorAsync</c>.
/// </remarks>
public static class ElseTaskAsyncExtensions
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
    public static Task<TResult> IfErrorAsync<TResult>(
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
    public static Task<TResult> IfErrorAsync<TResult>(
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
}
