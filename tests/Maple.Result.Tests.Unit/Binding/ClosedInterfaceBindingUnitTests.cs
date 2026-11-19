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
///     <see cref="IOperationResult" /> is closed (#277): only <see cref="Result" /> and <see cref="Result{T}" /> implement it. A type of
///     another assembly cannot implement the interface, whatever it declares, and code that only uses the interface is not affected.
/// </summary>
public class ClosedInterfaceBindingUnitTests
{
    /// <summary>
    ///     The internal member of the interface, which a type of another assembly cannot implement.
    /// </summary>
    private const string ClosingMember = "CannotBeImplementedOutsideTheLibrary";

    /// <summary>
    ///     The public members of the interface, as a type that tries to implement it declares them.
    /// </summary>
    private const string PublicMembers = """

            public Error? Error => null;
            public bool IsSuccess => false;
            public bool IsError => true;
            public bool TryGetError(out Error? error) { error = null; return true; }

        """;

    private const string UsingMembers = """
        static void Log(IOperationResult result) { if (result.TryGetError(out var error)) Console.WriteLine(error.Title); }
        static T Pass<T>(T result) where T : IOperationResult => result.IfError(e => { });
        """;

    [Theory]
    [InlineData("class")]
    [InlineData("struct")]
    [InlineData("record")]
    public void Implementation_TypeDeclaringThePublicMembers_IsCompileError(string kind)
    {
        // Arrange
        var members = $"{kind} Fake : IOperationResult {{ {PublicMembers} }}";

        // Act
        var snippet = SnippetCompiler.Compile("", members);

        // Assert
        string.Join(",", snippet.ErrorIds).ShouldBe("CS0535", string.Join(Environment.NewLine, snippet.Errors));
    }

    [Fact]
    public void Implementation_TypeImplementingTheInternalMemberExplicitly_IsCompileError()
    {
        // Arrange
        const string Members = $"class Fake : IOperationResult {{ {PublicMembers} void IOperationResult.{ClosingMember}() {{ }} }}";

        // Act
        var snippet = SnippetCompiler.Compile("", Members);

        // Assert
        string.Join(",", snippet.ErrorIds).ShouldBe("CS0122", string.Join(Environment.NewLine, snippet.Errors));
    }

    [Fact]
    public void Implementation_TypeDeclaringAPublicMemberWithTheNameOfTheInternalOne_IsCompileError()
    {
        // Arrange
        const string Members = $"class Fake : IOperationResult {{ {PublicMembers} public void {ClosingMember}() {{ }} }}";

        // Act
        var snippet = SnippetCompiler.Compile("", Members);

        // Assert
        string.Join(",", snippet.ErrorIds).ShouldBe("CS9044", string.Join(Environment.NewLine, snippet.Errors));
    }

    [Fact]
    public void Implementation_DerivedInterfaceGivingTheInternalMemberADefaultBody_IsCompileError()
    {
        // Arrange
        const string Members =
            $"interface IMine : IOperationResult {{ void IOperationResult.{ClosingMember}() {{ }} }}"
            + $"class Fake : IMine {{ {PublicMembers} }}";

        // Act
        var snippet = SnippetCompiler.Compile("", Members);

        // Assert
        string.Join(",", snippet.ErrorIds).ShouldBe("CS0122", string.Join(Environment.NewLine, snippet.Errors));
    }

    // An internal member that is static abstract would close the interface too, but it would turn the interface as a type argument
    // (the two List and Task rows, and IfError on a variable of the interface) into CS8920.
    [Theory]
    [InlineData("IOperationResult r = GetResult(); bool failed = r.IsError;")]
    [InlineData("IOperationResult r = ok; bool failed = r.TryGetError(out var error);")]
    [InlineData("IOperationResult r = GetResult(); IOperationResult r2 = r.IfError(e => { });")]
    [InlineData("var list = new List<IOperationResult> { GetResult(), ok };")]
    [InlineData("Task<IOperationResult> task = Task.FromResult<IOperationResult>(GetResult());")]
    [InlineData("Log(ok);")]
    [InlineData("Result r = Pass(GetResult());")]
    [InlineData("Result<int> r = Pass(ok);")]
    public void Use_InterfaceAsAVariableAParameterATypeArgumentOrAConstraint_Compiles(string statements)
    {
        // Act
        var snippet = SnippetCompiler.Compile(statements, UsingMembers);

        // Assert
        snippet.ErrorIds.ShouldBeEmpty(string.Join(Environment.NewLine, snippet.Errors));
    }
}
