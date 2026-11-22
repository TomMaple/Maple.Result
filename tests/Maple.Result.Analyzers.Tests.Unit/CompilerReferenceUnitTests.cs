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

namespace Maple.Results.Analyzers.Tests.Unit;

// A compiler loads an analyzer only when the analyzer references its own version of the compiler or an older one; a newer
// reference is warning CS9057, and the rules do nothing (#304). The oldest compiler that has to load them is the one of the
// .NET 10 SDK, Roslyn 5.0 (docs/Analyzers/README.md). Dependabot ignores the compiler packages (#331); this test is the guard
// that does not depend on its configuration.
public class CompilerReferenceUnitTests
{
    private static readonly Version OldestSupportedCompiler = new(5, 0, 0, 0);

    [Theory]
    [InlineData("Microsoft.CodeAnalysis")]
    [InlineData("Microsoft.CodeAnalysis.CSharp")]
    public void ReferencedCompiler_AnalyzerAssembly_IsNotNewerThanTheOldestSupportedCompiler(string assemblyName)
    {
        // Act
        var result = typeof(Rules).Assembly
            .GetReferencedAssemblies()
            .Single(reference => reference.Name == assemblyName)
            .Version;

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeLessThanOrEqualTo(OldestSupportedCompiler);
    }
}
