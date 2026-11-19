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
To transform or chain the result with a function that returns a value, a `Result`, a `Result<T>` or an `Error`, use [Then](ThenExtensions_Then.md). `IfSuccess` runs a function that returns a value as a side effect and ignores the value (e.g., `x => set.Add(x)` or `x => builder.Append(x)`). The analyzer reports the functions whose value is ignored by mistake:

* a function that returns a `Result`, a `Result<T>` or an `Error`, whose failure would be lost, is an error ([MR0004](../../Analyzers/MR0004.md));
* a function that returns a value of the type of the result’s value, in a call whose result is used, is a warning ([MR0005](../../Analyzers/MR0005.md)): in version 2 such a call transformed the result. For a side effect that happens to return that type (e.g., `x => count++` on a `Result<int>`), use a statement lambda: `x => { count++; }`.

An expression that cannot be a statement (e.g., `x => x + 1`) does not compile (CS0201).

`IfSuccess` does not await a task. A function that returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>`, or an `async` lambda, compiles as an action, and its task is discarded: the analyzer reports it as an error ([MR0002](../../Analyzers/MR0002.md)). Use [IfSuccessAsync](IfSuccessAsyncExtensions_IfSuccessAsync.md) instead, or discard the task explicitly in a statement lambda (`x => { _ = SaveAsync(x); }`). A lambda that only throws is not a task, and compiles (e.g., `IfSuccess(x => throw new InvalidOperationException())`).


## IfSuccess(Result, Action)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs#L46" target="_blank">IfSuccessExtensions.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs#L79" target="_blank">IfSuccessExtensions.cs</a>

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
