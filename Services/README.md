# Services

The `Services` directory contains the UI-independent calculation layer of **Planetoid-DB**. It provides the astronomical services used to compute topocentric ephemerides of minor planets from MPCORB orbital elements, including time-scale conversions, orbit propagation, coordinate transformations, visibility evaluation, and CSV export of the results.

All classes belong to the `Planetoid_DB.Services` namespace. The data types they exchange are located in the [`Models`](./Models/README.md) subdirectory.

## Purpose

The service layer separates numerical astronomy from the Windows Forms user interface.

Forms such as `EphemerisForm` collect user input (object, observer location, time range, options) and delegate the actual calculation to the services. Because the services do not depend on WinForms, they are also compiled directly into the test project (`Tests/Planetoid-DB.Tests`) and can be tested on any platform.

## Available Services

| Class | Kind | Description |
|---|---|---|
| `AnalyticalPlanetaryEphemeris` | `sealed class` | Provides approximate planetary positions from the JPL mean orbital elements. Used as a fallback when no JPL DE file is available. |
| `AstronomicalConstants` | `static class` | Provides astronomical constants (e.g. AU, speed of light, gravitational parameters) used by the ephemeris services. |
| `CoordinateTransformationService` | `static class` | Provides astronomical coordinate transformations (precession, nutation, aberration, equatorial/horizontal conversion, refraction). |
| `EphemerisExportService` | `static class` | Exports ephemerides in a culture-independent CSV format. |
| `EphemerisService` | `sealed class` | Calculates topocentric ephemerides (RA/Dec, azimuth/altitude, brightness, visibility) of minor planets. |
| `IPlanetaryEphemerisProvider` | `interface` | Defines a source of positions of the major solar system bodies. |
| `JplDevelopmentEphemeris` | `sealed class` | Reads JPL Development Ephemeris binary files (e.g. DE440, DE441). |
| `MpcorbElementsParser` | `static class` | Converts MPCORB records into numerical orbital elements. |
| `OrbitPropagationService` | `sealed class` | Propagates the orbit of a minor planet (two-body or with planetary perturbations). |
| `TimeScales` | `static class` | Provides conversions between UTC, TT, TDB and Julian dates, including ΔT. |
| `VisibilityCalculator` | `static class` | Computes brightness and visibility of a minor planet. |

## Architecture

```text
                    EphemerisForm (UI)
                           │
                           ▼
                    EphemerisService
                           │
     ┌──────────────┬──────┴───────┬──────────────────────┐
     │              │              │                      │
     ▼              ▼              ▼                      ▼
 TimeScales  OrbitPropagation  CoordinateTransfor-  VisibilityCalculator
                 Service        mationService
                    │
                    ▼
        IPlanetaryEphemerisProvider
          ┌─────────┴──────────┐
          ▼                    ▼
 JplDevelopmentEphemeris  AnalyticalPlanetaryEphemeris
     (DE440 / DE441)        (mean elements fallback)
```

`MpcorbElementsParser` converts the input record into `MinorPlanetOrbitalElements`, and `EphemerisExportService` writes the resulting `EphemerisEntry` list to CSV.

## Calculation Pipeline

For every time point, `EphemerisService` performs the following steps:

1. Conversion of UTC to TT/TDB (`TimeScales`).
2. Numerical propagation of the MPCORB osculating elements, optionally including planetary perturbations and a relativistic correction (`OrbitPropagationService`).
3. Light-time correction.
4. Topocentric parallax based on the WGS84 observer location.
5. Annual and diurnal aberration.
6. Precession (IAU 1976) and nutation (IAU 1980) to the true equator of date.
7. Conversion to horizontal coordinates and optional atmospheric refraction.
8. Brightness and visibility evaluation (`VisibilityCalculator`).

Both the apparent place (true equator and equinox of date) and the astrometric place (ICRF/J2000, as used by the MPC ephemeris service) are returned. All input and output times are UTC.

## Planetary Ephemeris Providers

`IPlanetaryEphemerisProvider` abstracts the source of the major-body positions required for perturbations and the observer's heliocentric position:

- `JplDevelopmentEphemeris` reads JPL DE binary files and evaluates the Chebyshev coefficients for high accuracy.
- `AnalyticalPlanetaryEphemeris` uses JPL mean orbital elements and is available without any external file, at reduced accuracy.

## Design Principles

1. Services are independent of WinForms and must not reference controls or forms.
2. Calculations are culture-invariant; formatting for display is done in the UI layer.
3. Input values are validated at the service boundary (e.g. non-finite values, invalid ranges).
4. Long-running calculations support cancellation via `CancellationToken`.
5. Data exchanged between services is represented by immutable records in `Services/Models`.
6. New or changed behaviour should be covered by tests in `Tests/Planetoid-DB.Tests`.

## File Organization

```text
Services/
├── AnalyticalPlanetaryEphemeris.cs
├── AstronomicalConstants.cs
├── CoordinateTransformationService.cs
├── EphemerisExportService.cs
├── EphemerisService.cs
├── IPlanetaryEphemerisProvider.cs
├── JplDevelopmentEphemeris.cs
├── MpcorbElementsParser.cs
├── OrbitPropagationService.cs
├── TimeScales.cs
├── VisibilityCalculator.cs
└── Models/
```

## Related Documentation

- [Main project README](../README.md)
- [Services/Models](./Models/README.md)
- [Forms](../Forms/README.md)
- [Helpers](../Helpers/README.md)
- [Tests](../Tests/README.md)
