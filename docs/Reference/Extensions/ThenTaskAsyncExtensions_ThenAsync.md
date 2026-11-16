# ThenTaskAsyncExtensions.ThenAsync Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a asynchronous function if the instance of [Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), and returns the next result: the value, the [Result](../Result/Result.md) or the [Result&lt;T&gt;](../ResultT/ResultT.md) that the function returns, or a failed result for an [Error](../Error/Error.md) it returns.

## Overloads
| Name | Description |
| ---- | ----------- |
| [ThenAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Boolean)](#thenasyncresult-functaskresult-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync(Result, Func&lt;Task&lt;Error&gt;&gt;, Boolean)](#thenasyncresult-functaskerror-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Boolean)](#thenasynctresult-functaskt-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](#thenasynctresult-functaskresultt-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Boolean)](#thenasynctresultt-funct-taskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Error&gt;&gt;, Boolean)](#thenasynctresultt-funct-taskerror-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Boolean)](#thenasynct-tnextresultt-funct-tasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](#thenasynct-tnextresultt-funct-taskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync(Task&lt;Result&gt;, Func&lt;Task&lt;Result&gt;&gt;, Boolean)](#thenasynctaskresult-functaskresult-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync(Task&lt;Result&gt;, Func&lt;Task&lt;Error&gt;&gt;, Boolean)](#thenasynctaskresult-functaskerror-boolean) | Executes the specified asynchronous function, if the asynchronous operation `resultTask` represents a successful [Result](../Result/Result.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;T&gt;&gt;, Boolean)](#thenasyncttaskresult-functaskt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](#thenasyncttaskresult-functaskresultt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Boolean)](#thenasyncttaskresultt-funct-taskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Error&gt;&gt;, Boolean)](#thenasyncttaskresultt-funct-taskerror-boolean) | Executes the specified asynchronous function, if the asynchronous operation `resultTask` represents a successful [Result&lt;T&gt;](../ResultT/ResultT.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Boolean)](#thenasynct-tnexttaskresultt-funct-tasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](#thenasynct-tnexttaskresultt-funct-taskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |

## Remarks
A returned `Result` or `Result<T>` is flattened, and a returned [Error](../Error/Error.md) makes the result fail. Explicit type arguments keep a nested result on purpose. An action (a function that returns a task without a value) is a compile error (CS0619) that names [IfSuccessAsync](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md), which runs a side effect and returns the result unchanged.

> [!NOTE]
> When an asynchronous method awaits a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task) directly, continuation usually occurs in the same thread that created the task, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, `ConfigureAwait(false)` is used by default, so in the methods that take a `Task` of a result the passed delegates run on a thread-pool thread. When the delegates need the original context (e.g., to update UI elements), set the `continueOnCapturedContext` parameter to `true`, or set [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) to `true` once at startup.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.


## ThenAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L60" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<Result> ThenAsync(this Result result, Func<Task<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.ThenAsync(async () =>
{
    return await _companyService.AddUserAsync(userData);
});
```


## ThenAsync(Result, Func&lt;Task&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L113" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function only if the [Result](../Result/Result.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
public static async Task<Result> ThenAsync(this Result result, Func<Task<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `result` represents a successful operation. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<Error>(…)`.

### Examples
```csharp
var result = await _userService.DeleteUser(userId)
    .ThenAsync(async () => await _policyService.GetDeletionErrorAsync(userId));
```


## ThenAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L163" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<Result<T>> ThenAsync<T>(this Result result, Func<Task<T>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async () =>
{
    return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
});
```


## ThenAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L213" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<Result<T>> ThenAsync<T>(this Result result, Func<Task<Result<T>>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async () =>
{
    return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
});
```


## ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L268" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<Result> ThenAsync<T>(this Result<T> result, Func<T, Task<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.ThenAsync(async (user) =>
{
    return await _companyService.AddUserAsync(user);
});
```


## ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L324" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function only if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
public static async Task<Result> ThenAsync<T>(this Result<T> result,
    Func<T, Task<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the `result`, passed to the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `result` represents a successful operation. The value of the `result` is passed as a parameter. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<T, Error>(…)`.

### Examples
```csharp
var result = await _userService.GetUser(userId)
    .ThenAsync(async user => await _policyService.GetDeletionErrorAsync(user));
```


## ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L379" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<Result<TNext>> ThenAsync<T, TNext>(this Result<T> result,
    Func<T, Task<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;TNext&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async (user) =>
{
    return await _companyService.GetCompanyByIdAsync(user.CompanyId);
});
```


## ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L437" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<Result<TNext>> ThenAsync<T, TNext>(this Result<T> result,
    Func<T, Task<Result<TNext>>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async (user) =>
{
    return await _companyService.GetCompanyByIdAsync(user.CompanyId);
});
```


## ThenAsync(Task&lt;Result&gt;, Func&lt;Task&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L497" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
public static async Task<Result> ThenAsync(this Task<Result> resultTask, Func<Task<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async () =>
    {
        return await _companyService.AddUserAsync(userData);
    });
```


## ThenAsync(Task&lt;Result&gt;, Func&lt;Task&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L555" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function only if the asynchronous operation `resultTask` represents a successful [Result](../Result/Result.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
public static async Task<Result> ThenAsync(this Task<Result> resultTask,
    Func<Task<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<Error>(…)`.

### Examples
```csharp
var result = await _userService.DeleteUserAsync(userId)
    .ThenAsync(async () => await _policyService.GetDeletionErrorAsync(userId));
```


## ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;T&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L617" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
public static async Task<Result<T>> ThenAsync<T>(this Task<Result> resultTask, Func<Task<T>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async () =>
    {
        return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
    });
```


## ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L677" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
public static async Task<Result<T>> ThenAsync<T>(
    this Task<Result> resultTask,
    Func<Task<Result<T>>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async () =>
    {
        return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
    });
```


## ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L744" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
public static async Task<Result> ThenAsync<T>(this Task<Result<T>> resultTask,
    Func<T, Task<Result>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async (user) =>
    {
        return await _companyService.AddUserAsync(user);
    });
```


## ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L807" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function only if the asynchronous operation `resultTask` represents a successful [Result&lt;T&gt;](../ResultT/ResultT.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
public static async Task<Result> ThenAsync<T>(this Task<Result<T>> resultTask,
    Func<T, Task<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result of `resultTask`, passed to the `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. The value of the result is passed as a parameter. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<T, Error>(…)`.

### Examples
```csharp
var result = await _userService.GetUserAsync(userId)
    .ThenAsync(async user => await _policyService.GetDeletionErrorAsync(user));
```


## ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L876" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
public static async Task<Result<TNext>> ThenAsync<T, TNext>(this Task<Result<T>> resultTask,
    Func<T, Task<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;TNext&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async (user) =>
    {
        return await _companyService.GetCompanyByIdAsync(user.CompanyId);
    });
```


## ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ThenTaskAsyncExtensions.cs#L945" target="_blank">ThenTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
public static async Task<Result<TNext>> ThenAsync<T, TNext>(this Task<Result<T>> resultTask,
    Func<T, Task<Result<TNext>>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async (user) =>
    {
        return await _companyService.GetCompanyByIdAsync(user.CompanyId);
    });
```

## See Also
* [IfSuccessAsync](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
