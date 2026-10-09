# Resources/TerminologyDocs

The `Resources/TerminologyDocs` directory contains the HTML documents explaining the orbital elements and other terms displayed in **Planetoid-DB**.

## Purpose

When the user requests an explanation of a value (for example from the main form or a data sheet), the `TerminologyForm` shows the matching document in an embedded web browser. This provides context-sensitive help without an internet connection.

## How It Works

1. HTML files that are available through `TerminologyForm` are registered as file-based string resources in `I18nStrings.resx` (e.g. `Resources\TerminologyDocs\terminology_SemiMajorAxis.html`).
2. The generated `I18nStrings` class exposes each registered document as a string property, e.g. `I18nStrings.terminology_SemiMajorAxis`.
3. `TerminologyForm` builds the resource key `terminology_<Element>` and loads the document; if no matching document exists, it falls back to `terminology_IndexNumber`.

## Naming Convention

```text
terminology_<ElementName>.html
```

To load a document through `TerminologyForm`, `<ElementName>` must be written in PascalCase and match an element name used by the form.

## Documents in This Directory

| Category | Documents |
|---|---|
| Identification | `IndexNumber`, `ReadableDesignation`, `Reference`, `Flags`, `ComputerName` |
| Orbital elements | `Epoch`, `MeanAnomalyAtTheEpoch`, `ArgumentOfThePerihelion`, `LongitudeOfTheAscendingNode`, `InclinationToTheEcliptic`, `OrbitalEccentricity`, `MeanDailyMotion`, `SemiMajorAxis` |
| Physical parameters | `AbsoluteMagnitude`, `SlopeParameter` |
| Observation data | `NumberOfObservations`, `NumberOfOppositions`, `ObservationSpan`, `DateOfLastObservation`, `RmsResidual` |
| Derived orbit geometry | `AphelionDistance`, `PerihelionDistance`, `ArgumentOfTheAphelion`, `LongitudeOfTheDescendingNode`, `MajorAxis`, `MinorAxis`, `SemiMinorAxis`, `MeanAxis`, `SemiMeanAxis`, `LinearEccentricity`, `LatusRectum`, `SemiLatusRectum`, `FocalParameter`, `Directrix`, `OrbitalArea`, `OrbitalPerimeter` |
| Anomalies and periods | `EccentricAnomaly`, `TrueAnomaly`, `OrbitalPeriod`, `StandardGravitationalParameter` |
| Orbital speeds | `AphelionOrbitalSpeed`, `PerihelionOrbitalSpeed`, `MajorOrbitalSpeed`, `MinorOrbitalSpeed`, `MeanOrbitalSpeed` |

Each entry corresponds to a file `terminology_<Name>.html`. Only documents registered in `I18nStrings.resx` and represented by an element in `TerminologyForm` can be loaded by the form.

## Adding a New Document

1. Create `terminology_<ElementName>.html` in this directory.
2. Add it as a file resource to `I18nStrings.resx` with the same key.
3. Make sure `TerminologyForm` is opened with the matching element name.
4. Keep the HTML self-contained (no external scripts or remote resources).

## Related Documentation

- [Main project README](../../README.md)
- [Resources](../README.md)
- [Forms](../../Forms/README.md)
