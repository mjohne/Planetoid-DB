# Contributing Guide (.NET / C#)

Thank you for your interest in contributing to this C#/.NET project. Community contributions are very welcome and play a key role in improving the quality, stability, and maintainability of the software.

This document defines binding guidelines and recommended practices for contributions in a C# and .NET environment. Please read it carefully before creating an issue or submitting a pull request.

---

## Table of Contents

- Code of Conduct
- Prerequisites
- Types of Contributions
- Project Structure
- Reporting Issues
- Feature Proposals
- Development Workflow
- Coding Standards (C#)
- Commits & Commit Messages
- Pull Requests
- Testing & Quality Assurance
- Documentation
- Build, CI & Dependencies
- License

---

## Code of Conduct

All contributors are expected to interact in a respectful, constructive, and professional manner. Feedback should be factual and solution-oriented.

Discriminatory, offensive, or inappropriate behavior will not be tolerated. Maintainers reserve the right to moderate discussions or reject contributions to ensure a productive working environment.

---

## Prerequisites

Contributing to this project typically requires:

- A current .NET SDK version (see `global.json` or README)
- An IDE such as Visual Studio, Rider, or VS Code
- Basic knowledge of C#, .NET, and the `dotnet` CLI
- Optional: GitHub CLI for working with pull requests

---

## Types of Contributions

The following types of contributions are welcome:

- Bug fixes
- New features or enhancements
- Refactorings (structure, readability, performance)
- Documentation (README, XML comments, examples)
- Unit, integration, or end-to-end tests
- Build, CI, or tooling improvements

Small changes such as typo fixes, naming corrections, or comments are also appreciated.

---

## Project Structure

Please familiarize yourself with the existing solution (`.sln`) structure. Typical conventions include:

- `src/` – production code
- `tests/` – test projects (xUnit, NUnit, MSTest)
- `docs/` – additional documentation
- `build/` or `.github/` – build and CI configuration

General principles:

- Clear separation of production code and tests
- One primary responsibility per project/assembly
- No dead code or unused references

---

## Reporting Issues

When reporting a bug or technical issue:

1. Check whether a similar issue already exists.
2. Create a new issue with a clear technical description.
3. Provide, if possible:
   - Expected behavior
   - Actual behavior
   - Steps to reproduce
   - Relevant stack traces or logs
   - .NET version, OS, and project version

Incomplete or non-reproducible issues may be closed.

---

## Feature Proposals

Feature requests should be technically sound and well motivated:

- What problem does the feature solve?
- Why is it beneficial to the project?
- Are there existing alternatives or workarounds?
- Optional: outline a possible implementation

Final decisions are made by the maintainers.

---

## Development Workflow

Recommended workflow:

1. Fork the repository
2. Create a new branch from the current main branch
3. Implement changes in small, logical commits
4. Run tests locally (`dotnet test`)
5. Open a pull request against the main branch

Recommended branch naming:

- `feature/<short-description>`
- `bugfix/<short-description>`
- `refactor/<short-description>`

---

## Coding Standards (C#)

Please follow established .NET and C# conventions:

- PascalCase for public types and members
- camelCase for local variables and parameters
- Clear separation between public API and internal logic
- Prefer `async`/`await` over blocking code
- Avoid unnecessary allocations and side effects

Additionally:

- XML documentation comments for public APIs
- No build warnings (especially if `TreatWarningsAsErrors` is enabled)
- Compliance with existing `.editorconfig` and analyzer rules

---

## Commits & Commit Messages

Commits should be:

- Small, focused, and logically complete
- Free of mixed refactoring and feature logic where possible

Recommended commit message format:

```
<type>: <short description>

<optional detailed description>
```

Examples:

- `fix: prevent NullReferenceException in parser`
- `feat: add JSON export support`
- `refactor: simplify validation logic`

---

## Pull Requests

Pull requests must:

- Have a clear title and technical description
- Reference related issues (e.g. `Fixes #123`)
- Build and test successfully
- Avoid unnecessary formatting-only changes

Maintainers may request changes or reject pull requests if quality or design requirements are not met.

---

## Testing & Quality Assurance

New functionality should be covered by appropriate tests.

Please ensure:

- `dotnet test` runs successfully locally
- New tests are clearly named and understandable
- Existing tests are not removed without good reason

Code coverage is desirable but not the sole quality metric.

---

## Documentation

Public APIs and relevant behavior changes must be documented:

- README files
- XML documentation comments
- Changelogs (if applicable)

---

## Build, CI & Dependencies

- Dependencies are managed via NuGet
- New packages must be well justified
- CI pipelines must pass successfully
- Changes to build or CI scripts must be clearly documented

---

## License

By submitting a contribution, you agree that your code will be published under the license specified in the repository.

---

Thank you for contributing to the continued improvement of this C#/.NET project.
