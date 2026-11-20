# Error.InstanceUri Property
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L179" target="_blank">Error.cs</a>

Gets a *URI* that identifies the specific occurrence of the problem.

```csharp
public ErrorUri? InstanceUri { get; }
```

### Property Value
[Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[ErrorUri](../ErrorUri/ErrorUri.md)&gt;

The *URI* that identifies the specific occurrence of the problem, or `null` when there is none; its text is the [Value](../ErrorUri/ErrorUri_Value.md).

## Remarks
This is an optional property: the [ErrorUri](../ErrorUri/ErrorUri.md) that the error was created with, which identifies the specific occurrence of the problem. Read its text with `error.InstanceUri?.Value`.

> [!NOTE]
> [Error](Error.md) is immutable, so this property is set when the error is created: by the factory methods (e.g., [Validation](Error_Validation.md)) or an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md). To change it, use [ToBuilder()](Error_ToBuilder.md).

The value of this property should be:
* a *URI Locator*, or
* a *URI Tag*.

If the value is a *URI Locator*, it is a *URL* (with an “http” or “https” scheme) that, when dereferenced, the problem details object can be fetched from it. It might also return information about the problem occurrence in other formats through use of proactive content negotiation.

> [!NOTE]
> It is **RECOMMENDED** that absolute *URI*s be used when possible, and that when relative *URI*s are used, they include the full path (e.g., `/instances/123`).

If the value is a *URI Tag*, it is not dereferenceable and serves as a unique identifier for the problem occurrence that may be of significance to the server but is opaque to the client.

> [!WARNING]
> The scheme is not restricted: a value read from another service can be, e.g., `javascript:` or `file:` ([ErrorUri.Other](../ErrorUri/ErrorUri_Other.md)). Treat it as untrusted: a consumer that shows it as a link (e.g., in HTML) or opens it has to check the scheme first, e.g., allow only `http` and `https`.

## Examples
```csharp
var error = Error.NotFound(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found"),
                "User has not been found.",
                // uses the ErrorUri record to provide value for the InstanceUri property
                ErrorUri.Locator("https://api.exampleapp.com/error/e-1234-5678-9012-345678901234/details"));
```

## See Also
* [ErrorUri](../ErrorUri/ErrorUri.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)
