# MatchTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/MatchTaskAsyncExtensions.cs" target="_blank">MatchTaskAsyncExtensions.cs</a>

Provides extension methods that await a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1) of a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) and fold it into one value: a function for the success and a function for the error, which return the same type.

```csharp
public static class MatchTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → MatchTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [MatchAsync&lt;TOut&gt;(Task&lt;Result&gt;, Func&lt;TOut&gt;, Func&lt;Error, TOut&gt;, Boolean)](MatchTaskAsyncExtensions_MatchAsync.md#matchasynctouttaskresult-functout-funcerror-tout-boolean) | Awaits the `resultTask`, invokes one of the specified synchronous functions depending on whether the [Result](../Result/Result.md) is successful and returns the value that it returns. |
| [MatchAsync&lt;TOut&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;TOut&gt;&gt;, Func&lt;Error, Task&lt;TOut&gt;&gt;, Boolean)](MatchTaskAsyncExtensions_MatchAsync.md#matchasynctouttaskresult-functasktout-funcerror-tasktout-boolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result](../Result/Result.md) is successful, awaits it and returns the value that it returns. |
| [MatchAsync(Task&lt;Result&gt;, Func&lt;Task&gt;, Func&lt;Error, Task&gt;, Boolean)](MatchTaskAsyncExtensions_MatchAsync.md#matchasynctaskresult-functask-funcerror-task-boolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result](../Result/Result.md) is successful, and awaits it. |
| [MatchAsync&lt;T, TOut&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, TOut&gt;, Func&lt;Error, TOut&gt;, Boolean)](MatchTaskAsyncExtensions_MatchAsync.md#matchasynct-touttaskresultt-funct-tout-funcerror-tout-boolean) | Awaits the `resultTask`, invokes one of the specified synchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful and returns the value that it returns. |
| [MatchAsync&lt;T, TOut&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TOut&gt;&gt;, Func&lt;Error, Task&lt;TOut&gt;&gt;, Boolean)](MatchTaskAsyncExtensions_MatchAsync.md#matchasynct-touttaskresultt-funct-tasktout-funcerror-tasktout-boolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful, awaits it and returns the value that it returns. |
| [MatchAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&gt;, Func&lt;Error, Task&gt;, Boolean)](MatchTaskAsyncExtensions_MatchAsync.md#matchasyncttaskresultt-funct-task-funcerror-task-boolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful, and awaits it. |

## See Also
* [SwitchTaskAsyncExtensions](SwitchTaskAsyncExtensions.md)
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
