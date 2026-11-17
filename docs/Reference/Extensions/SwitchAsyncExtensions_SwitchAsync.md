# SwitchAsyncExtensions.SwitchAsync Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Awaits a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1) or a [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1) of a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) and runs one of two actions depending on whether the result is successful, as an asynchronous operation.

## Overloads
| Name | Description |
| ---- | ----------- |
| [SwitchAsync(Task&lt;Result&gt;, Action, Action&lt;Error&gt;, Boolean)](#switchasynctaskresult-action-actionerror-boolean) | Awaits the `resultTask` and invokes one of the specified actions depending on whether the [Result](../Result/Result.md) is successful. |
| [SwitchAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Action&lt;T&gt;, Action&lt;Error&gt;, Boolean)](#switchasyncttaskresultt-actiont-actionerror-boolean) | Awaits the `resultTask` and invokes one of the specified actions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful. |
| [SwitchAsync(ValueTask&lt;Result&gt;, Action, Action&lt;Error&gt;, Boolean)](#switchasyncvaluetaskresult-action-actionerror-boolean) | Awaits the `resultTask` and invokes one of the specified actions depending on whether the [Result](../Result/Result.md) is successful. |
| [SwitchAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Action&lt;T&gt;, Action&lt;Error&gt;, Boolean)](#switchasynctvaluetaskresultt-actiont-actionerror-boolean) | Awaits the `resultTask` and invokes one of the specified actions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful. |

## Remarks
The overloads on a `Task` of a result return a `Task`, and those on a `ValueTask` of a result return a `ValueTask`.

`SwitchAsync` is [Switch](SwitchExtensions_Switch.md) on a task of a result: it ends a chain for side effects, as [MatchAsync](MatchAsyncExtensions_MatchAsync.md) ends it for a value. The actions are synchronous: they run after the task completes, and the returned task completes when the action has run. To run a side effect and keep chaining, use `IfSuccessAsync(…).IfErrorAsync(…)`, which return the result.

An action that returns a value is a side effect, and the value is ignored. A lambda that only throws is fine in either position.

An action that returns a `Task`, a `Task<T>`, a `ValueTask` or a `ValueTask<T>` (e.g., `x => SaveAsync(x)` or an `async` lambda) is a compile error (CS0619) in either position, and in both: `SwitchAsync` would not await the task. The message names [MatchAsync](MatchAsyncExtensions_MatchAsync.md) with two functions that return a task, which awaits the task of the function that ran (e.g., `await GetUserAsync(id).MatchAsync(x => SaveAsync(x), e => LogAsync(e))`). Two actions that both return a `ValueTask`, or one a `Task` and the other a `ValueTask`, are a compile error too, reported as an ambiguous call (CS0121).

> [!NOTE]
> When an asynchronous method awaits a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task) directly, continuation usually occurs in the same thread that created the task, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, `ConfigureAwait(false)` is used by default, so the passed actions run on a thread-pool thread. When the actions need the original context (e.g., to update UI elements), set the `continueOnCapturedContext` parameter to `true`, or set [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) to `true` once at startup.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.

> [!NOTE]
> A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask) can only be awaited once. Because of this, the overloads accepting `this ValueTask<Result> resultTask` / `this ValueTask<Result<T>> resultTask` consume the passed operation as part of evaluating its result, and they do not perform a `null` check on it (a `ValueTask<TResult>` is a non-nullable value type).

## SwitchAsync(Task&lt;Result&gt;, Action, Action&lt;Error&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/SwitchAsyncExtensions.Task.cs#L57" target="_blank">SwitchAsyncExtensions.Task.cs</a>

Awaits the `resultTask` and invokes the provided `ifSuccessAction` if the [Result](../Result/Result.md) represents a successful operation, or the provided `ifErrorAction` otherwise.

```csharp
public static Task SwitchAsync(this Task<Result> resultTask, Action ifSuccessAction, Action<Error> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the [Result](../Result/Result.md) to inspect.

#### `ifSuccessAction` [Action](https://learn.microsoft.com/dotnet/api/system.action)
The action to be invoked, only if the result represents a successful operation.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The action to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed actions, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)
The task that completes when the invoked action has run.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Examples
```csharp
await _userService.CreateUserAsync(userData).SwitchAsync(
    () => _logger.LogInformation("The user was added."),
    error => _logger.LogWarning("The user was not added: {Title}", error.Title));
```


## SwitchAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Action&lt;T&gt;, Action&lt;Error&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/SwitchAsyncExtensions.Task.cs#L101" target="_blank">SwitchAsyncExtensions.Task.cs</a>

Awaits the `resultTask` and invokes the provided `ifSuccessAction` with the value if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, or the provided `ifErrorAction` otherwise.

```csharp
public static Task SwitchAsync<T>(this Task<Result<T>> resultTask, Action<T> ifSuccessAction, Action<Error> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that returns the [Result&lt;T&gt;](../ResultT/ResultT.md) to inspect.

#### `ifSuccessAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;T&gt;
The action to be invoked, only if the result represents a successful operation. The value of the result is passed as a parameter to this action.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The action to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed actions, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)
The task that completes when the invoked action has run.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Examples
```csharp
await _userService.GetUserAsync(id).SwitchAsync(
    user => _logger.LogInformation("Found {Name}.", user.Name),
    error => _logger.LogWarning("No user: {Title}", error.Title));
```

## SwitchAsync(ValueTask&lt;Result&gt;, Action, Action&lt;Error&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/SwitchAsyncExtensions.ValueTask.cs#L44" target="_blank">SwitchAsyncExtensions.ValueTask.cs</a>

Awaits the `resultTask` and invokes the provided `ifSuccessAction` if the [Result](../Result/Result.md) represents a successful operation, or the provided `ifErrorAction` otherwise.

```csharp
public static ValueTask SwitchAsync(this ValueTask<Result> resultTask, Action ifSuccessAction, Action<Error> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the [Result](../Result/Result.md) to inspect.

#### `ifSuccessAction` [Action](https://learn.microsoft.com/dotnet/api/system.action)
The action to be invoked, only if the result represents a successful operation.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The action to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed actions, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)
The task that completes when the invoked action has run.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Examples
```csharp
await _userService.CreateUserAsync(userData).SwitchAsync(
    () => _logger.LogInformation("The user was added."),
    error => _logger.LogWarning("The user was not added: {Title}", error.Title));
```


## SwitchAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Action&lt;T&gt;, Action&lt;Error&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/SwitchAsyncExtensions.ValueTask.cs#L86" target="_blank">SwitchAsyncExtensions.ValueTask.cs</a>

Awaits the `resultTask` and invokes the provided `ifSuccessAction` with the value if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, or the provided `ifErrorAction` otherwise.

```csharp
public static ValueTask SwitchAsync<T>(this ValueTask<Result<T>> resultTask, Action<T> ifSuccessAction, Action<Error> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that returns the [Result&lt;T&gt;](../ResultT/ResultT.md) to inspect.

#### `ifSuccessAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;T&gt;
The action to be invoked, only if the result represents a successful operation. The value of the result is passed as a parameter to this action.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The action to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed actions, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)
The task that completes when the invoked action has run.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Examples
```csharp
await _userService.GetUserAsync(id).SwitchAsync(
    user => _logger.LogInformation("Found {Name}.", user.Name),
    error => _logger.LogWarning("No user: {Title}", error.Title));
```

## See Also
* [MatchAsync](MatchAsyncExtensions_MatchAsync.md)
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
