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
///     MR0006: reports a statement that is a call whose result, or task of a result, is not used, e.g., <c>await SaveAsync(order);</c> where
///     <c>SaveAsync</c> returns a <c>Task&lt;Result&gt;</c>: a failure is lost.
/// </summary>
/// <remarks>
///     Not reported: an explicit discard (<c>_ = Save();</c>); any use of the value; a statement that ends with <c>IfError</c>,
///     <c>IfErrorAsync</c>, <c>Match</c> or <c>MatchAsync</c> of the library, which handled the error; <c>IfSuccess</c> or
///     <c>IfSuccessAsync</c> on a result that is stored (<c>result.IfSuccess(…);</c>), which returns that result and loses nothing; and the
///     expression body of a function passed to a method of the library, which MR0002 and MR0004 cover.
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DiscardedResultAnalyzer : ResultAnalyzer
{
    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rules.DiscardedResult];

    private protected override void Register(CompilationStartAnalysisContext context, KnownTypes types)
        => context.RegisterOperationAction(operationContext => Analyze(operationContext, types), OperationKind.ExpressionStatement);

    private static void Analyze(OperationAnalysisContext context, KnownTypes types)
    {
        var statement = (IExpressionStatementOperation)context.Operation;
        var invocation = Call(statement.Operation);

        if (invocation is null || IsBodyOfLibraryFunction(statement, types))
            return;

        var method = invocation.TargetMethod;
        var returned = method.ReturnType;
        var result = types.AwaitedValue(returned) ?? returned;

        if (!types.IsResult(result))
            return;

        if (!LosesAResult(invocation, types))
            return;

        var location = statement.Operation.Syntax.GetLocation();
        var resultName = KnownTypes.Display(result);
        var diagnostic = Diagnostic.Create(Rules.DiscardedResult, location, resultName, method.Name);
        context.ReportDiagnostic(diagnostic);
    }

    #region helper methods

    // The call that produces the value of the statement: directly, awaited, or after a null-conditional access (order?.Save()).
    private static IInvocationOperation? Call(IOperation operation)
    {
        while (true)
        {
            switch (operation)
            {
                case IInvocationOperation invocation:
                    return invocation;

                case IAwaitOperation awaited:
                    operation = awaited.Operation;

                    break;

                case IConditionalAccessOperation access:
                    operation = access.WhenNotNull;

                    break;

                default:
                    return null;
            }
        }
    }

    // Whether the statement that ends with this call drops a result that nothing else holds.
    private static bool LosesAResult(IInvocationOperation invocation, KnownTypes types)
    {
        var method = invocation.TargetMethod;

        if (!types.IsLibraryMember(method))
            return true;

        switch (method.Name)
        {
            // The error was handled.
            case "IfError" or "IfErrorAsync" or "Match" or "MatchAsync":
                return false;

            // The method returns the result it was given: nothing is lost when that result is stored (a variable, a field, a property),
            // and what an earlier call of the chain produced is lost with it.
            case "IfSuccess" or "IfSuccessAsync":
                var receiver = Receiver(invocation);

                return receiver is not null && LosesAResult(receiver, types);

            default:
                return true;
        }
    }

    // The call that produces the receiver of an extension method of the library, or null when the receiver is not a call.
    private static IInvocationOperation? Receiver(IInvocationOperation invocation)
        => ReceiverArgument(invocation) is { } receiver ? Call(receiver.Value) : null;

    // The expression body of a lambda passed to a method of the library: MR0002 and MR0004 report what such a function drops.
    private static bool IsBodyOfLibraryFunction(IExpressionStatementOperation statement, KnownTypes types)
        => statement.Parent is IBlockOperation
        {
            Parent: IAnonymousFunctionOperation { Parent: IDelegateCreationOperation { Parent: IArgumentOperation { Parent: IInvocationOperation call } } }
        }
        && types.IsLibraryMember(call.TargetMethod);

    #endregion
}
