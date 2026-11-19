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
///     The base of the analyzers of this assembly: it resolves the types of the library once for a compilation, and registers nothing for a
///     compilation that does not reference the library.
/// </summary>
/// <remarks>
///     A rule that is an error replaces a compile error of the library, so it is also reported in generated code, as a compile error would
///     be; a rule that is a warning is not.
/// </remarks>
public abstract class ResultAnalyzer : DiagnosticAnalyzer
{
    /// <inheritdoc />
    public sealed override void Initialize(AnalysisContext context)
    {
        var generatedCode = SupportedDiagnostics[0].DefaultSeverity == DiagnosticSeverity.Error
            ? GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics
            : GeneratedCodeAnalysisFlags.None;

        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(generatedCode);
        context.RegisterCompilationStartAction(start =>
        {
            var types = KnownTypes.For(start.Compilation);

            if (types.IsLibraryReferenced)
                Register(start, types);
        });
    }

    /// <summary>
    ///     Returns the name that the <paramref name="invocation" /> calls the method by (<c>Then</c> in <c>result.Then(…)</c>, with its type
    ///     arguments when they are written); for a query clause, which has no name, the clause.
    /// </summary>
    private protected static SyntaxNode CalledName(IInvocationOperation invocation)
    {
        if (invocation.Syntax is not InvocationExpressionSyntax call)
            return invocation.Syntax;

        return call.Expression switch
        {
            MemberAccessExpressionSyntax access => access.Name,
            MemberBindingExpressionSyntax binding => binding.Name,
            var name => name,
        };
    }

    /// <summary>Whether the call names its type arguments, e.g., <c>Then&lt;int, Task&lt;int&gt;&gt;(…)</c>.</summary>
    private protected static bool HasExplicitTypeArguments(IInvocationOperation invocation) => CalledName(invocation) is GenericNameSyntax;

    /// <summary>
    ///     Registers the actions of the analyzer for a compilation that references the library.
    /// </summary>
    private protected abstract void Register(CompilationStartAnalysisContext context, KnownTypes types);

    /// <summary>
    ///     Returns the properties of a diagnostic that a code fix resolves by renaming the called method to <paramref name="newName" />.
    ///     No property for <see langword="null" />, and none for an extension method called in its static form
    ///     (<c>ThenExtensions.Then(result, …)</c>), where the other method is in another class.
    /// </summary>
    private protected static ImmutableDictionary<string, string?> RenameTo(IInvocationOperation invocation, string? newName)
    {
        var isStaticForm = false;

        foreach (var argument in invocation.Arguments)
        {
            // Written as an argument, the receiver of an extension method makes the call the static form.
            if (invocation.TargetMethod.IsExtensionMethod && argument.Parameter is { Ordinal: 0 } && argument.Syntax is ArgumentSyntax)
                isStaticForm = true;
        }

        return newName is null || isStaticForm
            ? ImmutableDictionary<string, string?>.Empty
            : ImmutableDictionary<string, string?>.Empty.Add(Rules.RenameToProperty, newName);
    }
}
