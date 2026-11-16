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

namespace Maple.Results.Tests.Unit.ResultTests;

// A function that returns a Result, a Result<T> or an Error is flattened (#193): its result is returned as it is, and an Error
// becomes a failed result, as in IfSuccess and Match. The declared types of the variables also check the overloads at compile
// time: without the flattening overloads, `Result<int> result = Result.Try(() => GetResult(), Map)` does not compile.
public class ResultTryFlatteningUnitTests
{
    private static readonly Error MappedError = Error.Unavailable(ErrorUri.None, "Mapped");
    private static readonly Error ReturnedError = Error.NotFound(ErrorUri.None, "Returned");

    private static Error Map(Exception exception)
    {
        return MappedError;
    }

    private static T Fail<T>(Exception exception)
    {
        throw exception;
    }

    #region Try(Func<Result>, Func<Exception, Error>)

    [Fact]
    public void Try_FunctionReturnsSuccessResult_ReturnsSuccess()
    {
        // Act
        Result result = Result.Try(() => Result.Success(), Map);

        // Assert
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void Try_FunctionReturnsFailedResult_ReturnsItsError()
    {
        // Act
        Result result = Result.Try(() => Result.FromError(ReturnedError), Map);

        // Assert
        result.Error.ShouldBe(ReturnedError);
    }

    [Fact]
    public void Try_ResultFunctionThrows_ReturnsTheMappedError()
    {
        // Act
        Result result = Result.Try(() => Fail<Result>(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public void Try_ResultFunctionThrowsOperationCanceledException_Rethrows()
    {
        // Act
        var exception = Record.Exception(() => Result.Try(() => Fail<Result>(new OperationCanceledException()), Map));

        // Assert
        exception.ShouldBeOfType<OperationCanceledException>();
    }

    // A null result is a bug of the caller, not a failure of the operation, so it is not mapped.
    [Fact]
    public void Try_ResultFunctionReturnsNull_ThrowsInvalidOperationExceptionWithoutMapping()
    {
        // Arrange
        const string ExpectedMessage = "The function ‘function’ returned null.";
        var mapperCalls = 0;

        // Act
        var exception = Record.Exception(() => Result.Try(() => (Result)null!, _ =>
        {
            mapperCalls++;
            return MappedError;
        }));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>().Message.ShouldBe(ExpectedMessage);
        mapperCalls.ShouldBe(0);
    }

    [Fact]
    public void Try_NullResultFunction_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => Result.Try((Func<Result>)null!, Map));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("function");
    }

    [Fact]
    public void Try_ResultFunctionAndNullMapper_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => Result.Try(() => Result.Success(), null!));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("mapException");
    }

    #endregion

    #region Try<TException>(Func<Result>, Func<TException, Error>)

    [Fact]
    public void TryTyped_ResultFunctionThrowsMatchingException_ReturnsTheMappedError()
    {
        // Act
        Result result = Result.Try(() => Fail<Result>(new TimeoutException()), (TimeoutException _) => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public void TryTyped_ResultFunctionThrowsOtherException_Propagates()
    {
        // Act
        var exception = Record.Exception(() => Result.Try(() => Fail<Result>(new TimeoutException()), (FormatException _) => MappedError));

        // Assert
        exception.ShouldBeOfType<TimeoutException>();
    }

    [Fact]
    public void TryTyped_ResultFunctionReturnsFailedResult_ReturnsItsError()
    {
        // Act
        Result result = Result.Try<FormatException>(() => Result.FromError(ReturnedError), _ => MappedError);

        // Assert
        result.Error.ShouldBe(ReturnedError);
    }

    #endregion

    #region Try<T>(Func<Result<T>>, Func<Exception, Error>)

    [Fact]
    public void Try_FunctionReturnsSuccessfulResultOfValue_ReturnsItsValue()
    {
        // Act
        Result<int> result = Result.Try(() => Result.FromValue(42), Map);

        // Assert
        result.Value.ShouldBe(42);
    }

    [Fact]
    public void Try_FunctionReturnsFailedResultOfValue_ReturnsItsError()
    {
        // Act
        Result<int> result = Result.Try(() => Result<int>.FromError(ReturnedError), Map);

        // Assert
        result.Error.ShouldBe(ReturnedError);
    }

    [Fact]
    public void Try_ResultOfValueFunctionThrows_ReturnsTheMappedError()
    {
        // Act
        Result<int> result = Result.Try(() => Fail<Result<int>>(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public void Try_ResultOfValueFunctionReturnsNull_ThrowsInvalidOperationException()
    {
        // Act
        var exception = Record.Exception(() => Result.Try(() => (Result<int>)null!, Map));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public void Try_NullResultOfValueFunction_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => Result.Try((Func<Result<int>>)null!, Map));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("function");
    }

    // Explicit type arguments keep the nested result on purpose, as IfSuccess<Error>(…) does.
    [Fact]
    public void Try_ExplicitResultTypeArgument_KeepsTheNestedResult()
    {
        // Act
        Result<Result<int>> result = Result.Try<Result<int>>(() => Result.FromValue(42), Map);

        // Assert
        result.Value.Value.ShouldBe(42);
    }

    #endregion

    #region Try<T, TException>(Func<Result<T>>, Func<TException, Error>)

    [Fact]
    public void TryTyped_FunctionReturnsSuccessfulResultOfValue_ReturnsItsValue()
    {
        // Act
        Result<int> result = Result.Try(() => Result.FromValue(42), (FormatException _) => MappedError);

        // Assert
        result.Value.ShouldBe(42);
    }

    [Fact]
    public void TryTyped_ResultOfValueFunctionThrowsMatchingException_ReturnsTheMappedError()
    {
        // Act
        Result<int> result = Result.Try<int, TimeoutException>(() => Fail<Result<int>>(new TimeoutException()), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public void TryTyped_ResultOfValueFunctionThrowsOtherException_Propagates()
    {
        // Act
        var exception = Record.Exception(() =>
            Result.Try<int, FormatException>(() => Fail<Result<int>>(new TimeoutException()), _ => MappedError));

        // Assert
        exception.ShouldBeOfType<TimeoutException>();
    }

    [Fact]
    public void TryTyped_ExplicitResultTypeArgument_KeepsTheNestedResult()
    {
        // Act
        Result<Result<int>> result = Result.Try<Result<int>, FormatException>(() => Result.FromValue(42), _ => MappedError);

        // Assert
        result.Value.Value.ShouldBe(42);
    }

    #endregion

    #region Try(Func<Error>, Func<Exception, Error>)

    [Fact]
    public void Try_FunctionReturnsError_ReturnsFailedResultWithIt()
    {
        // Act
        Result result = Result.Try(() => ReturnedError, Map);

        // Assert
        result.Error.ShouldBe(ReturnedError);
    }

    [Fact]
    public void Try_ErrorFunctionThrows_ReturnsTheMappedError()
    {
        // Act
        Result result = Result.Try(() => Fail<Error>(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public void Try_ErrorFunctionReturnsNull_ThrowsInvalidOperationException()
    {
        // Arrange
        const string ExpectedMessage = "The function ‘function’ returned null.";

        // Act
        var exception = Record.Exception(() => Result.Try(() => (Error)null!, Map));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>().Message.ShouldBe(ExpectedMessage);
    }

    [Fact]
    public void Try_NullErrorFunction_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => Result.Try((Func<Error>)null!, Map));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("function");
    }

    [Fact]
    public void Try_ExplicitErrorTypeArgument_KeepsTheErrorAsAValue()
    {
        // Act
        Result<Error> result = Result.Try<Error>(() => ReturnedError, Map);

        // Assert
        result.Value.ShouldBe(ReturnedError);
    }

    #endregion

    #region Try<TException>(Func<Error>, Func<TException, Error>)

    [Fact]
    public void TryTyped_FunctionReturnsError_ReturnsFailedResultWithIt()
    {
        // Act
        Result result = Result.Try(() => ReturnedError, (FormatException _) => MappedError);

        // Assert
        result.Error.ShouldBe(ReturnedError);
    }

    [Fact]
    public void TryTyped_ErrorFunctionThrowsMatchingException_ReturnsTheMappedError()
    {
        // Act
        Result result = Result.Try<TimeoutException>(() => Fail<Error>(new TimeoutException()), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public void TryTyped_ErrorFunctionThrowsOtherException_Propagates()
    {
        // Act
        var exception = Record.Exception(() => Result.Try<FormatException>(() => Fail<Error>(new TimeoutException()), _ => MappedError));

        // Assert
        exception.ShouldBeOfType<TimeoutException>();
    }

    #endregion
}
