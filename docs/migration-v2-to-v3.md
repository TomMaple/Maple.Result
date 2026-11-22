# Migrating from Maple.Result 2.x to 3.0

This guide lists the changes between the last 2.x release (`2.8.12`) and `3.0`, explains why they were made, and shows how to update your code.

Version 3.0 makes `Result`, `Result<T>` and `Error` **immutable** and **always valid**: an invalid instance can no longer be created, and an instance cannot change after it was created. It also adds the `LimitExceeded` error category, error metadata and an `ErrorBuilder`.

## Contents
* [Quick checklist](#quick-checklist)
* [Breaking changes](#breaking-changes)
  * [Namespace renamed to `Maple.Results`](#namespace-renamed-to-mapleresults)
  * [One namespace for the `Task` and `ValueTask` methods](#one-namespace-for-the-task-and-valuetask-methods)
  * [New `LimitExceeded` error category](#new-limitexceeded-error-category)
  * [`Unauthorized` renamed to `PermissionDenied`](#unauthorized-renamed-to-permissiondenied)
  * [Public constructors removed](#public-constructors-removed)
  * [`AddDetail()` removed](#adddetail-removed)
  * [`with` expressions no longer change errors](#with-expressions-no-longer-change-errors)
  * [No JSON deserialization without converters](#no-json-deserialization-without-converters)
  * [Changed JSON output](#changed-json-output)
  * [Stricter validation](#stricter-validation)
  * [Nullable parameters](#nullable-parameters)
  * [`TemplatedMessage` constructors](#templatedmessage-constructors)
  * [`TemplatedMessage.Params` renamed to `Parameters`](#templatedmessageparams-renamed-to-parameters)
  * [The factory methods take a `TemplatedMessage`](#the-factory-methods-take-a-templatedmessage)
  * [Other behaviour changes](#other-behaviour-changes)
  * [`continueOnCapturedContext` is `bool?`](#continueoncapturedcontext-is-bool)
  * [`IResult` renamed to `IOperationResult`](#iresult-renamed-to-ioperationresult)
  * [`IOperationResult` is closed](#ioperationresult-is-closed)
  * [`IsSuccess` is a property; new `IsError`](#issuccess-is-a-property-new-iserror)
  * [`ErrorUri.None()` is a property](#errorurinone-is-a-property)
  * [`Error.TypeUri` and `Error.InstanceUri` are an `ErrorUri`](#errortypeuri-and-errorinstanceuri-are-an-erroruri)
  * [`Result<T>.Value` throws on a failed result](#resulttvalue-throws-on-a-failed-result)
  * [The analyzer reports the calls that compile but changed](#the-analyzer-reports-the-calls-that-compile-but-changed)
  * [Transforming `IfSuccess` and `IfError` renamed to `Then` and `Else`](#transforming-ifsuccess-and-iferror-renamed-to-then-and-else)
  * [`Match` is a fold; `Switch` runs two actions](#match-is-a-fold-switch-runs-two-actions)
  * [`Then` and `Match` with an `Error` or a task](#then-and-match-with-an-error-or-a-task)
  * [`IfError` and `Match` with an error action that returns a task](#iferror-and-match-with-an-error-action-that-returns-a-task)
  * [LINQ queries that select a task](#linq-queries-that-select-a-task)
  * [LINQ queries that select a result](#linq-queries-that-select-a-result)
  * [Null results and null arguments of the extension methods](#null-results-and-null-arguments-of-the-extension-methods)
  * [Extension packages](#extension-packages)
* [New features](#new-features)
* [Search-and-replace reference](#search-and-replace-reference)

## Quick checklist
1. Replace `using Maple.Result…;` with `using Maple.Results…;`, and remove `using Maple.Result.Extensions.ValueTasks;` (see [Namespace renamed to `Maple.Results`](#namespace-renamed-to-mapleresults) and [One namespace for the `Task` and `ValueTask` methods](#one-namespace-for-the-task-and-valuetask-methods)).
2. Handle `ErrorCategory.LimitExceeded` in every exhaustive `switch` over `ErrorCategory`, and rename `Unauthorized` to `PermissionDenied` (see [`Unauthorized` renamed to `PermissionDenied`](#unauthorized-renamed-to-permissiondenied)).
3. Replace `error.AddDetail(...)` with `error = error.WithErrorDetail(...)` or an `ErrorBuilder`.
4. Replace `error with { … }` with `error.ToBuilder()…Build()`.
5. Replace `new Result()`, `new Result<T>()`, `new Error()` and `new ErrorUri(...)` with the factory methods.
6. If you **read** `Result`, `Result<T>` or `Error` from JSON (e.g., Azure Durable Functions, message queues, caches, HTTP clients), see [No JSON deserialization without converters](#no-json-deserialization-without-converters).
7. If you compare or snapshot the **written** JSON, see [Changed JSON output](#changed-json-output).
8. Make sure error titles are never `null` or blank, detail texts are `null` or not blank, metadata keys, template ids and template parameter names do not start or end with white space, two parameter names of one message do not differ only in case, and the pointers of error details are JSON Pointers (`/email`, not `Email`), and pass `null` instead of `ErrorUri.None()` as an instance *URI* (see [Stricter validation](#stricter-validation)).
9. Rename the calls of `IfSuccess`, `IfSuccessAsync`, `IfError` and `IfErrorAsync` that transform or replace the result to `Then`, `ThenAsync`, `Else` and `ElseAsync`; the analyzer or the compiler reports most of them, but not all (see [Transforming `IfSuccess` and `IfError` renamed to `Then` and `Else`](#transforming-ifsuccess-and-iferror-renamed-to-then-and-else)); replace `Match` and `MatchAsync` with two actions by `Switch` and `SwitchAsync`, `MatchAsync` on a result by `Match`, and `Match` with a success function and an error action by `Then(…).IfError(…)` (see [`Match` is a fold; `Switch` runs two actions](#match-is-a-fold-switch-runs-two-actions)).
10. Fix the calls of `Then`, `Match` and the LINQ queries that the analyzer reports, that no longer compile or that now return a failed result (see [`Then` and `Match` with an `Error` or a task](#then-and-match-with-an-error-or-a-task)).
11. Replace the error actions of `IfError` and `Match` that return a task with the asynchronous methods; the analyzer reports them in `IfError` (see [`IfError` and `Match` with an error action that returns a task](#iferror-and-match-with-an-error-action-that-returns-a-task)).
12. Replace the LINQ queries whose `select` returns a task with an asynchronous `from` clause or `ThenAsync` (see [LINQ queries that select a task](#linq-queries-that-select-a-task)), and check the queries whose `select` returns a result (see [LINQ queries that select a result](#linq-queries-that-select-a-result)).
13. Make sure the functions passed to the extension methods never return `null` where a result, an `Error` or a task is expected (see [Null results and null arguments of the extension methods](#null-results-and-null-arguments-of-the-extension-methods)).
14. Upgrade the `Maple.Result.Extensions.*` packages together with `Maple.Result` (see [Extension packages](#extension-packages)).
15. Replace `result.IsSuccess()` with `result.IsSuccess`, and `!result.IsSuccess()` with `result.IsError` (see [`IsSuccess` is a property; new `IsError`](#issuccess-is-a-property-new-iserror)).
16. Make sure `Result<T>.Value` is read only on a successful result, or use `TryGetValue(out var value)`; and do not write a failed `Result<T>` with the default serializers (see [`Result<T>.Value` throws on a failed result](#resulttvalue-throws-on-a-failed-result)).
17. Replace `ErrorUri.None()` with `ErrorUri.None` (see [`ErrorUri.None()` is a property](#errorurinone-is-a-property)), read the text of `error.TypeUri` and `error.InstanceUri` with `.Value`, and compare the type of an error with the `ErrorUri` that defines it (see [`Error.TypeUri` and `Error.InstanceUri` are an `ErrorUri`](#errortypeuri-and-errorinstanceuri-are-an-erroruri)).
18. Replace your own implementations and mocks of `IResult` with `Result` and `Result<T>` (see [`IOperationResult` is closed](#ioperationresult-is-closed)).
19. Fix every error and warning of the analyzer (MR0001 to MR0006), and do not switch the analyzers off: it reports the 2.x calls that still compile but changed their meaning (see [The analyzer reports the calls that compile but changed](#the-analyzer-reports-the-calls-that-compile-but-changed)).
20. Pass the template ID and the parameters of the `Error` factory methods as one `TemplatedMessage`: `detailTemplated: new("errors.order.notFound", ("orderId", id))` instead of `detailTemplateId: …, detailNamedValues: …` or two positional arguments (see [The factory methods take a `TemplatedMessage`](#the-factory-methods-take-a-templatedmessage)).
21. Replace `TemplatedMessage.Params` with `Parameters`, and the named argument `namedValues:` of `AddDetail` with `detailParameters:` of `WithErrorDetail` (see [`TemplatedMessage.Params` renamed to `Parameters`](#templatedmessageparams-renamed-to-parameters)).

## Breaking changes
### Namespace renamed to `Maple.Results`
The namespaces are now `Maple.Results` and `Maple.Results.Extensions`; the `ValueTask`-based extension methods are in `Maple.Results.Extensions` too (see [One namespace for the `Task` and `ValueTask` methods](#one-namespace-for-the-task-and-valuetask-methods)). The package ID (`Maple.Result`) and the assembly name (`Maple.Result.dll`) did not change.

```csharp
// before
using Maple.Result;
using Maple.Result.Extensions;
using Maple.Result.Extensions.ValueTasks;

// after
using Maple.Results;
using Maple.Results.Extensions;
```

The old namespace hid the type: inside any `Maple.*` namespace other than `Maple.Result.*` (an application or a library named `Maple.Something`), `Result` resolved to the namespace, so `Result.Success()`, `Result.FromValue(…)` and `Result.FromError(…)` did not compile (CS0234). A plural namespace with a singular type is the usual way out (`System.Threading.Tasks` and `Task`).

> [!NOTE]
> Inside any `Maple.*` namespace, the name `Results` now resolves to the namespace `Maple.Results`, so `Results.Ok()` of ASP.NET Core (Minimal APIs) does not compile there (CS0234). Use `TypedResults.Ok()` or the fully qualified `Microsoft.AspNetCore.Http.Results.Ok()`. Code in other namespaces is not affected.

If you referenced a type by its full name (e.g., `Maple.Result.Error` in a `using` alias, in reflection or in a serialized type name such as `$type`), update it to `Maple.Results.…`.

### One namespace for the `Task` and `ValueTask` methods
In 2.x, the `ValueTask`-based extension methods were in their own namespace, `Maple.Result.Extensions.ValueTasks`, and a file imported it or `Maple.Result.Extensions` (with both, a call with an `async` lambda was ambiguous). In 3.0, the `Task`-based and the `ValueTask`-based methods of each verb are one class in `Maple.Results.Extensions`: `ThenAsyncExtensions`, `ElseAsyncExtensions`, `IfSuccessAsyncExtensions`, `IfErrorAsyncExtensions`, `MatchAsyncExtensions`, `SwitchAsyncExtensions`, `LinqAsyncExtensions` and `ResultAsyncExtensions` (they were `…TaskAsyncExtensions` and `…ValueTaskAsyncExtensions`). Remove `using Maple.Result.Extensions.ValueTasks;`, and replace the old class names where they are written (e.g., a static call such as `ThenValueTaskAsyncExtensions.ThenAsync(result, …)`).

On a `Task` or a `ValueTask` of a result, the kind of the task is kept, as before, and a function that returns the other kind of task is awaited (it was a compile error). On a `Result` or a `Result<T>`, the function decides it: a function that returns a `ValueTask` gives a `ValueTask`, and an `async` lambda gives a `Task`. In a file that imported only the `ValueTask` namespace, an `async` lambda on a result gave a `ValueTask`; it gives a `Task` now. When the call is awaited or assigned to `var`, nothing changes but the allocation of a `Task`. Assigned to a `ValueTask` type, it is a compile error (CS0029); write the return type of the lambda:

```csharp
// 2.x, with using Maple.Result.Extensions.ValueTasks: a ValueTask<Result<int>>
ValueTask<Result<int>> next = result.ThenAsync(async x => await GetCountAsync(x));

// 3.0: an async lambda gives a Task; its return type selects a ValueTask
ValueTask<Result<int>> next = result.ThenAsync(async ValueTask<int> (int x) => await GetCountAsync(x));
```

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

### `Unauthorized` renamed to `PermissionDenied`
`ErrorCategory.Unauthorized`, `Error.Unauthorized(...)` and `ErrorBuilder.Unauthorized(...)` are renamed to `PermissionDenied`. Only the name changed: the numeric value (`2`), the meaning (the identity is known, but it does not have the permission) and the default status code (`403`) are the same. "Unauthorized" names the missing authentication in HTTP (`401 Unauthorized`), so a caller who wanted a `401` could pick it and get a `403`; the category for a `401` is still `Unauthenticated`.

```csharp
// 2.x
var error = Error.Unauthorized(typeUri, "The contact cannot be deleted.");

// 3.0
var error = Error.PermissionDenied(typeUri, "The contact cannot be deleted.");
```

Code that stores, compares or logs the **name** of the category (e.g., `category.ToString()`, an enumeration written as a string, or a string in a configuration) sees `PermissionDenied`. The problem details format is not affected: it carries the category as the `status`, not by name.

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
* `WithErrorDetail(...)`, which takes the same arguments in the same order and **returns a copy** with the detail appended (the parameters `messageId` and `namedValues` are renamed to `detailTemplateId` and `detailParameters`, so update calls that use named arguments),
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

Dedicated converter packages are coming soon: [Maple.Result.Serialization.SystemTextJson](https://github.com/TomMaple/Maple.Result.Serialization.SystemTextJson) and [Maple.Result.Serialization.NewtonsoftJson](https://github.com/TomMaple/Maple.Result.Serialization.NewtonsoftJson). Until they are available, if you need to read these types (e.g., as Azure Durable Functions activity results, Service Bus messages or cached values), map them to your own DTOs at the boundary:

```csharp
public sealed record ErrorDto(ErrorCategory Category, string TypeUri, string Title, string? Detail, string? InstanceUri);

public static ErrorDto ToDto(Error error) =>
    new(error.Category, error.TypeUri.Value, error.Title, error.Detail, error.InstanceUri?.Value);

public static Error FromDto(ErrorDto dto) =>
    new ErrorBuilder(dto.Category, ErrorUri.Parse(dto.TypeUri), dto.Title)
        .WithDetail(dto.Detail)
        .WithInstanceUri(dto.InstanceUri is null ? null : ErrorUri.Parse(dto.InstanceUri))
        .Build();
```

`ErrorUri.Parse` validates the text as the factory method of its kind does, and throws a `UriFormatException` for an invalid one; `ErrorUri.TryParse` returns `false` instead.

> [!NOTE]
> `Maple.Json.ObjectAsPrimitiveConverter`, which the 2.x documentation suggested for reading metadata values, is no longer relevant to `Maple.Result`.

### Changed JSON output
The JSON written by the default serializers changed as follows, and a failed `Result<T>` can no longer be written without a converter:

| | 2.x | 3.0 |
| --- | --- | --- |
| Failed `Result<T>` | `{"Error":{…}}` | throws (`Value` throws on a failed result) |
| `Error` | no `Metadata` member | always has `"Metadata":{…}` (`{}` when empty) |
| `TypeUri` and `InstanceUri` of an `Error` | a string, e.g., `"TypeUri":"tag:example.com,2026:errors:not-found"` | an object, e.g., `"TypeUri":{"Value":"tag:example.com,2026:errors:not-found"}`; `"InstanceUri":null` when there is none |
| `DetailTemplated` of an `Error`, and a `TemplatedMessage` | `"Params":{…}` | `"Parameters":{…}` (see [`TemplatedMessage.Params` renamed to `Parameters`](#templatedmessageparams-renamed-to-parameters)) |
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
| Template parameter names that differ only in case, e.g., `("Min", 3), ("min", 5)`: one of the values was ignored by a template engine that ignores case, and a naming policy wrote both as the same member of `params` (the rule of the metadata keys) | `ArgumentException` |
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

### Nullable parameters
The parameters of templated messages are now `object?` instead of `object`:
* the parameters of the `TemplatedMessage` constructors, which the `Error` factory methods now take instead of their `detailNamedValues` (see [The factory methods take a `TemplatedMessage`](#the-factory-methods-take-a-templatedmessage)),
* the `detailParameters` parameter of `WithErrorDetail` (formerly `namedValues` of `AddDetail`),
* `TemplatedMessage.Parameters`, formerly `Params` (`IReadOnlyDictionary<string, object?>?`).

Existing calls still compile. With nullable reference types enabled, passing a `Dictionary<string, object>` produces warning CS8620; declare it as `Dictionary<string, object?>` instead. Code that reads `Parameters[...]` may get nullable warnings (CS8602) and should handle `null` values.

`TemplatedMessage` now copies the parameters into its own read-only dictionary with an ordinal (case-sensitive) comparer, so changing the source dictionary later no longer changes the message.

`Parameters` is `null` whenever a message has no parameters: an empty dictionary, an empty collection (`[]`) or no parameters at all. In 2.x an empty dictionary was kept, so a message with an empty dictionary did not equal the same message without parameters; now they are equal. Code that told an empty dictionary apart from `null` only needs the `null` check.

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
* **`Deconstruct` is gone.** `var (id, parameters) = message;` and positional patterns such as `message is ("errors.length", var p)` no longer compile; use a property pattern instead: `message is { TemplateId: "errors.length", Parameters: var p }`.
* **Repeated or `null` parameter names, and names that differ only in case, throw** (see [Stricter validation](#stricter-validation)).

The parameters of `WithErrorDetail`, `ErrorBuilder.AddErrorDetail` and `ErrorBuilder.WithDetailTemplated` are now `params ReadOnlySpan<(string Key, object? Value)>` instead of `params (string Key, object? Value)[]`, so a call no longer allocates an array:
* inline values, arrays and collection expressions still compile; recompile the calling code (the signatures changed);
* an explicit `null` is now an empty list (e.g., `WithDetailTemplated("id", null)` no longer throws);
* a call with parameters cannot appear inside an expression tree (e.g., a Moq `Setup(x => …)` or an EF query); build the error outside the expression.

### `TemplatedMessage.Params` renamed to `Parameters`
The values of a message template have one name in 3.0, *parameters*, in the property and in the parameters of the methods:
* `TemplatedMessage.Params` is now `TemplatedMessage.Parameters`. Code that reads `Params`, and property patterns such as `message is { Params: var p }`, no longer compile; rename them.
* The named argument for the values of `WithErrorDetail` (formerly `namedValues` of `AddDetail`) is `detailParameters:`.
* Without the converters of the related packages, the default serializers write the member as `"Parameters"` (or `"parameters"` with a camel-case naming policy) instead of `"Params"`, and `ToString()` writes `Parameters = { … }`. The problem details format of the related packages keeps the member `params`.

```csharp
// 2.x
if (error.DetailTemplated?.Params is { } values) { … }
error.AddDetail("/name", "Too short.", messageId: "errors.length", namedValues: ("min", 3));

// 3.0
if (error.DetailTemplated?.Parameters is { } values) { … }
error = error.WithErrorDetail("/name", "Too short.", detailTemplateId: "errors.length", detailParameters: ("min", 3));
```

> [!NOTE]
> Code written against 3.0.0-rc.1: the parameters `namedValues` of `ErrorBuilder.WithDetailTemplated(String, ReadOnlySpan<(String, Object)>)`, and `detailNamedValues` of `WithErrorDetail` and `ErrorBuilder.AddErrorDetail`, are renamed to `parameters` and `detailParameters`. Only calls with named arguments are affected.

### The factory methods take a `TemplatedMessage`
Each `Error` factory method (e.g., `Error.NotFound`) had two overloads with a template ID and parameters (`detailTemplateId`, and `detailNamedValues` as tuples or as key-value pairs). There is now one per category, which takes the templated detail as one `TemplatedMessage`:

```csharp
public static Error NotFound(ErrorUri typeUri, string title, string? detail = null,
    ErrorUri? instanceUri = null, TemplatedMessage? detailTemplated = null)
```

Two optional strings close together let a template ID silently become the detail: `Error.NotFound(type, "Order not found", "errors.order.notFound")` compiles, and the detail is the template ID. A template ID now has to be wrapped, so it cannot be passed where the detail goes.

```csharp
// 2.x
var error = Error.NotFound(type, "Order not found", "The order does not exist.", null,
    "errors.order.notFound", ("orderId", id));
var error = Error.NotFound(type, "Order not found",
    detailTemplateId: "errors.order.notFound", detailNamedValues: new Dictionary<string, object> { ["orderId"] = id });

// 3.0
var error = Error.NotFound(type, "Order not found", "The order does not exist.", null,
    new TemplatedMessage("errors.order.notFound", ("orderId", id)));
var error = Error.NotFound(type, "Order not found",
    detailTemplated: new("errors.order.notFound", new Dictionary<string, object?> { ["orderId"] = id }));
```

Calls that pass a template ID or parameters to a factory method, by position or by name (`detailTemplateId:`, `detailNamedValues:`), no longer compile. An invalid template ID or parameter throws from the `TemplatedMessage` constructor, before the factory method is called, and the `ParamName` of the exception is `templateId` or `parameters` instead of `detailTemplateId` or `detailNamedValues`.

`WithErrorDetail`, `ErrorBuilder.AddErrorDetail` and `ErrorBuilder.WithDetailTemplated` still take a template ID and parameters (`detailParameters` and `parameters`).

### Other behaviour changes
* **`Result.Success()`** returns the same, shared instance on every call. It is immutable, so this is safe; only code that relied on reference inequality of two successful results is affected.
* **`ToString()`** of `Result` and `Result<T>` prints only the `Value` of a successful result or the `Error` of a failed one (e.g., `Result { Value = 42 }`, `Result { Error = Error { … } }`); 2.x printed every property, including the default `Value` of a failed result.
* **`ToString()`** of `Error` and `TemplatedMessage` now prints the contents of `ErrorDetails`, `Metadata` and `Parameters` (instead of their type names), and `ErrorUri.ToString()` returns just the *URI* (instead of `ErrorUri { Value = … }`). Update tests or log parsers that depend on the old text.
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

The interface also declares `TryGetError(out Error? error)`. Your own types can no longer implement it (see [`IOperationResult` is closed](#ioperationresult-is-closed)).

The old name clashed with `Microsoft.AspNetCore.Http.IResult`. ASP.NET Core web projects import `Microsoft.AspNetCore.Http` implicitly, and Minimal API endpoints commonly return `IResult`, so adding `using Maple.Result;` caused CS0104 (ambiguous reference) in endpoint signatures. If you added an alias to work around it (e.g., `using IResult = Microsoft.AspNetCore.Http.IResult;`), you can remove it.

```csharp
// 2.x
void Log(Maple.Result.IResult result) { … }

// 3.0
void Log(IOperationResult result) { … }
```

### `IOperationResult` is closed
`Result` and `Result<T>` are the only implementations of [`IOperationResult`](Reference/IOperationResult/IOperationResult.md). In 2.x, any type could implement `IResult`.

* A type of your own that implements the interface no longer compiles (CS0535: *'…' does not implement interface member 'IOperationResult.CannotBeImplementedOutsideTheLibrary()'*). The member is internal, so it cannot be implemented outside the library.
* A mocking library can no longer create a mock of the interface: the type that it generates at run time fails to load (e.g., a `TypeLoadException`).

The extension methods of the interface (`IfError`, `Else` and their asynchronous forms) rely on the two implementations: a failed result always has an `Error`. An implementation that reported a failure without an `Error` made `IfError` call its action with `null`. A closed interface can also get new members in a minor version.

Using the interface does not change: it can still be the type of a parameter or a variable, a type argument and a generic constraint.

```csharp
// 2.x
public sealed class Outcome : IResult { … }
var result = new Mock<IResult>();

// 3.0: return and create the results of the library
Result outcome = Result.FromError(error);
IOperationResult result = Result.Success();
```

### `ErrorUri.None()` is a property
`ErrorUri.None()` is now a static property, `ErrorUri.None`, like `CancellationToken.None` and `Guid.Empty`: it is a fixed value (`about:blank`, equal to `default(ErrorUri)`). Calls with parentheses no longer compile (CS1955: *Non-invocable member cannot be used like a method*); drop the parentheses.

```csharp
// before
var error = Error.NotFound(ErrorUri.None(), "Not Found");

// after
var error = Error.NotFound(ErrorUri.None, "Not Found");
```

### `Error.TypeUri` and `Error.InstanceUri` are an `ErrorUri`
`Error.TypeUri` is now an `ErrorUri` and `Error.InstanceUri` an `ErrorUri?` (both were a `string`), the type that every factory method and the `ErrorBuilder` take. An error keeps the `ErrorUri` it was created with, so its type can be compared with the `ErrorUri` that defines it. The text is the `Value` of the `ErrorUri`; `ToString()` returns it too, so string interpolation and the `ToString()` of the error write the same text as before. Equality is unchanged: two `ErrorUri` values are equal when their texts are equal (ordinal).

Code that reads the properties as a `string` no longer compiles (CS0029, or CS0019 for a comparison with a `string`), and a constant pattern on the type (`error is { TypeUri: "…" }`) is CS0029 too:

```csharp
// before
string type = error.TypeUri;
string? instance = error.InstanceUri;
if (error.TypeUri == "tag:example.com,2026:errors:not-found") { … }
if (error is { TypeUri: "tag:example.com,2026:errors:not-found" }) { … }

// after
string type = error.TypeUri.Value;
string? instance = error.InstanceUri?.Value;
if (error.TypeUri == OrderErrors.NotFoundType) { … }        // OrderErrors.NotFoundType is an ErrorUri
if (error.TypeUri.Value is "tag:example.com,2026:errors:not-found") { … }
```

The default JSON serializers write each of them as an object with a `Value` member (see [Changed JSON output](#changed-json-output)).

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

Both properties keep the nullable annotations: `Error` is not `null` after `IsError` is `true` or `IsSuccess` is `false`. To read the value of a successful result, use `result.TryGetValue(out var value)`.

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

### The analyzer reports the calls that compile but changed
The next sections change what the extension methods do with the delegates that they are given. In 3.0, `IfSuccess`, `IfError` and `Switch` take any action, and a lambda that returns a value or a task converts to an action, so the compiler alone cannot tell a side effect from a 2.x call that transformed the result. A 2.x call that is affected ends in one of four ways:

* **It does not compile.** The error is the compiler’s own (e.g., CS0411 for `Match` with two actions) and does not name the method to use. Each section gives the errors and the replacement; [What the compiler reports by itself](Analyzers/README.md#what-the-compiler-reports-by-itself) lists them.
* **It compiles, and the analyzer reports an error** (MR0001 to MR0004): a task that nobody awaits, or a result that nobody checks. These errors cannot be suppressed as a warning can.
* **It compiles, and the analyzer reports a warning** (MR0005 and MR0006).
* **It compiles without a message**, in one case: a function that returns a value of another type passed to `IfSuccess` or `IfError` (see the next section).

The analyzer is part of the `Maple.Result` package, so there is nothing to install. Keep it switched on while you migrate: with the analyzers off, the calls of the second and the third kind compile without a message. See [Analyzer](Analyzers/README.md) for the rules, the code fixes and the configuration.

### Transforming `IfSuccess` and `IfError` renamed to `Then` and `Else`
In 2.x, `IfSuccess` and `IfError` did two different things under one name: with an action they ran a side effect and returned the result unchanged; with a function they transformed the result (`IfSuccess`) or replaced a failed one (`IfError`). The overload, and so the behaviour, depended on whether the lambda returned a value: `result.IfSuccess(x => set.Add(x))` (a side effect whose expression returns a `bool`) turned a `Result<T>` into a `Result<bool>`, and `result.IfError(e => Interlocked.Increment(ref failures))` turned a failure into a success.

In 3.0 the name says what the call does:
* `IfSuccess`, `IfSuccessAsync`, `IfError` and `IfErrorAsync` run a side effect and return the result unchanged;
* `Then` and `ThenAsync` transform or chain a successful result with a function that returns a value, a `Result`, a `Result<T>` or an `Error` (the overloads, the flattening of a returned result and the handling of a returned `Error` are the same as before);
* `Else` and `ElseAsync` replace a failed result with the result that the function returns.

A 2.x call that transforms or replaces the result still compiles in 3.0 when the body of its lambda is a call: the function runs as a side effect, its value is ignored, and the call returns the original result. What reports the call depends on what the function returns:

| The function returns | A 2.x call in 3.0 |
| --- | --- |
| A `Result`, a `Result<T>` or an `Error`, or a task of one (e.g., `user => CreateOrder(user)`) | An error of the analyzer ([MR0004](Analyzers/MR0004.md)): a failure of the function would be lost. Its code fix renames `IfSuccess` to `Then` and `IfSuccessAsync` to `ThenAsync`, and `IfError` to `Else` and `IfErrorAsync` to `ElseAsync` when the function returns an `Error` or the type of the result itself. |
| A value of the type of the result’s value, or a task of it (e.g., `x => Normalize(x)` on a `Result<int>`) | A warning of the analyzer ([MR0005](Analyzers/MR0005.md)), when the result of the call is used. Its code fix renames the method. |
| A value of another type (e.g., `user => CountOrders(user)` on a `Result<User>`) | **No message for the call.** Its type is the type of the original result (a `Result<User>`, not a `Result<int>`), so the code that uses the transformed result usually no longer compiles (e.g., CS0029 where it is assigned to a `Result<int>`). Rename the method. |
| A value, as an expression that is not a call (e.g., `x => x + 1`, `user => user.Name`, `e => 0`) | The compiler error CS0201. Rename the method. |

An action passed to `Then` or `Else` does not compile: the error is the compiler’s own (e.g., CS0029, “Cannot implicitly convert type ‘void’ to ‘Result’”) and does not name `IfSuccess` or `IfError`. A function that returns a task without a value passed to `ThenAsync` on a task of a result compiles and keeps the task in the result without awaiting it: the analyzer reports it as an error ([MR0001](Analyzers/MR0001.md)); use `IfSuccessAsync`. In `ElseAsync`, and in `ThenAsync` on a result, such a function does not compile (CS0029 and CS1662); use `IfErrorAsync` or `IfSuccessAsync`.

```csharp
// 2.x; in 3.0: MR0004 on both lines, as the functions return a result
var orderResult = userResult.IfSuccess(user => CreateOrder(user));
var guestResult = userResult.IfError(error => Result<User>.FromValue(User.Guest));

// 3.0
var orderResult = userResult.Then(user => CreateOrder(user));
var guestResult = userResult.Else(error => Result<User>.FromValue(User.Guest));

// 3.0: a side effect whose expression returns a value needs no statement lambda
var orderResult = userResult
    .IfSuccess(user => seenIds.Add(user.Id))
    .Then(user => CreateOrder(user));
```

An assignment is a side effect too: `IfSuccess(user => lastUser = user)` and `IfError(error => lastError = error)` compile, and the analyzer does not report them. In 2.x the first one transformed the result into a `Result<User>` of the assigned value, which is the same value. An increment is reported when its type is the type of the result’s value, as any other function is (MR0005): in 2.x, `IfSuccess(x => total++)` on a `Result<int>` returned a result with the total. For the side effect, use a statement lambda:

```csharp
// 3.0: MR0005 on a Result<int>, when the result of the call is used
var checkedResult = countResult.IfSuccess(count => total++);

// 3.0: a statement lambda is a side effect, and is not reported
var checkedResult = countResult.IfSuccess(count => { total++; });
```

### `Match` is a fold; `Switch` runs two actions
In 2.x `Match` and `MatchAsync` had three meanings on one name: two **functions** that return a value, two functions that return a **result**, and two **actions** for a side effect, which returned the result; a **success function** with an **error action** (e.g., `Match(user => CreateOrder(user), error => Log(error))`) transformed the result, as `Then(…).IfError(…)` does, and the same call with a statement lambda tapped it, by the return type of the lambda. Each delegate could also return a `Task` or a `ValueTask`.

In 3.0 `Match` and `MatchAsync` are a **fold**: two functions that return one type, which the call returns as the function returned it (a value, a result or a task). Two actions move to the new **`Switch`** and **`SwitchAsync`**, which return nothing:

* **Two actions** do not compile: CS0411 for `Match` (the compiler cannot infer the return type), and CS0029 with CS1662 for `MatchAsync`. The errors do not name the replacement: use `Switch` (`SwitchAsync` on a task of a result). To keep chaining after a side effect, use `IfSuccess(…).IfError(…)`, which return the result.
* **A success function with an error action** does not compile either (CS0029 and CS1662): use `Then(…).IfError(…)` to transform the value and handle the error.
* **There is no `MatchAsync` on a `Result` or a `Result<T>`**: `Match` returns the task of the function that ran, which you await. `MatchAsync` takes a `Task` or a `ValueTask` of a result, with two synchronous functions or two functions that return a task.
* **Two functions that return a result** need no overload of their own: `Match(x => Validate(x), e => e)` is a `Result<T>`, because an `Error` converts to it. With explicit type arguments, the second one is the whole return type: `Match<int, Result<int>>(…)` instead of `Match<int, int>(…)`.
* **A function that returns `null`** gives `null`, also a `null` result: the fold does not know that its type is a result (see [Null results and null arguments of the extension methods](#null-results-and-null-arguments-of-the-extension-methods)).
* **In `Switch`**, an action that returns a value is a side effect (e.g., `Switch(x => set.Add(x), e => count++)`), and a lambda that only throws is fine. An action that returns a `Task`, a `Task<T>`, a `ValueTask` or a `ValueTask<T>` compiles, but its task is not awaited: the analyzer reports it as an error ([MR0002](Analyzers/MR0002.md)). Use `Match` with two functions that return a task.

| Call in 2.x | In 3.0 |
| --- | --- |
| `result.Match(x => Log(x), e => Log(e))` (returned the result) | `result.Switch(x => Log(x), e => Log(e))` (returns nothing), or `result.IfSuccess(x => Log(x)).IfError(e => Log(e))` to keep the result |
| `await task.MatchAsync(x => Log(x), e => Log(e))` | `await task.SwitchAsync(x => Log(x), e => Log(e))` |
| `await result.MatchAsync(x => CountAsync(x), e => CountAsync(0))` | `await result.Match(x => CountAsync(x), e => CountAsync(0))` |
| `await result.MatchAsync(x => SaveAsync(x), e => LogAsync(e))` (returned the result) | `await result.Match(x => SaveAsync(x), e => LogAsync(e))` (returns nothing) |
| `await task.MatchAsync(x => SaveAsync(x), e => LogAsync(e))` (returned the result) | the same call; it returns nothing |
| `result.Match(x => CreateOrder(x), e => Log(e))` (a success function and an error action) | `result.Then(x => CreateOrder(x)).IfError(e => Log(e))` |
| `result.Match(x => Validate(x), e => e)` | unchanged: a `Result<T>` |
| `result.Match(x => x.Name, e => "unknown")`, `result.Match(x => x, e => throw …)` | unchanged |
| `result.Match<T, TNext>(…)` with explicit type arguments for functions that return a `Result<TNext>` | `result.Match<T, Result<TNext>>(…)` |

```csharp
// 2.x: two actions, returned the result; 3.0: a compile error (CS0411)
userResult.Match(user => Log(user), error => Log(error));

// 3.0: two actions, nothing returned…
userResult.Switch(user => Log(user), error => Log(error));
await GetUserAsync(id).SwitchAsync(user => Log(user), error => Log(error));

// …or a side effect that keeps the result for chaining
var orderResult = userResult.IfSuccess(user => Log(user)).IfError(error => Log(error)).Then(user => CreateOrder(user));

// 2.x: a Result<Order>; 3.0: a compile error (CS0029, CS1662)
var orderResult = userResult.Match(user => CreateOrder(user), error => Log(error));

// 3.0: transform, then run the side effect
var orderResult = userResult.Then(user => CreateOrder(user)).IfError(error => Log(error));

// 2.x: MatchAsync on a result; 3.0: Match returns the task of the function that ran
int orderCount = await userResult.Match(user => CountOrdersAsync(user), error => Task.FromResult(0));

// unchanged: two functions that return one type
string name = userResult.Match(user => user.Name, error => "unknown");
Result<Address> addressResult = userResult.Match(user => FindAddress(user), error => error);
```

### `Then` and `Match` with an `Error` or a task
Two kinds of calls compiled in 2.x but did not do what they looked like:

* **A function that returns an `Error`** bound to the generic overload and returned a **successful** `Result<Error>`. In 3.0, `Then` and `ThenAsync` have overloads for such functions that return a **failed** `Result` with that error (`Match` returns what its functions return: `Match(x => Validate(x), e => e)` is a `Result<T>`; see [`Match` is a fold; `Switch` runs two actions](#match-is-a-fold-switch-runs-two-actions)). The same applies to a LINQ query that selects an `Error` (`from user in GetUser(id) select Error…`). Code that read `.Value` of the `Result<Error>` no longer compiles; if you need it, specify the type arguments.
* **A function that returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>`** passed to `IfSuccess` (now `Then`), or to `Match` with an error action, was wrapped in a `Result<Task>` and **never awaited**. In 3.0 such a function still compiles in `Then`, with the same result, and in `IfSuccess`, which discards the task; the analyzer reports both as errors ([MR0001](Analyzers/MR0001.md) for `Then`, [MR0002](Analyzers/MR0002.md) for `IfSuccess`). In `Match` with an error action it does not compile (CS0029 and CS1662). Use the asynchronous method. To keep the task in the result on purpose, name the type arguments (e.g., `Then<Task>(…)`) or return the task in a result; the analyzer does not report these.

```csharp
// 2.x (IfSuccess): a successful Result<Error>; 3.0: a failed Result with the error
var result = userResult.Then(user => Error.Conflict(typeUri, "The user already exists."));
var result = from user in userResult select Error.Conflict(typeUri, "The user already exists.");

// 2.x: the task was never awaited; 3.0: an error of the analyzer (MR0002, MR0001)
var result = Result.Success().IfSuccess(() => SaveAsync());
var result = Result.Success().Then(() => SaveAsync());

// 3.0: await it…
var result = await Result.Success().IfSuccessAsync(() => SaveAsync());

// …or keep the task on purpose
Result<Task> result = Result.Success().Then(() => Result.FromValue(SaveAsync()));
```

### `IfError` and `Match` with an error action that returns a task
In 2.x, an error action that returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>` (e.g., `e => LogAsync(e)`) was accepted as an `Action<Error>` by `IfError` and by the `Match` overloads with an error action, and its task was **never awaited**. An `async` lambda became `async void`, so an exception thrown in it ended the process. In 3.0 `IfError` still takes an `Action<Error>`, so such a call compiles as it did, and the analyzer reports it as an error ([MR0002](Analyzers/MR0002.md)). The `Match` overloads with an error action are gone, so such a call of `Match` does not compile. Use `IfErrorAsync`, or `Match` with two functions that return a task, or, to start the task without awaiting it on purpose, discard it explicitly in a statement lambda.

`Match` with two functions that return a task returns the task of the function that ran: `await result.Match(() => SaveAsync(), e => LogAsync(e))`. An `async` success lambda with a synchronous error action (e.g., `Match(async () => await SaveAsync(), e => { })`) was already a compile error, and still is (CS1643). In `Switch`, an action that returns a task is reported by the analyzer (MR0002).

```csharp
// 2.x: the task was never awaited (async void for an async lambda)
// 3.0: an error of the analyzer (MR0002) for IfError, a compile error for Match
var result = userResult.IfError(error => LogAsync(error));
var result = userResult.Match(user => user.Id, async error => await LogAsync(error));

// 3.0: await it…
var result = await userResult.IfErrorAsync(error => LogAsync(error));

// …or start it without awaiting on purpose
var result = userResult.IfError(error => { _ = LogAsync(error); });
```

### LINQ queries that select a task
In 2.x, a query whose `select` returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>` wrapped the task in a successful result (e.g., `Result<Task<int>>`) and **never awaited** it. This applies to every source (`Result<T>`, `Task<Result<T>>`, `ValueTask<Result<T>>`) and to queries with several `from` clauses. In 3.0 such a query compiles as it did, and the analyzer reports it as an error ([MR0001](Analyzers/MR0001.md)). Chain the operation in a `from` clause whose function returns a `Task<Result<T>>` (or `ValueTask<Result<T>>`), or use `ThenAsync`. To keep the task in the result on purpose, select it in a result (`select Result.FromValue(…)`), or call `Select` or `SelectMany` with explicit type arguments (e.g., `Select<T, Task>(…)`); the analyzer does not report these.

```csharp
// 2.x: a Result<Task<int>> whose task was never awaited; 3.0: an error of the analyzer (MR0001)
var result = from user in userResult select SaveAsync(user);

// 3.0: await it…
var result = await userResult.ThenAsync(user => SaveAsync(user));

// …or keep the task on purpose
Result<Task<int>> result = from user in userResult select Result.FromValue(SaveAsync(user));
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
* **`Match` and `MatchAsync`** are the exception: they are a fold and return what the function returns, a `null` result included (see [`Match` is a fold; `Switch` runs two actions](#match-is-a-fold-switch-runs-two-actions)). A function of `MatchAsync` that returns a `null` task still throws.

```csharp
// 2.x: returned null; 3.0: InvalidOperationException
var result = Result.Success().Then(() => (Result)null!);

// 2.x: the exception surfaced at await; 3.0: thrown by the call
var task = result.ThenAsync((Func<Task<Result>>)null!);
```

### Extension packages
The current (2.x-based) version of `Maple.Result.Extensions.HttpClient` calls an internal method of `Maple.Result` 2.x that no longer exists, so using it with `Maple.Result` 3.0 fails at runtime. Upgrade all `Maple.Result.Extensions.*` packages to versions built for 3.0 together with `Maple.Result`, and do not mix 2.x and 3.0 packages. The published versions of `Maple.Result.Extensions.AspNetCore`, `.MinimalApi`, `.Functions.Worker`, `.Functions.Worker.AspNetCore` and `.HttpClient` are built for 2.x; see [Related packages](../README.md#related-packages) for the list and the status of each package.

## New features
* **An analyzer in the package** — six rules (MR0001 to MR0006) report the calls that compile but lose a task or a failure, e.g., `result.Then(x => SaveAsync(x))`, whose task nobody awaits, or `await SaveAsync(order);`, which drops a failed `Result`; most of them have a code fix. The analyzer comes with the `Maple.Result` package. See [Analyzer](Analyzers/README.md) and [The analyzer reports the calls that compile but changed](#the-analyzer-reports-the-calls-that-compile-but-changed).
* **`ErrorBuilder`** — a mutable builder of an immutable `Error`. Create it with `new ErrorBuilder(category, typeUri, title)`, a static method per category (e.g., `ErrorBuilder.Validation(...)`) or `error.ToBuilder()`; set the members with `With…` / `AddErrorDetail` methods; call `Build()`. Every argument is validated when it is passed, so `Build()` never throws. See [ErrorBuilder](Reference/ErrorBuilder/ErrorBuilder.md).
* **`Error.Metadata`** — machine-readable, response-level values (RFC 9457 *extension members*), with `WithMetadata()`, `WithoutMetadata()` (one key or all) and `TryGetMetadata<T>()`. `TryGetMetadata<T>()` is strict: it returns `true` only when the stored value is a `T` (a stored `int` is not read as `long`), and never for a `null` value. See [Metadata](Reference/Error/Error_Metadata.md).
* **`WithMetadata` with tuples** — `error.WithMetadata(("limit", 100), ("remaining", 0))` on `Error` and `ErrorBuilder` adds several entries in one call.
* **`Error.WithErrorDetail()`** and **`Error.ToBuilder()`** — see above.
* **`ErrorCategory.LimitExceeded`** — see above.
* **`HttpErrorTitles` and `HttpErrorUris`** (namespace `Maple.Results.Http`) — the phrase of the default status code of each category (e.g., `HttpErrorTitles.NotFound` is “Not Found”), the title that *RFC 9457* asks for with `about:blank`, and a type *URI* that links to the section of that status code (e.g., `Error.NotFound(HttpErrorUris.NotFound, HttpErrorTitles.NotFound)`). See [HttpErrorTitles](Reference/Http/HttpErrorTitles.md) and [HttpErrorUris](Reference/Http/HttpErrorUris.md).
* **`ProblemDetailsMembers`** (namespace `Maple.Results.Http`) — the member names of the problem details object (`type`, `title`, `status`, …), shared by the packages that write and read problem details. See [Problem Details Format](problem-details.md).
* **`Result.Try()` and `Result.TryAsync()`** — run code that can throw and turn the exception into a failed result, with the error that a required mapper creates (e.g., `Result.Try(() => repository.Load(id), ex => …)`); an `OperationCanceledException` is rethrown, a function that returns a `Result`, a `Result<T>` or an `Error` is flattened, and a mapper that returns `null` declines the exception (it is rethrown). See [Try](Reference/Result/Result_Try.md) and [TryAsync](Reference/Result/Result_TryAsync.md).
* **`TryGetValue` and `TryGetError`** — `if (!GetUser(id).TryGetValue(out var user, out var error)) return error;` reads the value of a `Result<T>` without an exception and gives the early return in one statement; `TryGetError(out var error)` on `Result`, `Result<T>` and `IOperationResult` does the same for the error. See [TryGetValue](Reference/ResultT/ResultT_TryGetValue.md) and [TryGetError](Reference/Result/Result_TryGetError.md).
* **`ErrorUri.Other()`** — an absolute *URI* whose scheme has no factory method of its own (e.g., `mailto:`), for type and instance URIs read from other services; `Parse` and `TryParse` fall back to it. An `about` *URI* other than `about:blank` (e.g., `about:invalid`) is kept by it too. See [Other](Reference/ErrorUri/ErrorUri_Other.md).
* **`ErrorUri.Parse()`**, **`ErrorUri.TryParse()`** and **`IParsable<ErrorUri>`** — create an `ErrorUri` when the kind is not known in advance: the kind is detected from the value (`about:blank`, `/…`, `tag:`, `urn:`, `http:`/`https:`) and validated as by its factory method. `about:blank` is recognized in any case (e.g., `about:BLANK`) and read as `ErrorUri.None`. See [Parse](Reference/ErrorUri/ErrorUri_Parse.md).
* **`ErrorUri.Urn()`** and **`ErrorUri.Relative()`** — `urn:` types and instances (*RFC 8141*), and relative references with the full path (e.g., the *RFC 9457* example `/account/12345/msgs/abc`). See [ErrorUri](Reference/ErrorUri/ErrorUri.md).
* **Synchronous delegates after an asynchronous call** — `ThenAsync`, `ElseAsync`, `IfSuccessAsync`, `IfErrorAsync` and `MatchAsync` on a `Task` or `ValueTask` of a result also take synchronous delegates, e.g., `await GetUserAsync(id).ThenAsync(user => user.Name)`. A function that returns the other kind of task (a `ValueTask` on a `Task` of a result, a `Task` on a `ValueTask` of a result) is awaited, and the kind of the receiver is kept (e.g., `await GetUserAsync(id).ThenAsync(user => _cache.GetProfileAsync(user.Id))` with a `ValueTask` cache); and one synchronous and one asynchronous function of `MatchAsync` have no common return type, so the compiler rejects them. See [ThenAsync](Reference/Extensions/ThenAsyncExtensions_ThenAsync.md).
* **`ElseAsync` with a value or an `Error`** — as `Else` takes a function that returns a value or an `Error` through the implicit conversions, `ElseAsync` takes one that returns it in a task (e.g., `await GetCountAsync(id).ElseAsync(async error => 0)` or `ElseAsync(async error => error.WithMetadata("retried", true))`), on a result and on a task of a result, for both kinds of task; these calls were compile errors. See [ElseAsync](Reference/Extensions/ElseAsyncExtensions_ElseAsync.md).
* **`Switch` and `SwitchAsync`** — run one of two actions depending on whether a result is successful and return nothing (e.g., `result.Switch(user => Log(user), error => Log(error))`, or `await GetUserAsync(id).SwitchAsync(…)`); an action that returns a task is reported by the analyzer (MR0002). See [Switch](Reference/Extensions/SwitchExtensions_Switch.md) and [`Match` is a fold; `Switch` runs two actions](#match-is-a-fold-switch-runs-two-actions).
* **The non-generic `Result` in LINQ queries** — a step that returns a `Result` (e.g., a check) can be any `from` clause, first or in the middle (`from _ in EnsureActive(user)`), synchronous or asynchronous; its range variable is the successful `Result`. See [SelectMany](Reference/Extensions/LinqExtensions_SelectMany.md).
* **Mixed LINQ queries** — synchronous and asynchronous `from` clauses can be mixed in one query (e.g., `from user in GetUserAsync(id) from address in GetAddress(user) select …`); the query is then asynchronous and has to be awaited. See [SelectMany](Reference/Extensions/LinqAsyncExtensions_SelectMany.md).
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
| `error.TypeUri` / `error.InstanceUri` read as a `string` | `error.TypeUri.Value` / `error.InstanceUri?.Value` |
| `error.TypeUri == "…"`, `error is { TypeUri: "…" }` | `error.TypeUri == SomeType` (an `ErrorUri`), or `error.TypeUri.Value is "…"` |
| `result.IfSuccess(x => …)` (a function returning a value, a result or an error) | `result.Then(x => …)` |
| `await result.IfSuccessAsync(x => …)` (a function returning a value, a result or an error in a task) | `await result.ThenAsync(x => …)` |
| `result.IfError(e => …)` (a function returning the replacement result) | `result.Else(e => …)` |
| `await result.IfErrorAsync(e => …)` (a function returning the replacement result in a task) | `await result.ElseAsync(e => …)` |
| `result.IfSuccess(x => set.Add(x))`, `result.IfSuccess(x => last = x)` (a side effect whose expression returns a value, which 2.x took for a transformation) | unchanged: the value is ignored, and the call returns the original result |
| `result.IfSuccess(x => count++)` on a `Result<int>` (a side effect that returns the type of the result’s value; the analyzer warns, MR0005) | `result.IfSuccess(x => { count++; })` |
| `result.IfSuccess(() => SaveAsync())` (a function returning a task) | `await result.IfSuccessAsync(() => SaveAsync())` |
| `result.IfError(e => LogAsync(e))` (an error action returning a task) | `await result.IfErrorAsync(e => LogAsync(e))` |
| `result.Match(x => Log(x), e => LogAsync(e))` (an error action returning a task) | `await result.Match(x => LogAsync(x), e => LogAsync(e))` |
| `from x in result select SaveAsync(x)` (a selector returning a task) | `await result.ThenAsync(x => SaveAsync(x))` |
| `result.IfSuccess(x => Error…)` relied on as a `Result<Error>` | `result.Then<T, Error>(x => Error…)` |
| `from x in result select Save(x)` relied on as a `Result<Result<T>>` | `result.Select<T, Result<TNext>>(x => Save(x))` |
| `result.Match(x => …, e => Log(e))` (a success function and an error action) | `result.Then(x => …).IfError(e => Log(e))` |
| `await result.MatchAsync(x => …, e => LogAsync(e))` (a success function and an error action) | `await result.ThenAsync(x => …).IfErrorAsync(e => LogAsync(e))` |
| `result.Match(x => Log(x), e => Log(e))` (two actions) | `result.Switch(x => Log(x), e => Log(e))`, or `result.IfSuccess(x => Log(x)).IfError(e => Log(e))` to keep the result |
| `await task.MatchAsync(x => Log(x), e => Log(e))` (two actions) | `await task.SwitchAsync(x => Log(x), e => Log(e))` |
| `await result.MatchAsync(x => …Async(x), e => …Async(e))` (`MatchAsync` on a result) | `await result.Match(x => …Async(x), e => …Async(e))` |
| `result.Match<T, TNext>(…)` (functions returning a `Result<TNext>`) | `result.Match<T, Result<TNext>>(…)` |
| `WithErrorDetail("Email", …)` / `AddErrorDetail(nameof(request.Email), …)` (a pointer that is not a JSON Pointer) | `WithErrorDetail("/email", …)` / `AddErrorDetail("/email", …)` |
| `Dictionary<string, object>` passed as the parameters of a template | `Dictionary<string, object?>` |
| `using Maple.Result;` | `using Maple.Results;` |
| `using Maple.Result.Extensions;` | `using Maple.Results.Extensions;` |
| `using Maple.Result.Extensions.ValueTasks;` | `using Maple.Results.Extensions;` (one namespace for both kinds of task) |
| `ThenTaskAsyncExtensions`, `ThenValueTaskAsyncExtensions` (and the other verbs) | `ThenAsyncExtensions` |
| `ValueTask<Result<int>> r = result.ThenAsync(async x => …);` | `ValueTask<Result<int>> r = result.ThenAsync(async ValueTask<int> (int x) => …);` |
| `ErrorCategory.Unauthorized` | `ErrorCategory.PermissionDenied` |
| `Error.Unauthorized(…)`, `ErrorBuilder.Unauthorized(…)` | `Error.PermissionDenied(…)`, `ErrorBuilder.PermissionDenied(…)` |
| `Error.X(…, "template.id", ("name", value))` (a template ID and parameters by position) | `Error.X(…, new TemplatedMessage("template.id", ("name", value)))` |
| `Error.X(…, detailTemplateId: "template.id", detailNamedValues: …)` | `Error.X(…, detailTemplated: new("template.id", …))` |
| `IResult` (`Maple.Result`) | `IOperationResult` (`Maple.Results`) |
| `result.IsSuccess()` | `result.IsSuccess` |
| `!result.IsSuccess()` | `result.IsError` |
| `result.Value` relied on as the default value of a failed result | `result.TryGetValue(out var value) ? value : default` or a check of `result.IsSuccess` |
| `WithErrorDetail(…, messageId: …, namedValues: …)` (named arguments) | `WithErrorDetail(…, detailTemplateId: …, detailParameters: …)` |
| `var (id, parameters) = templatedMessage;` | `templatedMessage is { TemplateId: var id, Parameters: var parameters }` |
| `templatedMessage.Params`, `error.DetailTemplated?.Params` | `templatedMessage.Parameters`, `error.DetailTemplated?.Parameters` |
| `JsonSerializer.Deserialize<Result<T>>(…)` / `Deserialize<Error>(…)` | your own DTO, or the converter packages (coming soon) |

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
* [Issue #227: `Match` with a success function and an error action](https://github.com/TomMaple/Maple.Result/issues/227)
* [Issue #235: Reject template parameter names that differ only in case](https://github.com/TomMaple/Maple.Result/issues/235)
* [Issue #255: `Match` as a fold of two functions, and `Switch` and `SwitchAsync` for two actions](https://github.com/TomMaple/Maple.Result/issues/255)
