# Properties/PublishProfiles

The `Properties/PublishProfiles` directory contains the publish profiles (`*.pubxml`) used to build distributable packages of **Planetoid-DB**.

## Purpose

Publish profiles store the configuration for `dotnet publish` and the Visual Studio *Publish* dialog, such as the target runtime, deployment type, and output directory. All profiles build the `Release` configuration for `net10.0-windows7.0`.

## Available Profiles

| Profile | Runtime | Deployment | Description |
|---|---|---|---|
| `FolderProfile_win-x64.pubxml` | `win-x64` | Folder | Framework-dependent single-file build for 64-bit Windows. |
| `FolderProfile_win-x86.pubxml` | `win-x86` | Folder | Framework-dependent single-file build for 32-bit Windows. |
| `FolderProfile_win-arm64.pubxml` | `win-arm64` | Folder | Framework-dependent single-file build for Windows on ARM64. |
| `FolderProfile_win-x64_standalone.pubxml` | `win-x64` | Folder | Self-contained single-file build for 64-bit Windows. |
| `FolderProfile_win-x86_standalone.pubxml` | `win-x86` | Folder | Self-contained single-file build for 32-bit Windows. |
| `FolderProfile_win-arm64_standalone.pubxml` | `win-arm64` | Folder | Self-contained single-file build for Windows on ARM64. |
| `ClickOnceProfile_win-x64.pubxml` | `win-x64` | ClickOnce | ClickOnce deployment for 64-bit Windows. |
| `ClickOnceProfile_win-x86.pubxml` | `win-x86` | ClickOnce | ClickOnce deployment for 32-bit Windows. |
| `ClickOnceProfile_win-arm64.pubxml` | `win-arm64` | ClickOnce | ClickOnce deployment for Windows on ARM64. |

All Folder profiles set `PublishSingleFile` and `IncludeNativeLibrariesForSelfExtract`. The `_standalone` profiles additionally set `SelfContained` to `true`, so the .NET runtime does not need to be installed on the target machine.

## Usage

From Visual Studio, select **Build → Publish** and choose a profile, or use the command line on Windows:

```bash
dotnet publish Planetoid-DB.csproj -p:PublishProfile=FolderProfile_win-x64_standalone
```

For Folder profiles, the single-file output is written to the `PublishDir` defined in the profile (below `bin\Release\net10.0-windows\publish\`).

## Notes

- Publishing requires Windows and the Windows Desktop workload, because the application targets Windows Forms.
- Do not store signing passwords or other secrets in publish profiles.

## Related Documentation

- [Main project README](../../README.md)
- [Properties](../README.md)
