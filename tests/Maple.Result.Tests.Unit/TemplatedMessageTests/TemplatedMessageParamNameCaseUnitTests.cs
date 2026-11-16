// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Maple.Results.Tests.Unit.TemplatedMessageTests;

// Two parameter names that differ only in case (#235) are a typo: one of the values would be ignored, and a naming policy that changes the
// case of the keys would write both as the same member of "params". The rule of the metadata keys (#160) applies; the same name repeated
// keeps its own message.
public class TemplatedMessageParamNameCaseUnitTests
{
    private const string ExpectedMessage = "The parameter names ‘Min’ and ‘min’ differ only in case. "
        + "Names that differ only in case would be the same placeholder and the same member of ‘params’; use one spelling.";

    #region constructors

    [Fact]
    public void Ctor_TupleNamesDifferingOnlyInCase_ThrowsArgumentException()
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", ("Min", 3), ("min", 5)));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("parameters");
    }

    [Fact]
    public void Ctor_TupleNamesDifferingOnlyInCase_MessageNamesBothNames()
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", ("Min", 3), ("min", 5)));

        // Assert
        exception.ShouldNotBeNull().Message.ShouldBe($"{ExpectedMessage} (Parameter 'parameters')");
    }

    [Fact]
    public void Ctor_TupleNamesDifferingOnlyInCaseNotNextToEachOther_ThrowsArgumentException()
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", ("Min", 3), ("max", 9), ("min", 5)));

        // Assert
        exception.ShouldNotBeNull().Message.ShouldBe($"{ExpectedMessage} (Parameter 'parameters')");
    }

    [Fact]
    public void Ctor_PairNamesDifferingOnlyInCase_ThrowsArgumentException()
    {
        // Arrange
        var parameters = new List<KeyValuePair<string, object?>> { new("Min", 3), new("min", 5) };

        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", parameters));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("parameters");
    }

    [Fact]
    public void Ctor_PairNamesDifferingOnlyInCase_MessageNamesBothNames()
    {
        // Arrange
        var parameters = new List<KeyValuePair<string, object?>> { new("Min", 3), new("min", 5) };

        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", parameters));

        // Assert
        exception.ShouldNotBeNull().Message.ShouldBe($"{ExpectedMessage} (Parameter 'parameters')");
    }

    [Fact]
    public void Ctor_DictionaryWithNamesDifferingOnlyInCase_ThrowsArgumentException()
    {
        // Arrange
        var parameters = new Dictionary<string, object?> { ["Min"] = 3, ["min"] = 5 };

        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", parameters));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("parameters");
    }

    [Fact]
    public void Ctor_RepeatedTupleName_KeepsTheMessageOfARepeatedName()
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", ("min", 3), ("min", 5)));

        // Assert
        exception.ShouldNotBeNull().Message.ShouldBe("The parameter ‘min’ is repeated. (Parameter 'parameters')");
    }

    [Fact]
    public void Ctor_RepeatedPairName_KeepsTheMessageOfARepeatedName()
    {
        // Arrange
        var parameters = new List<KeyValuePair<string, object?>> { new("min", 3), new("min", 5) };

        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", parameters));

        // Assert
        exception.ShouldNotBeNull().Message.ShouldBe("The parameter ‘min’ is repeated. (Parameter 'parameters')");
    }

    [Fact]
    public void Ctor_NamesThatDifferNotOnlyInCase_AreAccepted()
    {
        // Act
        var result = new TemplatedMessage("errors.length", ("Min", 3), ("Max", 20), ("minLength", 1));

        // Assert
        result.Params.ShouldNotBeNull().Count.ShouldBe(3);
    }

    [Fact]
    public void Params_Lookup_StaysOrdinal()
    {
        // Arrange
        var sut = new TemplatedMessage("errors.length", ("Min", 3));

        // Act
        var result = sut.Params.ShouldNotBeNull();

        // Assert
        result.ContainsKey("Min").ShouldBeTrue();
        result.ContainsKey("min").ShouldBeFalse();
    }

    [Fact]
    public void Ctor_ParamsOfAnotherMessage_AreAccepted()
    {
        // Arrange
        var other = new TemplatedMessage("errors.length", ("Min", 3), ("Max", 20));

        // Act
        var result = new TemplatedMessage("errors.size", other.Params);

        // Assert
        result.Params.ShouldBeSameAs(other.Params);
    }

    #endregion

    #region linear time

    // The names are checked while the dictionary is built, with a lookup per name (#234): 40,000 names take milliseconds; the limit is
    // generous for slow CI machines.
    [Fact]
    public void Ctor_ManyTupleNames_AreCheckedInLinearTime()
    {
        // Arrange
        var maximum = TimeSpan.FromSeconds(2);
        var parameters = Enumerable.Range(0, 40_000)
            .Select(i => ($"name{i}", (object?)i))
            .ToArray();
        var stopwatch = Stopwatch.StartNew();

        // Act
        var result = new TemplatedMessage("errors.many", parameters);

        // Assert
        stopwatch.Elapsed.ShouldBeLessThan(maximum);
        result.Params.ShouldNotBeNull().Count.ShouldBe(40_000);
    }

    [Fact]
    public void Ctor_ManyPairNames_AreCheckedInLinearTime()
    {
        // Arrange
        var maximum = TimeSpan.FromSeconds(2);
        var parameters = Enumerable.Range(0, 40_000)
            .Select(i => new KeyValuePair<string, object?>($"name{i}", i))
            .ToList();
        var stopwatch = Stopwatch.StartNew();

        // Act
        var result = new TemplatedMessage("errors.many", parameters);

        // Assert
        stopwatch.Elapsed.ShouldBeLessThan(maximum);
        result.Params.ShouldNotBeNull().Count.ShouldBe(40_000);
    }

    #endregion

    #region the methods that create a templated message

    [Fact]
    public void ErrorFactory_NamedValuesDifferingOnlyInCase_ThrowsArgumentException()
    {
        // Act
        var exception = Record.Exception(() => Error.Validation(ErrorUri.None, "Invalid", detailTemplateId: "errors.length",
            detailNamedValues: [("Min", 3), ("min", 5)]));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().Message.ShouldBe($"{ExpectedMessage} (Parameter 'detailNamedValues')");
    }

    [Fact]
    public void ErrorFactoryWithPairs_NamedValuesDifferingOnlyInCase_ThrowsArgumentException()
    {
        // Arrange
        var namedValues = new Dictionary<string, object?> { ["Min"] = 3, ["min"] = 5 };

        // Act
        var exception = Record.Exception(() => Error.Validation(ErrorUri.None, "Invalid", detailTemplateId: "errors.length",
            detailNamedValues: namedValues));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("detailNamedValues");
    }

    [Fact]
    public void ErrorWithErrorDetail_NamedValuesDifferingOnlyInCase_ThrowsArgumentException()
    {
        // Arrange
        var sut = Error.Validation(ErrorUri.None, "Invalid");

        // Act
        var exception = Record.Exception(() => sut.WithErrorDetail("/name", "Too short.", "errors.length", ("Min", 3), ("min", 5)));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("detailNamedValues");
    }

    [Fact]
    public void ErrorBuilderWithDetailTemplated_NamedValuesDifferingOnlyInCase_ThrowsArgumentException()
    {
        // Arrange
        var sut = ErrorBuilder.Validation(ErrorUri.None, "Invalid");

        // Act
        var exception = Record.Exception(() => sut.WithDetailTemplated("errors.length", ("Min", 3), ("min", 5)));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("namedValues");
    }

    [Fact]
    public void ErrorBuilderAddErrorDetail_NamedValuesDifferingOnlyInCase_ThrowsArgumentException()
    {
        // Arrange
        var sut = ErrorBuilder.Validation(ErrorUri.None, "Invalid");

        // Act
        var exception = Record.Exception(() => sut.AddErrorDetail("/name", "Too short.", "errors.length", ("Min", 3), ("min", 5)));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("detailNamedValues");
    }

    #endregion
}
