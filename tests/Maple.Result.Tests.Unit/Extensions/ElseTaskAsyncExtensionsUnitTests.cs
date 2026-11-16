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
using Maple.Results.Tests.Unit.TestingInfrastructure.Fakes;
using Maple.Results.Tests.Unit.TestingInfrastructure.Helpers;
using Moq;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Extensions;

public class ElseTaskAsyncExtensionsUnitTests
{
    #region IfErrorAsync (Result, Func<Error, Task<Result>>)

    [Fact]
    public async Task IfErrorAsync_NoResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ElseAsync(_ => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<Result>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<Result>>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Task<Result>>>();

        var sut = Result.Success();

        // Act
        await sut.ElseAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultWithResultFunction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Task<Result>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = GetErrorResult();

        // Act
        await sut.ElseAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithSuccessResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var asyncFuncResult = Result.Success();

        var sut = GetErrorResult();

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(asyncFuncResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(asyncFuncResult);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithErrorResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var asyncFuncError = GetReplacementError();

        var sut = GetErrorResult();

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(Result.FromError(asyncFuncError)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(asyncFuncError);
    }

    #endregion

    #region IfErrorAsync (Result<T>, Func<Error, Task<IOperationResult>>)

    [Fact]
    public async Task IfErrorAsync_NoGenericResultWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ElseAsync(_ => Task.FromResult(Result.FromValue(1))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 24;
        const Func<Error, Task<Result<int>>>? Function = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<Result<int>>>? Function = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultWithGenericResultFunction_DoesNotCallFunction()
    {
        // Arrange
        const int InitialValue = 42;
        var functionMock = new Mock<Func<Error, Task<Result<int>>>>();

        Result<int> sut = InitialValue;

        // Act
        await sut.ElseAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultWithGenericResultFunction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 11;

        Result<int> sut = InitialValue;

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(Result.FromValue(2)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithGenericResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Task<Result<int>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var sut = GetErrorResult<int>();

        // Act
        await sut.ElseAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithGenericResultFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        var asyncFuncResult = Result.FromValue(58);

        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(asyncFuncResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(asyncFuncResult);
        result.Value.ShouldBe(58);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithGenericResultFunction_ReturnsFunctionValueResultWithError()
    {
        // Arrange
        var asyncFuncError = GetReplacementError();

        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(Result<int>.FromError(asyncFuncError)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(asyncFuncError);
    }

    #endregion

    #region IfErrorAsync (Task<Result>, Func<Error, Task<Result>>)

    [Fact]
    public async Task IfErrorAsync_NoResultTaskWithResultFunction_ThrowsException()
    {
        // Arrange
        const Task<Result>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ElseAsync(_ => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultTaskWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<Result>>? Function = null;

        var sut = Task.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultTaskWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<Result>>? Function = null;

        var sut = Task.FromResult(GetErrorResult());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultTaskWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Task<Result>>>();

        var sut = Task.FromResult(Result.Success());

        // Act
        await sut.ElseAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultTaskWithResultFunction_ReturnsOriginalResult()
    {
        // Arrange
        var successResult = Result.Success();
        var sut = Task.FromResult(successResult);

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successResult);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultTaskWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Task<Result>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        await sut.ElseAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultTaskWithSuccessResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var asyncFuncResult = Result.Success();

        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(asyncFuncResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(asyncFuncResult);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultTaskWithErrorResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var asyncFuncError = GetReplacementError();

        var sut = Task.FromResult(GetErrorResult());

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(Result.FromError(asyncFuncError)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(asyncFuncError);
    }

    #endregion

    #region IfErrorAsync (Task<Result<T>>, Func<Error, Task<IOperationResult>>)

    [Fact]
    public async Task IfErrorAsync_NoGenericResultTaskWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Task<Result<int>>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ElseAsync(_ => Task.FromResult(Result.FromValue(1))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultTaskWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 24;
        const Func<Error, Task<Result<int>>>? Function = null;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultTaskWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<Result<int>>>? Function = null;

        var sut = Task.FromResult(GetErrorResult<int>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultTaskWithGenericResultFunction_DoesNotCallFunction()
    {
        // Arrange
        const int InitialValue = 42;
        var functionMock = new Mock<Func<Error, Task<Result<int>>>>();

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.ElseAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultTaskWithGenericResultFunction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 42;

        var initialResult = Result.FromValue(InitialValue);
        var sut = Task.FromResult(initialResult);

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(Result.FromValue(2)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(initialResult);
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultTaskWithGenericResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Task<Result<int>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var errorResult = GetErrorResult<int>();
        var sut = Task.FromResult(errorResult);

        // Act
        await sut.ElseAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultTaskWithGenericResultFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        var asyncFuncResult = Result.FromValue(58);

        var sut = Task.FromResult(GetErrorResult<int>());

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(asyncFuncResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(asyncFuncResult);
        result.Value.ShouldBe(58);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultTaskWithGenericResultFunction_ReturnsFunctionValueResultWithError()
    {
        // Arrange
        var asyncFuncError = GetReplacementError();

        var sut = Task.FromResult(GetErrorResult<int>());

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(Result<int>.FromError(asyncFuncError)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(asyncFuncError);
    }

    #endregion

    #region helper methods

    private static Result GetErrorResult()
    {
        return Error.Failure(ErrorUri.None, "4b3612e7-d22a-45f2-9a5c-4a8c14d39141", "Error title 1");
    }

    private static Result<T> GetErrorResult<T>()
    {
        return Error.Failure(ErrorUri.None, "ff7711a7-c224-4280-816c-1722864177cf", "Error title 2");
    }

    private static Error GetReplacementError()
    {
        return Error.Failure(ErrorUri.None, "53e1e02c-3e21-47c9-9e1a-93ba22f03ba8", "Replacement error");
    }

    #endregion

    #region ElseAsync with a function returning a task without a value (overload trap, issue #197)

    // A function that returns nothing is a side effect: the transforming method reports it as a compile error (CS0619) that
    // names the side-effect method (#197). The bindings are checked with a consumer project.

    [Fact]
    public void ElseAsync_FunctionReturningTaskWithoutValueOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = typeof(ElseTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => OverloadTrapHelper.IsTrap(m, "The function returns a task without a value. ElseAsync needs"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(2);
        traps.ShouldAllBe(m => m.Name == "ElseAsync"
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    #endregion

    #region Task results with synchronous delegates (issue #214)

    private static readonly Error SyncTestError = Error.Failure(ErrorUri.None, "Synchronous delegate test error");
    private static readonly Error SyncOtherError = Error.Conflict(ErrorUri.None, "Synchronous delegate other error");

    [Fact]
    public async Task ElseAsync_FailedResultOfTTaskWithSyncFunction_ReturnsFunctionResult()
    {
        // Arrange
        var sut = Task.FromResult(Result<int>.FromError(SyncTestError));

        // Act
        var result = await sut.ElseAsync(_ => Result.FromValue(-1));

        // Assert
        result.Value.ShouldBe(-1);
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultOfTTaskWithSyncFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromValue(5));
        var called = false;

        // Act
        var result = await sut.ElseAsync(_ => { called = true; return Result.FromValue(-1); });

        // Assert
        result.Value.ShouldBe(5);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ElseAsync_FailedResultTaskWithSyncFunction_ReturnsFunctionResult()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromError(SyncTestError));

        // Act
        var result = await sut.ElseAsync(_ => Result.Success());

        // Assert
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task ElseAsync_IncompleteTaskWithSynchronousDelegate_RunsDelegateAfterCompletion()
    {
        // Arrange
        var source = new TaskCompletionSource<Result<int>>();
        var called = false;
        var sut = source.Task;

        // Act
        var task = sut.ElseAsync(_ => { called = true; return Result.FromValue(-1); });
        var calledBeforeCompletion = called;
        source.SetResult(Result<int>.FromError(SyncTestError));
        await task;

        // Assert
        calledBeforeCompletion.ShouldBeFalse();
        called.ShouldBeTrue();
    }

    [Fact]
    public void ElseAsync_SyncActionOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = typeof(ElseTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => OverloadTrapHelper.IsTrap(m, "The action returns nothing. ElseAsync needs"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(1);
        traps.ShouldAllBe(m => m.Name == "ElseAsync"
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    [Fact]
    public void ElseAsync_FunctionReturningValueTaskOverloads_AreHiddenCompileErrors()
    {
        // Act
        var traps = typeof(ElseTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => OverloadTrapHelper.IsTrap(m, "The function returns a ValueTask, which the Task-based ElseAsync does not await"))
            .ToList();

        // Assert
        traps.Count.ShouldBe(2);
        traps.ShouldAllBe(m => m.Name == "ElseAsync"
                               && m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                               && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    [Fact]
    public void ElseAsync_OverloadsWithTaskDelegatesOnTaskResults_HavePriorityOne()
    {
        // Act
        var priorities = typeof(ElseTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(Task<>)
                        && OverloadTrapHelper.IsTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadTrapHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(5);
        priorities.ShouldAllBe(p => p == 1);
    }

    [Fact]
    public void ElseAsync_SynchronousOverloadsOnTaskResults_HavePriorityZero()
    {
        // Act
        var priorities = typeof(ElseTaskAsyncExtensions).GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is null
                        && m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(Task<>)
                        && !OverloadTrapHelper.IsTaskFunction(m.GetParameters()[1].ParameterType)
                        && !OverloadTrapHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadTrapHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(1);
        priorities.ShouldAllBe(p => p == 0);
    }

    #endregion

    #region A function returning a value or an Error in a task (issue #237)

    // ElseAsync takes a function that returns the value or the Error of the replacement result, as Else does through the implicit
    // conversions. A successful result is returned unchanged; the null rules are checked over every overload by NullCheckUnitTests.

    private static readonly Error FailedError = Error.Failure(ErrorUri.None, "The failed result");
    private static readonly Error ReplacementError = Error.Conflict(ErrorUri.None, "The replacement error");

    [Fact]
    public async Task ElseAsync_FailedResultOfTWithValueFunction_ReturnsSuccessfulResultWithTheValue()
    {
        // Arrange
        var sut = Result<int>.FromError(FailedError);

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(-1));

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(-1);
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultOfTWithValueFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var sut = Result<int>.FromValue(5);
        var called = false;

        // Act
        var result = await sut.ElseAsync(_ => { called = true; return Task.FromResult(-1); });

        // Assert
        result.ShouldBeSameAs(sut);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ElseAsync_FailedResultOfTWithValueFunctionReturningNull_ReturnsSuccessfulResultWithNullValue()
    {
        // Arrange
        var sut = Result<string?>.FromError(FailedError);

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult<string?>(null));

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeNull();
    }

    [Fact]
    public async Task ElseAsync_FailedResultOfTWithErrorFunction_ReturnsFailedResultWithTheError()
    {
        // Arrange
        var sut = Result<int>.FromError(FailedError);

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(ReplacementError));

        // Assert
        result.Error.ShouldBe(ReplacementError);
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultOfTWithErrorFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var sut = Result<int>.FromValue(5);
        var called = false;

        // Act
        var result = await sut.ElseAsync(_ => { called = true; return Task.FromResult(ReplacementError); });

        // Assert
        result.ShouldBeSameAs(sut);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ElseAsync_FailedResultWithErrorFunction_ReturnsFailedResultWithTheError()
    {
        // Arrange
        var sut = Result.FromError(FailedError);

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(ReplacementError));

        // Assert
        result.Error.ShouldBe(ReplacementError);
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultWithErrorFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var sut = Result.Success();
        var called = false;

        // Act
        var result = await sut.ElseAsync(_ => { called = true; return Task.FromResult(ReplacementError); });

        // Assert
        result.ShouldBeSameAs(sut);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ElseAsync_FailedResultOfTTaskWithValueFunction_ReturnsSuccessfulResultWithTheValue()
    {
        // Arrange
        var sut = Task.FromResult(Result<int>.FromError(FailedError));

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult(-1));

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(-1);
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultOfTTaskWithValueFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var original = Result<int>.FromValue(5);
        var sut = Task.FromResult(original);
        var called = false;

        // Act
        var result = await sut.ElseAsync(_ => { called = true; return Task.FromResult(-1); });

        // Assert
        result.ShouldBeSameAs(original);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ElseAsync_FailedResultOfTTaskWithValueFunctionReturningNull_ReturnsSuccessfulResultWithNullValue()
    {
        // Arrange
        var sut = Task.FromResult(Result<string?>.FromError(FailedError));

        // Act
        var result = await sut.ElseAsync(_ => Task.FromResult<string?>(null));

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeNull();
    }

    [Fact]
    public async Task ElseAsync_FailedResultOfTTaskWithErrorFunction_ReturnsFailedResultWithTheError()
    {
        // Arrange
        var sut = Task.FromResult(Result<int>.FromError(FailedError));

        // Act
        var result = await sut.ElseAsync(e => Task.FromResult(e == FailedError ? ReplacementError : e));

        // Assert
        result.Error.ShouldBe(ReplacementError);
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultOfTTaskWithErrorFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var original = Result<int>.FromValue(5);
        var sut = Task.FromResult(original);
        var called = false;

        // Act
        var result = await sut.ElseAsync(_ => { called = true; return Task.FromResult(ReplacementError); });

        // Assert
        result.ShouldBeSameAs(original);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ElseAsync_FailedResultTaskWithErrorFunction_ReturnsFailedResultWithTheError()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromError(FailedError));

        // Act
        var result = await sut.ElseAsync(e => Task.FromResult(e.ToBuilder().WithTitle("Replaced").Build()));

        // Assert
        result.Error.ShouldNotBeNull().Title.ShouldBe("Replaced");
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultTaskWithErrorFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var original = Result.Success();
        var sut = Task.FromResult(original);
        var called = false;

        // Act
        var result = await sut.ElseAsync(_ => { called = true; return Task.FromResult(ReplacementError); });

        // Assert
        result.ShouldBeSameAs(original);
        called.ShouldBeFalse();
    }

    #endregion
}
