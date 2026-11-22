# Error.Validation(ErrorUri, String, String, Nullable&lt;ErrorUri&gt;, TemplatedMessage) Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Error.cs#L750" target="_blank">Error.cs</a>

Creates a new instance of the [Error](Error.md) object of the *Validation* category.

```csharp
public static Error Validation(
        ErrorUri typeUri,
        string title,
        string? detail = null,
        ErrorUri? instanceUri = null,
        TemplatedMessage? detailTemplated = null);
```

### Parameters
#### `typeUri` [ErrorUri](../ErrorUri/ErrorUri.md)
A mandatory record containing a *URI* reference that identifies the type of the problem.

#### `title` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory short, human-readable summary of the problem.

#### `detail` [String](https://learn.microsoft.com/dotnet/api/system.string)
An optional, human-readable explanation specific to this occurrence of the problem.

#### `instanceUri` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[ErrorUri](../ErrorUri/ErrorUri.md)&gt;
An optional *URI* reference that identifies the specific occurrence of the problem. It must not be `about:blank` ([ErrorUri.None](../ErrorUri/ErrorUri_None.md) or a default [ErrorUri](../ErrorUri/ErrorUri.md)); pass `null` when there is no instance.

#### `detailTemplated` [TemplatedMessage](../TemplatedMessage/TemplatedMessage.md)
An optional template identifier with its parameters, to generate a custom, localized detail message (e.g., `new("errors.user.create.invalid_data", ("entity", "user"))`), or `null` for none.

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record of the *Validation* [ErrorCategory](../ErrorCategory.md) initialized with provided parameters.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `title` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `title` or the `detail` is empty or consists only of white-space characters, or the `instanceUri` is `about:blank`.

## Remarks
This error category represents validation errors that prevent the underlying service from completing.

It relates to `400` HTTP status code (Bad Request), but can also be used in other contexts where validation errors need to be reported.

The template identifier and the parameters are validated by the [TemplatedMessage](../TemplatedMessage/TemplatedMessage.md) constructor (see [TemplatedMessage constructors](../TemplatedMessage/TemplatedMessage_constructors.md)), so an invalid one throws before the factory is called.

> [!NOTE]
> To add individual error details, use [WithErrorDetail](Error_WithErrorDetail.md), which returns a copy, or build the error with an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) when adding several.

## Examples
```csharp
var error = Error.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid_data"),
                "Invalid data to register a new user.",
                "The provided user details are invalid. Please update and retry.",
                ErrorUri.Locator("https://api.exampleapp.com/errors/7983750923784025/details"),
                new TemplatedMessage("errors.user.create.invalid_data", ("entity", "user"), ("canRetry", true)));

error = error.WithErrorDetail(
    "/username",
    "The username needs to be at least 3 characters long.",
    "error:user:create:username-length",
    ("username", "j"), ("minlength", 3));

error = error.WithErrorDetail(
    "/email",
    "The email address is required.",
    "error:user:create:email-required",
    ("email", ""));
```

## See Also
* [ErrorCategory](../ErrorCategory.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
* [JSON Pointer](https://datatracker.ietf.org/doc/html/rfc6901)

