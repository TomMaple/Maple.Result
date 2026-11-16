# LinqExtensions.Select Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Enables the single-clause LINQ query syntax (a `from … select …` query without an intermediate `from`) for [Result&lt;T&gt;](../ResultT/ResultT.md).

## Overloads
| Name | Description |
| ---- | ----------- |
| [Select&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)](#selectt-tnextresultt-funct-tnext) | Projects the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) into a new [Result&lt;TNext&gt;](../ResultT/ResultT.md) by applying the provided selector function; or returns a new [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the same error, otherwise. |
| [Select&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Error&gt;)](#selecttresultt-funct-error) | Applies the selector to the value of a successful result and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [Select&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Result&gt;)](#selecttresultt-funct-result) | Projects a successful result (the `select` clause of a query); a failed result is returned unchanged. |
| [Select&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;)](#selectt-tnextresultt-funct-resulttnext) | Projects a successful result (the `select` clause of a query); a failed result is returned unchanged. |
| [Select&lt;TNext&gt;(Result, Func&lt;Result, TNext&gt;)](#selecttnextresult-funcresult-tnext) | Projects a successful result (the `select` clause of a query); a failed result is returned unchanged. |
| [Select(Result, Func&lt;Result, Error&gt;)](#selectresult-funcresult-error) | Projects a successful result (the `select` clause of a query); a failed result is returned unchanged. |
| [Select(Result, Func&lt;Result, Result&gt;)](#selectresult-funcresult-result) | Projects a successful result (the `select` clause of a query); a failed result is returned unchanged. |
| [Select&lt;TNext&gt;(Result, Func&lt;Result, Result&lt;TNext&gt;&gt;)](#selecttnextresult-funcresult-resulttnext) | Projects a successful result (the `select` clause of a query); a failed result is returned unchanged. |

## Remarks
A non-generic [Result](../Result/Result.md) can be the `from` clause (`from _ in Validate(request) select …`): its range variable is the successful `Result` itself. A selector that returns a `Result` or a `Result<T>` is not nested: the query returns the selected result, as `Then` does (in 2.x it returned a `Result<Result<T>>`); specify the type arguments to keep it nested on purpose.

A selector that returns an [Error](../Error/Error.md) makes the query fail: such a query returns a failed [Result](../Result/Result.md) instead of a **successful** `Result<Error>`.

A selector that returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>` is not supported: the query does not await it, so such a query is a compile error. Await the operation with [ThenAsync](ThenTaskAsyncExtensions_ThenAsync.md) instead, or chain it in a `from` clause whose function returns a `Task<Result<T>>`. To keep the task in the result on purpose, call `Select<T, Task>(…)` with explicit type arguments.


## Select&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs#L52" target="_blank">LinqExtensions.cs</a>

Projects the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) into a new [Result&lt;TNext&gt;](../ResultT/ResultT.md) by applying the provided selector function; or returns a new [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the same error, otherwise.

```csharp
public static Result<TNext> Select<T, TNext>(
    this Result<T> result,
    Func<T, TNext> selector);
```

### Type Parameters
#### `T`
The type of the [Result](../ResultT/ResultT.md)&lt;T&gt; `result` value used to determine whether to execute the passed selector function. If successful, this is also the type of the parameter passed to that function.

#### `TNext`
The output type of the passed selector function and the type of the returned [Result](../ResultT/ResultT.md)&lt;TNext&gt; `result` value.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The instance of [Result](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, TNext&gt;
The function that maps the value of the initial result to the final value.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;TNext&gt;
A successful [Result&lt;TNext&gt;](../ResultT/ResultT.md) containing the value produced by the selector if the `result` is successful; otherwise, a failed [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the original [Error](../Error/Error.md).

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If any of the `result` or `selector` parameters are `null`.

### Remarks
This method is used to enable the single-clause LINQ query syntax (a `from … select …` query without an intermediate `from`) for [Result&lt;T&gt;](../ResultT/ResultT.md) records. It projects a successful value while propagating an error unchanged. To chain multiple operations that each return a [Result&lt;T&gt;](../ResultT/ResultT.md), use [SelectMany](LinqExtensions_SelectMany.md).

### Examples
```csharp
var displayNameResult =
    from user in _userService.GetUser(userId)
    select $"{user.FirstName} {user.LastName}";
```


## Select&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs#L91" target="_blank">LinqExtensions.cs</a>

Applies the provided `selector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns; otherwise, returns a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md).

```csharp
public static Result Select<T>(this Result<T> result, Func<T, Error> selector);
```

### Type Parameters
#### `T`
The type of the value of the initial result, passed to the `selector`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The initial result.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Result](../Result/Result.md)
A failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `selector` if the `result` is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result`, `selector` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `selector` function returns `null`.

### Remarks
This overload makes a query that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [Select&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)](#selectt-tnextresultt-funct-tnext) and return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `Select<T, Error>(…)`.

### Examples
```csharp
var result =
    from user in _userService.GetUser(userId)
    select Error.Conflict(typeUri, $"The user {user.Name} already exists.");
```


## Select&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs#L342" target="_blank">LinqExtensions.cs</a>

Projects a successful result (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static Result Select<T>(this Result<T> result, Func<T, Result> selector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The result to project. Must not be `null`.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The function that receives the value and returns the projection. Must not be `null`.

### Returns
#### [Result](../Result/Result.md)
The result that the selector returns (not nested), or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result` or the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `selector` returns `null`.

### Remarks
A result that the selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result =
    from user in GetUser(id)
    select Audit(user);
```


## Select&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs#L375" target="_blank">LinqExtensions.cs</a>

Projects a successful result (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static Result<TNext> Select<T, TNext>(this Result<T> result, Func<T, Result<TNext>> selector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The result to project. Must not be `null`.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and returns the projection. Must not be `null`.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;TNext&gt;
The result that the selector returns (not nested), or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result` or the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `selector` returns `null`.

### Remarks
A result that the selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result =
    from user in GetUser(id)
    select CreateOrder(user);
```


## Select&lt;TNext&gt;(Result, Func&lt;Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs#L407" target="_blank">LinqExtensions.cs</a>

Projects a successful result (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static Result<TNext> Select<TNext>(this Result result, Func<Result, TNext> selector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../Result/Result.md)
The result to project. Must not be `null`.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), TNext&gt;
The function that receives the successful result and returns the projection. Must not be `null`.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;TNext&gt;
A successful result with the value of the selector, or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result` or the `selector` is `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result =
    from _ in Validate(request)
    select request.Id;
```


## Select(Result, Func&lt;Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs#L436" target="_blank">LinqExtensions.cs</a>

Projects a successful result (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static Result Select(this Result result, Func<Result, Error> selector);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The result to project. Must not be `null`.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the successful result and returns the projection. Must not be `null`.

### Returns
#### [Result](../Result/Result.md)
A failed result with the error of the selector, or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result` or the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `selector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result =
    from _ in Validate(request)
    select Error.Conflict(typeUri, "Not allowed.");
```


## Select(Result, Func&lt;Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs#L474" target="_blank">LinqExtensions.cs</a>

Projects a successful result (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static Result Select(this Result result, Func<Result, Result> selector);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The result to project. Must not be `null`.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful result and returns the projection. Must not be `null`.

### Returns
#### [Result](../Result/Result.md)
The result that the selector returns (not nested), or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result` or the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `selector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result =
    from _ in Validate(request)
    select Audit(request);
```


## Select&lt;TNext&gt;(Result, Func&lt;Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs#L513" target="_blank">LinqExtensions.cs</a>

Projects a successful result (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static Result<TNext> Select<TNext>(this Result result, Func<Result, Result<TNext>> selector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../Result/Result.md)
The result to project. Must not be `null`.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful result and returns the projection. Must not be `null`.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;TNext&gt;
The result that the selector returns (not nested), or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result` or the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `selector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result =
    from _ in Validate(request)
    select CreateOrder(request);
```

## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
* [LinqExtensions.SelectMany](LinqExtensions_SelectMany.md)
* [Write C# Linq Queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries)
