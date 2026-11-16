# The Result pattern

Why an operation returns a result instead of throwing an exception, and where to draw the line between the two.

## Contents
* [Key benefits](#key-benefits)
* [Result vs. exceptions](#result-vs-exceptions)
* [When to use a Result and when an exception?](#when-to-use-a-result-and-when-an-exception)
* [What is the difference between a critical error and an exception?](#what-is-the-difference-between-a-critical-error-and-an-exception)
* [From the Stack Overflow discussion](#from-the-stack-overflow-discussion)
* [Articles](#articles)

## Key benefits
* **Explicitness** — clearly communicates (e.g., via the function signature) that the operation may fail; it forces developer to handle both scenarios: a success and a failure.
* **Consistency** — offers consistent type of error and consistent structure of responses.
* **Composability** — allows easier chaining of operations with extension methods, instead of multi-nested `if` blocks.
* **Predictable Error Handling** — clearly defines categories of errors, making it easier to properly handle expected failures (e.g., validation errors, availability problems, a not-found resource).
* **Support for localization** — uses data structures that allow to localize messages by the client.
* **Easier Testing** — provides an easy way to verify in tests whether the operation was successful or what errors occurred.
* **Improved Readability** — limits the usage and scope of `try-catch` blocks to places that actually can throw exceptions (e.g., calls to the database, cloud resources, disk operations, HTTP calls, etc.).
* **Improved Monitoring** — allows to limit the number of thrown exceptions to truly exceptional cases making it easier to monitor them (if we don’t need to filter out exceptions used for the flow control).
* **Improved Debugging** — allows to enrich the error outcome with additional metadata.
* **Follows Standards** — follows [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457) and other industry standards.

## Result vs. exceptions
| **Feature** | **Result** | **Exceptions** |
| ----------- | ---------- | -------------- |
| **Use Cases** | Expected, routine failures *(e.g., validation, not found, missing data, authentication and authorization errors)* | Rare, truly unexpected system or technical errors that usually indicate a bug or infrastructure problems *(e.g., database connection loss, network issues)* |
| **Mechanism** | Returns a structured record as the function outcome | Disrupts the normal program flow and bubbles up the call stack |
| **Error Handling Style** | Functional, return-value based | Imperative, runtime-based |
| **Type Safety** | Type-safe—the compiler knows a method can return an error | Not type-safe—errors are not visible in a method signature |
| **Control flow** | Clean, predictable flow maintained | Disrupted when an error occurs |
| **Composition** | Easy chaining of operations | Hard with multiple `try-catch` |
| **Boilerplate** | Can add some boilerplate code, which can be reduced with extension methods and other libraries | Centralizes error handling, keeping business logic clean of error checks |
| **Performance** | Better for frequent failures—avoids the overhead of exception handling | Slower—exceptions are expensive due to the cost of generating stack traces |
| **Monitoring** | Easy to distinguish expected errors from unexpected problems that needs potential attention | Requires filtering out expected errors from real issues. It can make it harder to investigate problems. |

## When to use a Result and when an exception?
It is up to you where to draw the line between expected failure and unexpected exception.

In some cases, a missing file is just a resource *Not Found* error (e.g., users tries to open a non-existing document); in others, it might be an unexpected exception (e.g., a DLL file or expected configuration is missing).

Sometimes, the detailed information is not required. Then, a simple enum or a boolean value can be used instead of the `Result` type.

My suggestion is:
* use **exceptions** for cases that require someone’s attention (e.g., database unavailable, connection string to a crucial resource is `null`, unexpected case that indicates a bug, expired API keys that require manual update),
* use **Result** for cases that you either want to handle in the normal program flow or return in the API (e.g., user input validation, user authentication/authorization errors, handling transient errors that will be recovered by retry, etc.),
* use **enum** or a **boolean** value if you want to handle multiple cases in the normal program flow, but you don’t need additional information (e.g., folder not exists, so it should be created; token is missing, invalid, valid or expired); most likely **NOT** for API responses.

To turn the exceptions of code you call into failed results, see [Turning exceptions into results](chaining.md#turning-exceptions-into-results).

## What is the difference between a critical error and an exception?
It is to a developer to draw a line between the errors that are critical and are handled in one way or another, and the ones that are unhandled and disrupt the normal program flow and bubble up the call stack.

## From the Stack Overflow discussion
From [Why not use exceptions as regular flow of control?](https://stackoverflow.com/questions/729379/why-not-use-exceptions-as-regular-flow-of-control):

> Have you ever tried to debug a program raising five exceptions per second in the normal course of operation ?

> Exceptions are basically non-local `goto` statements with all the consequences of the latter. Using exceptions for flow control violates a [principle of least astonishment](http://c2.com/cgi/wiki?PrincipleOfLeastAstonishment), make programs hard to read

> If you use exceptions for normal situations, how will you locate things that are *really* exception?

## Articles
* [Exception Vs Result Pattern. The Exception vs Result Pattern… | by Shreyans Padmani | Medium](https://medium.com/@shreyans_padmani/exception-vs-result-pattern-114f2e389153)
* [The Result Pattern: Simplifying Error Handling in Your Code | by Adam Hancock | Medium](https://medium.com/@dev-hancock/the-result-pattern-simplifying-error-handling-in-your-code-fc31bb50a244)
* [Working with the result pattern | by Andrew Lock](https://andrewlock.net/series/working-with-the-result-pattern/)
* [Exceptions for flow control in C# · Enterprise Craftsmanship](https://enterprisecraftsmanship.com/posts/exceptions-for-flow-control/)
* [language agnostic - Why not use exceptions as regular flow of control? - Stack Overflow](https://stackoverflow.com/questions/729379/why-not-use-exceptions-as-regular-flow-of-control)
