// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Http;

namespace Maple.Results.Tests.Unit.Http;

public class HttpErrorUrisUnitTests
{
    #region type URIs

    public static TheoryData<string, ErrorUri, string> TypeUris
        => new()
        {
            { nameof(HttpErrorUris.Validation), HttpErrorUris.Validation, "https://www.rfc-editor.org/rfc/rfc9110#section-15.5.1" },
            { nameof(HttpErrorUris.Unauthenticated), HttpErrorUris.Unauthenticated, "https://www.rfc-editor.org/rfc/rfc9110#section-15.5.2" },
            { nameof(HttpErrorUris.PermissionDenied), HttpErrorUris.PermissionDenied, "https://www.rfc-editor.org/rfc/rfc9110#section-15.5.4" },
            { nameof(HttpErrorUris.NotFound), HttpErrorUris.NotFound, "https://www.rfc-editor.org/rfc/rfc9110#section-15.5.5" },
            { nameof(HttpErrorUris.Timeout), HttpErrorUris.Timeout, "https://www.rfc-editor.org/rfc/rfc9110#section-15.5.9" },
            { nameof(HttpErrorUris.Conflict), HttpErrorUris.Conflict, "https://www.rfc-editor.org/rfc/rfc9110#section-15.5.10" },
            { nameof(HttpErrorUris.Failure), HttpErrorUris.Failure, "https://www.rfc-editor.org/rfc/rfc9110#section-15.5.21" },
            { nameof(HttpErrorUris.LimitExceeded), HttpErrorUris.LimitExceeded, "https://www.rfc-editor.org/rfc/rfc6585#section-4" },
            { nameof(HttpErrorUris.Critical), HttpErrorUris.Critical, "https://www.rfc-editor.org/rfc/rfc9110#section-15.6.1" },
            { nameof(HttpErrorUris.NotImplemented), HttpErrorUris.NotImplemented, "https://www.rfc-editor.org/rfc/rfc9110#section-15.6.2" },
            { nameof(HttpErrorUris.Unavailable), HttpErrorUris.Unavailable, "https://www.rfc-editor.org/rfc/rfc9110#section-15.6.4" },
        };

    // Each type is the section of the default status code of its category (RFC 9110, section 15; RFC 6585 for 429).
    [Theory]
    [MemberData(nameof(TypeUris))]
    public void TypeUri_Category_IsTheSectionOfTheDefaultStatusCode(string property, ErrorUri typeUri, string expected)
    {
        // Act & Assert
        typeUri.Value.ShouldBe(expected, property);
    }

    // The value parses as the same URI Locator, so a type read back from problem details equals the property.
    [Theory]
    [MemberData(nameof(TypeUris))]
    public void TypeUri_Category_EqualsTheParsedValue(string property, ErrorUri typeUri, string expected)
    {
        // Act
        var result = ErrorUri.Parse(expected);

        // Assert
        result.ShouldBe(typeUri, property);
    }

    #endregion
}
