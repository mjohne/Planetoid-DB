// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Properties;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>
/// Form to display database information.
/// </summary>
/// <remarks>
/// This form provides a user interface for displaying information about the database.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class DatabaseInformationForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance for the class.
	/// </summary>
	/// <remarks>
	/// This logger is used to log messages for the database information form.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="DatabaseInformationForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public DatabaseInformationForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is used to provide a visual representation of the object in the debugger.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	#endregion

	#region form event handlers

	/// <summary>
	/// Fired when the database information form loads.
	/// Populates UI labels with file information for the configured MPCORB.DAT file and displays detected file attributes.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the database information form loads.
	/// </remarks>
	private void DatabaseInformationForm_Load(object sender, EventArgs e)
	{
		// Path to the database file
		FileInfo fileInfo = new(fileName: Settings.Default.systemFilenameMpcorb);
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

	#region MouseDown event handlers

	/// <summary>
	/// Handles the MouseDown event for controls.
	/// Stores the control that triggered the event for future reference.
	/// </summary>
	/// <param name="sender">Event source (the control).</param>
	/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to store the control that triggered the event for future reference.
	/// </remarks>
	private void Control_MouseDown(object sender, MouseEventArgs e)
	{
		// Check if the sender is a Control
		if (sender is Control control)
		{
			// Store the control that triggered the event
			currentControl = control;
		}
	}

	#endregion

	#endregion
}