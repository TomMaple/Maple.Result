# ElseTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ElseTaskAsyncExtensions.cs" target="_blank">IfErrorExtensions.cs</a>

Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task), for the [IOperationResult](../IOperationResult/IOperationResult.md) interface implementations that replace a failed result.

```csharp
public static class ElseTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → ElseTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [ElseAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](ElseTaskAsyncExtensions_ElseAsync.md#elseasynctresulttresult-funcerror-tasktresult-boolean) | Executes the provided asynchronous function, if the `TResult` instance represents a failed operation and returns its result, as an asynchronous operation. |
| [ElseAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](ElseTaskAsyncExtensions_ElseAsync.md#elseasynctresulttasktresult-funcerror-tasktresult-boolean) | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |

## See Also
* [IfErrorTaskAsyncExtensions](IfErrorTaskAsyncExtensions.md)
* [IOperationResult](../IOperationResult/IOperationResult.md)
