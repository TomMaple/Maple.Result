// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Analyzers.CodeFixes.Tests.Unit.TestingInfrastructure.Helpers;
using System.Threading.Tasks;

namespace Maple.Results.Analyzers.CodeFixes.Tests.Unit.RenameMethodCodeFixProviderTests;

/// <summary>
///     Tests of the <see cref="RenameMethodCodeFixProvider" /> for a call that becomes asynchronous: the fix awaits the renamed call,
///     makes the containing function <see langword="async" /> when it returns a task and is not asynchronous yet, and only renames the
///     call where it cannot be awaited.
/// </summary>
public class RenameMethodCodeFixProviderAwaitUnitTests
{
    private const string Start = "    static async Task M() { await Task.Yield(); ";

    [Theory]
    [InlineData(
        Start + "var r = ok.{|MR0001:Then|}(x => GetIntAsync()); }",
        Start + "var r = await ok.ThenAsync(x => GetIntAsync()); }")]
    [InlineData(
        Start + "var s = ok.{|MR0001:Then|}(x => GetIntAsync()).IsSuccess; }",
        Start + "var s = (await ok.ThenAsync(x => GetIntAsync())).IsSuccess; }")]
    [InlineData(
        Start + "Console.WriteLine(ok.{|MR0001:Then|}(x => GetIntAsync())); }",
        Start + "Console.WriteLine(await ok.ThenAsync(x => GetIntAsync())); }")]
    [InlineData(
        Start + "var r = Result.{|MR0001:Try|}(() => GetIntAsync(), e => err); }",
        Start + "var r = await Result.TryAsync(() => GetIntAsync(), e => err); }")]
    public async Task RegisterCodeFixes_TaskInferredAsValueInAnAsyncMethod_AwaitsTheAsyncForm(string members, string fixedMembers)
    {
        // Act & Assert
        await CodeFixVerifier<InferredResultValueAnalyzer, RenameMethodCodeFixProvider>.VerifyMembersAsync(members, fixedMembers);
    }

    [Theory]
    [InlineData(
        Start + "ok.IfSuccess(x => {|MR0002:SaveAsync()|}); }",
        Start + "await ok.IfSuccessAsync(x => SaveAsync()); }")]
    [InlineData(
        Start + "var r = ok.IfError(e => {|MR0002:LogAsync(e)|}); }",
        Start + "var r = await ok.IfErrorAsync(e => LogAsync(e)); }")]
    [InlineData(
        Start + "var r = ok.IfError({|MR0002:async e => await LogAsync(e)|}); }",
        Start + "var r = await ok.IfErrorAsync(async e => await LogAsync(e)); }")]
    public async Task RegisterCodeFixes_TaskPassedAsAnActionInAnAsyncMethod_AwaitsTheAsyncForm(string members, string fixedMembers)
    {
        // Act & Assert
        await CodeFixVerifier<UnawaitedTaskAnalyzer, RenameMethodCodeFixProvider>.VerifyMembersAsync(members, fixedMembers);
    }

    [Theory]
    [InlineData(
        "    static Func<Task> f = async () => { await Task.Yield(); ok.IfSuccess(x => {|MR0002:SaveAsync()|}); };",
        "    static Func<Task> f = async () => { await Task.Yield(); await ok.IfSuccessAsync(x => SaveAsync()); };")]
    [InlineData(
        "    static void M() { async Task Local() { await Task.Yield(); ok.IfSuccess(x => {|MR0002:SaveAsync()|}); } _ = Local(); }",
        "    static void M() { async Task Local() { await Task.Yield(); await ok.IfSuccessAsync(x => SaveAsync()); } _ = Local(); }")]
    public async Task RegisterCodeFixes_CallInAnAsyncLambdaOrLocalFunction_AwaitsTheAsyncForm(string members, string fixedMembers)
    {
        // Act & Assert
        await CodeFixVerifier<UnawaitedTaskAnalyzer, RenameMethodCodeFixProvider>.VerifyMembersAsync(members, fixedMembers);
    }

    [Theory]
    [InlineData(
        Start + "ok.IfSuccess(x => {|MR0002:SaveAsync()|}); ok.IfSuccess(x => {|MR0002:GetIntAsync()|}); }",
        Start + "await ok.IfSuccessAsync(x => SaveAsync()); await ok.IfSuccessAsync(x => GetIntAsync()); }")]
    public async Task RegisterCodeFixes_SeveralCallsInAnAsyncMethod_AwaitsAll(string members, string fixedMembers)
    {
        // Act & Assert
        await CodeFixVerifier<UnawaitedTaskAnalyzer, RenameMethodCodeFixProvider>.VerifyMembersAsync(members, fixedMembers);
    }

    [Theory]
    [InlineData(
        "var ok = Result.FromValue(1);\nok.IfSuccess(x => {|MR0002:Task.Delay(1)|});",
        "var ok = Result.FromValue(1);\nawait ok.IfSuccessAsync(x => Task.Delay(1));")]
    public async Task RegisterCodeFixes_CallInTopLevelStatements_AwaitsTheAsyncForm(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<UnawaitedTaskAnalyzer, RenameMethodCodeFixProvider>.VerifyProgramAsync(statements, fixedStatements);
    }

    [Theory]
    [InlineData(
        "    static Task M()\n    {\n        ok.IfSuccess(x => {|MR0002:SaveAsync()|});\n        return SaveAsync();\n    }",
        "    static async Task M()\n    {\n        await ok.IfSuccessAsync(x => SaveAsync());\n        await SaveAsync();\n    }")]
    [InlineData(
        "    static Task<int> M()\n    {\n        ok.IfSuccess(x => {|MR0002:SaveAsync()|});\n        return GetIntAsync();\n    }",
        "    static async Task<int> M()\n    {\n        await ok.IfSuccessAsync(x => SaveAsync());\n        return await GetIntAsync();\n    }")]
    [InlineData(
        "    static Task<int> M()\n    {\n        ok.IfSuccess(x => {|MR0002:SaveAsync()|});\n        return Task.FromResult(5);\n    }",
        "    static async Task<int> M()\n    {\n        await ok.IfSuccessAsync(x => SaveAsync());\n        return 5;\n    }")]
    [InlineData(
        "    static ValueTask M()\n    {\n        ok.IfSuccess(x => {|MR0002:SaveAsync()|});\n        return new ValueTask(SaveAsync());\n    }",
        "    static async ValueTask M()\n    {\n        await ok.IfSuccessAsync(x => SaveAsync());\n"
        + "        await new ValueTask(SaveAsync());\n    }")]
    [InlineData(
        "    static Task<Result<int>> M() => Task.FromResult(ok.IfSuccess(x => {|MR0002:SaveAsync()|}));",
        "    static async Task<Result<int>> M() => await ok.IfSuccessAsync(x => SaveAsync());")]
    public async Task RegisterCodeFixes_CallInAMethodThatReturnsATask_MakesTheMethodAsyncAndAwaitsWhatItReturned(
        string members,
        string fixedMembers)
    {
        // Act & Assert
        await CodeFixVerifier<UnawaitedTaskAnalyzer, RenameMethodCodeFixProvider>.VerifyMembersAsync(members, fixedMembers);
    }

    [Theory]
    [InlineData(
        "    static Task M(bool early)\n    {\n        ok.IfSuccess(x => {|MR0002:SaveAsync()|});\n"
        + "        if (early)\n        {\n            return SaveAsync();\n        }\n\n        return Task.CompletedTask;\n    }",
        "    static async Task M(bool early)\n    {\n        await ok.IfSuccessAsync(x => SaveAsync());\n"
        + "        if (early)\n        {\n            await SaveAsync();\n            return;\n        }\n\n"
        + "        await Task.CompletedTask;\n    }")]
    [InlineData(
        "    static Task M(bool early)\n    {\n        ok.IfSuccess(x => {|MR0002:SaveAsync()|});\n"
        + "        if (early) return SaveAsync();\n        return Task.CompletedTask;\n    }",
        "    static async Task M(bool early)\n    {\n        await ok.IfSuccessAsync(x => SaveAsync());\n"
        + "        if (early) { await SaveAsync(); return; }\n        await Task.CompletedTask;\n    }")]
    [InlineData(
        "    static Task M(bool early)\n    {\n        ok.IfSuccess(x => {|MR0002:SaveAsync()|});\n"
        + "        if (early)\n        {\n            // nothing else to do\n\n            return SaveAsync();\n        }\n\n"
        + "        return Task.CompletedTask;\n    }",
        "    static async Task M(bool early)\n    {\n        await ok.IfSuccessAsync(x => SaveAsync());\n"
        + "        if (early)\n        {\n            // nothing else to do\n\n            await SaveAsync();\n            return;\n        }\n\n"
        + "        await Task.CompletedTask;\n    }")]
    public async Task RegisterCodeFixes_EarlyReturnInAMethodThatReturnsATask_AwaitsTheTaskAndReturns(string members, string fixedMembers)
    {
        // Act & Assert
        await CodeFixVerifier<UnawaitedTaskAnalyzer, RenameMethodCodeFixProvider>.VerifyMembersAsync(members, fixedMembers);
    }

    [Theory]
    [InlineData(
        "    static Task M(bool early)\n    {\n        ok.IfSuccess(x => {|MR0002:SaveAsync()|});\n"
        + "        if (early) {|CS0126:return|};\n        return SaveAsync();\n    }",
        "    static async Task M(bool early)\n    {\n        await ok.IfSuccessAsync(x => SaveAsync());\n"
        + "        if (early) return;\n        await SaveAsync();\n    }")]
    public async Task RegisterCodeFixes_ReturnWithoutATaskInAMethodThatReturnsOne_LeavesIt(string members, string fixedMembers)
    {
        // Act & Assert
        await CodeFixVerifier<UnawaitedTaskAnalyzer, RenameMethodCodeFixProvider>.VerifyMembersAsync(members, fixedMembers);
    }

    [Theory]
    [InlineData(
        "    static Task M()\n    {\n        ok.IfSuccess(x => {|MR0002:SaveAsync()|});\n"
        + "        Func<Task> lambda = () => { return SaveAsync(); };\n"
        + "        Func<int, Task> simpleLambda = n => { return SaveAsync(); };\n"
        + "        Func<Task> anonymousMethod = delegate { return SaveAsync(); };\n"
        + "        Task Local() { return SaveAsync(); }\n        return Local();\n    }",
        "    static async Task M()\n    {\n        await ok.IfSuccessAsync(x => SaveAsync());\n"
        + "        Func<Task> lambda = () => { return SaveAsync(); };\n"
        + "        Func<int, Task> simpleLambda = n => { return SaveAsync(); };\n"
        + "        Func<Task> anonymousMethod = delegate { return SaveAsync(); };\n"
        + "        Task Local() { return SaveAsync(); }\n        await Local();\n    }")]
    public async Task RegisterCodeFixes_MethodWithNestedFunctions_LeavesTheirReturnStatements(string members, string fixedMembers)
    {
        // Act & Assert
        await CodeFixVerifier<UnawaitedTaskAnalyzer, RenameMethodCodeFixProvider>.VerifyMembersAsync(members, fixedMembers);
    }

    [Theory]
    [InlineData(
        "    static Func<Task> f = () =>\n    {\n        ok.IfSuccess(x => {|MR0002:SaveAsync()|});\n        return SaveAsync();\n    };",
        "    static Func<Task> f = async () =>\n    {\n        await ok.IfSuccessAsync(x => SaveAsync());\n        await SaveAsync();\n    };")]
    [InlineData(
        "    static Func<int, Task<int>> f = n =>\n    {\n        ok.IfSuccess(x => {|MR0002:SaveAsync()|});\n"
        + "        return GetIntAsync();\n    };",
        "    static Func<int, Task<int>> f = async n =>\n    {\n        await ok.IfSuccessAsync(x => SaveAsync());\n"
        + "        return await GetIntAsync();\n    };")]
    [InlineData(
        "    static void M()\n    {\n        Task Local()\n        {\n            ok.IfSuccess(x => {|MR0002:SaveAsync()|});\n"
        + "            return SaveAsync();\n        }\n\n        _ = Local();\n    }",
        "    static void M()\n    {\n        async Task Local()\n        {\n            await ok.IfSuccessAsync(x => SaveAsync());\n"
        + "            await SaveAsync();\n        }\n\n        _ = Local();\n    }")]
    [InlineData(
        "    static Func<Task<Result<int>>> f = () => Task.FromResult(ok.IfSuccess(x => {|MR0002:SaveAsync()|}));",
        "    static Func<Task<Result<int>>> f = async () => await ok.IfSuccessAsync(x => SaveAsync());")]
    [InlineData(
        "    static void M()\n    {\n"
        + "        Task<Result<int>> Local() => Task.FromResult(ok.IfSuccess(x => {|MR0002:SaveAsync()|}));\n        _ = Local();\n    }",
        "    static void M()\n    {\n"
        + "        async Task<Result<int>> Local() => await ok.IfSuccessAsync(x => SaveAsync());\n        _ = Local();\n    }")]
    public async Task RegisterCodeFixes_CallInALambdaOrLocalFunctionThatReturnsATask_MakesItAsync(string members, string fixedMembers)
    {
        // Act & Assert
        await CodeFixVerifier<UnawaitedTaskAnalyzer, RenameMethodCodeFixProvider>.VerifyMembersAsync(members, fixedMembers);
    }

    [Theory]
    [InlineData(
        "    static void M() { ok.IfSuccess(x => {|MR0002:SaveAsync()|}); }",
        "    static void M() { ok.IfSuccessAsync(x => SaveAsync()); }")]
    [InlineData(
        "    static Result<int> M() { return ok.IfSuccess(x => {|MR0002:SaveAsync()|}); }",
        "    static Result<int> M() { return {|CS0029:ok.IfSuccessAsync(x => SaveAsync())|}; }")]
    [InlineData(
        "    static Func<Result<int>> f = () => ok.IfSuccess(x => {|MR0002:SaveAsync()|});",
        "    static Func<Result<int>> f = () => {|CS0029:{|CS1662:ok.IfSuccessAsync(x => SaveAsync())|}|};")]
    [InlineData(
        "    static int count = Count();\n    static int Count() => ok.IfSuccess(x => {|MR0002:SaveAsync()|}).IsSuccess ? 1 : 0;",
        "    static int count = Count();\n    static int Count() => ok.IfSuccessAsync(x => SaveAsync()).{|CS1061:IsSuccess|} ? 1 : 0;")]
    [InlineData(
        "    static bool isOk = ok.IfSuccess(x => {|MR0002:SaveAsync()|}).IsSuccess;",
        "    static bool isOk = ok.IfSuccessAsync(x => SaveAsync()).{|CS1061:IsSuccess|};")]
    public async Task RegisterCodeFixes_CallInAMemberThatDoesNotReturnATask_OnlyRenamesIt(string members, string fixedMembers)
    {
        // Act & Assert
        await CodeFixVerifier<UnawaitedTaskAnalyzer, RenameMethodCodeFixProvider>.VerifyMembersAsync(members, fixedMembers);
    }

    [Theory]
    [InlineData(
        "    static readonly object gate = new();\n"
        + Start + "lock (gate) { ok.IfSuccess(x => {|MR0002:SaveAsync()|}); } }",
        "    static readonly object gate = new();\n"
        + Start + "lock (gate) { ok.IfSuccessAsync(x => SaveAsync()); } }")]
    [InlineData(
        "    static Result<int>? maybe = ok;\n"
        + Start + "var r = maybe?.IfSuccess(x => {|MR0002:SaveAsync()|}); }",
        "    static Result<int>? maybe = ok;\n"
        + Start + "var r = maybe?.IfSuccessAsync(x => SaveAsync()); }")]
    [InlineData(
        "    static Task M()\n    {\n        ok.IfSuccess(x => {|MR0002:SaveAsync()|});\n        lock (ok) { return SaveAsync(); }\n    }",
        "    static Task M()\n    {\n        ok.IfSuccessAsync(x => SaveAsync());\n        lock (ok) { return SaveAsync(); }\n    }")]
    public async Task RegisterCodeFixes_CallWhereAnAwaitIsNotAllowed_OnlyRenamesIt(string members, string fixedMembers)
    {
        // Act & Assert
        await CodeFixVerifier<UnawaitedTaskAnalyzer, RenameMethodCodeFixProvider>.VerifyMembersAsync(members, fixedMembers);
    }
}
