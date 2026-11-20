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
using System.Linq;

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
    ///     Registers the actions of the analyzer for a compilation that references the library.
    /// </summary>
    private protected abstract void Register(CompilationStartAnalysisContext context, KnownTypes types);

    #region helper methods

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

    /// <summary>
    ///     Whether the call names its type arguments, e.g., <c>Then&lt;int, Task&lt;int&gt;&gt;(…)</c>.
    /// </summary>
    private protected static bool HasExplicitTypeArguments(IInvocationOperation invocation)
        => CalledName(invocation) is GenericNameSyntax;

    /// <summary>
    ///     Whether the <paramref name="invocation" /> is <c>ConfigureAwait</c> of a task, whose awaiter gives the value of the task.
    /// </summary>
    private protected static bool IsConfigureAwait(IInvocationOperation invocation)
        => invocation.TargetMethod is { Name: "ConfigureAwait", ContainingNamespace: { Name: "Tasks", ContainingNamespace.Name: "Threading" } };

    /// <summary>
    ///     Whether the value of the <paramref name="call" /> is used. It is not when the call is a statement of its own, also awaited, configured
    ///     with <c>ConfigureAwait</c> or after a null-conditional access (<c>result?.IfSuccess(…);</c>), and when it is discarded (<c>_ = …</c>).
    /// </summary>
    private protected static bool IsResultUsed(IOperation call)
    {
        var operation = call;

        while (true)
        {
            var parent = operation.Parent;

            switch (parent)
            {
                case IExpressionStatementOperation:
                    return false;

                case ISimpleAssignmentOperation { Target: IDiscardOperation } discard when discard.Value == operation:
                    return false;

                case IAwaitOperation:
                case IConditionalAccessOperation access when access.WhenNotNull == operation:
                case IInvocationOperation configured when configured.Instance == operation && IsConfigureAwait(configured):
                    operation = parent;

                    continue;

                default:
                    return true;
            }
        }
    }

    /// <summary>
    ///     Returns the argument of the first parameter of the called method: the receiver of an extension method, in both forms of the call
    ///     (<c>result.Then(…)</c> and <c>ThenExtensions.Then(result, …)</c>). The arguments are in the order they are evaluated, so a named
    ///     argument can put it anywhere.
    /// </summary>
    private protected static IArgumentOperation? ReceiverArgument(IInvocationOperation invocation)
        => invocation.Arguments.FirstOrDefault(static argument => argument.Parameter is { Ordinal: 0 });

    /// <summary>
    ///     Returns the properties of a diagnostic that a code fix resolves by renaming the called method to <paramref name="newName" />.
    ///     No property for <see langword="null" />, and none for an extension method called in its static form
    ///     (<c>ThenExtensions.Then(result, …)</c>), where the other method is in another class.
    /// </summary>
    private protected static ImmutableDictionary<string, string?> RenameTo(IInvocationOperation invocation, string? newName)
    {
        // Written as an argument, the receiver of an extension method makes the call the static form.
        var isStaticForm = invocation.TargetMethod.IsExtensionMethod && ReceiverArgument(invocation)?.Syntax is ArgumentSyntax;

        return newName is null || isStaticForm
            ? ImmutableDictionary<string, string?>.Empty
            : ImmutableDictionary<string, string?>.Empty.Add(Rules.RenameToProperty, newName);
    }

    #endregion
}
