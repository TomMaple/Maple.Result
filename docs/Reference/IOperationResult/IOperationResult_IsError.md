# IOperationResult.IsError Property
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L48" target="_blank">Result.cs</a>

Gets a value indicating whether the [IOperationResult](IOperationResult.md) represents a failed operation.

```csharp
public bool IsError { get; }
```

### Property Value
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the operation failed and [Error](IOperationResult_Error.md) is not `null`; otherwise, `false`.

## Remarks
The property is annotated with `[MemberNotNullWhen(true, nameof(Error))]`, so the compiler treats [Error](IOperationResult_Error.md) as non-null when it is `true`. It is the opposite of [IsSuccess](IOperationResult_IsSuccess.md).

## Examples
```csharp
if (result.IsError)
{
    Console.WriteLine($"Operation failed: {result.Error.Title}");
}
```

## See Also
* [IsSuccess](IOperationResult_IsSuccess.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
