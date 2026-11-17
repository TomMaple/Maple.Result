# IOperationResult.TryGetError(Error) Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L67" target="_blank">Result.cs</a>

Gets the error of a failed [IOperationResult](IOperationResult.md).

```csharp
public bool TryGetError(out Error? error);
```

### Parameters
`error` [Error](../Error/Error.md)

When this method returns, the error if the result is failed; otherwise, `null`. It is not `null` when the method returns `true`.

### Returns
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the result is failed; otherwise, `false`.

## Remarks
It gives the early return in one statement for any result, with or without a value.

## Examples
```csharp
public static void LogIfFailed(IOperationResult result, ILogger logger)
{
    if (result.TryGetError(out var error))
        logger.LogWarning("The operation failed: {Title}", error.Title);
}
```

## See Also
* [Result.TryGetError(Error)](../Result/Result_TryGetError.md)
* [Result&lt;T&gt;.TryGetError(Error)](../ResultT/ResultT_TryGetError.md)
