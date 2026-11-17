// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Reflection;

namespace Maple.Results.Tests.Unit.TestingInfrastructure.Fakes;

/// <summary>
///     A <see cref="DispatchProxy" /> without behaviour. <c>DispatchProxy.Create</c> derives a type from it at run time that implements
///     an interface, as a mocking library does; the class is therefore not sealed.
/// </summary>
public class InterfaceProxy : DispatchProxy
{
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        return null;
    }
}
