# Resources/FugueIcons

The `Resources/FugueIcons` directory contains the 16 × 16 pixel PNG icons of the **Fugue Icons** set by Yusuke Kamiyamane used in the user interface of **Planetoid-DB**.

## Purpose

The icons are used for menus, toolbars, context menus, and buttons in the forms. They are embedded via `Resources/FugueIcons16px.resx` and exposed through the generated `FugueIcons16px` class.

## Naming Convention

All files follow the pattern:

```text
fugue_<icon-name>_16px.png
```

Examples: `fugue_abacus_16px.png`, `fugue_address-book--arrow_16px.png`.

Fugue uses double hyphens (`--`) to mark overlay variants of a base icon (e.g. `address-book--arrow` is the address-book icon with an arrow overlay). In the generated `FugueIcons16px` class, characters that are not valid in C# identifiers are replaced by underscores.

## Usage

```csharp
button.Image = FugueIcons16px.fugue_abacus_16px;
```

In the WinForms designer, select the image from the `FugueIcons16px` project resource.

## License

The Fugue Icons by Yusuke Kamiyamane are published under the Creative Commons Attribution 3.0 license. Attribution must be retained when the icons are redistributed.

## Development Guidelines

1. Keep the naming convention when adding icons.
2. Add new icons to `FugueIcons16px.resx` so they are available through the generated class.
3. Remove unused icons from both the directory and the `.resx` file together.

## Related Documentation

- [Main project README](../../README.md)
- [Resources](../README.md)
- [FatcowIcons](../FatcowIcons/README.md)
