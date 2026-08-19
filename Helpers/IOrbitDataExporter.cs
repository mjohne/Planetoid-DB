/*
 * File:        IOrbitDataExporter.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Helpers
 * Description: Manages loading and saving of bookmark entries per database file.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 *
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */


namespace Planetoid_DB.Helpers;

/// <summary>Interface for exporting orbit data to different file formats.</summary>
/// <remarks>This interface defines the properties and methods required for exporting orbit data to various file formats.</remarks>
public interface IOrbitDataExporter
{
	/// <summary>Gets the file extension associated with the export format.</summary>
	/// <remarks>This property returns the file extension (e.g., ".csv", ".json") that corresponds to the export format.</remarks>
	string Extension { get; }

	/// <summary>Gets the filter string used for file dialogs when selecting files for export.</summary>
	/// <remarks>This property returns a filter string (e.g., "CSV files (*.csv)|*.csv") that can be used in file dialogs to filter the displayed files based on the export format.</remarks>
	string Filter { get; }

	/// <summary>Gets the title or description of the export format.</summary>
	/// <remarks>This property returns a title or description (e.g., "Comma-Separated Values") that provides information about the export format.</remarks>
	string Title { get; }

	/// <summary>Exports the selected orbit data to a file at the specified file path.</summary>
	/// <param name="filePath">The path of the file to which the data will be exported.</param>
	/// <param name="exportTitle">The title of the export operation.</param>
	/// <param name="selectedData">The orbit data to be exported.</param>
	/// <remarks>This method performs the export operation, writing the selected orbit data to a file at the specified file path. The exportTitle parameter provides context for the export operation, and the selectedData parameter contains the data to be exported.</remarks>
	void Export(string filePath, string exportTitle, Dictionary<string, string> selectedData);
}