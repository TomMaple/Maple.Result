# Error.WithErrorDetail Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Returns a copy of the [Error](Error.md) with an individual error occurrence appended to its [ErrorDetails](Error_ErrorDetails.md).

## Overloads
| Name | Description |
| ---- | ----------- |
| [WithErrorDetail(ErrorDetail)](#witherrordetailerrordetail) | Returns a copy of the error with the specified error detail appended. |
| [WithErrorDetail(String, String, String, ReadOnlySpan&lt;(String, Object)&gt;)](#witherrordetailstring-string-string-readonlyspanstring-object) | Returns a copy of the error with a new error detail appended. |

## Remarks
These methods **do not modify** the current instance; they return a new [Error](Error.md). The other members, including the existing error details, are kept.

Each call copies the error. To add several details, use [ToBuilder()](Error_ToBuilder.md) (or start with an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md)) and [AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md), which appends the details in place and builds the error once.


## WithErrorDetail(ErrorDetail)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Error.cs#L258" target="_blank">Error.cs</a>

Returns a copy of the current error with the specified individual error occurrence appended to the [ErrorDetails](Error_ErrorDetails.md).

```csharp
public Error WithErrorDetail(ErrorDetail errorDetail);
```

### Parameters
#### `errorDetail` [ErrorDetail](../ErrorDetail/ErrorDetail.md)
A mandatory error occurrence to append.

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record with the error detail appended. The current instance is not modified.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `errorDetail` is `null`.

### Examples
```csharp
var errorDetail = new ErrorDetail("/username", "The username is already taken.");

var error = Error.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid"),
                "Invalid data to register a new user.")
            .WithErrorDetail(errorDetail);
```


## WithErrorDetail(String, String, String, ReadOnlySpan&lt;(String, Object)&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Error.cs#L297" target="_blank">Error.cs</a>

Returns a copy of the current error with a new individual error occurrence appended to the [ErrorDetails](Error_ErrorDetails.md).

```csharp
public Error WithErrorDetail(
        string? propertyPointer,
        string detail,
        string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailParameters);
```

### Parameters
#### `propertyPointer` [String](https://learn.microsoft.com/dotnet/api/system.string)
An optional [JSON Pointer](https://datatracker.ietf.org/doc/html/rfc6901) to the specific part of the request that caused the error: `null`, empty (the whole input), a pointer that starts with `/` (e.g., `/email`), or its URI fragment form (e.g., `#/email`). See [PropertyPointer](../ErrorDetail/ErrorDetail_PropertyPointer.md).

#### `detail` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory human-readable message that describes the specific error occurrence.

#### `detailTemplateId` [String](https://learn.microsoft.com/dotnet/api/system.string)
An optional identifier of the message template to generate localized message. When it is `null`, no [DetailTemplated](../ErrorDetail/ErrorDetail_DetailTemplated.md) is set.

#### `detailParameters` [ReadOnlySpan](https://learn.microsoft.com/dotnet/api/system.readonlyspan-1)&lt;([String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object))&gt;
Optional parameters (inline, an array or a collection expression) to be used when rendering the templated message. Values can be `null`; names must be unique. Requires `detailTemplateId`.

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record with the error detail appended. The current instance is not modified.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `detail` or a name in `detailParameters` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `propertyPointer` is not a *JSON Pointer* (e.g., `Email` or `Items[0].Name`), the `detail` is empty or consists only of white-space characters, the `detailTemplateId` is empty, consists only of white-space characters or starts or ends with white space, a name in `detailParameters` is empty, starts or ends with white space, is repeated, or differs only in case from another name (e.g., `Min` and `min`), or `detailParameters` are passed without a `detailTemplateId`.

### Examples
```csharp
var error = Error.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid"),
                "Invalid data to register a new user.")
            .WithErrorDetail(
                "/username",
                "The username is already taken.",
                "error:user:create:username-taken",
                ("username", "john_doe"));
```

## See Also
* [ErrorDetails](Error_ErrorDetails.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [ErrorBuilder.AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md)
* [JSON Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
