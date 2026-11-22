# ErrorUri.None Property
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/ErrorUri.cs#L103" target="_blank">ErrorUri.cs</a>

Gets the no-value [ErrorUri](ErrorUri.md): `about:blank`.

```csharp
public static ErrorUri None { get; }
```

### Property Value
[ErrorUri](ErrorUri.md)

The [ErrorUri](ErrorUri.md) with the `about:blank` [Value](ErrorUri_Value.md).

## Remarks
*RFC 9457* (section 4.2.1) defines `about:blank` as the type of a problem that has no additional semantics beyond the HTTP status code. It is a fixed value, like `CancellationToken.None` or `Guid.Empty`: it equals `default(ErrorUri)` and `ErrorUri.Parse("about:blank")`.

With `about:blank`, the title should be the phrase of the HTTP status code (*RFC 9457*, section 4.2.1), e.g., “Not Found” for 404: [HttpErrorTitles](../Http/HttpErrorTitles.md) (in the `Maple.Results.Http` namespace) has the phrase of the default status code of each category. An error with a title of its own (e.g., “The account has not been found.”) gets a type of its own, e.g., an [ErrorUri.Tag(String)](ErrorUri_Tag.md). [HttpErrorUris](../Http/HttpErrorUris.md) has a dereferenceable type for each category, the section of its status code in *RFC 9110*.

> [!IMPORTANT]
> `about:blank` has a meaning only as a type. Passing [None](ErrorUri_None.md) as the instance of an error throws an [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception); pass `null` when there is no instance.

## Examples
```csharp
using Maple.Results.Http;

var error = Error.NotFound(ErrorUri.None, HttpErrorTitles.NotFound);
```

```csharp
if (error.TypeUri == ErrorUri.None)
{
    // A problem without a specific type: the HTTP status code says it all.
}
```

## See Also
* [ErrorUri](ErrorUri.md)
* [Error](../Error/Error.md)
* [HttpErrorTitles](../Http/HttpErrorTitles.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
