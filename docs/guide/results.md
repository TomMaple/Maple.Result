# Results

How to return the outcome of an operation as a `Result` or a `Result<T>`, and how to read it. A result is either successful (with a value, for `Result<T>`) or failed (with an [`Error`](errors.md)).

## Contents
* [`Result` or `Result<T>`](#result-or-resultt)
* [Returning a successful result](#returning-a-successful-result)
* [Returning a failed result](#returning-a-failed-result)
* [Reading a result](#reading-a-result)
* [Converting `Result<T>` to `Result`](#converting-resultt-to-result)
* [Example](#example)

## `Result` or `Result<T>`
Use:
* `Result` for operations that do not return a value if successful (e.g., `DeleteBook(id)`),
* `Result<T>` for operations that return a value if successful (e.g., `GetUser(id)`).

## Returning a successful result
Use `Result.Success()` to return a successful `Result` without a value:
```csharp
return Result.Success();
```

Return the value to convert it to a successful `Result<T>` with the implicit operator, or use `Result.FromValue(…)` to create it explicitly:
```csharp
public Result<User> GetUserById(int userId)
{
    // code to retrieve the User object
    return user;    // or: return Result.FromValue(user);
}
```

The implicit conversion does not compile when the value is declared as an interface (`IUser user = …; Result<IUser> r = user;` is CS0029: C# applies no user-defined conversion from an interface type); use `Result.FromValue(user)` there.

## Returning a failed result
Return the `Error`: it converts implicitly to a failed `Result` or `Result<T>` (or use `Result.FromError(error)` and `Result<T>.FromError(error)`):
```csharp
if (user is null)
{
    return Error.NotFound(ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:notfound"), "The user has not been found.");
}
```

See [Errors](errors.md) for the categories, the properties and how to build an error.

## Reading a result
* The `IsSuccess` and `IsError` properties tell success from failure (e.g., the early return `if (result.IsError) return result.Error;`).
* `TryGetValue` and `TryGetError` read the value or the error without an exception (e.g., `if (!GetUser(id).TryGetValue(out var user, out var error)) return error;`).
* The extension methods (`Then()`, `Else()`, `IfSuccess()`, `IfError()`, `Match()`, `Switch()` and their asynchronous versions) chain and handle results; see [Chaining operations](chaining.md).

> [!NOTE]
> A successful `Result<T>` normally carries a non-null value. When `T` is nullable—a nullable reference type or `Nullable<T>` (e.g., `Result<string?>` or `Result<int?>`)—a `null` is a valid successful value. Always use `IsSuccess` (or `IsError`) to distinguish success from failure rather than checking the value against `null`: reading the `Value` of a failed result throws an `InvalidOperationException`, and `TryGetValue(out var value)` reads it without an exception.

## Converting `Result<T>` to `Result`
Use `ToResult()` when you need to return a non-generic result without a value:
```csharp
var result = userResult.ToResult();
```

## Example
```csharp
public async Task<Result<User>> GetUserAsync(int userId)
{
    try
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.id == userId);

        if (user is null)
        {
            return Error.NotFound(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:account:notfound"),
                "Invalid user ID.",
                "The user account with the provided ID has not been found.",
                ErrorUri.Locator("https://api.exampleapp.com/errors/8783927589734857/details"),
                "errors.user.account.notFound",
                ("accountId", userId), ("action", "account:retrieve"));
        }

        return user;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An unexpected error occurred while retrieving user with ID {UserId}", userId);

        return Error.Critical(
            ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:account:geterror"),
            "An unexpected error occurred.",
            "An unexpected error occurred while getting the user account details.",
            ErrorUri.Locator("https://api.exampleapp.com/errors/2393284590348/details"),
            "errors.internal.dberror",
            ("accountId", userId), ("action", "account:retrieve"));
    }
}
```

The exception is logged, and its message is not copied into the error, which is meant for clients. To turn the exceptions of the code you call into failed results without a `try` block, see [Turning exceptions into results](chaining.md#turning-exceptions-into-results).

See more: [Result](../Reference/Result/Result.md), [Result&lt;T&gt;](../Reference/ResultT/ResultT.md)
