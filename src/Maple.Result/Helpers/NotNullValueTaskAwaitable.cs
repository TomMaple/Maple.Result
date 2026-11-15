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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Results.Helpers;

/// <summary>
///     Awaits a <see cref="ValueTask{TResult}" /> as <see cref="ConfiguredValueTaskAwaitable{TResult}" /> does and throws if its result is
///     <see langword="null" />. It is its own awaiter.
/// </summary>
internal readonly struct NotNullValueTaskAwaitable<T> : ICriticalNotifyCompletion
{
    private readonly ConfiguredValueTaskAwaitable<T>.ConfiguredValueTaskAwaiter _awaiter;
    private readonly bool _isAwaitedOperation;
    private readonly string _name;

    internal NotNullValueTaskAwaitable(ConfiguredValueTaskAwaitable<T>.ConfiguredValueTaskAwaiter awaiter, string name, bool isAwaitedOperation)
    {
        _awaiter = awaiter;
        _name = name;
        _isAwaitedOperation = isAwaitedOperation;
    }

    public bool IsCompleted => _awaiter.IsCompleted;

    public NotNullValueTaskAwaitable<T> GetAwaiter()
    {
        return this;
    }

    public T GetResult()
    {
        var result = _awaiter.GetResult();

        if (result is null)
        {
            throw _isAwaitedOperation
                ? new InvalidOperationException($"The asynchronous operation represented by ‘{_name}’ returned null.")
                : new InvalidOperationException($"The function ‘{_name}’ returned null.");
        }

        return result;
    }

    public void OnCompleted(Action continuation)
    {
        _awaiter.OnCompleted(continuation);
    }

    public void UnsafeOnCompleted(Action continuation)
    {
        _awaiter.UnsafeOnCompleted(continuation);
    }
}
