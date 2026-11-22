# HttpErrorTitles Class
## Definition
Namespace: [Maple.Results.Http](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorTitles.cs" target="_blank">HttpErrorTitles.cs</a>

The reason phrases of the HTTP status codes that the [ErrorCategory](../ErrorCategory.md) values map to by default, for the [Title](../Error/Error_Title.md) of an error whose type has no semantics of its own.

```csharp
public static class HttpErrorTitles
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → HttpErrorTitles

## Remarks
*RFC 9457* (section 4.2.1): when the type is `about:blank` ([ErrorUri.None](../ErrorUri/ErrorUri_None.md)), the title should be the reason phrase of the HTTP status code (e.g., “Not Found” for 404). The same pair is used with the type URIs of [HttpErrorUris](HttpErrorUris.md), which name the section of the status code.

Each phrase is the one of the [default status code](../../problem-details.md#category-and-status) of its category. A package that writes problem details with a customized mapping writes another status code; the title has to match that code.

An error with a title of its own (e.g., “The account has not been found.”) needs a type of its own too (e.g., an `ErrorUri.Tag(…)`).

## Fields
| Name | Value | Description |
| ---- | ----- | ----------- |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorTitles.cs#L35" target="_blank">Conflict</a> | `"Conflict"` | The phrase of 409, the default status code of [ErrorCategory.Conflict](../ErrorCategory.md) (RFC 9110, section 15.5.10). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorTitles.cs#L40" target="_blank">Critical</a> | `"Internal Server Error"` | The phrase of 500, the default status code of [ErrorCategory.Critical](../ErrorCategory.md) (RFC 9110, section 15.6.1). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorTitles.cs#L45" target="_blank">Failure</a> | `"Unprocessable Content"` | The phrase of 422, the default status code of [ErrorCategory.Failure](../ErrorCategory.md) (RFC 9110, section 15.5.21). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorTitles.cs#L50" target="_blank">LimitExceeded</a> | `"Too Many Requests"` | The phrase of 429, the default status code of [ErrorCategory.LimitExceeded](../ErrorCategory.md) (RFC 6585, section 4). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorTitles.cs#L55" target="_blank">NotFound</a> | `"Not Found"` | The phrase of 404, the default status code of [ErrorCategory.NotFound](../ErrorCategory.md) (RFC 9110, section 15.5.5). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorTitles.cs#L60" target="_blank">NotImplemented</a> | `"Not Implemented"` | The phrase of 501, the default status code of [ErrorCategory.NotImplemented](../ErrorCategory.md) (RFC 9110, section 15.6.2). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorTitles.cs#L65" target="_blank">PermissionDenied</a> | `"Forbidden"` | The phrase of 403, the default status code of [ErrorCategory.PermissionDenied](../ErrorCategory.md) (RFC 9110, section 15.5.4). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorTitles.cs#L70" target="_blank">Timeout</a> | `"Request Timeout"` | The phrase of 408, the default status code of [ErrorCategory.Timeout](../ErrorCategory.md) (RFC 9110, section 15.5.9). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorTitles.cs#L75" target="_blank">Unauthenticated</a> | `"Unauthorized"` | The phrase of 401, the default status code of [ErrorCategory.Unauthenticated](../ErrorCategory.md) (RFC 9110, section 15.5.2). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorTitles.cs#L80" target="_blank">Unavailable</a> | `"Service Unavailable"` | The phrase of 503, the default status code of [ErrorCategory.Unavailable](../ErrorCategory.md) (RFC 9110, section 15.6.4). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorTitles.cs#L85" target="_blank">Validation</a> | `"Bad Request"` | The phrase of 400, the default status code of [ErrorCategory.Validation](../ErrorCategory.md) (RFC 9110, section 15.5.1). |

## Examples
```csharp
using Maple.Results.Http;

var error = Error.NotFound(ErrorUri.None, HttpErrorTitles.NotFound);
```

## See Also
* [Maple.Results.Http namespace](namespace.md)
* [HttpErrorUris](HttpErrorUris.md)
* [ErrorUri.None](../ErrorUri/ErrorUri_None.md)
* [Problem Details Format](../../problem-details.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
