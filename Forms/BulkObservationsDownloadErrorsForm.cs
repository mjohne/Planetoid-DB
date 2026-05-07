using System.Globalization;

using Planetoid_DB.Forms;
using Planetoid_DB.Resources;

namespace Planetoid_DB;

/// <summary>Represents a single error entry produced during bulk observations download.</summary>
/// <param name="Timestamp">Date and time when the error occurred.</param>
/// <param name="Url">URL that was being requested or processed.</param>
/// <param name="ErrorType">Error type/category.</param>
/// <param name="ErrorDescription">Detailed error explanation.</param>
internal sealed record BulkObservationsDownloadErrorEntry(
	DateTime Timestamp,
	string Url,
	string ErrorType,
	string ErrorDescription);

/// <summary>Dialog form that displays detailed bulk-download errors in a list view.</summary>
internal sealed class BulkObservationsDownloadErrorsForm : BaseKryptonForm
{
	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	protected override ToolStripStatusLabel? StatusLabel => _toolStripStatusLabel;

	/// <summary>Initializes a new instance of the <see cref="BulkObservationsDownloadErrorsForm"/> class.</summary>
	/// <param name="entries">The error entries to display.</param>
	internal BulkObservationsDownloadErrorsForm(IReadOnlyList<BulkObservationsDownloadErrorEntry> entries)
	{
		Text = "Bulk Download Errors";
		StartPosition = FormStartPosition.CenterParent;
		FormBorderStyle = FormBorderStyle.SizableToolWindow;
		Size = new Size(width: 1000, height: 450);
		MinimumSize = new Size(width: 700, height: 300);

		_listView.Dock = DockStyle.Fill;
		_listView.View = View.Details;
		_listView.FullRowSelect = true;
		_listView.GridLines = true;
		_listView.MultiSelect = false;
		_listView.HideSelection = false;
		_listView.AccessibleName = "Error list";
		_listView.AccessibleDescription = "Shows all logged download errors with timestamp, URL and details";
		_listView.Columns.Add(headerText: "Datum / Uhrzeit", width: 170);
		_listView.Columns.Add(headerText: "URL", width: 390);
		_listView.Columns.Add(headerText: "Fehler", width: 420);
		_listView.MouseEnter += Control_Enter;
		_listView.MouseLeave += Control_Leave;

		foreach (BulkObservationsDownloadErrorEntry entry in entries)
		{
			string timestampText = entry.Timestamp.ToString(format: "yyyy-MM-dd HH:mm:ss", provider: CultureInfo.InvariantCulture);
			string errorText = $"{entry.ErrorType}: {entry.ErrorDescription}";
			ListViewItem item = new(text: timestampText);
			_ = item.SubItems.Add(text: entry.Url);
			_ = item.SubItems.Add(text: errorText);
			_ = _listView.Items.Add(item);
		}

		_statusStrip.Dock = DockStyle.Bottom;
		_statusStrip.Items.Add(value: _toolStripStatusLabel);
		_toolStripStatusLabel.Text = $"{entries.Count} error(s)";
		_toolStripStatusLabel.Image = Resources.FatcowIcons16px.fatcow_information_16px;
		_toolStripStatusLabel.MouseEnter += Control_Enter;
		_toolStripStatusLabel.MouseLeave += Control_Leave;
		_statusStrip.MouseEnter += Control_Enter;
		_statusStrip.MouseLeave += Control_Leave;

		Controls.Add(_listView);
		Controls.Add(_statusStrip);
	}

	private readonly ListView _listView = new();
	private readonly StatusStrip _statusStrip = new();
	private readonly ToolStripStatusLabel _toolStripStatusLabel = new();
}
