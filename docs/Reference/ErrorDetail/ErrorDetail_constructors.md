# ErrorDetail Constructor
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorDetail.cs#L47" target="_blank">ErrorDetail.cs</a>

Initializes a new instance of the [ErrorDetail](ErrorDetail.md) record.

```csharp
public sealed record ErrorDetail(string? PropertyPointer, string Detail, TemplatedMessage? DetailTemplated = null);
```

## Remarks
> [!NOTE]
> To add a new instance to an error, use the [WithErrorDetail](../Error/Error_WithErrorDetail.md) method of the [Error](../Error/Error.md) record or the [AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md) method of the [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md); both also accept an instance created with this constructor.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `Detail` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `PropertyPointer` is not `null` and not a *JSON Pointer*, e.g., `Email`, `Items[0].Name` or `/a~2b` (see [PropertyPointer](ErrorDetail_PropertyPointer.md)).

## See also
* [Error](../Error/Error.md)
* [TemplatedMessage](../TemplatedMessage/TemplatedMessage.md)
* [RFC 6901: JavaScript Object Notation (JSON) Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
