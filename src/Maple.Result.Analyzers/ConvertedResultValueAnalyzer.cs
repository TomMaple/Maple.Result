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
///     MR0003: reports the implicit conversion from a value to <c>Result&lt;T&gt;</c> applied to a task, a result or an <c>Error</c>, e.g.,
///     <c>Result&lt;object&gt; r = SaveAsync();</c>: the result is successful and holds the task as its value.
/// </summary>
/// <remarks>
///     Not reported: the conversion from an <c>Error</c> to a failed result (<c>Result&lt;object&gt; r = error;</c>).
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class ConvertedResultValueAnalyzer : ResultAnalyzer
{
    /// <inheritdoc />
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Rules.ConvertedResultValue);

    private protected override void Register(CompilationStartAnalysisContext context, KnownTypes types) =>
        context.RegisterOperationAction(operationContext => Analyze(operationContext, types), OperationKind.Conversion);

    private static void Analyze(OperationAnalysisContext context, KnownTypes types)
    {
        var conversion = (IConversionOperation)context.Operation;
        var conversionOperator = conversion.OperatorMethod;

        // Only the operator that takes the value (T); the operator that takes an Error gives a failed result.
        var isValueConversion = conversionOperator is { Name: "op_Implicit" }
                                && types.IsLibraryMember(conversionOperator)
                                && conversionOperator.OriginalDefinition.Parameters[0].Type is ITypeParameterSymbol;

        if (!isValueConversion || conversion.Type is null)
            return;

        // The operand is first converted to T (e.g., a Task to object); look at what the caller wrote.
        var operand = conversion.Operand;

        while (operand is IConversionOperation { Conversion.IsUserDefined: false } standardConversion)
            operand = standardConversion.Operand;

        var operandType = operand.Type;

        if (operandType is null || (!types.IsTask(operandType) && !types.IsResultOrError(operandType)))
            return;

        var location = conversion.Operand.Syntax.GetLocation();
        var operandName = KnownTypes.Display(operandType);
        var resultName = KnownTypes.Display(conversion.Type);
        var diagnostic = Diagnostic.Create(Rules.ConvertedResultValue, location, operandName, resultName);
        context.ReportDiagnostic(diagnostic);
    }
}
