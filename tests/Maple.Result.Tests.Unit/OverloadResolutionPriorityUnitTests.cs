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
using Maple.Results.Tests.Unit.TestingInfrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Maple.Results.Tests.Unit;

/// <summary>
///     Where the <see cref="OverloadResolutionPriorityAttribute" /> is, on the extension methods and on <c>Result.Try</c> and
///     <c>Result.TryAsync</c>. An <see langword="async" /> lambda converts to a function that returns a <c>Task</c> and to one that returns
///     a <c>ValueTask</c>, so the overload whose function returns a <c>Task</c> has priority 1, and no other overload has a priority. In
///     <c>LinqAsyncExtensions</c>, only the overloads on a synchronous first source have it. The library has no overload traps (#223),
///     which had most of the attributes before.
/// </summary>
public class OverloadResolutionPriorityUnitTests
{
    private static readonly Dictionary<string, MethodInfo> Methods = typeof(Result).Assembly.GetExportedTypes()
        .Where(t => t.Namespace == typeof(ThenExtensions).Namespace)
        .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
        .Concat(TryMethods())
        .ToDictionary(Describe);

    public static TheoryData<string> MethodCases()
    {
        return new TheoryData<string>(Methods.Keys.Order(StringComparer.Ordinal));
    }

    [Theory]
    [InlineData(typeof(ElseAsyncExtensions), 26, 12)]
    [InlineData(typeof(ElseExtensions), 1, 0)]
    [InlineData(typeof(IfErrorAsyncExtensions), 8, 3)]
    [InlineData(typeof(IfErrorExtensions), 1, 0)]
    [InlineData(typeof(IfSuccessAsyncExtensions), 16, 6)]
    [InlineData(typeof(IfSuccessExtensions), 2, 0)]
    [InlineData(typeof(LinqAsyncExtensions), 112, 16)]
    [InlineData(typeof(LinqExtensions), 24, 0)]
    [InlineData(typeof(MatchAsyncExtensions), 20, 8)]
    [InlineData(typeof(MatchExtensions), 2, 0)]
    [InlineData(typeof(Result), 30, 10)]
    [InlineData(typeof(ResultAsyncExtensions), 2, 0)]
    [InlineData(typeof(ResultExtensions), 1, 0)]
    [InlineData(typeof(SwitchAsyncExtensions), 4, 0)]
    [InlineData(typeof(SwitchExtensions), 2, 0)]
    [InlineData(typeof(ThenAsyncExtensions), 64, 24)]
    [InlineData(typeof(ThenExtensions), 8, 0)]
    public void Methods_OfAClass_HaveTheExpectedNumberOfPriorityAttributes(Type type, int expectedMethods, int expectedAttributes)
    {
        // Arrange
        var methods = Methods.Values.Where(m => m.DeclaringType == type).ToList();

        // Act
        var attributes = methods.Count(m => m.IsDefined(typeof(OverloadResolutionPriorityAttribute)));

        // Assert
        methods.Count.ShouldBe(expectedMethods);
        attributes.ShouldBe(expectedAttributes);
    }

    [Fact]
    public void Methods_OfTheExtensionClasses_Have69PriorityAttributesOn293Methods()
    {
        // Arrange
        var methods = Methods.Values.Where(m => m.DeclaringType != typeof(Result)).ToList();

        // Act
        var attributes = methods.Count(m => m.IsDefined(typeof(OverloadResolutionPriorityAttribute)));

        // Assert
        methods.Count.ShouldBe(293);
        attributes.ShouldBe(69);
    }

    [Theory]
    [MemberData(nameof(MethodCases))]
    public void Priority_OfAMethod_IsOneForAFunctionReturningATaskAndNotSetOtherwise(string method)
    {
        // Arrange
        var sut = Methods[method];
        int? expected = HasPriority(sut) ? 1 : null;

        // Act
        var result = sut.GetCustomAttribute<OverloadResolutionPriorityAttribute>()?.Priority;

        // Assert
        result.ShouldBe(expected);
    }

    private static string Describe(MethodInfo method)
    {
        var parameters = string.Join(", ", method.GetParameters().Select(p => TypeName(p.ParameterType)));
        var typeParameters = method.IsGenericMethodDefinition
            ? $"<{string.Join(", ", method.GetGenericArguments().Select(a => a.Name))}>"
            : "";

        return $"{method.DeclaringType!.Name}.{method.Name}{typeParameters}({parameters})";
    }

    // The rule: a method has the attribute when one of its functions returns a Task or a Task<T>; in LinqAsyncExtensions, only when
    // its first source is a result, not a task of one.
    private static bool HasPriority(MethodInfo method)
    {
        var takesAFunctionReturningATask = method.GetParameters()
            .Select(p => p.ParameterType)
            .Any(t => typeof(Delegate).IsAssignableFrom(t) && OverloadReflectionHelper.IsTaskFunction(t));

        return takesAFunctionReturningATask
               && (method.DeclaringType != typeof(LinqAsyncExtensions) || OverloadReflectionHelper.HasSynchronousReceiver(method));
    }

    private static IEnumerable<MethodInfo> TryMethods()
    {
        return typeof(Result)
            .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(m => m.Name is nameof(Result.Try) or nameof(Result.TryAsync));
    }

    private static string TypeName(Type type)
    {
        if (!type.IsGenericType)
            return type.Name;

        var name = type.Name[..type.Name.IndexOf('`')];

        return $"{name}<{string.Join(", ", type.GetGenericArguments().Select(TypeName))}>";
    }
}
