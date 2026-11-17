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

namespace Maple.Results.Tests.Unit;

// The package ID and the assembly name stay Maple.Result; the namespaces are Maple.Results.*,
// so that `Result` is not shadowed by a namespace of the same name (see NamespaceShadowingUnitTests).
public class NamespaceUnitTests
{
    [Theory]
    [InlineData(typeof(Result))]
    [InlineData(typeof(Result<>))]
    [InlineData(typeof(IOperationResult))]
    [InlineData(typeof(Error))]
    [InlineData(typeof(ErrorBuilder))]
    [InlineData(typeof(ErrorCategory))]
    [InlineData(typeof(ErrorDetail))]
    [InlineData(typeof(ErrorUri))]
    [InlineData(typeof(TemplatedMessage))]
    [InlineData(typeof(ResultConfiguration))]
    [InlineData(typeof(ProblemDetailsMembers))]
    public void Namespace_PublicType_IsMapleResults(Type type)
    {
        // Act
        var result = type.Namespace;

        // Assert
        result.ShouldBe("Maple.Results");
    }

    [Fact]
    public void Namespace_ExtensionMethods_IsMapleResultsExtensions()
    {
        // Act
        var result = typeof(IfSuccessExtensions).Namespace;

        // Assert
        result.ShouldBe("Maple.Results.Extensions");
    }

    // The Task-based and the ValueTask-based methods share one class per verb in one namespace (#248).
    [Fact]
    public void Namespace_AsynchronousExtensionMethods_IsMapleResultsExtensions()
    {
        // Act
        var result = typeof(IfSuccessAsyncExtensions).Namespace;

        // Assert
        result.ShouldBe("Maple.Results.Extensions");
    }

    [Fact]
    public void ExportedTypes_NamespaceOfValueTaskExtensionMethods_IsGone()
    {
        // Act
        var result = typeof(Result).Assembly.GetExportedTypes()
            .Where(t => t.Namespace == "Maple.Results.Extensions.ValueTasks")
            .ToList();

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public void AssemblyName_ResultType_IsMapleResult()
    {
        // Act
        var result = typeof(Result).Assembly.GetName().Name;

        // Assert
        result.ShouldBe("Maple.Result");
    }
}
