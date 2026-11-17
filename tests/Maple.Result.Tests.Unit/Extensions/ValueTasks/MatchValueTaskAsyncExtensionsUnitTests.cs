// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Extensions.ValueTasks;
using Maple.Results.Tests.Unit.TestingInfrastructure.Helpers;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Extensions.ValueTasks;

public class MatchValueTaskAsyncExtensionsUnitTests
{
    private static readonly Error SourceError = Error.Failure(ErrorUri.None, "50f1c1fb-560e-4bb3-84da-034072df7ca2", "Error title 6");

    #region MatchAsync (ValueTask<Result>, Func<TOut>, Func<Error, TOut>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithFunctions_ReturnsValueOfSuccessFunction()
    {
        // Arrange
        var errorCalled = false;
        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => "success", _ =>
        {
            errorCalled = true;

            return "error";
        });

        // Assert
        result.ShouldBe("success");
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultValueTaskWithFunctions_ReturnsValueOfErrorFunctionCalledWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = ValueTask.FromResult(Result.FromError(SourceError));

        // Act
        var result = await sut.MatchAsync(() =>
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
    public async Task MatchAsync_ResultValueTaskReturningNull_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = ValueTask.FromResult<Result>(null!);

        // Act
        var exception = await Record.ExceptionAsync(async () => await sut.MatchAsync(() => 1, _ => 0));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task MatchAsync_ResultValueTaskWithFunctionReturningNullResult_ReturnsNull()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => (Result)null!, e => e);

        // Assert
        result.ShouldBeNull();
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask<TOut>>, Func<Error, ValueTask<TOut>>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithValueTaskFunctions_ReturnsValueOfSuccessFunction()
    {
        // Arrange
        var errorCalled = false;
        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(async () =>
        {
            await Task.Yield();

            return "success";
        }, _ =>
        {
            errorCalled = true;

            return ValueTask.FromResult("error");
        });

        // Assert
        result.ShouldBe("success");
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultValueTaskWithValueTaskFunctions_ReturnsValueOfErrorFunctionCalledWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = ValueTask.FromResult(Result.FromError(SourceError));

        // Act
        var result = await sut.MatchAsync(() =>
        {
            successCalled = true;

            return ValueTask.FromResult("success");
        }, async e =>
        {
            await Task.Yield();
            passedError = e;

            return "error";
        });

        // Assert
        result.ShouldBe("error");
        passedError.ShouldBeSameAs(SourceError);
        successCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_ResultValueTaskWithValueTaskFunctionReturningNullResult_ReturnsNull()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult<Result>(null!), e => ValueTask.FromResult<Result>(e));

        // Assert
        result.ShouldBeNull();
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithValueTaskActions_AwaitsOnlySuccessFunction()
    {
        // Arrange
        var successCompleted = false;
        var errorCalled = false;
        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(async () =>
        {
            await Task.Yield();
            successCompleted = true;
        }, _ =>
        {
            errorCalled = true;

            return ValueTask.CompletedTask;
        });

        // Assert
        successCompleted.ShouldBeTrue();
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultValueTaskWithValueTaskActions_AwaitsOnlyErrorFunctionWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = ValueTask.FromResult(Result.FromError(SourceError));

        // Act
        await sut.MatchAsync(() =>
        {
            successCalled = true;

            return ValueTask.CompletedTask;
        }, async e =>
        {
            await Task.Yield();
            passedError = e;
        });

        // Assert
        passedError.ShouldBeSameAs(SourceError);
        successCalled.ShouldBeFalse();
    }

    #endregion

    #region MatchAsync (ValueTask<Result<T>>, Func<T, TOut>, Func<Error, TOut>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTValueTaskWithFunctions_ReturnsValueOfSuccessFunctionCalledWithTheValue()
    {
        // Arrange
        var errorCalled = false;
        var sut = ValueTask.FromResult(Result.FromValue(5));

        // Act
        var result = await sut.MatchAsync(x => $"value {x}", _ =>
        {
            errorCalled = true;

            return "error";
        });

        // Assert
        result.ShouldBe("value 5");
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTValueTaskWithFunctions_ReturnsValueOfErrorFunctionCalledWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = ValueTask.FromResult(Result<int>.FromError(SourceError));

        // Act
        var result = await sut.MatchAsync(x =>
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
    public async Task MatchAsync_FailedResultOfTValueTaskWithErrorFunctionReturningTheError_ReturnsFailedResult()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(SourceError));

        // Act
        var result = await sut.MatchAsync(x => Result.FromValue(x.ToString()), e => e);

        // Assert
        result.Error.ShouldBeSameAs(SourceError);
    }

    [Fact]
    public async Task MatchAsync_ResultOfTValueTaskReturningNull_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = ValueTask.FromResult<Result<int>>(null!);

        // Act
        var exception = await Record.ExceptionAsync(async () => await sut.MatchAsync(x => x, _ => 0));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    #endregion

    #region MatchAsync (ValueTask<Result<T>>, Func<T, ValueTask<TOut>>, Func<Error, ValueTask<TOut>>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTValueTaskWithValueTaskFunctions_ReturnsValueOfSuccessFunctionCalledWithTheValue()
    {
        // Arrange
        var errorCalled = false;
        var sut = ValueTask.FromResult(Result.FromValue(5));

        // Act
        var result = await sut.MatchAsync(async x =>
        {
            await Task.Yield();

            return $"value {x}";
        }, _ =>
        {
            errorCalled = true;

            return ValueTask.FromResult("error");
        });

        // Assert
        result.ShouldBe("value 5");
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTValueTaskWithValueTaskFunctions_ReturnsValueOfErrorFunctionCalledWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = ValueTask.FromResult(Result<int>.FromError(SourceError));

        // Act
        var result = await sut.MatchAsync(x =>
        {
            successCalled = true;

            return ValueTask.FromResult($"value {x}");
        }, async e =>
        {
            await Task.Yield();
            passedError = e;

            return "error";
        });

        // Assert
        result.ShouldBe("error");
        passedError.ShouldBeSameAs(SourceError);
        successCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_ResultOfTValueTaskWithValueTaskFunctionThrowing_PropagatesTheException()
    {
        // Arrange
        var thrown = new InvalidOperationException("success function");
        var sut = ValueTask.FromResult(Result.FromValue(5));

        // Act
        var exception = await Record.ExceptionAsync(async () => await sut.MatchAsync<int, int>(_ => throw thrown, _ => ValueTask.FromResult(0)));

        // Assert
        exception.ShouldBeSameAs(thrown);
    }

    #endregion

    #region MatchAsync (ValueTask<Result<T>>, Func<T, ValueTask>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultOfTValueTaskWithValueTaskActions_AwaitsOnlySuccessFunctionWithTheValue()
    {
        // Arrange
        int? passedValue = null;
        var errorCalled = false;
        var sut = ValueTask.FromResult(Result.FromValue(5));

        // Act
        await sut.MatchAsync(async x =>
        {
            await Task.Yield();
            passedValue = x;
        }, _ =>
        {
            errorCalled = true;

            return ValueTask.CompletedTask;
        });

        // Assert
        passedValue.ShouldBe(5);
        errorCalled.ShouldBeFalse();
    }

    [Fact]
    public async Task MatchAsync_FailedResultOfTValueTaskWithValueTaskActions_AwaitsOnlyErrorFunctionWithTheError()
    {
        // Arrange
        var successCalled = false;
        Error? passedError = null;
        var sut = ValueTask.FromResult(Result<int>.FromError(SourceError));

        // Act
        await sut.MatchAsync(_ =>
        {
            successCalled = true;

            return ValueTask.CompletedTask;
        }, async e =>
        {
            await Task.Yield();
            passedError = e;
        });

        // Assert
        passedError.ShouldBeSameAs(SourceError);
        successCalled.ShouldBeFalse();
    }

    #endregion

    #region overload traps

    // See issue #255.

    [Fact]
    public void MatchAsync_TwoActionsOverloads_AreHiddenCompileErrorsThatNameSwitchAsync()
    {
        // Act
        var traps = typeof(MatchValueTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => OverloadTrapHelper.IsTrap(m, "MatchAsync takes two functions that return a value. Use SwitchAsync"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(2);
        traps.ShouldAllBe(m => m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never
                               && m.GetParameters().Skip(1).Take(2).All(p => p.ParameterType.Name.StartsWith("Action", StringComparison.Ordinal)));
    }

    [Fact]
    public void MatchAsync_TaskFunctionsOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = typeof(MatchValueTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => OverloadTrapHelper.IsTrap(m, "The delegates return a Task"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(4);
        traps.ShouldAllBe(m => m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never
                               && m.GetParameters().Skip(1).Take(2).All(p => OverloadTrapHelper.IsTaskFunction(p.ParameterType)));
    }

    [Fact]
    public void MatchAsync_AllMethods_AreOverloadsOnAValueTaskOrTheTwoKindsOfTraps()
    {
        // Act
        var methods = typeof(MatchValueTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).ToList();

        // Assert
        methods.Count.ShouldBe(6 + 2 + 4);
        methods.ShouldAllBe(m => m.Name == "MatchAsync" && m.GetParameters()[0].ParameterType.GetGenericTypeDefinition() == typeof(ValueTask<>));
    }

    [Fact]
    public void MatchAsync_OverloadsWithValueTaskFunctions_HavePriorityOne()
    {
        // Act
        var overloads = typeof(MatchValueTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is null && OverloadTrapHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .ToList();

        // Assert
        overloads.Count.ShouldBe(4);
        overloads.ShouldAllBe(m => OverloadTrapHelper.Priority(m) == 1);
    }

    #endregion
}
