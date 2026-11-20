# ThenAsyncExtensions.ThenAsync Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a asynchronous function if the instance of [Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), and returns the next result: the value, the [Result](../Result/Result.md) or the [Result&lt;T&gt;](../ResultT/ResultT.md) that the function returns, or a failed result for an [Error](../Error/Error.md) it returns.

## Overloads
| Name | Description |
| ---- | ----------- |
| [ThenAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncresult-functaskresult-nullableboolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync(Result, Func&lt;Task&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncresult-functaskerror-nullableboolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctresult-functaskt-nullableboolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctresult-functaskresultt-nullableboolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctresultt-funct-taskresult-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctresultt-funct-taskerror-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnextresultt-funct-tasktnext-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnextresultt-funct-taskresulttnext-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync(Task&lt;Result&gt;, Func&lt;Task&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctaskresult-functaskresult-nullableboolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync(Task&lt;Result&gt;, Func&lt;Task&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctaskresult-functaskerror-nullableboolean) | Executes the specified asynchronous function, if the asynchronous operation `resultTask` represents a successful [Result](../Result/Result.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncttaskresult-functaskt-nullableboolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncttaskresult-functaskresultt-nullableboolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncttaskresultt-funct-taskresult-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncttaskresultt-funct-taskerror-nullableboolean) | Executes the specified asynchronous function, if the asynchronous operation `resultTask` represents a successful [Result&lt;T&gt;](../ResultT/ResultT.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnexttaskresultt-funct-tasktnext-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnexttaskresultt-funct-taskresulttnext-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;T&gt;, Nullable&lt;Boolean&gt;)](#thenasyncttaskresult-funct-nullableboolean) | Runs the synchronous function if the result that the `resultTask` returns is successful, and returns a result with its value. |
| [ThenAsync(Task&lt;Result&gt;, Func&lt;Result&gt;, Nullable&lt;Boolean&gt;)](#thenasynctaskresult-funcresult-nullableboolean) | Runs the synchronous function if the result that the `resultTask` returns is successful, and returns the result that it returns. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Result&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncttaskresult-funcresultt-nullableboolean) | Runs the synchronous function if the result that the `resultTask` returns is successful, and returns the result that it returns. |
| [ThenAsync(Task&lt;Result&gt;, Func&lt;Error&gt;, Nullable&lt;Boolean&gt;)](#thenasynctaskresult-funcerror-nullableboolean) | Runs the synchronous function if the result that the `resultTask` returns is successful, and returns a failed result with the error that it returns. |
| [ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnexttaskresultt-funct-tnext-nullableboolean) | Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns a result with the value that it returns. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Nullable&lt;Boolean&gt;)](#thenasyncttaskresultt-funct-result-nullableboolean) | Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns the result that it returns. |
| [ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnexttaskresultt-funct-resulttnext-nullableboolean) | Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns the result that it returns. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Error&gt;, Nullable&lt;Boolean&gt;)](#thenasyncttaskresultt-funct-error-nullableboolean) | Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns a failed result with the error that it returns. |
| [ThenAsync(Task&lt;Result&gt;, Func&lt;ValueTask&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctaskresult-funcvaluetaskresult-nullableboolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync(Task&lt;Result&gt;, Func&lt;ValueTask&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctaskresult-funcvaluetaskerror-nullableboolean) | Executes the specified asynchronous function, if the asynchronous operation `resultTask` represents a successful [Result](../Result/Result.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;ValueTask&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncttaskresult-funcvaluetaskt-nullableboolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncttaskresult-funcvaluetaskresultt-nullableboolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncttaskresultt-funct-valuetaskresult-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncttaskresultt-funct-valuetaskerror-nullableboolean) | Executes the specified asynchronous function, if the asynchronous operation `resultTask` represents a successful [Result&lt;T&gt;](../ResultT/ResultT.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnexttaskresultt-funct-valuetasktnext-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnexttaskresultt-funct-valuetaskresulttnext-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync(Result, Func&lt;ValueTask&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncresult-funcvaluetaskresult-nullableboolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync(Result, Func&lt;ValueTask&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncresult-funcvaluetaskerror-nullableboolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctresult-funcvaluetaskt-nullableboolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctresult-funcvaluetaskresultt-nullableboolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctresultt-funct-valuetaskresult-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctresultt-funct-valuetaskerror-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnextresultt-funct-valuetasktnext-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnextresultt-funct-valuetaskresulttnext-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncvaluetaskresult-funcvaluetaskresult-nullableboolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncvaluetaskresult-funcvaluetaskerror-nullableboolean) | Executes the specified asynchronous function, if the asynchronous operation `resultTask` represents a successful [Result](../Result/Result.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctvaluetaskresult-funcvaluetaskt-nullableboolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctvaluetaskresult-funcvaluetaskresultt-nullableboolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctvaluetaskresultt-funct-valuetaskresult-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctvaluetaskresultt-funct-valuetaskerror-nullableboolean) | Executes the specified asynchronous function, if the asynchronous operation `resultTask` represents a successful [Result&lt;T&gt;](../ResultT/ResultT.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnextvaluetaskresultt-funct-valuetasktnext-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnextvaluetaskresultt-funct-valuetaskresulttnext-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;T&gt;, Nullable&lt;Boolean&gt;)](#thenasynctvaluetaskresult-funct-nullableboolean) | Runs the synchronous function if the result that the `resultTask` returns is successful, and returns a result with its value. |
| [ThenAsync(ValueTask&lt;Result&gt;, Func&lt;Result&gt;, Nullable&lt;Boolean&gt;)](#thenasyncvaluetaskresult-funcresult-nullableboolean) | Runs the synchronous function if the result that the `resultTask` returns is successful, and returns the result that it returns. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;Result&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctvaluetaskresult-funcresultt-nullableboolean) | Runs the synchronous function if the result that the `resultTask` returns is successful, and returns the result that it returns. |
| [ThenAsync(ValueTask&lt;Result&gt;, Func&lt;Error&gt;, Nullable&lt;Boolean&gt;)](#thenasyncvaluetaskresult-funcerror-nullableboolean) | Runs the synchronous function if the result that the `resultTask` returns is successful, and returns a failed result with the error that it returns. |
| [ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnextvaluetaskresultt-funct-tnext-nullableboolean) | Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns a result with the value that it returns. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Nullable&lt;Boolean&gt;)](#thenasynctvaluetaskresultt-funct-result-nullableboolean) | Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns the result that it returns. |
| [ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnextvaluetaskresultt-funct-resulttnext-nullableboolean) | Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns the result that it returns. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Error&gt;, Nullable&lt;Boolean&gt;)](#thenasynctvaluetaskresultt-funct-error-nullableboolean) | Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns a failed result with the error that it returns. |
| [ThenAsync(ValueTask&lt;Result&gt;, Func&lt;Task&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncvaluetaskresult-functaskresult-nullableboolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync(ValueTask&lt;Result&gt;, Func&lt;Task&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasyncvaluetaskresult-functaskerror-nullableboolean) | Executes the specified asynchronous function, if the asynchronous operation `resultTask` represents a successful [Result](../Result/Result.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;Task&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctvaluetaskresult-functaskt-nullableboolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctvaluetaskresult-functaskresultt-nullableboolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctvaluetaskresultt-funct-taskresult-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynctvaluetaskresultt-funct-taskerror-nullableboolean) | Executes the specified asynchronous function, if the asynchronous operation `resultTask` represents a successful [Result&lt;T&gt;](../ResultT/ResultT.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnextvaluetaskresultt-funct-tasktnext-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)](#thenasynct-tnextvaluetaskresultt-funct-taskresulttnext-nullableboolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |

## Remarks
On a `Result` or a `Result<T>`, the function decides the kind of task: a function that returns a `ValueTask` gives a `ValueTask`, and an `async` lambda gives a `Task` (with an explicit return type, e.g., `async ValueTask<int> (int x) => …`, it gives a `ValueTask`). On a `Task` or a `ValueTask` of a result, the kind of the receiver is kept.

The overloads with a synchronous function (e.g., `ThenAsync(user => user.Name)`) run it after the task completes. A synchronous action does not compile: the compiler reports that it returns no value (e.g., CS0029); use `IfSuccessAsync` for a side effect. On a task of a result, a function that returns the other kind of task (a `ValueTask` on a `Task`, a `Task` on a `ValueTask`) is awaited, and the kind of the receiver is kept.

A returned `Result` or `Result<T>` is flattened, and a returned [Error](../Error/Error.md) makes the result fail. Explicit type arguments keep a nested result on purpose. A function that returns a `Task` or a `ValueTask` without a value has no result to continue with; use [IfSuccessAsync](IfSuccessAsyncExtensions_IfSuccessAsync.md), which runs a side effect and returns the result unchanged. On a result, such a function does not compile (CS0029 and CS1662, and CS0266 for a `Task`). On a task of a result, it compiles, and the task becomes the value of the result, which nobody awaits: the analyzer reports it as an error ([MR0001](../../Analyzers/MR0001.md)).

> [!NOTE]
> When an asynchronous method awaits a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task) directly, continuation usually occurs in the same thread that created the task, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, `ConfigureAwait(false)` is used by default, so in the methods that take a `Task` of a result the passed delegates run on a thread-pool thread. When the delegates need the original context (e.g., to update UI elements), set the `continueOnCapturedContext` parameter to `true`, or set [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) to `true` once at startup.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.

> [!NOTE]
> A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask) can only be awaited once. Because of this, the overloads accepting `this ValueTask<TResult> resultTask` consume the passed operation as part of evaluating its result, and they do not perform a `null` check on it (a `ValueTask<TResult>` is a non-nullable value type).

## ThenAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L67" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result> ThenAsync(this Result result, Func<Task<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.ThenAsync(async () =>
{
    return await _companyService.AddUserAsync(userData);
});
```


## ThenAsync(Result, Func&lt;Task&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L121" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function only if the [Result](../Result/Result.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result> ThenAsync(this Result result, Func<Task<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `result` represents a successful operation. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<Error>(…)`.

### Examples
```csharp
var result = await _userService.DeleteUser(userId)
    .ThenAsync(async () => await _policyService.GetDeletionErrorAsync(userId));
```


## ThenAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L172" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result<T>> ThenAsync<T>(this Result result, Func<Task<T>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async () =>
{
    return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
});
```


## ThenAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L223" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result<T>> ThenAsync<T>(this Result result, Func<Task<Result<T>>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async () =>
{
    return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
});
```


## ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L279" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result> ThenAsync<T>(this Result<T> result, Func<T, Task<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.ThenAsync(async (user) =>
{
    return await _companyService.AddUserAsync(user);
});
```


## ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L336" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function only if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result> ThenAsync<T>(this Result<T> result,
    Func<T, Task<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the `result`, passed to the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `result` represents a successful operation. The value of the `result` is passed as a parameter. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<T, Error>(…)`.

### Examples
```csharp
var result = await _userService.GetUser(userId)
    .ThenAsync(async user => await _policyService.GetDeletionErrorAsync(user));
```


## ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L392" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result<TNext>> ThenAsync<T, TNext>(this Result<T> result,
    Func<T, Task<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TNext&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async (user) =>
{
    return await _companyService.GetCompanyByIdAsync(user.CompanyId);
});
```


## ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L451" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result<TNext>> ThenAsync<T, TNext>(this Result<T> result,
    Func<T, Task<Result<TNext>>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async (user) =>
{
    return await _companyService.GetCompanyByIdAsync(user.CompanyId);
});
```


## ThenAsync(Task&lt;Result&gt;, Func&lt;Task&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L512" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result> ThenAsync(this Task<Result> resultTask, Func<Task<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async () =>
    {
        return await _companyService.AddUserAsync(userData);
    });
```


## ThenAsync(Task&lt;Result&gt;, Func&lt;Task&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L571" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function only if the asynchronous operation `resultTask` represents a successful [Result](../Result/Result.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result> ThenAsync(this Task<Result> resultTask,
    Func<Task<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<Error>(…)`.

### Examples
```csharp
var result = await _userService.DeleteUserAsync(userId)
    .ThenAsync(async () => await _policyService.GetDeletionErrorAsync(userId));
```


## ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L634" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result<T>> ThenAsync<T>(this Task<Result> resultTask, Func<Task<T>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async () =>
    {
        return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
    });
```


## ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L695" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result<T>> ThenAsync<T>(
    this Task<Result> resultTask,
    Func<Task<Result<T>>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async () =>
    {
        return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
    });
```


## ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L763" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result> ThenAsync<T>(this Task<Result<T>> resultTask,
    Func<T, Task<Result>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async (user) =>
    {
        return await _companyService.AddUserAsync(user);
    });
```


## ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L827" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function only if the asynchronous operation `resultTask` represents a successful [Result&lt;T&gt;](../ResultT/ResultT.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result> ThenAsync<T>(this Task<Result<T>> resultTask,
    Func<T, Task<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result of `resultTask`, passed to the `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. The value of the result is passed as a parameter. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<T, Error>(…)`.

### Examples
```csharp
var result = await _userService.GetUserAsync(userId)
    .ThenAsync(async user => await _policyService.GetDeletionErrorAsync(user));
```


## ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L897" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result<TNext>> ThenAsync<T, TNext>(this Task<Result<T>> resultTask,
    Func<T, Task<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TNext&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async (user) =>
    {
        return await _companyService.GetCompanyByIdAsync(user.CompanyId);
    });
```


## ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L967" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
[OverloadResolutionPriority(1)]
public static Task<Result<TNext>> ThenAsync<T, TNext>(this Task<Result<T>> resultTask,
    Func<T, Task<Result<TNext>>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async (user) =>
    {
        return await _companyService.GetCompanyByIdAsync(user.CompanyId);
    });
```


## ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;T&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1018" target="_blank">ThenAsyncExtensions.Task.cs</a>

Runs the synchronous function if the result that the `resultTask` returns is successful, and returns a result with its value.

```csharp
public static Task<Result<T>> ThenAsync<T>(this Task<Result> resultTask, Func<T> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value that the function returns.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result to inspect. Must not be `null`.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;T&gt;
The synchronous function that returns the value of the next result. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The successful result with the value of the function, or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `resultTask` or the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var result = await _service.DeleteAsync(id)
    .ThenAsync(() => id);
```


## ThenAsync(Task&lt;Result&gt;, Func&lt;Result&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1056" target="_blank">ThenAsyncExtensions.Task.cs</a>

Runs the synchronous function if the result that the `resultTask` returns is successful, and returns the result that it returns.

```csharp
public static Task<Result> ThenAsync(this Task<Result> resultTask, Func<Result> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result to inspect. Must not be `null`.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../Result/Result.md)&gt;
The synchronous function that returns the next result. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result of the function, or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `resultTask` or the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var result = await _service.DeleteAsync(id)
    .ThenAsync(() => _audit.Record(id));
```


## ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Result&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1095" target="_blank">ThenAsyncExtensions.Task.cs</a>

Runs the synchronous function if the result that the `resultTask` returns is successful, and returns the result that it returns.

```csharp
public static Task<Result<T>> ThenAsync<T>(this Task<Result> resultTask, Func<Result<T>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the next result.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result to inspect. Must not be `null`.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The synchronous function that returns the next result. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The result of the function, or a failed result with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `resultTask` or the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var result = await _service.DeleteAsync(id)
    .ThenAsync(() => _cache.Get(id));
```


## ThenAsync(Task&lt;Result&gt;, Func&lt;Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1134" target="_blank">ThenAsyncExtensions.Task.cs</a>

Runs the synchronous function if the result that the `resultTask` returns is successful, and returns a failed result with the error that it returns.

```csharp
public static Task<Result> ThenAsync(this Task<Result> resultTask, Func<Error> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result to inspect. Must not be `null`.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Error](../Error/Error.md)&gt;
The synchronous function that returns the error. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the function, or the original failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `resultTask` or the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var result = await _service.DeleteAsync(id)
    .ThenAsync(() => Error.Conflict(typeUri, "The record is locked."));
```


## ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1176" target="_blank">ThenAsyncExtensions.Task.cs</a>

Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns a result with the value that it returns.

```csharp
public static Task<Result<TNext>> ThenAsync<T, TNext>(this Task<Result<T>> resultTask, Func<T, TNext> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

#### `TNext`
The type of the value that the function returns.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the result to inspect. Must not be `null`.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, TNext&gt;
The synchronous function that receives the value and returns the next value. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The successful result with the value of the function, or a failed result with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `resultTask` or the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var nameResult = await _userService.GetUserAsync(id)
    .ThenAsync(user => user.Name);
```


## ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1218" target="_blank">ThenAsyncExtensions.Task.cs</a>

Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns the result that it returns.

```csharp
public static Task<Result> ThenAsync<T>(this Task<Result<T>> resultTask, Func<T, Result> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the result to inspect. Must not be `null`.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The synchronous function that receives the value and returns the next result. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result of the function, or a failed result with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `resultTask` or the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var result = await _userService.GetUserAsync(id)
    .ThenAsync(user => _validator.Validate(user));
```


## ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1261" target="_blank">ThenAsyncExtensions.Task.cs</a>

Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns the result that it returns.

```csharp
public static Task<Result<TNext>> ThenAsync<T, TNext>(this Task<Result<T>> resultTask, Func<T, Result<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

#### `TNext`
The type of the value of the next result.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the result to inspect. Must not be `null`.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The synchronous function that receives the value and returns the next result. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result of the function, or a failed result with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `resultTask` or the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var addressResult = await _userService.GetUserAsync(id)
    .ThenAsync(user => _addressBook.Find(user.Id));
```


## ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1301" target="_blank">ThenAsyncExtensions.Task.cs</a>

Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns a failed result with the error that it returns.

```csharp
public static Task<Result> ThenAsync<T>(this Task<Result<T>> resultTask, Func<T, Error> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the result to inspect. Must not be `null`.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Error](../Error/Error.md)&gt;
The synchronous function that receives the value and returns the error. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the function, or a failed result with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `resultTask` or the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var result = await _userService.GetUserAsync(id)
    .ThenAsync(user => Error.PermissionDenied(typeUri, $"{user.Name} cannot do this."));
```

## ThenAsync(Task&lt;Result&gt;, Func&lt;ValueTask&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1362" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
public static Task<Result> ThenAsync(this Task<Result> resultTask, Func<ValueTask<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
The function returns a `ValueTask`, the other kind of task: it is awaited, and a `Task` is returned, as for a function that returns a `Task`.

The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

## ThenAsync(Task&lt;Result&gt;, Func&lt;ValueTask&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1424" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function only if the asynchronous operation `resultTask` represents a successful [Result](../Result/Result.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
public static Task<Result> ThenAsync(this Task<Result> resultTask,
    Func<ValueTask<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
The function returns a `ValueTask`, the other kind of task: it is awaited, and a `Task` is returned, as for a function that returns a `Task`.

Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<Error>(…)`.

## ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;ValueTask&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1489" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
public static Task<Result<T>> ThenAsync<T>(this Task<Result> resultTask, Func<ValueTask<T>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The function returns a `ValueTask`, the other kind of task: it is awaited, and a `Task` is returned, as for a function that returns a `Task`.

The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

## ThenAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1553" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
public static Task<Result<T>> ThenAsync<T>(
    this Task<Result> resultTask,
    Func<ValueTask<Result<T>>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
The function returns a `ValueTask`, the other kind of task: it is awaited, and a `Task` is returned, as for a function that returns a `Task`.

The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

## ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1621" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
public static Task<Result> ThenAsync<T>(this Task<Result<T>> resultTask,
    Func<T, ValueTask<Result>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
The function returns a `ValueTask`, the other kind of task: it is awaited, and a `Task` is returned, as for a function that returns a `Task`.

The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

## ThenAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1688" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function only if the asynchronous operation `resultTask` represents a successful [Result&lt;T&gt;](../ResultT/ResultT.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
public static Task<Result> ThenAsync<T>(this Task<Result<T>> resultTask,
    Func<T, ValueTask<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result of `resultTask`, passed to the `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. The value of the result is passed as a parameter. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
The function returns a `ValueTask`, the other kind of task: it is awaited, and a `Task` is returned, as for a function that returns a `Task`.

Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<T, Error>(…)`.

## ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1761" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
public static Task<Result<TNext>> ThenAsync<T, TNext>(this Task<Result<T>> resultTask,
    Func<T, ValueTask<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TNext&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The function returns a `ValueTask`, the other kind of task: it is awaited, and a `Task` is returned, as for a function that returns a `Task`.

The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

## ThenAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.Task.cs#L1834" target="_blank">ThenAsyncExtensions.Task.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
public static Task<Result<TNext>> ThenAsync<T, TNext>(this Task<Result<T>> resultTask,
    Func<T, ValueTask<Result<TNext>>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
The function returns a `ValueTask`, the other kind of task: it is awaited, and a `Task` is returned, as for a function that returns a `Task`.

The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

## ThenAsync(Result, Func&lt;ValueTask&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L51" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static ValueTask<Result> ThenAsync(this Result result, Func<ValueTask<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a value task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.ThenAsync(async () =>
{
    return await _companyService.AddUserAsync(userData);
});
```


## ThenAsync(Result, Func&lt;ValueTask&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L104" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function only if the [Result](../Result/Result.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
public static ValueTask<Result> ThenAsync(this Result result, Func<ValueTask<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `result` represents a successful operation. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<Error>(…)`.

### Examples
```csharp
var result = await _userService.DeleteUser(userId)
    .ThenAsync(async () => await _policyService.GetDeletionErrorAsync(userId));
```


## ThenAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L136" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static ValueTask<Result<T>> ThenAsync<T>(this Result result, Func<ValueTask<T>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async () =>
{
    return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
});
```


## ThenAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L161" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static ValueTask<Result<T>> ThenAsync<T>(this Result result, Func<ValueTask<Result<T>>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a value task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async () =>
{
    return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
});
```


## ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L217" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static ValueTask<Result> ThenAsync<T>(this Result<T> result, Func<T, ValueTask<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a value task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.ThenAsync(async (user) =>
{
    return await _companyService.AddUserAsync(user);
});
```


## ThenAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L274" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function only if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
public static ValueTask<Result> ThenAsync<T>(this Result<T> result,
    Func<T, ValueTask<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the `result`, passed to the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `result` represents a successful operation. The value of the `result` is passed as a parameter. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<T, Error>(…)`.

### Examples
```csharp
var result = await _userService.GetUser(userId)
    .ThenAsync(async user => await _policyService.GetDeletionErrorAsync(user));
```


## ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L302" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static ValueTask<Result<TNext>> ThenAsync<T, TNext>(this Result<T> result,
    Func<T, ValueTask<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TNext&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async (user) =>
{
    return await _companyService.GetCompanyByIdAsync(user.CompanyId);
});
```


## ThenAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L355" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static ValueTask<Result<TNext>> ThenAsync<T, TNext>(this Result<T> result,
    Func<T, ValueTask<Result<TNext>>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns a value task with a `null` result.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.ThenAsync(async (user) =>
{
    return await _companyService.GetCompanyByIdAsync(user.CompanyId);
});
```


## ThenAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L413" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
public static ValueTask<Result> ThenAsync(this ValueTask<Result> resultTask, Func<ValueTask<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async () =>
    {
        return await _companyService.AddUserAsync(userData);
    });
```


## ThenAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L469" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function only if the asynchronous operation `resultTask` represents a successful [Result](../Result/Result.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
public static ValueTask<Result> ThenAsync(this ValueTask<Result> resultTask,
    Func<ValueTask<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<Error>(…)`.

### Examples
```csharp
var result = await _userService.DeleteUserAsync(userId)
    .ThenAsync(async () => await _policyService.GetDeletionErrorAsync(userId));
```


## ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L528" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
public static ValueTask<Result<T>> ThenAsync<T>(this ValueTask<Result> resultTask, Func<ValueTask<T>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async () =>
    {
        return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
    });
```


## ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L584" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
public static ValueTask<Result<T>> ThenAsync<T>(
    this ValueTask<Result> resultTask,
    Func<ValueTask<Result<T>>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async () =>
    {
        return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
    });
```


## ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L649" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
public static ValueTask<Result> ThenAsync<T>(this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<Result>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async (user) =>
    {
        return await _companyService.AddUserAsync(user);
    });
```


## ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L709" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function only if the asynchronous operation `resultTask` represents a successful [Result&lt;T&gt;](../ResultT/ResultT.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
public static ValueTask<Result> ThenAsync<T>(this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result of `resultTask`, passed to the `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. The value of the result is passed as a parameter. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<T, Error>(…)`.

### Examples
```csharp
var result = await _userService.GetUserAsync(userId)
    .ThenAsync(async user => await _policyService.GetDeletionErrorAsync(user));
```


## ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L775" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
public static ValueTask<Result<TNext>> ThenAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TNext&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async (user) =>
    {
        return await _companyService.GetCompanyByIdAsync(user.CompanyId);
    });
```


## ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L836" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
public static ValueTask<Result<TNext>> ThenAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<Result<TNext>>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a value task with a `null` result.

### Remarks
The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .ThenAsync(async (user) =>
    {
        return await _companyService.GetCompanyByIdAsync(user.CompanyId);
    });
```


## ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;T&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L887" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Runs the synchronous function if the result that the `resultTask` returns is successful, and returns a result with its value.

```csharp
public static ValueTask<Result<T>> ThenAsync<T>(this ValueTask<Result> resultTask, Func<T> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value that the function returns.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result to inspect.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;T&gt;
The synchronous function that returns the value of the next result. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The successful result with the value of the function, or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var result = await _service.DeleteAsync(id)
    .ThenAsync(() => id);
```


## ThenAsync(ValueTask&lt;Result&gt;, Func&lt;Result&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L922" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Runs the synchronous function if the result that the `resultTask` returns is successful, and returns the result that it returns.

```csharp
public static ValueTask<Result> ThenAsync(this ValueTask<Result> resultTask, Func<Result> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result to inspect.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../Result/Result.md)&gt;
The synchronous function that returns the next result. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The result of the function, or the failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var result = await _service.DeleteAsync(id)
    .ThenAsync(() => _audit.Record(id));
```


## ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;Result&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L958" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Runs the synchronous function if the result that the `resultTask` returns is successful, and returns the result that it returns.

```csharp
public static ValueTask<Result<T>> ThenAsync<T>(this ValueTask<Result> resultTask, Func<Result<T>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the next result.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result to inspect.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The synchronous function that returns the next result. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The result of the function, or a failed result with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var result = await _service.DeleteAsync(id)
    .ThenAsync(() => _cache.Get(id));
```


## ThenAsync(ValueTask&lt;Result&gt;, Func&lt;Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L994" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Runs the synchronous function if the result that the `resultTask` returns is successful, and returns a failed result with the error that it returns.

```csharp
public static ValueTask<Result> ThenAsync(this ValueTask<Result> resultTask, Func<Error> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the result to inspect.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Error](../Error/Error.md)&gt;
The synchronous function that returns the error. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the function, or the original failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var result = await _service.DeleteAsync(id)
    .ThenAsync(() => Error.Conflict(typeUri, "The record is locked."));
```


## ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L1033" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns a result with the value that it returns.

```csharp
public static ValueTask<Result<TNext>> ThenAsync<T, TNext>(this ValueTask<Result<T>> resultTask, Func<T, TNext> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

#### `TNext`
The type of the value that the function returns.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the result to inspect.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, TNext&gt;
The synchronous function that receives the value and returns the next value. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The successful result with the value of the function, or a failed result with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var nameResult = await _userService.GetUserAsync(id)
    .ThenAsync(user => user.Name);
```


## ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L1072" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns the result that it returns.

```csharp
public static ValueTask<Result> ThenAsync<T>(this ValueTask<Result<T>> resultTask, Func<T, Result> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the result to inspect.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The synchronous function that receives the value and returns the next result. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The result of the function, or a failed result with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var result = await _userService.GetUserAsync(id)
    .ThenAsync(user => _validator.Validate(user));
```


## ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L1112" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns the result that it returns.

```csharp
public static ValueTask<Result<TNext>> ThenAsync<T, TNext>(this ValueTask<Result<T>> resultTask, Func<T, Result<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

#### `TNext`
The type of the value of the next result.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the result to inspect.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The synchronous function that receives the value and returns the next result. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result of the function, or a failed result with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var addressResult = await _userService.GetUserAsync(id)
    .ThenAsync(user => _addressBook.Find(user.Id));
```


## ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Error&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L1149" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Runs the synchronous function on the value if the result that the `resultTask` returns is successful, and returns a failed result with the error that it returns.

```csharp
public static ValueTask<Result> ThenAsync<T>(this ValueTask<Result<T>> resultTask, Func<T, Error> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the result to inspect.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Error](../Error/Error.md)&gt;
The synchronous function that receives the value and returns the error. Must not be `null`.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuation, including the passed delegate, on the captured context (e.g., the UI thread); `false` to run it on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the function, or a failed result with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `ifSuccessFunction` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` returns `null`.

### Remarks
The function is synchronous: it runs after the `resultTask` completes, and it is not called for a failed result, whose [Error](../Error/Error.md) is returned. Use the overloads with an asynchronous function for asynchronous work.

### Examples
```csharp
var result = await _userService.GetUserAsync(id)
    .ThenAsync(user => Error.PermissionDenied(typeUri, $"{user.Name} cannot do this."));
```

## ThenAsync(ValueTask&lt;Result&gt;, Func&lt;Task&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L1208" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
[OverloadResolutionPriority(1)]
public static ValueTask<Result> ThenAsync(this ValueTask<Result> resultTask, Func<Task<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The function returns a `Task`, the other kind of task: it is awaited, and a `ValueTask` is returned, as for a function that returns a `ValueTask`.

The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

## ThenAsync(ValueTask&lt;Result&gt;, Func&lt;Task&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L1268" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function only if the asynchronous operation `resultTask` represents a successful [Result](../Result/Result.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
[OverloadResolutionPriority(1)]
public static ValueTask<Result> ThenAsync(this ValueTask<Result> resultTask,
    Func<Task<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The function returns a `Task`, the other kind of task: it is awaited, and a `ValueTask` is returned, as for a function that returns a `ValueTask`.

Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<Error>(…)`.

## ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;Task&lt;T&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L1332" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
[OverloadResolutionPriority(1)]
public static ValueTask<Result<T>> ThenAsync<T>(this ValueTask<Result> resultTask, Func<Task<T>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The function returns a `Task`, the other kind of task: it is awaited, and a `ValueTask` is returned, as for a function that returns a `ValueTask`.

The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

## ThenAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L1394" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../Result/Result.md).

```csharp
[OverloadResolutionPriority(1)]
public static ValueTask<Result<T>> ThenAsync<T>(
    this ValueTask<Result> resultTask,
    Func<Task<Result<T>>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The function returns a `Task`, the other kind of task: it is awaited, and a `ValueTask` is returned, as for a function that returns a `ValueTask`.

The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

## ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L1460" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
[OverloadResolutionPriority(1)]
public static ValueTask<Result> ThenAsync<T>(this ValueTask<Result<T>> resultTask,
    Func<T, Task<Result>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The function returns a `Task`, the other kind of task: it is awaited, and a `ValueTask` is returned, as for a function that returns a `ValueTask`.

The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

## ThenAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Error&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L1525" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function only if the asynchronous operation `resultTask` represents a successful [Result&lt;T&gt;](../ResultT/ResultT.md), and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
[OverloadResolutionPriority(1)]
public static ValueTask<Result> ThenAsync<T>(this ValueTask<Result<T>> resultTask,
    Func<T, Task<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result of `resultTask`, passed to the `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Error](../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. The value of the result is passed as a parameter. It returns the [Error](../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The function returns a `Task`, the other kind of task: it is awaited, and a `ValueTask` is returned, as for a function that returns a `ValueTask`.

Without this overload, an asynchronous function returning an [Error](../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `ThenAsync<T, Error>(…)`.

## ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L1597" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
[OverloadResolutionPriority(1)]
public static ValueTask<Result<TNext>> ThenAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
    Func<T, Task<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TNext&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The function returns a `Task`, the other kind of task: it is awaited, and a `ValueTask` is returned, as for a function that returns a `ValueTask`.

The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

## ThenAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Nullable&lt;Boolean&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/ThenAsyncExtensions.ValueTask.cs#L1668" target="_blank">ThenAsyncExtensions.ValueTask.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../ResultT/ResultT.md).

```csharp
[OverloadResolutionPriority(1)]
public static ValueTask<Result<TNext>> ThenAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
    Func<T, Task<Result<TNext>>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result](../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`, or the `ifSuccessFunction` function returns a `null` task or a task with a `null` result.

### Remarks
The function returns a `Task`, the other kind of task: it is awaited, and a `ValueTask` is returned, as for a function that returns a `ValueTask`.

The `ThenAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

## See Also
* [IfSuccessAsync](IfSuccessAsyncExtensions_IfSuccessAsync.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
