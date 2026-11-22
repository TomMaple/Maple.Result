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

public class HttpErrorTitlesUnitTests
{
    #region titles

    // Each title is the reason phrase of the default status code of its category (RFC 9110, section 15; RFC 6585 for 429).
    [Theory]
    [InlineData(nameof(HttpErrorTitles.Validation), HttpErrorTitles.Validation, "Bad Request")]
    [InlineData(nameof(HttpErrorTitles.Unauthenticated), HttpErrorTitles.Unauthenticated, "Unauthorized")]
    [InlineData(nameof(HttpErrorTitles.PermissionDenied), HttpErrorTitles.PermissionDenied, "Forbidden")]
    [InlineData(nameof(HttpErrorTitles.NotFound), HttpErrorTitles.NotFound, "Not Found")]
    [InlineData(nameof(HttpErrorTitles.Timeout), HttpErrorTitles.Timeout, "Request Timeout")]
    [InlineData(nameof(HttpErrorTitles.Conflict), HttpErrorTitles.Conflict, "Conflict")]
    [InlineData(nameof(HttpErrorTitles.Failure), HttpErrorTitles.Failure, "Unprocessable Content")]
    [InlineData(nameof(HttpErrorTitles.LimitExceeded), HttpErrorTitles.LimitExceeded, "Too Many Requests")]
    [InlineData(nameof(HttpErrorTitles.Critical), HttpErrorTitles.Critical, "Internal Server Error")]
    [InlineData(nameof(HttpErrorTitles.NotImplemented), HttpErrorTitles.NotImplemented, "Not Implemented")]
    [InlineData(nameof(HttpErrorTitles.Unavailable), HttpErrorTitles.Unavailable, "Service Unavailable")]
    public void Title_Category_IsThePhraseOfTheDefaultStatusCode(string constant, string title, string expected)
    {
        // Act & Assert
        title.ShouldBe(expected, constant);
    }

    #endregion
}
