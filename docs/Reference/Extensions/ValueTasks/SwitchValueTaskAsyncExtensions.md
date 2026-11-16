# SwitchValueTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions.ValueTasks](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/SwitchValueTaskAsyncExtensions.cs" target="_blank">SwitchValueTaskAsyncExtensions.cs</a>

Provides extension methods that await a [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1) of a [Result](../../Result/Result.md) or a [Result&lt;T&gt;](../../ResultT/ResultT.md) and run one of two actions depending on whether the result is successful.

```csharp
public static class SwitchValueTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → SwitchValueTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [SwitchAsync(ValueTask&lt;Result&gt;, Action, Action&lt;Error&gt;, Boolean)](SwitchValueTaskAsyncExtensions_SwitchAsync.md#switchasyncvaluetaskresult-action-actionerror-boolean) | Awaits the `resultTask` and invokes one of the specified actions depending on whether the [Result](../../Result/Result.md) is successful. |
| [SwitchAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Action&lt;T&gt;, Action&lt;Error&gt;, Boolean)](SwitchValueTaskAsyncExtensions_SwitchAsync.md#switchasynctvaluetaskresultt-actiont-actionerror-boolean) | Awaits the `resultTask` and invokes one of the specified actions depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) is successful. |

## See Also
* [MatchValueTaskAsyncExtensions](MatchValueTaskAsyncExtensions.md)
* [Error](../../Error/Error.md)
* [Result](../../Result/Result.md)
* [Result&lt;T&gt;](../../ResultT/ResultT.md)
