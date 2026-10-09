# Properties

The `Properties` directory contains project-level metadata of **Planetoid-DB**: application settings, project resources, Visual Studio data sources, and publishing profiles.

## Purpose

This directory follows the standard Visual Studio layout for .NET Windows Forms projects. Most files here are created and maintained by Visual Studio designers.

## Contents

| File / Directory | Description |
|---|---|
| `Settings.settings` | Definition of the application settings (download URLs, file names, homepage, logging, application directory, experimental features, …). |
| `Settings.Designer.cs` | Generated strongly typed `Settings` class for accessing the settings in code. |
| `Resources.resx` | Project-level resource file. |
| `Resources.Designer.cs` | Generated strongly typed `Resources` class for accessing `Resources.resx`. |
| [`DataSources/`](./DataSources/README.md) | Visual Studio object data source definitions. |
| [`PublishProfiles/`](./PublishProfiles/README.md) | Publish profiles for Folder and ClickOnce deployments. |

## Application Settings

The settings in `Settings.settings` are mostly organized in pairs:

- `system*` settings contain the built-in default values (e.g. `systemMpcorbDatUrl`).
- `user*` settings contain the user-configurable values (e.g. `userMpcorbDatUrl`).

Settings import and export is handled by helper classes in the [`Helpers`](../Helpers/README.md) directory.

## Development Guidelines

1. Do not hand-edit `*.Designer.cs` files; edit the `.settings` / `.resx` file via Visual Studio or regenerate the code.
2. UI strings that are shown to the user belong in `I18nStrings.resx` in the project root, not in `Properties/Resources.resx`.
3. Never store secrets or credentials in settings or resources.

## File Organization

```text
Properties/
├── DataSources/
├── PublishProfiles/
├── Resources.Designer.cs
├── Resources.resx
├── Settings.Designer.cs
└── Settings.settings
```

## Related Documentation

- [Main project README](../README.md)
- [DataSources](./DataSources/README.md)
- [PublishProfiles](./PublishProfiles/README.md)
- [Resources](../Resources/README.md)
- [Helpers](../Helpers/README.md)
