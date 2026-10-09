# Resources/FatcowIcons

The `Resources/FatcowIcons` directory contains the 16 × 16 pixel PNG icons of the **FatCow** "Farm-Fresh Web Icons" set used in the user interface of **Planetoid-DB**.

## Purpose

The icons are used for menus, toolbars, context menus, and buttons in the forms. They are embedded via `Resources/FatcowIcons16px.resx` and exposed through the generated `FatcowIcons16px` class.

## Naming Convention

All files follow the pattern:

```text
fatcow_<icon-name>_16px.png
```

Examples: `fatcow_32_bit_16px.png`, `fatcow_3d_glasses_16px.png`.

The resource name in `FatcowIcons16px` corresponds to the file name without extension, e.g. `FatcowIcons16px.fatcow_3d_glasses_16px`.

## Usage

```csharp
button.Image = FatcowIcons16px.fatcow_3d_glasses_16px;
```

In the WinForms designer, select the image from the `FatcowIcons16px` project resource.

## License

The FatCow icons are published by FatCow Web Hosting under the Creative Commons Attribution 3.0 license. Attribution must be retained when the icons are redistributed. See [ATTRIBUTIONS.md](../../ATTRIBUTIONS.md) and [LICENSES/CC-BY-3.0.txt](../../LICENSES/CC-BY-3.0.txt).

## Development Guidelines

1. Keep the naming convention when adding icons.
2. Add new icons to `FatcowIcons16px.resx` so they are available through the generated class.
3. Remove unused icons from both the directory and the `.resx` file together.

## Related Documentation

- [Main project README](../../README.md)
- [Resources](../README.md)
- [FugueIcons](../FugueIcons/README.md)
