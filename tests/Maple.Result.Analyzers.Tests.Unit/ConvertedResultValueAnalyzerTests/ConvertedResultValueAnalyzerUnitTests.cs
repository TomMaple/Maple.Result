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
using Microsoft.CodeAnalysis;

namespace Maple.Results.Analyzers.Tests.Unit.ConvertedResultValueAnalyzerTests;

/// <summary>
///     Tests of the <see cref="ConvertedResultValueAnalyzer" /> (MR0003): a task, a result or an <c>Error</c> that the implicit conversion
///     from a value stores in a <c>Result&lt;T&gt;</c> is reported; a value, and an <c>Error</c> that becomes a failed result, are not.
/// </summary>
public class ConvertedResultValueAnalyzerUnitTests
{
    private const string Members = """
        static Result<object> obj = Result<object>.FromValue(1);
        static Task<Result<object>> objTask = Task.FromResult(obj);
        static Task LogAsync(Error e) => Task.CompletedTask;
        """;

    [Theory]
    [InlineData("Result<object> r = SaveAsync();", "SaveAsync()", "A Task is converted to a successful Result<object> that holds it as its value")]
    [InlineData("Result<object> r = GetIntValueTask();", "GetIntValueTask()",
        "A ValueTask<int> is converted to a successful Result<object> that holds it as its value")]
    [InlineData("Result<object> r = GetResultOfInt();", "GetResultOfInt()",
        "A Result<int> is converted to a successful Result<object> that holds it as its value")]
    [InlineData("Result<object> r = GetResult();", "GetResult()", "A Result is converted to a successful Result<object> that holds it as its value")]
    [InlineData("Result<Task> r = GetIntAsync();", "GetIntAsync()", "A Task<int> is converted to a successful Result<Task> that holds it as its value")]
    [InlineData("var r = obj.Else(e => GetResultOfInt());", "GetResultOfInt()",
        "A Result<int> is converted to a successful Result<object> that holds it as its value")]
    [InlineData("var r = obj.Else(e => LogAsync(e));", "LogAsync(e)", "A Task is converted to a successful Result<object> that holds it as its value")]
    [InlineData("var r = objTask.ElseAsync(e => LogAsync(e));", "LogAsync(e)",
        "A Task is converted to a successful Result<object> that holds it as its value")]
    public void Analyze_TaskOrResultConvertedToAValue_MarksItAndNamesBothTypes(string statements, string markedText, string message)
    {
        // Act
        var result = AnalyzerRunner.Run<ConvertedResultValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Id.ShouldBe(Rules.ConvertedResultValueId);
        diagnostic.Severity.ShouldBe(DiagnosticSeverity.Error);
        result.MarkedText(diagnostic).ShouldBe(markedText);
        diagnostic.GetMessage().ShouldBe(message);
    }

    [Fact]
    public void Analyze_TaskReturnedFromAMethod_ReportsTheReturnedExpression()
    {
        // Act
        var result = AnalyzerRunner.Run<ConvertedResultValueAnalyzer>("var r = ReturnsTask();", "static Result<object> ReturnsTask() => SaveAsync();");

        // Assert
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        result.MarkedText(diagnostic).ShouldBe("SaveAsync()");
    }

    [Theory]
    [InlineData("Result<object> r = err;")]
    [InlineData("Result<object> r = 5;")]
    [InlineData("Result<object> r = \"text\";")]
    [InlineData("Result<int> r = 5;")]
    [InlineData("Result<int> r = err;")]
    [InlineData("Result<object> r = Result<object>.FromValue(GetIntAsync());")]
    [InlineData("var r = obj.Else(e => 0);")]
    [InlineData("var r = obj.Else(e => err);")]
    public void Analyze_ValueOrErrorConverted_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<ConvertedResultValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("Result<Task<int>> r = GetIntAsync();")]
    [InlineData("Result<Task> r = SaveAsync();")]
    [InlineData("Result<ValueTask<int>> r = GetIntValueTask();")]
    [InlineData("Result<Result<int>> r = GetResultOfInt();")]
    [InlineData("var r = (Result<Task<int>>)GetIntAsync();")]
    public void Analyze_TypeArgumentWrittenAsTheTypeOfTheOperand_ReportsNothing(string statements)
    {
        // Act
        var result = AnalyzerRunner.Run<ConvertedResultValueAnalyzer>(statements, Members);

        // Assert
        result.CompilerErrorIds.ShouldBeEmpty(result.CompilerErrors);
        result.DiagnosticIds.ShouldBeEmpty();
    }
}
