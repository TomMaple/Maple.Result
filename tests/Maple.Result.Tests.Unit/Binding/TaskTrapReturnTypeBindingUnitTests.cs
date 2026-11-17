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
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Binding;

/// <summary>
///     A task trap returns the type that the call would have without it (#274): <c>IfSuccess</c> the type of its receiver, and
///     <c>Then</c>, <c>Select</c> and <c>SelectMany</c> a result of the task. A call that continues with the result then reports CS0619
///     only, not a second error about a type that is not in the code of the caller.
/// </summary>
public class TaskTrapReturnTypeBindingUnitTests
{
    private const string Members = """
        static Task<Result<int>> okTask = Task.FromResult(Result.FromValue(1));
        static ValueTask<Result<int>> okVt = new(Result.FromValue(1));
        static Result done = Result.Success();
        static Task<Result> doneTask = Task.FromResult(Result.Success());
        static ValueTask<Result> doneVt = new(Result.Success());
        static ValueTask SaveValueTask() => default;
        """;

    private static readonly string[] TrapNames = ["IfSuccess", "Then", "Select", "SelectMany"];

    [Theory]
    // IfSuccess keeps the type of its receiver
    [InlineData("Result<int> r = ok.IfSuccess(x => SaveAsync());")]
    [InlineData("Result<int> r = ok.IfSuccess(x => SaveValueTask());")]
    [InlineData("Result<int> r = ok.IfSuccess(x => GetIntValueTask());")]
    [InlineData("var r = ok.IfSuccess(x => SaveAsync()).Then(x => x + 1);")]
    [InlineData("var r = ok.IfSuccess(x => GetIntAsync()).Value;")]
    // Then gives a result of the task
    [InlineData("var r = ok.Then(x => GetIntAsync()).Then(t => 1);")]
    [InlineData("var r = ok.Then(x => GetIntAsync()).IfSuccess(t => { });")]
    [InlineData("var r = ok.Then(x => GetIntAsync()).Value;")]
    [InlineData("var r = ok.Then(x => GetIntAsync()).Match(t => 1, e => 0);")]
    [InlineData("var r = ok.Then(x => SaveAsync()).Then(t => 1);")]
    [InlineData("var r = ok.Then(x => SaveValueTask()).Then(t => 1);")]
    [InlineData("var r = ok.Then(x => GetIntValueTask()).Then(t => 1);")]
    [InlineData("Result<Task<int>> r = ok.Then(x => GetIntAsync());")]
    [InlineData("var r = done.Then(() => GetIntAsync()).Then(t => 1);")]
    [InlineData("var r = done.Then(() => SaveValueTask()).Value;")]
    [InlineData("Result<ValueTask<int>> r = done.Then(() => GetIntValueTask());")]
    // Select gives a result of the task
    [InlineData("var r = (from x in ok select GetIntAsync()).Value;")]
    [InlineData("var r = (from x in ok select SaveValueTask()).Then(t => 1);")]
    [InlineData("var r = (from x in ok select GetIntValueTask()).Value;")]
    [InlineData("var r = (from x in done select GetIntAsync()).Value;")]
    [InlineData("async Task L() { var r = (await (from x in okTask select GetIntAsync())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in okVt select SaveValueTask())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in doneTask select GetIntValueTask())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in doneVt select GetIntAsync())).Value; } _ = L();")]
    // SelectMany gives a result of the task, on every receiver
    [InlineData("var r = (from x in ok from y in ok select GetIntAsync()).Value;")]
    [InlineData("var r = (from x in ok from y in done select SaveValueTask()).Value;")]
    [InlineData("var r = (from x in done from y in ok select GetIntValueTask()).Value;")]
    [InlineData("var r = (from x in done from y in done select GetIntAsync()).Value;")]
    [InlineData("async Task L() { var r = (await (from x in okTask from y in okTask select GetIntAsync())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in okTask from y in ok select SaveValueTask())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in ok from y in okTask select GetIntAsync())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in okVt from y in okVt select GetIntValueTask())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in ok from y in okVt select GetIntAsync())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in doneTask from y in doneTask select GetIntAsync())).Value; } _ = L();")]
    [InlineData("async Task L() { var r = (await (from x in doneVt from y in done select SaveAsync())).Value; } _ = L();")]
    public void TaskTrap_CallContinuedWithTheResult_ReportsOnlyCS0619(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.Distinct().ShouldBe(["CS0619"], string.Join(Environment.NewLine, snippet.Errors));
    }

    [Fact]
    public void IfSuccessTaskTraps_ReturnTheTypeOfTheReceiver()
    {
        // Arrange
        var traps = TaskTraps().Where(x => x.Method.Name == "IfSuccess").ToList();

        // Act
        var mismatched = traps
            .Where(x => x.Method.ReturnType != x.Method.GetParameters()[0].ParameterType)
            .Select(x => x.Method.ToString())
            .ToList();

        // Assert
        traps.Count.ShouldBe(6);
        mismatched.ShouldBeEmpty();
    }

    [Fact]
    public void ThenSelectAndSelectManyTaskTraps_ReturnAResultOfTheTask()
    {
        // Arrange
        var traps = TaskTraps().Where(x => x.Method.Name != "IfSuccess").ToList();

        // Act
        var mismatched = traps
            .Where(x => Unwrap(x.Method.ReturnType) != typeof(Result<>).MakeGenericType(x.TaskType))
            .Select(x => x.Method.ToString())
            .ToList();

        // Assert
        traps.Count.ShouldBeGreaterThan(100);
        mismatched.ShouldBeEmpty();
    }

    // The traps whose delegate returns a type parameter constrained to a task (TTask or TValueTask), or a ValueTask<TIgnored>, with the
    // type that delegate returns.
    private static (MethodInfo Method, Type TaskType)[] TaskTraps()
    {
        return
        [
            .. typeof(ThenExtensions).Assembly.GetExportedTypes()
                .Where(t => t.Namespace == typeof(ThenExtensions).Namespace)
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                .Where(m => TrapNames.Contains(m.Name) && m.GetCustomAttribute<ObsoleteAttribute>() is { IsError: true })
                .Select(m => (Method: m, TaskType: m.GetParameters().Select(p => DelegateTaskType(m, p.ParameterType)).LastOrDefault(t => t is not null)))
                .Where(x => x.TaskType is not null)
                .Select(x => (x.Method, x.TaskType!)),
        ];
    }

    private static Type? DelegateTaskType(MethodInfo method, Type parameterType)
    {
        if (!parameterType.IsGenericType || !parameterType.Name.StartsWith("Func`", StringComparison.Ordinal))
            return null;

        var returned = parameterType.GetGenericArguments()[^1];
        var methodTypeParameters = method.GetGenericArguments();

        var constrainedToTask = returned.IsGenericParameter
                                && returned.GetGenericParameterConstraints().Any(c => c == typeof(Task) || c == typeof(IEquatable<ValueTask>));
        var valueTaskOfIgnored = returned.IsGenericType
                                 && returned.GetGenericTypeDefinition() == typeof(ValueTask<>)
                                 && methodTypeParameters.Contains(returned.GetGenericArguments()[0]);

        return constrainedToTask || valueTaskOfIgnored ? returned : null;
    }

    private static Type Unwrap(Type type)
    {
        return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Task<>) || type.GetGenericTypeDefinition() == typeof(ValueTask<>))
            ? type.GetGenericArguments()[0]
            : type;
    }
}
