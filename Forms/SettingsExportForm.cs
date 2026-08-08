// Main logic for SettingsExportForm.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Represents a dialog that exports all application settings to one of five formats:
/// CSV, INI, XML, JSON, or YAML.</summary>
/// <remarks>The form provides five equally-sized, horizontally-arranged export buttons.
/// Settings are collected automatically from <c>Settings.settings</c> at export time and
/// saved with their name, data type, scope (User / Application), and current value.</remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class SettingsExportForm : BaseKryptonForm
{
	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages and errors for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Overrides the base property to return the form's own status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="SettingsExportForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public SettingsExportForm() =>
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a custom display string for the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event of <see cref="SettingsExportForm"/>.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Clears the status bar when the form is first displayed.</remarks>
	private void SettingsExportForm_Load(object sender, EventArgs e) =>
		ClearStatusBar(label: labelInformation);

	#endregion

	#region Click event handlers

	/// <summary>Handles the Click event of the Export as CSV button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Prompts the user for a file path and exports all settings as CSV.</remarks>
	private void ButtonExportCsv_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Initiating settings export to CSV.");
		ExportSettings(filter: "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*", defaultExt: "csv", dialogTitle: "Export Settings as CSV", filePrefix: "Settings", exporter: SettingsExporter.SaveAsCsv);
	}

	/// <summary>Handles the Click event of the Export as INI button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Prompts the user for a file path and exports all settings as INI.</remarks>
	private void ButtonExportIni_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Initiating settings export to INI.");
		ExportSettings(filter: "INI Files (*.ini)|*.ini|All Files (*.*)|*.*", defaultExt: "ini", dialogTitle: "Export Settings as INI", filePrefix: "Settings", exporter: SettingsExporter.SaveAsIni);
	}

	/// <summary>Handles the Click event of the Export as XML button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Prompts the user for a file path and exports all settings as XML.</remarks>
	private void ButtonExportXml_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Initiating settings export to XML.");
		ExportSettings(filter: "XML Files (*.xml)|*.xml|All Files (*.*)|*.*", defaultExt: "xml", dialogTitle: "Export Settings as XML", filePrefix: "Settings", exporter: SettingsExporter.SaveAsXml);
	}

	/// <summary>Handles the Click event of the Export as JSON button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Prompts the user for a file path and exports all settings as JSON.</remarks>
	private void ButtonExportJson_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Initiating settings export to JSON.");
		ExportSettings(filter: "JSON Files (*.json)|*.json|All Files (*.*)|*.*", defaultExt: "json", dialogTitle: "Export Settings as JSON", filePrefix: "Settings", exporter: SettingsExporter.SaveAsJson);
	}

	/// <summary>Handles the Click event of the Export as YAML button.</summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Prompts the user for a file path and exports all settings as YAML.</remarks>
	private void ButtonExportYaml_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Initiating settings export to YAML.");
		ExportSettings(filter: "YAML Files (*.yaml)|*.yaml|All Files (*.*)|*.*", defaultExt: "yaml", dialogTitle: "Export Settings as YAML", filePrefix: "Settings", exporter: SettingsExporter.SaveAsYaml);
	}

	#endregion

	#region private helpers

	/// <summary>Shows a save file dialog and, if confirmed, calls the supplied <paramref name="exporter"/> action.</summary>
	/// <param name="filter">The file-type filter string for the save dialog.</param>
	/// <param name="defaultExt">The default file extension.</param>
	/// <param name="dialogTitle">The title of the save dialog.</param>
	/// <param name="filePrefix">The prefix used to compose the suggested file name.</param>
	/// <param name="exporter">The action that performs the actual export given a file path.</param>
	/// <remarks>The suggested file name is composed of <paramref name="filePrefix"/> and the current timestamp.
	/// The wait cursor is shown during export. Any unexpected exception is logged and shown as an error.</remarks>
	private void ExportSettings(string filter, string defaultExt, string dialogTitle, string filePrefix, Action<string> exporter)
	{
		using SaveFileDialog saveFileDialog = new()
		{
			Filter = filter,
			DefaultExt = defaultExt,
			Title = dialogTitle,
			InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments),
			FileName = $"{filePrefix}_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.{defaultExt}",
		};

		if (saveFileDialog.ShowDialog(owner: this) != DialogResult.OK)
		{
			logger.Warn(message: "User cancelled the settings export dialog.");
			return;
		}

		try
		{
			Cursor.Current = Cursors.WaitCursor;
			exporter(saveFileDialog.FileName);
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: $"An error occurred during settings export: {ex.Message}");
			ShowErrorMessage(message: $"An error has occurred during export: {ex.Message}");
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	#endregion
}
