# ElseTaskAsyncExtensions.ElseAsync Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a asynchronous function if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), and returns the result that the function returns instead: e.g., a recovered value or another error.

## Overloads
| Name | Description |
| ---- | ----------- |
| [ElseAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](#elseasynctresulttresult-funcerror-tasktresult-boolean) | Executes the provided asynchronous function, if the `TResult` instance represents a failed operation and returns its result, as an asynchronous operation. |
| [ElseAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](#elseasynctresulttasktresult-funcerror-tasktresult-boolean) | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |
| [ElseAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, TResult&gt;, Boolean)](#elseasynctresulttasktresult-funcerror-tresult-boolean) | Runs the synchronous function if the result that the `resultTask` returns is failed, and returns the result that the function returns instead (e.g., a recovered value or another error). |

## Remarks
The overload with a synchronous function (e.g., `ElseAsync(error => Result.FromValue(User.Guest))`) runs it after the task completes. A synchronous action is a compile error (CS0619) that names `IfErrorAsync`. A delegate that returns a `ValueTask` is a compile error too: this page is for the `Task` namespace; import `Maple.Results.Extensions.ValueTasks` for the other one.

A successful result is returned unchanged, and the function is not called. An action (a function that returns a task without a value) is a compile error (CS0619) that names [IfErrorAsync](IfErrorTaskAsyncExtensions_IfErrorAsync.md), which runs a side effect and returns the result unchanged.

> [!NOTE]
> When an asynchronous method awaits a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task) directly, continuation usually occurs in the same thread that created the task, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, `ConfigureAwait(false)` is used by default, so in the methods that take a `Task` of a result the passed delegates run on a thread-pool thread. When the delegates need the original context (e.g., to update UI elements), set the `continueOnCapturedContext` parameter to `true`, or set [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) to `true` once at startup.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.


## ElseAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ElseTaskAsyncExtensions.cs#L64" target="_blank">IfErrorExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<TResult> ElseAsync<TResult>(this TResult result, Func<Error, Task<TResult>> ifErrorFunction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `result` that implements the [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;TResult&gt;&gt;
The asynchronous function to be executed, only if the current instance of `TResult` represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `TResult` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;TResult&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifErrorFunction` function returns a `null` task or a task with a `null` result.

### Examples
```csharp
var userResult = await _userService.GetUserAsync(userData);

await userResult.ElseAsync(async error =>
{
    if (error.Category == ErrorCategory.NotFound)
    {
        // return a default user when not found
        return Result.FromValue(Users.NotFoundDefaultUser());
    }

    await _auditService.LogErrorAsync("Failed to create user: {ErrorDetail}", error.Detail);
    Metrics.IncrementCounter("user_retrieval_failures");

    return error;
});
```


## ElseAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ElseTaskAsyncExtensions.cs#L129" target="_blank">ElseTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, if the provided asynchronous operation returns a failed`TResult` instance (i.e., contains an [Error](../Error/Error.md)), and returns its result as an asynchronous operation.

```csharp
public static async Task<TResult> ElseAsync<TResult>(this Task<TResult> resultTask,
    Func<Error, Task<TResult>> ifErrorFunction, bool? continueOnCapturedContext = null)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `resultTask` that implements the [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `resultTask` TResult
The asynchronous operation that represents an instance of `TResult` (that implements [IOperationResult](../IOperationResult/IOperationResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) to be executed and which result is to be evaluated.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;TResult&gt;&gt;
The asynchronous function to be executed, only if the current instance of `TResult` represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the outcome will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;TResult&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifErrorFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The `ElseAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var userResult = await _userService.GetUserAsync(userData)
    .ElseAsync(async error =>
    {
        if (error.Category == ErrorCategory.NotFound)
        {
            // return a default user when not found
            return Result.FromValue(Users.NotFoundDefaultUser());
        }

        await _auditService.LogErrorAsync("Failed to create user: {ErrorDetail}", error.Detail);
        Metrics.IncrementCounter("user_retrieval_failures");

        return error;
    });
```


## ElseAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, TResult&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ElseTaskAsyncExtensions.cs#L218" target="_blank">ElseTaskAsyncExtensions.cs</a>

Runs the synchronous function if the result that the `resultTask` returns is failed, and returns the result that the function returns instead (e.g., a recovered value or another error).

```csharp
public static Task<TResult> ElseAsync<TResult>(this Task<TResult> resultTask, Func<Error, TResult> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `TResult`
The type of the result: [Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md).

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TResult&gt;
The asynchronous operation that returns the result to inspect. Must not be `null`.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), TResult&gt;
The synchronous function that receives the error and returns the replacement result. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TResult&gt;
The result of the function, or the original successful result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `resultTask` or the `ifErrorFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`, or the `ifErrorFunction` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a successful result, which is returned unchanged.

### Examples
```csharp
var userResult = await _userService.GetUserAsync(id)
    .ElseAsync(error => Result.FromValue(User.Guest));
```

## See Also
* [IfErrorAsync](IfErrorTaskAsyncExtensions_IfErrorAsync.md)
* [IOperationResult](../IOperationResult/IOperationResult.md)
