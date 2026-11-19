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

namespace Maple.Results.Tests.Unit.Binding;

/// <summary>
///     Compiles call shapes against the library and checks what they bind to, which a reflection test of the methods cannot show:
///     the shapes of <c>Result.Try</c> and of the synchronous methods that run, and a side effect given an expression that is not a
///     statement, which the compiler rejects by itself. The library has no overload traps (#223): the shapes that compile into
///     something wrong (e.g., a task that nobody awaits) are reported by the analyzer (rules MR0001 to MR0006 of
///     <c>Maple.Result.Analyzers</c>) and tested with it.
/// </summary>
public class CallShapeBindingUnitTests
{
    [Fact]
    public void Compile_ValidSnippet_HasNoErrors()
    {
        // Act
        var snippet = SnippetCompiler.Compile("var r = Result.Try(() => Compute(), (IOException e) => err);");

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    // A side effect given a lambda whose body is a value that is not a statement (not a call, an assignment or an increment): it does
    // not convert to the action, so the compiler reports it by itself.
    [Theory]
    [InlineData("ok.IfSuccess(x => x + 1);", "CS0201")]
    [InlineData("ok.IfError(e => 0);", "CS0201")]
    public void Compile_SideEffectGivenAnExpressionThatIsNotAStatement_ReportsTheCompilerErrors(string statement, string errorIds)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statement);

        // Assert
        var reported = string.Join(",", snippet.ErrorIds.Distinct().Order(StringComparer.Ordinal));
        reported.ShouldBe(errorIds, string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("Result.Try(() => 5, (IOException e) => err);")]
    [InlineData("Result.Try(() => { }, (IOException e) => err);")]
    [InlineData("Result.Try<int, IOException>(() => 5, e => err);")]
    [InlineData("Result.Try(() => Compute(), (IOException e) => err);")]
    [InlineData("Result.Try(Compute, (IOException e) => err);")]
    [InlineData("Result.Try(() => GetResult(), e => err);")]
    [InlineData("Result.Try(() => GetResultOfInt(), e => err);")]
    [InlineData("Result.Try(() => GetError(), e => err);")]
    [InlineData("ok.IfSuccess(x => { });")]
    [InlineData("ok.Then(x => x + 1);")]
    [InlineData("ok.Else(e => 0);")]
    [InlineData("ok.IfError(e => { _ = SaveAsync(); });")]
    [InlineData("var q = from x in ok select x + 1;")]
    public void Compile_AllowedCallShape_HasNoErrors(string statement)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statement);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    // D1 (#193): a function that returns a result or an error is flattened, not wrapped.
    [Theory]
    [InlineData("var r = Result.Try(() => GetResultOfInt(), e => err);", "Result.Try<int>(Func<Result<int>>, Func<Exception, Error?>)")]
    [InlineData("var r = Result.Try(() => GetResult(), e => err);", "Result.Try(Func<Result>, Func<Exception, Error?>)")]
    [InlineData("var r = Result.Try(() => GetError(), e => err);", "Result.Try(Func<Error>, Func<Exception, Error?>)")]
    [InlineData("var r = Result.Try(() => Compute(), e => err);", "Result.Try<int>(Func<int>, Func<Exception, Error?>)")]
    public void Try_FunctionReturningResultOrError_BindsToTheFlatteningOverload(string statement, string expectedSignature)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statement);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
        snippet.BoundMethod("Try").ShouldBe(expectedSignature);
    }
}
