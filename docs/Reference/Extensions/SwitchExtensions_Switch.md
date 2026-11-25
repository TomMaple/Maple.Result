# SwitchExtensions.Switch Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs one of two actions depending on whether the instance of [Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful or a failed operation, and returns nothing.

## Overloads
| Name | Description |
| ---- | ----------- |
| [Switch(Result, Action, Action&lt;Error&gt;)](#switchresult-action-actionerror) | Invokes one of the specified actions depending on whether the [Result](../Result/Result.md) is successful. |
| [Switch&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;, Action&lt;Error&gt;)](#switchtresultt-actiont-actionerror) | Invokes one of the specified actions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful. |

## Remarks
`Switch` ends a chain for side effects, as [Match](MatchExtensions_Match.md) ends it for a value: it returns nothing. To run a side effect and keep chaining, use [IfSuccess](IfSuccessExtensions_IfSuccess.md)`(…).`[IfError](IfErrorExtensions_IfError.md)`(…)`, which return the result.

An action that returns a value is a side effect, and the value is ignored (e.g., `Switch(x => set.Add(x), e => log.Append(e.Title))`). A lambda that only throws is fine in either position, and in both.

An action that returns a `Task`, a `Task<T>`, a `ValueTask` or a `ValueTask<T>` (e.g., `x => SaveAsync(x)` or an `async` lambda) compiles, in either position and in both, but `Switch` does not await the task, and an `async` lambda runs as `async void`: the analyzer reports each such action as an error ([MR0002](../../Analyzers/MR0002.md)). Use [Match](MatchExtensions_Match.md) with two functions that return a task, which the caller awaits. To start a task without awaiting it on purpose, discard it in a statement lambda (`x => { _ = SaveAsync(x); }`).

## Switch(Result, Action, Action&lt;Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/SwitchExtensions.cs#L42" target="_blank">SwitchExtensions.cs</a>

Invokes the provided `ifSuccessAction` if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorAction` otherwise.

```csharp
public static void Switch(this Result result, Action ifSuccessAction, Action<Error> ifErrorAction);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record whose state selects the action.

#### `ifSuccessAction` [Action](https://learn.microsoft.com/dotnet/api/system.action)
The action to be invoked, only if the `result` represents a successful operation.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The action to be invoked, only if the `result` represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` is passed as a parameter to this action.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessAction` or `ifErrorAction` parameter is `null`.

### Examples
```csharp
_userService.CreateUser(userData).Switch(
    () => _auditService.LogUserAdded(userData),
    error => _auditService.LogError(error));
```


## Switch&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;, Action&lt;Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/SwitchExtensions.cs#L73" target="_blank">SwitchExtensions.cs</a>

Invokes the provided `ifSuccessAction` with the value if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorAction` otherwise.

```csharp
public static void Switch<T>(this Result<T> result, Action<T> ifSuccessAction, Action<Error> ifErrorAction);
```

### Type Parameters
#### `T`
The type of the value of the `result`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record whose state selects the action.

#### `ifSuccessAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;T&gt;
The action to be invoked, only if the `result` represents a successful operation. The value of the `result` is passed as a parameter to this action.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The action to be invoked, only if the `result` represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` is passed as a parameter to this action.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessAction` or `ifErrorAction` parameter is `null`.

### Examples
```csharp
_userService.GetUser(id).Switch(
    user => Console.WriteLine($"Hello, {user.Name}!"),
    error => Console.WriteLine($"No user: {error.Title}"));
```


## See Also
* [Match](MatchExtensions_Match.md)
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
