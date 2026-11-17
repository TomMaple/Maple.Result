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

namespace Maple.Results.Extensions;

/// <summary>
///     The collection of extension methods that transform or chain a successful <see cref="Result" /> or <see cref="Result{T}" />: a function that
///     returns a value, a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> gives the next result.
/// </summary>
/// <remarks>
///     A failed result is returned unchanged, and the function is not called. A returned <see cref="Result" /> or <see cref="Result{T}" /> is
///     flattened, and a returned <see cref="Error" /> gives a failed result. For a side effect that keeps the result unchanged, use
///     <c>IfSuccess</c>.
/// </remarks>
public static partial class ThenExtensions
{
    #region Result extensions

    /// <summary>
    ///     Invokes the specified function if the <paramref name="result" /> represents a successful outcome;
    ///     otherwise, returns an error <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional operations that should only execute if the previous <see cref="Result" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function to execute if <paramref name="result" /> is successful.
    ///     The function must return a <see cref="Result" /> and must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Result" /> returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, the original <paramref name="result" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessFunction" /> returns <see langword="null" />.</exception>
    public static Result Then(this Result result, Func<Result> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess)
        {
            var successResult = ifSuccessFunction();
            return Guard.EnsureNotNullReturned(successResult, nameof(ifSuccessFunction));
        }

        return result;
    }

    /// <summary>
    ///     Invokes the specified function if the <paramref name="result" /> represents a successful outcome and returns a failed <see cref="Result" />
    ///     with the <see cref="Error" /> it returns; otherwise, returns the original <paramref name="result" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use it to turn a successful <see cref="Result" /> into a failure, e.g., <c>result.Then(() => Error.Conflict(…))</c>.
    ///         The returned <see cref="Result" /> is always failed.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a call would bind to <see cref="Then{T}(Result, Func{T})" /> and return a <b>successful</b>
    ///         <c>Result&lt;Error&gt;</c>. To get that on purpose, specify the type argument: <c>Then&lt;Error&gt;(…)</c>.
    ///     </para>
    /// </remarks>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function to execute if <paramref name="result" /> is successful.
    ///     It returns the <see cref="Error" /> of the failed <see cref="Result" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A failed <see cref="Result" /> with the <see cref="Error" /> returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is
    ///     successful; otherwise, the original <paramref name="result" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessFunction" /> returns <see langword="null" />.</exception>
    public static Result Then(this Result result, Func<Error> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess)
        {
            var returnedError = ifSuccessFunction();
            var error = Guard.EnsureNotNullReturned(returnedError, nameof(ifSuccessFunction));

            return Result.FromError(error);
        }

        return result;
    }

    /// <summary>
    ///     Invokes the specified function and returns its output as a <see cref="Result{T}" /> if the operation represented by the <paramref name="result" />
    ///     is successful; otherwise, returns a failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional operations that should only execute if the previous <see cref="Result" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> value to return if the operation is successful.</typeparam>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function to execute if the <paramref name="result" /> is successful.
    ///     The function’s return value is used as the successful <see cref="Result{T}" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A successful <see cref="Result{T}" /> containing the value returned by <paramref name="ifSuccessFunction" />
    ///     if <paramref name="result" /> is successful; otherwise, a failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<T> Then<T>(this Result result, Func<T> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess)
            return ifSuccessFunction();

        return result.Error;
    }

    /// <summary>
    ///     Invokes the specified function and returns a <see cref="Result{T}" /> with its output if the operation represented by the <paramref name="result" />
    ///     is successful; otherwise, returns a failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional operations that should only execute if the previous <see cref="Result" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> value to return if the operation is successful.</typeparam>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function to execute if the <paramref name="result" /> is successful.
    ///     The function’s return value is used as the successful <see cref="Result{T}" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A successful <see cref="Result{T}" /> returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessFunction" /> returns <see langword="null" />.</exception>
    public static Result<T> Then<T>(this Result result, Func<Result<T>> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess)
        {
            var successResult = ifSuccessFunction();
            return Guard.EnsureNotNullReturned(successResult, nameof(ifSuccessFunction));
        }

        return result.Error;
    }

    #endregion

    #region Result<T> extensions

    /// <summary>
    ///     Invokes the specified function if the <paramref name="result" /> represents a successful outcome;
    ///     otherwise, returns an error <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional operations that should only execute if the previous <see cref="Result" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function to execute if <paramref name="result" /> is successful. The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     The function must return a <see cref="Result" /> and must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Result" /> returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessFunction" /> returns <see langword="null" />.</exception>
    public static Result Then<T>(this Result<T> result, Func<T, Result> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess)
        {
            var successResult = ifSuccessFunction(result.Value);
            return Guard.EnsureNotNullReturned(successResult, nameof(ifSuccessFunction));
        }

        return result.Error;
    }

    /// <summary>
    ///     Invokes the specified function if the <paramref name="result" /> represents a successful outcome and returns a failed <see cref="Result" />
    ///     with the <see cref="Error" /> it returns; otherwise, returns a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use it to turn a successful <see cref="Result{T}" /> into a failure, e.g., <c>result.Then(user => Error.Conflict(…))</c>.
    ///         The returned <see cref="Result" /> is always failed.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a call would bind to <see cref="Then{T, TNext}(Result{T}, Func{T, TNext})" /> and return
    ///         a <b>successful</b> <c>Result&lt;Error&gt;</c>. To get that on purpose, specify the type arguments: <c>Then&lt;T, Error&gt;(…)</c>.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the <paramref name="result" />.</typeparam>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function to execute if <paramref name="result" /> is successful. The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     It returns the <see cref="Error" /> of the failed <see cref="Result" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A failed <see cref="Result" /> with the <see cref="Error" /> returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is
    ///     successful; otherwise, a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessFunction" /> returns <see langword="null" />.</exception>
    public static Result Then<T>(this Result<T> result, Func<T, Error> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess)
        {
            var returnedError = ifSuccessFunction(result.Value);
            var error = Guard.EnsureNotNullReturned(returnedError, nameof(ifSuccessFunction));

            return Result.FromError(error);
        }

        return result.Error;
    }

    /// <summary>
    ///     Invokes the specified function and returns its output as a <see cref="Result{TNext}" /> if the operation represented by
    ///     the <paramref name="result" /> is successful; otherwise, returns a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional operations that should only execute if the previous <see cref="Result{T}" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
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
    ///     The function to execute if the <paramref name="result" /> is successful. The value of the successful <see cref="Result{T}" /> is passed as
    ///     a parameter. The function’s return value is used as the successful <see cref="Result{TNext}" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A successful <see cref="Result{TNext}" /> containing the value returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is
    ///     successful; otherwise, a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<TNext> Then<T, TNext>(this Result<T> result, Func<T, TNext> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess)
            return ifSuccessFunction(result.Value);

        return result.Error;
    }

    /// <summary>
    ///     Invokes the specified function and returns a <see cref="Result{TNext}" /> with its output if the operation represented by
    ///     the <paramref name="result" /> is successful; otherwise, returns a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional operations that should only execute if the previous <see cref="Result{T}" /> was successful.
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
    ///     The function to execute if the <paramref name="result" /> is successful. The value of the successful <see cref="Result{T}" /> is passed as
    ///     a parameter. The function’s must return <see cref="Result{TNext}" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A successful <see cref="Result{TNext}" /> returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessFunction" /> returns <see langword="null" />.</exception>
    public static Result<TNext> Then<T, TNext>(this Result<T> result, Func<T, Result<TNext>> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess)
        {
            var successResult = ifSuccessFunction(result.Value);
            return Guard.EnsureNotNullReturned(successResult, nameof(ifSuccessFunction));
        }

        return result.Error;
    }

    #endregion
}
