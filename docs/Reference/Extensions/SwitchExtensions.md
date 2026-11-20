# SwitchExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/SwitchExtensions.cs" target="_blank">SwitchExtensions.cs</a>

Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records that run one of two actions depending on whether a result is successful, and return nothing.

```csharp
public static class SwitchExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → SwitchExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [Switch(Result, Action, Action&lt;Error&gt;)](SwitchExtensions_Switch.md#switchresult-action-actionerror) | Invokes one of the specified actions depending on whether the [Result](../Result/Result.md) is successful. |
| [Switch&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;, Action&lt;Error&gt;)](SwitchExtensions_Switch.md#switchtresultt-actiont-actionerror) | Invokes one of the specified actions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful. |

## See Also
* [MatchExtensions](MatchExtensions.md)
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
