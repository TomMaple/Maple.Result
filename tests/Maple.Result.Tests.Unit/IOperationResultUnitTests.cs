// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Tests.Unit.TestingInfrastructure.Fakes;
using System;
using System.Linq;
using System.Reflection;

namespace Maple.Results.Tests.Unit;

// IOperationResult is closed (#277): an internal member without a body keeps a type of another assembly from implementing it, so that
// Result and Result<T> are its only implementations. What the compiler reports for such a type is in ClosedInterfaceBindingUnitTests.
public class IOperationResultUnitTests
{
    private const string ClosingMember = "CannotBeImplementedOutsideTheLibrary";

    [Fact]
    public void Interface_NonPublicMembers_AreOneInternalMethodWithoutABody()
    {
        // Act
        var result = typeof(IOperationResult).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);

        // Assert
        var method = result.ShouldHaveSingleItem();
        method.Name.ShouldBe(ClosingMember);
        method.IsAssembly.ShouldBeTrue();

        // With a default body, a type of another assembly could implement the interface again.
        method.IsAbstract.ShouldBeTrue();
    }

    [Fact]
    public void Interface_TypesOfTheLibrary_IsImplementedOnlyByResultAndResultOfT()
    {
        // Act
        var result = typeof(IOperationResult).Assembly
            .GetTypes()
            .Where(type => !type.IsInterface && typeof(IOperationResult).IsAssignableFrom(type));

        // Assert
        result.ShouldBe([typeof(Result), typeof(Result<>)], ignoreOrder: true);
    }

    [Fact]
    public void ClosingMember_Result_DoesNothing()
    {
        // Arrange
        IOperationResult sut = Result.Success();

        // Act
        var exception = Record.Exception(() => InvokeClosingMember(sut));

        // Assert
        exception.ShouldBeNull();
    }

    [Fact]
    public void ClosingMember_ResultOfT_DoesNothing()
    {
        // Arrange
        IOperationResult sut = Result.FromValue(1);

        // Act
        var exception = Record.Exception(() => InvokeClosingMember(sut));

        // Assert
        exception.ShouldBeNull();
    }

    // A mocking library creates its mock in the same way: with a type that is generated at run time in another assembly.
    [Fact]
    public void DispatchProxy_CreatedForTheInterface_ThrowsTypeLoadExceptionNamingTheInternalMember()
    {
        // Act
        var exception = Record.Exception(() => DispatchProxy.Create<IOperationResult, InterfaceProxy>());

        // Assert
        exception.ShouldBeOfType<TypeLoadException>();
        var typeLoadException = exception as TypeLoadException;
        typeLoadException.ShouldNotBeNull();
        typeLoadException.Message.ShouldContain(ClosingMember);
    }

    #region helper methods

    /// <summary>
    ///     Calls the internal member by reflection, so that this class compiles before the member exists.
    /// </summary>
    private static void InvokeClosingMember(IOperationResult result)
    {
        var method = typeof(IOperationResult).GetMethod(ClosingMember, BindingFlags.Instance | BindingFlags.NonPublic);
        method.ShouldNotBeNull();
        method.Invoke(result, null);
    }

    #endregion
}
