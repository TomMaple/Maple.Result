// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Maple.Results.Http;

/// <summary>
///     The reason phrases of the HTTP status codes that the <see cref="ErrorCategory" /> values map to by default, for the
///     <see cref="Error.Title" /> of an error whose type has no semantics of its own.
/// </summary>
/// <remarks>
///     <para>
///         <i>RFC 9457</i> (section 4.2.1): when the type is <c>about:blank</c> (<see cref="ErrorUri.None" />), the title should be the
///         reason phrase of the HTTP status code (e.g., “Not Found” for 404). The same pair is used with the type <i>URI</i>s of
///         <see cref="HttpErrorUris" />, which name the section of the status code.
///     </para>
///     <para>
///         Each phrase is the one of the default status code of its category (see
///         <see href="https://github.com/TomMaple/Maple.Result/blob/main/docs/problem-details.md#category-and-status" />). A package
///         that writes problem details with a customized mapping writes another status code; the title has to match that code.
///     </para>
///     <para>An error with a title of its own (e.g., “The account has not been found.”) needs a type of its own too.</para>
/// </remarks>
public static class HttpErrorTitles
{
    /// <summary>
    ///     “Conflict”, the phrase of 409, the default status code of <see cref="ErrorCategory.Conflict" /> (<i>RFC 9110</i>, section 15.5.10).
    /// </summary>
    public const string Conflict = "Conflict";

    /// <summary>
    ///     “Internal Server Error”, the phrase of 500, the default status code of <see cref="ErrorCategory.Critical" /> (<i>RFC 9110</i>, section 15.6.1).
    /// </summary>
    public const string Critical = "Internal Server Error";

    /// <summary>
    ///     “Unprocessable Content”, the phrase of 422, the default status code of <see cref="ErrorCategory.Failure" /> (<i>RFC 9110</i>, section 15.5.21).
    /// </summary>
    public const string Failure = "Unprocessable Content";

    /// <summary>
    ///     “Too Many Requests”, the phrase of 429, the default status code of <see cref="ErrorCategory.LimitExceeded" /> (<i>RFC 6585</i>, section 4).
    /// </summary>
    public const string LimitExceeded = "Too Many Requests";

    /// <summary>
    ///     “Not Found”, the phrase of 404, the default status code of <see cref="ErrorCategory.NotFound" /> (<i>RFC 9110</i>, section 15.5.5).
    /// </summary>
    public const string NotFound = "Not Found";

    /// <summary>
    ///     “Not Implemented”, the phrase of 501, the default status code of <see cref="ErrorCategory.NotImplemented" /> (<i>RFC 9110</i>, section 15.6.2).
    /// </summary>
    public const string NotImplemented = "Not Implemented";

    /// <summary>
    ///     “Forbidden”, the phrase of 403, the default status code of <see cref="ErrorCategory.PermissionDenied" /> (<i>RFC 9110</i>, section 15.5.4).
    /// </summary>
    public const string PermissionDenied = "Forbidden";

    /// <summary>
    ///     “Request Timeout”, the phrase of 408, the default status code of <see cref="ErrorCategory.Timeout" /> (<i>RFC 9110</i>, section 15.5.9).
    /// </summary>
    public const string Timeout = "Request Timeout";

    /// <summary>
    ///     “Unauthorized”, the phrase of 401, the default status code of <see cref="ErrorCategory.Unauthenticated" /> (<i>RFC 9110</i>, section 15.5.2).
    /// </summary>
    public const string Unauthenticated = "Unauthorized";

    /// <summary>
    ///     “Service Unavailable”, the phrase of 503, the default status code of <see cref="ErrorCategory.Unavailable" /> (<i>RFC 9110</i>, section 15.6.4).
    /// </summary>
    public const string Unavailable = "Service Unavailable";

    /// <summary>
    ///     “Bad Request”, the phrase of 400, the default status code of <see cref="ErrorCategory.Validation" /> (<i>RFC 9110</i>, section 15.5.1).
    /// </summary>
    public const string Validation = "Bad Request";
}
