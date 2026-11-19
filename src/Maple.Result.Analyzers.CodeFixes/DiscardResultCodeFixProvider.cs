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
using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace Maple.Results.Analyzers.CodeFixes;

/// <summary>
///     Fixes MR0006 by discarding the result explicitly: <c>Save();</c> becomes <c>_ = Save();</c>.
/// </summary>
/// <remarks>
///     The fix only states that the result is ignored on purpose. Checking the result, or returning it, is the better fix where a failure
///     matters.
/// </remarks>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DiscardResultCodeFixProvider))]
[Shared]
public sealed class DiscardResultCodeFixProvider : CodeFixProvider
{
    private const string Title = "Discard the result";

    /// <inheritdoc />
    public override ImmutableArray<string> FixableDiagnosticIds { get; } = [Rules.DiscardedResultId];

    /// <inheritdoc />
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    /// <inheritdoc />
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        // A C# document always has a syntax root.
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        foreach (var diagnostic in context.Diagnostics)
        {
            var expression = root!.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true) as ExpressionSyntax;

            if (expression is null)
                continue;

            var action = CodeAction.Create(
                Title,
                cancellationToken => DiscardAsync(context.Document, expression, cancellationToken),
                equivalenceKey: Title);
            context.RegisterCodeFix(action, diagnostic);
        }
    }

    private static async Task<Document> DiscardAsync(Document document, ExpressionSyntax expression, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        // The discard takes the leading trivia (indentation, comments) of the expression that it now precedes. The tokens get their
        // trivia explicitly: the elastic trivia of the factory defaults would let the formatter move the statement to a new line.
        var space = SyntaxFactory.TriviaList(SyntaxFactory.Space);
        var underscore = SyntaxFactory.Identifier(expression.GetLeadingTrivia(), "_", space);
        var discard = SyntaxFactory.IdentifierName(underscore);
        var equals = SyntaxFactory.Token(SyntaxFactory.TriviaList(), SyntaxKind.EqualsToken, space);
        var value = expression.WithoutLeadingTrivia();
        var assignment = SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression, discard, equals, value);
        var newRoot = root!.ReplaceNode(expression, assignment);

        return document.WithSyntaxRoot(newRoot);
    }
}
