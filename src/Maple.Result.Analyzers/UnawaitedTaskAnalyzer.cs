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

            var returnsTask = function.ExpressionBody is not null && types.IsTask(function.ReturnedType);

            if (!function.IsAsyncLambda && !returnsTask)
                continue;

            var what = function.IsAsyncLambda
                ? "this async lambda becomes async void and"
                : "the task that this function returns";

            var location = function.IsAsyncLambda
                ? function.Syntax.GetLocation()
                : function.ExpressionBody!.GetLocation();

            var newName = KnownTypes.MethodsWithAsyncForm.Contains(method.Name)
                ? method.Name + "Async"
                : null;

            var nameLocations = ImmutableArray.Create(CalledName(invocation).GetLocation());
            var advice = Advice(method);
            var diagnostic = Diagnostic.Create(Rules.UnawaitedTask, location, nameLocations, RenameTo(invocation, newName), method.Name, what, advice);
            context.ReportDiagnostic(diagnostic);
        }
    }

    #region helper methods

    private static string Advice(IMethodSymbol method)
    {
        if (KnownTypes.MethodsWithAsyncForm.Contains(method.Name))
            return "Use " + method.Name + "Async";

        return method.Name is "Switch" or "SwitchAsync"
            ? "Use Match or MatchAsync with two functions that return a task"
            : "Pass a function that returns a Task or a ValueTask without a value, e.g., an async lambda that awaits it";
    }

    #endregion
}
