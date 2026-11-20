# Maple.Result
The *Result* pattern for .NET: an operation returns a `Result` or a `Result<T>`, which is either successful (with or without a value) or failed (with an `Error` that follows [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)).

* Consistent error categories (e.g., `Validation`, `NotFound`, `Conflict`), with a default HTTP status code for each category.
* Precise errors: a type *URI*, a title, a detail, an instance *URI*, error details (e.g., one per invalid field) and machine-readable metadata.
* Localization: a message template and named values next to each message.
* Chaining with `Then`, `Else`, `IfSuccess`, `IfError`, `Match`, `Switch`, their asynchronous versions for `Task` and `ValueTask`, and the LINQ query syntax.
* An [analyzer](https://github.com/TomMaple/Maple.Result/blob/main/docs/Analyzers/README.md) in the package, with nothing to install: it reports the calls that compile but lose a task or a failure (e.g., a task that nobody awaits in `Then`, or a statement that discards a `Result`).
* Native AOT and trimming compatible, with no dependencies.

**Version 3.0 is a major release with breaking changes.** Read the [migration guide from 2.x to 3.0](https://github.com/TomMaple/Maple.Result/blob/main/docs/migration-v2-to-v3.md) before you upgrade, and upgrade the [related packages](https://github.com/TomMaple/Maple.Result#related-packages) together with `Maple.Result`: their published versions are built for 2.x.

## Getting started
```shellscript
dotnet add package Maple.Result
```

```csharp
using Maple.Results;
using Maple.Results.Extensions;  // extension methods: Then, Else, IfSuccess, Match, Switch, LINQ, …
```

## Usage
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

## Additional documentation
* [Results](https://github.com/TomMaple/Maple.Result/blob/main/docs/guide/results.md) — returning and reading a `Result` and a `Result<T>`
* [Errors](https://github.com/TomMaple/Maple.Result/blob/main/docs/guide/errors.md) — categories, properties, *URI*s, error details, metadata, `ErrorBuilder` and localization
* [Chaining operations](https://github.com/TomMaple/Maple.Result/blob/main/docs/guide/chaining.md) — extension methods, asynchronous operations, the query syntax and `Result.Try()`
* [Analyzer](https://github.com/TomMaple/Maple.Result/blob/main/docs/Analyzers/README.md) — the rules MR0001 to MR0006, their code fixes and their configuration
* [The Result pattern](https://github.com/TomMaple/Maple.Result/blob/main/docs/guide/result-pattern.md) — benefits, a comparison with exceptions, and articles
* [Problem details format](https://github.com/TomMaple/Maple.Result/blob/main/docs/problem-details.md) — how an `Error` is written as an RFC 9457 problem details object and read back
* [Reference](https://github.com/TomMaple/Maple.Result/blob/main/docs/Reference/namespace.md) — every type and member

## Related packages
* [Maple.Result.Extensions.AspNetCore](https://www.nuget.org/packages/Maple.Result.Extensions.AspNetCore/) — maps a *Result* to an *ASP.NET Core* HTTP response
* [Maple.Result.Extensions.MinimalApi](https://www.nuget.org/packages/Maple.Result.Extensions.MinimalApi/) — maps a *Result* to a *.NET Minimal API* result
* [Maple.Result.Extensions.Functions.Worker](https://www.nuget.org/packages/Maple.Result.Extensions.Functions.Worker/) — maps a *Result* to an *Azure Functions* (isolated worker model) HTTP response
* [Maple.Result.Extensions.Functions.Worker.AspNetCore](https://www.nuget.org/packages/Maple.Result.Extensions.Functions.Worker.AspNetCore/) — maps a *Result* to an `ActionResult` in *Azure Functions* with the *ASP.NET Core* integration
* [Maple.Result.Extensions.HttpClient](https://www.nuget.org/packages/Maple.Result.Extensions.HttpClient/) — maps an `HttpClient` response to a *Result*
* Coming soon: [Maple.Result.Extensions.Resilience](https://github.com/TomMaple/Maple.Result.Extensions.Resilience), [Maple.Result.Serialization.SystemTextJson](https://github.com/TomMaple/Maple.Result.Serialization.SystemTextJson) and [Maple.Result.Serialization.NewtonsoftJson](https://github.com/TomMaple/Maple.Result.Serialization.NewtonsoftJson)

See [Related packages](https://github.com/TomMaple/Maple.Result#related-packages) for the status of each package.

## Feedback
* [Discussions](https://github.com/TomMaple/Maple.Result/discussions) — questions and ideas
* [Issues](https://github.com/TomMaple/Maple.Result/issues/new/choose) — bug reports and feature requests
* [Contributing guide](https://github.com/TomMaple/.github/blob/main/CONTRIBUTING.md)

Licensed under the [MIT License](https://github.com/TomMaple/Maple.Result/blob/main/LICENSE).
