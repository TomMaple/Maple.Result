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
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Maple.Results.Analyzers.CodeFixes.Tests.Unit;

/// <summary>
///     Tests of the <see cref="DiscardResultCodeFixProvider" />: the fix of MR0006 discards the result with <c>_ =</c>, keeps the rest of
///     the statement, and the fixed code compiles and is not reported again.
/// </summary>
public class DiscardResultCodeFixProviderUnitTests
{
    [Theory]
    [InlineData("{|MR0006:GetResult()|};", "_ = GetResult();")]
    [InlineData("{|MR0006:GetResultOfInt()|};", "_ = GetResultOfInt();")]
    [InlineData("{|MR0006:await GetResultAsync()|};", "_ = await GetResultAsync();")]
    [InlineData("{|MR0006:ok.Then(x => x + 1)|};", "_ = ok.Then(x => x + 1);")]
    [InlineData("new List<int> { 1 }.ForEach(x => {|MR0006:GetResult()|});", "new List<int> { 1 }.ForEach(x => _ = GetResult());")]
    public async Task RegisterCodeFixes_ResultThatIsNotUsed_DiscardsIt(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<DiscardedResultAnalyzer, DiscardResultCodeFixProvider>.VerifyAsync(statements, fixedStatements);
    }

    [Theory]
    [InlineData("{|MR0006:GetResult()|}; {|MR0006:GetResultOfInt()|};", "_ = GetResult(); _ = GetResultOfInt();")]
    public async Task RegisterCodeFixes_SeveralResultsThatAreNotUsed_DiscardsAll(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<DiscardedResultAnalyzer, DiscardResultCodeFixProvider>.VerifyAsync(statements, fixedStatements);
    }

    [Theory]
    [InlineData("/* saved */ {|MR0006:GetResult()|}; // done", "/* saved */ _ = GetResult(); // done")]
    public async Task RegisterCodeFixes_StatementWithComments_KeepsThem(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<DiscardedResultAnalyzer, DiscardResultCodeFixProvider>.VerifyAsync(statements, fixedStatements);
    }

    [Theory]
    [InlineData("await Task.Yield(); {|MR0006:GetResultAsync()|};", "await Task.Yield(); _ = await GetResultAsync();")]
    [InlineData("await Task.Yield(); {|MR0006:okTask.ThenAsync(x => x + 1)|};", "await Task.Yield(); _ = await okTask.ThenAsync(x => x + 1);")]
    [InlineData(
        "Func<Task> f = async () => { await Task.Yield(); {|MR0006:GetResultOfIntAsync()|}; };",
        "Func<Task> f = async () => { await Task.Yield(); _ = await GetResultOfIntAsync(); };")]
    public async Task RegisterCodeFixes_TaskThatIsNotAwaitedInAnAsyncFunction_AwaitsItAndDiscardsTheResult(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<DiscardedResultAnalyzer, DiscardResultCodeFixProvider>.VerifyAsync(statements, fixedStatements);
    }

    [Theory]
    [InlineData(
        "{|MR0006:Task.FromResult(Result.Success())|};",
        "_ = await Task.FromResult(Result.Success());")]
    public async Task RegisterCodeFixes_TaskThatIsNotAwaitedInTopLevelStatements_AwaitsItAndDiscardsTheResult(string statements, string fixedStatements)
    {
        // Act & Assert
        await CodeFixVerifier<DiscardedResultAnalyzer, DiscardResultCodeFixProvider>.VerifyProgramAsync(statements, fixedStatements);
    }

    [Theory]
    [InlineData("{|MR0006:GetResultAsync()|};")]
    [InlineData("await Task.Yield(); Action a = () => {|MR0006:GetResultAsync()|};")]
    [InlineData("await Task.Yield(); lock (ok) { {|MR0006:GetResultAsync()|}; }")]
    [InlineData("await Task.Yield(); unsafe { {|MR0006:GetResultAsync()|}; }")]
    [InlineData("await Task.Yield(); {|MR0006:ok?.ThenAsync(x => GetIntAsync())|};")]
    public async Task RegisterCodeFixes_TaskThatCannotBeAwaited_OffersNoFix(string statements)
    {
        // Act & Assert
        await CodeFixVerifier<DiscardedResultAnalyzer, DiscardResultCodeFixProvider>.VerifyAsync(statements, statements);
    }

    [Fact]
    public async Task RegisterCodeFixes_ResultThatIsNotUsed_WritesADiscardThatBindsWithoutReparsing()
    {
        // Arrange
        const string Source = "class C { void M() { Save(); } object Save() => new(); }";

        using var workspace = new AdhocWorkspace();
        var document = workspace.AddProject("Test", LanguageNames.CSharp).AddDocument("Test.cs", Source);
        var call = (await document.GetSyntaxRootAsync(TestContext.Current.CancellationToken))!
            .DescendantNodes()
            .OfType<InvocationExpressionSyntax>()
            .First();

        var descriptor = new DiagnosticDescriptor(Rules.DiscardedResultId, "title", "message", "Usage", DiagnosticSeverity.Warning, true);
        var properties = ImmutableDictionary<string, string?>.Empty.Add(Rules.DiscardFixProperty, Rules.DiscardFix);
        var diagnostic = Diagnostic.Create(descriptor, call.GetLocation(), properties);
        var actions = new List<CodeAction>();
        var context = new CodeFixContext(document, diagnostic, (action, _) => actions.Add(action), TestContext.Current.CancellationToken);

        // Act
        await new DiscardResultCodeFixProvider().RegisterCodeFixesAsync(context);
        var operations = await actions.ShouldHaveSingleItem().GetOperationsAsync(TestContext.Current.CancellationToken);
        var fixedDocument = operations.OfType<ApplyChangesOperation>().Single().ChangedSolution.GetDocument(document.Id)!;
        var fixedRoot = (await fixedDocument.GetSyntaxRootAsync(TestContext.Current.CancellationToken))!;

        // Assert: the tree that the fix produced is compiled as it is, not parsed again from its text, as a chained fix would see it.
        var platform = (string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!;
        var references = platform.Split(Path.PathSeparator).Select(path => MetadataReference.CreateFromFile(path));
        var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
        var compilation = CSharpCompilation.Create("Test", [fixedRoot.SyntaxTree], references, options);
        var assignment = fixedRoot.DescendantNodes().OfType<AssignmentExpressionSyntax>().Single();
        var operation = compilation.GetSemanticModel(fixedRoot.SyntaxTree).GetOperation(assignment, TestContext.Current.CancellationToken);
        operation.ShouldBeAssignableTo<ISimpleAssignmentOperation>()!.Target.ShouldBeAssignableTo<IDiscardOperation>();
        compilation.GetDiagnostics(TestContext.Current.CancellationToken).Where(d => d.Severity == DiagnosticSeverity.Error).ShouldBeEmpty();
    }
}
