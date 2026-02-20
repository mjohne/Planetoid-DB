// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Planetoid_DB.Forms;

namespace Planetoid_DB;

/// <summary>
/// Represents a form that displays terminology information.
/// </summary>
/// <remarks>
/// This form provides a user interface for viewing and managing terminology information.
/// </remarks>
public partial class TerminologyForm : BaseKryptonForm
{
	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="TerminologyForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public TerminologyForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>
	/// Gets the status label to be used for displaying information.
	/// </summary>
	/// <remarks>
	/// Derived classes should override this property to provide the specific label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>
	/// Updates the content displayed in the web browser control.
	/// </summary>
	/// <remarks>
	/// This method is used to update the web browser content based on the selected terminology element.
	/// </remarks>
	private void UpdateBrowserContent()
	{
		// Construct the resource key based on the selected element
		string resourceKey = $"terminology_{SelectedElement}";
		// Attempt to retrieve the corresponding string from the resource manager
		string? text = I18nStrings.ResourceManager.GetString(name: resourceKey);
		// Fallback to IndexNumber if not found to avoid empty screen
		webBrowser.DocumentText = text ?? I18nStrings.terminology_IndexNumber;
	}

	/// <summary>
	/// Retrieves or sets the currently selected terminology element.
	/// Automatically updates the display when set.
	/// </summary>
	/// <value>The currently selected terminology element.</value>
	/// <remarks>
	/// This property is configured for code serialization.
	/// </remarks>
	[System.ComponentModel.DesignerSerializationVisibility(visibility: System.ComponentModel.DesignerSerializationVisibility.Visible)]
	public TerminologyElement SelectedElement
	{
		get;
		set
		{
			// Check if the value is different from the current field
			if (field != value)
			{
				// Update the field and refresh the browser content
				field = value;
				UpdateBrowserContent();
				// Update the list box selection if needed
				if ((int)value >= -1 && (int)value < listBox.Items.Count && listBox.SelectedIndex != (int)value)
				{
					listBox.SelectedIndex = (int)value;
				}
			}
		}
	} = TerminologyElement.IndexNumber;

	#endregion

	#region form event handlers

	/// <summary>
	/// Fired when the form has finished loading.
	/// Initializes the displayed content (sets the active terminology element) and clears the status bar.
	/// </summary>
	/// <param name="sender">The event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is triggered when the form has finished loading.
	/// </remarks>
	private void TerminologyForm_Load(object sender, EventArgs e)
	{
		// Initial update of the browser content
		UpdateBrowserContent();
		// Clear the status bar text
		ClearStatusBar(label: labelInformation);
	}

	#endregion

	#region SelectedValueChanged event handler

	/// <summary>
	/// Handles the list box SelectedValueChanged event.
	/// Updates the current <see cref="TerminologyElement"/> based on the selected index
	/// and refreshes the displayed content in the embedded web browser.
	/// </summary>
	/// <param name="sender">The event source (expected to be the terminology list box).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// This event is triggered when the selected value in the list box changes.
	/// </remarks>
	private void ListBox_SelectedValueChanged(object sender, EventArgs e)
	{
		// Get the selected element from the list box
		SelectedElement = (TerminologyElement)listBox.SelectedIndex;
	}

	#endregion
}