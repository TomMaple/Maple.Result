# Result&lt;T&gt;.IsSuccess Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L290" target="_blank">Result.cs</a>

Gets a value indicating whether the [Result&lt;T&gt;](ResultT.md) represents a successful operation.

```csharp
public bool IsSuccess { get; }
```

### Property Value
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the operation was successful; otherwise, `false`.

## Remarks
When it is `true`, [Value](ResultT_Value.md) can be read (it throws on a failed result). The property is annotated with `[MemberNotNullWhen(false, nameof(Error))]`, so the compiler treats [Error](ResultT_Error.md) as non-null when it is `false`. It can also be used in property patterns (e.g., `result is { IsSuccess: true, Value: var value }`; the pattern reads `Value` only after `IsSuccess` matched).

A result is successful exactly when its [Error](ResultT_Error.md) is `null`. It is the opposite of [IsError](ResultT_IsError.md).

## Examples
```csharp
if (result.IsSuccess)
{
    Console.WriteLine("Operation succeeded.");
}
else
{
    Console.WriteLine($"Operation failed: {result.Error.Title}");
}
```

## See Also
* [IsError](ResultT_IsError.md)
* [Result](../Result/Result.md)
