# ThenExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/Extensions/ThenExtensions.cs" target="_blank">ThenExtensions.cs</a>

Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records that transform or chain a successful result.

```csharp
public static class ThenExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → ThenExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [Then(Result, Func&lt;Result&gt;)](ThenExtensions_Then.md#thenresult-funcresult) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [Then&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;)](ThenExtensions_Then.md#thent-tnextresultt-funct-resulttnext) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [Then&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)](ThenExtensions_Then.md#thent-tnextresultt-funct-tnext) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [Then&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;)](ThenExtensions_Then.md#thentresult-funcresultt) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [Then&lt;T&gt;(Result, Func&lt;T&gt;)](ThenExtensions_Then.md#thentresult-funct) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [Then&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Result&gt;)](ThenExtensions_Then.md#thentresultt-funct-result) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |

## See Also
* [IfSuccessExtensions](IfSuccessExtensions.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
