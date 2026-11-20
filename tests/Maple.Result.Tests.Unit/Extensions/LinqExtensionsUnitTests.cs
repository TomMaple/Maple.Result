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
using Moq;
using System;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Extensions;

public class LinqExtensionsUnitTests
{
    #region SelectMany(Result<T>, Func<T, Result<TMiddle>>, Func<T, TMiddle, TNext>)

    [Fact]
    public void SelectMany_NoResultWithSelectorFunctions_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.SelectMany(x => Result.FromValue(x.ToString()), (x, _) => (double)x));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void SelectMany_SuccessfulResultWithNoCollectionSelectorAndResultSelector_ThrowsException()
    {
        // Arrange
        const Func<int, Result<string>>? CollectionSelector = null;

        Result<int> sut = 5;

        // Act
        var exception = Record.Exception(() => sut.SelectMany(CollectionSelector!, (x, _) => (double)x));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void SelectMany_ErrorResultWithNoCollectionSelectorAndResultSelector_ThrowsException()
    {
        // Arrange
        const Func<int, Result<string>>? CollectionSelector = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = Record.Exception(() => sut.SelectMany(CollectionSelector!, (x, _) => (double)x));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void SelectMany_SuccessfulResultWithNoResultSelectorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, string, double>? ResultSelectorFunction = null;

        Result<int> sut = 5;

        // Act
        var exception = Record.Exception(() => sut.SelectMany(x => Result.FromValue(x.ToString()), ResultSelectorFunction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void SelectMany_ErrorResultWithNoResultSelectorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, string, double>? ResultSelectorFunction = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = Record.Exception(() => sut.SelectMany(x => Result.FromValue(x.ToString()), ResultSelectorFunction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void SelectMany_SuccessfulResultWithSelectorFunctions_CallsBothFunctions()
    {
        // Arrange
        const int InitialValue = 4839;
        const string ExpectedMiddleValue = "4839";
        const double ExpectedFinalValue = 4839;

        var collectionSelectorMock = new Mock<Func<int, Result<string>>>();
        collectionSelectorMock
            .Setup(x => x.Invoke(InitialValue))
            .Returns(Result.FromValue(ExpectedMiddleValue));

        var resultSelectorMock = new Mock<Func<int, string, double>>();
        resultSelectorMock
            .Setup(x => x.Invoke(InitialValue, ExpectedMiddleValue))
            .Returns(ExpectedFinalValue);

        Result<int> sut = InitialValue;

        // Act
        sut.SelectMany(collectionSelectorMock.Object, resultSelectorMock.Object);

        // Assert
        collectionSelectorMock.Verify(x => x.Invoke(InitialValue), Times.Once);
        resultSelectorMock.Verify(x => x.Invoke(InitialValue, ExpectedMiddleValue), Times.Once);
    }

    [Fact]
    public void SelectMany_SuccessfulResultWithSelectorFunctions_ReturnsValue()
    {
        // Arrange
        const int InitialValue = 4839;
        const double ExpectedFinalValue = 4839;

        Result<int> sut = InitialValue;

        // Act
        var result = sut.SelectMany(x => Result.FromValue(x.ToString()), (x, _) => (double)x);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<double>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(ExpectedFinalValue);
    }

    [Fact]
    public void SelectMany_ErrorResultWithSelectorFunctions_DoesNotCallFunctions()
    {
        // Arrange
        var collectionSelectorMock = new Mock<Func<int, Result<string>>>();
        var resultSelectorMock = new Mock<Func<int, string, double>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.SelectMany(collectionSelectorMock.Object, resultSelectorMock.Object);

        // Assert
        collectionSelectorMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
        resultSelectorMock.Verify(x => x.Invoke(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void SelectMany_ErrorResultWithSelectorFunctions_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = sut.SelectMany(x => Result.FromValue(x.ToString()), (x, _) => (double)x);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<double>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region Select(Result<T>, Func<T, TNext>)

    [Fact]
    public void Select_NoResultWithSelector_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Select(x => x.ToString()));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Select_SuccessfulResultWithNoSelector_ThrowsException()
    {
        // Arrange
        const Func<int, string>? Selector = null;

        Result<int> sut = 5;

        // Act
        var exception = Record.Exception(() => sut.Select(Selector!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Select_ErrorResultWithNoSelector_ThrowsException()
    {
        // Arrange
        const Func<int, string>? Selector = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = Record.Exception(() => sut.Select(Selector!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Select_SuccessfulResultWithSelector_CallsSelector()
    {
        // Arrange
        const int InitialValue = 4839;
        const string ExpectedValue = "4839";

        var selectorMock = new Mock<Func<int, string>>();
        selectorMock
            .Setup(x => x.Invoke(InitialValue))
            .Returns(ExpectedValue);

        Result<int> sut = InitialValue;

        // Act
        sut.Select(selectorMock.Object);

        // Assert
        selectorMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public void Select_SuccessfulResultWithSelector_ReturnsValue()
    {
        // Arrange
        const int InitialValue = 4839;
        const string ExpectedValue = "4839";

        Result<int> sut = InitialValue;

        // Act
        var result = sut.Select(x => x.ToString());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void Select_ErrorResultWithSelector_DoesNotCallSelector()
    {
        // Arrange
        var selectorMock = new Mock<Func<int, string>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.Select(selectorMock.Object);

        // Assert
        selectorMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Select_ErrorResultWithSelector_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = sut.Select(x => x.ToString());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region LINQ query syntax

    [Fact]
    public void LinqQuerySyntax_SingleClauseSuccessfulResult_ReturnsProjectedValueResult()
    {
        // Arrange
        const string ExpectedValue = "90";

        Result<int> sut = 90;

        // Act
        var result =
            from initialValue in sut
            select initialValue.ToString();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void LinqQuerySyntax_SingleClauseErrorResult_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result =
            from initialValue in sut
            select initialValue.ToString();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    [Fact]
    public void LinqQuerySyntax_SuccessfulResult_CallsFunctions()
    {
        // Arrange
        Result<int> sut = 90;

        var function1Mock = new Mock<Func<int, Result<int>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .Returns(100);

        var function2Mock = new Mock<Func<int, Result<string>>>();
        function2Mock
            .Setup(x => x.Invoke(100))
            .Returns("100");

        // Act
        _ = from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
        function2Mock.Verify(x => x.Invoke(100), Times.Once);
    }

    [Fact]
    public void LinqQuerySyntax_SuccessfulResult_ReturnsValueResult()
    {
        // Arrange
        const string ExpectedValue = "100";

        Result<int> sut = 90;

        var function1 = (int value) => Result.FromValue(value + 10);
        var function2 = (int value) => Result.FromValue(value.ToString());

        // Act
        var result = from initialValue in sut
                     from function1Result in function1(initialValue)
                     from function2Result in function2(function1Result)
                     select function2Result;

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void LinqQuerySyntax_FirstErrorFunction_CallsFirstFunction()
    {
        // Arrange
        Result<int> sut = 90;

        var function1Mock = new Mock<Func<int, Result<int>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .Returns(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, Result<string>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .Returns("100");

        // Act
        _ = from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
    }

    [Fact]
    public void LinqQuerySyntax_FirstErrorFunction_DoesNotCallSecondFunction()
    {
        // Arrange
        Result<int> sut = 90;

        var function1Mock = new Mock<Func<int, Result<int>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .Returns(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, Result<string>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .Returns("100");

        // Act
        _ = from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function2Mock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void LinqQuerySyntax_FirstErrorFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        var function1Mock = new Mock<Func<int, Result<int>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .Returns(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, Result<string>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .Returns("100");

        // Act
        var result = from initialValue in sut
                     from function1Result in function1Mock.Object(initialValue)
                     from function2Result in function2Mock.Object(function1Result)
                     select function2Result;

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    [Fact]
    public void LinqQuerySyntax_SecondErrorFunction_CallsFirstFunction()
    {
        // Arrange
        Result<int> sut = 90;

        var function1Mock = new Mock<Func<int, Result<int>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .Returns(100);

        var function2Mock = new Mock<Func<int, Result<string>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .Returns(GetErrorResult<string>());

        // Act
        _ = from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
    }

    [Fact]
    public void LinqQuerySyntax_SecondErrorFunction_CallsSecondFunction()
    {
        // Arrange
        Result<int> sut = 90;

        var function1Mock = new Mock<Func<int, Result<int>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .Returns(100);

        var function2Mock = new Mock<Func<int, Result<string>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .Returns(GetErrorResult<string>());

        // Act
        _ = from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function2Mock.Verify(x => x.Invoke(100), Times.Once);
    }

    [Fact]
    public void LinqQuerySyntax_SecondErrorFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        var function1Mock = new Mock<Func<int, Result<int>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .Returns(100);

        var function2Mock = new Mock<Func<int, Result<string>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .Returns(GetErrorResult<string>());

        // Act
        var result = from initialValue in sut
                     from function1Result in function1Mock.Object(initialValue)
                     from function2Result in function2Mock.Object(function1Result)
                     select function2Result;

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region helper methods

    private static Result<T> GetErrorResult<T>()
    {
        return Error.Failure(ErrorUri.None, "6b648e52-bde5-4e23-a233-c5dd4c4d2e73", "Error title 4");
    }

    #endregion

    #region LINQ query selecting an Error

    // See issue #119.

    private static readonly Error TrapError = Error.Conflict(ErrorUri.Tag("tag:test.com,2024:Trap"), "Trap title");

    private static Result<int> Get(int value)
        => Result<int>.FromValue(value);

    [Fact]
    public void LinqQuerySyntax_SuccessfulResultSelectingError_ReturnsFailedResultWithSelectedError()
    {
        // Arrange
        var passedValue = 0;

        // Act
        Result result = from a in Get(5)
                        select Track(a);

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
    public void LinqQuerySyntax_FailedResultSelectingError_ReturnsOriginalError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        Result result = from a in sut
                        select TrapError;

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    [Fact]
    public void LinqQuerySyntax_SuccessfulResultsWithTwoFromClausesSelectingError_ReturnsFailedResultWithSelectedError()
    {
        // Arrange
        var passedValue = 0;

        // Act
        Result result = from a in Get(2)
                        from b in Get(a + 3)
                        select Track(a * b);

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
    public void LinqQuerySyntax_FailedIntermediateResultSelectingError_ReturnsErrorOfIntermediateResult()
    {
        // Arrange
        var intermediate = GetErrorResult<int>();

        // Act
        Result result = from a in Get(2)
                        from b in intermediate
                        select TrapError;

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldBeSameAs(intermediate.Error);
    }

    [Fact]
    public void Select_SuccessfulResultWithErrorSelectorReturningNull_ThrowsInvalidOperationException()
    {
        // Act
        var exception = Record.Exception(() => Get(5).Select(_ => (Error)null!));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>();
        var invalidOperationException = exception as InvalidOperationException;
        invalidOperationException.ShouldNotBeNull();
        invalidOperationException.Message.ShouldBe("The function ‘selector’ returned null.");
    }

    [Fact]
    public void Select_SuccessfulResultWithErrorSelectorAndExplicitTypeArguments_ReturnsSuccessfulResultOfError()
    {
        // Act
        Result<Error> result = Get(5).Select<int, Error>(_ => TrapError);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeSameAs(TrapError);
    }

    #endregion

    #region LINQ query selecting a task in a result

    [Fact]
    public void Select_SuccessfulResultWithSelectorReturningTaskInResult_ReturnsResultWithTaskNotAwaited()
    {
        // Arrange
        var sut = Result<int>.FromValue(5);
        var taskSource = new TaskCompletionSource();

        // Act
        Result<Task> result = sut.Select(_ => Result.FromValue(taskSource.Task));

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

    private static Result Check(Action onCall)
    {
        onCall();
        return Result.Success();
    }

    private static Result<int> Next(Action onCall)
    {
        onCall();
        return Result.FromValue(1);
    }

    [Fact]
    public void Select_SuccessfulResultSelectingResultOfT_ReturnsSelectedResultNotNested()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = from value in sut select Result.FromValue(value.ToString());

        // Assert
        result.Value.ShouldBe("5");
    }

    [Fact]
    public void Select_SuccessfulResultSelectingFailedResult_ReturnsSelectedError()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = from value in sut select Result.FromError(QuerySelectedError);

        // Assert
        result.Error.ShouldBeSameAs(QuerySelectedError);
    }

    [Fact]
    public void Select_FailedResultSelectingResultOfT_ReturnsOriginalError()
    {
        // Arrange
        var sut = Result<int>.FromError(QuerySourceError);

        // Act
        var result = from value in sut select Result.FromValue(value.ToString());

        // Assert
        result.Error.ShouldBeSameAs(QuerySourceError);
    }

    [Fact]
    public void Select_ExplicitTypeArgumentsSelectingResult_KeepsNestedResult()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = sut.Select<int, Result<string>>(value => Result.FromValue(value.ToString()));

        // Assert
        result.Value.Value.ShouldBe("5");
    }

    [Fact]
    public void Select_SuccessfulNonGenericResult_ReturnsSelectedValue()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = from _ in sut select 7;

        // Assert
        result.Value.ShouldBe(7);
    }

    [Fact]
    public void Select_FailedNonGenericResult_ReturnsOriginalError()
    {
        // Arrange
        var sut = Result.FromError(QueryCheckError);

        // Act
        var result = from _ in sut select 7;

        // Assert
        result.Error.ShouldBeSameAs(QueryCheckError);
    }

    [Fact]
    public void Select_SuccessfulNonGenericResultSelectingError_ReturnsFailedResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = from _ in sut select QuerySelectedError;

        // Assert
        result.Error.ShouldBeSameAs(QuerySelectedError);
    }

    [Fact]
    public void Select_SuccessfulNonGenericResultSelectingResult_ReturnsSelectedResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = from _ in sut select Result.FromError(QuerySelectedError);

        // Assert
        result.Error.ShouldBeSameAs(QuerySelectedError);
    }

    [Fact]
    public void Select_SuccessfulNonGenericResultSelectingResultOfT_ReturnsSelectedResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = from _ in sut select Result.FromValue("next");

        // Assert
        result.Value.ShouldBe("next");
    }

    [Fact]
    public void Select_NonGenericResult_PassesTheResultAsRangeVariable()
    {
        // Arrange
        var sut = Result.Success();
        Result? received = null;

        // Act
        var result = from validated in sut select received = validated;

        // Assert
        received.ShouldBeSameAs(sut);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public void SelectMany_SuccessfulResultAndSuccessfulNonGenericClause_ReturnsSelectedValue()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = from value in sut from _ in Result.Success() select value * 2;

        // Assert
        result.Value.ShouldBe(10);
    }

    [Fact]
    public void SelectMany_SuccessfulResultAndFailedNonGenericClause_ReturnsClauseError()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = from value in sut from _ in Result.FromError(QueryCheckError) select value * 2;

        // Assert
        result.Error.ShouldBeSameAs(QueryCheckError);
    }

    [Fact]
    public void SelectMany_FailedResultAndNonGenericClause_ReturnsSourceErrorAndDoesNotRunClause()
    {
        // Arrange
        var sut = Result<int>.FromError(QuerySourceError);
        var called = false;

        // Act
        var result = from value in sut from _ in Check(() => called = true) select value * 2;

        // Assert
        result.Error.ShouldBeSameAs(QuerySourceError);
        called.ShouldBeFalse();
    }

    [Fact]
    public void SelectMany_NonGenericClauseReturningError_ReturnsThatError()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = from value in sut from _ in QueryCheckError select value;

        // Assert
        result.Error.ShouldBeSameAs(QueryCheckError);
    }

    [Fact]
    public void SelectMany_NonGenericClauseAndSelectedError_ReturnsSelectedError()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = from value in sut from _ in Result.Success() select QuerySelectedError;

        // Assert
        result.Error.ShouldBeSameAs(QuerySelectedError);
    }

    [Fact]
    public void SelectMany_NonGenericClauseAndSelectedResult_ReturnsSelectedResult()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = from value in sut from _ in Result.Success() select Result.FromError(QuerySelectedError);

        // Assert
        result.Error.ShouldBeSameAs(QuerySelectedError);
    }

    [Fact]
    public void SelectMany_NonGenericClauseAndSelectedResultOfT_ReturnsSelectedResult()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = from value in sut from _ in Result.Success() select Result.FromValue(value.ToString());

        // Assert
        result.Value.ShouldBe("5");
    }

    [Fact]
    public void SelectMany_SuccessfulNonGenericFirstClause_ReturnsSelectedValue()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = from _ in sut from value in Result.FromValue(5) select value * 2;

        // Assert
        result.Value.ShouldBe(10);
    }

    [Fact]
    public void SelectMany_FailedNonGenericFirstClause_ReturnsItsErrorAndDoesNotRunNextClause()
    {
        // Arrange
        var sut = Result.FromError(QueryCheckError);
        var called = false;

        // Act
        var result = from _ in sut from value in Next(() => called = true) select value;

        // Assert
        result.Error.ShouldBeSameAs(QueryCheckError);
        called.ShouldBeFalse();
    }

    [Fact]
    public void SelectMany_NonGenericFirstClauseAndFailedNextClause_ReturnsNextError()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = from _ in sut from value in Result<int>.FromError(QuerySourceError) select value;

        // Assert
        result.Error.ShouldBeSameAs(QuerySourceError);
    }

    [Fact]
    public void SelectMany_NonGenericFirstClauseAndSelectedError_ReturnsSelectedError()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = from _ in sut from value in Result.FromValue(5) select QuerySelectedError;

        // Assert
        result.Error.ShouldBeSameAs(QuerySelectedError);
    }

    [Fact]
    public void SelectMany_NonGenericFirstClauseAndSelectedResult_ReturnsSelectedResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = from _ in sut from value in Result.FromValue(5) select Result.FromError(QuerySelectedError);

        // Assert
        result.Error.ShouldBeSameAs(QuerySelectedError);
    }

    [Fact]
    public void SelectMany_NonGenericFirstClauseAndSelectedResultOfT_ReturnsSelectedResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = from _ in sut from value in Result.FromValue(5) select Result.FromValue(value.ToString());

        // Assert
        result.Value.ShouldBe("5");
    }

    [Fact]
    public void SelectMany_TwoSuccessfulNonGenericClauses_ReturnsSelectedValue()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = from _ in sut from checkedAgain in Result.Success() select 7;

        // Assert
        result.Value.ShouldBe(7);
    }

    [Fact]
    public void SelectMany_SecondNonGenericClauseFailed_ReturnsItsError()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = from _ in sut from checkedAgain in Result.FromError(QueryCheckError) select 7;

        // Assert
        result.Error.ShouldBeSameAs(QueryCheckError);
    }

    [Fact]
    public void SelectMany_TwoNonGenericClausesAndSelectedError_ReturnsSelectedError()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = from _ in sut from checkedAgain in Result.Success() select QuerySelectedError;

        // Assert
        result.Error.ShouldBeSameAs(QuerySelectedError);
    }

    [Fact]
    public void SelectMany_TwoNonGenericClausesAndSelectedResult_ReturnsSelectedResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = from _ in sut from checkedAgain in Result.Success() select Result.FromError(QuerySelectedError);

        // Assert
        result.Error.ShouldBeSameAs(QuerySelectedError);
    }

    [Fact]
    public void SelectMany_TwoNonGenericClausesAndSelectedResultOfT_ReturnsSelectedResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = from _ in sut from checkedAgain in Result.Success() select Result.FromValue("next");

        // Assert
        result.Value.ShouldBe("next");
    }

    [Fact]
    public void SelectMany_GenericClausesSelectingResult_ReturnsSelectedResult()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = from value in sut from other in Result.FromValue(5) select Result.FromError(QuerySelectedError);

        // Assert
        result.Error.ShouldBeSameAs(QuerySelectedError);
    }

    [Fact]
    public void SelectMany_GenericClausesSelectingResultOfT_ReturnsSelectedResultNotNested()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = from value in sut from other in Result.FromValue(5) select Result.FromValue(value + other);

        // Assert
        result.Value.ShouldBe(10);
    }

    [Fact]
    public void SelectMany_ThreeClausesWithNonGenericMiddle_ReturnsSelectedValue()
    {
        // Arrange
        var sut = Result.FromValue(5);

        // Act
        var result = from value in sut from _ in Result.Success() from other in Result.FromValue(5) select value + other;

        // Assert
        result.Value.ShouldBe(10);
    }

    #endregion
}
