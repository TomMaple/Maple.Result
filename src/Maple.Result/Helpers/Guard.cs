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
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Maple.Result.Helpers;

/// <summary>
///     Checks what the functions passed to the extension methods, and the asynchronous operations they extend, return. A <see langword="null" />
///     where a result, an <see cref="Error" /> or a task is required is a bug in the function or the operation, not in an argument, so it is
///     reported with an <see cref="InvalidOperationException" /> that names the function or the parameter of the operation.
/// </summary>
/// <remarks>
///     A <see langword="null" /> value of a <see cref="Result{T}" /> is not checked: it is a valid value when the type is nullable.
/// </remarks>
internal static class Guard
{
    /// <summary>
    ///     Returns the <paramref name="value" /> returned by the function named <paramref name="functionName" />, or throws if it is
    ///     <see langword="null" />.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the <paramref name="value" /> is <see langword="null" />.</exception>
    internal static T EnsureNotNullReturned<T>([NotNull] T? value, string functionName)
    {
        if (value is null)
            throw new InvalidOperationException($"The function ‘{functionName}’ returned null.");

        return value;
    }

    /// <summary>
    ///     Returns an awaitable for the asynchronous operation passed as the parameter named <paramref name="parameterName" /> that throws if
    ///     its result is <see langword="null" />.
    /// </summary>
    /// <exception cref="InvalidOperationException">When awaited, if the result of the <paramref name="task" /> is <see langword="null" />.</exception>
    internal static NotNullTaskAwaitable<T> EnsureNotNullAwaited<T>(Task<T> task, string parameterName, bool continueOnCapturedContext)
    {
        var awaiter = task.ConfigureAwait(continueOnCapturedContext).GetAwaiter();
        return new NotNullTaskAwaitable<T>(awaiter, parameterName, true);
    }

    /// <inheritdoc cref="EnsureNotNullAwaited{T}(Task{T}, string, bool)" />
    internal static NotNullValueTaskAwaitable<T> EnsureNotNullAwaited<T>(ValueTask<T> task, string parameterName, bool continueOnCapturedContext)
    {
        var awaiter = task.ConfigureAwait(continueOnCapturedContext).GetAwaiter();
        return new NotNullValueTaskAwaitable<T>(awaiter, parameterName, true);
    }

    /// <summary>
    ///     Returns an awaitable for the <paramref name="task" /> returned by the function named <paramref name="functionName" /> that throws if
    ///     the task, or its result, is <see langword="null" />.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="task" /> is <see langword="null" />; when awaited, if its result is <see langword="null" />.
    /// </exception>
    internal static NotNullTaskAwaitable<T> EnsureNotNullResult<T>(Task<T>? task, string functionName, bool continueOnCapturedContext)
    {
        var awaiter = EnsureNotNullTask(task, functionName).ConfigureAwait(continueOnCapturedContext).GetAwaiter();
        return new NotNullTaskAwaitable<T>(awaiter, functionName, false);
    }

    /// <summary>
    ///     Returns an awaitable for the <paramref name="task" /> returned by the function named <paramref name="functionName" /> that throws if
    ///     its result is <see langword="null" />.
    /// </summary>
    /// <exception cref="InvalidOperationException">When awaited, if the result of the <paramref name="task" /> is <see langword="null" />.</exception>
    internal static NotNullValueTaskAwaitable<T> EnsureNotNullResult<T>(ValueTask<T> task, string functionName, bool continueOnCapturedContext)
    {
        var awaiter = task.ConfigureAwait(continueOnCapturedContext).GetAwaiter();
        return new NotNullValueTaskAwaitable<T>(awaiter, functionName, false);
    }

    /// <summary>
    ///     Returns the <paramref name="task" /> returned by the function named <paramref name="functionName" />, or throws if it is
    ///     <see langword="null" />. The result of the task is not checked.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the <paramref name="task" /> is <see langword="null" />.</exception>
    internal static TTask EnsureNotNullTask<TTask>([NotNull] TTask? task, string functionName)
        where TTask : Task
    {
        return task ?? throw new InvalidOperationException($"The function ‘{functionName}’ returned a null task.");
    }
}
