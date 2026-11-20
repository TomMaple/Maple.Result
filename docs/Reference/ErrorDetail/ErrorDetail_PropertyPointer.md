# ErrorDetail.PropertyPointer Property
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/ErrorDetail.cs#L58" target="_blank">ErrorDetail.cs</a>

Gets a *JSON Pointer* that identifies the input value field that the [ErrorDetail](ErrorDetail.md) record describes.

```csharp
public string? PropertyPointer { get; }
```

### Property Value
[String](https://learn.microsoft.com/dotnet/api/system.string)

A *JSON Pointer* that identifies the field of the input value, or `null`.

## Remarks
This is an optional property that contains a *JSON Pointer* that identifies the input value field that the [ErrorDetail](ErrorDetail.md) record describes.

The value is `null` or a *JSON Pointer* in one of two forms ([RFC 6901](https://datatracker.ietf.org/doc/html/rfc6901)):
* the string form: empty (the whole input), or it starts with `/`, e.g., `/email` or `/items/0/name`; a `~` is followed by `0` or `1`;
* the URI fragment form: `#` followed by the percent-encoded string form, e.g., `#/email`.

The constructor throws an `ArgumentException` (with the parameter name `PropertyPointer`) for any other value, e.g., `Email`, `Items[0].Name` or `/a~2b`. The value is stored as it is given: `/email` and `#/email` are different values. Whether the pointer leads to anything in the input is not checked.

The pointer addresses the input that was validated, as the client sent it, whatever the transport: a JSON body (`/address/street`), the query string (`/page`, or `/ids/1` for the second `ids` parameter), a message (`/items/1/sku`) or a header (`/Idempotency-Key`); it is `null` for a rule across several values. See [What the pointer points to](../../problem-details.md#what-the-pointer-points-to).

> [!NOTE]
> A name taken from the input has to be escaped: `~` as `~0` first, then `/` as `~1` (e.g., the member `a/b` is `/a~1b`). Otherwise, a name with a `~` makes the constructor throw, and a name with a `/` gives a wrong pointer. [Pointer(ReadOnlySpan&lt;Object&gt;)](ErrorDetail_Pointer.md) does it: `ErrorDetail.Pointer("items", 0, name)`.

## Examples
```csharp
var error = Error.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid"),
                "Invalid data to register a new user.");

error = error.WithErrorDetail(
    "/username",
    "The username is already taken.",
    "error:user:create:username-taken",
    ("username", "john_doe"));

error = error.WithErrorDetail(
    "/email",
    "The email address is required.",
    "error:user:create:email-required",
    ("email", ""));
```

## See Also
* [Error](../Error/Error.md)
* [RFC 6901: JavaScript Object Notation (JSON) Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
