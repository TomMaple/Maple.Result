# Result&lt;T&gt;.Value Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L212" target="_blank">Result.cs</a>

Gets the value of a successful operation.

```csharp
public T Value { get; }
```

### Property Value
T

The value of the successful operation. It may be `null` when `T` is nullable (a nullable reference type or `Nullable<T>`).

### Exceptions
[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

The result is failed, so it has no value. The message names the title and the type *URI* of the [Error](ResultT_Error.md), e.g., *The result has no value, because the operation failed with the error ‘Missing’ (type ‘about:blank’).*

## Remarks
> [!CAUTION]
> Check [IsSuccess](ResultT_IsSuccess.md) or [IsError](ResultT_IsError.md) before reading this property, or use [GetValueOrDefault(T)](ResultT_GetValueOrDefault.md), which never throws.

A failed result has no value. Reading it throws instead of returning the default value of `T`, which for a value type would look like a valid value (e.g., `0` for a `Result<int>`), so a forgotten check fails at the point where the value is used.

The type of the property is `T`, so its nullability follows `T`: it is a `string` for a `Result<string>` (no nullable warning after the check) and a `string?` for a `Result<string?>` (the compiler asks for a `null` check).

## Examples
```csharp
if (result.IsError)
    return result.Error;

Console.WriteLine(result.Value.Name);
```

## See Also
* [GetValueOrDefault(T)](ResultT_GetValueOrDefault.md)
* [IsSuccess](ResultT_IsSuccess.md)
* [Error](../Error/Error.md)
