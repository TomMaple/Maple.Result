[![NuGet Version](https://img.shields.io/nuget/v/Maple.Result?label=Maple.Result%20%E2%80%94%20nuget)](https://www.nuget.org/packages/Maple.Result/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Maple.Result)](https://www.nuget.org/packages/Maple.Result/)
[![CI](https://img.shields.io/github/actions/workflow/status/TomMaple/Maple.Result/ci.yml?branch=main&label=CI)](https://github.com/TomMaple/Maple.Result/actions/workflows/ci.yml?query=branch%3Amain)
[![GitHub last commit](https://img.shields.io/github/last-commit/TomMaple/Maple.Result/main)](https://github.com/TomMaple/Maple.Result/commits/main/)
[![License: MIT](https://img.shields.io/github/license/TomMaple/Maple.Result)](https://github.com/TomMaple/Maple.Result/blob/main/LICENSE)
[![OpenSSF Scorecard](https://api.scorecard.dev/projects/github.com/TomMaple/Maple.Result/badge)](https://scorecard.dev/viewer/?uri=github.com/TomMaple/Maple.Result)

# Maple.Result
The *Result* pattern for .NET: an operation returns a `Result` or a `Result<T>`, which is either successful (with or without a value) or failed (with an `Error` that follows [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)).

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
> **Version 3.0 is a major release with breaking changes**, and this README describes it. `Result`, `Result<T>` and `Error` are immutable (with a new `ErrorBuilder`), the namespace is `Maple.Results`, the transforming `IfSuccess` / `IfError` are `Then` / `Else`, and the types can no longer be read from JSON without the converter packages. Read the [migration guide from 2.x to 3.0](docs/migration-v2-to-v3.md) before you upgrade, and upgrade the [related packages](#related-packages) together with `Maple.Result`.

## Contents
* [Why this library?](#why-this-library)
* [Installation](#installation)
* [Quick start](#quick-start)
* [Features](#features)
* [Related packages](#related-packages)
* [Upgrading to 3.0](#upgrading-to-30)
* [FAQ](#faq)
* [Documentation](#documentation)
* [Give it a star ⭐](#give-it-a-star-)
* [Contributing](#contributing)
* [License](#license)

## Why this library?
The *Result* pattern returns the outcome of an operation—either successful or failed—in a structured and precise way:
* No guessing what the `null` means!
* Clear way of indicating whether the operation is successful!
* No need for misusing exceptions for not *exceptional* cases or controlling the flow!

There are many implementations of the *Result* pattern in C#. This one adds a few things that are missing in others:
* consistent error categorization (e.g., `Validation`, `NotFound`, `Conflict`), with a default HTTP status code for each category,
* precise error description: a type *URI*, a title, a detail, an instance *URI*, error details (e.g., one per invalid field) and machine-readable metadata,
* following standards ([RFC 9457](https://datatracker.ietf.org/doc/html/rfc9457)),
* support for localization (a message template and named values next to each message),
* Native AOT and trimming compatibility (checked by the trim and AOT analyzers in every build), and no dependencies.

See [The Result pattern](docs/guide/result-pattern.md) for its benefits, a comparison with exceptions and when to use which.

## Installation
Add the `Maple.Result` package to your project using the Package Manager in your IDE or the *dotnet* tool in the console:

```shellscript
dotnet add package Maple.Result
```

The types are in the `Maple.Results` namespace, and the extension methods in `Maple.Results.Extensions`:

```csharp
using Maple.Results;
using Maple.Results.Extensions;  // extension methods: Then, Else, IfSuccess, Match, Switch, LINQ, …
```

## Quick start
Return a `Result<T>` (or a `Result`, when there is no value) instead of throwing an exception for an expected failure. A value and an `Error` convert to it implicitly:
```csharp
public async Task<Result<Contact>> GetContactAsync(Guid id)
{
    if (id == Guid.Empty)
    {
        return Error.Validation(
            ErrorUri.Tag("tag:exampleapp.com,2026:errors:contact:id:invalid"),
            "Invalid contact ID.",
            "The contact ID must not be empty.");
    }

    var contact = await _contactRepository.GetByIdAsync(id);
    if (contact is null)
    {
        return Error.NotFound(
            ErrorUri.Tag("tag:exampleapp.com,2026:errors:contact:notfound"),
            "The contact has not been found.");
    }

    return contact;
}
```

Read the value, or return the error early:
```csharp
var contactResult = await GetContactAsync(id);
if (!contactResult.TryGetValue(out var contact, out var error))
{
    return error;
}
```

Handle both cases:
```csharp
string greeting = contactResult.Match(
    contact => $"Hello, {contact.Name}!",
    error => error.Title);
```

Or chain the next steps, which run only on success:
```csharp
Result sendResult = await GetContactAsync(id)
    .ThenAsync(contact => _emailService.SendWelcomeAsync(contact))     // returns a Task<Result>
    .IfErrorAsync(error => _logger.LogWarning("{Title}", error.Title));
```

## Features
* **[Results](docs/guide/results.md)** — `Result` and `Result<T>`, the implicit conversions, `TryGetValue` and `TryGetError`, and nullable values.
* **[Errors](docs/guide/errors.md)** — eleven [categories](docs/guide/errors.md#error-categories) (with their default HTTP status codes), type and instance *URI*s, [error details](docs/guide/errors.md#error-details-validation-errors) for validation errors (with JSON Pointers), [metadata](docs/guide/errors.md#metadata), the [ErrorBuilder](docs/guide/errors.md#errorbuilder) and [localization](docs/guide/errors.md#localization).
* **[Chaining operations](docs/guide/chaining.md)** — `Then`, `Else`, `IfSuccess`, `IfError`, `Match`, `Switch`, their asynchronous versions for `Task` and `ValueTask`, and the LINQ query syntax.
* **[Turning exceptions into results](docs/guide/chaining.md#turning-exceptions-into-results)** — `Result.Try()` and `Result.TryAsync()` with a mapper from the exception to the `Error`.
* **[Analyzer](docs/Analyzers/README.md)** — six rules in the package, with nothing to install, report the calls that compile but lose a task or a failure: e.g., a task that nobody awaits in `Then()`, or a statement that discards a `Result`. Most of them have a code fix.
* **[Problem details format](docs/problem-details.md)** — how an `Error` is written as an RFC 9457 problem details object and read back by the [related packages](#related-packages).
* **[UI applications](docs/guide/chaining.md#asynchronous-methods-in-ui-applications)** — `continueOnCapturedContext` and a global default for WPF, Windows Forms, .NET MAUI and Blazor Server.

## Related packages
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
> The published versions above are built for `Maple.Result` 2.x: do not mix them with `Maple.Result` 3.0 (e.g., the current `Maple.Result.Extensions.HttpClient` fails at runtime with 3.0). Upgrade all packages together, to the versions built for 3.0 (see [Extension packages](docs/migration-v2-to-v3.md#extension-packages) in the migration guide).

The problem details that these packages write and read follow one format, described in [Problem details format](docs/problem-details.md).

## Upgrading to 3.0
Version 3.0 has breaking changes. The main ones:
* the namespace is `Maple.Results` (it was `Maple.Result`),
* `Result`, `Result<T>` and `Error` are immutable: they have no public constructors or setters, `with { … }` no longer changes an error, and `AddDetail()` is replaced by `WithErrorDetail()` and the new `ErrorBuilder`,
* the transforming `IfSuccess()` and `IfError()` are `Then()` and `Else()`, and the analyzer of the package reports most of the calls that changed meaning,
* `IsSuccess` is a property, `IsError` is new, and `Result<T>.Value` throws on a failed result,
* `IResult` is renamed to `IOperationResult`,
* the types can no longer be read from JSON without the converter packages,
* exhaustive `switch` statements over `ErrorCategory` must handle the new `LimitExceeded` category.

The [migration guide from 2.x to 3.0](docs/migration-v2-to-v3.md) has a checklist, before/after examples of every breaking change, the new features and a search-and-replace reference.

## FAQ
### When to use a Result and when an exception?
Use a *Result* for expected failures that you handle in the normal program flow or return in an API (e.g., validation, a missing resource, authorization), and exceptions for cases that require someone’s attention (e.g., a bug, an unavailable database, an expired API key). See [When to use a Result and when an exception?](docs/guide/result-pattern.md#when-to-use-a-result-and-when-an-exception)

### Why does `Unauthenticated` correspond to the `401` (Unauthorized) HTTP status code?
By the *HTTP* reference from *Mozilla* [[link](https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status)]:
> Although the HTTP standard specifies "unauthorized", semantically this response means "unauthenticated". That is, the client must authenticate itself to get the requested response.

> The HTTP **`401 Unauthorized`** [client error response](https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status#client_error_responses) status code indicates that a request was not successful because it lacks valid authentication credentials for the requested resource.

> A 401 Unauthorized is similar to the 403 Forbidden response, except that a 403 is returned when a request contains valid credentials, but the client does not have permissions to perform a certain action.

The `403` (Forbidden) status code corresponds to `PermissionDenied`: the identity is known, but it does not have the permission. The library does not use the name "Unauthorized", which means the other case in HTTP.

### Why are methods like `Bind()`, `Map()` and `Tap()` not available?
This library minimizes the amount of extension methods:
* `Bind()` and `Map()` are both `Then()`: a function that returns a value maps it (e.g., `Then<T, TNext>(Result<T>, Func<T, TNext>)`), and a function that returns a `Result<TNext>` binds it (e.g., `Then<T, TNext>(Result<T>, Func<T, Result<TNext>>)`),
* `Tap()` is `IfSuccess()`, and `TapError()` is `IfError()`,
* `Recover()` is `Else()` (e.g., `Else<TResult>(TResult, Func<Error, TResult>)`),
* `Fold()` is `Match()`: two functions that return one type (e.g., `Match<T, TOut>(Result<T>, Func<T, TOut>, Func<Error, TOut>)`); `Switch()` runs two actions instead.

## Documentation
### Guides
* [Results](docs/guide/results.md) — returning and reading a `Result` and a `Result<T>`
* [Errors](docs/guide/errors.md) — categories, properties, *URI*s, error details, metadata, `ErrorBuilder` and localization
* [Chaining operations](docs/guide/chaining.md) — extension methods, asynchronous operations, the query syntax and `Result.Try()`
* [Analyzer](docs/Analyzers/README.md) — the rules MR0001 to MR0006, their code fixes and their configuration
* [The Result pattern](docs/guide/result-pattern.md) — benefits, a comparison with exceptions, and articles
* [Problem details format](docs/problem-details.md) — how an `Error` is written as an RFC 9457 problem details object and read back
* [Migration guide from 2.x to 3.0](docs/migration-v2-to-v3.md)

### Reference
* [Maple.Results](docs/Reference/namespace.md) — `Result`, `Error`, and other types
* [Maple.Results.Extensions](docs/Reference/Extensions/namespace.md) — extension methods, for `Task` and `ValueTask` alike

### See also
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
* [tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)
* [RFC 1738: Uniform Resource Locators (URL)](https://datatracker.ietf.org/doc/html/rfc1738)
* [RFC 3986: Uniform Resource Identifier (URI): Generic Syntax](https://datatracker.ietf.org/doc/html/rfc3986)
* [RFC 4151: The 'tag' URI Scheme](https://datatracker.ietf.org/doc/html/rfc4151)
* [RFC 6901: JavaScript Object Notation (JSON) Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)

## Give it a star ⭐
Do you like it? Show your support by giving this project a star!

## Contributing
Questions, ideas and contributions are welcome:
* [Discussions](https://github.com/TomMaple/Maple.Result/discussions) — questions and ideas,
* [Issues](https://github.com/TomMaple/Maple.Result/issues/new/choose) — bug reports and feature requests,
* [Contributing guide](https://github.com/TomMaple/.github/blob/main/CONTRIBUTING.md) — how to propose a change,
* [Code of Conduct](https://github.com/TomMaple/.github/blob/main/CODE_OF_CONDUCT.md),
* [Security policy](https://github.com/TomMaple/.github/blob/main/SECURITY.md) — report vulnerabilities privately, never in a public issue.

## License
Maple.Result is licensed under the [MIT License](LICENSE).
