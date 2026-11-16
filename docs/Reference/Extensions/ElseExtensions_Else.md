# ElseExtensions.Else Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a function if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), and returns the result that the function returns instead: e.g., a recovered value or another error.

## Overloads
| Name | Description |
| ---- | ----------- |
| [Else&lt;TResult&gt;(TResult, Func&lt;Error, TResult&gt;)](#elsetresulttresult-funcerror-tresult) | Executes the provided function if the `TResult` instance represents a failed operation and returns its result. |

## Remarks
A successful result is returned unchanged, and the function is not called. An action (a function that returns nothing) is a compile error (CS0619) that names [IfError](IfErrorExtensions_IfError.md), which runs a side effect and returns the result unchanged.

A function that returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>` is not supported: `Else` does not await it, so such a call is a compile error (CS0619). Use [ElseAsync](ElseTaskAsyncExtensions_ElseAsync.md) instead.


## Else&lt;TResult&gt;(TResult, Func&lt;Error, TResult&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ElseExtensions.cs#L50" target="_blank">ElseExtensions.cs</a>

Executes the provided function only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)).

```csharp
public static TResult Else<TResult>(this TResult result, Func<Error, TResult> ifErrorFunction)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `result` that implements the [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), TResult&gt;

The function to be executed only if the current instance of `TResult` represents a failed operation. The [Error](../Error/Error.md) instance contained in the `TResult` will be passed as a parameter to this function.

### Returns
#### TResult
The original `TResult` instance passed as a `result` parameter.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifErrorFunction` function returns `null`.

### Examples
```csharp
var userResult = _userService.GetUser(userId)
    .Else(error =>
    {
        _logger.LogWarning("Falling back to the cache: {ErrorDetail}", error.Detail);
        return _userCache.GetUser(userId);
    });
```

## See Also
* [IfError](IfErrorExtensions_IfError.md)
* [IOperationResult](../IOperationResult/IOperationResult.md)
