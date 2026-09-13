/*
 * File:        BulkObservationsDownloadErrorsForm.cs
 * Project:     Planetoid-DB
 * Namespace:   Planetoid_DB.Forms
 * Description: Dialog form that displays detailed bulk-download errors in a list view.
 *
 * Author:      Michael Johne
 * Company:     Mijo Software
 * 
 * Copyright (c) 2026 Michael Johne
 *
 * Licensed under the GNU General Public License v3.0.
 * See LICENSE file in the project root for license information.
 */

using Planetoid_DB.Resources;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB.Forms;

/// <summary>Dialog form that displays detailed bulk-download errors in a list view.</summary>
/// <remarks>This form is used to present a list of errors that occurred during the bulk download process.</remarks>
// You can customize the debugger display for this class by providing a property that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the DebuggerDisplay property is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this property should be used for the debugger display.
[DebuggerDisplay(value: $"{{{nameof(DebuggerDisplay)},nq}}")]
internal sealed class BulkObservationsDownloadErrorsForm : BaseKryptonForm
{
	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to provide the specific status label used in this form.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => _toolStripStatusLabel;

	/// <summary>Initializes a new instance of the <see cref="BulkObservationsDownloadErrorsForm"/> class.</summary>
	/// <param name="entries">The error entries to display.</param>
	/// <remarks>Configures the form's appearance and populates the list view with the provided error entries.</remarks>
	internal BulkObservationsDownloadErrorsForm(IReadOnlyList<BulkObservationsDownloadErrorEntry> entries)
	{
		// Form configuration
		Text = "Bulk Download Errors";
		StartPosition = FormStartPosition.CenterParent;
		FormBorderStyle = FormBorderStyle.SizableToolWindow;
		Size = new Size(width: 1000, height: 450);
		MinimumSize = new Size(width: 700, height: 300);
		// ListView configuration
		_listView.Dock = DockStyle.Fill;
		_listView.View = View.Details;
		_listView.FullRowSelect = true;
		_listView.GridLines = true;
		_listView.MultiSelect = false;
		_listView.HideSelection = false;
		_listView.AccessibleName = "Error list";
		_listView.AccessibleDescription = "Shows all logged download errors with timestamp, URL and details";
		_ = _listView.Columns.Add(text: "Datum / Uhrzeit", width: 170);
		_ = _listView.Columns.Add(text: "URL", width: 390);
		_ = _listView.Columns.Add(text: "Fehler", width: 420);
		_listView.MouseEnter += Control_Enter;
		_listView.MouseLeave += Control_Leave;
		// Populate ListView with error entries
		foreach (BulkObservationsDownloadErrorEntry entry in entries)
		{
			string timestampText = entry.Timestamp.ToString(format: "yyyy-MM-dd HH:mm:ss", provider: CultureInfo.InvariantCulture);
			string errorText = $"{entry.ErrorType}: {entry.ErrorDescription}";
			ListViewItem item = new(text: timestampText);
			_ = item.SubItems.Add(text: entry.Url);
			_ = item.SubItems.Add(text: errorText);
			_ = _listView.Items.Add(value: item);
		}
		// StatusStrip configuration
		_statusStrip.Dock = DockStyle.Bottom;
		_ = _statusStrip.Items.Add(value: _toolStripStatusLabel);
		_toolStripStatusLabel.Text = $"{entries.Count} error(s)";
		_toolStripStatusLabel.Image = FatcowIcons16px.fatcow_information_16px;
		_toolStripStatusLabel.MouseEnter += Control_Enter;
		_toolStripStatusLabel.MouseLeave += Control_Leave;
		_statusStrip.MouseEnter += Control_Enter;
		_statusStrip.MouseLeave += Control_Leave;
		// Add controls to form
		Controls.Add(value: _listView);
		Controls.Add(value: _statusStrip);
	}

	/// <summary>Releases the resources used by the form.</summary>
	/// <param name="disposing"><see langword="true"/> to release managed resources; otherwise, <see langword="false"/>.</param>
	/// <remarks>Overrides the base class method to dispose of the ListView and StatusStrip controls.</remarks>
	protected override void Dispose(bool disposing)
	{
		// Dispose of managed resources if disposing is true
		if (disposing)
		{
			_listView.Dispose();
			_toolStripStatusLabel.Dispose();
			_statusStrip.Dispose();
		}
		// Call the base class Dispose method
		base.Dispose(disposing);
	}

	/// <summary>Represents the ListView control used to display a collection of items in the user interface.</summary>
	/// <remarks>This control is configured to show detailed information about bulk download errors, including timestamp, URL, and error details.</remarks>
	private readonly ListView _listView = new();

	/// <summary>Represents the status strip control used to display status information in the user interface.</summary>
	/// <remarks>This control is configured to show the number of errors and provide additional information through a status label.</remarks>
	private readonly StatusStrip _statusStrip = new();

	/// <summary>Represents the status label displayed in the associated ToolStrip control.</summary>
	/// <remarks>This label is used to show the count of errors and can also display an icon for visual indication.</remarks>
	private readonly ToolStripStatusLabel _toolStripStatusLabel = new();

	/// <summary>Returns a string representation of the current object for debugging purposes.</summary>
	/// <returns>A string representation of the current object.</returns>
	/// <remarks>This property is used by the debugger to display a concise representation of the object.</remarks>
	private string DebuggerDisplay => ToString() ?? string.Empty;
}
