// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Results.Helpers;
using System;
using System.Threading.Tasks;

namespace Maple.Results.Tests.Unit.Helpers;

// An await uses UnsafeOnCompleted (the awaiter is an ICriticalNotifyCompletion); OnCompleted is the INotifyCompletion member that other
// callers may use (#246).
public class NotNullTaskAwaitableUnitTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);

    [Fact]
    public async Task OnCompleted_PendingTask_RunsContinuationWhenTheTaskCompletes()
    {
        // Arrange
        var source = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        var sut = Guard.EnsureNotNullAwaited(source.Task, "resultTask", continueOnCapturedContext: false);
        var continued = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        // Act
        sut.OnCompleted(() => continued.SetResult());
        source.SetResult("value");
        var completed = await Task.WhenAny(continued.Task, Task.Delay(Timeout, TestContext.Current.CancellationToken));

        // Assert
        completed.ShouldBeSameAs(continued.Task);
    }
}
