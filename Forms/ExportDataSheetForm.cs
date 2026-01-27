using Planetoid_DB.Forms;

using System.Diagnostics;
using System.IO;
using System.Text;

namespace Planetoid_DB;

/// <summary>
/// Form for exporting data sheets with various formats.
/// </summary>
/// <remarks>
/// This form allows users to select orbital elements and export them in different formats.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class ExportDataSheetForm : BaseKryptonForm
{
	/// <summary>
	/// List of orbit elements to be exported
	/// </summary>
	/// <remarks>
	/// This list contains the names of the orbital elements that the user has selected for export.
	/// </remarks>
	private List<string> orbitElements = [];

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="ExportDataSheetForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public ExportDataSheetForm() =>
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
	/// This method is used to set the status bar text and enable the information label.
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

	/// <summary>
	/// Sets the internal list of orbit elements that will be used for export operations.
	/// </summary>
	/// <param name="list">A list of orbit element values (strings). The list is stored by reference.</param>
	/// <remarks>
	/// This method is used to set the internal list of orbit elements that will be used for export operations.
	/// </remarks>
	public void SetDatabase(List<string> list) => orbitElements = list;

	/// <summary>
	/// Checks or unchecks all items in the orbital elements checklist and toggles export buttons.
	/// </summary>
	/// <param name="check">If true, all items are checked; if false, all items are unchecked.</param>
	/// <remarks>
	/// This method is used to check or uncheck all items in the orbital elements checklist
	/// and toggle the export buttons accordingly.
	/// </remarks>
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
	/// Checks all items in the orbital elements checklist.
	/// </summary>
	/// <remarks>
	/// This method is used to mark all items in the orbital elements checklist.
	/// </remarks>
	private void MarkAll() => CheckIt(check: true);

	/// <summary>
	/// Unchecks all items in the orbital elements checklist.
	/// </summary>
	/// <remarks>
	/// This method is used to unmark all items in the orbital elements checklist.
	/// </remarks>
	private void UnmarkAll() => CheckIt(check: false);

	/// <summary>
	/// Determines whether all items in the orbital elements checklist are unmarked (unchecked).
	/// </summary>
	/// <returns><c>true</c> if every item is unchecked; otherwise <c>false</c>.</returns>
	/// <remarks>
	/// This method is used to determine whether all items in the orbital elements checklist are unmarked (unchecked).
	/// </remarks>
	private bool IsAllUnmarked()
	{
		// Check if all items in the checked list box are unmarked
		// and return true if they are, otherwise return false
		return checkedListBoxOrbitalElements.Items.OfType<object>()
			.Select(selector: item => item.ToString() ?? string.Empty)
			.Select(selector: itemString => checkedListBoxOrbitalElements.GetItemChecked(index: checkedListBoxOrbitalElements.FindStringExact(str: itemString)))
			.All(predicate: isChecked => !isChecked);
	}

	#endregion

	#region form event handlers

	/// <summary>
	/// Fired when the export form loads.
	/// Clears the status area and selects all available orbital elements by default.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to initialize the form and set up any necessary data.
	/// </remarks>
	private void ExportDataSheetForm_Load(object sender, EventArgs e)
	{
		ClearStatusBar(); // Clear the status bar text
		MarkAll(); // Mark all items in the list
	}

	/// <summary>
	/// Fired when the export form is closed.
	/// Releases managed resources and disposes the form instance.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to release any resources held by the form.
	/// </remarks>
	private void ExportDataSheetForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

	#endregion

	#region Enter event handlers

	/// <summary>
	/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
	/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is used to set the status bar text when a control or ToolStrip item is focused.
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
		// If a description is available, set it in the status bar
		if (description != null)
		{
			SetStatusBar(text: description);
		}
	}

	#endregion

	#region Leave event handlers

	/// <summary>
	/// Called when the mouse pointer leaves a control or the control loses focus.
	/// Clears the status bar text (delegates to <see cref="ClearStatusBar"/>).
	/// </summary>
	/// <param name="sender">Event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is used to clear the status bar text when the mouse pointer leaves a control or the control loses focus.
	/// </remarks>
	private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

	#endregion

	#region Click & ButtonClick event handlers

	/// <summary>
	/// Handles the Click event of the Export As TXT button.
	/// Prompts the user for a destination file, then writes each checked orbital element
	/// and its corresponding value as plain text lines in the format "Label: Value".
	/// </summary>
	/// <param name="sender">Event source (the export button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to export the selected orbital elements as a plain text file.
	/// </remarks>
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
	/// Handles the Click event of the Export As HTML button.
	/// Prompts the user for a destination file, then writes each checked orbital element
	/// and its corresponding value as HTML lines in the format "<p>Label: Value</p>".
	/// </summary>
	/// <param name="sender">Event source (the export button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to export the selected orbital elements as an HTML file.
	/// </remarks>
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
	/// Handles the Click event of the Export As XML button.
	/// Prompts the user for a destination file, then writes each checked orbital element
	/// and its corresponding value as XML lines in the format "Label: Value".
	/// </summary>
	/// <param name="sender">Event source (the export button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to export the selected orbital elements as an XML file.
	/// </remarks>
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
	/// Handles the Click event of the Export As JSON button.
	/// Prompts the user for a destination file, then writes each checked orbital element
	/// and its corresponding value as JSON lines in the format "\"Label\": \"Value\"".
	/// </summary>
	/// <param name="sender">Event source (the export button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to export the selected orbital elements as JSON.
	/// </remarks>
	private void ButtonExportAsJson_Click(object sender, EventArgs e)
	{
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogJson.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogJson.FileName = $"{orbitElements[index: 0]}.{saveFileDialogJson.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogJson.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Create a new StreamWriter to write the JSON content to the specified file
		using StreamWriter streamWriter = new(path: saveFileDialogJson.FileName);
		// Create a StringBuilder to build the JSON content
		StringBuilder sb = new();
		// Append the JSON content to the StringBuilder
		_ = sb.AppendLine(value: "{");
		// Append the orbit elements to the JSON content
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			// Check if the item is checked
			// If it is checked, append the orbit element to the JSON content
			if (checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				// Append the orbit element to the JSON content
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
					_ => string.Empty // Default case if no match is found
				});
			}
		}
		// Append the closing tag for the JSON content
		_ = sb.AppendLine(value: "}");
		// Write the JSON content to the file
		streamWriter.Write(value: sb.ToString());
	}

	/// <summary>
	/// Handles the Click event of the Mark All button.
	/// Marks all items in the orbital elements checklist and enables export buttons.
	/// </summary>
	/// <param name="sender">Event source (the Mark All button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to mark all items in the orbital elements checklist.
	/// </remarks>
	private void ButtonMarkAll_Click(object sender, EventArgs e) => MarkAll();

	/// <summary>
	/// Handles the Click event of the Unmark All button.
	/// Unmarks all items in the orbital elements checklist and disables export buttons.
	/// </summary>
	/// <param name="sender">Event source (the Unmark All button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to unmark all items in the orbital elements checklist.
	/// </remarks>
	private void ButtonUnmarkAll_Click(object sender, EventArgs e) => UnmarkAll();

	#endregion

	#region SelectedIndexChanged event handlers

	/// <summary>
	/// Handles the SelectedIndexChanged event of the orbital elements checklist.
	/// Enables or disables the export buttons depending on whether any items are checked.
	/// If all items are unmarked (unchecked) the export buttons are disabled; otherwise they are enabled.
	/// </summary>
	/// <param name="sender">Event source (the checked list box).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to enable or disable the export buttons based on the selection state of the orbital elements.
	/// </remarks>
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