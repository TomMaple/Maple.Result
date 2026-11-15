// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Maple.Results.Tests.Unit.ResultTests;

// Some tests also check the nullable annotations at compile time: they read the out values without `!`,
// so a missing or wrong annotation is a nullable warning (an error in CI).
public class ResultTryGetUnitTests
{
    private static readonly Error TestError = Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title");

    #region Result<T>, TryGetValue(out T, out Error)

    [Fact]
    public void TryGetValueWithError_Success_ReturnsTrueAndTheValue()
    {
        // Arrange
        var sut = Result.FromValue(42);

        // Act
        var result = sut.TryGetValue(out var value, out var error);

        // Assert
        result.ShouldBeTrue();
        value.ShouldBe(42);
        error.ShouldBeNull();
    }

    [Fact]
    public void TryGetValueWithError_SuccessWithNullValue_ReturnsTrueAndNull()
    {
        // Arrange
        var sut = Result.FromValue<string?>(null);

        // Act
        var result = sut.TryGetValue(out var value, out var error);

        // Assert
        result.ShouldBeTrue();
        value.ShouldBeNull();
        error.ShouldBeNull();
    }

    [Fact]
    public void TryGetValueWithError_Failure_ReturnsFalseAndTheError()
    {
        // Arrange
        var sut = Result<int>.FromError(TestError);

        // Act
        var result = sut.TryGetValue(out var value, out var error);

        // Assert
        result.ShouldBeFalse();
        value.ShouldBe(0);
        error.ShouldBe(TestError);
    }

    [Fact]
    public void TryGetValueWithError_EarlyReturnOnAnExpression_ValueAndErrorAreNotNull()
    {
        // Act
        var success = GetLength(Result.FromValue("value"));
        var failure = GetLength(Result<string>.FromError(TestError));

        // Assert
        success.ShouldBe(Result.FromValue(5));
        failure.ShouldBe(Result<int>.FromError(TestError));

        static Result<int> GetLength(Result<string> source)
        {
            if (!source.TryGetValue(out var text, out var error))
                return error;

            return text.Length;
        }
    }

    #endregion

    #region Result<T>, TryGetValue(out T)

    [Fact]
    public void TryGetValue_Success_ReturnsTrueAndTheValue()
    {
        // Arrange
        var sut = Result.FromValue("value");

        // Act
        var result = sut.TryGetValue(out var value);

        // Assert
        result.ShouldBeTrue();
        value.ShouldBe("value");
    }

    [Fact]
    public void TryGetValue_Failure_ReturnsFalseAndTheDefaultValue()
    {
        // Arrange
        var sut = Result<string>.FromError(TestError);

        // Act
        var result = sut.TryGetValue(out var value);

        // Assert
        result.ShouldBeFalse();
        value.ShouldBeNull();
    }

    [Fact]
    public void TryGetValue_EarlyReturn_ValueAndErrorPropertyAreNotNull()
    {
        // Act
        var success = GetLength(Result.FromValue("value"));
        var failure = GetLength(Result<string>.FromError(TestError));

        // Assert
        success.ShouldBe(Result.FromValue(5));
        failure.ShouldBe(Result<int>.FromError(TestError));

        static Result<int> GetLength(Result<string> source)
        {
            if (!source.TryGetValue(out var text))
                return source.Error;

            return text.Length;
        }
    }

    #endregion

    #region Result, TryGetError

    [Fact]
    public void TryGetError_ResultFailure_ReturnsTrueAndTheError()
    {
        // Arrange
        var sut = Result.FromError(TestError);

        // Act
        var result = sut.TryGetError(out var error);

        // Assert
        result.ShouldBeTrue();
        error.ShouldBe(TestError);
    }

    [Fact]
    public void TryGetError_ResultSuccess_ReturnsFalseAndNull()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = sut.TryGetError(out var error);

        // Assert
        result.ShouldBeFalse();
        error.ShouldBeNull();
    }

    [Fact]
    public void TryGetError_ResultEarlyReturn_ErrorIsNotNull()
    {
        // Act
        var success = Save(Result.Success());
        var failure = Save(Result.FromError(TestError));

        // Assert
        success.ShouldBe(Result.Success());
        failure.ShouldBe(Result.FromError(TestError));

        static Result Save(Result validation)
        {
            if (validation.TryGetError(out var error))
                return error;

            return Result.Success();
        }
    }

    #endregion

    #region Result<T>, TryGetError

    [Fact]
    public void TryGetError_ResultTFailure_ReturnsTrueAndTheError()
    {
        // Arrange
        var sut = Result<int>.FromError(TestError);

        // Act
        var result = sut.TryGetError(out var error);

        // Assert
        result.ShouldBeTrue();
        error.ShouldBe(TestError);
    }

    [Fact]
    public void TryGetError_ResultTSuccess_ReturnsFalseAndNull()
    {
        // Arrange
        var sut = Result.FromValue(42);

        // Act
        var result = sut.TryGetError(out var error);

        // Assert
        result.ShouldBeFalse();
        error.ShouldBeNull();
    }

    [Fact]
    public void TryGetError_ResultTEarlyReturn_ErrorIsNotNull()
    {
        // Act
        var success = Describe(Result.FromValue(42));
        var failure = Describe(Result<int>.FromError(TestError));

        // Assert
        success.ShouldBe("ok");
        failure.ShouldBe("Test title");

        static string Describe(Result<int> source)
        {
            if (source.TryGetError(out var error))
                return error.Title;

            return "ok";
        }
    }

    #endregion

    #region IOperationResult, TryGetError

    [Fact]
    public void TryGetError_ResultFailureAsInterface_ReturnsTrueAndTheError()
    {
        // Arrange
        IOperationResult sut = Result.FromError(TestError);

        // Act
        var result = sut.TryGetError(out var error);

        // Assert
        result.ShouldBeTrue();
        error.ShouldBe(TestError);
    }

    [Fact]
    public void TryGetError_ResultTSuccessAsInterface_ReturnsFalseAndNull()
    {
        // Arrange
        IOperationResult sut = Result.FromValue(42);

        // Act
        var result = sut.TryGetError(out var error);

        // Assert
        result.ShouldBeFalse();
        error.ShouldBeNull();
    }

    [Fact]
    public void TryGetError_InterfaceEarlyReturn_ErrorIsNotNull()
    {
        // Act
        var success = Describe(Result.FromValue(42));
        var failure = Describe(Result<int>.FromError(TestError));

        // Assert
        success.ShouldBe("ok");
        failure.ShouldBe("Test title");

        static string Describe(IOperationResult source)
        {
            if (source.TryGetError(out var error))
                return error.Title;

            return "ok";
        }
    }

    #endregion
}
