// This file contains the DatabaseInformationForm implementation.

using Krypton.Toolkit;

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Form to display database information.</summary>
/// <remarks>This form provides a user interface for displaying information about the database.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class DatabaseInformationForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the table layout panel used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's table layout panel.</remarks>
	protected override TableLayoutPanel? ExportTableLayoutPanel => tableLayoutPanel;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "Database Information";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "Database-Information";

	#endregion

	/// <summary>NLog logger instance for the class.</summary>
	/// <remarks>This logger is used to log messages for the form.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="DatabaseInformationForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public DatabaseInformationForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	#endregion

	#region form event handlers

	/// <summary>Fired when the database information form loads.
	/// Populates UI labels with file information for the configured MPCORB.DAT file and displays detected file attributes.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the database information form loads.</remarks>
	private void DatabaseInformationForm_Load(object sender, EventArgs e)
	{
		// Path to the database file
		FileInfo fileInfo = new(fileName: Settings.Default.systemFilenameMpcorbDat);
		// Check if the file exists
		if (!File.Exists(path: fileInfo.FullName))
		{
			// Log an error and show a message if the file does not exist
			logger.Error(message: $"Database file not found: {fileInfo.FullName}");
			ShowErrorMessage(message: $"Database file not found: {fileInfo.FullName}");
			return;
		}
		// Set the file information in the labels
		labelNameValue.Text = fileInfo.Name;
		// Set the file name in the label
		labelDirectoryValue.Text = fileInfo.DirectoryName;
		// Set the file size in the label
		labelSizeValue.Text = $"{fileInfo.Length:N0} {I18nStrings.BytesText}";
		// Set the file type in the label
		labelDateCreatedValue.Text = fileInfo.CreationTime.ToString(format: "G", provider: CultureInfo.CurrentCulture);
		// Set the file creation time in the label
		labelDateAccessedValue.Text = fileInfo.LastAccessTime.ToString(format: "G", provider: CultureInfo.CurrentCulture);
		// Set the file last access time in the label
		labelDateWritedValue.Text = fileInfo.LastWriteTime.ToString(format: "G", provider: CultureInfo.CurrentCulture);
		// Set the file attributes in the label
		labelAttributesValue.Text = $"{fileInfo.Attributes}";
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the click event of the copy to clipboard button.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard button is clicked.</remarks>
	private void ToolStripDropDownButtonCopyToClipboard_Click(object sender, EventArgs e)
	{
		// Check if the sender is a tool strip button
		if (sender is ToolStripButton button && button.Owner is not null)
		{
			// Show the context menu for copying to the clipboard
			contextMenuFullCopyToClipboard.Show(control: button.Owner, x: button.Bounds.Left, y: button.Bounds.Bottom);
		}
	}

	/// <summary>Handles the click event of the copy to clipboard menu item for the database name.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard menu item for the database name is clicked.</remarks>
	private void MenuitemCopyToClipboardName_Click(object sender, EventArgs e) => CopyToClipboard(text: labelNameValue.Text);

	/// <summary>Handles the click event of the copy to clipboard menu item for the database path.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard menu item for the database path is clicked.</remarks>
	private void MenuitemCopyToClipboardPath_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDirectoryValue.Text);

	/// <summary>Handles the click event of the copy to clipboard menu item for the database size.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard menu item for the database size is clicked.</remarks>
	private void MenuitemCopyToClipboardSize_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSizeValue.Text);

	/// <summary>Handles the click event of the copy to clipboard menu item for the database creation date.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard menu item for the database creation date is clicked.</remarks>
	private void MenuitemCopyToClipboardCreationDate_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDateCreatedValue.Text);

	/// <summary>Handles the click event of the copy to clipboard menu item for the database last access date.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard menu item for the database last access date is clicked.</remarks>
	private void MenuitemCopyToClipboardLastAccessDate_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDateAccessedValue.Text);

	/// <summary>Handles the click event of the copy to clipboard menu item for the database last write date.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard menu item for the database last write date is clicked.</remarks>
	private void MenuitemCopyToClipboardLastWriteDate_Click(object sender, EventArgs e) => CopyToClipboard(text: labelDateWritedValue.Text);

	/// <summary>Handles the click event of the copy to clipboard menu item for the database attributes.</summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the copy to clipboard menu item for the database attributes is clicked.</remarks>
	private void MenuitemCopyToClipboardAttributes_Click(object sender, EventArgs e) => CopyToClipboard(text: labelAttributesValue.Text);

	/// <summary>Handles the click event of the save to file button.</summary>
	/// <param name="sender">Event source (the button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This method is called when the save to file button is clicked.</remarks>
	private void ToolStripButtonSaveToFile_Click(object sender, EventArgs e)
	{
		// Check if the sender is a tool strip button
		if (sender is ToolStripButton button && button.Owner is not null)
		{
			// Show the context menu for saving to a file
			contextMenuSaveToFile.Show(control: button.Owner, x: button.Bounds.Left, y: button.Bounds.Bottom);
		}
	}

	#endregion
}