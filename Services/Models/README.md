# Services/Models

The `Services/Models` directory contains the data types used by the [service layer](../README.md) of **Planetoid-DB**. They describe inputs, options, intermediate vectors, and results of the ephemeris calculation.

## Purpose

The models decouple the calculation services from each other and from the user interface. They are small, immutable value objects (`record` / `readonly record struct`) or enumerations without UI dependencies.

## Available Models

| Type | Kind | Description |
|---|---|---|
| `EphemerisEntry` | `sealed record` | Represents the calculated position of a minor planet at a specific time (apparent and astrometric J2000 coordinates, horizontal coordinates, distances, brightness, visibility). |
| `EphemerisOptions` | `sealed record` | Represents options controlling the physical model of an ephemeris calculation (e.g. perturbations, refraction). |
| `Matrix3d` | `readonly record struct` | Represents a 3×3 rotation matrix used for coordinate frame transformations. |
| `MinorPlanetOrbitalElements` | `sealed record` | Represents the osculating orbital elements of a minor planet. |
| `ObserverLocation` | `sealed record` | Represents a geodetic observer location on the Earth (WGS84). |
| `SolarSystemBody` | `enum` | Enumerates the major solar system bodies used by the planetary ephemeris providers. |
| `StateVector` | `readonly record struct` | Represents a heliocentric state vector (position and velocity) in the ICRF/J2000 equatorial frame. |
| `Vector3d` | `readonly record struct` | Represents a three-dimensional Cartesian vector with double precision. |
| `VisibilityCriteria` | `sealed record` | Represents the criteria used to decide whether a minor planet is observable. |

## Data Flow

```text
MPCORB record ──► MpcorbElementsParser ──► MinorPlanetOrbitalElements
                                                     │
ObserverLocation, EphemerisOptions,                  ▼
VisibilityCriteria ───────────────────────►  EphemerisService
                                                     │
                       Vector3d / Matrix3d / StateVector (internal math)
                                                     │
                                                     ▼
                                        IReadOnlyList<EphemerisEntry>
```

## Coordinate Conventions

- `StateVector` and `Vector3d` positions are expressed in astronomical units (AU) in the ICRF/J2000 equatorial frame unless stated otherwise.
- `EphemerisEntry` contains both the apparent place (true equator and equinox of date) and the astrometric place (J2000). Only the astrometric fields are directly comparable with the Minor Planet Center ephemeris service.
- `ObserverLocation` uses geodetic longitude, latitude, and height on the WGS84 ellipsoid.

## Design Principles

1. Models are immutable and free of UI dependencies.
2. Validation of user-supplied values happens when a model is created or consumed by a service.
3. Models belong to the `Planetoid_DB.Services` namespace together with the services.
4. Any change to a model should be reflected in the related tests in `Tests/Planetoid-DB.Tests`.

## Related Documentation

- [Main project README](../../README.md)
- [Services](../README.md)
- [Tests](../../Tests/README.md)
