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
/// Compiles call shapes against the library and checks what they bind to. The reflection tests of the overload traps check
/// the attributes only, which is why a missing trap (B2 of the review after #188) passed.
/// </summary>
public class OverloadTrapBindingUnitTests
{
    [Fact]
    public void Compile_ValidSnippet_HasNoErrors()
    {
        // Act
        var snippet = SnippetCompiler.Compile("var r = Result.Try(() => Compute(), (IOException e) => err);");

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    // B2 (#191): a task-returning function with a typed mapper.
    [Theory]
    [InlineData("Result.Try(async () => { await Task.Yield(); return 5; }, (IOException e) => err);")]
    [InlineData("Result.Try(() => GetIntValueTask(), (IOException e) => err);")]
    [InlineData("Result.Try(GetIntValueTask, (IOException e) => err);")]
    [InlineData("Result.Try(async () => await GetIntAsync(), (IOException e) => err);")]
    [InlineData("Result.Try(() => GetIntAsync(), (IOException e) => err);")]
    [InlineData("Result.Try(() => SaveAsync(), (IOException e) => err);")]
    // The same shapes with an untyped mapper.
    [InlineData("Result.Try(() => GetIntValueTask(), e => err);")]
    [InlineData("Result.Try(() => GetIntAsync(), e => err);")]
    // Side-effect methods given a function that returns a value (#197).
    [InlineData("ok.IfSuccess(x => x + 1);")]
    [InlineData("ok.IfError(e => 0);")]
    // An asynchronous action in IfError and Match (#124).
    [InlineData("ok.IfError(e => SaveAsync());")]
    public void Compile_TrappedCallShape_ReportsCS0619(string statement)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statement);

        // Assert
        snippet.ErrorIds.ShouldContain("CS0619", string.Join(Environment.NewLine, snippet.Errors));
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

    // A LINQ query that selects a task (#127).
    [Theory]
    [InlineData("var q = from x in ok select GetIntAsync();")]
    [InlineData("var q = from x in ok select SaveAsync();")]
    public void Compile_QuerySelectingTask_ReportsCS0619(string statement)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statement);

        // Assert
        snippet.ErrorIds.ShouldContain("CS0619", string.Join(Environment.NewLine, snippet.Errors));
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
