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
///     The type <i>URI</i>s of the HTTP status codes that the <see cref="ErrorCategory" /> values map to by default: <i>URI Locator</i>s of
///     the section of the specification that defines each status code.
/// </summary>
/// <remarks>
///     <para>
///         They are for an error that has no semantics beyond its status code, but whose type should be dereferenceable rather than
///         <c>about:blank</c> (<see cref="ErrorUri.None" />). Pair each one with the title of the same name in <see cref="HttpErrorTitles" />,
///         e.g., <c>Error.NotFound(HttpErrorUris.NotFound, HttpErrorTitles.NotFound)</c>; an error with a title of its own needs a type of its own.
///     </para>
///     <para>
///         Each section is the one of the default status code of its category (see
///         <see href="https://github.com/TomMaple/Maple.Result/blob/main/docs/problem-details.md#category-and-status" />): <i>RFC 9110</i>,
///         section 15, and <i>RFC 6585</i>, section 4, for 429 (Too Many Requests).
///     </para>
/// </remarks>
public static class HttpErrorUris
{
    /// <summary>
    ///     The section of 409 (Conflict), the default status code of <see cref="ErrorCategory.Conflict" /> (<i>RFC 9110</i>, section 15.5.10).
    /// </summary>
    public static ErrorUri Conflict { get; } = ErrorUri.Locator("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.10");

    /// <summary>
    ///     The section of 500 (Internal Server Error), the default status code of <see cref="ErrorCategory.Critical" /> (<i>RFC 9110</i>, section 15.6.1).
    /// </summary>
    public static ErrorUri Critical { get; } = ErrorUri.Locator("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.1");

    /// <summary>
    ///     The section of 422 (Unprocessable Content), the default status code of <see cref="ErrorCategory.Failure" /> (<i>RFC 9110</i>, section 15.5.21).
    /// </summary>
    public static ErrorUri Failure { get; } = ErrorUri.Locator("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.21");

    /// <summary>
    ///     The section of 429 (Too Many Requests), the default status code of <see cref="ErrorCategory.LimitExceeded" /> (<i>RFC 6585</i>, section 4).
    /// </summary>
    public static ErrorUri LimitExceeded { get; } = ErrorUri.Locator("https://www.rfc-editor.org/rfc/rfc6585#section-4");

    /// <summary>
    ///     The section of 404 (Not Found), the default status code of <see cref="ErrorCategory.NotFound" /> (<i>RFC 9110</i>, section 15.5.5).
    /// </summary>
    public static ErrorUri NotFound { get; } = ErrorUri.Locator("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.5");

    /// <summary>
    ///     The section of 501 (Not Implemented), the default status code of <see cref="ErrorCategory.NotImplemented" /> (<i>RFC 9110</i>, section 15.6.2).
    /// </summary>
    public static ErrorUri NotImplemented { get; } = ErrorUri.Locator("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.2");

    /// <summary>
    ///     The section of 403 (Forbidden), the default status code of <see cref="ErrorCategory.PermissionDenied" /> (<i>RFC 9110</i>, section 15.5.4).
    /// </summary>
    public static ErrorUri PermissionDenied { get; } = ErrorUri.Locator("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.4");

    /// <summary>
    ///     The section of 408 (Request Timeout), the default status code of <see cref="ErrorCategory.Timeout" /> (<i>RFC 9110</i>, section 15.5.9).
    /// </summary>
    public static ErrorUri Timeout { get; } = ErrorUri.Locator("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.9");

    /// <summary>
    ///     The section of 401 (Unauthorized), the default status code of <see cref="ErrorCategory.Unauthenticated" /> (<i>RFC 9110</i>, section 15.5.2).
    /// </summary>
    public static ErrorUri Unauthenticated { get; } = ErrorUri.Locator("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.2");

    /// <summary>
    ///     The section of 503 (Service Unavailable), the default status code of <see cref="ErrorCategory.Unavailable" /> (<i>RFC 9110</i>, section 15.6.4).
    /// </summary>
    public static ErrorUri Unavailable { get; } = ErrorUri.Locator("https://www.rfc-editor.org/rfc/rfc9110#section-15.6.4");

    /// <summary>
    ///     The section of 400 (Bad Request), the default status code of <see cref="ErrorCategory.Validation" /> (<i>RFC 9110</i>, section 15.5.1).
    /// </summary>
    public static ErrorUri Validation { get; } = ErrorUri.Locator("https://www.rfc-editor.org/rfc/rfc9110#section-15.5.1");
}
