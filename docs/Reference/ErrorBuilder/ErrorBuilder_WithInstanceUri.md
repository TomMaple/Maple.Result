# ErrorBuilder.WithInstanceUri(Nullable&lt;ErrorUri&gt;) Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/ErrorBuilder.cs#L328" target="_blank">ErrorBuilder.cs</a>

Sets the [InstanceUri](../Error/Error_InstanceUri.md) of the error to build.

```csharp
public ErrorBuilder WithInstanceUri(ErrorUri? instanceUri);
```

### Parameters
#### `instanceUri` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[ErrorUri](../ErrorUri/ErrorUri.md)&gt;
A *URI* reference that identifies the specific occurrence of the problem, or `null` to clear it.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Exceptions
[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `instanceUri` is `about:blank` ([ErrorUri.None](../ErrorUri/ErrorUri_None.md) or a default [ErrorUri](../ErrorUri/ErrorUri.md)). The builder is not changed.

## Remarks
`about:blank` has a meaning only as a type (*RFC 9457*, section 4.2.1), so it is rejected as an instance; pass `null` when there is no instance.

## Examples
```csharp
var error = ErrorBuilder.Failure(typeUri, "Cannot generate the report.")
    .WithInstanceUri(ErrorUri.Locator("https://api.exampleapp.com/errors/6283365904847834/details"))
    .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [InstanceUri](../Error/Error_InstanceUri.md)
