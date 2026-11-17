// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Tests.Unit.TestingInfrastructure.Fakes;
using Maple.Results.Tests.Unit.TestingInfrastructure.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit;

/// <summary>
///     The <c>continueOnCapturedContext</c> argument of every asynchronous method that has one, checked by reflection (#246): the argument
///     wins over <see cref="ResultConfiguration.ContinueOnCapturedContext" />, so the returned task completes on the captured context when it is
///     <see langword="true" />, and off it when it is <see langword="false" />.
/// </summary>
/// <remarks>
///     Every task that the method awaits (the source and the tasks that the passed functions return) waits on a gate, which a thread-pool
///     thread opens after the method has started awaiting. The method therefore resumes after an <c>await</c>, and the context on which its task
///     completes depends only on the effective <c>continueOnCapturedContext</c>. The source of <c>ElseAsync</c> and <c>IfErrorAsync</c> is
///     failed, so that their function runs.
/// </remarks>
[Collection(ResultConfigurationCollection.Name)]
public sealed class ContinueOnCapturedContextUnitTests : IDisposable
{
    private static readonly Error SourceError = Error.Failure(ErrorUri.None, "8a2f8f8e-7f1c-4d0e-9f53-3c7d9b0e6a41", "Source error");

    private static readonly Dictionary<string, MethodInfo> Methods = typeof(Result).Assembly.GetExportedTypes()
        .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
        .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is null)
        .Where(m => m.GetParameters().Any(p => p.Name == "continueOnCapturedContext"))
        .ToDictionary(Describe, Close);

    public void Dispose()
    {
        ResultConfiguration.ContinueOnCapturedContext = false;
    }

    public static TheoryData<string> MethodCases()
    {
        return new TheoryData<string>(Methods.Keys.Order(StringComparer.Ordinal));
    }

    [Fact]
    public void Methods_WithContinueOnCapturedContext_AreFound()
    {
        // Act & Assert
        Methods.Count.ShouldBe(160);
    }

    [Theory]
    [MemberData(nameof(MethodCases))]
    public void AsyncMethod_ArgumentTrueAndGlobalSettingFalse_CompletesOnCapturedContext(string method)
    {
        // Arrange
        ResultConfiguration.ContinueOnCapturedContext = false;

        // Act
        var (context, completionContext) = Run(Methods[method], continueOnCapturedContext: true);

        // Assert
        completionContext.ShouldBeSameAs(context);
    }

    [Theory]
    [MemberData(nameof(MethodCases))]
    public void AsyncMethod_ArgumentFalseAndGlobalSettingTrue_CompletesOffCapturedContext(string method)
    {
        // Arrange
        ResultConfiguration.ContinueOnCapturedContext = true;

        // Act
        var (context, completionContext) = Run(Methods[method], continueOnCapturedContext: false);

        // Assert
        completionContext.ShouldNotBeSameAs(context);
    }

    #region helper methods

    private static (SynchronizationContext Context, SynchronizationContext? CompletionContext) Run(MethodInfo method, bool continueOnCapturedContext)
    {
        return SingleThreadSynchronizationContext.Run(async context =>
        {
            var gate = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            var arguments = CreateArguments(method, gate.Task, continueOnCapturedContext);

            var returned = method.Invoke(null, arguments);
            var task = AsTask(returned);

            SynchronizationContext? completionContext = null;
            var observed = task.ContinueWith(
                _ => completionContext = SynchronizationContext.Current,
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);

            ThreadPool.QueueUserWorkItem(_ => gate.SetResult());
            await observed;
            await task;

            return ((SynchronizationContext)context, completionContext);
        });
    }

    private static MethodInfo Close(MethodInfo method)
    {
        if (!method.IsGenericMethodDefinition)
            return method;

        var arguments = method.GetGenericArguments()
            .Select(a => a.Name switch
            {
                "TException" => typeof(Exception),
                "TResult" => typeof(Result),
                _ => typeof(int)
            })
            .ToArray();

        return method.MakeGenericMethod(arguments);
    }

    private static string Describe(MethodInfo method)
    {
        var parameters = string.Join(", ", method.GetParameters().Select(p => TypeName(p.ParameterType)));
        var typeParameters = method.IsGenericMethodDefinition
            ? $"<{string.Join(", ", method.GetGenericArguments().Select(a => a.Name))}>"
            : "";

        return $"{method.DeclaringType!.Name}.{method.Name}{typeParameters}({parameters})";
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

    private static object?[] CreateArguments(MethodInfo method, Task gate, bool continueOnCapturedContext)
    {
        var successfulSource = method.Name is not ("ElseAsync" or "IfErrorAsync");
        var parameters = method.GetParameters();
        var arguments = new object?[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            var type = parameters[i].ParameterType;

            if (type == typeof(bool?))
                arguments[i] = continueOnCapturedContext;
            else
                arguments[i] = CreateValue(type, gate, successful: i > 0 || successfulSource);
        }

        return arguments;
    }

    private static object? CreateValue(Type type, Task gate, bool successful)
    {
        if (type == typeof(int))
            return 1;

        if (type == typeof(Error))
            return SourceError;

        if (type == typeof(Result))
        {
            return successful
                ? Result.Success()
                : Result.FromError(SourceError);
        }

        if (type == typeof(Task))
            return gate.ContinueWith(static _ => { }, TaskScheduler.Default);

        if (type == typeof(ValueTask))
            return new ValueTask(gate.ContinueWith(static _ => { }, TaskScheduler.Default));

        if (typeof(Delegate).IsAssignableFrom(type))
            return CreateDelegate(type, gate);

        if (type.IsGenericType)
        {
            var definition = type.GetGenericTypeDefinition();
            var argument = type.GetGenericArguments()[0];

            if (definition == typeof(Result<>))
            {
                return successful
                    ? type.GetMethod(nameof(Result<>.FromValue))!.Invoke(null, [CreateValue(argument, gate, successful: true)])
                    : type.GetMethod(nameof(Result<>.FromError))!.Invoke(null, [SourceError]);
            }

            if (definition == typeof(Task<>))
                return After(gate, argument, CreateValue(argument, gate, successful));

            if (definition == typeof(ValueTask<>))
                return type.GetConstructor([typeof(Task<>).MakeGenericType(argument)])!.Invoke([After(gate, argument, CreateValue(argument, gate, successful))]);
        }

        throw new NotSupportedException($"No test value for {type}.");
    }

    // A task that completes with the value after the gate opens.
    private static object After(Task gate, Type type, object? value)
    {
        return typeof(ContinueOnCapturedContextUnitTests).GetMethod(nameof(AfterGate), BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(type)
            .Invoke(null, [gate, value])!;
    }

    private static Task<T> AfterGate<T>(Task gate, T value)
    {
        return gate.ContinueWith(_ => value, TaskScheduler.Default);
    }

    private static Delegate CreateDelegate(Type delegateType, Task gate)
    {
        var invoke = delegateType.GetMethod("Invoke")!;
        var parameters = invoke.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();
        Expression body = invoke.ReturnType == typeof(void)
            ? Expression.Empty()
            : Expression.Constant(CreateValue(invoke.ReturnType, gate, successful: true), invoke.ReturnType);

        return Expression.Lambda(delegateType, body, parameters).Compile();
    }

    private static Task AsTask(object? returned)
    {
        return returned switch
        {
            Task task => task,
            ValueTask valueTask => valueTask.AsTask(),
            not null when returned.GetType().IsGenericType && returned.GetType().GetGenericTypeDefinition() == typeof(ValueTask<>)
                => (Task)returned.GetType().GetMethod(nameof(ValueTask<>.AsTask))!.Invoke(returned, null)!,
            _ => throw new NotSupportedException($"Not a task: {returned}.")
        };
    }

    #endregion
}
