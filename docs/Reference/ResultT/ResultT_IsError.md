# Result&lt;T&gt;.IsError Property
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Result.cs#L329" target="_blank">Result.cs</a>

Gets a value indicating whether the [Result&lt;T&gt;](ResultT.md) represents a failed operation.

```csharp
public bool IsError { get; }
```

### Property Value
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the operation failed and [Error](ResultT_Error.md) is not `null`; otherwise, `false`.

## Remarks
This property is annotated with `[MemberNotNullWhen(true, nameof(Error))]`, so the compiler treats [Error](ResultT_Error.md) as non-null when it is `true`. That is what allows the early return `if (result.IsError) return result.Error;`, after which [Value](ResultT_Value.md) can be read. It is the opposite of [IsSuccess](ResultT_IsSuccess.md).

## Examples
```csharp
public Result<decimal> GetTotal(int orderId)
{
    var order = GetOrder(orderId);
    if (order.IsError)
        return order.Error;

    return order.Value.Lines.Sum(line => line.Price);
}
```

## See Also
* [IsSuccess](ResultT_IsSuccess.md)
* [Result](../Result/Result.md)
