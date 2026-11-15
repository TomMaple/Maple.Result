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

namespace Maple.Results.Tests.Unit;

public class ProblemDetailsMembersUnitTests
{
    #region member names

    // The constant name is a parameter only to tell the rows apart, because some members share a value (e.g., "detail").
    [Theory]
    [InlineData(nameof(ProblemDetailsMembers.Type), ProblemDetailsMembers.Type, "type")]
    [InlineData(nameof(ProblemDetailsMembers.Title), ProblemDetailsMembers.Title, "title")]
    [InlineData(nameof(ProblemDetailsMembers.Status), ProblemDetailsMembers.Status, "status")]
    [InlineData(nameof(ProblemDetailsMembers.Detail), ProblemDetailsMembers.Detail, "detail")]
    [InlineData(nameof(ProblemDetailsMembers.Instance), ProblemDetailsMembers.Instance, "instance")]
    [InlineData(nameof(ProblemDetailsMembers.Errors), ProblemDetailsMembers.Errors, "errors")]
    [InlineData(nameof(ProblemDetailsMembers.DetailTemplated), ProblemDetailsMembers.DetailTemplated, "detailTemplated")]
    [InlineData(nameof(ProblemDetailsMembers.ErrorPointer), ProblemDetailsMembers.ErrorPointer, "pointer")]
    [InlineData(nameof(ProblemDetailsMembers.ErrorDetail), ProblemDetailsMembers.ErrorDetail, "detail")]
    [InlineData(nameof(ProblemDetailsMembers.ErrorDetailTemplated), ProblemDetailsMembers.ErrorDetailTemplated, "detailTemplated")]
    [InlineData(nameof(ProblemDetailsMembers.TemplateId), ProblemDetailsMembers.TemplateId, "templateId")]
    [InlineData(nameof(ProblemDetailsMembers.TemplateParams), ProblemDetailsMembers.TemplateParams, "params")]
    public void Member_Name_IsTheNameOfTheFormat(string constant, string member, string expected)
    {
        // Act & Assert
        member.ShouldBe(expected, constant);
    }

    #endregion

    #region reserved metadata keys

    // Every top-level member of the format is a reserved metadata key, so that a metadata entry never collides with it.
    [Theory]
    [InlineData(ProblemDetailsMembers.Type)]
    [InlineData(ProblemDetailsMembers.Title)]
    [InlineData(ProblemDetailsMembers.Status)]
    [InlineData(ProblemDetailsMembers.Detail)]
    [InlineData(ProblemDetailsMembers.Instance)]
    [InlineData(ProblemDetailsMembers.Errors)]
    [InlineData(ProblemDetailsMembers.DetailTemplated)]
    public void WithMetadata_TopLevelMemberAsKey_ThrowsArgumentException(string member)
    {
        // Arrange
        var sut = Error.Validation(ErrorUri.None, "Test title");

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(member, 1));

        // Assert
        exception.ShouldBeOfType<ArgumentException>().ParamName.ShouldBe("key");
    }

    // The names inside an "errors" item or a "detailTemplated" object are not top-level members, so they are valid keys.
    [Theory]
    [InlineData(ProblemDetailsMembers.ErrorPointer)]
    [InlineData(ProblemDetailsMembers.TemplateId)]
    [InlineData(ProblemDetailsMembers.TemplateParams)]
    public void WithMetadata_NestedMemberAsKey_AddsTheEntry(string member)
    {
        // Arrange
        var sut = Error.Validation(ErrorUri.None, "Test title");

        // Act
        var result = sut.WithMetadata(member, 1);

        // Assert
        result.Metadata.ShouldContainKey(member);
    }

    #endregion
}
