// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Tests.Unit.TestingInfrastructure.Fakes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.ResultTests;

// The functions return completed, faulted or canceled tasks directly (not async lambdas) where the overload matters:
// a function that returns a ValueTask binds to the ValueTask overloads only when it is not an async lambda.
public class ResultTryAsyncUnitTests
{
    private static readonly Error MappedError = Error.Unavailable(ErrorUri.None, "Mapped");

    private static Error Map(Exception exception)
    {
        return MappedError;
    }

    private static T Fail<T>(Exception exception)
    {
        throw exception;
    }

    #region TryAsync(Func<Task>, Func<Exception, Error>)

    [Fact]
    public async Task TryAsync_TaskCompletes_ReturnsSuccess()
    {
        // Arrange
        var calls = 0;

        // Act
        var result = await Result.TryAsync(async () =>
        {
            await Task.Yield();
            calls++;
        }, Map);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        calls.ShouldBe(1);
    }

    [Fact]
    public async Task TryAsync_TaskFaults_ReturnsTheMappedError()
    {
        // Act
        var result = await Result.TryAsync(() => Task.FromException(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsync_TaskFaultsAfterAwait_PassesTheExceptionToTheMapper()
    {
        // Arrange
        var thrown = new TimeoutException();
        Exception? received = null;

        // Act
        _ = await Result.TryAsync(async () =>
        {
            await Task.Yield();
            throw thrown;
        }, exception =>
        {
            received = exception;
            return MappedError;
        });

        // Assert
        received.ShouldBeSameAs(thrown);
    }

    [Fact]
    public async Task TryAsync_ActionThrowsBeforeReturningTheTask_ReturnsTheMappedError()
    {
        // Act
        var result = await Result.TryAsync(() => Fail<Task>(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsync_TaskCanceled_Rethrows()
    {
        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => Task.FromCanceled(new CancellationToken(true)), Map));

        // Assert
        exception.ShouldBeAssignableTo<OperationCanceledException>();
    }

    [Fact]
    public async Task TryAsync_ActionReturnsNullTask_ThrowsInvalidOperationExceptionWithoutMapping()
    {
        // Arrange
        const string ExpectedMessage = "The function ‘action’ returned a null task.";
        var mapperCalls = 0;

        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => (Task)null!, _ =>
        {
            mapperCalls++;
            return MappedError;
        }));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>().Message.ShouldBe(ExpectedMessage);
        mapperCalls.ShouldBe(0);
    }

    [Fact]
    public async Task TryAsync_TaskFaultsAndMapperReturnsNull_ThrowsInvalidOperationException()
    {
        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => Task.FromException(new TimeoutException()), _ => null!));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    // The arguments are checked when the method is called, not when the returned task is awaited.
    [Fact]
    public void TryAsync_NullAction_ThrowsArgumentNullExceptionSynchronously()
    {
        // Act
        var exception = Record.Exception(() => { _ = Result.TryAsync((Func<Task>)null!, Map); });

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("action");
    }

    [Fact]
    public void TryAsync_TaskActionAndNullMapper_ThrowsArgumentNullExceptionSynchronously()
    {
        // Act
        var exception = Record.Exception(() => { _ = Result.TryAsync(() => Task.CompletedTask, null!); });

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("mapException");
    }

    #endregion

    #region TryAsync<TException>(Func<Task>, Func<TException, Error>)

    [Fact]
    public async Task TryAsyncTyped_TaskFaultsWithMatchingException_ReturnsTheMappedError()
    {
        // Act
        var result = await Result.TryAsync<TimeoutException>(() => Task.FromException(new TimeoutException()), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsyncTyped_TaskFaultsWithOtherException_Propagates()
    {
        // Act
        var exception = await Record.ExceptionAsync(() =>
            Result.TryAsync<FormatException>(() => Task.FromException(new TimeoutException()), _ => MappedError));

        // Assert
        exception.ShouldBeOfType<TimeoutException>();
    }

    [Fact]
    public async Task TryAsyncTyped_OperationCanceledExceptionAndTaskCanceled_ReturnsTheMappedError()
    {
        // Act
        var result = await Result.TryAsync<OperationCanceledException>(
            () => Task.FromCanceled(new CancellationToken(true)), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    #endregion

    #region TryAsync<T>(Func<Task<T>>, Func<Exception, Error>)

    [Fact]
    public async Task TryAsync_TaskReturnsValue_ReturnsSuccessWithTheValue()
    {
        // Act
        var result = await Result.TryAsync(() => Task.FromResult(42), Map);

        // Assert
        result.Value.ShouldBe(42);
    }

    [Fact]
    public async Task TryAsync_TaskReturnsNull_ReturnsSuccessWithNull()
    {
        // Act
        var result = await Result.TryAsync(() => Task.FromResult<string?>(null), Map);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeNull();
    }

    [Fact]
    public async Task TryAsync_TaskOfValueFaults_ReturnsTheMappedError()
    {
        // Act
        var result = await Result.TryAsync(() => Task.FromException<int>(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsync_TaskOfValueCanceled_Rethrows()
    {
        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => Task.FromCanceled<int>(new CancellationToken(true)), Map));

        // Assert
        exception.ShouldBeAssignableTo<OperationCanceledException>();
    }

    [Fact]
    public async Task TryAsync_FunctionReturnsNullTask_ThrowsInvalidOperationException()
    {
        // Arrange
        const string ExpectedMessage = "The function ‘function’ returned a null task.";

        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => (Task<int>)null!, Map));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>().Message.ShouldBe(ExpectedMessage);
    }

    [Fact]
    public void TryAsync_NullTaskFunction_ThrowsArgumentNullExceptionSynchronously()
    {
        // Act
        var exception = Record.Exception(() => { _ = Result.TryAsync((Func<Task<int>>)null!, Map); });

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("function");
    }

    #endregion

    #region TryAsync<T, TException>(Func<Task<T>>, Func<TException, Error>)

    [Fact]
    public async Task TryAsyncTyped_TaskOfValueFaultsWithMatchingException_ReturnsTheMappedError()
    {
        // Act
        var result = await Result.TryAsync<int, TimeoutException>(() => Task.FromException<int>(new TimeoutException()), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsyncTyped_TaskOfValueFaultsWithOtherException_Propagates()
    {
        // Act
        var exception = await Record.ExceptionAsync(() =>
            Result.TryAsync<int, FormatException>(() => Task.FromException<int>(new TimeoutException()), _ => MappedError));

        // Assert
        exception.ShouldBeOfType<TimeoutException>();
    }

    [Fact]
    public async Task TryAsyncTyped_ExceptionAndTaskOfValueCanceled_Rethrows()
    {
        // Act
        var exception = await Record.ExceptionAsync(() =>
            Result.TryAsync<int, Exception>(() => Task.FromCanceled<int>(new CancellationToken(true)), _ => MappedError));

        // Assert
        exception.ShouldBeAssignableTo<OperationCanceledException>();
    }

    #endregion

    #region TryAsync(Func<ValueTask>, Func<Exception, Error>)

    [Fact]
    public async Task TryAsync_ValueTaskCompletes_ReturnsSuccess()
    {
        // Act
        var result = await Result.TryAsync(() => ValueTask.CompletedTask, Map);

        // Assert
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task TryAsync_ValueTaskFaults_ReturnsTheMappedError()
    {
        // Act
        var result = await Result.TryAsync(() => ValueTask.FromException(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsync_ValueTaskActionThrowsBeforeReturningTheTask_ReturnsTheMappedError()
    {
        // Act
        var result = await Result.TryAsync(() => Fail<ValueTask>(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsync_ValueTaskCanceled_Rethrows()
    {
        // Act
        var exception = await Record.ExceptionAsync(() =>
            Result.TryAsync(() => ValueTask.FromCanceled(new CancellationToken(true)), Map).AsTask());

        // Assert
        exception.ShouldBeAssignableTo<OperationCanceledException>();
    }

    [Fact]
    public void TryAsync_NullValueTaskAction_ThrowsArgumentNullExceptionSynchronously()
    {
        // Act
        var exception = Record.Exception(() => { _ = Result.TryAsync((Func<ValueTask>)null!, Map); });

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("action");
    }

    [Fact]
    public void TryAsync_ValueTaskActionAndNullMapper_ThrowsArgumentNullExceptionSynchronously()
    {
        // Act
        var exception = Record.Exception(() => { _ = Result.TryAsync(() => ValueTask.CompletedTask, null!); });

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("mapException");
    }

    #endregion

    #region TryAsync<TException>(Func<ValueTask>, Func<TException, Error>)

    [Fact]
    public async Task TryAsyncTyped_ValueTaskFaultsWithMatchingException_ReturnsTheMappedError()
    {
        // Act
        var result = await Result.TryAsync<TimeoutException>(() => ValueTask.FromException(new TimeoutException()), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsyncTyped_ValueTaskFaultsWithOtherException_Propagates()
    {
        // Act
        var exception = await Record.ExceptionAsync(() =>
            Result.TryAsync<FormatException>(() => ValueTask.FromException(new TimeoutException()), _ => MappedError).AsTask());

        // Assert
        exception.ShouldBeOfType<TimeoutException>();
    }

    #endregion

    #region TryAsync<T>(Func<ValueTask<T>>, Func<Exception, Error>)

    [Fact]
    public async Task TryAsync_ValueTaskReturnsValue_ReturnsSuccessWithTheValue()
    {
        // Act
        var result = await Result.TryAsync(() => ValueTask.FromResult(42), Map);

        // Assert
        result.Value.ShouldBe(42);
    }

    [Fact]
    public async Task TryAsync_ValueTaskOfValueFaults_ReturnsTheMappedError()
    {
        // Act
        var result = await Result.TryAsync(() => ValueTask.FromException<int>(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsync_ValueTaskOfValueCanceled_Rethrows()
    {
        // Act
        var exception = await Record.ExceptionAsync(() =>
            Result.TryAsync(() => ValueTask.FromCanceled<int>(new CancellationToken(true)), Map).AsTask());

        // Assert
        exception.ShouldBeAssignableTo<OperationCanceledException>();
    }

    [Fact]
    public void TryAsync_NullValueTaskFunction_ThrowsArgumentNullExceptionSynchronously()
    {
        // Act
        var exception = Record.Exception(() => { _ = Result.TryAsync((Func<ValueTask<int>>)null!, Map); });

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("function");
    }

    #endregion

    #region TryAsync<T, TException>(Func<ValueTask<T>>, Func<TException, Error>)

    [Fact]
    public async Task TryAsyncTyped_ValueTaskOfValueFaultsWithMatchingException_ReturnsTheMappedError()
    {
        // Act
        var result = await Result.TryAsync<int, TimeoutException>(() => ValueTask.FromException<int>(new TimeoutException()), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsyncTyped_ValueTaskOfValueFaultsWithOtherException_Propagates()
    {
        // Act
        var exception = await Record.ExceptionAsync(() =>
            Result.TryAsync<int, FormatException>(() => ValueTask.FromException<int>(new TimeoutException()), _ => MappedError).AsTask());

        // Assert
        exception.ShouldBeOfType<TimeoutException>();
    }

    #endregion

    #region continueOnCapturedContext

    // The task faults on a thread-pool thread after TryAsync has started awaiting it, so the mapper runs in the continuation,
    // on the captured context only when continueOnCapturedContext is true.

    [Fact]
    public void TryAsync_TaskAndContinueOnCapturedContextTrue_RunsTheMapperOnTheCapturedContext()
    {
        // Act
        var (context, mapperContext) = RunTaskTryAsync(continueOnCapturedContext: true);

        // Assert
        mapperContext.ShouldBeSameAs(context);
    }

    [Fact]
    public void TryAsync_TaskAndContinueOnCapturedContextFalse_RunsTheMapperOffTheCapturedContext()
    {
        // Act
        var (context, mapperContext) = RunTaskTryAsync(continueOnCapturedContext: false);

        // Assert
        mapperContext.ShouldNotBeSameAs(context);
    }

    [Fact]
    public void TryAsync_ValueTaskAndContinueOnCapturedContextTrue_RunsTheMapperOnTheCapturedContext()
    {
        // Act
        var (context, mapperContext) = RunValueTaskTryAsync(continueOnCapturedContext: true);

        // Assert
        mapperContext.ShouldBeSameAs(context);
    }

    [Fact]
    public void TryAsync_ValueTaskAndContinueOnCapturedContextFalse_RunsTheMapperOffTheCapturedContext()
    {
        // Act
        var (context, mapperContext) = RunValueTaskTryAsync(continueOnCapturedContext: false);

        // Assert
        mapperContext.ShouldNotBeSameAs(context);
    }

    private static (SynchronizationContext Context, SynchronizationContext? MapperContext) RunTaskTryAsync(bool continueOnCapturedContext)
    {
        return SingleThreadSynchronizationContext.Run(async context =>
        {
            var source = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            SynchronizationContext? captured = null;

            var call = Result.TryAsync(() => source.Task, _ =>
            {
                captured = SynchronizationContext.Current;
                return MappedError;
            }, continueOnCapturedContext);

            ThreadPool.QueueUserWorkItem(_ => source.SetException(new TimeoutException()));
            await call;

            return (context, captured);
        });
    }

    private static (SynchronizationContext Context, SynchronizationContext? MapperContext) RunValueTaskTryAsync(bool continueOnCapturedContext)
    {
        return SingleThreadSynchronizationContext.Run(async context =>
        {
            var source = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            SynchronizationContext? captured = null;

            var call = Result.TryAsync(() => new ValueTask(source.Task), _ =>
            {
                captured = SynchronizationContext.Current;
                return MappedError;
            }, continueOnCapturedContext);

            ThreadPool.QueueUserWorkItem(_ => source.SetException(new TimeoutException()));
            await call;

            return (context, captured);
        });
    }

    #endregion
}
