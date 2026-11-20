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
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace Maple.Results.Analyzers;

/// <summary>
///     MR0004: reports a function that returns a result or an <c>Error</c> (or a task of one) passed to a parameter of a library method whose
///     delegate returns nothing or a task without a value, e.g., <c>result.IfSuccess(x => Validate(x))</c>: a failure of the function is lost.
/// </summary>
/// <remarks>
///     Not reported: an assignment (<c>e => lastError = e</c>), whose value is stored. The diagnostic names the method that keeps the
///     result: <c>Then</c> for <c>IfSuccess</c>, and <c>Else</c> for <c>IfError</c> when the function returns an <c>Error</c> or the type
///     of the result itself.
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DroppedResultAnalyzer : ResultAnalyzer
{
    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rules.DroppedResult];

    private protected override void Register(CompilationStartAnalysisContext context, KnownTypes types)
        => context.RegisterOperationAction(operationContext => Analyze(operationContext, types), OperationKind.Invocation);

    private static void Analyze(OperationAnalysisContext context, KnownTypes types)
    {
        var invocation = (IInvocationOperation)context.Operation;
        var method = invocation.TargetMethod;

        if (!types.IsLibraryMember(method))
            return;

        var receiverType = ReceiverArgument(invocation)?.Value.Type;
        var resultType = types.AwaitedValue(receiverType) ?? receiverType;

        foreach (var argument in invocation.Arguments)
        {
            if (!DelegateArgument.TryCreate(argument, context.CancellationToken, out var function) || function.IsAssignment)
                continue;

            // What the delegate drops: the returned value of an action, or the value in the task of an asynchronous action.
            var isAsyncAction = types.IsPlainTask(function.Invoke.ReturnType);
            var dropped = function.Invoke.ReturnsVoid || function.IsAsyncLambda
                ? function.ReturnedType
                : types.AwaitedValue(function.ReturnedType);

            if ((!function.Invoke.ReturnsVoid && !isAsyncAction) || dropped is null || !types.IsResultOrError(dropped))
                continue;

            // Else takes a function that returns an Error or the type of the result itself; a result of another type cannot replace it.
            var canReplaceTheResult = !types.IsResult(dropped) || SymbolEqualityComparer.Default.Equals(dropped, resultType);
            var newName = method.Name switch
            {
                "IfSuccess" => "Then",
                "IfSuccessAsync" => "ThenAsync",
                "IfError" when canReplaceTheResult => "Else",
                "IfErrorAsync" when canReplaceTheResult => "ElseAsync",
                _ => null,
            };

            var advice = newName is null
                ? "Handle it inside the function"
                : "Use " + newName + " to continue with it";

            var location = function.Syntax.GetLocation();
            var nameLocations = ImmutableArray.Create(CalledName(invocation).GetLocation());
            var droppedName = KnownTypes.Display(function.ReturnedType!);
            var diagnostic = Diagnostic.Create(Rules.DroppedResult, location, nameLocations, RenameTo(invocation, newName), method.Name, droppedName, advice);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
