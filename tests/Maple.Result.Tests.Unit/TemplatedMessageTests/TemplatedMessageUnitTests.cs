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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Maple.Results.Tests.Unit.TemplatedMessageTests;

public class TemplatedMessageUnitTests
{
    #region read-only fields

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    #endregion

    #region ctor

    [Fact]
    public void Ctor_NullTemplateId_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage(null!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        var argumentNullException = exception as ArgumentNullException;
        argumentNullException.ShouldNotBeNull();
        argumentNullException.ParamName.ShouldBe("templateId");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Ctor_BlankTemplateId_ThrowsArgumentException(string templateId)
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage(templateId));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        var argumentException = exception as ArgumentException;
        argumentException.ShouldNotBeNull();
        argumentException.ParamName.ShouldBe("templateId");
    }

    [Fact]
    public void Ctor_NullParameters_KeepsNullParameters()
    {
        // Act
        var sut = new TemplatedMessage("message-id");

        // Assert
        sut.TemplateId.ShouldBe("message-id");
        sut.Parameters.ShouldBeNull();
    }

    [Fact]
    public void Ctor_SourceDictionaryChangedLater_ParametersAreNotAffected()
    {
        // Arrange
        var source = new Dictionary<string, object?> { ["min"] = 3 };

        // Act
        var sut = new TemplatedMessage("message-id", source);
        source["min"] = 5;
        source["max"] = 10;

        // Assert
        sut.Parameters.ShouldNotBeNull();
        sut.Parameters.Count.ShouldBe(1);
        sut.Parameters["min"].ShouldBe(3);
        sut.Parameters.ShouldNotBeSameAs(source);
    }

    [Fact]
    public void Ctor_NullParamValue_KeepsNullValue()
    {
        // Act
        var sut = new TemplatedMessage("message-id", new Dictionary<string, object?> { ["name"] = null });

        // Assert
        sut.Parameters.ShouldNotBeNull();
        sut.Parameters.ShouldContainKey("name");
        sut.Parameters["name"].ShouldBeNull();
    }

    [Fact]
    public void Parameters_CastToMutableDictionary_CannotBeModified()
    {
        // Arrange
        var sut = new TemplatedMessage("message-id", new Dictionary<string, object?> { ["min"] = 3 });

        // Act & Assert
        var dictionary = sut.Parameters.ShouldBeAssignableTo<IDictionary<string, object?>>();
        dictionary.ShouldNotBeNull();
        dictionary.IsReadOnly.ShouldBeTrue();
        Should.Throw<NotSupportedException>(() => dictionary["min"] = 5);
        sut.Parameters!["min"].ShouldBe(3);
    }

    [Fact]
    public void Parameters_Keys_AreCaseSensitive()
    {
        // Arrange
        var source = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase) { ["min"] = 3 };

        // Act
        var sut = new TemplatedMessage("message-id", source);

        // Assert
        sut.Parameters.ShouldNotBeNull();
        sut.Parameters.ContainsKey("min").ShouldBeTrue();
        sut.Parameters.ContainsKey("MIN").ShouldBeFalse();
    }

    #endregion

    #region parameters

    [Fact]
    public void Ctor_InlineTuples_BuildsParameters()
    {
        // Act
        var sut = new TemplatedMessage("errors.length", ("min", 3), ("max", null));

        // Assert
        sut.Parameters.ShouldNotBeNull();
        sut.Parameters.Count.ShouldBe(2);
        sut.Parameters["min"].ShouldBe(3);
        sut.Parameters["max"].ShouldBeNull();
    }

    [Fact]
    public void Ctor_TupleArray_BuildsParameters()
    {
        // Arrange
        (string Key, object? Value)[] parameters = [("min", 3), ("max", 10)];

        // Act
        var sut = new TemplatedMessage("errors.length", parameters);

        // Assert
        sut.Parameters.ShouldNotBeNull();
        sut.Parameters.Count.ShouldBe(2);
        sut.Parameters["max"].ShouldBe(10);
    }

    [Fact]
    public void Ctor_ListOfPairs_BuildsParameters()
    {
        // Arrange
        var parameters = new List<KeyValuePair<string, object?>> { new("min", 3) };

        // Act
        var sut = new TemplatedMessage("errors.length", parameters);

        // Assert
        sut.Parameters.ShouldNotBeNull();
        sut.Parameters["min"].ShouldBe(3);
    }

    [Fact]
    public void Ctor_NullSecondArgument_KeepsNullParameters()
    {
        // Act
        var sut = new TemplatedMessage("errors.length", null);

        // Assert
        sut.Parameters.ShouldBeNull();
    }

    [Fact]
    public void Ctor_EmptyCollectionExpression_BuildsNullParameters()
    {
        // Act
        var sut = new TemplatedMessage("errors.length", []);

        // Assert
        sut.Parameters.ShouldBeNull();
    }

    [Fact]
    public void Ctor_EmptyTupleArray_BuildsNullParameters()
    {
        // Act
        var sut = new TemplatedMessage("errors.length", Array.Empty<(string Key, object? Value)>());

        // Assert
        sut.Parameters.ShouldBeNull();
    }

    [Fact]
    public void Ctor_EmptyDictionary_BuildsNullParameters()
    {
        // Act
        var sut = new TemplatedMessage("errors.length", new Dictionary<string, object?>());

        // Assert
        sut.Parameters.ShouldBeNull();
    }

    [Fact]
    public void Ctor_EmptySequenceOfUnknownLength_BuildsNullParameters()
    {
        // Arrange
        var parameters = NoParameters();

        // Act
        var sut = new TemplatedMessage("errors.length", parameters);

        // Assert
        sut.Parameters.ShouldBeNull();
    }

    [Fact]
    public void Ctor_EmptyMetadataOfAnError_BuildsNullParameters()
    {
        // Arrange
        var error = Error.NotFound(ErrorUri.None, "Missing");

        // Act
        var sut = new TemplatedMessage("errors.length", error.Metadata);

        // Assert
        sut.Parameters.ShouldBeNull();
    }

    [Fact]
    public void Ctor_RepeatedTupleName_ThrowsArgumentException()
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", ("min", 3), ("min", 4)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        var argumentException = exception as ArgumentException;
        argumentException.ShouldNotBeNull();
        exception.Message.ShouldContain("‘min’ is repeated");
        argumentException.ParamName.ShouldBe("parameters");
    }

    [Fact]
    public void Ctor_RepeatedPairName_ThrowsArgumentException()
    {
        // Arrange
        var parameters = new List<KeyValuePair<string, object?>> { new("min", 3), new("min", 4) };

        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", parameters));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        var argumentException = exception as ArgumentException;
        argumentException.ShouldNotBeNull();
        argumentException.ParamName.ShouldBe("parameters");
    }

    [Fact]
    public void Ctor_NullTupleName_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", (null!, 3)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        var argumentNullException = exception as ArgumentNullException;
        argumentNullException.ShouldNotBeNull();
        argumentNullException.ParamName.ShouldBe("parameters");
    }

    [Fact]
    public void Ctor_NullPairName_ThrowsArgumentNullException()
    {
        // Arrange
        var parameters = new List<KeyValuePair<string, object?>> { new(null!, 3) };

        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", parameters));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        var argumentNullException = exception as ArgumentNullException;
        argumentNullException.ShouldNotBeNull();
        argumentNullException.ParamName.ShouldBe("parameters");
    }

    [Fact]
    public void Equals_SameParametersFromTuplesAndDictionary_AreEqual()
    {
        // Arrange
        var fromTuples = new TemplatedMessage("errors.length", ("min", 3), ("max", 10));
        var fromDictionary = new TemplatedMessage("errors.length",
            new Dictionary<string, object?> { ["max"] = 10, ["min"] = 3 });

        // Act & Assert
        fromTuples.ShouldBe(fromDictionary);
        fromTuples.GetHashCode().ShouldBe(fromDictionary.GetHashCode());
    }

    #endregion

    #region ToString

    [Fact]
    public void ToString_WithParameters_WritesTheirContent()
    {
        // Arrange
        var sut = new TemplatedMessage("message-id", new Dictionary<string, object?> { ["min"] = 3 });

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe("TemplatedMessage { TemplateId = message-id, Parameters = { min = 3 } }");
    }

    [Fact]
    public void ToString_SeveralParameters_SeparatesThemWithCommas()
    {
        // Arrange
        var sut = new TemplatedMessage("message-id", ("min", 3), ("max", 5));

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe("TemplatedMessage { TemplateId = message-id, Parameters = { min = 3, max = 5 } }");
    }

    [Fact]
    public void ToString_EmptyParameters_WritesNothingForParameters()
    {
        // Arrange
        var sut = new TemplatedMessage("message-id", new Dictionary<string, object?>());

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe("TemplatedMessage { TemplateId = message-id, Parameters =  }");
    }

    [Fact]
    public void ToString_NullParameters_WritesNothingForParameters()
    {
        // Arrange
        var sut = new TemplatedMessage("message-id");

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe("TemplatedMessage { TemplateId = message-id, Parameters =  }");
    }

    #endregion

    #region Serialize

    [Fact]
    public void Serialize_OnlyTemplateId_ReturnsSerializedValueWithoutParameters()
    {
        // Arrange
        const string ExpectedText = "{\"templateId\":\"user.account.activated\"}";

        const string TemplateId = "user.account.activated";
        var sut = new TemplatedMessage(TemplateId);

        // Act
        var result = Serialize(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_EmptyParameters_ReturnsSerializedValueWithoutParameters()
    {
        // Arrange
        const string ExpectedText = "{\"templateId\":\"user.account.activated\"}";

        const string TemplateId = "user.account.activated";
        var sut = new TemplatedMessage(TemplateId, new Dictionary<string, object?>());

        // Act
        var result = Serialize(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_NotEmptyParameters_ReturnsSerializedValueWithParameters()
    {
        // Arrange
        const string ExpectedText =
            "{\"templateId\":\"user.account.activated\",\"parameters\":{\"userName\":\"johnny\",\"isSuccessful\":true,\"bonusPoints\":100," +
            "\"weight\":78.3,\"dob\":\"1980-04-19\",\"registered\":\"2022-04-15T12:17:58-05:00\"}}";

        const string TemplateId = "user.account.activated";
        var parameters = new Dictionary<string, object?>
        {
            ["userName"] = "johnny",
            ["isSuccessful"] = true,
            ["bonusPoints"] = 100,
            ["weight"] = 78.3m,
            ["dob"] = new DateOnly(1980, 4, 19),
            ["registered"] = new DateTimeOffset(2022, 4, 15, 12, 17, 58, TimeSpan.FromHours(-5))
        };

        var sut = new TemplatedMessage(TemplateId, parameters);

        // Act
        var result = Serialize(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    #endregion

    #region helper methods

    // An iterator: its length is unknown until it is enumerated.
    private static IEnumerable<KeyValuePair<string, object?>> NoParameters()
    {
        yield break;
    }

    private string Serialize(TemplatedMessage templatedMessage)
    {
        var result = JsonSerializer.Serialize(templatedMessage, _jsonSerializerOptions);
        return result;
    }

    #endregion

    #region empty and padded names

    // See issue #203.

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" min")]
    [InlineData("min ")]
    [InlineData("\tmin")]
    public void Ctor_EmptyOrPaddedTupleName_ThrowsArgumentException(string name)
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", (name, 3)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        var argumentException = exception as ArgumentException;
        argumentException.ShouldNotBeNull();
        argumentException.ParamName.ShouldBe("parameters");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" min")]
    [InlineData("min ")]
    [InlineData("\tmin")]
    public void Ctor_EmptyOrPaddedPairName_ThrowsArgumentException(string name)
    {
        // Arrange
        var parameters = new List<KeyValuePair<string, object?>> { new(name, 3) };

        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", parameters));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        var argumentException = exception as ArgumentException;
        argumentException.ShouldNotBeNull();
        argumentException.ParamName.ShouldBe("parameters");
    }

    [Fact]
    public void Ctor_PaddedName_MessageShowsName()
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", (" min", 3)));

        // Assert
        exception.ShouldNotBeNull();
        exception.Message.ShouldContain("‘ min’");
    }

    [Fact]
    public void Ctor_NameWithWhiteSpaceInside_BuildsParameters()
    {
        // Act
        var sut = new TemplatedMessage("errors.length", ("min length", 3));

        // Assert
        sut.Parameters.ShouldNotBeNull();
        sut.Parameters["min length"].ShouldBe(3);
    }

    [Theory]
    [InlineData(" errors.length")]
    [InlineData("errors.length ")]
    [InlineData("\terrors.length")]
    public void Ctor_PaddedTemplateId_ThrowsArgumentException(string templateId)
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage(templateId, ("min", 3)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        var argumentException = exception as ArgumentException;
        argumentException.ShouldNotBeNull();
        argumentException.ParamName.ShouldBe("templateId");
    }

    [Theory]
    [InlineData(" errors.length")]
    [InlineData("errors.length ")]
    [InlineData("\terrors.length")]
    public void Ctor_PaddedTemplateIdWithPairs_ThrowsArgumentException(string templateId)
    {
        // Arrange
        var parameters = new List<KeyValuePair<string, object?>> { new("min", 3) };

        // Act
        var exception = Record.Exception(() => new TemplatedMessage(templateId, parameters));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        var argumentException = exception as ArgumentException;
        argumentException.ShouldNotBeNull();
        argumentException.ParamName.ShouldBe("templateId");
    }

    [Fact]
    public void Ctor_PaddedTemplateId_MessageShowsTemplateId()
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length "));

        // Assert
        exception.ShouldNotBeNull();
        exception.Message.ShouldContain("‘errors.length ’");
    }

    #endregion
}
