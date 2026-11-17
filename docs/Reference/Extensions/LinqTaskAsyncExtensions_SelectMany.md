# LinqTaskAsyncExtensions.SelectMany Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Enables the LINQ query syntax with more than one `from` clause for [Result&lt;T&gt;](../ResultT/ResultT.md) and [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;, including queries that mix synchronous and asynchronous `from` clauses.

## Overloads
| Name | Description |
| ---- | ----------- |
| [SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnexttaskresultt-funct-taskresulttmiddle-funct-tmiddle-tnext) | Applies an asynchronous selector to the value of an asynchronous result. |
| [SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnexttaskresultt-funct-resulttmiddle-funct-tmiddle-tnext) | Applies a synchronous selector to the value of an asynchronous result. |
| [SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnextresultt-funct-taskresulttmiddle-funct-tmiddle-tnext) | Applies an asynchronous selector to the value of a synchronous result. |
| [SelectMany&lt;T, TMiddle&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)](#selectmanyt-tmiddletaskresultt-funct-taskresulttmiddle-funct-tmiddle-error) | Applies the asynchronous selector and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the result selector. |
| [SelectMany&lt;T, TMiddle&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)](#selectmanyt-tmiddletaskresultt-funct-resulttmiddle-funct-tmiddle-error) | Applies the synchronous selector and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the result selector. |
| [SelectMany&lt;T, TMiddle&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)](#selectmanyt-tmiddleresultt-funct-taskresulttmiddle-funct-tmiddle-error) | Applies the asynchronous selector and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the result selector. |
| [SelectMany&lt;T, TMiddle&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&gt;)](#selectmanyt-tmiddletaskresultt-funct-taskresulttmiddle-funct-tmiddle-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&lt;TNext&gt;&gt;)](#selectmanyt-tmiddle-tnexttaskresultt-funct-taskresulttmiddle-funct-tmiddle-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, TNext&gt;)](#selectmanyt-tnexttaskresultt-funct-taskresult-funct-result-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, Error&gt;)](#selectmanyttaskresultt-funct-taskresult-funct-result-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, Result&gt;)](#selectmanyttaskresultt-funct-taskresult-funct-result-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, Result&lt;TNext&gt;&gt;)](#selectmanyt-tnexttaskresultt-funct-taskresult-funct-result-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle, TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, TNext&gt;)](#selectmanytmiddle-tnexttaskresult-funcresult-taskresulttmiddle-funcresult-tmiddle-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle&gt;(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Error&gt;)](#selectmanytmiddletaskresult-funcresult-taskresulttmiddle-funcresult-tmiddle-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle&gt;(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&gt;)](#selectmanytmiddletaskresult-funcresult-taskresulttmiddle-funcresult-tmiddle-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle, TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&lt;TNext&gt;&gt;)](#selectmanytmiddle-tnexttaskresult-funcresult-taskresulttmiddle-funcresult-tmiddle-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, TNext&gt;)](#selectmanytnexttaskresult-funcresult-taskresult-funcresult-result-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, Error&gt;)](#selectmanytaskresult-funcresult-taskresult-funcresult-result-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&gt;)](#selectmanytaskresult-funcresult-taskresult-funcresult-result-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&lt;TNext&gt;&gt;)](#selectmanytnexttaskresult-funcresult-taskresult-funcresult-result-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TMiddle&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Result&gt;)](#selectmanyt-tmiddletaskresultt-funct-resulttmiddle-funct-tmiddle-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Result&lt;TNext&gt;&gt;)](#selectmanyt-tmiddle-tnexttaskresultt-funct-resulttmiddle-funct-tmiddle-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, TNext&gt;)](#selectmanyt-tnexttaskresultt-funct-result-funct-result-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, Error&gt;)](#selectmanyttaskresultt-funct-result-funct-result-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, Result&gt;)](#selectmanyttaskresultt-funct-result-funct-result-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, Result&lt;TNext&gt;&gt;)](#selectmanyt-tnexttaskresultt-funct-result-funct-result-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle, TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, TNext&gt;)](#selectmanytmiddle-tnexttaskresult-funcresult-resulttmiddle-funcresult-tmiddle-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle&gt;(Task&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, Error&gt;)](#selectmanytmiddletaskresult-funcresult-resulttmiddle-funcresult-tmiddle-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle&gt;(Task&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, Result&gt;)](#selectmanytmiddletaskresult-funcresult-resulttmiddle-funcresult-tmiddle-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle, TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, Result&lt;TNext&gt;&gt;)](#selectmanytmiddle-tnexttaskresult-funcresult-resulttmiddle-funcresult-tmiddle-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, TNext&gt;)](#selectmanytnexttaskresult-funcresult-result-funcresult-result-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany(Task&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, Error&gt;)](#selectmanytaskresult-funcresult-result-funcresult-result-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany(Task&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, Result&gt;)](#selectmanytaskresult-funcresult-result-funcresult-result-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, Result&lt;TNext&gt;&gt;)](#selectmanytnexttaskresult-funcresult-result-funcresult-result-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TMiddle&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&gt;)](#selectmanyt-tmiddleresultt-funct-taskresulttmiddle-funct-tmiddle-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&lt;TNext&gt;&gt;)](#selectmanyt-tmiddle-tnextresultt-funct-taskresulttmiddle-funct-tmiddle-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, TNext&gt;)](#selectmanyt-tnextresultt-funct-taskresult-funct-result-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, Error&gt;)](#selectmanytresultt-funct-taskresult-funct-result-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, Result&gt;)](#selectmanytresultt-funct-taskresult-funct-result-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, Result&lt;TNext&gt;&gt;)](#selectmanyt-tnextresultt-funct-taskresult-funct-result-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle, TNext&gt;(Result, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, TNext&gt;)](#selectmanytmiddle-tnextresult-funcresult-taskresulttmiddle-funcresult-tmiddle-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle&gt;(Result, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Error&gt;)](#selectmanytmiddleresult-funcresult-taskresulttmiddle-funcresult-tmiddle-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle&gt;(Result, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&gt;)](#selectmanytmiddleresult-funcresult-taskresulttmiddle-funcresult-tmiddle-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle, TNext&gt;(Result, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&lt;TNext&gt;&gt;)](#selectmanytmiddle-tnextresult-funcresult-taskresulttmiddle-funcresult-tmiddle-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TNext&gt;(Result, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, TNext&gt;)](#selectmanytnextresult-funcresult-taskresult-funcresult-result-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany(Result, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, Error&gt;)](#selectmanyresult-funcresult-taskresult-funcresult-result-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany(Result, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&gt;)](#selectmanyresult-funcresult-taskresult-funcresult-result-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TNext&gt;(Result, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&lt;TNext&gt;&gt;)](#selectmanytnextresult-funcresult-taskresult-funcresult-result-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |

## Remarks
A non-generic [Result](../Result/Result.md) can be any `from` clause, e.g., a check in the middle of a query (`from _ in EnsureActive(user)`) or the first clause (`from _ in Validate(request)`): its range variable is the successful `Result` itself. A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`). A result selector that returns a `Result` or a `Result<T>` is not nested, as `Then` does.

A result selector that returns an [Error](../Error/Error.md) makes the query fail: such a query returns a failed [Result](../Result/Result.md) instead of a **successful** `Result<Error>`.

A result selector that returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>` is not supported: the query does not await it, so such a query is a compile error. Await the operation with [ThenAsync](ThenTaskAsyncExtensions_ThenAsync.md) instead, or chain it in a `from` clause whose function returns a `Task<Result<T>>`. To keep the task in the result on purpose, call `SelectMany<T, TMiddle, Task>(…)` with explicit type arguments.

The query syntax needs a value in every `from` clause, so it works with [Result&lt;T&gt;](../ResultT/ResultT.md) but not with the non-generic [Result](../Result/Result.md). A `Task` and a `ValueTask` cannot be mixed in one query.


## SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L167" target="_blank">LinqTaskAsyncExtensions.cs</a>

Returns a new instance of [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; instance by applying the provided selector functions if the current [Result&lt;T&gt;](../ResultT/ResultT.md) if it is successful; or a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; instance with the same error, otherwise.


```csharp
public static Task<Result<TNext>> SelectMany<T, TMiddle, TNext>(
    this Task<Result<T>> result,
    Func<T, Task<Result<TMiddle>>> collectionSelector,
    Func<T, TMiddle, TNext> resultSelector);
```

### Type Parameters
#### `T`
The type of the [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; `result` value used to determine whether to execute the passed selector functions. If successful, this is also the type of the parameter passed to that function.

#### `TMiddle`
The type of the [Result](../ResultT/ResultT.md)&lt;TMiddle&gt; `result` output type of the passed collection selector and the type of one of the parameters passed to the result selector.

#### `TNext`
The type of the returned [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt; `result` value.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The task object representing the asynchronous operation that returns an instance of a [Result](../ResultT/ResultT.md) record to be evaluated.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that takes the value of the initial result and returns a task object representing the asynchronous operation that returns a new result representing an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, TNext&gt;
The function that combines the value from the initial result and the intermediate value to produce the final value.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If any of the `result` or `collectionSelector` or `resultSelector` parameters are `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`, or the `collectionSelector` function returns a `null` task or a task with a `null` result.

### Remarks
This method is used to enable LINQ query syntax for [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; records. It allows chaining multiple asynchronous operations that return [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; while maintaining the error state, if any of the operations fail.

### Examples
```csharp
var emailResult =
    await from addUser in _validationService.ValidateUserDataAsync(addUserRequest)
    from user in _userService.CreateUserAsync(addUser)
    from userGroup in _groupService.AddUserToGroupAsync(user)
    from sendWelcomeEmailResult in _emailService.SendWelcomeEmailAsync(user)
    select sendWelcomeEmailResult;
```


## SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L226" target="_blank">LinqTaskAsyncExtensions.cs</a>

Applies the synchronous `collectionSelector` and the `resultSelector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result`; otherwise, returns a failed result with the original [Error](../Error/Error.md).

```csharp
public static Task<Result<TNext>> SelectMany<T, TMiddle, TNext>(
    this Task<Result<T>> result,
    Func<T, Result<TMiddle>> collectionSelector,
    Func<T, TMiddle, TNext> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.

#### `TMiddle`
The type of the value of the intermediate result returned by `collectionSelector`.

#### `TNext`
The type of the value of the final result, produced by `resultSelector`.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The task object representing the asynchronous operation that returns the initial result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The synchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, TNext&gt;
The function that combines the value of the initial result and the intermediate value to produce the final value.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A task object representing the asynchronous operation: the result of applying the selectors if both the initial and the intermediate results are successful; otherwise, a failed result with the first [Error](../Error/Error.md).

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If any of the `result`, `collectionSelector` or `resultSelector` parameters are `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`, or the `collectionSelector` function returns `null`.

### Remarks
This overload enables a query in which an asynchronous `from` clause is followed by a synchronous one.

### Examples
```csharp
var addressResult =
    await from user in _userService.GetUserAsync(userId)    // asynchronous
    from address in _addressService.GetAddress(user)        // synchronous
    select address;
```


## SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L275" target="_blank">LinqTaskAsyncExtensions.cs</a>

Applies the asynchronous `collectionSelector` and the `resultSelector` to the value of a successful `result`; otherwise, returns a failed result with the original [Error](../Error/Error.md).

```csharp
public static Task<Result<TNext>> SelectMany<T, TMiddle, TNext>(
    this Result<T> result,
    Func<T, Task<Result<TMiddle>>> collectionSelector,
    Func<T, TMiddle, TNext> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.

#### `TMiddle`
The type of the value of the intermediate result returned by `collectionSelector`.

#### `TNext`
The type of the value of the final result, produced by `resultSelector`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The initial result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The asynchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, TNext&gt;
The function that combines the value of the initial result and the intermediate value to produce the final value.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A task object representing the asynchronous operation: the result of applying the selectors if both the initial and the intermediate results are successful; otherwise, a failed result with the first [Error](../Error/Error.md).

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If any of the `result`, `collectionSelector` or `resultSelector` parameters are `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `collectionSelector` function returns a `null` task or a task with a `null` result.

### Remarks
This overload enables a query in which a synchronous `from` clause is followed by an asynchronous one.

### Examples
```csharp
var ordersResult =
    await from user in _userService.GetUser(userId)         // synchronous
    from orders in _orderService.GetOrdersAsync(user)       // asynchronous
    select orders;
```


## SelectMany&lt;T, TMiddle&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L338" target="_blank">LinqTaskAsyncExtensions.cs</a>

Applies the asynchronous `collectionSelector` and the `resultSelector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector`; otherwise, returns a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md).

```csharp
public static Task<Result> SelectMany<T, TMiddle>(
    this Task<Result<T>> result,
    Func<T, Task<Result<TMiddle>>> collectionSelector,
    Func<T, TMiddle, Error> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.

#### `TMiddle`
The type of the value of the intermediate result returned by `collectionSelector`.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The task object representing the asynchronous operation that returns the initial result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The asynchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and the intermediate value, and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector` if both the initial and the intermediate results are successful; otherwise, a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result`, `collectionSelector` or `resultSelector` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`, the `collectionSelector` function returns a `null` task or a task with a `null` result, or the `resultSelector` function returns `null`.

### Remarks
This overload makes a query with more than one `from` clause that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnexttaskresultt-funct-taskresulttmiddle-funct-tmiddle-tnext) and return a **successful** `Result<Error>`.

### Examples
```csharp
var result =
    await from user in _userService.GetUserAsync(userId)
    from order in _orderService.GetOrderAsync(user, orderId)
    select Error.Conflict(typeUri, $"The order {order.Number} is already shipped.");
```


## SelectMany&lt;T, TMiddle&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L406" target="_blank">LinqTaskAsyncExtensions.cs</a>

Applies the synchronous `collectionSelector` and the `resultSelector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector`; otherwise, returns a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md).

```csharp
public static Task<Result> SelectMany<T, TMiddle>(
    this Task<Result<T>> result,
    Func<T, Result<TMiddle>> collectionSelector,
    Func<T, TMiddle, Error> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.

#### `TMiddle`
The type of the value of the intermediate result returned by `collectionSelector`.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The task object representing the asynchronous operation that returns the initial result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The synchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and the intermediate value, and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector` if both the initial and the intermediate results are successful; otherwise, a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result`, `collectionSelector` or `resultSelector` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`, the `collectionSelector` function returns `null`, or the `resultSelector` function returns `null`.

### Remarks
This overload makes a query with more than one `from` clause that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnexttaskresultt-funct-resulttmiddle-funct-tmiddle-tnext) and return a **successful** `Result<Error>`.

### Examples
```csharp
var result =
    await from user in _userService.GetUserAsync(userId)
    from order in _orderService.GetOrder(user, orderId)
    select Error.Conflict(typeUri, $"The order {order.Number} is already shipped.");
```


## SelectMany&lt;T, TMiddle&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L462" target="_blank">LinqTaskAsyncExtensions.cs</a>

Applies the asynchronous `collectionSelector` and the `resultSelector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector`; otherwise, returns a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md).

```csharp
public static Task<Result> SelectMany<T, TMiddle>(
    this Result<T> result,
    Func<T, Task<Result<TMiddle>>> collectionSelector,
    Func<T, TMiddle, Error> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.

#### `TMiddle`
The type of the value of the intermediate result returned by `collectionSelector`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The initial result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The asynchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and the intermediate value, and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector` if both the initial and the intermediate results are successful; otherwise, a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result`, `collectionSelector` or `resultSelector` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `collectionSelector` function returns a `null` task or a task with a `null` result, or the `resultSelector` function returns `null`.

### Remarks
This overload makes a query with more than one `from` clause that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnextresultt-funct-taskresulttmiddle-funct-tmiddle-tnext) and return a **successful** `Result<Error>`.

### Examples
```csharp
var result =
    await from user in _userService.GetUser(userId)
    from order in _orderService.GetOrderAsync(user, orderId)
    select Error.Conflict(typeUri, $"The order {order.Number} is already shipped.");
```


## SelectMany&lt;T, TMiddle&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L987" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<T, TMiddle>(this Task<Result<T>> result, Func<T, Task<Result<TMiddle>>> collectionSelector, Func<T, TMiddle, Result> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Result](../Result/Result.md)&gt;
The function that receives the value and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    from order in GetLastOrderAsync(user.Id)
    select Audit(user));
```


## SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1052" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<T, TMiddle, TNext>(this Task<Result<T>> result, Func<T, Task<Result<TMiddle>>> collectionSelector, Func<T, TMiddle, Result<TNext>> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    from order in GetLastOrderAsync(user.Id)
    select CreateOrder(user));
```


## SelectMany&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1116" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<T, TNext>(this Task<Result<T>> result, Func<T, Task<Result>> collectionSelector, Func<T, Result, TNext> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), TNext&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null` or a task with a `null` result.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    from _ in EnsureActiveAsync(user)
    select user.Name);
```


## SelectMany&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1173" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<T>(this Task<Result<T>> result, Func<T, Task<Result>> collectionSelector, Func<T, Result, Error> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    from _ in EnsureActiveAsync(user)
    select Error.Conflict(typeUri, "Not allowed."));
```


## SelectMany&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1238" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<T>(this Task<Result<T>> result, Func<T, Task<Result>> collectionSelector, Func<T, Result, Result> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    from _ in EnsureActiveAsync(user)
    select Audit(user));
```


## SelectMany&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1304" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<T, TNext>(this Task<Result<T>> result, Func<T, Task<Result>> collectionSelector, Func<T, Result, Result<TNext>> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    from _ in EnsureActiveAsync(user)
    select CreateOrder(user));
```


## SelectMany&lt;TMiddle, TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1369" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<TMiddle, TNext>(this Task<Result> result, Func<Result, Task<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, TNext> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, TNext&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null` or a task with a `null` result.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from user in GetUserAsync(request.UserId)
    select user.Name);
```


## SelectMany&lt;TMiddle&gt;(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1430" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<TMiddle>(this Task<Result> result, Func<Result, Task<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, Error> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Error](../Error/Error.md)&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from user in GetUserAsync(request.UserId)
    select Error.Conflict(typeUri, "Not allowed."));
```


## SelectMany&lt;TMiddle&gt;(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1499" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<TMiddle>(this Task<Result> result, Func<Result, Task<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, Result> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Result](../Result/Result.md)&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from user in GetUserAsync(request.UserId)
    select Audit(user));
```


## SelectMany&lt;TMiddle, TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1569" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<TMiddle, TNext>(this Task<Result> result, Func<Result, Task<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, Result<TNext>> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from user in GetUserAsync(request.UserId)
    select CreateOrder(user));
```


## SelectMany&lt;TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1633" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<TNext>(this Task<Result> result, Func<Result, Task<Result>> collectionSelector, Func<Result, Result, TNext> resultSelector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), TNext&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null` or a task with a `null` result.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from unique in EnsureUniqueAsync(request)
    select request.Id);
```


## SelectMany(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1693" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany(this Task<Result> result, Func<Result, Task<Result>> collectionSelector, Func<Result, Result, Error> resultSelector);
```

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from unique in EnsureUniqueAsync(request)
    select Error.Conflict(typeUri, "Not allowed."));
```


## SelectMany(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1758" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany(this Task<Result> result, Func<Result, Task<Result>> collectionSelector, Func<Result, Result, Result> resultSelector);
```

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from unique in EnsureUniqueAsync(request)
    select Audit(request));
```


## SelectMany&lt;TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1827" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<TNext>(this Task<Result> result, Func<Result, Task<Result>> collectionSelector, Func<Result, Result, Result<TNext>> resultSelector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from unique in EnsureUniqueAsync(request)
    select CreateOrder(request));
```


## SelectMany&lt;T, TMiddle&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1890" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<T, TMiddle>(this Task<Result<T>> result, Func<T, Result<TMiddle>> collectionSelector, Func<T, TMiddle, Result> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The function that receives the value and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Result](../Result/Result.md)&gt;
The function that receives the value and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null`, or the `resultSelector` returns `null`.

### Remarks
A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    from order in GetLastOrder(user.Id)
    select Audit(user));
```


## SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1939" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<T, TMiddle, TNext>(this Task<Result<T>> result, Func<T, Result<TMiddle>> collectionSelector, Func<T, TMiddle, Result<TNext>> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The function that receives the value and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null`, or the `resultSelector` returns `null`.

### Remarks
A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    from order in GetLastOrder(user.Id)
    select CreateOrder(user));
```


## SelectMany&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L1990" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<T, TNext>(this Task<Result<T>> result, Func<T, Result> collectionSelector, Func<T, Result, TNext> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The function that receives the value and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), TNext&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    from _ in EnsureActive(user)
    select user.Name);
```


## SelectMany&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2038" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<T>(this Task<Result<T>> result, Func<T, Result> collectionSelector, Func<T, Result, Error> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The function that receives the value and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null`, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    from _ in EnsureActive(user)
    select Error.Conflict(typeUri, "Not allowed."));
```


## SelectMany&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2087" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<T>(this Task<Result<T>> result, Func<T, Result> collectionSelector, Func<T, Result, Result> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The function that receives the value and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null`, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    from _ in EnsureActive(user)
    select Audit(user));
```


## SelectMany&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2137" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<T, TNext>(this Task<Result<T>> result, Func<T, Result> collectionSelector, Func<T, Result, Result<TNext>> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The function that receives the value and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null`, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUserAsync(id)
    from _ in EnsureActive(user)
    select CreateOrder(user));
```


## SelectMany&lt;TMiddle, TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2189" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<TMiddle, TNext>(this Task<Result> result, Func<Result, Result<TMiddle>> collectionSelector, Func<Result, TMiddle, TNext> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, TNext&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from user in GetUser(request.UserId)
    select user.Name);
```


## SelectMany&lt;TMiddle&gt;(Task&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2241" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<TMiddle>(this Task<Result> result, Func<Result, Result<TMiddle>> collectionSelector, Func<Result, TMiddle, Error> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Error](../Error/Error.md)&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null`, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from user in GetUser(request.UserId)
    select Error.Conflict(typeUri, "Not allowed."));
```


## SelectMany&lt;TMiddle&gt;(Task&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2297" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<TMiddle>(this Task<Result> result, Func<Result, Result<TMiddle>> collectionSelector, Func<Result, TMiddle, Result> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Result](../Result/Result.md)&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null`, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from user in GetUser(request.UserId)
    select Audit(user));
```


## SelectMany&lt;TMiddle, TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2354" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<TMiddle, TNext>(this Task<Result> result, Func<Result, Result<TMiddle>> collectionSelector, Func<Result, TMiddle, Result<TNext>> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null`, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from user in GetUser(request.UserId)
    select CreateOrder(user));
```


## SelectMany&lt;TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2405" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<TNext>(this Task<Result> result, Func<Result, Result> collectionSelector, Func<Result, Result, TNext> resultSelector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), TNext&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from unique in EnsureUnique(request)
    select request.Id);
```


## SelectMany(Task&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2456" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany(this Task<Result> result, Func<Result, Result> collectionSelector, Func<Result, Result, Error> resultSelector);
```

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null`, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from unique in EnsureUnique(request)
    select Error.Conflict(typeUri, "Not allowed."));
```


## SelectMany(Task&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2508" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany(this Task<Result> result, Func<Result, Result> collectionSelector, Func<Result, Result, Result> resultSelector);
```

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null`, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from unique in EnsureUnique(request)
    select Audit(request));
```


## SelectMany&lt;TNext&gt;(Task&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2561" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<TNext>(this Task<Result> result, Func<Result, Result> collectionSelector, Func<Result, Result, Result<TNext>> resultSelector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`, or the `collectionSelector` returns `null`, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in ValidateAsync(request)
    from unique in EnsureUnique(request)
    select CreateOrder(request));
```


## SelectMany&lt;T, TMiddle&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2612" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<T, TMiddle>(this Result<T> result, Func<T, Task<Result<TMiddle>>> collectionSelector, Func<T, TMiddle, Result> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Result](../Result/Result.md)&gt;
The function that receives the value and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUser(id)
    from order in GetLastOrderAsync(user.Id)
    select Audit(user));
```


## SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2671" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<T, TMiddle, TNext>(this Result<T> result, Func<T, Task<Result<TMiddle>>> collectionSelector, Func<T, TMiddle, Result<TNext>> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUser(id)
    from order in GetLastOrderAsync(user.Id)
    select CreateOrder(user));
```


## SelectMany&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2732" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<T, TNext>(this Result<T> result, Func<T, Task<Result>> collectionSelector, Func<T, Result, TNext> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), TNext&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `collectionSelector` returns `null` or a task with a `null` result.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from user in GetUser(id)
    from _ in EnsureActiveAsync(user)
    select user.Name);
```


## SelectMany&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2786" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<T>(this Result<T> result, Func<T, Task<Result>> collectionSelector, Func<T, Result, Error> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from user in GetUser(id)
    from _ in EnsureActiveAsync(user)
    select Error.Conflict(typeUri, "Not allowed."));
```


## SelectMany&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2845" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<T>(this Result<T> result, Func<T, Task<Result>> collectionSelector, Func<T, Result, Result> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUser(id)
    from _ in EnsureActiveAsync(user)
    select Audit(user));
```


## SelectMany&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Func&lt;T, Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2905" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<T, TNext>(this Result<T> result, Func<T, Task<Result>> collectionSelector, Func<T, Result, Result<TNext>> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from user in GetUser(id)
    from _ in EnsureActiveAsync(user)
    select CreateOrder(user));
```


## SelectMany&lt;TMiddle, TNext&gt;(Result, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L2967" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<TMiddle, TNext>(this Result result, Func<Result, Task<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, TNext> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, TNext&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `collectionSelector` returns `null` or a task with a `null` result.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in Validate(request)
    from user in GetUserAsync(request.UserId)
    select user.Name);
```


## SelectMany&lt;TMiddle&gt;(Result, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L3025" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<TMiddle>(this Result result, Func<Result, Task<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, Error> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Error](../Error/Error.md)&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in Validate(request)
    from user in GetUserAsync(request.UserId)
    select Error.Conflict(typeUri, "Not allowed."));
```


## SelectMany&lt;TMiddle&gt;(Result, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L3091" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany<TMiddle>(this Result result, Func<Result, Task<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, Result> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Result](../Result/Result.md)&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in Validate(request)
    from user in GetUserAsync(request.UserId)
    select Audit(user));
```


## SelectMany&lt;TMiddle, TNext&gt;(Result, Func&lt;Result, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L3158" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<TMiddle, TNext>(this Result result, Func<Result, Task<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, Result<TNext>> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in Validate(request)
    from user in GetUserAsync(request.UserId)
    select CreateOrder(user));
```


## SelectMany&lt;TNext&gt;(Result, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L3219" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<TNext>(this Result result, Func<Result, Task<Result>> collectionSelector, Func<Result, Result, TNext> resultSelector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), TNext&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `collectionSelector` returns `null` or a task with a `null` result.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in Validate(request)
    from unique in EnsureUniqueAsync(request)
    select request.Id);
```


## SelectMany(Result, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L3276" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany(this Result result, Func<Result, Task<Result>> collectionSelector, Func<Result, Result, Error> resultSelector);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

### Examples
```csharp
var result = await (
    from _ in Validate(request)
    from unique in EnsureUniqueAsync(request)
    select Error.Conflict(typeUri, "Not allowed."));
```


## SelectMany(Result, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L3338" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result> SelectMany(this Result result, Func<Result, Task<Result>> collectionSelector, Func<Result, Result, Result> resultSelector);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in Validate(request)
    from unique in EnsureUniqueAsync(request)
    select Audit(request));
```


## SelectMany&lt;TNext&gt;(Result, Func&lt;Result, Task&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L3401" target="_blank">LinqTaskAsyncExtensions.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static Task<Result<TNext>> SelectMany<TNext>(this Result result, Func<Result, Task<Result>> collectionSelector, Func<Result, Result, Result<TNext>> resultSelector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result`, the `collectionSelector` or the `resultSelector` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the `collectionSelector` returns `null` or a task with a `null` result, or the `resultSelector` returns `null`.

### Remarks
A non-generic [Result](../Result/Result.md) clause has no value: its range variable is the successful [Result](../Result/Result.md) itself (e.g., `from _ in Validate(input)`). A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`).

A result that the result selector returns is not nested (as by `Then`): a [Result](../Result/Result.md) or a [Result&lt;T&gt;](../ResultT/ResultT.md) is returned as it is. Specify the type arguments to keep it nested on purpose.

### Examples
```csharp
var result = await (
    from _ in Validate(request)
    from unique in EnsureUniqueAsync(request)
    select CreateOrder(request));
```

## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
* [Write C# Linq Queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries)
