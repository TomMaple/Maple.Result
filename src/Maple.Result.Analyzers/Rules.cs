// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Microsoft.CodeAnalysis;

namespace Maple.Results.Analyzers;

/// <summary>
///     The identifiers, the descriptors and the property names of the rules that the analyzers of this assembly report.
/// </summary>
public static class Rules
{
    #region identifiers

    /// <summary>MR0001: a task becomes the value of a result.</summary>
    public const string InferredResultValueId = "MR0001";

    /// <summary>MR0002: a task is not awaited by a side effect method.</summary>
    public const string UnawaitedTaskId = "MR0002";

    /// <summary>MR0003: a task, a result or an <c>Error</c> is stored as the value of a result.</summary>
    public const string ConvertedResultValueId = "MR0003";

    /// <summary>MR0004: a side effect method drops the result of its function.</summary>
    public const string DroppedResultId = "MR0004";

    /// <summary>MR0005: <c>IfSuccess</c> or <c>IfError</c> ignores the value of its function.</summary>
    public const string IgnoredValueId = "MR0005";

    /// <summary>MR0006: a result is discarded.</summary>
    public const string DiscardedResultId = "MR0006";

    #endregion

    #region properties

    /// <summary>
    ///     The name of the diagnostic property that holds the method name a code fix renames the call to (e.g., <c>ThenAsync</c>).
    ///     The property is absent when no rename fixes the call.
    /// </summary>
    public const string RenameToProperty = "RenameTo";

    #endregion

    #region descriptors

    private const string Category = "Usage";

    private const string HelpLinkBase = "https://github.com/TomMaple/Maple.Result/blob/main/docs/Analyzers/";

    // The rules that replace the compile errors of the former overload traps cannot be suppressed or have their severity changed by
    // NoWarn, .editorconfig or #pragma (as CS0619 could not); [SuppressMessage] and turning analyzers off still silence them.
    private static readonly string[] ErrorTags = [WellKnownDiagnosticTags.NotConfigurable];

    internal static readonly DiagnosticDescriptor ConvertedResultValue = new(
        ConvertedResultValueId,
        "A task, a result or an Error is stored as the value of a result",
        "A {0} is converted to a successful {1} that holds it as its value",
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The implicit conversion from a value to Result<T> was applied to a task, a result or an Error, because T is object or one of their base "
                     + "types. The result is successful and holds the task, the result or the Error as its value.",
        helpLinkUri: HelpLinkBase + ConvertedResultValueId + ".md",
        customTags: ErrorTags);

    internal static readonly DiagnosticDescriptor DiscardedResult = new(
        DiscardedResultId,
        "A result is discarded",
        "The {0} returned by {1} is not used, so a failure is lost. Check it, return it, or discard it with ‘_ =’.",
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "A statement produces a result, or a task of a result, and does nothing with it. If the operation failed, nobody notices.",
        helpLinkUri: HelpLinkBase + DiscardedResultId + ".md");

    internal static readonly DiagnosticDescriptor DroppedResult = new(
        DroppedResultId,
        "A side effect method drops the result of its function",
        "{0} ignores the {1} that this function returns, so a failure is lost. {2}.",
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "IfSuccess, IfError, Switch and their asynchronous forms run a function for its side effect and ignore what it returns. "
                     + "A function that returns a result or an Error is therefore never checked.",
        helpLinkUri: HelpLinkBase + DroppedResultId + ".md",
        customTags: ErrorTags);

    internal static readonly DiagnosticDescriptor IgnoredValue = new(
        IgnoredValueId,
        "IfSuccess or IfError ignores the value of its function",
        "{0} runs this function as a side effect and keeps the original result; in version 2 it transformed the result. "
        + "Use {1} to continue with the returned value.",
        Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Since version 3, IfSuccess and IfError only run side effects. A function that returns a value of the type of the result’s value, "
                     + "in a call whose result is used, is what version 2 code wrote to transform the result.",
        helpLinkUri: HelpLinkBase + IgnoredValueId + ".md");

    internal static readonly DiagnosticDescriptor InferredResultValue = new(
        InferredResultValueId,
        "A task becomes the value of a result",
        "The function passed to {0} returns a {1}, {2}. {3}.",
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The method stores what the function returns as the value of the result. A task stored this way is never awaited. "
                     + "To keep it on purpose, name the type arguments or wrap it with Result.FromValue.",
        helpLinkUri: HelpLinkBase + InferredResultValueId + ".md",
        customTags: ErrorTags);

    internal static readonly DiagnosticDescriptor UnawaitedTask = new(
        UnawaitedTaskId,
        "A task is not awaited by a side effect method",
        "{0} takes an action here, so {1} is not awaited. {2}.",
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "A function that returns a task, or an async lambda, was converted to a delegate that returns nothing. "
                     + "The task is discarded: the method returns before it completes, and its exception is lost.",
        helpLinkUri: HelpLinkBase + UnawaitedTaskId + ".md",
        customTags: ErrorTags);

    #endregion
}
