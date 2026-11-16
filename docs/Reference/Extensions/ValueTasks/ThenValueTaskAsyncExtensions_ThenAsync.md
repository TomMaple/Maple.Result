# ThenValueTaskAsyncExtensions.ThenAsync Methods
## Definition
Namespace: [Maple.Results.Extensions.ValueTasks](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a asynchronous function if the instance of [Result](../../Result/Result.md) or [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), and returns the next result: the value, the [Result](../../Result/Result.md) or the [Result&lt;T&gt;](../../ResultT/ResultT.md) that the function returns, or a failed result for an [Error](../../Error/Error.md) it returns.

## Overloads
| Name | Description |
| ---- | ----------- |
| [ThenAsync(Result, Func&lt;ValueTask&lt;Result&gt;&gt;, Boolean)](#thenasyncresult-funcvaluetaskresult-boolean) | Executes the specified asynchronous function, if the [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync(Result, Func&lt;ValueTask&lt;Error&gt;&gt;, Boolean)](#thenasyncresult-funcvaluetaskerror-boolean) | Executes the specified asynchronous function, if the [Result](../../Result/Result.md) represents a successful operation, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) it returns. |
| [ThenAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;T&gt;&gt;, Boolean)](#thenasynctresult-funcvaluetaskt-boolean) | Executes the specified asynchronous function, if the [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](#thenasynctresult-funcvaluetaskresultt-boolean) | Executes the specified asynchronous function, if the [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Boolean)](#thenasynctresultt-funct-valuetaskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Boolean)](#thenasynctresultt-funct-valuetaskerror-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) it returns. |
| [ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Boolean)](#thenasynct-tnextresultt-funct-valuetasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](#thenasynct-tnextresultt-funct-valuetaskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&gt;&gt;, Boolean)](#thenasyncvaluetaskresult-funcvaluetaskresult-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../../Result/Result.md), as an asynchronous operation. |
| [ThenAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Error&gt;&gt;, Boolean)](#thenasyncvaluetaskresult-funcvaluetaskerror-boolean) | Executes the specified asynchronous function, if the asynchronous operation `resultTask` represents a successful [Result](../../Result/Result.md), and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) it returns. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;T&gt;&gt;, Boolean)](#thenasynctvaluetaskresult-funcvaluetaskt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](#thenasynctvaluetaskresult-funcvaluetaskresultt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Boolean)](#thenasynctvaluetaskresultt-funct-valuetaskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Boolean)](#thenasynctvaluetaskresultt-funct-valuetaskerror-boolean) | Executes the specified asynchronous function, if the asynchronous operation `resultTask` represents a successful [Result&lt;T&gt;](../../ResultT/ResultT.md), and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) it returns. |
| [ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Boolean)](#thenasynct-tnextvaluetaskresultt-funct-valuetasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](#thenasynct-tnextvaluetaskresultt-funct-valuetaskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |

## Remarks
A returned `Result` or `Result<T>` is flattened, and a returned [Error](../../Error/Error.md) makes the result fail. Explicit type arguments keep a nested result on purpose. An action (a function that returns a task without a value) is a compile error (CS0619) that names [IfSuccessAsync](IfSuccessValueTaskAsyncExtensions_IfSuccessAsync.md), which runs a side effect and returns the result unchanged.

> [!NOTE]
> When an asynchronous method awaits a [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask) directly, continuation usually occurs in the same thread that created the task, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, `ConfigureAwait(false)` is used by default, so in the methods that take a `ValueTask` of a result the passed delegates run on a thread-pool thread. When the delegates need the original context (e.g., to update UI elements), set the `continueOnCapturedContext` parameter to `true`, or set [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) to `true` once at startup.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.

> [!NOTE]
> A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask) can only be awaited once. Because of this, the overloads accepting `this ValueTask<TResult> resultTask` consume the passed operation as part of evaluating its result, and they do not perform a `null` check on it (a `ValueTask<TResult>` is a non-nullable value type).


## ThenAsync(Result, Func&lt;ValueTask&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L67" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<Result> ThenAsync(this Result result, Func<ValueTask<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a value task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.ThenAsync(async () =>
{
    return await _companyService.AddUserAsync(userData);
});
```


## ThenAsync(Result, Func&lt;ValueTask&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L120" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function only if the [Result](../../Result/Result.md) represents a successful operation, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by the function.

```csharp
public static async ValueTask<Result> ThenAsync(this Result result, Func<ValueTask<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `result` represents a successful operation. It returns the [Error](../../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../../Result/Result.md) with the original [Error](../../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<Error>(…)`.

### Examples
```csharp
var result = await _userService.DeleteUser(userId)
    .ThenAsync(async () => await _policyService.GetDeletionErrorAsync(userId));
```


## ThenAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;T&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L152" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<Result<T>> ThenAsync<T>(this Result result, Func<ValueTask<T>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async () =>
{
    return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
});
```


## ThenAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L177" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<Result<T>> ThenAsync<T>(this Result result, Func<ValueTask<Result<T>>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a value task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async () =>
{
    return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
});
```


## ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L233" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<Result> ThenAsync<T>(this Result<T> result, Func<T, ValueTask<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a value task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.ThenAsync(async (user) =>
{
    return await _companyService.AddUserAsync(user);
});
```


## ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L290" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function only if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by the function.

```csharp
public static async ValueTask<Result> ThenAsync<T>(this Result<T> result,
    Func<T, ValueTask<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the `result`, passed to the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `result` represents a successful operation. The value of the `result` is passed as a parameter. It returns the [Error](../../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../../Result/Result.md) with the original [Error](../../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<T, Error>(…)`.

### Examples
```csharp
var result = await _userService.GetUser(userId)
    .ThenAsync(async user => await _policyService.GetDeletionErrorAsync(user));
```


## ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L318" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<Result<TNext>> ThenAsync<T, TNext>(this Result<T> result,
    Func<T, ValueTask<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;TNext&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async (user) =>
{
    return await _companyService.GetCompanyByIdAsync(user.CompanyId);
});
```


## ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L371" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<Result<TNext>> ThenAsync<T, TNext>(this Result<T> result,
    Func<T, ValueTask<Result<TNext>>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a value task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async (user) =>
{
    return await _companyService.GetCompanyByIdAsync(user.CompanyId);
});
```


## ThenAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L429" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../../Result/Result.md).

```csharp
public static async ValueTask<Result> ThenAsync(this ValueTask<Result> resultTask, Func<ValueTask<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async () =>
    {
        return await _companyService.AddUserAsync(userData);
    });
```


## ThenAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L487" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function only if the asynchronous operation `resultTask` represents a successful [Result](../../Result/Result.md), and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by the function.

```csharp
public static async ValueTask<Result> ThenAsync(this ValueTask<Result> resultTask,
    Func<ValueTask<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. It returns the [Error](../../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../../Result/Result.md) with the original [Error](../../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<Error>(…)`.

### Examples
```csharp
var result = await _userService.DeleteUserAsync(userId)
    .ThenAsync(async () => await _policyService.GetDeletionErrorAsync(userId));
```


## ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;T&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L546" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../../Result/Result.md).

```csharp
public static async ValueTask<Result<T>> ThenAsync<T>(this ValueTask<Result> resultTask, Func<ValueTask<T>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async () =>
    {
        return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
    });
```


## ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L602" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../../Result/Result.md).

```csharp
public static async ValueTask<Result<T>> ThenAsync<T>(
    this ValueTask<Result> resultTask,
    Func<ValueTask<Result<T>>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async () =>
    {
        return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
    });
```


## ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L667" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../../ResultT/ResultT.md).

```csharp
public static async ValueTask<Result> ThenAsync<T>(this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<Result>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async (user) =>
    {
        return await _companyService.AddUserAsync(user);
    });
```


## ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L729" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function only if the asynchronous operation `resultTask` represents a successful [Result&lt;T&gt;](../../ResultT/ResultT.md), and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by the function.

```csharp
public static async ValueTask<Result> ThenAsync<T>(this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result of `resultTask`, passed to the `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. The value of the result is passed as a parameter. It returns the [Error](../../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../../Result/Result.md) with the original [Error](../../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<T, Error>(…)`.

### Examples
```csharp
var result = await _userService.GetUserAsync(userId)
    .ThenAsync(async user => await _policyService.GetDeletionErrorAsync(user));
```


## ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L795" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../../ResultT/ResultT.md).

```csharp
public static async ValueTask<Result<TNext>> ThenAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;TNext&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async (user) =>
    {
        return await _companyService.GetCompanyByIdAsync(user.CompanyId);
    });
```


## ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/ThenValueTaskAsyncExtensions.cs#L856" target="_blank">ThenValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../../ResultT/ResultT.md).

```csharp
public static async ValueTask<Result<TNext>> ThenAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<Result<TNext>>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async (user) =>
    {
        return await _companyService.GetCompanyByIdAsync(user.CompanyId);
    });
```

## See Also
* [IfSuccessAsync](IfSuccessValueTaskAsyncExtensions_IfSuccessAsync.md)
* [Result](../../Result/Result.md)
* [Result&lt;T&gt;](../../ResultT/ResultT.md)
