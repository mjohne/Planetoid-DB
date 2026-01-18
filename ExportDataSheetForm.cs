using NLog;

using System.Diagnostics;
using System.IO;
using System.Text;

namespace Planetoid_DB
{
	/// <summary>
	/// Form for exporting data sheets with various formats.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class ExportDataSheetForm : BaseKryptonForm
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); // NLog logger instance
		private List<string> orbitElements = []; // List of orbit elements to be exported

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ExportDataSheetForm"/> class.
		/// </summary>
		public ExportDataSheetForm()
		{
			// Initialize the form components
			InitializeComponent();
		}

		#endregion

		#region Local methods

		/// <summary>
		/// Returns a string representation of the object for the debugger.
		/// </summary>
		/// <returns>A string representation of the object.</returns>
		private string GetDebuggerDisplay() => ToString();

		/// <summary>
		/// Displays an error message.
		/// </summary>
		/// <param name="message">The error message.</param>
		private static void ShowErrorMessage(string message) =>
			// Show an error message box with the specified message
			_ = MessageBox.Show(text: message, caption: I10nStrings.ErrorCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);

		/// <summary>
		/// Copies the specified text to the clipboard and displays a confirmation message.
		/// </summary>
		/// <param name="text">The text to be copied.</param>
		private static void CopyToClipboard(string text)
		{
			try
			{
				// Copy the text to the clipboard
				Clipboard.SetText(text: text);
				_ = MessageBox.Show(text: I10nStrings.CopiedToClipboard, caption: I10nStrings.InformationCaption, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				// Log the exception and show an error message
				Logger.Error(exception: ex, message: ex.Message);
				// Show an error message
				ShowErrorMessage(message: $"File not found: {ex.Message}");
			}
		}

		/// <summary>
		/// Sets the status bar text.
		/// </summary>
		/// <param name="text">The main text to be displayed on the status bar.</param>
		/// <param name="additionalInfo">Additional information to be displayed alongside the main text.</param>
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
		/// Clears the status bar text.
		/// </summary>
		private void ClearStatusBar()
		{
			// Clear the status bar text and disable it
			labelInformation.Enabled = false;
			labelInformation.Text = string.Empty;
		}

		/// <summary>
		/// Sets the database with the provided list of orbit elements.
		/// </summary>
		/// <param name="list">The list of orbit elements.</param>
		public void SetDatabase(List<string> list) => orbitElements = list;

		/// <summary>
		/// Checks or unchecks all items in the list.
		/// </summary>
		/// <param name="check">True to check all items, false to uncheck all items.</param>
		private void CheckIt(bool check)
		{
			// Check or uncheck all items in the checked list box
			// based on the provided boolean value
			// and enable or disable the export buttons accordingly
			for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
			{
				// Check or uncheck the item at index i
				checkedListBoxOrbitalElements.SetItemChecked(index: i, value: check);
			}
			// Enable or disable the export buttons based on the check state
			buttonExportAsTxt.Enabled = buttonExportAsHtml.Enabled = buttonExportAsXml.Enabled = buttonExportAsJson.Enabled = check;
		}

		/// <summary>
		/// Marks all items in the list.
		/// </summary>
		private void MarkAll() => CheckIt(check: true);

		/// <summary>
		/// Unmarks all items in the list.
		/// </summary>
		private void UnmarkAll() => CheckIt(check: false);

		/// <summary>
		/// Determines whether all items are unmarked.
		/// </summary>
		/// <returns>True if all items are unmarked, otherwise false.</returns>
		private bool IsAllUnmarked()
		{
			// Check if all items in the checked list box are unmarked
			// and return true if they are, otherwise return false
			return checkedListBoxOrbitalElements.Items.OfType<object>().Select(item => item.ToString() ?? string.Empty).Select(itemString => checkedListBoxOrbitalElements.GetItemChecked(index: checkedListBoxOrbitalElements.FindStringExact(str: itemString))).All(isChecked => !isChecked);
			// If all items are unmarked, return true
		}

		#endregion

		#region Form event handler

		/// <summary>
		/// Handles the Load event of the form.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ExportDataSheetForm_Load(object sender, EventArgs e)
		{
			ClearStatusBar(); // Clear the status bar text
			MarkAll(); // Mark all items in the list
		}

		#endregion

		#region Enter-Handler

		/// <summary>
		/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
		/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
		/// </summary>
		/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
		/// <param name="e">Event arguments.</param>
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

		#region Leave-Handler

		/// <summary>
		/// Called when the mouse pointer leaves a control or the control loses focus.
		/// Clears the status bar text (delegates to <see cref="ClearStatusBar"/>).
		/// </summary>
		/// <param name="sender">Event source.</param>
		/// <param name="e">Event arguments.</param>
		private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

		#endregion

		#region Click & ButtonClick event handler

		/// <summary>
		/// Handles the Click event of the ButtonExportAsTxt control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonExportAsTxt_Click(object sender, EventArgs e)
		{
			// Set the initial directory for the save file dialog to the user's documents folder
			saveFileDialogTxt.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
			// Set the initial directory for the save file dialog to the user's documents folder
			saveFileDialogTxt.FileName = $"{orbitElements[index: 0]}.{saveFileDialogTxt.DefaultExt}";
			// Show the save file dialog to select the file path and name
			// If the user selects a file, proceed with exporting
			if (saveFileDialogTxt.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			// Create a new StreamWriter to write the text content to the specified file
			using StreamWriter streamWriter = new(path: saveFileDialogTxt.FileName);
			// Write the orbit elements to the text file
			for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
			{
				// Check if the item is checked
				// If it is checked, write the orbit element to the text file
				if (!checkedListBoxOrbitalElements.GetItemChecked(index: i))
				{
					continue;
				}
				// Write the orbit element to the text file
				streamWriter.Write(value: $"{checkedListBoxOrbitalElements.Items[index: i]}: {orbitElements[index: i]}");
				// If it is not the last item, write a new line
				// to separate the elements in the text file
				if (i < checkedListBoxOrbitalElements.Items.Count - 1)
				{
					// Write a new line to separate the elements
					streamWriter.Write(value: Environment.NewLine);
				}
			}
		}

		/// <summary>
		/// Handles the Click event of the ButtonExportAsHtml control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonExportAsHtml_Click(object sender, EventArgs e)
		{
			// Set the initial directory for the save file dialog to the user's documents folder
			saveFileDialogHtml.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
			// Set the initial directory for the save file dialog to the user's documents folder
			saveFileDialogHtml.FileName = $"{orbitElements[index: 0]}.{saveFileDialogHtml.DefaultExt}";
			// Show the save file dialog to select the file path and name
			if (saveFileDialogHtml.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			// Create a new StreamWriter to write the HTML content to the specified file
			using StreamWriter streamWriter = new(path: saveFileDialogHtml.FileName);
			// Create a StringBuilder to build the HTML content
			StringBuilder sb = new();
			// Append the HTML content to the StringBuilder
			_ = sb.AppendLine(value: "<!DOCTYPE html>");
			_ = sb.AppendLine(value: "<html lang=\"en\">");
			_ = sb.AppendLine(value: "\t<head>");
			_ = sb.AppendLine(value: "\t\t<meta charset=\"utf-8\">");
			_ = sb.AppendLine(value: "\t\t<meta name=\"description\" content=\"\">");
			_ = sb.AppendLine(value: "\t\t<meta name=\"keywords\" content=\"\">");
			_ = sb.AppendLine(value: "\t\t<meta name=\"generator\" content=\"Planetoid-DB\">");
			_ = sb.AppendLine(handler: $"\t\t<title>Export for [{orbitElements[index: 0]}] {orbitElements[index: 1]}</title>");
			_ = sb.AppendLine(value: "\t\t<style>");
			_ = sb.AppendLine(value: "\t\t\t* {font-family: sans-serif;}");
			_ = sb.AppendLine(value: "\t\t\t.italic {font-style: italic;}");
			_ = sb.AppendLine(value: "\t\t\t.bold {font-weight: bold;}");
			_ = sb.AppendLine(value: "\t\t\t.sup {vertical-align: super; font-size: smaller;}");
			_ = sb.AppendLine(value: "\t\t\t.sub {vertical-align: sub; font-size: smaller;}");
			_ = sb.AppendLine(value: "\t\t\t.block {width:350px; display: inline-block;}");
			_ = sb.AppendLine(value: "\t\t</style>");
			_ = sb.AppendLine(value: "\t</head>");
			_ = sb.AppendLine(value: "\t<body>");
			_ = sb.AppendLine(value: "\t\t<p>");
			// Append the orbit elements to the HTML content
			for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
			{
				// Check if the item is checked
				// If it is checked, append the orbit element to the HTML content
				if (checkedListBoxOrbitalElements.GetItemChecked(index: i))
				{
					// Append the orbit element to the HTML content
					_ = sb.AppendLine(handler: $"\t\t\t<span class=\"bold block\" xml:id=\"element-id-{i}\">{checkedListBoxOrbitalElements.Items[index: i]}:</span> <span xml:id=\"value-id-{i}\">{orbitElements[index: i]}</span><br />");
				}
			}
			// Append the closing tags for the HTML content
			_ = sb.AppendLine(value: "\t\t</p>");
			_ = sb.AppendLine(value: "\t</body>");
			_ = sb.Append(value: "</html>");
			// Write the HTML content to the file
			streamWriter.Write(value: sb.ToString());
		}

		/// <summary>
		/// Handles the Click event of the ButtonExportAsXml control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonExportAsXml_Click(object sender, EventArgs e)
		{
			// Set the initial directory for the save file dialog to the user's documents folder
			saveFileDialogXml.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
			// Set the initial directory for the save file dialog to the user's documents folder
			saveFileDialogXml.FileName = $"{orbitElements[index: 0]}.{saveFileDialogXml.DefaultExt}";
			// Show the save file dialog to select the file path and name
			if (saveFileDialogXml.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			// Create a new StreamWriter to write the XML content to the specified file
			using StreamWriter streamWriter = new(path: saveFileDialogXml.FileName);
			// Create a StringBuilder to build the XML content
			StringBuilder sb = new();
			// Append the XML content to the StringBuilder
			_ = sb.AppendLine(value: "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
			_ = sb.AppendLine(value: "<MinorPlanet xmlns=\"https://planet-db.de\">");
			// Append the orbit elements to the XML content
			for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
			{
				// Check if the item is checked
				// If it is checked, append the orbit element to the XML content
				if (checkedListBoxOrbitalElements.GetItemChecked(index: i))
				{
					// Append the orbit element to the XML content
					_ = sb.AppendLine(value: i switch
					{
						0 => $"\t<Index value=\"{orbitElements[index: i]}\"/>",
						1 => $"\t<ReadableDesignation value=\"{orbitElements[index: i]}\"/>",
						2 => $"\t<Epoch value=\"{orbitElements[index: i]}\"/>",
						3 => $"\t<MeanAnomalyAtTheEpoch unit=\"degrees\" value=\"{orbitElements[index: i]}\"/>",
						4 => $"\t<ArgumentOfPerihelion unit=\"degrees\" value=\"{orbitElements[index: i]}\"/>",
						5 => $"\t<LongitudeOfTheAscendingNode unit=\"degrees\" value=\"{orbitElements[index: i]}\"/>",
						6 => $"\t<InclinationToTheEcliptic unit=\"degrees\" value=\"{orbitElements[index: i]}\"/>",
						7 => $"\t<OrbitalEccentricity value=\"{orbitElements[index: i]}\"/>",
						8 => $"\t<MeanDailyMotion unit=\"degrees per day\" value=\"{orbitElements[index: i]}\"/>",
						9 => $"\t<SemiMajorAxis unit=\"astronomical units\" value=\"{orbitElements[index: i]}\"/>",
						10 => $"\t<AbsoluteMagnitude unit=\"mag\" value=\"{orbitElements[index: i]}\"/>",
						11 => $"\t<SlopeParameter value=\"{orbitElements[index: i]}\"/>",
						12 => $"\t<Reference value=\"{orbitElements[index: i]}\"/>",
						13 => $"\t<NumberOfOppositions value=\"{orbitElements[index: i]}\"/>",
						14 => $"\t<NumberOfObservations value=\"{orbitElements[index: i]}\"/>",
						15 => $"\t<ObservationSpan value=\"{orbitElements[index: i]}\"/>",
						16 => $"\t<RmsResidual unit=\"arcseconds\" value=\"{orbitElements[index: i]}\"/>",
						17 => $"\t<ComputerName value=\"{orbitElements[index: i]}\"/>",
						18 => $"\t<FourHexdigitFlags value=\"{orbitElements[index: i]}\"/>",
						19 => $"\t<DateOfLastObservation unit=\"yyyymmdd\" value=\"{orbitElements[index: i]}\"/>",
						20 => $"\t<LinearEccentricity value=\"{orbitElements[index: i]}\"/>",
						21 => $"\t<SemiMinorAxis unit=\"astronomical units\" value=\"{orbitElements[index: i]}\"/>",
						22 => $"\t<MajorAxis unit=\"astronomical units\" value=\"{orbitElements[index: i]}\"/>",
						23 => $"\t<MinorAxis unit=\"astronomical units\" value=\"{orbitElements[index: i]}\"/>",
						24 => $"\t<EccentricAnomaly unit=\"degrees\" value=\"{orbitElements[index: i]}\"/>",
						25 => $"\t<TrueAnomaly unit=\"degrees\" value=\"{orbitElements[index: i]}\"/>",
						26 => $"\t<PerihelionDistance unit=\"astronomical units\" value=\"{orbitElements[index: i]}\"/>",
						27 => $"\t<AphelionDistance unit=\"astronomical units\" value=\"{orbitElements[index: i]}\"/>",
						28 => $"\t<LongitudeOfDescendingNode unit=\"degrees\" value=\"{orbitElements[index: i]}\"/>",
						29 => $"\t<ArgumentOfAphelion unit=\"degrees\" value=\"{orbitElements[index: i]}\"/>",
						30 => $"\t<FocalParameter unit=\"astronomical units\" value=\"{orbitElements[index: i]}\"/>",
						31 => $"\t<SemiLatusRectum unit=\"astronomical units\" value=\"{orbitElements[index: i]}\"/>",
						32 => $"\t<LatusRectum unit=\"astronomical units\" value=\"{orbitElements[index: i]}\"/>",
						33 => $"\t<OrbitalPeriod unit=\"years\" value=\"{orbitElements[index: i]}\"/>",
						34 => $"\t<OrbitalArea unit=\"squared astronomical units\" value=\"{orbitElements[index: i]}\"/>",
						35 => $"\t<OrbitalPerimeter unit=\"astronomical units\" value=\"{orbitElements[index: i]}\"/>",
						36 => $"\t<SemiMeanAxis unit=\"astronomical units\" value=\"{orbitElements[index: i]}\"/>",
						37 => $"\t<MeanAxis unit=\"astronomical units\" value=\"{orbitElements[index: i]}\"/>",
						38 => $"\t<StandardGravitationalParameter unit=\"AU³/a²\" value=\"{orbitElements[index: i]}\"/>",
						_ => string.Empty // Default case if no match is found
					});
				}
			}
			// Append the closing tag for the XML content
			_ = sb.Append(value: "</MinorPlanet>");
			// Write the XML content to the file
			streamWriter.Write(value: sb.ToString());
		}

		/// <summary>
		/// Handles the Click event of the ButtonExportAsJson control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonExportAsJson_Click(object sender, EventArgs e)
		{
			saveFileDialogJson.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
			saveFileDialogJson.FileName = $"{orbitElements[index: 0]}.{saveFileDialogJson.DefaultExt}";
			if (saveFileDialogJson.ShowDialog() != DialogResult.OK)
			{
				return;
			}

			using StreamWriter streamWriter = new(path: saveFileDialogJson.FileName);
			StringBuilder sb = new();
			_ = sb.AppendLine(value: "{");
			for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
			{
				if (checkedListBoxOrbitalElements.GetItemChecked(index: i))
				{
					_ = sb.AppendLine(value: i switch
					{
						0 => $"\t\"Index\": \"{orbitElements[index: i]}\",",
						1 => $"\t\"ReadableDesignation\": \"{orbitElements[index: i]}\",",
						2 => $"\t\"Epoch\": \"{orbitElements[index: i]}\",",
						3 => $"\t\"MeanAnomalyAtTheEpoch\": {orbitElements[index: i]},",
						4 => $"\t\"ArgumentOfPerihelion\": {orbitElements[index: i]},",
						5 => $"\t\"LongitudeOfTheAscendingNode\": {orbitElements[index: i]},",
						6 => $"\t\"InclinationToTheEcliptic\": {orbitElements[index: i]},",
						7 => $"\t\"OrbitalEccentricity\": {orbitElements[index: i]},",
						8 => $"\t\"MeanDailyMotion\": {orbitElements[index: i]},",
						9 => $"\t\"SemiMajorAxis\": {orbitElements[index: i]},",
						10 => $"\t\"AbsoluteMagnitude\": {orbitElements[index: i]},",
						11 => $"\t\"SlopeParameter\": {orbitElements[index: i]} ",
						12 => $"\t\"Reference\": \"{orbitElements[index: i]}\",",
						13 => $"\t\"NumberOfOppositions\": {orbitElements[index: i]},",
						14 => $"\t\"NumberOfObservations\": {orbitElements[index: i]},",
						15 => $"\t\"ObservationSpan\": \"{orbitElements[index: i]}\",",
						16 => $"\t\"RmsResidual\": {orbitElements[index: i]},",
						17 => $"\t\"ComputerName\": \"{orbitElements[index: i]}\",",
						18 => $"\t\"FourHexdigitFlags\": \"{orbitElements[index: i]}\",",
						19 => $"\t\"DateOfLastObservation\": \"{orbitElements[index: i]}\",",
						20 => $"\t\"LinearEccentricity\": {orbitElements[index: i]},",
						21 => $"\t\"SemiMinorAxis\": {orbitElements[index: i]},",
						22 => $"\t\"MajorAxis\": {orbitElements[index: i]},",
						23 => $"\t\"MinorAxis\": {orbitElements[index: i]},",
						24 => $"\t\"EccentricAnomaly\": {orbitElements[index: i]},",
						25 => $"\t\"TrueAnomaly\": {orbitElements[index: i]},",
						26 => $"\t\"PerihelionDistance\": {orbitElements[index: i]},",
						27 => $"\t\"AphelionDistance\": {orbitElements[index: i]},",
						28 => $"\t\"LongitudeOfDescendingNode\": {orbitElements[index: i]},",
						29 => $"\t\"ArgumentOfAphelion\": {orbitElements[index: i]},",
						30 => $"\t\"FocalParameter\": {orbitElements[index: i]},",
						31 => $"\t\"SemiLatusRectum\": {orbitElements[index: i]},",
						32 => $"\t\"LatusRectum\": {orbitElements[index: i]},",
						33 => $"\t\"OrbitalPeriod\": {orbitElements[index: i]},",
						34 => $"\t\"OrbitalArea\": {orbitElements[index: i]},",
						35 => $"\t\"OrbitalPerimeter\": {orbitElements[index: i]},",
						36 => $"\t\"SemiMeanAxis\": {orbitElements[index: i]},",
						37 => $"\t\"MeanAxis\": {orbitElements[index: i]},",
						38 => $"\t\"StandardGravitationalParameter\": {orbitElements[index: i]}",
						_ => string.Empty
					});
				}
			}
			_ = sb.AppendLine(value: "}");
			streamWriter.Write(value: sb.ToString());
		}

		/// <summary>
		/// Handles the Click event of the ButtonMarkAll control.
		/// Marks all items in the list.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonMarkAll_Click(object sender, EventArgs e) => MarkAll();

		/// <summary>
		/// Handles the Click event of the ButtonUnmarkAll control.
		/// Unmarks all items in the list.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ButtonUnmarkAll_Click(object sender, EventArgs e) => UnmarkAll();

		#endregion

		#region ItemCheck event handler

		/// <summary>
		/// Handles the ItemCheck event of the CheckedListBoxOrbitalElements control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void CheckedListBoxOrbitalElements_ItemCheck(object sender, ItemCheckEventArgs e)
		{
		}

		#endregion

		#region SelectedIndexChanged event handler

		/// <summary>
		/// Handles the SelectedIndexChanged event of the CheckedListBoxOrbitalElements control.
		/// Enables or disables export buttons based on whether all items are unmarked.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void CheckedListBoxOrbitalElements_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Enable or disable the export buttons based on whether all items are unmarked
			// If all items are unmarked, disable the export buttons
			// If not all items are unmarked, enable the export buttons
			buttonExportAsTxt.Enabled = IsAllUnmarked()
				? (buttonExportAsHtml.Enabled = buttonExportAsXml.Enabled = buttonExportAsJson.Enabled = false)
				: (buttonExportAsHtml.Enabled = buttonExportAsXml.Enabled = buttonExportAsJson.Enabled = true);
		}

		#endregion
	}
}
