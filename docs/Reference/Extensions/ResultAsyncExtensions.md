# ResultAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ResultAsyncExtensions.Task.cs" target="_blank">ResultAsyncExtensions.Task.cs</a>, <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ResultAsyncExtensions.ValueTask.cs" target="_blank">ResultAsyncExtensions.ValueTask.cs</a>

Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task) or [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [Result&lt;T&gt;](../ResultT/ResultT.md) records.

```csharp
public static partial class ResultAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → ResultAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [ToResultAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Boolean)](ResultAsyncExtensions_ToResultAsync.md#toresultasyncttaskresultt-boolean) | Asynchronously converts the outcome of the asynchronous operation represented by the [Result&lt;T&gt;](../ResultT/ResultT.md) to the non-generic [Result](../Result/Result.md). |
| [ToResultAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Boolean)](ResultAsyncExtensions_ToResultAsync.md#toresultasynctvaluetaskresultt-boolean) | Asynchronously converts the outcome of the asynchronous operation represented by the [Result&lt;T&gt;](../ResultT/ResultT.md) to the non-generic [Result](../Result/Result.md). |

## See Also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
