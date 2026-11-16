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

namespace Maple.Results;

// Result.Try and Result.TryAsync: run code that can throw and turn the exception into a failed result.
//
// The mapper is required: an Error needs a category, a type URI and a title, which the library cannot pick on its own, and the
// exception message and stack trace are not meant for the clients that receive the error. An OperationCanceledException is never
// mapped, unless the caller asks for it as the exception type: cancellation is not a failure.
public sealed partial record Result
{
    #region Try

    /// <summary>
    ///     Runs the <paramref name="action" /> and returns a successful <see cref="Result" />, or a failed one with the <see cref="Error" />
    ///     that the <paramref name="mapException" /> function creates from the exception the action throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         An <see cref="OperationCanceledException" /> (e.g., a <see cref="TaskCanceledException" />) is not mapped but rethrown:
    ///         cancellation is not a failure.
    ///     </para>
    ///     <para>
    ///         The library does not copy the exception message or stack trace into the error, because the error is meant to be sent to
    ///         clients; the <paramref name="mapException" /> function decides what the error contains (e.g., the exception type in
    ///         <see cref="Error.Metadata" />).
    ///     </para>
    ///     <para>
    ///         For an action that returns a task, use <see cref="TryAsync(Func{Task}, Func{Exception, Error}, bool?)" />.
    ///     </para>
    /// </remarks>
    /// <param name="action">The action to run. Must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception thrown by the <paramref name="action" />.
    ///     Must not be <see langword="null" />, nor return <see langword="null" />.
    /// </param>
    /// <returns>A successful <see cref="Result" /> if the <paramref name="action" /> completes; otherwise, a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="action" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="mapException" /> function returns <see langword="null" />.</exception>
    public static Result Try(Action action, Func<Exception, Error> mapException)
    {
        return Try<Exception>(action, mapException);
    }

    /// <summary>
    ///     Runs the <paramref name="action" /> and returns a successful <see cref="Result" />, or a failed one with the <see cref="Error" />
    ///     that the <paramref name="mapException" /> function creates from a <typeparamref name="TException" /> the action throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" />
    ///         is <see cref="OperationCanceledException" /> or a type derived from it.
    ///     </para>
    ///     <para>
    ///         The library does not copy the exception message or stack trace into the error; the <paramref name="mapException" /> function
    ///         decides what the error contains.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="action">The action to run. Must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception. Must not be <see langword="null" />, nor return
    ///     <see langword="null" />.
    /// </param>
    /// <returns>A successful <see cref="Result" /> if the <paramref name="action" /> completes; otherwise, a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="action" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="mapException" /> function returns <see langword="null" />.</exception>
    public static Result Try<TException>(Action action, Func<TException, Error> mapException)
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(mapException);

        try
        {
            action();
        }
        catch (TException exception) when (IsMapped(exception))
        {
            var error = MapException(mapException, exception);
            return FromError(error);
        }

        return Success();
    }

    /// <summary>
    ///     Runs the <paramref name="function" /> and returns a successful <see cref="Result{T}" /> with its value, or a failed one with the
    ///     <see cref="Error" /> that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         An <see cref="OperationCanceledException" /> is not mapped but rethrown: cancellation is not a failure.
    ///     </para>
    ///     <para>
    ///         The library does not copy the exception message or stack trace into the error; the <paramref name="mapException" /> function
    ///         decides what the error contains.
    ///     </para>
    ///     <para>
    ///         For a function that returns a task, use <see cref="TryAsync{T}(Func{Task{T}}, Func{Exception, Error}, bool?)" />.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="function">The function to run. Must not be <see langword="null" />; it may return <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception. Must not be <see langword="null" />, nor return
    ///     <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A successful <see cref="Result{T}" /> with the value of the <paramref name="function" />; otherwise, a failed one with the mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="mapException" /> function returns <see langword="null" />.</exception>
    public static Result<T> Try<T>(Func<T> function, Func<Exception, Error> mapException)
    {
        return Try<T, Exception>(function, mapException);
    }

    /// <summary>
    ///     Runs the <paramref name="function" /> and returns a successful <see cref="Result{T}" /> with its value, or a failed one with the
    ///     <see cref="Error" /> that the <paramref name="mapException" /> function creates from a <typeparamref name="TException" /> the
    ///     function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" />
    ///         is <see cref="OperationCanceledException" /> or a type derived from it.
    ///     </para>
    ///     <para>
    ///         The library does not copy the exception message or stack trace into the error; the <paramref name="mapException" /> function
    ///         decides what the error contains.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="function">The function to run. Must not be <see langword="null" />; it may return <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception. Must not be <see langword="null" />, nor return
    ///     <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A successful <see cref="Result{T}" /> with the value of the <paramref name="function" />; otherwise, a failed one with the mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="mapException" /> function returns <see langword="null" />.</exception>
    public static Result<T> Try<T, TException>(Func<T> function, Func<TException, Error> mapException)
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(mapException);

        T value;
        try
        {
            value = function();
        }
        catch (TException exception) when (IsMapped(exception))
        {
            var error = MapException(mapException, exception);
            return Result<T>.FromError(error);
        }

        return Result<T>.FromValue(value);
    }

    #endregion

    #region TryAsync (Task)

    /// <summary>
    ///     Runs the asynchronous <paramref name="action" /> and returns a successful <see cref="Result" />, or a failed one with the
    ///     <see cref="Error" /> that the <paramref name="mapException" /> function creates from the exception the action throws, either before
    ///     returning its task or when the task faults.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         An <see cref="OperationCanceledException" /> (e.g., a canceled task) is not mapped but rethrown: cancellation is not a failure.
    ///     </para>
    ///     <para>
    ///         The library does not copy the exception message or stack trace into the error; the <paramref name="mapException" /> function
    ///         decides what the error contains.
    ///     </para>
    ///     <para>
    ///         The arguments are checked when the method is called, before the returned task starts.
    ///     </para>
    /// </remarks>
    /// <param name="action">The asynchronous action to run. Must not be <see langword="null" />, nor return a <see langword="null" /> task.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception. Must not be <see langword="null" />, nor return
    ///     <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>A task with a successful <see cref="Result" /> if the <paramref name="action" /> completes; otherwise, a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="action" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="action" /> returns a <see langword="null" /> task, or the <paramref name="mapException" /> function returns
    ///     <see langword="null" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> TryAsync(Func<Task> action, Func<Exception, Error> mapException, bool? continueOnCapturedContext = null)
    {
        return TryAsync<Exception>(action, mapException, continueOnCapturedContext);
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="action" /> and returns a successful <see cref="Result" />, or a failed one with the
    ///     <see cref="Error" /> that the <paramref name="mapException" /> function creates from a <typeparamref name="TException" /> the action
    ///     throws, either before returning its task or when the task faults.
    /// </summary>
    /// <remarks>
    ///     Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" /> is
    ///     <see cref="OperationCanceledException" /> or a type derived from it. The arguments are checked when the method is called.
    /// </remarks>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="action">The asynchronous action to run. Must not be <see langword="null" />, nor return a <see langword="null" /> task.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception. Must not be <see langword="null" />, nor return
    ///     <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>A task with a successful <see cref="Result" /> if the <paramref name="action" /> completes; otherwise, a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="action" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="action" /> returns a <see langword="null" /> task, or the <paramref name="mapException" /> function returns
    ///     <see langword="null" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> TryAsync<TException>(
        Func<Task> action,
        Func<TException, Error> mapException,
        bool? continueOnCapturedContext = null
    )
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(mapException);

        return Core(action, mapException, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Func<Task> action, Func<TException, Error> mapException, bool continueOnCapturedContext)
        {
            Task task;
            try
            {
                task = action();
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = MapException(mapException, exception);
                return FromError(error);
            }

            // Outside the try blocks: a null task is a bug of the caller, not an exception to map.
            var notNullTask = Guard.EnsureNotNullTask(task, nameof(action));
            try
            {
                await notNullTask.ConfigureAwait(continueOnCapturedContext);
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = MapException(mapException, exception);
                return FromError(error);
            }

            return Success();
        }
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns a successful <see cref="Result{T}" /> with its value, or a failed one
    ///     with the <see cref="Error" /> that the <paramref name="mapException" /> function creates from the exception the function throws,
    ///     either before returning its task or when the task faults.
    /// </summary>
    /// <remarks>
    ///     An <see cref="OperationCanceledException" /> (e.g., a canceled task) is not mapped but rethrown: cancellation is not a failure.
    ///     The library does not copy the exception message or stack trace into the error. The arguments are checked when the method is called.
    /// </remarks>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="function">
    ///     The asynchronous function to run. Must not be <see langword="null" />, nor return a <see langword="null" /> task; the value of the
    ///     task may be <see langword="null" />.
    /// </param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception. Must not be <see langword="null" />, nor return
    ///     <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A task with a successful <see cref="Result{T}" /> with the value of the <paramref name="function" />; otherwise, a failed one with the
    ///     mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="function" /> returns a <see langword="null" /> task, or the <paramref name="mapException" /> function returns
    ///     <see langword="null" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> TryAsync<T>(Func<Task<T>> function, Func<Exception, Error> mapException, bool? continueOnCapturedContext = null)
    {
        return TryAsync<T, Exception>(function, mapException, continueOnCapturedContext);
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns a successful <see cref="Result{T}" /> with its value, or a failed one
    ///     with the <see cref="Error" /> that the <paramref name="mapException" /> function creates from a <typeparamref name="TException" /> the
    ///     function throws, either before returning its task or when the task faults.
    /// </summary>
    /// <remarks>
    ///     Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" /> is
    ///     <see cref="OperationCanceledException" /> or a type derived from it. The arguments are checked when the method is called.
    /// </remarks>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="function">
    ///     The asynchronous function to run. Must not be <see langword="null" />, nor return a <see langword="null" /> task; the value of the
    ///     task may be <see langword="null" />.
    /// </param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception. Must not be <see langword="null" />, nor return
    ///     <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A task with a successful <see cref="Result{T}" /> with the value of the <paramref name="function" />; otherwise, a failed one with the
    ///     mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="function" /> returns a <see langword="null" /> task, or the <paramref name="mapException" /> function returns
    ///     <see langword="null" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> TryAsync<T, TException>(
        Func<Task<T>> function,
        Func<TException, Error> mapException,
        bool? continueOnCapturedContext = null
    )
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(mapException);

        return Core(function, mapException, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Func<Task<T>> function, Func<TException, Error> mapException, bool continueOnCapturedContext)
        {
            Task<T> task;
            try
            {
                task = function();
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = MapException(mapException, exception);
                return Result<T>.FromError(error);
            }

            // Outside the try blocks: a null task is a bug of the caller, not an exception to map.
            var notNullTask = Guard.EnsureNotNullTask(task, nameof(function));
            T value;
            try
            {
                value = await notNullTask.ConfigureAwait(continueOnCapturedContext);
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = MapException(mapException, exception);
                return Result<T>.FromError(error);
            }

            return Result<T>.FromValue(value);
        }
    }

    #endregion

    #region TryAsync (ValueTask)

    /// <summary>
    ///     Runs the asynchronous <paramref name="action" /> and returns a successful <see cref="Result" />, or a failed one with the
    ///     <see cref="Error" /> that the <paramref name="mapException" /> function creates from the exception the action throws, either before
    ///     returning its task or when the task faults.
    /// </summary>
    /// <remarks>
    ///     An <see cref="OperationCanceledException" /> (e.g., a canceled task) is not mapped but rethrown: cancellation is not a failure.
    ///     The library does not copy the exception message or stack trace into the error. The arguments are checked when the method is called.
    ///     An async lambda binds to the <see cref="Task" /> overload; this one is used for a function that returns a <see cref="ValueTask" />.
    /// </remarks>
    /// <param name="action">The asynchronous action to run. Must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception. Must not be <see langword="null" />, nor return
    ///     <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>A task with a successful <see cref="Result" /> if the <paramref name="action" /> completes; otherwise, a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="action" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="mapException" /> function returns <see langword="null" />.</exception>
    public static ValueTask<Result> TryAsync(Func<ValueTask> action, Func<Exception, Error> mapException, bool? continueOnCapturedContext = null)
    {
        return TryAsync<Exception>(action, mapException, continueOnCapturedContext);
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="action" /> and returns a successful <see cref="Result" />, or a failed one with the
    ///     <see cref="Error" /> that the <paramref name="mapException" /> function creates from a <typeparamref name="TException" /> the action
    ///     throws, either before returning its task or when the task faults.
    /// </summary>
    /// <remarks>
    ///     Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" /> is
    ///     <see cref="OperationCanceledException" /> or a type derived from it. The arguments are checked when the method is called.
    /// </remarks>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="action">The asynchronous action to run. Must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception. Must not be <see langword="null" />, nor return
    ///     <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>A task with a successful <see cref="Result" /> if the <paramref name="action" /> completes; otherwise, a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="action" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="mapException" /> function returns <see langword="null" />.</exception>
    public static ValueTask<Result> TryAsync<TException>(
        Func<ValueTask> action,
        Func<TException, Error> mapException,
        bool? continueOnCapturedContext = null
    )
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(mapException);

        return Core(action, mapException, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(Func<ValueTask> action, Func<TException, Error> mapException, bool continueOnCapturedContext)
        {
            try
            {
                var task = action();
                await task.ConfigureAwait(continueOnCapturedContext);
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = MapException(mapException, exception);
                return FromError(error);
            }

            return Success();
        }
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns a successful <see cref="Result{T}" /> with its value, or a failed one
    ///     with the <see cref="Error" /> that the <paramref name="mapException" /> function creates from the exception the function throws,
    ///     either before returning its task or when the task faults.
    /// </summary>
    /// <remarks>
    ///     An <see cref="OperationCanceledException" /> (e.g., a canceled task) is not mapped but rethrown: cancellation is not a failure.
    ///     The library does not copy the exception message or stack trace into the error. The arguments are checked when the method is called.
    ///     An async lambda binds to the <see cref="Task{TResult}" /> overload; this one is used for a function that returns a
    ///     <see cref="ValueTask{TResult}" />.
    /// </remarks>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="function">
    ///     The asynchronous function to run. Must not be <see langword="null" />; the value of the task may be <see langword="null" />.
    /// </param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception. Must not be <see langword="null" />, nor return
    ///     <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A task with a successful <see cref="Result{T}" /> with the value of the <paramref name="function" />; otherwise, a failed one with the
    ///     mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="mapException" /> function returns <see langword="null" />.</exception>
    public static ValueTask<Result<T>> TryAsync<T>(Func<ValueTask<T>> function, Func<Exception, Error> mapException, bool? continueOnCapturedContext = null)
    {
        return TryAsync<T, Exception>(function, mapException, continueOnCapturedContext);
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns a successful <see cref="Result{T}" /> with its value, or a failed one
    ///     with the <see cref="Error" /> that the <paramref name="mapException" /> function creates from a <typeparamref name="TException" /> the
    ///     function throws, either before returning its task or when the task faults.
    /// </summary>
    /// <remarks>
    ///     Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" /> is
    ///     <see cref="OperationCanceledException" /> or a type derived from it. The arguments are checked when the method is called.
    /// </remarks>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="function">
    ///     The asynchronous function to run. Must not be <see langword="null" />; the value of the task may be <see langword="null" />.
    /// </param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception. Must not be <see langword="null" />, nor return
    ///     <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A task with a successful <see cref="Result{T}" /> with the value of the <paramref name="function" />; otherwise, a failed one with the
    ///     mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="mapException" /> function returns <see langword="null" />.</exception>
    public static ValueTask<Result<T>> TryAsync<T, TException>(
        Func<ValueTask<T>> function,
        Func<TException, Error> mapException,
        bool? continueOnCapturedContext = null
    )
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(mapException);

        return Core(function, mapException, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(Func<ValueTask<T>> function, Func<TException, Error> mapException, bool continueOnCapturedContext)
        {
            T value;
            try
            {
                var task = function();
                value = await task.ConfigureAwait(continueOnCapturedContext);
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = MapException(mapException, exception);
                return Result<T>.FromError(error);
            }

            return Result<T>.FromValue(value);
        }
    }

    #endregion

    #region helper methods

    /// <summary>
    ///     Whether the exception is mapped to an error: an <see cref="OperationCanceledException" /> is rethrown, unless the caller asked for
    ///     that type (or a type derived from it) as <typeparamref name="TException" />.
    /// </summary>
    private static bool IsMapped<TException>(TException exception)
        where TException : Exception
    {
        return exception is not OperationCanceledException
               || typeof(TException).IsAssignableTo(typeof(OperationCanceledException));
    }

    /// <summary>
    ///     Returns the <see cref="Error" /> that the <paramref name="mapException" /> function creates from the <paramref name="exception" />.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the <paramref name="mapException" /> function returns <see langword="null" />.</exception>
    private static Error MapException<TException>(Func<TException, Error> mapException, TException exception)
        where TException : Exception
    {
        var error = mapException(exception);
        return Guard.EnsureNotNullReturned(error, nameof(mapException));
    }

    #endregion

    #region Not supported: functions returning a task

    // These overloads turn a call with a function that returns a task into a compile error (CS0619). Without them, an async lambda
    // converts to Action (async void: its exceptions escape Try), and a lambda returning a task binds to Try<T> with T = Task (a result
    // holding a task that is never awaited). A Task<T> converts to Task, so the Func<Task> overloads also cover most functions returning
    // Task<T>; their priority makes them win over Try(Action) and Try<T>(Func<T>). A lambda that always throws converts to any delegate
    // type, so it binds to these overloads too: pass the work as a method that can complete, or use TryAsync.
    //
    // Both mapper families need a trap for each shape. An async lambda with a block body that returns a value does not convert to
    // Func<Task>, and a ValueTask<T> does not convert to ValueTask: the untyped family catches both with Func<ValueTask<TTaskResult>>,
    // and the typed family, whose calls have two type parameters, needs Func<Task<TTaskResult>> and Func<ValueTask<TTaskResult>> of its
    // own (#191).

    private const string FunctionReturnsTaskMessage =
        "The function returns a task, which Try does not await (an async lambda would also become async void). Use TryAsync.";

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Try(Func<Task> function, Func<Exception, Error> mapException)
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    public static Result Try(Func<ValueTask> function, Func<Exception, Error> mapException)
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    public static Result Try<TTaskResult>(Func<ValueTask<TTaskResult>> function, Func<Exception, Error> mapException)
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Try<TException>(Func<Task> function, Func<TException, Error> mapException)
        where TException : Exception
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    public static Result Try<TException>(Func<ValueTask> function, Func<TException, Error> mapException)
        where TException : Exception
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result Try<TTaskResult, TException>(Func<Task<TTaskResult>> function, Func<TException, Error> mapException)
        where TException : Exception
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>Try</c> does not await; use <c>TryAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(FunctionReturnsTaskMessage, error: true)]
    public static Result Try<TTaskResult, TException>(Func<ValueTask<TTaskResult>> function, Func<TException, Error> mapException)
        where TException : Exception
    {
        throw new NotSupportedException(FunctionReturnsTaskMessage);
    }

    #endregion
}
