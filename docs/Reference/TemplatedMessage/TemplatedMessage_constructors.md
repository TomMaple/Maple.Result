# TemplatedMessage Constructors
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Initializes a new instance of the [TemplatedMessage](TemplatedMessage.md) record.

## Overloads
| Name | Description |
| ---- | ----------- |
| [TemplatedMessage(String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](#templatedmessagestring-ienumerablekeyvaluepairstring-object) | Initializes a new instance with the parameters given as key-value pairs (e.g., a dictionary). |
| [TemplatedMessage(String, ReadOnlySpan&lt;(String, Object)&gt;)](#templatedmessagestring-readonlyspanstring-object) | Initializes a new instance with the parameters given as name-value tuples. |

## Remarks
Both constructors copy the parameters into one read-only dictionary whose comparer ignores case (whatever the comparer of the source), so later changes to the source are not reflected. The [Parameters](TemplatedMessage_Parameters.md) of another message are shared instead of copied, since they cannot change. Parameter values can be `null`; parameter names must be unique and not `null`.

> [!NOTE]
> An instance is created with a constructor and passed to the factory methods of the [Error](../Error/Error.md) record (the `detailTemplated` parameter, e.g., [NotFound](../Error/Error_NotFound.md)), or created from a template identifier and parameters by [Error.WithErrorDetail](../Error/Error_WithErrorDetail.md) and by the [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) methods [WithDetailTemplated](../ErrorBuilder/ErrorBuilder_WithDetailTemplated.md) and [AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md).


## TemplatedMessage(String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/TemplatedMessage.cs#L54" target="_blank">TemplatedMessage.cs</a>

Initializes a new instance of the [TemplatedMessage](TemplatedMessage.md) record with the parameters given as key-value pairs (e.g., a dictionary).

```csharp
[OverloadResolutionPriority(1)]
public TemplatedMessage(string templateId, IEnumerable<KeyValuePair<string, object?>>? parameters = null);
```

### Parameters
#### `templateId` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory identifier of the message template.

#### `parameters` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1)&lt;[KeyValuePair](https://learn.microsoft.com/dotnet/api/system.collections.generic.keyvaluepair-2)&lt;[String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object)&gt;&gt;
The optional parameters (names and values); values can be `null`. When `null` or empty, [Parameters](TemplatedMessage_Parameters.md) is `null`.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `templateId` or a parameter name is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `templateId` is empty, consists only of white-space characters or starts or ends with white space, or a parameter name is empty, starts or ends with white space, is repeated, or differs only in case from another name (e.g., `Min` and `min`).

### Remarks
`null` and an empty collection expression (`[]`) as the second argument bind to this constructor.

### Examples
```csharp
var message = new TemplatedMessage(
    "errors.account.notFound",
    new Dictionary<string, object?> { ["accountId"] = "D12345" });
```


## TemplatedMessage(String, ReadOnlySpan&lt;(String, Object)&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/TemplatedMessage.cs#L75" target="_blank">TemplatedMessage.cs</a>

Initializes a new instance of the [TemplatedMessage](TemplatedMessage.md) record with the parameters given as name-value tuples.

```csharp
public TemplatedMessage(string templateId, params ReadOnlySpan<(string Key, object? Value)> parameters);
```

### Parameters
#### `templateId` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory identifier of the message template.

#### `parameters` [ReadOnlySpan](https://learn.microsoft.com/dotnet/api/system.readonlyspan-1)&lt;([String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object))&gt;
The parameters (names and values), given inline, as an array or as a collection expression; values can be `null`. When none are passed, [Parameters](TemplatedMessage_Parameters.md) is `null`.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `templateId` or a parameter name is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `templateId` is empty, consists only of white-space characters or starts or ends with white space, or a parameter name is empty, starts or ends with white space, is repeated, or differs only in case from another name (e.g., `Min` and `min`).

### Examples
```csharp
var message = new TemplatedMessage("errors.username.length", ("min", 3), ("max", 20));
```

## See also
* [TemplatedMessage](TemplatedMessage.md)
* [Error](../Error/Error.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
