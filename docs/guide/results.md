# Results
## Result vs Result&lt;T&gt;
Use:
* `Result` for operations that does not return a value if successful (e.g., `DeleteBook(id)`),
* `Result<T>` for operations that return a value if successful (e.g., `GetUser(id)`).

## Result
Use
```csharp
return Result.Success();
```

to return successful `Result` without a value.

See more: [Result](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/Result/Result.md)

## Result&lt;T&gt;
Use
```csharp
public Result<User> GetUserById(int userId)
{
    // code to retrieve the User object
    return user;
}
```
to use the implicit operator to convert the value to `Result<T>`, or
```csharp
return Result.FromValue(user);
```
to explicitly create a `Result<T>` with a value.

The implicit conversion does not compile when the value is declared as an interface (`IUser user = …; Result<IUser> r = user;` is CS0029: C# applies no user-defined conversion from an interface type); use `Result.FromValue(user)` there.

Use
```csharp
var result = userResult.ToResult();
```
to convert `Result<T>` to `Result` if you need to return a non-generic result without a value.

> [!NOTE]
> A successful `Result<T>` normally carries a non-null value. When `T` is nullable—a nullable reference type or `Nullable<T>` (e.g., `Result<string?>` or `Result<int?>`)—a `null` is a valid successful value. Always use `IsSuccess` (or `IsError`) to distinguish success from failure rather than checking the value against `null`: reading the `Value` of a failed result throws an `InvalidOperationException`, and `TryGetValue(out var value)` reads it without an exception.

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
                ("accountId", "12345"), ("action", "account:update"));
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
            ("accountId", userId.ToString()), ("action", "account:retrieve"), ("_exception", ex.Message));
    }
}
```

See more: [Result&lt;T&gt;](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/ResultT/ResultT.md)
