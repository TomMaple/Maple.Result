# ResultConfiguration Class
## Definition
Namespace: [Maple.Results](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.2/src/Maple.Result/ResultConfiguration.cs" target="_blank">ResultConfiguration.cs</a>

Global settings of the library.

```csharp
public static class ResultConfiguration
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → ResultConfiguration

## Remarks
Set the properties once at application startup, before any other code of the library runs. They apply to the whole process.

## Properties
| Name | Description |
| ---- | ----------- |
| [ContinueOnCapturedContext](ResultConfiguration_ContinueOnCapturedContext.md) | The default value of the `continueOnCapturedContext` argument of the asynchronous extension methods, also used by the LINQ query operators. |

## See Also
* [Maple.Results.Extensions namespace](../Extensions/namespace.md)
