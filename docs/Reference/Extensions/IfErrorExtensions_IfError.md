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
To replace a failed result (recover with a value, or return another error), use [Else](ElseExtensions_Else.md). A function that returns a value passed to `IfError` is a compile error (CS0619) that names `Else`, so that a failure is not turned into a success by mistake (e.g., `e => Interlocked.Increment(ref failures)`). For a side effect, use a statement lambda.


## IfError&lt;TResult&gt;(TResult, Action&lt;Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfErrorExtensions.cs#L49" target="_blank">IfErrorExtensions.cs</a>

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
