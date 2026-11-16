# ElseValueTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions.ValueTasks](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ElseValueTaskAsyncExtensions.cs" target="_blank">ElseValueTaskAsyncExtensions.cs</a>

Provides asynchronous extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [IOperationResult](../../IOperationResult/IOperationResult.md) interface implementations that replace a failed result.

```csharp
public static class ElseValueTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → ElseValueTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [ElseAsync&lt;TResult&gt;(TResult, Func&lt;Error, ValueTask&lt;TResult&gt;&gt;, Boolean)](ElseValueTaskAsyncExtensions_ElseAsync.md#elseasynctresulttresult-funcerror-valuetasktresult-boolean) | Executes the provided asynchronous function, if the `TResult` instance represents a failed operation and returns its result, as an asynchronous operation. |
| [ElseAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&lt;TResult&gt;&gt;, Boolean)](ElseValueTaskAsyncExtensions_ElseAsync.md#elseasynctresultvaluetasktresult-funcerror-valuetasktresult-boolean) | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |
| [ElseAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, TResult&gt;, Boolean)](ElseValueTaskAsyncExtensions_ElseAsync.md#elseasynctresultvaluetasktresult-funcerror-tresult-boolean) | Runs the synchronous function if the result that the `resultTask` returns is failed, and returns the result that the function returns instead (e.g., a recovered value or another error). |

## See Also
* [IfErrorValueTaskAsyncExtensions](IfErrorValueTaskAsyncExtensions.md)
* [IOperationResult](../../IOperationResult/IOperationResult.md)
