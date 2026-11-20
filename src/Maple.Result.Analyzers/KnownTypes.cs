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
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Maple.Results.Analyzers;

/// <summary>
///     The types of the library and the task types, resolved once for a compilation and shared by the analyzers.
/// </summary>
internal sealed class KnownTypes
{
    #region consts

    // Keyed weakly by the compilation, so that the six analyzers resolve the types once and nothing outlives the compilation.
    private static readonly ConditionalWeakTable<Compilation, KnownTypes> Cache = new();

    #endregion

    #region read-only fields

    private readonly INamedTypeSymbol? _error;
    private readonly INamedTypeSymbol? _operationResult;
    private readonly INamedTypeSymbol? _result;
    private readonly INamedTypeSymbol? _resultOfT;
    private readonly INamedTypeSymbol? _task;
    private readonly INamedTypeSymbol? _taskOfT;
    private readonly INamedTypeSymbol? _valueTask;
    private readonly INamedTypeSymbol? _valueTaskOfT;

    #endregion

    #region constructors

    private KnownTypes(Compilation compilation)
    {
        _result = compilation.GetTypeByMetadataName("Maple.Results.Result");
        _resultOfT = compilation.GetTypeByMetadataName("Maple.Results.Result`1");
        _error = compilation.GetTypeByMetadataName("Maple.Results.Error");
        _operationResult = compilation.GetTypeByMetadataName("Maple.Results.IOperationResult");
        _task = compilation.GetTypeByMetadataName("System.Threading.Tasks.Task");
        _taskOfT = compilation.GetTypeByMetadataName("System.Threading.Tasks.Task`1");
        _valueTask = compilation.GetTypeByMetadataName("System.Threading.Tasks.ValueTask");
        _valueTaskOfT = compilation.GetTypeByMetadataName("System.Threading.Tasks.ValueTask`1");
    }

    #endregion

    /// <summary>
    ///     The names of the synchronous methods that have a form with the <c>Async</c> suffix for an asynchronous function.
    /// </summary>
    public static ImmutableHashSet<string> MethodsWithAsyncForm { get; } = ImmutableHashSet.Create("Else", "IfError", "IfSuccess", "Then", "Try");

    /// <summary>
    ///     Whether the compilation references the library; the analyzers register nothing when it does not.
    /// </summary>
    public bool IsLibraryReferenced => _result is not null;

    /// <summary>
    ///     Returns the type that a <c>Task&lt;T&gt;</c> or a <c>ValueTask&lt;T&gt;</c> produces, or <see langword="null" /> for any other type.
    /// </summary>
    public ITypeSymbol? AwaitedValue(ITypeSymbol? type)
    {
        if (type is not INamedTypeSymbol { IsGenericType: true } named)
            return null;

        var isTaskOfT = Same(named.OriginalDefinition, _taskOfT) || Same(named.OriginalDefinition, _valueTaskOfT);

        return isTaskOfT
            ? named.TypeArguments[0]
            : null;
    }

    /// <summary>
    ///     The name of the <paramref name="type" /> as a caller writes it (e.g., <c>Task&lt;int&gt;</c>).
    /// </summary>
    public static string Display(ITypeSymbol type)
        => type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

    /// <summary>
    ///     Returns the known types of the <paramref name="compilation" />.
    /// </summary>
    public static KnownTypes For(Compilation compilation)
        => Cache.GetValue(compilation, static key => new KnownTypes(key));

    /// <summary>
    ///     Whether the <paramref name="symbol" /> is declared in the library.
    /// </summary>
    public bool IsLibraryMember(ISymbol symbol)
        => Same(symbol.ContainingAssembly, _result?.ContainingAssembly);

    /// <summary>
    ///     Whether the <paramref name="type" /> is <c>IOperationResult</c>, the interface of <c>Result</c> and <c>Result&lt;T&gt;</c>.
    /// </summary>
    public bool IsOperationResult(ITypeSymbol? type)
        => Same(type, _operationResult);

    /// <summary>
    ///     Whether the <paramref name="type" /> is a <c>Task</c> or a <c>ValueTask</c> without a value.
    /// </summary>
    public bool IsPlainTask(ITypeSymbol? type)
        => Same(type, _task) || Same(type, _valueTask);

    /// <summary>
    ///     Whether the <paramref name="type" /> is a <c>Result</c> or a <c>Result&lt;T&gt;</c>.
    /// </summary>
    public bool IsResult(ITypeSymbol? type)
        => Same(type, _result) || Same(type?.OriginalDefinition, _resultOfT);

    /// <summary>
    ///     Whether the <paramref name="type" /> is a result or an <c>Error</c>.
    /// </summary>
    public bool IsResultOrError(ITypeSymbol? type)
        => IsResult(type) || Same(type, _error);

    /// <summary>
    ///     Whether the <paramref name="type" /> is a <c>Task</c>, a <c>Task&lt;T&gt;</c>, a <c>ValueTask</c> or a <c>ValueTask&lt;T&gt;</c>.
    /// </summary>
    public bool IsTask(ITypeSymbol? type)
        => IsPlainTask(type) || AwaitedValue(type) is not null;

    /// <summary>
    ///     Returns the type argument of a <c>Result&lt;T&gt;</c>, or <see langword="null" /> for any other type.
    /// </summary>
    public ITypeSymbol? ResultValue(ITypeSymbol? type)
        => type is INamedTypeSymbol named && Same(named.OriginalDefinition, _resultOfT)
            ? named.TypeArguments[0]
            : null;

    #region helper methods

    private static bool Same(ISymbol? left, ISymbol? right)
        => left is not null && SymbolEqualityComparer.Default.Equals(left, right);

    #endregion
}
