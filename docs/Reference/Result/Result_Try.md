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

## Remarks
* **The mapper is required.** An [Error](../Error/Error.md) needs a category, a type *URI* and a title, which the library cannot pick on its own, so there is no default mapping (and no global one in [ResultConfiguration](../ResultConfiguration/ResultConfiguration.md)).
* **Nothing of the exception is copied into the error** by the library: an error is meant to be sent to clients, and the message or the stack trace can reveal internals. The mapper decides what the error contains, e.g., the exception type in [Metadata](../Error/Error_Metadata.md). The exception itself is not stored.
* **Cancellation is not a failure.** An [OperationCanceledException](https://learn.microsoft.com/dotnet/api/system.operationcanceledexception) (e.g., a `TaskCanceledException`) is rethrown, unless `TException` is `OperationCanceledException` or a type derived from it.
* **A mapper that returns `null`** throws an [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception); **a mapper that throws** lets its exception propagate.
* **For code that returns a task**, use [TryAsync](Result_TryAsync.md). A call such as `Result.Try(async () => await SaveAsync(), map)` or `Result.Try(() => LoadValueTaskAsync(), (IOException e) => …)` does not compile (CS0619), with an untyped or a typed mapper: the async lambda would become `async void`, whose exceptions escape `Try`, and a lambda that returns a task would give a result holding a task that is never awaited.

> [!NOTE]
> A lambda that always throws (`() => throw new …`) converts to any delegate type, including the hidden overloads for functions that return a task, so `Result.Try(() => throw new X(), map)` does not compile. Pass work that can complete (e.g., a method), or call `Result.FromError` directly.

## Try(Action, Func&lt;Exception, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L54" target="_blank">Result.Try.cs</a>

```csharp
public static Result Try(Action action, Func<Exception, Error> mapException);
```

### Parameters
`action` [Action](https://learn.microsoft.com/dotnet/api/system.action)

The action to run. Must not be `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)&gt;

The function that creates the error from the exception. Must not be `null`, nor return `null`.

### Returns
[Result](Result.md)

A successful result if the `action` completes; otherwise, a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`action` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`mapException` returns `null`.

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
public static Result Try<TException>(Action action, Func<TException, Error> mapException)
    where TException : Exception;
```

### Type Parameters
#### `TException`
The type of the exceptions to map; other exceptions propagate. An `OperationCanceledException` is mapped only when `TException` is `OperationCanceledException` or a type derived from it.

### Parameters
`action` [Action](https://learn.microsoft.com/dotnet/api/system.action)

The action to run. Must not be `null`.

`mapException` Func&lt;TException, [Error](../Error/Error.md)&gt;

The function that creates the error from the exception. Must not be `null`, nor return `null`.

### Returns
[Result](Result.md)

A successful result if the `action` completes; otherwise, a failed one with the mapped error. Exceptions of other types propagate.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`action` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`mapException` returns `null`.

## Try&lt;T&gt;(Func&lt;T&gt;, Func&lt;Exception, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L128" target="_blank">Result.Try.cs</a>

```csharp
public static Result<T> Try<T>(Func<T> function, Func<Exception, Error> mapException);
```

### Type Parameters
#### `T`
The type of the value.

### Parameters
`function` Func&lt;T&gt;

The function to run. Must not be `null`; it may return `null`.

`mapException` Func&lt;Exception, [Error](../Error/Error.md)&gt;

The function that creates the error from the exception. Must not be `null`, nor return `null`.

### Returns
[Result&lt;T&gt;](../ResultT/ResultT.md)

A successful result with the value of the `function`; otherwise, a failed one with the mapped error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`mapException` returns `null`.

### Examples
```csharp
Result<Customer> customer = Result.Try(
    () => _repository.Load(id),
    ex => ErrorBuilder.Unavailable(ErrorUri.Locator("https://exampleapp.com/docs/errors/database"), "The database is unavailable.")
        .WithMetadata("exceptionType", ex.GetType().Name)
        .Build());
```

## Try&lt;T, TException&gt;(Func&lt;T&gt;, Func&lt;TException, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.Try.cs#L160" target="_blank">Result.Try.cs</a>

```csharp
public static Result<T> Try<T, TException>(Func<T> function, Func<TException, Error> mapException)
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

`mapException` Func&lt;TException, [Error](../Error/Error.md)&gt;

The function that creates the error from the exception. Must not be `null`, nor return `null`.

### Returns
[Result&lt;T&gt;](../ResultT/ResultT.md)

A successful result with the value of the `function`; otherwise, a failed one with the mapped error. Exceptions of other types propagate.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

`function` or `mapException` is `null`.

[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

`mapException` returns `null`.

### Examples
```csharp
Result<int> number = Result.Try<int, FormatException>(
    () => int.Parse(text),
    _ => Error.Validation(ErrorUri.None, "The value is not a number."));
```

## See Also
* [TryAsync](Result_TryAsync.md)
* [Result](Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
