# Resources

The `Resources` directory contains the embedded and design-time resources of **Planetoid-DB**: application icons and images, icon libraries, terminology documents, and text data files.

## Purpose

Resources are bundled with the application so that it can display icons, images, help texts, and sample data without external files.

## Contents

| File / Directory | Description |
|---|---|
| [`FatcowIcons/`](./FatcowIcons/README.md) | 16 px icons from the FatCow icon set. |
| `FatcowIcons16px.resx` / `.Designer.cs` | Resource file and generated strongly typed class exposing the FatCow icons. |
| [`FugueIcons/`](./FugueIcons/README.md) | 16 px icons from the Fugue icon set. |
| `FugueIcons16px.resx` / `.Designer.cs` | Resource file and generated strongly typed class exposing the Fugue icons. |
| [`TerminologyDocs/`](./TerminologyDocs/README.md) | HTML documents explaining orbital elements and other terms, shown in the terminology dialog. |
| `Planetoid-DB-*.ico` | Application icons (standard, alternate, and 32 px variants). |
| `PlanetoidDB3.png`, `PlanetoidDB3-alternate.png`, `PlanetoidDB4.png` | Application logos. |
| `about_banner.png` | Banner image used in the About dialog. |
| `c-sharp_64px.png`, `dot-net_64px.png`, `visual-studio-2026_64px.png`, `github.png`, `github-copilot.png`, `gplv3-logo.png` | Technology, platform, and license logos. |
| `ChatGPT Image 22. Feb. 2026, 00_33_39.png` | Additional artwork image. |
| `demoset-10000.txt` | Embedded demo dataset with 10,000 MPCORB records for trying the application without downloading the full database. |
| `ObservatoryCodes.txt` | Embedded list of MPC observatory codes. |

## Usage in Code

Icons are accessed through the generated classes, for example:

```csharp
toolStripButton.Image = FugueIcons16px.fugue_disk_16px;
```

Terminology documents are referenced from `I18nStrings.resx` and are accessed through the generated `I18nStrings` class (e.g. `I18nStrings.terminology_SemiMajorAxis`).

## Development Guidelines

1. Do not hand-edit the `*.Designer.cs` files; add resources through the `.resx` files.
2. Use the existing icon sets before adding new image files.
3. Keep the license and attribution requirements of third-party resources in mind.
4. Large data files should only be embedded if they are needed offline.

## File Organization

```text
Resources/
├── FatcowIcons/
├── FugueIcons/
├── TerminologyDocs/
├── FatcowIcons16px.resx / .Designer.cs
├── FugueIcons16px.resx / .Designer.cs
├── *.ico, *.png
├── demoset-10000.txt
└── ObservatoryCodes.txt
```

## Related Documentation

- [Main project README](../README.md)
- [FatcowIcons](./FatcowIcons/README.md)
- [FugueIcons](./FugueIcons/README.md)
- [TerminologyDocs](./TerminologyDocs/README.md)
- [Properties](../Properties/README.md)
- [Forms](../Forms/README.md)
