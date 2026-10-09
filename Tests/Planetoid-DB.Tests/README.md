# Planetoid-DB.Tests

`Planetoid-DB.Tests` is the xUnit unit-test project of **Planetoid-DB**. It verifies the astronomical calculation layer located in [`Services`](../../Services/README.md).

## Purpose

The project tests the services without the WinForms application. Instead of referencing `Planetoid-DB.csproj` (which targets `net10.0-windows`), it compiles the required source files directly into the test assembly:

```xml
<Compile Include="..\..\Services\**\*.cs" Link="Services\%(RecursiveDir)%(Filename)%(Extension)" />
<Compile Include="..\..\Helpers\PlanetoidRecord.cs" Link="Helpers\PlanetoidRecord.cs" />
```

This allows the tests to target plain `net10.0` and run on Windows, Linux, and macOS.

## Project Settings

| Setting | Value |
|---|---|
| Target framework | `net10.0` |
| Root namespace | `Planetoid_DB.Tests` |
| Test framework | xUnit (`xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`) |
| Nullable reference types | enabled |
| Implicit usings | enabled, plus global `Xunit` and `Planetoid_DB.Services` |

## Test Classes

| File | Class | Description |
|---|---|---|
| `EphemerisTests.cs` | `EphemerisTests` | Unit tests for the ephemeris calculation: time grids, Julian dates, ΔT, continuity across midnight, time zones and daylight saving time, horizon/visibility, azimuth normalization, observer validation, MPCORB parsing, cancellation, culture-invariant CSV export, and comparison of Ceres with Minor Planet Center astrometric J2000 coordinates. |
| `JplDevelopmentEphemerisTests.cs` | `JplDevelopmentEphemerisTests` | Unit tests for reading JPL Development Ephemeris binary files, including extended headers, Earth position evaluation, and rejection of non-finite header values. |
| `OrbitPropagationTests.cs` | `OrbitPropagationTests` | Unit tests for orbit propagation input validation (non-finite semi-major axes and Julian dates, invalid orbital elements). |
| `TestData.cs` | `TestData` | Provides shared test data (e.g. MPCORB records) for the ephemeris tests. |

## Running the Tests

```bash
dotnet test Tests/Planetoid-DB.Tests/Planetoid-DB.Tests.csproj
```

or, for the whole solution:

```bash
dotnet test Planetoid-DB.slnx
```

## Adding New Tests

1. Add a new test class or extend an existing one matching the tested service.
2. Use `[Fact]` for single cases and `[Theory]` with `[InlineData]` for parameterized cases.
3. Put reusable input records into `TestData.cs`.
4. Keep tests deterministic and culture-independent; do not depend on network access or external files.
5. Add XML documentation comments in English, consistent with the rest of the project.

## Related Documentation

- [Main project README](../../README.md)
- [Tests](../README.md)
- [Services](../../Services/README.md)
- [Services/Models](../../Services/Models/README.md)
