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
| [MatchExtensions](MatchExtensions.md) | Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records to handle both success and error scenarios. |
| [MatchTaskAsyncExtensions](MatchTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task), for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records to handle both success and error scenarios. |
| [ResultExtensions](ResultExtensions.md) | Provides extension methods for the [Result&lt;T&gt;](../ResultT/ResultT.md) records. |
| [ResultTaskAsyncExtensions](ResultTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task), for the [Result&lt;T&gt;](../ResultT/ResultT.md) records. |
| [ThenExtensions](ThenExtensions.md) | Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records that transform or chain a successful result. |
| [ThenTaskAsyncExtensions](ThenTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task), for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records that transform or chain a successful result. |

## See Also
* [Maple.Results namespace](../namespace.md)
* [Maple.Results.Extensions.ValueTasks namespace](ValueTasks/namespace.md)
