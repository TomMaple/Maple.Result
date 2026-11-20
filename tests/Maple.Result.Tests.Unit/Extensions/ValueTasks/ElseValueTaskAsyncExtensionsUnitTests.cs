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
using System.Linq;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Extensions.ValueTasks;

public class ElseValueTaskAsyncExtensionsUnitTests
{
    #region IfErrorAsync (Result, Func<Error, ValueTask<Result>>)

    [Fact]
    public async Task IfErrorAsync_NoResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ElseAsync(_ => ValueTask.FromResult(Result.Success())).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result>>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, ValueTask<Result>>>();

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
        var result = await sut.ElseAsync(_ => ValueTask.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, ValueTask<Result>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

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
        var result = await sut.ElseAsync(_ => ValueTask.FromResult(asyncFuncResult));

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
        var result = await sut.ElseAsync(_ => ValueTask.FromResult(Result.FromError(asyncFuncError)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(asyncFuncError);
    }

    #endregion

    #region IfErrorAsync (Result<T>, Func<Error, ValueTask<IOperationResult>>)

    [Fact]
    public async Task IfErrorAsync_NoGenericResultWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ElseAsync(_ => ValueTask.FromResult(Result.FromValue(1))).AsTask());

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
        const Func<Error, ValueTask<Result<int>>>? Function = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result<int>>>? Function = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!).AsTask());

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
        var functionMock = new Mock<Func<Error, ValueTask<Result<int>>>>();

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
        var result = await sut.ElseAsync(_ => ValueTask.FromResult(Result.FromValue(2)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithGenericResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, ValueTask<Result<int>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

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
        var result = await sut.ElseAsync(_ => ValueTask.FromResult(asyncFuncResult));

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
        var result = await sut.ElseAsync(_ => ValueTask.FromResult(Result<int>.FromError(asyncFuncError)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(asyncFuncError);
    }

    #endregion

    #region IfErrorAsync (ValueTask<Result>, Func<Error, ValueTask<Result>>)

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultValueTaskWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result>>? Function = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultValueTaskWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result>>? Function = null;

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultValueTaskWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, ValueTask<Result>>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.ElseAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultValueTaskWithResultFunction_ReturnsOriginalResult()
    {
        // Arrange
        var successResult = Result.Success();
        var sut = ValueTask.FromResult(successResult);

        // Act
        var result = await sut.ElseAsync(_ => ValueTask.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successResult);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultValueTaskWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, ValueTask<Result>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.ElseAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultValueTaskWithSuccessResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var asyncFuncResult = Result.Success();

        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.ElseAsync(_ => ValueTask.FromResult(asyncFuncResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(asyncFuncResult);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultValueTaskWithErrorResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var asyncFuncError = GetReplacementError();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        var result = await sut.ElseAsync(_ => ValueTask.FromResult(Result.FromError(asyncFuncError)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBe(asyncFuncError);
    }

    #endregion

    #region IfErrorAsync (ValueTask<Result<T>>, Func<Error, ValueTask<IOperationResult>>)

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultValueTaskWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 24;
        const Func<Error, ValueTask<Result<int>>>? Function = null;

        var sut = ValueTask.FromResult(Result.FromValue(InitialValue));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultValueTaskWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result<int>>>? Function = null;

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.ElseAsync(Function!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultValueTaskWithGenericResultFunction_DoesNotCallFunction()
    {
        // Arrange
        const int InitialValue = 42;
        var functionMock = new Mock<Func<Error, ValueTask<Result<int>>>>();

        var sut = ValueTask.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.ElseAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultValueTaskWithGenericResultFunction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 42;

        var initialResult = Result.FromValue(InitialValue);
        var sut = ValueTask.FromResult(initialResult);

        // Act
        var result = await sut.ElseAsync(_ => ValueTask.FromResult(Result.FromValue(2)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(initialResult);
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultValueTaskWithGenericResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, ValueTask<Result<int>>>> { DefaultValueProvider = ResultDefaultValueProvider.Instance };

        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.ElseAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultValueTaskWithGenericResultFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        var asyncFuncResult = Result.FromValue(58);

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        var result = await sut.ElseAsync(_ => ValueTask.FromResult(asyncFuncResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(asyncFuncResult);
        result.Value.ShouldBe(58);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultValueTaskWithGenericResultFunction_ReturnsFunctionValueResultWithError()
    {
        // Arrange
        var asyncFuncError = GetReplacementError();

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        var result = await sut.ElseAsync(_ => ValueTask.FromResult(Result<int>.FromError(asyncFuncError)));

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

    #region ValueTask results with synchronous delegates

    // See issue #214.

    private static readonly Error SyncTestError = Error.Failure(ErrorUri.None, "Synchronous delegate test error");
    private static readonly Error SyncOtherError = Error.Conflict(ErrorUri.None, "Synchronous delegate other error");

    [Fact]
    public async Task ElseAsync_FailedResultOfTTaskWithSyncFunction_ReturnsFunctionResult()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(SyncTestError));

        // Act
        var result = await sut.ElseAsync(_ => Result.FromValue(-1));

        // Assert
        result.Value.ShouldBe(-1);
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultOfTTaskWithSyncFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(5));
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
        var sut = ValueTask.FromResult(Result.FromError(SyncTestError));

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
        var sut = new ValueTask<Result<int>>(source.Task);

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
    public void ElseAsync_NoOverloadTakesASynchronousAction()
    {
        // Act
        var overloads = OverloadReflectionHelper.ValueTaskPartMethods(typeof(ElseAsyncExtensions))
            .Where(m => m.Name == "ElseAsync" && m.GetParameters().Any(p => OverloadReflectionHelper.IsAction(p.ParameterType)))
            .ToList();

        // Assert
        overloads.ShouldBeEmpty();
    }

    // A function that returns a Task on a ValueTask of a result is awaited by an overload of its own, which returns a ValueTask (#226).
    [Fact]
    public void ElseAsync_FunctionReturningTaskOnValueTaskResult_IsAnOverloadWithPriorityOne()
    {
        // Act
        var overloads = OverloadReflectionHelper.ValueTaskPartMethods(typeof(ElseAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && m.GetParameters().Where(p => p.ParameterType.Name.StartsWith("Func`", StringComparison.Ordinal))
                            .All(p => OverloadReflectionHelper.IsTaskFunction(p.ParameterType))
                        && m.GetParameters().Any(p => OverloadReflectionHelper.IsTaskFunction(p.ParameterType)))
            .ToList();

        // Assert
        overloads.Count.ShouldBe(4);
        overloads.ShouldAllBe(m => m.Name == "ElseAsync" && OverloadReflectionHelper.Priority(m) == 1);
    }

    // An async lambda binds to the overloads whose delegates return a Task, on every receiver (#226): on a ValueTask of a
    // result, they have priority 1, and those whose delegates return a ValueTask priority 0.
    [Fact]
    public void ElseAsync_OverloadsWithTaskDelegatesOnValueTaskResults_HavePriorityOne()
    {
        // Act
        var priorities = OverloadReflectionHelper.ValueTaskPartMethods(typeof(ElseAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && OverloadReflectionHelper.IsTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadReflectionHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(4);
        priorities.ShouldAllBe(p => p == 1);
    }

    [Fact]
    public void ElseAsync_OverloadsWithValueTaskDelegatesOnValueTaskResults_HavePriorityZero()
    {
        // Act
        var priorities = OverloadReflectionHelper.ValueTaskPartMethods(typeof(ElseAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && OverloadReflectionHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadReflectionHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(4);
        priorities.ShouldAllBe(p => p == 0);
    }

    [Fact]
    public void ElseAsync_SynchronousOverloadsOnValueTaskResults_HavePriorityZero()
    {
        // Act
        var priorities = OverloadReflectionHelper.ValueTaskPartMethods(typeof(ElseAsyncExtensions))
            .Where(m => m.GetParameters()[0].ParameterType is { IsGenericType: true } receiver && receiver.GetGenericTypeDefinition() == typeof(ValueTask<>)
                        && !OverloadReflectionHelper.IsTaskFunction(m.GetParameters()[1].ParameterType)
                        && !OverloadReflectionHelper.IsValueTaskFunction(m.GetParameters()[1].ParameterType))
            .Select(OverloadReflectionHelper.Priority)
            .ToList();

        // Assert
        priorities.Count.ShouldBe(1);
        priorities.ShouldAllBe(p => p == 0);
    }

    #endregion

    #region A function returning a value or an Error in a value task

    // ElseAsync takes a function that returns the value or the Error of the replacement result, as Else does through the implicit
    // conversions. A successful result is returned unchanged; the null rules are checked over every overload by NullCheckUnitTests.
    // See issue #237.

    private static readonly Error FailedError = Error.Failure(ErrorUri.None, "The failed result");
    private static readonly Error ReplacementError = Error.Conflict(ErrorUri.None, "The replacement error");

    [Fact]
    public async Task ElseAsync_FailedResultOfTWithValueFunction_ReturnsSuccessfulResultWithTheValue()
    {
        // Arrange
        var sut = Result<int>.FromError(FailedError);

        // Act
        var result = await sut.ElseAsync(_ => new ValueTask<int>(-1));

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
        var result = await sut.ElseAsync(_ => { called = true; return new ValueTask<int>(-1); });

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
        var result = await sut.ElseAsync(_ => new ValueTask<string?>((string?)null));

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
        var result = await sut.ElseAsync(_ => new ValueTask<Error>(ReplacementError));

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
        var result = await sut.ElseAsync(_ => { called = true; return new ValueTask<Error>(ReplacementError); });

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
        var result = await sut.ElseAsync(_ => new ValueTask<Error>(ReplacementError));

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
        var result = await sut.ElseAsync(_ => { called = true; return new ValueTask<Error>(ReplacementError); });

        // Assert
        result.ShouldBeSameAs(sut);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ElseAsync_FailedResultOfTValueTaskWithValueFunction_ReturnsSuccessfulResultWithTheValue()
    {
        // Arrange
        var sut = new ValueTask<Result<int>>(Result<int>.FromError(FailedError));

        // Act
        var result = await sut.ElseAsync(_ => new ValueTask<int>(-1));

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(-1);
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultOfTValueTaskWithValueFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var original = Result<int>.FromValue(5);
        var sut = new ValueTask<Result<int>>(original);
        var called = false;

        // Act
        var result = await sut.ElseAsync(_ => { called = true; return new ValueTask<int>(-1); });

        // Assert
        result.ShouldBeSameAs(original);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ElseAsync_FailedResultOfTValueTaskWithValueFunctionReturningNull_ReturnsSuccessfulResultWithNullValue()
    {
        // Arrange
        var sut = new ValueTask<Result<string?>>(Result<string?>.FromError(FailedError));

        // Act
        var result = await sut.ElseAsync(_ => new ValueTask<string?>((string?)null));

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeNull();
    }

    [Fact]
    public async Task ElseAsync_FailedResultOfTValueTaskWithErrorFunction_ReturnsFailedResultWithTheError()
    {
        // Arrange
        var sut = new ValueTask<Result<int>>(Result<int>.FromError(FailedError));

        // Act
        var result = await sut.ElseAsync(e => new ValueTask<Error>(e == FailedError ? ReplacementError : e));

        // Assert
        result.Error.ShouldBe(ReplacementError);
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultOfTValueTaskWithErrorFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var original = Result<int>.FromValue(5);
        var sut = new ValueTask<Result<int>>(original);
        var called = false;

        // Act
        var result = await sut.ElseAsync(_ => { called = true; return new ValueTask<Error>(ReplacementError); });

        // Assert
        result.ShouldBeSameAs(original);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ElseAsync_FailedResultValueTaskWithErrorFunction_ReturnsFailedResultWithTheError()
    {
        // Arrange
        var sut = new ValueTask<Result>(Result.FromError(FailedError));

        // Act
        var result = await sut.ElseAsync(e => new ValueTask<Error>(e.ToBuilder().WithTitle("Replaced").Build()));

        // Assert
        result.Error.ShouldNotBeNull().Title.ShouldBe("Replaced");
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultValueTaskWithErrorFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var original = Result.Success();
        var sut = new ValueTask<Result>(original);
        var called = false;

        // Act
        var result = await sut.ElseAsync(_ => { called = true; return new ValueTask<Error>(ReplacementError); });

        // Assert
        result.ShouldBeSameAs(original);
        called.ShouldBeFalse();
    }

    #endregion

    #region ValueTask results with functions returning a Task

    // A function that returns the other kind of task is awaited, and the receiver decides the kind (#226).

    private static readonly Error OtherKindError = Error.Failure(ErrorUri.None, "Other kind of task test error");
    private static readonly Error OtherKindReplacement = Error.Conflict(ErrorUri.None, "Other kind of task replacement error");

    [Fact]
    public async Task ElseAsync_FailedResultOfTValueTaskWithTaskResultFunction_ReturnsTheReplacement()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(OtherKindError));

        // Act
        var result = await sut.ElseAsync(e => Task.FromResult(Result<int>.FromValue(9)));

        // Assert
        result.Value.ShouldBe(9);
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultOfTValueTaskWithTaskResultFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(5));
        var called = false;

        // Act
        var result = await sut.ElseAsync(e => { called = true; return Task.FromResult(Result<int>.FromValue(9)); });

        // Assert
        result.Value.ShouldBe(5);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ElseAsync_FailedResultOfTValueTaskWithTaskValueFunction_ReturnsTheReplacement()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(OtherKindError));

        // Act
        var result = await sut.ElseAsync(e => Task.FromResult(9));

        // Assert
        result.Value.ShouldBe(9);
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultOfTValueTaskWithTaskValueFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(5));
        var called = false;

        // Act
        var result = await sut.ElseAsync(e => { called = true; return Task.FromResult(9); });

        // Assert
        result.Value.ShouldBe(5);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ElseAsync_FailedResultOfTValueTaskWithTaskErrorFunction_ReturnsTheReplacement()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromError(OtherKindError));

        // Act
        var result = await sut.ElseAsync(e => Task.FromResult(OtherKindReplacement));

        // Assert
        result.Error.ShouldBeSameAs(OtherKindReplacement);
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultOfTValueTaskWithTaskErrorFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(5));
        var called = false;

        // Act
        var result = await sut.ElseAsync(e => { called = true; return Task.FromResult(OtherKindReplacement); });

        // Assert
        result.Value.ShouldBe(5);
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ElseAsync_FailedResultValueTaskWithTaskErrorFunction_ReturnsTheReplacement()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromError(OtherKindError));

        // Act
        var result = await sut.ElseAsync(e => Task.FromResult(OtherKindReplacement));

        // Assert
        result.Error.ShouldBeSameAs(OtherKindReplacement);
    }

    [Fact]
    public async Task ElseAsync_SuccessfulResultValueTaskWithTaskErrorFunction_ReturnsOriginalResultAndDoesNotCallFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.Success());
        var called = false;

        // Act
        var result = await sut.ElseAsync(e => { called = true; return Task.FromResult(OtherKindReplacement); });

        // Assert
        result.IsSuccess.ShouldBeTrue();
        called.ShouldBeFalse();
    }

    [Fact]
    public async Task ElseAsync_TaskFunctionOnIncompleteValueTask_RunsAfterTheResultIsAvailable()
    {
        // Arrange
        var source = new TaskCompletionSource<Result<int>>();
        var called = false;

        // Act
        var task = Run();
        var calledBeforeCompletion = called;
        source.SetResult(Result<int>.FromError(OtherKindError));
        await task;

        // Assert
        calledBeforeCompletion.ShouldBeFalse();
        called.ShouldBeTrue();

        async Task Run()
        {
            var sut = new ValueTask<Result<int>>(source.Task);
            _ = await sut.ElseAsync(e => { called = true; return Task.FromResult(0); });
        }
    }

    #endregion
}
