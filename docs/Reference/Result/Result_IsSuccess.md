# Result.IsSuccess Property
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Result.cs#L116" target="_blank">Result.cs</a>

Gets a value indicating whether the [Result](Result.md) represents a successful operation.

```csharp
public bool IsSuccess { get; }
```

### Property Value
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the operation was successful; otherwise, `false`.

## Remarks
A result is successful exactly when its [Error](Result_Error.md) is `null`. The property is annotated with `[MemberNotNullWhen(false, nameof(Error))]`, so the compiler treats [Error](Result_Error.md) as non-null when it is `false`. It is the opposite of [IsError](Result_IsError.md), and it can be used in property patterns (e.g., `result is { IsSuccess: true }`).

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
* [IsError](Result_IsError.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
