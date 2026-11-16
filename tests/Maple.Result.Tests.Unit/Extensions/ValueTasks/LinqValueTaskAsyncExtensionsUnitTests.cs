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
using Moq;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Extensions.ValueTasks;

public class LinqValueTaskAsyncExtensionsUnitTests
{
    #region SelectMany(ValueTask<Result<T>>, Func<T, ValueTask<Result<TMiddle>>>, Func<T, TMiddle, TNext>)

    [Fact]
    public async Task SelectMany_SuccessfulResultWithNoCollectionSelectorAndResultSelector_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask<Result<string>>>? CollectionSelector = null;

        var sut = ValueTask.FromResult(Result.FromValue(5));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SelectMany(CollectionSelector!, (x, _) => (double)x).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task SelectMany_NoResultValueWithSelectorFunctions_ThrowsException()
    {
        // Arrange
        var sut = ValueTask.FromResult<Result<int>>(null!);

        // Act
        var exception = await Record.ExceptionAsync(() => sut
            .SelectMany(x => ValueTask.FromResult(Result.FromValue(x.ToString())), (x, _) => (double)x)
            .AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidOperationException>();
        exception.Message.ShouldStartWith("The asynchronous operation represented by ‘result’ returned null.");
    }

    [Fact]
    public async Task SelectMany_ErrorResultWithNoCollectionSelectorAndResultSelector_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask<Result<string>>>? CollectionSelector = null;

        var sut = GetErrorResultValueTask<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SelectMany(CollectionSelector!, (x, _) => (double)x).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task SelectMany_SuccessfulResultWithNoResultSelectorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, string, double>? ResultSelectorFunction = null;

        var sut = ValueTask.FromResult(Result.FromValue(5));

        // Act
        var exception = await Record.ExceptionAsync(() => sut
            .SelectMany(x => ValueTask.FromResult(Result.FromValue(x.ToString())), ResultSelectorFunction!)
            .AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task SelectMany_ErrorResultWithNoResultSelectorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, string, double>? ResultSelectorFunction = null;

        var sut = GetErrorResultValueTask<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut
            .SelectMany(x => ValueTask.FromResult(Result.FromValue(x.ToString())), ResultSelectorFunction!)
            .AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task SelectMany_SuccessfulResultWithSelectorFunctions_CallsBothFunctions()
    {
        // Arrange
        const int InitialValue = 4839;
        const string ExpectedMiddleValue = "4839";
        const double ExpectedFinalValue = 4839;

        var collectionSelectorMock = new Mock<Func<int, ValueTask<Result<string>>>>();
        collectionSelectorMock
            .Setup(x => x.Invoke(InitialValue))
            .ReturnsAsync(Result.FromValue(ExpectedMiddleValue));

        var resultSelectorMock = new Mock<Func<int, string, double>>();
        resultSelectorMock
            .Setup(x => x.Invoke(InitialValue, ExpectedMiddleValue))
            .Returns(ExpectedFinalValue);

        var sut = ValueTask.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.SelectMany(collectionSelectorMock.Object, resultSelectorMock.Object);

        // Assert
        collectionSelectorMock.Verify(x => x.Invoke(InitialValue), Times.Once);
        resultSelectorMock.Verify(x => x.Invoke(InitialValue, ExpectedMiddleValue), Times.Once);
    }

    [Fact]
    public async Task SelectMany_SuccessfulResultWithSelectorFunctions_ReturnsValue()
    {
        // Arrange
        const int InitialValue = 4839;
        const double ExpectedFinalValue = 4839;

        var sut = ValueTask.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.SelectMany(x => ValueTask.FromResult(Result.FromValue(x.ToString())), (x, _) => (double)x);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<double>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(ExpectedFinalValue);
    }

    [Fact]
    public async Task SelectMany_ErrorResultWithSelectorFunctions_DoesNotCallFunctions()
    {
        // Arrange
        var collectionSelectorMock = new Mock<Func<int, ValueTask<Result<string>>>>();
        var resultSelectorMock = new Mock<Func<int, string, double>>();

        var sut = GetErrorResultValueTask<int>();

        // Act
        await sut.SelectMany(collectionSelectorMock.Object, resultSelectorMock.Object);

        // Assert
        collectionSelectorMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
        resultSelectorMock.Verify(x => x.Invoke(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task SelectMany_ErrorResultWithSelectorFunctions_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.SelectMany(x => ValueTask.FromResult(Result.FromValue(x.ToString())), (x, _) => (double)x);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<double>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region Select(ValueTask<Result<T>>, Func<T, TNext>)

    [Fact]
    public async Task Select_NoResultValueWithSelector_ThrowsException()
    {
        // Arrange
        var sut = ValueTask.FromResult<Result<int>>(null!);

        // Act
        var exception = await Record.ExceptionAsync(() => sut.Select(x => x.ToString()).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidOperationException>();
        exception.Message.ShouldStartWith("The asynchronous operation represented by ‘result’ returned null.");
    }

    [Fact]
    public async Task Select_SuccessfulResultWithNoSelector_ThrowsException()
    {
        // Arrange
        const Func<int, string>? Selector = null;

        var sut = ValueTask.FromResult(Result.FromValue(5));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.Select(Selector!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task Select_ErrorResultWithNoSelector_ThrowsException()
    {
        // Arrange
        const Func<int, string>? Selector = null;

        var sut = GetErrorResultValueTask<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.Select(Selector!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task Select_SuccessfulResultWithSelector_CallsSelector()
    {
        // Arrange
        const int InitialValue = 4839;
        const string ExpectedValue = "4839";

        var selectorMock = new Mock<Func<int, string>>();
        selectorMock
            .Setup(x => x.Invoke(InitialValue))
            .Returns(ExpectedValue);

        var sut = ValueTask.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.Select(selectorMock.Object);

        // Assert
        selectorMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task Select_SuccessfulResultWithSelector_ReturnsValue()
    {
        // Arrange
        const int InitialValue = 4839;
        const string ExpectedValue = "4839";

        var sut = ValueTask.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.Select(x => x.ToString());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task Select_ErrorResultWithSelector_DoesNotCallSelector()
    {
        // Arrange
        var selectorMock = new Mock<Func<int, string>>();

        var sut = GetErrorResultValueTask<int>();

        // Act
        await sut.Select(selectorMock.Object);

        // Assert
        selectorMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Select_ErrorResultWithSelector_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.Select(x => x.ToString());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region LINQ query syntax

    // ReSharper disable RedundantAssignment

    [Fact]
    public async Task LinqQueryAsyncSyntax_SingleClauseSuccessfulResult_ReturnsProjectedValueResult()
    {
        // Arrange
        const string ExpectedValue = "90";

        var sut = ValueTask.FromResult(Result.FromValue(90));

        // Act
        var result = await (
            from initialValue in sut
            select initialValue.ToString());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_SingleClauseErrorResult_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await (
            from initialValue in sut
            select initialValue.ToString());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_SuccessfulResult_CallsFunctions()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, ValueTask<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(Result.FromValue(100));

        var function2Mock = new Mock<Func<int, ValueTask<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(100))
            .ReturnsAsync(Result.FromValue("100"));

        // Act
        await (
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result);

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
        function2Mock.Verify(x => x.Invoke(100), Times.Once);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_SuccessfulResult_ReturnsValueResult()
    {
        // Arrange
        const string ExpectedValue = "100";

        var sut = ValueTask.FromResult(Result.FromValue(90));

        static ValueTask<Result<int>> Function1(int value) => ValueTask.FromResult(Result.FromValue(value + 10));
        static ValueTask<Result<string>> Function2(int value) => ValueTask.FromResult(Result.FromValue(value.ToString()));

        // Act
        var result = await (
            from initialValue in sut
            from function1Result in Function1(initialValue)
            from function2Result in Function2(function1Result)
            select function2Result);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_FirstErrorFunction_CallsFirstFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, ValueTask<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, ValueTask<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync("100");

        // Act
        await (
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result);

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_FirstErrorFunction_DoesNotCallSecondFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, ValueTask<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, ValueTask<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync("100");

        // Act
        await (
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result);

        // Assert
        function2Mock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_FirstErrorFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        var function1Mock = new Mock<Func<int, ValueTask<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, ValueTask<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync("100");

        // Act
        var result = await (
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_SecondErrorFunction_CallsFirstFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, ValueTask<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(100);

        var function2Mock = new Mock<Func<int, ValueTask<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync(GetErrorResult<string>());

        // Act
        await (
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result);

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_SecondErrorFunction_CallsSecondFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, ValueTask<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(100);

        var function2Mock = new Mock<Func<int, ValueTask<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync(GetErrorResult<string>());

        // Act
        await (
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result);

        // Assert
        function2Mock.Verify(x => x.Invoke(100), Times.Once);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_SecondErrorFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        var function1Mock = new Mock<Func<int, ValueTask<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(100);

        var function2Mock = new Mock<Func<int, ValueTask<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync(GetErrorResult<string>());

        // Act
        var result = await (
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    // ReSharper restore RedundantAssignment

    #endregion

    #region helper methods

    private static Result<T> GetErrorResult<T>()
    {
        return Error.Failure(ErrorUri.None, "6b648e52-bde5-4e23-a233-c5dd4c4d2e73", "Error title 4");
    }

    private static ValueTask<Result<T>> GetErrorResultValueTask<T>()
    {
        Result<T> result = Error.Failure(ErrorUri.None, "7e5de15a-8851-4d75-8308-ec18351d7b70", "Error title 4");
        return ValueTask.FromResult(result);
    }

    #endregion

    #region Mixed synchronous and asynchronous from clauses

    // See issue #119.

    private static readonly Error MixedError = Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Mixed"), "Mixed title");

    private static ValueTask<Result<int>> GetAsync(int value) => ValueTask.FromResult(Result<int>.FromValue(value));

    private static ValueTask<Result<int>> GetErrorAsync() => ValueTask.FromResult(Result<int>.FromError(MixedError));

    private static Result<int> Get(int value) => Result<int>.FromValue(value);

    [Fact]
    public async Task LinqQuerySyntax_AsyncThenSyncSuccessfulResults_ReturnsCombinedValue()
    {
        // Act
        var result = await (from a in GetAsync(2)
                            from b in Get(a + 3)
                            select a * b);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(10);
    }

    [Fact]
    public async Task LinqQuerySyntax_AsyncThenSyncFailedResult_ReturnsErrorOfSyncResult()
    {
        // Act
        var result = await (from a in GetAsync(2)
                            from b in Result<int>.FromError(MixedError)
                            select a * b);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(MixedError);
    }

    [Fact]
    public async Task LinqQuerySyntax_FailedAsyncThenSync_ReturnsErrorAndDoesNotCallSyncSelector()
    {
        // Arrange
        var called = false;

        // Act
        var result = await (from a in GetErrorAsync()
                            from b in Track(a)
                            select a * b);

        // Assert
        result.Error.ShouldBeSameAs(MixedError);
        called.ShouldBeFalse();

        Result<int> Track(int value)
        {
            called = true;
            return Get(value);
        }
    }

    [Fact]
    public async Task LinqQuerySyntax_SyncThenAsyncSuccessfulResults_ReturnsCombinedValue()
    {
        // Act
        var result = await (from a in Get(2)
                            from b in GetAsync(a + 3)
                            select a * b);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(10);
    }

    [Fact]
    public async Task LinqQuerySyntax_SyncThenAsyncFailedResult_ReturnsErrorOfAsyncResult()
    {
        // Act
        var result = await (from a in Get(2)
                            from b in GetErrorAsync()
                            select a * b);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(MixedError);
    }

    [Fact]
    public async Task LinqQuerySyntax_FailedSyncThenAsync_ReturnsErrorAndDoesNotCallAsyncSelector()
    {
        // Arrange
        var called = false;

        // Act
        var result = await (from a in Result<int>.FromError(MixedError)
                            from b in Track(a)
                            select a * b);

        // Assert
        result.Error.ShouldBeSameAs(MixedError);
        called.ShouldBeFalse();

        ValueTask<Result<int>> Track(int value)
        {
            called = true;
            return GetAsync(value);
        }
    }

    [Fact]
    public async Task LinqQuerySyntax_SyncAsyncAndSyncSuccessfulResults_ReturnsCombinedValue()
    {
        // Act
        var result = await (from a in Get(1)
                            from b in GetAsync(a + 1)
                            from c in Get(b + 1)
                            select a + b + c);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(6);
    }

    [Fact]
    public async Task SelectMany_AsyncSourceWithNullSyncSelector_ThrowsArgumentNullException()
    {
        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await GetAsync(1).SelectMany((Func<int, Result<int>>)null!, (a, b) => a + b));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task SelectMany_SyncSourceWithNullAsyncSelector_ThrowsArgumentNullException()
    {
        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await Get(1).SelectMany((Func<int, ValueTask<Result<int>>>)null!, (a, b) => a + b));

        // Assert
        exception.ShouldBeOfType<ArgumentNullException>();
    }

    #endregion

    #region LINQ query selecting an Error (overload trap)

    // See issue #119.

    private static readonly Error TrapError = Error.Conflict(ErrorUri.Tag("tag:test.com,2024:Trap"), "Trap title");

    [Fact]
    public async Task LinqQuerySyntax_AsyncSuccessfulResultSelectingError_ReturnsFailedResultWithSelectedError()
    {
        // Arrange
        var passedValue = 0;

        // Act
        Result result = await (from a in GetAsync(5)
                               select Track(a));

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(TrapError);
        passedValue.ShouldBe(5);

        Error Track(int value)
        {
            passedValue = value;
            return TrapError;
        }
    }

    [Fact]
    public async Task LinqQuerySyntax_AsyncFailedResultSelectingError_ReturnsOriginalError()
    {
        // Act
        Result result = await (from a in GetErrorAsync()
                               select TrapError);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(MixedError);
    }

    [Fact]
    public async Task LinqQuerySyntax_AsyncThenAsyncSuccessfulResultsSelectingError_ReturnsFailedResultWithSelectedError()
    {
        // Arrange
        var passedValue = 0;

        // Act
        Result result = await (from a in GetAsync(2)
                               from b in GetAsync(a + 3)
                               select Track(a * b));

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(TrapError);
        passedValue.ShouldBe(10);

        Error Track(int value)
        {
            passedValue = value;
            return TrapError;
        }
    }

    [Fact]
    public async Task LinqQuerySyntax_AsyncThenFailedAsyncResultSelectingError_ReturnsErrorOfIntermediateResult()
    {
        // Act
        Result result = await (from a in GetAsync(2)
                               from b in GetErrorAsync()
                               select TrapError);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(MixedError);
    }

    [Fact]
    public async Task LinqQuerySyntax_AsyncThenSyncSuccessfulResultsSelectingError_ReturnsFailedResultWithSelectedError()
    {
        // Act
        Result result = await (from a in GetAsync(2)
                               from b in Get(a + 3)
                               select TrapError);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(TrapError);
    }

    [Fact]
    public async Task LinqQuerySyntax_FailedAsyncThenSyncSelectingError_ReturnsOriginalError()
    {
        // Act
        Result result = await (from a in GetErrorAsync()
                               from b in Get(a + 3)
                               select TrapError);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(MixedError);
    }

    [Fact]
    public async Task LinqQuerySyntax_SyncThenAsyncSuccessfulResultsSelectingError_ReturnsFailedResultWithSelectedError()
    {
        // Act
        Result result = await (from a in Get(2)
                               from b in GetAsync(a + 3)
                               select TrapError);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(TrapError);
    }

    [Fact]
    public async Task LinqQuerySyntax_FailedSyncThenAsyncSelectingError_ReturnsOriginalErrorAndDoesNotCallAsyncSelector()
    {
        // Arrange
        var called = false;

        // Act
        Result result = await (from a in Result<int>.FromError(MixedError)
                               from b in Track(a)
                               select TrapError);

        // Assert
        result.Error.ShouldBeSameAs(MixedError);
        called.ShouldBeFalse();

        ValueTask<Result<int>> Track(int value)
        {
            called = true;
            return GetAsync(value);
        }
    }

    #endregion

    #region LINQ query selecting a task (overload trap)

    // See issue #126.

    [Fact]
    public void Select_SelectorReturningTaskOverloads_AreHiddenCompileErrors()
    {
        // Arrange
        var overloads = typeof(LinqValueTaskAsyncExtensions).GetMethods()
            .Where(m => m.Name == "Select" && OverloadTrapHelper.IsTrap(m, "The selector returns a task"))
            .ToArray();

        // Act & Assert
        overloads.Length.ShouldBe(8);
        overloads.ShouldAllBe(m => m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                                   && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    [Fact]
    public void SelectMany_ResultSelectorReturningTaskOverloads_AreHiddenCompileErrors()
    {
        // Arrange
        var overloads = typeof(LinqValueTaskAsyncExtensions).GetMethods()
            .Where(m => m.Name == "SelectMany" && OverloadTrapHelper.IsTrap(m, "The selector returns a task"))
            .ToArray();

        // Act & Assert
        overloads.Length.ShouldBe(48);
        overloads.ShouldAllBe(m => m.GetCustomAttribute<ObsoleteAttribute>()!.IsError
                                   && m.GetCustomAttribute<EditorBrowsableAttribute>()!.State == EditorBrowsableState.Never);
    }

    [Fact]
    public void SelectAndSelectMany_Traps_HavePriorityOneForTaskSelectors()
    {
        // Arrange
        var overloads = typeof(LinqValueTaskAsyncExtensions).GetMethods()
            .Where(m => OverloadTrapHelper.IsTrap(m, "The selector returns a task")
                        && OverloadTrapHelper.IsTaskFunction(m.GetParameters()[^1].ParameterType))
            .ToArray();

        // Act & Assert
        overloads.Length.ShouldBe(28);
        overloads.ShouldAllBe(m => OverloadTrapHelper.Priority(m) == 1);
    }

    [Fact]
    public void SelectAndSelectMany_Traps_HaveDefaultPriorityForValueTaskSelectors()
    {
        // Arrange
        var overloads = typeof(LinqValueTaskAsyncExtensions).GetMethods()
            .Where(m => OverloadTrapHelper.IsTrap(m, "The selector returns a task")
                        && !OverloadTrapHelper.IsTaskFunction(m.GetParameters()[^1].ParameterType))
            .ToArray();

        // Act & Assert
        overloads.Length.ShouldBe(28);
        overloads.ShouldAllBe(m => OverloadTrapHelper.Priority(m) == 0);
    }

    [Fact]
    public async Task Select_SuccessfulResultValueTaskWithTaskSelectorAndExplicitTypeArguments_ReturnsResultWithTaskNotAwaited()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result<int>.FromValue(5));
        var taskSource = new TaskCompletionSource();

        // Act
        Result<Task> result = await sut.Select<int, Task>(_ => taskSource.Task);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(taskSource.Task);
        result.Value.IsCompleted.ShouldBeFalse();
    }

    #endregion

    #region non-generic results and selected results

    // See issue #217.

    private static readonly Error QuerySourceError = Error.NotFound(ErrorUri.None, "Query source error");
    private static readonly Error QueryCheckError = Error.Conflict(ErrorUri.None, "Query check error");
    private static readonly Error QuerySelectedError = Error.Failure(ErrorUri.None, "Query selected error");

    [Fact]
    public async Task SelectMany_TaskResultAndTaskNonGenericClause_ReturnsSelectedValue()
    {
        // Arrange
        var sut = new ValueTask<Result<int>>(Result.FromValue(5));

        // Act
        var result = await (from value in sut from _ in new ValueTask<Result>(Result.Success()) select value * 2);

        // Assert
        result.Value.ShouldBe(10);
    }

    [Fact]
    public async Task SelectMany_TaskResultAndFailedTaskNonGenericClause_ReturnsClauseError()
    {
        // Arrange
        var sut = new ValueTask<Result<int>>(Result.FromValue(5));

        // Act
        var result = await (from value in sut from _ in new ValueTask<Result>(Result.FromError(QueryCheckError)) select value * 2);

        // Assert
        result.Error.ShouldBeSameAs(QueryCheckError);
    }

    [Fact]
    public async Task SelectMany_FailedTaskResultAndTaskNonGenericClause_ReturnsSourceError()
    {
        // Arrange
        var sut = new ValueTask<Result<int>>(Result<int>.FromError(QuerySourceError));

        // Act
        var result = await (from value in sut from _ in new ValueTask<Result>(Result.Success()) select value * 2);

        // Assert
        result.Error.ShouldBeSameAs(QuerySourceError);
    }

    [Fact]
    public async Task SelectMany_TaskResultAndNonGenericClause_ReturnsSelectedValue()
    {
        // Arrange
        var sut = new ValueTask<Result<int>>(Result.FromValue(5));

        // Act
        var result = await (from value in sut from _ in Result.Success() select value * 2);

        // Assert
        result.Value.ShouldBe(10);
    }

    [Fact]
    public async Task SelectMany_ResultAndTaskNonGenericClause_ReturnsSelectedValue()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = await (from value in sut from _ in new ValueTask<Result>(Result.Success()) select value * 2);

        // Assert
        result.Value.ShouldBe(10);
    }

    [Fact]
    public async Task SelectMany_TaskNonGenericFirstClause_ReturnsSelectedValue()
    {
        // Arrange
        var sut = new ValueTask<Result>(Result.Success());

        // Act
        var result = await (from _ in sut from value in new ValueTask<Result<int>>(Result.FromValue(5)) select value * 2);

        // Assert
        result.Value.ShouldBe(10);
    }

    [Fact]
    public async Task SelectMany_FailedTaskNonGenericFirstClause_ReturnsItsError()
    {
        // Arrange
        var sut = new ValueTask<Result>(Result.FromError(QueryCheckError));

        // Act
        var result = await (from _ in sut from value in new ValueTask<Result<int>>(Result.FromValue(5)) select value * 2);

        // Assert
        result.Error.ShouldBeSameAs(QueryCheckError);
    }

    [Fact]
    public async Task SelectMany_TaskNonGenericFirstClauseAndSynchronousNextClause_ReturnsSelectedValue()
    {
        // Arrange
        var sut = new ValueTask<Result>(Result.Success());

        // Act
        var result = await (from _ in sut from value in Result.FromValue(5) select value * 2);

        // Assert
        result.Value.ShouldBe(10);
    }

    [Fact]
    public async Task Select_TaskNonGenericResult_ReturnsSelectedValue()
    {
        // Arrange
        var sut = new ValueTask<Result>(Result.Success());

        // Act
        var result = await (from _ in sut select 7);

        // Assert
        result.Value.ShouldBe(7);
    }

    [Fact]
    public async Task Select_TaskResultSelectingResultOfT_ReturnsSelectedResultNotNested()
    {
        // Arrange
        var sut = new ValueTask<Result<int>>(Result.FromValue(5));

        // Act
        var result = await (from value in sut select Result.FromValue(value.ToString()));

        // Assert
        result.Value.ShouldBe("5");
    }

    [Fact]
    public async Task SelectMany_TaskClausesSelectingResult_ReturnsSelectedResult()
    {
        // Arrange
        var sut = new ValueTask<Result<int>>(Result.FromValue(5));

        // Act
        var result = await (from value in sut from other in new ValueTask<Result<int>>(Result.FromValue(5)) select Result.FromError(QuerySelectedError));

        // Assert
        result.Error.ShouldBeSameAs(QuerySelectedError);
    }

    #endregion
}
