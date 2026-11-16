# ThenValueTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions.ValueTasks](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Provides asynchronous extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [Result](../../Result/Result.md) and [Result&lt;T&gt;](../../ResultT/ResultT.md) records that transform or chain a successful result.

```csharp
public static class ThenValueTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → ThenValueTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [ThenAsync(Result, Func&lt;ValueTask&lt;Result&gt;&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasyncresult-funcvaluetaskresult-boolean) | Executes the specified asynchronous function, if the [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;T&gt;&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynctresult-funcvaluetaskt-boolean) | Executes the specified asynchronous function, if the [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynctresult-funcvaluetaskresultt-boolean) | Executes the specified asynchronous function, if the [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynctresultt-funct-valuetaskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynct-tnextresultt-funct-valuetasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynct-tnextresultt-funct-valuetaskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&gt;&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasyncvaluetaskresult-funcvaluetaskresult-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;T&gt;&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynctvaluetaskresult-funcvaluetaskt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynctvaluetaskresult-funcvaluetaskresultt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynctvaluetaskresultt-funct-valuetaskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynct-tnextvaluetaskresultt-funct-valuetasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynct-tnextvaluetaskresultt-funct-valuetaskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;T&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynctvaluetaskresult-funct-boolean) | Runs the synchronous function if the result that the `resultTask` returns is successful, and returns a result with its value. |
| [ThenAsync(ValueTask&lt;Result&gt;, Func&lt;Result&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasyncvaluetaskresult-funcresult-boolean) | Runs the synchronous function if the result that the `resultTask` returns is successful, and returns the result that it returns. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;Result&lt;T&gt;&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynctvaluetaskresult-funcresultt-boolean) | Runs the synchronous function if the result that the `resultTask` returns is successful, and returns the result that it returns. |
| [ThenAsync(ValueTask&lt;Result&gt;, Func&lt;Error&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasyncvaluetaskresult-funcerror-boolean) | Runs the synchronous function if the result that the `resultTask` returns is successful, and returns a failed result with the error that it returns. |
| [ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynct-tnextvaluetaskresultt-funct-tnext-boolean) | Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns a result with the value that it returns. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynctvaluetaskresultt-funct-result-boolean) | Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns the result that it returns. |
| [ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynct-tnextvaluetaskresultt-funct-resulttnext-boolean) | Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns the result that it returns. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Error&gt;, Boolean)](ThenValueTaskAsyncExtensions_ThenAsync.md#thenasynctvaluetaskresultt-funct-error-boolean) | Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns a failed result with the error that it returns. |

## See Also
* [IfSuccessValueTaskAsyncExtensions](IfSuccessValueTaskAsyncExtensions.md)
* [Result](../../Result/Result.md)
* [Result&lt;T&gt;](../../ResultT/ResultT.md)
