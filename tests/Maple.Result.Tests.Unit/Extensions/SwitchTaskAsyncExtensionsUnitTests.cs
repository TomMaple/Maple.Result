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

public class SwitchTaskAsyncExtensionsUnitTests
{
    private static readonly Error SourceError = Error.Failure(ErrorUri.None, "8c0f5d0e-7d4b-4f0e-9d0a-2d3f8e6c1b7a", "Switch error");

    #region SwitchAsync (Task<Result>, Action, Action<Error>)

    [Fact]
    public void SwitchAsync_NullResultTask_ThrowsArgumentNullExceptionFromTheCall()
    {
        // Arrange
        const Task<Result>? Sut = null;

        // Act
        var exception = Record.Exception(() => { _ = Sut!.SwitchAsync(() => { }, _ => { }); });

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("resultTask");
    }

    [Fact]
    public async Task SwitchAsync_SuccessfulResultTask_RunsOnlySuccessAction()
    {
        // Arrange
        var successCalls = 0;
        var errorCalls = 0;
        var sut = Task.FromResult(Result.Success());

        // Act
        await sut.SwitchAsync(() => successCalls++, _ => errorCalls++);

        // Assert
        successCalls.ShouldBe(1);
        errorCalls.ShouldBe(0);
    }

    [Fact]
    public async Task SwitchAsync_FailedResultTask_RunsOnlyErrorActionWithTheError()
    {
        // Arrange
        var successCalls = 0;
        Error? passedError = null;
        var sut = Task.FromResult(Result.FromError(SourceError));

        // Act
        await sut.SwitchAsync(() => successCalls++, e => passedError = e);

        // Assert
        successCalls.ShouldBe(0);
        passedError.ShouldBeSameAs(SourceError);
    }

    [Fact]
    public async Task SwitchAsync_ActionsRunAfterTheResultTaskCompletes()
    {
        // Arrange
        var source = new TaskCompletionSource<Result>(TaskCreationOptions.RunContinuationsAsynchronously);
        var successCalls = 0;

        // Act
        var switchTask = source.Task.SwitchAsync(() => successCalls++, _ => { });
        var callsBeforeCompletion = successCalls;
        source.SetResult(Result.Success());
        await switchTask;

        // Assert
        callsBeforeCompletion.ShouldBe(0);
        successCalls.ShouldBe(1);
    }

    [Fact]
    public async Task SwitchAsync_ResultTaskReturningNull_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = Task.FromResult<Result>(null!);

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SwitchAsync(() => { }, _ => { }));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task SwitchAsync_SuccessActionThrows_PropagatesTheException()
    {
        // Arrange
        var thrown = new InvalidOperationException("success action");
        var sut = Task.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SwitchAsync(() => throw thrown, _ => { }));

        // Assert
        exception.ShouldBeSameAs(thrown);
    }

    #endregion

    #region SwitchAsync (Task<Result<T>>, Action<T>, Action<Error>)

    [Fact]
    public void SwitchAsync_ResultOfTTaskWithNullSuccessAction_ThrowsArgumentNullExceptionFromTheCall()
    {
        // Arrange
        const Action<int>? SuccessAction = null;

        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        var exception = Record.Exception(() => { _ = sut.SwitchAsync(SuccessAction!, _ => { }); });

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe("ifSuccessAction");
    }

    [Fact]
    public async Task SwitchAsync_SuccessfulResultOfTTask_RunsOnlySuccessActionWithTheValue()
    {
        // Arrange
        int? passedValue = null;
        var errorCalls = 0;
        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        await sut.SwitchAsync(x => passedValue = x, _ => errorCalls++);

        // Assert
        passedValue.ShouldBe(5);
        errorCalls.ShouldBe(0);
    }

    [Fact]
    public async Task SwitchAsync_FailedResultOfTTask_RunsOnlyErrorActionWithTheError()
    {
        // Arrange
        var successCalls = 0;
        Error? passedError = null;
        var sut = Task.FromResult(Result<int>.FromError(SourceError));

        // Act
        await sut.SwitchAsync(_ => successCalls++, e => passedError = e);

        // Assert
        successCalls.ShouldBe(0);
        passedError.ShouldBeSameAs(SourceError);
    }

    [Fact]
    public async Task SwitchAsync_ResultOfTTaskReturningNull_ThrowsInvalidOperationException()
    {
        // Arrange
        var sut = Task.FromResult<Result<int>>(null!);

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SwitchAsync(_ => { }, _ => { }));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task SwitchAsync_FailedResultOfTTaskWithErrorActionThrowing_PropagatesTheException()
    {
        // Arrange
        var thrown = new InvalidOperationException("error action");
        var sut = Task.FromResult(Result<int>.FromError(SourceError));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SwitchAsync(_ => { }, _ => throw thrown));

        // Assert
        exception.ShouldBeSameAs(thrown);
    }

    #endregion

    #region SwitchAsync with an action returning a task (overload trap, issue #255)

    [Fact]
    public void SwitchAsync_ActionReturningTaskOverloads_AreHiddenCompileErrorsThatNameMatchAsync()
    {
        // Act
        var traps = typeof(SwitchTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is not null)
            .ToList();

        // Assert
        traps.Count.ShouldBe(14);
        traps.ShouldAllBe(m => m.Name == "SwitchAsync"
                               && OverloadTrapHelper.IsTrap(m, "An action returns a task, which SwitchAsync does not await. Use MatchAsync")
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    [Fact]
    public void SwitchAsync_TrapsConstrainedToTheTaskType_HavePriorityOne()
    {
        // Act
        var traps = typeof(SwitchTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is not null
                        && m.GetGenericArguments().Any(a => a.GetGenericParameterConstraints().Length > 0))
            .ToList();

        // Assert
        traps.Count.ShouldBe(10);
        traps.ShouldAllBe(m => OverloadTrapHelper.Priority(m) == 1);
    }

    [Fact]
    public void SwitchAsync_Overloads_ReturnATaskWithDefaultPriority()
    {
        // Act
        var overloads = typeof(SwitchTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is null)
            .ToList();

        // Assert
        overloads.Count.ShouldBe(2);
        overloads.ShouldAllBe(m => m.Name == "SwitchAsync" && m.ReturnType == typeof(Task) && OverloadTrapHelper.Priority(m) == 0);
    }

    #endregion
}
