# Planetoid-DB - Viewer for the MPCORB.DAT file

![GitHub License](https://img.shields.io/github/license/mjohne/Planetoid-DB)
![GitHub Created At](https://img.shields.io/github/created-at/mjohne/Planetoid-DB)
![GitHub contributors](https://img.shields.io/github/contributors/mjohne/Planetoid-DB)
![GitHub commit activity](https://img.shields.io/github/commit-activity/t/mjohne/Planetoid-DB)
![GitHub last commit](https://img.shields.io/github/last-commit/mjohne/Planetoid-DB)
![GitHub Release Date](https://img.shields.io/github/release-date/mjohne/Planetoid-DB)
![GitHub Release](https://img.shields.io/github/v/release/mjohne/Planetoid-DB)
![GitHub language count](https://img.shields.io/github/languages/count/mjohne/Planetoid-DB)
![GitHub top language](https://img.shields.io/github/languages/top/mjohne/Planetoid-DB)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/mjohne/Planetoid-DB)
![GitHub branch status](https://img.shields.io/github/checks-status/mjohne/Planetoid-DB/master)
![GitHub repo size](https://img.shields.io/github/repo-size/mjohne/Planetoid-DB)
![GitHub Downloads (all assets, all releases)](https://img.shields.io/github/downloads/mjohne/Planetoid-DB/total)
![GitHub Downloads (all assets, latest release)](https://img.shields.io/github/downloads/mjohne/Planetoid-DB/latest/total)
![GitHub number of milestones](https://img.shields.io/github/milestones/all/mjohne/Planetoid-DB)
![GitHub number of milestones](https://img.shields.io/github/milestones/open/mjohne/Planetoid-DB)
![GitHub number of milestones](https://img.shields.io/github/milestones/closed/mjohne/Planetoid-DB)
![GitHub Discussions](https://img.shields.io/github/discussions/mjohne/Planetoid-DB)

---

Planetoid-DB reads the large ASCII database of the Minor Planet Center Orbit Table (MPCORB.DAT), which contains orbital data of hundreds of thousands of small celestial bodies (including orbital elements, absolute brightness, latest observation data, etc.) and displays it in a graphical interface.

This file is the official MPC database for minor planets and comets and contains, among other things, the following information for each object:

- Identification number
- Name/Designation
- Orbital features (eccentricity, semi-major axis, inclination, etc.)
- Observation and orbital parameters
- Magnitude and other properties

The viewer allows the user to search objects according to various criteria, select individual entries, view their parameters, and optionally retrieve further properties (e.g., ephemeral data or status). The exact interface and controls depend on the specific version, as the project has many releases.

Planetoid-DB is not a complete database management system in the classic IT sense (like SQL Server, PostgreSQL, etc.). Rather, it is a specialized reader/viewer for a specific astronomical data file (MPCORB), similar to a data browser or data tool.

The MPCORB.DAT dataset originates from the Minor Planet Center, the central repository for astronomical observations and orbital data for minor planets, asteroids, and comets. This file is used by astronomers and amateur astronomers to evaluate or further process orbital data.

Planetoid-DB serves this purpose by providing a user-friendly interface to read, filter, and graphically/tabularly examine this large text file — especially for users who do not want to work directly with scripts or data parsing in Python/Fortran etc.

<img width="868" height="423" alt="Screenshot of Planetoid-DB 0.7.21.48" src="https://github.com/user-attachments/assets/6b63a387-c1eb-4219-9a96-cbe2c6c20e87" />
