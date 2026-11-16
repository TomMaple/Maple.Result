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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.ResultTests;

public class ResultTryUnitTests
{
    private static readonly Error MappedError = Error.Unavailable(ErrorUri.None, "Mapped");

    private static Error Map(Exception exception)
    {
        return MappedError;
    }

    // A lambda that always throws (`() => throw …`) converts to every delegate type, including the Func<Task> traps, so the
    // tests throw through these methods: `() => Throw(…)` is an Action, `() => Fail<int>(…)` a Func<int>.
    private static void Throw(Exception exception)
    {
        throw exception;
    }

    private static T Fail<T>(Exception exception)
    {
        throw exception;
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

    [Fact]
    public void Try_ActionThrowsAndMapperReturnsNull_ThrowsInvalidOperationException()
    {
        // Arrange
        const string ExpectedMessage = "The function ‘mapException’ returned null.";

        // Act
        var exception = Record.Exception(() => Result.Try(() => Throw(new TimeoutException()), _ => null!));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>().Message.ShouldBe(ExpectedMessage);
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
    public void Try_FunctionThrowsAndMapperReturnsNull_ThrowsInvalidOperationException()
    {
        // Act
        var exception = Record.Exception(() => Result.Try(() => Fail<int>(new TimeoutException()), _ => null!));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
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

    #region Not supported: functions returning a task

    // An async lambda converts to Action (async void: its exceptions escape Try), and a lambda returning a task binds to Try<T>
    // with T = Task (a result holding a task that is never awaited). These overloads turn both into compile errors (CS0619).

    [Fact]
    public void Try_TaskFunctionOverloads_AreHiddenCompileErrors()
    {
        // Arrange
        const int ExpectedCount = 7;

        // Act
        var traps = typeof(Result).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == nameof(Result.Try) && OverloadTrapHelper.IsTrap(m, "The function returns a task"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(ExpectedCount);
        traps.ShouldAllBe(m => m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    // The traps for functions returning a Task or a Task<T> win over Try(Action), Try<T>(Func<T>) and Try<T, TException>(Func<T>),
    // which would otherwise bind an async lambda: Func<Task> for both mappers, and Func<Task<T>> for the typed one (#191).
    [Fact]
    public void Try_TaskTrapOverloads_HavePriorityOne()
    {
        // Act
        var priorities = typeof(Result).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == nameof(Result.Try)
                        && OverloadTrapHelper.IsTrap(m, "The function returns a task")
                        && OverloadTrapHelper.IsTaskFunction(m.GetParameters()[0].ParameterType))
            .Select(OverloadTrapHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(3);
        priorities.ShouldAllBe(p => p == 1);
    }

    // With the untyped mapper, the Func<Task> trap also catches Task<T> (a Task<T> converts to Task) and the Func<ValueTask<T>> trap
    // catches an async lambda that returns a value. With a typed mapper the call has two type parameters, so the typed family needs its
    // own traps for Task<T> and ValueTask<T>: without them, three shapes bound to Try<T, TException> with T = a task (#191).
    [Fact]
    public void Try_TypedMapperTraps_CoverTaskValueTaskAndTheirGenericForms()
    {
        // Arrange
        var expected = new[] { "Task", "Task`1", "ValueTask", "ValueTask`1" };

        // Act
        var returnTypes = typeof(Result).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == nameof(Result.Try)
                        && OverloadTrapHelper.IsTrap(m, "The function returns a task")
                        && m.GetParameters()[1].ParameterType.GetGenericArguments()[0].IsGenericParameter)
            .Select(m => m.GetParameters()[0].ParameterType.GetGenericArguments()[0].Name)
            .Order()
            .ToList();

        // Assert
        returnTypes.ShouldBe(expected);
    }

    // An async lambda converts to both Func<Task…> and Func<ValueTask…>; the Task overloads win (no CS0121), and a function that
    // returns a ValueTask still binds to the ValueTask overloads, because the Task ones do not apply.
    [Fact]
    public void TryAsync_TaskOverloads_HavePriorityOne()
    {
        // Act
        var priorities = typeof(Result).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == nameof(Result.TryAsync) && OverloadTrapHelper.IsTaskFunction(m.GetParameters()[0].ParameterType))
            .Select(OverloadTrapHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(10);
        priorities.ShouldAllBe(p => p == 1);
    }

    #endregion
}
