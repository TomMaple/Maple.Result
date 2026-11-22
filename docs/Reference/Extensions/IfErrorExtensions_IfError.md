# IfErrorExtensions.IfError Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a side effect if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), and returns the result unchanged.

## Overloads
| Name | Description |
| ---- | ----------- |
| [IfError&lt;TResult&gt;(TResult, Action&lt;Error&gt;)](#iferrortresulttresult-actionerror) | Executes the provided action if the `TResult` instance represents a failed operation. |

## Remarks
To replace a failed result (recover with a value, or return another error), use [Else](ElseExtensions_Else.md). `IfError` runs a function that returns a value as a side effect and ignores the value, so a failure is not turned into a success (e.g., `e => titles.Add(e.Title)`). The analyzer reports the functions whose value is ignored by mistake:

* a function that returns a `Result`, a `Result<T>` or an `Error` is an error ([MR0004](../../Analyzers/MR0004.md));
* a function that returns a value of the type of the result’s value, in a call whose result is used, is a warning ([MR0005](../../Analyzers/MR0005.md)): in version 2 such a call replaced the failed result. For a side effect that happens to return that type (e.g., `e => Interlocked.Increment(ref failures)` on a `Result<int>`), use a statement lambda.

An expression that cannot be a statement (e.g., `e => 0`) does not compile (CS0201).

`IfError` does not await a task. An error action that returns a task, or an `async` lambda, compiles as an action, and its task is discarded: the analyzer reports it as an error ([MR0002](../../Analyzers/MR0002.md)). Use [IfErrorAsync](IfErrorAsyncExtensions_IfErrorAsync.md) instead. A lambda that only throws is not a task, and compiles (e.g., `IfError(e => throw new OrderException(e.Title))`).


## IfError&lt;TResult&gt;(TResult, Action&lt;Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Extensions/IfErrorExtensions.cs#L45" target="_blank">IfErrorExtensions.cs</a>

Executes the provided action only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)).

```csharp
public static TResult IfError<TResult>(this TResult result, Action<Error> ifErrorAction)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `result` that implements the [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;

The action to be executed only if the current instance of `TResult` represents a failed operation. The [Error](../Error/Error.md) instance contained in the `TResult` will be passed as a parameter to this action.

### Returns
#### TResult
The original `TResult` instance passed as a `result` parameter.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifErrorAction` parameter is `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

createUserResult.IfError(error =>
{
    _logger.LogError("Failed to create user: {ErrorDetail}", error.Detail);
    Metrics.IncrementCounter("user_creation_failures");
});
```

## See Also
* [Else](ElseExtensions_Else.md)
* [IOperationResult](../IOperationResult/IOperationResult.md)
