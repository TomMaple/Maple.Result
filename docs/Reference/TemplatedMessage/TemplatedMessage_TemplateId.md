# TemplatedMessage.TemplateId Property
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/TemplatedMessage.cs#L91" target="_blank">TemplatedMessage.cs</a>

Gets a template identifier to be used when rendering the templated message.

```csharp
public string TemplateId { get; }
```

### Property Value
[String](https://learn.microsoft.com/dotnet/api/system.string)

A template identifier to be used when rendering the templated message.

## Examples
```csharp
var error = Error.NotFound(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:account:notfound"),
                "Cannot update the account.",
                "The account with the provided ID has not been found.",
                ErrorUri.Locator("https://api.exampleapp.com/errors/8783927589734857/details"),
                new TemplatedMessage("errors.account.notFound", ("accountId", "D12345"), ("action", "account:update")));
```


## Remarks
This is a mandatory property. It is never `null` or empty, and it does not start or end with white space: the [constructor](TemplatedMessage_constructors.md) rejects such a value and does not trim it (`" errors.length"` throws). White space inside is allowed.

> [!NOTE]
> An instance is created with a constructor and passed to the factory methods of the [Error](../Error/Error.md) record (the `detailTemplated` parameter, e.g., [NotFound](../Error/Error_NotFound.md)), or created from a template identifier and named values by [Error.WithErrorDetail](../Error/Error_WithErrorDetail.md) and by the [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) methods [WithDetailTemplated](../ErrorBuilder/ErrorBuilder_WithDetailTemplated.md) and [AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md).

## See Also
* [Error](../Error/Error.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
