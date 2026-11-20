// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.ResultTests;

/// <summary>
///     The exception paths of every <c>Result.Try</c> and <c>Result.TryAsync</c> overload, checked by reflection (#246): the function throws
///     (for <c>TryAsync</c>, before it returns its task), or the task of <c>TryAsync</c> faults; in each case, an error that the mapper returns
///     is the error of the result, and an exception that the mapper declines (returns <see langword="null" /> for) is rethrown as it is.
/// </summary>
public class ResultTryExceptionPathsUnitTests
{
    private const string Throws = "throws";
    private const string Faults = "faults";

    private static readonly Error MappedError = Error.Failure(ErrorUri.None, "4c7a1e9b-2f3d-4b6a-8e0c-5d9f1a3b7c2e", "Mapped error");

    private static readonly Dictionary<string, MethodInfo> Methods = typeof(Result)
        .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
        .Where(m => m.Name is nameof(Result.Try) or nameof(Result.TryAsync))
        .ToDictionary(Describe, Close);

    public static TheoryData<string, string> MethodCases()
    {
        var data = new TheoryData<string, string>();

        foreach (var method in Methods.Keys.Order(StringComparer.Ordinal))
        {
            data.Add(method, Throws);

            if (method.StartsWith(nameof(Result.TryAsync), StringComparison.Ordinal))
                data.Add(method, Faults);
        }

        return data;
    }

    [Fact]
    public void Methods_TryAndTryAsync_AreFound()
    {
        // Act & Assert
        Methods.Count.ShouldBe(30);
    }

    [Theory]
    [MemberData(nameof(MethodCases))]
    public async Task Try_ExceptionMapped_ReturnsMappedError(string method, string failure)
    {
        // Arrange
        var exception = new InvalidOperationException("Failed.");
        var arguments = CreateArguments(Methods[method], exception, failure, mappedError: MappedError);

        // Act
        var result = await InvokeAndAwait(Methods[method], arguments);

        // Assert
        result.Error.ShouldBeSameAs(MappedError);
    }

    [Theory]
    [MemberData(nameof(MethodCases))]
    public async Task Try_ExceptionDeclined_RethrowsIt(string method, string failure)
    {
        // Arrange
        var exception = new InvalidOperationException("Failed.");
        var arguments = CreateArguments(Methods[method], exception, failure, mappedError: null);

        // Act
        var thrown = await Record.ExceptionAsync(() => InvokeAndAwait(Methods[method], arguments));

        // Assert
        thrown.ShouldBeSameAs(exception);
    }

    #region helper methods

    private static MethodInfo Close(MethodInfo method)
    {
        if (!method.IsGenericMethodDefinition)
            return method;

        var arguments = method.GetGenericArguments()
            .Select(a => a.Name == "TException"
                ? typeof(Exception)
                : typeof(int))
            .ToArray();

        return method.MakeGenericMethod(arguments);
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
        if (Nullable.GetUnderlyingType(type) is { } underlying)
            return TypeName(underlying) + "?";

        if (!type.IsGenericType)
            return type.Name;

        var name = type.Name[..type.Name.IndexOf('`')];

        return $"{name}<{string.Join(", ", type.GetGenericArguments().Select(TypeName))}>";
    }

    // The function (the first parameter) throws the exception or returns a task faulted with it; the mapper (the second) returns the error;
    // the continueOnCapturedContext of TryAsync (the third) is null.
    private static object?[] CreateArguments(MethodInfo method, Exception exception, string failure, Error? mappedError)
    {
        var parameters = method.GetParameters();
        var function = CreateFunction(parameters[0].ParameterType, exception, failure);
        var mapper = CreateMapper(parameters[1].ParameterType, mappedError);

        return parameters.Length == 3
            ? [function, mapper, null]
            : [function, mapper];
    }

    private static Delegate CreateFunction(Type delegateType, Exception exception, string failure)
    {
        var returnType = delegateType.GetMethod("Invoke")!.ReturnType;
        Expression body = failure == Throws
            ? Expression.Throw(Expression.Constant(exception), returnType)
            : Expression.Constant(Faulted(returnType, exception), returnType);

        return Expression.Lambda(delegateType, body).Compile();
    }

    private static object Faulted(Type taskType, Exception exception)
    {
        if (taskType == typeof(Task))
            return Task.FromException(exception);

        if (taskType == typeof(ValueTask))
            return ValueTask.FromException(exception);

        var argument = taskType.GetGenericArguments()[0];
        var definition = taskType.GetGenericTypeDefinition() == typeof(Task<>) ? typeof(Task) : typeof(ValueTask);

        return definition.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Single(m => m is { Name: nameof(Task.FromException), IsGenericMethodDefinition: true })
            .MakeGenericMethod(argument)
            .Invoke(null, [exception])!;
    }

    private static Delegate CreateMapper(Type delegateType, Error? mappedError)
    {
        var parameter = Expression.Parameter(delegateType.GetMethod("Invoke")!.GetParameters()[0].ParameterType, "exception");

        return Expression.Lambda(delegateType, Expression.Constant(mappedError, typeof(Error)), parameter).Compile();
    }

    private static async Task<IOperationResult> InvokeAndAwait(MethodInfo method, object?[] arguments)
    {
        object? returned;

        try
        {
            returned = method.Invoke(null, arguments);
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            throw exception.InnerException;
        }

        if (returned is IOperationResult result)
            return result;

        var task = returned as Task ?? (Task)returned!.GetType().GetMethod(nameof(ValueTask<>.AsTask))!.Invoke(returned, null)!;

        await task;

        return (IOperationResult)task.GetType().GetProperty(nameof(Task<>.Result))!.GetValue(task)!;
    }

    #endregion
}
