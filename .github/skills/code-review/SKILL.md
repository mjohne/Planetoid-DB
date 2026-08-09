---
name: code-review
description: Review C#/.NET WinForms code changes in Planetoid-DB for correctness, maintainability, and consistency with the project's conventions. Use this skill whenever reviewing a pull request, diff, or set of changed files in this repository — including form code-behind, data access, MPCORB parsing, caching, and background/async work.
---

# Code Review — Planetoid-DB

You are reviewing changes to **Planetoid-DB**, a C# WinForms application for working with asteroid data
from the Minor Planet Center's MPCORB database (orbital elements, family classification, archive
compression, AppData-based caching/preferences).

## Review process

1. **Understand the change first.** Read the diff plus enough surrounding context (the containing
   class/form, related data models) to know *why* the change was made before judging *how*.
2. **Work top-down.** Flag correctness and design issues before style nits. Don't bury a real bug in a
   list of formatting comments.
3. **Be concrete.** Point to the exact file/line, explain the risk or benefit, and suggest a fix — not
   just "this could be better."
4. **Distinguish severity.** Label feedback as **Blocking** (bug, data loss, crash, security), **Should
   fix** (maintainability, correctness edge case), or **Nit** (style, naming, minor).

## What to check

### Correctness
- Numeric parsing of MPCORB fields (packed designations, epoch, orbital elements) — watch for culture-
  specific parsing bugs (`double.Parse` without `CultureInfo.InvariantCulture` is a common WinForms trap
  on non-English systems, and this project spans German/English users).
- Off-by-one or boundary errors in orbital element calculations, family/cluster detection thresholds,
  and MOID-style distance comparisons.
- Null/empty handling for optional MPCORB fields and malformed or truncated archive/catalog rows.
- Exceptions from file I/O (archive decompression, AppData reads/writes) — are failures caught and
  surfaced to the user, or do they crash the UI thread?

### WinForms-specific
- Cross-thread UI updates: any background work (downloads, decompression, family detection) touching
  controls must marshal back via `Invoke`/`BeginInvoke` or use `IProgress<T>`/`async`-`await` correctly.
- Event handler wiring: no duplicate subscriptions on repeated form loads; handlers unsubscribed in
  `Dispose`/`FormClosing` where needed to avoid leaks.
- Long-running work on the UI thread (large MPCORB file parsing, big archive operations) should be
  offloaded with `Task.Run`/async, with the UI kept responsive (progress bar, cursor, disabled controls).
- Designer-generated code (`*.Designer.cs`) should not contain hand-written logic — flag if it does.

### Data & file handling
- AppData paths built via `Environment.GetFolderPath(SpecialFolder.ApplicationData)` (or equivalent),
  not hardcoded paths — check consistency with the project's existing AppData directory structure.
- Archive compression/decompression: streams disposed (`using`), partial writes cleaned up on failure,
  no silent overwrite of existing user data/caches.
- Caching logic: cache invalidation is correct (stale data won't silently persist); concurrent
  read/write to the same cache file is safe.

### Maintainability & consistency
- New code follows existing naming/style conventions already present in the file/project.
- Magic numbers (thresholds for family classification, proper-element cutoffs) are named constants with
  a comment on their origin (e.g., citing AstDyS/MPC convention) rather than bare literals.
- No obvious duplication of logic that already exists elsewhere in the codebase.
- Public methods/classes have enough doc comments for a future maintainer to understand intent, not just
  behavior.

### Tests (if present)
- New logic (especially orbital element math, parsing) has at least a basic unit test covering a normal
  case and one edge case.
- Existing tests aren't weakened (assertions loosened, tests skipped) to make the change pass.

## Output format

Structure the review as:

```
## Summary
One or two sentences on what the change does and overall assessment.

## Blocking
- [file:line] issue — why it matters — suggested fix

## Should fix
- [file:line] issue — why it matters — suggested fix

## Nits
- [file:line] issue

## Positive notes (optional)
Call out good patterns worth keeping/reusing, if any.
```

If there is nothing to flag in a category, omit it rather than writing "None."
