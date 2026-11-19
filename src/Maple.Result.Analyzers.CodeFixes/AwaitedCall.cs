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
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Maple.Results.Analyzers.CodeFixes;

/// <summary>
///     Turns a call of a synchronous method into an awaited call of its asynchronous form: <c>result.Then(…)</c> becomes
///     <c>await result.ThenAsync(…)</c>, in parentheses where the call is part of a larger expression, and the function that contains the
///     call becomes <see langword="async" /> when it is not yet.
/// </summary>
/// <remarks>
///     <para>
///         It follows the code fixes of Microsoft for the same change (VSTHRD103 of the threading analyzers, and the asynchronous assertions
///         of MSTest): the name is replaced and the call is awaited in one step; a method, a local function or an anonymous function that
///         returns a task and is not asynchronous gets the <see langword="async" /> modifier, and its <see langword="return" /> statements
///         await the task that they returned.
///     </para>
///     <para>
///         The call is not awaited, and only renamed by the caller, where an <see langword="await" /> is not possible or would change the
///         signature of the containing member: after a null-conditional access, inside a <see langword="lock" />, <see langword="unsafe" />
///         or <see langword="fixed" /> statement, a query or a catch filter, in an expression tree, and in a member that does not return a
///         task.
///     </para>
/// </remarks>
internal sealed class AwaitedCall
{
    private readonly SyntaxNode? _function;
    private readonly bool _functionReturnsValue;
    private readonly InvocationExpressionSyntax _invocation;
    private readonly SimpleNameSyntax _name;

    private AwaitedCall(SimpleNameSyntax name, InvocationExpressionSyntax invocation, SyntaxNode? function, bool functionReturnsValue)
    {
        _name = name;
        _invocation = invocation;
        _function = function;
        _functionReturnsValue = functionReturnsValue;
    }

    /// <summary>
    ///     Returns the <paramref name="root" /> with the call renamed to <paramref name="newName" /> and awaited.
    /// </summary>
    public SyntaxNode Apply(SyntaxNode root, string newName)
    {
        var nodes = _function is null
            ? new SyntaxNode[] { _invocation }
            : new[] { _invocation, _function };

        // The call is inside the function, so it is replaced first, and the function is rewritten with the awaited call in it.
        return root.ReplaceNodes(
            nodes,
            (original, rewritten) => original == _invocation
                ? Await((InvocationExpressionSyntax)rewritten, newName)
                : MakeAsync(rewritten, _functionReturnsValue));
    }

    /// <summary>
    ///     Returns how to await the call that the <paramref name="name" /> names, or <see langword="null" /> if it cannot be awaited where it
    ///     is.
    /// </summary>
    public static AwaitedCall? TryCreate(SimpleNameSyntax name, SemanticModel model, CancellationToken cancellationToken)
    {
        // result.Then(…) or Then(…); after a null-conditional access (result?.Then(…)) the task could be null.
        var callee = name.Parent is MemberAccessExpressionSyntax access && access.Name == name
            ? access
            : (ExpressionSyntax)name;

        if (callee.Parent is not InvocationExpressionSyntax invocation || invocation.Expression != callee)
            return null;

        // The first thing around the call that decides whether an await is possible: a statement or clause that forbids it, the function
        // that contains the call, or the member (every call is inside one of them).
        var container = invocation.Ancestors().First(IsContainer);

        switch (container)
        {
            case AnonymousFunctionExpressionSyntax lambda:
                var isAsyncLambda = lambda.AsyncKeyword.IsKind(SyntaxKind.AsyncKeyword);
                var lambdaType = model.GetTypeInfo(lambda, cancellationToken).ConvertedType as INamedTypeSymbol;

                return Create(name, invocation, lambda, isAsyncLambda, lambdaType?.DelegateInvokeMethod?.ReturnType);

            case LocalFunctionStatementSyntax localFunction:
                var isAsyncLocalFunction = localFunction.Modifiers.Any(SyntaxKind.AsyncKeyword);
                var localFunctionSymbol = model.GetDeclaredSymbol(localFunction, cancellationToken);

                return Create(name, invocation, localFunction, isAsyncLocalFunction, localFunctionSymbol?.ReturnType);

            case MethodDeclarationSyntax method:
                var isAsyncMethod = method.Modifiers.Any(SyntaxKind.AsyncKeyword);
                var methodSymbol = model.GetDeclaredSymbol(method, cancellationToken);

                return Create(name, invocation, method, isAsyncMethod, methodSymbol?.ReturnType);

            // Top-level statements can await.
            case GlobalStatementSyntax:
                return new AwaitedCall(name, invocation, function: null, functionReturnsValue: false);

            // A lock, unsafe or fixed statement, a query, a catch filter; a constructor, an accessor, an operator, an initializer.
            default:
                return null;
        }
    }

    private static bool IsContainer(SyntaxNode node) =>
        node is LockStatementSyntax or UnsafeStatementSyntax or FixedStatementSyntax or QueryExpressionSyntax or CatchFilterClauseSyntax
            or AnonymousFunctionExpressionSyntax or LocalFunctionStatementSyntax or MemberDeclarationSyntax or AccessorDeclarationSyntax;

    private static SyntaxToken AsyncKeyword(SyntaxTriviaList leadingTrivia) =>
        SyntaxFactory.Token(leadingTrivia, SyntaxKind.AsyncKeyword, SyntaxFactory.TriviaList(SyntaxFactory.Space));

    private static ExpressionSyntax Await(ExpressionSyntax expression)
    {
        var keyword = SyntaxFactory.Token(expression.GetLeadingTrivia(), SyntaxKind.AwaitKeyword, SyntaxFactory.TriviaList(SyntaxFactory.Space));

        return SyntaxFactory.AwaitExpression(keyword, expression.WithoutLeadingTrivia());
    }

    private ExpressionSyntax Await(InvocationExpressionSyntax invocation, string newName)
    {
        var name = invocation.Expression is MemberAccessExpressionSyntax access
            ? access.Name
            : (SimpleNameSyntax)invocation.Expression;
        var newIdentifier = SyntaxFactory.Identifier(newName).WithTriviaFrom(name.Identifier);
        var renamed = invocation.ReplaceToken(name.Identifier, newIdentifier);
        var awaited = Await(renamed);

        // As a statement of its own the await needs no parentheses; elsewhere the simplifier removes those that are not needed.
        return _invocation.Parent is ExpressionStatementSyntax
            ? awaited
            : SyntaxFactory.ParenthesizedExpression(awaited.WithoutLeadingTrivia())
                .WithLeadingTrivia(awaited.GetLeadingTrivia())
                .WithAdditionalAnnotations(Simplifier.Annotation);
    }

    private static AwaitedCall? Create(SimpleNameSyntax name, InvocationExpressionSyntax invocation, SyntaxNode function, bool isAsync, ITypeSymbol? returnType)
    {
        if (isAsync)
            return new AwaitedCall(name, invocation, function: null, functionReturnsValue: false);

        // Making the function asynchronous keeps its signature only when it returns a task already.
        var isTask = returnType is INamedTypeSymbol
        {
            Name: "Task" or "ValueTask",
            ContainingNamespace: { Name: "Tasks", ContainingNamespace: { Name: "Threading", ContainingNamespace.Name: "System" } }
        };

        // A returned task cannot be awaited inside a lock. (A lambda that becomes an expression tree has no delegate type, so no task.)
        if (!isTask || ReturnsInsideUnawaitableStatement(function))
            return null;

        return new AwaitedCall(name, invocation, function, ((INamedTypeSymbol)returnType!).IsGenericType);
    }

    private static SyntaxNode MakeAsync(SyntaxNode function, bool returnsValue)
    {
        switch (function)
        {
            case AnonymousFunctionExpressionSyntax lambda:
                var lambdaBody = lambda.Block is null
                    ? (CSharpSyntaxNode)AwaitReturned(lambda.ExpressionBody!, returnsValue)
                    : ReturnRewriter.Rewrite(lambda.Block, returnsValue);
                var lambdaKeyword = AsyncKeyword(lambda.GetLeadingTrivia());

                return lambda.WithoutLeadingTrivia().WithBody(lambdaBody).WithAsyncKeyword(lambdaKeyword).WithAdditionalAnnotations(Formatter.Annotation);

            case LocalFunctionStatementSyntax localFunction:
                var localFunctionBody = localFunction.Body is null
                    ? null
                    : ReturnRewriter.Rewrite(localFunction.Body, returnsValue);
                var localFunctionExpression = AwaitReturned(localFunction.ExpressionBody, returnsValue);
                var localFunctionModifiers = WithAsync(localFunction.Modifiers, localFunction.ReturnType, out var localFunctionReturnType);

                return localFunction
                    .WithBody(localFunctionBody)
                    .WithExpressionBody(localFunctionExpression)
                    .WithReturnType(localFunctionReturnType)
                    .WithModifiers(localFunctionModifiers)
                    .WithAdditionalAnnotations(Formatter.Annotation);

            default:
                var method = (MethodDeclarationSyntax)function;
                var methodBody = method.Body is null
                    ? null
                    : ReturnRewriter.Rewrite(method.Body, returnsValue);
                var methodExpression = AwaitReturned(method.ExpressionBody, returnsValue);
                var methodModifiers = WithAsync(method.Modifiers, method.ReturnType, out var methodReturnType);

                return method
                    .WithBody(methodBody)
                    .WithExpressionBody(methodExpression)
                    .WithReturnType(methodReturnType)
                    .WithModifiers(methodModifiers)
                    .WithAdditionalAnnotations(Formatter.Annotation);
        }
    }

    private static ArrowExpressionClauseSyntax? AwaitReturned(ArrowExpressionClauseSyntax? expressionBody, bool returnsValue) =>
        expressionBody?.WithExpression(AwaitReturned(expressionBody.Expression, returnsValue));

    // What an asynchronous function returns in place of the task that the synchronous one returned: the awaited task, or the value itself
    // for Task.FromResult(value).
    private static ExpressionSyntax AwaitReturned(ExpressionSyntax returned, bool returnsValue)
    {
        var isFromResult = returnsValue
                           && returned is InvocationExpressionSyntax
                           {
                               Expression: MemberAccessExpressionSyntax { Name.Identifier.ValueText: "FromResult" },
                               ArgumentList.Arguments.Count: 1
                           };

        return isFromResult
            ? ((InvocationExpressionSyntax)returned).ArgumentList.Arguments[0].Expression.WithTriviaFrom(returned)
            : Await(returned);
    }

    private static bool ReturnsInsideUnawaitableStatement(SyntaxNode function)
    {
        foreach (var statement in function.DescendantNodes(node => node == function || !IsFunction(node)).OfType<ReturnStatementSyntax>())
        {
            var isInside = statement.Expression is not null
                           && statement.Ancestors()
                               .TakeWhile(ancestor => ancestor != function)
                               .Any(ancestor => ancestor is LockStatementSyntax or UnsafeStatementSyntax or FixedStatementSyntax);

            if (isInside)
                return true;
        }

        return false;
    }

    private static bool IsFunction(SyntaxNode node) => node is AnonymousFunctionExpressionSyntax or LocalFunctionStatementSyntax;

    // The async modifier goes after the other modifiers but before partial; without modifiers it takes the place, and the leading trivia,
    // of the return type.
    private static SyntaxTokenList WithAsync(SyntaxTokenList modifiers, TypeSyntax returnType, out TypeSyntax newReturnType)
    {
        newReturnType = returnType;

        if (modifiers.Count == 0)
        {
            newReturnType = returnType.WithoutLeadingTrivia();

            return modifiers.Add(AsyncKeyword(returnType.GetLeadingTrivia()));
        }

        var index = modifiers.IndexOf(SyntaxKind.PartialKeyword);
        var keyword = AsyncKeyword(SyntaxFactory.TriviaList());

        return index < 0
            ? modifiers.Add(keyword)
            : modifiers.Insert(index, keyword);
    }

    /// <summary>
    ///     Rewrites the <see langword="return" /> statements of a function that returned a task and becomes asynchronous: with a value,
    ///     <c>return task;</c> becomes <c>return await task;</c>; without one it becomes <c>await task; return;</c>, and the
    ///     <see langword="return" /> is left out at the end of the function. Nested functions are not touched.
    /// </summary>
    private sealed class ReturnRewriter : CSharpSyntaxRewriter
    {
        private readonly bool _returnsValue;
        private readonly BlockSyntax _body;

        private ReturnRewriter(BlockSyntax body, bool returnsValue)
        {
            _body = body;
            _returnsValue = returnsValue;
        }

        public static BlockSyntax Rewrite(BlockSyntax body, bool returnsValue) => (BlockSyntax)new ReturnRewriter(body, returnsValue).Visit(body)!;

        public override SyntaxNode? VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node) => node;

        public override SyntaxNode? VisitBlock(BlockSyntax node)
        {
            if (_returnsValue)
                return base.VisitBlock(node);

            var statements = new List<StatementSyntax>(node.Statements.Count + 1);

            for (var index = 0; index < node.Statements.Count; index++)
            {
                var statement = node.Statements[index];

                if (statement is ReturnStatementSyntax { Expression: { } returned } returnStatement)
                {
                    var isLastOfTheFunction = node == _body && index == node.Statements.Count - 1;
                    statements.AddRange(AwaitThenReturn(returnStatement, returned, withReturn: !isLastOfTheFunction));
                }
                else
                {
                    statements.Add((StatementSyntax)Visit(statement)!);
                }
            }

            return node.WithStatements(SyntaxFactory.List(statements));
        }

        public override SyntaxNode? VisitLocalFunctionStatement(LocalFunctionStatementSyntax node) => node;

        public override SyntaxNode? VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node) => node;

        public override SyntaxNode? VisitReturnStatement(ReturnStatementSyntax node)
        {
            if (node.Expression is null)
                return node;

            if (_returnsValue)
                return node.WithExpression(AwaitReturned(node.Expression, returnsValue: true));

            // A return that is not a statement of a block (e.g., the statement of an if) becomes a block of its own, on one line:
            // "{ await task; return; }".
            var space = SyntaxFactory.TriviaList(SyntaxFactory.Space);
            var awaitStatement = SyntaxFactory.ExpressionStatement(Await(node.Expression.WithoutLeadingTrivia()), Semicolon(space));
            var returnStatement = SyntaxFactory.ReturnStatement(Keyword(SyntaxKind.ReturnKeyword), expression: null, Semicolon(space));
            var openBrace = SyntaxFactory.Token(node.GetLeadingTrivia(), SyntaxKind.OpenBraceToken, space);
            var closeBrace = SyntaxFactory.Token(SyntaxFactory.TriviaList(), SyntaxKind.CloseBraceToken, node.GetTrailingTrivia());

            return SyntaxFactory.Block(openBrace, SyntaxFactory.List<StatementSyntax>([awaitStatement, returnStatement]), closeBrace);
        }

        public override SyntaxNode? VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node) => node;

        // "return task;" as two statements, each on a line of its own with the indentation and the line end of the original. The tokens
        // get their trivia explicitly, so that the formatter has nothing to decide.
        private static IEnumerable<StatementSyntax> AwaitThenReturn(ReturnStatementSyntax statement, ExpressionSyntax returned, bool withReturn)
        {
            var lineEnd = statement.GetTrailingTrivia();

            yield return SyntaxFactory.ExpressionStatement(Await(returned.WithoutLeadingTrivia()), Semicolon(lineEnd))
                .WithLeadingTrivia(statement.GetLeadingTrivia());

            if (withReturn)
            {
                // The indentation is what follows the last line break before the statement.
                var leading = statement.GetLeadingTrivia();
                var lastLineBreak = -1;

                for (var index = 0; index < leading.Count; index++)
                {
                    if (leading[index].IsKind(SyntaxKind.EndOfLineTrivia))
                        lastLineBreak = index;
                }

                var indentation = SyntaxFactory.TriviaList(leading.Skip(lastLineBreak + 1));

                yield return SyntaxFactory.ReturnStatement(Keyword(SyntaxKind.ReturnKeyword), expression: null, Semicolon(lineEnd))
                    .WithLeadingTrivia(indentation);
            }
        }

        private static SyntaxToken Keyword(SyntaxKind kind) => SyntaxFactory.Token(SyntaxFactory.TriviaList(), kind, SyntaxFactory.TriviaList());

        private static SyntaxToken Semicolon(SyntaxTriviaList trailingTrivia) =>
            SyntaxFactory.Token(SyntaxFactory.TriviaList(), SyntaxKind.SemicolonToken, trailingTrivia);
    }
}
