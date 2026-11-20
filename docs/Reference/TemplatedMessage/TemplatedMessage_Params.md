# TemplatedMessage.Params Property
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/TemplatedMessage.cs#L102" target="_blank">TemplatedMessage.cs</a>

Gets a read-only dictionary of named values to be used when rendering the templated message.

```csharp
public IReadOnlyDictionary<string, object?>? Params { get; }
```

### Property Value
[IReadOnlyDictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary-2)&lt;[String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object)&gt;

A collection of named values to be used when rendering the templated message.

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
This is an optional property.

It is a read-only dictionary built from the parameters passed to the [constructor](TemplatedMessage_constructors.md), with keys compared ordinally (case-sensitive). A key (a parameter name) is not empty and does not start or end with white space; white space inside is allowed. Two names never differ only in case (e.g., `Min` and `min`): they would be the same placeholder for a template engine that ignores case, and the same member of `params` once a naming policy changes the case of the keys. It is `null` when the message has no parameters (`null`, an empty collection or no parameters at all were passed), so it is never empty. Later changes to the source are not reflected, and the dictionary cannot be modified, also through a cast. Values can be `null`.

> [!NOTE]
> An instance is created with a constructor and passed to the factory methods of the [Error](../Error/Error.md) record (the `detailTemplated` parameter, e.g., [NotFound](../Error/Error_NotFound.md)), or created from a template identifier and named values by [Error.WithErrorDetail](../Error/Error_WithErrorDetail.md) and by the [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) methods [WithDetailTemplated](../ErrorBuilder/ErrorBuilder_WithDetailTemplated.md) and [AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md).


## See Also
* [Error](../Error/Error.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
