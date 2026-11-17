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
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Extensions;

/// <summary>
///     The priority rule of the merged classes (#248): an <see langword="async" /> lambda converts to a function that returns a
///     <see cref="Task" /> and to one that returns a <see cref="ValueTask" />, so on a result every overload of the <c>Task</c>-based part
///     has a priority one above its twin in the <c>ValueTask</c>-based part (the same overload with a <c>ValueTask</c> function). Without it,
///     such a call is ambiguous (CS0121), and only a binding test of that exact shape would notice.
/// </summary>
public class AsyncExtensionsPriorityUnitTests
{
    [Theory]
    [InlineData(typeof(ThenAsyncExtensions), 8)]
    [InlineData(typeof(ElseAsyncExtensions), 4)]
    [InlineData(typeof(IfSuccessAsyncExtensions), 4)]
    [InlineData(typeof(IfErrorAsyncExtensions), 2)]
    [InlineData(typeof(LinqAsyncExtensions), 28)]
    public void TaskPartOverloadOnAResult_HasPriorityOneAboveItsValueTaskTwin(Type type, int expectedCount)
    {
        // Arrange
        var valueTaskPart = OverloadTrapHelper.ValueTaskPartMethods(type)
            .Where(OverloadTrapHelper.HasSynchronousReceiver)
            .ToDictionary(m => Signature(m, withValueTaskFunction: false));

        var taskPart = OverloadTrapHelper.TaskPartMethods(type)
            .Where(OverloadTrapHelper.HasSynchronousReceiver)
            .ToList();

        // Act
        var mismatched = taskPart
            .Where(m => !valueTaskPart.TryGetValue(Signature(m, withValueTaskFunction: true), out var twin)
                        || OverloadTrapHelper.Priority(m) != OverloadTrapHelper.Priority(twin) + 1)
            .Select(m => m.ToString())
            .ToList();

        // Assert
        taskPart.Count.ShouldBe(expectedCount);
        mismatched.ShouldBeEmpty();
    }

    // On a task of a result, an async lambda binds to the overload for a function that returns a Task, on both kinds of receiver
    // (#226): every overload whose delegates return a Task has priority 1, and every overload whose delegates return a ValueTask has
    // priority 0. The receiver decides the kind of the returned task.
    [Theory]
    [InlineData(typeof(ThenAsyncExtensions))]
    [InlineData(typeof(ElseAsyncExtensions))]
    [InlineData(typeof(IfSuccessAsyncExtensions))]
    [InlineData(typeof(IfErrorAsyncExtensions))]
    [InlineData(typeof(MatchAsyncExtensions))]
    public void OverloadOnATaskOfAResult_HasPriorityOneForTaskDelegatesAndZeroForValueTaskDelegates(Type type)
    {
        // Arrange
        var onATask = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => !OverloadTrapHelper.HasSynchronousReceiver(m))
            .ToList();

        // Act
        var mismatched = onATask
            .Where(m => (Delegates(m).All(OverloadTrapHelper.IsTaskFunction) && OverloadTrapHelper.Priority(m) != 1)
                        || (Delegates(m).All(OverloadTrapHelper.IsValueTaskFunction) && OverloadTrapHelper.Priority(m) != 0))
            .Select(m => m.ToString())
            .ToList();

        // Assert
        onATask.Count(m => Delegates(m).All(OverloadTrapHelper.IsValueTaskFunction)).ShouldBeGreaterThan(0);
        mismatched.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(typeof(MatchAsyncExtensions))]
    [InlineData(typeof(SwitchAsyncExtensions))]
    [InlineData(typeof(ResultAsyncExtensions))]
    public void MethodsOfATaskOfAResultOnly_HaveNoOverloadOnAResult(Type type)
    {
        // Act
        var onAResult = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(OverloadTrapHelper.HasSynchronousReceiver)
            .ToList();

        // Assert
        onAResult.ShouldBeEmpty();
    }

    // The delegate parameters of the method (an empty list is never "all of a kind").
    private static Type[] Delegates(MethodInfo method)
    {
        var delegates = method.GetParameters()
            .Select(p => p.ParameterType)
            .Where(t => typeof(Delegate).IsAssignableFrom(t))
            .ToArray();

        return delegates.Length == 0 ? [typeof(object)] : delegates;
    }

    // The name, the number of type parameters and the parameter types; with withValueTaskFunction, a Task in the type of the first
    // function (the second parameter) becomes a ValueTask, which gives the signature of the twin in the ValueTask-based part.
    private static string Signature(MethodInfo method, bool withValueTaskFunction)
    {
        var parameters = method.GetParameters()
            .Select((p, i) => Format(p.ParameterType, withValueTaskFunction && i == 1));

        return $"{method.Name}`{method.GetGenericArguments().Length}({string.Join(", ", parameters)})";
    }

    private static string Format(Type type, bool withValueTask)
    {
        if (type.IsGenericMethodParameter)
            return "!!" + type.GenericParameterPosition;

        if (withValueTask && type == typeof(Task))
            return typeof(ValueTask).FullName!;

        if (!type.IsGenericType)
            return type.FullName ?? type.Name;

        var definition = type.GetGenericTypeDefinition();
        var name = withValueTask && definition == typeof(Task<>) ? typeof(ValueTask<>).FullName! : definition.FullName!;
        var arguments = type.GetGenericArguments().Select(a => Format(a, withValueTask));

        return $"{name}[{string.Join(", ", arguments)}]";
    }
}
