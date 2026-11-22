# Error.Category Property
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Error.cs#L79" target="_blank">Error.cs</a>

Gets the category of the error.
```csharp
public ErrorCategory Category { get; }
```

### Property Value
[ErrorCategory](../ErrorCategory.md)

The category of the error.

## Remarks
This is a mandatory property. It is always a defined [ErrorCategory](../ErrorCategory.md) value.

If not specified, the category defaults to *Validation*.

## See Also
* [ErrorCategory](../ErrorCategory.md) — for the list of defined error categories.
