# Result.IsError Property
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L107" target="_blank">Result.cs</a>

Gets a value indicating whether the [Result](Result.md) represents a failed operation.

```csharp
public bool IsError { get; }
```

### Property Value
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the operation failed and [Error](Result_Error.md) is not `null`; otherwise, `false`.

## Remarks
The property is annotated with `[MemberNotNullWhen(true, nameof(Error))]`, so the compiler treats [Error](Result_Error.md) as non-null when it is `true`; this allows the early return `if (result.IsError) return result.Error;`. It is the opposite of [IsSuccess](Result_IsSuccess.md).

## Examples
```csharp
public Result Save(Order order)
{
    var validation = Validate(order);
    if (validation.IsError)
        return validation.Error;

    // ...
    return Result.Success();
}
```

## See Also
* [IsSuccess](Result_IsSuccess.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
