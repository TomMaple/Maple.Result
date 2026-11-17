# ElseValueTaskAsyncExtensions.ElseAsync Methods
## Definition
Namespace: [Maple.Results.Extensions.ValueTasks](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a asynchronous function if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../../Error/Error.md)), and returns the result that the function returns instead: e.g., a recovered value or another error.

## Overloads
| Name | Description |
| ---- | ----------- |
| [ElseAsync&lt;TResult&gt;(TResult, Func&lt;Error, ValueTask&lt;TResult&gt;&gt;, Boolean)](#elseasynctresulttresult-funcerror-valuetasktresult-boolean) | Executes the provided asynchronous function, if the `TResult` instance represents a failed operation and returns its result, as an asynchronous operation. |
| [ElseAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&lt;TResult&gt;&gt;, Boolean)](#elseasynctresultvaluetasktresult-funcerror-valuetasktresult-boolean) | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |
| [ElseAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, TResult&gt;, Boolean)](#elseasynctresultvaluetasktresult-funcerror-tresult-boolean) | Runs the synchronous function if the result that the `resultTask` returns is failed, and returns the result that the function returns instead (e.g., a recovered value or another error). |
| [ElseAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;Error, ValueTask&lt;T&gt;&gt;, Boolean)](#elseasynctresultt-funcerror-valuetaskt-boolean) | Runs the asynchronous function if the `result` is failed, and returns a successful [Result&lt;T&gt;](../../ResultT/ResultT.md) with the value that the function returns; otherwise, returns the original result. |
| [ElseAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;Error, ValueTask&lt;Error&gt;&gt;, Boolean)](#elseasynctresultt-funcerror-valuetaskerror-boolean) | Runs the asynchronous function if the `result` is failed, and returns a failed [Result&lt;T&gt;](../../ResultT/ResultT.md) with the [Error](../../Error/Error.md) that the function returns; otherwise, returns the original result. |
| [ElseAsync(Result, Func&lt;Error, ValueTask&lt;Error&gt;&gt;, Boolean)](#elseasyncresult-funcerror-valuetaskerror-boolean) | Runs the asynchronous function if the `result` is failed, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) that the function returns; otherwise, returns the original result. |
| [ElseAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;Error, ValueTask&lt;T&gt;&gt;, Boolean)](#elseasynctvaluetaskresultt-funcerror-valuetaskt-boolean) | Runs the asynchronous function if the result that the `resultTask` returns is failed, and returns a successful [Result&lt;T&gt;](../../ResultT/ResultT.md) with the value that the function returns; otherwise, returns the original result. |
| [ElseAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;Error, ValueTask&lt;Error&gt;&gt;, Boolean)](#elseasynctvaluetaskresultt-funcerror-valuetaskerror-boolean) | Runs the asynchronous function if the result that the `resultTask` returns is failed, and returns a failed [Result&lt;T&gt;](../../ResultT/ResultT.md) with the [Error](../../Error/Error.md) that the function returns; otherwise, returns the original result. |
| [ElseAsync(ValueTask&lt;Result&gt;, Func&lt;Error, ValueTask&lt;Error&gt;&gt;, Boolean)](#elseasyncvaluetaskresult-funcerror-valuetaskerror-boolean) | Runs the asynchronous function if the result that the `resultTask` returns is failed, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) that the function returns; otherwise, returns the original result. |

## Remarks
The overload with a synchronous function (e.g., `ElseAsync(error => Result.FromValue(User.Guest))`) runs it after the task completes. A synchronous action does not compile: the compiler reports that it returns no value (e.g., CS0029); use `IfErrorAsync` for a side effect. A delegate that returns a `Task` is a compile error too: this page is for the `ValueTask` namespace; import `Maple.Results.Extensions` for the other one.

A function that returns the value or the [Error](../../Error/Error.md) of the replacement result is accepted too, as `Else` takes one through the implicit conversions: e.g., `ElseAsync(async error => 0)` gives a successful result with the value, and `ElseAsync(async error => error.WithMetadata("retried", true))` a failed one with that error. A function whose value task returns a value that does not convert to `T` is a compile error (e.g., a `ValueTask<string>` for a `Result<int>`). When `T` is [Error](../../Error/Error.md), a returned `Error` is the error of a failed result; return `Result<Error>.FromValue(…)` for a successful one.

A successful result is returned unchanged, and the function is not called. A function that returns a task without a value does not compile either: on a task of a result, it is a compile error (CS0619) that names [IfErrorAsync](IfErrorValueTaskAsyncExtensions_IfErrorAsync.md), which runs a side effect and returns the result unchanged; on a result, the compiler reports it (e.g., CS0029).

> [!NOTE]
> When an asynchronous method awaits a [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask) directly, continuation usually occurs in the same thread that created the operation, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, `ConfigureAwait(false)` is used by default, so in the methods that take a `ValueTask` of a result the passed delegates run on a thread-pool thread. When the delegates need the original context (e.g., to update UI elements), set the `continueOnCapturedContext` parameter to `true`, or set [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) to `true` once at startup.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.

> [!NOTE]
> A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask) can only be awaited once. Because of this, the overloads accepting `this ValueTask<TResult> resultTask` consume the passed operation as part of evaluating its result, and they do not perform a `null` check on it (a `ValueTask<TResult>` is a non-nullable value type).


## ElseAsync&lt;TResult&gt;(TResult, Func&lt;Error, ValueTask&lt;TResult&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ElseValueTaskAsyncExtensions.cs#L48" target="_blank">ElseValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static ValueTask<TResult> ElseAsync<TResult>(this TResult result, Func<Error, ValueTask<TResult>> ifErrorFunction, bool? continueOnCapturedContext = null)
        where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `result` that implements the [IOperationResult](../../IOperationResult/IOperationResult.md) interface—[Result](../../Result/Result.md) or [Result&lt;T&gt;](../../ResultT/ResultT.md) record.

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IOperationResult](../../IOperationResult/IOperationResult.md) interface—[Result](../../Result/Result.md) or [Result&lt;T&gt;](../../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TResult&gt;&gt;
The asynchronous function to be executed, only if the current instance of `TResult` represents a failed operation, as an asynchronous operation. The [Error](../../Error/Error.md) instance contained in the `TResult` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;TResult&gt;
The value task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifErrorFunction` function returns a value task with a `null` result.

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


## ElseAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&lt;TResult&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ElseValueTaskAsyncExtensions.cs#L111" target="_blank">ElseValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, if the provided asynchronous operation returns a failed`TResult` instance (i.e., contains an [Error](../../Error/Error.md)), and returns its result as an asynchronous operation.

```csharp
public static ValueTask<TResult> ElseAsync<TResult>(this ValueTask<TResult> resultTask,
    Func<Error, ValueTask<TResult>> ifErrorFunction, bool? continueOnCapturedContext = null)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the `resultTask` that implements the [IOperationResult](../../IOperationResult/IOperationResult.md) interface—[Result](../../Result/Result.md) or [Result&lt;T&gt;](../../ResultT/ResultT.md) record.

### Parameters
#### `resultTask` TResult
The asynchronous operation that represents an instance of `TResult` (that implements [IOperationResult](../../IOperationResult/IOperationResult.md) interface—[Result](../../Result/Result.md) or [Result&lt;T&gt;](../../ResultT/ResultT.md) record) to be executed and which result is to be evaluated.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;TResult&gt;&gt;
The asynchronous function to be executed, only if the current instance of `TResult` represents a failed operation, as an asynchronous operation. The [Error](../../Error/Error.md) instance contained in the outcome will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;TResult&gt;
The value task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifErrorFunction` function returns a value task with a `null` result.

### Remarks
The `ElseAsync` method awaits the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

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


## ElseAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, TResult&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ElseValueTaskAsyncExtensions.cs#L377" target="_blank">ElseValueTaskAsyncExtensions.cs</a>

Runs the synchronous function if the result that the `resultTask` returns is failed, and returns the result that the function returns instead (e.g., a recovered value or another error).

```csharp
public static ValueTask<TResult> ElseAsync<TResult>(this ValueTask<TResult> resultTask, Func<Error, TResult> ifErrorFunction, bool? continueOnCapturedContext = null)
    where TResult : IOperationResult;
```

### Type Parameters
#### `TResult`
The type of the result: [Result](../../Result/Result.md) or [Result&lt;T&gt;](../../ResultT/ResultT.md).

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TResult&gt;
The asynchronous operation that returns the result to inspect.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), TResult&gt;
The synchronous function that receives the error and returns the replacement result. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TResult&gt;
The result of the function, or the original successful result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `ifErrorFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`, or the `ifErrorFunction` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a successful result, which is returned unchanged.

### Examples
```csharp
var userResult = await _userService.GetUserAsync(id)
    .ElseAsync(error => Result.FromValue(User.Guest));
```

## ElseAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;Error, ValueTask&lt;T&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ElseValueTaskAsyncExtensions.cs#L150" target="_blank">ElseValueTaskAsyncExtensions.cs</a>

Runs the asynchronous function if the `result` is failed, and returns a successful [Result&lt;T&gt;](../../ResultT/ResultT.md) with the value that the function returns; otherwise, returns the original result.

```csharp
public static ValueTask<Result<T>> ElseAsync<T>(this Result<T> result, Func<Error, ValueTask<T>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The result to inspect. Must not be `null`.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;T&gt;&gt;
The asynchronous function that receives the [Error](../../Error/Error.md) and returns the value of the successful [Result&lt;T&gt;](../../ResultT/ResultT.md). Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt;
The original successful result, or a successful [Result&lt;T&gt;](../../ResultT/ResultT.md) with the value returned by the `ifErrorFunction`.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or the `ifErrorFunction` is `null`.

### Remarks
Use it to recover from an error with a value, as `Else(error => 0)` does. A `null` value is a valid value (for a nullable `T`).

### Examples
```csharp
Result<int> countResult = _counterService.GetCount(id);
var recovered = await countResult.ElseAsync(async error => await _cache.GetLastCountAsync(id));
```

## ElseAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;Error, ValueTask&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ElseValueTaskAsyncExtensions.cs#L178" target="_blank">ElseValueTaskAsyncExtensions.cs</a>

Runs the asynchronous function if the `result` is failed, and returns a failed [Result&lt;T&gt;](../../ResultT/ResultT.md) with the [Error](../../Error/Error.md) that the function returns; otherwise, returns the original result.

```csharp
public static ValueTask<Result<T>> ElseAsync<T>(this Result<T> result, Func<Error, ValueTask<Error>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The result to inspect. Must not be `null`.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function that receives the [Error](../../Error/Error.md) and returns the [Error](../../Error/Error.md) of the failed [Result&lt;T&gt;](../../ResultT/ResultT.md). Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt;
The original successful result, or a failed [Result&lt;T&gt;](../../ResultT/ResultT.md) with the [Error](../../Error/Error.md) returned by the `ifErrorFunction`.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or the `ifErrorFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifErrorFunction` returns a value task with a `null` result.

### Remarks
Use it to replace an error with another one, as `Else(error => error.WithMetadata(…))` does.

### Examples
```csharp
Result<User> userResult = _userService.GetUser(id);
var translated = await userResult.ElseAsync(async error => await _errorTranslator.TranslateAsync(error));
```

## ElseAsync(Result, Func&lt;Error, ValueTask&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ElseValueTaskAsyncExtensions.cs#L206" target="_blank">ElseValueTaskAsyncExtensions.cs</a>

Runs the asynchronous function if the `result` is failed, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) that the function returns; otherwise, returns the original result.

```csharp
public static ValueTask<Result> ElseAsync(this Result result, Func<Error, ValueTask<Error>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../../Result/Result.md)
The result to inspect. Must not be `null`.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function that receives the [Error](../../Error/Error.md) and returns the [Error](../../Error/Error.md) of the failed [Result](../../Result/Result.md). Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The original successful result, or a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by the `ifErrorFunction`.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or the `ifErrorFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifErrorFunction` returns a value task with a `null` result.

### Remarks
Use it to replace an error with another one, as `Else(error => error.WithMetadata(…))` does.

### Examples
```csharp
Result deleteResult = _userService.DeleteUser(id);
var translated = await deleteResult.ElseAsync(async error => await _errorTranslator.TranslateAsync(error));
```

## ElseAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;Error, ValueTask&lt;T&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ElseValueTaskAsyncExtensions.cs#L235" target="_blank">ElseValueTaskAsyncExtensions.cs</a>

Runs the asynchronous function if the result that the `resultTask` returns is failed, and returns a successful [Result&lt;T&gt;](../../ResultT/ResultT.md) with the value that the function returns; otherwise, returns the original result.

```csharp
public static ValueTask<Result<T>> ElseAsync<T>(this ValueTask<Result<T>> resultTask, Func<Error, ValueTask<T>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt;
The asynchronous operation that returns the result to inspect.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;T&gt;&gt;
The asynchronous function that receives the [Error](../../Error/Error.md) and returns the value of the successful [Result&lt;T&gt;](../../ResultT/ResultT.md). Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt;
The original successful result, or a successful [Result&lt;T&gt;](../../ResultT/ResultT.md) with the value returned by the `ifErrorFunction`.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifErrorFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
Use it to recover from an error with a value, as `Else(error => 0)` does. A `null` value is a valid value (for a nullable `T`).

### Examples
```csharp
var countResult = await _counterService.GetCountAsync(id)
    .ElseAsync(async error => await _cache.GetLastCountAsync(id));
```

## ElseAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;Error, ValueTask&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ElseValueTaskAsyncExtensions.cs#L268" target="_blank">ElseValueTaskAsyncExtensions.cs</a>

Runs the asynchronous function if the result that the `resultTask` returns is failed, and returns a failed [Result&lt;T&gt;](../../ResultT/ResultT.md) with the [Error](../../Error/Error.md) that the function returns; otherwise, returns the original result.

```csharp
public static ValueTask<Result<T>> ElseAsync<T>(this ValueTask<Result<T>> resultTask, Func<Error, ValueTask<Error>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt;
The asynchronous operation that returns the result to inspect.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function that receives the [Error](../../Error/Error.md) and returns the [Error](../../Error/Error.md) of the failed [Result&lt;T&gt;](../../ResultT/ResultT.md). Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt;
The original successful result, or a failed [Result&lt;T&gt;](../../ResultT/ResultT.md) with the [Error](../../Error/Error.md) returned by the `ifErrorFunction`.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifErrorFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifErrorFunction` returns a value task with a `null` result.

### Remarks
Use it to replace an error with another one, as `Else(error => error.WithMetadata(…))` does.

### Examples
```csharp
var userResult = await _userService.GetUserAsync(id)
    .ElseAsync(async error => await _errorTranslator.TranslateAsync(error));
```

## ElseAsync(ValueTask&lt;Result&gt;, Func&lt;Error, ValueTask&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ElseValueTaskAsyncExtensions.cs#L301" target="_blank">ElseValueTaskAsyncExtensions.cs</a>

Runs the asynchronous function if the result that the `resultTask` returns is failed, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) that the function returns; otherwise, returns the original result.

```csharp
public static ValueTask<Result> ElseAsync(this ValueTask<Result> resultTask, Func<Error, ValueTask<Error>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that returns the result to inspect.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function that receives the [Error](../../Error/Error.md) and returns the [Error](../../Error/Error.md) of the failed [Result](../../Result/Result.md). Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The original successful result, or a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by the `ifErrorFunction`.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifErrorFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifErrorFunction` returns a value task with a `null` result.

### Remarks
Use it to replace an error with another one, as `Else(error => error.WithMetadata(…))` does.

### Examples
```csharp
var deleteResult = await _userService.DeleteUserAsync(id)
    .ElseAsync(async error => await _errorTranslator.TranslateAsync(error));
```

## See Also
* [IfErrorAsync](IfErrorValueTaskAsyncExtensions_IfErrorAsync.md)
* [IOperationResult](../../IOperationResult/IOperationResult.md)
