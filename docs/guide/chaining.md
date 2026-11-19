# Chaining operations

How to chain operations that return results with the extension methods and the LINQ query syntax, synchronously and asynchronously, and how to turn exceptions into results. The extension methods are in the `Maple.Results.Extensions` namespace, for `Task` and `ValueTask` alike.

## Contents
* [Extension methods](#extension-methods)
* [Asynchronous operations](#asynchronous-operations)
* [Query syntax](#query-syntax)
* [Turning exceptions into results](#turning-exceptions-into-results)
* [Analyzer](#analyzer)
* [Asynchronous methods in UI applications](#asynchronous-methods-in-ui-applications)

## Extension methods
* `Then()` and `ThenAsync()`: transform or chain a successful result (a function that returns a value, a `Result`, a `Result<T>` or an `Error`),
* `Else()` and `ElseAsync()`: replace a failed result (e.g., recover with a value, or return another error),
* `IfSuccess()` and `IfSuccessAsync()`: run a side effect on a successful result and return it unchanged,
* `IfError()` and `IfErrorAsync()`: run a side effect on a failed result and return it unchanged,
* `Match()` and `MatchAsync()`: fold both cases into one value, with two functions that return the same type (e.g., `int count = result.Match(x => x.Count, e => 0)`); the value can be a result or a task, which the caller awaits,
* `Switch()` and `SwitchAsync()`: handle both cases with two actions for a side effect, and return nothing.

A function that returns a value passed to `IfSuccess()` or `IfError()` runs as a side effect, and its value is ignored (e.g., `result.IfSuccess(x => builder.Append(x))`); to continue with the value, use `Then()` or `Else()`. An action passed to `Then()` or `Else()` does not compile: it has no value to return, and the compiler reports it (e.g., CS0029, “Cannot implicitly convert type ‘void’ to ‘Result’”); use `IfSuccess()` or `IfError()` for a side effect. `Match()` with two actions does not compile either (CS0411); use `Switch()`. A call that compiles but loses a task or a failure (e.g., an action that returns a task passed to `Switch()`) is reported by the [analyzer](#analyzer).

A lambda that only throws is fine in `IfSuccess()`, `IfError()`, `Then()`, `Else()`, `Switch()` and `Match()`, in their asynchronous versions except `ElseAsync()`, and in `Select()` and `SelectMany()` called as methods on a result or a task of a result (e.g., `result.IfError(e => throw new OrderException(e.Title))`, or `int id = result.Match(x => x, e => throw new OrderException(e.Title))` to get the value). In `ElseAsync()` it is ambiguous (CS0121), because several overloads take a function that returns a task; use `Else()` after the `await`, or `MatchAsync()`. `Match()` with two lambdas that only throw does not compile either (CS0411): they have no type to infer.

```csharp
var createUserResult = _userService.CreateUser(userData);
createUserResult.Switch(
    user => _auditService.LogUserAdded(user),
    error => _auditService.LogError(error));

var companyModel = createUserResult
    .Then(user => _companyService.GetCompanyById(user.CompanyId))
    .Match(
        company => _mapper.Map(company),
        error => _mapper.Map(error));
```

The library has no `Bind()`, `Map()` or `Tap()`: see [the FAQ](../../README.md#why-are-methods-like-bind-map-and-tap-not-available) for their equivalents.

## Asynchronous operations
```csharp
var userTokenResult = await _userService.GetUserAsync(userData)
    .ThenAsync(user => _companyService.AddUserAsync(user))
    .ThenAsync(user => _loginService.GetUserTokenAsync(user))
    .IfErrorAsync(error => _auditService.LogErrorAsync(error));
```

The asynchronous methods also take synchronous delegates, so a synchronous step does not need `Task.FromResult` or an `await` first:
```csharp
var userNameResult = await _userService.GetUserAsync(userId)
    .ThenAsync(user => user.Name)
    .IfErrorAsync(error => _logger.LogWarning("{Title}", error.Title));
```

As `Else()`, `ElseAsync()` takes a function that returns the replacement result, a value or an `Error`: a value recovers from the error, and an `Error` replaces it:
```csharp
var countResult = await _counterService.GetCountAsync(id)
    .ElseAsync(async error => await _cache.GetLastCountAsync(id));       // a value: a successful Result<int>

var userResult = await _userService.GetUserAsync(id)
    .ElseAsync(async error => await _errorTranslator.TranslateAsync(error));   // an Error: a failed Result<User>
```

> [!NOTE]
> The asynchronous extension methods (`ThenAsync()`, `ElseAsync()`, `IfSuccessAsync()`, `IfErrorAsync()`, `MatchAsync()`, `SwitchAsync()`, `ToResultAsync()` and the LINQ `Select()` and `SelectMany()`) support both `Task` and `ValueTask`, so you can chain and `await` results regardless of which one your methods return.
>
> On a `Task` or a `ValueTask` of a result, the kind of the task is kept, and a function that returns the other kind of task is awaited (e.g., `await GetUserAsync(id).ThenAsync(user => _cache.GetProfileAsync(user.Id))`, where `GetUserAsync()` returns a `Task` and `GetProfileAsync()` a `ValueTask`). On a `Result` or a `Result<T>`, the function decides it: `result.ThenAsync(x => FindAsync(x))` gives a `ValueTask` when `FindAsync()` returns one, and an `async` lambda gives a `Task`. For a `ValueTask` from an `async` lambda, write its return type: `result.ThenAsync(async ValueTask<int> (int x) => …)`.

## Query syntax
The query syntax chains synchronous and asynchronous operations that return a `Result<T>`:
```csharp
var userTokenResult =
    await from userData in _validationService.ValidateUserDataAsync(userRequest)
    from user in _userService.GetUserAsync(userData)
    from token in _loginService.GetUserTokenAsync(user)
    select token;
```

Synchronous and asynchronous `from` clauses can be mixed in one query; the query is then asynchronous and has to be awaited:
```csharp
var userTokenResult =
    await from userData in _validationService.ValidateUserData(userRequest)   // synchronous
    from user in _userService.GetUserAsync(userData)                          // asynchronous
    select user.Token;
```

A single-clause query that only projects a successful value (via `Select()`) is supported too—the projection runs only on success, while an error is propagated unchanged:
```csharp
var displayNameResult =
    from user in _userService.GetUser(userId)
    select $"{user.FirstName} {user.LastName}";
```

A step that returns the non-generic `Result` (e.g., a check) can be a `from` clause too; its range variable is the successful `Result`, so it is usually named `_`. A `select` that returns a `Result` or a `Result<T>` gives that result, not a nested one:
```csharp
var orderResult =
    from user in _userService.GetUser(userId)
    from _ in _policy.EnsureActive(user)          // returns Result
    select _orderService.CreateOrder(user);       // returns Result<Order>: orderResult is a Result<Order>
```

> [!NOTE]
> A query cannot declare a range variable twice, so a second non-generic clause needs another name (e.g., `from active in …`). Mixing a `Task` and a `ValueTask` in one query is not supported. A `select` that returns a task is not awaited by the query, and the analyzer reports it as an error ([MR0001](../Analyzers/MR0001.md)): chain the operation in a `from` clause (with a function that returns `Task<Result<T>>`) or use `ThenAsync()`.

## Turning exceptions into results
`Result.Try()` and `Result.TryAsync()` run code that can throw (e.g., a library or a database call) and return a failed result instead of the exception. The mapper from the exception to the `Error` is required: the library cannot pick the category, the type *URI* and the title, and it never copies the exception message into the error, which is meant for clients.

```csharp
Result<Customer> customer = await Result.TryAsync(
    () => _repository.LoadAsync(id, cancellationToken),
    ex => ErrorBuilder.Unavailable(ErrorUri.Locator("https://exampleapp.com/docs/errors/database"), "The database is unavailable.")
        .WithMetadata("exceptionType", ex.GetType().Name)
        .Build());

// Only a FormatException is mapped; other exceptions propagate.
Result<int> number = Result.Try<int, FormatException>(
    () => int.Parse(text),
    _ => Error.Validation(ErrorUri.None, "The value is not a number."));
```

To map only one exception type, pass both type arguments (as above) or type the parameter of the mapper (`(FormatException e) => …`). Do not pass the exception type alone: C# infers all type arguments or none, so `Result.Try<FormatException>(() => int.Parse(text), …)` discards the number and returns a non-generic `Result`.

An `OperationCanceledException` is rethrown: cancellation is not a failure.

The mapper may return `null` to decline an exception, which is then rethrown as it is, so one mapper can handle several types:

```csharp
Result result = Result.Try(
    () => _repository.Save(order),
    ex => ex switch
    {
        DbUpdateConcurrencyException => Error.Conflict(ErrorUri.Tag("tag:exampleapp.com,2026:errors:orders:conflict"), "The order was changed."),
        TimeoutException => Error.Timeout(ErrorUri.Tag("tag:exampleapp.com,2026:errors:database:timeout"), "The database did not respond."),
        _ => null    // any other exception propagates
    });
```

A function that returns a `Result`, a `Result<T>` or an `Error` is flattened: `Result.Try(() => _repository.Load(id), map)` is a `Result<Customer>` when `Load` returns a `Result<Customer>`, and an `Error` gives a failed result.

See more: [Try](../Reference/Result/Result_Try.md), [TryAsync](../Reference/Result/Result_TryAsync.md)

## Analyzer
The package contains an analyzer, with nothing to install. It reports the calls that compile but do not do what they look like: a task that nobody awaits, a failure that nobody sees, and a task or a result stored as the value of another result.

```csharp
// MR0001 (error): a Result<Task<Result<User>>>, whose task nobody awaits; use ThenAsync()
var userResult = userDataResult.Then(userData => _userService.GetUserAsync(userData));

// MR0002 (error): IfError() takes an action, so the task is discarded; use IfErrorAsync()
userDataResult.IfError(error => _auditService.LogErrorAsync(error));

// MR0006 (warning): SaveAsync() returns a Task<Result>, and a failed result is lost
await _userService.SaveAsync(user);
```

| Rule | Severity | Reports |
| ---- | -------- | ------- |
| [MR0001](../Analyzers/MR0001.md) | Error | A task becomes the value of a result (e.g., a function that returns a task in `Then()`, `Result.Try()` or a `select` clause). |
| [MR0002](../Analyzers/MR0002.md) | Error | A function that returns a task, or an `async` lambda, is passed as an action (e.g., to `IfSuccess()`, `IfError()` or `Switch()`), and its task is not awaited. |
| [MR0003](../Analyzers/MR0003.md) | Error | A task, a result or an `Error` is converted to a `Result<object>` and stored as its value. |
| [MR0004](../Analyzers/MR0004.md) | Error | A function passed to `IfSuccess()`, `IfError()` or `Switch()` returns a result or an `Error`, which is ignored. |
| [MR0005](../Analyzers/MR0005.md) | Warning | `IfSuccess()` or `IfError()` ignores a value of the type of the result’s value; in version 2 such a call transformed the result. |
| [MR0006](../Analyzers/MR0006.md) | Warning | A statement calls a method that returns a result, and nothing uses the result. |

The errors cannot be suppressed as a warning can; a call that is meant as it is written says so in the code (e.g., explicit type arguments or `Result.FromValue()` to keep a task as a value). A delegate that fits no overload is rejected by the compiler, without advice of the library.

See more: [Analyzer](../Analyzers/README.md), [Configuring the rules](../Analyzers/README.md#configuring-the-rules), [What the compiler reports by itself](../Analyzers/README.md#what-the-compiler-reports-by-itself)

## Asynchronous methods in UI applications
The asynchronous extension methods that take a `Task` or `ValueTask` of a result await it with `ConfigureAwait(false)` by default, so the delegates you pass run on a thread-pool thread. That is the right choice for ASP.NET Core, console applications, Azure Functions and background services.

In WPF, Windows Forms, .NET MAUI or Blazor Server, when the delegates access UI elements, either pass `continueOnCapturedContext: true`, or set the global default once at startup (it is also used by the LINQ query syntax, which cannot pass the argument):
```csharp
ResultConfiguration.ContinueOnCapturedContext = true;
```

See more: [ResultConfiguration.ContinueOnCapturedContext](../Reference/ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md), [Maple.Results.Extensions](../Reference/Extensions/namespace.md)
