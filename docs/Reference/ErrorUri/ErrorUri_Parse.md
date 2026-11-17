# ErrorUri.Parse(String) Method
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorUri.cs#L265" target="_blank">ErrorUri.cs</a>

Creates a new instance of the [ErrorUri](ErrorUri.md) record of the kind that the value has, validated by the factory method of that kind.

```csharp
public static ErrorUri Parse(string value);
```

### Parameters
#### `value` [String](https://learn.microsoft.com/dotnet/api/system.string)
The *URI*: `about:blank`, a relative reference starting with `/`, or an absolute *URI* (e.g., `tag:`, `urn:`, `https:` or `mailto:`).

### Returns
[ErrorUri](ErrorUri.md)

A new instance of the [ErrorUri](ErrorUri.md) record with the specified value.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `value` is `null`.

[UriFormatException](https://learn.microsoft.com/dotnet/api/system.uriformatexception)

The `value` is not of a known kind, or it is not valid for its kind (the exception then has the message of the factory method of that kind).

## Remarks
Use this method when the kind of the *URI* is not known in advance, e.g., when reading problem details from another service or writing a converter. When the kind is known, call its factory method directly.

The value can come from an untrusted source: the validation runs in linear time in the length of the value (the patterns use `RegexOptions.NonBacktracking`), so a long or crafted value cannot make it slow.

The kind is detected from the value:

| Value | Factory method |
| ----- | -------------- |
| `about:blank`, in any case (e.g., `about:BLANK`) | [None](ErrorUri_None.md) |
| starts with `/` | [Relative(String)](ErrorUri_Relative.md) |
| scheme `tag` | [Tag(String)](ErrorUri_Tag.md) |
| scheme `urn` | [Urn(String)](ErrorUri_Urn.md) |
| scheme `http` or `https` | [Locator(String)](ErrorUri_Locator.md) |
| any other scheme (e.g., `mailto:`, `ftp:`), including an `about` *URI* other than `about:blank` (e.g., `about:invalid`) | [Other(String)](ErrorUri_Other.md) |

A value without a scheme (a relative path such as `out-of-credit` or `../probs/x`, an empty string) is rejected.

> [!WARNING]
> Any other scheme is accepted (see [Other(String)](ErrorUri_Other.md)), e.g., `javascript:` or `file:`. The value can be checked for its syntax and length, but not for its intent: check the scheme before showing it as a link (e.g., allow only `http` and `https`).

Schemes are case-insensitive (*RFC 3986*, section 3.1), so `TAG:example.com,2026:x` is a *URI Tag*; the value is stored as passed. `about:blank` is the exception: it is recognized in any case (e.g., `ABOUT:blank`, `about:BLANK`), and the result always has the value `about:blank`. This is more lenient than *RFC 3986*, in which only the scheme is case-insensitive, so that a value that reads as `about:blank` does not become a problem type of its own. Any other `about` *URI* (e.g., `about:invalid`, `about:blank#x`) is kept as it is.

`ErrorUri.Parse(uri.Value)` returns an [ErrorUri](ErrorUri.md) equal to `uri`. [TryParse(String, ErrorUri)](ErrorUri_TryParse.md) applies the same rules without throwing. [ErrorUri](ErrorUri.md) also implements [IParsable&lt;ErrorUri&gt;](https://learn.microsoft.com/dotnet/api/system.iparsable-1), so generic code (e.g., ASP.NET Core Minimal API binding) can parse it.

## Examples
```csharp
// The kind of the type URI of a problem details object read from another service is not known in advance.
var typeUri = ErrorUri.Parse(problemDetails.Type ?? "about:blank");
var error = new ErrorBuilder(ErrorCategory.Failure, typeUri, problemDetails.Title ?? "Unknown error").Build();
```

## See Also
* [ErrorUri](ErrorUri.md)
* [TryParse(String, ErrorUri)](ErrorUri_TryParse.md)
* [RFC 3986: Uniform Resource Identifier (URI): Generic Syntax](https://datatracker.ietf.org/doc/html/rfc3986)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
