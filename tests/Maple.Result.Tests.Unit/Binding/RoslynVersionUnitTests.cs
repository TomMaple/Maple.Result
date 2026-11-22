// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Linq;
using System.Reflection;

namespace Maple.Results.Tests.Unit.Binding;

// The snippets are compiled with the Roslyn packages of the version TestRoslynVersion (Directory.Build.props): 5.0.0 by
// default, the oldest compiler that has to load the analyzers, and the compiler of the newer SDK in its CI job (#330).
// This test fails when the version that the build was given is not the one that compiles the snippets.
public class RoslynVersionUnitTests
{
    [Fact]
    public void CompilerVersion_Loaded_IsTheConfiguredVersion()
    {
        // Arrange
        var configured = typeof(RoslynVersionUnitTests).Assembly
            .GetCustomAttributes<AssemblyMetadataAttribute>()
            .Single(attribute => attribute.Key == "TestRoslynVersion")
            .Value;
        var expected = Version.Parse(configured!);

        // Act
        var result = typeof(CSharpCompilation).Assembly.GetName().Version;

        // Assert
        result.ShouldNotBeNull();
        (result.Major, result.Minor).ShouldBe((expected.Major, expected.Minor));
    }
}
