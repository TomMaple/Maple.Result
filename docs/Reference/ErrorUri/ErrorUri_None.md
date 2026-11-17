# ErrorUri.None Property
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorUri.cs#L100" target="_blank">ErrorUri.cs</a>

Gets the no-value [ErrorUri](ErrorUri.md): `about:blank`.

```csharp
public static ErrorUri None { get; }
```

### Property Value
[ErrorUri](ErrorUri.md)

The [ErrorUri](ErrorUri.md) with the `about:blank` [Value](ErrorUri_Value.md).

## Remarks
*RFC 9457* (section 4.2.1) defines `about:blank` as the type of a problem that has no additional semantics beyond the HTTP status code. It is a fixed value, like `CancellationToken.None` or `Guid.Empty`: it equals `default(ErrorUri)` and `ErrorUri.Parse("about:blank")`.

> [!IMPORTANT]
> `about:blank` has a meaning only as a type. Passing [None](ErrorUri_None.md) as the instance of an error throws an [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception); pass `null` when there is no instance.

## Examples
```csharp
var error = Error.Failure(ErrorUri.None, "The operation could not be completed.");
```

```csharp
if (error.TypeUri == ErrorUri.None.Value)
{
    // A problem without a specific type: the HTTP status code says it all.
}
```

## See Also
* [ErrorUri](ErrorUri.md)
* [Error](../Error/Error.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
