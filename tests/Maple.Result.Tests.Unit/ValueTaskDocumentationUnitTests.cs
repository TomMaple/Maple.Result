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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Maple.Results.Tests.Unit;

// The XML documentation of a ValueTask overload is often copied from its Task overload. A ValueTask is a struct, so it cannot be null and
// is not checked for null, and a method that returns one does not return "a task" (#276). The documentation is read from Maple.Result.xml,
// which the build copies next to the library.
public class ValueTaskDocumentationUnitTests
{
    private static readonly Lazy<Dictionary<string, XElement>> Members = new(LoadMembers);

    [Fact]
    public void PublicMethodsWithAValueTask_HaveDocumentation()
    {
        // Act
        var undocumented = MethodsWithAValueTask()
            .Where(m => !Members.Value.ContainsKey(DocumentationId(m)))
            .Select(DocumentationId)
            .ToList();

        // Assert
        undocumented.ShouldBeEmpty();
    }

    [Fact]
    public void ValueTaskParameter_IsNotDocumentedAsNotNull()
    {
        // Act
        var documentedAsNotNull = MethodsWithAValueTask()
            .SelectMany(m => ValueTaskParameters(m).Select(p => (Method: m, Parameter: p)))
            .Where(x => Documentation(x.Method)?.Elements("param").FirstOrDefault(e => (string?)e.Attribute("name") == x.Parameter.Name) is { } param
                        && param.ToString().Contains("""Must not be <see langword="null" />""", StringComparison.Ordinal))
            .Select(x => $"{DocumentationId(x.Method)}: {x.Parameter.Name}")
            .ToList();

        // Assert
        documentedAsNotNull.ShouldBeEmpty();
    }

    [Fact]
    public void ValueTaskParameter_IsNotListedUnderArgumentNullException()
    {
        // Act
        var listed = MethodsWithAValueTask()
            .SelectMany(m => ValueTaskParameters(m).Select(p => (Method: m, Parameter: p)))
            .Where(x => Documentation(x.Method)?.Elements("exception")
                .Where(e => (string?)e.Attribute("cref") == "T:System.ArgumentNullException")
                .SelectMany(e => e.Descendants("paramref"))
                .Any(e => (string?)e.Attribute("name") == x.Parameter.Name) == true)
            .Select(x => $"{DocumentationId(x.Method)}: {x.Parameter.Name}")
            .ToList();

        // Assert
        listed.ShouldBeEmpty();
    }

    [Fact]
    public void MethodThatReturnsAValueTask_IsNotDocumentedAsReturningATask()
    {
        // Act
        var documentedAsTask = MethodsWithAValueTask()
            .Where(m => IsValueTask(m.ReturnType)
                        && Documentation(m)?.Element("returns") is { } returns
                        && Regex.IsMatch(returns.Value, @"^\s*A task\b", RegexOptions.IgnoreCase))
            .Select(DocumentationId)
            .ToList();

        // Assert
        documentedAsTask.ShouldBeEmpty();
    }

    private static IEnumerable<MethodInfo> MethodsWithAValueTask()
    {
        return typeof(Result).Assembly.GetExportedTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            .Where(m => !m.IsSpecialName && !IsTrap(m))
            .Where(m => IsValueTask(m.ReturnType) || m.GetParameters().Any(p => IsValueTask(p.ParameterType)));
    }

    private static IEnumerable<ParameterInfo> ValueTaskParameters(MethodInfo method)
    {
        return method.GetParameters().Where(p => IsValueTask(p.ParameterType));
    }

    private static bool IsTrap(MethodInfo method)
    {
        return method.GetCustomAttribute<ObsoleteAttribute>() is { IsError: true };
    }

    private static bool IsValueTask(Type type)
    {
        return type == typeof(ValueTask) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ValueTask<>));
    }

    private static XElement? Documentation(MethodInfo method)
    {
        return Members.Value.GetValueOrDefault(DocumentationId(method));
    }

    private static Dictionary<string, XElement> LoadMembers()
    {
        var path = Path.ChangeExtension(typeof(Result).Assembly.Location, ".xml");

        return XDocument.Load(path).Root!.Element("members")!.Elements("member").ToDictionary(e => (string)e.Attribute("name")!);
    }

    // The ID that the compiler gives a method in the XML documentation file, e.g.,
    // M:Maple.Results.Result.TryAsync``1(System.Func{System.Threading.Tasks.ValueTask{``0}},System.Func{System.Exception,Maple.Results.Error},System.Nullable{System.Boolean}).
    private static string DocumentationId(MethodInfo method)
    {
        var name = method.IsGenericMethodDefinition ? $"{method.Name}``{method.GetGenericArguments().Length}" : method.Name;
        var parameters = method.GetParameters();
        var list = parameters.Length == 0 ? "" : $"({string.Join(",", parameters.Select(p => TypeId(p.ParameterType)))})";

        return $"M:{method.DeclaringType!.FullName!.Replace('+', '.')}.{name}{list}";
    }

    private static string TypeId(Type type)
    {
        if (type.IsByRef)
            return $"{TypeId(type.GetElementType()!)}@";

        if (type.IsArray)
            return $"{TypeId(type.GetElementType()!)}[]";

        if (type.IsGenericParameter)
            return type.DeclaringMethod is null ? $"`{type.GenericParameterPosition}" : $"``{type.GenericParameterPosition}";

        if (type.IsGenericType)
        {
            var definition = type.GetGenericTypeDefinition().FullName!;

            return $"{definition[..definition.IndexOf('`')].Replace('+', '.')}{{{string.Join(",", type.GetGenericArguments().Select(TypeId))}}}";
        }

        return type.FullName!.Replace('+', '.');
    }
}
