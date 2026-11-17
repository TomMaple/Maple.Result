# IfSuccessAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessAsyncExtensions.Task.cs" target="_blank">IfSuccessAsyncExtensions.Task.cs</a>, <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessAsyncExtensions.ValueTask.cs" target="_blank">IfSuccessAsyncExtensions.ValueTask.cs</a>

Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task) or [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records that run a side effect on a successful result and return it unchanged.

```csharp
public static partial class IfSuccessAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → IfSuccessAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [IfSuccessAsync(Result, Func&lt;Task&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncresult-functask-boolean) | Executes the specified asynchronous action, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresultt-funct-task-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(Task&lt;Result&gt;, Func&lt;Task&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctaskresult-functask-boolean) | Executes the specified asynchronous action, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncttaskresultt-funct-task-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(Task&lt;Result&gt;, Action, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctaskresult-action-boolean) | Runs the synchronous action if the result that the `resultTask` returns is successful, and returns the result unchanged. |
| [IfSuccessAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Action&lt;T&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncttaskresultt-actiont-boolean) | Runs the synchronous action on the value if the result that the `resultTask` returns is successful, and returns the result unchanged. |
| [IfSuccessAsync(Task&lt;Result&gt;, Func&lt;ValueTask&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctaskresult-funcvaluetask-boolean) | Executes the specified asynchronous action, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncttaskresultt-funct-valuetask-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(Result, Func&lt;ValueTask&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncresult-funcvaluetask-boolean) | Executes the specified asynchronous action, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresultt-funct-valuetask-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncvaluetaskresult-funcvaluetask-boolean) | Executes the specified asynchronous action, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctvaluetaskresultt-funct-valuetask-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(ValueTask&lt;Result&gt;, Action, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncvaluetaskresult-action-boolean) | Runs the synchronous action if the result that the `resultTask` returns is successful, and returns the result unchanged. |
| [IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Action&lt;T&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctvaluetaskresultt-actiont-boolean) | Runs the synchronous action on the value if the result that the `resultTask` returns is successful, and returns the result unchanged. |
| [IfSuccessAsync(ValueTask&lt;Result&gt;, Func&lt;Task&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncvaluetaskresult-functask-boolean) | Executes the specified asynchronous action, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctvaluetaskresultt-funct-task-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |

## See Also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
