# Attributions

This file documents third-party software, icon sets, data sources, and other externally created material used by **Planetoid-DB**.

Planetoid-DB itself is licensed under the **GNU General Public License v3.0 (GPL-3.0)**. See [LICENSE](LICENSE).

> **Scope:** The software dependency table below reflects the direct `PackageReference` entries currently declared in `Planetoid-DB.csproj`. NuGet may resolve additional transitive dependencies; those are governed by their respective package licenses and are not exhaustively enumerated here.

---

## 1. Third-party software dependencies

### Krypton Standard Toolkit

Planetoid-DB uses the following Krypton Standard Toolkit packages:

| Package             |      Version | License      | Source                                            |
| ------------------- | -----------: | ------------ | ------------------------------------------------- |
| `Krypton.Docking`   | 105.26.7.201 | BSD 3-Clause | https://github.com/Krypton-Suite/Standard-Toolkit |
| `Krypton.Navigator` | 105.26.7.201 | BSD 3-Clause | https://github.com/Krypton-Suite/Standard-Toolkit |
| `Krypton.Ribbon`    | 105.26.7.201 | BSD 3-Clause | https://github.com/Krypton-Suite/Standard-Toolkit |
| `Krypton.Toolkit`   | 105.26.7.201 | BSD 3-Clause | https://github.com/Krypton-Suite/Standard-Toolkit |
| `Krypton.Workspace` | 105.26.7.201 | BSD 3-Clause | https://github.com/Krypton-Suite/Standard-Toolkit |

Copyright information for the current Krypton Standard Toolkit includes the Krypton Suite contributors and earlier Component Factory work. See the upstream repository and package license for the complete notices.

The BSD 3-Clause license text used by the project is available at [LICENSES/BSD-3-Clause.txt](LICENSES/BSD-3-Clause.txt).

### Krypton Extended Toolkit

| Package                                   |     Version | License | Source                                            |
| ----------------------------------------- | ----------: | ------- | ------------------------------------------------- |
| `Krypton.Toolkit.Suite.Extended.Ultimate` | 95.25.5.145 | MIT     | https://github.com/Krypton-Suite/Extended-Toolkit |

Copyright: Krypton Suite, 2017–2023.

The package's upstream license is available from the Extended Toolkit repository.

### NLog

| Package            | Version | License      | Source                       |
| ------------------ | ------: | ------------ | ---------------------------- |
| `NLog`             |   6.2.1 | BSD 3-Clause | https://github.com/NLog/NLog |
| NLog documentation |       — | —            | https://nlog-project.org/    |

Copyright: NLog Project, 2004–2026.

The BSD 3-Clause license text used by the project is available at [LICENSES/BSD-3-Clause.txt](LICENSES/BSD-3-Clause.txt).

### OpenTK

| Package            | Version | License | Source                           |
| ------------------ | ------: | ------- | -------------------------------- |
| `OpenTK`           |   4.9.4 | MIT     | https://github.com/opentk/opentk |
| `OpenTK.GLControl` |   4.0.2 | MIT     | https://github.com/opentk/opentk |

OpenTK 4.x incorporates work originating with the Open Toolkit project. The NuGet package identifies copyright for the Open Toolkit library as belonging to Stefanos Apostolopoulos (2006–2020), with the GLControl package identifying Team OpenTK for its current package work.

### ScottPlot

| Package              | Version | License | Source                 |
| -------------------- | ------: | ------- | ---------------------- |
| `ScottPlot`          |  5.1.59 | MIT     | https://scottplot.net/ |
| `ScottPlot.WinForms` |  5.1.59 | MIT     | https://scottplot.net/ |

Copyright: Scott Harden / Harden Technologies, LLC.

### System.Data.SQLite

| Package              | Version | License       | Source                          |
| -------------------- | ------: | ------------- | ------------------------------- |
| `System.Data.SQLite` |   2.0.4 | Public Domain | https://system.data.sqlite.org/ |

System.Data.SQLite is released into the public domain. SQLite itself is also public-domain software.

---

## 2. Third-party icon and graphic assets

### FatCow Icons

Planetoid-DB includes and uses the **FatCow** 16px icon set.

| Asset        | License                                      | Author / provider  | Source                        |
| ------------ | -------------------------------------------- | ------------------ | ----------------------------- |
| FatCow Icons | Creative Commons Attribution 3.0 (CC BY 3.0) | FatCow Web Hosting | https://fatcow.com/free-icons |

The icons are embedded in project resources under:

* `Resources/FatcowIcons/`
* `Resources/FatcowIcons16px.resx`

The CC BY 3.0 license text is available at [LICENSES/CC-BY-3.0.txt](LICENSES/CC-BY-3.0.txt).

### Fugue Icons

Planetoid-DB includes the **Fugue Icons** set by **Yusuke Kamiyamane**.

| Asset       | License                                      | Author            | Source                                            |
| ----------- | -------------------------------------------- | ----------------- | ------------------------------------------------- |
| Fugue Icons | Creative Commons Attribution 3.0 (CC BY 3.0) | Yusuke Kamiyamane | https://p.yusukekamiyamane.com/icon/search/fugue/ |

The icons are embedded in project resources under:

* `Resources/FugueIcons/`
* `Resources/FugueIcons16px.resx`

The project uses the same CC BY 3.0 license text stored at [LICENSES/CC-BY-3.0.txt](LICENSES/CC-BY-3.0.txt).

Suggested attribution:

> Some icons by Yusuke Kamiyamane. Licensed under a Creative Commons Attribution 3.0 License.

### Application icon

The current application icon is created by AI.

The old application icon is based on the **Asteroid** icon by **monkik**, distributed through Flaticon.

| Asset         | License / terms                                      | Author | Source                                              |
| ------------- | ---------------------------------------------------- | ------ | --------------------------------------------------- |
| Asteroid icon | Flaticon License; free use with required attribution | monkik | https://www.flaticon.com/free-icon/asteroid_1086068 |

The repository includes the corresponding Flaticon license text at [LICENSES/flaticon_license.txt](LICENSES/flaticon_license.txt).

---

## 3. Astronomical data sources

### Minor Planet Center (MPC)

Planetoid-DB is designed to read the **MPCORB.DAT** minor-planet orbit catalogue published by the **Minor Planet Center (MPC)**.

Source:

* MPC data services: https://data.minorplanetcenter.net/
* MPC public documentation: https://docs.minorplanetcenter.net/
* MPCORB.DAT download: https://www.minorplanetcenter.net/iau/MPCORB/MPCORB.DAT.gz

The MPCORB catalogue is external astronomical data and is **not part of the copyright license of the Planetoid-DB source code**. Users should consult the MPC's current terms and documentation when downloading, redistributing, or otherwise using MPC data.

### MPC observatory codes

The repository also contains:

* `Resources/ObservatoryCodes.txt`

This file represents MPC observatory-code data used by the application. The authoritative source is the Minor Planet Center's observatory-code service and documentation.

---

## 4. Repository-provided demonstration data

The repository contains:

* `Resources/demoset-10000.txt`

The file is used as an embedded demonstration dataset by Planetoid-DB. It is maintained as part of the project and is distinct from the application's main source-code license.

The project's changelog identifies this file as the internal demo database. Where individual records originate from external astronomical catalogues, the applicable source terms remain relevant.

---

## 5. Branding and trademarks

The application resources and About dialog contain branding associated with development and hosting technologies, including:

* **C#**
* **.NET**
* **Visual Studio**
* **GitHub**
* **GitHub Copilot**

These names and logos are trademarks or branded assets of their respective owners. Their presence in Planetoid-DB is for identification and informational purposes and does not grant any trademark rights.

Unless explicitly stated otherwise, these branding assets are **not** relicensed under the GPL-3.0 license that applies to the Planetoid-DB source code.

---

## 6. Project license and attribution files

The following files in this repository contain license and attribution information:

| File                                                           | Purpose                                       |
| -------------------------------------------------------------- | --------------------------------------------- |
| [LICENSE](LICENSE)                                             | GPL-3.0 license for Planetoid-DB              |
| [ATTRIBUTIONS.md](ATTRIBUTIONS.md)                             | Consolidated attribution information          |
| [LICENSES/BSD-3-Clause.txt](LICENSES/BSD-3-Clause.txt)         | BSD 3-Clause license text                     |
| [LICENSES/CC-BY-3.0.txt](LICENSES/CC-BY-3.0.txt)               | Creative Commons Attribution 3.0 license text |
| [LICENSES/flaticon_license.txt](LICENSES/flaticon_license.txt) | Flaticon license text                         |

---

## 7. License and attribution maintenance

This document is intended as a practical attribution index. It should be reviewed whenever:

1. A dependency is added, removed, or upgraded.
2. A new icon set, image, font, dataset, or other external asset is added.
3. The source or licensing terms of an existing third-party component change.
4. External astronomical data sources are changed or additional data providers are integrated.

For the most authoritative legal wording, refer to the original license text distributed by each upstream project or provider.
