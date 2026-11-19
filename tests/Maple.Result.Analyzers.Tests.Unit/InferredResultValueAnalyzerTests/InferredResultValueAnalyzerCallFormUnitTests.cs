// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Analyzers.Tests.Unit.TestingInfrastructure.Helpers;

namespace Maple.Results.Analyzers.Tests.Unit.InferredResultValueAnalyzerTests;

/// <summary>
///     Tests that the <see cref="InferredResultValueAnalyzer" /> (MR0001) finds the method name, and the type arguments that keep a task on
///     purpose, in every form of a call: as an extension method, as a static method, after a null-conditional access, and by its simple name.
/// </summary>
public class InferredResultValueAnalyzerCallFormUnitTests
{
    private const string Members = """
        static Result<int>? maybe = ok;
        """;

    private const string SourceWithUsingStatic = """
        using System;
        using System.Threading.Tasks;
        using Maple.Results;
        using static Maple.Results.Result;

        public static class Snippet
        {
            static readonly Error err = Error.Failure(ErrorUri.None, "e");
            static Task<int> GetIntAsync() => Task.FromResult(5);

            static void Run()
            {
                STATEMENT
            }
        }
        """;

    [Theory]
    [InlineData("var r = ok.Then(x => GetIntAsync());")]
    [InlineData("var r = ThenExtensions.Then(ok, x => GetIntAsync());")]
    [InlineData("var r = maybe?.Then(x => GetIntAsync());")]
    public void Analyze_TaskInferredInAnyCallForm_MarksTheMethodName(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<InferredResultValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        result.MarkedText(diagnostic).ShouldBe("Then");
    }

    [Theory]
    [InlineData("var r = ok.Then<int, Task<int>>(x => GetIntAsync());")]
    [InlineData("var r = ThenExtensions.Then<int, Task<int>>(ok, x => GetIntAsync());")]
    [InlineData("var r = maybe?.Then<int, Task<int>>(x => GetIntAsync());")]
    public void Analyze_TypeArgumentsNamedInAnyCallForm_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<InferredResultValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }

    [Fact]
    public void Analyze_TaskInferredInACallByTheSimpleName_MarksTheMethodName()
    {
        // Arrange
        var source = SourceWithUsingStatic.Replace("STATEMENT", "var r = Try(() => GetIntAsync(), e => err);");

        // Act
        var result = AnalyzerRunner.RunSource(source, [new InferredResultValueAnalyzer()]);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        result.MarkedText(diagnostic).ShouldBe("Try");
    }

    [Fact]
    public void Analyze_TypeArgumentsNamedInACallByTheSimpleName_ReportsNothing()
    {
        // Arrange
        var source = SourceWithUsingStatic.Replace("STATEMENT", "var r = Try<Task<int>>(() => GetIntAsync(), e => err);");

        // Act
        var result = AnalyzerRunner.RunSource(source, [new InferredResultValueAnalyzer()]);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
