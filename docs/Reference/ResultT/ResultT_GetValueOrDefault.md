# Result&lt;T&gt;.GetValueOrDefault(T) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L266" target="_blank">Result.cs</a>

Returns the value of a successful result, or the specified fallback value for a failed one.

```csharp
public T GetValueOrDefault(T fallback);
```

### Parameters
`fallback` T

The value to return when the result is failed.

### Returns
T

The [Value](ResultT_Value.md) if the result is successful; otherwise, `fallback`.

## Remarks
This method never throws. A successful result returns its own value, even when it is `null` (for a nullable `T`); the `fallback` is used only for a failed result.

## Examples
```csharp
var pageSize = GetPageSizeSetting().GetValueOrDefault(20);
```

## See Also
* [Value](ResultT_Value.md)
* [IsSuccess](ResultT_IsSuccess.md)
