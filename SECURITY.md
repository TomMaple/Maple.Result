# Security policy

## Supported versions

Only the latest major version receives security fixes.

| Version | Supported |
|---------|-----------|
| 2.8.x   | ✅ (until 3.0.0 is released) |
| < 2.8   | ❌ |

Once 3.0.0 is released, only 3.x is supported. Fixes are released as a new patch or minor version of the latest major
version; older versions are not patched.

## Reporting a vulnerability

**Do not report vulnerabilities in public issues, discussions or pull requests.**

Report them privately through GitHub:
[**Report a vulnerability**](https://github.com/TomMaple/Maple.Result/security/advisories/new) (the *Security* tab →
*Advisories* → *Report a vulnerability*). Only the maintainer can see the report.

Please include:

- the affected package version(s) and the .NET version;
- a description of the vulnerability and its impact (e.g. denial of service through a crafted input to an `ErrorUri`
  factory, or data from one `Error` leaking into another);
- a minimal reproduction: code, input values and the expected and actual behaviour;
- any known workaround, and whether the issue is already public.

## What to expect

This is a project maintained by one person in their spare time, so the times below are a best effort:

- an acknowledgement within 7 days;
- an assessment (confirmed or not, and the planned fix) within 30 days;
- a fix, a GitHub security advisory and, where it applies, a CVE once the fixed version is on NuGet. You are credited
  in the advisory unless you prefer otherwise.

Please keep the report confidential until the advisory is published.
