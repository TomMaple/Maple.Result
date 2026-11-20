# Result&lt;T&gt;.Error Property
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Result.cs#L303" target="_blank">Result.cs</a>

Gets the error that caused the operation to fail.

```csharp
public Error? Error { get; }
```

### Property Value
[Error](../Error/Error.md)

The error, if the result represents a failure; otherwise, `null`.

## Remarks
> [!CAUTION]
> Use the [IsSuccess](ResultT_IsSuccess.md) or [IsError](ResultT_IsError.md) property to check whether the result represents a success or a failure before accessing this property.

A result is successful exactly when this property is `null`. A failed result always has an error, and a successful result never has one.

## See Also
* [Error](../Error/Error.md)
