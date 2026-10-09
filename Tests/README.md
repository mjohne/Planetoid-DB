# Tests

The `Tests` directory contains the automated test projects of **Planetoid-DB**.

## Purpose

The tests verify the UI-independent parts of the application, in particular the astronomical calculations in the [`Services`](../Services/README.md) directory. They are kept separate from the main WinForms project so that they can be built and executed on any platform supported by .NET, including the Linux runners used in CI.

## Test Projects

| Project | Framework | Description |
|---|---|---|
| [`Planetoid-DB.Tests`](./Planetoid-DB.Tests/README.md) | xUnit, `net10.0` | Unit tests for ephemeris calculation, orbit propagation, time scales, coordinate transformations, visibility, CSV export, and JPL DE file reading. |

## Running the Tests

From the repository root:

```bash
dotnet test Planetoid-DB.slnx
```

The test project is part of the solution file. The main project excludes the `Tests` folder from compilation (`<Compile Remove="Tests\**" />`), so test sources never end up in the application binary.

## File Organization

```text
Tests/
└── Planetoid-DB.Tests/
```

## Related Documentation

- [Main project README](../README.md)
- [Planetoid-DB.Tests](./Planetoid-DB.Tests/README.md)
- [Services](../Services/README.md)
- [CONTRIBUTING](../CONTRIBUTING.md)
