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
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace Maple.Results.Analyzers.CodeFixes;

/// <summary>
///     Fixes MR0001, MR0002, MR0004 and MR0005 by calling the method that the diagnostic names instead: the <c>Async</c> form of a
///     synchronous method for a function that returns a task (<c>Then</c> to <c>ThenAsync</c>), or the transforming method for a function
///     whose value should be kept (<c>IfSuccess</c> to <c>Then</c>, <c>IfError</c> to <c>Else</c>).
/// </summary>
/// <remarks>
///     <para>
///         A call that becomes asynchronous returns a task. The fix awaits it, and makes the containing method, local function or
///         anonymous function <see langword="async" /> when that function returns a task and is not asynchronous yet, as the code fixes
///         of Microsoft for the same change do (see <see cref="AwaitedCall" />). Where the call cannot be awaited, only the name changes.
///     </para>
///     <para>
///         No fix is offered for a diagnostic without the <see cref="Rules.RenameToProperty" /> property.
///     </para>
/// </remarks>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RenameMethodCodeFixProvider))]
[Shared]
public sealed class RenameMethodCodeFixProvider : CodeFixProvider
{
    private const string AsyncSuffix = "Async";

    /// <inheritdoc />
    public override ImmutableArray<string> FixableDiagnosticIds { get; } =
        [Rules.InferredResultValueId, Rules.UnawaitedTaskId, Rules.DroppedResultId, Rules.IgnoredValueId];

    /// <inheritdoc />
    public override FixAllProvider GetFixAllProvider()
        => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc />
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        // A C# document always has a syntax root and a semantic model.
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var model = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);

        foreach (var diagnostic in context.Diagnostics)
        {
            if (!diagnostic.Properties.TryGetValue(Rules.RenameToProperty, out var newName) || newName is null)
                continue;

            // MR0001 marks the method name itself; the other rules mark the function and give the name as an additional location.
            var nameLocation = diagnostic.AdditionalLocations.Count > 0
                ? diagnostic.AdditionalLocations[0]
                : diagnostic.Location;
            var name = root!.FindNode(nameLocation.SourceSpan, getInnermostNodeForTie: true) as SimpleNameSyntax;

            if (name is null)
                continue;

            // A synchronous method renamed to its asynchronous form returns a task: it is awaited where that is possible.
            var becomesAsynchronous = newName.EndsWith(AsyncSuffix, StringComparison.Ordinal)
                                      && !name.Identifier.ValueText.EndsWith(AsyncSuffix, StringComparison.Ordinal);
            var awaitedCall = becomesAsynchronous
                ? AwaitedCall.TryCreate(name, model!, context.CancellationToken)
                : null;
            var title = awaitedCall is null
                ? "Use " + newName
                : "Use " + newName + " and await it";
            var action = CodeAction.Create(
                title,
                cancellationToken => awaitedCall is null
                    ? RenameAsync(context.Document, name, newName, cancellationToken)
                    : Task.FromResult(context.Document.WithSyntaxRoot(awaitedCall.Apply(root, newName))),
                equivalenceKey: title);
            context.RegisterCodeFix(action, diagnostic);
        }
    }

    private static async Task<Document> RenameAsync(Document document, SimpleNameSyntax name, string newName, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        // Only the identifier is replaced, so that type arguments (IfSuccess<int>) and trivia stay.
        var newIdentifier = SyntaxFactory.Identifier(newName).WithTriviaFrom(name.Identifier);
        var newRoot = root!.ReplaceToken(name.Identifier, newIdentifier);

        return document.WithSyntaxRoot(newRoot);
    }
}
