# Migrating from Maple.Result 2.x to 3.0

This guide lists the changes between the last 2.x release (`2.8.12`) and `3.0`, explains why they were made, and shows how to update your code.

Version 3.0 makes `Result`, `Result<T>` and `Error` **immutable** and **always valid**: an invalid instance can no longer be created, and an instance cannot change after it was created. It also adds the `LimitExceeded` error category, error metadata and an `ErrorBuilder`.

## Contents
* [Quick checklist](#quick-checklist)
* [Breaking changes](#breaking-changes)
  * [Namespace renamed to `Maple.Results`](#namespace-renamed-to-mapleresults)
  * [New `LimitExceeded` error category](#new-limitexceeded-error-category)
  * [Public constructors removed](#public-constructors-removed)
  * [`AddDetail()` removed](#adddetail-removed)
  * [`with` expressions no longer change errors](#with-expressions-no-longer-change-errors)
  * [No JSON deserialization without converters](#no-json-deserialization-without-converters)
  * [Changed JSON output](#changed-json-output)
  * [Stricter validation](#stricter-validation)
  * [Nullable named values](#nullable-named-values)
  * [`TemplatedMessage` constructors](#templatedmessage-constructors)
  * [Other behaviour changes](#other-behaviour-changes)
  * [`continueOnCapturedContext` is `bool?`](#continueoncapturedcontext-is-bool)
  * [`IResult` renamed to `IOperationResult`](#iresult-renamed-to-ioperationresult)
  * [`IsSuccess` is a property; new `IsError`](#issuccess-is-a-property-new-iserror)
  * [`ErrorUri.None()` is a property](#errorurinone-is-a-property)
  * [`Result<T>.Value` throws on a failed result](#resulttvalue-throws-on-a-failed-result)
  * [Transforming `IfSuccess` and `IfError` renamed to `Then` and `Else`](#transforming-ifsuccess-and-iferror-renamed-to-then-and-else)
  * [`Then` and `Match` with an `Error` or a task](#then-and-match-with-an-error-or-a-task)
  * [`IfError` and `Match` with an error action that returns a task](#iferror-and-match-with-an-error-action-that-returns-a-task)
  * [LINQ queries that select a task](#linq-queries-that-select-a-task)
  * [LINQ queries that select a result](#linq-queries-that-select-a-result)
  * [Null results and null arguments of the extension methods](#null-results-and-null-arguments-of-the-extension-methods)
  * [Extension packages](#extension-packages)
* [New features](#new-features)
* [Search-and-replace reference](#search-and-replace-reference)

## Quick checklist
1. Replace `using Maple.Result…;` with `using Maple.Results…;` (see [Namespace renamed to `Maple.Results`](#namespace-renamed-to-mapleresults)).
2. Handle `ErrorCategory.LimitExceeded` in every exhaustive `switch` over `ErrorCategory`.
3. Replace `error.AddDetail(...)` with `error = error.WithErrorDetail(...)` or an `ErrorBuilder`.
4. Replace `error with { … }` with `error.ToBuilder()…Build()`.
5. Replace `new Result()`, `new Result<T>()`, `new Error()` and `new ErrorUri(...)` with the factory methods.
6. If you **read** `Result`, `Result<T>` or `Error` from JSON (e.g., Azure Durable Functions, message queues, caches, HTTP clients), see [No JSON deserialization without converters](#no-json-deserialization-without-converters).
7. If you compare or snapshot the **written** JSON, see [Changed JSON output](#changed-json-output).
8. Make sure error titles are never `null` or blank, detail texts are `null` or not blank, metadata keys, template ids and template parameter names do not start or end with white space, and the pointers of error details are JSON Pointers (`/email`, not `Email`), and pass `null` instead of `ErrorUri.None()` as an instance *URI* (see [Stricter validation](#stricter-validation)).
9. Rename the calls of `IfSuccess`, `IfSuccessAsync`, `IfError` and `IfErrorAsync` that transform or replace the result to `Then`, `ThenAsync`, `Else` and `ElseAsync`; the compiler reports each of them (see [Transforming `IfSuccess` and `IfError` renamed to `Then` and `Else`](#transforming-ifsuccess-and-iferror-renamed-to-then-and-else)).
10. Fix the calls of `Then`, `Match` and the LINQ queries that no longer compile or that now return a failed result (see [`Then` and `Match` with an `Error` or a task](#then-and-match-with-an-error-or-a-task)).
11. Replace the error actions of `IfError` and `Match` that return a task with the asynchronous methods (see [`IfError` and `Match` with an error action that returns a task](#iferror-and-match-with-an-error-action-that-returns-a-task)).
12. Replace the LINQ queries whose `select` returns a task with an asynchronous `from` clause or `ThenAsync` (see [LINQ queries that select a task](#linq-queries-that-select-a-task)), and check the queries whose `select` returns a result (see [LINQ queries that select a result](#linq-queries-that-select-a-result)).
13. Make sure the functions passed to the extension methods never return `null` where a result, an `Error` or a task is expected (see [Null results and null arguments of the extension methods](#null-results-and-null-arguments-of-the-extension-methods)).
14. Upgrade the `Maple.Result.Extensions.*` packages together with `Maple.Result` (see [Extension packages](#extension-packages)).
15. Replace `result.IsSuccess()` with `result.IsSuccess`, and `!result.IsSuccess()` with `result.IsError` (see [`IsSuccess` is a property; new `IsError`](#issuccess-is-a-property-new-iserror)).
16. Make sure `Result<T>.Value` is read only on a successful result, or use `TryGetValue(out var value)`; and do not write a failed `Result<T>` with the default serializers (see [`Result<T>.Value` throws on a failed result](#resulttvalue-throws-on-a-failed-result)).
17. Replace `ErrorUri.None()` with `ErrorUri.None` (see [`ErrorUri.None()` is a property](#errorurinone-is-a-property)).

## Breaking changes
### Namespace renamed to `Maple.Results`
The namespaces are now `Maple.Results`, `Maple.Results.Extensions` and `Maple.Results.Extensions.ValueTasks`. The package ID (`Maple.Result`) and the assembly name (`Maple.Result.dll`) did not change.

```csharp
// before
using Maple.Result;
using Maple.Result.Extensions;

// after
using Maple.Results;
using Maple.Results.Extensions;
```

The old namespace hid the type: inside any `Maple.*` namespace other than `Maple.Result.*` (an application or a library named `Maple.Something`), `Result` resolved to the namespace, so `Result.Success()`, `Result.FromValue(…)` and `Result.FromError(…)` did not compile (CS0234). A plural namespace with a singular type is the usual way out (`System.Threading.Tasks` and `Task`).

> [!NOTE]
> Inside any `Maple.*` namespace, the name `Results` now resolves to the namespace `Maple.Results`, so `Results.Ok()` of ASP.NET Core (Minimal APIs) does not compile there (CS0234). Use `TypedResults.Ok()` or the fully qualified `Microsoft.AspNetCore.Http.Results.Ok()`. Code in other namespaces is not affected.

If you referenced a type by its full name (e.g., `Maple.Result.Error` in a `using` alias, in reflection or in a serialized type name such as `$type`), update it to `Maple.Results.…`.

### New `LimitExceeded` error category
`ErrorCategory.LimitExceeded` (numeric value `10`) and the `Error.LimitExceeded(...)` factory methods have been added. Use the category when the request is valid and permitted, but a usage limit, quota or rate has been exhausted (HTTP `429`). `Unavailable` (HTTP `503`) now means only that a service is unavailable.

The numeric values of the existing members have not changed, but exhaustive `switch` statements and expressions must handle the new member:

```csharp
var statusCode = error.Category switch
{
    ErrorCategory.Validation => 400,
    // ...
    ErrorCategory.Unavailable => 503,
    ErrorCategory.LimitExceeded => 429, // new
    _ => 500
};
```

The default mapping of the status codes that are **read** (e.g., by the `HttpClient` package) changed as well: in 2.x, every code without a category of its own was read as `Failure`. In 3.0, `502` is read as `Unavailable`, any other `5xx` code as `Critical`, `410` as `NotFound`, `412` and `423` as `Conflict`, and `413`, `414` and `431` as `Validation`; only the other `4xx` codes are still read as `Failure`. See [Category and status](problem-details.md#category-and-status).

`default(ErrorCategory)` is still `Validation` (`0`), and the values did not change. Do not use the default value to mean "not set".

### Public constructors removed
The types can only be created through their factory methods, which always produce a valid instance.

| 2.x | 3.0 |
| --- | --- |
| `new Result()` | `Result.Success()` |
| `new Result { Error = error }` | `Result.FromError(error)` or the implicit conversion from `Error` |
| `new Result<T>()` | — (a result is always either successful or failed) |
| `new Result<T> { Value = value }` | `Result<T>.FromValue(value)`, `Result.FromValue(value)` or the implicit conversion from `T` |
| `new Result<T> { Error = error }` | `Result<T>.FromError(error)` or the implicit conversion from `Error` |
| `new Error()` / `new Error { … }` | `Error.Validation(...)`, `Error.NotFound(...)`, … or `ErrorBuilder` |
| `new ErrorUri("…")` | `ErrorUri.Tag("…")`, `ErrorUri.Locator("…")`, `ErrorUri.Urn("…")`, `ErrorUri.Relative("/…")` or `ErrorUri.None` |

`ErrorDetail` and `TemplatedMessage` keep their public constructors.

`ErrorUri.Urn` (*RFC 8141*) and `ErrorUri.Relative` (a relative reference with the full path, e.g., `/account/12345/msgs/abc`) are new in 3.0, for values that 2.x could create only with the constructor, such as `urn:` types and relative instances. Other values, e.g., a relative path without a leading `/`, cannot be created.

> [!NOTE]
> In 2.x, `new Result<T>()` created a result with neither a value nor an error, and `IsSuccess()` then threw an `InvalidOperationException`. That state no longer exists: a `Result<T>` is successful exactly when its `Error` is `null`.

### `AddDetail()` removed
In 2.x, `AddDetail()` **changed the error it was called on** and returned the same instance. That was easy to misuse: adding a detail to an error kept in a `static readonly` field changed it for every caller, and the hash code of the error changed after it was used as a dictionary key.

In 3.0, `Error` is immutable. Use one of:
* `WithErrorDetail(...)`, which takes the same arguments in the same order and **returns a copy** with the detail appended (the parameters `messageId` and `namedValues` are renamed to `detailTemplateId` and `detailNamedValues`, so update calls that use named arguments),
* an `ErrorBuilder` with `AddErrorDetail(...)`, to add several details without copying the error each time.

```csharp
// 2.x
var error = Error.Validation(typeUri, "Invalid data to register a new user.");
error.AddDetail("/email", "The email address is required.");
error.AddDetail("/username", "The username is too short.", "errors.username.length", ("min", 3));

// 3.0, a few details: WithErrorDetail returns a copy, so assign the result
var error = Error.Validation(typeUri, "Invalid data to register a new user.")
    .WithErrorDetail("/email", "The email address is required.")
    .WithErrorDetail("/username", "The username is too short.", "errors.username.length", ("min", 3));

// 3.0, details collected in a loop: use a builder
var builder = ErrorBuilder.Validation(typeUri, "Invalid data to register a new user.");
foreach (var failure in validationFailures)
    builder.AddErrorDetail(failure.PropertyPointer, failure.Message);

var error = builder.Build();
```

> [!CAUTION]
> `error.WithErrorDetail(...);` on its own line compiles but does nothing useful: the returned copy is discarded and `error` is unchanged. Assign the result: `error = error.WithErrorDetail(...);`.

The 2.x `internal AddDetail(ErrorDetail)` overload is replaced by the public `WithErrorDetail(ErrorDetail)` and `ErrorBuilder.AddErrorDetail(ErrorDetail)`.

### `with` expressions no longer change errors
All properties of `Error`, `ErrorDetail` and `TemplatedMessage` are now get-only, so `with { Property = … }` does not compile. Use `ToBuilder()` for `Error`:

```csharp
// 2.x
var copy = error with { Title = "Localized title", Detail = null };

// 3.0
var copy = error.ToBuilder()
    .WithTitle("Localized title")
    .WithDetail(null)
    .Build();
```

For `ErrorDetail` and `TemplatedMessage`, create a new instance with the constructor.

### No JSON deserialization without converters
In 2.x, `Result`, `Result<T>`, `Error` and `ErrorUri` could be read from JSON by *System.Text.Json* and *Newtonsoft.Json*, thanks to public parameterless constructors, `init` setters and a built-in converter. That support allowed invalid instances (for example, an empty `Result<T>` read from `{}`), so it was removed:
* the built-in `ResultJsonConverter` and the `[JsonConverter]` attribute on `Result<T>` are gone,
* the types have no public constructors or setters.

Writing the types as JSON still works with the default settings, except for a failed `Result<T>` (see [`Result<T>.Value` throws on a failed result](#resulttvalue-throws-on-a-failed-result)). **Reading them is not supported**: for `Result`, `Result<T>` and `Error`, *System.Text.Json* throws a `NotSupportedException` and *Newtonsoft.Json* throws a `JsonSerializationException`.

> [!WARNING]
> `ErrorUri` is a struct, so both serializers can create it without a constructor: an `ErrorUri` that is a member of your own type is silently read as `about:blank`. Do not deserialize it without a dedicated converter; store the *URI* as a `string` in your DTOs instead.

Dedicated converter packages for *System.Text.Json* and *Newtonsoft.Json* are planned. Until they are available, if you need to read these types (e.g., as Azure Durable Functions activity results, Service Bus messages or cached values), map them to your own DTOs at the boundary:

```csharp
public sealed record ErrorDto(ErrorCategory Category, string TypeUri, string Title, string? Detail, string? InstanceUri);

public static ErrorDto ToDto(Error error) =>
    new(error.Category, error.TypeUri, error.Title, error.Detail, error.InstanceUri);

public static Error FromDto(ErrorDto dto) =>
    new ErrorBuilder(dto.Category, ToErrorUri(dto.TypeUri), dto.Title)
        .WithDetail(dto.Detail)
        .WithInstanceUri(dto.InstanceUri is null ? null : ToErrorUri(dto.InstanceUri))
        .Build();

private static ErrorUri ToErrorUri(string value) =>
    value == "about:blank" ? ErrorUri.None
    : value.StartsWith("tag:", StringComparison.Ordinal) ? ErrorUri.Tag(value)
    : value.StartsWith("urn:", StringComparison.OrdinalIgnoreCase) ? ErrorUri.Urn(value)
    : value.StartsWith('/') ? ErrorUri.Relative(value)
    : ErrorUri.Locator(value);
```

> [!NOTE]
> `Maple.Json.ObjectAsPrimitiveConverter`, which the 2.x documentation suggested for reading metadata values, is no longer relevant to `Maple.Result`.

### Changed JSON output
The JSON written by the default serializers changed in three ways, and a failed `Result<T>` can no longer be written without a converter:

| | 2.x | 3.0 |
| --- | --- | --- |
| Failed `Result<T>` | `{"Error":{…}}` | throws (`Value` throws on a failed result) |
| `Error` | no `Metadata` member | always has `"Metadata":{…}` (`{}` when empty) |
| `Result` and `Result<T>` | no `IsSuccess` member | always has `"IsSuccess":…,"IsError":…`, e.g., `{"Value":128,"Error":null,"IsSuccess":true,"IsError":false}` |

Writing a failed `Result<T>` needs a dedicated converter, or a DTO of your own; see [`Result<T>.Value` throws on a failed result](#resulttvalue-throws-on-a-failed-result). Update any snapshot tests or contract tests that compare the JSON.

### Stricter validation
These values were accepted in 2.x and now throw:

| Value | Exception |
| --- | --- |
| A `null` error title (factory methods, `ErrorBuilder`) | `ArgumentNullException` |
| An empty or white-space-only error title | `ArgumentException` |
| An undefined `ErrorCategory` value, e.g., `(ErrorCategory)42` (`ErrorBuilder`) | `ArgumentOutOfRangeException` |
| An empty or white-space-only `detailTemplateId` / `TemplatedMessage` template id | `ArgumentException` |
| A repeated named-value name, e.g., `("min", 3), ("min", 4)` (was an exception from LINQ's `ToDictionary`, now with a clear message) | `ArgumentException` |
| A `null` named-value name | `ArgumentNullException` |
| Named values without a template id, e.g., `WithErrorDetail("/name", "Too short.", null, ("min", 3))` (2.x **silently dropped** them) | `ArgumentException` |
| A `null` `TemplatedMessage.TemplateId` | `ArgumentNullException` |
| A `null` `ErrorDetail.Detail` | `ArgumentNullException` |
| An empty or white-space-only detail text: `ErrorBuilder.WithDetail("")`, `detail: ""` of the factory methods, `new ErrorDetail("/name", " ")` (pass `null` when there is no detail of the error) | `ArgumentException` |
| An empty, white-space-only or padded template parameter name, e.g., `("", 1)` or `(" min", 1)` | `ArgumentException` |
| A padded template id, e.g., `" errors.length"` | `ArgumentException` |
| A padded metadata key, e.g., `" type"` or `"limit "` | `ArgumentException` |
| An `ErrorDetail.PropertyPointer` that is not a JSON Pointer, e.g., `Email` or `Items[0].Name` (write `/email`, `/items/0/name`; see [What the pointer points to](problem-details.md#what-the-pointer-points-to)) | `ArgumentException` |
| `about:blank` as an instance *URI*, e.g., `instanceUri: ErrorUri.None` (factory methods, `ErrorBuilder.WithInstanceUri`); pass `null` instead | `ArgumentException` |
| An `ErrorUri.Tag` or `ErrorUri.Locator` value with white space (including a trailing new line), invalid percent-encoding (e.g., `%zz`), or, for locators, characters that need escaping (e.g., a space) or literal non-ASCII characters; 2.x accepted them and stored the value as passed | `UriFormatException` |

In return, every `Error` you receive has a non-blank title, a defined category, a non-`null` type *URI* and no meaningless `about:blank` instance *URI*.

`about:blank` has a meaning only as a type (*RFC 9457*, section 4.2.1). A default `ErrorUri` is `about:blank` too, so `instanceUri: default` throws as well.

### Nullable named values
The named values of templated messages are now `object?` instead of `object`:
* the `detailNamedValues` parameters of the `Error` factory methods,
* the `detailNamedValues` parameter of `WithErrorDetail` (formerly `namedValues` of `AddDetail`),
* `TemplatedMessage.Params` (`IReadOnlyDictionary<string, object?>?`).

Existing calls still compile. With nullable reference types enabled, passing a `Dictionary<string, object>` produces warning CS8620; declare it as `Dictionary<string, object?>` instead. Code that reads `Params[...]` may get nullable warnings (CS8602) and should handle `null` values.

`TemplatedMessage` now copies `Params` into its own read-only dictionary with an ordinal (case-sensitive) comparer, so changing the source dictionary later no longer changes the message.

`Params` is `null` whenever a message has no parameters: an empty dictionary, an empty collection (`[]`) or no parameters at all. In 2.x an empty dictionary was kept, so a message with an empty dictionary did not equal the same message without parameters; now they are equal. Code that told an empty `Params` apart from `null` only needs the `null` check.

### `TemplatedMessage` constructors
`TemplatedMessage` is no longer a positional record. It has two constructors:
* `TemplatedMessage(string templateId, IEnumerable<KeyValuePair<string, object?>>? parameters = null)`: passing a dictionary still compiles, and lists of key-value pairs now work too;
* `TemplatedMessage(string templateId, params ReadOnlySpan<(string Key, object? Value)> parameters)`: new; the parameters can be given inline, as an array or as a collection expression.

```csharp
// 2.x
var message = new TemplatedMessage("errors.length", new Dictionary<string, object> { ["min"] = 3 });

// 3.0 (both work)
var message = new TemplatedMessage("errors.length", new Dictionary<string, object?> { ["min"] = 3 });
var message = new TemplatedMessage("errors.length", ("min", 3));
```

Changes that can break existing code:
* **The parameter names** changed from `TemplateId` / `Params` to `templateId` / `parameters`. Update calls that use named arguments.
* **`Deconstruct` is gone.** `var (id, parameters) = message;` and positional patterns such as `message is ("errors.length", var p)` no longer compile; use a property pattern instead: `message is { TemplateId: "errors.length", Params: var p }`.
* **Repeated or `null` parameter names throw** (see [Stricter validation](#stricter-validation)).

The named values of the factory methods (`detailNamedValues`), `WithErrorDetail`, `ErrorBuilder.AddErrorDetail` and `ErrorBuilder.WithDetailTemplated` are now `params ReadOnlySpan<(string Key, object? Value)>` instead of `params (string Key, object? Value)[]`, so a call no longer allocates an array:
* inline values, arrays and collection expressions still compile; recompile the calling code (the signatures changed);
* an explicit `null` is now an empty list (e.g., `WithDetailTemplated("id", null)` no longer throws);
* a call with named values cannot appear inside an expression tree (e.g., a Moq `Setup(x => …)` or an EF query); build the error outside the expression.

### Other behaviour changes
* **`Result.Success()`** returns the same, shared instance on every call. It is immutable, so this is safe; only code that relied on reference inequality of two successful results is affected.
* **`ToString()`** of `Result` and `Result<T>` prints only the `Value` of a successful result or the `Error` of a failed one (e.g., `Result { Value = 42 }`, `Result { Error = Error { … } }`); 2.x printed every property, including the default `Value` of a failed result.
* **`ToString()`** of `Error` and `TemplatedMessage` now prints the contents of `ErrorDetails`, `Metadata` and `Params` (instead of their type names), and `ErrorUri.ToString()` returns just the *URI* (instead of `ErrorUri { Value = … }`). Update tests or log parsers that depend on the old text.
* **`GetHashCode()`** of an `Error` never changes, because the error cannot change. An `Error` can be used safely as a dictionary key or in a `HashSet<T>`.
* **`ErrorDetails`** is a read-only collection; casting it to `IList<ErrorDetail>` and modifying it throws a `NotSupportedException`.
* **`ErrorCategory.Timeout`** is now documented as `408` (Request Timeout) when the client did not send the complete request in time, or `504` (Gateway Timeout) when an upstream server or a dependency did not respond in time; 2.x documented only `408`. The `ErrorCategory` members now have explicit values, equal to the 2.x ones, so stored and serialized values do not change.

### `continueOnCapturedContext` is `bool?`
The `continueOnCapturedContext` parameter of the asynchronous extension methods (`ThenAsync`, `ElseAsync`, `IfSuccessAsync`, `IfErrorAsync`, `MatchAsync`, `ToResultAsync`, for `Task` and `ValueTask`) changed from `bool continueOnCapturedContext = false` to `bool? continueOnCapturedContext = null`. `null` uses the new global setting [`ResultConfiguration.ContinueOnCapturedContext`](Reference/ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md), which is `false` by default, so **the behaviour does not change** unless you change the setting.

Calls that pass `true` or `false` still compile; recompile code that calls these methods (the signature changed, so binaries built against 2.x do not bind to 3.0).

The LINQ query operators (`Select`, `SelectMany` on `Task` and `ValueTask` of a result) used a hard-coded `ConfigureAwait(false)`; they now use the global setting too.

If your application has a synchronization context (WPF, Windows Forms, .NET MAUI, Blazor Server) and the delegates you pass access UI elements, set the global setting once at startup instead of passing `continueOnCapturedContext: true` everywhere:

```csharp
ResultConfiguration.ContinueOnCapturedContext = true;
```

### `IResult` renamed to `IOperationResult`
The common interface of `Result` and `Result<T>` was renamed from `IResult` to [`IOperationResult`](Reference/IOperationResult/IOperationResult.md). Its `Error` property did not change; `IsSuccess()` became a property and `IsError` was added (see [`IsSuccess` is a property; new `IsError`](#issuccess-is-a-property-new-iserror)).

The interface also declares `TryGetError(out Error? error)`. If your own types implement `IOperationResult`, implement the `IsSuccess` and `IsError` properties and `TryGetError`, with the same nullable annotations (`[MemberNotNullWhen(false, nameof(Error))]` on `IsSuccess`, `[MemberNotNullWhen(true, nameof(Error))]` on `IsError`, `[NotNullWhen(true)]` on the `error` parameter).

The old name clashed with `Microsoft.AspNetCore.Http.IResult`. ASP.NET Core web projects import `Microsoft.AspNetCore.Http` implicitly, and Minimal API endpoints commonly return `IResult`, so adding `using Maple.Result;` caused CS0104 (ambiguous reference) in endpoint signatures. If you added an alias to work around it (e.g., `using IResult = Microsoft.AspNetCore.Http.IResult;`), you can remove it.

```csharp
// 2.x
void Log(Maple.Result.IResult result) { … }

// 3.0
void Log(IOperationResult result) { … }
```

### `ErrorUri.None()` is a property
`ErrorUri.None()` is now a static property, `ErrorUri.None`, like `CancellationToken.None` and `Guid.Empty`: it is a fixed value (`about:blank`, equal to `default(ErrorUri)`). Calls with parentheses no longer compile (CS1955: *Non-invocable member cannot be used like a method*); drop the parentheses.

```csharp
// before
var error = Error.NotFound(ErrorUri.None(), "The account has not been found.");

// after
var error = Error.NotFound(ErrorUri.None, "The account has not been found.");
```

### `IsSuccess` is a property; new `IsError`
`IsSuccess()` is now a property, `IsSuccess`, on `Result`, `Result<T>` and `IOperationResult`, and the new `IsError` property is its opposite. Calls with parentheses no longer compile (CS1955: *Non-invocable member cannot be used like a method*); drop the parentheses.

```csharp
// before
if (!result.IsSuccess())
    return result.Error;

// after
if (result.IsError)
    return result.Error;
```

Both properties keep the nullable annotations: `Error` is not `null` after `IsError` is `true` or `IsSuccess` is `false`. As properties, they can be used in property patterns, e.g., `result is { IsSuccess: true, Value: var value }`.

The default serializers write public properties, so the JSON written without a converter now contains both of them (see [Changed JSON output](#changed-json-output)).

### `Result<T>.Value` throws on a failed result
Reading the `Value` of a failed `Result<T>` now throws an `InvalidOperationException` (the message names the error), instead of returning the default value of `T`. A forgotten check used to read as a valid value (e.g., `0` for a `Result<int>`); now it fails where the value is read.

```csharp
// before: 0 on a failed result
var count = result.Value;

// after
if (result.IsError)
    return result.Error;

var count = result.Value;

// or, in one statement
if (!result.TryGetValue(out var count, out var error))
    return error;
```

The type of `Value` is `T` instead of `T?`, and `IsSuccess` and `IsError` no longer carry a hint about `Value`: its nullability follows `T` (`string` for a `Result<string>`, `string?` for a `Result<string?>`), so the compiler no longer hides a `null` value of a nullable `T`.

Consequences:
* the default serializers read every public property, so **writing a failed `Result<T>` without a converter throws** (*System.Text.Json*: `InvalidOperationException`; *Newtonsoft.Json*: `JsonSerializationException`); a successful one is still written;
* `ToString()` prints only the value of a successful result or the error of a failed one (see [Other behaviour changes](#other-behaviour-changes)).

### Transforming `IfSuccess` and `IfError` renamed to `Then` and `Else`
In 2.x, `IfSuccess` and `IfError` did two different things under one name: with an action they ran a side effect and returned the result unchanged; with a function they transformed the result (`IfSuccess`) or replaced a failed one (`IfError`). The overload, and so the behaviour, depended on whether the lambda returned a value: `result.IfSuccess(x => set.Add(x))` (a side effect whose expression returns a `bool`) turned a `Result<T>` into a `Result<bool>`, and `result.IfError(e => Interlocked.Increment(ref failures))` turned a failure into a success.

In 3.0 the name says what the call does:
* `IfSuccess`, `IfSuccessAsync`, `IfError` and `IfErrorAsync` run a side effect and return the result unchanged;
* `Then` and `ThenAsync` transform or chain a successful result with a function that returns a value, a `Result`, a `Result<T>` or an `Error` (the overloads, the flattening of a returned result and the handling of a returned `Error` are the same as before);
* `Else` and `ElseAsync` replace a failed result with the result that the function returns.

A 2.x call that transforms or replaces the result is a compile error (CS0619) whose message names the method to use, so the compiler finds every such call. An action passed to `Then` or `Else` is a compile error that names `IfSuccess` or `IfError`.

```csharp
// 2.x
var orderResult = userResult.IfSuccess(user => CreateOrder(user));
var guestResult = userResult.IfError(error => Result<User>.FromValue(User.Guest));

// 3.0
var orderResult = userResult.Then(user => CreateOrder(user));
var guestResult = userResult.Else(error => Result<User>.FromValue(User.Guest));

// 3.0: a side effect whose expression returns a value needs a statement lambda
userResult.IfSuccess(user => { seenIds.Add(user.Id); });
```

### `Then` and `Match` with an `Error` or a task
Two kinds of calls compiled in 2.x but did not do what they looked like:

* **A function that returns an `Error`** bound to the generic overload and returned a **successful** `Result<Error>`. In 3.0, `Then`, `ThenAsync`, `Match` and `MatchAsync` (the overloads with an error action) have overloads for such functions that return a **failed** `Result` with that error. The same applies to a LINQ query that selects an `Error` (`from user in GetUser(id) select Error…`). Code that read `.Value` of the `Result<Error>` no longer compiles; if you need it, specify the type arguments.
* **A function that returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>`** passed to `IfSuccess` (now `Then`), or to the `Match` overloads with an `Action<Error>`, was wrapped in a `Result<Task>` and **never awaited**. In 3.0 such a call is a compile error (CS0619); use the asynchronous method, or specify the type argument to keep the task on purpose.

```csharp
// 2.x (IfSuccess): a successful Result<Error>; 3.0: a failed Result with the error
var result = userResult.Then(user => Error.Conflict(typeUri, "The user already exists."));
var result = userResult.Match(user => Error.Conflict(typeUri, "The user already exists."), error => Log(error));
var result = from user in userResult select Error.Conflict(typeUri, "The user already exists.");

// 2.x: the task was never awaited; 3.0: a compile error
var result = Result.Success().IfSuccess(() => SaveAsync());
var result = Result.Success().Then(() => SaveAsync());

// 3.0: await it…
var result = await Result.Success().IfSuccessAsync(() => SaveAsync());

// …or keep the task on purpose
Result<Task> result = Result.Success().Then<Task>(() => SaveAsync());
```

### `IfError` and `Match` with an error action that returns a task
In 2.x, an error action that returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>` (e.g., `e => LogAsync(e)`) was accepted as an `Action<Error>` by `IfError` and by the `Match` overloads with an error action, and its task was **never awaited**. An `async` lambda became `async void`, so an exception thrown in it ended the process. In 3.0 such a call is a compile error (CS0619): use `IfErrorAsync` or `MatchAsync`, or, to start the task without awaiting it on purpose, discard it explicitly in a statement lambda.

The `Match` overloads in which both functions return a value are not affected: `await result.Match(() => SaveAsync(), e => LogAsync(e))` still returns the task of the function that ran. An `async` success function in the synchronous `Match` (e.g., `Match(async () => await SaveAsync(), e => { })`) was already a compile error; it now reports the same message as the other unsupported calls instead of an ambiguity (CS0121).

```csharp
// 2.x: the task was never awaited (async void for an async lambda); 3.0: a compile error
var result = userResult.IfError(error => LogAsync(error));
var result = userResult.Match(user => user.Id, async error => await LogAsync(error));

// 3.0: await it…
var result = await userResult.IfErrorAsync(error => LogAsync(error));

// …or start it without awaiting on purpose
var result = userResult.IfError(error => { _ = LogAsync(error); });
```

### LINQ queries that select a task
In 2.x, a query whose `select` returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>` wrapped the task in a successful result (e.g., `Result<Task<int>>`) and **never awaited** it. This applies to every source (`Result<T>`, `Task<Result<T>>`, `ValueTask<Result<T>>`) and to queries with several `from` clauses. In 3.0 such a query is a compile error (CS0619). Chain the operation in a `from` clause whose function returns a `Task<Result<T>>` (or `ValueTask<Result<T>>`), or use `ThenAsync`. To keep the task in the result on purpose, call `Select` with explicit type arguments.

```csharp
// 2.x: a Result<Task<int>> whose task was never awaited; 3.0: a compile error
var result = from user in userResult select SaveAsync(user);

// 3.0: await it…
var result = await userResult.ThenAsync(user => SaveAsync(user));

// …or keep the task on purpose
Result<Task<int>> result = userResult.Select<User, Task<int>>(user => SaveAsync(user));
```

### LINQ queries that select a result
In 2.x, a query whose `select` returns a `Result` or a `Result<T>` nested it: `from user in userResult select CreateOrder(user)`, with `Result<Order> CreateOrder(User)`, was a `Result<Result<Order>>`. In 3.0 it is the `Result<Order>` that `CreateOrder` returns, as `Then` gives it; the same applies to a `select` after several `from` clauses, for every source (`Result<T>`, `Task<Result<T>>`, `ValueTask<Result<T>>`). Code that unwrapped the nested result no longer compiles; to keep it nested on purpose, call `Select` with explicit type arguments.

```csharp
// 2.x: a Result<Result<Order>>; 3.0: a Result<Order>
var orderResult = from user in userResult select CreateOrder(user);

// …or keep it nested on purpose
Result<Result<Order>> nested = userResult.Select<User, Result<Order>>(user => CreateOrder(user));
```

### Null results and null arguments of the extension methods
In 2.x, a `null` returned by a function passed to `IfSuccess`, `IfError` (now `Then` and `Else`), `Match` or a LINQ query was returned to the caller, caused a `NullReferenceException` inside the library, or an `ArgumentNullException` for a parameter the caller did not pass. In 3.0 the rules are the same for all extension methods (sync, `Task` and `ValueTask`):

* **A `null` argument** throws an `ArgumentNullException`. The asynchronous methods now throw it **from the call**, not from `await`: `var task = result.ThenAsync((Func<Task<Result>>)null!)` throws immediately. This only changes code that stored such a task and awaited it inside a `try`.
* **A function that returns `null`** where a `Result`, `Result<T>`, `Error` or a task is required — a `null` result, a `null` task, or a task with a `null` result — throws an `InvalidOperationException` that names the function, e.g., "The function ‘ifSuccessFunction’ returned null." A `null` **value** returned for a `Result<T>` is still a successful result.
* **An awaited source** (`Task<Result>`, `ValueTask<Result<T>>`, …) that returns `null` throws an `InvalidOperationException` as before; the message now names the parameter: "The asynchronous operation represented by ‘resultTask’ returned null."

```csharp
// 2.x: returned null; 3.0: InvalidOperationException
var result = Result.Success().Then(() => (Result)null!);

// 2.x: the exception surfaced at await; 3.0: thrown by the call
var task = result.ThenAsync((Func<Task<Result>>)null!);
```

### Extension packages
The current (2.x-based) version of `Maple.Result.Extensions.HttpClient` calls an internal method of `Maple.Result` 2.x that no longer exists, so using it with `Maple.Result` 3.0 fails at runtime. Upgrade all `Maple.Result.Extensions.*` packages to versions built for 3.0 together with `Maple.Result`, and do not mix 2.x and 3.0 packages.

## New features
* **`ErrorBuilder`** — a mutable builder of an immutable `Error`. Create it with `new ErrorBuilder(category, typeUri, title)`, a static method per category (e.g., `ErrorBuilder.Validation(...)`) or `error.ToBuilder()`; set the members with `With…` / `AddErrorDetail` methods; call `Build()`. Every argument is validated when it is passed, so `Build()` never throws. See [ErrorBuilder](Reference/ErrorBuilder/ErrorBuilder.md).
* **`Error.Metadata`** — machine-readable, response-level values (RFC 9457 *extension members*), with `WithMetadata()`, `WithoutMetadata()` (one key or all) and `TryGetMetadata<T>()`. `TryGetMetadata<T>()` is strict: it returns `true` only when the stored value is a `T` (a stored `int` is not read as `long`), and never for a `null` value. See [Metadata](Reference/Error/Error_Metadata.md).
* **`WithMetadata` with tuples** — `error.WithMetadata(("limit", 100), ("remaining", 0))` on `Error` and `ErrorBuilder` adds several entries in one call.
* **`Error.WithErrorDetail()`** and **`Error.ToBuilder()`** — see above.
* **`ErrorCategory.LimitExceeded`** — see above.
* **`ProblemDetailsMembers`** — the member names of the problem details object (`type`, `title`, `status`, …), shared by the packages that write and read problem details. See [Problem Details Format](problem-details.md).
* **`Result.Try()` and `Result.TryAsync()`** — run code that can throw and turn the exception into a failed result, with the error that a required mapper creates (e.g., `Result.Try(() => repository.Load(id), ex => …)`); an `OperationCanceledException` is rethrown, a function that returns a `Result`, a `Result<T>` or an `Error` is flattened, and a mapper that returns `null` declines the exception (it is rethrown). See [Try](Reference/Result/Result_Try.md) and [TryAsync](Reference/Result/Result_TryAsync.md).
* **`TryGetValue` and `TryGetError`** — `if (!GetUser(id).TryGetValue(out var user, out var error)) return error;` reads the value of a `Result<T>` without an exception and gives the early return in one statement; `TryGetError(out var error)` on `Result`, `Result<T>` and `IOperationResult` does the same for the error. See [TryGetValue](Reference/ResultT/ResultT_TryGetValue.md) and [TryGetError](Reference/Result/Result_TryGetError.md).
* **`ErrorUri.Other()`** — an absolute *URI* whose scheme has no factory method of its own (e.g., `mailto:`), for type and instance URIs read from other services; `Parse` and `TryParse` fall back to it. See [Other](Reference/ErrorUri/ErrorUri_Other.md).
* **`ErrorUri.Parse()`**, **`ErrorUri.TryParse()`** and **`IParsable<ErrorUri>`** — create an `ErrorUri` when the kind is not known in advance: the kind is detected from the value (`about:blank`, `/…`, `tag:`, `urn:`, `http:`/`https:`) and validated as by its factory method. See [Parse](Reference/ErrorUri/ErrorUri_Parse.md).
* **`ErrorUri.Urn()`** and **`ErrorUri.Relative()`** — `urn:` types and instances (*RFC 8141*), and relative references with the full path (e.g., the *RFC 9457* example `/account/12345/msgs/abc`). See [ErrorUri](Reference/ErrorUri/ErrorUri.md).
* **Synchronous delegates after an asynchronous call** — `ThenAsync`, `ElseAsync`, `IfSuccessAsync`, `IfErrorAsync` and `MatchAsync` on a `Task` or `ValueTask` of a result also take synchronous delegates, e.g., `await GetUserAsync(id).ThenAsync(user => user.Name)`. A delegate that returns the other kind of task (a `ValueTask` with `Maple.Results.Extensions`, a `Task` with `Maple.Results.Extensions.ValueTasks`), and one synchronous and one asynchronous delegate of `MatchAsync`, are compile errors (CS0619). See [ThenAsync](Reference/Extensions/ThenTaskAsyncExtensions_ThenAsync.md).
* **The non-generic `Result` in LINQ queries** — a step that returns a `Result` (e.g., a check) can be any `from` clause, first or in the middle (`from _ in EnsureActive(user)`), synchronous or asynchronous; its range variable is the successful `Result`. See [SelectMany](Reference/Extensions/LinqExtensions_SelectMany.md).
* **Mixed LINQ queries** — synchronous and asynchronous `from` clauses can be mixed in one query (e.g., `from user in GetUserAsync(id) from address in GetAddress(user) select …`); the query is then asynchronous and has to be awaited. See [SelectMany](Reference/Extensions/LinqTaskAsyncExtensions_SelectMany.md).
* **`ResultConfiguration.ContinueOnCapturedContext`** — a global default for the `continueOnCapturedContext` argument of the asynchronous extension methods and for the LINQ query operators; see above.
* **Short debugger display** — the debugger shows an `Error` as its category and title (e.g., `Validation "Invalid request" (3 details)`) and a failed result as `Error: NotFound "Missing"`; `ToString()` stays complete for the logs.
* **Native AOT and trimming** — the core package no longer contains JSON code and produces no trimming or AOT warnings.

## Search-and-replace reference
| Find (2.x) | Replace with (3.0) |
| --- | --- |
| `error.AddDetail(…);` | `error = error.WithErrorDetail(…);` |
| `Error.X(…).AddDetail(…).AddDetail(…)` | `Error.X(…).WithErrorDetail(…).WithErrorDetail(…)` or `ErrorBuilder.X(…).AddErrorDetail(…).AddErrorDetail(…).Build()` |
| `error with { Title = t }` | `error.ToBuilder().WithTitle(t).Build()` |
| `error with { Detail = d }` | `error.ToBuilder().WithDetail(d).Build()` |
| `error with { Category = c }` | `error.ToBuilder().WithCategory(c).Build()` |
| `new Result()` | `Result.Success()` |
| `new Result<T> { Value = v }` | `Result<T>.FromValue(v)` |
| `new Result<T> { Error = e }` | `Result<T>.FromError(e)` |
| `new ErrorUri(s)` | `ErrorUri.Tag(s)` / `ErrorUri.Locator(s)` / `ErrorUri.Urn(s)` / `ErrorUri.Relative(s)` / `ErrorUri.None` |
| `instanceUri: ErrorUri.None()` / `WithInstanceUri(ErrorUri.None())` | `instanceUri: null` / `WithInstanceUri(null)` |
| `ErrorUri.None()` | `ErrorUri.None` |
| `result.IfSuccess(x => …)` (a function returning a value, a result or an error) | `result.Then(x => …)` |
| `await result.IfSuccessAsync(x => …)` (a function returning a value, a result or an error in a task) | `await result.ThenAsync(x => …)` |
| `result.IfError(e => …)` (a function returning the replacement result) | `result.Else(e => …)` |
| `await result.IfErrorAsync(e => …)` (a function returning the replacement result in a task) | `await result.ElseAsync(e => …)` |
| `result.IfSuccess(x => set.Add(x))` (a side effect whose expression returns a value) | `result.IfSuccess(x => { set.Add(x); })` |
| `result.IfSuccess(() => SaveAsync())` (a function returning a task) | `await result.IfSuccessAsync(() => SaveAsync())` |
| `result.IfError(e => LogAsync(e))` (an error action returning a task) | `await result.IfErrorAsync(e => LogAsync(e))` |
| `result.Match(…, e => LogAsync(e))` (an error action returning a task) | `await result.MatchAsync(…, e => LogAsync(e))` |
| `from x in result select SaveAsync(x)` (a selector returning a task) | `await result.ThenAsync(x => SaveAsync(x))` |
| `result.IfSuccess(x => Error…)` relied on as a `Result<Error>` | `result.Then<T, Error>(x => Error…)` |
| `from x in result select Save(x)` relied on as a `Result<Result<T>>` | `result.Select<T, Result<TNext>>(x => Save(x))` |
| `result.Match(x => Error…, e => …)` relied on as a `Result<Error>` | `result.Match<T, Error>(x => Error…, e => …)` |
| `WithErrorDetail("Email", …)` / `AddErrorDetail(nameof(request.Email), …)` (a pointer that is not a JSON Pointer) | `WithErrorDetail("/email", …)` / `AddErrorDetail("/email", …)` |
| `Dictionary<string, object>` passed as named values | `Dictionary<string, object?>` |
| `using Maple.Result;` | `using Maple.Results;` |
| `using Maple.Result.Extensions;` | `using Maple.Results.Extensions;` |
| `using Maple.Result.Extensions.ValueTasks;` | `using Maple.Results.Extensions.ValueTasks;` |
| `IResult` (`Maple.Result`) | `IOperationResult` (`Maple.Results`) |
| `result.IsSuccess()` | `result.IsSuccess` |
| `!result.IsSuccess()` | `result.IsError` |
| `result.Value` relied on as the default value of a failed result | `result.TryGetValue(out var value) ? value : default` or a check of `result.IsSuccess` |
| `WithErrorDetail(…, messageId: …, namedValues: …)` (named arguments) | `WithErrorDetail(…, detailTemplateId: …, detailNamedValues: …)` |
| `var (id, parameters) = templatedMessage;` | `templatedMessage is { TemplateId: var id, Params: var parameters }` |
| `JsonSerializer.Deserialize<Result<T>>(…)` / `Deserialize<Error>(…)` | your own DTO, or the planned converter packages |

## See Also
* [README: Upgrading to 3.0](../README.md#upgrading-to-30)
* [Reference documentation](Reference/namespace.md)
* [Issue #178: Rename the namespace to Maple.Results](https://github.com/TomMaple/Maple.Result/issues/178)
* [Issue #102: Simplify Result and Error](https://github.com/TomMaple/Maple.Result/issues/102)
* [Issue #105: SynchronizationContext of the asynchronous extension methods](https://github.com/TomMaple/Maple.Result/issues/105)
* [Issue #121: Null results and synchronous argument checks of the extension methods](https://github.com/TomMaple/Maple.Result/issues/121)
* [Issue #123: Task-returning error actions in `IfError` and `Match`](https://github.com/TomMaple/Maple.Result/issues/123)
* [Issue #126: LINQ selectors that return a task](https://github.com/TomMaple/Maple.Result/issues/126)
* [Issue #197: Split `IfSuccess` and `IfError` by intent: `Then` and `Else`](https://github.com/TomMaple/Maple.Result/issues/197)
* [Issue #198: Validate `ErrorDetail.PropertyPointer` as a JSON Pointer](https://github.com/TomMaple/Maple.Result/issues/198)
* [Issue #203: Reject empty or whitespace-only detail texts and names padded with white space](https://github.com/TomMaple/Maple.Result/issues/203)
* [Issue #205: The default `ErrorCategory` and the status codes read as `Failure`](https://github.com/TomMaple/Maple.Result/issues/205)
* [Issue #217: The non-generic `Result` in LINQ `from` clauses, and a selected result](https://github.com/TomMaple/Maple.Result/issues/217)
