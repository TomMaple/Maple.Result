# Error.WithoutErrorDetails() Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Error.cs#L319" target="_blank">Error.cs</a>

Returns a copy of the current error without any [ErrorDetails](Error_ErrorDetails.md).

```csharp
public Error WithoutErrorDetails();
```

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record with an empty [ErrorDetails](Error_ErrorDetails.md) collection. The other members are unchanged; if the error has no details, the returned copy is equal to the current instance.

## Remarks
This method **does not modify** the current instance; it returns a new [Error](Error.md). Use it to clean an error before it is passed on (e.g., one read from another service), without building it again member by member. To remove the details and change other members at once, use [ToBuilder()](Error_ToBuilder.md) and [ErrorBuilder.WithoutErrorDetails()](../ErrorBuilder/ErrorBuilder_WithoutErrorDetails.md).

## Examples
```csharp
// Removes the details of the downstream service before the error is returned to the client.
var publicError = downstreamError.WithoutErrorDetails();
```

## See Also
* [ErrorDetails](Error_ErrorDetails.md)
* [WithErrorDetail](Error_WithErrorDetail.md)
* [WithoutMetadata](Error_WithoutMetadata.md)
