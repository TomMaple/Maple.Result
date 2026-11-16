# ThenTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs" target="_blank">ThenTaskAsyncExtensions.cs</a>

Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task), for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records that transform or chain a successful result.

```csharp
public static class ThenTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → ThenTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [ThenAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Boolean)](ThenTaskAsyncExtensions_ThenAsync.md#thenasyncresult-functaskresult-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Boolean)](ThenTaskAsyncExtensions_ThenAsync.md#thenasynctresult-functaskt-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](ThenTaskAsyncExtensions_ThenAsync.md#thenasynctresult-functaskresultt-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Boolean)](ThenTaskAsyncExtensions_ThenAsync.md#thenasynctresultt-funct-taskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Boolean)](ThenTaskAsyncExtensions_ThenAsync.md#thenasynct-tnextresultt-funct-tasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](ThenTaskAsyncExtensions_ThenAsync.md#thenasynct-tnextresultt-funct-taskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync(Task&lt;Result&gt;, Func&lt;Task&lt;Result&gt;&gt;, Boolean)](ThenTaskAsyncExtensions_ThenAsync.md#thenasynctaskresult-functaskresult-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;T&gt;&gt;, Boolean)](ThenTaskAsyncExtensions_ThenAsync.md#thenasyncttaskresult-functaskt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](ThenTaskAsyncExtensions_ThenAsync.md#thenasyncttaskresult-functaskresultt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Boolean)](ThenTaskAsyncExtensions_ThenAsync.md#thenasyncttaskresultt-funct-taskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Boolean)](ThenTaskAsyncExtensions_ThenAsync.md#thenasynct-tnexttaskresultt-funct-tasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](ThenTaskAsyncExtensions_ThenAsync.md#thenasynct-tnexttaskresultt-funct-taskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |

## See Also
* [IfSuccessTaskAsyncExtensions](IfSuccessTaskAsyncExtensions.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
