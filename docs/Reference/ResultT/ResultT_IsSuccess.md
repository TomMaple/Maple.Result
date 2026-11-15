# Result&lt;T&gt;.IsSuccess Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L205" target="_blank">Result.cs</a>

Gets a value indicating whether the [Result&lt;T&gt;](ResultT.md) represents a successful operation.

```csharp
public bool IsSuccess { get; }
```

### Property Value
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the operation was successful; otherwise, `false`.

## Remarks
This property is annotated with `[MemberNotNullWhen(true, nameof(Value))]` and `[MemberNotNullWhen(false, nameof(Error))]`, so the compiler treats [Value](ResultT_Value.md) as non-null when it is `true`, and [Error](ResultT_Error.md) as non-null when it is `false`. That is what allows `result.Value` to be used without a null check inside a success branch. It can also be used in property patterns (e.g., `result is { IsSuccess: true, Value: var value }`).

A result is successful exactly when its [Error](ResultT_Error.md) is `null`. It is the opposite of [IsError](ResultT_IsError.md).

> [!NOTE]
> When `T` is nullable (a nullable reference type or `Nullable<T>`), a successful result may carry a `null` value (e.g., `Result<int?>.FromValue(null)`), so the `[MemberNotNullWhen(true, nameof(Value))]` hint is a best effort for those types.

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
