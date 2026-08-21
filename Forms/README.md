# Forms

The `Forms` directory contains the Windows Forms user interface of **Planetoid-DB**. It brings together the main application window, data browsing and filtering dialogs, astronomical analysis tools, database maintenance functions, visualizations, export/print dialogs, and supporting information windows.

The forms are implemented as C# WinForms classes and are generally accompanied by a Visual Studio designer file (`*.Designer.cs`) and, where required, a resource file (`*.resx`). The directory also contains `BaseKryptonForm.cs`, which provides the common form foundation used by the application's themed dialogs.

## Purpose

The `Forms` layer is responsible for the presentation and user interaction of Planetoid-DB. It connects the application's controls and dialogs with the underlying data, astronomical calculations, database services, configuration, logging, and visualization components.

The forms can broadly be grouped into the following areas:

* **Application shell** – main window, splash screen, application information, settings, licensing, terminology and logging.
* **Database and data management** – database inspection, downloading, validation, comparison, archive management and record-oriented views.
* **Search and filtering** – object search, filtering, table modes and readable-designation lists.
* **Orbital analysis** – derived orbital elements, orbital-element grouping, Tisserand parameters, orbital resonances, MOID/MAOID-related analyses and asteroid-family analysis.
* **Visualization** – 2D/3D orbit views, 3D semi-major-axis/eccentricity/inclination diagrams, scatter plots and other analytical displays.
* **Observations and ephemerides** – observation browsing, bulk observation downloads, error reporting and ephemeris functions.
* **Output** – data-sheet printing and exporting.
* **Utilities and documentation** – application information, terminology, license, settings import/export and related dialogs.

## Main Forms

| Form                                      | Purpose                                                                                                     |
| ----------------------------------------- | ----------------------------------------------------------------------------------------------------------- |
| `AEIDiagram3DForm`                        | Displays a three-dimensional orbital-parameter diagram using semi-major axis, eccentricity and inclination. |
| `AppInfoForm`                             | Displays application information and project metadata.                                                      |
| `ArchiveMpcorbForm`                       | Works with archived MPCORB data.                                                                            |
| `AsteroidFamiliesForm`                    | Analyzes and displays asteroid-family information.                                                          |
| `AsteroidGameForm`                        | Provides an interactive asteroid-themed application/game view.                                              |
| `AverageAsteroidForm`                     | Calculates or displays average properties for asteroid data.                                                |
| `BaseKryptonForm`                         | Shared base class for the application's Krypton-based WinForms UI.                                          |
| `BulkObservationsDataDownloaderForm`      | Downloads observation data for multiple objects in bulk.                                                    |
| `BulkObservationsDownloadErrorsForm`      | Displays errors encountered during bulk observation downloads.                                              |
| `CheckDatabaseForm`                       | Checks the database/data source for consistency or validity.                                                |
| `DatabaseDifferencesForm`                 | Compares database versions or data sets and displays differences.                                           |
| `DatabaseDownloaderForm`                  | Downloads an astronomical catalogue/data file for use by Planetoid-DB.                                      |
| `DatabaseInformationForm`                 | Displays information and statistics about the current database/data source.                                 |
| `DerivedOrbitElementsForm`                | Displays and works with derived orbital elements and related calculations.                                  |
| `DistributionsForm`                       | Displays distributions of selected astronomical parameters.                                                 |
| `EphemerisForm`                           | Displays ephemeris-related calculations and information for minor planets.                                  |
| `ExportDataSheetForm`                     | Exports object data sheets to an external format.                                                           |
| `FilterForm`                              | Applies filtering criteria to the displayed data set.                                                       |
| `LicenseForm`                             | Displays license information.                                                                               |
| `ListReadableDesignationsForm`            | Lists readable / formatted object designations.                                                             |
| `LogViewerForm`                           | Displays application log information for diagnostics and troubleshooting.                                   |
| `MaxoidsOfAllMinorPlanetsForm`            | Processes MAOID/maximum-approach-related results for all applicable minor planets.                          |
| `MaxoidsOfOneMinorPlanetForm`             | Calculates and displays MAOID/maximum-approach-related results for one minor planet.                        |
| `MaxoidsRelativeToMinorPlanetsForm`       | Analyzes maximum-approach relationships relative to selected minor planets.                                 |
| `MoidsAndMaxoidsOfOneMinorPlanetForm`     | Combines MOID and maximum-approach analysis for one minor planet.                                           |
| `MoidsOfAllMinorPlanetsForm`              | Processes MOID-related results for all applicable minor planets.                                            |
| `MoidsOfOneMinorPlanetForm`               | Calculates and displays MOID-related results for one minor planet.                                          |
| `MoidsRelativeToMinorPlanetsForm`         | Analyzes MOID relationships relative to selected minor planets.                                             |
| `ObservationsForm`                        | Displays and manages observational data associated with objects.                                            |
| `ObservatoryCodesForm`                    | Provides access to MPC observatory codes and related information.                                           |
| `Orbit2DSideViewForm`                     | Provides a two-dimensional side view of an orbit.                                                           |
| `Orbit3DForm`                             | Provides a three-dimensional orbital visualization.                                                         |
| `OrbitalResonancesOfOneMinorPlanetForm`   | Analyzes orbital resonances for a single minor planet.                                                      |
| `OrbitalResonancesOfAllMinorPlanetsForm`  | Performs orbital-resonance analysis across the catalogue.                                                   |
| `OrbitElementsGroupingForm`               | Groups or categorizes objects according to orbital elements.                                                |
| `PlanetoidDBForm`                         | Main application window and central entry point for browsing and working with the planetoid catalogue.      |
| `PrintDataSheetForm`                      | Provides printing functionality for object data sheets.                                                     |
| `RecordsForm`                             | Displays catalogue records and related record information.                                                  |
| `RecordsTop10Form`                        | Presents top-10 style record statistics or rankings.                                                        |
| `ScatterplotsForm`                        | Displays scatter plots for investigating relationships between catalogue parameters.                        |
| `SearchForm`                              | Searches the loaded planetoid data.                                                                         |
| `SettingsExportForm`                      | Exports application settings.                                                                               |
| `SettingsForm`                            | Provides access to application settings and configuration.                                                  |
| `SettingsImportForm`                      | Imports saved application settings.                                                                         |
| `SplashScreenForm`                        | Startup / splash screen shown while the application is initialized.                                         |
| `TableModeForm`                           | Configures or presents an alternative table-oriented viewing mode.                                          |
| `TerminologyForm`                         | Provides terminology and explanatory information used by the application.                                   |
| `TisserandParameterOfAllMinorPlanetsForm` | Calculates/displays Tisserand parameters across the catalogue.                                              |
| `TisserandParameterOfOneMinorPlanetForm`  | Calculates/displays the Tisserand parameter for one minor planet.                                           |

## UI File Structure

A typical form consists of the following files:

```text
Forms/
├── ExampleForm.cs
├── ExampleForm.Designer.cs
└── ExampleForm.resx
```

### `*.cs`

Contains the application-specific behaviour of the form, including:

* event handlers
* data loading and processing
* user interaction
* validation
* calls to database and astronomy services
* orchestration of calculations and visualizations

### `*.Designer.cs`

Contains Visual Studio-generated control declarations and initialization code. These files should normally be edited through the WinForms Designer rather than manually.

### `*.resx`

Contains Windows Forms resources such as localized strings, icons, images and other designer-managed resources where required.

## Architectural Role

The forms form the **presentation layer** of Planetoid-DB. They should primarily coordinate user interaction and presentation rather than implement reusable domain logic directly.

Conceptually, the UI layer can be viewed as:

```text
                    Planetoid-DB
                         │
                         ▼
                  ┌──────────────┐
                  │    Forms     │
                  │  WinForms UI │
                  └──────┬───────┘
                         │
          ┌──────────────┼──────────────┐
          ▼              ▼              ▼
     Data / I/O     Astronomy      Configuration
          │          & Analysis        & Logging
          └──────────────┼──────────────┘
                         ▼
                   Domain / Models
```

This separation makes the UI easier to maintain while allowing the astronomical calculations and data-processing logic to be reused independently of the Windows Forms presentation layer.

## Navigation and Dependencies

Many forms are not standalone windows. They are opened from the main `PlanetoidDBForm` and often exchange a selected minor-planet record, filtering criteria, calculation parameters or application settings with the caller.

Typical interaction paths include:

```text
PlanetoidDBForm
    ├── SearchForm
    ├── FilterForm
    ├── RecordsForm
    ├── TableModeForm
    ├── DerivedOrbitElementsForm
    ├── Orbit3DForm
    ├── Orbit2DSideViewForm
    ├── AEIDiagram3DForm
    ├── EphemerisForm
    ├── ObservationsForm
    ├── ExportDataSheetForm
    └── PrintDataSheetForm
```

Database-oriented functions are centered around forms such as `DatabaseDownloaderForm`, `CheckDatabaseForm`, `DatabaseInformationForm`, `DatabaseDifferencesForm` and `ArchiveMpcorbForm`.

Astronomical analysis is distributed across specialized forms such as `AsteroidFamiliesForm`, `MoidsOfOneMinorPlanetForm`, `MoidsOfAllMinorPlanetsForm`, `MaxoidsOfOneMinorPlanetForm`, `OrbitalResonancesOfOneMinorPlanetForm`, `TisserandParameterOfOneMinorPlanetForm` and `OrbitElementsGroupingForm`.

## Development Guidelines

When adding a new form to this directory:

1. Derive from the appropriate base form, preferably `BaseKryptonForm` where the existing UI architecture requires it.
2. Keep UI event handling in the form and move reusable calculations/business rules into the appropriate non-UI classes.
3. Avoid modifying `*.Designer.cs` manually unless there is a specific technical reason.
4. Use consistent naming: `SomethingForm.cs`, `SomethingForm.Designer.cs` and `SomethingForm.resx`.
5. Pass domain objects or clearly defined parameters into forms instead of coupling forms to global state.
6. Support cancellation and asynchronous execution for long-running operations where appropriate so the UI remains responsive.
7. Reuse shared logging, settings and validation infrastructure instead of implementing local variants in individual forms.

## Related Documentation

* [Main project README](../README.md)
* [Project repository](https://github.com/mjohne/Planetoid-DB)
* [Forms directory](https://github.com/mjohne/Planetoid-DB/tree/main/Forms)
