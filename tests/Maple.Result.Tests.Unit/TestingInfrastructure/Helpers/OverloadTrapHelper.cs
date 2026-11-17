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
///     Reflection helpers for the hidden overloads that turn an unsupported call into a compile error (overload traps).
/// </summary>
internal static class OverloadTrapHelper
{
    /// <summary>Whether the method is a trap whose obsolete message starts with the prefix.</summary>
    internal static bool IsTrap(MethodInfo method, string messagePrefix)
    {
        var obsolete = method.GetCustomAttribute<ObsoleteAttribute>();
        return obsolete?.Message is not null && obsolete.Message.StartsWith(messagePrefix, StringComparison.Ordinal);
    }

    /// <summary>Whether the parameter is an <see cref="Action" /> or an <see cref="Action{T}" />.</summary>
    internal static bool IsAction(Type parameterType)
    {
        return parameterType == typeof(Action)
               || (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Action<>));
    }

    /// <summary>
    ///     Whether a type parameter of the method is constrained to <see cref="Task" /> or to <see cref="IEquatable{T}" /> of
    ///     <see cref="ValueTask" />: a trap that only a function that returns a task reaches, not a lambda that only throws (#251).
    /// </summary>
    internal static bool IsConstrainedToATask(MethodInfo method)
    {
        return method.GetGenericArguments()
            .Any(a => a.GetGenericParameterConstraints().Any(c => c == typeof(Task) || c == typeof(IEquatable<ValueTask>)));
    }

    /// <summary>Whether the parameter is a delegate that returns a <see cref="Task" /> or a <see cref="Task{TResult}" />.</summary>
    internal static bool IsTaskFunction(Type parameterType)
    {
        if (!parameterType.IsGenericType)
            return false;

        var returnType = parameterType.GetGenericArguments()[^1];

        return returnType == typeof(Task) || (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>));
    }

    /// <summary>Whether the parameter is a delegate that returns a <see cref="ValueTask" /> or a <see cref="ValueTask{TResult}" />.</summary>
    internal static bool IsValueTaskFunction(Type parameterType)
    {
        if (!parameterType.IsGenericType)
            return false;

        var returnType = parameterType.GetGenericArguments()[^1];

        return returnType == typeof(ValueTask)
               || (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ValueTask<>));
    }

    /// <summary>The overload resolution priority of the method; 0 when it has none.</summary>
    internal static int Priority(MethodInfo method)
    {
        return method.GetCustomAttribute<OverloadResolutionPriorityAttribute>()?.Priority ?? 0;
    }
}
