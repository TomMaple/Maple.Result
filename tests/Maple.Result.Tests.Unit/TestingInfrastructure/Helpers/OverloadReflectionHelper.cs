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
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.TestingInfrastructure.Helpers;

/// <summary>
///     Reflection helpers for the overloads of the extension methods and of <c>Result.Try</c>: the kind of task that a method or a
///     delegate returns, the kind of receiver, and the overload resolution priority.
/// </summary>
internal static class OverloadReflectionHelper
{
    /// <summary>
    ///     The public static methods of the <c>Task</c>-based part of a class that holds the methods of both kinds of task (#248): those that
    ///     return a <see cref="Task" /> or a <see cref="Task{TResult}" />.
    /// </summary>
    internal static MethodInfo[] TaskPartMethods(Type type)
    {
        return [.. type.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(ReturnsTask)];
    }

    /// <summary>
    ///     The public static methods of the <c>ValueTask</c>-based part of a class that holds the methods of both kinds of task (#248):
    ///     those that return a <see cref="ValueTask" /> or a <see cref="ValueTask{TResult}" />.
    /// </summary>
    internal static MethodInfo[] ValueTaskPartMethods(Type type)
    {
        return [.. type.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(ReturnsValueTask)];
    }

    /// <summary>
    ///     Whether the method returns a <see cref="Task" /> or a <see cref="Task{TResult}" />.
    /// </summary>
    internal static bool ReturnsTask(MethodInfo method)
    {
        var returnType = method.ReturnType;

        return returnType == typeof(Task) || (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>));
    }

    /// <summary>
    ///     Whether the method returns a <see cref="ValueTask" /> or a <see cref="ValueTask{TResult}" />.
    /// </summary>
    internal static bool ReturnsValueTask(MethodInfo method)
    {
        var returnType = method.ReturnType;

        return returnType == typeof(ValueTask) || (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ValueTask<>));
    }

    /// <summary>
    ///     Whether the receiver of the extension method is a result, not a task of one.
    /// </summary>
    internal static bool HasSynchronousReceiver(MethodInfo method)
    {
        var receiver = method.GetParameters()[0].ParameterType;
        if (!receiver.IsGenericType)
            return true;

        var definition = receiver.GetGenericTypeDefinition();

        return definition != typeof(Task<>) && definition != typeof(ValueTask<>);
    }

    /// <summary>
    ///     Whether the parameter is an <see cref="Action" /> or an <see cref="Action{T}" />.
    /// </summary>
    internal static bool IsAction(Type parameterType)
    {
        return parameterType == typeof(Action)
               || (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Action<>));
    }

    /// <summary>
    ///     Whether the parameter is a delegate that returns a <see cref="Task" /> or a <see cref="Task{TResult}" />.
    /// </summary>
    internal static bool IsTaskFunction(Type parameterType)
    {
        if (!parameterType.IsGenericType)
            return false;

        var returnType = parameterType.GetGenericArguments()[^1];

        return returnType == typeof(Task) || (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>));
    }

    /// <summary>
    ///     Whether the parameter is a delegate that returns a <see cref="ValueTask" /> or a <see cref="ValueTask{TResult}" />.
    /// </summary>
    internal static bool IsValueTaskFunction(Type parameterType)
    {
        if (!parameterType.IsGenericType)
            return false;

        var returnType = parameterType.GetGenericArguments()[^1];

        return returnType == typeof(ValueTask)
               || (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ValueTask<>));
    }

    /// <summary>
    ///     The overload resolution priority of the method; 0 when it has none.
    /// </summary>
    internal static int Priority(MethodInfo method)
    {
        return method.GetCustomAttribute<OverloadResolutionPriorityAttribute>()?.Priority ?? 0;
    }
}
