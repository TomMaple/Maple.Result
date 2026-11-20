# LinqAsyncExtensions.SelectMany Methods
## Definition
Namespace: [Maple.Results.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Enables the LINQ query syntax with more than one `from` clause for [Result&lt;T&gt;](../ResultT/ResultT.md) and [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; or [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;, including queries that mix synchronous and asynchronous `from` clauses.

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
| [SelectMany&lt;T, TMiddle, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnextvaluetaskresultt-funct-valuetaskresulttmiddle-funct-tmiddle-tnext) | Applies an asynchronous selector to the value of an asynchronous result. |
| [SelectMany&lt;T, TMiddle, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnextvaluetaskresultt-funct-resulttmiddle-funct-tmiddle-tnext) | Applies a synchronous selector to the value of an asynchronous result. |
| [SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnextresultt-funct-valuetaskresulttmiddle-funct-tmiddle-tnext) | Applies an asynchronous selector to the value of a synchronous result. |
| [SelectMany&lt;T, TMiddle&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)](#selectmanyt-tmiddlevaluetaskresultt-funct-valuetaskresulttmiddle-funct-tmiddle-error) | Applies the asynchronous selector and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the result selector. |
| [SelectMany&lt;T, TMiddle&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)](#selectmanyt-tmiddlevaluetaskresultt-funct-resulttmiddle-funct-tmiddle-error) | Applies the synchronous selector and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the result selector. |
| [SelectMany&lt;T, TMiddle&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)](#selectmanyt-tmiddleresultt-funct-valuetaskresulttmiddle-funct-tmiddle-error) | Applies the asynchronous selector and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the result selector. |
| [SelectMany&lt;T, TMiddle&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&gt;)](#selectmanyt-tmiddlevaluetaskresultt-funct-valuetaskresulttmiddle-funct-tmiddle-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TMiddle, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&lt;TNext&gt;&gt;)](#selectmanyt-tmiddle-tnextvaluetaskresultt-funct-valuetaskresulttmiddle-funct-tmiddle-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, TNext&gt;)](#selectmanyt-tnextvaluetaskresultt-funct-valuetaskresult-funct-result-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, Error&gt;)](#selectmanytvaluetaskresultt-funct-valuetaskresult-funct-result-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, Result&gt;)](#selectmanytvaluetaskresultt-funct-valuetaskresult-funct-result-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, Result&lt;TNext&gt;&gt;)](#selectmanyt-tnextvaluetaskresultt-funct-valuetaskresult-funct-result-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle, TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, TNext&gt;)](#selectmanytmiddle-tnextvaluetaskresult-funcresult-valuetaskresulttmiddle-funcresult-tmiddle-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Error&gt;)](#selectmanytmiddlevaluetaskresult-funcresult-valuetaskresulttmiddle-funcresult-tmiddle-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&gt;)](#selectmanytmiddlevaluetaskresult-funcresult-valuetaskresulttmiddle-funcresult-tmiddle-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle, TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&lt;TNext&gt;&gt;)](#selectmanytmiddle-tnextvaluetaskresult-funcresult-valuetaskresulttmiddle-funcresult-tmiddle-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, TNext&gt;)](#selectmanytnextvaluetaskresult-funcresult-valuetaskresult-funcresult-result-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, Error&gt;)](#selectmanyvaluetaskresult-funcresult-valuetaskresult-funcresult-result-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&gt;)](#selectmanyvaluetaskresult-funcresult-valuetaskresult-funcresult-result-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&lt;TNext&gt;&gt;)](#selectmanytnextvaluetaskresult-funcresult-valuetaskresult-funcresult-result-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TMiddle&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Result&gt;)](#selectmanyt-tmiddlevaluetaskresultt-funct-resulttmiddle-funct-tmiddle-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TMiddle, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Result&lt;TNext&gt;&gt;)](#selectmanyt-tmiddle-tnextvaluetaskresultt-funct-resulttmiddle-funct-tmiddle-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, TNext&gt;)](#selectmanyt-tnextvaluetaskresultt-funct-result-funct-result-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, Error&gt;)](#selectmanytvaluetaskresultt-funct-result-funct-result-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, Result&gt;)](#selectmanytvaluetaskresultt-funct-result-funct-result-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, Result&lt;TNext&gt;&gt;)](#selectmanyt-tnextvaluetaskresultt-funct-result-funct-result-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle, TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, TNext&gt;)](#selectmanytmiddle-tnextvaluetaskresult-funcresult-resulttmiddle-funcresult-tmiddle-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, Error&gt;)](#selectmanytmiddlevaluetaskresult-funcresult-resulttmiddle-funcresult-tmiddle-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, Result&gt;)](#selectmanytmiddlevaluetaskresult-funcresult-resulttmiddle-funcresult-tmiddle-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle, TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, Result&lt;TNext&gt;&gt;)](#selectmanytmiddle-tnextvaluetaskresult-funcresult-resulttmiddle-funcresult-tmiddle-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, TNext&gt;)](#selectmanytnextvaluetaskresult-funcresult-result-funcresult-result-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany(ValueTask&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, Error&gt;)](#selectmanyvaluetaskresult-funcresult-result-funcresult-result-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany(ValueTask&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, Result&gt;)](#selectmanyvaluetaskresult-funcresult-result-funcresult-result-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, Result&lt;TNext&gt;&gt;)](#selectmanytnextvaluetaskresult-funcresult-result-funcresult-result-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TMiddle&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&gt;)](#selectmanyt-tmiddleresultt-funct-valuetaskresulttmiddle-funct-tmiddle-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&lt;TNext&gt;&gt;)](#selectmanyt-tmiddle-tnextresultt-funct-valuetaskresulttmiddle-funct-tmiddle-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, TNext&gt;)](#selectmanyt-tnextresultt-funct-valuetaskresult-funct-result-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, Error&gt;)](#selectmanytresultt-funct-valuetaskresult-funct-result-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, Result&gt;)](#selectmanytresultt-funct-valuetaskresult-funct-result-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, Result&lt;TNext&gt;&gt;)](#selectmanyt-tnextresultt-funct-valuetaskresult-funct-result-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle, TNext&gt;(Result, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, TNext&gt;)](#selectmanytmiddle-tnextresult-funcresult-valuetaskresulttmiddle-funcresult-tmiddle-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle&gt;(Result, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Error&gt;)](#selectmanytmiddleresult-funcresult-valuetaskresulttmiddle-funcresult-tmiddle-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle&gt;(Result, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&gt;)](#selectmanytmiddleresult-funcresult-valuetaskresulttmiddle-funcresult-tmiddle-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TMiddle, TNext&gt;(Result, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&lt;TNext&gt;&gt;)](#selectmanytmiddle-tnextresult-funcresult-valuetaskresulttmiddle-funcresult-tmiddle-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TNext&gt;(Result, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, TNext&gt;)](#selectmanytnextresult-funcresult-valuetaskresult-funcresult-result-tnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany(Result, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, Error&gt;)](#selectmanyresult-funcresult-valuetaskresult-funcresult-result-error) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany(Result, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&gt;)](#selectmanyresult-funcresult-valuetaskresult-funcresult-result-result) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |
| [SelectMany&lt;TNext&gt;(Result, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&lt;TNext&gt;&gt;)](#selectmanytnextresult-funcresult-valuetaskresult-funcresult-result-resulttnext) | Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result. |

## Remarks
On a `Result` or a `Result<T>`, the function decides the kind of task: a function that returns a `ValueTask` gives a `ValueTask`, and an `async` lambda gives a `Task` (with an explicit return type, e.g., `async ValueTask<int> (int x) => …`, it gives a `ValueTask`). On a `Task` or a `ValueTask` of a result, the kind of the receiver is kept.

A non-generic [Result](../Result/Result.md) can be any `from` clause, e.g., a check in the middle of a query (`from _ in EnsureActive(user)`) or the first clause (`from _ in Validate(request)`): its range variable is the successful `Result` itself. A query cannot declare a range variable twice, so a second such clause needs another name (e.g., `from active in …`). A result selector that returns a `Result` or a `Result<T>` is not nested, as `Then` does.

A result selector that returns an [Error](../Error/Error.md) makes the query fail: such a query returns a failed [Result](../Result/Result.md) instead of a **successful** `Result<Error>`.

A result selector that returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>` is not awaited by the query: the query compiles, and the task becomes the value of the result, which the analyzer reports as an error ([MR0001](../../Analyzers/MR0001.md)). Await the operation with [ThenAsync](ThenAsyncExtensions_ThenAsync.md) instead, or chain it in a `from` clause whose function returns a `Task<Result<T>>`. To keep the task in the result on purpose, select it in a result (e.g., `select Result.FromValue(SaveAsync(x))`) or call the method with explicit type arguments (e.g., `SelectMany<T, TMiddle, Task>(…)`); the analyzer does not report these.

The query syntax needs a value in every `from` clause, so it works with [Result&lt;T&gt;](../ResultT/ResultT.md) but not with the non-generic [Result](../Result/Result.md). A `Task` and a `ValueTask` cannot be mixed in one query.


## SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L172" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L231" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L281" target="_blank">LinqAsyncExtensions.Task.cs</a>

Applies the asynchronous `collectionSelector` and the `resultSelector` to the value of a successful `result`; otherwise, returns a failed result with the original [Error](../Error/Error.md).

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L344" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L412" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L469" target="_blank">LinqAsyncExtensions.Task.cs</a>

Applies the asynchronous `collectionSelector` and the `resultSelector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector`; otherwise, returns a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md).

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L759" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L824" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L888" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L945" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1010" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1076" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1141" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1202" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1271" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1341" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1405" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1465" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1530" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1599" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1662" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1711" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1762" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1810" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1859" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1909" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L1961" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2013" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2069" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2126" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2177" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2228" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2280" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2333" target="_blank">LinqAsyncExtensions.Task.cs</a>

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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2385" target="_blank">LinqAsyncExtensions.Task.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2445" target="_blank">LinqAsyncExtensions.Task.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2507" target="_blank">LinqAsyncExtensions.Task.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2562" target="_blank">LinqAsyncExtensions.Task.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2622" target="_blank">LinqAsyncExtensions.Task.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2683" target="_blank">LinqAsyncExtensions.Task.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2746" target="_blank">LinqAsyncExtensions.Task.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2805" target="_blank">LinqAsyncExtensions.Task.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2872" target="_blank">LinqAsyncExtensions.Task.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L2940" target="_blank">LinqAsyncExtensions.Task.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L3002" target="_blank">LinqAsyncExtensions.Task.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L3060" target="_blank">LinqAsyncExtensions.Task.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L3123" target="_blank">LinqAsyncExtensions.Task.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
[OverloadResolutionPriority(1)]
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
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.Task.cs#L3187" target="_blank">LinqAsyncExtensions.Task.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
[OverloadResolutionPriority(1)]
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

## SelectMany&lt;T, TMiddle, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L152" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Returns a new instance of [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; instance by applying the provided selector functions if the current [Result&lt;T&gt;](../ResultT/ResultT.md) if it is successful; or a new [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; instance with the same error, otherwise.


```csharp
public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
    this ValueTask<Result<T>> result,
    Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
    Func<T, TMiddle, TNext> resultSelector);
```

### Type Parameters
#### `T`
The type of the [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; `result` value used to determine whether to execute the passed selector functions. If successful, this is also the type of the parameter passed to that function.

#### `TMiddle`
The type of the [Result](../ResultT/ResultT.md)&lt;TMiddle&gt; `result` output type of the passed collection selector and the type of one of the parameters passed to the result selector.

#### `TNext`
The type of the returned [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt; `result` value.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The value task object representing the asynchronous operation that returns an instance of a [Result](../ResultT/ResultT.md) record to be evaluated.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that takes the value of the initial result and returns a value task object representing the asynchronous operation that returns a new result representing an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, TNext&gt;
The function that combines the value from the initial result and the intermediate value to produce the final value.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A value task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If any of the `collectionSelector` or `resultSelector` parameters are `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`, or the `collectionSelector` function returns a value task with a `null` result.

### Remarks
This method is used to enable LINQ query syntax for [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; records. It allows chaining multiple asynchronous operations that return [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; while maintaining the error state, if any of the operations fail.

### Examples
```csharp
var emailResult =
    await from addUser in _validationService.ValidateUserDataAsync(addUserRequest)
    from user in _userService.CreateUserAsync(addUser)
    from userGroup in _groupService.AddUserToGroupAsync(user)
    from sendWelcomeEmailResult in _emailService.SendWelcomeEmailAsync(user)
    select sendWelcomeEmailResult;
```


## SelectMany&lt;T, TMiddle, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L212" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Applies the synchronous `collectionSelector` and the `resultSelector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result`; otherwise, returns a failed result with the original [Error](../Error/Error.md).

```csharp
public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
    this ValueTask<Result<T>> result,
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
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The value task object representing the asynchronous operation that returns the initial result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The synchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, TNext&gt;
The function that combines the value of the initial result and the intermediate value to produce the final value.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A value task object representing the asynchronous operation: the result of applying the selectors if both the initial and the intermediate results are successful; otherwise, a failed result with the first [Error](../Error/Error.md).

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If any of the `collectionSelector` or `resultSelector` parameters are `null`.

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


## SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L263" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Applies the asynchronous `collectionSelector` and the `resultSelector` to the value of a successful `result`; otherwise, returns a failed result with the original [Error](../Error/Error.md).

```csharp
public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
    this Result<T> result,
    Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
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

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The asynchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, TNext&gt;
The function that combines the value of the initial result and the intermediate value to produce the final value.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A value task object representing the asynchronous operation: the result of applying the selectors if both the initial and the intermediate results are successful; otherwise, a failed result with the first [Error](../Error/Error.md).

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If any of the `result`, `collectionSelector` or `resultSelector` parameters are `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `collectionSelector` function returns a value task with a `null` result.

### Remarks
This overload enables a query in which a synchronous `from` clause is followed by an asynchronous one.

### Examples
```csharp
var ordersResult =
    await from user in _userService.GetUser(userId)         // synchronous
    from orders in _orderService.GetOrdersAsync(user)       // asynchronous
    select orders;
```


## SelectMany&lt;T, TMiddle&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L328" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Applies the asynchronous `collectionSelector` and the `resultSelector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector`; otherwise, returns a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md).

```csharp
public static ValueTask<Result> SelectMany<T, TMiddle>(
    this ValueTask<Result<T>> result,
    Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
    Func<T, TMiddle, Error> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.

#### `TMiddle`
The type of the value of the intermediate result returned by `collectionSelector`.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The value task object representing the asynchronous operation that returns the initial result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The asynchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and the intermediate value, and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The value task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector` if both the initial and the intermediate results are successful; otherwise, a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `collectionSelector` or `resultSelector` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`, the `collectionSelector` function returns a value task with a `null` result, or the `resultSelector` function returns `null`.

### Remarks
This overload makes a query with more than one `from` clause that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [SelectMany&lt;T, TMiddle, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnextvaluetaskresultt-funct-valuetaskresulttmiddle-funct-tmiddle-tnext) and return a **successful** `Result<Error>`.

### Examples
```csharp
var result =
    await from user in _userService.GetUserAsync(userId)
    from order in _orderService.GetOrderAsync(user, orderId)
    select Error.Conflict(typeUri, $"The order {order.Number} is already shipped.");
```


## SelectMany&lt;T, TMiddle&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L397" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Applies the synchronous `collectionSelector` and the `resultSelector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector`; otherwise, returns a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md).

```csharp
public static ValueTask<Result> SelectMany<T, TMiddle>(
    this ValueTask<Result<T>> result,
    Func<T, Result<TMiddle>> collectionSelector,
    Func<T, TMiddle, Error> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.

#### `TMiddle`
The type of the value of the intermediate result returned by `collectionSelector`.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The value task object representing the asynchronous operation that returns the initial result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The synchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and the intermediate value, and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The value task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector` if both the initial and the intermediate results are successful; otherwise, a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `collectionSelector` or `resultSelector` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`, the `collectionSelector` function returns `null`, or the `resultSelector` function returns `null`.

### Remarks
This overload makes a query with more than one `from` clause that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [SelectMany&lt;T, TMiddle, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnextvaluetaskresultt-funct-resulttmiddle-funct-tmiddle-tnext) and return a **successful** `Result<Error>`.

### Examples
```csharp
var result =
    await from user in _userService.GetUserAsync(userId)
    from order in _orderService.GetOrder(user, orderId)
    select Error.Conflict(typeUri, $"The order {order.Number} is already shipped.");
```


## SelectMany&lt;T, TMiddle&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L452" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Applies the asynchronous `collectionSelector` and the `resultSelector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector`; otherwise, returns a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md).

```csharp
public static ValueTask<Result> SelectMany<T, TMiddle>(
    this Result<T> result,
    Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
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

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The asynchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and the intermediate value, and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The value task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector` if both the initial and the intermediate results are successful; otherwise, a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result`, `collectionSelector` or `resultSelector` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `collectionSelector` function returns a value task with a `null` result, or the `resultSelector` function returns `null`.

### Remarks
This overload makes a query with more than one `from` clause that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnextresultt-funct-valuetaskresulttmiddle-funct-tmiddle-tnext) and return a **successful** `Result<Error>`.

### Examples
```csharp
var result =
    await from user in _userService.GetUser(userId)
    from order in _orderService.GetOrderAsync(user, orderId)
    select Error.Conflict(typeUri, $"The order {order.Number} is already shipped.");
```


## SelectMany&lt;T, TMiddle&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L735" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<T, TMiddle>(this ValueTask<Result<T>> result, Func<T, ValueTask<Result<TMiddle>>> collectionSelector, Func<T, TMiddle, Result> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Result](../Result/Result.md)&gt;
The function that receives the value and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;T, TMiddle, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L798" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(this ValueTask<Result<T>> result, Func<T, ValueTask<Result<TMiddle>>> collectionSelector, Func<T, TMiddle, Result<TNext>> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L860" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<T, TNext>(this ValueTask<Result<T>> result, Func<T, ValueTask<Result>> collectionSelector, Func<T, Result, TNext> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), TNext&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L918" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<T>(this ValueTask<Result<T>> result, Func<T, ValueTask<Result>> collectionSelector, Func<T, Result, Error> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L984" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<T>(this ValueTask<Result<T>> result, Func<T, ValueTask<Result>> collectionSelector, Func<T, Result, Result> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1051" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<T, TNext>(this ValueTask<Result<T>> result, Func<T, ValueTask<Result>> collectionSelector, Func<T, Result, Result<TNext>> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;TMiddle, TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1114" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(this ValueTask<Result> result, Func<Result, ValueTask<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, TNext> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, TNext&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;TMiddle&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1173" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<TMiddle>(this ValueTask<Result> result, Func<Result, ValueTask<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, Error> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Error](../Error/Error.md)&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;TMiddle&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1240" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<TMiddle>(this ValueTask<Result> result, Func<Result, ValueTask<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, Result> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Result](../Result/Result.md)&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;TMiddle, TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1308" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(this ValueTask<Result> result, Func<Result, ValueTask<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, Result<TNext>> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1370" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<TNext>(this ValueTask<Result> result, Func<Result, ValueTask<Result>> collectionSelector, Func<Result, Result, TNext> resultSelector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), TNext&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1428" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany(this ValueTask<Result> result, Func<Result, ValueTask<Result>> collectionSelector, Func<Result, Result, Error> resultSelector);
```

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1494" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany(this ValueTask<Result> result, Func<Result, ValueTask<Result>> collectionSelector, Func<Result, Result, Result> resultSelector);
```

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1561" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<TNext>(this ValueTask<Result> result, Func<Result, ValueTask<Result>> collectionSelector, Func<Result, Result, Result<TNext>> resultSelector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;T, TMiddle&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1622" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<T, TMiddle>(this ValueTask<Result<T>> result, Func<T, Result<TMiddle>> collectionSelector, Func<T, TMiddle, Result> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The function that receives the value and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Result](../Result/Result.md)&gt;
The function that receives the value and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;T, TMiddle, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1672" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(this ValueTask<Result<T>> result, Func<T, Result<TMiddle>> collectionSelector, Func<T, TMiddle, Result<TNext>> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The function that receives the value and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1721" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<T, TNext>(this ValueTask<Result<T>> result, Func<T, Result> collectionSelector, Func<T, Result, TNext> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The function that receives the value and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), TNext&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1770" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<T>(this ValueTask<Result<T>> result, Func<T, Result> collectionSelector, Func<T, Result, Error> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The function that receives the value and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1820" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<T>(this ValueTask<Result<T>> result, Func<T, Result> collectionSelector, Func<T, Result, Result> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The function that receives the value and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&gt;, Func&lt;T, Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1871" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<T, TNext>(this ValueTask<Result<T>> result, Func<T, Result> collectionSelector, Func<T, Result, Result<TNext>> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The function that receives the value and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;TMiddle, TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1921" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(this ValueTask<Result> result, Func<Result, Result<TMiddle>> collectionSelector, Func<Result, TMiddle, TNext> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, TNext&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;TMiddle&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L1971" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<TMiddle>(this ValueTask<Result> result, Func<Result, Result<TMiddle>> collectionSelector, Func<Result, TMiddle, Error> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Error](../Error/Error.md)&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;TMiddle&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2025" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<TMiddle>(this ValueTask<Result> result, Func<Result, Result<TMiddle>> collectionSelector, Func<Result, TMiddle, Result> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Result](../Result/Result.md)&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;TMiddle, TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, Result&lt;TMiddle&gt;&gt;, Func&lt;Result, TMiddle, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2080" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(this ValueTask<Result> result, Func<Result, Result<TMiddle>> collectionSelector, Func<Result, TMiddle, Result<TNext>> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2129" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<TNext>(this ValueTask<Result> result, Func<Result, Result> collectionSelector, Func<Result, Result, TNext> resultSelector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), TNext&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A successful result with the value of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany(ValueTask&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2178" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany(this ValueTask<Result> result, Func<Result, Result> collectionSelector, Func<Result, Result, Error> resultSelector);
```

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
A failed result with the error of the result selector, or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany(ValueTask&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2231" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany(this ValueTask<Result> result, Func<Result, Result> collectionSelector, Func<Result, Result, Result> resultSelector);
```

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;TNext&gt;(ValueTask&lt;Result&gt;, Func&lt;Result, Result&gt;, Func&lt;Result, Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2285" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<TNext>(this ValueTask<Result> result, Func<Result, Result> collectionSelector, Func<Result, Result, Result<TNext>> resultSelector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The asynchronous operation that returns the source result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful source result and returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The result that the result selector returns (not nested), or the first failed result.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `collectionSelector` or the `resultSelector` is `null`.

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


## SelectMany&lt;T, TMiddle&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2335" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<T, TMiddle>(this Result<T> result, Func<T, ValueTask<Result<TMiddle>>> collectionSelector, Func<T, TMiddle, Result> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Result](../Result/Result.md)&gt;
The function that receives the value and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
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


## SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2397" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(this Result<T> result, Func<T, ValueTask<Result<TMiddle>>> collectionSelector, Func<T, TMiddle, Result<TNext>> resultSelector);
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

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
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


## SelectMany&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2458" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<T, TNext>(this Result<T> result, Func<T, ValueTask<Result>> collectionSelector, Func<T, Result, TNext> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), TNext&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
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


## SelectMany&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2515" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<T>(this Result<T> result, Func<T, ValueTask<Result>> collectionSelector, Func<T, Result, Error> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
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


## SelectMany&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2577" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<T>(this Result<T> result, Func<T, ValueTask<Result>> collectionSelector, Func<T, Result, Result> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
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


## SelectMany&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Func&lt;T, Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2640" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<T, TNext>(this Result<T> result, Func<T, ValueTask<Result>> collectionSelector, Func<T, Result, Result<TNext>> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the source result.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the value and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, [Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the value and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
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


## SelectMany&lt;TMiddle, TNext&gt;(Result, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2702" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(this Result result, Func<Result, ValueTask<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, TNext> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, TNext&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
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


## SelectMany&lt;TMiddle&gt;(Result, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2760" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<TMiddle>(this Result result, Func<Result, ValueTask<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, Error> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Error](../Error/Error.md)&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
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


## SelectMany&lt;TMiddle&gt;(Result, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2826" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany<TMiddle>(this Result result, Func<Result, ValueTask<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, Result> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Result](../Result/Result.md)&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
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


## SelectMany&lt;TMiddle, TNext&gt;(Result, Func&lt;Result, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;Result, TMiddle, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2893" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<TMiddle, TNext>(this Result result, Func<Result, ValueTask<Result<TMiddle>>> collectionSelector, Func<Result, TMiddle, Result<TNext>> resultSelector);
```

### Type Parameters
#### `TMiddle`
The type of the value of the result that the collection selector returns.

#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), TMiddle, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful source result and the value of the next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
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


## SelectMany&lt;TNext&gt;(Result, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L2954" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<TNext>(this Result result, Func<Result, ValueTask<Result>> collectionSelector, Func<Result, Result, TNext> resultSelector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), TNext&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
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


## SelectMany(Result, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L3011" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany(this Result result, Func<Result, ValueTask<Result>> collectionSelector, Func<Result, Result, Error> resultSelector);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Error](../Error/Error.md)&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
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


## SelectMany(Result, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L3076" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result> SelectMany(this Result result, Func<Result, ValueTask<Result>> collectionSelector, Func<Result, Result, Result> resultSelector);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Result](../Result/Result.md)&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
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


## SelectMany&lt;TNext&gt;(Result, Func&lt;Result, ValueTask&lt;Result&gt;&gt;, Func&lt;Result, Result, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/v3.0.0-rc.1/src/Maple.Result/Extensions/LinqAsyncExtensions.ValueTask.cs#L3142" target="_blank">LinqAsyncExtensions.ValueTask.cs</a>

Chains a `from` clause of a query: if the source result is successful, gets the next result from the collection selector, and if that one is successful too, combines both with the result selector; otherwise, returns the first failed result.

```csharp
public static ValueTask<Result<TNext>> SelectMany<TNext>(this Result result, Func<Result, ValueTask<Result>> collectionSelector, Func<Result, Result, Result<TNext>> resultSelector);
```

### Type Parameters
#### `TNext`
The type of the value of the returned result.

### Parameters
#### `result` [Result](../Result/Result.md)
The source result. Must not be `null`.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Result](../Result/Result.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The function that receives the successful source result and returns the asynchronous operation that returns the next result. Must not be `null`.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;[Result](../Result/Result.md), [Result](../Result/Result.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function that receives the successful source result and the successful next result, and returns the projection. Must not be `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
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
* [Write C# Linq Queries](https://learn.microsoft.com/dotnet/csharp/linq/get-started/write-linq-queries)
