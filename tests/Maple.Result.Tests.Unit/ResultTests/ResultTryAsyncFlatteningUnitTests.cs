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
using System.Threading;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.ResultTests;

// The asynchronous counterpart of ResultTryFlatteningUnitTests (#193). The functions return completed, faulted or canceled tasks
// directly (not async lambdas) where the overload matters: a function that returns a ValueTask binds to the ValueTask overloads only
// when it is not an async lambda.
public class ResultTryAsyncFlatteningUnitTests
{
    private static readonly Error MappedError = Error.Unavailable(ErrorUri.None, "Mapped");
    private static readonly Error ReturnedError = Error.NotFound(ErrorUri.None, "Returned");

    private static Error Map(Exception exception)
    {
        return MappedError;
    }

    #region TryAsync(Func<Task<Result>>, Func<Exception, Error>)

    [Fact]
    public async Task TryAsync_TaskOfSuccessResult_ReturnsSuccess()
    {
        // Act
        Result result = await Result.TryAsync(async () =>
        {
            await Task.Yield();
            return Result.Success();
        }, Map);

        // Assert
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task TryAsync_TaskOfFailedResult_ReturnsItsError()
    {
        // Act
        Result result = await Result.TryAsync(() => Task.FromResult(Result.FromError(ReturnedError)), Map);

        // Assert
        result.Error.ShouldBe(ReturnedError);
    }

    [Fact]
    public async Task TryAsync_TaskOfResultFaults_ReturnsTheMappedError()
    {
        // Act
        Result result = await Result.TryAsync(() => Task.FromException<Result>(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsync_TaskOfResultCanceled_Rethrows()
    {
        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => Task.FromCanceled<Result>(new CancellationToken(true)), Map));

        // Assert
        exception.ShouldBeAssignableTo<OperationCanceledException>();
    }

    [Fact]
    public async Task TryAsync_ResultFunctionReturnsNullTask_ThrowsInvalidOperationException()
    {
        // Arrange
        const string ExpectedMessage = "The function ‘function’ returned a null task.";

        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => (Task<Result>)null!, Map));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>().Message.ShouldBe(ExpectedMessage);
    }

    // A null result is a bug of the caller, not a failure of the operation, so it is not mapped.
    [Fact]
    public async Task TryAsync_TaskOfNullResult_ThrowsInvalidOperationExceptionWithoutMapping()
    {
        // Arrange
        const string ExpectedMessage = "The function ‘function’ returned null.";
        var mapperCalls = 0;

        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => Task.FromResult<Result>(null!), _ =>
        {
            mapperCalls++;
            return MappedError;
        }));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>().Message.ShouldBe(ExpectedMessage);
        mapperCalls.ShouldBe(0);
    }

    [Fact]
    public void TryAsync_NullResultTaskFunction_ThrowsArgumentNullExceptionSynchronously()
    {
        // Act
        var exception = Record.Exception(() => { _ = Result.TryAsync((Func<Task<Result>>)null!, Map); });

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("function");
    }

    [Fact]
    public async Task TryAsync_TaskOfResultFaultsAndMapperDeclines_RethrowsTheSameException()
    {
        // Arrange
        var thrown = new TimeoutException();

        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => Task.FromException<Result>(thrown), _ => null));

        // Assert
        exception.ShouldBeSameAs(thrown);
    }

    [Fact]
    public async Task TryAsync_ValueTaskOfResultOfValueFaultsAndMapperDeclines_RethrowsTheSameException()
    {
        // Arrange
        var thrown = new TimeoutException();

        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => ValueTask.FromException<Result<int>>(thrown), _ => null).AsTask());

        // Assert
        exception.ShouldBeSameAs(thrown);
    }

    #endregion

    #region TryAsync<TException>(Func<Task<Result>>, Func<TException, Error>)

    [Fact]
    public async Task TryAsyncTyped_TaskOfResultFaultsWithMatchingException_ReturnsTheMappedError()
    {
        // Act
        Result result = await Result.TryAsync<TimeoutException>(() => Task.FromException<Result>(new TimeoutException()), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsyncTyped_TaskOfResultFaultsWithOtherException_Propagates()
    {
        // Act
        var exception = await Record.ExceptionAsync(()
            => Result.TryAsync<FormatException>(() => Task.FromException<Result>(new TimeoutException()), _ => MappedError));

        // Assert
        exception.ShouldBeOfType<TimeoutException>();
    }

    #endregion

    #region TryAsync<T>(Func<Task<Result<T>>>, Func<Exception, Error>)

    [Fact]
    public async Task TryAsync_TaskOfSuccessfulResultOfValue_ReturnsItsValue()
    {
        // Act
        Result<int> result = await Result.TryAsync(async () =>
        {
            await Task.Yield();
            return Result.FromValue(42);
        }, Map);

        // Assert
        result.Value.ShouldBe(42);
    }

    [Fact]
    public async Task TryAsync_TaskOfFailedResultOfValue_ReturnsItsError()
    {
        // Act
        Result<int> result = await Result.TryAsync(() => Task.FromResult(Result<int>.FromError(ReturnedError)), Map);

        // Assert
        result.Error.ShouldBe(ReturnedError);
    }

    [Fact]
    public async Task TryAsync_TaskOfResultOfValueFaults_ReturnsTheMappedError()
    {
        // Act
        Result<int> result = await Result.TryAsync(() => Task.FromException<Result<int>>(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsync_TaskOfNullResultOfValue_ThrowsInvalidOperationException()
    {
        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => Task.FromResult<Result<int>>(null!), Map));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task TryAsync_ExplicitResultTypeArgument_KeepsTheNestedResult()
    {
        // Act
        Result<Result<int>> result = await Result.TryAsync<Result<int>>(() => Task.FromResult(Result.FromValue(42)), Map);

        // Assert
        result.Value.Value.ShouldBe(42);
    }

    #endregion

    #region TryAsync<T, TException>(Func<Task<Result<T>>>, Func<TException, Error>)

    [Fact]
    public async Task TryAsyncTyped_TaskOfResultOfValueFaultsWithMatchingException_ReturnsTheMappedError()
    {
        // Act
        Result<int> result = await Result.TryAsync<int, TimeoutException>(
            () => Task.FromException<Result<int>>(new TimeoutException()), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsyncTyped_TaskOfResultOfValueFaultsWithOtherException_Propagates()
    {
        // Act
        var exception = await Record.ExceptionAsync(()
            => Result.TryAsync<int, FormatException>(() => Task.FromException<Result<int>>(new TimeoutException()), _ => MappedError));

        // Assert
        exception.ShouldBeOfType<TimeoutException>();
    }

    #endregion

    #region TryAsync(Func<Task<Error>>, Func<Exception, Error>)

    [Fact]
    public async Task TryAsync_TaskOfError_ReturnsFailedResultWithIt()
    {
        // Act
        Result result = await Result.TryAsync(async () =>
        {
            await Task.Yield();
            return ReturnedError;
        }, Map);

        // Assert
        result.Error.ShouldBe(ReturnedError);
    }

    [Fact]
    public async Task TryAsync_TaskOfErrorFaults_ReturnsTheMappedError()
    {
        // Act
        Result result = await Result.TryAsync(() => Task.FromException<Error>(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsync_TaskOfNullError_ThrowsInvalidOperationException()
    {
        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => Task.FromResult<Error>(null!), Map));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    #endregion

    #region TryAsync<TException>(Func<Task<Error>>, Func<TException, Error>)

    [Fact]
    public async Task TryAsyncTyped_TaskOfErrorFaultsWithMatchingException_ReturnsTheMappedError()
    {
        // Act
        Result result = await Result.TryAsync<TimeoutException>(() => Task.FromException<Error>(new TimeoutException()), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsyncTyped_TaskOfError_ReturnsFailedResultWithIt()
    {
        // Act
        Result result = await Result.TryAsync<FormatException>(() => Task.FromResult(ReturnedError), _ => MappedError);

        // Assert
        result.Error.ShouldBe(ReturnedError);
    }

    #endregion

    #region TryAsync(Func<ValueTask<Result>>, Func<Exception, Error>)

    [Fact]
    public async Task TryAsync_ValueTaskOfFailedResult_ReturnsItsError()
    {
        // Act
        Result result = await Result.TryAsync(() => ValueTask.FromResult(Result.FromError(ReturnedError)), Map);

        // Assert
        result.Error.ShouldBe(ReturnedError);
    }

    [Fact]
    public async Task TryAsync_ValueTaskOfResultFaults_ReturnsTheMappedError()
    {
        // Act
        Result result = await Result.TryAsync(() => ValueTask.FromException<Result>(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsync_ValueTaskOfNullResult_ThrowsInvalidOperationException()
    {
        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => ValueTask.FromResult<Result>(null!), Map).AsTask());

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public void TryAsync_NullResultValueTaskFunction_ThrowsArgumentNullExceptionSynchronously()
    {
        // Act
        var exception = Record.Exception(() => { _ = Result.TryAsync((Func<ValueTask<Result>>)null!, Map); });

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("function");
    }

    [Fact]
    public async Task TryAsyncTyped_ValueTaskOfResultFaultsWithMatchingException_ReturnsTheMappedError()
    {
        // Act
        Result result = await Result.TryAsync<TimeoutException>(() => ValueTask.FromException<Result>(new TimeoutException()), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    #endregion

    #region TryAsync<T>(Func<ValueTask<Result<T>>>, Func<Exception, Error>)

    [Fact]
    public async Task TryAsync_ValueTaskOfSuccessfulResultOfValue_ReturnsItsValue()
    {
        // Act
        Result<int> result = await Result.TryAsync(() => ValueTask.FromResult(Result.FromValue(42)), Map);

        // Assert
        result.Value.ShouldBe(42);
    }

    [Fact]
    public async Task TryAsync_ValueTaskOfResultOfValueFaults_ReturnsTheMappedError()
    {
        // Act
        Result<int> result = await Result.TryAsync(() => ValueTask.FromException<Result<int>>(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public async Task TryAsync_ValueTaskOfNullResultOfValue_ThrowsInvalidOperationException()
    {
        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => ValueTask.FromResult<Result<int>>(null!), Map).AsTask());

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task TryAsyncTyped_ValueTaskOfResultOfValueFaultsWithOtherException_Propagates()
    {
        // Act
        var exception = await Record.ExceptionAsync(()
            => Result.TryAsync<int, FormatException>(() => ValueTask.FromException<Result<int>>(new TimeoutException()), _ => MappedError).AsTask());

        // Assert
        exception.ShouldBeOfType<TimeoutException>();
    }

    #endregion

    #region TryAsync(Func<ValueTask<Error>>, Func<Exception, Error>)

    [Fact]
    public async Task TryAsync_ValueTaskOfError_ReturnsFailedResultWithIt()
    {
        // Act
        Result result = await Result.TryAsync(() => ValueTask.FromResult(ReturnedError), Map);

        // Assert
        result.Error.ShouldBe(ReturnedError);
    }

    [Fact]
    public async Task TryAsync_ValueTaskOfNullError_ThrowsInvalidOperationException()
    {
        // Act
        var exception = await Record.ExceptionAsync(() => Result.TryAsync(() => ValueTask.FromResult<Error>(null!), Map).AsTask());

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task TryAsyncTyped_ValueTaskOfErrorFaultsWithMatchingException_ReturnsTheMappedError()
    {
        // Act
        Result result = await Result.TryAsync<TimeoutException>(() => ValueTask.FromException<Error>(new TimeoutException()), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    #endregion
}
