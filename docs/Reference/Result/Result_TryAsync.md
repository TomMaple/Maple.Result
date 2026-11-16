# Result.TryAsync Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs asynchronous code that can throw and turns the exception into a failed result, with the [Error](../Error/Error.md) that the `mapException` function creates.

## Overloads
| Name | Description |
| ---- | ----------- |
| [TryAsync(Func&lt;Task&gt;, Func&lt;Exception, Error&gt;, Boolean?)](#tryasyncfunctask-funcexception-error-boolean) | Runs the asynchronous action; a [Result](Result.md). |
| [TryAsync&lt;TException&gt;(Func&lt;Task&gt;, Func&lt;TException, Error&gt;, Boolean?)](#tryasynctexceptionfunctask-functexception-error-boolean) | Runs the asynchronous action and maps only the exceptions of the given type. |
| [TryAsync&lt;T&gt;(Func&lt;Task&lt;T&gt;&gt;, Func&lt;Exception, Error&gt;, Boolean?)](#tryasynctfunctaskt-funcexception-error-boolean) | Runs the asynchronous function; a [Result&lt;T&gt;](../ResultT/ResultT.md) with its value. |
| [TryAsync&lt;T, TException&gt;(Func&lt;Task&lt;T&gt;&gt;, Func&lt;TException, Error&gt;, Boolean?)](#tryasynct-texceptionfunctaskt-functexception-error-boolean) | Runs the asynchronous function and maps only the exceptions of the given type. |
| [TryAsync(Func&lt;ValueTask&gt;, Func&lt;Exception, Error&gt;, Boolean?)](#tryasyncfuncvaluetask-funcexception-error-boolean) | The same for an action that returns a `ValueTask`. |
| [TryAsync&lt;TException&gt;(Func&lt;ValueTask&gt;, Func&lt;TException, Error&gt;, Boolean?)](#tryasynctexceptionfuncvaluetask-functexception-error-boolean) | The same for an action that returns a `ValueTask`, mapping only the given type. |
| [TryAsync&lt;T&gt;(Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;Exception, Error&gt;, Boolean?)](#tryasynctfuncvaluetaskt-funcexception-error-boolean) | The same for a function that returns a `ValueTask<T>`. |
| [TryAsync&lt;T, TException&gt;(Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;TException, Error&gt;, Boolean?)](#tryasynct-texceptionfuncvaluetaskt-functexception-error-boolean) | The same for a function that returns a `ValueTask<T>`, mapping only the given type. |

## Remarks
The rules of [Try](Result_Try.md) apply: the mapper is required, nothing of the exception is copied into the error, an `OperationCanceledException` (e.g., a canceled task) is rethrown unless `TException` asks for it, and a mapper that returns `null` throws an `InvalidOperationException`. In addition:
* An exception is mapped whether the function throws before returning its task or the task faults.
* A `null` task is not mapped: it throws an `InvalidOperationException` (a bug of the caller, not a failure of the operation).
* The arguments are checked when the method is called, before the returned task starts.
* An async lambda binds to the `Task` overloads (they have a higher [overload resolution priority](https://learn.microsoft.com/dotnet/api/system.runtime.compilerservices.overloadresolutionpriorityattribute)); a method or a lambda that returns a `ValueTask` binds to the `ValueTask` overloads.

## TryAsync(Func&lt;Task&gt;, Func&lt;Exception, Error&gt;, Boolean?)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L218" target="_blank">Result.Try.cs</a>

```csharp
public static Task<Result> TryAsync(Func<Task> action, Func<Exception, Error> mapException, bool? continueOnCapturedContext = null);
```

### Parameters
`action` Func&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;

The asynchronous action to run. Must not be `null`, nor return a `null` task.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)&gt;

The function that creates the error from the exception. Must not be `null`, nor return `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
Task&lt;[Result](Result.md)&gt;

A successful result if the `action` completes; otherwise, a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`action` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`action` returns a `null` task, or `mapException` returns `null`.

### Examples
```csharp
var result = await Result.TryAsync(
    async () => await _mailer.SendAsync(message, cancellationToken),
    ex => Error.Unavailable(ErrorUri.Tag("tag:exampleapp.com,2026:errors:mail:unavailable"), "The e-mail could not be sent."));
```

## TryAsync&lt;TException&gt;(Func&lt;Task&gt;, Func&lt;TException, Error&gt;, Boolean?)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L250" target="_blank">Result.Try.cs</a>

```csharp
public static Task<Result> TryAsync<TException>(Func<Task> action, Func<TException, Error> mapException, bool? continueOnCapturedContext = null)
    where TException : Exception;
```

### Type Parameters
#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`action` Func&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;

The asynchronous action to run. Must not be `null`, nor return a `null` task.

`mapException` Func&lt;TException, [Error](../Error/Error.md)&gt;

The function that creates the error from the exception. Must not be `null`, nor return `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
Task&lt;[Result](Result.md)&gt;

A successful result if the `action` completes; otherwise, a failed one with the mapped error. Exceptions of other types propagate.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`action` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`action` returns a `null` task, or `mapException` returns `null`.

## TryAsync&lt;T&gt;(Func&lt;Task&lt;T&gt;&gt;, Func&lt;Exception, Error&gt;, Boolean?)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L324" target="_blank">Result.Try.cs</a>

```csharp
public static Task<Result<T>> TryAsync<T>(Func<Task<T>> function, Func<Exception, Error> mapException, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value.

### Parameters
`function` Func&lt;Task&lt;T&gt;&gt;

The asynchronous function to run. Must not be `null`, nor return a `null` task; the value may be `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)&gt;

The function that creates the error from the exception. Must not be `null`, nor return `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
Task&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;

A successful result with the value of the `function`; otherwise, a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` task, or `mapException` returns `null`.

### Examples
```csharp
Result<Customer> customer = await Result.TryAsync(
    () => _repository.LoadAsync(id, cancellationToken),
    MapDatabaseError);
```

## TryAsync&lt;T, TException&gt;(Func&lt;Task&lt;T&gt;&gt;, Func&lt;TException, Error&gt;, Boolean?)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L363" target="_blank">Result.Try.cs</a>

```csharp
public static Task<Result<T>> TryAsync<T, TException>(Func<Task<T>> function, Func<TException, Error> mapException, bool? continueOnCapturedContext = null)
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

`mapException` Func&lt;TException, [Error](../Error/Error.md)&gt;

The function that creates the error from the exception. Must not be `null`, nor return `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
Task&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;

A successful result with the value of the `function`; otherwise, a failed one with the mapped error. Exceptions of other types propagate.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` task, or `mapException` returns `null`.

## TryAsync(Func&lt;ValueTask&gt;, Func&lt;Exception, Error&gt;, Boolean?)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L432" target="_blank">Result.Try.cs</a>

```csharp
public static ValueTask<Result> TryAsync(Func<ValueTask> action, Func<Exception, Error> mapException, bool? continueOnCapturedContext = null);
```

### Parameters
`action` Func&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;

The asynchronous action to run. Must not be `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)&gt;

The function that creates the error from the exception. Must not be `null`, nor return `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
ValueTask&lt;[Result](Result.md)&gt;

A successful result if the `action` completes; otherwise, a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`action` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`mapException` returns `null`.

## TryAsync&lt;TException&gt;(Func&lt;ValueTask&gt;, Func&lt;TException, Error&gt;, Boolean?)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L460" target="_blank">Result.Try.cs</a>

```csharp
public static ValueTask<Result> TryAsync<TException>(Func<ValueTask> action, Func<TException, Error> mapException, bool? continueOnCapturedContext = null)
    where TException : Exception;
```

### Type Parameters
#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`action` Func&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;

The asynchronous action to run. Must not be `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)&gt;

The function that creates the error from the exception. Must not be `null`, nor return `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
ValueTask&lt;[Result](Result.md)&gt;

A successful result if the `action` completes; otherwise, a failed one with the mapped error. Exceptions of other types propagate.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`action` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`mapException` returns `null`.

## TryAsync&lt;T&gt;(Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;Exception, Error&gt;, Boolean?)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L519" target="_blank">Result.Try.cs</a>

```csharp
public static ValueTask<Result<T>> TryAsync<T>(Func<ValueTask<T>> function, Func<Exception, Error> mapException, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value.

### Parameters
`function` Func&lt;ValueTask&lt;T&gt;&gt;

The asynchronous function to run. Must not be `null`; the value may be `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)&gt;

The function that creates the error from the exception. Must not be `null`, nor return `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
ValueTask&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;

A successful result with the value of the `function`; otherwise, a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`mapException` returns `null`.

## TryAsync&lt;T, TException&gt;(Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;TException, Error&gt;, Boolean?)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L553" target="_blank">Result.Try.cs</a>

```csharp
public static ValueTask<Result<T>> TryAsync<T, TException>(Func<ValueTask<T>> function, Func<TException, Error> mapException, bool? continueOnCapturedContext = null)
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

`mapException` Func&lt;TException, [Error](../Error/Error.md)&gt;

The function that creates the error from the exception. Must not be `null`, nor return `null`.

`continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)?

`true` to run the continuations, including `mapException` when it runs after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` (the default) to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md).

### Returns
ValueTask&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;

A successful result with the value of the `function`; otherwise, a failed one with the mapped error. Exceptions of other types propagate.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`mapException` returns `null`.

## See Also
* [Try](Result_Try.md)
* [Result](Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
