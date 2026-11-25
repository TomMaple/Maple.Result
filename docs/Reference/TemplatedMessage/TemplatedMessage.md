# TemplatedMessage Record
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/TemplatedMessage.cs" target="_blank">TemplatedMessage.cs</a>

Provides a set of values to generate a message based on the template for the purpose of [ErrorDetail](../ErrorDetail/ErrorDetail.md) type used in the [Error](../Error/Error.md).

```csharp
public sealed record TemplatedMessage
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → TemplatedMessage


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
The [TemplatedMessage](TemplatedMessage.md) record is used by the [Error](../Error/Error.md) and [ErrorDetail](../ErrorDetail/ErrorDetail.md) records to store the data required for generating **localized** error messages.

The record is immutable: its properties are get-only, and `with` expressions cannot change them. The [TemplateId](TemplatedMessage_TemplateId.md) must not be `null`, empty or white-space only. The parameters are copied into one read-only dictionary ([Parameters](TemplatedMessage_Parameters.md)) when the instance is created; their names must be unique and not `null`. The `Parameters` of another message cannot change, so passing them to a new message shares them instead of copying them.

It is not a positional record and has no `Deconstruct` method; use a property pattern instead, e.g., `message is { TemplateId: "errors.email.required", Parameters: var parameters }`.

`ToString()` writes the contents of [Parameters](TemplatedMessage_Parameters.md), e.g., `TemplatedMessage { TemplateId = message-id, Parameters = { min = 3 } }`.

> [!NOTE]
> An instance is created with a constructor and passed to the factory methods of the [Error](../Error/Error.md) record (the `detailTemplated` parameter, e.g., [NotFound](../Error/Error_NotFound.md)), or created from a template identifier and parameters by [Error.WithErrorDetail](../Error/Error_WithErrorDetail.md) and by the [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) methods [WithDetailTemplated](../ErrorBuilder/ErrorBuilder_WithDetailTemplated.md) and [AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md).


## Constructors
| Name | Description |
| ---- | ----------- |
| [TemplatedMessage(String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](TemplatedMessage_constructors.md#templatedmessagestring-ienumerablekeyvaluepairstring-object) | Initializes a new instance with the parameters given as key-value pairs (e.g., a dictionary). |
| [TemplatedMessage(String, ReadOnlySpan&lt;(String, Object)&gt;)](TemplatedMessage_constructors.md#templatedmessagestring-readonlyspanstring-object) | Initializes a new instance with the parameters given as name-value tuples. |


## Properties
| Name | Description |
| ---- | ----------- |
| [Parameters](TemplatedMessage_Parameters.md) | Gets a read-only dictionary of parameters to be used when rendering the templated message. |
| [TemplateId](TemplatedMessage_TemplateId.md) | Gets the template identifier to be used to generate an error message. |


## Methods
| Name | Description |
| ---- | ----------- |
| [Equals(TemplatedMessage)](TemplatedMessage_Equals.md) | Determines whether the specified message is equal to the current one, comparing the parameters by content. |
| [GetHashCode()](TemplatedMessage_GetHashCode.md) | Returns a hash code consistent with [Equals(TemplatedMessage)](TemplatedMessage_Equals.md), incorporating the content of the parameters. |


## See Also
* [Error](../Error/Error.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
