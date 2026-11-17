// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Extensions;

/// <summary>
///     The success and the failure path of every <c>Select</c> and <c>SelectMany</c> overload, checked by reflection (#246): a failed source is
///     returned with its error and no selector runs; a successful source runs every selector once and returns a successful result, or the
///     <see cref="Error" /> that the last selector returns.
/// </summary>
public class LinqOverloadsUnitTests
{
    private static readonly Error SourceError = Error.NotFound(ErrorUri.None, "2d1c6b0a-5e8f-4a7b-9c3d-1f0e2a4b6c8d", "Source error");
    private static readonly Error SelectedError = Error.Conflict(ErrorUri.None, "7b9e4c2a-0d6f-4e1b-8a5c-3f2d1e0c9b7a", "Selected error");

    private static readonly Dictionary<string, MethodInfo> Methods = new[] { typeof(LinqExtensions), typeof(LinqAsyncExtensions) }
        .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
        .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is null)
        .ToDictionary(Describe, Close);

    public static TheoryData<string> MethodCases()
    {
        return new TheoryData<string>(Methods.Keys.Order(StringComparer.Ordinal));
    }

    [Fact]
    public void Methods_SelectAndSelectMany_AreFound()
    {
        // Act & Assert
        Methods.Count.ShouldBe(136);
    }

    [Theory]
    [MemberData(nameof(MethodCases))]
    public async Task Method_FailedSource_ReturnsSourceError(string method)
    {
        // Arrange
        var calls = new CallCounter();
        var arguments = CreateArguments(Methods[method], successfulSource: false, calls);

        // Act
        var result = await InvokeAndAwait(Methods[method], arguments);

        // Assert
        result.Error.ShouldBeSameAs(SourceError);
    }

    [Theory]
    [MemberData(nameof(MethodCases))]
    public async Task Method_FailedSource_RunsNoSelector(string method)
    {
        // Arrange
        var calls = new CallCounter();
        var arguments = CreateArguments(Methods[method], successfulSource: false, calls);

        // Act
        await InvokeAndAwait(Methods[method], arguments);

        // Assert
        calls.Count.ShouldBe(0);
    }

    [Theory]
    [MemberData(nameof(MethodCases))]
    public async Task Method_SuccessfulSource_RunsEverySelectorOnce(string method)
    {
        // Arrange
        var calls = new CallCounter();
        var arguments = CreateArguments(Methods[method], successfulSource: true, calls);
        var selectorCount = Methods[method].GetParameters().Count(p => typeof(Delegate).IsAssignableFrom(p.ParameterType));

        // Act
        await InvokeAndAwait(Methods[method], arguments);

        // Assert
        calls.Count.ShouldBe(selectorCount);
    }

    [Theory]
    [MemberData(nameof(MethodCases))]
    public async Task Method_SuccessfulSource_ReturnsTheResultOfTheLastSelector(string method)
    {
        // Arrange
        var calls = new CallCounter();
        var arguments = CreateArguments(Methods[method], successfulSource: true, calls);
        var lastSelectorReturnsError = UnwrapTask(DelegateReturnType(Methods[method].GetParameters()[^1].ParameterType)) == typeof(Error);

        // Act
        var result = await InvokeAndAwait(Methods[method], arguments);

        // Assert
        result.Error.ShouldBeSameAs(lastSelectorReturnsError ? SelectedError : null);
    }

    #region helper methods

    private static MethodInfo Close(MethodInfo method)
    {
        return method.IsGenericMethodDefinition
            ? method.MakeGenericMethod([.. method.GetGenericArguments().Select(_ => typeof(int))])
            : method;
    }

    private static string Describe(MethodInfo method)
    {
        var parameters = string.Join(", ", method.GetParameters().Select(p => TypeName(p.ParameterType)));
        var typeParameters = method.IsGenericMethodDefinition
            ? $"<{string.Join(", ", method.GetGenericArguments().Select(a => a.Name))}>"
            : "";

        return $"{method.Name}{typeParameters}({parameters})";
    }

    private static string TypeName(Type type)
    {
        if (!type.IsGenericType)
            return type.Name;

        var name = type.Name[..type.Name.IndexOf('`')];

        return $"{name}<{string.Join(", ", type.GetGenericArguments().Select(TypeName))}>";
    }

    private static Type DelegateReturnType(Type delegateType)
    {
        return delegateType.GetMethod("Invoke")!.ReturnType;
    }

    private static Type UnwrapTask(Type type)
    {
        return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Task<>) || type.GetGenericTypeDefinition() == typeof(ValueTask<>))
            ? type.GetGenericArguments()[0]
            : type;
    }

    private static object?[] CreateArguments(MethodInfo method, bool successfulSource, CallCounter calls)
    {
        var parameters = method.GetParameters();
        var arguments = new object?[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            var type = parameters[i].ParameterType;

            arguments[i] = typeof(Delegate).IsAssignableFrom(type)
                ? CreateSelector(type, calls)
                : CreateValue(type, successful: successfulSource);
        }

        return arguments;
    }

    private static object? CreateValue(Type type, bool successful)
    {
        if (type == typeof(int))
            return 1;

        if (type == typeof(Error))
            return SelectedError;

        if (type == typeof(Result))
        {
            return successful
                ? Result.Success()
                : Result.FromError(SourceError);
        }

        if (type.IsGenericType)
        {
            var definition = type.GetGenericTypeDefinition();
            var argument = type.GetGenericArguments()[0];

            if (definition == typeof(Result<>))
            {
                return successful
                    ? type.GetMethod(nameof(Result<>.FromValue))!.Invoke(null, [CreateValue(argument, successful: true)])
                    : type.GetMethod(nameof(Result<>.FromError))!.Invoke(null, [SourceError]);
            }

            if (definition == typeof(Task<>))
            {
                return typeof(Task).GetMethod(nameof(Task.FromResult))!.MakeGenericMethod(argument)
                    .Invoke(null, [CreateValue(argument, successful)])!;
            }

            if (definition == typeof(ValueTask<>))
                return type.GetConstructor([argument])!.Invoke([CreateValue(argument, successful)]);
        }

        throw new NotSupportedException($"No test value for {type}.");
    }

    // A selector that counts its calls and returns a successful value (or the selected error, for an Error).
    private static Delegate CreateSelector(Type delegateType, CallCounter calls)
    {
        var invoke = delegateType.GetMethod("Invoke")!;
        var parameters = invoke.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();
        var body = Expression.Block(
            Expression.Call(Expression.Constant(calls), typeof(CallCounter).GetMethod(nameof(CallCounter.Add))!),
            Expression.Constant(CreateValue(invoke.ReturnType, successful: true), invoke.ReturnType));

        return Expression.Lambda(delegateType, body, parameters).Compile();
    }

    private static async Task<IOperationResult> InvokeAndAwait(MethodInfo method, object?[] arguments)
    {
        var returned = method.Invoke(null, arguments);

        switch (returned)
        {
            case IOperationResult result:
                return result;

            case Task task:
                await task;
                return (IOperationResult)task.GetType().GetProperty(nameof(Task<>.Result))!.GetValue(task)!;

            default:
                var asTask = (Task)returned!.GetType().GetMethod(nameof(ValueTask<>.AsTask))!.Invoke(returned, null)!;
                await asTask;
                return (IOperationResult)asTask.GetType().GetProperty(nameof(Task<>.Result))!.GetValue(asTask)!;
        }
    }

    private sealed class CallCounter
    {
        public int Count { get; private set; }

        public void Add()
        {
            Count++;
        }
    }

    #endregion
}
