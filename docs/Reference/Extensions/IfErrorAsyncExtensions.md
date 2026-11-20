# IfErrorAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/IfErrorAsyncExtensions.Task.cs" target="_blank">IfErrorAsyncExtensions.Task.cs</a>, <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/IfErrorAsyncExtensions.ValueTask.cs" target="_blank">IfErrorAsyncExtensions.ValueTask.cs</a>

Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task) or [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [IOperationResult](../IOperationResult/IOperationResult.md) interface implementations that run a side effect on a failed result and return it unchanged.

```csharp
public static class IfErrorAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → IfErrorAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [IfErrorAsync&lt;TResult, TValue&gt;(Task&lt;TResult&gt;, Func&lt;Error, ValueTask&lt;TValue&gt;&gt;, Nullable&lt;Boolean&gt;)](IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresult-tvaluetasktresult-funcerror-valuetasktvalue-nullableboolean) | Executes the provided asynchronous function as a side effect, if the provided asynchronous operation returns a failed `TResult` instance (i.e., contains an [Error](../Error/Error.md)); the value of the task that it returns is ignored. |
| [IfErrorAsync&lt;TResult, TValue&gt;(TResult, Func&lt;Error, ValueTask&lt;TValue&gt;&gt;, Nullable&lt;Boolean&gt;)](IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresult-tvaluetresult-funcerror-valuetasktvalue-nullableboolean) | Executes the provided asynchronous function as a side effect, only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)); the value of the task that it returns is ignored. |
| [IfErrorAsync&lt;TResult, TValue&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&lt;TValue&gt;&gt;, Nullable&lt;Boolean&gt;)](IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresult-tvaluevaluetasktresult-funcerror-valuetasktvalue-nullableboolean) | Executes the provided asynchronous function as a side effect, if the provided asynchronous operation returns a failed `TResult` instance (i.e., contains an [Error](../Error/Error.md)); the value of the task that it returns is ignored. |
| [IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Action&lt;Error&gt;, Nullable&lt;Boolean&gt;)](IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttasktresult-actionerror-nullableboolean) | Runs the synchronous action if the result that the `resultTask` returns is failed, and returns the result unchanged. |
| [IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)](IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttasktresult-funcerror-task-nullableboolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)](IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttasktresult-funcerror-valuetask-nullableboolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)](IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttresult-funcerror-task-nullableboolean)  | Executes the provided asynchronous action, if the `TResult` instance represents a failed operation, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)](IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttresult-funcerror-valuetask-nullableboolean)  | Executes the provided asynchronous action, if the `TResult` instance represents a failed operation, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Action&lt;Error&gt;, Nullable&lt;Boolean&gt;)](IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresultvaluetasktresult-actionerror-nullableboolean) | Runs the synchronous action if the result that the `resultTask` returns is failed, and returns the result unchanged. |
| [IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)](IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresultvaluetasktresult-funcerror-task-nullableboolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)](IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresultvaluetasktresult-funcerror-valuetask-nullableboolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |

## See Also
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
