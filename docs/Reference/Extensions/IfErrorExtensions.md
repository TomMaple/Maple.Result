# IfErrorExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/IfErrorExtensions.cs" target="_blank">IfErrorExtensions.cs</a>

Provides extension methods for the [IOperationResult](../IOperationResult/IOperationResult.md) interface implementations that run a side effect on a failed result and return it unchanged.

```csharp
public static class IfErrorExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → IfErrorExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [IfError&lt;TResult&gt;(TResult, Action&lt;Error&gt;)](IfErrorExtensions_IfError.md#iferrortresulttresult-actionerror) | Executes the provided action if the `TResult` instance represents a failed operation. |

## See Also
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
