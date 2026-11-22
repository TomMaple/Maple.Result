# SwitchAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Extensions/SwitchAsyncExtensions.Task.cs" target="_blank">SwitchAsyncExtensions.Task.cs</a>, <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Extensions/SwitchAsyncExtensions.ValueTask.cs" target="_blank">SwitchAsyncExtensions.ValueTask.cs</a>

Provides extension methods that await a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1) or a [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1) of a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) and run one of two actions depending on whether the result is successful.

```csharp
public static class SwitchAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → SwitchAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [SwitchAsync(Task&lt;Result&gt;, Action, Action&lt;Error&gt;, Nullable&lt;Boolean&gt;)](SwitchAsyncExtensions_SwitchAsync.md#switchasynctaskresult-action-actionerror-nullableboolean) | Awaits the `resultTask` and invokes one of the specified actions depending on whether the [Result](../Result/Result.md) is successful. |
| [SwitchAsync(ValueTask&lt;Result&gt;, Action, Action&lt;Error&gt;, Nullable&lt;Boolean&gt;)](SwitchAsyncExtensions_SwitchAsync.md#switchasyncvaluetaskresult-action-actionerror-nullableboolean) | Awaits the `resultTask` and invokes one of the specified actions depending on whether the [Result](../Result/Result.md) is successful. |
| [SwitchAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Action&lt;T&gt;, Action&lt;Error&gt;, Nullable&lt;Boolean&gt;)](SwitchAsyncExtensions_SwitchAsync.md#switchasyncttaskresultt-actiont-actionerror-nullableboolean) | Awaits the `resultTask` and invokes one of the specified actions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful. |
| [SwitchAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Action&lt;T&gt;, Action&lt;Error&gt;, Nullable&lt;Boolean&gt;)](SwitchAsyncExtensions_SwitchAsync.md#switchasynctvaluetaskresultt-actiont-actionerror-nullableboolean) | Awaits the `resultTask` and invokes one of the specified actions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful. |

## See Also
* [MatchAsyncExtensions](MatchAsyncExtensions.md)
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
