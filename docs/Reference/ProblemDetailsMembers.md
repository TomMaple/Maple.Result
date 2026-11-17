# ProblemDetailsMembers Class
## Definition
Namespace: [Maple.Results](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ProblemDetailsMembers.cs#L29" target="_blank">ProblemDetailsMembers.cs</a>

The member names of the problem details object ([RFC 9457](https://datatracker.ietf.org/doc/html/rfc9457)) that represents an [Error](Error/Error.md).

```csharp
public static class ProblemDetailsMembers
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → ProblemDetailsMembers

## Remarks
The packages that write and read problem details share these names, so that an `Error` written by one service is read back the same way by another. The format is described in [Problem Details Format](../problem-details.md).

The top-level members (`Type`, `Title`, `Status`, `Detail`, `Instance`, `Errors` and `DetailTemplated`) are **reserved**: they cannot be used as [Metadata](Error/Error_Metadata.md) keys, in any case, because each metadata entry becomes a top-level extension member. The names inside an `errors` item or a `detailTemplated` object are not reserved.

## Fields
### Top-level members
| Name            | Value               | Description |
| --------------- | ------------------- | ----------- |
| <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ProblemDetailsMembers.cs#L34" target="_blank">Type</a> | `"type"` | The [TypeUri](Error/Error_TypeUri.md) that identifies the problem type (RFC 9457, section 3.1.1). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ProblemDetailsMembers.cs#L39" target="_blank">Title</a> | `"title"` | The [Title](Error/Error_Title.md), a short summary of the problem type (RFC 9457, section 3.1.3). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ProblemDetailsMembers.cs#L44" target="_blank">Status</a> | `"status"` | The HTTP status code mapped from the [Category](Error/Error_Category.md) (RFC 9457, section 3.1.2); the [default mapping](../problem-details.md#category-and-status) can be customized. |
| <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ProblemDetailsMembers.cs#L49" target="_blank">Detail</a> | `"detail"` | The [Detail](Error/Error_Detail.md), an explanation of this occurrence (RFC 9457, section 3.1.4). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ProblemDetailsMembers.cs#L54" target="_blank">Instance</a> | `"instance"` | The [InstanceUri](Error/Error_InstanceUri.md) of this occurrence (RFC 9457, section 3.1.5). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ProblemDetailsMembers.cs#L62" target="_blank">Errors</a> | `"errors"` | An array with one object per [ErrorDetails](Error/Error_ErrorDetails.md) item (extension member). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ProblemDetailsMembers.cs#L70" target="_blank">DetailTemplated</a> | `"detailTemplated"` | The [DetailTemplated](Error/Error_DetailTemplated.md) object (extension member). |

### Members of an `errors` item
| Name            | Value               | Description |
| --------------- | ------------------- | ----------- |
| <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ProblemDetailsMembers.cs#L75" target="_blank">ErrorPointer</a> | `"pointer"` | The [PropertyPointer](ErrorDetail/ErrorDetail_PropertyPointer.md) of the error detail. |
| <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ProblemDetailsMembers.cs#L80" target="_blank">ErrorDetail</a> | `"detail"` | The [Detail](ErrorDetail/ErrorDetail_Detail.md) of the error detail. |
| <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ProblemDetailsMembers.cs#L86" target="_blank">ErrorDetailTemplated</a> | `"detailTemplated"` | The [DetailTemplated](ErrorDetail/ErrorDetail_DetailTemplated.md) object of the error detail. |

### Members of a templated message
| Name            | Value               | Description |
| --------------- | ------------------- | ----------- |
| <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ProblemDetailsMembers.cs#L91" target="_blank">TemplateId</a> | `"templateId"` | The [TemplateId](TemplatedMessage/TemplatedMessage_TemplateId.md). |
| <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ProblemDetailsMembers.cs#L96" target="_blank">TemplateParams</a> | `"params"` | The [Params](TemplatedMessage/TemplatedMessage_Params.md), omitted when there are none. |

## See Also
* [Problem Details Format](../problem-details.md)
* [Error.Metadata](Error/Error_Metadata.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
