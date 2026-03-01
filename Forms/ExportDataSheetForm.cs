// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Planetoid_DB.Forms;

using System.Diagnostics;
using System.IO.Compression;
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

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelStatus;

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
		toolStripDropDownButtonExport.Enabled = !IsAllUnmarked();
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
		ClearStatusBar(label: labelStatus); // Clear the status bar text
		MarkAll(); // Mark all items in the list
	}

	#endregion

	#region Click & ButtonClick event handlers

	/// <summary>
	/// Handles the Click event of the Mark All tool strip button.
	/// Marks all items in the orbital elements checklist.
	/// </summary>
	/// <param name="sender">Event source (the Mark All tool strip button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to mark all items in the orbital elements checklist.
	/// </remarks>
	private void ToolStripButtonMarkAll_Click(object sender, EventArgs e) => MarkAll();

	/// <summary>
	/// Handles the Click event of the Unmark All tool strip button.
	/// Unmarks all items in the orbital elements checklist.
	/// </summary>
	/// <param name="sender">Event source (the Unmark All tool strip button).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to unmark all items in the orbital elements checklist.
	/// </remarks>
	private void ToolStripButtonUnmarkAll_Click(object sender, EventArgs e) => UnmarkAll();

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
		toolStripDropDownButtonExport.Enabled = !IsAllUnmarked();
	}

	#endregion

	/// <summary>
	/// Handles the Click event of the Export As Text menu item.
	/// Exports the selected orbital elements to a text file.
	/// </summary>
	/// <param name="sender">Event source (the Export As Text menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to export the selected orbital elements to a text file.
	/// </remarks>
	private void ToolStripMenuItemExportAsText_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported text file
		using SaveFileDialog saveFileDialogText = new()
		{
			Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
			DefaultExt = "txt",
			Title = "Save database information as text"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogText.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogText.FileName = $"{orbitElements[index: 0]}.{saveFileDialogText.DefaultExt}";
		// Show the save file dialog to select the file path and name
		// If the user selects a file, proceed with exporting
		if (saveFileDialogText.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Create a new StreamWriter to write the text content to the specified file
		using StreamWriter streamWriter = new(path: saveFileDialogText.FileName);
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
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the Click event of the Export As LaTeX menu item.
	/// Exports the selected orbital elements to a LaTeX file.
	/// </summary>
	/// <param name="sender">Event source (the Export As LaTeX menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to export the selected orbital elements to a LaTeX file.
	/// </remarks>
	private void ToolStripMenuItemExportAsLatex_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported LaTeX file
		using SaveFileDialog saveFileDialogLatex = new()
		{
			Filter = "LaTeX files (*.tex)|*.tex|All files (*.*)|*.*",
			DefaultExt = "tex",
			Title = "Save database information as LaTeX"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogLatex.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogLatex.FileName = $"{orbitElements[index: 0]}.{saveFileDialogLatex.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogLatex.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Create a new StreamWriter to write the LaTeX content to the specified file
		using StreamWriter streamWriter = new(path: saveFileDialogLatex.FileName);
		// Create a StringBuilder to build the LaTeX content
		StringBuilder sb = new();
		// Append the LaTeX content to the StringBuilder
		_ = sb.AppendLine(value: "\\documentclass{article}");
		_ = sb.AppendLine(value: "\\usepackage[utf8]{inputenc}");
		_ = sb.AppendLine(value: "\\usepackage{amsmath}");
		_ = sb.AppendLine(value: "\\usepackage{amsfonts}");
		_ = sb.AppendLine(value: "\\usepackage{amssymb}");
		_ = sb.AppendLine(value: "\\usepackage{geometry}");
		_ = sb.AppendLine(value: "\\geometry{a4paper, margin=1in}");
		_ = sb.AppendLine(handler: $"\\title{{Export for [{orbitElements[index: 0]}] {orbitElements[index: 1]}}}");
		_ = sb.AppendLine(value: "\\begin{document}");
		_ = sb.AppendLine(value: "\\maketitle");
		_ = sb.AppendLine(value: "\\begin{itemize}");
		// Append the orbit elements to the LaTeX content
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			// Check if the item is checked
			// If it is checked, append the orbit element to the LaTeX content
			if (checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				// Append the orbit element to the LaTeX content
				_ = sb.AppendLine(handler: $"\t\\item \\textbf{{{checkedListBoxOrbitalElements.Items[index: i]}:}} {orbitElements[index: i]}");
			}
		}
		// Append the closing tags for the LaTeX content
		_ = sb.AppendLine(value: "\\end{itemize}");
		_ = sb.AppendLine(value: "\\end{document}");
		// Write the LaTeX content to the file
		streamWriter.Write(value: sb.ToString());
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the Click event of the Export As Markdown menu item.
	/// Exports the selected orbital elements to a Markdown file.
	/// </summary>
	/// <param name="sender">Event source (the Export As Markdown menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to export the selected orbital elements to a Markdown file.
	/// </remarks>
	private void ToolStripMenuItemExportAsMarkdown_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported Markdown file
		using SaveFileDialog saveFileDialogMarkdown = new()
		{
			Filter = "Markdown files (*.md)|*.md|All files (*.*)|*.*",
			DefaultExt = "md",
			Title = "Save database information as Markdown"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogMarkdown.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogMarkdown.FileName = $"{orbitElements[index: 0]}.{saveFileDialogMarkdown.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogMarkdown.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Create a new StreamWriter to write the Markdown content to the specified file
		using StreamWriter streamWriter = new(path: saveFileDialogMarkdown.FileName);
		// Create a StringBuilder to build the Markdown content
		StringBuilder sb = new();
		// Write the Markdown header
		_ = sb.AppendLine(value: $"# Export for [{orbitElements[index: 0]}] {orbitElements[index: 1]}");
		_ = sb.AppendLine();
		// Append the orbit elements to the Markdown content
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			// Check if the item is checked
			// If it is checked, append the orbit element to the Markdown content
			if (checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				// Append the orbit element to the Markdown content
				_ = sb.AppendLine(value: $"* **{checkedListBoxOrbitalElements.Items[index: i]}:** {orbitElements[index: i]}");
			}
		}
		// Write the Markdown content to the file
		streamWriter.Write(value: sb.ToString());
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the Click event of the Export As Word menu item.
	/// Exports the selected orbital elements to a Word document.
	/// </summary>
	/// <param name="sender">Event source (the Export As Word menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This method is used to export the selected orbital elements to a Word document.
	/// </remarks>
	private void ToolStripMenuItemExportAsWord_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported Word document
		using SaveFileDialog saveFileDialogWord = new()
		{
			Filter = "Word files (*.docx)|*.docx|All files (*.*)|*.*",
			DefaultExt = "docx",
			Title = "Save database information as Word document"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogWord.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogWord.FileName = $"{orbitElements[index: 0]}.{saveFileDialogWord.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogWord.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Helper method to escape special XML characters in the content
		static string EscapeXml(string value) => value
			.Replace(oldValue: "&", newValue: "&amp;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "<", newValue: "&lt;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: ">", newValue: "&gt;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "\"", newValue: "&quot;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "'", newValue: "&apos;", comparisonType: StringComparison.Ordinal);
		// Create a list of lines to be included in the Word document
		List<string> lines = [];
		lines.Add(item: $"Export for [{orbitElements[index: 0]}] {orbitElements[index: 1]}");
		lines.Add(item: string.Empty);
		// Append the selected orbital elements to the list of lines
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			if (!checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				continue;
			}
			lines.Add(item: $"{checkedListBoxOrbitalElements.Items[index: i]}: {orbitElements[index: i]}");
		}
		// If no elements are selected, add a line indicating that no elements are selected
		if (lines.Count == 2)
		{
			lines.Add(item: "No selected elements.");
		}
		// Create a StringBuilder to build the XML content for the Word document
		StringBuilder paragraphs = new();
		// Append each line as a separate paragraph in the Word document XML
		foreach (string line in lines)
		{
			_ = paragraphs.Append(value: "<w:p><w:r><w:t xml:space=\"preserve\">");
			_ = paragraphs.Append(value: EscapeXml(value: line));
			_ = paragraphs.Append(value: "</w:t></w:r></w:p>");
		}
		// Define the XML content for the content types of the Word document
		string contentTypesXml = """
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
				<Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
				<Default Extension="xml" ContentType="application/xml"/>
				<Override PartName="/word/document.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"/>
			</Types>
			""";
		// Define the XML content for the root relationships of the Word document
		string rootRelsXml = """
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
				<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="word/document.xml"/>
			</Relationships>
			""";
		// Define the XML content for the main document of the Word document, including the paragraphs with the orbital elements
		string documentXml = $"""
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<w:document xmlns:wpc="http://schemas.microsoft.com/office/word/2010/wordprocessingCanvas"
				xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
				xmlns:o="urn:schemas-microsoft-com:office:office"
				xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships"
				xmlns:m="http://schemas.openxmlformats.org/officeDocument/2006/math"
				xmlns:v="urn:schemas-microsoft-com:vml"
				xmlns:wp14="http://schemas.microsoft.com/office/word/2010/wordprocessingDrawing"
				xmlns:wp="http://schemas.openxmlformats.org/drawingml/2006/wordprocessingDrawing"
				xmlns:w10="urn:schemas-microsoft-com:office:word"
				xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main"
				xmlns:w14="http://schemas.microsoft.com/office/word/2010/wordml"
				xmlns:wpg="http://schemas.microsoft.com/office/word/2010/wordprocessingGroup"
				xmlns:wpi="http://schemas.microsoft.com/office/word/2010/wordprocessingInk"
				xmlns:wne="http://schemas.microsoft.com/office/word/2006/wordml"
				xmlns:wps="http://schemas.microsoft.com/office/word/2010/wordprocessingShape"
				mc:Ignorable="w14 wp14">
				<w:body>
					{paragraphs}
					<w:sectPr>
						<w:pgSz w:w="11906" w:h="16838"/>
						<w:pgMar w:top="1440" w:right="1440" w:bottom="1440" w:left="1440" w:header="708" w:footer="708" w:gutter="0"/>
						<w:cols w:space="708"/>
						<w:docGrid w:linePitch="360"/>
					</w:sectPr>
				</w:body>
			</w:document>
			""";
		// Create a new FileStream to write the Word document content to the specified file
		using FileStream fileStream = new(path: saveFileDialogWord.FileName, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None);
		// Create a new ZipArchive to create the Word document as a ZIP file containing the necessary XML parts
		using ZipArchive archive = new(stream: fileStream, mode: ZipArchiveMode.Create);
		// Helper method to add an entry to the ZIP archive with the specified name and content
		void AddEntry(string entryName, string content)
		{
			ZipArchiveEntry entry = archive.CreateEntry(entryName: entryName, compressionLevel: CompressionLevel.Optimal);
			using StreamWriter writer = new(stream: entry.Open(), encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			writer.Write(value: content);
		}
		// Add the necessary XML parts to the ZIP archive to create a valid Word document
		AddEntry(entryName: "[Content_Types].xml", content: contentTypesXml);
		AddEntry(entryName: "_rels/.rels", content: rootRelsXml);
		AddEntry(entryName: "word/document.xml", content: documentXml);
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for exporting data as an ODT document.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>
	/// This method handles the export of data as an ODT document.
	/// </remarks>
	private void ToolStripMenuItemExportAsOdt_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported ODT document
		using SaveFileDialog saveFileDialogOdt = new()
		{
			Filter = "ODT files (*.odt)|*.odt|All files (*.*)|*.*",
			DefaultExt = "odt",
			Title = "Save database information as ODT document"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogOdt.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogOdt.FileName = $"{orbitElements[index: 0]}.{saveFileDialogOdt.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogOdt.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Helper method to escape special XML characters in the content
		static string EscapeXml(string value) => value
			.Replace(oldValue: "&", newValue: "&amp;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "<", newValue: "&lt;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: ">", newValue: "&gt;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "\"", newValue: "&quot;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "'", newValue: "&apos;", comparisonType: StringComparison.Ordinal);
		// Create a StringBuilder to build the XML content for the ODT document
		StringBuilder paragraphs = new();
		_ = paragraphs.AppendLine(value: $"<text:h text:outline-level=\"1\"><text:span text:style-name=\"T1\">{EscapeXml(value: $"Export for [{orbitElements[index: 0]}] {orbitElements[index: 1]}")}</text:span></text:h>");
		// Append the selected orbital elements to the XML content for the ODT document
		bool hasSelectedElements = false;
		// Iterate through the items in the checked list box and append the checked items to the XML content
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			if (!checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				continue;
			}
			// Mark that there are selected elements to be included in the ODT document
			hasSelectedElements = true;
			// Append the checked item and its corresponding orbit element value to the XML content for the ODT document
			string elementName = EscapeXml(value: checkedListBoxOrbitalElements.Items[index: i].ToString() ?? string.Empty);
			string elementValue = EscapeXml(value: orbitElements[index: i]);
			_ = paragraphs.AppendLine(value: $"<text:p><text:span text:style-name=\"T1\">{elementName}:</text:span> {elementValue}</text:p>");
		}
		// If no elements are selected, add a paragraph indicating that no elements are selected
		if (!hasSelectedElements)
		{
			_ = paragraphs.AppendLine(value: "<text:p>No selected elements.</text:p>");
		}
		// Define the XML content for the content types of the ODT document
		string contentXml = $"""
			<?xml version="1.0" encoding="UTF-8"?>
			<office:document-content
				xmlns:office="urn:oasis:names:tc:opendocument:xmlns:office:1.0"
				xmlns:text="urn:oasis:names:tc:opendocument:xmlns:text:1.0"
				xmlns:style="urn:oasis:names:tc:opendocument:xmlns:style:1.0"
				office:version="1.2">
				<office:automatic-styles>
					<style:style style:name="T1" style:family="text">
						<style:text-properties fo:font-weight="bold" xmlns:fo="urn:oasis:names:tc:opendocument:xmlns:xsl-fo-compatible:1.0"/>
					</style:style>
				</office:automatic-styles>
				<office:body>
					<office:text>
						{paragraphs}
					</office:text>
				</office:body>
			</office:document-content>
			""";
		// Define the XML content for the styles of the ODT document
		string stylesXml = """
			<?xml version="1.0" encoding="UTF-8"?>
			<office:document-styles
				xmlns:office="urn:oasis:names:tc:opendocument:xmlns:office:1.0"
				office:version="1.2">
				<office:styles/>
			</office:document-styles>
			""";
		// Define the XML content for the meta information of the ODT document
		string metaXml = """
			<?xml version="1.0" encoding="UTF-8"?>
			<office:document-meta
				xmlns:office="urn:oasis:names:tc:opendocument:xmlns:office:1.0"
				xmlns:meta="urn:oasis:names:tc:opendocument:xmlns:meta:1.0"
				office:version="1.2">
				<office:meta>
					<meta:generator>Planetoid-DB</meta:generator>
				</office:meta>
			</office:document-meta>
			""";
		// Define the XML content for the settings of the ODT document
		string settingsXml = """
			<?xml version="1.0" encoding="UTF-8"?>
			<office:document-settings
				xmlns:office="urn:oasis:names:tc:opendocument:xmlns:office:1.0"
				office:version="1.2">
				<office:settings/>
			</office:document-settings>
			""";
		// Define the XML content for the manifest of the ODT document, which lists the files included in the ODT package
		string manifestXml = """
			<?xml version="1.0" encoding="UTF-8"?>
			<manifest:manifest xmlns:manifest="urn:oasis:names:tc:opendocument:xmlns:manifest:1.0" manifest:version="1.2">
				<manifest:file-entry manifest:full-path="/" manifest:media-type="application/vnd.oasis.opendocument.text"/>
				<manifest:file-entry manifest:full-path="content.xml" manifest:media-type="text/xml"/>
				<manifest:file-entry manifest:full-path="styles.xml" manifest:media-type="text/xml"/>
				<manifest:file-entry manifest:full-path="meta.xml" manifest:media-type="text/xml"/>
				<manifest:file-entry manifest:full-path="settings.xml" manifest:media-type="text/xml"/>
			</manifest:manifest>
			""";
		// Create a new FileStream to write the ODT document content to the specified file
		using FileStream fileStream = new(path: saveFileDialogOdt.FileName, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None);
		// Create a new ZipArchive to create the ODT document as a ZIP file containing the necessary XML parts
		using ZipArchive archive = new(stream: fileStream, mode: ZipArchiveMode.Create);
		// Helper method to add an entry to the ZIP archive with the specified name, content, and compression level
		void AddEntry(string entryName, string content, CompressionLevel compressionLevel = CompressionLevel.Optimal)
		{
			ZipArchiveEntry entry = archive.CreateEntry(entryName, compressionLevel);
			using StreamWriter writer = new(stream: entry.Open(), encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			writer.Write(value: content);
		}
		// Add the necessary XML parts to the ZIP archive to create a valid ODT document
		AddEntry(entryName: "mimetype", content: "application/vnd.oasis.opendocument.text", compressionLevel: CompressionLevel.NoCompression);
		AddEntry(entryName: "content.xml", content: contentXml);
		AddEntry(entryName: "styles.xml", content: stylesXml);
		AddEntry(entryName: "meta.xml", content: metaXml);
		AddEntry(entryName: "settings.xml", content: settingsXml);
		AddEntry(entryName: "META-INF/manifest.xml", content: manifestXml);
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event to export selected orbital element data as a Rich Text Format (RTF) document.
	/// </summary>
	/// <remarks>This method displays a save file dialog allowing the user to specify the location and name of the
	/// RTF file. It formats the selected orbital elements into RTF and writes the content to the chosen file. If no
	/// elements are selected, the output will indicate this in the document.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs instance containing the event data.</param>
	private void ToolStripMenuItemExportAsRtf_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported RTF document
		using SaveFileDialog saveFileDialogRtf = new()
		{
			Filter = "RTF files (*.rtf)|*.rtf|All files (*.*)|*.*",
			DefaultExt = "rtf",
			Title = "Save database information as RTF document"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogRtf.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogRtf.FileName = $"{orbitElements[index: 0]}.{saveFileDialogRtf.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogRtf.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Helper method to escape special characters in RTF format
		static string EscapeRtf(string value) => value
			.Replace(oldValue: "\\", newValue: "\\\\", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "{", newValue: "\\{", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "}", newValue: "\\}", comparisonType: StringComparison.Ordinal);
		// Create a StringBuilder to build the RTF content for the document
		StringBuilder sb = new();
		_ = sb.AppendLine(value: "{\\rtf1\\ansi\\deff0");
		_ = sb.AppendLine(value: $"\\b {EscapeRtf(value: $"Export for [{orbitElements[index: 0]}] {orbitElements[index: 1]}")}\\b0\\par");
		_ = sb.AppendLine(value: "\\par");
		// Append the selected orbital elements to the RTF content
		bool hasSelectedElements = false;
		// Iterate through the items in the checked list box and append the checked items to the RTF content
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			if (!checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				continue;
			}
			// Mark that there are selected elements to be included in the RTF document
			hasSelectedElements = true;
			string elementName = EscapeRtf(value: checkedListBoxOrbitalElements.Items[index: i].ToString() ?? string.Empty);
			string elementValue = EscapeRtf(value: orbitElements[index: i]);
			// Append the checked item and its corresponding orbit element value to the RTF content for the document
			_ = sb.AppendLine(value: $"\\b {elementName}:\\b0 {elementValue}\\par");
		}
		// If no elements are selected, add a line indicating that no elements are selected
		if (!hasSelectedElements)
		{
			_ = sb.AppendLine(value: "No selected elements.\\par");
		}
		// Append the closing brace for the RTF content
		_ = sb.Append(value: "}");
		// Create a new StreamWriter to write the RTF content to the specified file
		using StreamWriter streamWriter = new(path: saveFileDialogRtf.FileName);
		// Write the RTF content to the file
		streamWriter.Write(value: sb.ToString());
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event to export selected orbital element data to an Excel file in .xlsx format.
	/// </summary>
	/// <remarks>This method displays a save file dialog allowing the user to specify the location and name of the
	/// Excel file. It generates an Excel document containing the selected orbital elements from the list, formatted as
	/// XML. If no elements are selected, the output will indicate this in the exported file.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs instance containing the event data.</param>
	private void ToolStripMenuItemExportAsExcel_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported Excel document
		using SaveFileDialog saveFileDialogExcel = new()
		{
			Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
			DefaultExt = "xlsx",
			Title = "Save database information as Excel document"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogExcel.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogExcel.FileName = $"{orbitElements[index: 0]}.{saveFileDialogExcel.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogExcel.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Helper method to escape special XML characters in the content
		static string EscapeXml(string value) => value
			.Replace(oldValue: "&", newValue: "&amp;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "<", newValue: "&lt;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: ">", newValue: "&gt;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "\"", newValue: "&quot;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "'", newValue: "&apos;", comparisonType: StringComparison.Ordinal);
		// Create a StringBuilder to build the rows of the Excel sheet
		StringBuilder rows = new();
		_ = rows.AppendLine(value: "<row r=\"1\"><c r=\"A1\" t=\"inlineStr\"><is><t>Element</t></is></c><c r=\"B1\" t=\"inlineStr\"><is><t>Value</t></is></c></row>");
		// Append the selected orbital elements to the rows of the Excel sheet
		int excelRow = 2;
		bool hasSelectedElements = false;
		// Iterate through the items in the checked list box and append the checked items to the rows of the Excel sheet
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			if (!checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				continue;
			}
			// Mark that there are selected elements to be included in the Excel document
			hasSelectedElements = true;
			string elementName = EscapeXml(value: checkedListBoxOrbitalElements.Items[index: i].ToString() ?? string.Empty);
			string elementValue = EscapeXml(value: orbitElements[index: i]);
			// Append the checked item and its corresponding orbit element value as a new row in the Excel sheet XML content
			_ = rows.AppendLine(value: $"<row r=\"{excelRow}\"><c r=\"A{excelRow}\" t=\"inlineStr\"><is><t>{elementName}</t></is></c><c r=\"B{excelRow}\" t=\"inlineStr\"><is><t>{elementValue}</t></is></c></row>");
			excelRow++;
		}
		// If no elements are selected, add a row indicating that no elements are selected
		if (!hasSelectedElements)
		{
			_ = rows.AppendLine(value: "<row r=\"2\"><c r=\"A2\" t=\"inlineStr\"><is><t>No selected elements.</t></is></c></row>");
		}
		// Define the XML content for the content types of the Excel document
		string contentTypesXml = """
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
				<Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
				<Default Extension="xml" ContentType="application/xml"/>
				<Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>
				<Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
			</Types>
			""";
		// Define the XML content for the root relationships of the Excel document
		string rootRelsXml = """
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
				<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>
			</Relationships>
			""";
		// Define the XML content for the workbook of the Excel document, which references the worksheet containing the orbital elements
		string workbookXml = """
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
				<sheets>
					<sheet name="Export" sheetId="1" r:id="rId1"/>
				</sheets>
			</workbook>
			""";
		// Define the XML content for the workbook relationships of the Excel document, which defines the relationship to the worksheet containing the orbital elements
		string workbookRelsXml = """
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
				<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>
			</Relationships>
			""";
		// Define the XML content for the worksheet of the Excel document, which contains the rows with the selected orbital elements
		string worksheetXml = $"""
			<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
			<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
				<sheetData>
					{rows}
				</sheetData>
			</worksheet>
			""";
		// Create a new FileStream to write the Excel document content to the specified file
		using FileStream fileStream = new(path: saveFileDialogExcel.FileName, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None);
		// Create a new ZipArchive to create the Excel document as a ZIP file containing the necessary XML parts
		using ZipArchive archive = new(stream: fileStream, mode: ZipArchiveMode.Create);
		// Helper method to add an entry to the ZIP archive with the specified name and content
		void AddEntry(string entryName, string content)
		{
			ZipArchiveEntry entry = archive.CreateEntry(entryName: entryName, compressionLevel: CompressionLevel.Optimal);
			using StreamWriter writer = new(stream: entry.Open(), encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			writer.Write(value: content);
		}
		// Add the necessary XML parts to the ZIP archive to create a valid Excel document
		AddEntry(entryName: "[Content_Types].xml", content: contentTypesXml);
		AddEntry(entryName: "_rels/.rels", content: rootRelsXml);
		AddEntry(entryName: "xl/workbook.xml", content: workbookXml);
		AddEntry(entryName: "xl/_rels/workbook.xml.rels", content: workbookRelsXml);
		AddEntry(entryName: "xl/worksheets/sheet1.xml", content: worksheetXml);
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for exporting data as an ODS file.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>
	/// This method creates an ODS file containing the selected orbital elements from the database.
	/// </remarks>
	private void ToolStripMenuItemExportAsOds_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported ODS file
		using SaveFileDialog saveFileDialogOds = new()
		{
			Filter = "ODS files (*.ods)|*.ods|All files (*.*)|*.*",
			DefaultExt = "ods",
			Title = "Save database information as ODS"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogOds.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial file name for the save file dialog
		saveFileDialogOds.FileName = $"{orbitElements[index: 0]}.{saveFileDialogOds.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogOds.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Helper method to escape special XML characters in the content
		static string EscapeXml(string value) => value
			.Replace(oldValue: "&", newValue: "&amp;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "<", newValue: "&lt;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: ">", newValue: "&gt;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "\"", newValue: "&quot;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "'", newValue: "&apos;", comparisonType: StringComparison.Ordinal);
		// Create a StringBuilder to build the XML content for the ODS file, starting with the table rows for the selected orbital elements
		StringBuilder tableRows = new();
		_ = tableRows.AppendLine(value: "<table:table-row><table:table-cell office:value-type=\"string\"><text:p>Element</text:p></table:table-cell><table:table-cell office:value-type=\"string\"><text:p>Value</text:p></table:table-cell></table:table-row>");
		// Append the selected orbital elements to the XML content for the ODS file
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			if (!checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				continue;
			}
			// Append the checked item and its corresponding orbit element value as a new row in the XML content for the ODS file
			string elementName = EscapeXml(value: checkedListBoxOrbitalElements.Items[index: i].ToString() ?? string.Empty);
			string elementValue = EscapeXml(value: orbitElements[index: i]);
			// Append the checked item and its corresponding orbit element value as a new row in the XML content for the ODS file
			_ = tableRows.AppendLine(value: $"<table:table-row><table:table-cell office:value-type=\"string\"><text:p>{elementName}</text:p></table:table-cell><table:table-cell office:value-type=\"string\"><text:p>{elementValue}</text:p></table:table-cell></table:table-row>");
		}
		// If no elements are selected, add a row indicating that no elements are selected
		if (tableRows.Length == 0)
		{
			_ = tableRows.AppendLine(value: "<table:table-row><table:table-cell office:value-type=\"string\"><text:p>No elements selected</text:p></table:table-cell></table:table-row>");
		}
		// Define the XML content for the content of the ODS file, including the table with the selected orbital elements
		string contentXml = $"""
			<?xml version="1.0" encoding="UTF-8"?>
			<office:document-content
				xmlns:office="urn:oasis:names:tc:opendocument:xmlns:office:1.0"
				xmlns:table="urn:oasis:names:tc:opendocument:xmlns:table:1.0"
				xmlns:text="urn:oasis:names:tc:opendocument:xmlns:text:1.0"
				office:version="1.2">
				<office:body>
					<office:spreadsheet>
						<table:table table:name="Export">
							{tableRows}
						</table:table>
					</office:spreadsheet>
				</office:body>
			</office:document-content>
			""";
		// Define the XML content for the styles of the ODS file
		string stylesXml = """
			<?xml version="1.0" encoding="UTF-8"?>
			<office:document-styles
				xmlns:office="urn:oasis:names:tc:opendocument:xmlns:office:1.0"
				office:version="1.2">
				<office:styles/>
			</office:document-styles>
			""";
		// Define the XML content for the meta information of the ODS file
		string metaXml = """
			<?xml version="1.0" encoding="UTF-8"?>
			<office:document-meta
				xmlns:office="urn:oasis:names:tc:opendocument:xmlns:office:1.0"
				xmlns:meta="urn:oasis:names:tc:opendocument:xmlns:meta:1.0"
				office:version="1.2">
				<office:meta>
					<meta:generator>Planetoid-DB</meta:generator>
				</office:meta>
			</office:document-meta>
			""";
		// Define the XML content for the settings of the ODS file
		string settingsXml = """
			<?xml version="1.0" encoding="UTF-8"?>
			<office:document-settings
				xmlns:office="urn:oasis:names:tc:opendocument:xmlns:office:1.0"
				office:version="1.2">
				<office:settings/>
			</office:document-settings>
			""";
		// Define the XML content for the manifest of the ODS file
		string manifestXml = """
			<?xml version="1.0" encoding="UTF-8"?>
			<manifest:manifest xmlns:manifest="urn:oasis:names:tc:opendocument:xmlns:manifest:1.0" manifest:version="1.2">
				<manifest:file-entry manifest:full-path="/" manifest:media-type="application/vnd.oasis.opendocument.spreadsheet"/>
				<manifest:file-entry manifest:full-path="content.xml" manifest:media-type="text/xml"/>
				<manifest:file-entry manifest:full-path="styles.xml" manifest:media-type="text/xml"/>
				<manifest:file-entry manifest:full-path="meta.xml" manifest:media-type="text/xml"/>
				<manifest:file-entry manifest:full-path="settings.xml" manifest:media-type="text/xml"/>
			</manifest:manifest>
			""";
		// Create a new FileStream to write the ODS file content to the specified file
		using FileStream fileStream = new(path: saveFileDialogOds.FileName, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None);
		// Create a new ZipArchive to create the ODS file as a ZIP file containing the necessary XML parts
		using ZipArchive archive = new(stream: fileStream, mode: ZipArchiveMode.Create);
		// Helper method to add an entry to the ZIP archive with the specified name, content, and compression level
		void AddEntry(string entryName, string content, CompressionLevel compressionLevel = CompressionLevel.Optimal)
		{
			ZipArchiveEntry entry = archive.CreateEntry(entryName: entryName, compressionLevel: compressionLevel);
			using StreamWriter writer = new(stream: entry.Open(), encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			writer.Write(value: content);
		}
		// Add the necessary XML parts to the ZIP archive to create a valid ODS file
		AddEntry(entryName: "mimetype", content: "application/vnd.oasis.opendocument.spreadsheet", compressionLevel: CompressionLevel.NoCompression);
		AddEntry(entryName: "content.xml", content: contentXml);
		AddEntry(entryName: "styles.xml", content: stylesXml);
		AddEntry(entryName: "meta.xml", content: metaXml);
		AddEntry(entryName: "settings.xml", content: settingsXml);
		AddEntry(entryName: "META-INF/manifest.xml", content: manifestXml);
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the Click event of the ToolStripMenuItemExportAsCsv control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method allows the user to export the selected orbital elements as a CSV file.
	/// </remarks>
	private void ToolStripMenuItemExportAsCsv_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported CSV file
		using SaveFileDialog saveFileDialogCsv = new()
		{
			Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
			DefaultExt = "csv",
			Title = "Save database information as CSV"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogCsv.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogCsv.FileName = $"{orbitElements[index: 0]}.{saveFileDialogCsv.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogCsv.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Create a new StreamWriter to write the CSV content to the specified file
		using StreamWriter streamWriter = new(path: saveFileDialogCsv.FileName);
		// Write the orbit elements to the CSV file
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			// Check if the item is checked
			// If it is checked, write the orbit element to the CSV file
			if (!checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				continue;
			}
			// Write the orbit element to the CSV file
			streamWriter.Write(value: $"{checkedListBoxOrbitalElements.Items[index: i]};{orbitElements[index: i]}");
			// If it is not the last item, write a new line
			// to separate the elements in the CSV file
			if (i < checkedListBoxOrbitalElements.Items.Count - 1)
			{
				// Write a new line to separate the elements
				streamWriter.Write(value: Environment.NewLine);
			}
		}
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the Click event of the ToolStripMenuItemExportAsTsv control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method allows the user to export the selected orbital elements as a TSV file.
	/// </remarks>
	private void ToolStripMenuItemExportAsTsv_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported TSV file
		using SaveFileDialog saveFileDialogTsv = new()
		{
			Filter = "TSV files (*.tsv)|*.tsv|All files (*.*)|*.*",
			DefaultExt = "tsv",
			Title = "Save database information as TSV"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogTsv.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogTsv.FileName = $"{orbitElements[index: 0]}.{saveFileDialogTsv.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogTsv.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Create a new StreamWriter to write the TSV content to the specified file
		using StreamWriter streamWriter = new(path: saveFileDialogTsv.FileName);
		// Write the orbit elements to the TSV file
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			// Check if the item is checked
			// If it is checked, write the orbit element to the TSV file
			if (!checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				continue;
			}
			// Write the orbit element to the TSV file
			streamWriter.Write(value: $"{checkedListBoxOrbitalElements.Items[index: i]}\t{orbitElements[index: i]}");
			// If it is not the last item, write a new line
			// to separate the elements in the TSV file
			if (i < checkedListBoxOrbitalElements.Items.Count - 1)
			{
				// Write a new line to separate the elements
				streamWriter.Write(value: Environment.NewLine);
			}
		}
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the Click event of the ToolStripMenuItemExportAsPsv control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method allows the user to export the selected orbital elements as a PSV file.
	/// </remarks>
	private void ToolStripMenuItemExportAsPsv_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported PSV file
		using SaveFileDialog saveFileDialogPsv = new()
		{
			Filter = "PSV files (*.psv)|*.psv|All files (*.*)|*.*",
			DefaultExt = "psv",
			Title = "Save database information as PSV"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogPsv.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogPsv.FileName = $"{orbitElements[index: 0]}.{saveFileDialogPsv.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogPsv.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Create a new StreamWriter to write the PSV content to the specified file
		using StreamWriter streamWriter = new(path: saveFileDialogPsv.FileName);
		// Write the orbit elements to the PSV file
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			// Check if the item is checked
			// If it is checked, write the orbit element to the PSV file
			if (!checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				continue;
			}
			// Write the orbit element to the PSV file
			streamWriter.Write(value: $"{checkedListBoxOrbitalElements.Items[index: i]}|{orbitElements[index: i]}");
			// If it is not the last item, write a new line
			// to separate the elements in the PSV file
			if (i < checkedListBoxOrbitalElements.Items.Count - 1)
			{
				// Write a new line to separate the elements
				streamWriter.Write(value: Environment.NewLine);
			}
		}
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the Click event of the ToolStripMenuItemExportAsHtml control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method allows the user to export the selected orbital elements as an HTML file.
	/// </remarks>
	private void ToolStripMenuItemExportAsHtml_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported HTML file
		using SaveFileDialog saveFileDialogHtml = new()
		{
			Filter = "HTML files (*.html)|*.html|All files (*.*)|*.*",
			DefaultExt = "html",
			Title = "Save database information as HTML"
		};
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
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the Click event of the ToolStripMenuItemExportAsXml control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method allows the user to export the selected orbital elements as an XML file.
	/// </remarks>
	private void ToolStripMenuItemExportAsXml_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported XML file
		using SaveFileDialog saveFileDialogXml = new()
		{
			Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
			DefaultExt = "xml",
			Title = "Save database information as XML"
		};
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
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the Click event of the ToolStripMenuItemExportAsJson control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method allows the user to export the selected orbital elements as a JSON file.
	/// </remarks>
	private void ToolStripMenuItemExportAsJson_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported JSON file
		using SaveFileDialog saveFileDialogJson = new()
		{
			Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
			DefaultExt = "json",
			Title = "Save database information as JSON"
		};
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
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the Click event of the ToolStripMenuItemExportAsYaml control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method allows the user to export the selected orbital elements as a YAML file.
	/// </remarks>
	private void ToolStripMenuItemExportAsYaml_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported YAML file
		using SaveFileDialog saveFileDialogYaml = new()
		{
			Filter = "YAML files (*.yaml)|*.yaml|All files (*.*)|*.*",
			DefaultExt = "yaml",
			Title = "Save database information as YAML"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogYaml.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogYaml.FileName = $"{orbitElements[index: 0]}.{saveFileDialogYaml.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogYaml.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Create a new StreamWriter to write the YAML content to the specified file
		using StreamWriter streamWriter = new(path: saveFileDialogYaml.FileName);
		// Create a StringBuilder to build the YAML content
		StringBuilder sb = new();
		// Append the YAML content to the StringBuilder
		// Append the orbit elements to the YAML content
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			// Check if the item is checked
			// If it is checked, append the orbit element to the YAML content
			if (checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				// Append the orbit element to the YAML content
				_ = sb.AppendLine(value: $"{checkedListBoxOrbitalElements.Items[index: i]}: {orbitElements[index: i]}");
			}
		}
		// Write the YAML content to the file
		streamWriter.Write(value: sb.ToString());
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the Click event of the ToolStripMenuItemExportAsSql control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method allows the user to export the selected orbital elements as a SQL file.
	/// </remarks>
	private void ToolStripMenuItemExportAsSql_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported SQL file
		using SaveFileDialog saveFileDialogSql = new()
		{
			Filter = "SQL files (*.sql)|*.sql|All files (*.*)|*.*",
			DefaultExt = "sql",
			Title = "Save database information as SQL"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogSql.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogSql.FileName = $"{orbitElements[index: 0]}.{saveFileDialogSql.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogSql.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Create a new StreamWriter to write the SQL content to the specified file
		using StreamWriter streamWriter = new(path: saveFileDialogSql.FileName);
		StringBuilder sb = new();
		// Append the SQL content to the StringBuilder
		_ = sb.AppendLine(value: "INSERT INTO MinorPlanets (");
		// Append the column names to the SQL content
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			// Check if the item is checked
			// If it is checked, append the column name to the SQL content
			if (checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				// Append the column name to the SQL content
				_ = sb.AppendLine(value: $"{checkedListBoxOrbitalElements.Items[index: i]},");
			}
		}
		// Append the closing parenthesis for the column names
		_ = sb.AppendLine(value: ") VALUES (");
		// Append the values to the SQL content
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			// Check if the item is checked
			// If it is checked, append the value to the SQL content
			if (checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				// Append the value to the SQL content
				_ = sb.AppendLine(value: $"'{orbitElements[index: i]}',");
			}
		}
		// Append the closing parenthesis for the values
		_ = sb.AppendLine(value: ");");
		// Write the SQL content to the file
		streamWriter.Write(value: sb.ToString());
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the Click event of the ToolStripMenuItemExportAsPdf control.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
	/// <remarks>
	/// This method allows the user to export the selected orbital elements as a PDF file.
	/// </remarks>
	private void ToolStripMenuItemExportAsPdf_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported PDF file
		using SaveFileDialog saveFileDialogPdf = new()
		{
			Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
			DefaultExt = "pdf",
			Title = "Save database information as PDF"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogPdf.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial file name for the save file dialog
		saveFileDialogPdf.FileName = $"{orbitElements[index: 0]}.{saveFileDialogPdf.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogPdf.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Define a method to escape special characters in PDF text
		static string EscapePdfText(string value)
		{
			StringBuilder escaped = new();
			// Iterate through each character in the input string and escape special characters as needed
			foreach (char character in value)
			{
				_ = character switch
				{
					'\\' => escaped.Append(value: "\\\\"),
					'(' => escaped.Append(value: "\\("),
					')' => escaped.Append(value: "\\)"),
					'\r' or '\n' => escaped.Append(value: " "),
					_ => escaped.Append(value: character is >= ' ' and <= '~' ? character : '?'),
				};
			}
			// Return the escaped string
			return escaped.ToString();
		}
		// Create a list to hold the lines of text to be included in the PDF content
		List<string> lines = [];
		// Add a header line to the list with the index and readable designation of the minor planet
		lines.Add(item: $"Export for [{orbitElements[index: 0]}] {orbitElements[index: 1]}");
		lines.Add(item: string.Empty);
		// Iterate through the checked items in the checkedListBoxOrbitalElements and add the selected elements to the list of lines
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			if (!checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				continue;

			}
			// Add the selected element to the list of lines in the format "ElementName: ElementValue"
			lines.Add(item: $"{checkedListBoxOrbitalElements.Items[index: i]}: {orbitElements[index: i]}");
		}
		// If no elements are selected, add a line indicating that no elements are selected
		if (lines.Count == 2)
		{
			lines.Add(item: "No selected elements.");
		}
		// Create a StringBuilder to build the content of the PDF file
		StringBuilder contentBuilder = new();
		// Append the PDF content to the StringBuilder using PDF syntax
		_ = contentBuilder.AppendLine(value: "BT");
		_ = contentBuilder.AppendLine(value: "/F1 12 Tf");
		_ = contentBuilder.AppendLine(value: "50 800 Td");
		_ = contentBuilder.AppendLine(value: "15 TL");
		// Iterate through the lines of text and append them to the PDF content with proper escaping and formatting
		foreach (string line in lines)
		{
			_ = contentBuilder.AppendLine(value: $"({EscapePdfText(value: line)}) Tj");
			_ = contentBuilder.AppendLine(value: "T*");
		}
		// Append the end text operator to the PDF content
		_ = contentBuilder.Append(value: "ET");
		// Convert the PDF content to a byte array using ASCII encoding
		Encoding asciiEncoding = Encoding.ASCII;
		byte[] contentBytes = asciiEncoding.GetBytes(s: contentBuilder.ToString());
		// Define the PDF objects for the catalog, pages, page, font, and content stream
		string object1 = "1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n";
		string object2 = "2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n";
		string object3 = "3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>\nendobj\n";
		string object4 = "4 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>\nendobj\n";
		string object5 = $"5 0 obj\n<< /Length {contentBytes.Length} >>\nstream\n{contentBuilder}\nendstream\nendobj\n";
		// Create a MemoryStream to hold the PDF content and write the PDF header, objects, cross-reference table, trailer, and EOF marker to the stream
		using MemoryStream memoryStream = new();
		// Define a local method to write ASCII-encoded strings to the MemoryStream
		void WriteAscii(string value)
		{
			byte[] bytes = asciiEncoding.GetBytes(s: value);
			memoryStream.Write(buffer: bytes, offset: 0, count: bytes.Length);
		}
		// Write the PDF header to the MemoryStream
		WriteAscii(value: "%PDF-1.4\n");
		// Create a list to hold the byte offsets of the PDF objects for the cross-reference table
		List<long> offsets = [0];
		// Write each PDF object to the MemoryStream and record its byte offset for the cross-reference table
		offsets.Add(item: memoryStream.Position);
		WriteAscii(value: object1);
		offsets.Add(item: memoryStream.Position);
		WriteAscii(value: object2);
		offsets.Add(item: memoryStream.Position);
		WriteAscii(value: object3);
		offsets.Add(item: memoryStream.Position);
		WriteAscii(value: object4);
		offsets.Add(item: memoryStream.Position);
		WriteAscii(value: object5);
		// Write the cross-reference table to the MemoryStream using the recorded byte offsets of the PDF objects
		long xrefOffset = memoryStream.Position;
		WriteAscii(value: "xref\n");
		WriteAscii(value: "0 6\n");
		WriteAscii(value: "0000000000 65535 f \n");
		// Iterate through the recorded byte offsets of the PDF objects and write each offset to the cross-reference table in the required format
		for (int i = 1; i <= 5; i++)
		{
			_ = offsets[index: i];
			WriteAscii(value: $"{offsets[index: i]:0000000000} 00000 n \n");
		}
		// Write the trailer, startxref, and EOF marker to the MemoryStream to complete the PDF file structure
		WriteAscii(value: "trailer\n");
		WriteAscii(value: "<< /Size 6 /Root 1 0 R >>\n");
		WriteAscii(value: "startxref\n");
		WriteAscii(value: $"{xrefOffset}\n");
		WriteAscii(value: "%%EOF");
		// Write the content of the MemoryStream to the specified file path as a PDF file
		File.WriteAllBytes(path: saveFileDialogPdf.FileName, bytes: memoryStream.ToArray());
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for exporting data as a PostScript file.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>
	/// This method allows the user to export the current data as a PostScript file.
	/// </remarks>
	private void ToolStripMenuItemExportAsPostScript_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported PostScript file
		using SaveFileDialog saveFileDialogPostScript = new()
		{
			Filter = "PostScript files (*.ps)|*.ps|All files (*.*)|*.*",
			DefaultExt = "ps",
			Title = "Save database information as PostScript"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogPostScript.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial file name for the save file dialog
		saveFileDialogPostScript.FileName = $"{orbitElements[index: 0]}.{saveFileDialogPostScript.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogPostScript.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Define a method to escape special characters in PostScript strings
		static string EscapePostScriptString(string value) => value
			.Replace(oldValue: "\\", newValue: "\\\\", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "(", newValue: "\\(", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: ")", newValue: "\\)", comparisonType: StringComparison.Ordinal);
		// Create a new StreamWriter to write the PostScript content to the specified file
		using StreamWriter streamWriter = new(path: saveFileDialogPostScript.FileName);
		StringBuilder sb = new();
		// Append the PostScript content to the StringBuilder using PostScript syntax
		_ = sb.AppendLine(value: "%!PS-Adobe-3.0");
		_ = sb.AppendLine(value: "%%Creator: Planetoid-DB");
		_ = sb.AppendLine(value: "%%Pages: 1");
		_ = sb.AppendLine(value: "%%BoundingBox: 0 0 595 842");
		_ = sb.AppendLine(value: "%%EndComments");
		_ = sb.AppendLine(value: "/Helvetica findfont 12 scalefont setfont");
		_ = sb.AppendLine(value: "50 800 moveto");
		_ = sb.AppendLine(value: $"({EscapePostScriptString(value: $"Export for [{orbitElements[index: 0]}] {orbitElements[index: 1]}")}) show");
		_ = sb.AppendLine(value: "0 -20 rmoveto");
		// Iterate through the checked items in the checkedListBoxOrbitalElements and append the selected elements to the PostScript content
		bool hasSelectedElements = false;
		// Loop through the items in the checkedListBoxOrbitalElements and check if they are checked
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			if (!checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				continue;
			}
			// If the item is checked, append the orbit element to the PostScript content
			hasSelectedElements = true;
			string line = EscapePostScriptString(value: $"{checkedListBoxOrbitalElements.Items[index: i]}: {orbitElements[index: i]}");
			_ = sb.AppendLine(value: $"({line}) show");
			_ = sb.AppendLine(value: "0 -15 rmoveto");
		}
		// If no elements were selected, add a message to the PostScript content
		if (!hasSelectedElements)
		{
			_ = sb.AppendLine(value: "(No selected elements.) show");
		}
		// Append the showpage operator to the PostScript content to indicate the end of the page
		_ = sb.AppendLine(value: "showpage");
		streamWriter.Write(value: sb.ToString());
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Export as EPUB" menu item.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>
	/// This method handles the export of database information as an EPUB file.
	/// </remarks>
	private void ToolStripMenuItemExportAsEpub_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported EPUB file
		using SaveFileDialog saveFileDialogEpub = new()
		{
			Filter = "EPUB files (*.epub)|*.epub|All files (*.*)|*.*",
			DefaultExt = "epub",
			Title = "Save database information as EPUB"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogEpub.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial file name for the save file dialog
		saveFileDialogEpub.FileName = $"{orbitElements[index: 0]}.{saveFileDialogEpub.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogEpub.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Create a new StreamWriter to write the EPUB content to the specified file
		static string EscapeXml(string value) => value
			.Replace(oldValue: "&", newValue: "&amp;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "<", newValue: "&lt;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: ">", newValue: "&gt;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "\"", newValue: "&quot;", comparisonType: StringComparison.Ordinal)
			.Replace(oldValue: "'", newValue: "&apos;", comparisonType: StringComparison.Ordinal);
		// Create a StringBuilder to build the EPUB content
		string title = $"Export for [{orbitElements[index: 0]}] {orbitElements[index: 1]}";
		// Build the body content of the EPUB
		StringBuilder bodyBuilder = new();
		_ = bodyBuilder.AppendLine(value: $"<h1>{EscapeXml(value: title)}</h1>");
		_ = bodyBuilder.AppendLine(value: "<ul>");
		// Append the orbit elements to the body content of the EPUB
		bool hasSelectedElements = false;
		// Loop through the items in the checkedListBoxOrbitalElements and check if they are checked
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			if (!checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				continue;
			}
			// If the item is checked, append the orbit element to the body content of the EPUB
			hasSelectedElements = true;
			string elementName = EscapeXml(value: checkedListBoxOrbitalElements.Items[index: i].ToString() ?? string.Empty);
			string elementValue = EscapeXml(value: orbitElements[index: i]);
			_ = bodyBuilder.AppendLine(value: $"<li><strong>{elementName}:</strong> {elementValue}</li>");
		}
		// If no elements were selected, add a message to the body content of the EPUB
		if (!hasSelectedElements)
		{
			_ = bodyBuilder.AppendLine(value: "<li>No selected elements.</li>");
		}
		// Append the closing tags for the body content of the EPUB
		_ = bodyBuilder.AppendLine(value: "</ul>");
		// Define the XML content for the EPUB structure
		string containerXml = """
			<?xml version="1.0" encoding="UTF-8"?>
			<container version="1.0" xmlns="urn:oasis:names:tc:opendocument:xmlns:container">
				<rootfiles>
					<rootfile full-path="OEBPS/content.opf" media-type="application/oebps-package+xml"/>
				</rootfiles>
			</container>
			""";
		// Define the XML content for the EPUB package and metadata
		string contentOpf = $"""
			<?xml version="1.0" encoding="UTF-8"?>
			<package xmlns="http://www.idpf.org/2007/opf" unique-identifier="bookid" version="3.0">
				<metadata xmlns:dc="http://purl.org/dc/elements/1.1/">
					<dc:identifier id="bookid">urn:uuid:{Guid.NewGuid()}</dc:identifier>
					<dc:title>{EscapeXml(value: title)}</dc:title>
					<dc:language>en</dc:language>
				</metadata>
				<manifest>
					<item id="nav" href="nav.xhtml" media-type="application/xhtml+xml" properties="nav"/>
					<item id="content" href="content.xhtml" media-type="application/xhtml+xml"/>
				</manifest>
				<spine>
					<itemref idref="content"/>
				</spine>
			</package>
			""";
		// Define the XML content for the EPUB navigation document
		string navXhtml = $"""
			<?xml version="1.0" encoding="UTF-8"?>
			<!DOCTYPE html>
			<html xmlns="http://www.w3.org/1999/xhtml" xmlns:epub="http://www.idpf.org/2007/ops" lang="en" xml:lang="en">
			<head>
				<meta charset="utf-8" />
				<title>Table of Contents</title>
			</head>
			<body>
				<nav epub:type="toc" id="toc">
					<h1>Contents</h1>
					<ol>
						<li><a href="content.xhtml">{EscapeXml(value: title)}</a></li>
					</ol>
				</nav>
			</body>
			</html>
			""";
		// Define the XML content for the EPUB main content document
		string contentXhtml = $"""
			<?xml version="1.0" encoding="UTF-8"?>
			<!DOCTYPE html>
			<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
			<head>
				<meta charset="utf-8" />
				<title>{EscapeXml(value: title)}</title>
			</head>
			<body>
				{bodyBuilder}
			</body>
			</html>
			""";
		// Create a new FileStream to write the EPUB content to the specified file
		using FileStream fileStream = new(path: saveFileDialogEpub.FileName, mode: FileMode.Create, access: FileAccess.Write, share: FileShare.None);
		// Create a new ZipArchive to write the EPUB content in ZIP format
		using ZipArchive archive = new(stream: fileStream, mode: ZipArchiveMode.Create);
		// Define a local function to add an entry to the ZIP archive with the specified name, content, and compression level
		void AddEntry(string entryName, string content, CompressionLevel compressionLevel = CompressionLevel.Optimal)
		{
			ZipArchiveEntry entry = archive.CreateEntry(entryName, compressionLevel);
			using StreamWriter writer = new(stream: entry.Open(), encoding: new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
			writer.Write(value: content);
		}
		// Add the necessary entries to the ZIP archive for the EPUB structure and content
		AddEntry(entryName: "mimetype", content: "application/epub+zip", compressionLevel: CompressionLevel.NoCompression);
		AddEntry(entryName: "META-INF/container.xml", content: containerXml);
		AddEntry(entryName: "OEBPS/content.opf", content: contentOpf);
		AddEntry(entryName: "OEBPS/nav.xhtml", content: navXhtml);
		AddEntry(entryName: "OEBPS/content.xhtml", content: contentXhtml);
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}

	/// <summary>
	/// Handles the click event for the "Export as MOBI" menu item.
	/// </summary>
	/// <param name="sender">The source of the event.</param>
	/// <param name="e">The event data.</param>
	/// <remarks>
	/// This method handles the export of database information as a MOBI file.
	/// </remarks>
	private void ToolStripMenuItemExportAsMobi_Click(object sender, EventArgs e)
	{
		// Create a new SaveFileDialog to allow the user to select the file path and name for the exported MOBI file
		using SaveFileDialog saveFileDialogMobi = new()
		{
			Filter = "MOBI files (*.mobi)|*.mobi|All files (*.*)|*.*",
			DefaultExt = "mobi",
			Title = "Save database information as MOBI"
		};
		// Set the initial directory for the save file dialog to the user's documents folder
		saveFileDialogMobi.InitialDirectory = Environment.GetFolderPath(folder: Environment.SpecialFolder.MyDocuments);
		// Set the initial file name for the save file dialog
		saveFileDialogMobi.FileName = $"{orbitElements[index: 0]}.{saveFileDialogMobi.DefaultExt}";
		// Show the save file dialog to select the file path and name
		if (saveFileDialogMobi.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		// Create a StringBuilder to build the content of the MOBI file
		StringBuilder sb = new();
		// Append the content to the StringBuilder in a simple text format for the MOBI file
		_ = sb.AppendLine(value: "BOOKMOBI");
		_ = sb.AppendLine(value: $"Export for [{orbitElements[index: 0]}] {orbitElements[index: 1]}");
		_ = sb.AppendLine();
		// Iterate through the checked items in the checkedListBoxOrbitalElements and append the selected elements to the MOBI content
		bool hasSelectedElements = false;
		// Loop through the items in the checkedListBoxOrbitalElements and check if they are checked
		for (int i = 0; i < checkedListBoxOrbitalElements.Items.Count; i++)
		{
			if (!checkedListBoxOrbitalElements.GetItemChecked(index: i))
			{
				continue;
			}
			// If the item is checked, append the orbit element to the MOBI content
			hasSelectedElements = true;
			_ = sb.AppendLine(value: $"{checkedListBoxOrbitalElements.Items[index: i]}: {orbitElements[index: i]}");
		}
		// If no elements were selected, add a message to the MOBI content
		if (!hasSelectedElements)
		{
			_ = sb.AppendLine(value: "No selected elements.");
		}
		// Write the content of the StringBuilder to the specified file path as a MOBI file
		File.WriteAllBytes(path: saveFileDialogMobi.FileName, bytes: Encoding.UTF8.GetBytes(s: sb.ToString()));
		// Show a message box indicating that the data was exported successfully
		MessageBox.Show(text: "Data exported successfully.", caption: "Export Complete", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
	}
}