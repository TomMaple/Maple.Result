# IOperationResult Interface
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Result.cs" target="_blank">Result.cs</a>

Represents the outcome of an operation that can either succeed or fail with an [Error](../Error/Error.md).

```csharp
public interface IOperationResult
```

Derived [Maple.Results.Result](../Result/Result.md), [Maple.Results.Result&lt;T&gt;](../ResultT/ResultT.md)

## Remarks
Use the interface for code that handles both kinds of result: as the type of a parameter or a variable, as a type argument (e.g., `List<IOperationResult>`) or as a generic constraint (`where TResult : IOperationResult`).

> [!IMPORTANT]
> The interface is closed: [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) are its only implementations, so a failed result always has an [Error](IOperationResult_Error.md). A type outside the library that implements the interface does not compile (CS0535), and a mocking library cannot create a mock of it. In a test, create a result instead, e.g., with `Result.Success()`, `Result.FromError(error)` or `Result.FromValue(value)`.

## Properties
| Name | Description |
| ---- | ----------- |
| [Error](IOperationResult_Error.md) | Gets the error details if the result represents a failure. |
| [IsError](IOperationResult_IsError.md) | Gets a value indicating whether the result represents a failed operation. |
| [IsSuccess](IOperationResult_IsSuccess.md) | Gets a value indicating whether the result represents a successful operation. |

## Methods
| Name | Description |
| ---- | ----------- |
| [TryGetError(Error)](IOperationResult_TryGetError.md) | Gets the error of a failed result. |

## Extension Methods
| Name | Description |
| ---- | ----------- |
| [IfError&lt;TResult&gt;(TResult, Action&lt;Error&gt;)](../Extensions/IfErrorExtensions_IfError.md#iferrortresulttresult-actionerror) | Executes the specified action, if the [IOperationResult](IOperationResult.md) represents a failure. |
| [Else&lt;TResult&gt;(TResult, Func&lt;Error, TResult&gt;)](../Extensions/ElseExtensions_Else.md#elsetresulttresult-funcerror-tresult) | Executes the specified function, if the [IOperationResult](IOperationResult.md) represents a failure. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)](../Extensions/IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttresult-funcerror-task-nullableboolean) | Executes the specified asynchronous function, if the [IOperationResult](IOperationResult.md) represents a failure, as an asynchronous operation. |
| [ElseAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&lt;TResult&gt;&gt;, Nullable&lt;Boolean&gt;)](../Extensions/ElseAsyncExtensions_ElseAsync.md#elseasynctresulttresult-funcerror-tasktresult-nullableboolean) | Executes the specified asynchronous function, if the [IOperationResult](IOperationResult.md) represents a failure, as an asynchronous operation. |

## See also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
