# Maple.Results.Extensions.ValueTasks Namespace
Provides classes with the [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)-based asynchronous extension methods for the [Result](../../Result/Result.md) and [Result&lt;T&gt;](../../ResultT/ResultT.md) types.

These extension methods are kept in a separate namespace on purpose. Their [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)-based counterparts in the [Maple.Results.Extensions](../namespace.md) namespace declare the same method names, and an `async` lambda passed to them is convertible to both a `Func<Task>` and a `Func<ValueTask>` delegate—so having both sets in scope makes such a call ambiguous (*CS0121*). Import the namespace matching the style you use:

```csharp
using Maple.Results.Extensions;             // Task-based MatchAsync, SwitchAsync, IfSuccessAsync, …
```

```csharp
using Maple.Results.Extensions.ValueTasks;  // ValueTask-based MatchAsync, SwitchAsync, IfSuccessAsync, …
```

If both namespaces have to be imported in one file, disambiguate the call by using the static method syntax—for example `ThenValueTaskAsyncExtensions.ThenAsync(result, …)`—or by passing an explicitly typed delegate.

## Classes
| Name | Description |
| ---- | ----------- |
| [ElseValueTaskAsyncExtensions](ElseValueTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [IOperationResult](../../IOperationResult/IOperationResult.md) interface implementations that replace a failed result. |
| [IfErrorValueTaskAsyncExtensions](IfErrorValueTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [IOperationResult](../../IOperationResult/IOperationResult.md) interface implementations that run a side effect on a failed result and return it unchanged. |
| [IfSuccessValueTaskAsyncExtensions](IfSuccessValueTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [Result](../../Result/Result.md) and [Result&lt;T&gt;](../../ResultT/ResultT.md) records that run a side effect on a successful result and return it unchanged. |
| [LinqValueTaskAsyncExtensions](LinqValueTaskAsyncExtensions.md) | Provides extension methods for the ValueTask&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt; to allow using them with LINQ query syntax. |
| [MatchValueTaskAsyncExtensions](MatchValueTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), that await a value task of a [Result](../../Result/Result.md) or a [Result&lt;T&gt;](../../ResultT/ResultT.md) and fold it into one value. |
| [ResultValueTaskAsyncExtensions](ResultValueTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [Result&lt;T&gt;](../../ResultT/ResultT.md) records. |
| [SwitchValueTaskAsyncExtensions](SwitchValueTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), that await a value task of a [Result](../../Result/Result.md) or a [Result&lt;T&gt;](../../ResultT/ResultT.md) and run one of two actions depending on whether it is successful. |
| [ThenValueTaskAsyncExtensions](ThenValueTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [Result](../../Result/Result.md) and [Result&lt;T&gt;](../../ResultT/ResultT.md) records that transform or chain a successful result. |

## See Also
* [Maple.Results namespace](../../namespace.md)
* [Maple.Results.Extensions namespace](../namespace.md)
