# LinqAsyncExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs" target="_blank">LinqAsyncExtensions.Task.cs</a>, <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Provides extension methods for the [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; and the [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; to allow use it with asynchronous methods using LINQ query syntax.

```csharp
public static class LinqAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → LinqAsyncExtensions

## Remarks
The query syntax cannot pass a `continueOnCapturedContext` argument, so these methods use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) to decide where the selectors run: on a thread-pool thread (`false`, the default) or on the captured context, e.g., the UI thread (`true`).

## Methods
| Name | Description |
| ---- | ----------- |
| [Select(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;)](LinqAsyncExtensions_Select.md) | Projects the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited result into a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; by applying the provided selector function; or returns a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; with the same error, otherwise. |
| [Select(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;)](LinqAsyncExtensions_Select.md) | Projects the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited result into a new [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; by applying the provided selector function; or returns a new [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; with the same error, otherwise. |
| [SelectMany(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](LinqAsyncExtensions_SelectMany.md) | Returns a new instance of [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; instance by applying the provided selector functions if the current [Result&lt;T&gt;](../ResultT/ResultT.md) if it is successful; or a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; instance with the same error, otherwise. |
| [SelectMany(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](LinqAsyncExtensions_SelectMany.md) | Returns a new instance of [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; instance by applying the provided selector functions if the current [Result&lt;T&gt;](../ResultT/ResultT.md) if it is successful; or a new [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; instance with the same error, otherwise. |

## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
* [Write C# Linq Queries](https://learn.microsoft.com/dotnet/csharp/linq/get-started/write-linq-queries)
