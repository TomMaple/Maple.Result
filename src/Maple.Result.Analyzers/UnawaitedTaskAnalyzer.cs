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
///     MR0002: reports a function that returns a task, or an <see langword="async" /> lambda, passed to a parameter of a library method whose
///     delegate returns nothing, e.g., <c>result.IfSuccess(x => SaveAsync(x))</c>: the task is discarded and never awaited.
/// </summary>
/// <remarks>
///     Not reported: a block body, which can discard a task explicitly (<c>e => { _ = LogAsync(e); }</c>).
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class UnawaitedTaskAnalyzer : ResultAnalyzer
{
    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [Rules.UnawaitedTask];

    private protected override void Register(CompilationStartAnalysisContext context, KnownTypes types)
        => context.RegisterOperationAction(operationContext => Analyze(operationContext, types), OperationKind.Invocation);

    private static void Analyze(OperationAnalysisContext context, KnownTypes types)
    {
        var invocation = (IInvocationOperation)context.Operation;
        var method = invocation.TargetMethod;

        if (!types.IsLibraryMember(method))
            return;

        foreach (var argument in invocation.Arguments)
        {
            if (!DelegateArgument.TryCreate(argument, context.CancellationToken, out var function) || !function.Invoke.ReturnsVoid)
                continue;

            // An assignment stores the task (x => _ = SaveAsync(x), x => last = SaveAsync(x)), so it is not lost.
            var returnsTask = function.ExpressionBody is not null && !function.IsAssignment && types.IsTask(function.ReturnedType);

            if (!function.IsAsyncLambda && !returnsTask)
                continue;

            var what = function.IsAsyncLambda
                ? "this async lambda becomes async void and"
                : "the task that this function returns";

            var location = function.IsAsyncLambda
                ? function.Syntax.GetLocation()
                : function.ExpressionBody!.GetLocation();

            var newName = KnownTypes.MethodsWithAsyncForm.Contains(method.Name)
                ? KeepingMethod(invocation, function, types) ?? method.Name + "Async"
                : null;

            var nameLocations = ImmutableArray.Create(CalledName(invocation).GetLocation());
            var advice = newName is null
                ? Advice(method)
                : "Use " + newName;

            var diagnostic = Diagnostic.Create(Rules.UnawaitedTask, location, nameLocations, RenameTo(invocation, newName), method.Name, what, advice);
            context.ReportDiagnostic(diagnostic);
        }
    }

    #region helper methods

    // The advice for a method without an asynchronous form (Then and the others with one are renamed).
    private static string Advice(IMethodSymbol method)
    {
        return method.Name is "Switch" or "SwitchAsync"
            ? "Use Match or MatchAsync with two functions that return a task"
            : "Pass a function that returns a Task or a ValueTask without a value, e.g., an async lambda that awaits it";
    }

    // IfSuccess and IfError with a function whose task holds a result, an Error or the value of the result are transformations: ThenAsync
    // and ElseAsync keep what the task returns, where IfSuccessAsync and IfErrorAsync would drop it (MR0004) or ignore it (MR0005). The
    // value of the result counts only when the result of the call is used and not discarded (_ = …); otherwise the function is a side effect.
    private static string? KeepingMethod(IInvocationOperation invocation, DelegateArgument function, KnownTypes types)
    {
        var awaited = function.IsAsyncLambda
            ? function.ReturnedType
            : types.AwaitedValue(function.ReturnedType);

        if (awaited is null)
            return null;

        var resultType = ReceiverArgument(invocation)?.Value.Type;
        var isUsed = IsResultUsed(invocation.Syntax)
                     && invocation.Syntax.Parent is not AssignmentExpressionSyntax { Left: IdentifierNameSyntax { Identifier.ValueText: "_" } };
        var isTheValue = isUsed && SymbolEqualityComparer.Default.Equals(awaited, types.ResultValue(resultType));

        return invocation.TargetMethod.Name switch
        {
            "IfSuccess" when types.IsResultOrError(awaited) || isTheValue => "ThenAsync",

            // ElseAsync takes an Error, the type of the result itself or its value; a result of another type cannot replace the result.
            "IfError" when !types.IsResult(awaited) && types.IsResultOrError(awaited) => "ElseAsync",
            "IfError" when SymbolEqualityComparer.Default.Equals(awaited, resultType) || isTheValue => "ElseAsync",
            _ => null,
        };
    }

    #endregion
}
