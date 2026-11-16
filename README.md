[![NuGet Version](https://img.shields.io/nuget/v/Maple.Result?label=Maple.Result%20%E2%80%94%20nuget)](https://www.nuget.org/packages/Maple.Result/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Maple.Result)](https://www.nuget.org/packages/Maple.Result/)
[![CI](https://img.shields.io/github/actions/workflow/status/TomMaple/Maple.Result/ci.yml?branch=main&label=CI)](https://github.com/TomMaple/Maple.Result/actions/workflows/ci.yml?query=branch%3Amain)
[![GitHub last commit](https://img.shields.io/github/last-commit/TomMaple/Maple.Result/main)](https://github.com/TomMaple/Maple.Result/commits/main/)
[![License: MIT](https://img.shields.io/github/license/TomMaple/Maple.Result)](https://github.com/TomMaple/Maple.Result/blob/main/LICENSE)
[![OpenSSF Scorecard](https://api.scorecard.dev/projects/github.com/TomMaple/Maple.Result/badge)](https://scorecard.dev/viewer/?uri=github.com/TomMaple/Maple.Result)



# Result
An abstraction of the operation result which can be either successful (with or without a value) or failed (with an error).

It can be mapped to or from the HTTP response if needed. Check:
* [Maple.Result.Extensions.AspNetCore](https://github.com/TomMaple/Maple.Result.Extensions.AspNetCore),
* [Maple.Result.Extensions.HttpClient](https://github.com/TomMaple/Maple.Result.Extensions.HttpClient).


# Give it a star ⭐
Do you like it? Show your support by giving this project a star!

# Why❓
## Why the Result pattern?
This software design approach provides a structured and precise way to return the operation outcome—either successful or failed.

* No guessing what the `null` means!
* Clear way of indicating whether the operation is successful!
* No need for misusing exceptions for not *exceptional* cases or controlling the flow!

## Why this library?
Although, there are many existing implementation of the *Result* pattern in C#, I decided to create this implementation to address a few missing functionalities in other libraries, e.g.

* support for localization,
* support for consistent error categorization,
* support for precise error description,
* following standards ([RFC 9457](https://datatracker.ietf.org/doc/html/rfc9457)),
* Native AOT and trimming compatibility (checked by the trim and AOT analyzers in every build).

# Quick Links
* [Quick Start](https://github.com/TomMaple/Maple.Result#quick-start)
* [The Result Pattern](https://github.com/TomMaple/Maple.Result#the-result-pattern)
* [How To Use Result library](https://github.com/TomMaple/Maple.Result#how-to-use-result-library)
* [Mapping to HTTP responses](https://github.com/TomMaple/Maple.Result#mapping-to-http-responses)
* [Upgrading to 3.0](https://github.com/TomMaple/Maple.Result/blob/main/docs/migration-v2-to-v3.md)
* [FAQ](https://github.com/TomMaple/Maple.Result#faq)
* [Learn More & Documentation](https://github.com/TomMaple/Maple.Result#learn-more)

# Quick Start 
## Adding the *NuGet* Package
Add the `Maple.Result` package to your project using the Package Manager in your IDE or the *dotnet* tool in the console:

```shellscript
dotnet add package Maple.Result
```

The `Maple.Result` package contains all the types and the core functionality of the *Result* pattern, in the `Maple.Results` namespace:

```csharp
using Maple.Results;
using Maple.Results.Extensions;  // extension methods: Then, Else, IfSuccess, Match, LINQ, …
```

## Using the *Result*
```csharp
public async Task<Result> DeleteContactAsync(Guid id)
{
    if (id == Guid.Empty)
    {
        return Error.Validation(
            ErrorUri.Tag("tag:exampleapp.com,2026-01:errors:contact:delete:validation"),
            "Cannot delete the contact.",
            "Invalid ID of the contact to delete.",
            detailTemplateId: "errors.contact.delete.id.validation");
    }

    var user = await _contactRepository.GetByIdAsync(id);
    if (user is null)
    {
        return Error.NotFound(
            ErrorUri.Tag("tag:exampleapp.com,2026-01:errors.contact.notFound"),
            "Cannot delete the contact.",
            "Contact with provided ID has not been found.",
            detailTemplateId: "errors.contact.delete.id.notFound");
    }

    await _contactRepository.DeleteAsync(id);
    return Result.Success();
}
```

## Chaining operations
### Available extension methods
* `Then()` and `ThenAsync()`: transform or chain a successful result (a function that returns a value, a `Result`, a `Result<T>` or an `Error`),
* `Else()` and `ElseAsync()`: replace a failed result (e.g., recover with a value, or return another error),
* `IfSuccess()` and `IfSuccessAsync()`: run a side effect on a successful result and return it unchanged,
* `IfError()` and `IfErrorAsync()`: run a side effect on a failed result and return it unchanged,
* `Match()` and `MatchAsync()`: handle both cases, with two functions that return a value (e.g., `int count = result.Match(x => x.Count, e => 0)`) or two actions for a side effect.

A function that returns a value passed to `IfSuccess()` or `IfError()` is a compile error that names `Then()` or `Else()`, so that its value is not discarded by mistake; an action passed to `Then()` or `Else()` is a compile error that names `IfSuccess()` or `IfError()`. `Match()` with a success function and an error action is a compile error that names `Then(…).IfError(…)`.

#### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);
await createUserResult.MatchAsync(
    user => _auditService.LogUserAddedAsync(user),
    error => _auditService.LogErrorAsync(error));

var companyResult = createUserResult
    .Then(() => _companyService.GetCompanyById(userData.CompanyId))
    .Match(
        company => _mapper.Map(company),
        error => _mapper.Map(error));
```
or chaining multiple asynchronous operations:
```csharp
var userTokenResult =
    await _validationService.ValidateUserDataAsync(userRequest)
        .ThenAsync(userData => _userService.GetUserAsync(userData))
        .ThenAsync(user => _loginService.GetUserTokenAsync(user));
```

The asynchronous methods also take synchronous delegates, so a synchronous step does not need `Task.FromResult` or an `await` first:
```csharp
var userNameResult = await _userService.GetUserAsync(userId)
    .ThenAsync(user => user.Name)
    .IfErrorAsync(error => _logger.LogWarning("{Title}", error.Title));
```

or chaining multiple asynchronous operations using the query syntax:
```csharp
var userTokenResult =
    await from userData in _validationService.ValidateUserDataAsync(userRequest)
    from user in _userService.GetUserAsync(userData)
    from token in _loginService.GetUserTokenAsync(user)
    select token;
```

A single-clause query that only projects a successful value (via `Select()`) is supported too—the projection runs only on success, while an error is propagated unchanged:
```csharp
var displayNameResult =
    from user in _userService.GetUser(userId)
    select $"{user.FirstName} {user.LastName}";
```

> [!NOTE]
> The asynchronous extension methods (`ThenAsync()`, `ElseAsync()`, `IfSuccessAsync()`, `IfErrorAsync()`, `MatchAsync()`, `ToResultAsync()` and the LINQ `Select()` and `SelectMany()`) support both `Task` and `ValueTask`, so you can chain and `await` results regardless of which one your methods return.

> [!IMPORTANT]
> The `Task`-based methods live in the `Maple.Results.Extensions` namespace and the `ValueTask`-based ones in `Maple.Results.Extensions.ValueTasks`—import the one matching the style you use, as having both in scope makes a call with an `async` lambda ambiguous.

See more: [Maple.Results.Extensions](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/Extensions/namespace.md)

# The Result Pattern
## Key Benefits
* **Explicitness** — clearly communicates (e.g., via the function signature) that the operation may fail; it forces developer to handle both scenarios: a success and a failure.
* **Consistency** — offers consistent type of error and consistent structure of responses.
* **Composability** — allows easier chaining of operations with extension methods, instead of multi-nested `if` blocks.
* **Predictable Error Handling** — clearly defines categories of errors, making it easier to properly handle expected failures (e.g., validation errors, availability problems, a not-found resource).
* **Support for localization** — uses data structures that allow to localize messages by the client.
* **Easier Testing** — provides an easy way to verify in tests whether the operation was successful or what errors occurred.
* **Improved Readability** — limits the usage and scope of `try-catch` blocks to places that actually can throw exceptions (e.g., calls to the database, cloud resources, disk operations, HTTP calls, etc.).
* **Improved Monitoring** — allows to limit the number of thrown exceptions to truly exceptional cases making it easier to monitor them (if we don’t need to filter out exceptions used for the flow control).
* **Improved Debugging** — allows to enrich the error outcome with additional metadata.
* **Follows Standards** — follows [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457) and other industry standards.

## Result vs. Exceptions
| **Feature** | **Result** | **Exceptions** |
| ----------- | ---------- | -------------- |
| **Use Cases** | Expected, routine failures *(e.g., validation, not found, missing data, authentication and authorization errors)* | Rare, truly unexpected system or technical errors that usually indicate a bug or infrastructure problems *(e.g., database connection loss, network issues)* |
| **Mechanism** | Returns a structured record as the function outcome | Disrupts the normal program flow and bubbles up the call stack |
| **Error Handling Style** | Functional, return-value based | Imperative, runtime-based |
| **Type Safety** | Type-safe—the compiler knows a method can return an error | Not type-safe—errors are not visible in a method signature |
| **Control flow** | Clean, predictable flow maintained | Disrupted when an error occurs |
| **Composition** | Easy chaining of operations | Hard with multiple `try-catch` |
| **Boilerplate** | Can add some boilerplate code, which can be reduced with extension methods and other libraries | Centralizes error handling, keeping business logic clean of error checks |
| **Performance** | Better for frequent failures—avoids the overhead of exception handling | Slower—exceptions are expensive due to the cost of generating stack traces |
| **Monitoring** | Easy to distinguish expected errors from unexpected problems that needs potential attention | Requires filtering out expected errors from real issues. It can make it harder to investigate problems. |

## From [Stack Overflow](https://stackoverflow.com/questions/729379/why-not-use-exceptions-as-regular-flow-of-control) discussion:
> Have you ever tried to debug a program raising five exceptions per second in the normal course of operation ?

> Exceptions are basically non-local`goto`statements with all the consequences of the latter. Using exceptions for flow control violates a [principle of least astonishment](http://c2.com/cgi/wiki?PrincipleOfLeastAstonishment), make programs hard to read

> If you use exceptions for normal situations, how will you locate things that are*really*exception?

# How To Use Result library
## Result
### Result vs Result&lt;T&gt;
Use:
* `Result` for operations that does not return a value if successful (e.g., `DeleteBook(id)`),
* `Result<T>` for operations that return a value if successful (e.g., `GetUser(id)`).

### Result
Use
```csharp
return Result.Success();
```

to return successful `Result` without a value.

See more: [Result](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/Result/Result.md)

### Result&lt;T&gt;
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

### Example
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

## Error
Use proper static methods to create an `Error` instance, e.g.
```csharp
public Result UpdateUser(int userId, UpdateUserModel updateModel)
{
    // code to retrieve the User object
    if (user is null)
    {
        return Error.NotFound(
                        ErrorUri.Tag("tag:exampleapp.com,2026:errors:account:notfound"),
                        "Cannot update the account.");
    }
    // code to update the user
}
```
or
```csharp
var error = Error.Validation(
        ErrorUri.Tag("tag:exampleapp.com,2026:errors:signup:form"),
        "Invalid data to create a user.",
        "The user cannot be created because of missing or invalid data. Address the validation errors and try again.",
        ErrorUri.Locator("https://exampleapp.com/errors/7894375839"),
        "errors.signup.validation"
    )
    .WithErrorDetail("/email", "Email is required.", "errors.signup.validation.email.required")
    .WithErrorDetail("/firstName", "At least 3 characters are required.", "errors.signup.validation.email.minLength", ("minLength", 3));
```

### Error Categories
The error categories are based on the HTTP status codes—that’s why those have been included in the table.

Although, mapping to HTTP responses is not part of this library and can be done differently.

> [!NOTE]
> The status codes in the table are the **default** mapping of the `Maple.Result.Extensions.*` packages, which can be customized in both directions (category → status code and status code → category); see the [problem details format](https://github.com/TomMaple/Maple.Result/blob/main/docs/problem-details.md#category-and-status).

| Category | Description | Solution | Example error | Corresponding HTTP status code |
| -------- | ----------- | -------- | ------------- | ------------------------------ |
| **Conflict** | The operation failed due to the data or state conflict (such as a unique constraint violation). Refresh data and try again. | Refresh the data, then try again if applicable. May require repeating the whole process. | Details have been modified. Please refresh the page. | `409` (Conflict) |
| **Critical** | Unexpected, critical error. | Contact customer support. | Payment failed.&#xA;3rd party API is not available. | `500` (Internal Server Error) |
| **Failure** | An expected error that is not critical to the operation. Might be related to e.g., a business logic rules, or the data that passes the validation, but still incorrect. | Even if the data is correct, the operation is not. | Invalid file format.&#xA;Cannot delete the payment which processing has started. | `422` (Unprocessable Content) |
| **Limit Exceeded** | The request is valid and permitted, but a usage limit, quota or rate has been exhausted. Applies to business limits as well as throttling. Not for input-size checks (use **Validation**). | Wait and try again later, or raise the limit (e.g. upgrade the plan). | Too many requests—please try again in 30 seconds.&#xA;The monthly limit of 100 reports has been reached. | `429` (Too Many Requests) |
| **Not Found** | The resource was not found. | Invalid identifier. Try a different one. | Company with that name has not been found. | `404` (Not Found) |
| **Not Implemented** | The operation, feature or case is not implemented yet. | To be implemented in the future. | We don’t support debit cards yet. | `501` (Not Implemented) |
| **Unauthorized** | The authentication might be valid, but the identity does not have the required permissions. | Ask for permissions, or, a different user is required. | Action is not allowed for that user. | `403` (Forbidden) |
| **Timeout** | The operation timed out. | Heavy load. Try again later. | Server did not respond within 30 seconds. Try again later. | `504` (Gateway Timeout) when an upstream server or a dependency did not respond in time;&#xA;`408` (Request Timeout) when the client did not send the complete request in time |
| **Unavailable** | The service is not available (e.g. it is down or overloaded), but please try again later. Not for exhausted usage limits (use **Limit Exceeded**). | Some service is down. Please try again later. | Report is not available—please try again later. | `503` (Service Unavailable) |
| **Unauthenticated** | The authentication is either missing or invalid. | Log in. | User is not logged in. | `401` (Unauthorized) |
| **Validation** | User input data is invalid. | Update the data and try again. | Email is required. | `400` (Bad Request) |

### Error properties
| Property | Type | Description | Required | Example |
| -------- | ---- | ----------- | -------- | ------- |
| `Category` | `ErrorCategory` | A category of the error (one of predefined). | Yes | `ErrorCategory.Validation` |
| `TypeUri` | `string` | A *URI* that identifies the type of the problem. When dereferenced, provides human-readable documentation about the problem. | Yes | `https://app.example.com/errors/user/invalid-email` |
| `Title` | `string` | A short, human-readable summary of the problem type. It stays the same for the same type of error. | Yes | The provided value is not a valid email. |
| `Detail` | `string` | The human-readable explanation specific to this occurrence of the problem. | No | Special character `/` is not allowed in the email address. |
| `DetailTemplated` | `TemplatedMessage` | A structure to generate a localized detail message. | No | `{ "templateId": "errors.signup.email.specialcharacters", "params": { "invalidCharacter": "/" }}` |
| `InstanceUri` | `string` | A *URI* that identifies the specific occurrence of the problem. | No | `https://api.exampleapp.com/errors/8783927589734857/details` |
| `ErrorDetails` | `IReadOnlyList<ErrorDetail>` | The collection of additional error details. | No | `[{ "propertyPointer": "/email", "detail": "Invalid format", "detailTemplated": { "templateId": "errors.signup.email.invalidFormat" }}]` |
| `Metadata` | `IReadOnlyDictionary<string, object?>` | Additional, machine-readable members of the problem (RFC 9457 *extension members*). | No | `{ "limit": 100, "resetAt": "2026-12-01T00:00:00Z" }` |

### Type URI
It is a mandatory URI that identifies the type of the problem. It can be:
* `about:blank` (`ErrorUri.None`) — the type is not specified,
* *URI Locator* (`ErrorUri.Locator(…)`) — dereferencing it SHOULD provide human-readable documentation for the problem type (use an absolute rather than a relative value) (e.g., `https://exampleapp.com/docs/errors/signup/validation/email-required`),
* *URI Tag* (`ErrorUri.Tag(…)`) — it is not dereferenceable and uniquely represents the type of the problem (e.g., `tag:exampleapp.com,2026:errors.user.signup.validation.emailRequired`),
* *URN* (`ErrorUri.Urn(…)`) — a persistent, non-dereferenceable name of the type of the problem (e.g., `urn:exampleapp:errors:signup:email-required`),
* relative reference (`ErrorUri.Relative(…)`) — a full path resolved against the base URI of the response (e.g., `/errors/signup/email-required`); RFC 9457 recommends absolute URIs when possible.

When the kind is not known in advance (e.g., a type URI read from another service), `ErrorUri.Parse(…)` and `ErrorUri.TryParse(…)` detect it from the value and apply the same validation. A URI with any other scheme (e.g., `mailto:`, which RFC 9457 allows) becomes `ErrorUri.Other(…)`.

### Instance URI
It is an optional URI that identifies the specific occurrence of the problem. It can be:
* *URI Locator* (`ErrorUri.Locator(…)`) — dereferencing it SHOULD provide the problem details object or information about the problem occurrence in other formats (e.g., `https://exampleapp/api/errors/982384293852/details`),
* *URI Tag* (`ErrorUri.Tag(…)`) — a unique identifier for the problem occurrence which may be meaningful to the server but is opaque to the client (e.g., `tag:exampleapp.com,2026:errors.func-sgnapi-p-01.logs.20260221.db382751-81b8-40da-8302-0a07dd16a5de`),
* *URN* (`ErrorUri.Urn(…)`) — e.g., `urn:uuid:6e8bc430-9c3a-11d9-9669-0800200c9a66`,
* relative reference (`ErrorUri.Relative(…)`) — a full path resolved against the base URI of the response (e.g., `/account/12345/msgs/abc`, as in RFC 9457).

`about:blank` (`ErrorUri.None`) is not allowed as an instance; pass `null` when there is no instance.

### Support for multiple error details / validation errors
```csharp
var builder = ErrorBuilder.Validation(
    ErrorUri.Tag("tag:exampleapp.com,2026:errors:signup:form"),
    "Invalid data to create a user.");

if (string.IsNullOrWhiteSpace(userInput.email))
{
    builder.AddErrorDetail(
        "/email",
        "The email is required.",
        "errors.user.input.email.required");
}
else if (userInput.email.Length < 8)
{
    builder.AddErrorDetail(
        "/email",
        "The email must be at least 8 characters long.",
        "errors.user.input.email.minLength",
        ("minLength", 8));
}

var error = builder.Build();
```

To add a single detail to an existing error, use `error.WithErrorDetail(...)`, which returns a copy.

The `propertyPointer` is a JSON Pointer. For a name taken from the input (a dictionary key, a form field), build it with `ErrorDetail.Pointer("items", 0, name)`: it escapes `~` and `/` in the names, so the pointer is always valid.

where the parameters are:

| Parameter | Type | Description | Required | Example |
| --------- | ---- | ----------- | -------- | ------- |
| `propertyPointer` | `string` | JSON Pointer to the input data. | no | `/firstName` |
| `detail` | `string` | A human-readable explanation of this error detail. | yes | `Mininum 5 characters are required.` |
| `detailTemplateId` | `string` | A message template ID. | no | `errors.signup.validation.firstName.minLength` |
| `detailNamedValues` | `(string, object?)[]` | The named values for the message template; names must be unique, and values require `detailTemplateId`. | no | `("minLength", 5)` |

### Metadata
`Metadata` holds machine-readable, response-level values—the *extension members* of [RFC 9457](https://datatracker.ietf.org/doc/html/rfc9457#section-3.2)—that a client can read instead of parsing the `Detail` text. The library does not define any well-known keys.

```csharp
var error = Error.LimitExceeded(
        ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
        "Cannot generate the report.",
        "The monthly limit of 100 reports has been reached.")
    .WithMetadata("limit", 100)
    .WithMetadata("resetAt", "2026-12-01T00:00:00Z");

// Reads the value only if it is an int (no conversion).
if (error.TryGetMetadata<int>("limit", out var limit))
{
    // ...
}
```

* `WithMetadata()` and `WithoutMetadata()` return a **copy**; the original error is not modified. Several entries can be added in one call: `WithMetadata(("limit", 100), ("remaining", 0))`.
* Keys must not match the standard members `type`, `title`, `status`, `detail`, `instance`, `errors` and `detailTemplated` (case-insensitive), and two keys of one error must not differ only in case (e.g., `RetryAfter` and `retryAfter`); `WithMetadata()` throws an `ArgumentException` for them. Each entry becomes a top-level member of the problem details object; see the [problem details format](https://github.com/TomMaple/Maple.Result/blob/main/docs/problem-details.md).
* Keys should start with a letter, use letters, digits and `_`, and be at least 3 characters long (RFC 9457 recommendation, not enforced).
* Values can be `null`. `TryGetMetadata<T>()` is strict: it returns `true` only when the stored value is a `T` (a stored `int` is not read as `long`), and never for a `null` value.

See more: [Metadata](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/Error/Error_Metadata.md)

### ErrorBuilder
`Error` is immutable. Use an `ErrorBuilder` to assemble an error step by step, and `Build()` to get it:
```csharp
var error = ErrorBuilder.LimitExceeded(
        ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
        "Cannot generate the report.")
    .WithDetail("The monthly limit of 100 reports has been reached.")
    .WithMetadata("limit", 100)
    .Build();
```

To change an existing error, use `ToBuilder()`:
```csharp
var publicError = error.ToBuilder()
    .WithTitle("The report cannot be generated.")
    .WithoutMetadata()
    .Build();
```

* Create a builder with `new ErrorBuilder(category, typeUri, title)`, a static method per category (e.g., `ErrorBuilder.Validation(...)`), or `error.ToBuilder()`.
* Every argument is validated when it is passed, so `Build()` never throws. `Build()` takes a snapshot; the builder stays usable.
* The builder is not thread-safe.

See more: [ErrorBuilder](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/ErrorBuilder/ErrorBuilder.md)

### Support for localization
The localization can be achieved by generating messages based on the template.

#### Example
```csharp
var error = Error.Unauthorized(
    ErrorUri.Tag("tag:exampleapp.com,2026:errors:contact:delete:authorization:missing"),
    "User is not authorized to delete a contact.",
    "The ‘contact_delete’ permission is required to delete a contact.",
    detailTemplateId: "errors.contact.delete.unauthorized",
    detailNamedValues: [("requiredPermission", "contact_delete")]);
```

It uses:
* a template with ID: `errors.contact.delete.unauthorized`
* and named values:
  * `requiredPermission`: `contact_delete`

|   | en\_ca | fr\_ca | es | zh\_cn |
| - | ------ | ------ | -- | ------ |
| Template | `The ‘{requiredPermission}’ permission is required to delete a contact.` | `L’autorisation «{requiredPermission}» est requise pour supprimer un contact.` | `Se requiere el permiso «{requiredPermission}» para eliminar un contacto.` | `删除联系人需要“{requiredPermission}”权限。` |
| Localized message | `The ‘contact_delete’ permission is required to delete a contact.` | `L’autorisation «contact_delete» est requise pour supprimer un contact.` | `Se requiere el permiso «contact_delete» para eliminar un contacto.` | `删除联系人需要“contact_delete”权限。` |

See more: [Error](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/Error/Error.md)

## Controlling the execution flow
You can use:
* the `IsSuccess` and `IsError` properties of the `Result` (e.g., the early return `if (result.IsError) return result.Error;`),
* the `TryGetValue` and `TryGetError` methods (e.g., `if (!GetUser(id).TryGetValue(out var user, out var error)) return error;`),
* `Result.Try()` and `Result.TryAsync()` to turn an exception of code you call into a failed result (see below),
* extension methods:
  * `Then()`, `ThenAsync()`,
  * `Else()`, `ElseAsync()`,
  * `IfSuccess()`, `IfSuccessAsync()`,
  * `IfError()`, `IfErrorAsync()`,
  * `Match()`, `MatchAsync()`.

```csharp
var userResult = await _userService.GetUserAsync(userData);

var userAddedResult = await userResult.ThenAsync(async (user) =>
{
    return await _companyService.AddUserAsync(user);
});

var userTokenResult = await userAddedResult
    .ThenAsync(user => _loginService.GetUserTokenAsync(user))
    .IfErrorAsync(error => _auditService.LogErrorAsync(error));
```
or chaining multiple asynchronous operations:
```csharp
var userTokenResult = await _userService.GetUserAsync(userData)
    .ThenAsync(user => _companyService.AddUserAsync(user))
    .ThenAsync(user => _loginService.GetUserTokenAsync(user))
    .IfErrorAsync(error => _auditService.LogErrorAsync(error));
```

There is also available a query syntax for chaining multiple synchronous and asynchronous operations that return `Result<T>`:
```csharp
var userTokenResult =
    await from userData in _validationService.ValidateUserDataAsync(addUserRequest)
    from user in _userService.GetUserAsync(userData)
    from token in _loginService.GetUserTokenAsync(user)
    select token;
```

Synchronous and asynchronous `from` clauses can be mixed in one query; the query is then asynchronous and has to be awaited:
```csharp
var userTokenResult =
    await from userData in _validationService.ValidateUserData(addUserRequest)   // synchronous
    from user in _userService.GetUserAsync(userData)                             // asynchronous
    select user.Token;
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

### Turning exceptions into results
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

See more: [Try](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/Result/Result_Try.md), [TryAsync](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/Result/Result_TryAsync.md)

### Asynchronous methods in UI applications
The asynchronous extension methods that take a `Task` or `ValueTask` of a result await it with `ConfigureAwait(false)` by default, so the delegates you pass run on a thread-pool thread. That is the right choice for ASP.NET Core, console applications, Azure Functions and background services.

In WPF, Windows Forms, .NET MAUI or Blazor Server, when the delegates access UI elements, either pass `continueOnCapturedContext: true`, or set the global default once at startup (it is also used by the LINQ query syntax, which cannot pass the argument):
```csharp
ResultConfiguration.ContinueOnCapturedContext = true;
```

See more: [ResultConfiguration.ContinueOnCapturedContext](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md)

See more: [Maple.Results.Extensions](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/Extensions/namespace.md)

# Mapping to HTTP responses
Check:
* [Maple.Result.Extensions.AspNetCore](https://github.com/TomMaple/Maple.Result.Extensions.AspNetCore) library *(in development)* for mapping the *Result* to *ASP.NET Core* HTTP responses.

### Coming soon:
* *Maple.Result.Extensions.Functions.Worker* library for mapping the *Result* to *Azure Functions* HTTP responses.

# Mapping from HTTP responses
Check:
* [Maple.Result.Extensions.HttpClient](https://github.com/TomMaple/Maple.Result.Extensions.HttpClient) library for mapping *HTTP Client* responses to *Result* objects.

# Upgrading to 3.0
> [!TIP]
> See the [migration guide from 2.x to 3.0](https://github.com/TomMaple/Maple.Result/blob/main/docs/migration-v2-to-v3.md) for a checklist, before/after examples of every breaking change and a search-and-replace reference.

## New `LimitExceeded` error category
`ErrorCategory.LimitExceeded` (numeric value `10`) and the `Error.LimitExceeded(...)` factory methods have been added. Use the category when the request is valid and permitted, but a usage limit, quota or rate has been exhausted (it corresponds to the `429` HTTP status code). `Unavailable` (`503`) is now limited to a service that is unavailable.

> [!WARNING]
> **Breaking change:** exhaustive `switch` statements and expressions over `ErrorCategory` (e.g. mapping categories to HTTP status codes) must handle the new `LimitExceeded` member. The numeric values of existing members have not changed.

## New `Error.Metadata` property
`Error` has a new `Metadata` dictionary for machine-readable values (RFC 9457 *extension members*), with the `WithMetadata()`, `WithoutMetadata()` and `TryGetMetadata<T>()` methods. See [Metadata](#metadata).

* Serialized errors always contain a `Metadata` member (`"Metadata":{}` when empty).
* Metadata values can be `null`.
* `Equals()` and `GetHashCode()` of `Error` take the metadata into account.

## Immutable `Result`, `Result<T>` and `Error`, and the new `ErrorBuilder`
> [!WARNING]
> **Breaking changes.**

* **No deserialization without converters.** `Result`, `Result<T>`, `Error` and `ErrorUri` have no public constructors or setters, so *System.Text.Json* and *Newtonsoft.Json* can no longer read them from JSON: `Result`, `Result<T>` and `Error` throw, and an `ErrorUri` is silently read as `about:blank` (see the [migration guide](https://github.com/TomMaple/Maple.Result/blob/main/docs/migration-v2-to-v3.md#no-json-deserialization-without-converters)). Writing them still works with the default settings, except for a failed `Result<T>` (its `Value` throws). Reading them requires dedicated converter packages (planned).
* **`AddDetail()` has been removed.** Use `error.WithErrorDetail(...)`, which returns a copy instead of changing the error, or an `ErrorBuilder` and its `AddErrorDetail(...)` to add several details:
  ```csharp
  // before
  error.AddDetail("/email", "The email is required.");
  // after
  error = error.WithErrorDetail("/email", "The email is required.");
  ```
* **`with { … }` no longer changes an `Error`, `ErrorDetail` or `TemplatedMessage`.** All their properties are get-only. Use `ToBuilder()`:
  ```csharp
  // before
  var copy = error with { Title = "New title" };
  // after
  var copy = error.ToBuilder().WithTitle("New title").Build();
  ```
* **Public constructors removed:** `new Result()`, `new Result<T>()`, `new Error()` and `new ErrorUri(string)`. Use `Result.Success()`, `Result.FromError()`, `Result<T>.FromValue()`, `Result<T>.FromError()` (or the implicit conversions), the `Error` factory methods or `ErrorBuilder`, and `ErrorUri.None`, `Locator()`, `Tag()`, `Urn()` or `Relative()`.
* **`TryGetMetadata<T>()` is strict.** It returns `true` only when the stored value is a `T`; there is no conversion (a stored `int` is no longer read as `long`), and a `null` value returns `false`.
* **Nullable values.** Metadata values and the named values of templated messages (`TemplatedMessage.Params`, `detailNamedValues`, `namedValues`) are `object?`.
* **More checks.** The title of an error must not be `null`, empty or white-space only; the category must be a defined `ErrorCategory` value; `TemplatedMessage.TemplateId` must not be blank; `ErrorDetail.Detail` must not be `null`.
* **`Result<T>.Value` throws on a failed result** (an `InvalidOperationException` that names the error) instead of returning the default value of `T`; its type is `T`. Use `TryGetValue(out var value)` to read it without an exception. Without a converter, the default serializers can no longer write a failed `Result<T>`.
* `Result.Success()` returns the same, shared instance on every call.
* **`IsSuccess` is a property, and `IsError` is new.** Replace `result.IsSuccess()` with `result.IsSuccess`, and `!result.IsSuccess()` with `result.IsError`. The JSON written without a converter now contains both properties.
* **`continueOnCapturedContext` is `bool?`.** The parameter of the asynchronous extension methods is now `bool? continueOnCapturedContext = null`; `null` uses the new global `ResultConfiguration.ContinueOnCapturedContext` (`false` by default, so the behaviour is unchanged). Source-compatible; recompile code that calls the methods.
* **`IResult` renamed to `IOperationResult`.** The common interface of `Result` and `Result<T>` no longer clashes with `Microsoft.AspNetCore.Http.IResult` (CS0104 in ASP.NET Core projects); its members did not change.
* **`TemplatedMessage` constructors and named values.** `TemplatedMessage` is no longer a positional record: it takes the parameters as key-value pairs (e.g., a dictionary) or as tuples, and has no `Deconstruct`. Repeated or `null` parameter names throw a clear exception, and named values passed without a template id throw instead of being silently dropped. `WithErrorDetail` takes `detailTemplateId` / `detailNamedValues`, like the factory methods.

# FAQ
## When to use a Result and when an Exception?
It is up to you where to draw the line between expected failure and unexpected exception.

In some cases, a missing file is just a resource *Not Found* error (e.g., users tries to open a non-existing document); in others, it might be an unexpected exception (e.g., a DLL file or expected configuration is missing).

Sometimes, the detailed information is not required. Then, a simple enum or a boolean value can be used instead of the `Result` type.

My suggestion is:
* use **exceptions** for cases that require someone’s attention (e.g., database unavailable, connection string to a crucial resource is `null`, unexpected case that indicates a bug, expired API keys that require manual update),
* use **Result** for cases that you either want to handle in the normal program flow or return in the API (e.g., user input validation, user authentication/authorization errors, handling transient errors that will be recovered by retry, etc.),
* use **enum** or a **boolean** value if you want to handle multiple cases in the normal program flow, but you don’t need additional information (e.g., folder not exists, so it should be created; token is missing, invalid, valid or expired); most likely **NOT** for API responses.

## Why `Unauthenticated` corresponds to the `401` (Unauthorized) HTTP status code?
By the *HTTP* reference from *Mozilla* [[link](https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status)]:
> Although the HTTP standard specifies "unauthorized", semantically this response means "unauthenticated". That is, the client must authenticate itself to get the requested response.

> The HTTP **`401 Unauthorized`** [client error response](https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status#client_error_responses) status code indicates that a request was not successful because it lacks valid authentication credentials for the requested resource.

> A 401 Unauthorized is similar to the 403 Forbidden response, except that a 403 is returned when a request contains valid credentials, but the client does not have permissions to perform a certain action.

## What is the difference between a Critical Error and an Exception?
It is to a developer to draw a line between the errors that are critical and are handled in one way or another, and the ones that are unhandled and disrupt the normal program flow and bubbles up the call stack.

## Why are methods like `Bind()`, `Map()` and `Switch()` not available?
This library minimizes the amount of extension methods:
* `Bind()` and `Map()` are both `Then()`: a function that returns a value maps it (e.g., `Then<T, TNext>(Result<T>, Func<T, TNext>)`), and a function that returns a `Result<TNext>` binds it (e.g., `Then<T, TNext>(Result<T>, Func<T, Result<TNext>>)`),
* `Tap()` is `IfSuccess()`, and `TapError()` is `IfError()`,
* `Recover()` is `Else()` (e.g., `Else<TResult>(TResult, Func<Error, TResult>)`),
* `Switch()` is `Match()` with actions (e.g., `Match<T>(Result<T>, Action<T>, Action<Error>)`).

# Learn More
## Documentation
* [Maple.Results](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/namespace.md) — `Result`, `Error`, and other types
* [Maple.Results.Extensions](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/Extensions/namespace.md) — extension methods
* [Maple.Results.Extensions.ValueTasks](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/Extensions/ValueTasks/namespace.md) — `ValueTask`-based extension methods
* [Problem details format](https://github.com/TomMaple/Maple.Result/blob/main/docs/problem-details.md) — how an `Error` is written as an RFC 9457 problem details object and read back

## See also
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
* [tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)
* [RFC 1738: Uniform Resource Locators (URL)](https://datatracker.ietf.org/doc/html/rfc1738)
* [RFC 3986: Uniform Resource Identifier (URI): Generic Syntax](https://datatracker.ietf.org/doc/html/rfc3986)
* [RFC 4151: The 'tag' URI Scheme](https://datatracker.ietf.org/doc/html/rfc4151)
* [RFC 6901: JavaScript Object Notation (JSON) Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)

### Articles
* [Exception Vs Result Pattern. The Exception vs Result Pattern… | by Shreyans Padmani | Medium](https://medium.com/@shreyans_padmani/exception-vs-result-pattern-114f2e389153)
* [The Result Pattern: Simplifying Error Handling in Your Code | by Adam Hancock | Medium](https://medium.com/@dev-hancock/the-result-pattern-simplifying-error-handling-in-your-code-fc31bb50a244)
* [Working with the result pattern | by Andrew Lock](https://andrewlock.net/series/working-with-the-result-pattern/)
* [Exceptions for flow control in C# · Enterprise Craftsmanship](https://enterprisecraftsmanship.com/posts/exceptions-for-flow-control/)
* [language agnostic - Why not use exceptions as regular flow of control? - Stack Overflow](https://stackoverflow.com/questions/729379/why-not-use-exceptions-as-regular-flow-of-control)

# Contribution
Questions, ideas and contributions are welcome:
* [Discussions](https://github.com/TomMaple/Maple.Result/discussions) — questions and ideas,
* [Issues](https://github.com/TomMaple/Maple.Result/issues/new/choose) — bug reports and feature requests,
* [Contributing guide](https://github.com/TomMaple/.github/blob/main/CONTRIBUTING.md) — how to propose a change,
* [Code of Conduct](https://github.com/TomMaple/.github/blob/main/CODE_OF_CONDUCT.md),
* [Security policy](https://github.com/TomMaple/.github/blob/main/SECURITY.md) — report vulnerabilities privately, never in a public issue.
