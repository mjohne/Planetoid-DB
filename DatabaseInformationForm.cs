using Planetoid_DB.Properties;

using System.Diagnostics;
using System.IO;
using System.Text;

namespace Planetoid_DB
{
	/// <summary>
	/// Form to display database information.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class DatabaseInformationForm : BaseKryptonForm
	{
		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="DatabaseInformationForm"/> class.
		/// </summary>
		public DatabaseInformationForm()
		{
			// Initialize the form components
			InitializeComponent();
		}

		#endregion

		#region helper methods

		/// <summary>
		/// Returns a short debugger display string for this instance.
		/// </summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		private string GetDebuggerDisplay() => ToString();

		/// <summary>
		/// Sets the status bar text and enables the information label when text is provided.
		/// </summary>
		/// <param name="text">Main status text to display. If null or whitespace the method returns without changing the UI.</param>
		/// <param name="additionalInfo">Optional additional information appended to the main text, separated by " - ".</param>
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
		/// Handles the Load event of the DatabaseInformationForm control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
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
		/// Handles the FormClosed event of the DatabaseInformationForm control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		private void DatabaseInformationForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

		#endregion

		#region Enter event handlers

		/// <summary>
		/// Called when the mouse pointer moves over a control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void SetStatusBar_Enter(object sender, EventArgs e)
		{
			// Set the status bar text based on the sender's accessible description
			switch (sender)
			{
				// If the sender is a control with an accessible description, set the status bar text
				// If the sender is a ToolStripItem with an accessible description, set the status bar text
				case Control { AccessibleDescription: not null } control:
					SetStatusBar(text: control.AccessibleDescription);
					break;
				case ToolStripItem { AccessibleDescription: not null } item:
					SetStatusBar(text: item.AccessibleDescription);
					break;
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
		private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

		#endregion

		#region DoubleClick event handlers

		/// <summary>
		/// Called when a control is double-clicked to copy its text to the clipboard.
		/// If the sender is a <see cref="Control"/>, the control's <see cref="Control.Text"/> is copied.
		/// </summary>
		/// <param name="sender">Event source (expected to be a <see cref="Control"/>).</param>
		/// <param name="e">Event arguments.</param>
		private void CopyToClipboard_DoubleClick(object sender, EventArgs e)
		{
			// Check if the sender is null
			ArgumentNullException.ThrowIfNull(argument: sender);
			if (sender is Control control)
			{
				// Copy the text to the clipboard
				CopyToClipboard(text: control.Text);
			}
		}

		#endregion
	}
}
