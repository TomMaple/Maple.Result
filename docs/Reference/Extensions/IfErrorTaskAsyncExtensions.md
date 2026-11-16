# IfErrorTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfErrorTaskAsyncExtensions.cs" target="_blank">IfErrorTaskAsyncExtensions.cs</a>

Provides extension methods for the [IOperationResult](../IOperationResult/IOperationResult.md) interface implementations that run a side effect on a failed result and return it unchanged.

```csharp
public static class IfErrorTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → IfErrorTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;, Boolean)](IfErrorTaskAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttresult-funcerror-task-boolean)  | Executes the provided asynchronous action, if the `TResult` instance represents a failed operation, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&gt;, Boolean)](IfErrorTaskAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttasktresult-funcerror-task-boolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Action&lt;Error&gt;, Boolean)](IfErrorTaskAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttasktresult-actionerror-boolean) | Runs the synchronous action if the result that the `resultTask` returns is failed, and returns the result unchanged. |

## See Also
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
