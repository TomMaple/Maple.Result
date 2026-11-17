# Result Record
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L71" target="_blank">Result.cs</a>

Represents the outcome of an operation that can either succeed or fail with an [Error](../Error/Error.md).

```csharp
public sealed record Result : IOperationResult
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → Result

Implements [IOperationResult](../IOperationResult/IOperationResult.md)

## Remarks
> [!NOTE]
> This type is used to represent the result of an operation that does not return a value.
> 
> For operations that return a value, consider using the generic [Result&lt;T&gt;](../ResultT/ResultT.md) record.

If the operation is successful, the [IsSuccess](Result_IsSuccess.md) property is `true`; otherwise, it is `false` (and [IsError](Result_IsError.md) is `true`), and the [Error](Result_Error.md) property will contain details about the failure.

The record is immutable and has no public constructor. Create it with [Result.Success()](Result_Success.md), [Result.FromError(Error)](Result_FromError.md) or the [Implicit(Error to Result)](Result_implicit_Error_to_Result.md) operator.

The default serializers can write a [Result](Result.md) as JSON; reading it back requires a dedicated converter.

`ToString()` writes the [Error](Result_Error.md) of a failed result (e.g., `Result { Error = Error { … } }`) and no members for a successful one (`Result { }`). The debugger shows a failed result in a short form, e.g., `Error: NotFound "Missing"`.

## Examples
```csharp
public Result DeleteUser(int userId)
{
    if (!UserExists(userId))
    {
        return Error.NotFound(
            ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found"),
            "User has not been found.",
            "User with the specified ID does not exist in the system.",
            ErrorUri.Locator("https://api.exampleapp.com/error/e-1234-5678-9012-345678901234/details"),
            "errors-user-notfound",
            ("userId", userId));
    }

    // Proceed with user deletion

    return Result.Success();
}
```

## Properties
| Name | Description |
| ---- | ----------- |
| [Error](Result_Error.md) | Gets the error details if the result represents a failure. |
| [IsError](Result_IsError.md) | Gets a value indicating whether the result represents a failed operation. |
| [IsSuccess](Result_IsSuccess.md) | Gets a value indicating whether the result represents a successful operation. |

## Methods
| Name | Description |
| ---- | ----------- |
| [FromError(Error)](Result_FromError.md) | Creates a new failure [Result](Result.md) from the specified [Error](../Error/Error.md). |
| [FromValue&lt;T&gt;(T)](Result_FromValue.md) | Creates a new successful [Result&lt;T&gt;](../ResultT/ResultT.md) containing the specified value. |
| [Success()](Result_Success.md) | Creates a new instance of successful [Result](Result.md). |
| [Try](Result_Try.md) | Runs code that can throw and turns the exception into a failed result, with the error that a mapper function creates. |
| [TryAsync](Result_TryAsync.md) | Runs asynchronous code that can throw and turns the exception into a failed result, with the error that a mapper function creates. |
| [TryGetError(Error)](Result_TryGetError.md) | Gets the error of a failed result. |

## Operators
| Name | Description |
| ---- | ----------- |
| [Implicit(Error to Result)](Result_implicit_Error_to_Result.md) | Defines an implicit conversion of a given [Error](../Error/Error.md) to a failure [Result](Result.md). |

## Extension Methods
| Name | Description |
| ---- | ----------- |
| [IfError&lt;TResult&gt;(TResult, Action&lt;Error&gt;)](../Extensions/IfErrorExtensions_IfError.md#iferrortresulttresult-actionerror) | Executes the provided action if the [Result](../Result/Result.md) instance represents a failed operation. |
| [Else&lt;TResult&gt;(TResult, Func&lt;Error, TResult&gt;)](../Extensions/ElseExtensions_Else.md#elsetresulttresult-funcerror-tresult) | Executes the provided function if the [Result](../Result/Result.md) instance represents a failed operation and returns its result. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;, Boolean)](../Extensions/IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttresult-funcerror-task-boolean)  | Executes the provided asynchronous action, if the [Result](../Result/Result.md) instance represents a failed operation, as an asynchronous operation. |
| [ElseAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](../Extensions/ElseAsyncExtensions_ElseAsync.md#elseasynctresulttresult-funcerror-tasktresult-boolean) | Executes the provided asynchronous function, if the [Result](../Result/Result.md) instance represents a failed operation and returns its result, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttasktresult-funcerror-task-boolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed [Result](../Result/Result.md) instance, and returns its result as an asynchronous operation. |
| [ElseAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](../Extensions/ElseAsyncExtensions_ElseAsync.md#elseasynctresulttasktresult-funcerror-tasktresult-boolean) | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed [Result](../Result/Result.md) instance, and returns its result as an asynchronous operation. |
| [IfSuccess(Result, Action)](../Extensions/IfSuccessExtensions_IfSuccess.md#ifsuccessresult-action) | Executes the specified action, if the [Result](../Result/Result.md) represents a successful operation. |
| [Then(Result, Func&lt;Result&gt;)](../Extensions/ThenExtensions_Then.md#thenresult-funcresult) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [Then&lt;T&gt;(Result, Func&lt;T&gt;)](../Extensions/ThenExtensions_Then.md#thentresult-funct) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [Then&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;)](../Extensions/ThenExtensions_Then.md#thentresult-funcresultt) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccessAsync(Result, Func&lt;Task&gt;, Boolean)](../Extensions/IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncresult-functask-boolean) | Executes the specified asynchronous action, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Boolean)](../Extensions/ThenAsyncExtensions_ThenAsync.md#thenasyncresult-functaskresult-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Boolean)](../Extensions/ThenAsyncExtensions_ThenAsync.md#thenasynctresult-functaskt-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](../Extensions/ThenAsyncExtensions_ThenAsync.md#thenasynctresult-functaskresultt-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(Task&lt;Result&gt;, Func&lt;Task&gt;, Boolean)](../Extensions/IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctaskresult-functask-boolean) | Executes the specified asynchronous action, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync(Task&lt;Result&gt;, Func&lt;Task&lt;Result&gt;&gt;, Boolean)](../Extensions/ThenAsyncExtensions_ThenAsync.md#thenasynctaskresult-functaskresult-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;T&gt;&gt;, Boolean)](../Extensions/ThenAsyncExtensions_ThenAsync.md#thenasyncttaskresult-functaskt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](../Extensions/ThenAsyncExtensions_ThenAsync.md#thenasyncttaskresult-functaskresultt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [Match&lt;TOut&gt;(Result, Func&lt;TOut&gt;, Func&lt;Error, TOut&gt;)](../Extensions/MatchExtensions_Match.md#matchtoutresult-functout-funcerror-tout) | Invokes one of the specified functions depending on whether the [Result](../Result/Result.md) is successful and returns the value that it returns. |
| [MatchAsync&lt;TOut&gt;(Task&lt;Result&gt;, Func&lt;TOut&gt;, Func&lt;Error, TOut&gt;, Boolean)](../Extensions/MatchAsyncExtensions_MatchAsync.md#matchasynctouttaskresult-functout-funcerror-tout-boolean) | Awaits the `resultTask`, invokes one of the specified synchronous functions depending on whether the [Result](../Result/Result.md) is successful and returns the value that it returns. |
| [MatchAsync&lt;TOut&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;TOut&gt;&gt;, Func&lt;Error, Task&lt;TOut&gt;&gt;, Boolean)](../Extensions/MatchAsyncExtensions_MatchAsync.md#matchasynctouttaskresult-functasktout-funcerror-tasktout-boolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result](../Result/Result.md) is successful, awaits it and returns the value that it returns. |
| [MatchAsync(Task&lt;Result&gt;, Func&lt;Task&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/MatchAsyncExtensions_MatchAsync.md#matchasynctaskresult-functask-funcerror-task-boolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result](../Result/Result.md) is successful, and awaits it. |
| [Switch(Result, Action, Action&lt;Error&gt;)](../Extensions/SwitchExtensions_Switch.md#switchresult-action-actionerror) | Invokes one of the specified actions depending on whether the [Result](../Result/Result.md) is successful. |
| [SwitchAsync(Task&lt;Result&gt;, Action, Action&lt;Error&gt;, Boolean)](../Extensions/SwitchAsyncExtensions_SwitchAsync.md#switchasynctaskresult-action-actionerror-boolean) | Awaits the `resultTask` and invokes one of the specified actions depending on whether the [Result](../Result/Result.md) is successful. |

## See Also
* [IOperationResult](../IOperationResult/IOperationResult.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
