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
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace Maple.Results.Analyzers;

/// <summary>
///     MR0005: reports a function passed to <c>IfSuccess</c>, <c>IfError</c> or their asynchronous forms that returns a value of the type of
///     the result’s value (or a task of it), when the result of the call is used, e.g.,
///     <c>var normalized = result.IfSuccess(x => Normalize(x));</c>. In version 2 such a call transformed the result; since version 3 it runs
///     the function as a side effect and keeps the original result.
/// </summary>
/// <remarks>
///     Not reported: a side effect that returns another type (<c>x => builder.Append(x)</c>), an assignment, and a call whose result is not
///     used.
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class IgnoredValueAnalyzer : ResultAnalyzer
{
    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rules.IgnoredValue];

    private protected override void Register(CompilationStartAnalysisContext context, KnownTypes types)
        => context.RegisterOperationAction(operationContext => Analyze(operationContext, types), OperationKind.Invocation);

    private static void Analyze(OperationAnalysisContext context, KnownTypes types)
    {
        var invocation = (IInvocationOperation)context.Operation;
        var method = invocation.TargetMethod;
        var newName = method.Name switch
        {
            "IfSuccess" => "Then",
            "IfSuccessAsync" => "ThenAsync",
            "IfError" => "Else",
            "IfErrorAsync" => "ElseAsync",
            _ => null,
        };

        if (newName is null || !types.IsLibraryMember(method) || !IsResultUsed(invocation.Syntax))
            return;

        // The receiver is the first parameter of the extension method; the function is the only delegate among the others.
        var receiver = ReceiverArgument(invocation);
        var receiverType = receiver?.Value.Type;
        DelegateArgument function = default;

        foreach (var argument in invocation.Arguments)
        {
            if (argument != receiver && DelegateArgument.TryCreate(argument, context.CancellationToken, out var candidate))
                function = candidate;
        }

        var valueType = types.ResultValue(types.AwaitedValue(receiverType) ?? receiverType);

        if (valueType is null || function.ReturnedType is null || function.IsAssignment)
            return;

        var returned = function.IsAsyncLambda
            ? function.ReturnedType
            : types.AwaitedValue(function.ReturnedType) ?? function.ReturnedType;

        if (!SymbolEqualityComparer.Default.Equals(returned, valueType))
            return;

        var location = (function.ExpressionBody ?? function.Syntax).GetLocation();
        var nameLocations = ImmutableArray.Create(CalledName(invocation).GetLocation());
        var diagnostic = Diagnostic.Create(Rules.IgnoredValue, location, nameLocations, RenameTo(invocation, newName), method.Name, newName);
        context.ReportDiagnostic(diagnostic);
    }

    // A call that is a statement of its own (also awaited) does not use the result it returns.
    private static bool IsResultUsed(SyntaxNode call)
    {
        var node = call;

        while (node.Parent is ParenthesizedExpressionSyntax or AwaitExpressionSyntax)
            node = node.Parent;

        return node.Parent is not ExpressionStatementSyntax;
    }
}
