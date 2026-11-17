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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Results;

// Result.Try and Result.TryAsync: run code that can throw and turn the exception into a failed result.
//
// The mapper is required: an Error needs a category, a type URI and a title, which the library cannot pick on its own, and the
// exception message and stack trace are not meant for the clients that receive the error. The mapper may return null to decline an
// exception (e.g., a type it does not handle): it is then rethrown with `throw;`, inside the catch block, so that it keeps its
// stack trace (#195). An OperationCanceledException is never
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
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <returns>A successful <see cref="Result" /> if the <paramref name="action" /> completes; otherwise, a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="action" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    public static Result Try(Action action, Func<Exception, Error?> mapException)
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
    ///     <para>
    ///         A call that names only the exception type binds to this overload even when the function returns a value, because C# infers
    ///         all type arguments or none: <c>Result.Try&lt;IOException&gt;(() =&gt; File.ReadAllText(path), e =&gt; …)</c> discards the text
    ///         and returns a <see cref="Result" />. To keep the value, type the parameter of the mapper
    ///         (<c>Result.Try(() =&gt; File.ReadAllText(path), (IOException e) =&gt; …)</c>) or pass both type arguments
    ///         (<c>Result.Try&lt;string, IOException&gt;(…)</c>).
    ///     </para>
    /// </remarks>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="action">The action to run. Must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <returns>A successful <see cref="Result" /> if the <paramref name="action" /> completes; otherwise, a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="action" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    public static Result Try<TException>(Action action, Func<TException, Error?> mapException)
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
            var error = mapException(exception);
            if (error is null)
                throw;

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
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A successful <see cref="Result{T}" /> with the value of the <paramref name="function" />; otherwise, a failed one with the mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    public static Result<T> Try<T>(Func<T> function, Func<Exception, Error?> mapException)
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
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A successful <see cref="Result{T}" /> with the value of the <paramref name="function" />; otherwise, a failed one with the mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    public static Result<T> Try<T, TException>(Func<T> function, Func<TException, Error?> mapException)
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
            var error = mapException(exception);
            if (error is null)
                throw;

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
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>A task with a successful <see cref="Result" /> if the <paramref name="action" /> completes; otherwise, a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="action" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="action" /> returns a <see langword="null" /> task.</exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> TryAsync(Func<Task> action, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null)
    {
        return TryAsync<Exception>(action, mapException, continueOnCapturedContext);
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="action" /> and returns a successful <see cref="Result" />, or a failed one with the
    ///     <see cref="Error" /> that the <paramref name="mapException" /> function creates from a <typeparamref name="TException" /> the action
    ///     throws, either before returning its task or when the task faults.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" /> is
    ///         <see cref="OperationCanceledException" /> or a type derived from it. The arguments are checked when the method is called.
    ///     </para>
    ///     <para>
    ///         A call that names only the exception type binds to this overload even when the task has a value, because C# infers all type
    ///         arguments or none and a <see cref="Task{TResult}" /> is a <see cref="Task" />:
    ///         <c>Result.TryAsync&lt;IOException&gt;(() =&gt; File.ReadAllTextAsync(path), e =&gt; …)</c> discards the text and returns a
    ///         <see cref="Result" />. To keep the value, type the parameter of the mapper (<c>(IOException e) =&gt; …</c>) or pass both type
    ///         arguments (<c>Result.TryAsync&lt;string, IOException&gt;(…)</c>).
    ///     </para>
    /// </remarks>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="action">The asynchronous action to run. Must not be <see langword="null" />, nor return a <see langword="null" /> task.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>A task with a successful <see cref="Result" /> if the <paramref name="action" /> completes; otherwise, a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="action" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="action" /> returns a <see langword="null" /> task.</exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> TryAsync<TException>(
        Func<Task> action,
        Func<TException, Error?> mapException,
        bool? continueOnCapturedContext = null
    )
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(mapException);

        return Core(action, mapException, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Func<Task> action, Func<TException, Error?> mapException, bool continueOnCapturedContext)
        {
            Task task;
            try
            {
                task = action();
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = mapException(exception);
                if (error is null)
                    throw;

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
                var error = mapException(exception);
                if (error is null)
                    throw;

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
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
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
    /// <exception cref="InvalidOperationException">If the <paramref name="function" /> returns a <see langword="null" /> task.</exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> TryAsync<T>(Func<Task<T>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null)
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
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
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
    /// <exception cref="InvalidOperationException">If the <paramref name="function" /> returns a <see langword="null" /> task.</exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> TryAsync<T, TException>(
        Func<Task<T>> function,
        Func<TException, Error?> mapException,
        bool? continueOnCapturedContext = null
    )
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(mapException);

        return Core(function, mapException, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Func<Task<T>> function, Func<TException, Error?> mapException, bool continueOnCapturedContext)
        {
            Task<T> task;
            try
            {
                task = function();
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = mapException(exception);
                if (error is null)
                    throw;

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
                var error = mapException(exception);
                if (error is null)
                    throw;

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
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a successful <see cref="Result" /> if the <paramref name="action" /> completes; otherwise, a
    ///     failed one with the mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="action" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    public static ValueTask<Result> TryAsync(Func<ValueTask> action, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null)
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
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a successful <see cref="Result" /> if the <paramref name="action" /> completes; otherwise, a
    ///     failed one with the mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="action" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    public static ValueTask<Result> TryAsync<TException>(
        Func<ValueTask> action,
        Func<TException, Error?> mapException,
        bool? continueOnCapturedContext = null
    )
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(mapException);

        return Core(action, mapException, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(Func<ValueTask> action, Func<TException, Error?> mapException, bool continueOnCapturedContext)
        {
            try
            {
                var task = action();
                await task.ConfigureAwait(continueOnCapturedContext);
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = mapException(exception);
                if (error is null)
                    throw;

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
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a successful <see cref="Result{T}" /> with the value of the <paramref name="function" />;
    ///     otherwise, a failed one with the mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    public static ValueTask<Result<T>> TryAsync<T>(Func<ValueTask<T>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null)
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
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a successful <see cref="Result{T}" /> with the value of the <paramref name="function" />;
    ///     otherwise, a failed one with the mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    public static ValueTask<Result<T>> TryAsync<T, TException>(
        Func<ValueTask<T>> function,
        Func<TException, Error?> mapException,
        bool? continueOnCapturedContext = null
    )
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(mapException);

        return Core(function, mapException, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(Func<ValueTask<T>> function, Func<TException, Error?> mapException, bool continueOnCapturedContext)
        {
            T value;
            try
            {
                var task = function();
                value = await task.ConfigureAwait(continueOnCapturedContext);
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = mapException(exception);
                if (error is null)
                    throw;

                return Result<T>.FromError(error);
            }

            return Result<T>.FromValue(value);
        }
    }

    #endregion

    #region Try (flattening)

    /// <summary>
    ///     Runs the <paramref name="function" /> and returns the <see cref="Result" /> it returns, or a failed one with the <see cref="Error" /> that
    ///     the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result, as in <c>IfSuccess</c>. Pass the type
    ///         argument explicitly to keep the nested result (e.g., <c>Result.Try&lt;Result&lt;int&gt;&gt;(…)</c>).
    ///     </para>
    ///     <para>
    ///         An <see cref="OperationCanceledException" /> is not mapped but rethrown: cancellation is not a failure.
    ///     </para>
    ///     <para>
    ///         A function that returns <see langword="null" /> throws an <see cref="InvalidOperationException" />; it is not mapped.
    ///     </para>
    /// </remarks>
    /// <param name="function">The function to run. Must not be <see langword="null" />, nor return <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The <see cref="Result" /> returned by the <paramref name="function" />, as it is; or a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="function" /> returns <see langword="null" />.</exception>
    public static Result Try(Func<Result> function, Func<Exception, Error?> mapException)
    {
        return Try<Exception>(function, mapException);
    }

    /// <summary>
    ///     Runs the <paramref name="function" /> and returns the <see cref="Result" /> it returns, or a failed one with the <see cref="Error" /> that
    ///     the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result, as in <c>IfSuccess</c>. Pass the type
    ///         argument explicitly to keep the nested result (e.g., <c>Result.Try&lt;Result&lt;int&gt;&gt;(…)</c>).
    ///     </para>
    ///     <para>
    ///         Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" /> is <see cref="OperationCanceledException" /> or a type derived from it.
    ///     </para>
    ///     <para>
    ///         A function that returns <see langword="null" /> throws an <see cref="InvalidOperationException" />; it is not mapped.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="function">The function to run. Must not be <see langword="null" />, nor return <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The <see cref="Result" /> returned by the <paramref name="function" />, as it is; or a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="function" /> returns <see langword="null" />.</exception>
    public static Result Try<TException>(Func<Result> function, Func<TException, Error?> mapException)
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(mapException);

        Result result;
        try
        {
            result = function();
        }
        catch (TException exception) when (IsMapped(exception))
        {
            var error = mapException(exception);
            if (error is null)
                throw;

            return FromError(error);
        }

        // Outside the try block: a null result is a bug of the caller, not an exception to map.
        return Guard.EnsureNotNullReturned(result, nameof(function));
    }

    /// <summary>
    ///     Runs the <paramref name="function" /> and returns the <see cref="Result{T}" /> it returns, or a failed one with the <see cref="Error" />
    ///     that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result, as in <c>IfSuccess</c>. Pass the type
    ///         argument explicitly to keep the nested result (e.g., <c>Result.Try&lt;Result&lt;int&gt;&gt;(…)</c>).
    ///     </para>
    ///     <para>
    ///         An <see cref="OperationCanceledException" /> is not mapped but rethrown: cancellation is not a failure.
    ///     </para>
    ///     <para>
    ///         A function that returns <see langword="null" /> throws an <see cref="InvalidOperationException" />; it is not mapped.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="function">The function to run. Must not be <see langword="null" />, nor return <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The <see cref="Result{T}" /> returned by the <paramref name="function" />, as it is; or a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="function" /> returns <see langword="null" />.</exception>
    public static Result<T> Try<T>(Func<Result<T>> function, Func<Exception, Error?> mapException)
    {
        return Try<T, Exception>(function, mapException);
    }

    /// <summary>
    ///     Runs the <paramref name="function" /> and returns the <see cref="Result{T}" /> it returns, or a failed one with the <see cref="Error" />
    ///     that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result, as in <c>IfSuccess</c>. Pass the type
    ///         argument explicitly to keep the nested result (e.g., <c>Result.Try&lt;Result&lt;int&gt;&gt;(…)</c>).
    ///     </para>
    ///     <para>
    ///         Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" /> is <see cref="OperationCanceledException" /> or a type derived from it.
    ///     </para>
    ///     <para>
    ///         A function that returns <see langword="null" /> throws an <see cref="InvalidOperationException" />; it is not mapped.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="function">The function to run. Must not be <see langword="null" />, nor return <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The <see cref="Result{T}" /> returned by the <paramref name="function" />, as it is; or a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="function" /> returns <see langword="null" />.</exception>
    public static Result<T> Try<T, TException>(Func<Result<T>> function, Func<TException, Error?> mapException)
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(mapException);

        Result<T> result;
        try
        {
            result = function();
        }
        catch (TException exception) when (IsMapped(exception))
        {
            var error = mapException(exception);
            if (error is null)
                throw;

            return Result<T>.FromError(error);
        }

        // Outside the try block: a null result is a bug of the caller, not an exception to map.
        return Guard.EnsureNotNullReturned(result, nameof(function));
    }

    /// <summary>
    ///     Runs the <paramref name="function" /> and returns a failed <see cref="Result" /> with the <see cref="Error" /> it returns, or with the
    ///     error that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result, as in <c>IfSuccess</c>. Pass the type
    ///         argument explicitly to keep the nested result (e.g., <c>Result.Try&lt;Result&lt;int&gt;&gt;(…)</c>).
    ///     </para>
    ///     <para>
    ///         An <see cref="OperationCanceledException" /> is not mapped but rethrown: cancellation is not a failure.
    ///     </para>
    ///     <para>
    ///         A function that returns <see langword="null" /> throws an <see cref="InvalidOperationException" />; it is not mapped.
    ///     </para>
    /// </remarks>
    /// <param name="function">The function to run. Must not be <see langword="null" />, nor return <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <returns>A failed <see cref="Result" /> with the <see cref="Error" /> returned by the <paramref name="function" />, or with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="function" /> returns <see langword="null" />.</exception>
    public static Result Try(Func<Error> function, Func<Exception, Error?> mapException)
    {
        return Try<Exception>(function, mapException);
    }

    /// <summary>
    ///     Runs the <paramref name="function" /> and returns a failed <see cref="Result" /> with the <see cref="Error" /> it returns, or with the
    ///     error that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result, as in <c>IfSuccess</c>. Pass the type
    ///         argument explicitly to keep the nested result (e.g., <c>Result.Try&lt;Result&lt;int&gt;&gt;(…)</c>).
    ///     </para>
    ///     <para>
    ///         Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" /> is <see cref="OperationCanceledException" /> or a type derived from it.
    ///     </para>
    ///     <para>
    ///         A function that returns <see langword="null" /> throws an <see cref="InvalidOperationException" />; it is not mapped.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="function">The function to run. Must not be <see langword="null" />, nor return <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <returns>A failed <see cref="Result" /> with the <see cref="Error" /> returned by the <paramref name="function" />, or with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="function" /> returns <see langword="null" />.</exception>
    public static Result Try<TException>(Func<Error> function, Func<TException, Error?> mapException)
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(mapException);

        Error returnedError;
        try
        {
            returnedError = function();
        }
        catch (TException exception) when (IsMapped(exception))
        {
            var error = mapException(exception);
            if (error is null)
                throw;

            return FromError(error);
        }

        // Outside the try block: a null result is a bug of the caller, not an exception to map.
        var notNullError = Guard.EnsureNotNullReturned(returnedError, nameof(function));

        return FromError(notNullError);
    }

    #endregion

    #region TryAsync (Task, flattening)

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns the <see cref="Result" /> it returns, or a failed one with the <see
    ///     cref="Error" /> that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result. Pass the type argument explicitly to keep
    ///         the nested result.
    ///     </para>
    ///     <para>
    ///         An <see cref="OperationCanceledException" /> is not mapped but rethrown: cancellation is not a failure. An exception is mapped whether the function throws before returning its task or the task faults.
    ///     </para>
    ///     <para>
    ///         A function that returns a <see langword="null" /> task or a <see langword="null" /> result throws an <see cref="InvalidOperationException" />; it is not mapped. The arguments are checked
    ///         when the method is called.
    ///     </para>
    /// </remarks>
    /// <param name="function">The asynchronous function to run. Must not be <see langword="null" />, nor return a <see langword="null" /> task; its result must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>A task with the <see cref="Result" /> returned by the <paramref name="function" />, as it is; or a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="function" /> returns a <see langword="null" /> task or a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> TryAsync(Func<Task<Result>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null)
    {
        return TryAsync<Exception>(function, mapException, continueOnCapturedContext);
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns the <see cref="Result" /> it returns, or a failed one with the <see
    ///     cref="Error" /> that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result. Pass the type argument explicitly to keep
    ///         the nested result.
    ///     </para>
    ///     <para>
    ///         Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" /> is <see cref="OperationCanceledException" /> or a type derived from it. An exception is mapped whether the function throws before returning its task or the task faults.
    ///     </para>
    ///     <para>
    ///         A function that returns a <see langword="null" /> task or a <see langword="null" /> result throws an <see cref="InvalidOperationException" />; it is not mapped. The arguments are checked
    ///         when the method is called.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="function">The asynchronous function to run. Must not be <see langword="null" />, nor return a <see langword="null" /> task; its result must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>A task with the <see cref="Result" /> returned by the <paramref name="function" />, as it is; or a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="function" /> returns a <see langword="null" /> task or a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> TryAsync<TException>(
        Func<Task<Result>> function,
        Func<TException, Error?> mapException,
        bool? continueOnCapturedContext = null
    )
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(mapException);

        return Core(function, mapException, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Func<Task<Result>> function, Func<TException, Error?> mapException, bool continueOnCapturedContext)
        {
            Task<Result> task;
            try
            {
                task = function();
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = mapException(exception);
                if (error is null)
                    throw;

                return FromError(error);
            }

            // Outside the try blocks: a null task or a null result is a bug of the caller, not an exception to map.
            var notNullTask = Guard.EnsureNotNullTask(task, nameof(function));
            Result result;
            try
            {
                result = await notNullTask.ConfigureAwait(continueOnCapturedContext);
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = mapException(exception);
                if (error is null)
                    throw;

                return FromError(error);
            }

            return Guard.EnsureNotNullReturned(result, nameof(function));
        }
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns the <see cref="Result{T}" /> it returns, or a failed one with the <see
    ///     cref="Error" /> that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result. Pass the type argument explicitly to keep
    ///         the nested result.
    ///     </para>
    ///     <para>
    ///         An <see cref="OperationCanceledException" /> is not mapped but rethrown: cancellation is not a failure. An exception is mapped whether the function throws before returning its task or the task faults.
    ///     </para>
    ///     <para>
    ///         A function that returns a <see langword="null" /> task or a <see langword="null" /> result throws an <see cref="InvalidOperationException" />; it is not mapped. The arguments are checked
    ///         when the method is called.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="function">The asynchronous function to run. Must not be <see langword="null" />, nor return a <see langword="null" /> task; its result must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>A task with the <see cref="Result{T}" /> returned by the <paramref name="function" />, as it is; or a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="function" /> returns a <see langword="null" /> task or a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> TryAsync<T>(Func<Task<Result<T>>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null)
    {
        return TryAsync<T, Exception>(function, mapException, continueOnCapturedContext);
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns the <see cref="Result{T}" /> it returns, or a failed one with the <see
    ///     cref="Error" /> that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result. Pass the type argument explicitly to keep
    ///         the nested result.
    ///     </para>
    ///     <para>
    ///         Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" /> is <see cref="OperationCanceledException" /> or a type derived from it. An exception is mapped whether the function throws before returning its task or the task faults.
    ///     </para>
    ///     <para>
    ///         A function that returns a <see langword="null" /> task or a <see langword="null" /> result throws an <see cref="InvalidOperationException" />; it is not mapped. The arguments are checked
    ///         when the method is called.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="function">The asynchronous function to run. Must not be <see langword="null" />, nor return a <see langword="null" /> task; its result must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>A task with the <see cref="Result{T}" /> returned by the <paramref name="function" />, as it is; or a failed one with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="function" /> returns a <see langword="null" /> task or a <see langword="null" /> result.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result<T>> TryAsync<T, TException>(
        Func<Task<Result<T>>> function,
        Func<TException, Error?> mapException,
        bool? continueOnCapturedContext = null
    )
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(mapException);

        return Core(function, mapException, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result<T>> Core(Func<Task<Result<T>>> function, Func<TException, Error?> mapException, bool continueOnCapturedContext)
        {
            Task<Result<T>> task;
            try
            {
                task = function();
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = mapException(exception);
                if (error is null)
                    throw;

                return Result<T>.FromError(error);
            }

            // Outside the try blocks: a null task or a null result is a bug of the caller, not an exception to map.
            var notNullTask = Guard.EnsureNotNullTask(task, nameof(function));
            Result<T> result;
            try
            {
                result = await notNullTask.ConfigureAwait(continueOnCapturedContext);
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = mapException(exception);
                if (error is null)
                    throw;

                return Result<T>.FromError(error);
            }

            return Guard.EnsureNotNullReturned(result, nameof(function));
        }
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns a failed <see cref="Result" /> with the <see cref="Error" /> it returns, or
    ///     with the error that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result. Pass the type argument explicitly to keep
    ///         the nested result.
    ///     </para>
    ///     <para>
    ///         An <see cref="OperationCanceledException" /> is not mapped but rethrown: cancellation is not a failure. An exception is mapped whether the function throws before returning its task or the task faults.
    ///     </para>
    ///     <para>
    ///         A function that returns a <see langword="null" /> task or a <see langword="null" /> error throws an <see cref="InvalidOperationException" />; it is not mapped. The arguments are checked
    ///         when the method is called.
    ///     </para>
    /// </remarks>
    /// <param name="function">The asynchronous function to run. Must not be <see langword="null" />, nor return a <see langword="null" /> task; its result must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>A task with a failed <see cref="Result" /> with the <see cref="Error" /> returned by the <paramref name="function" />, or with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="function" /> returns a <see langword="null" /> task or a <see langword="null" /> error.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> TryAsync(Func<Task<Error>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null)
    {
        return TryAsync<Exception>(function, mapException, continueOnCapturedContext);
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns a failed <see cref="Result" /> with the <see cref="Error" /> it returns, or
    ///     with the error that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result. Pass the type argument explicitly to keep
    ///         the nested result.
    ///     </para>
    ///     <para>
    ///         Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" /> is <see cref="OperationCanceledException" /> or a type derived from it. An exception is mapped whether the function throws before returning its task or the task faults.
    ///     </para>
    ///     <para>
    ///         A function that returns a <see langword="null" /> task or a <see langword="null" /> error throws an <see cref="InvalidOperationException" />; it is not mapped. The arguments are checked
    ///         when the method is called.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="function">The asynchronous function to run. Must not be <see langword="null" />, nor return a <see langword="null" /> task; its result must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>A task with a failed <see cref="Result" /> with the <see cref="Error" /> returned by the <paramref name="function" />, or with the mapped error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="function" /> returns a <see langword="null" /> task or a <see langword="null" /> error.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Task<Result> TryAsync<TException>(
        Func<Task<Error>> function,
        Func<TException, Error?> mapException,
        bool? continueOnCapturedContext = null
    )
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(mapException);

        return Core(function, mapException, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async Task<Result> Core(Func<Task<Error>> function, Func<TException, Error?> mapException, bool continueOnCapturedContext)
        {
            Task<Error> task;
            try
            {
                task = function();
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = mapException(exception);
                if (error is null)
                    throw;

                return FromError(error);
            }

            // Outside the try blocks: a null task or a null result is a bug of the caller, not an exception to map.
            var notNullTask = Guard.EnsureNotNullTask(task, nameof(function));
            Error returnedError;
            try
            {
                returnedError = await notNullTask.ConfigureAwait(continueOnCapturedContext);
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = mapException(exception);
                if (error is null)
                    throw;

                return FromError(error);
            }

            var notNullError = Guard.EnsureNotNullReturned(returnedError, nameof(function));

            return FromError(notNullError);
        }
    }

    #endregion

    #region TryAsync (ValueTask, flattening)

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns the <see cref="Result" /> it returns, or a failed one with the <see
    ///     cref="Error" /> that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result. Pass the type argument explicitly to keep
    ///         the nested result.
    ///     </para>
    ///     <para>
    ///         An <see cref="OperationCanceledException" /> is not mapped but rethrown: cancellation is not a failure. An exception is mapped whether the function throws before returning its task or the task faults.
    ///     </para>
    ///     <para>
    ///         A function that returns a <see langword="null" /> result throws an <see cref="InvalidOperationException" />; it is not mapped. The arguments are checked
    ///         when the method is called.
    ///     </para>
    /// </remarks>
    /// <param name="function">The asynchronous function to run. Must not be <see langword="null" />; its result must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the <see cref="Result" /> returned by the <paramref name="function" />, as it is; or a failed
    ///     one with the mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="function" /> returns a <see langword="null" /> result.</exception>
    public static ValueTask<Result> TryAsync(Func<ValueTask<Result>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null)
    {
        return TryAsync<Exception>(function, mapException, continueOnCapturedContext);
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns the <see cref="Result" /> it returns, or a failed one with the <see
    ///     cref="Error" /> that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result. Pass the type argument explicitly to keep
    ///         the nested result.
    ///     </para>
    ///     <para>
    ///         Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" /> is <see cref="OperationCanceledException" /> or a type derived from it. An exception is mapped whether the function throws before returning its task or the task faults.
    ///     </para>
    ///     <para>
    ///         A function that returns a <see langword="null" /> result throws an <see cref="InvalidOperationException" />; it is not mapped. The arguments are checked
    ///         when the method is called.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="function">The asynchronous function to run. Must not be <see langword="null" />; its result must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the <see cref="Result" /> returned by the <paramref name="function" />, as it is; or a failed
    ///     one with the mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="function" /> returns a <see langword="null" /> result.</exception>
    public static ValueTask<Result> TryAsync<TException>(
        Func<ValueTask<Result>> function,
        Func<TException, Error?> mapException,
        bool? continueOnCapturedContext = null
    )
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(mapException);

        return Core(function, mapException, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(Func<ValueTask<Result>> function, Func<TException, Error?> mapException, bool continueOnCapturedContext)
        {
            Result result;
            try
            {
                var task = function();
                result = await task.ConfigureAwait(continueOnCapturedContext);
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = mapException(exception);
                if (error is null)
                    throw;

                return FromError(error);
            }

            // Outside the try block: a null result is a bug of the caller, not an exception to map.
            return Guard.EnsureNotNullReturned(result, nameof(function));
        }
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns the <see cref="Result{T}" /> it returns, or a failed one with the <see
    ///     cref="Error" /> that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result. Pass the type argument explicitly to keep
    ///         the nested result.
    ///     </para>
    ///     <para>
    ///         An <see cref="OperationCanceledException" /> is not mapped but rethrown: cancellation is not a failure. An exception is mapped whether the function throws before returning its task or the task faults.
    ///     </para>
    ///     <para>
    ///         A function that returns a <see langword="null" /> result throws an <see cref="InvalidOperationException" />; it is not mapped. The arguments are checked
    ///         when the method is called.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="function">The asynchronous function to run. Must not be <see langword="null" />; its result must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{T}" /> returned by the <paramref name="function" />, as it is; or a failed
    ///     one with the mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="function" /> returns a <see langword="null" /> result.</exception>
    public static ValueTask<Result<T>> TryAsync<T>(Func<ValueTask<Result<T>>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null)
    {
        return TryAsync<T, Exception>(function, mapException, continueOnCapturedContext);
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns the <see cref="Result{T}" /> it returns, or a failed one with the <see
    ///     cref="Error" /> that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result. Pass the type argument explicitly to keep
    ///         the nested result.
    ///     </para>
    ///     <para>
    ///         Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" /> is <see cref="OperationCanceledException" /> or a type derived from it. An exception is mapped whether the function throws before returning its task or the task faults.
    ///     </para>
    ///     <para>
    ///         A function that returns a <see langword="null" /> result throws an <see cref="InvalidOperationException" />; it is not mapped. The arguments are checked
    ///         when the method is called.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="function">The asynchronous function to run. Must not be <see langword="null" />; its result must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{T}" /> returned by the <paramref name="function" />, as it is; or a failed
    ///     one with the mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="function" /> returns a <see langword="null" /> result.</exception>
    public static ValueTask<Result<T>> TryAsync<T, TException>(
        Func<ValueTask<Result<T>>> function,
        Func<TException, Error?> mapException,
        bool? continueOnCapturedContext = null
    )
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(mapException);

        return Core(function, mapException, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(Func<ValueTask<Result<T>>> function, Func<TException, Error?> mapException, bool continueOnCapturedContext)
        {
            Result<T> result;
            try
            {
                var task = function();
                result = await task.ConfigureAwait(continueOnCapturedContext);
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = mapException(exception);
                if (error is null)
                    throw;

                return Result<T>.FromError(error);
            }

            // Outside the try block: a null result is a bug of the caller, not an exception to map.
            return Guard.EnsureNotNullReturned(result, nameof(function));
        }
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns a failed <see cref="Result" /> with the <see cref="Error" /> it returns, or
    ///     with the error that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result. Pass the type argument explicitly to keep
    ///         the nested result.
    ///     </para>
    ///     <para>
    ///         An <see cref="OperationCanceledException" /> is not mapped but rethrown: cancellation is not a failure. An exception is mapped whether the function throws before returning its task or the task faults.
    ///     </para>
    ///     <para>
    ///         A function that returns a <see langword="null" /> error throws an <see cref="InvalidOperationException" />; it is not mapped. The arguments are checked
    ///         when the method is called.
    ///     </para>
    /// </remarks>
    /// <param name="function">The asynchronous function to run. Must not be <see langword="null" />; its result must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by the
    ///     <paramref name="function" />, or with the mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="function" /> returns a <see langword="null" /> error.</exception>
    public static ValueTask<Result> TryAsync(Func<ValueTask<Error>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null)
    {
        return TryAsync<Exception>(function, mapException, continueOnCapturedContext);
    }

    /// <summary>
    ///     Runs the asynchronous <paramref name="function" /> and returns a failed <see cref="Result" /> with the <see cref="Error" /> it returns, or
    ///     with the error that the <paramref name="mapException" /> function creates from the exception the function throws.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The result is flattened: a <see cref="Result" />, a <see cref="Result{T}" /> or an <see cref="Error" /> returned by the function is
    ///         not wrapped in a <see cref="Result{T}" />, and an <see cref="Error" /> gives a failed result. Pass the type argument explicitly to keep
    ///         the nested result.
    ///     </para>
    ///     <para>
    ///         Other exceptions propagate. An <see cref="OperationCanceledException" /> is rethrown, unless <typeparamref name="TException" /> is <see cref="OperationCanceledException" /> or a type derived from it. An exception is mapped whether the function throws before returning its task or the task faults.
    ///     </para>
    ///     <para>
    ///         A function that returns a <see langword="null" /> error throws an <see cref="InvalidOperationException" />; it is not mapped. The arguments are checked
    ///         when the method is called.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TException">The type of the exceptions to map; other exceptions propagate.</typeparam>
    /// <param name="function">The asynchronous function to run. Must not be <see langword="null" />; its result must not be <see langword="null" />.</param>
    /// <param name="mapException">
    ///     The function that creates the <see cref="Error" /> from the exception, or returns <see langword="null" /> to decline it:
    ///     a declined exception is rethrown as it is. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the <paramref name="mapException" /> function when it runs after an
    ///     <c>await</c>, on the captured context; <see langword="false" /> to run them on any thread; <see langword="null" /> (the default) to use
    ///     <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by the
    ///     <paramref name="function" />, or with the mapped error.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="function" /> or the <paramref name="mapException" /> is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">If the <paramref name="function" /> returns a <see langword="null" /> error.</exception>
    public static ValueTask<Result> TryAsync<TException>(
        Func<ValueTask<Error>> function,
        Func<TException, Error?> mapException,
        bool? continueOnCapturedContext = null
    )
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(mapException);

        return Core(function, mapException, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(Func<ValueTask<Error>> function, Func<TException, Error?> mapException, bool continueOnCapturedContext)
        {
            Error returnedError;
            try
            {
                var task = function();
                returnedError = await task.ConfigureAwait(continueOnCapturedContext);
            }
            catch (TException exception) when (IsMapped(exception))
            {
                var error = mapException(exception);
                if (error is null)
                    throw;

                return FromError(error);
            }

            // Outside the try block: a null result is a bug of the caller, not an exception to map.
            var notNullError = Guard.EnsureNotNullReturned(returnedError, nameof(function));

            return FromError(notNullError);
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

    #endregion
}
