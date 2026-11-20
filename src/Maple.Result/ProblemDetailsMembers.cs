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
///     The member names of the problem details object (<i>RFC 9457</i>) that represents an <see cref="Error" />.
/// </summary>
/// <remarks>
///     <para>
///         The packages that write and read problem details share these names, so that an <see cref="Error" /> written by one service
///         is read back the same way by another. The format is described in
///         <see href="https://github.com/TomMaple/Maple.Result/blob/main/docs/problem-details.md" />.
///     </para>
///     <para>
///         The top-level members (<see cref="Type" />, <see cref="Title" />, <see cref="Status" />, <see cref="Detail" />,
///         <see cref="Instance" />, <see cref="Errors" /> and <see cref="DetailTemplated" />) are reserved: they cannot be used
///         (case-insensitively) as <see cref="Error.Metadata" /> keys, because each metadata entry becomes a top-level extension member.
///         The names inside an <see cref="Errors" /> item or a <see cref="DetailTemplated" /> object are not reserved.
///     </para>
/// </remarks>
public static class ProblemDetailsMembers
{
    /// <summary>
    ///     The <c>type</c> member: the <see cref="Error.TypeUri" /> URI reference that identifies the problem type (<i>RFC 9457</i>, section 3.1.1).
    /// </summary>
    public const string Type = "type";

    /// <summary>
    ///     The <c>title</c> member: the <see cref="Error.Title" />, a short summary of the problem type (<i>RFC 9457</i>, section 3.1.3).
    /// </summary>
    public const string Title = "title";

    /// <summary>
    ///     The <c>status</c> member: the HTTP status code mapped from the <see cref="Error.Category" /> (<i>RFC 9457</i>, section 3.1.2).
    /// </summary>
    public const string Status = "status";

    /// <summary>
    ///     The <c>detail</c> member: the <see cref="Error.Detail" />, an explanation of this occurrence (<i>RFC 9457</i>, section 3.1.4).
    /// </summary>
    public const string Detail = "detail";

    /// <summary>
    ///     The <c>instance</c> member: the <see cref="Error.InstanceUri" /> URI reference of this occurrence (<i>RFC 9457</i>, section 3.1.5).
    /// </summary>
    public const string Instance = "instance";

    /// <summary>
    ///     The <c>errors</c> extension member: an array with one object per <see cref="Error.ErrorDetails" /> item.
    /// </summary>
    /// <remarks>
    ///     Each item has the <see cref="ErrorPointer" />, <see cref="ErrorDetail" /> and <see cref="ErrorDetailTemplated" /> members.
    /// </remarks>
    public const string Errors = "errors";

    /// <summary>
    ///     The <c>detailTemplated</c> extension member: the <see cref="Error.DetailTemplated" /> object.
    /// </summary>
    /// <remarks>
    ///     The object has the <see cref="TemplateId" /> and <see cref="TemplateParams" /> members.
    /// </remarks>
    public const string DetailTemplated = "detailTemplated";

    /// <summary>
    ///     The <c>pointer</c> member of an <see cref="Errors" /> item: the <see cref="Maple.Results.ErrorDetail.PropertyPointer" />.
    /// </summary>
    public const string ErrorPointer = "pointer";

    /// <summary>
    ///     The <c>detail</c> member of an <see cref="Errors" /> item: the <see cref="Maple.Results.ErrorDetail.Detail" />.
    /// </summary>
    public const string ErrorDetail = "detail";

    /// <summary>
    ///     The <c>detailTemplated</c> member of an <see cref="Errors" /> item: the <see cref="Maple.Results.ErrorDetail.DetailTemplated" />
    ///     object, with the <see cref="TemplateId" /> and <see cref="TemplateParams" /> members.
    /// </summary>
    public const string ErrorDetailTemplated = "detailTemplated";

    /// <summary>
    ///     The <c>templateId</c> member of a templated message: the <see cref="TemplatedMessage.TemplateId" />.
    /// </summary>
    public const string TemplateId = "templateId";

    /// <summary>
    ///     The <c>params</c> member of a templated message: the <see cref="TemplatedMessage.Parameters" />, omitted when there are none.
    /// </summary>
    public const string TemplateParams = "params";
}
