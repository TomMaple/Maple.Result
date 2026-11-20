# Error Record
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs" target="_blank">Error.cs</a>

Represents an error in a [Result](../Result/Result.md) with optional properties for localization and multiple error details.

```csharp
public sealed record Error
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → Error

## Examples
```csharp
public Result DeleteUser(int userId)
{
    if (!UserExists(userId))
    {
        return Error.NotFound(
            ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found"),
            "User has not been found.",
            "User with the specified ID does not exist in the system.",
            ErrorUri.Locator("https://api.exampleapp.com/error/e-1234-5678-9012-345678901234/details"),
            new TemplatedMessage("errors-user-notfound", ("userId", userId)));
    }

    // Proceed with user deletion

    return Result.Success();
}
```

## Remarks
It follows the [RFC 9457](https://datatracker.ietf.org/doc/html/rfc9457) standard for error responses.

### Equality
[Error](Error.md) provides value-based equality: two instances are equal when all of their properties are equal, including the [ErrorDetails](Error_ErrorDetails.md) collection, which is compared **by content** (order-sensitive) rather than by reference, and the [Metadata](Error_Metadata.md) dictionary, which is compared **by content** independent of the order of its entries. See [Equals(Error)](Error_Equals.md) and [GetHashCode()](Error_GetHashCode.md).

### Immutability
[Error](Error.md) is immutable: all its properties are get-only, and the [ErrorDetails](Error_ErrorDetails.md) and [Metadata](Error_Metadata.md) collections are read-only. It has no public constructor. Create it with:
* one of the factory methods named after the categories (e.g., [Validation](Error_Validation.md)), or
* an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md), when the error is assembled step by step (e.g., one error detail per invalid field).

Methods such as [WithErrorDetail](Error_WithErrorDetail.md), [WithMetadata](Error_WithMetadata.md) and [WithoutMetadata](Error_WithoutMetadata.md) return a changed copy. To change any other member, or several at once, use [ToBuilder()](Error_ToBuilder.md) and [ErrorBuilder.Build()](../ErrorBuilder/ErrorBuilder_Build.md).

An [Error](Error.md) is not validated again after it is created: the title is never `null` or blank, and the category is always a defined [ErrorCategory](../ErrorCategory.md) value.

### Serialization
The default serializers (*System.Text.Json* and *Newtonsoft.Json*) can write an [Error](Error.md) as JSON. Reading it back requires a dedicated converter, because the type has no public constructor or setters.

### Metadata
[Metadata](Error_Metadata.md) holds machine-readable, response-level values (the *extension members* of RFC 9457). Use [WithMetadata(String, Object)](Error_WithMetadata.md) and [WithoutMetadata](Error_WithoutMetadata.md) to obtain a changed copy, and [TryGetMetadata&lt;T&gt;(String, T)](Error_TryGetMetadata.md) to read a value of a specific type.

### Text representation
`ToString()` writes all members, including the contents of [ErrorDetails](Error_ErrorDetails.md) and [Metadata](Error_Metadata.md), e.g., `Error { Category = NotFound, TypeUri = about:blank, Title = Missing, …, ErrorDetails = [], Metadata = {} }`.

The debugger shows a short form instead: the category, the title and the number of error details, if there are any (e.g., `Validation "Invalid request" (3 details)`).

## Properties
| Name | Description |
| ---- | ----------- |
| [Category](Error_Category.md) | Gets the category of the error. |
| [Detail](Error_Detail.md) | Gets a human-readable explanation of the specific occurrence of the problem. |
| [DetailTemplated](Error_DetailTemplated.md) | Gets a templated explanation of the specific occurrence of the problem. |
| [ErrorDetails](Error_ErrorDetails.md) | Gets a collection of error details. |
| [InstanceUri](Error_InstanceUri.md) | Gets a *URI* that identifies the specific occurrence of the problem. |
| [Metadata](Error_Metadata.md) | Gets the additional, machine-readable members of the problem. |
| [Title](Error_Title.md) | Gets a short, human-readable summary of the problem type. |
| [TypeUri](Error_TypeUri.md) | Gets a *URI* that identifies the type of the problem. |

## Methods
| Name | Description |
| ---- | ----------- |
| [Conflict(ErrorUri, String, String, Nullable&lt;ErrorUri&gt;, TemplatedMessage)](Error_Conflict.md) | Creates a new conflict error with specified parameters. |
| [Critical(ErrorUri, String, String, Nullable&lt;ErrorUri&gt;, TemplatedMessage)](Error_Critical.md) | Creates a new critical error with specified parameters. |
| [Equals(Error)](Error_Equals.md) | Determines whether the specified error is equal to the current one, comparing the error details and the metadata by content. |
| [Failure(ErrorUri, String, String, Nullable&lt;ErrorUri&gt;, TemplatedMessage)](Error_Failure.md) | Creates a new failure error with specified parameters. |
| [GetHashCode()](Error_GetHashCode.md) | Returns a hash code consistent with [Equals(Error)](Error_Equals.md), incorporating the error details and the metadata content. |
| [LimitExceeded(ErrorUri, String, String, Nullable&lt;ErrorUri&gt;, TemplatedMessage)](Error_LimitExceeded.md) | Creates a new limit exceeded error with specified parameters. |
| [NotFound(ErrorUri, String, String, Nullable&lt;ErrorUri&gt;, TemplatedMessage)](Error_NotFound.md) | Creates a new not found error with specified parameters. |
| [NotImplemented(ErrorUri, String, String, Nullable&lt;ErrorUri&gt;, TemplatedMessage)](Error_NotImplemented.md) | Creates a new not implemented error with specified parameters. |
| [PermissionDenied(ErrorUri, String, String, Nullable&lt;ErrorUri&gt;, TemplatedMessage)](Error_PermissionDenied.md) | Creates a new permission denied error with specified parameters. |
| [Timeout(ErrorUri, String, String, Nullable&lt;ErrorUri&gt;, TemplatedMessage)](Error_Timeout.md) | Creates a new timeout error with specified parameters. |
| [ToBuilder()](Error_ToBuilder.md) | Returns a new [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) initialized with the members of the current error. |
| [TryGetMetadata&lt;T&gt;(String, T)](Error_TryGetMetadata.md) | Gets the metadata value of the specified key if it is of the requested type. |
| [Unauthenticated(ErrorUri, String, String, Nullable&lt;ErrorUri&gt;, TemplatedMessage)](Error_Unauthenticated.md) | Creates a new unauthenticated error with specified parameters. |
| [Unavailable(ErrorUri, String, String, Nullable&lt;ErrorUri&gt;, TemplatedMessage)](Error_Unavailable.md) | Creates a new unavailable error with specified parameters. |
| [Validation(ErrorUri, String, String, Nullable&lt;ErrorUri&gt;, TemplatedMessage)](Error_Validation.md) | Creates a new validation error with specified parameters. |
| [WithErrorDetail(ErrorDetail)](Error_WithErrorDetail.md#witherrordetailerrordetail) | Returns a copy of the error with the specified error detail appended. |
| [WithErrorDetail(String, String, String, ReadOnlySpan&lt;(String, Object)&gt;)](Error_WithErrorDetail.md#witherrordetailstring-string-string-readonlyspanstring-object) | Returns a copy of the error with a new error detail appended. |
| [WithMetadata(IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_WithMetadata.md#withmetadataienumerablekeyvaluepairstring-object) | Returns a copy of the error with the specified metadata entries added or replaced. |
| [WithMetadata(ReadOnlySpan&lt;(String, Object)&gt;)](Error_WithMetadata.md#withmetadatareadonlyspanstring-object) | Returns a copy of the error with the specified metadata entries, given as key-value tuples, added or replaced. |
| [WithMetadata(String, Object)](Error_WithMetadata.md#withmetadatastring-object) | Returns a copy of the error with the specified metadata entry added or replaced. |
| [WithoutMetadata()](Error_WithoutMetadata.md#withoutmetadata) | Returns a copy of the error without any metadata. |
| [WithoutMetadata(String)](Error_WithoutMetadata.md#withoutmetadatastring) | Returns a copy of the error without the metadata entry of the specified key. |

## See Also
* [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md)
* [ErrorCategory](../ErrorCategory.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
