# IfErrorAsyncExtensions.IfErrorAsync Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a asynchronous side effect if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), and returns the result unchanged.

## Overloads
| Name | Description |
| ---- | ----------- |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)](#iferrorasynctresulttresult-funcerror-task-nullableboolean)  | Executes the provided asynchronous action, if the `TResult` instance represents a failed operation, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)](#iferrorasynctresulttasktresult-funcerror-task-nullableboolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Action&lt;Error&gt;, Nullable&lt;Boolean&gt;)](#iferrorasynctresulttasktresult-actionerror-nullableboolean) | Runs the synchronous action if the result that the `resultTask` returns is failed, and returns the result unchanged. |
| [IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)](#iferrorasynctresulttasktresult-funcerror-valuetask-nullableboolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)](#iferrorasynctresulttresult-funcerror-valuetask-nullableboolean)  | Executes the provided asynchronous action, if the `TResult` instance represents a failed operation, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)](#iferrorasynctresultvaluetasktresult-funcerror-valuetask-nullableboolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Action&lt;Error&gt;, Nullable&lt;Boolean&gt;)](#iferrorasynctresultvaluetasktresult-actionerror-nullableboolean) | Runs the synchronous action if the result that the `resultTask` returns is failed, and returns the result unchanged. |
| [IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)](#iferrorasynctresultvaluetasktresult-funcerror-task-nullableboolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |
| [IfErrorAsync&lt;TResult, TValue&gt;(Task&lt;TResult&gt;, Func&lt;Error, ValueTask&lt;TValue&gt;&gt;, Nullable&lt;Boolean&gt;)](#iferrorasynctresult-tvaluetasktresult-funcerror-valuetasktvalue-nullableboolean) | Executes the provided asynchronous function as a side effect, if the provided asynchronous operation returns a failed `TResult` instance (i.e., contains an [Error](../Error/Error.md)); the value of the task that it returns is ignored. |
| [IfErrorAsync&lt;TResult, TValue&gt;(TResult, Func&lt;Error, ValueTask&lt;TValue&gt;&gt;, Nullable&lt;Boolean&gt;)](#iferrorasynctresult-tvaluetresult-funcerror-valuetasktvalue-nullableboolean) | Executes the provided asynchronous function as a side effect, only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)); the value of the task that it returns is ignored. |
| [IfErrorAsync&lt;TResult, TValue&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&lt;TValue&gt;&gt;, Nullable&lt;Boolean&gt;)](#iferrorasynctresult-tvaluevaluetasktresult-funcerror-valuetasktvalue-nullableboolean) | Executes the provided asynchronous function as a side effect, if the provided asynchronous operation returns a failed `TResult` instance (i.e., contains an [Error](../Error/Error.md)); the value of the task that it returns is ignored. |

## Remarks
On a `Result` or a `Result<T>`, the function decides the kind of task: a function that returns a `ValueTask` gives a `ValueTask`, and an `async` lambda gives a `Task` (with an explicit return type, e.g., `async ValueTask<int> (int x) => …`, it gives a `ValueTask`). On a `Task` or a `ValueTask` of a result, the kind of the receiver is kept.

The overload with a synchronous action (e.g., `IfErrorAsync(error => Log(error))`) runs it after the task completes. A synchronous function that returns a value runs as an action, and its value is ignored. On a task of a result, a function that returns the other kind of task (a `ValueTask` on a `Task`, a `Task` on a `ValueTask`) is awaited, and the kind of the receiver is kept.

To replace a failed result (recover with a value, or return another error), use [ElseAsync](ElseAsyncExtensions_ElseAsync.md). `IfErrorAsync` awaits a function that returns a `Task<TValue>` and ignores its value, so a failure is not turned into a success. The analyzer reports the functions whose value is ignored by mistake:

* a function that returns a `Result`, a `Result<T>` or an `Error`, in a task or not, is an error ([MR0004](../../Analyzers/MR0004.md));
* a function that returns a value of the type of the result’s value, or a task of it, in a call whose result is used, is a warning ([MR0005](../../Analyzers/MR0005.md)): in version 2 such a call replaced the failed result. For a side effect that happens to return that type, use a statement lambda (e.g., `async e => { await CountFailureAsync(e); }`).

A function that returns a `ValueTask<TValue>` has overloads of its own, because a `ValueTask<TValue>` does not convert to a `ValueTask`: the task is awaited, and its value is ignored. An `async` lambda with a block body that returns a value (e.g., `async e => { await Task.Yield(); return 0; }`) binds to these overloads too. The analyzer rules above apply to them as well.

> [!NOTE]
> When an asynchronous method awaits a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task) directly, continuation usually occurs in the same thread that created the task, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, `ConfigureAwait(false)` is used by default, so in the methods that take a `Task` of a result the passed delegates run on a thread-pool thread. When the delegates need the original context (e.g., to update UI elements), set the `continueOnCapturedContext` parameter to `true`, or set [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) to `true` once at startup.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.

> [!NOTE]
> A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask) can only be awaited once. Because of this, the overloads accepting `this ValueTask<TResult> resultTask` consume the passed operation as part of evaluating its result, and they do not perform a `null` check on it (a `ValueTask<TResult>` is a non-nullable value type).

## IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/IfErrorAsyncExtensions.Task.cs#L67" target="_blank">IfErrorAsyncExtensions.Task.cs</a>

Executes the provided asynchronous action, only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
[OverloadResolutionPriority(1)]
public static Task<TResult> IfErrorAsync<TResult>(this TResult result, Func<Error, Task> ifErrorAction, bool? continueOnCapturedContext = null)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `result` that implements the [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed only if the current instance of `TResult` represents a failed operation. The [Error](../Error/Error.md) instance contained in the `TResult` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;TResult&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifErrorAction` function returns a `null` task.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

await createUserResult.IfErrorAsync(async error =>
{
    await _auditService.LogErrorAsync("Failed to create user: {ErrorDetail}", error.Detail);
    Metrics.IncrementCounter("user_creation_failures");
});
```


## IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/IfErrorAsyncExtensions.Task.cs#L131" target="_blank">IfErrorAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, if the provided asynchronous operation returns a failed`TResult` instance (i.e., contains an [Error](../Error/Error.md)), and returns its result as an asynchronous operation.

```csharp
[OverloadResolutionPriority(1)]
public static Task<TResult> IfErrorAsync<TResult>(
    this Task<TResult> resultTask,
    Func<Error, Task> ifErrorAction,
    bool? continueOnCapturedContext = null)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `resultTask` that implements the [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TResult&gt;
The asynchronous operation that represents an instance of `TResult` (that implements [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) to be executed and which result is to be evaluated.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed only if the `resultTask` represents a failed operation. The [Error](../Error/Error.md) instance contained in the outcome will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;TResult&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifErrorAction` function returns a `null` task.

### Remarks
The `IfErrorAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .IfErrorAsync(async error =>
    {
        await _auditService.LogErrorAsync("Failed to create user: {ErrorDetail}", error.Detail);
        Metrics.IncrementCounter("user_creation_failures");
    });
```


## IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Action&lt;Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/IfErrorAsyncExtensions.Task.cs#L184" target="_blank">IfErrorAsyncExtensions.Task.cs</a>

Runs the synchronous action if the result that the `resultTask` returns is failed, and returns the result unchanged.

```csharp
public static Task<TResult> IfErrorAsync<TResult>(this Task<TResult> resultTask, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the result: [Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md).

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TResult&gt;
The asynchronous operation that returns the result to inspect. Must not be `null`.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The synchronous action that receives the error. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TResult&gt;
The result that the `resultTask` returns.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `resultTask` or the `ifErrorAction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The action is synchronous: it runs after the `resultTask` completes, and it is not called for a successful result. The result is returned unchanged.

### Examples
```csharp
var userResult = await _userService.GetUserAsync(id)
    .IfErrorAsync(error => _logger.LogWarning("{Title}", error.Title));
```

## IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/IfErrorAsyncExtensions.Task.cs#L248" target="_blank">IfErrorAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, if the provided asynchronous operation returns a failed`TResult` instance (i.e., contains an [Error](../Error/Error.md)), and returns its result as an asynchronous operation.

```csharp
public static Task<TResult> IfErrorAsync<TResult>(
    this Task<TResult> resultTask,
    Func<Error, ValueTask> ifErrorAction,
    bool? continueOnCapturedContext = null)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `resultTask` that implements the [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TResult&gt;
The asynchronous operation that represents an instance of `TResult` (that implements [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) to be executed and which result is to be evaluated.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed only if the `resultTask` represents a failed operation. The [Error](../Error/Error.md) instance contained in the outcome will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;TResult&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The function returns a `ValueTask`, the other kind of task: it is awaited, and a `Task` is returned, as for a function that returns a `Task`.

The `IfErrorAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

## IfErrorAsync&lt;TResult, TValue&gt;(Task&lt;TResult&gt;, Func&lt;Error, ValueTask&lt;TValue&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/IfErrorAsyncExtensions.Task.cs#L317" target="_blank">IfErrorAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function as a side effect, if the provided asynchronous operation returns a failed `TResult` instance (i.e., contains an [Error](../Error/Error.md)): the task that the function returns is awaited, and its value is ignored.

```csharp
public static Task<TResult> IfErrorAsync<TResult, TValue>(
    this Task<TResult> resultTask,
    Func<Error, ValueTask<TValue>> ifErrorAction,
    bool? continueOnCapturedContext = null)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `resultTask` that implements the [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

#### `TValue`
The type of the value that the function produces. The value is ignored.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TResult&gt;
The asynchronous operation that represents an instance of `TResult` (that implements [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) to be executed and which result is to be evaluated.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TValue&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a failed operation. The [Error](../Error/Error.md) instance of the result will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TResult&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
A `ValueTask<TValue>` does not convert to a `ValueTask`, so this overload takes it: the task is awaited, and its value is ignored, so a failure is not turned into a success. To replace the failed result, use [ElseAsync](ElseAsyncExtensions_ElseAsync.md). A `Task` is returned, as for the other functions on a `Task` of a result.

## IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/IfErrorAsyncExtensions.ValueTask.cs#L27" target="_blank">IfErrorAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous action, only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static ValueTask<TResult> IfErrorAsync<TResult>(this TResult result, Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `result` that implements the [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed only if the current instance of `TResult` represents a failed operation. The [Error](../Error/Error.md) instance contained in the `TResult` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;TResult&gt;
The value task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifErrorAction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

await createUserResult.IfErrorAsync(async error =>
{
    await _auditService.LogErrorAsync("Failed to create user: {ErrorDetail}", error.Detail);
    Metrics.IncrementCounter("user_creation_failures");
});
```


## IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/IfErrorAsyncExtensions.ValueTask.cs#L84" target="_blank">IfErrorAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, if the provided asynchronous operation returns a failed`TResult` instance (i.e., contains an [Error](../Error/Error.md)), and returns its result as an asynchronous operation.

```csharp
public static ValueTask<TResult> IfErrorAsync<TResult>(
    this ValueTask<TResult> resultTask,
    Func<Error, ValueTask> ifErrorAction,
    bool? continueOnCapturedContext = null)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `resultTask` that implements the [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TResult&gt;
The asynchronous operation that represents an instance of `TResult` (that implements [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) to be executed and which result is to be evaluated.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed only if the `resultTask` represents a failed operation. The [Error](../Error/Error.md) instance contained in the outcome will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;TResult&gt;
The value task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `IfErrorAsync` method awaits the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .IfErrorAsync(async error =>
    {
        await _auditService.LogErrorAsync("Failed to create user: {ErrorDetail}", error.Detail);
        Metrics.IncrementCounter("user_creation_failures");
    });
```


## IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Action&lt;Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/IfErrorAsyncExtensions.ValueTask.cs#L131" target="_blank">IfErrorAsyncExtensions.ValueTask.cs</a>

Runs the synchronous action if the result that the `resultTask` returns is failed, and returns the result unchanged.

```csharp
public static ValueTask<TResult> IfErrorAsync<TResult>(this ValueTask<TResult> resultTask, Action<Error> ifErrorAction, bool? continueOnCapturedContext = null)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the result: [Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md).

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TResult&gt;
The asynchronous operation that returns the result to inspect.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The synchronous action that receives the error. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TResult&gt;
The result that the `resultTask` returns.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `ifErrorAction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The action is synchronous: it runs after the `resultTask` completes, and it is not called for a successful result. The result is returned unchanged.

### Examples
```csharp
var userResult = await _userService.GetUserAsync(id)
    .IfErrorAsync(error => _logger.LogWarning("{Title}", error.Title));
```

## IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, Task&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/IfErrorAsyncExtensions.ValueTask.cs#L194" target="_blank">IfErrorAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, if the provided asynchronous operation returns a failed`TResult` instance (i.e., contains an [Error](../Error/Error.md)), and returns its result as an asynchronous operation.

```csharp
[OverloadResolutionPriority(1)]
public static ValueTask<TResult> IfErrorAsync<TResult>(
    this ValueTask<TResult> resultTask,
    Func<Error, Task> ifErrorAction,
    bool? continueOnCapturedContext = null)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `resultTask` that implements the [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TResult&gt;
The asynchronous operation that represents an instance of `TResult` (that implements [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) to be executed and which result is to be evaluated.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed only if the `resultTask` represents a failed operation. The [Error](../Error/Error.md) instance contained in the outcome will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;TResult&gt;
The value task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The function returns a `Task`, the other kind of task: it is awaited, and a `ValueTask` is returned, as for a function that returns a `ValueTask`.

The `IfErrorAsync` method awaits the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

## IfErrorAsync&lt;TResult, TValue&gt;(TResult, Func&lt;Error, ValueTask&lt;TValue&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/IfErrorAsyncExtensions.ValueTask.cs#L256" target="_blank">IfErrorAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function as a side effect, only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)): the task that the function returns is awaited, and its value is ignored.

```csharp
public static ValueTask<TResult> IfErrorAsync<TResult, TValue>(
    this TResult result,
    Func<Error, ValueTask<TValue>> ifErrorAction,
    bool? continueOnCapturedContext = null)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `result` that implements the [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

#### `TValue`
The type of the value that the function produces. The value is ignored.

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TValue&gt;&gt;
The asynchronous function to be executed only if the current instance of `TResult` represents a failed operation. The [Error](../Error/Error.md) instance of the result will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;TResult&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifErrorAction` parameter is `null`.

### Remarks
A `ValueTask<TValue>` does not convert to a `ValueTask`, so this overload takes it: the task is awaited, and its value is ignored, so a failure is not turned into a success. To replace the failed result, use [ElseAsync](ElseAsyncExtensions_ElseAsync.md).

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

// IncrementAsync returns a ValueTask<long>.
await createUserResult.IfErrorAsync(error => _failureCounter.IncrementAsync(error.Category));
```

## IfErrorAsync&lt;TResult, TValue&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&lt;TValue&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/IfErrorAsyncExtensions.ValueTask.cs#L311" target="_blank">IfErrorAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function as a side effect, if the provided asynchronous operation returns a failed `TResult` instance (i.e., contains an [Error](../Error/Error.md)): the task that the function returns is awaited, and its value is ignored.

```csharp
public static ValueTask<TResult> IfErrorAsync<TResult, TValue>(
    this ValueTask<TResult> resultTask,
    Func<Error, ValueTask<TValue>> ifErrorAction,
    bool? continueOnCapturedContext = null)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `resultTask` that implements the [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

#### `TValue`
The type of the value that the function produces. The value is ignored.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TResult&gt;
The asynchronous operation that represents an instance of `TResult` (that implements [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) to be executed and which result is to be evaluated.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TValue&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a failed operation. The [Error](../Error/Error.md) instance of the result will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TResult&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
A `ValueTask<TValue>` does not convert to a `ValueTask`, so this overload takes it: the task is awaited, and its value is ignored, so a failure is not turned into a success. To replace the failed result, use [ElseAsync](ElseAsyncExtensions_ElseAsync.md).

## See Also
* [ElseAsync](ElseAsyncExtensions_ElseAsync.md)
* [IOperationResult](../IOperationResult/IOperationResult.md)
