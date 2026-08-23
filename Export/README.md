# Export

The `Export` directory contains the export layer of **Planetoid-DB**. It provides format-specific exporters for writing selected orbit/database information to a variety of common text, markup, data-interchange, and calendar-oriented formats.

The exporters implement the common `IOrbitDataExporter` interface defined in `Helpers/IOrbitDataExporter.cs`. This gives the application a consistent way to discover an export format, populate file-dialog metadata, and execute the actual export operation.

## Purpose

The export layer separates output formatting from the user interface and from the underlying astronomy/data-processing logic.

Instead of having individual forms implement CSV, JSON, XML, HTML, or other formatting rules themselves, Planetoid-DB can pass the selected data to an appropriate exporter.

The common export contract is:

```text
IOrbitDataExporter
├── Extension
├── Filter
├── Title
└── Export(filePath, exportTitle, selectedData)
```

`selectedData` is represented by a `Dictionary<string, string>` containing the selected key/value data to be written to the target file.

## Available Exporters

| Class | Format | Typical extension | Description |
|---|---|---:|---|
| `BbcodeExporter` | BBCode | `.bbcode` | Exports data using BBCode-style markup suitable for forums and systems supporting BBCode. |
| `CreoleExporter` | Creole Wiki Markup | `.creole` | Produces wiki markup based on the Creole syntax. |
| `CsvExporter` | CSV | `.csv` | Exports selected data in a delimited text format suitable for spreadsheets and data-processing tools. |
| `ExcelExporter` | Excel | `.xlsx` | ... |
| `HtmlExporter` | HTML | `.html` | Generates a standalone HTML document containing the selected data and basic presentation markup. |
| `IcsExporter` | iCalendar | `.ics` | Exports relevant information in iCalendar format for use with calendar applications and compatible tools. |
| `JsonExporter` | JSON | `.json` | Serializes selected data into JSON for machine-readable interchange. |
| `LatexExporter` | LaTeX | `.tex` | Produces LaTeX-compatible output for scientific and technical documents. |
| `MarkdownExporter` | Markdown | `.md` | Generates Markdown suitable for documentation, GitHub, notes, and other Markdown-compatible systems. |
| `OdsExporter` | Open Document Type | `.ods` | ... |
| `OdtExporter` | Open Document Type | `.odt` | ... |
| `PsvExporter` | Pipe-Separated Values | `.psv` | Writes data using the pipe (`|`) character as the field separator. |
| `RtfExporter` | Rich Text Format | `.rtf` | ... |
| `TextExporter` | Plain text | `.txt` | Produces a human-readable plain-text representation of the selected data. |
| `TsvExporter` | Tab-Separated Values | `.tsv` | Writes data with tab characters as field separators. |
| `RtfExporter` | Word Document | `.docx` | ... |
| `YamlExporter` | YAML | `.yaml` | Produces YAML-style structured text for configuration- and data-oriented use cases. |
| `XmlExporter` | XML | `.xml` | Generates structured XML output for interoperability and machine processing. |

## Architecture

The export classes belong to the `Planetoid_DB.Export` namespace, while the common interface is located in `Planetoid_DB.Helpers`.

```text
                         Planetoid-DB
                              │
                              ▼
                     Export selection / UI
                              │
                              ▼
                   IOrbitDataExporter
                              │
          ┌───────────────────┼───────────────────┐
          │                   │                   │
          ▼                   ▼                   ▼
      CsvExporter        JsonExporter       HtmlExporter
          │                   │                   │
          └───────────────────┼───────────────────┘
                              │
                              ▼
                         File output
```

Each exporter is responsible only for converting the supplied `selectedData` into its target representation and writing it to the requested file path.

## Common Export Contract

Every exporter implements:

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

### `Extension`

Returns the file extension associated with the exporter.

Example:

```csharp
public string Extension => "json";
```

### `Filter`

Provides the filter string used by a WinForms `SaveFileDialog`.

Example:

```csharp
public string Filter =>
    "JSON files (*.json)|*.json|All files (*.*)|*.*";
```

### `Title`

Provides a human-readable description of the export operation and can be used as the save-dialog title.

Example:

```csharp
public string Title =>
    "Save database information as JSON";
```

### `Export(...)`

Performs the actual conversion and file creation.

```csharp
void Export(
    string filePath,
    string exportTitle,
    Dictionary<string, string> selectedData);
```

The exporter receives the destination path, the title associated with the export, and the selected key/value data.

## Example

A calling form can work against the interface instead of depending on a concrete exporter:

```csharp
IOrbitDataExporter exporter = new JsonExporter();

exporter.Export(
    filePath,
    "Minor Planet Data",
    selectedData);
```

This makes it possible to replace the output format without changing the rest of the export workflow.

## Supported Output Categories

The current exporters cover several important categories:

### Structured Data

- JSON
- XML
- YAML

These formats are especially useful when the exported information is intended for further processing by software.

### Delimited Data

- CSV
- PSV
- TSV

These formats are useful for spreadsheets, databases, scripts, and statistical processing.

### Human-Readable Text

- Plain text
- Markdown

These formats are suitable for notes, documentation, reports, source control repositories, and quick inspection.

### Markup and Publishing

- HTML
- BBCode
- Creole
- LaTeX

These formats allow orbit information to be transferred into web pages, forums, wiki systems, or scientific documents.

### Calendar Data

- iCalendar (`.ics`)

This format is intended for applications and workflows that consume calendar-style data.

## HTML Export

`HtmlExporter` produces a complete HTML document rather than only a fragment. The generated output includes a document declaration, metadata, a title derived from `exportTitle`, basic CSS, and the selected key/value pairs.

A simplified output structure is:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <title>Minor Planet Data</title>
    ...
</head>
<body>
    ...
</body>
</html>
```

The exporter also identifies Planetoid-DB as the generator of the document.

## Logging

The exporters use **NLog** for export-related logging. Export operations can therefore be traced through the application's logging infrastructure.

A typical exporter logs the target file before writing it:

```text
Exporting data to CSV file: ...
```

This is useful for diagnostics and for identifying failed or unexpected export operations.

## File Organization

The directory follows a deliberately simple structure:

```text
Export/
├── BbcodeExporter.cs
├── CreoleExporter.cs
├── CsvExporter.cs
├── ExcelExporter.cs
├── HtmlExporter.cs
├── IcsExporter.cs
├── JsonExporter.cs
├── LatexExporter.cs
├── MarkdownExporter.cs
├── OdsExporter.cs
├── OdtExporter.cs
├── PsvExporter.cs
├── RtfExporter.cs
├── TextExporter.cs
├── TsvExporter.cs
├── TsvExporter.cs
├── WordExporter.cs
└── XmlExporter.cs
```

The common abstraction is located outside the directory:

```text
Helpers/
└── IOrbitDataExporter.cs
```

## Adding a New Export Format

To add another export format:

1. Create a new class in `Export/`.
2. Implement `IOrbitDataExporter`.
3. Provide the correct file extension.
4. Provide a suitable `SaveFileDialog` filter.
5. Provide a descriptive title.
6. Implement the `Export(...)` method.
7. Register the exporter with the appropriate export-selection logic in the application.

Example skeleton:

```csharp
namespace Planetoid_DB.Export;

public sealed class ExampleExporter : IOrbitDataExporter
{
    public string Extension => "example";

    public string Filter =>
        "Example files (*.example)|*.example|All files (*.*)|*.*";

    public string Title =>
        "Save database information as Example";

    public void Export(
        string filePath,
        string exportTitle,
        Dictionary<string, string> selectedData)
    {
        // Convert selectedData into the target format
        // and write the result to filePath.
    }
}
```

## Design Principles

The export subsystem follows several useful design principles:

- **Single responsibility** – each exporter handles one output format.
- **Common abstraction** – all exporters implement `IOrbitDataExporter`.
- **UI independence** – formatting logic is kept outside the WinForms forms.
- **Extensibility** – additional formats can be added without redesigning the existing exporters.
- **Consistent file-dialog integration** – every exporter exposes its extension, filter, and title.
- **Logging** – export operations integrate with the application's NLog infrastructure.

## Related Documentation

- [Main project README](../README.md)
- [Forms](../Forms/README.md)
- [Helpers](../Helpers/README.md)
- [IOrbitDataExporter](../Helpers/IOrbitDataExporter.cs)
- [Export directory](https://github.com/mjohne/Planetoid-DB/tree/main/Export)
