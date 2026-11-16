# ErrorDetail.Pointer(Object[]) Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorDetail.cs#L95" target="_blank">ErrorDetail.cs</a>

Builds a *JSON Pointer* ([RFC 6901](https://datatracker.ietf.org/doc/html/rfc6901)) in the string form from names and array indexes, escaping the names.

```csharp
public static string Pointer(params ReadOnlySpan<object> segments);
```

### Parameters
#### `segments` [ReadOnlySpan](https://learn.microsoft.com/dotnet/api/system.readonlyspan-1)&lt;[Object](https://learn.microsoft.com/dotnet/api/system.object)&gt;
The names ([string](https://learn.microsoft.com/dotnet/api/system.string)) and the array indexes (any built-in integer type, e.g., [int](https://learn.microsoft.com/dotnet/api/system.int32) or [long](https://learn.microsoft.com/dotnet/api/system.int64)), in order.

### Returns
[string](https://learn.microsoft.com/dotnet/api/system.string)

The pointer, e.g., `/items/0/name`; empty when there are no segments.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)<br>
A segment is `null`.

[ArgumentOutOfRangeException](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception)<br>
An index is negative.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)<br>
A segment is not a `string` or a built-in integer type (e.g., a `char`, an enumeration value, a `double` or an `Int128`).

## Remarks
A `string` segment is a name: `~` is written as `~0` first, then `/` as `~1` (e.g., the name `a/b` gives `/a~1b`). An empty name is a valid segment (`Pointer("")` is `/`). A segment of a built-in integer type (`sbyte`, `byte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `nint` or `nuint`) is an array index: it must not be negative, and it is written in decimal with the invariant culture. A `char` (a character, not a number) and an enumeration value are not indexes.

The result is always accepted by the constructor of [ErrorDetail](ErrorDetail.md). Without segments, it is the empty pointer, which points to the whole input.

Use it for a name that comes from the input, e.g., a dictionary key or a form field, because a `~` in a name that is not escaped makes the constructor throw, and a `/` gives a wrong pointer.

## Examples
```csharp
ErrorDetail.Pointer("items", 0, "name");   // /items/0/name
ErrorDetail.Pointer("a/b", "c~d");         // /a~1b/c~0d
ErrorDetail.Pointer("rows", row.Index);    // /rows/3, e.g., with a uint index
ErrorDetail.Pointer();                     // (empty): the whole input

var error = Error.Validation(ErrorUri.None, "Invalid data.")
    .WithErrorDetail(ErrorDetail.Pointer("items", 1, fieldName), "The value is required.");
```

## See Also
* [ErrorDetail.PropertyPointer](ErrorDetail_PropertyPointer.md)
* [RFC 6901: JavaScript Object Notation (JSON) Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
