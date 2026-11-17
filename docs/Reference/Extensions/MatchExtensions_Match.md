# MatchExtensions.Match Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Folds a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) into one value: runs the function for the success or the function for the error, and returns what it returns.

## Overloads
| Name | Description |
| ---- | ----------- |
| [Match&lt;TOut&gt;(Result, Func&lt;TOut&gt;, Func&lt;Error, TOut&gt;)](#matchtoutresult-functout-funcerror-tout) | Invokes one of the specified functions depending on whether the [Result](../Result/Result.md) is successful and returns the value that it returns. |
| [Match&lt;T, TOut&gt;(Result&lt;T&gt;, Func&lt;T, TOut&gt;, Func&lt;Error, TOut&gt;)](#matcht-toutresultt-funct-tout-funcerror-tout) | Invokes one of the specified functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful and returns the value that it returns. |

## Remarks
`Match` is a fold: two functions that return one type, `TOut`, which the call returns. It returns what the function that ran returns and does not look at it:

* a value: `result.Match(user => user.Name, error => "unknown")` is a `string`;
* a result: a function that returns a `Result<T>` and an error function that returns the [Error](../Error/Error.md) give a `Result<T>`, because an `Error` converts to it (`result.Match(x => Validate(x), e => e)`);
* a task: `await result.Match(x => CountAsync(x), e => CountAsync(0))` awaits the task of the function that ran. There is no `MatchAsync` on a result; `MatchAsync` takes a task of a result ([MatchTaskAsyncExtensions](MatchTaskAsyncExtensions_MatchAsync.md), [MatchValueTaskAsyncExtensions](ValueTasks/MatchValueTaskAsyncExtensions_MatchAsync.md)).

A function that returns `null` gives `null`, also when `TOut` is a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md): the fold does not check the value.

Two actions (e.g., `Match(x => Log(x), e => Log(e))`) are a compile error (CS0619) that names [Switch](SwitchExtensions_Switch.md), which runs one of two actions and returns nothing. To run a side effect and keep the result, use [IfSuccess](IfSuccessExtensions_IfSuccess.md)`(…).`[IfError](IfErrorExtensions_IfError.md)`(…)`; to transform the value and handle the error, [Then](ThenExtensions_Then.md)`(…).IfError(…)`. Two lambdas that only throw are a compile error too: they have no type to infer.

A call whose functions have no common return type is rejected by the compiler (e.g., a value and an `Error`, or one synchronous and one asynchronous function).

With explicit type arguments, `TOut` is the whole return type: `Match<int, Result<int>>(x => x + 1, e => e)`.

## Match&lt;TOut&gt;(Result, Func&lt;TOut&gt;, Func&lt;Error, TOut&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs#L51" target="_blank">MatchExtensions.cs</a>

Invokes the provided `ifSuccessFunction` if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorFunction` otherwise, and returns the value that the invoked function returns.

```csharp
public static TOut Match<TOut>(this Result result, Func<TOut> ifSuccessFunction, Func<Error, TOut> ifErrorFunction);
```

### Type Parameters
#### `TOut`
The type that both functions return.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record whose state selects the function.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;TOut&gt;
The function to be invoked, only if the `result` represents a successful operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), TOut&gt;
The function to be invoked, only if the `result` represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` is passed as a parameter to this function.

### Returns
#### TOut
The value that `ifSuccessFunction` or `ifErrorFunction` returns, `null` included.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

### Examples
```csharp
var message = _userService.DeleteUser(id).Match(
    () => "The user was deleted.",
    error => $"The user was not deleted: {error.Title}");

// A Result: the error function returns the Error.
Result auditResult = _userService.DeleteUser(id).Match(
    () => _auditService.LogUserDeleted(id),
    error => error);

// A task: awaited by the caller.
await _userService.DeleteUser(id).Match(
    () => _auditService.LogUserDeletedAsync(id),
    error => _auditService.LogErrorAsync(error));
```


## Match&lt;T, TOut&gt;(Result&lt;T&gt;, Func&lt;T, TOut&gt;, Func&lt;Error, TOut&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs#L89" target="_blank">MatchExtensions.cs</a>

Invokes the provided `ifSuccessFunction` with the value if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorFunction` otherwise, and returns the value that the invoked function returns.

```csharp
public static TOut Match<T, TOut>(this Result<T> result, Func<T, TOut> ifSuccessFunction, Func<Error, TOut> ifErrorFunction);
```

### Type Parameters
#### `T`
The type of the value of the `result`.

#### `TOut`
The type that both functions return.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record whose state selects the function.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, TOut&gt;
The function to be invoked, only if the `result` represents a successful operation. The value of the `result` is passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), TOut&gt;
The function to be invoked, only if the `result` represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` is passed as a parameter to this function.

### Returns
#### TOut
The value that `ifSuccessFunction` or `ifErrorFunction` returns, `null` included.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

### Examples
```csharp
var greeting = _userService.GetUser(id).Match(
    user => $"Hello, {user.Name}!",
    error => "Hello, stranger!");

// A Result<Address>: the error function returns the Error.
var addressResult = _userService.GetUser(id).Match(
    user => _addressBook.Find(user.Id),
    error => error);

// A task: awaited by the caller.
var orderCount = await _userService.GetUser(id).Match(
    user => _orderService.CountOrdersAsync(user.Id),
    error => Task.FromResult(0));
```


## See Also
* [Switch](SwitchExtensions_Switch.md)
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
