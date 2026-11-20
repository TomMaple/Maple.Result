; Shipped analyzer releases
; https://github.com/dotnet/roslyn/blob/main/src/RoslynAnalyzers/Microsoft.CodeAnalysis.Analyzers/ReleaseTrackingAnalyzers.Help.md

## Release 3.0.0

### New Rules

Rule ID | Category | Severity | Notes
--------|----------|----------|-------
MR0001 | Usage | Error | A task becomes the value of a result
MR0002 | Usage | Error | A task is not awaited by a side effect method
MR0003 | Usage | Error | A task, a result or an Error is stored as the value of a result
MR0004 | Usage | Error | A side effect method drops the result of its function
MR0005 | Usage | Warning | IfSuccess or IfError ignores the value of its function
MR0006 | Usage | Warning | A result is discarded
