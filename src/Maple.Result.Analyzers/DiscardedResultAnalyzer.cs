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

        if (IsBodyOfLibraryFunction(statement, types))
            return;

        // An awaited statement drops what the await gives, whatever is awaited: a call, a call configured with ConfigureAwait, or a variable.
        // Otherwise the statement is a call, and a task of a result that it returns is not awaited at all.
        var invocation = Call(statement.Operation);
        var awaited = statement.Operation as IAwaitOperation;
        var returned = awaited?.Type ?? invocation?.TargetMethod.ReturnType;
        var isTask = awaited is null && types.AwaitedValue(returned) is not null;
        var result = isTask
            ? types.AwaitedValue(returned)
            : returned;

        if (!types.IsResult(result) && !types.IsOperationResult(result))
            return;

        if (invocation is not null && !LosesAResult(invocation, types))
            return;

        var location = statement.Operation.Syntax.GetLocation();
        var source = invocation?.TargetMethod.Name ?? awaited!.Operation.Syntax.ToString();
        // A task can be awaited only in an async function; elsewhere no fix keeps the code compiling and the failure visible.
        var fix = isTask
            ? CanAwait(statement, context) ? Rules.AwaitAndDiscardFix : null
            : Rules.DiscardFix;

        var (state, advice) = isTask
            ? ("is not awaited", "Await it and check the result, or discard the result with ‘_ = await’")
            : ("is not used", "Check it, return it, or discard it with ‘_ =’");

        var properties = fix is null
            ? ImmutableDictionary<string, string?>.Empty
            : ImmutableDictionary<string, string?>.Empty.Add(Rules.DiscardFixProperty, fix);

        var shownType = isTask
            ? returned!
            : result!;

        var diagnostic = Diagnostic.Create(Rules.DiscardedResult, location, properties, KnownTypes.Display(shownType), source, state, advice);

        context.ReportDiagnostic(diagnostic);
    }

    #region helper methods

    // The call that produces the value of the statement: directly, awaited, configured with ConfigureAwait, or after a null-conditional
    // access (order?.Save()).
    private static IInvocationOperation? Call(IOperation operation)
    {
        while (true)
        {
            switch (operation)
            {
                case IInvocationOperation configured when IsConfigureAwait(configured) && configured.Instance is not null:
                    operation = configured.Instance;

                    break;

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

    // Whether the statement is in an async method, local function or lambda, where the task can be awaited.
    private static bool CanAwait(IExpressionStatementOperation statement, OperationAnalysisContext context)
    {
        for (var operation = statement.Parent; operation is not null; operation = operation.Parent)
        {
            switch (operation)
            {
                case IAnonymousFunctionOperation function:
                    return function.Symbol.IsAsync;

                case ILocalFunctionOperation function:
                    return function.Symbol.IsAsync;
            }
        }

        return context.ContainingSymbol is IMethodSymbol { IsAsync: true };
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

    // The expression body of a lambda passed to a method of the library: MR0002 and MR0004 report what such a function drops. A statement
    // in a block body is reported here, as in any other method.
    private static bool IsBodyOfLibraryFunction(IExpressionStatementOperation statement, KnownTypes types)
        => statement.Parent is IBlockOperation
        {
            Parent: IAnonymousFunctionOperation
            {
                Syntax: AnonymousFunctionExpressionSyntax { ExpressionBody: not null },
                Parent: IDelegateCreationOperation { Parent: IArgumentOperation { Parent: IInvocationOperation call } },
            },
        }
        && types.IsLibraryMember(call.TargetMethod);

    #endregion
}
