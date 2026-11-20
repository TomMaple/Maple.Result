# IfSuccessExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/IfSuccessExtensions.cs" target="_blank">IfSuccessExtensions.cs</a>

Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records that run a side effect on a successful result and return it unchanged.

```csharp
public static class IfSuccessExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → IfSuccessExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [IfSuccess(Result, Action)](IfSuccessExtensions_IfSuccess.md#ifsuccessresult-action) | Executes the specified action, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccess&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;)](IfSuccessExtensions_IfSuccess.md#ifsuccesstresultt-actiont) | Executes the specified action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |

## See Also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
