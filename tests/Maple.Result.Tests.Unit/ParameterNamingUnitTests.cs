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
// the parameter of an Action, even though its type is a Func. The LINQ parameters keep the names of the query pattern.
public class ParameterNamingUnitTests
{
    private static readonly string[] QueryPatternNames = ["selector", "collectionSelector", "resultSelector"];

    [Fact]
    public void PublicMethods_ParameterOfAsynchronousActionType_IsNamedAction()
    {
        // Act
        var misnamed = typeof(Result).Assembly.GetExportedTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly))
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

    private static bool IsAsynchronousAction(Type type)
    {
        if (!type.IsGenericType || !type.GetGenericTypeDefinition().FullName!.StartsWith("System.Func`", StringComparison.Ordinal))
            return false;

        var returned = type.GetGenericArguments()[^1];

        return returned == typeof(Task)
               || returned == typeof(ValueTask)
               || (returned.IsGenericParameter && returned.GetGenericParameterConstraints().Any(c => c == typeof(Task) || c == typeof(IEquatable<ValueTask>)));
    }
}
