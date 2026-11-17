# ThenExtensions.Then Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a function if the instance of [Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), and returns the next result: the value, the [Result](../Result/Result.md) or the [Result&lt;T&gt;](../ResultT/ResultT.md) that the function returns, or a failed result for an [Error](../Error/Error.md) it returns.

## Overloads
| Name | Description |
| ---- | ----------- |
| [Then(Result, Func&lt;Result&gt;)](#thenresult-funcresult) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [Then(Result, Func&lt;Error&gt;)](#thenresult-funcerror) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [Then&lt;T&gt;(Result, Func&lt;T&gt;)](#thentresult-funct) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [Then&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;)](#thentresult-funcresultt) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [Then&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Result&gt;)](#thentresultt-funct-result) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [Then&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Error&gt;)](#thentresultt-funct-error) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [Then&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)](#thent-tnextresultt-funct-tnext) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [Then&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;)](#thent-tnextresultt-funct-resulttnext) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |

## Remarks
A returned `Result` or `Result<T>` is flattened, and a returned [Error](../Error/Error.md) makes the result fail. Explicit type arguments keep a nested result on purpose. An action (a function that returns nothing) does not compile: the compiler reports that it returns no value (e.g., CS0029). Use [IfSuccess](IfSuccessExtensions_IfSuccess.md) for a side effect; it returns the result unchanged.

A function that returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>` is not supported: `Then` does not await it, so such a call is a compile error (CS0619). Use [ThenAsync](ThenAsyncExtensions_ThenAsync.md) instead, or return the task in a result to keep it on purpose (e.g., `x => Result.FromValue(SaveAsync(x))`); explicit type arguments (e.g., `Then<T, Task>(…)`) are a compile error too. A lambda that only throws is not a task, and compiles (e.g., `Then(x => throw new InvalidOperationException())`).


## Then(Result, Func&lt;Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenExtensions.cs#L50" target="_blank">ThenExtensions.cs</a>

Executes the provided function only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result Then(this Result result, Func<Result> ifSuccessFunction);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../Result/Result.md)&gt;
The function to be executed only if the current instance of [Result](../Result/Result.md) represents a successful operation.

### Returns
#### [Result](../Result/Result.md)
The original `result` passed as a parameter. Can be used for method chaining.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var userAddedResult = createUserResult.Then(() =>
{
    return _companyService.AddUser(userData);
});
```


## Then(Result, Func&lt;Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenExtensions.cs#L91" target="_blank">ThenExtensions.cs</a>

Executes the provided function only if the instance of [Result](../Result/Result.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
public static Result Then(this Result result, Func<Error> ifSuccessFunction);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Error](../Error/Error.md)&gt;
The function to be executed only if the `result` represents a successful operation. It returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Result](../Result/Result.md)
A failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the `result` is successful; otherwise, the original `result`. The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns `null`.

### Remarks
Without this overload, a function returning an [Error](../Error/Error.md) would bind to [Then&lt;T&gt;(Result, Func&lt;T&gt;)](#thentresult-funct) and return a **successful** `Result<Error>`. To get that on purpose, specify the type argument: `Then<Error>(…)`.

### Examples
```csharp
var result = _userService.DeleteUser(userId)
    .Then(() => Error.Conflict(typeUri, "The user cannot be deleted."));
```


## Then&lt;T&gt;(Result, Func&lt;T&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenExtensions.cs#L128" target="_blank">ThenExtensions.cs</a>

Executes the provided function only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result<T> Then<T>(this Result result, Func<T> ifSuccessFunction);
```

### Type Parameters
#### `T`
The type of the value returned by the `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;T&gt;
The function to be executed only if the current instance of [Result](../Result/Result.md) represents a successful operation.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;T&gt;
If the `result` is successful, the [Result](../ResultT/ResultT.md)&lt;T&gt; record with the value returned by the `ifSuccessFunction`; otherwise, the failed [Result](../ResultT/ResultT.md)&lt;T&gt; record with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var companyResult = createUserResult.Then(() =>
{
    return _companyService.GetCompanyById(userData.CompanyId);
});
```


## Then&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenExtensions.cs#L161" target="_blank">ThenExtensions.cs</a>

Executes the provided function only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result<T> Then<T>(this Result result, Func<Result<T>> ifSuccessFunction);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The function to be executed only if the current instance of [Result](../Result/Result.md) represents a successful operation.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;T&gt;
If the `result` is successful, the [Result](../ResultT/ResultT.md)&lt;T&gt; record returned by the `ifSuccessFunction`; otherwise, the failed [Result](../ResultT/ResultT.md)&lt;T&gt; record with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var companyResult = createUserResult.Then(() =>
{
    return _companyService.GetCompanyById(userData.CompanyId);
});
```


## Then&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenExtensions.cs#L200" target="_blank">ThenExtensions.cs</a>

Executes the provided function only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result Then<T>(this Result<T> result, Func<T, Result> ifSuccessFunction);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The function to be executed only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

### Returns
#### [Result](../Result/Result.md)
If the `result` is successful, the [Result](../Result/Result.md) returned by the `ifSuccessFunction`; otherwise, the failed [Result](../Result/Result.md) with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var userAddedResult = createUserResult.Then(user =>
{
    return _companyService.AddUser(user);
});
```


## Then&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenExtensions.cs#L242" target="_blank">ThenExtensions.cs</a>

Executes the provided function only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
public static Result Then<T>(this Result<T> result, Func<T, Error> ifSuccessFunction);
```

### Type Parameters
#### `T`
The type of the value of the `result`, passed to the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Error](../Error/Error.md)&gt;
The function to be executed only if the `result` represents a successful operation. The value of the `result` is passed as a parameter. It returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Result](../Result/Result.md)
A failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the `result` is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns `null`.

### Remarks
Without this overload, a function returning an [Error](../Error/Error.md) would bind to [Then&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)](#thent-tnextresultt-funct-tnext) and return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `Then<T, Error>(…)`.

### Examples
```csharp
var result = _userService.GetUser(userId)
    .Then(user => Error.Conflict(typeUri, $"The user ‘{user.Name}’ already exists."));
```


## Then&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenExtensions.cs#L285" target="_blank">ThenExtensions.cs</a>

Executes the provided function only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result<TNext> Then<T, TNext>(this Result<T> result, Func<T, TNext> ifSuccessFunction);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, TNext&gt;
The function to be executed only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;TNext&gt;
If the `result` is successful, the [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the value returned by the `ifSuccessFunction`; otherwise, the failed [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var companyResult = createUserResult.Then(user =>
{
    return _companyService.GetCompanyById(user.CompanyId);
});
```


## Then&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenExtensions.cs#L324" target="_blank">ThenExtensions.cs</a>

Executes the provided function only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result<TNext> Then<T, TNext>(this Result<T> result, Func<T, Result<TNext>> ifSuccessFunction);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The function to be executed only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;TNext&gt;
If the `result` is successful, the [Result&lt;TNext&gt;](../ResultT/ResultT.md) returned by the `ifSuccessFunction`; otherwise, the failed [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var companyResult = createUserResult.Then(user =>
{
    return _companyService.GetCompanyById(user.CompanyId);
});
```

## See Also
* [IfSuccess](IfSuccessExtensions_IfSuccess.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
