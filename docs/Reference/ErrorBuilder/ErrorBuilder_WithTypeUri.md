# ErrorBuilder.WithTypeUri(ErrorUri) Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/ErrorBuilder.cs#L227" target="_blank">ErrorBuilder.cs</a>

Sets the [TypeUri](../Error/Error_TypeUri.md) of the error to build.

```csharp
public ErrorBuilder WithTypeUri(ErrorUri typeUri);
```

### Parameters
#### `typeUri` [ErrorUri](../ErrorUri/ErrorUri.md)
A record containing a *URI* reference that identifies the type of the problem. A default instance means `about:blank`.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

## Remarks
An [ErrorUri](../ErrorUri/ErrorUri.md) is always valid, so this method never throws.

## Examples
```csharp
var error = existingError.ToBuilder()
    .WithTypeUri(ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found"))
    .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [TypeUri](../Error/Error_TypeUri.md)
