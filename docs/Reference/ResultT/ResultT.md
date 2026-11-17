# Result&lt;T&gt; Record
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L223" target="_blank">Result.cs</a>

Represents the outcome of an operation that can either succeed or fail with an [Error](../Error/Error.md).

```csharp
public sealed record Result<T> : IOperationResult
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → Result&lt;T&gt;

Implements [IOperationResult](../IOperationResult/IOperationResult.md)

## Remarks
> [!NOTE]
> This type is used to represent the result of an operation that can return a value.
> 
> For operations that does not return a value, consider using [Result](../Result/Result.md) record.

If the operation is successful, the [IsSuccess](ResultT_IsSuccess.md) property is `true` and the [Value](ResultT_Value.md) property will hold the returned value. If the operation has failed, the [IsSuccess](ResultT_IsSuccess.md) property is `false` (and [IsError](ResultT_IsError.md) is `true`), and the [Error](ResultT_Error.md) property will contain details about the failure.

> [!CAUTION]
> Check [IsSuccess](ResultT_IsSuccess.md) or [IsError](ResultT_IsError.md) before reading the [Value](ResultT_Value.md) property: on a failed result it throws an `InvalidOperationException`. Use [TryGetValue](ResultT_TryGetValue.md) to read the value without an exception.

The record is immutable and has no public constructor. Create it with [Result&lt;T&gt;.FromValue(T)](ResultT_FromValue.md), [Result&lt;T&gt;.FromError(Error)](ResultT_FromError.md), or the [Implicit(Error to Result&lt;T&gt;)](ResultT_implicit_Error_to_ResultT.md) or [Implicit(T to Result&lt;T&gt;)](ResultT_implicit_T_to_ResultT.md) operator. A result is successful exactly when it has no [Error](ResultT_Error.md); a successful result of a nullable `T` can carry a `null` value.

## Examples
```csharp
public Result<User> GetUser(int userId)
{
    try
    {
        var user = await _userRepository.GetById(userId);
        return user is null
            ? Result<User>.FromError(
                Error.NotFound(
                    ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found"),
                    "User has not been found.",
                    "User with the specified ID does not exist in the system.",
                    ErrorUri.Locator("https://api.exampleapp.com/error/e-1234-5678-9012-345678901234/details"),
                    "errors-user-notfound",
                    ("userId", userId)))
            : Result<User>.FromValue(user);
    }
    catch (Exception ex)
    {
        // Log exception details here
        // Map the exception to a proper error

        return Result<User>.FromError(error);
    }
```

```csharp
public Result<User> GetUser(int userId)
{
    try
    {
        var user = await _userRepository.GetById(userId);
        if (user is null)
        {
            return Error.NotFound(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found"),
                "User has not been found.",
                "User with the specified ID does not exist in the system.",
                ErrorUri.Locator("https://api.exampleapp.com/error/e-1234-5678-9012-345678901234/details"),
                "errors-user-notfound",
                ("userId", userId));
        }

        return user;
    }
    catch (Exception ex)
    {
        // Log exception details here
        // Map the exception to a proper error

        return Result<User>.FromError(error);
    }
```

## Serialization
The default serializers ([System.Text.Json](https://learn.microsoft.com/dotnet/api/system.text.json.jsonserializer) and [Newtonsoft.Json](https://www.newtonsoft.com/json)) read every public property. They can write a **successful** [Result&lt;T&gt;](ResultT.md) (e.g., `{"Value":128,"Error":null,"IsSuccess":true,"IsError":false}`), but writing a **failed** one throws, because its [Value](ResultT_Value.md) throws: *System.Text.Json* throws the `InvalidOperationException`, and *Newtonsoft.Json* wraps it in a `JsonSerializationException`. Use a dedicated converter, or map the result to your own DTO.

Reading a [Result&lt;T&gt;](ResultT.md) back requires a dedicated converter, because the type has no public constructor or setters. Without one, deserialization fails.

`ToString()` writes only the [Value](ResultT_Value.md) of a successful result (e.g., `Result { Value = 42 }`) or the [Error](ResultT_Error.md) of a failed one (`Result { Error = Error { … } }`); it never throws. The debugger shows a failed result in a short form, e.g., `Error: NotFound "Missing"`.

## Properties
| Name | Description |
| ---- | ----------- |
| [Error](ResultT_Error.md) | Gets the error details if the result represents a failure. |
| [IsError](ResultT_IsError.md) | Gets a value indicating whether the result represents a failed operation. |
| [IsSuccess](ResultT_IsSuccess.md) | Gets a value indicating whether the result represents a successful operation. |
| [Value](ResultT_Value.md) | Gets the value of a successful operation; throws for a failed one. |

## Methods
| Name | Description |
| ---- | ----------- |
| [FromError(Error)](ResultT_FromError.md) | Creates a new failure [Result&lt;T&gt;](ResultT.md) from the specified [Error](../Error/Error.md). |
| [FromValue(T)](ResultT_FromValue.md) | Creates a new successful [Result&lt;T&gt;](ResultT.md) containing the specified value. |
| [TryGetError(Error)](ResultT_TryGetError.md) | Gets the error of a failed result. |
| [TryGetValue(T)](ResultT_TryGetValue.md#trygetvaluet) | Gets the value of a successful result. |
| [TryGetValue(T, Error)](ResultT_TryGetValue.md#trygetvaluet-error) | Gets the value of a successful result, or the error of a failed one. |

## Operators
| Name | Description |
| ---- | ----------- |
| [Implicit(Error to Result&lt;T&gt;)](ResultT_implicit_Error_to_ResultT.md) | Defines an implicit conversion of a given [Error](../Error/Error.md) to a failure [Result&lt;T&gt;](ResultT.md). |
| [Implicit(T to Result&lt;T&gt;)](ResultT_implicit_T_to_ResultT.md) | Defines an implicit conversion of a given value of type `T` to a successful [Result&lt;T&gt;](ResultT.md). |

## Extension Methods
| Name | Description |
| ---- | ----------- |
| [IfError&lt;TResult&gt;(TResult, Action&lt;Error&gt;)](../Extensions/IfErrorExtensions_IfError.md#iferrortresulttresult-actionerror) | Executes the provided action if the [Result&lt;T&gt;](../ResultT/ResultT.md) instance represents a failed operation. |
| [Else&lt;TResult&gt;(TResult, Func&lt;Error, TResult&gt;)](../Extensions/ElseExtensions_Else.md#elsetresulttresult-funcerror-tresult) | Executes the provided function if the [Result&lt;T&gt;](../ResultT/ResultT.md) instance represents a failed operation and returns its result. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;, Boolean)](../Extensions/IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttresult-funcerror-task-boolean)  | Executes the provided asynchronous action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) instance represents a failed operation, as an asynchronous operation. |
| [ElseAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](../Extensions/ElseAsyncExtensions_ElseAsync.md#elseasynctresulttresult-funcerror-tasktresult-boolean) | Executes the provided asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) instance represents a failed operation and returns its result, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttasktresult-funcerror-task-boolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed [Result&lt;T&gt;](../ResultT/ResultT.md) instance, and returns its result as an asynchronous operation. |
| [ElseAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](../Extensions/ElseAsyncExtensions_ElseAsync.md#elseasynctresulttasktresult-funcerror-tasktresult-boolean) | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed [Result&lt;T&gt;](../ResultT/ResultT.md) instance, and returns its result as an asynchronous operation. |
| [IfSuccess&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;)](../Extensions/IfSuccessExtensions_IfSuccess.md#ifsuccesstresultt-actiont) | Executes the specified action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [Then&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Result&gt;)](../Extensions/ThenExtensions_Then.md#thentresultt-funct-result) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [Then&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)](../Extensions/ThenExtensions_Then.md#thent-tnextresultt-funct-tnext) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [Then&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;)](../Extensions/ThenExtensions_Then.md#thent-tnextresultt-funct-resulttnext) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&gt;, Boolean)](../Extensions/IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresultt-funct-task-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Boolean)](../Extensions/ThenAsyncExtensions_ThenAsync.md#thenasynctresultt-funct-taskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Boolean)](../Extensions/ThenAsyncExtensions_ThenAsync.md#thenasynct-tnextresultt-funct-tasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](../Extensions/ThenAsyncExtensions_ThenAsync.md#thenasynct-tnextresultt-funct-taskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&gt;, Boolean)](../Extensions/IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncttaskresultt-funct-task-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Boolean)](../Extensions/ThenAsyncExtensions_ThenAsync.md#thenasyncttaskresultt-funct-taskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Boolean)](../Extensions/ThenAsyncExtensions_ThenAsync.md#thenasynct-tnexttaskresultt-funct-tasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](../Extensions/ThenAsyncExtensions_ThenAsync.md#thenasynct-tnexttaskresultt-funct-taskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [Match&lt;T, TOut&gt;(Result&lt;T&gt;, Func&lt;T, TOut&gt;, Func&lt;Error, TOut&gt;)](../Extensions/MatchExtensions_Match.md#matcht-toutresultt-funct-tout-funcerror-tout) | Invokes one of the specified functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful and returns the value that it returns. |
| [MatchAsync&lt;T, TOut&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, TOut&gt;, Func&lt;Error, TOut&gt;, Boolean)](../Extensions/MatchAsyncExtensions_MatchAsync.md#matchasynct-touttaskresultt-funct-tout-funcerror-tout-boolean) | Awaits the `resultTask`, invokes one of the specified synchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful and returns the value that it returns. |
| [MatchAsync&lt;T, TOut&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TOut&gt;&gt;, Func&lt;Error, Task&lt;TOut&gt;&gt;, Boolean)](../Extensions/MatchAsyncExtensions_MatchAsync.md#matchasynct-touttaskresultt-funct-tasktout-funcerror-tasktout-boolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful, awaits it and returns the value that it returns. |
| [MatchAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/MatchAsyncExtensions_MatchAsync.md#matchasyncttaskresultt-funct-task-funcerror-task-boolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful, and awaits it. |
| [Switch&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;, Action&lt;Error&gt;)](../Extensions/SwitchExtensions_Switch.md#switchtresultt-actiont-actionerror) | Invokes one of the specified actions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful. |
| [SwitchAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Action&lt;T&gt;, Action&lt;Error&gt;, Boolean)](../Extensions/SwitchAsyncExtensions_SwitchAsync.md#switchasyncttaskresultt-actiont-actionerror-boolean) | Awaits the `resultTask` and invokes one of the specified actions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful. |
| [SelectMany(Result&lt;T&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](../Extensions/LinqExtensions_SelectMany.md) | Returns a new instance of [Result&lt;TNext&gt;](../ResultT/ResultT.md) instance by applying the provided selector functions if the current [Result&lt;T&gt;](../ResultT/ResultT.md) if it is successful, or a new [Result&lt;TNext&gt;](../ResultT/ResultT.md) instance with the same error otherwise. |
| [SelectMany(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](../Extensions/LinqAsyncExtensions_SelectMany.md) | Returns a new instance of [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; instance by applying the provided selector functions if the current [Result&lt;T&gt;](../ResultT/ResultT.md) if it is successful; or a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; instance with the same error, otherwise. |
| [ToResult(Result&lt;T&gt;)](../Extensions/ResultExtensions_ToResult.md) | Converts the generic [Result&lt;T&gt;](../ResultT/ResultT.md)  to the non-generic [Result](../Result/Result.md). |
| [ToResultAsync(Task&lt;Result&lt;T&gt;&gt;)](../Extensions/ResultAsyncExtensions_ToResultAsync.md) | Asynchronously converts the outcome of the asynchronous operation represented by the [Result&lt;T&gt;](../ResultT/ResultT.md) to the non-generic [Result](../Result/Result.md). |

## See Also
* [IOperationResult](../IOperationResult/IOperationResult.md)
* [Result](../Result/Result.md)
