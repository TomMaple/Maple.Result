# Error.Unavailable(ErrorUri, String, String, Nullable&lt;ErrorUri&gt;, TemplatedMessage) Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Error.cs#L736" target="_blank">Error.cs</a>

Creates a new instance of the [Error](Error.md) object of the *Unavailable* category.

```csharp
public static Error Unavailable(
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
An optional template identifier with its parameters, to generate a custom, localized detail message (e.g., `new("errors.common.userservice.unavailable", ("accountId", "D12345"))`), or `null` for none.

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record of the *Unavailable* [ErrorCategory](../ErrorCategory.md) initialized with provided parameters.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `title` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `title` or the `detail` is empty or consists only of white-space characters, or the `instanceUri` is `about:blank`.

## Remarks
This error category is used when the service (or a resource it depends on) is temporarily unavailable, e.g. it is down or overloaded. The operation can be retried later.

It relates to `503` HTTP status code (Service Unavailable), but can also be used in other contexts where a service was unavailable and needs to be reported.

Do not use it when a usage limit, quota or rate has been exhausted; use [LimitExceeded](Error_LimitExceeded.md) instead.

The template identifier and the parameters are validated by the [TemplatedMessage](../TemplatedMessage/TemplatedMessage.md) constructor (see [TemplatedMessage constructors](../TemplatedMessage/TemplatedMessage_constructors.md)), so an invalid one throws before the factory is called.

> [!NOTE]
> To add individual error details, use [WithErrorDetail](Error_WithErrorDetail.md), which returns a copy, or build the error with an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) when adding several.

## Examples
```csharp
var error = Error.Unavailable(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:common:unavailable"),
                "Cannot update the account.",
                "The UserService is unavaiable. Please retry.",
                ErrorUri.Locator("https://api.exampleapp.com/errors/546523452/details"),
                new TemplatedMessage("errors.common.userservice.unavailable", ("accountId", "D12345"), ("action", "account:update")));
```

## See Also
* [ErrorCategory](../ErrorCategory.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)

