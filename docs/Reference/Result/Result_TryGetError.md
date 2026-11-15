# Result.TryGetError(Error) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L107" target="_blank">Result.cs</a>

Gets the error of a failed [Result](Result.md).

```csharp
public bool TryGetError([NotNullWhen(true)] out Error? error);
```

### Parameters
`error` [Error](../Error/Error.md)

When this method returns, the error if the result is failed; otherwise, `null`. It is not `null` when the method returns `true`.

### Returns
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the result is failed; otherwise, `false`.

## Remarks
It gives the early return in one statement. `result.Error is { } error` does the same; the method is easier to find and to read.

## Examples
```csharp
public Result Save(Order order)
{
    if (Validate(order).TryGetError(out var error))
        return error;

    // ...
    return Result.Success();
}
```
