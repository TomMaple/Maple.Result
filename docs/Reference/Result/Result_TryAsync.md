# Result.TryAsync Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs asynchronous code that can throw and turns the exception into a failed result, with the [Error](../Error/Error.md) that the `mapException` function creates.

## Overloads
| Name | Description |
| ---- | ----------- |
| [TryAsync(Func&lt;Task&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasyncfunctask-funcexception-error-nullableboolean) | Runs the asynchronous action; a [Result](Result.md). |
| [TryAsync&lt;TException&gt;(Func&lt;Task&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynctexceptionfunctask-functexception-error-nullableboolean) | Runs the asynchronous action and maps only the exceptions of the given type. |
| [TryAsync&lt;T&gt;(Func&lt;Task&lt;T&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynctfunctaskt-funcexception-error-nullableboolean) | Runs the asynchronous function; a [Result&lt;T&gt;](../ResultT/ResultT.md) with its value. |
| [TryAsync&lt;T, TException&gt;(Func&lt;Task&lt;T&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynct-texceptionfunctaskt-functexception-error-nullableboolean) | Runs the asynchronous function and maps only the exceptions of the given type. |
| [TryAsync(Func&lt;ValueTask&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasyncfuncvaluetask-funcexception-error-nullableboolean) | The same for an action that returns a `ValueTask`. |
| [TryAsync&lt;TException&gt;(Func&lt;ValueTask&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynctexceptionfuncvaluetask-functexception-error-nullableboolean) | The same for an action that returns a `ValueTask`, mapping only the given type. |
| [TryAsync&lt;T&gt;(Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynctfuncvaluetaskt-funcexception-error-nullableboolean) | The same for a function that returns a `ValueTask<T>`. |
| [TryAsync&lt;T, TException&gt;(Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynct-texceptionfuncvaluetaskt-functexception-error-nullableboolean) | The same for a function that returns a `ValueTask<T>`, mapping only the given type. |
| [TryAsync(Func&lt;Task&lt;Result&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasyncfunctaskresult-funcexception-error-nullableboolean) | The Task form for a function returning a [Result](Result.md). |
| [TryAsync&lt;TException&gt;(Func&lt;Task&lt;Result&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynctexceptionfunctaskresult-functexception-error-nullableboolean) | The Task form for a function returning a [Result](Result.md); maps only the exceptions of the given type. |
| [TryAsync&lt;T&gt;(Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynctfunctaskresultt-funcexception-error-nullableboolean) | The Task form for a function returning a [Result&lt;T&gt;](../ResultT/ResultT.md). |
| [TryAsync&lt;T, TException&gt;(Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynct-texceptionfunctaskresultt-functexception-error-nullableboolean) | The Task form for a function returning a [Result&lt;T&gt;](../ResultT/ResultT.md); maps only the exceptions of the given type. |
| [TryAsync(Func&lt;Task&lt;Error&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasyncfunctaskerror-funcexception-error-nullableboolean) | The Task form for a function returning an [Error](../Error/Error.md). |
| [TryAsync&lt;TException&gt;(Func&lt;Task&lt;Error&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynctexceptionfunctaskerror-functexception-error-nullableboolean) | The Task form for a function returning an [Error](../Error/Error.md); maps only the exceptions of the given type. |
| [TryAsync(Func&lt;ValueTask&lt;Result&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasyncfuncvaluetaskresult-funcexception-error-nullableboolean) | The ValueTask form for a function returning a [Result](Result.md). |
| [TryAsync&lt;TException&gt;(Func&lt;ValueTask&lt;Result&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynctexceptionfuncvaluetaskresult-functexception-error-nullableboolean) | The ValueTask form for a function returning a [Result](Result.md); maps only the exceptions of the given type. |
| [TryAsync&lt;T&gt;(Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynctfuncvaluetaskresultt-funcexception-error-nullableboolean) | The ValueTask form for a function returning a [Result&lt;T&gt;](../ResultT/ResultT.md). |
| [TryAsync&lt;T, TException&gt;(Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynct-texceptionfuncvaluetaskresultt-functexception-error-nullableboolean) | The ValueTask form for a function returning a [Result&lt;T&gt;](../ResultT/ResultT.md); maps only the exceptions of the given type. |
| [TryAsync(Func&lt;ValueTask&lt;Error&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasyncfuncvaluetaskerror-funcexception-error-nullableboolean) | The ValueTask form for a function returning an [Error](../Error/Error.md). |
| [TryAsync&lt;TException&gt;(Func&lt;ValueTask&lt;Error&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynctexceptionfuncvaluetaskerror-functexception-error-nullableboolean) | The ValueTask form for a function returning an [Error](../Error/Error.md); maps only the exceptions of the given type. |

## Remarks
The rules of [Try](Result_Try.md) apply: the mapper is required, nothing of the exception is copied into the error, an `OperationCanceledException` (e.g., a canceled task) is rethrown unless `TException` asks for it, and a mapper that returns `null` declines the exception, which is rethrown (see [Try](Result_Try.md) for the timeout of `HttpClient`). In addition:
* An exception is mapped whether the function throws before returning its task or the task faults.
* A function that returns a `Result`, a `Result<T>` or an `Error` (in a task) is flattened, as for [Try](Result_Try.md); a `null` result or error throws an `InvalidOperationException` and is not mapped.
* A `null` task is not mapped: it throws an `InvalidOperationException` (a bug of the caller, not a failure of the operation).
* The arguments are checked when the method is called, before the returned task starts.
* An async lambda binds to the `Task` overloads (they have a higher [overload resolution priority](https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.overloadresolutionpriorityattribute)); a method or a lambda that returns a `ValueTask` binds to the `ValueTask` overloads.

> [!WARNING]
> **Do not pass the exception type alone.** C# infers all type arguments or none, and a `Task<T>` is a `Task`, so `Result.TryAsync<IOException>(() => File.ReadAllTextAsync(path), e => …)` (or with `async () => await …`) binds to [TryAsync&lt;TException&gt;(Func&lt;Task&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)](#tryasynctexceptionfunctask-functexception-error-nullableboolean): the text is discarded and the result is a non-generic `Result`, without a warning. Type the parameter of the mapper instead, `Result.TryAsync(() => File.ReadAllTextAsync(path), (IOException e) => …)`, or pass both type arguments, `Result.TryAsync<string, IOException>(…)`; both return a `Result<string>`. A function that returns a `ValueTask<T>` does not compile with the exception type alone.

## TryAsync(Func&lt;Task&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L225" target="_blank">Result.Try.cs</a>

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result> TryAsync(Func<Task> action, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null);
```

### Parameters
`action` Func&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;

The asynchronous action to run. Must not be `null`, nor return a `null` task.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
Task&lt;[Result](Result.md)&gt;

A successful result if the `action` completes; otherwise, a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`action` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`action` returns a `null` task.

### Examples
```csharp
var result = await Result.TryAsync(
    async () => await _mailer.SendAsync(message, cancellationToken),
    ex => Error.Unavailable(ErrorUri.Tag("tag:exampleapp.com,2026:errors:mail:unavailable"), "The e-mail could not be sent."));
```

## TryAsync&lt;TException&gt;(Func&lt;Task&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L263" target="_blank">Result.Try.cs</a>

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result> TryAsync<TException>(Func<Task> action, Func<TException, Error?> mapException, bool? continueOnCapturedContext = null)
    where TException : Exception;
```

### Type Parameters
#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`action` Func&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;

The asynchronous action to run. Must not be `null`, nor return a `null` task.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
Task&lt;[Result](Result.md)&gt;

A successful result if the `action` completes; otherwise, a failed one with the mapped error. Exceptions of other types propagate.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`action` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`action` returns a `null` task.

## TryAsync&lt;T&gt;(Func&lt;Task&lt;T&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L340" target="_blank">Result.Try.cs</a>

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result<T>> TryAsync<T>(Func<Task<T>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value.

### Parameters
`function` Func&lt;Task&lt;T&gt;&gt;

The asynchronous function to run. Must not be `null`, nor return a `null` task; the value may be `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
Task&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;

A successful result with the value of the `function`; otherwise, a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` task.

### Examples
```csharp
Result<Customer> customer = await Result.TryAsync(
    () => _repository.LoadAsync(id, cancellationToken),
    MapDatabaseError);
```

## TryAsync&lt;T, TException&gt;(Func&lt;Task&lt;T&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L376" target="_blank">Result.Try.cs</a>

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result<T>> TryAsync<T, TException>(Func<Task<T>> function, Func<TException, Error?> mapException, bool? continueOnCapturedContext = null)
    where TException : Exception;
```

### Type Parameters
#### `T`
The type of the value.

#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`function` Func&lt;Task&lt;T&gt;&gt;

The asynchronous function to run. Must not be `null`, nor return a `null` task; the value may be `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
Task&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;

A successful result with the value of the `function`; otherwise, a failed one with the mapped error. Exceptions of other types propagate.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` task.

## TryAsync(Func&lt;ValueTask&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L453" target="_blank">Result.Try.cs</a>

```csharp
public static ValueTask<Result> TryAsync(Func<ValueTask> action, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null);
```

### Parameters
`action` Func&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;

The asynchronous action to run. Must not be `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
ValueTask&lt;[Result](Result.md)&gt;

A successful result if the `action` completes; otherwise, a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`action` or `mapException` is `null`.

## TryAsync&lt;TException&gt;(Func&lt;ValueTask&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L483" target="_blank">Result.Try.cs</a>

```csharp
public static ValueTask<Result> TryAsync<TException>(Func<ValueTask> action, Func<TException, Error?> mapException, bool? continueOnCapturedContext = null)
    where TException : Exception;
```

### Type Parameters
#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`action` Func&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;

The asynchronous action to run. Must not be `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
ValueTask&lt;[Result](Result.md)&gt;

A successful result if the `action` completes; otherwise, a failed one with the mapped error. Exceptions of other types propagate.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`action` or `mapException` is `null`.

## TryAsync&lt;T&gt;(Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L544" target="_blank">Result.Try.cs</a>

```csharp
public static ValueTask<Result<T>> TryAsync<T>(Func<ValueTask<T>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value.

### Parameters
`function` Func&lt;ValueTask&lt;T&gt;&gt;

The asynchronous function to run. Must not be `null`; the value may be `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
ValueTask&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;

A successful result with the value of the `function`; otherwise, a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

## TryAsync&lt;T, TException&gt;(Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L577" target="_blank">Result.Try.cs</a>

```csharp
public static ValueTask<Result<T>> TryAsync<T, TException>(Func<ValueTask<T>> function, Func<TException, Error?> mapException, bool? continueOnCapturedContext = null)
    where TException : Exception;
```

### Type Parameters
#### `T`
The type of the value.

#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`function` Func&lt;ValueTask&lt;T&gt;&gt;

The asynchronous function to run. Must not be `null`; the value may be `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
ValueTask&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;

A successful result with the value of the `function`; otherwise, a failed one with the mapped error. Exceptions of other types propagate.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

## TryAsync(Func&lt;Task&lt;Result&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L896" target="_blank">Result.Try.cs</a>

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result> TryAsync(Func<Task<Result>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null);
```

### Parameters
`function` Func&lt;Task&lt;Result&gt;&gt;

The asynchronous function to run. Must not be `null`, nor return a `null` task; its result must not be `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context; `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
Task&lt;[Result](Result.md)&gt;

A task with the [Result](Result.md) returned by the `function`, as it is; or a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` task or a `null` result.

## TryAsync&lt;TException&gt;(Func&lt;Task&lt;Result&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L936" target="_blank">Result.Try.cs</a>

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result> TryAsync<TException>(Func<Task<Result>> function, Func<TException, Error?> mapException, bool? continueOnCapturedContext = null)
    where TException : Exception;
```

### Type Parameters
#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`function` Func&lt;Task&lt;Result&gt;&gt;

The asynchronous function to run. Must not be `null`, nor return a `null` task; its result must not be `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context; `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
Task&lt;[Result](Result.md)&gt;

A task with the [Result](Result.md) returned by the `function`, as it is; or a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` task or a `null` result.

## TryAsync&lt;T&gt;(Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L1019" target="_blank">Result.Try.cs</a>

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result<T>> TryAsync<T>(Func<Task<Result<T>>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value.

### Parameters
`function` Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;

The asynchronous function to run. Must not be `null`, nor return a `null` task; its result must not be `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context; `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
Task&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;

A task with the [Result&lt;T&gt;](../ResultT/ResultT.md) returned by the `function`, as it is; or a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` task or a `null` result.

## TryAsync&lt;T, TException&gt;(Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L1060" target="_blank">Result.Try.cs</a>

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result<T>> TryAsync<T, TException>(Func<Task<Result<T>>> function, Func<TException, Error?> mapException, bool? continueOnCapturedContext = null)
    where TException : Exception;
```

### Type Parameters
#### `T`
The type of the value.

#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`function` Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;

The asynchronous function to run. Must not be `null`, nor return a `null` task; its result must not be `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context; `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
Task&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;

A task with the [Result&lt;T&gt;](../ResultT/ResultT.md) returned by the `function`, as it is; or a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` task or a `null` result.

## TryAsync(Func&lt;Task&lt;Error&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L1142" target="_blank">Result.Try.cs</a>

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result> TryAsync(Func<Task<Error>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null);
```

### Parameters
`function` Func&lt;Task&lt;Error&gt;&gt;

The asynchronous function to run. Must not be `null`, nor return a `null` task; its result must not be `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context; `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
Task&lt;[Result](Result.md)&gt;

A task with a failed [Result](Result.md) with the [Error](../Error/Error.md) returned by the `function`, or with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` task or a `null` error.

## TryAsync&lt;TException&gt;(Func&lt;Task&lt;Error&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L1182" target="_blank">Result.Try.cs</a>

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result> TryAsync<TException>(Func<Task<Error>> function, Func<TException, Error?> mapException, bool? continueOnCapturedContext = null)
    where TException : Exception;
```

### Type Parameters
#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`function` Func&lt;Task&lt;Error&gt;&gt;

The asynchronous function to run. Must not be `null`, nor return a `null` task; its result must not be `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context; `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
Task&lt;[Result](Result.md)&gt;

A task with a failed [Result](Result.md) with the [Error](../Error/Error.md) returned by the `function`, or with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` task or a `null` error.

## TryAsync(Func&lt;ValueTask&lt;Result&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L1270" target="_blank">Result.Try.cs</a>

```csharp
public static ValueTask<Result> TryAsync(Func<ValueTask<Result>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null);
```

### Parameters
`function` Func&lt;ValueTask&lt;Result&gt;&gt;

The asynchronous function to run. Must not be `null`; its result must not be `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context; `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
ValueTask&lt;[Result](Result.md)&gt;

A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1) that represents the [Result](Result.md) returned by the `function`, as it is; or a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` result.

## TryAsync&lt;TException&gt;(Func&lt;ValueTask&lt;Result&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L1310" target="_blank">Result.Try.cs</a>

```csharp
public static ValueTask<Result> TryAsync<TException>(Func<ValueTask<Result>> function, Func<TException, Error?> mapException, bool? continueOnCapturedContext = null)
    where TException : Exception;
```

### Type Parameters
#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`function` Func&lt;ValueTask&lt;Result&gt;&gt;

The asynchronous function to run. Must not be `null`; its result must not be `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context; `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
ValueTask&lt;[Result](Result.md)&gt;

A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1) that represents the [Result](Result.md) returned by the `function`, as it is; or a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` result.

## TryAsync&lt;T&gt;(Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L1379" target="_blank">Result.Try.cs</a>

```csharp
public static ValueTask<Result<T>> TryAsync<T>(Func<ValueTask<Result<T>>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value.

### Parameters
`function` Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;

The asynchronous function to run. Must not be `null`; its result must not be `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context; `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
ValueTask&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;

A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1) that represents the [Result&lt;T&gt;](../ResultT/ResultT.md) returned by the `function`, as it is; or a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` result.

## TryAsync&lt;T, TException&gt;(Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L1420" target="_blank">Result.Try.cs</a>

```csharp
public static ValueTask<Result<T>> TryAsync<T, TException>(Func<ValueTask<Result<T>>> function, Func<TException, Error?> mapException, bool? continueOnCapturedContext = null)
    where TException : Exception;
```

### Type Parameters
#### `T`
The type of the value.

#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`function` Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;

The asynchronous function to run. Must not be `null`; its result must not be `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context; `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
ValueTask&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;

A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1) that represents the [Result&lt;T&gt;](../ResultT/ResultT.md) returned by the `function`, as it is; or a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` result.

## TryAsync(Func&lt;ValueTask&lt;Error&gt;&gt;, Func&lt;Exception, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L1488" target="_blank">Result.Try.cs</a>

```csharp
public static ValueTask<Result> TryAsync(Func<ValueTask<Error>> function, Func<Exception, Error?> mapException, bool? continueOnCapturedContext = null);
```

### Parameters
`function` Func&lt;ValueTask&lt;Error&gt;&gt;

The asynchronous function to run. Must not be `null`; its result must not be `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context; `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
ValueTask&lt;[Result](Result.md)&gt;

A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1) that represents a failed [Result](Result.md) with the [Error](../Error/Error.md) returned by the `function`, or with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` error.

## TryAsync&lt;TException&gt;(Func&lt;ValueTask&lt;Error&gt;&gt;, Func&lt;TException, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.Try.cs#L1528" target="_blank">Result.Try.cs</a>

```csharp
public static ValueTask<Result> TryAsync<TException>(Func<ValueTask<Error>> function, Func<TException, Error?> mapException, bool? continueOnCapturedContext = null)
    where TException : Exception;
```

### Type Parameters
#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`function` Func&lt;ValueTask&lt;Error&gt;&gt;

The asynchronous function to run. Must not be `null`; its result must not be `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context; `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
ValueTask&lt;[Result](Result.md)&gt;

A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1) that represents a failed [Result](Result.md) with the [Error](../Error/Error.md) returned by the `function`, or with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` error.

## See Also
* [Try](Result_Try.md)
* [Result](Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
