# MatchValueTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions.ValueTasks](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Provides extension methods that await a [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1) of a [Result](../../Result/Result.md) or a [Result&lt;T&gt;](../../ResultT/ResultT.md) and fold it into one value: a function for the success and a function for the error, which return the same type.

```csharp
public static class MatchValueTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → MatchValueTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [MatchAsync&lt;TOut&gt;(ValueTask&lt;Result&gt;, Func&lt;TOut&gt;, Func&lt;Error, TOut&gt;, Boolean)](MatchValueTaskAsyncExtensions_MatchAsync.md#matchasynctoutvaluetaskresult-functout-funcerror-tout-boolean) | Awaits the `resultTask`, invokes one of the specified synchronous functions depending on whether the [Result](../../Result/Result.md) is successful and returns the value that it returns. |
| [MatchAsync&lt;TOut&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;TOut&gt;&gt;, Func&lt;Error, ValueTask&lt;TOut&gt;&gt;, Boolean)](MatchValueTaskAsyncExtensions_MatchAsync.md#matchasynctoutvaluetaskresult-funcvaluetasktout-funcerror-valuetasktout-boolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result](../../Result/Result.md) is successful, awaits it and returns the value that it returns. |
| [MatchAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](MatchValueTaskAsyncExtensions_MatchAsync.md#matchasyncvaluetaskresult-funcvaluetask-funcerror-valuetask-boolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result](../../Result/Result.md) is successful, and awaits it. |
| [MatchAsync&lt;T, TOut&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, TOut&gt;, Func&lt;Error, TOut&gt;, Boolean)](MatchValueTaskAsyncExtensions_MatchAsync.md#matchasynct-toutvaluetaskresultt-funct-tout-funcerror-tout-boolean) | Awaits the `resultTask`, invokes one of the specified synchronous functions depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) is successful and returns the value that it returns. |
| [MatchAsync&lt;T, TOut&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TOut&gt;&gt;, Func&lt;Error, ValueTask&lt;TOut&gt;&gt;, Boolean)](MatchValueTaskAsyncExtensions_MatchAsync.md#matchasynct-toutvaluetaskresultt-funct-valuetasktout-funcerror-valuetasktout-boolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) is successful, awaits it and returns the value that it returns. |
| [MatchAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](MatchValueTaskAsyncExtensions_MatchAsync.md#matchasynctvaluetaskresultt-funct-valuetask-funcerror-valuetask-boolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) is successful, and awaits it. |

## See Also
* [SwitchValueTaskAsyncExtensions](SwitchValueTaskAsyncExtensions.md)
* [Error](../../Error/Error.md)
* [Result](../../Result/Result.md)
* [Result&lt;T&gt;](../../ResultT/ResultT.md)
