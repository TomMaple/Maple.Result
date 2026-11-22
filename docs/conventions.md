# Conventions
These conventions add to the shared [contributing guide](https://github.com/TomMaple/.github/blob/main/CONTRIBUTING.md) and win
over it.

## Git
1. Workflow: [GitHub flow](https://docs.github.com/en/get-started/using-github/github-flow)
1. Branch naming:
	* `main`,
	* `feature/{issue_id}-{kebab-case-name}`
	* `bugfix/{issue_id}-{kebab-case-name}`
1. Branches:
	* Create a branch from an up-to-date `main`, after the previous pull request is merged.
	* Delete a branch once its pull request is merged.
1. Commit messages:
	* [Conventional commits](https://www.conventionalcommits.org/)
	* Past tense
	* The issue number at the end of the subject, e.g., `feat: Rejected invalid ErrorUri tags (#112)`.
	* A breaking change is marked with `!` (e.g., `feat!:`) or a `BREAKING CHANGE:` footer; it drives the version bump
	  (see [Releasing](releasing.md)).
1. Commits:
	* Changes in the `docs` folder go in a separate `docs:` commit, made after all the other changes (code and tests) are
	  committed.
1. Signing:
	* Commits are signed (SSH signing, `commit.gpgsign = true`); `main` accepts only signed commits.
	* A contributor’s unsigned commits are fine: the pull request is squashed, and GitHub signs the squash commit.
	* Before merging your own branch with a merge commit, check that every commit is signed
	  (`git log --format='%h %G? %s' main..`, all `G`); re-sign with `git rebase --force-rebase --gpg-sign main`.
1. Before opening a pull request, all of these pass:
	* `dotnet build -c Release`: no warnings (CI treats them as errors; this includes the code style, naming, trim and AOT
	  rules),
	* `dotnet test`,
	* `dotnet format --verify-no-changes`,
	* `python .github/scripts/check_reference_docs.py`: the reference pages agree with the code (see *Documentation*).

	The required checks are `build`, `newer-sdk`, `pr-title`, `Analyze (csharp)` and `Analyze (actions)`. On a pull
	request without code changes (e.g., only `docs/`), `build`, `newer-sdk` and `Analyze (csharp)` are skipped, which
	counts as passed. The job `reference` checks the reference pages on every pull request, also on one that changes
	only the documentation.

	`newer-sdk` builds and tests with the .NET SDK after the one of `build` (now .NET 11), because consumers may compile
	the package with a newer compiler than ours. The test projects compile their snippets (the binding, analyzer and
	code-fix tests) with the Roslyn packages of the version `TestRoslynVersion` (`Directory.Build.props`): 5.0.0 by
	default, the oldest compiler that has to load the analyzers, and the compiler of that SDK in `newer-sdk`
	(`TEST_ROSLYN_VERSION` in `ci.yml`). To run them locally with it, install that SDK and run
	`dotnet test -c Release -p:TestRoslynVersion=5.12.0`, then revert the lock files that it updates. The job
	warns when the compiler of the SDK has another version than `TEST_ROSLYN_VERSION`: raise it with the next feature band
	or major version of the SDK.
1. Pull requests:
	* The title follows the commit message rules: a squash merge turns the title into the subject of the commit on
	  `main` and the description into its body, so the release notes and the version bump come from them.
	* A breaking change is marked with `!` in the title (e.g., `feat!: …`) or a `BREAKING CHANGE:` footer at the end of
	  the description; a `!` on a branch commit alone is lost in a squash merge.
	* The description links the issue with `Closes #…`.
1. Merging:
	* Squash and merge by default, including contributor and Dependabot pull requests.
	* Create a merge commit only for your own branch whose every commit is a signed conventional commit; the commits
	  then reach `main` unchanged.
	* Rebase merging is disabled.
	* Dependabot pull requests for NuGet minor and patch updates merge automatically once the required checks pass;
	  NuGet majors and GitHub Actions updates are reviewed and merged by hand.
1. Workflows:
	* Every action is pinned to a full commit SHA with the version in a comment (`uses: owner/action@<sha> # vX.Y.Z`);
	  the repository rejects unpinned actions.
	* A new third-party action must be added to the allowlist (Settings → Actions → General) first.
	* A workflow that provides a required check has no `paths` filter; its jobs are skipped instead
	  (`.github/scripts/changes.py`).

## C#
1. Source code
	* [Framework design guidelines](https://learn.microsoft.com/dotnet/standard/design-guidelines/) (public API) and
	  [C# coding conventions](https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions)
	  (code style); where they differ from the rules below or from `.editorconfig`, the rules here win.
	* Indent character: *space*.
	* Constant naming style: *PascalCase*.
	* Formatting: `.editorconfig`; lines up to 160 characters.
	* Every source file starts with the SPDX license identifier and the copyright header.
	* Members are ordered alphabetically within each `#region`.
	* A call that compiles but does not do what it looks like (e.g., a task that nobody awaits) is reported by a rule of the
	  analyzer project, `src/Maple.Result.Analyzers`. The library has no overload whose only purpose is to turn a call into a
	  compile error, and `[Obsolete]` is not used for that.
	* Chained calls go one per line, unless the whole statement fits on one line.
	* An expression-bodied method or local function is never written on one line: the `=>` starts the next line, indented
	  once (e.g., `private static bool IsFunction(SyntaxNode node)` and, on the next line, `=> node is …;`). Any other `=>`
	  that is followed by a line break (a lambda, a property, a switch expression arm) starts the next line too, unless it
	  opens a block (`{`).
	* Empty lines:
		* before a `return`, unless the block has only two one-line statements;
		* before the other control-flow statements (`throw`, `break`, `continue`, `goto`, `yield return`, `yield break`),
		  unless the statement directly follows the line that opens it (e.g., the body of an `if` without braces, or the
		  first statement after `{`); not in a `switch` section (no empty line before its `break`);
		* after each section of a `switch` statement (after its `break`), except the last one;
		* after a statement that spans several lines (Rider applies it, `.editorconfig`);
		* after `#region` and before `#endregion`, so that the region's content is set off from both.
	* Conditional expressions (`?:`) go one operand per line, with `?` and `:` at the start of the line (Rider applies it,
	  `.editorconfig`).
	* In `src`, an argument or an initializer value is a simple expression: a variable, a constant or a literal, a member
	  access, `nameof`, a lambda, a `??` (but not `?? throw …`), a simple arithmetic expression (e.g., `count + 1`), or a
	  string built for an exception message. Anything else is assigned to a well-named local variable first:
		* to make it readable: a conditional expression, an `await`, a long or nested expression;
		* to give it a meaning: a compound condition whose purpose is not obvious (`var isExpired = …;` rather than
		  `if (a && (b || c))`);
		* to make a failure easy to locate: a call, an object creation or any other expression that can throw is its own
		  statement, also inside an argument list or an object initializer (e.g., `var successResult = ifSuccessFunction(x);`
		  and then `Guard.EnsureNotNullReturned(successResult, …)`, not `Guard.EnsureNotNullReturned(ifSuccessFunction(x), …)`).

	  The exceptions are a `this(…)` or `base(…)` constructor initializer and a field initializer, which no statement can
	  precede. Tests may pass calls and other expressions as arguments where it keeps them readable.
	* Exceptions name the caller’s parameter (e.g., `detailParameters` rather than the parameter of an internal helper).
	* Invalid input is rejected with an exception; it is never silently normalized (e.g., trimmed or escaped) or dropped.
1. XML documentation
	* Every public member is documented (`GenerateDocumentationFile` is on).
	* Documentation shared by several members is written once and inherited with `<inheritdoc cref="…" />`; the local
	  tags (e.g., `<summary>`, `<returns>`) go before it.
1. Tests
	* Types: unit, integration, functional, guard (convention); one project per type, named
	  `Maple.Result.Tests.<Type>` (e.g., `Maple.Result.Tests.Unit`). Each other project of `src` has its own test project,
	  named `<project>.Tests.Unit` (e.g., `src/Maple.Result.Analyzers` → `tests/Maple.Result.Analyzers.Tests.Unit`).
	* Layout: the folders mirror the tested project (e.g., `src/Maple.Result/Validators/MetadataKeyValidator.cs` →
	  `tests/Maple.Result.Tests.Unit/Validators/MetadataKeyValidatorUnitTests.cs`); namespaces follow the folders,
	  under the root namespace `Maple.Results.Tests.<Type>` (the project and folder names keep the package name
	  `Maple.Result`; the namespaces use `Maple.Results`, like the library).
	* Test class: `<TestedType><TestType>Tests` (e.g., `ErrorBuilderUnitTests`). Tests of one concern across many types
	  are named after the concern (e.g., `NullCheckUnitTests`, `SynchronizationContextUnitTests`).
	* Test method: `<Member>_<Scenario>_<ExpectedBehavior>`
	  ([naming convention](https://learn.microsoft.com/dotnet/core/testing/unit-testing-best-practices#naming-your-tests)),
	  e.g., `WithMetadata_InvalidKey_ThrowsArgumentException`.
	* Several test files for one type: a `<TestedType>Tests` folder holds `<TestedType><TestType>Tests.cs` for the
	  general tests and `<TestedType><Aspect><TestType>Tests.cs` for each aspect (e.g., `ErrorTests/ErrorUnitTests.cs`,
	  `ErrorTests/ErrorEqualityUnitTests.cs`, `ErrorTests/ErrorMetadataUnitTests.cs`). The `Tests` suffix keeps the
	  namespace from hiding the type (a folder `Error` would make the namespace `….Unit.Error` hide the `Error` type).
	* Every class that is not a test class goes under `TestingInfrastructure/`, by kind:
		* `Fakes/`: test doubles and mocking support (e.g., `SingleThreadSynchronizationContext`,
		  `ResultDefaultValueProvider`),
		* `Fixtures/`: xUnit class and collection fixtures, and collection definitions (e.g.,
		  `ResultConfigurationCollection`),
		* `Helpers/`: static helper methods (e.g., `SerializationHelper`),
		* `TestData/`: test data shared by several test classes (e.g., `TheoryData` sources); test data used by one
		  test class stays in that class.
	* Each test is split into `// Arrange`, `// Act` and `// Assert` sections (`// Act & Assert` when they cannot be
	  separated).
	* Assertions use [Shouldly](https://docs.shouldly.org/); exceptions are captured with `Record.Exception(…)` and
	  then asserted.
	* No loops in assertions: one case per test, or per `[InlineData]` / `[MemberData]` row of a `[Theory]`.
	* Every bug fix comes with a test that fails without the fix.
	* Code coverage: CI collects it with `tests/CodeCoverage.config`, which leaves nothing out, and shows the summary in the
	  job summary. The `build` job fails when the line or
	  the branch coverage is below 90% (`MINIMUM_COVERAGE` in `ci.yml`). Every line of the code that runs is covered; a
	  branch that cannot be reached is explained in the pull request that leaves it. To measure it locally, pass the
	  settings file with an absolute path (a relative one is resolved against the output folder of the tests):
	  `dotnet test -c Release --coverage --coverage-output-format cobertura --coverage-settings "$PWD/tests/CodeCoverage.config"`.
	* Code written with an AI assistant is developed test-first
	  ([TDD](https://martinfowler.com/bliki/TestDrivenDevelopment.html)): first the tests that describe the change are
	  written and seen to fail, then the implementation makes them pass, then the code is refactored while they stay
	  green. The tests are reviewed before the implementation, so they state the intended behaviour rather than repeat
	  what the code happens to do.

## Analyzer rules
1. Identifiers: the prefix `MR` and four digits, with a range of 1,000 for each package or group of packages, so that
   the rules of the companion packages form one family with those of the core and never collide:

	| Range | Package |
	| ----- | ------- |
	| `MR0001`–`MR0999` | `Maple.Result` (this repository) |
	| `MR1000`–`MR1999` | the JSON converters: `Maple.Result.Serialization.SystemTextJson` and `Serialization.NewtonsoftJson` |
	| `MR2000`–`MR2999` | `Maple.Result.Extensions.HttpClient` |
	| `MR3000`–`MR3999` | the mapping to HTTP responses: `Maple.Result.Extensions.AspNetCore`, `Extensions.MinimalApi`, `Extensions.Functions.Worker` and `Extensions.Functions.Worker.AspNetCore` |
	| `MR4000`–`MR9999` | reserved for packages to come (e.g., `Maple.Result.Extensions.Resilience`), a range of 1,000 each, given when a package gets its first rule |

1. A new rule takes the next free number of its range. The number says nothing about the severity, which a user can
   change (and a later version may too), nor about the kind of check; the severity and the category are in the
   descriptor and on the page of the rule.
1. An identifier is permanent: it is never renamed or reused, as users write it in `NoWarn`, `#pragma` and
   `.editorconfig`. A rule that is removed stays in `AnalyzerReleases.Shipped.md` as removed (the release tracking of
   the analyzer), and its page in `docs/Analyzers` says since which version.
1. These ranges follow the analyzer families of the platform, which group by subject and never by severity (e.g.,
   `CA1xxx` and `CA2xxx` of the .NET code analysis, `xUnit1xxx` and `xUnit2xxx`).

## Documentation
1. Typography: all texts (XML documentation, exception messages, Markdown documentation) use typographic characters:
   quotes (‘…’, “…”), apostrophes (’), em dashes (—), en dashes for ranges (e.g., 2–32) and the ellipsis (…).
   Code, identifiers, code spans and string values that are part of an API or a format (e.g., JSON) keep the ASCII
   characters.
1. The `README.md` of the repository is an overview for GitHub (relative links, GitHub alerts); the guides are in
   `docs/guides`. The package README, `src/Maple.Result/README.md`, is shown on nuget.org, which renders CommonMark:
   it uses absolute links only, no GitHub alerts, and images only from the hosts that nuget.org allows. Keep the quick
   start of both in sync.
1. The reference pages in `docs/Reference` are written by hand, in the layout of the
   [.NET API reference](https://learn.microsoft.com/dotnet/api/) of Microsoft:
	* Every public member has a page, or a section of the page of its overloads.
	* A type page links to the file of its type, without a line (one link for each file of a `partial` type). Its
	  declaration is the one of the code, without `partial`. Its member tables (*Constructors*, *Properties*,
	  *Methods*, *Operators*) are in alphabetical order, static and instance members together.
	* Every member has a `Source:` link that opens the line of its declaration (`…/blob/main/src/….cs#L<line>`) and is
	  labelled with the file name. A source link opens `main` or the tag of a release (`…/blob/v3.0.0/src/…`): the
	  pull request that prepares a release pins every link to its tag (see [Releasing](releasing.md)), and between
	  releases a link keeps that tag while the declaration that it opens stays the same.
	* The `csharp` block under the link is that declaration, with neither `async` nor `partial`, which are not part of
	  a signature. It shows the attributes of the code (e.g., `[OverloadResolutionPriority]`, which decides what a
	  call binds to), except those that the .NET API reference leaves out: the attributes of the nullable analysis
	  (e.g., `[MemberNotNullWhen]`, `[NotNullWhen]`, `[MaybeNullWhen]`, `[DisallowNull]`) and those for the debugger
	  (e.g., `[DebuggerDisplay]`). The prose of the page says when a value is `null`.
	* A heading names a member with its type parameters and the types of its parameters, e.g.,
	  `Then<T, TNext>(Result<T>, Func<T, TNext>)`. Types are written with their .NET names (`String`, `Boolean`), a
	  nullable value type as `Nullable<T>` (e.g., `Nullable<Boolean>` for `bool?`), and a nullable reference type
	  without the `?`. The *Type Parameters*, *Parameters*, *Returns* and *Property Value* sections follow the same
	  rules, and a link that names a member with its parameters is written as the heading that it opens.
	* A link to learn.microsoft.com has no locale (`/dotnet/api/…`, not `/en-us/dotnet/api/…`).
1. After a code change, update the pages of the changed members, and link each of them to `main` with the line of its
   declaration. A link to `main` below the change gets its new line; a link to a release keeps its tag and line, as
   it opens the code of that release. `python .github/scripts/check_reference_docs.py` checks the rules above: a
   link to a release against the code at its tag (a declaration that changed since is reported, so that the member is
   linked to `main`), and that every relative link and anchor of `README.md` and `docs/` resolves. It needs the tags
   of the repository (`git fetch --tags`).
1. Every breaking change is documented in the migration guide of the major version it is released in, with what
   changed, why, and how to update the calling code.
