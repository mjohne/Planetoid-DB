# AGENTS.md — AI Agent Instructions for Planetoid-DB

This file provides guidance for AI coding agents (e.g., GitHub Copilot, Codex, Claude) working in this repository.

---

## Project Overview

**Planetoid-DB** is a Windows Forms (.NET / C#) desktop application that reads and visualises the Minor Planet Center Orbit Table (`MPCORB.DAT` / `MPCORB.JSON`). It lets users browse, filter, search, and analyse orbital data for hundreds of thousands of minor planets, asteroids, and comets.

- **Technology**: C# 12+, .NET (WinForms), NuGet packages
- **Target platform**: Windows (WinExe output type)
- **Current version**: see `<Version>` in `Planetoid-DB.csproj`
- **License**: GPL-3.0

---

## Repository Layout

```
/
├── Forms/          # WinForms form classes (.cs / .Designer.cs / .resx per form)
├── Helpers/        # Reusable helper/utility classes (calculators, exporters, …)
├── Properties/     # Assembly attributes, settings
├── Resources/      # Icons, images, embedded resources
├── docs/           # Additional documentation and GitHub Pages content
├── .github/
│   ├── workflows/  # GitHub Actions CI workflows
│   └── ISSUE_TEMPLATE/
├── CHANGELOG.md
├── CONTRIBUTING.md
├── ROADMAP.md
├── SECURITY.md
├── Planetoid-DB.csproj
└── Planetoid-DB.slnx
```

Key source files at the root level:

| File | Purpose |
|------|---------|
| `Program.cs` | Entry point |
| `PlanetoidDBForm.cs` (+ partials) | Main application form, split across several partial-class files |
| `I18nStrings.resx` / `.Designer.cs` | Localisation resource strings |
| `GlobalSuppressions.cs` | Roslyn analyser suppressions |

---

## Build

```bash
dotnet build Planetoid-DB.slnx
```

Or build the project directly:

```bash
dotnet build Planetoid-DB.csproj
```

The project targets Windows and requires the Windows Forms workload. Build on a non-Windows host will fail unless the appropriate SDK is available.

---

## Tests

```bash
dotnet test
```

Run this after any code change to verify nothing is broken. New functionality should be accompanied by tests where practical.

---

## Code Style & Conventions

Follow the existing `.editorconfig` rules and Roslyn analyser configuration already present in the project.

### Naming

| Identifier | Convention |
|------------|-----------|
| Public types and members | `PascalCase` |
| Local variables and parameters | `camelCase` |
| Private fields | `camelCase` (or `_camelCase` if already used in the file) |
| Constants | `PascalCase` |

### Documentation Comments

**All public and internal methods, constructors, fields, properties, and event handlers must have XML documentation comments written in English.** This is enforced by `<GenerateDocumentationFile>True</GenerateDocumentationFile>` in the project file.

```csharp
/// <summary>
/// Loads the MPCORB database from the specified file path.
/// </summary>
/// <param name="filePath">The full path to the MPCORB.DAT file.</param>
/// <returns><c>true</c> if the database was loaded successfully; otherwise, <c>false</c>.</returns>
public bool LoadDatabase(string filePath) { ... }
```

### General C# Guidelines

- Enable nullable reference types (`<Nullable>enable</Nullable>` is set). Always handle potential `null` values explicitly.
- Prefer `async`/`await` over blocking calls for I/O and network operations.
- Avoid unnecessary allocations and unintended side effects.
- No build warnings — the project may have `TreatWarningsAsErrors` enabled.
- Do not add `using` directives that are already covered by `<ImplicitUsings>enable</ImplicitUsings>`.

### WinForms Specifics

- UI updates must occur on the UI thread; use `Invoke`/`BeginInvoke` or `SynchronizationContext` when updating controls from background threads.
- Designer-generated code lives in `.Designer.cs` partial files — do **not** hand-edit these files.
- Resource strings for the UI live in `I18nStrings.resx`; reference them through the generated `I18nStrings` class.

---

## Commit Message Format

Use the conventional-commit style:

```
<type>: <short description>

<optional body>
```

Common types: `feat`, `fix`, `refactor`, `docs`, `test`, `chore`, `ci`, `perf`.

Examples:

```
fix: prevent NullReferenceException in MPCORB parser
feat: add JSON export for filtered dataset
refactor: simplify orbital resonance calculation
docs: update CONTRIBUTING with new branch naming rules
```

---

## Branch Naming

```
feature/<short-description>
bugfix/<short-description>
refactor/<short-description>
```

---

## Pull Requests

- Title should be clear and descriptive.
- Reference related issues with `Fixes #<number>` or `Closes #<number>`.
- The PR must build and all tests must pass before merging.
- Avoid unrelated formatting-only changes in a PR.

---

## Dependencies

- All dependencies are managed via **NuGet**.
- Adding new packages requires justification; prefer using existing libraries already referenced in the project.
- Run `dotnet restore` after modifying `Planetoid-DB.csproj`.

---

## Security

- Never commit secrets, credentials, API keys, or tokens.
- Report security vulnerabilities via GitHub Security Advisories — not through public issues. See [SECURITY.md](SECURITY.md).
- Follow secure coding practices and avoid introducing new vulnerabilities.

---

## Changelog

When contributing a notable change, add an entry to [CHANGELOG.md](CHANGELOG.md) under the current version section following the existing format:

```markdown
## Planetoid-DB <version>

* <Description of change> by @<author> in <PR link>
```

---

## Useful References

- [CONTRIBUTING.md](CONTRIBUTING.md) — full contributor guide
- [ROADMAP.md](ROADMAP.md) — planned features by version
- [SECURITY.md](SECURITY.md) — security policy
- [Minor Planet Center](https://www.minorplanetcenter.net/) — upstream data source
