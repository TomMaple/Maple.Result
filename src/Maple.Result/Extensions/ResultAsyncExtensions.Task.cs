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
using System.Threading.Tasks;

namespace Maple.Results.Extensions;

/// <summary>
///     The collection of asynchronous extension methods for the <see cref="Result{T}" /> type.
/// </summary>
/// <remarks>
///     <para>
///         The methods for a <see cref="System.Threading.Tasks.Task" /> and for a <see cref="System.Threading.Tasks.ValueTask" /> of a result
///         are in this one class; the kind of the task is kept.
///     </para>
/// </remarks>
public static partial class ResultAsyncExtensions
{
    /// <summary>
    ///     Asynchronously converts a task that produces a generic result into a non-generic result, preserving success or <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     Use this method to convert a <see cref="Task{TResult}" /> of a <see cref="Result{T}" /> to a <see cref="Task{TResult}" /> of a
    ///     <see cref="Result" /> when the value is not needed but the success or error state with the original <see cref="Error" /> must be preserved.
    /// </remarks>
    /// <typeparam name="T">The type of the value contained in the original result.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A task that represents the <see cref="Result" /> without a value.
    ///     The result indicates success if the original result was successful; otherwise, it contains the <see cref="Error" /> from the original result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static Task<Result> ToResultAsync<T>(this Task<Result<T>> resultTask, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(resultTask);

        return Core(resultTask, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Task<Result<T>> resultTask, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            return result.IsSuccess
                ? Result.Success()
                : Result.FromError(result.Error);
        }
    }
}
