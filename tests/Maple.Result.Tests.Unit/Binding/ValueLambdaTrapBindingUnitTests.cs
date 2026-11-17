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
///     A lambda whose body is an assignment or an increment has a value, so it binds to the traps for a function whose value would be
///     discarded (#197, #227): the traps stay, and their messages name the assignment and the increment (#238). A statement lambda compiles.
/// </summary>
public class ValueLambdaTrapBindingUnitTests
{
    private const string AssignmentOrIncrement = "an assignment or an increment";

    private const string Members = """
        static readonly Error Err = Error.Failure(ErrorUri.None, "x");
        static int Last;
        static int Count;
        static Error? LastError;
        static Result<int> Ok() => 1;
        static Result<int> Failed() => Err;
        static Task<Error> GetErrorAsync() => Task.FromResult(Err);
        static ValueTask<int> GetIntVt() => new(1);
        static ValueTask<Error> GetErrorVt() => new(Err);
        """;

    [Theory]
    [InlineData("_ = Ok().IfSuccess(x => Last = x);")]
    [InlineData("_ = Ok().IfSuccess(x => Count++);")]
    [InlineData("_ = Ok().IfSuccess(x => ++Count);")]
    [InlineData("_ = Ok().IfSuccess(x => Count += x);")]
    [InlineData("_ = Failed().IfError(e => LastError = e);")]
    [InlineData("_ = Failed().IfError(e => Count++);")]
    public void Sync_AssignmentOrIncrement_ReportsCS0619ThatNamesIt(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
        var message = snippet.Errors.First().GetMessage();
        message.ShouldContain(AssignmentOrIncrement);
    }

    [Theory]
    [InlineData("_ = Task.FromResult(Ok()).IfSuccessAsync(x => Last = x);")]
    [InlineData("_ = Task.FromResult(Ok()).IfSuccessAsync(x => Count++);")]
    [InlineData("_ = Task.FromResult(Failed()).IfErrorAsync(e => LastError = e);")]
    [InlineData("_ = Ok().IfSuccessAsync(async x => Last = await GetIntAsync());")]
    [InlineData("_ = Failed().IfErrorAsync(async e => LastError = await GetErrorAsync());")]
    public void TaskBased_AssignmentOrIncrement_ReportsCS0619ThatNamesIt(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
        var message = snippet.Errors.First().GetMessage();
        message.ShouldContain(AssignmentOrIncrement);
    }

    [Theory]
    [InlineData("_ = new ValueTask<Result<int>>(Ok()).IfSuccessAsync(x => Last = x);")]
    [InlineData("_ = new ValueTask<Result<int>>(Failed()).IfErrorAsync(e => Count++);")]
    [InlineData("_ = Ok().IfSuccessAsync(async x => Last = await GetIntVt());")]
    [InlineData("_ = Failed().IfErrorAsync(async e => LastError = await GetErrorVt());")]
    public void ValueTaskBased_AssignmentOrIncrement_ReportsCS0619ThatNamesIt(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldNotBeEmpty();
        snippet.ErrorIds.ShouldAllBe(id => id == "CS0619", string.Join(Environment.NewLine, snippet.Errors));
        var message = snippet.Errors.First().GetMessage();
        message.ShouldContain(AssignmentOrIncrement);
    }

    [Theory]
    [InlineData("_ = Ok().IfSuccess(x => { Last = x; });")]
    [InlineData("_ = Ok().IfSuccess(x => { Count++; });")]
    [InlineData("_ = Failed().IfError(e => { LastError = e; });")]
    [InlineData("Ok().Switch(x => { Last = x; }, e => { LastError = e; });")]
    public void Sync_StatementLambdaWithAnAssignmentOrAnIncrement_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("_ = Task.FromResult(Ok()).IfSuccessAsync(x => { Last = x; });")]
    [InlineData("_ = Ok().IfSuccessAsync(async x => { Last = await GetIntAsync(); });")]
    [InlineData("_ = Task.FromResult(Ok()).SwitchAsync(x => { Last = x; }, e => { LastError = e; });")]
    public void TaskBased_StatementLambdaWithAnAssignmentOrAnIncrement_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }

    [Theory]
    [InlineData("_ = new ValueTask<Result<int>>(Failed()).IfErrorAsync(e => { Count++; });")]
    [InlineData("_ = Ok().IfSuccessAsync(async x => { Last = await GetIntVt(); });")]
    public void ValueTaskBased_StatementLambdaWithAnAssignmentOrAnIncrement_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, members: Members);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }
}
