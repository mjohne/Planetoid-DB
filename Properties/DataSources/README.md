# Properties/DataSources

The `Properties/DataSources` directory contains Visual Studio object data source definitions used by the Windows Forms designer of **Planetoid-DB**.

## Purpose

An object data source lets the WinForms designer know the shape of a .NET type so that controls (e.g. via `BindingSource`) can be bound to its properties at design time.

## Contents

| File | Type | Description |
|---|---|---|
| `Planetoid_DB.Helpers.PlanetoidRecord.datasource` | `Planetoid_DB.Helpers.PlanetoidRecord` | Generic object data source for the `PlanetoidRecord` domain object, which represents a single MPCORB record. |

## Notes

- The `.datasource` files are generated and maintained by Visual Studio. Renaming the file extension or editing their content manually may make them unrecognizable.
- The data source only affects design-time support; it has no effect on application runtime behaviour.
- If `PlanetoidRecord` is moved or renamed, the data source must be recreated.

## Related Documentation

- [Main project README](../../README.md)
- [Properties](../README.md)
- [Helpers](../../Helpers/README.md)
