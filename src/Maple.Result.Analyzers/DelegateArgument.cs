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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using System.Threading;

namespace Maple.Results.Analyzers;

/// <summary>
///     A lambda or a method group passed to a delegate parameter, with what the analyzers need to know about it.
/// </summary>
internal readonly struct DelegateArgument
{
    #region constructors

    private DelegateArgument(IMethodSymbol invoke, SyntaxNode syntax, SyntaxNode? expressionBody, ITypeSymbol? returnedType, bool isAsyncLambda)
    {
        Invoke = invoke;
        Syntax = syntax;
        ExpressionBody = expressionBody;
        ReturnedType = returnedType;
        IsAsyncLambda = isAsyncLambda;
    }

    #endregion

    /// <summary>
    ///     The body of the lambda when it is an expression; <see langword="null" /> for a block body and for a method group.
    /// </summary>
    public SyntaxNode? ExpressionBody { get; }

    /// <summary>
    ///     The <c>Invoke</c> method of the delegate type that the argument was converted to.
    /// </summary>
    public IMethodSymbol Invoke { get; }

    /// <summary>
    ///     Whether the argument is a lambda with the <see langword="async" /> modifier.
    /// </summary>
    public bool IsAsyncLambda { get; }

    /// <summary>
    ///     The type of the expression body of the lambda (inside an <see langword="async" /> lambda, the awaited type), or the return type of
    ///     the method group; <see langword="null" /> for a block body.
    /// </summary>
    public ITypeSymbol? ReturnedType { get; }

    /// <summary>
    ///     The lambda or the method group.
    /// </summary>
    public SyntaxNode Syntax { get; }

    /// <summary>
    ///     Whether the body of the lambda is an assignment, whose value is stored and not lost.
    /// </summary>
    public bool IsAssignment => ExpressionBody is AssignmentExpressionSyntax;

    /// <summary>
    ///     Reads the <paramref name="argument" />; returns <see langword="false" /> if it is not a lambda or a method group converted to a delegate.
    /// </summary>
    public static bool TryCreate(IArgumentOperation argument, CancellationToken cancellationToken, out DelegateArgument delegateArgument)
    {
        delegateArgument = default;

        if (argument.Value is not IDelegateCreationOperation { Type: INamedTypeSymbol { DelegateInvokeMethod: { } invoke } } creation)
            return false;

        switch (creation.Target)
        {
            case IAnonymousFunctionOperation lambda:
                var body = (lambda.Syntax as AnonymousFunctionExpressionSyntax)?.ExpressionBody;
                var bodyType = body is null
                    ? null
                    : lambda.SemanticModel?.GetTypeInfo(body, cancellationToken).Type;
                delegateArgument = new DelegateArgument(invoke, lambda.Syntax, body, bodyType, lambda.Symbol.IsAsync);

                return true;

            case IMethodReferenceOperation reference:
                delegateArgument = new DelegateArgument(invoke, reference.Syntax, expressionBody: null, reference.Method.ReturnType, isAsyncLambda: false);

                return true;

            default:
                return false;
        }
    }
}
