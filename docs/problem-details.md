# Problem Details Format

How an `Error` is written as a problem details object ([RFC 9457](https://datatracker.ietf.org/doc/html/rfc9457)) and read back. The packages that write problem details (`Maple.Result.Extensions.AspNetCore`, `MinimalApi`, `Functions.Worker`) and the ones that read them (`Maple.Result.Extensions.HttpClient`) follow this format, so an error written by one service is read the same way by another.

The member names are available as constants in [ProblemDetailsMembers](Reference/Http/ProblemDetailsMembers.md), in the `Maple.Results.Http` namespace.

## Contents
* [Example](#example)
* [Members](#members)
* [Error details (`errors`)](#error-details-errors)
* [Templated messages (`detailTemplated`)](#templated-messages-detailtemplated)
* [Metadata (extension members)](#metadata-extension-members)
* [Category and status](#category-and-status)
* [Reading problem details](#reading-problem-details)

## Example
```csharp
var error = ErrorBuilder.Validation(
        ErrorUri.Locator("https://exampleapp.com/docs/errors/signup/validation"),
        "The sign-up data is not valid.")
    .WithDetail("Two fields are not valid.")
    .WithInstanceUri(ErrorUri.Relative("/signup/requests/4815162342"))
    .AddErrorDetail(new ErrorDetail(
        "/email",
        "The email address is required.",
        new TemplatedMessage("signup.email.required")))
    .AddErrorDetail(new ErrorDetail(
        "/age",
        "The age must be at least 18.",
        new TemplatedMessage("signup.age.min", ("min", 18))))
    .WithMetadata("retryable", false)
    .Build();
```

```json
{
  "type": "https://exampleapp.com/docs/errors/signup/validation",
  "title": "The sign-up data is not valid.",
  "status": 400,
  "detail": "Two fields are not valid.",
  "instance": "/signup/requests/4815162342",
  "errors": [
    {
      "pointer": "/email",
      "detail": "The email address is required.",
      "detailTemplated": { "templateId": "signup.email.required" }
    },
    {
      "pointer": "/age",
      "detail": "The age must be at least 18.",
      "detailTemplated": { "templateId": "signup.age.min", "params": { "min": 18 } }
    }
  ],
  "retryable": false
}
```

## Members
A member is written only when it has a value: `null` values and an empty `errors` array are omitted.

| Member            | JSON type | `Error` property  | Present                          | Constant |
| ----------------- | --------- | ----------------- | -------------------------------- | -------- |
| `type`            | string    | `TypeUri`         | always (`about:blank` when not specified) | `ProblemDetailsMembers.Type` |
| `title`           | string    | `Title`           | always                           | `ProblemDetailsMembers.Title` |
| `status`          | number    | `Category`        | always (mapped as in [Category and status](#category-and-status)) | `ProblemDetailsMembers.Status` |
| `detail`          | string    | `Detail`          | when not `null` (never an empty text) | `ProblemDetailsMembers.Detail` |
| `instance`        | string    | `InstanceUri`     | when not `null`                  | `ProblemDetailsMembers.Instance` |
| `errors`          | array     | `ErrorDetails`    | when not empty                   | `ProblemDetailsMembers.Errors` |
| `detailTemplated` | object    | `DetailTemplated` | when not `null`                  | `ProblemDetailsMembers.DetailTemplated` |
| *(metadata key)*  | any       | `Metadata`        | one member per entry             | — |

`type`, `title`, `status`, `detail` and `instance` are the standard members of RFC 9457 (section 3.1). `errors` and `detailTemplated` are extension members defined by this format (RFC 9457, section 3.2). The `status` member is advisory: the HTTP response carries the same status code.

`type` and `instance` are written as the text of `TypeUri` and `InstanceUri`, the `Value` of their `ErrorUri`; [Reading problem details](#reading-problem-details) describes the way back.

There is **no `category` member**: the category is carried by `status`. A metadata entry may still use the key `category`.

## Error details (`errors`)
Each `ErrorDetail` is one object of the `errors` array:

| Member            | JSON type | `ErrorDetail` property | Present          | Constant |
| ----------------- | --------- | ---------------------- | ---------------- | -------- |
| `pointer`         | string    | `PropertyPointer`      | when not `null`  | `ProblemDetailsMembers.ErrorPointer` |
| `detail`          | string    | `Detail`               | always (never an empty text) | `ProblemDetailsMembers.ErrorDetail` |
| `detailTemplated` | object    | `DetailTemplated`      | when not `null`  | `ProblemDetailsMembers.ErrorDetailTemplated` |

The `pointer` is a JSON Pointer ([RFC 6901](https://datatracker.ietf.org/doc/html/rfc6901)) to the invalid part of the request, as in the `errors` example of RFC 9457 (section 3). `PropertyPointer` is `null` (no `pointer` member) or a pointer in one of two forms; the constructor of `ErrorDetail` throws an `ArgumentException` for any other value (e.g., `Email` or `Items[0].Name`):

* the **string form** (RFC 6901, section 5): empty (the whole input), or it starts with `/`, e.g., `/email` or `/items/0/name`. In a name, `~` is written as `~0` and `/` as `~1`. The examples of this library use this form.
* the **URI fragment form** (RFC 6901, section 6): `#` followed by the percent-encoded string form, e.g., `#/email` (as `#/age` in the example of RFC 9457) or `#/first%20name`.

The pointer is written as it was given: `/email` and `#/email` are not turned into one form. Whether the pointer leads to anything in the input is not checked.

To build a pointer from names and indexes, use `ErrorDetail.Pointer("items", 0, name)`: it escapes the names (`~` as `~0` first, then `/` as `~1`), so a name taken from the input cannot make the constructor throw or give a wrong pointer.

### What the pointer points to
RFC 9457 leaves the `errors` member to the problem type (its example scopes the pointer to "the request's content"), so this format defines it: the pointer addresses **the input that was validated, as the client sent it**, whatever the transport. JSON Pointer is used as a path syntax (names and indexes separated by `/`); the input does not have to be a JSON body.

| What was validated | Example | Pointer |
| --- | --- | --- |
| A JSON body | `{"address":{"street":""}}` | `/address/street` |
| The query string of a GET | `?page=-1` | `/page` |
| A repeated parameter | `?ids=1&ids=x` | `/ids/1` |
| A message from a queue or a gRPC call | the second item has no `sku` | `/items/1/sku` |
| Headers | `Idempotency-Key` is missing | `/Idempotency-Key` |
| A rule across several fields | the start date is after the end date | `null` |

The pointer does not say which part of the request it refers to (query string, header or body); the problem type or the request makes that clear. A problem about a single header can also be its own problem type, or have a `null` pointer and a metadata entry.

A name taken from the input has to be escaped: `~` as `~0` first, then `/` as `~1` (e.g., the member `a/b` is `/a~1b`). Otherwise, a name with a `~` makes the constructor throw, and a name with a `/` gives a wrong pointer.

## Templated messages (`detailTemplated`)
A `TemplatedMessage` (of the error or of an error detail) is an object:

| Member       | JSON type | `TemplatedMessage` property | Present          | Constant |
| ------------ | --------- | --------------------------- | ---------------- | -------- |
| `templateId` | string    | `TemplateId`                | always           | `ProblemDetailsMembers.TemplateId` |
| `params`     | object    | `Parameters`                | when not `null`  | `ProblemDetailsMembers.TemplateParams` |

`params` has one member per parameter. `Parameters` is `null` when there are no parameters, so an empty `params` object is never written.

Two parameter names of one message never differ only in case (e.g., `Min` and `min`), as two metadata keys of one error: they would be the same placeholder for a template engine that ignores case, and the same member of `params` once a naming policy changes the case of the keys (e.g., `DictionaryKeyPolicy` of *System.Text.Json*). `TemplatedMessage` throws an `ArgumentException` for them, and the lookups in `Parameters` ignore the case (`Parameters["MIN"]` finds `min`). Like the other rules of the names, this is a rule of this library, not of an RFC: JSON allows any string as a member name (RFC 8259, section 4).

## Metadata (extension members)
Each `Metadata` entry is a top-level extension member: the key is the member name and the value is written as JSON.

The top-level members of the format are **reserved**: `type`, `title`, `status`, `detail`, `instance`, `errors` and `detailTemplated` cannot be used as metadata keys, in any case (e.g., `Title` or `ERRORS`), because the entry would collide with the member once the metadata is flattened into the object, and clients often read JSON case-insensitively. `Error.WithMetadata()` and `ErrorBuilder.WithMetadata()` throw an `ArgumentException` for them. For the same reason, keys that differ only in case (e.g., `RetryAfter` and `retryAfter`) are one key: an error never holds both, and a key in another case replaces the entry, with the spelling of the last call, which is the one written. The names inside an `errors` item or a `detailTemplated` object (`pointer`, `templateId`, `params`) are not reserved.

A metadata key is not empty and does not start or end with white space (e.g., `" type"`, which would be written next to `type`); white space inside a key is allowed. Like the reserved names and the comparison of the keys ignoring case, this is a rule of this library: JSON allows any string as a member name (RFC 8259, section 4), and the naming recommendations of RFC 9457 (section 4) are not mandatory.

Members added by the host are not reserved either: e.g., ASP.NET Core adds `traceId`. When a metadata entry has the same key, the package that writes the response decides which value wins and documents it.

## Category and status
> [!NOTE]
> The table shows the **default** mapping. It can be customized in both directions: the packages that write problem details let you choose the `status` written for each category, and the packages that read them let you choose the category read for each status code (including the codes not listed below). The two directions are configured separately, so a custom mapping should keep them consistent when the same services write and read the errors.

| `ErrorCategory`    | `status` written (default) | `status` read (default) |
| ------------------ | -------------------------- | ----------------------- |
| `Validation`       | 400                        | 400, 413, 414, 431      |
| `Unauthenticated`  | 401                        | 401                     |
| `PermissionDenied` | 403                        | 403                     |
| `NotFound`         | 404                        | 404, 410                |
| `Timeout`          | 408                        | 408, 504                |
| `Conflict`         | 409                        | 409, 412, 423           |
| `Failure`          | 422                        | 422, any other 4xx code, any other code outside 4xx and 5xx |
| `LimitExceeded`    | 429                        | 429                     |
| `Critical`         | 500                        | 500, any other 5xx code (e.g., 505, 507) |
| `NotImplemented`   | 501                        | 501                     |
| `Unavailable`      | 503                        | 502, 503                |

[HttpErrorTitles](Reference/Http/HttpErrorTitles.md) (namespace `Maple.Results.Http`) has the phrase of the status code that each category writes by default, the title of an error whose type is `about:blank` (RFC 9457, section 4.2.1), and [HttpErrorUris](Reference/Http/HttpErrorUris.md) a type that links to the section of that status code.

A status code that a category does not write is read by its meaning: `410` (Gone) is a resource that is not there, `412` (Precondition Failed) and `423` (Locked) are conflicts with the state of the resource, and `413`, `414` and `431` reject the size of the request (input-size checks are `Validation`, not `LimitExceeded`). A server error is never read as `Failure`, which is an expected error and is written as `422`: a gateway that passes on an upstream `502` answers `503`, and any other `5xx` answers `500`.

`LimitExceeded` (429) is new in 3.0; the 2.x packages map 429 to `Failure`. The reading of `410`, `412`, `413`, `414`, `423`, `431`, `502` and the other `5xx` codes is new in 3.0 too; the 2.x packages read all of them as `Failure`. See [ErrorCategory](Reference/ErrorCategory.md) for when to use each category.

## Reading problem details
Problem details written by other services do not always follow this format. The reader builds the `Error` with `ErrorBuilder`, so the validation of the core applies, and adapts the values as follows:

* **`status`**: not read. The category is mapped from the status code of the HTTP response by the [status mapping](#category-and-status) (the default one or a customized one). The `status` member is advisory (RFC 9457, section 3.1.2), and it can differ from the code of the response, e.g., when a gateway passes on the body of an upstream service with another code.
* **`type`**: read with `ErrorUri.Parse()`; a missing `type` is `about:blank` (RFC 9457, section 3.1.1), and `about:blank` in any case (e.g., `about:BLANK`) is read as `ErrorUri.None`. A relative reference (e.g., `out-of-credit`) is resolved against the URI of the request (RFC 3986, section 5). Other schemes (e.g., `mailto:`) are kept as `ErrorUri.Other()`, including `javascript:`, `file:` and an `about` URI other than `about:blank` (e.g., `about:invalid`): the value of another service is untrusted, so a consumer that shows a `type` or an `instance` as a link has to check the scheme first (e.g., allow only `http` and `https`).
* **`instance`**: read like `type`; a missing `instance` stays `null`.
* **`title`**: a missing or blank `title` is replaced by the phrase of the HTTP status code (e.g., "Not Found", RFC 9457, section 3.1.3).
* **`pointer`** of an `errors` item: RFC 6901 (section 7) asks an application to state what it does with an invalid pointer. An invalid `pointer` is dropped and the `detail` of the item is kept, because the core rejects it; a reader may convert the keys of a known other syntax into a pointer (e.g., `Items[0].Name` of ASP.NET Core into `/Items/0/Name`). The case of the names is kept by default; a reader may let you change it (e.g., with a naming policy, so camel case gives `/items/0/name`).
* **Other members**: every member that is not a reserved member becomes a metadata entry with its key as written (e.g., `traceId`). When two members differ only in case (e.g., `traceId` and `TraceId`), the last one in the document is kept, with its spelling.

## See Also
* [ProblemDetailsMembers](Reference/Http/ProblemDetailsMembers.md)
* [Error](Reference/Error/Error.md)
* [ErrorCategory](Reference/ErrorCategory.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
