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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Result.Tests.Unit.TestingInfrastructure.Helpers;

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

    /// <summary>Whether the parameter is a delegate that returns a <see cref="Task" /> or a <see cref="Task{TResult}" />.</summary>
    internal static bool IsTaskFunction(Type parameterType)
    {
        if (!parameterType.IsGenericType)
            return false;

        var returnType = parameterType.GetGenericArguments()[^1];

        return returnType == typeof(Task) || (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>));
    }

    /// <summary>The overload resolution priority of the method; 0 when it has none.</summary>
    internal static int Priority(MethodInfo method)
    {
        return method.GetCustomAttribute<OverloadResolutionPriorityAttribute>()?.Priority ?? 0;
    }
}
