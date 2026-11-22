# Maple.Results.Http Namespace
Provides the definitions of HTTP and of the problem details format (*RFC 9457*) for an [Error](../Error/Error.md): the member names of the format, and the titles and type URIs of the default status codes of the categories.

They are in their own namespace so that using them is a choice: `using Maple.Results.Http;`. An error with a type and a title of its own (e.g., an `ErrorUri.Tag(…)`) does not need them.

## Classes
| Name | Description |
| ---- | ----------- |
| [HttpErrorTitles](HttpErrorTitles.md) | The reason phrases of the HTTP status codes that the [ErrorCategory](../ErrorCategory.md) values map to by default, for the title of an error whose type has no semantics of its own. |
| [HttpErrorUris](HttpErrorUris.md) | The type URIs of the HTTP status codes that the [ErrorCategory](../ErrorCategory.md) values map to by default: the section of the specification that defines each status code. |
| [ProblemDetailsMembers](ProblemDetailsMembers.md) | The member names of the problem details object (RFC 9457) that represents an [Error](../Error/Error.md). |

## See Also
* [Maple.Results namespace](../namespace.md)
* [Problem Details Format](../../problem-details.md)
