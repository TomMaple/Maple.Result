# ErrorBuilder.WithDetail(String) Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/ErrorBuilder.cs#L266" target="_blank">ErrorBuilder.cs</a>

Sets the [Detail](../Error/Error_Detail.md) of the error to build.

```csharp
public ErrorBuilder WithDetail(string? detail);
```

### Parameters
#### `detail` [String](https://learn.microsoft.com/dotnet/api/system.string)
A human-readable explanation specific to this occurrence of the problem (the RFC 9457 "detail" member), or `null` to clear it.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Exceptions
[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `detail` is empty or consists only of white-space characters. Pass `null` when there is no detail.

## Remarks
To add an individual error occurrence (an [ErrorDetail](../ErrorDetail/ErrorDetail.md)), use [AddErrorDetail](ErrorBuilder_AddErrorDetail.md).

The detail is `null` or a text with content: an empty or whitespace-only text is rejected, and a text is not trimmed (`" Too short. "` is stored as it is). This is a rule of this library, for consistency with the title; RFC 9457 has no rule for the content of `detail`.

> [!NOTE]
> When the text comes from data (e.g., `detail: exception.Message` in a mapper of [Result.Try](../Result/Result_Try.md)), pass `null` or a fallback text when it can be empty: an empty text makes the call throw, and inside a mapper that exception replaces the original one.

## Examples
```csharp
var error = ErrorBuilder.NotFound(typeUri, "Cannot update the account.")
    .WithDetail("The account with the provided ID has not been found.")
    .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [Detail](../Error/Error_Detail.md)
