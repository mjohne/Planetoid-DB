# Helpers

The `Helpers` directory contains reusable support classes and utility components used throughout **Planetoid-DB**.

It forms a cross-cutting support layer between the application's user interface, astronomical calculations, data models, import/export functionality, logging, configuration, downloads, and Windows-specific UI functionality.

The directory deliberately contains small, focused building blocks as well as several larger domain-oriented helper classes such as orbital-element derivation, MOID/MAOID calculation, Tisserand-parameter calculation, and `ListView` export functionality.

## Purpose

The `Helpers` layer provides functionality that is required by multiple parts of the application but does not belong exclusively to a single form or export format.

Typical responsibilities include:

- astronomical and orbital calculations
- application data models
- averages and statistical calculations
- MOID and MAOID calculations
- orbital-element derivation
- Tisserand parameters
- bookmarks
- import/export support
- WinForms control helpers
- `ListView` sorting and exporting
- download progress reporting
- taskbar progress integration
- logging data transfer and storage
- application settings import/export
- assembly metadata
- common UI performance helpers

The current repository contains helper classes such as `AverageCalculator`, `BookmarkEntry`, `BookmarkStore`, `DerivedElements`, `DoubleBufferingHelper`, `DownloadProgressInfo`, `ExportEscapeHelper`, `ExportFeedbackHelper`, `IOrbitDataExporter`, `ListViewExporter`, `ListViewItemComparer`, `LogEventDto`, `LogEventStore`, `LogEventTarget`, `MaxoidCalculator`, `MoidCalculator`, `PlanetoidRecord`, `SettingsExporter`, `SettingsImporter`, `TaskbarProgress`, `TextBoxExporter`, and `TisserandParameterCalculator`.

## Architecture

The helper layer can be viewed as a collection of reusable services and support objects:

```text
                         Planetoid-DB
                              │
             ┌────────────────┼────────────────┐
             │                │                │
             ▼                ▼                ▼
           Forms           Export           Models
             │                │                │
             └────────────────┼────────────────┘
                              ▼
                         ┌───────────┐
                         │  Helpers  │
                         └─────┬─────┘
                               │
       ┌───────────────┬───────┼───────────┬───────────────┐
       ▼               ▼       ▼           ▼               ▼
  Astronomy/Data      UI    Export      Logging        Utilities
```

The helpers therefore do not represent one single subsystem. They are a collection of reusable components that support several application layers.

## Helper Categories

### 1. Astronomical and Orbital Calculations

Several helper classes implement calculations that are central to Planetoid-DB's astronomical functionality.

| Class | Responsibility |
|---|---|
| `DerivedElements` | Derives additional orbital quantities from the available orbital elements. |
| `MoidCalculator` | Calculates the Minimum Orbit Intersection Distance (MOID). |
| `MaxoidCalculator` | Calculates maximum-orbit-approach / MAOID-related quantities. |
| `TisserandParameterCalculator` | Calculates Tisserand parameters for orbital analysis. |

These classes allow the calculation logic to be reused by several forms instead of embedding the algorithms directly in the WinForms layer.

### 2. Data and Domain Objects

`PlanetoidRecord` represents the application-side data structure for a planetoid/minor-planet record.

It acts as an important data-transfer/model object between catalogue data, UI components, calculations, and export functionality. fileciteturn10file9

### 3. Statistical Calculations

`AverageCalculator` provides reusable average/statistical calculations used by the application.

Keeping these calculations in a helper class avoids duplicating statistical logic in individual forms.

### 4. Bookmarks

The bookmark subsystem consists primarily of:

```text
BookmarkEntry
      │
      ▼
BookmarkStore
```

`BookmarkEntry` represents an individual bookmark, while `BookmarkStore` manages bookmark persistence and access.

This keeps bookmark handling separate from the forms that display or use bookmarks.

### 5. Export Infrastructure

The helper layer contains the abstraction used by the format-specific exporters:

```text
IOrbitDataExporter
        │
        ├── CSV
        ├── JSON
        ├── XML
        ├── HTML
        ├── Markdown
        └── other formats
```

`IOrbitDataExporter` defines the common contract between the application and the classes in the `Export` directory.

Additional helper classes support escaping and user feedback during export operations:

- `ExportEscapeHelper`
- `ExportFeedbackHelper`

The actual format-specific exporters reside in the separate `Export` directory.

### 6. WinForms and ListView Utilities

The application makes extensive use of WinForms controls. Several helpers centralize common control-related operations:

| Class | Responsibility |
|---|---|
| `ListViewExporter` | Exports `ListView` content and provides extensive support for converting displayed tabular data into output formats. |
| `ListViewExporter.NewRecord` | Contains additional `ListViewExporter` functionality related to record creation/output. |
| `ListViewItemComparer` | Provides sorting/comparison functionality for `ListViewItem` instances. |
| `TextBoxExporter` | Exports text-based control content. |
| `DoubleBufferingHelper` | Enables/improves double-buffered rendering for controls to reduce visual flicker. |

The `ListViewExporter` implementation is one of the larger helper components in the project.

### 7. Download and Progress Reporting

`DownloadProgressInfo` provides a data structure for transferring download-progress information between asynchronous download operations and the UI.

This is particularly useful for operations where a catalogue or observation data is downloaded while a WinForms progress indicator is updated. fileciteturn10file5

`TaskbarProgress` extends progress feedback to the Windows taskbar, allowing long-running operations to expose their current state outside the application window.

### 8. Logging

The helper layer contains several classes supporting application logging:

```text
LogEventDto
     │
     ├── LogEventStore
     │
     └── LogEventTarget
```

These classes provide data-transfer, storage, and target functionality around application log events.

This allows the application's logging UI and other components to consume structured log information without directly depending on the underlying logging implementation.

### 9. Application Settings

The following helpers support settings persistence:

- `SettingsExporter`
- `SettingsImporter`

They provide the non-UI implementation used by the corresponding settings forms.

This keeps serialization/import logic out of the WinForms layer.

### 10. Assembly Information

`AssemblyInfo.cs` contains assembly-level metadata and attributes associated with the application.

It is infrastructure rather than application-domain logic and is therefore kept with the project's other helper/support components.

## Key Classes

### `PlanetoidRecord`

`PlanetoidRecord` is the central record-oriented helper/model type for representing a minor planet in the application.

It is used wherever catalogue records need to be passed between different application components, including UI, analysis, and output functionality.

### `DerivedElements`

`DerivedElements` encapsulates calculations for secondary orbital quantities derived from the basic orbital elements.

This is particularly useful because derived quantities can be calculated once in a dedicated component and then consumed by different analysis forms.

### `MoidCalculator`

`MoidCalculator` contains the application's MOID calculation functionality.

MOID analysis is an important part of minor-planet orbital analysis because it provides a measure of the minimum spatial separation between two osculating orbits.

### `MaxoidCalculator`

`MaxoidCalculator` provides the corresponding maximum-approach/MAOID-oriented calculations used by the application's Maxoid analysis forms.

### `TisserandParameterCalculator`

`TisserandParameterCalculator` encapsulates the calculation of the Tisserand parameter, allowing the corresponding analysis to remain independent of the UI.

## Export Interface

The export interface is intentionally located in `Helpers` rather than `Export` because it represents the contract shared by the application and all concrete exporters.

```csharp
public interface IOrbitDataExporter
{
    string Extension { get; }

    string Filter { get; }

    string Title { get; }

    void Export(
        string filePath,
        string exportTitle,
        Dictionary<string, string> selectedData);
}
```

A form or other caller can therefore depend on `IOrbitDataExporter` without knowing which concrete format is being used.

## Design Principles

The helper layer follows several architectural principles:

- **Reuse** – common functionality is implemented once and shared.
- **Separation of concerns** – UI code is separated from calculations, persistence, and utility operations.
- **Single responsibility** – individual helpers generally focus on a specific technical or domain task.
- **Loose coupling** – interfaces such as `IOrbitDataExporter` allow components to depend on abstractions.
- **Testability** – calculations can be isolated from the WinForms presentation layer.
- **Extensibility** – new calculations or utility functionality can be added without embedding them into existing forms.
- **UI responsiveness** – progress and asynchronous-operation support can be communicated through dedicated data structures.

## Relationship to Other Directories

The `Helpers` directory works closely with several other areas of the project:

```text
Planetoid-DB/
│
├── Forms/
│   └── WinForms presentation layer
│
├── Export/
│   └── Format-specific exporters
│
├── Helpers/
│   └── Shared support and domain utilities
│
├── Models/
│   └── Domain/data structures
│
└── ...
```

In particular:

- `Forms` consumes helper functionality for calculations, settings, bookmarks, downloads, logging, and UI operations.
- `Export` implements `IOrbitDataExporter`.
- Astronomical analysis forms use calculation helpers such as `MoidCalculator`, `MaxoidCalculator`, `DerivedElements`, and `TisserandParameterCalculator`.
- Settings forms use `SettingsImporter` and `SettingsExporter`.
- Logging-related forms use the structured logging helpers.

## Adding a New Helper

When adding a new helper class, consider the following guidelines:

1. Place the class in the `Planetoid_DB.Helpers` namespace.
2. Give the class a focused responsibility.
3. Keep reusable logic independent of WinForms whenever possible.
4. Avoid accessing controls directly from calculation or domain helpers.
5. Use interfaces where multiple implementations are expected.
6. Keep long-running operations cancellable where practical.
7. Reuse the existing logging and configuration infrastructure.
8. Add XML documentation for public APIs where the class is intended for broader reuse.
9. Prefer immutable/value-oriented data structures for progress and calculation results where appropriate.
10. Update this README when a significant new helper subsystem is introduced.

## File Organization

The directory currently contains a mixture of:

```text
Helpers/
├── Calculation helpers
├── Domain/data objects
├── Bookmark infrastructure
├── Export infrastructure
├── ListView/TextBox helpers
├── Download/progress helpers
├── Logging helpers
├── Settings import/export
├── Windows UI helpers
└── Assembly metadata
```

This reflects the role of `Helpers` as a shared infrastructure layer rather than a single functional module.

## Related Documentation

- [Main project README](../README.md)
- [Forms](../Forms/README.md)
- [Export](../Export/README.md)
- [Helpers directory](https://github.com/mjohne/Planetoid-DB/tree/main/Helpers)
- [IOrbitDataExporter](./IOrbitDataExporter.cs)
