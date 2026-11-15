# Result&lt;T&gt;.IsError Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L220" target="_blank">Result.cs</a>

Gets a value indicating whether the [Result&lt;T&gt;](ResultT.md) represents a failed operation.

```csharp
public bool IsError { get; }
```

### Property Value
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the operation failed and [Error](ResultT_Error.md) is not `null`; otherwise, `false`.

## Remarks
This property is annotated with `[MemberNotNullWhen(true, nameof(Error))]` and `[MemberNotNullWhen(false, nameof(Value))]`, so the compiler treats [Error](ResultT_Error.md) as non-null when it is `true`, and [Value](ResultT_Value.md) as non-null when it is `false`. That is what allows the early return `if (result.IsError) return result.Error;` followed by `result.Value` without a null check. It is the opposite of [IsSuccess](ResultT_IsSuccess.md).

> [!NOTE]
> When `T` is nullable (a nullable reference type or `Nullable<T>`), a successful result may carry a `null` value, so the `[MemberNotNullWhen(false, nameof(Value))]` hint is a best effort for those types, as for [IsSuccess](ResultT_IsSuccess.md).

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
