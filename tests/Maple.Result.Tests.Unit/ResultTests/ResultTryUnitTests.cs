// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Tests.Unit.TestingInfrastructure.Helpers;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.ResultTests;

public class ResultTryUnitTests
{
    private static readonly Error MappedError = Error.Unavailable(ErrorUri.None, "Mapped");

    private static Error Map(Exception exception)
    {
        return MappedError;
    }

    // A lambda that always throws (`() => throw …`) has no return type and converts to every delegate type, so it binds to the
    // overload for a function that returns an Error. The tests throw through these methods to reach the other overloads:
    // `() => Throw(…)` is an Action, `() => Fail<int>(…)` a Func<int>.
    private static void Throw(Exception exception)
    {
        throw exception;
    }

    private static T Fail<T>(Exception exception)
    {
        throw exception;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowFromNamedMethod(Exception exception)
    {
        throw exception;
    }

    // One mapper for several types: the others are declined (null) and rethrown.
    private static Error? MapFormatOrArgument(Exception exception)
    {
        return exception switch
        {
            FormatException => MappedError,
            ArgumentException => MappedError,
            _ => null
        };
    }

    #region Try(Action, Func<Exception, Error>)

    [Fact]
    public void Try_ActionCompletes_ReturnsSuccess()
    {
        // Arrange
        var calls = 0;

        // Act
        var result = Result.Try(() => { calls++; }, Map);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        calls.ShouldBe(1);
    }

    [Fact]
    public void Try_ActionCompletes_DoesNotCallTheMapper()
    {
        // Arrange
        var mapperCalls = 0;

        // Act
        _ = Result.Try(() => { }, _ =>
        {
            mapperCalls++;
            return MappedError;
        });

        // Assert
        mapperCalls.ShouldBe(0);
    }

    [Fact]
    public void Try_ActionThrows_ReturnsTheMappedError()
    {
        // Act
        var result = Result.Try(() => Throw(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public void Try_ActionThrows_PassesTheExceptionToTheMapper()
    {
        // Arrange
        var thrown = new TimeoutException();
        Exception? received = null;

        // Act
        _ = Result.Try(() => Throw(thrown), exception =>
        {
            received = exception;
            return MappedError;
        });

        // Assert
        received.ShouldBeSameAs(thrown);
    }

    [Fact]
    public void Try_ActionThrowsOperationCanceledException_Rethrows()
    {
        // Act
        var exception = Record.Exception(() => Result.Try(() => Throw(new OperationCanceledException()), Map));

        // Assert
        exception.ShouldBeOfType<OperationCanceledException>();
    }

    [Fact]
    public void Try_ActionThrowsTaskCanceledException_Rethrows()
    {
        // Act
        var exception = Record.Exception(() => Result.Try(() => Throw(new TaskCanceledException()), Map));

        // Assert
        exception.ShouldBeOfType<TaskCanceledException>();
    }

    [Fact]
    public void Try_NullAction_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => Result.Try((Action)null!, Map));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("action");
    }

    [Fact]
    public void Try_ActionAndNullMapper_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => Result.Try(() => { }, null!));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("mapException");
    }

    // A mapper that returns null declines the exception: it is rethrown as it is (#195).
    [Fact]
    public void Try_ActionThrowsAndMapperDeclines_RethrowsTheSameException()
    {
        // Arrange
        var thrown = new TimeoutException();

        // Act
        var exception = Record.Exception(() => Result.Try(() => Throw(thrown), _ => null));

        // Assert
        exception.ShouldBeSameAs(thrown);
    }

    // `throw;` inside the catch block keeps the stack trace of the original exception.
    [Fact]
    public void Try_ActionThrowsAndMapperDeclines_KeepsTheStackTrace()
    {
        // Act
        var exception = Record.Exception(() => Result.Try(() => ThrowFromNamedMethod(new TimeoutException()), _ => null));

        // Assert
        exception.ShouldNotBeNull().StackTrace.ShouldNotBeNull().ShouldContain(nameof(ThrowFromNamedMethod));
    }

    [Fact]
    public void Try_MapperForSeveralTypes_MapsTheHandledTypes()
    {
        // Act
        var result = Result.Try(() => Throw(new FormatException()), MapFormatOrArgument);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public void Try_MapperForSeveralTypes_RethrowsTheOtherTypes()
    {
        // Act
        var exception = Record.Exception(() => Result.Try(() => Throw(new TimeoutException()), MapFormatOrArgument));

        // Assert
        exception.ShouldBeOfType<TimeoutException>();
    }

    [Fact]
    public void TryTyped_ActionThrowsAndMapperDeclines_RethrowsTheSameException()
    {
        // Arrange
        var thrown = new FormatException();

        // Act
        var exception = Record.Exception(() => Result.Try<FormatException>(() => Throw(thrown), _ => null));

        // Assert
        exception.ShouldBeSameAs(thrown);
    }

    [Fact]
    public void Try_ActionThrowsAndMapperThrows_PropagatesTheMapperException()
    {
        // Arrange
        var mapperException = new FormatException();

        // Act
        var exception = Record.Exception(() => Result.Try(() => Throw(new TimeoutException()), _ => throw mapperException));

        // Assert
        exception.ShouldBeSameAs(mapperException);
    }

    #endregion

    #region Try<TException>(Action, Func<TException, Error>)

    [Fact]
    public void TryTyped_ActionThrowsMatchingException_ReturnsTheMappedError()
    {
        // Arrange
        FormatException? received = null;

        // Act
        var result = Result.Try<FormatException>(() => Throw(new FormatException()), exception =>
        {
            received = exception;
            return MappedError;
        });

        // Assert
        result.Error.ShouldBe(MappedError);
        received.ShouldNotBeNull();
    }

    [Fact]
    public void TryTyped_ActionThrowsDerivedException_ReturnsTheMappedError()
    {
        // Act
        var result = Result.Try<ArgumentException>(() => Throw(new ArgumentNullException("value")), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public void TryTyped_ActionThrowsOtherException_Propagates()
    {
        // Act
        var exception = Record.Exception(() => Result.Try<FormatException>(() => Throw(new TimeoutException()), _ => MappedError));

        // Assert
        exception.ShouldBeOfType<TimeoutException>();
    }

    [Fact]
    public void TryTyped_ExceptionAndActionThrowsOperationCanceledException_Rethrows()
    {
        // Act
        var exception = Record.Exception(() => Result.Try<Exception>(() => Throw(new OperationCanceledException()), _ => MappedError));

        // Assert
        exception.ShouldBeOfType<OperationCanceledException>();
    }

    // Asking for the cancellation exception itself is an explicit choice, so it is caught.
    [Fact]
    public void TryTyped_OperationCanceledExceptionAndActionThrowsIt_ReturnsTheMappedError()
    {
        // Act
        var result = Result.Try<OperationCanceledException>(() => Throw(new TaskCanceledException()), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public void TryTyped_ActionCompletes_ReturnsSuccess()
    {
        // Act
        var result = Result.Try<FormatException>(() => { }, _ => MappedError);

        // Assert
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void TryTyped_NullAction_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => Result.Try<FormatException>((Action)null!, _ => MappedError));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("action");
    }

    [Fact]
    public void TryTyped_ActionAndNullMapper_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => Result.Try<FormatException>(() => { }, null!));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("mapException");
    }

    #endregion

    #region Try<T>(Func<T>, Func<Exception, Error>)

    [Fact]
    public void Try_FunctionReturnsValue_ReturnsSuccessWithTheValue()
    {
        // Act
        var result = Result.Try(() => 42, Map);

        // Assert
        result.Value.ShouldBe(42);
    }

    [Fact]
    public void Try_FunctionReturnsNull_ReturnsSuccessWithNull()
    {
        // Act
        var result = Result.Try<string?>(() => null, Map);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeNull();
    }

    [Fact]
    public void Try_FunctionThrows_ReturnsTheMappedError()
    {
        // Act
        var result = Result.Try(() => Fail<int>(new TimeoutException()), Map);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public void Try_FunctionThrowsOperationCanceledException_Rethrows()
    {
        // Act
        var exception = Record.Exception(() => Result.Try(() => Fail<int>(new OperationCanceledException()), Map));

        // Assert
        exception.ShouldBeOfType<OperationCanceledException>();
    }

    [Fact]
    public void Try_NullFunction_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => Result.Try((Func<int>)null!, Map));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("function");
    }

    [Fact]
    public void Try_FunctionAndNullMapper_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => Result.Try(() => 42, null!));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("mapException");
    }

    [Fact]
    public void Try_FunctionThrowsAndMapperDeclines_RethrowsTheSameException()
    {
        // Arrange
        var thrown = new TimeoutException();

        // Act
        var exception = Record.Exception(() => Result.Try(() => Fail<int>(thrown), _ => null));

        // Assert
        exception.ShouldBeSameAs(thrown);
    }

    #endregion

    #region Try<T, TException>(Func<T>, Func<TException, Error>)

    [Fact]
    public void TryTyped_FunctionReturnsValue_ReturnsSuccessWithTheValue()
    {
        // Act
        var result = Result.Try<int, FormatException>(() => int.Parse("42"), _ => MappedError);

        // Assert
        result.Value.ShouldBe(42);
    }

    [Fact]
    public void TryTyped_FunctionThrowsMatchingException_ReturnsTheMappedError()
    {
        // Act
        var result = Result.Try<int, FormatException>(() => int.Parse("not a number"), _ => MappedError);

        // Assert
        result.Error.ShouldBe(MappedError);
    }

    [Fact]
    public void TryTyped_FunctionThrowsOtherException_Propagates()
    {
        // Act
        var exception = Record.Exception(() => Result.Try<int, FormatException>(() => Fail<int>(new TimeoutException()), _ => MappedError));

        // Assert
        exception.ShouldBeOfType<TimeoutException>();
    }

    [Fact]
    public void TryTyped_ExceptionAndFunctionThrowsOperationCanceledException_Rethrows()
    {
        // Act
        var exception = Record.Exception(() => Result.Try<int, Exception>(() => Fail<int>(new OperationCanceledException()), _ => MappedError));

        // Assert
        exception.ShouldBeOfType<OperationCanceledException>();
    }

    [Fact]
    public void TryTyped_NullFunction_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => Result.Try<int, FormatException>((Func<int>)null!, _ => MappedError));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("function");
    }

    [Fact]
    public void TryTyped_FunctionAndNullMapper_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => Result.Try<int, FormatException>(() => 42, null!));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("mapException");
    }

    #endregion

    #region TryAsync overloads and their priority

    // An async lambda converts to both Func<Task…> and Func<ValueTask…>; the Task overloads win (no CS0121), and a function that
    // returns a ValueTask still binds to the ValueTask overloads, because the Task ones do not apply.
    [Fact]
    public void TryAsync_TaskOverloads_HavePriorityOne()
    {
        // Act
        var priorities = typeof(Result).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == nameof(Result.TryAsync) && OverloadReflectionHelper.IsTaskFunction(m.GetParameters()[0].ParameterType))
            .Select(OverloadReflectionHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(10);
        priorities.ShouldAllBe(p => p == 1);
    }

    #endregion
}
