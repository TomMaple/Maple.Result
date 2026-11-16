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

namespace Maple.Results.Extensions.ValueTasks;

/// <summary>
///     The collection of extension methods that replace a failed <see cref="IOperationResult" />: a function, represented by a <see cref="ValueTask"
///     />, that receives the <see cref="Error" /> returns the replacement result, e.g., a recovered value or another error.
/// </summary>
/// <remarks>
///     A successful result is returned unchanged, and the function is not called. For a side effect that keeps the result unchanged, use
///     <c>ElseAsync</c>.
///     <para>
///         These methods are declared in the <c>Maple.Results.Extensions.ValueTasks</c> namespace, separately from their
///         <see cref="System.Threading.Tasks.Task" />-based counterparts in <c>Maple.Results.Extensions</c>, because both sets declare the same method names and
///         an asynchronous lambda is convertible to a <c>Func&lt;Task&gt;</c> as well as to a <c>Func&lt;ValueTask&gt;</c> delegate.
///         Importing only one of these namespaces keeps such calls unambiguous.
///     </para>
/// </remarks>
public static class ElseValueTaskAsyncExtensions
{
    #region None

    /// <returns>
    ///     A <see cref="ValueTask{IOperationResult}" /> that represents the <see cref="IOperationResult" /> outcome of the asynchronous operation.
    ///     The <see cref="ValueTask{IOperationResult}" /> is either the original <paramref name="result" /> if it indicates success,
    ///     or the result returned by the <paramref name="ifErrorFunction" /> if an <see cref="Error" /> is present.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    /// <inheritdoc cref="ElseTaskAsyncExtensions.ElseAsync{TResult}(TResult, Func{Error, Task{TResult}}, bool?)" />
    public static ValueTask<TResult> ElseAsync<TResult>(
        [DisallowNull] this TResult result,
        Func<Error, ValueTask<TResult>> ifErrorFunction,
        bool? continueOnCapturedContext = null
    )
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<TResult> Core(TResult result, Func<Error, ValueTask<TResult>> ifErrorFunction, bool continueOnCapturedContext)
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
    /// <param name="resultTask">The asynchronous operation to be executed and which result is to be inspected.</param>
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
    ///     A <see cref="ValueTask{IOperationResult}" /> that represents the <see cref="IOperationResult" /> outcome of the asynchronous operation.
    ///     The <see cref="ValueTask{IOperationResult}" /> is either the outcome of the original <paramref name="resultTask" /> if it indicates success,
    ///     or the result returned by the <paramref name="ifErrorFunction" /> if an <see cref="Error" /> is present.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="ifErrorFunction" /> parameter is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<TResult> ElseAsync<TResult>(
        this ValueTask<TResult> resultTask,
        Func<Error, ValueTask<TResult>> ifErrorFunction,
        bool? continueOnCapturedContext = null
    )
        where TResult : IOperationResult
    {
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<TResult> Core(ValueTask<TResult> resultTask, Func<Error, ValueTask<TResult>> ifErrorFunction, bool continueOnCapturedContext)
        {
            var result = await Guard.EnsureNotNullAwaited(resultTask, nameof(resultTask), continueOnCapturedContext);

            if (result.IsSuccess)
                return result;

            var errorTask = ifErrorFunction(result.Error);

            return await Guard.EnsureNotNullResult(errorTask, nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    #endregion

    #region Not supported: actions (#197)

    // A function that returns nothing is a side effect, not a transformation (#197): these overloads turn the call into a compile
    // error (CS0619) that names the method to use, instead of the conversion errors CS0029 and CS1662.

    private const string ActionMessage =
        "The function returns a task without a value. ElseAsync needs a function whose task returns the replacement " +
        "result; use IfErrorAsync for a side effect.";

    /// <summary>
    ///     Not supported: the function returns a task without a value; use <c>IfErrorAsync</c> for a side effect.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionMessage, error: true)]
    public static ValueTask<TResult> ElseAsync<TResult>([DisallowNull] this TResult result, Func<Error, ValueTask> ifErrorFunction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ActionMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task without a value; use <c>IfErrorAsync</c> for a side effect.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(ActionMessage, error: true)]
    public static ValueTask<TResult> ElseAsync<TResult>(this ValueTask<TResult> resultTask, Func<Error, ValueTask> ifErrorFunction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult
    {
        throw new NotSupportedException(ActionMessage);
    }

    #endregion
}
