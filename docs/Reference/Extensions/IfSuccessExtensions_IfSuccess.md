# IfSuccessExtensions.IfSuccess Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a side effect if the instance of [Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), and returns the result unchanged.

## Overloads
| Name | Description |
| ---- | ----------- |
| [IfSuccess(Result, Action)](#ifsuccessresult-action) | Executes the specified action, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccess&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;)](#ifsuccesstresultt-actiont) | Executes the specified action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |

## Remarks
To transform or chain the result with a function that returns a value, a `Result`, a `Result<T>` or an `Error`, use [Then](ThenExtensions_Then.md). Such a function passed to `IfSuccess` is a compile error (CS0619) that names `Then`, so that its value is not discarded by mistake. For a side effect whose expression happens to return a value (e.g., `set.Add(x)`), use a statement lambda: `x => { set.Add(x); }`.

A function that returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>` is not supported: `IfSuccess` does not await it, so such a call is a compile error (CS0619). Use [IfSuccessAsync](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md) instead, or discard the task explicitly in a statement lambda (`x => { _ = SaveAsync(x); }`).


## IfSuccess(Result, Action)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs#L50" target="_blank">IfSuccessExtensions.cs</a>

Executes the provided action, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result IfSuccess(this Result result, Action ifSuccessAction);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessAction` [Action](https://learn.microsoft.com/dotnet/api/system.action)
The action to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation.

### Returns
#### [Result](../Result/Result.md)
The original `result` passed as a parameter. Can be used for method chaining.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessAction` parameter is `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

createUserResult.IfSuccess(() =>
{
    _auditService.LogUserAdded(userData);
});
```


## IfSuccess&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs#L83" target="_blank">IfSuccessExtensions.cs</a>

Executes the provided action, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result<T> IfSuccess<T>(this Result<T> result, Action<T> ifSuccessAction);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;T&gt;
The action to be executed only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;T&gt;
The original `result` passed as a parameter. Can be used for method chaining.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessAction` parameter is `null`.

### Examples
```csharp
var userResult = _userService.CreateUser(userData);

userResult.IfSuccess(user =>
{
    _auditService.LogUserAdded(user);
});
```

## See Also
* [Then](ThenExtensions_Then.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
