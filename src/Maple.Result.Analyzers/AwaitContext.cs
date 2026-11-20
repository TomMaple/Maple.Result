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
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Maple.Results.Analyzers;

/// <summary>
///     Where an <see langword="await" /> is allowed: the checks that MR0006 and the code fixes share, so that a diagnostic offers an
///     await exactly where the code fix can write one.
/// </summary>
internal static class AwaitContext
{
    /// <summary>
    ///     Returns the first node around the <paramref name="node" /> that decides whether an await is possible there: a statement or a
    ///     clause that forbids it (a <see langword="lock" />, <see langword="unsafe" /> or <see langword="fixed" /> statement, a query, a
    ///     catch filter), the function that contains the node, or the member. Every node of a member is inside one of them.
    /// </summary>
    public static SyntaxNode Container(SyntaxNode node)
        => node.Ancestors().First(IsContainer);

    /// <summary>
    ///     Whether the <paramref name="node" /> is in an unsafe context: an unsafe member, local function or type around it, where an await
    ///     is not allowed.
    /// </summary>
    public static bool IsUnsafe(SyntaxNode node)
        => node.AncestorsAndSelf().Any(static ancestor => ancestor switch
        {
            MemberDeclarationSyntax member => member.Modifiers.Any(SyntaxKind.UnsafeKeyword),
            LocalFunctionStatementSyntax localFunction => localFunction.Modifiers.Any(SyntaxKind.UnsafeKeyword),
            _ => false,
        });

    #region helper methods

    private static bool IsContainer(SyntaxNode node)
        => node is LockStatementSyntax or UnsafeStatementSyntax or FixedStatementSyntax or QueryExpressionSyntax or CatchFilterClauseSyntax
            or AnonymousFunctionExpressionSyntax or LocalFunctionStatementSyntax or MemberDeclarationSyntax or AccessorDeclarationSyntax;

    #endregion
}
