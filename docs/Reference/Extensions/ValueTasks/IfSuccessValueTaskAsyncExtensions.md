# IfSuccessValueTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions.ValueTasks](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Provides extension methods for the [Result](../../Result/Result.md) and [Result&lt;T&gt;](../../ResultT/ResultT.md) records that run a side effect on a successful result and return it unchanged.

```csharp
public static class IfSuccessValueTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → IfSuccessValueTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [IfSuccessAsync(Result, Func&lt;ValueTask&gt;, Boolean)](IfSuccessValueTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncresult-funcvaluetask-boolean) | Executes the specified asynchronous action, if the [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&gt;, Boolean)](IfSuccessValueTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresultt-funct-valuetask-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&gt;, Boolean)](IfSuccessValueTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncvaluetaskresult-funcvaluetask-boolean) | Executes the specified asynchronous action, if the provided asynchronous operation returns a successful [Result](../../Result/Result.md), as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&gt;, Boolean)](IfSuccessValueTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctvaluetaskresultt-funct-valuetask-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(ValueTask&lt;Result&gt;, Action, Boolean)](IfSuccessValueTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncvaluetaskresult-action-boolean) | Runs the synchronous action if the result that the `resultTask` returns is successful, and returns the result unchanged. |
| [IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Action&lt;T&gt;, Boolean)](IfSuccessValueTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctvaluetaskresultt-actiont-boolean) | Runs the synchronous action on the value if the result that the `resultTask` returns is successful, and returns the result unchanged. |

## See Also
* [Result](../../Result/Result.md)
* [Result&lt;T&gt;](../../ResultT/ResultT.md)
