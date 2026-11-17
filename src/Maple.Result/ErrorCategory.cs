// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Maple.Results;

/// <summary>
///     An enumeration representing the different categories of errors that can be returned by the Result record.
/// </summary>
/// <remarks>
///     <para>
///         The members are ordered by the HTTP status code they can be mapped to.
///         The values are explicit and must not change: they are stored and serialized (e.g., as numbers in JSON).
///     </para>
///     <para>
///         The default value (<c>default(ErrorCategory)</c>, <c>0</c>) is <see cref="Validation" />, a real category: do not use it to mean
///         "not set". A category that is missing, e.g., from a JSON payload, has to be handled as missing by the code that reads it, not
///         read as <see cref="Validation" />.
///     </para>
///     <para>
///         The status codes are the default mapping of the packages that write and read problem details; both directions can be customized.
///     </para>
/// </remarks>
public enum ErrorCategory
{
    /// <summary>
    ///     A validation error.
    /// </summary>
    /// <remarks>
    ///     The operation failed due to something wrong with the input data.<br />
    ///     Can be mapped to the 400 HTTP status code (Bad Request); the 413 (Content Too Large), 414 (URI Too Long) and 431 (Request Header
    ///     Fields Too Large) status codes are read as this category.<br />
    ///     It is the default value of the enumeration (<c>0</c>); do not use the default value to mean "not set".
    /// </remarks>
    Validation = 0,

    /// <summary>
    ///     The action is not authenticated.
    /// </summary>
    /// <remarks>
    ///     The authentication is either missing or invalid.<br />
    ///     Can be mapped to the 401 HTTP status code (Unauthorized).
    /// </remarks>
    Unauthenticated = 1,

    /// <summary>
    ///     The permission for the action is denied.
    /// </summary>
    /// <remarks>
    ///     The authentication might be valid, but the identity does not have the required permissions.<br />
    ///     Can be mapped to the 403 HTTP status code (Forbidden).
    /// </remarks>
    PermissionDenied = 2,

    /// <summary>
    ///     The resource was not found error.
    /// </summary>
    /// <remarks>
    ///     Can be mapped to the 404 HTTP status code (Not Found); the 410 status code (Gone) is read as this category.
    /// </remarks>
    NotFound = 3,

    /// <summary>
    ///     A timeout error.
    /// </summary>
    /// <remarks>
    ///     The operation timed out.<br />
    ///     Can be mapped to the 408 HTTP status code (Request Timeout) when the client did not send the complete request in time,
    ///     or to the 504 HTTP status code (Gateway Timeout) when an upstream server or a dependency did not respond in time.
    /// </remarks>
    Timeout = 4,

    /// <summary>
    ///     A conflict error.
    /// </summary>
    /// <remarks>
    ///     The operation failed due to the data or state conflict (such as a unique constraint violation).<br />
    ///     Can be mapped to the 409 HTTP status code (Conflict); the 412 (Precondition Failed) and 423 (Locked) status codes are read as this
    ///     category.
    /// </remarks>
    Conflict = 5,

    /// <summary>
    ///     An expected error that is not critical to the operation.
    /// </summary>
    /// <remarks>
    ///     Do not use for authentication, authorization, validation, data conflict or unexpected errors.<br />
    ///     Can be mapped to the 422 HTTP status code (Unprocessable Content); a 4xx status code that no other category is read from is read as
    ///     this category.
    /// </remarks>
    Failure = 6,

    /// <summary>
    ///     A usage limit has been exceeded.
    /// </summary>
    /// <remarks>
    ///     The request is valid and permitted, but a usage limit, quota or rate has been exhausted. It may succeed later or once the limit is raised.<br />
    ///     Applies to business limits as well as throttling.<br />
    ///     Do not use for input-size checks (use <see cref="Validation" />).<br />
    ///     Can be mapped to the 429 HTTP status code (Too Many Requests).
    /// </remarks>
    LimitExceeded = 10,

    /// <summary>
    ///     An unexpected, critical error.
    /// </summary>
    /// <remarks>
    ///     Can be mapped to the 500 HTTP status code (Internal Server Error); a 5xx status code that no other category is read from (e.g., 505
    ///     or 507) is read as this category.
    /// </remarks>
    Critical = 7,

    /// <summary>
    ///     The not implemented error.
    /// </summary>
    /// <remarks>
    ///     The operation, feature or case is not implemented.<br />
    ///     Can be mapped to the 501 HTTP status code (Not Implemented).
    /// </remarks>
    NotImplemented = 8,

    /// <summary>
    ///     The service is not available.
    /// </summary>
    /// <remarks>
    ///     The service (or a resource it depends on) is temporarily unavailable, e.g. it is down or overloaded. Try again later.<br />
    ///     Do not use when a usage limit, quota or rate has been exhausted (use <see cref="LimitExceeded" />).<br />
    ///     Can be mapped to the 503 HTTP status code (Service Unavailable); the 502 status code (Bad Gateway) is read as this category.
    /// </remarks>
    Unavailable = 9
}
