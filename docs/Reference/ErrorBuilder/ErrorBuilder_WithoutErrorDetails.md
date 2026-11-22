# ErrorBuilder.WithoutErrorDetails() Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/ErrorBuilder.cs#L408" target="_blank">ErrorBuilder.cs</a>

Removes all individual error occurrences from the [ErrorDetails](../Error/Error_ErrorDetails.md) of the error to build.

```csharp
public ErrorBuilder WithoutErrorDetails();
```

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

## Remarks
The details added after this call with [AddErrorDetail](ErrorBuilder_AddErrorDetail.md) are kept. An [Error](../Error/Error.md) built earlier, and the error that the builder was created from with [Error.ToBuilder()](../Error/Error_ToBuilder.md), keep their details.

## Examples
```csharp
// Replaces the details of the downstream service with the details of the current request.
var publicError = downstreamError.ToBuilder()
    .WithoutErrorDetails()
    .AddErrorDetail("/email", "The email address is already registered.")
    .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [AddErrorDetail](ErrorBuilder_AddErrorDetail.md)
* [Error.ErrorDetails](../Error/Error_ErrorDetails.md)
