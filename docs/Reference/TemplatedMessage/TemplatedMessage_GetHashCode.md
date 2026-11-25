# TemplatedMessage.GetHashCode() Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/TemplatedMessage.cs#L160" target="_blank">TemplatedMessage.cs</a>

Returns a hash code that is consistent with [Equals(TemplatedMessage)](TemplatedMessage_Equals.md), incorporating the content of the [Parameters](TemplatedMessage_Parameters.md) collection.

```csharp
public override int GetHashCode();
```

### Returns
[Int32](https://learn.microsoft.com/dotnet/api/system.int32)

A hash code for the current [TemplatedMessage](TemplatedMessage.md).

## Remarks
The hash code is computed from the [TemplateId](TemplatedMessage_TemplateId.md) and from the entries of the [Parameters](TemplatedMessage_Parameters.md) dictionary, which are combined independently of their order. Equal [TemplatedMessage](TemplatedMessage.md) instances always produce the same hash code.

[TemplatedMessage](TemplatedMessage.md) is immutable, so its hash code never changes over the lifetime of an instance.

> [!NOTE]
> The hash code of a parameter comes from the value itself. Storing a mutable object (e.g., a list) and changing it later changes the hash code of the message.

## See Also
* [Equals(TemplatedMessage)](TemplatedMessage_Equals.md)
* [Parameters](TemplatedMessage_Parameters.md)
* [Error.GetHashCode()](../Error/Error_GetHashCode.md)
