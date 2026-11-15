# Problem Details Format

How an `Error` is written as a problem details object ([RFC 9457](https://datatracker.ietf.org/doc/html/rfc9457)) and read back. The packages that write problem details (`Maple.Result.Extensions.AspNetCore`, `MinimalApi`, `Functions.Worker`) and the ones that read them (`Maple.Result.Extensions.HttpClient`) follow this format, so an error written by one service is read the same way by another.

The member names are available as constants in [ProblemDetailsMembers](Reference/ProblemDetailsMembers.md).

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
| `detail`          | string    | `Detail`          | when not `null`                  | `ProblemDetailsMembers.Detail` |
| `instance`        | string    | `InstanceUri`     | when not `null`                  | `ProblemDetailsMembers.Instance` |
| `errors`          | array     | `ErrorDetails`    | when not empty                   | `ProblemDetailsMembers.Errors` |
| `detailTemplated` | object    | `DetailTemplated` | when not `null`                  | `ProblemDetailsMembers.DetailTemplated` |
| *(metadata key)*  | any       | `Metadata`        | one member per entry             | — |

`type`, `title`, `status`, `detail` and `instance` are the standard members of RFC 9457 (section 3.1). `errors` and `detailTemplated` are extension members defined by this format (RFC 9457, section 3.2). The `status` member is advisory: the HTTP response carries the same status code.

There is **no `category` member**: the category is carried by `status`. A metadata entry may still use the key `category`.

## Error details (`errors`)
Each `ErrorDetail` is one object of the `errors` array:

| Member            | JSON type | `ErrorDetail` property | Present          | Constant |
| ----------------- | --------- | ---------------------- | ---------------- | -------- |
| `pointer`         | string    | `PropertyPointer`      | when not `null`  | `ProblemDetailsMembers.ErrorPointer` |
| `detail`          | string    | `Detail`               | always           | `ProblemDetailsMembers.ErrorDetail` |
| `detailTemplated` | object    | `DetailTemplated`      | when not `null`  | `ProblemDetailsMembers.ErrorDetailTemplated` |

The `pointer` is a JSON Pointer ([RFC 6901](https://datatracker.ietf.org/doc/html/rfc6901)) to the invalid part of the request, as in the `errors` example of RFC 9457 (appendix A).

## Templated messages (`detailTemplated`)
A `TemplatedMessage` (of the error or of an error detail) is an object:

| Member       | JSON type | `TemplatedMessage` property | Present          | Constant |
| ------------ | --------- | --------------------------- | ---------------- | -------- |
| `templateId` | string    | `TemplateId`                | always           | `ProblemDetailsMembers.TemplateId` |
| `params`     | object    | `Params`                    | when not `null`  | `ProblemDetailsMembers.TemplateParams` |

`params` has one member per parameter. `Params` is `null` when there are no parameters, so an empty `params` object is never written.

## Metadata (extension members)
Each `Metadata` entry is a top-level extension member: the key is the member name and the value is written as JSON.

The top-level members of the format are **reserved**: `type`, `title`, `status`, `detail`, `instance`, `errors` and `detailTemplated` cannot be used as metadata keys, in any case (e.g., `Title` or `ERRORS`), because the entry would collide with the member once the metadata is flattened into the object, and clients often read JSON case-insensitively. `Error.WithMetadata()`, `ErrorBuilder.WithMetadata()` and the `Error` factory methods throw an `ArgumentException` for them. The names inside an `errors` item or a `detailTemplated` object (`pointer`, `templateId`, `params`) are not reserved.

Members added by the host are not reserved either: e.g., ASP.NET Core adds `traceId`. When a metadata entry has the same key, the package that writes the response decides which value wins and documents it.

## Category and status
> [!NOTE]
> The table shows the **default** mapping. It can be customized in both directions: the packages that write problem details let you choose the `status` written for each category, and the packages that read them let you choose the category read for each status code (including the codes not listed below). The two directions are configured separately, so a custom mapping should keep them consistent when the same services write and read the errors.

| `ErrorCategory`   | `status` written (default) | `status` read (default) |
| ----------------- | -------------------------- | ----------------------- |
| `Validation`      | 400                        | 400                     |
| `Unauthenticated` | 401                        | 401                     |
| `Unauthorized`    | 403                        | 403                     |
| `NotFound`        | 404                        | 404                     |
| `Timeout`         | 408                        | 408, 504                |
| `Conflict`        | 409                        | 409                     |
| `Failure`         | 422                        | 422, any other code     |
| `LimitExceeded`   | 429                        | 429                     |
| `Critical`        | 500                        | 500                     |
| `NotImplemented`  | 501                        | 501                     |
| `Unavailable`     | 503                        | 503                     |

`LimitExceeded` (429) is new in 3.0; the 2.x packages map 429 to `Failure`. See [ErrorCategory](Reference/ErrorCategory.md) for when to use each category.

## Reading problem details
Problem details written by other services do not always follow this format. The reader builds the `Error` with `ErrorBuilder`, so the validation of the core applies, and adapts the values as follows:

* **`status`**: mapped to the category by the [status mapping](#category-and-status) (the default one or a customized one). A missing `status` is taken from the HTTP response.
* **`type`**: read with `ErrorUri.Parse()`; a missing `type` is `about:blank` (RFC 9457, section 3.1.1). A relative reference (e.g., `out-of-credit`) is resolved against the URI of the request (RFC 3986, section 5). Other schemes (e.g., `mailto:`) are kept as `ErrorUri.Other()`.
* **`instance`**: read like `type`; a missing `instance` stays `null`.
* **`title`**: a missing or blank `title` is replaced by the phrase of the HTTP status code (e.g., "Not Found", RFC 9457, section 3.1.3).
* **Other members**: every member that is not a reserved member becomes a metadata entry with its key as written (e.g., `traceId`).

## See Also
* [ProblemDetailsMembers](Reference/ProblemDetailsMembers.md)
* [Error](Reference/Error/Error.md)
* [ErrorCategory](Reference/ErrorCategory.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
