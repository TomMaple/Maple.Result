# ErrorBuilder.AddErrorDetail Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Appends an individual error occurrence to the [ErrorDetails](../Error/Error_ErrorDetails.md) of the error to build.

## Overloads
| Name | Description |
| ---- | ----------- |
| [AddErrorDetail(ErrorDetail)](#adderrordetailerrordetail) | Appends the specified error detail. |
| [AddErrorDetail(String, String, String, ReadOnlySpan&lt;(String, Object)&gt;)](#adderrordetailstring-string-string-readonlyspanstring-object) | Appends a new error detail. |

## Remarks
The details are kept in the order they were added. Unlike [Error.WithErrorDetail](../Error/Error_WithErrorDetail.md), which copies the error on every call, these methods append in place, so they are the better choice when adding several details (e.g., in a loop).


## AddErrorDetail(ErrorDetail)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/ErrorBuilder.cs#L352" target="_blank">ErrorBuilder.cs</a>

Appends the specified individual error occurrence.

```csharp
public ErrorBuilder AddErrorDetail(ErrorDetail errorDetail);
```

### Parameters
#### `errorDetail` [ErrorDetail](../ErrorDetail/ErrorDetail.md)
A mandatory error occurrence to append.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `errorDetail` is `null`.

### Examples
```csharp
var builder = ErrorBuilder.Validation(typeUri, "Invalid data to register a new user.");

foreach (var errorDetail in validationResult.ErrorDetails)
    builder.AddErrorDetail(errorDetail);
```


## AddErrorDetail(String, String, String, ReadOnlySpan&lt;(String, Object)&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/ErrorBuilder.cs#L386" target="_blank">ErrorBuilder.cs</a>

Appends a new individual error occurrence.

```csharp
public ErrorBuilder AddErrorDetail(
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
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `detail` or a name in `detailParameters` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `propertyPointer` is not a *JSON Pointer* (e.g., `Email` or `Items[0].Name`), the `detail` is empty or consists only of white-space characters, the `detailTemplateId` is empty, consists only of white-space characters or starts or ends with white space, a name in `detailParameters` is empty, starts or ends with white space, is repeated, or differs only in case from another name (e.g., `Min` and `min`), or `detailParameters` are passed without a `detailTemplateId`.

In both cases nothing is appended.

### Examples
```csharp
var error = ErrorBuilder.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid"),
                "Invalid data to register a new user.")
            .AddErrorDetail(
                "/username",
                "The username needs to be at least 3 characters long.",
                "error:user:create:username-length",
                ("username", "j"), ("minLength", 3))
            .AddErrorDetail(
                "/email",
                "The email address is required.",
                "error:user:create:email-required")
            .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [Error.WithErrorDetail](../Error/Error_WithErrorDetail.md)
* [JSON Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
