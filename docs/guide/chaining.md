# Chaining operations

How to chain operations that return results with the extension methods and the LINQ query syntax, synchronously and asynchronously, and how to turn exceptions into results. The extension methods are in the `Maple.Results.Extensions` namespace (with `Task`) and `Maple.Results.Extensions.ValueTasks` (with `ValueTask`).

## Contents
* [Extension methods](#extension-methods)
* [Asynchronous operations](#asynchronous-operations)
* [Query syntax](#query-syntax)
* [Turning exceptions into results](#turning-exceptions-into-results)
* [Asynchronous methods in UI applications](#asynchronous-methods-in-ui-applications)

## Extension methods
* `Then()` and `ThenAsync()`: transform or chain a successful result (a function that returns a value, a `Result`, a `Result<T>` or an `Error`),
* `Else()` and `ElseAsync()`: replace a failed result (e.g., recover with a value, or return another error),
* `IfSuccess()` and `IfSuccessAsync()`: run a side effect on a successful result and return it unchanged,
* `IfError()` and `IfErrorAsync()`: run a side effect on a failed result and return it unchanged,
* `Match()` and `MatchAsync()`: handle both cases, with two functions that return a value (e.g., `int count = result.Match(x => x.Count, e => 0)`) or two actions for a side effect.

A function that returns a value passed to `IfSuccess()` or `IfError()` is a compile error that names `Then()` or `Else()`, so that its value is not discarded by mistake; an action passed to `Then()` or `Else()` is a compile error that names `IfSuccess()` or `IfError()`. `Match()` with a success function and an error action is a compile error that names `Then(…).IfError(…)`.

```csharp
var createUserResult = _userService.CreateUser(userData);
createUserResult.Match(
    user => _auditService.LogUserAdded(user),
    error => _auditService.LogError(error));

var companyModel = createUserResult
    .Then(user => _companyService.GetCompanyById(user.CompanyId))
    .Match(
        company => _mapper.Map(company),
        error => _mapper.Map(error));
```

The library has no `Bind()`, `Map()` or `Switch()`: see [the FAQ](../../README.md#why-are-methods-like-bind-map-and-switch-not-available) for their equivalents.

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
> The asynchronous extension methods (`ThenAsync()`, `ElseAsync()`, `IfSuccessAsync()`, `IfErrorAsync()`, `MatchAsync()`, `ToResultAsync()` and the LINQ `Select()` and `SelectMany()`) support both `Task` and `ValueTask`, so you can chain and `await` results regardless of which one your methods return.

> [!IMPORTANT]
> The `Task`-based methods live in the `Maple.Results.Extensions` namespace and the `ValueTask`-based ones in `Maple.Results.Extensions.ValueTasks`—import the one matching the style you use, as having both in scope makes a call with an `async` lambda ambiguous.

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
> A query cannot declare a range variable twice, so a second non-generic clause needs another name (e.g., `from active in …`). Mixing a `Task` and a `ValueTask` in one query is not supported. A `select` that returns a task is a compile error, because the query would not await it: chain the operation in a `from` clause (with a function that returns `Task<Result<T>>`) or use `ThenAsync()`.

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

## Asynchronous methods in UI applications
The asynchronous extension methods that take a `Task` or `ValueTask` of a result await it with `ConfigureAwait(false)` by default, so the delegates you pass run on a thread-pool thread. That is the right choice for ASP.NET Core, console applications, Azure Functions and background services.

In WPF, Windows Forms, .NET MAUI or Blazor Server, when the delegates access UI elements, either pass `continueOnCapturedContext: true`, or set the global default once at startup (it is also used by the LINQ query syntax, which cannot pass the argument):
```csharp
ResultConfiguration.ContinueOnCapturedContext = true;
```

See more: [ResultConfiguration.ContinueOnCapturedContext](../Reference/ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md), [Maple.Results.Extensions](../Reference/Extensions/namespace.md), [Maple.Results.Extensions.ValueTasks](../Reference/Extensions/ValueTasks/namespace.md)
