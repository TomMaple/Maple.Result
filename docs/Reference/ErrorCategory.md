# ErrorCategory Enum
## Definition
Namespace: [Maple.Results](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorCategory.cs" target="_blank">ErrorCategory.cs</a>

Defines the category of an error.

```csharp
public enum ErrorCategory
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → [ValueType](https://learn.microsoft.com/dotnet/api/system.valuetype) → [Enum](https://learn.microsoft.com/dotnet/api/system.enum) → ErrorCategory

## Remarks
The members are ordered by the HTTP status code they relate to. The values are explicit and do not change between versions, because they are stored and serialized (e.g., as numbers in JSON). New categories are only ever added with new values.

> [!WARNING]
> The default value of the enumeration (`default(ErrorCategory)`, `0`) is `Validation`, a real category: do not use it to mean "not set". A category that is missing, e.g., from a JSON payload, has to be handled as missing by the code that reads it, not read as `Validation` (some serializer settings, such as `DefaultValueHandling.Ignore` of *Newtonsoft.Json*, even leave `Validation` out when they write).

> [!NOTE]
> The HTTP status codes in the descriptions are the **default** mapping of the packages that write and read problem details; the mapping can be customized in both directions (category → status code and status code → category). See [Problem Details Format](../problem-details.md#category-and-status).

## Fields
| Name            | Value | Description                          |
| --------------- | ----- | ------------------------------------ |
| Validation      | 0     | A validation error. The operation failed due to something wrong with the input data.<br>It relates to the `400` HTTP status code (Bad Request); `413`, `414` and `431` are read as this category. It is the default value (`0`). |
| Unauthenticated | 1     | The action is not authenticated. The authentication is either missing or invalid.<br>It relates to the `401` HTTP status code (Unauthorized). |
| Unauthorized    | 2     | The action is not authorized. The authentication might be valid, but the identity does not have the required permissions.<br>It relates to the `403` HTTP status code (Forbidden). |
| NotFound        | 3     | The resource was not found. The requested resource does not exist.<br>It relates to the `404` HTTP status code (Not Found); `410` (Gone) is read as this category. |
| Timeout         | 4     | A timeout error. The operation timed out before it could complete.<br>It relates to the `408` HTTP status code (Request Timeout) when the client did not send the complete request in time, or to the `504` HTTP status code (Gateway Timeout) when an upstream server or a dependency did not respond in time. |
| Conflict        | 5     | A conflict error. The operation failed due to the data or state conflict (such as a unique constraint violation).<br>It relates to the `409` HTTP status code (Conflict); `412` (Precondition Failed) and `423` (Locked) are read as this category. |
| Failure         | 6     | An expected error that is not critical to the operation. Do not use for authentication, authorization, validation, data conflict or unexpected errors.<br>It relates to the `422` HTTP status code (Unprocessable Content); any other `4xx` status code is read as this category. |
| LimitExceeded   | 10    | The request is valid and permitted, but a usage limit, quota or rate has been exhausted. It may succeed later or once the limit is raised. Applies to business limits as well as throttling. Do not use for input-size checks (use `Validation`).<br>It relates to the `429` HTTP status code (Too Many Requests). |
| Critical        | 7     | An unexpected, critical error.<br>It relates to the `500` HTTP status code (Internal Server Error); any other `5xx` status code (e.g., `505` or `507`) is read as this category. |
| NotImplemented  | 8     | The not implemented error. The operation, feature or case is not implemented.<br>It relates to the `501` HTTP status code (Not Implemented). |
| Unavailable     | 9     | The service is not available (e.g. it is down or overloaded). Try again later. Do not use when a usage limit has been exhausted (use `LimitExceeded`).<br>It relates to the `503` HTTP status code (Service Unavailable); `502` (Bad Gateway) is read as this category. |

## See Also
* [HTTP Status Codes](https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status)
* [HttpStatusCode Enum](https://learn.microsoft.com/en-us/dotnet/api/system.net.httpstatuscode)
