// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Collections.Generic;
using Sut = Maple.Results.TemplatedMessage;

namespace Maple.Results.Tests.Unit.TemplatedMessageTests;

public class TemplatedMessageEqualityUnitTests
{
    #region value equality

    [Fact]
    public void Equals_TwoMessagesWithoutParameters_AreEqual()
    {
        // Arrange
        var first = new Sut("template.id");
        var second = new Sut("template.id");

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_TwoMessagesWithEqualParameters_AreEqual()
    {
        // Arrange
        var first = new Sut("template.id", new Dictionary<string, object?>
        {
            ["userName"] = "johnny",
            ["bonusPoints"] = 100
        });
        var second = new Sut("template.id", new Dictionary<string, object?>
        {
            ["userName"] = "johnny",
            ["bonusPoints"] = 100
        });

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_MessagesWithParametersInDifferentOrder_AreEqual()
    {
        // Arrange
        var first = new Sut("template.id", new Dictionary<string, object?>
        {
            ["userName"] = "johnny",
            ["bonusPoints"] = 100
        });
        var second = new Sut("template.id", new Dictionary<string, object?>
        {
            ["bonusPoints"] = 100,
            ["userName"] = "johnny"
        });

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_MessagesWithDifferentParamValues_AreNotEqual()
    {
        // Arrange
        var first = new Sut("template.id", new Dictionary<string, object?> { ["userName"] = "johnny" });
        var second = new Sut("template.id", new Dictionary<string, object?> { ["userName"] = "jane" });

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_MessagesWithDifferentParamKeys_AreNotEqual()
    {
        // Arrange
        var first = new Sut("template.id", new Dictionary<string, object?> { ["userName"] = "johnny" });
        var second = new Sut("template.id", new Dictionary<string, object?> { ["userId"] = "johnny" });

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    // Lookups ignore the case, but the spelling is serialized and shown by ToString, so it counts for equality (#349).
    [Fact]
    public void Equals_MessagesWithParamNamesDifferingOnlyInCase_AreNotEqual()
    {
        // Arrange
        var first = new Sut("template.id", ("userName", "johnny"));
        var second = new Sut("template.id", ("UserName", "johnny"));

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_MessagesWithDifferentParamCount_AreNotEqual()
    {
        // Arrange
        var first = new Sut("template.id", new Dictionary<string, object?> { ["userName"] = "johnny" });
        var second = new Sut("template.id", new Dictionary<string, object?>
        {
            ["userName"] = "johnny",
            ["bonusPoints"] = 100
        });

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_MessagesWithDifferentTemplateId_AreNotEqual()
    {
        // Arrange
        var first = new Sut("template.first");
        var second = new Sut("template.second");

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_MessageWithoutParametersVsEmptyParameters_AreEqual()
    {
        // Arrange
        var withoutParameters = new Sut("template.id");
        var withEmptyParameters = new Sut("template.id", new Dictionary<string, object?>());

        // Act & Assert
        withoutParameters.Equals(withEmptyParameters).ShouldBeTrue();
        (withoutParameters == withEmptyParameters).ShouldBeTrue();
        withoutParameters.GetHashCode().ShouldBe(withEmptyParameters.GetHashCode());
    }

    #endregion

    #region read-only collection

    [Fact]
    public void Parameters_ExposedAsReadOnly()
    {
        // Arrange
        var sut = new Sut("template.id", new Dictionary<string, object?> { ["userName"] = "johnny" });

        // Act & Assert
        sut.Parameters.ShouldBeAssignableTo<IReadOnlyDictionary<string, object?>>();
    }

    #endregion
}
