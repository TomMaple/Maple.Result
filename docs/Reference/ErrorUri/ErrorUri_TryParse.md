# ErrorUri.TryParse(String, ErrorUri) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorUri.cs#L278" target="_blank">ErrorUri.cs</a>

Tries to create a new instance of the [ErrorUri](ErrorUri.md) record of the kind that the value has, with the rules of [Parse(String)](ErrorUri_Parse.md).

```csharp
public static bool TryParse(string? value, out ErrorUri result);
```

### Parameters
#### `value` [String](https://learn.microsoft.com/dotnet/api/system.string)
The *URI*; see [Parse(String)](ErrorUri_Parse.md).

#### `result` [ErrorUri](ErrorUri.md)
The [ErrorUri](ErrorUri.md) with the specified value, if it is valid; otherwise, the default [ErrorUri](ErrorUri.md).

### Returns
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the `value` is a valid error *URI*; otherwise, `false` (also when it is `null`).

## Remarks
The method never throws, and a rejected value is checked without throwing an exception internally, so it suits values that are often invalid.

On failure, `result` is the default [ErrorUri](ErrorUri.md), whose [Value](ErrorUri_Value.md) is `about:blank`; check the return value rather than `result`.

## Examples
```csharp
if (!ErrorUri.TryParse(problemDetails.Type, out var typeUri))
    typeUri = ErrorUri.None();
```

## See Also
* [ErrorUri](ErrorUri.md)
* [Parse(String)](ErrorUri_Parse.md)
