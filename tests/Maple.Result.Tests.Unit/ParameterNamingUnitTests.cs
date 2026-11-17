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
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit;

// A delegate that returns a Task or a ValueTask without a value is an asynchronous action: its parameter is named …Action (or action), like
// the parameter of an Action, even though its type is a Func. The LINQ parameters keep the names of the query pattern. An overload trap
// follows another rule: it has the names of the overload that the call would bind to without it (#273).
public class ParameterNamingUnitTests
{
    private static readonly string[] QueryPatternNames = ["selector", "collectionSelector", "resultSelector"];

    [Fact]
    public void PublicMethods_ParameterOfAsynchronousActionType_IsNamedAction()
    {
        // Act
        var misnamed = typeof(Result).Assembly.GetExportedTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            .Where(m => !IsTrap(m))
            .SelectMany(m => m.GetParameters().Select(p => (Method: m, Parameter: p)))
            .Where(x => IsAsynchronousAction(x.Parameter.ParameterType)
                        && !QueryPatternNames.Contains(x.Parameter.Name)
                        && !(x.Parameter.Name!.EndsWith("Action", StringComparison.Ordinal) || x.Parameter.Name == "action"))
            .Select(x => $"{x.Method.DeclaringType!.Name}.{x.Method.Name}({x.Parameter.ParameterType.Name} {x.Parameter.Name})")
            .Distinct()
            .ToList();

        // Assert
        misnamed.ShouldBeEmpty();
    }

    // A trap applies only to a call that uses the names of its parameters. When the function parameter of a trap has a name that no
    // overload that runs has in that position, a call with the name of such an overload gets past the trap and binds to that overload,
    // which keeps or discards the task (#273). A parameter that is an Action is not checked: the traps of Match for two actions keep the
    // names of the 2.x overload that they replace.
    [Fact]
    public void Traps_FunctionParameter_HasTheNameThatAnOverloadThatRunsHasInThatPosition()
    {
        // Act
        var misnamed = typeof(Result).Assembly.GetExportedTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            .Where(IsTrap)
            .SelectMany(m => m.GetParameters().Where(p => IsFunction(p.ParameterType)).Select(p => (Trap: m, Parameter: p)))
            .Where(x => !OverloadsThatRun(x.Trap).Any(m => m.GetParameters().ElementAtOrDefault(x.Parameter.Position)?.Name == x.Parameter.Name))
            .Select(x => $"{x.Trap.DeclaringType!.Name}.{x.Trap.Name}({x.Parameter.ParameterType.Name} {x.Parameter.Name})")
            .Distinct()
            .ToList();

        // Assert
        misnamed.ShouldBeEmpty();
    }

    private static bool IsTrap(MethodInfo method)
    {
        return method.GetCustomAttribute<ObsoleteAttribute>() is { IsError: true };
    }

    private static MethodInfo[] OverloadsThatRun(MethodInfo trap)
    {
        return
        [
            .. trap.DeclaringType!
                .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(m => m.Name == trap.Name && !IsTrap(m))
        ];
    }

    private static bool IsFunction(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition().FullName!.StartsWith("System.Func`", StringComparison.Ordinal);
    }

    private static bool IsAsynchronousAction(Type type)
    {
        if (!IsFunction(type))
            return false;

        var returned = type.GetGenericArguments()[^1];

        return returned == typeof(Task)
               || returned == typeof(ValueTask)
               || (returned.IsGenericParameter && returned.GetGenericParameterConstraints().Any(c => c == typeof(Task) || c == typeof(IEquatable<ValueTask>)));
    }
}
