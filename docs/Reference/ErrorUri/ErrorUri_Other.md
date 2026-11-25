# ErrorUri.Other(String) Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/ErrorUri.cs#L229" target="_blank">ErrorUri.cs</a>

Creates a new instance of the [ErrorUri](ErrorUri.md) record with an absolute *URI* whose scheme has no factory method of its own (e.g., `mailto:`, `ftp:` or a custom scheme).

```csharp
public static ErrorUri Other(string uri);
```

### Parameters
#### `uri` [String](https://learn.microsoft.com/dotnet/api/system.string)
The absolute *URI* (*RFC 3986*, section 3), e.g., `mailto:support@example.com`; non-ASCII characters must be percent-encoded.

### Returns
[ErrorUri](ErrorUri.md)

A new instance of the [ErrorUri](ErrorUri.md) record with the specified *URI*.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `uri` is `null`.

[UriFormatException](https://learn.microsoft.com/dotnet/api/system.uriformatexception)

The `uri` is not a valid absolute *URI*, its scheme has a factory method of its own (`tag`, `urn`, `http`, `https`, in any case), or it is `about:blank` (in any case); the message then names that method, or [None](ErrorUri_None.md).

## Remarks
*RFC 9457* allows any *URI* reference as the type and the instance of a problem (sections 3.1.1 and 3.1.5) and requires consumers to use the type as its identifier. This method keeps the values that other services write with other schemes, so that reading them does not change the identity of the problem type. To create an error, prefer [Locator(String)](ErrorUri_Locator.md), [Tag(String)](ErrorUri_Tag.md), [Urn(String)](ErrorUri_Urn.md) or [Relative(String)](ErrorUri_Relative.md).

The method validates the value against the *RFC 3986* syntax of an absolute *URI*, `scheme ":" hier-part [ "?" query ] [ "#" fragment ]`:
* the scheme starts with a letter, followed by letters, digits, `+`, `-` or `.`; it is case-insensitive;
* the rest consists of letters, digits, `-._~:/?[]@!$&'()*+,;=` and percent-encoded octets (`%XX`), with at most one `#`.

The value is stored as passed, without normalization, so white space (including a trailing new line), invalid percent-encoding (e.g., `%zz`) and literal non-ASCII characters are rejected. Values are compared character by character, so `MAILTO:a` and `mailto:a` are different.

An `about` *URI* is accepted like any other scheme (e.g., `about:invalid`, `about:blank#x`). The only exception is `about:blank` itself, in any case (e.g., `about:BLANK`): it is [None](ErrorUri_None.md), which [Parse(String)](ErrorUri_Parse.md) returns for it, so it is rejected here.

[Parse(String)](ErrorUri_Parse.md) and [TryParse(String, ErrorUri)](ErrorUri_TryParse.md) use this method for the schemes without a factory method of their own.

> [!WARNING]
> Any other scheme is accepted, including `javascript:`, `vbscript:`, `data:` and `file:`: *RFC 3986* and *RFC 9457* do not restrict the scheme, and the values of other services must be kept. Treat such a value as untrusted: a consumer that shows a type or an instance *URI* as a link (e.g., in HTML) or opens it has to check the scheme first, e.g., allow only `http` and `https`. *RFC 9457* (section 3.1.1) also says that consumers should not dereference the type *URI* automatically.

## Examples
```csharp
// A problem type written by another service with a scheme that has no factory method of its own.
var typeUri = ErrorUri.Other("mailto:support@example.com");
```

## See Also
* [ErrorUri](ErrorUri.md)
* [Parse(String)](ErrorUri_Parse.md)
* [RFC 3986: Uniform Resource Identifier (URI): Generic Syntax](https://datatracker.ietf.org/doc/html/rfc3986)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
