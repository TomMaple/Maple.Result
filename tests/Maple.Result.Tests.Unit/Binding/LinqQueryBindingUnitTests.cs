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

namespace Maple.Results.Tests.Unit.Binding;

/// <summary>
///     What the query syntax compiles to (#217): a non-generic <see cref="Result" /> in a <c>from</c> clause, and a selected
///     <see cref="Result" /> or <see cref="Result{T}" />, which is not nested. Each query is assigned to a variable of the expected
///     type, so a different binding (e.g., a nested result) is a compile error.
/// </summary>
public class LinqQueryBindingUnitTests
{
    private const string Locals = """
        static Result Check(int value) => value > 0 ? Result.Success() : Error.Failure(ErrorUri.None, "negative");
        static Result<string> ToText(int value) => Result.FromValue(value.ToString());
        static Task<Result<int>> GetOkAsync() => Task.FromResult(Result.FromValue(1));
        static Task<Result> CheckAsync() => Task.FromResult(Result.Success());
        static ValueTask<Result<int>> GetOkValueTask() => new(Result.FromValue(1));
        static ValueTask<Result> CheckValueTask() => new(Result.Success());

        """;

    [Theory]
    [InlineData(Locals + "Result<int> r = from x in ok from _ in GetResult() select x;")]
    [InlineData(Locals + "Result<int> r = from x in ok from _ in Check(x) select x;")]
    [InlineData(Locals + "Result<int> r = from x in ok from _ in GetError() select x;")]
    [InlineData(Locals + "Result<int> r = from _ in GetResult() from x in ok select x;")]
    [InlineData(Locals + "Result<int> r = from _ in GetResult() from checkedAgain in GetResult() select 5;")]
    [InlineData(Locals + "Result<int> r = from _ in GetResult() select 5;")]
    [InlineData(Locals + "Result<int> r = from _ in GetResult() let five = 5 select five;")]
    [InlineData(Locals + "Result<int> r = from x in ok from _ in GetResult() from y in GetResultOfInt() select x + y;")]
    [InlineData(Locals + "Result<int> r = from x in ok from y in GetResultOfInt() from _ in Check(y) select x + y;")]
    [InlineData(Locals + "Result r = from _ in GetResult() from x in ok select GetError();")]
    public void Query_NonGenericResultInFromClause_CompilesToExpectedType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData(Locals + "Result<string> r = from x in ok select ToText(x);")]
    [InlineData(Locals + "Result r = from x in ok select GetResult();")]
    [InlineData(Locals + "Result<string> r = from x in ok from y in GetResultOfInt() select ToText(x + y);")]
    [InlineData(Locals + "Result r = from x in ok from y in GetResultOfInt() select Check(x + y);")]
    [InlineData(Locals + "Result r = from _ in GetResult() select GetResult();")]
    [InlineData(Locals + "Result<string> r = from _ in GetResult() from x in ok select ToText(x);")]
    [InlineData(Locals + "Result<string> r = from x in ok from _ in GetResult() select ToText(x);")]
    public void Query_SelectingResult_CompilesToFlatResult(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData(Locals + "Result<Result<string>> r = ok.Select<int, Result<string>>(x => ToText(x));")]
    [InlineData(Locals + "Result<Result> r = ok.Select<int, Result>(x => GetResult());")]
    public void Select_ExplicitTypeArguments_KeepsNestedResult(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData(Locals + "Result<int> r = from x in ok from y in GetResultOfInt() select x + y;")]
    [InlineData(Locals + "Result r = from x in ok select GetError();")]
    [InlineData(Locals + "Result<int> r = from x in ok select x * 2;")]
    [InlineData(Locals + "Task<Result<int>> r = from x in GetOkAsync() from y in GetResultOfInt() select x + y;")]
    public void Query_ExistingShape_KeepsItsType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData(Locals + "Task<Result<int>> r = from x in GetOkAsync() from _ in CheckAsync() select x;")]
    [InlineData(Locals + "Task<Result<int>> r = from x in ok from _ in CheckAsync() select x;")]
    [InlineData(Locals + "Task<Result<int>> r = from x in GetOkAsync() from _ in GetResult() select x;")]
    [InlineData(Locals + "Task<Result<int>> r = from _ in CheckAsync() from x in GetOkAsync() select x;")]
    [InlineData(Locals + "Task<Result<int>> r = from _ in GetResult() from x in GetOkAsync() select x;")]
    [InlineData(Locals + "Task<Result<int>> r = from _ in CheckAsync() from x in ok select x;")]
    [InlineData(Locals + "Task<Result<int>> r = from _ in CheckAsync() select 5;")]
    [InlineData(Locals + "Task<Result<string>> r = from x in GetOkAsync() select ToText(x);")]
    [InlineData(Locals + "Task<Result> r = from x in GetOkAsync() from y in GetOkAsync() select Check(x + y);")]
    [InlineData(Locals + "ValueTask<Result<int>> r = from x in GetOkValueTask() from _ in CheckValueTask() select x;")]
    [InlineData(Locals + "ValueTask<Result<int>> r = from _ in CheckValueTask() from x in ok select x;")]
    [InlineData(Locals + "ValueTask<Result<string>> r = from _ in CheckValueTask() from x in GetOkValueTask() select ToText(x);")]
    public void Query_AsynchronousNonGenericOrSelectedResult_CompilesToExpectedType(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData(Locals + "var r = from x in ok from _ in GetResult() select SaveAsync();")]
    [InlineData(Locals + "var r = from _ in GetResult() select SaveAsync();")]
    [InlineData(Locals + "var r = from _ in GetResult() select GetIntAsync();")]
    [InlineData(Locals + "var r = from _ in GetResult() from x in ok select GetIntAsync();")]
    [InlineData(Locals + "var r = from _ in GetResult() from x in ok select GetIntValueTask();")]
    [InlineData(Locals + "var r = from x in GetOkAsync() from _ in CheckAsync() select GetIntAsync();")]
    [InlineData(Locals + "var r = from _ in CheckAsync() select SaveAsync();")]
    [InlineData(Locals + "var r = from x in GetOkValueTask() from _ in CheckValueTask() select GetIntValueTask();")]
    public void Query_NonGenericClauseSelectingTask_ReportsCS0619(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements);

        // Assert
        snippet.ErrorIds.ShouldContain("CS0619", string.Join(Environment.NewLine, snippet.Errors));
    }
}
