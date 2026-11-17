# SwitchTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/SwitchTaskAsyncExtensions.cs" target="_blank">SwitchTaskAsyncExtensions.cs</a>

Provides extension methods that await a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1) of a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) and run one of two actions depending on whether the result is successful.

```csharp
public static class SwitchTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → SwitchTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [SwitchAsync(Task&lt;Result&gt;, Action, Action&lt;Error&gt;, Boolean)](SwitchTaskAsyncExtensions_SwitchAsync.md#switchasynctaskresult-action-actionerror-boolean) | Awaits the `resultTask` and invokes one of the specified actions depending on whether the [Result](../Result/Result.md) is successful. |
| [SwitchAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Action&lt;T&gt;, Action&lt;Error&gt;, Boolean)](SwitchTaskAsyncExtensions_SwitchAsync.md#switchasyncttaskresultt-actiont-actionerror-boolean) | Awaits the `resultTask` and invokes one of the specified actions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful. |

## See Also
* [MatchTaskAsyncExtensions](MatchTaskAsyncExtensions.md)
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
