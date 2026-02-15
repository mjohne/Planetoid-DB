using Planetoid_DB.Forms;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>
/// Represents a form that displays terminology information.
/// </summary>
/// <remarks>
/// This form provides a user interface for viewing and managing terminology information.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
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
	/// Returns a short debugger display string for this instance.
	/// </summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>
	/// This method is used to provide a visual representation of the object in the debugger.
	/// </remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>
	/// Updates the content displayed in the web browser control.
	/// </summary>
	/// <remarks>
	/// This method is used to update the web browser content based on the selected terminology element.
	/// </remarks>
	private void UpdateBrowserContent()
	{
		// If the selected element is IndexNumber and the current document text is already the corresponding string, do nothing
		if (SelectedElement == TerminologyElement.IndexNumber && webBrowser.DocumentText == I10nStrings.terminology_IndexNumber)
		{
			return;
		}
		// Construct the resource key based on the selected element
		string resourceKey = $"terminology_{SelectedElement}";
		// Attempt to retrieve the corresponding string from the resource manager
		string? text = I10nStrings.ResourceManager.GetString(name: resourceKey);
		// Fallback to IndexNumber if not found to avoid empty screen
		webBrowser.DocumentText = text ?? I10nStrings.terminology_IndexNumber;
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
				if (listBox.SelectedIndex != (int)value)
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

	#region Enter event handlers

	/// <summary>
	/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
	/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
	/// </summary>
	/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is called when the mouse pointer enters a control or the control receives focus.
	/// </remarks>
	private void Control_Enter(object sender, EventArgs e)
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
			SetStatusBar(label: labelInformation, text: description);
		}
	}

	#endregion

	#region Leave event handlers

	/// <summary>
	/// Called when the mouse pointer leaves a control or the control loses focus.
	/// Clears the status bar text.
	/// </summary>
	/// <param name="sender">Event source.</param>
	/// <param name="e">Event arguments.</param>
	/// <remarks>
	/// This method is called when the mouse pointer leaves a control or the control loses focus.
	/// </remarks>
	private void Control_Leave(object sender, EventArgs e) => ClearStatusBar(label: labelInformation);

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