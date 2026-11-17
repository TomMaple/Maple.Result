// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Extensions;
using Maple.Results.Tests.Unit.TestingInfrastructure.Helpers;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Extensions;

public class MatchExtensionsUnitTests
{
    private static readonly Error SourceError = Error.Failure(ErrorUri.None, "50f1c1fb-560e-4bb3-84da-034072df7ca2", "Error title 6");
    private static readonly Error OtherError = Error.Conflict(ErrorUri.None, "3d7a81ec-801d-45f4-b4b0-8d58e93b1d89", "Error title 7");

    #region Match (Result, Func<TOut>, Func<Error, TOut>)

    [Fact]
    public void Match_NullResult_ThrowsArgumentNullException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Match(() => 1, _ => 0));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("result");
    }

    [Fact]
    public void Match_NullSuccessFunction_ThrowsArgumentNullException()
    {
        // Arrange
        const Func<int>? SuccessFunction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(SuccessFunction!, _ => 0));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("ifSuccessFunction");
    }

    [Fact]
    public void Match_NullErrorFunction_ThrowsArgumentNullException()
    {
        // Arrange
        const Func<Error, int>? ErrorFunction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(() => 1, ErrorFunction!));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("ifErrorFunction");
    }

    [Fact]
    public void Match_SuccessfulResult_ReturnsValueOfSuccessFunction()
    {
        // Arrange
        var errorCalled = false;
        var sut = Result.Success();

        // Act
        var result = sut.Match(() => "success", _ =>
        {
            errorCalled = true;

            return "error";
        });

        // Assert
        result.ShouldBe("success");
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public void Match_FailedResult_ReturnsValueOfErrorFunctionCalledWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = Result.FromError(SourceError);

        // Act
        var result = sut.Match(() =>
        {
            successCalled = true;

            return "success";
        }, e =>
        {
            passedError = e;

            return "error";
        });

        // Assert
        result.ShouldBe("error");
        passedError.ShouldBeSameAs(SourceError);
        successCalled.ShouldBeFalse();
    }

    [Fact]
    public void Match_FunctionsReturningResult_ReturnsResultOfTheFunction()
    {
        // Arrange
        var successResult = Result.FromError(OtherError);
        var sut = Result.Success();

        // Act
        var result = sut.Match(() => successResult, e => e);

        // Assert
        result.ShouldBeSameAs(successResult);
    }

    [Fact]
    public void Match_FailedResultWithErrorFunctionReturningTheError_ReturnsFailedResult()
    {
        // Arrange
        var sut = Result.FromError(SourceError);

        // Act
        var result = sut.Match(() => Result.FromValue(1), e => e);

        // Assert
        result.Error.ShouldBeSameAs(SourceError);
    }

    [Fact]
    public void Match_FunctionReturningNullResult_ReturnsNull()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = sut.Match(() => (Result)null!, e => e);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void Match_FunctionsReturningTask_ReturnsTaskOfSuccessFunction()
    {
        // Arrange
        var sut = Result.Success();
        var taskSource = new TaskCompletionSource();

        // Act
        var result = sut.Match(() => taskSource.Task, _ => Task.CompletedTask);

        // Assert
        result.ShouldBeSameAs(taskSource.Task);
    }

    [Fact]
    public void Match_FailedResultWithFunctionsReturningTask_ReturnsTaskOfErrorFunction()
    {
        // Arrange
        var sut = Result.FromError(SourceError);
        var taskSource = new TaskCompletionSource();

        // Act
        var result = sut.Match(() => Task.CompletedTask, _ => taskSource.Task);

        // Assert
        result.ShouldBeSameAs(taskSource.Task);
    }

    [Fact]
    public void Match_SuccessFunctionThrows_PropagatesTheException()
    {
        // Arrange
        var thrown = new InvalidOperationException("success function");
        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match<int>(() => throw thrown, _ => 0));

        // Assert
        exception.ShouldBeSameAs(thrown);
    }

    #endregion

    #region Match (Result<T>, Func<T, TOut>, Func<Error, TOut>)

    [Fact]
    public void Match_NullResultOfT_ThrowsArgumentNullException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Match(x => x, _ => 0));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("result");
    }

    [Fact]
    public void Match_ResultOfTWithNullSuccessFunction_ThrowsArgumentNullException()
    {
        // Arrange
        const Func<int, int>? SuccessFunction = null;

        var sut = Result.FromValue(5);

        // Act
        var exception = Record.Exception(() => sut.Match(SuccessFunction!, _ => 0));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("ifSuccessFunction");
    }

    [Fact]
    public void Match_ResultOfTWithNullErrorFunction_ThrowsArgumentNullException()
    {
        // Arrange
        const Func<Error, int>? ErrorFunction = null;

        var sut = Result.FromValue(5);

        // Act
        var exception = Record.Exception(() => sut.Match(x => x, ErrorFunction!));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("ifErrorFunction");
    }

    [Fact]
    public void Match_SuccessfulResultOfT_ReturnsValueOfSuccessFunctionCalledWithTheValue()
    {
        // Arrange
        var errorCalled = false;
        var sut = Result.FromValue(5);

        // Act
        var result = sut.Match(x => $"value {x}", _ =>
        {
            errorCalled = true;

            return "error";
        });

        // Assert
        result.ShouldBe("value 5");
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public void Match_FailedResultOfT_ReturnsValueOfErrorFunctionCalledWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = Result<int>.FromError(SourceError);

        // Act
        var result = sut.Match(x =>
        {
            successCalled = true;

            return $"value {x}";
        }, e =>
        {
            passedError = e;

            return "error";
        });

        // Assert
        result.ShouldBe("error");
        passedError.ShouldBeSameAs(SourceError);
        successCalled.ShouldBeFalse();
    }

    [Fact]
    public void Match_ResultOfTWithFunctionsReturningResult_ReturnsResultOfTheFunction()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = sut.Match(x => Result.FromValue(x * 2), e => e);

        // Assert
        result.Value.ShouldBe(10);
    }

    [Fact]
    public void Match_FailedResultOfTWithErrorFunctionReturningTheError_ReturnsFailedResult()
    {
        // Arrange
        var sut = Result<int>.FromError(SourceError);

        // Act
        var result = sut.Match(x => Result.FromValue(x.ToString()), e => e);

        // Assert
        result.Error.ShouldBeSameAs(SourceError);
    }

    [Fact]
    public void Match_ResultOfTWithFunctionReturningNullResult_ReturnsNull()
    {
        // Arrange
        var sut = Result<int>.FromError(SourceError);

        // Act
        var result = sut.Match(x => Result.FromValue(x), _ => (Result<int>)null!);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void Match_ResultOfTWithExplicitTypeArguments_ReturnsTheWholeReturnType()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = sut.Match<int, Result<int>>(x => x + 1, e => e);

        // Assert
        result.Value.ShouldBe(6);
    }

    [Fact]
    public void Match_SuccessfulResultOfTWithFunctionsReturningTask_ReturnsTaskOfSuccessFunction()
    {
        // Arrange
        var sut = Result.FromValue(5);
        var taskSource = new TaskCompletionSource<int>();

        // Act
        var result = sut.Match(_ => taskSource.Task, _ => Task.FromResult(0));

        // Assert
        result.ShouldBeSameAs(taskSource.Task);
    }

    [Fact]
    public void Match_FailedResultOfTWithErrorFunctionThrowing_PropagatesTheException()
    {
        // Arrange
        var thrown = new InvalidOperationException("error function");
        var sut = Result<int>.FromError(SourceError);

        // Act
        var exception = Record.Exception(() => sut.Match(x => x, _ => throw thrown));

        // Assert
        exception.ShouldBeSameAs(thrown);
    }

    #endregion

    #region Match with two actions (overload trap)

    // See issue #255.

    [Fact]
    public void Match_TwoActionsOverloads_AreHiddenCompileErrorsThatNameSwitch()
    {
        // Act
        var traps = typeof(MatchExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is not null)
            .ToList();

        // Assert
        traps.Count.ShouldBe(2);
        traps.ShouldAllBe(m => m.Name == "Match"
                               && OverloadTrapHelper.IsTrap(m, "Match takes two functions that return a value. Use Switch")
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never
                               && m.GetParameters().Skip(1).All(p => p.ParameterType.Name.StartsWith("Action", StringComparison.Ordinal)));
    }

    [Fact]
    public void Match_Overloads_HaveDefaultPriority()
    {
        // Act
        var overloads = typeof(MatchExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == "Match")
            .ToList();

        // Assert
        overloads.Count.ShouldBe(4);
        overloads.ShouldAllBe(m => OverloadTrapHelper.Priority(m) == 0);
    }

    [Fact]
    public void Match_TwoActionsTrapCalledByReflection_ThrowsNotSupportedException()
    {
        // Arrange
        var trap = typeof(MatchExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(m => m.GetCustomAttribute<ObsoleteAttribute>() is not null && !m.IsGenericMethodDefinition);
        object?[] arguments = [Result.Success(), (Action)(() => { }), (Action<Error>)(_ => { })];

        // Act
        var exception = Record.Exception(() => trap.Invoke(null, arguments));

        // Assert
        exception.ShouldBeOfType<TargetInvocationException>().InnerException.ShouldBeOfType<NotSupportedException>();
    }

    #endregion
}
