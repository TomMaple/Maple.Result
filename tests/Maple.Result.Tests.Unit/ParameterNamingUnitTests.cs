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

    // The values of a message template have one name in the whole API, after TemplatedMessage.Parameters: parameters next to templateId,
    // and detailParameters next to detailTemplateId.
    [Fact]
    public void PublicMethods_ValuesOfMessageTemplate_AreNamedParameters()
    {
        // Act
        var misnamed = typeof(Result).Assembly.GetExportedTypes()
            .SelectMany(t => t.GetMembers(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            .OfType<MethodBase>()
            .Select(m => (Method: m, Parameters: m.GetParameters()))
            .SelectMany(x => x.Parameters
                .Where(p => p.Name!.EndsWith("emplateId", StringComparison.Ordinal))
                .Select(p => (x.Method, TemplateId: p, Values: x.Parameters.FirstOrDefault(v => IsTemplateValues(v.ParameterType)))))
            .Where(x => x.Values is not null && x.Values.Name != ExpectedValuesName(x.TemplateId.Name!))
            .Select(x => $"{x.Method.DeclaringType!.Name}.{x.Method.Name}({x.TemplateId.Name}, {x.Values!.Name})")
            .Distinct()
            .ToList();

        // Assert
        misnamed.ShouldBeEmpty();
    }

    private static string ExpectedValuesName(string templateIdName)
    {
        var prefix = templateIdName[..^"TemplateId".Length];

        return prefix.Length == 0
            ? "parameters"
            : $"{prefix}Parameters";
    }

    private static bool IsTemplateValues(Type type)
    {
        return type == typeof(ReadOnlySpan<(string, object?)>) || type == typeof(IEnumerable<KeyValuePair<string, object?>>);
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
