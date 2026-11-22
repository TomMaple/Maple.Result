# Error.WithMetadata Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Returns a copy of the [Error](Error.md) with the specified [Metadata](Error_Metadata.md) entries added or replaced.

## Overloads
| Name | Description |
| ---- | ----------- |
| [WithMetadata(String, Object)](#withmetadatastring-object) | Returns a copy of the error with the specified metadata entry added or replaced. |
| [WithMetadata(IEnumerable<KeyValuePair<String, Object>>)](#withmetadataienumerablekeyvaluepairstring-object) | Returns a copy of the error with the specified metadata entries added or replaced. |
| [WithMetadata(ReadOnlySpan<(String, Object)>)](#withmetadatareadonlyspanstring-object) | Returns a copy of the error with the specified metadata entries, given as key-value tuples, added or replaced. |

## Remarks
These methods **do not modify** the current instance; they return a new [Error](Error.md) with a new, read-only [Metadata](Error_Metadata.md) dictionary. To change several entries without copying the error for each one, use an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) (see [ErrorBuilder.WithMetadata](../ErrorBuilder/ErrorBuilder_WithMetadata.md)).

Keys are compared ordinally (case-sensitive), and a key that differs only in case from an existing key is rejected; values can be `null`. See [Metadata](Error_Metadata.md#keys) for the rules for keys.

A key that already exists is never an error: its value is replaced, so code that handles an error created elsewhere can set an entry without checking first. A key in a different case is an error; see [Adding metadata to an error created elsewhere](Error_Metadata.md#adding-metadata-to-an-error-created-elsewhere).


## WithMetadata(String, Object)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Error.cs#L374" target="_blank">Error.cs</a>

Returns a copy of the current error with the specified metadata entry added, or replaced if the key already exists.

```csharp
public Error WithMetadata(string key, object? value);
```

### Parameters
#### `key` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory key of the metadata entry.

#### `value` [Object](https://learn.microsoft.com/dotnet/api/system.object)
The value of the metadata entry; it can be `null`.

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record with the entry added or replaced. The current instance is not modified.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `key` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `key` is empty, consists only of white-space characters, starts or ends with white space, is [reserved](Error_Metadata.md#keys), or differs only in case from an existing key.

### Examples
```csharp
var error = Error.LimitExceeded(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
                "Cannot generate the report.")
            .WithMetadata("limit", 100)
            .WithMetadata("remaining", 0);
```


## WithMetadata(IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Error.cs#L396" target="_blank">Error.cs</a>

Returns a copy of the current error with the specified metadata entries added. Entries with existing keys are replaced; the other existing entries are kept.

```csharp
[OverloadResolutionPriority(1)]
public Error WithMetadata(IEnumerable<KeyValuePair<string, object?>> metadata);
```

### Parameters
#### `metadata` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1)&lt;[KeyValuePair](https://learn.microsoft.com/dotnet/api/system.collections.generic.keyvaluepair-2)&lt;[String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object)&gt;&gt;
A mandatory sequence of the metadata entries to add or replace; values can be `null`.

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record with the entries added or replaced. The current instance is not modified.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `metadata` sequence or any of its keys is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Any key is empty, consists only of white-space characters, starts or ends with white space, is [reserved](Error_Metadata.md#keys), or differs only in case from an existing key or from another key of the same call.

### Examples
```csharp
var error = Error.LimitExceeded(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
                "Cannot generate the report.")
            .WithMetadata(new Dictionary<string, object?> { ["limit"] = 100, ["remaining"] = 0 });
```

### Remarks
`null` and an empty collection expression (`[]`) bind to this overload.


## WithMetadata(ReadOnlySpan&lt;(String, Object)&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Error.cs#L418" target="_blank">Error.cs</a>

Returns a copy of the current error with the specified metadata entries, given as key-value tuples, added. Entries with existing keys are replaced; the other existing entries are kept.

```csharp
public Error WithMetadata(params ReadOnlySpan<(string Key, object? Value)> metadata);
```

### Parameters
#### `metadata` [ReadOnlySpan](https://learn.microsoft.com/dotnet/api/system.readonlyspan-1)&lt;([String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object))&gt;
The metadata entries to add or replace, given inline, as an array or as a collection expression; values can be `null`. A key repeated in the same call takes the last value.

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record with the entries added or replaced. The current instance is not modified.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

Any key is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Any key is empty, consists only of white-space characters, starts or ends with white space, is [reserved](Error_Metadata.md#keys), or differs only in case from an existing key or from another key of the same call.

### Examples
```csharp
var error = Error.LimitExceeded(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
                "Cannot generate the report.")
            .WithMetadata(("limit", 100), ("remaining", 0));
```

## See Also
* [Metadata](Error_Metadata.md)
* [WithoutMetadata](Error_WithoutMetadata.md)
* [TryGetMetadata&lt;T&gt;(String, T)](Error_TryGetMetadata.md)
