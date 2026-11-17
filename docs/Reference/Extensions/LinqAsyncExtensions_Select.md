# LinqAsyncExtensions.Select Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Enables the single-clause LINQ query syntax (a `from … select …` query without an intermediate `from`) for [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; and [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;.

## Overloads
| Name | Description |
| ---- | ----------- |
| [Select&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;)](#selectt-tnexttaskresultt-funct-tnext) | Projects the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` into a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; by applying the provided selector function; or returns a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; with the same error, otherwise. |
| [Select&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Error&gt;)](#selectttaskresultt-funct-error) | Applies the selector to the value of a successful result and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [Select&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;)](#selectttaskresultt-funct-result) | Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged. |
| [Select&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;)](#selectt-tnexttaskresultt-funct-resulttnext) | Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged. |
| [Select&lt;TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, TNext&gt;)](#selecttnexttaskresult-funcresult-tnext) | Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged. |
| [Select(Task&lt;Result&gt;, Func&lt;Result, Error&gt;)](#selecttaskresult-funcresult-error) | Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged. |
| [Select(Task&lt;Result&gt;, Func&lt;Result, Result&gt;)](#selecttaskresult-funcresult-result) | Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged. |
| [Select&lt;TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Result&lt;TNext&gt;&gt;)](#selecttnexttaskresult-funcresult-resulttnext) | Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged. |
| [Select&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;)](#selectt-tnextvaluetaskresultt-funct-tnext) | Projects the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` into a new [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; by applying the provided selector function; or returns a new [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; with the same error, otherwise. |
| [Select&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Error&gt;)](#selecttvaluetaskresultt-funct-error) | Applies the selector to the value of a successful result and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [Select&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;)](#selecttvaluetaskresultt-funct-result) | Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged. |
| [Select&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;)](#selectt-tnextvaluetaskresultt-funct-resulttnext) | Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged. |
| [Select&lt;TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, TNext&gt;)](#selecttnextvaluetaskresult-funcresult-tnext) | Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged. |
| [Select(ValueTask&lt;Result&gt;, Func&lt;Result, Error&gt;)](#selectvaluetaskresult-funcresult-error) | Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged. |
| [Select(ValueTask&lt;Result&gt;, Func&lt;Result, Result&gt;)](#selectvaluetaskresult-funcresult-result) | Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged. |
| [Select&lt;TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, Result&lt;TNext&gt;&gt;)](#selecttnextvaluetaskresult-funcresult-resulttnext) | Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged. |

## Remarks
On a `Result` or a `Result<T>`, the function decides the kind of task: a function that returns a `ValueTask` gives a `ValueTask`, and an `async` lambda gives a `Task` (with an explicit return type, e.g., `async ValueTask<int> (int x) => …`, it gives a `ValueTask`). On a `Task` or a `ValueTask` of a result, the kind of the receiver is kept.

A non-generic [Result](../Result/Result.md) can be the `from` clause (`from _ in Validate(request) select …`): its range variable is the successful `Result` itself. A selector that returns a `Result` or a `Result<T>` is not nested: the query returns the selected result, as `Then` does (in 2.x it returned a `Result<Result<T>>`); specify the type arguments to keep it nested on purpose.

A selector that returns an [Error](../Error/Error.md) makes the query fail: such a query returns a failed [Result](../Result/Result.md) instead of a **successful** `Result<Error>`.

A selector that returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>` is not supported: the query does not await it, so such a query is a compile error. Await the operation with [ThenAsync](ThenAsyncExtensions_ThenAsync.md) instead, or chain it in a `from` clause whose function returns a `Task<Result<T>>`. To keep the task in the result on purpose, call `Select<T, Task>(…)` with explicit type arguments.


## Select&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L67" target="_blank">LinqAsyncExtensions.Task.cs</a>

Projects the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` into a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; by applying the provided selector function; or returns a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; with the same error, otherwise.

```csharp
public static Task<Result<TNext>> Select<T, TNext>(
    this Task<Result<T>> result,
    Func<T, TNext> selector);
```

### Type Parameters
#### `T`
The type of the [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; `result` value used to determine whether to execute the passed selector function. If successful, this is also the type of the parameter passed to that function.

#### `TNext`
The output type of the passed selector function and the type of the returned [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt; `result` value.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The task object representing the asynchronous operation that returns an instance of a [Result](../ResultT/ResultT.md) record to be evaluated.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, TNext&gt;
The function that maps the value of the initial result to the final value.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A task object representing the asynchronous operation. Its result is a successful [Result&lt;TNext&gt;](../ResultT/ResultT.md) containing the value produced by the selector if the `result` is successful; otherwise, a failed [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the original [Error](../Error/Error.md).

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If any of the `result` or `selector` parameters are `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`.

### Remarks
This method is used to enable the single-clause LINQ query syntax (a `from … select …` query without an intermediate `from`) for [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;. It projects a successful value while propagating an error unchanged. To chain multiple asynchronous operations that each return a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;, use [SelectMany](LinqAsyncExtensions_SelectMany.md).

### Examples
```csharp
var displayNameResult =
    await from user in _userService.GetUserAsync(userId)
    select $"{user.FirstName} {user.LastName}";
```


## Select&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L115" target="_blank">LinqAsyncExtensions.Task.cs</a>

Applies the provided `selector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns; otherwise, returns a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md).

```csharp
public static Task<Result> Select<T>(this Task<Result<T>> result, Func<T, Error> selector);
```

### Type Parameters
#### `T`
The type of the value of the initial result, passed to the `selector`.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The task object representing the asynchronous operation that returns the initial result.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `selector` if the `result` is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result`, `selector` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`, or the `selector` function returns `null`.

### Remarks
This overload makes a query that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [Select&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;)](#selectt-tnexttaskresultt-funct-tnext) and return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `Select<T, Error>(…)`.

### Examples
```csharp
var result =
    await from user in _userService.GetUserAsync(userId)
    select Error.Conflict(typeUri, $"The user {user.Name} already exists.");
```


## Select&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L759" target="_blank">LinqAsyncExtensions.Task.cs</a>

Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static Task<Result> Select<T>(this Task<Result<T>> result, Func<T, Result> selector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the result. Must not be `null`.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The function that receives the value and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result that the selector returns (not nested), or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result` or the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `selector` returns `null`.

### Remarks
A result that the selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    select Audit(user));
```


## Select&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L795" target="_blank">LinqAsyncExtensions.Task.cs</a>

Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static Task<Result<TNext>> Select<T, TNext>(this Task<Result<T>> result, Func<T, Result<TNext>> selector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the result. Must not be `null`.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the selector returns (not nested), or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result` or the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `selector` returns `null`.

### Remarks
A result that the selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    select CreateOrder(user));
```


## Select&lt;TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L832" target="_blank">LinqAsyncExtensions.Task.cs</a>

Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static Task<Result<TNext>> Select<TNext>(this Task<Result> result, Func<Result, TNext> selector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result. Must not be `null`.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), TNext&gt;
The function that receives the successful result and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the selector, or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result` or the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    select request.Id);
```


## Select(Task&lt;Result&gt;, Func&lt;Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L869" target="_blank">LinqAsyncExtensions.Task.cs</a>

Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static Task<Result> Select(this Task<Result> result, Func<Result, Error> selector);
```

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result. Must not be `null`.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the successful result and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the selector, or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result` or the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `selector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    select Error.Conflict(typeUri, "Not allowed."));
```


## Select(Task&lt;Result&gt;, Func&lt;Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L910" target="_blank">LinqAsyncExtensions.Task.cs</a>

Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static Task<Result> Select(this Task<Result> result, Func<Result, Result> selector);
```

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result. Must not be `null`.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful result and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result that the selector returns (not nested), or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result` or the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `selector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    select Audit(request));
```


## Select&lt;TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L952" target="_blank">LinqAsyncExtensions.Task.cs</a>

Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static Task<Result<TNext>> Select<TNext>(this Task<Result> result, Func<Result, Result<TNext>> selector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result. Must not be `null`.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful result and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the selector returns (not nested), or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result` or the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `selector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    select CreateOrder(request));
```

## Select&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L53" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Projects the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` into a new [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; by applying the provided selector function; or returns a new [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; with the same error, otherwise.

```csharp
public static ValueTask<Result<TNext>> Select<T, TNext>(
    this ValueTask<Result<T>> result,
    Func<T, TNext> selector);
```

### Type Parameters
#### `T`
The type of the [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; `result` value used to determine whether to execute the passed selector function. If successful, this is also the type of the parameter passed to that function.

#### `TNext`
The output type of the passed selector function and the type of the returned [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt; `result` value.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The value task object representing the asynchronous operation that returns an instance of a [Result](../ResultT/ResultT.md) record to be evaluated.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, TNext&gt;
The function that maps the value of the initial result to the final value.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A value task object representing the asynchronous operation. Its result is a successful [Result&lt;TNext&gt;](../ResultT/ResultT.md) containing the value produced by the selector if the `result` is successful; otherwise, a failed [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the original [Error](../Error/Error.md).

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `selector` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`.

### Remarks
This method is used to enable the single-clause LINQ query syntax (a `from … select …` query without an intermediate `from`) for [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;. It projects a successful value while propagating an error unchanged. To chain multiple asynchronous operations that each return a [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;, use [SelectMany](LinqAsyncExtensions_SelectMany.md).

### Examples
```csharp
var displayNameResult =
    await from user in _userService.GetUserAsync(userId)
    select $"{user.FirstName} {user.LastName}";
```


## Select&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L98" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Applies the provided `selector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns; otherwise, returns a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md).

```csharp
public static ValueTask<Result> Select<T>(this ValueTask<Result<T>> result, Func<T, Error> selector);
```

### Type Parameters
#### `T`
The type of the value of the initial result, passed to the `selector`.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The value task object representing the asynchronous operation that returns the initial result.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The value task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `selector` if the `result` is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `selector` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`, or the `selector` function returns `null`.

### Remarks
This overload makes a query that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [Select&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;)](#selectt-tnextvaluetaskresultt-funct-tnext) and return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `Select<T, Error>(…)`.

### Examples
```csharp
var result =
    await from user in _userService.GetUserAsync(userId)
    select Error.Conflict(typeUri, $"The user {user.Name} already exists.");
```


## Select&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L736" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static ValueTask<Result> Select<T>(this ValueTask<Result<T>> result, Func<T, Result> selector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the result.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The function that receives the value and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The result that the selector returns (not nested), or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `selector` returns `null`.

### Remarks
A result that the selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    select Audit(user));
```


## Select&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L771" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static ValueTask<Result<TNext>> Select<T, TNext>(this ValueTask<Result<T>> result, Func<T, Result<TNext>> selector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the result.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the selector returns (not nested), or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `selector` returns `null`.

### Remarks
A result that the selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    select CreateOrder(user));
```


## Select&lt;TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L807" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static ValueTask<Result<TNext>> Select<TNext>(this ValueTask<Result> result, Func<Result, TNext> selector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), TNext&gt;
The function that receives the successful result and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the selector, or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    select request.Id);
```


## Select(ValueTask&lt;Result&gt;, Func&lt;Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L843" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static ValueTask<Result> Select(this ValueTask<Result> result, Func<Result, Error> selector);
```

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the successful result and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the selector, or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `selector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    select Error.Conflict(typeUri, "Not allowed."));
```


## Select(ValueTask&lt;Result&gt;, Func&lt;Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L883" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static ValueTask<Result> Select(this ValueTask<Result> result, Func<Result, Result> selector);
```

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful result and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The result that the selector returns (not nested), or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `selector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    select Audit(request));
```


## Select&lt;TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L924" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Awaits the result and projects it if it is successful (the `select` clause of a query); a failed result is returned unchanged.

```csharp
public static ValueTask<Result<TNext>> Select<TNext>(this ValueTask<Result> result, Func<Result, Result<TNext>> selector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful result and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the selector returns (not nested), or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `selector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `selector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    select CreateOrder(request));
```

## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
* [LinqAsyncExtensions.SelectMany](LinqAsyncExtensions_SelectMany.md)
* [Write C# Linq Queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries)
