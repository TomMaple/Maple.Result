# Analyzer

The `Maple.Result` package contains a Roslyn analyzer, `Maple.Result.Analyzers`. It reports the calls that compile but do not do what they look like: a task that nobody awaits, a failure that nobody sees, and a task or a result stored as the value of another result.

## Contents
* [Installation](#installation)
* [Rules](#rules)
* [Code fixes](#code-fixes)
* [What the analyzer looks at](#what-the-analyzer-looks-at)
* [What the compiler reports by itself](#what-the-compiler-reports-by-itself)
* [Configuring the rules](#configuring-the-rules)
* [Without the analyzer](#without-the-analyzer)

## Installation
There is nothing to install. The analyzer is part of the `Maple.Result` package: a project that references the package gets the analyzer too, with no second package to reference. It also reaches a project that references the package through another package. The analyzer is for C#: a Visual Basic project does not get it.

## Rules
| Rule | Severity | Title | Example |
| ---- | -------- | ----- | ------- |
| [MR0001](MR0001.md) | Error | A task becomes the value of a result | `result.Then(x => GetAsync(x))` is a `Result<Task<int>>` whose task nobody awaits |
| [MR0002](MR0002.md) | Error | A task is not awaited by a side effect method | `result.IfSuccess(x => SaveAsync(x))` discards the task |
| [MR0003](MR0003.md) | Error | A task, a result or an Error is stored as the value of a result | `Result<object> result = SaveAsync();` is a successful result that holds the task |
| [MR0004](MR0004.md) | Error | A side effect method drops the result of its function | `result.IfSuccess(x => Validate(x))` loses a failure of `Validate` |
| [MR0005](MR0005.md) | Warning | IfSuccess or IfError ignores the value of its function | `var normalized = result.IfSuccess(x => Normalize(x));` keeps the original result |
| [MR0006](MR0006.md) | Warning | A result is discarded | `await SaveAsync(order);` loses a failed `Result` |

The category of every rule is `Usage`.

The identifiers of this package are `MR0001` to `MR0999`. The companion packages use their own ranges of the same
family: `MR1xxx` for the JSON converters, `MR2xxx` for `HttpClient` and `MR3xxx` for the mapping to HTTP responses
(ASP.NET Core, Minimal API, Azure Functions); `MR4xxx` and up are reserved for packages to come. An identifier says
nothing about the severity of a rule, and it is never reused for another rule.

## Code fixes
| Rule | Code fix |
| ---- | -------- |
| [MR0001](MR0001.md) | Renames a synchronous method to the method that awaits the task: `Then` to `ThenAsync` (to `IfSuccessAsync` for a task without a value), `Try` to `TryAsync`. |
| [MR0002](MR0002.md) | Renames a synchronous method to its `Async` form: `IfSuccess` to `IfSuccessAsync`, `IfError` to `IfErrorAsync`, `Try` to `TryAsync`. |
| [MR0003](MR0003.md) | None. |
| [MR0004](MR0004.md) | Renames `IfSuccess` to `Then` and `IfSuccessAsync` to `ThenAsync`; `IfError` to `Else` and `IfErrorAsync` to `ElseAsync` when the function returns an `Error` or the type of the result itself. |
| [MR0005](MR0005.md) | Renames `IfSuccess` to `Then`, `IfError` to `Else`, `IfSuccessAsync` to `ThenAsync`, and `IfErrorAsync` to `ElseAsync`. |
| [MR0006](MR0006.md) | Adds a discard: `_ = …`. For a task of a result that is not awaited, it awaits the task and discards the result (`_ = await …`) where an `await` is allowed in an `async` function or in top-level statements (not in a `lock` or `unsafe` statement, nor after a null-conditional access), and is not offered elsewhere. |

A method renamed to its `Async` form returns a task, so the code fix also awaits the call, as the code fixes of Microsoft for the same change do (VSTHRD103 of the threading analyzers, and the asynchronous assertions of MSTest):

* In an `async` method, local function or lambda, and in top-level statements, the renamed call is awaited: `result.Then(x => GetAsync(x))` becomes `await result.ThenAsync(x => GetAsync(x))`, in parentheses where the call is part of a larger expression (e.g., `(await result.ThenAsync(…)).IsSuccess`).
* In a method, local function or lambda that returns a `Task`, a `Task<T>`, a `ValueTask` or a `ValueTask<T>` and is not `async` yet, the code fix also adds the `async` modifier and rewrites the `return` statements: `return task;` becomes `return await task;` (or `await task;` when the task has no value), and `return Task.FromResult(value);` becomes `return value;`. A task that is complete already is left out instead of awaited: `return Task.CompletedTask;`, `return ValueTask.CompletedTask;` and `return default;` in a function that returns a `ValueTask` become `return;`, or nothing at the end of the function, and `return default;` stays as it is in a function that returns a `ValueTask<T>`.
* Elsewhere only the name changes, and the code that used the result no longer compiles or is reported by [MR0006](MR0006.md), until the call is awaited by hand. That is the case in a member that does not return a task (the code fix does not change a signature), after a null-conditional access (`result?.Then(…)`), inside a `lock`, `unsafe` or `fixed` statement, in a query, and in a catch filter; and in a function that cannot become `async`: a lambda converted to an expression tree, one with a `ref`, `out` or `in` parameter or a parameter of a `ref struct` (e.g., `Span<T>`), one in an unsafe member or type, and one that returns a `Task` that is `null` (`return null!;` or `return default!;`).

No rename is offered for an extension method called in its static form (e.g., `ThenExtensions.Then(result, …)`), where the other method is in another class.

Fix All (in a document, a project or the solution) applies the fix of each diagnostic, whatever method it renames to and whether it awaits the call.

## What the analyzer looks at
* C# code. A project that does not reference the library is not analyzed.
* Generated code, for the errors: MR0001 to MR0004 are reported there too, as a compile error would be. MR0005 and MR0006 are not reported in generated code.
* MR0001 to MR0005 report only the calls of the members of the library. A call of another method is not reported, also when it discards a task in the same way (e.g., `ids.ForEach(id => SaveAsync(id))`).
* MR0006 reports a call of any method that returns a `Result`, a `Result<T>`, an `IOperationResult`, or a `Task` or a `ValueTask` of one, and an `await` of anything that gives one (e.g., a call with `ConfigureAwait`, or a variable).
* A call that does not compile is the compiler’s to report: see the next section.

## What the compiler reports by itself
The analyzer reports what compiles. A delegate that fits no overload is rejected by the compiler, and its message does not say which method to use instead. The common cases:

| Call | Compiler errors | Use |
| ---- | --------------- | --- |
| A value or an `Error` as an expression in `IfSuccess` or `IfError`: `result.IfSuccess(x => x + 1)`, `result.IfError(e => 0)` | CS0201 | `Then` or `Else` |
| An action in `Then` or `Else`: `result.Then(x => Show(x))`, `result.Else(e => Log(e))` | CS0029, CS1662 | `IfSuccess` or `IfError` |
| A function that returns a task, or an `async` lambda, in `Else`: `result.Else(e => RecoverAsync(e))` | CS0411 | `ElseAsync` |
| Two actions in `Match`: `result.Match(x => Show(x), e => Log(e))` | CS0411 | `Switch` |
| Two actions in `MatchAsync`: `task.MatchAsync(x => Show(x), e => Log(e))` | CS0029, CS1662 | `SwitchAsync` |
| A function and an error action in `Match`: `result.Match(x => Compute(x), e => Log(e))` | CS0029, CS1662 | `Then(…).IfError(…)` |
| A synchronous function in `ThenAsync` or `ElseAsync` on a result (not on a task of a result): `result.ThenAsync(x => x + 1)` | CS0029, CS1662 | `Then` or `Else` |
| A synchronous action in `IfSuccessAsync` or `IfErrorAsync` on a result (not on a task of a result): `result.IfSuccessAsync(x => Show(x))` | CS0029, CS1662 | `IfSuccess` or `IfError` |
| A function that returns a task without a value in `ThenAsync` on a result, or in `ElseAsync`: `result.ThenAsync(x => SaveAsync(x))`, `task.ElseAsync(e => LogAsync(e))` | CS0029, CS1662, and CS0266 for a `Task` | `IfSuccessAsync` or `IfErrorAsync` |
| A method group that returns a task in `IfError`: `result.IfError(LogAsync)` | CS0407 | `IfErrorAsync` |

## Configuring the rules
**The errors (MR0001 to MR0004) are not configurable.** As a compile error, they cannot be suppressed and their severity cannot be changed:

* not with `<NoWarn>` in the project file,
* not in an `.editorconfig` file (`dotnet_diagnostic.MR0001.severity`, or a line for the category or for all rules),
* not with `#pragma warning disable`.

A call that is meant as it is written says so in the code, and is then not reported: explicit type arguments or `Result.FromValue` keep a task as a value (MR0001), and so does a type argument written as the type of the value (`Result<Task<int>> r = GetAsync();`, MR0003); a discard keeps a task that is not awaited (e.g., `e => _ = LogAsync(e)` or `e => { _ = LogAsync(e); }`, MR0002). The page of each rule has its way. Two things still silence the errors:

* `[SuppressMessage]` on the member or on the assembly;
* switching the analyzers off as a whole: the MSBuild property `RunAnalyzers` set to `false` (or `RunAnalyzersDuringBuild`, for builds only), which switches off every analyzer of the project;
* switching off only the analyzers of this package: the MSBuild property `MapleResultAnalyzers` set to `false` (`<MapleResultAnalyzers>false</MapleResultAnalyzers>` in the project file, or in a `Directory.Build.props` file for several projects). `ExcludeAssets="analyzers"` on the `PackageReference` is not enough: the package adds its analyzers to the C# projects that reference it, also through another package.

**The warnings (MR0005 and MR0006) are ordinary rules.** Their severity is set as for any other analyzer:

* in an `.editorconfig` file: `dotnet_diagnostic.MR0005.severity = none` (or `suggestion`, `warning`, `error`);
* in the code, around one place: `#pragma warning disable MR0006` and `#pragma warning restore MR0006`;
* in the project file: `<NoWarn>$(NoWarn);MR0006</NoWarn>`.

Visual Studio skips the analyzers in the builds that it starts implicitly (e.g., Run Tests, Start Debugging), unless `TreatWarningsAsErrors` is on. The errors are guaranteed to appear in the live analysis of the IDE, and in a build from the command line or in CI.

See more: [Configuration options for code analysis](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/configuration-options), [Suppress code analysis warnings](https://learn.microsoft.com/dotnet/fundamentals/code-analysis/suppress-warnings)

## Without the analyzer
The analyzer is the only check of these calls: the library has no other one. With the analyzers switched off, or a rule silenced, the calls that it reports compile without a message and run as they are written: the task is not awaited, the failure is lost, the result holds a task.
