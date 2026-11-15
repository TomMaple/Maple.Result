// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Maple.Result;

/// <summary>
///     Defines the contract for an operation result, providing information about success or failure and any associated error details.
/// </summary>
/// <remarks>
///     Use the <see cref="IsSuccess" /> or <see cref="IsError" /> property to determine whether the operation completed successfully,
///     and access the <see cref="Error" /> property for error information if the operation failed.
/// </remarks>
public interface IOperationResult
{
    /// <summary>
    ///     Returns an <see cref="Error" /> object if the operation failed; otherwise, returns <see langword="null" />.
    /// </summary>
    public Error? Error { get; }

    /// <summary>
    ///     Gets a value indicating whether the operation was successful.
    /// </summary>
    /// <value>
    ///     <see langword="true" /> if the operation was successful; otherwise, <see langword="false" />.
    /// </value>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }

    /// <summary>
    ///     Gets a value indicating whether the operation failed.
    /// </summary>
    /// <value>
    ///     <see langword="true" /> if the operation failed and <see cref="Error" /> is not <see langword="null" />; otherwise, <see langword="false" />.
    /// </value>
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsError { get; }

    /// <summary>
    ///     Gets the <see cref="Error" /> of a failed result.
    /// </summary>
    /// <remarks>
    ///     For the early return in one statement: <c>if (result.TryGetError(out var error)) return error;</c>.
    /// </remarks>
    /// <param name="error">
    ///     When this method returns, the <see cref="Error" /> if the result is failed; otherwise, <see langword="null" />.
    /// </param>
    /// <returns><see langword="true" /> if the result is failed; otherwise, <see langword="false" />.</returns>
    public bool TryGetError([NotNullWhen(true)] out Error? error);
}

/// <summary>
///     Defines a result of an operation that can either be successful or contain an error.
/// </summary>
/// <remarks>
///     The record is immutable. Create it with <see cref="Success" />, <see cref="FromError" /> or the implicit conversion from an <see cref="Error" />.
/// </remarks>
/// <inheritdoc cref="IOperationResult" />
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed record Result : IOperationResult
{
    private static readonly Result SuccessResult = new(null);

    #region constructors

    // The only constructor takes a parameter on purpose: a parameterless one (even a private one) lets
    // Newtonsoft.Json create an instance and silently read every payload, including a failed one, as a success.
    private Result(Error? error)
    {
        Error = error;
    }

    #endregion

    /// <summary>
    ///     Returns an <see cref="Error" /> object if the operation failed; otherwise, returns <see langword="null" />.
    /// </summary>
    public Error? Error { get; }

    /// <summary>
    ///     Gets a value indicating whether the operation was successful.
    /// </summary>
    /// <value>
    ///     <see langword="true" /> if the operation was successful; otherwise, <see langword="false" />.
    /// </value>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => Error is null;

    /// <summary>
    ///     Gets a value indicating whether the operation failed.
    /// </summary>
    /// <value>
    ///     <see langword="true" /> if the operation failed and <see cref="Error" /> is not <see langword="null" />; otherwise, <see langword="false" />.
    /// </value>
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsError => Error is not null;

    /// <summary>
    ///     Gets the <see cref="Error" /> of a failed result.
    /// </summary>
    /// <remarks>
    ///     For the early return in one statement: <c>if (result.TryGetError(out var error)) return error;</c>.
    /// </remarks>
    /// <param name="error">
    ///     When this method returns, the <see cref="Error" /> if the result is failed; otherwise, <see langword="null" />.
    /// </param>
    /// <returns><see langword="true" /> if the result is failed; otherwise, <see langword="false" />.</returns>
    public bool TryGetError([NotNullWhen(true)] out Error? error)
    {
        error = Error;
        return error is not null;
    }

    /// <summary>
    ///     Returns a successful <see cref="Result" /> instance.
    /// </summary>
    /// <returns>A successful <see cref="Result" /> instance (always the same, shared instance).</returns>
    public static Result Success()
    {
        return SuccessResult;
    }

    #region implicit operators

    /// <summary>
    ///     Converts an <see cref="Error" /> to a failed <see cref="Result" /> with the specified error.
    /// </summary>
    /// <param name="error">The <see cref="Error" /> to convert.</param>
    /// <returns>A failed <see cref="Result" /> instance with the provided error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="error" /> is <see langword="null" />.</exception>
    public static implicit operator Result(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new Result(error);
    }

    #endregion

    #region factory methods

    /// <summary>
    ///     Creates a <see cref="Result" /> instance representing a failure with the specified <see cref="Error" />.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the <paramref name="error" /> is <see langword="null" />.</exception>
    public static Result FromError(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new Result(error);
    }

    /// <summary>
    ///     Creates a successful <see cref="Result{T}" /> instance containing the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="value" />.</typeparam>
    /// <remarks>
    ///     A <see langword="null" /> <paramref name="value" /> is permitted when <typeparamref name="T" /> is nullable
    ///     (a nullable reference type or <see cref="Nullable{T}" />);
    ///     the resulting <see cref="Result{T}" /> is successful and its <see cref="Result{T}.Value" /> is <see langword="null" />.
    /// </remarks>
    public static Result<T> FromValue<T>(T value)
    {
        return Result<T>.FromValue(value);
    }

    #endregion

    #region string representation

    /// <summary>
    ///     The short text shown for the result in the debugger: <c>Error: </c> and the debugger display of the <see cref="Error" /> for a failed
    ///     result (e.g., <c>Error: NotFound "Missing"</c>), or <see cref="ToString" /> for a successful one.
    /// </summary>
    internal string DebuggerDisplay
    {
        get
        {
            if (Error is null)
                return ToString();

            return $"Error: {Error.DebuggerDisplay}";
        }
    }

    /// <summary>
    ///     Appends the members of the result to the <see cref="ToString" /> output: the <see cref="Error" /> of a failed result,
    ///     and nothing for a successful one.
    /// </summary>
    private bool PrintMembers(StringBuilder builder)
    {
        if (Error is null)
            return false;

        builder.Append("Error = ").Append(Error);

        return true;
    }

    #endregion
}

/// <summary>
///     Defines a result of an operation that can either be successful and contain a value of type <typeparamref name="T" />,
///     or contain an <see cref="Error" />.
/// </summary>
/// <typeparam name="T">The type of the successful value.</typeparam>
/// <remarks>
///     The record is immutable. Create it with <see cref="FromValue" />, <see cref="FromError" /> or the implicit conversions from a <typeparamref name="T" />
///     or an <see cref="Error" />. It is successful exactly when it has no <see cref="Error" />.
/// </remarks>
/// <inheritdoc cref="IOperationResult" />
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed record Result<T> : IOperationResult
{
    private readonly T _value;

    #region constructors

    private Result(T value)
    {
        _value = value;
    }

    private Result(Error error)
    {
        _value = default!;
        Error = error
                ?? throw new ArgumentNullException(nameof(error), "Error cannot be null!");
    }

    #endregion

    /// <summary>
    ///     Gets the value of a successful result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Check <see cref="IsSuccess" /> or <see cref="IsError" /> before reading the value, or use <see cref="TryGetValue(out T, out Error)" />
    ///         (or <see cref="GetValueOrDefault" /> when a fallback value is fine).
    ///         A failed result has no value, so reading it throws instead of returning the default value of <typeparamref name="T" />
    ///         (which, for a value type, would look like a valid value, e.g., <c>0</c> for a <see cref="Result{T}" /> of <see cref="int" />).
    ///     </para>
    ///     <para>
    ///         The value of a successful result may be <see langword="null" /> when <typeparamref name="T" /> is nullable
    ///         (a nullable reference type or <see cref="Nullable{T}" />); its type follows <typeparamref name="T" />.
    ///     </para>
    /// </remarks>
    /// <exception cref="InvalidOperationException">If the result is failed; the message names the <see cref="Error" />.</exception>
    public T Value
    {
        get
        {
            if (Error is not null)
            {
                throw new InvalidOperationException(
                    $"The result has no value, because the operation failed with the error ‘{Error.Title}’ (type ‘{Error.TypeUri}’). " +
                    $"Check {nameof(IsSuccess)} or {nameof(IsError)} before reading {nameof(Value)}, or use {nameof(TryGetValue)}.");
            }

            return _value;
        }
    }

    /// <summary>
    ///     Returns an <see cref="Error" /> object if the operation failed; otherwise, returns <see langword="null" />.
    /// </summary>
    public Error? Error { get; }

    /// <summary>
    ///     Gets a value indicating whether the operation was successful.
    /// </summary>
    /// <remarks>
    ///     When it is <see langword="true" />, <see cref="Value" /> can be read; when it is <see langword="false" />, <see cref="Error" /> is not
    ///     <see langword="null" />.
    /// </remarks>
    /// <value>
    ///     <see langword="true" /> if the operation was successful; otherwise, <see langword="false" />.
    /// </value>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => Error is null;

    /// <summary>
    ///     Gets a value indicating whether the operation failed.
    /// </summary>
    /// <remarks>
    ///     The opposite of <see cref="IsSuccess" />, for the early return <c>if (result.IsError) return result.Error;</c>,
    ///     after which <see cref="Value" /> can be read.
    /// </remarks>
    /// <value>
    ///     <see langword="true" /> if the operation failed and <see cref="Error" /> is not <see langword="null" />; otherwise, <see langword="false" />.
    /// </value>
    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsError => Error is not null;

    /// <summary>
    ///     Returns the value of a successful result, or the <paramref name="fallback" /> value for a failed one.
    /// </summary>
    /// <remarks>
    ///     It never throws. A successful result returns its own value, even when it is <see langword="null" />.
    /// </remarks>
    /// <param name="fallback">The value to return when the result is failed.</param>
    /// <returns>The <see cref="Value" /> if the result is successful; otherwise, <paramref name="fallback" />.</returns>
    public T GetValueOrDefault(T fallback)
    {
        return Error is null
            ? _value
            : fallback;
    }

    /// <summary>
    ///     Gets the value of a successful result, or the <see cref="Error" /> of a failed one.
    /// </summary>
    /// <remarks>
    ///     It never throws, and it works on an expression, with no variable for the result:
    ///     <c>if (!GetUser(id).TryGetValue(out var user, out var error)) return error;</c>.
    /// </remarks>
    /// <param name="value">
    ///     When this method returns, the <see cref="Value" /> if the result is successful; otherwise, the default value of <typeparamref name="T" />.
    /// </param>
    /// <param name="error">
    ///     When this method returns, <see langword="null" /> if the result is successful; otherwise, the <see cref="Error" />.
    /// </param>
    /// <returns><see langword="true" /> if the result is successful; otherwise, <see langword="false" />.</returns>
    public bool TryGetValue([MaybeNullWhen(false)] out T value, [NotNullWhen(false)] out Error? error)
    {
        value = _value;
        error = Error;

        return error is null;
    }

    /// <summary>
    ///     Gets the value of a successful result.
    /// </summary>
    /// <remarks>
    ///     It never throws. Use it when the error is not needed (e.g., to fall back to another value); after it returns
    ///     <see langword="false" />, <see cref="Error" /> is not <see langword="null" />.
    /// </remarks>
    /// <param name="value">
    ///     When this method returns, the <see cref="Value" /> if the result is successful; otherwise, the default value of <typeparamref name="T" />.
    /// </param>
    /// <returns><see langword="true" /> if the result is successful; otherwise, <see langword="false" />.</returns>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool TryGetValue([MaybeNullWhen(false)] out T value)
    {
        value = _value;
        return Error is null;
    }

    /// <summary>
    ///     Gets the <see cref="Error" /> of a failed result.
    /// </summary>
    /// <remarks>
    ///     For the early return in one statement: <c>if (result.TryGetError(out var error)) return error;</c>.
    /// </remarks>
    /// <param name="error">
    ///     When this method returns, the <see cref="Error" /> if the result is failed; otherwise, <see langword="null" />.
    /// </param>
    /// <returns><see langword="true" /> if the result is failed; otherwise, <see langword="false" />.</returns>
    public bool TryGetError([NotNullWhen(true)] out Error? error)
    {
        error = Error;
        return error is not null;
    }

    #region implicit operators

    /// <summary>
    ///     Converts a value of type <typeparamref name="T" /> to a <see cref="Result{T}" /> representing a successful result.
    /// </summary>
    /// <remarks>
    ///     A <see langword="null" /> <paramref name="value" /> is permitted when <typeparamref name="T" /> is nullable
    ///     (a nullable reference type or <see cref="Nullable{T}" />);
    ///     the resulting <see cref="Result{T}" /> is successful and its <see cref="Value" /> is <see langword="null" />.
    /// </remarks>
    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    /// <summary>
    ///     Converts an <see cref="Error" /> to a <see cref="Result{T}" /> representing a failed result.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the <paramref name="error" /> is <see langword="null" />.</exception>
    public static implicit operator Result<T>(Error error)
    {
        return new Result<T>(error);
    }

    #endregion

    #region factory methods

    /// <summary>
    ///     Returns a successful <see cref="Result{T}" /> instance containing the specified value.
    /// </summary>
    /// <remarks>
    ///     A <see langword="null" /> <paramref name="value" /> is permitted when <typeparamref name="T" /> is nullable
    ///     (a nullable reference type or <see cref="Nullable{T}" />);
    ///     the resulting <see cref="Result{T}" /> is successful and its <see cref="Value" /> is <see langword="null" />.
    /// </remarks>
    public static Result<T> FromValue(T value)
    {
        return new Result<T>(value);
    }

    /// <summary>
    ///     Returns a <see cref="Result{T}" /> instance representing a failure with the specified <see cref="Error" />.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the <paramref name="error" /> is <see langword="null" />.</exception>
    public static Result<T> FromError(Error error)
    {
        return new Result<T>(error);
    }

    #endregion

    #region string representation

    /// <summary>
    ///     The short text shown for the result in the debugger: <c>Error: </c> and the debugger display of the <see cref="Error" /> for a failed
    ///     result (e.g., <c>Error: NotFound "Missing"</c>), or <see cref="ToString" /> for a successful one.
    /// </summary>
    internal string DebuggerDisplay
    {
        get
        {
            if (Error is null)
                return ToString();

            return $"Error: {Error.DebuggerDisplay}";
        }
    }

    /// <summary>
    ///     Appends the members of the result to the <see cref="ToString" /> output: the <see cref="Value" /> of a successful result,
    ///     or the <see cref="Error" /> of a failed one.
    /// </summary>
    private bool PrintMembers(StringBuilder builder)
    {
        if (Error is null)
            builder.Append("Value = ").Append(_value);
        else
            builder.Append("Error = ").Append(Error);

        return true;
    }

    #endregion
}
