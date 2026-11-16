# MatchExtensions Class
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs" target="_blank">MatchExtensions.cs</a>

Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records that fold a result into one value: a function for the success and a function for the error, which return the same type.

```csharp
public static class MatchExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → MatchExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [Match&lt;TOut&gt;(Result, Func&lt;TOut&gt;, Func&lt;Error, TOut&gt;)](MatchExtensions_Match.md#matchtoutresult-functout-funcerror-tout) | Invokes one of the specified functions depending on whether the [Result](../Result/Result.md) is successful and returns the value that it returns. |
| [Match&lt;T, TOut&gt;(Result&lt;T&gt;, Func&lt;T, TOut&gt;, Func&lt;Error, TOut&gt;)](MatchExtensions_Match.md#matcht-toutresultt-funct-tout-funcerror-tout) | Invokes one of the specified functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) is successful and returns the value that it returns. |

## See Also
* [SwitchExtensions](SwitchExtensions.md)
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
