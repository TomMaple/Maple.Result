# Errors

How to create an `Error`: its categories and properties, the type and instance URIs, error details for validation, metadata, the `ErrorBuilder` and localized messages. An `Error` follows [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457); see [Problem details format](../problem-details.md) for how it is written and read as JSON.

## Contents
* [Creating an error](#creating-an-error)
* [Error categories](#error-categories)
* [Error properties](#error-properties)
* [Type URI](#type-uri)
* [Instance URI](#instance-uri)
* [Error details (validation errors)](#error-details-validation-errors)
* [Metadata](#metadata)
* [ErrorBuilder](#errorbuilder)
* [Localization](#localization)

## Creating an error
Use the static method of the category (e.g., `Error.NotFound(…)`) to create an `Error`, and return it as a failed result:
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
        new TemplatedMessage("errors.signup.validation"))
    .WithErrorDetail("/email", "Email is required.", "errors.signup.validation.email.required")
    .WithErrorDetail("/firstName", "At least 3 characters are required.", "errors.signup.validation.firstName.minLength", ("minLength", 3));
```

`Error` is immutable: the `With…()` methods return a copy. To assemble an error step by step, use an [ErrorBuilder](#errorbuilder).

## Error categories
The error categories are based on the HTTP status codes—that’s why those have been included in the table. Mapping to HTTP responses is not part of this library, though, and can be done differently.

> [!NOTE]
> The status codes in the table are the **default** mapping of the `Maple.Result.Extensions.*` packages, which can be customized in both directions (category → status code and status code → category); see the [problem details format](../problem-details.md#category-and-status).

| Category | Description | Solution | Example error | Corresponding HTTP status code |
| -------- | ----------- | -------- | ------------- | ------------------------------ |
| **Conflict** | The operation failed due to the data or state conflict (such as a unique constraint violation). Refresh data and try again. | Refresh the data, then try again if applicable. May require repeating the whole process. | Details have been modified. Please refresh the page. | `409` (Conflict) |
| **Critical** | Unexpected, critical error. | Contact customer support. | Payment failed.&#xA;3rd party API is not available. | `500` (Internal Server Error) |
| **Failure** | An expected error that is not critical to the operation. Might be related to e.g., a business logic rules, or the data that passes the validation, but still incorrect. | Even if the data is correct, the operation is not. | Invalid file format.&#xA;Cannot delete the payment which processing has started. | `422` (Unprocessable Content) |
| **Limit Exceeded** | The request is valid and permitted, but a usage limit, quota or rate has been exhausted. Applies to business limits as well as throttling. Not for input-size checks (use **Validation**). | Wait and try again later, or raise the limit (e.g. upgrade the plan). | Too many requests—please try again in 30 seconds.&#xA;The monthly limit of 100 reports has been reached. | `429` (Too Many Requests) |
| **Not Found** | The resource was not found. | Invalid identifier. Try a different one. | Company with that name has not been found. | `404` (Not Found) |
| **Not Implemented** | The operation, feature or case is not implemented yet. | To be implemented in the future. | We don’t support debit cards yet. | `501` (Not Implemented) |
| **Permission Denied** | The authentication might be valid, but the identity does not have the required permissions. | Ask for permissions, or, a different user is required. | Action is not allowed for that user. | `403` (Forbidden) |
| **Timeout** | The operation timed out. | Heavy load. Try again later. | Server did not respond within 30 seconds. Try again later. | `504` (Gateway Timeout) when an upstream server or a dependency did not respond in time;&#xA;`408` (Request Timeout) when the client did not send the complete request in time |
| **Unavailable** | The service is not available (e.g. it is down or overloaded), but please try again later. Not for exhausted usage limits (use **Limit Exceeded**). | Some service is down. Please try again later. | Report is not available—please try again later. | `503` (Service Unavailable) |
| **Unauthenticated** | The authentication is either missing or invalid. | Log in. | User is not logged in. | `401` (Unauthorized) |
| **Validation** | User input data is invalid. | Update the data and try again. | Email is required. | `400` (Bad Request) |

See more: [ErrorCategory](../Reference/ErrorCategory.md)

## Error properties
| Property | Type | Description | Required | Example |
| -------- | ---- | ----------- | -------- | ------- |
| `Category` | `ErrorCategory` | A category of the error (one of predefined). | Yes | `ErrorCategory.Validation` |
| `TypeUri` | `ErrorUri` | A *URI* that identifies the type of the problem. When dereferenced, provides human-readable documentation about the problem. | Yes | `https://app.example.com/errors/user/invalid-email` |
| `Title` | `string` | A short, human-readable summary of the problem type. It stays the same for the same type of error. | Yes | The provided value is not a valid email. |
| `Detail` | `string` | The human-readable explanation specific to this occurrence of the problem. | No | Special character `/` is not allowed in the email address. |
| `DetailTemplated` | `TemplatedMessage` | A structure to generate a localized detail message. | No | `{ "templateId": "errors.signup.email.specialcharacters", "params": { "invalidCharacter": "/" }}` |
| `InstanceUri` | `ErrorUri?` | A *URI* that identifies the specific occurrence of the problem. | No | `https://api.exampleapp.com/errors/8783927589734857/details` |
| `ErrorDetails` | `IReadOnlyList<ErrorDetail>` | The collection of additional error details. | No | `[{ "propertyPointer": "/email", "detail": "Invalid format", "detailTemplated": { "templateId": "errors.signup.email.invalidFormat" }}]` |
| `Metadata` | `IReadOnlyDictionary<string, object?>` | Additional, machine-readable members of the problem (RFC 9457 *extension members*). | No | `{ "limit": 100, "resetAt": "2026-12-01T00:00:00Z" }` |

See more: [Error](../Reference/Error/Error.md)

## Type URI
It is a mandatory URI that identifies the type of the problem. It can be:
* `about:blank` (`ErrorUri.None`) — the type is not specified,
* *URI Locator* (`ErrorUri.Locator(…)`) — dereferencing it SHOULD provide human-readable documentation for the problem type (use an absolute rather than a relative value) (e.g., `https://exampleapp.com/docs/errors/signup/validation/email-required`),
* *URI Tag* (`ErrorUri.Tag(…)`) — it is not dereferenceable and uniquely represents the type of the problem (e.g., `tag:exampleapp.com,2026:errors.user.signup.validation.emailRequired`),
* *URN* (`ErrorUri.Urn(…)`) — a persistent, non-dereferenceable name of the type of the problem (e.g., `urn:exampleapp:errors:signup:email-required`),
* relative reference (`ErrorUri.Relative(…)`) — a full path resolved against the base URI of the response (e.g., `/errors/signup/email-required`); RFC 9457 recommends absolute URIs when possible.

When the kind is not known in advance (e.g., a type URI read from another service), `ErrorUri.Parse(…)` and `ErrorUri.TryParse(…)` detect it from the value and apply the same validation. A URI with any other scheme (e.g., `mailto:`, which RFC 9457 allows) becomes `ErrorUri.Other(…)`.

The error keeps the `ErrorUri` it was created with, so define each type once and compare an error with it; the text of the URI is its `Value`:

```csharp
public static class OrderErrors
{
    public static readonly ErrorUri NotFoundType = ErrorUri.Tag("tag:exampleapp.com,2026:errors:order:not-found");
}

if (error.TypeUri == OrderErrors.NotFoundType)
{
    logger.LogWarning("Order not found ({Type})", error.TypeUri.Value);
}
```

See more: [ErrorUri](../Reference/ErrorUri/ErrorUri.md)

## Instance URI
It is an optional URI that identifies the specific occurrence of the problem. It can be:
* *URI Locator* (`ErrorUri.Locator(…)`) — dereferencing it SHOULD provide the problem details object or information about the problem occurrence in other formats (e.g., `https://exampleapp/api/errors/982384293852/details`),
* *URI Tag* (`ErrorUri.Tag(…)`) — a unique identifier for the problem occurrence which may be meaningful to the server but is opaque to the client (e.g., `tag:exampleapp.com,2026:errors.func-sgnapi-p-01.logs.20260221.db382751-81b8-40da-8302-0a07dd16a5de`),
* *URN* (`ErrorUri.Urn(…)`) — e.g., `urn:uuid:6e8bc430-9c3a-11d9-9669-0800200c9a66`,
* relative reference (`ErrorUri.Relative(…)`) — a full path resolved against the base URI of the response (e.g., `/account/12345/msgs/abc`, as in RFC 9457).

`about:blank` (`ErrorUri.None`) is not allowed as an instance; pass `null` when there is no instance.

## Error details (validation errors)
An error can carry several error details, e.g., one per invalid field of a form. Add them with an `ErrorBuilder`:
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

where the parameters are:

| Parameter | Type | Description | Required | Example |
| --------- | ---- | ----------- | -------- | ------- |
| `propertyPointer` | `string` | JSON Pointer to the input data. | no | `/firstName` |
| `detail` | `string` | A human-readable explanation of this error detail. | yes | `Minimum 5 characters are required.` |
| `detailTemplateId` | `string` | A message template ID. | no | `errors.signup.validation.firstName.minLength` |
| `detailParameters` | `(string, object?)[]` | The parameters for the message template; names must be unique and must not differ only in case (e.g., `Min` and `min`), and values require `detailTemplateId`. | no | `("minLength", 5)` |

To add a single detail to an existing error, use `error.WithErrorDetail(…)`, which returns a copy. To remove all details, e.g., from an error read from another service before it is passed on, use `error.WithoutErrorDetails()`, which returns a copy too, or `WithoutErrorDetails()` of an `ErrorBuilder`.

The `propertyPointer` is a JSON Pointer. For a name taken from the input (a dictionary key, a form field), build it with `ErrorDetail.Pointer("items", 0, name)`: it escapes `~` and `/` in the names, so the pointer is always valid.

See more: [ErrorDetail](../Reference/ErrorDetail/ErrorDetail.md)

## Metadata
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
* Keys must not match the standard members `type`, `title`, `status`, `detail`, `instance`, `errors` and `detailTemplated` (case-insensitive), and two keys of one error must not differ only in case (e.g., `RetryAfter` and `retryAfter`); `WithMetadata()` throws an `ArgumentException` for them. Each entry becomes a top-level member of the problem details object; see the [problem details format](../problem-details.md#metadata-extension-members).
* Keys should start with a letter, use letters, digits and `_`, and be at least 3 characters long (RFC 9457 recommendation, not enforced).
* Values can be `null`. `TryGetMetadata<T>()` is strict: it returns `true` only when the stored value is a `T` (a stored `int` is not read as `long`), and never for a `null` value.

See more: [Metadata](../Reference/Error/Error_Metadata.md)

## ErrorBuilder
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

* Create a builder with `new ErrorBuilder(category, typeUri, title)`, a static method per category (e.g., `ErrorBuilder.Validation(…)`), or `error.ToBuilder()`.
* Every argument is validated when it is passed, so `Build()` never throws. `Build()` takes a snapshot; the builder stays usable.
* The builder is not thread-safe.

See more: [ErrorBuilder](../Reference/ErrorBuilder/ErrorBuilder.md)

## Localization
An error carries a message template ID and parameters next to the message, so a client can generate the message in its own language:
```csharp
var error = Error.PermissionDenied(
    ErrorUri.Tag("tag:exampleapp.com,2026:errors:contact:delete:authorization:missing"),
    "User is not authorized to delete a contact.",
    "The ‘contact_delete’ permission is required to delete a contact.",
    detailTemplated: new TemplatedMessage("errors.contact.delete.unauthorized", ("requiredPermission", "contact_delete")));
```

It uses:
* a template with ID: `errors.contact.delete.unauthorized`
* and parameters:
  * `requiredPermission`: `contact_delete`

The factory methods take the template as one [TemplatedMessage](../Reference/TemplatedMessage/TemplatedMessage.md) (`detailTemplated`), so a template ID cannot be passed where the detail goes. The parameters can be given inline, as an array or as a dictionary, and the constructor validates them. An error detail takes the template ID and the parameters directly: `WithErrorDetail("/email", "Email is required.", "errors.signup.validation.email.required")`.

|   | en\_ca | fr\_ca | es | zh\_cn |
| - | ------ | ------ | -- | ------ |
| Template | `The ‘{requiredPermission}’ permission is required to delete a contact.` | `L’autorisation «{requiredPermission}» est requise pour supprimer un contact.` | `Se requiere el permiso «{requiredPermission}» para eliminar un contacto.` | `删除联系人需要“{requiredPermission}”权限。` |
| Localized message | `The ‘contact_delete’ permission is required to delete a contact.` | `L’autorisation «contact_delete» est requise pour supprimer un contact.` | `Se requiere el permiso «contact_delete» para eliminar un contacto.` | `删除联系人需要“contact_delete”权限。` |

See more: [TemplatedMessage](../Reference/TemplatedMessage/TemplatedMessage.md)
