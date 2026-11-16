# Result.Try Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs code that can throw and turns the exception into a failed result, with the [Error](../Error/Error.md) that the `mapException` function creates.

## Overloads
| Name | Description |
| ---- | ----------- |
| [Try(Action, Func&lt;Exception, Error&gt;)](#tryaction-funcexception-error) | Runs the action; a [Result](Result.md). |
| [Try&lt;TException&gt;(Action, Func&lt;TException, Error&gt;)](#trytexceptionaction-functexception-error) | Runs the action and maps only the exceptions of the given type. |
| [Try&lt;T&gt;(Func&lt;T&gt;, Func&lt;Exception, Error&gt;)](#trytfunct-funcexception-error) | Runs the function; a [Result&lt;T&gt;](../ResultT/ResultT.md) with its value. |
| [Try&lt;T, TException&gt;(Func&lt;T&gt;, Func&lt;TException, Error&gt;)](#tryt-texceptionfunct-functexception-error) | Runs the function and maps only the exceptions of the given type. |
| [Try(Func&lt;Result&gt;, Func&lt;Exception, Error&gt;)](#tryfuncresult-funcexception-error) | Runs the function and returns the [Result](Result.md) it returns. |
| [Try&lt;TException&gt;(Func&lt;Result&gt;, Func&lt;TException, Error&gt;)](#trytexceptionfuncresult-functexception-error) | Runs the function and returns the [Result](Result.md) it returns; maps only the exceptions of the given type. |
| [Try&lt;T&gt;(Func&lt;Result&lt;T&gt;&gt;, Func&lt;Exception, Error&gt;)](#trytfuncresultt-funcexception-error) | Runs the function and returns the [Result&lt;T&gt;](../ResultT/ResultT.md) it returns. |
| [Try&lt;T, TException&gt;(Func&lt;Result&lt;T&gt;&gt;, Func&lt;TException, Error&gt;)](#tryt-texceptionfuncresultt-functexception-error) | Runs the function and returns the [Result&lt;T&gt;](../ResultT/ResultT.md) it returns; maps only the exceptions of the given type. |
| [Try(Func&lt;Error&gt;, Func&lt;Exception, Error&gt;)](#tryfuncerror-funcexception-error) | Runs the function and returns a failed [Result](Result.md) with the [Error](../Error/Error.md) it returns. |
| [Try&lt;TException&gt;(Func&lt;Error&gt;, Func&lt;TException, Error&gt;)](#trytexceptionfuncerror-functexception-error) | Runs the function and returns a failed [Result](Result.md) with the [Error](../Error/Error.md) it returns; maps only the exceptions of the given type. |

## Remarks
* **The mapper is required.** An [Error](../Error/Error.md) needs a category, a type *URI* and a title, which the library cannot pick on its own, so there is no default mapping (and no global one in [ResultConfiguration](../ResultConfiguration/ResultConfiguration.md)).
* **Nothing of the exception is copied into the error** by the library: an error is meant to be sent to clients, and the message or the stack trace can reveal internals. The mapper decides what the error contains, e.g., the exception type in [Metadata](../Error/Error_Metadata.md). The exception itself is not stored.
* **A function that returns a `Result`, a `Result<T>` or an `Error` is flattened.** Its result is returned as it is (a failed result keeps its error), and an `Error` gives a failed result, as in `Then` and `Match`: `Result.Try(() => repository.Load(id), map)` is a `Result<Customer>` when `Load` returns a `Result<Customer>`. A `null` result or error throws an `InvalidOperationException` and is not mapped. Pass the type argument explicitly to keep the nested result (e.g., `Result.Try<Result<int>>(…)`).
* **Cancellation is not a failure.** An [OperationCanceledException](https://learn.microsoft.com/dotnet/api/system.operationcanceledexception) (e.g., a `TaskCanceledException`) is rethrown, unless `TException` is `OperationCanceledException` or a type derived from it.
* **A mapper that returns `null` declines the exception**: it is rethrown as it is (the same instance, with its stack trace). One mapper can handle several types and let the others through, e.g., `ex => ex switch { DbUpdateConcurrencyException => Error.Conflict(…), TimeoutException => Error.Timeout(…), _ => null }`. **A mapper that throws** lets its exception propagate.
* **A timeout of `HttpClient`** is a `TaskCanceledException` that wraps a `TimeoutException`, so it is rethrown as a cancellation by default. To map it, ask for the cancellation type and decline the real cancellations: `Result.TryAsync<OperationCanceledException>(…, e => e.InnerException is TimeoutException ? Error.Timeout(…) : null)`.
* **For code that returns a task**, use [TryAsync](Result_TryAsync.md). A call such as `Result.Try(async () => await SaveAsync(), map)` or `Result.Try(() => LoadValueTaskAsync(), (IOException e) => …)` does not compile (CS0619), with an untyped or a typed mapper: the async lambda would become `async void`, whose exceptions escape `Try`, and a lambda that returns a task would give a result holding a task that is never awaited.

> [!NOTE]
> A lambda that always throws (`() => throw new …`) converts to any delegate type, including the hidden overloads for functions that return a task, so `Result.Try(() => throw new X(), map)` does not compile. Pass work that can complete (e.g., a method), or call `Result.FromError` directly.

## Try(Action, Func&lt;Exception, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L55" target="_blank">Result.Try.cs</a>

```csharp
public static Result Try(Action action, Func<Exception, Error?> mapException);
```

### Parameters
`action` [Action](https://learn.microsoft.com/dotnet/api/system.action)

The action to run. Must not be `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

### Returns
[Result](Result.md)

A successful result if the `action` completes; otherwise, a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`action` or `mapException` is `null`.

### Examples
```csharp
var result = Result.Try(
    () => File.Delete(path),
    ex => Error.Failure(ErrorUri.Tag("tag:exampleapp.com,2026:errors:files:delete"), "The file could not be deleted.")
        .WithMetadata("exceptionType", ex.GetType().Name));
```

## Try&lt;TException&gt;(Action, Func&lt;TException, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L82" target="_blank">Result.Try.cs</a>

```csharp
public static Result Try<TException>(Action action, Func<TException, Error?> mapException)
    where TException : Exception;
```

### Type Parameters
#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`action` [Action](https://learn.microsoft.com/dotnet/api/system.action)

The action to run. Must not be `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

### Returns
[Result](Result.md)

A successful result if the `action` completes; otherwise, a failed one with the mapped error. Exceptions of other types propagate.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`action` or `mapException` is `null`.

## Try&lt;T&gt;(Func&lt;T&gt;, Func&lt;Exception, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L130" target="_blank">Result.Try.cs</a>

```csharp
public static Result<T> Try<T>(Func<T> function, Func<Exception, Error?> mapException);
```

### Type Parameters
#### `T`
The type of the value.

### Parameters
`function` Func&lt;T&gt;

The function to run. Must not be `null`; it may return `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

### Returns
[Result&lt;T&gt;](../ResultT/ResultT.md)

A successful result with the value of the `function`; otherwise, a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

### Examples
```csharp
Result<Customer> customer = Result.Try(
    () => _repository.Load(id),
    ex => ErrorBuilder.Unavailable(ErrorUri.Locator("https://exampleapp.com/docs/errors/database"), "The database is unavailable.")
        .WithMetadata("exceptionType", ex.GetType().Name)
        .Build());
```

## Try&lt;T, TException&gt;(Func&lt;T&gt;, Func&lt;TException, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L161" target="_blank">Result.Try.cs</a>

```csharp
public static Result<T> Try<T, TException>(Func<T> function, Func<TException, Error?> mapException)
    where TException : Exception;
```

### Type Parameters
#### `T`
The type of the value.

#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`function` Func&lt;T&gt;

The function to run. Must not be `null`; it may return `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

### Returns
[Result&lt;T&gt;](../ResultT/ResultT.md)

A successful result with the value of the `function`; otherwise, a failed one with the mapped error. Exceptions of other types propagate.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

### Examples
```csharp
Result<int> number = Result.Try<int, FormatException>(
    () => int.Parse(text),
    _ => Error.Validation(ErrorUri.None, "The value is not a number."));
```

## Try(Func&lt;Result&gt;, Func&lt;Exception, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L618" target="_blank">Result.Try.cs</a>

```csharp
public static Result Try(Func<Result> function, Func<Exception, Error?> mapException);
```

### Parameters
`function` Func&lt;Result&gt;

The function to run. Must not be `null`, nor return `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

### Returns
[Result](Result.md)

The [Result](Result.md) returned by the `function`, as it is; or a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` result.

## Try&lt;TException&gt;(Func&lt;Result&gt;, Func&lt;TException, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L649" target="_blank">Result.Try.cs</a>

```csharp
public static Result Try<TException>(Func<Result> function, Func<TException, Error?> mapException)
    where TException : Exception;
```

### Type Parameters
#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`function` Func&lt;Result&gt;

The function to run. Must not be `null`, nor return `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

### Returns
[Result](Result.md)

The [Result](Result.md) returned by the `function`, as it is; or a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` result.

## Try&lt;T&gt;(Func&lt;Result&lt;T&gt;&gt;, Func&lt;Exception, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L699" target="_blank">Result.Try.cs</a>

```csharp
public static Result<T> Try<T>(Func<Result<T>> function, Func<Exception, Error?> mapException);
```

### Type Parameters
#### `T`
The type of the value.

### Parameters
`function` Func&lt;Result&lt;T&gt;&gt;

The function to run. Must not be `null`, nor return `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

### Returns
[Result&lt;T&gt;](../ResultT/ResultT.md)

The [Result&lt;T&gt;](../ResultT/ResultT.md) returned by the `function`, as it is; or a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` result.

## Try&lt;T, TException&gt;(Func&lt;Result&lt;T&gt;&gt;, Func&lt;TException, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L731" target="_blank">Result.Try.cs</a>

```csharp
public static Result<T> Try<T, TException>(Func<Result<T>> function, Func<TException, Error?> mapException)
    where TException : Exception;
```

### Type Parameters
#### `T`
The type of the value.

#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`function` Func&lt;Result&lt;T&gt;&gt;

The function to run. Must not be `null`, nor return `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

### Returns
[Result&lt;T&gt;](../ResultT/ResultT.md)

The [Result&lt;T&gt;](../ResultT/ResultT.md) returned by the `function`, as it is; or a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` result.

## Try(Func&lt;Error&gt;, Func&lt;Exception, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L780" target="_blank">Result.Try.cs</a>

```csharp
public static Result Try(Func<Error> function, Func<Exception, Error?> mapException);
```

### Parameters
`function` Func&lt;Error&gt;

The function to run. Must not be `null`, nor return `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

### Returns
[Result](Result.md)

A failed [Result](Result.md) with the [Error](../Error/Error.md) returned by the `function`, or with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` error.

## Try&lt;TException&gt;(Func&lt;Error&gt;, Func&lt;TException, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L811" target="_blank">Result.Try.cs</a>

```csharp
public static Result Try<TException>(Func<Error> function, Func<TException, Error?> mapException)
    where TException : Exception;
```

### Type Parameters
#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`function` Func&lt;Error&gt;

The function to run. Must not be `null`, nor return `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)?&gt;

The function that creates the error from the exception, or returns `null` to decline it (the exception is then rethrown as it is). Must not be `null`.

### Returns
[Result](Result.md)

A failed [Result](Result.md) with the [Error](../Error/Error.md) returned by the `function`, or with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`function` returns a `null` error.

## See Also
* [TryAsync](Result_TryAsync.md)
* [Result](Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
