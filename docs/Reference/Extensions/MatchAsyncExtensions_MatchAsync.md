# MatchAsyncExtensions.MatchAsync Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Awaits a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1) or a [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1) of a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) and folds it into one value: runs the function for the success or the function for the error, and returns what it returns, as an asynchronous operation.

## Overloads
| Name | Description |
| ---- | ----------- |
| [MatchAsync&lt;TOut&gt;(Task&lt;Result&gt;, Func&lt;TOut&gt;, Func&lt;Error, TOut&gt;, Nullable&lt;Boolean&gt;)](#matchasynctouttaskresult-functout-funcerror-tout-nullableboolean) | Awaits the `resultTask`, invokes one of the specified synchronous functions depending on whether the [Result](../Result/Result.md) is successful and returns the value that it returns. |
| [MatchAsync&lt;TOut&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;TOut&gt;&gt;, Func&lt;Error, Task&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)](#matchasynctouttaskresult-functasktout-funcerror-tasktout-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result](../Result/Result.md) is successful, awaits it and returns the value that it returns. |
| [MatchAsync(Task&lt;Result&gt;, Func&lt;Task&gt;, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)](#matchasynctaskresult-functask-funcerror-task-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result](../Result/Result.md) is successful, and awaits it. |
| [MatchAsync&lt;T, TOut&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, TOut&gt;, Func&lt;Error, TOut&gt;, Nullable&lt;Boolean&gt;)](#matchasynct-touttaskresultt-funct-tout-funcerror-tout-nullableboolean) | Awaits the `resultTask`, invokes one of the specified synchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful and returns the value that it returns. |
| [MatchAsync&lt;T, TOut&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TOut&gt;&gt;, Func&lt;Error, Task&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)](#matchasynct-touttaskresultt-funct-tasktout-funcerror-tasktout-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful, awaits it and returns the value that it returns. |
| [MatchAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&gt;, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)](#matchasyncttaskresultt-funct-task-funcerror-task-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful, and awaits it. |
| [MatchAsync&lt;TOut&gt;(Task&lt;Result&gt;, Func&lt;ValueTask&lt;TOut&gt;&gt;, Func&lt;Error, ValueTask&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)](#matchasynctouttaskresult-funcvaluetasktout-funcerror-valuetasktout-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result](../Result/Result.md) is successful, awaits it and returns the value that it returns. |
| [MatchAsync(Task&lt;Result&gt;, Func&lt;ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)](#matchasynctaskresult-funcvaluetask-funcerror-valuetask-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result](../Result/Result.md) is successful, and awaits it. |
| [MatchAsync&lt;T, TOut&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TOut&gt;&gt;, Func&lt;Error, ValueTask&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)](#matchasynct-touttaskresultt-funct-valuetasktout-funcerror-valuetasktout-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful, awaits it and returns the value that it returns. |
| [MatchAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)](#matchasyncttaskresultt-funct-valuetask-funcerror-valuetask-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful, and awaits it. |
| [MatchAsync&lt;TOut&gt;(ValueTask&lt;Result&gt;, Func&lt;TOut&gt;, Func&lt;Error, TOut&gt;, Nullable&lt;Boolean&gt;)](#matchasynctoutvaluetaskresult-functout-funcerror-tout-nullableboolean) | Awaits the `resultTask`, invokes one of the specified synchronous functions depending on whether the [Result](../Result/Result.md) is successful and returns the value that it returns. |
| [MatchAsync&lt;TOut&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;TOut&gt;&gt;, Func&lt;Error, ValueTask&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)](#matchasynctoutvaluetaskresult-funcvaluetasktout-funcerror-valuetasktout-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result](../Result/Result.md) is successful, awaits it and returns the value that it returns. |
| [MatchAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)](#matchasyncvaluetaskresult-funcvaluetask-funcerror-valuetask-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result](../Result/Result.md) is successful, and awaits it. |
| [MatchAsync&lt;T, TOut&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, TOut&gt;, Func&lt;Error, TOut&gt;, Nullable&lt;Boolean&gt;)](#matchasynct-toutvaluetaskresultt-funct-tout-funcerror-tout-nullableboolean) | Awaits the `resultTask`, invokes one of the specified synchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful and returns the value that it returns. |
| [MatchAsync&lt;T, TOut&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TOut&gt;&gt;, Func&lt;Error, ValueTask&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)](#matchasynct-toutvaluetaskresultt-funct-valuetasktout-funcerror-valuetasktout-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful, awaits it and returns the value that it returns. |
| [MatchAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)](#matchasynctvaluetaskresultt-funct-valuetask-funcerror-valuetask-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful, and awaits it. |
| [MatchAsync&lt;TOut&gt;(ValueTask&lt;Result&gt;, Func&lt;Task&lt;TOut&gt;&gt;, Func&lt;Error, Task&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)](#matchasynctoutvaluetaskresult-functasktout-funcerror-tasktout-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result](../Result/Result.md) is successful, awaits it and returns the value that it returns. |
| [MatchAsync(ValueTask&lt;Result&gt;, Func&lt;Task&gt;, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)](#matchasyncvaluetaskresult-functask-funcerror-task-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result](../Result/Result.md) is successful, and awaits it. |
| [MatchAsync&lt;T, TOut&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TOut&gt;&gt;, Func&lt;Error, Task&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)](#matchasynct-toutvaluetaskresultt-funct-tasktout-funcerror-tasktout-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful, awaits it and returns the value that it returns. |
| [MatchAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&gt;, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)](#matchasynctvaluetaskresultt-funct-task-funcerror-task-nullableboolean) | Awaits the `resultTask`, invokes one of the specified asynchronous functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful, and awaits it. |

## Remarks
The overloads on a `Task` of a result return a `Task`, and those on a `ValueTask` of a result return a `ValueTask`.

`MatchAsync` is the fold of [Match](MatchExtensions_Match.md) on a task of a result: two functions that return one type. The functions are both synchronous (they run after the task completes), or both return a `Task<TOut>` or a `Task`, which is awaited. The value is returned as the function returns it, `null` included: a function that returns a `null` result gives `null`.

There is no `MatchAsync` on a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md): [Match](MatchExtensions_Match.md) returns the task of the function that ran (e.g., `await result.Match(x => CountAsync(x), e => CountAsync(0))`).

Two actions (e.g., `MatchAsync(x => Log(x), e => Log(e))`) do not compile (CS0029 and CS1662). Use [SwitchAsync](SwitchAsyncExtensions_SwitchAsync.md), which runs one of two actions after the task completes. To transform the value and handle the error, use `ThenAsync(…).IfErrorAsync(…)`. A call whose functions have no common return type (e.g., one synchronous and one asynchronous function) is rejected by the compiler.

Two functions that return a `Task` or a `ValueTask` without a value are awaited as actions: pass them without names, or as `ifSuccessAction` and `ifErrorAction`. Named `ifSuccessFunction` and `ifErrorFunction`, they bind to the overload with two synchronous functions, and the call returns the task of one function in a task, which nobody awaits: the analyzer reports it as an error ([MR0001](../../Analyzers/MR0001.md)).

On a task of a result, two functions that return the other kind of task (a `ValueTask` on a `Task`, a `Task` on a `ValueTask`) are awaited, and the kind of the receiver is kept.

> [!NOTE]
> When an asynchronous method awaits a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task) directly, continuation usually occurs in the same thread that created the task, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, `ConfigureAwait(false)` is used by default, so the passed functions run on a thread-pool thread. When the functions need the original context (e.g., to update UI elements), set the `continueOnCapturedContext` parameter to `true`, or set [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) to `true` once at startup.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.

> [!NOTE]
> A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask) can only be awaited once. Because of this, the overloads accepting `this ValueTask<Result> resultTask` / `this ValueTask<Result<T>> resultTask` consume the passed operation as part of evaluating its result, and they do not perform a `null` check on it (a `ValueTask<TResult>` is a non-nullable value type).

## MatchAsync&lt;TOut&gt;(Task&lt;Result&gt;, Func&lt;TOut&gt;, Func&lt;Error, TOut&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.Task.cs#L65" target="_blank">MatchAsyncExtensions.Task.cs</a>

Awaits the `resultTask` and invokes the provided synchronous `ifSuccessFunction` if the [Result](../Result/Result.md) represents a successful operation, or the provided `ifErrorFunction` otherwise, and returns the value that the invoked function returns.

```csharp
public static Task<TOut> MatchAsync<TOut>(this Task<Result> resultTask, Func<TOut> ifSuccessFunction, Func<Error, TOut> ifErrorFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `TOut`
The type that both functions return.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the [Result](../Result/Result.md) to fold.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;TOut&gt;
The function to be invoked, only if the result represents a successful operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), TOut&gt;
The function to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed functions, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TOut&gt;
The task of the value that `ifSuccessFunction` or `ifErrorFunction` returns.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Examples
```csharp
var message = await _userService.DeleteUserAsync(id).MatchAsync(
    () => "The user was deleted.",
    error => $"The user was not deleted: {error.Title}");
```


## MatchAsync&lt;TOut&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;TOut&gt;&gt;, Func&lt;Error, Task&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.Task.cs#L113" target="_blank">MatchAsyncExtensions.Task.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessFunction` if the [Result](../Result/Result.md) represents a successful operation, or the provided asynchronous `ifErrorFunction` otherwise, awaits it and returns the value that it returns.

```csharp
[OverloadResolutionPriority(1)]
public static Task<TOut> MatchAsync<TOut>(this Task<Result> resultTask, Func<Task<TOut>> ifSuccessFunction,
    Func<Error, Task<TOut>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `TOut`
The type of the value of the tasks that both functions return.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the [Result](../Result/Result.md) to fold.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a successful operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed functions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TOut&gt;
The task of the value that `ifSuccessFunction` or `ifErrorFunction` returns.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` or `ifErrorFunction` function returns a `null` task.

### Examples
```csharp
var remainingUsers = await _userService.DeleteUserAsync(id).MatchAsync(
    () => _userService.CountUsersAsync(),
    async error =>
    {
        await _auditService.LogErrorAsync(error);
        return -1;
    });
```


## MatchAsync(Task&lt;Result&gt;, Func&lt;Task&gt;, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.Task.cs#L166" target="_blank">MatchAsyncExtensions.Task.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessAction` if the [Result](../Result/Result.md) represents a successful operation, or the provided asynchronous `ifErrorAction` otherwise, and awaits it.

```csharp
[OverloadResolutionPriority(1)]
public static Task MatchAsync(this Task<Result> resultTask, Func<Task> ifSuccessAction, Func<Error, Task> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the [Result](../Result/Result.md) to fold.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be invoked, only if the result represents a successful operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed actions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)
The task that completes when the invoked action completes.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessAction` or `ifErrorAction` action returns a `null` task.

### Examples
```csharp
await _userService.DeleteUserAsync(id).MatchAsync(
    () => _auditService.LogUserDeletedAsync(id),
    error => _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T, TOut&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, TOut&gt;, Func&lt;Error, TOut&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.Task.cs#L227" target="_blank">MatchAsyncExtensions.Task.cs</a>

Awaits the `resultTask` and invokes the provided synchronous `ifSuccessFunction` with the value if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, or the provided `ifErrorFunction` otherwise, and returns the value that the invoked function returns.

```csharp
public static Task<TOut> MatchAsync<T, TOut>(this Task<Result<T>> resultTask, Func<T, TOut> ifSuccessFunction,
    Func<Error, TOut> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

#### `TOut`
The type that both functions return.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that returns the [Result&lt;T&gt;](../ResultT/ResultT.md) to fold.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, TOut&gt;
The function to be invoked, only if the result represents a successful operation. The value of the result is passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), TOut&gt;
The function to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed functions, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TOut&gt;
The task of the value that `ifSuccessFunction` or `ifErrorFunction` returns.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Examples
```csharp
var name = await _userService.GetUserAsync(id).MatchAsync(
    user => user.Name,
    error => "unknown");

// A Result<Address>: the error function returns the Error.
var addressResult = await _userService.GetUserAsync(id).MatchAsync(
    user => _addressBook.Find(user.Id),
    error => error);
```


## MatchAsync&lt;T, TOut&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TOut&gt;&gt;, Func&lt;Error, Task&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.Task.cs#L276" target="_blank">MatchAsyncExtensions.Task.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessFunction` with the value if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, or the provided asynchronous `ifErrorFunction` otherwise, awaits it and returns the value that it returns.

```csharp
[OverloadResolutionPriority(1)]
public static Task<TOut> MatchAsync<T, TOut>(this Task<Result<T>> resultTask, Func<T, Task<TOut>> ifSuccessFunction,
    Func<Error, Task<TOut>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

#### `TOut`
The type of the value of the tasks that both functions return.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that returns the [Result&lt;T&gt;](../ResultT/ResultT.md) to fold.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a successful operation. The value of the result is passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed functions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TOut&gt;
The task of the value that `ifSuccessFunction` or `ifErrorFunction` returns.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` or `ifErrorFunction` function returns a `null` task.

### Examples
```csharp
var orderCount = await _userService.GetUserAsync(id).MatchAsync(
    user => _orderService.CountOrdersAsync(user.Id),
    error => Task.FromResult(0));
```


## MatchAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&gt;, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.Task.cs#L330" target="_blank">MatchAsyncExtensions.Task.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessAction` with the value if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, or the provided asynchronous `ifErrorAction` otherwise, and awaits it.

```csharp
[OverloadResolutionPriority(1)]
public static Task MatchAsync<T>(this Task<Result<T>> resultTask, Func<T, Task> ifSuccessAction, Func<Error, Task> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that returns the [Result&lt;T&gt;](../ResultT/ResultT.md) to fold.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be invoked, only if the result represents a successful operation. The value of the result is passed as a parameter to this action.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed actions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)
The task that completes when the invoked action completes.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessAction` or `ifErrorAction` action returns a `null` task.

### Examples
```csharp
await _userService.GetUserAsync(id).MatchAsync(
    user => _mailService.SendWelcomeAsync(user),
    error => _auditService.LogErrorAsync(error));
```

## MatchAsync&lt;TOut&gt;(Task&lt;Result&gt;, Func&lt;ValueTask&lt;TOut&gt;&gt;, Func&lt;Error, ValueTask&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.Task.cs#L395" target="_blank">MatchAsyncExtensions.Task.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessFunction` if the [Result](../Result/Result.md) represents a successful operation, or the provided asynchronous `ifErrorFunction` otherwise, awaits it and returns the value that it returns.

```csharp
public static Task<TOut> MatchAsync<TOut>(this Task<Result> resultTask, Func<ValueTask<TOut>> ifSuccessFunction,
    Func<Error, ValueTask<TOut>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `TOut`
The type of the value of the tasks that both functions return.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the [Result](../Result/Result.md) to fold.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a successful operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed functions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TOut&gt;
The task of the value that `ifSuccessFunction` or `ifErrorFunction` returns.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` or `ifErrorFunction` function returns a `null` task.


### Remarks
The function returns a `ValueTask`, the other kind of task: it is awaited, and a `Task` is returned, as for a function that returns a `Task`.

## MatchAsync(Task&lt;Result&gt;, Func&lt;ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.Task.cs#L451" target="_blank">MatchAsyncExtensions.Task.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessAction` if the [Result](../Result/Result.md) represents a successful operation, or the provided asynchronous `ifErrorAction` otherwise, and awaits it.

```csharp
public static Task MatchAsync(this Task<Result> resultTask, Func<ValueTask> ifSuccessAction, Func<Error, ValueTask> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the [Result](../Result/Result.md) to fold.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be invoked, only if the result represents a successful operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed actions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)
The task that completes when the invoked action completes.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessAction` or `ifErrorAction` action returns a `null` task.


### Remarks
The function returns a `ValueTask`, the other kind of task: it is awaited, and a `Task` is returned, as for a function that returns a `Task`.

## MatchAsync&lt;T, TOut&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TOut&gt;&gt;, Func&lt;Error, ValueTask&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.Task.cs#L511" target="_blank">MatchAsyncExtensions.Task.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessFunction` with the value if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, or the provided asynchronous `ifErrorFunction` otherwise, awaits it and returns the value that it returns.

```csharp
public static Task<TOut> MatchAsync<T, TOut>(this Task<Result<T>> resultTask, Func<T, ValueTask<TOut>> ifSuccessFunction,
    Func<Error, ValueTask<TOut>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

#### `TOut`
The type of the value of the tasks that both functions return.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that returns the [Result&lt;T&gt;](../ResultT/ResultT.md) to fold.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a successful operation. The value of the result is passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed functions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TOut&gt;
The task of the value that `ifSuccessFunction` or `ifErrorFunction` returns.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` or `ifErrorFunction` function returns a `null` task.


### Remarks
The function returns a `ValueTask`, the other kind of task: it is awaited, and a `Task` is returned, as for a function that returns a `Task`.

## MatchAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.Task.cs#L568" target="_blank">MatchAsyncExtensions.Task.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessAction` with the value if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, or the provided asynchronous `ifErrorAction` otherwise, and awaits it.

```csharp
public static Task MatchAsync<T>(this Task<Result<T>> resultTask, Func<T, ValueTask> ifSuccessAction, Func<Error, ValueTask> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that returns the [Result&lt;T&gt;](../ResultT/ResultT.md) to fold.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be invoked, only if the result represents a successful operation. The value of the result is passed as a parameter to this action.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed actions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)
The task that completes when the invoked action completes.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessAction` or `ifErrorAction` action returns a `null` task.


### Remarks
The function returns a `ValueTask`, the other kind of task: it is awaited, and a `Task` is returned, as for a function that returns a `Task`.

## MatchAsync&lt;TOut&gt;(ValueTask&lt;Result&gt;, Func&lt;TOut&gt;, Func&lt;Error, TOut&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.ValueTask.cs#L51" target="_blank">MatchAsyncExtensions.ValueTask.cs</a>

Awaits the `resultTask` and invokes the provided synchronous `ifSuccessFunction` if the [Result](../Result/Result.md) represents a successful operation, or the provided `ifErrorFunction` otherwise, and returns the value that the invoked function returns.

```csharp
public static ValueTask<TOut> MatchAsync<TOut>(this ValueTask<Result> resultTask, Func<TOut> ifSuccessFunction, Func<Error, TOut> ifErrorFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `TOut`
The type that both functions return.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the [Result](../Result/Result.md) to fold.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;TOut&gt;
The function to be invoked, only if the result represents a successful operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), TOut&gt;
The function to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed functions, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TOut&gt;
The task of the value that `ifSuccessFunction` or `ifErrorFunction` returns.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Examples
```csharp
var message = await _userService.DeleteUserAsync(id).MatchAsync(
    () => "The user was deleted.",
    error => $"The user was not deleted: {error.Title}");
```


## MatchAsync&lt;TOut&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;TOut&gt;&gt;, Func&lt;Error, ValueTask&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.ValueTask.cs#L94" target="_blank">MatchAsyncExtensions.ValueTask.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessFunction` if the [Result](../Result/Result.md) represents a successful operation, or the provided asynchronous `ifErrorFunction` otherwise, awaits it and returns the value that it returns.

```csharp
public static ValueTask<TOut> MatchAsync<TOut>(this ValueTask<Result> resultTask, Func<ValueTask<TOut>> ifSuccessFunction,
    Func<Error, ValueTask<TOut>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `TOut`
The type of the value of the tasks that both functions return.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the [Result](../Result/Result.md) to fold.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a successful operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed functions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TOut&gt;
The task of the value that `ifSuccessFunction` or `ifErrorFunction` returns.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Examples
```csharp
var remainingUsers = await _userService.DeleteUserAsync(id).MatchAsync(
    () => _userService.CountUsersAsync(),
    async error =>
    {
        await _auditService.LogErrorAsync(error);
        return -1;
    });
```


## MatchAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.ValueTask.cs#L142" target="_blank">MatchAsyncExtensions.ValueTask.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessAction` if the [Result](../Result/Result.md) represents a successful operation, or the provided asynchronous `ifErrorAction` otherwise, and awaits it.

```csharp
public static ValueTask MatchAsync(this ValueTask<Result> resultTask, Func<ValueTask> ifSuccessAction, Func<Error, ValueTask> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the [Result](../Result/Result.md) to fold.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be invoked, only if the result represents a successful operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed actions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)
The task that completes when the invoked action completes.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Examples
```csharp
await _userService.DeleteUserAsync(id).MatchAsync(
    () => _auditService.LogUserDeletedAsync(id),
    error => _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T, TOut&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, TOut&gt;, Func&lt;Error, TOut&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.ValueTask.cs#L201" target="_blank">MatchAsyncExtensions.ValueTask.cs</a>

Awaits the `resultTask` and invokes the provided synchronous `ifSuccessFunction` with the value if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, or the provided `ifErrorFunction` otherwise, and returns the value that the invoked function returns.

```csharp
public static ValueTask<TOut> MatchAsync<T, TOut>(this ValueTask<Result<T>> resultTask, Func<T, TOut> ifSuccessFunction,
    Func<Error, TOut> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

#### `TOut`
The type that both functions return.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that returns the [Result&lt;T&gt;](../ResultT/ResultT.md) to fold.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, TOut&gt;
The function to be invoked, only if the result represents a successful operation. The value of the result is passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), TOut&gt;
The function to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed functions, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TOut&gt;
The task of the value that `ifSuccessFunction` or `ifErrorFunction` returns.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Examples
```csharp
var name = await _userService.GetUserAsync(id).MatchAsync(
    user => user.Name,
    error => "unknown");

// A Result<Address>: the error function returns the Error.
var addressResult = await _userService.GetUserAsync(id).MatchAsync(
    user => _addressBook.Find(user.Id),
    error => error);
```


## MatchAsync&lt;T, TOut&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TOut&gt;&gt;, Func&lt;Error, ValueTask&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.ValueTask.cs#L245" target="_blank">MatchAsyncExtensions.ValueTask.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessFunction` with the value if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, or the provided asynchronous `ifErrorFunction` otherwise, awaits it and returns the value that it returns.

```csharp
public static ValueTask<TOut> MatchAsync<T, TOut>(this ValueTask<Result<T>> resultTask, Func<T, ValueTask<TOut>> ifSuccessFunction,
    Func<Error, ValueTask<TOut>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

#### `TOut`
The type of the value of the tasks that both functions return.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that returns the [Result&lt;T&gt;](../ResultT/ResultT.md) to fold.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a successful operation. The value of the result is passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed functions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TOut&gt;
The task of the value that `ifSuccessFunction` or `ifErrorFunction` returns.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Examples
```csharp
var orderCount = await _userService.GetUserAsync(id).MatchAsync(
    user => _orderService.CountOrdersAsync(user.Id),
    error => ValueTask.FromResult(0));
```


## MatchAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.ValueTask.cs#L294" target="_blank">MatchAsyncExtensions.ValueTask.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessAction` with the value if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, or the provided asynchronous `ifErrorAction` otherwise, and awaits it.

```csharp
public static ValueTask MatchAsync<T>(this ValueTask<Result<T>> resultTask, Func<T, ValueTask> ifSuccessAction, Func<Error, ValueTask> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that returns the [Result&lt;T&gt;](../ResultT/ResultT.md) to fold.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be invoked, only if the result represents a successful operation. The value of the result is passed as a parameter to this action.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed actions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)
The task that completes when the invoked action completes.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Examples
```csharp
await _userService.GetUserAsync(id).MatchAsync(
    user => _mailService.SendWelcomeAsync(user),
    error => _auditService.LogErrorAsync(error));
```

## MatchAsync&lt;TOut&gt;(ValueTask&lt;Result&gt;, Func&lt;Task&lt;TOut&gt;&gt;, Func&lt;Error, Task&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.ValueTask.cs#L359" target="_blank">MatchAsyncExtensions.ValueTask.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessFunction` if the [Result](../Result/Result.md) represents a successful operation, or the provided asynchronous `ifErrorFunction` otherwise, awaits it and returns the value that it returns.

```csharp
[OverloadResolutionPriority(1)]
public static ValueTask<TOut> MatchAsync<TOut>(this ValueTask<Result> resultTask, Func<Task<TOut>> ifSuccessFunction,
    Func<Error, Task<TOut>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `TOut`
The type of the value of the tasks that both functions return.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the [Result](../Result/Result.md) to fold.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a successful operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed functions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TOut&gt;
The task of the value that `ifSuccessFunction` or `ifErrorFunction` returns.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.


### Remarks
The function returns a `Task`, the other kind of task: it is awaited, and a `ValueTask` is returned, as for a function that returns a `ValueTask`.

## MatchAsync(ValueTask&lt;Result&gt;, Func&lt;Task&gt;, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.ValueTask.cs#L415" target="_blank">MatchAsyncExtensions.ValueTask.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessAction` if the [Result](../Result/Result.md) represents a successful operation, or the provided asynchronous `ifErrorAction` otherwise, and awaits it.

```csharp
[OverloadResolutionPriority(1)]
public static ValueTask MatchAsync(this ValueTask<Result> resultTask, Func<Task> ifSuccessAction, Func<Error, Task> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the [Result](../Result/Result.md) to fold.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be invoked, only if the result represents a successful operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed actions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)
The task that completes when the invoked action completes.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.


### Remarks
The function returns a `Task`, the other kind of task: it is awaited, and a `ValueTask` is returned, as for a function that returns a `ValueTask`.

## MatchAsync&lt;T, TOut&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TOut&gt;&gt;, Func&lt;Error, Task&lt;TOut&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.ValueTask.cs#L475" target="_blank">MatchAsyncExtensions.ValueTask.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessFunction` with the value if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, or the provided asynchronous `ifErrorFunction` otherwise, awaits it and returns the value that it returns.

```csharp
[OverloadResolutionPriority(1)]
public static ValueTask<TOut> MatchAsync<T, TOut>(this ValueTask<Result<T>> resultTask, Func<T, Task<TOut>> ifSuccessFunction,
    Func<Error, Task<TOut>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

#### `TOut`
The type of the value of the tasks that both functions return.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that returns the [Result&lt;T&gt;](../ResultT/ResultT.md) to fold.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a successful operation. The value of the result is passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TOut&gt;&gt;
The asynchronous function to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed functions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TOut&gt;
The task of the value that `ifSuccessFunction` or `ifErrorFunction` returns.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.


### Remarks
The function returns a `Task`, the other kind of task: it is awaited, and a `ValueTask` is returned, as for a function that returns a `ValueTask`.

## MatchAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&gt;, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/MatchAsyncExtensions.ValueTask.cs#L532" target="_blank">MatchAsyncExtensions.ValueTask.cs</a>

Awaits the `resultTask` and invokes the provided asynchronous `ifSuccessAction` with the value if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, or the provided asynchronous `ifErrorAction` otherwise, and awaits it.

```csharp
[OverloadResolutionPriority(1)]
public static ValueTask MatchAsync<T>(this ValueTask<Result<T>> resultTask, Func<T, Task> ifSuccessAction, Func<Error, Task> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that returns the [Result&lt;T&gt;](../ResultT/ResultT.md) to fold.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be invoked, only if the result represents a successful operation. The value of the result is passed as a parameter to this action.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be invoked, only if the result represents a failed operation. The [Error](../Error/Error.md) instance contained in the result is passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed actions when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)
The task that completes when the invoked action completes.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.


### Remarks
The function returns a `Task`, the other kind of task: it is awaited, and a `ValueTask` is returned, as for a function that returns a `ValueTask`.

## See Also
* [SwitchAsync](SwitchAsyncExtensions_SwitchAsync.md)
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
