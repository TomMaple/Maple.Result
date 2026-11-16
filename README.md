[![NuGet Version](https://img.shields.io/nuget/v/Maple.Result?label=Maple.Result%20%E2%80%94%20nuget)](https://www.nuget.org/packages/Maple.Result/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Maple.Result)](https://www.nuget.org/packages/Maple.Result/)
[![CI](https://img.shields.io/github/actions/workflow/status/TomMaple/Maple.Result/ci.yml?branch=main&label=CI)](https://github.com/TomMaple/Maple.Result/actions/workflows/ci.yml?query=branch%3Amain)
[![GitHub last commit](https://img.shields.io/github/last-commit/TomMaple/Maple.Result/main)](https://github.com/TomMaple/Maple.Result/commits/main/)
[![License: MIT](https://img.shields.io/github/license/TomMaple/Maple.Result)](https://github.com/TomMaple/Maple.Result/blob/main/LICENSE)
[![OpenSSF Scorecard](https://api.scorecard.dev/projects/github.com/TomMaple/Maple.Result/badge)](https://scorecard.dev/viewer/?uri=github.com/TomMaple/Maple.Result)



# Result
An abstraction of the operation result which can be either successful (with or without a value) or failed (with an error).

Check also:
* [![Maple.Result.Extensions.AspNetCore](https://img.shields.io/nuget/v/Maple.Result.Extensions.AspNetCore?label=Maple.Result.Extensions.AspNetCore%20%E2%80%94%20nuget)](https://www.nuget.org/packages/Maple.Result.Extensions.AspNetCore/)
* [![Maple.Result.Extensions.MinimalApi](https://img.shields.io/nuget/v/Maple.Result.Extensions.MinimalApi?label=Maple.Result.Extensions.MinimalApi%20%E2%80%94%20nuget)](https://www.nuget.org/packages/Maple.Result.Extensions.MinimalApi/)
* [![Maple.Result.Extensions.Functions.Worker](https://img.shields.io/nuget/v/Maple.Result.Extensions.Functions.Worker?label=Maple.Result.Extensions.Functions.Worker%20%E2%80%94%20nuget)](https://www.nuget.org/packages/Maple.Result.Extensions.Functions.Worker/)
* [![Maple.Result.Extensions.Functions.Worker.AspNetCore](https://img.shields.io/nuget/v/Maple.Result.Extensions.Functions.Worker.AspNetCore?label=Maple.Result.Extensions.Functions.Worker.AspNetCore%20%E2%80%94%20nuget)](https://www.nuget.org/packages/Maple.Result.Extensions.Functions.Worker.AspNetCore/)
* [![Maple.Result.Extensions.HttpClient](https://img.shields.io/nuget/v/Maple.Result.Extensions.HttpClient?label=Maple.Result.Extensions.HttpClient%20%E2%80%94%20nuget)](https://www.nuget.org/packages/Maple.Result.Extensions.HttpClient/)
* [![Maple.Result.Extensions.Resilience](https://img.shields.io/badge/Maple.Result.Extensions.Resilience%20%E2%80%94%20nuget-coming%20soon-lightgrey)](https://github.com/TomMaple/Maple.Result.Extensions.Resilience)
* [![Maple.Result.Serialization.SystemTextJson](https://img.shields.io/badge/Maple.Result.Serialization.SystemTextJson%20%E2%80%94%20nuget-coming%20soon-lightgrey)](https://github.com/TomMaple/Maple.Result.Serialization.SystemTextJson)
* [![Maple.Result.Serialization.NewtonsoftJson](https://img.shields.io/badge/Maple.Result.Serialization.NewtonsoftJson%20%E2%80%94%20nuget-coming%20soon-lightgrey)](https://github.com/TomMaple/Maple.Result.Serialization.NewtonsoftJson)

> [!IMPORTANT]
> **Version 3.0 is a major release with breaking changes**, and this README describes it. `Result`, `Result<T>` and `Error` are immutable (with a new `ErrorBuilder`), the namespace is `Maple.Results`, the transforming `IfSuccess` / `IfError` are `Then` / `Else`, and the types can no longer be read from JSON without the converter packages. Read the [migration guide from 2.x to 3.0](https://github.com/TomMaple/Maple.Result/blob/main/docs/migration-v2-to-v3.md) before you upgrade, and upgrade the [related packages](https://github.com/TomMaple/Maple.Result#related-packages) together with `Maple.Result`.

It can be mapped to and from HTTP responses, and written to and read from JSON, by the [related packages](https://github.com/TomMaple/Maple.Result#related-packages): *ASP.NET Core*, *Minimal API*, *Azure Functions* and `HttpClient` are published; resilience and the JSON converters are coming soon.


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
* [Related packages](https://github.com/TomMaple/Maple.Result#related-packages)
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

# Related packages
The core package (`Maple.Result`) has no dependencies. These packages add the mapping to and from HTTP responses, resilience and JSON serialization:

| Package | Purpose | Status |
| ------- | ------- | ------ |
| [Maple.Result.Extensions.AspNetCore](https://github.com/TomMaple/Maple.Result.Extensions.AspNetCore) | Maps a *Result* to an *ASP.NET Core* HTTP response (`ActionResult`, problem details). | Published ([0.15.0](https://www.nuget.org/packages/Maple.Result.Extensions.AspNetCore/0.15.0), for `Maple.Result` 2.x); a version for 3.0 is released with it |
| [Maple.Result.Extensions.MinimalApi](https://github.com/TomMaple/Maple.Result.Extensions.MinimalApi) | Maps a *Result* to a *.NET Minimal API* result (`IResult`, problem details). | Published ([0.4.0](https://www.nuget.org/packages/Maple.Result.Extensions.MinimalApi/0.4.0), for `Maple.Result` 2.x); a version for 3.0 is released with it |
| [Maple.Result.Extensions.Functions.Worker](https://github.com/TomMaple/Maple.Result.Extensions.Functions.Worker) | Maps a *Result* to an *Azure Functions* (isolated worker model) HTTP response (`HttpResponseData`). | Published ([0.2.0](https://www.nuget.org/packages/Maple.Result.Extensions.Functions.Worker/0.2.0), for `Maple.Result` 2.x); a version for 3.0 is released with it |
| [Maple.Result.Extensions.Functions.Worker.AspNetCore](https://github.com/TomMaple/Maple.Result.Extensions.AspNetCore) | Maps a *Result* to an `ActionResult` in *Azure Functions* (isolated worker model) with the *ASP.NET Core* integration. | Published ([0.15.0](https://www.nuget.org/packages/Maple.Result.Extensions.Functions.Worker.AspNetCore/0.15.0), for `Maple.Result` 2.x); a version for 3.0 is released with it |
| [Maple.Result.Extensions.HttpClient](https://github.com/TomMaple/Maple.Result.Extensions.HttpClient) | Maps an `HttpClient` response (`HttpResponseMessage`, problem details) to a *Result*. | Published ([0.12.0](https://www.nuget.org/packages/Maple.Result.Extensions.HttpClient/0.12.0), for `Maple.Result` 2.x); a version for 3.0 is released with it |
| [Maple.Result.Extensions.Resilience](https://github.com/TomMaple/Maple.Result.Extensions.Resilience) | The retry-after metadata of an `Error` (e.g., for `LimitExceeded`) and helpers for the retry pipeline of an application (no dependency on *Polly*). | **Coming soon** (not on NuGet yet) |
| [Maple.Result.Serialization.SystemTextJson](https://github.com/TomMaple/Maple.Result.Serialization.SystemTextJson) | *System.Text.Json* converters that write and read `Result`, `Result<T>` and `Error`. | **Coming soon** (not on NuGet yet) |
| [Maple.Result.Serialization.NewtonsoftJson](https://github.com/TomMaple/Maple.Result.Serialization.NewtonsoftJson) | *Newtonsoft.Json* (Json.NET) converters that write and read `Result`, `Result<T>` and `Error`. | **Coming soon** (not on NuGet yet) |

> [!WARNING]
> The published versions above are built for `Maple.Result` 2.x: do not mix them with `Maple.Result` 3.0 (e.g., the current `Maple.Result.Extensions.HttpClient` fails at runtime with 3.0). Upgrade all packages together, to the versions built for 3.0 (see [Extension packages](https://github.com/TomMaple/Maple.Result/blob/main/docs/migration-v2-to-v3.md#extension-packages) in the migration guide).

The problem details that these packages write and read follow one format, described in [Problem details format](https://github.com/TomMaple/Maple.Result/blob/main/docs/problem-details.md).

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

* **No deserialization without converters.** `Result`, `Result<T>`, `Error` and `ErrorUri` have no public constructors or setters, so *System.Text.Json* and *Newtonsoft.Json* can no longer read them from JSON: `Result`, `Result<T>` and `Error` throw, and an `ErrorUri` is silently read as `about:blank` (see the [migration guide](https://github.com/TomMaple/Maple.Result/blob/main/docs/migration-v2-to-v3.md#no-json-deserialization-without-converters)). Writing them still works with the default settings, except for a failed `Result<T>` (its `Value` throws). Reading them requires the converter packages [Maple.Result.Serialization.SystemTextJson](https://github.com/TomMaple/Maple.Result.Serialization.SystemTextJson) and [Maple.Result.Serialization.NewtonsoftJson](https://github.com/TomMaple/Maple.Result.Serialization.NewtonsoftJson) (coming soon).
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
## Why `Unauthenticated` corresponds to the `401` (Unauthorized) HTTP status code?
By the *HTTP* reference from *Mozilla* [[link](https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status)]:
> Although the HTTP standard specifies "unauthorized", semantically this response means "unauthenticated". That is, the client must authenticate itself to get the requested response.

> The HTTP **`401 Unauthorized`** [client error response](https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status#client_error_responses) status code indicates that a request was not successful because it lacks valid authentication credentials for the requested resource.

> A 401 Unauthorized is similar to the 403 Forbidden response, except that a 403 is returned when a request contains valid credentials, but the client does not have permissions to perform a certain action.

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

# Contribution
Questions, ideas and contributions are welcome:
* [Discussions](https://github.com/TomMaple/Maple.Result/discussions) — questions and ideas,
* [Issues](https://github.com/TomMaple/Maple.Result/issues/new/choose) — bug reports and feature requests,
* [Contributing guide](https://github.com/TomMaple/.github/blob/main/CONTRIBUTING.md) — how to propose a change,
* [Code of Conduct](https://github.com/TomMaple/.github/blob/main/CODE_OF_CONDUCT.md),
* [Security policy](https://github.com/TomMaple/.github/blob/main/SECURITY.md) — report vulnerabilities privately, never in a public issue.
