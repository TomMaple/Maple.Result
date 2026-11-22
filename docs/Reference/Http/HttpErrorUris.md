# HttpErrorUris Class
## Definition
Namespace: [Maple.Results.Http](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorUris.cs" target="_blank">HttpErrorUris.cs</a>

The type URIs of the HTTP status codes that the [ErrorCategory](../ErrorCategory.md) values map to by default: *URI Locators* of the section of the specification that defines each status code.

```csharp
public static class HttpErrorUris
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → HttpErrorUris

## Remarks
They are for an error that has no semantics beyond its status code, but whose type should be dereferenceable rather than `about:blank` ([ErrorUri.None](../ErrorUri/ErrorUri_None.md)). Pair each one with the title of the same name in [HttpErrorTitles](HttpErrorTitles.md); an error with a title of its own needs a type of its own.

Each section is the one of the [default status code](../../problem-details.md#category-and-status) of its category: *RFC 9110*, section 15, and *RFC 6585*, section 4, for 429 (Too Many Requests).

## Properties
| Name | Value | Description |
| ---- | ----- | ----------- |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorUris.cs#L34" target="_blank">Conflict</a> | `https://www.rfc-editor.org/rfc/rfc9110#section-15.5.10` | The section of 409 (Conflict), the default status code of [ErrorCategory.Conflict](../ErrorCategory.md) (RFC 9110, section 15.5.10). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorUris.cs#L39" target="_blank">Critical</a> | `https://www.rfc-editor.org/rfc/rfc9110#section-15.6.1` | The section of 500 (Internal Server Error), the default status code of [ErrorCategory.Critical](../ErrorCategory.md) (RFC 9110, section 15.6.1). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorUris.cs#L44" target="_blank">Failure</a> | `https://www.rfc-editor.org/rfc/rfc9110#section-15.5.21` | The section of 422 (Unprocessable Content), the default status code of [ErrorCategory.Failure](../ErrorCategory.md) (RFC 9110, section 15.5.21). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorUris.cs#L49" target="_blank">LimitExceeded</a> | `https://www.rfc-editor.org/rfc/rfc6585#section-4` | The section of 429 (Too Many Requests), the default status code of [ErrorCategory.LimitExceeded](../ErrorCategory.md) (RFC 6585, section 4). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorUris.cs#L54" target="_blank">NotFound</a> | `https://www.rfc-editor.org/rfc/rfc9110#section-15.5.5` | The section of 404 (Not Found), the default status code of [ErrorCategory.NotFound](../ErrorCategory.md) (RFC 9110, section 15.5.5). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorUris.cs#L59" target="_blank">NotImplemented</a> | `https://www.rfc-editor.org/rfc/rfc9110#section-15.6.2` | The section of 501 (Not Implemented), the default status code of [ErrorCategory.NotImplemented](../ErrorCategory.md) (RFC 9110, section 15.6.2). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorUris.cs#L64" target="_blank">PermissionDenied</a> | `https://www.rfc-editor.org/rfc/rfc9110#section-15.5.4` | The section of 403 (Forbidden), the default status code of [ErrorCategory.PermissionDenied](../ErrorCategory.md) (RFC 9110, section 15.5.4). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorUris.cs#L69" target="_blank">Timeout</a> | `https://www.rfc-editor.org/rfc/rfc9110#section-15.5.9` | The section of 408 (Request Timeout), the default status code of [ErrorCategory.Timeout](../ErrorCategory.md) (RFC 9110, section 15.5.9). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorUris.cs#L74" target="_blank">Unauthenticated</a> | `https://www.rfc-editor.org/rfc/rfc9110#section-15.5.2` | The section of 401 (Unauthorized), the default status code of [ErrorCategory.Unauthenticated](../ErrorCategory.md) (RFC 9110, section 15.5.2). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorUris.cs#L79" target="_blank">Unavailable</a> | `https://www.rfc-editor.org/rfc/rfc9110#section-15.6.4` | The section of 503 (Service Unavailable), the default status code of [ErrorCategory.Unavailable](../ErrorCategory.md) (RFC 9110, section 15.6.4). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Http/HttpErrorUris.cs#L84" target="_blank">Validation</a> | `https://www.rfc-editor.org/rfc/rfc9110#section-15.5.1` | The section of 400 (Bad Request), the default status code of [ErrorCategory.Validation](../ErrorCategory.md) (RFC 9110, section 15.5.1). |

## Examples
```csharp
using Maple.Results.Http;

var error = Error.NotFound(HttpErrorUris.NotFound, HttpErrorTitles.NotFound);
```

## See Also
* [Maple.Results.Http namespace](namespace.md)
* [HttpErrorTitles](HttpErrorTitles.md)
* [ErrorUri](../ErrorUri/ErrorUri.md)
* [Problem Details Format](../../problem-details.md)
* [RFC 9110: HTTP Semantics](https://datatracker.ietf.org/doc/html/rfc9110#section-15)
* [RFC 6585: Additional HTTP Status Codes](https://datatracker.ietf.org/doc/html/rfc6585#section-4)
