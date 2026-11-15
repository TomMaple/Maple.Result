# Result&lt;T&gt;.TryGetValue Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Gets the value of a successful result without an exception.

## Overloads
| Name | Description |
| ---- | ----------- |
| [TryGetValue(T)](#trygetvaluet) | Gets the value of a successful result. |
| [TryGetValue(T, Error)](#trygetvaluet-error) | Gets the value of a successful result, or the error of a failed one. |

## Remarks
Unlike [Value](ResultT_Value.md), these methods never throw. Their nullable annotations follow `T`: after `true`, the value is a `string` for a `Result<string>` and a `string?` for a `Result<string?>`; using the `out` values without checking the returned value gives a nullable warning.


## TryGetValue(T)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L317" target="_blank">Result.cs</a>

Gets the value of a successful result.

```csharp
[MemberNotNullWhen(false, nameof(Error))]
public bool TryGetValue([MaybeNullWhen(false)] out T value);
```

### Parameters
`value` T

When this method returns, the [Value](ResultT_Value.md) if the result is successful; otherwise, the default value of `T`.

### Returns
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the result is successful; otherwise, `false`. After `false`, [Error](ResultT_Error.md) is not `null`.

### Examples
For when the error is not needed, e.g., to fall back to another value:
```csharp
var name = settings.TryGetValue(out var configured)
    ? configured.DisplayName
    : "Guest";
```


## TryGetValue(T, Error)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L337" target="_blank">Result.cs</a>

Gets the value of a successful result, or the error of a failed one.

```csharp
public bool TryGetValue([MaybeNullWhen(false)] out T value, [NotNullWhen(false)] out Error? error);
```

### Parameters
`value` T

When this method returns, the [Value](ResultT_Value.md) if the result is successful; otherwise, the default value of `T`.

`error` [Error](../Error/Error.md)

When this method returns, `null` if the result is successful; otherwise, the [Error](ResultT_Error.md). It is not `null` when the method returns `false`.

### Returns
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the result is successful; otherwise, `false`.

### Examples
It works on an expression, with no variable for the result:
```csharp
public Result<Order> PlaceOrder(int userId)
{
    if (!GetUser(userId).TryGetValue(out var user, out var error))
        return error;

    return CreateOrder(user);
}
```

## See Also
* [Value](ResultT_Value.md)
* [TryGetError(Error)](ResultT_TryGetError.md)
