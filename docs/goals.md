# Goals
1. Follow standards where possible:
	1. Industry standard for error responses: [RFC 9457](https://datatracker.ietf.org/doc/html/rfc9457) (which updates the [RFC 7807](https://datatracker.ietf.org/doc/html/rfc7807)).
	1. The standards that the values build on: *JSON Pointer* ([RFC 6901](https://datatracker.ietf.org/doc/html/rfc6901)) for the pointers of error details, the *URN* ([RFC 8141](https://datatracker.ietf.org/doc/html/rfc8141)) and *tag* ([RFC 4151](https://datatracker.ietf.org/doc/html/rfc4151)) *URI*s, and the meaning of the HTTP status codes (e.g., `LimitExceeded` for `429`, and `PermissionDenied` for `403`, as `401 Unauthorized` means a missing authentication).
	1. Error types: existing *Result* implementations use the same or similar type of errors.
	1. Naming: using idiomatic and clear names and the .NET patterns (e.g., `TryGetValue`, `Parse` and `TryParse`, a property for a state or a fixed value), that do not clash with the names of .NET and ASP.NET Core.
1. Always valid:
	1. A `Result`, a `Result<T>` and an `Error` are valid when they are created, and never change afterwards: an error kept in a static field or used as a dictionary key stays the same.
	1. A result is either successful or failed; there is no third state.
	1. An invalid value is rejected where it is passed, with an exception that names the parameter, instead of being stored or silently dropped.
1. No silent mistakes:
	1. A call does what it looks like, or the compiler or the analyzer reports it: no task that nobody awaits, and no failure that nobody sees.
	1. One meaning per method name, which does not depend on what a lambda returns (e.g., `Then` transforms a result, `IfSuccess` runs a side effect).
	1. A forgotten check fails where the value is read (e.g., `Result<T>.Value` of a failed result), instead of reading as a valid value.
1. Easy to use:
	1. Easy API, idiomatic, with easy to guess usage, that does not require studying documentation.
	1. Limited number of required parameters, data.
	1. Limited number of additional required code (e.g., additional classes, enums, records, etc.).
	1. The common cases need no extra code (e.g., synchronous and asynchronous steps in one chain or one *LINQ* query, both kinds of tasks, turning an exception into a result).
1. Flexible:
	1. Easy to extend—without writing additional code.
	1. Easy to extend—by writing additional code.
	1. Supports i18n.
1. Runs on modern .NET:
	1. Supports *Native AOT* and trimming: the core package produces no trimming or AOT warnings, and the JSON converters are separate packages.
	1. Easy to debug and log: a short debugger display, and a complete `ToString()`.
1. Minimize code degradation:
	1. Well-organized.
	1. Consistent.
	1. Supporting standard scenarios so there is no need to “reinvent the wheel”.
