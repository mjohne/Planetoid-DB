/*
 * File:        SettingsImportForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB
 * Description: Represents a dialog that imports all user-scoped application settings from one of five formats: CSV, INI, XML, JSON, or YAML.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Represents a dialog that imports all user-scoped application settings from one of five formats: CSV, INI, XML, JSON, or YAML.</summary>
/// <remarks>The form provides five equally-sized, horizontally-arranged import buttons. Settings are read from the chosen file and applied to <c>Settings.Default</c> by name, data type, and value. Application-scoped settings found in the file are silently ignored. The form also accepts drag-and-drop of a settings file: the correct import method is selected automatically based on the file extension.</remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class SettingsImportForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages and errors for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Overrides the base property to return the form's own status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a custom display string for the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Shows an open file dialog and, if confirmed, calls the supplied <paramref name="importer"/> action.</summary>
	/// <param name="filter">The file-type filter string for the open dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the open dialog.</param>
	/// <param name="importer">The action that performs the actual import given a file path.</param>
	/// <remarks>The initial directory defaults to the user's Documents folder. The wait cursor is shown during import. Any unexpected exception is logged and shown as an error.</remarks>
	private void ImportSettings(string filter, string defaultExt, string dialogTitle, Action<string> importer)
	{
		// Show the open file dialog to the user
		using OpenFileDialog openFileDialog = new()
		{
			Filter = filter,
			DefaultExt = defaultExt,
			Title = dialogTitle,
			InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments),
			CheckFileExists = true,
		};
		// If the user cancels the dialog, log a warning and return
		if (openFileDialog.ShowDialog(owner: this) != DialogResult.OK)
		{
			logger.Warn(message: "User cancelled the settings import dialog.");
			return;
		}
		// Perform the import operation with a wait cursor and error handling
		try
		{
			// Set the cursor to a wait cursor during the import operation
			Cursor.Current = Cursors.WaitCursor;
			// Call the provided importer method with the selected file path
			importer(obj: openFileDialog.FileName);
		}
		// Catch any exceptions that occur during the import operation
		catch (Exception ex)
		{
			// Log the exception and show an error message
			logger.Error(exception: ex, message: $"An error occurred during settings import: {ex.Message}");
			ShowErrorMessage(message: $"An error has occurred during import: {ex.Message}");
		}
		// Ensure that the cursor is reset to the default cursor after the import operation
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	#endregion

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="SettingsImportForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public SettingsImportForm() => InitializeComponent();

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event of <see cref="SettingsImportForm"/>.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Clears the status bar when the form is first displayed.</remarks>
	private void SettingsImportForm_Load(object sender, EventArgs e)
	{
		logger.Info(message: "SettingsImportForm loaded.");
		ClearStatusBar(label: labelInformation);
	}

	/// <summary>Handles the DragEnter event to indicate whether the dragged data is acceptable.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="DragEventArgs"/> instance that contains the event data.</param>
	/// <remarks>Accepts a drag operation only when the dragged data contains at least one file path.</remarks>
	private void SettingsImportForm_DragEnter(object sender, DragEventArgs e)
	{
		logger.Info(message: "DragEnter event triggered for settings import form.");
		e.Effect = e.Data is not null && e.Data.GetDataPresent(format: DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
	}

	/// <summary>Handles the DragDrop event to import settings from the dropped file.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="DragEventArgs"/> instance that contains the event data.</param>
	/// <remarks>The correct import method is chosen automatically based on the file extension.
	/// Only the first dropped file is processed; others are ignored.</remarks>
	private void SettingsImportForm_DragDrop(object sender, DragEventArgs e)
	{
		logger.Info(message: "DragDrop event triggered for settings import form.");
		// Check if the dragged data is null
		if (e.Data is null)
		{
			logger.Warn(message: "DragDrop event triggered with null data.");
			ShowWarnMessage(caption: "DragDrop Error", message: "No data was provided in the drag-and-drop operation.");
			return;
		}
		// Check if the dragged data contains file paths
		if (e.Data.GetData(format: DataFormats.FileDrop) is not string[] files || files.Length == 0)
		{
			logger.Warn(message: "DragDrop event triggered with no files.");
			ShowWarnMessage(caption: "DragDrop Error", message: "No files were provided in the drag-and-drop operation.");
			return;
		}
		// Only process the first file
		string filePath = files[0];
		string ext = Path.GetExtension(path: filePath).TrimStart(trimChar: '.').ToLowerInvariant();
		// Log the file path and extension
		logger.Info(message: $"File dropped for import: '{filePath}' (extension: '{ext}').");
		// Determine the appropriate importer based on the file extension
		Action<string>? importer = ext switch
		{
			"csv" => SettingsImporter.LoadFromCsv,
			"ini" => SettingsImporter.LoadFromIni,
			"xml" => SettingsImporter.LoadFromXml,
			"json" => SettingsImporter.LoadFromJson,
			"yaml" or "yml" => SettingsImporter.LoadFromYaml,
			_ => null,
		};
		// If no importer is found for the file extension, log a warning and show an error message
		if (importer is null)
		{
			logger.Warn(message: $"Unsupported file extension for import: '{ext}'.");
			ShowWarnMessage(caption: "Unsupported File Extension", message: $"The file extension '.{ext}' is not supported for settings import. Supported formats: CSV, INI, XML, JSON, YAML.");
			return;
		}
		// Perform the import operation with a wait cursor and error handling
		try
		{
			// Set the cursor to a wait cursor during the import operation
			Cursor.Current = Cursors.WaitCursor;
			// Call the appropriate importer method with the file path
			importer(obj: filePath);
		}
		// Catch any exceptions that occur during the import operation
		catch (Exception ex)
		{
			// Log the exception and show an error message
			string shownExt = string.IsNullOrEmpty(ext) ? "<none>" : $".{ext}";
			logger.Warn(exception: ex, message: $"Unsupported file extension for import: '{shownExt}'.");
			ShowErrorMessage(message: $"The file extension '{shownExt}' is not supported for settings import. Supported formats: CSV, INI, XML, JSON, YAML.");
		}
		// Ensure that the cursor is reset to the default cursor after the import operation
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Import from CSV button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Prompts the user for a file path and imports settings from a CSV file.</remarks>
	private void ButtonImportCsv_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Initiating settings import from CSV.");
		ImportSettings(filter: "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Import Settings from CSV", importer: SettingsImporter.LoadFromCsv);
	}

	/// <summary>Handles the Click event of the Import from INI button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Prompts the user for a file path and imports settings from an INI file.</remarks>
	private void ButtonImportIni_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Initiating settings import from INI.");
		ImportSettings(filter: "INI Files (*.ini)|*.ini|All Files (*.*)|*.*", defaultExt: "ini", dialogTitle: "Import Settings from INI", importer: SettingsImporter.LoadFromIni);
	}

	/// <summary>Handles the Click event of the Import from XML button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Prompts the user for a file path and imports settings from an XML file.</remarks>
	private void ButtonImportXml_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Initiating settings import from XML.");
		ImportSettings(filter: "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Import Settings from XML", importer: SettingsImporter.LoadFromXml);
	}

	/// <summary>Handles the Click event of the Import from JSON button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Prompts the user for a file path and imports settings from a JSON file.</remarks>
	private void ButtonImportJson_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Initiating settings import from JSON.");
		ImportSettings(filter: "JSON Files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Import Settings from JSON", importer: SettingsImporter.LoadFromJson);
	}

	/// <summary>Handles the Click event of the Import from YAML button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Prompts the user for a file path and imports settings from a YAML file.</remarks>
	private void ButtonImportYaml_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Initiating settings import from YAML.");
		ImportSettings(filter: "YAML Files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Import Settings from YAML", importer: SettingsImporter.LoadFromYaml);
	}

	#endregion
}
