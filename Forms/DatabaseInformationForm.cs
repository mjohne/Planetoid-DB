using Planetoid_DB.Forms;
using Planetoid_DB.Properties;

using System.Diagnostics;
using System.IO;
using System.Text;

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
	/// Stores the currently selected control for clipboard operations.
	/// </summary>
	/// <remarks>
	/// This field is used to keep track of the control that is currently selected for clipboard operations.
	/// </remarks>
	private Control currentControl;

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

	/// <summary>
	/// Sets the status bar text and enables the information label when text is provided.
	/// </summary>
	/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
	/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
	/// <remarks>
	/// This method is used to update the status bar with the provided text.
	/// </remarks>
	private void SetStatusBar(string text, string additionalInfo = "")
	{
		// Check if the text is not null or whitespace
		if (string.IsNullOrWhiteSpace(value: text))
		{
			return;
		}
		// Set the status bar text and enable it
		labelInformation.Enabled = true;
		labelInformation.Text = string.IsNullOrWhiteSpace(value: additionalInfo) ? text : $"{text} - {additionalInfo}";
	}

	/// <summary>
	/// Clears the status bar text and disables the information label.
	/// </summary>
	/// <remarks>
	/// Resets the UI state of the status area so that no message is shown.
	/// Use when there is no status to display or when leaving a control.
	/// </remarks>
	private void ClearStatusBar()
	{
		// Clear the status bar text and disable it
		labelInformation.Enabled = false;
		labelInformation.Text = string.Empty;
	}

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
		// Get the file attributes
		FileAttributes attributes = File.GetAttributes(path: fileInfo.FullName);
		// Check if the file is an archive
		bool isArchive = (attributes & FileAttributes.Archive) == FileAttributes.Archive;
		// Check if the file is compressed
		bool isCompressed = (attributes & FileAttributes.Compressed) == FileAttributes.Compressed;
		// Check if the file is hidden
		bool isHidden = (attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
		// Check if the file is read-only
		bool isReadOnly = (attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
		// Check if the file is a system file
		bool isSystem = (attributes & FileAttributes.System) == FileAttributes.System;
		// Set the file information in the labels
		labelNameValue.Text = fileInfo.Name;
		// Set the file name in the label
		labelDirectoryValue.Text = fileInfo.DirectoryName;
		// Set the file size in the label
		labelSizeValue.Text = $"{fileInfo.Length:N0} {I10nStrings.BytesText}";
		// Set the file type in the label
		labelDateCreatedValue.Text = fileInfo.CreationTime.ToString();
		// Set the file creation time in the label
		labelDateAccessedValue.Text = fileInfo.LastAccessTime.ToString();
		// Set the file last access time in the label
		labelDateWritedValue.Text = fileInfo.LastWriteTime.ToString();
		// Set the file attributes in the label
		StringBuilder attributesText = new(value: $"({fileInfo.Attributes})");
		// Check if the file is an archive, compressed, hidden, read-only, or a system file
		// and prepend the corresponding attribute name to the attributes text
		// Check if the file is an archive
		if (isArchive)
		{
			// Prepend "archive" to the attributes text
			_ = attributesText.Insert(index: 0, value: "archive, ");
		}
		// Check if the file is compressed
		if (isCompressed)
		{
			// Prepend "compressed" to the attributes text
			_ = attributesText.Insert(index: 0, value: "compressed, ");
		}
		// Check if the file is hidden
		if (isHidden)
		{
			// Prepend "hidden" to the attributes text
			_ = attributesText.Insert(index: 0, value: "hidden, ");
		}
		// Check if the file is read-only
		if (isReadOnly)
		{
			// Prepend "read-only" to the attributes text
			_ = attributesText.Insert(index: 0, value: "readonly, ");
		}
		// Check if the file is a system file
		if (isSystem)
		{
			// Prepend "system" to the attributes text
			_ = attributesText.Insert(index: 0, value: "system, ");
		}
		// Set the file attributes text in the label
		labelAttributesValue.Text = attributesText.ToString();
	}

	/// <summary>
	/// Fired when the database information form is closed.
	/// Disposes managed resources associated with the form.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when the database information form is closed.
	/// Disposes managed resources associated with the form.
	/// </remarks>
	private void DatabaseInformationForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

	#endregion

	#region Enter event handlers

	/// <summary>
	/// Called when the mouse pointer moves over a control.
	/// </summary>
	/// <param name="sender">The event source.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to set the status bar text when a control is entered.
	/// </remarks>
	private void SetStatusBar_Enter(object sender, EventArgs e)
	{
		// Check if the sender is null
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Get the accessible description based on the sender type
		string? description = sender switch
		{
			Control c => c.AccessibleDescription,
			ToolStripItem t => t.AccessibleDescription,
			_ => null
		};
		// If we have a description, set it in the status bar
		if (description != null)
		{
			SetStatusBar(text: description);
		}
	}

	#endregion

	#region Leave event handlers

	/// <summary>
	/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
	/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is called when the mouse leaves a control.
	/// </remarks>
	private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

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

	#region DoubleClick event handlers

	/// <summary>
	/// Called when a control is double-clicked. If the <paramref name="sender"/> is a <see cref="Control"/> or
	/// or a <see cref="ToolStripItem"/>, its <see cref="Control.Text"/> value is copied to the clipboard
	/// using the shared helper.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or a <see cref="ToolStripItem"/>.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is called when a control is double-clicked.
	/// </remarks>
	private void CopyToClipboard_DoubleClick(object sender, EventArgs e)
	{
		// Check if the sender is null
		ArgumentNullException.ThrowIfNull(argument: sender);
		// Get the text to copy based on the sender type
		string? textToCopy = sender switch
		{
			Control c => c.Text,
			ToolStripItem => currentControl?.Text,
			_ => null
		};
		// If we have text to copy, use the helper method to copy it to the clipboard
		if (!string.IsNullOrEmpty(value: textToCopy))
		{
			// Try to set the clipboard text
			try { CopyToClipboard(text: textToCopy); }
			catch
			{ // Throw an exception
				throw new ArgumentException(message: "Unsupported sender type", paramName: nameof(sender));
			}
		}
	}

	#endregion
}