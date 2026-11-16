# Maple.Results.Extensions Namespace
Provides classes with extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) types.

## Classes
| Name | Description |
| ---- | ----------- |
| [ElseExtensions](ElseExtensions.md) | Provides extension methods for the [IOperationResult](../IOperationResult/IOperationResult.md) interface implementations that replace a failed result. |
| [ElseTaskAsyncExtensions](ElseTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task), for the [IOperationResult](../IOperationResult/IOperationResult.md) interface implementations that replace a failed result. |
| [IfErrorExtensions](IfErrorExtensions.md) | Provides extension methods for the [IOperationResult](../IOperationResult/IOperationResult.md) interface implementations that run a side effect on a failed result and return it unchanged. |
| [IfErrorTaskAsyncExtensions](IfErrorTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task), for the [IOperationResult](../IOperationResult/IOperationResult.md) interface implementations that run a side effect on a failed result and return it unchanged. |
| [IfSuccessExtensions](IfSuccessExtensions.md) | Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records that run a side effect on a successful result and return it unchanged. |
| [IfSuccessTaskAsyncExtensions](IfSuccessTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task), for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records that run a side effect on a successful result and return it unchanged. |
| [LinqExtensions](LinqExtensions.md) | Provides extension methods for the [Result&lt;T&gt;](../ResultT/ResultT.md) records to allow using them with LINQ query syntax. |
| [LinqTaskAsyncExtensions](LinqTaskAsyncExtensions.md) | Provides extension methods for the Task&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; to allow using them with LINQ query syntax. |
| [MatchExtensions](MatchExtensions.md) | Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records that fold a result into one value: a function for the success and a function for the error. |
| [MatchTaskAsyncExtensions](MatchTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task), that await a task of a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) and fold it into one value. |
| [ResultExtensions](ResultExtensions.md) | Provides extension methods for the [Result&lt;T&gt;](../ResultT/ResultT.md) records. |
| [ResultTaskAsyncExtensions](ResultTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task), for the [Result&lt;T&gt;](../ResultT/ResultT.md) records. |
| [SwitchExtensions](SwitchExtensions.md) | Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records that run one of two actions depending on whether a result is successful, and return nothing. |
| [SwitchTaskAsyncExtensions](SwitchTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task), that await a task of a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) and run one of two actions depending on whether it is successful. |
| [ThenExtensions](ThenExtensions.md) | Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records that transform or chain a successful result. |
| [ThenTaskAsyncExtensions](ThenTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task), for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records that transform or chain a successful result. |

## See Also
* [Maple.Results namespace](../namespace.md)
* [Maple.Results.Extensions.ValueTasks namespace](ValueTasks/namespace.md)
