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
///     MR0001: reports a call of a library method in which the compiler inferred a task as the value of the returned result, e.g.,
///     <c>result.Then(x => GetAsync(x))</c>, which is a <c>Result&lt;Task&lt;T&gt;&gt;</c> whose task nobody awaits; and a call that returns
///     a task of a task (<c>MatchAsync</c> with functions that it does not await).
/// </summary>
/// <remarks>
///     Not reported: a call that names its type arguments, <c>Result.FromValue</c>, and a function that returns the result itself
///     (e.g., <c>x => Result.FromValue(task)</c>); these keep the task on purpose. A result or an <c>Error</c> returned by a function is
///     flattened by an overload of its own, so it never becomes a value this way.
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class InferredResultValueAnalyzer : ResultAnalyzer
{
    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rules.InferredResultValue];

    private protected override void Register(CompilationStartAnalysisContext context, KnownTypes types) =>
        context.RegisterOperationAction(operationContext => Analyze(operationContext, types), OperationKind.Invocation);

    private static string Advice(IMethodSymbol method, string? asyncForm, bool isTaskOfTask)
    {
        if (isTaskOfTask)
            return "Pass functions that the method awaits";

        if (method.Name is "Select" or "SelectMany")
            return "Await it in a from clause, or use ThenAsync";

        // Without an asynchronous form, what is left is ThenAsync on a task of a result with a task without a value: a function that returns
        // a task with a value is awaited by an overload of its own.
        return asyncForm is null or "IfSuccessAsync"
            ? "Use IfSuccessAsync for a side effect that returns no value"
            : "Use " + asyncForm + " to await it";
    }

    private static void Analyze(OperationAnalysisContext context, KnownTypes types)
    {
        var invocation = (IInvocationOperation)context.Operation;
        var method = invocation.TargetMethod;

        if (!method.IsGenericMethod || method.Name == "FromValue" || !types.IsLibraryMember(method) || HasExplicitTypeArguments(invocation))
            return;

        var definition = method.OriginalDefinition;
        var valueParameter = ValueParameter(types, definition, out var isTaskOfTask);

        if (valueParameter is null || !IsReturnedByAFunction(definition, valueParameter))
            return;

        var inferred = method.TypeArguments[valueParameter.Ordinal];

        if (!types.IsTask(inferred))
            return;

        var asyncForm = AsyncForm(types, method, inferred);
        var consequence = isTaskOfTask
            ? "so the call returns a task of a task"
            : "which becomes the value of the result";
        var location = CalledName(invocation).GetLocation();
        var typeName = KnownTypes.Display(inferred);
        var advice = Advice(method, asyncForm, isTaskOfTask);
        var diagnostic = Diagnostic.Create(Rules.InferredResultValue, location, RenameTo(invocation, asyncForm), method.Name, typeName, consequence, advice);
        context.ReportDiagnostic(diagnostic);
    }

    // The method that awaits the task: the Async form, or IfSuccessAsync where Then got a task without a value (ThenAsync takes none).
    private static string? AsyncForm(KnownTypes types, IMethodSymbol method, ITypeSymbol inferred) =>
        method.Name switch
        {
            "Then" when types.IsPlainTask(inferred) => "IfSuccessAsync",
            "Then" => "ThenAsync",
            "Try" => "TryAsync",
            _ => null,
        };

    // Whether a function parameter of the method returns the type parameter itself: a function that returns a Result<TNext> built the result
    // on purpose, so a task in it is not reported.
    private static bool IsReturnedByAFunction(IMethodSymbol definition, ITypeParameterSymbol valueParameter)
    {
        foreach (var parameter in definition.Parameters)
        {
            if (parameter.Type is INamedTypeSymbol { DelegateInvokeMethod: { } invoke }
                && SymbolEqualityComparer.Default.Equals(invoke.ReturnType, valueParameter))
            {
                return true;
            }
        }

        return false;
    }

    private static bool Mentions(ITypeSymbol type, ITypeParameterSymbol parameter)
    {
        if (SymbolEqualityComparer.Default.Equals(type, parameter))
            return true;

        if (type is not INamedTypeSymbol named)
            return false;

        foreach (var argument in named.TypeArguments)
        {
            if (Mentions(argument, parameter))
                return true;
        }

        return false;
    }

    // The type parameter of the method that is the value of the returned result (Result<T>, or a task of it), or the value of the returned
    // task (Task<TOut> of MatchAsync); not the one of the receiver, which the caller chose before this call.
    private static ITypeParameterSymbol? ValueParameter(KnownTypes types, IMethodSymbol definition, out bool isTaskOfTask)
    {
        var awaited = types.AwaitedValue(definition.ReturnType);
        var value = types.ResultValue(awaited ?? definition.ReturnType);
        isTaskOfTask = value is null && awaited is ITypeParameterSymbol;

        if ((value ?? awaited) is not ITypeParameterSymbol { TypeParameterKind: TypeParameterKind.Method } parameter)
            return null;

        var isOfReceiver = definition.IsExtensionMethod && Mentions(definition.Parameters[0].Type, parameter);

        return isOfReceiver
            ? null
            : parameter;
    }
}
