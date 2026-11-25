# ElseExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.3/src/Maple.Result/Extensions/ElseExtensions.cs" target="_blank">ElseExtensions.cs</a>

Provides extension methods for the [IOperationResult](../IOperationResult/IOperationResult.md) interface implementations that replace a failed result.

```csharp
public static class ElseExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → ElseExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [Else&lt;TResult&gt;(TResult, Func&lt;Error, TResult&gt;)](ElseExtensions_Else.md#elsetresulttresult-funcerror-tresult) | Executes the provided function if the `TResult` instance represents a failed operation and returns its result. |

## See Also
* [IfErrorExtensions](IfErrorExtensions.md)
* [IOperationResult](../IOperationResult/IOperationResult.md)
