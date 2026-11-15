# IOperationResult.IsSuccess Property
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L39" target="_blank">Result.cs</a>

Gets a value indicating whether the [IOperationResult](IOperationResult.md) represents a successful operation.

```csharp
public bool IsSuccess { get; }
```

### Property Value
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the operation was successful; otherwise, `false`.

## Remarks
The property is annotated with `[MemberNotNullWhen(false, nameof(Error))]`, so the compiler treats [Error](IOperationResult_Error.md) as non-null when it is `false`. It is the opposite of [IsError](IOperationResult_IsError.md).

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
* [IsError](IOperationResult_IsError.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
