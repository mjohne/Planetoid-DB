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
		// Dynamically generate the resource name based on the selected element.
		string resourceKey = $"terminology_{SelectedElement}";
		// Try to load the text. Fallback to IndexNumber if not found.
		// This is a trick to avoid a large switch statement.
		string? text = I10nStrings.ResourceManager.GetString(name: resourceKey);
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

	/// <summary>
	/// Sets the active terminology element and updates the web browser content.
	/// </summary>
	/// <remarks>
	/// This method is used to update the web browser content based on the selected terminology element.
	/// </remarks>
	private void SetActiveElement()
	{
		// Set the selected element in the list box
		webBrowser.DocumentText = SelectedElement switch
		{
			TerminologyElement.IndexNumber => I10nStrings.terminology_IndexNumber,
			TerminologyElement.ReadableDesignation => I10nStrings.terminology_ReadableDesignation,
			TerminologyElement.Epoch => I10nStrings.terminology_Epoch,
			TerminologyElement.MeanAnomalyAtTheEpoch => I10nStrings.terminology_MeanAnomalyAtTheEpoch,
			TerminologyElement.ArgumentOfThePerihelion => I10nStrings.terminology_ArgumentOfThePerihelion,
			TerminologyElement.LongitudeOfTheAscendingNode => I10nStrings.terminology_LongitudeOfTheAscendingNode,
			TerminologyElement.InclinationToTheEcliptic => I10nStrings.terminology_InclinationToTheEcliptic,
			TerminologyElement.OrbitalEccentricity => I10nStrings.terminology_OrbitalEccentricity,
			TerminologyElement.MeanDailyMotion => I10nStrings.terminology_MeanDailyMotion,
			TerminologyElement.SemiMajorAxis => I10nStrings.terminology_SemiMajorAxis,
			TerminologyElement.AbsoluteMagnitude => I10nStrings.terminology_AbsoluteMagnitude,
			TerminologyElement.SlopeParameter => I10nStrings.terminology_SlopeParameter,
			TerminologyElement.Reference => I10nStrings.terminology_Reference,
			TerminologyElement.NumberOfOppositions => I10nStrings.terminology_NumberOfOppositions,
			TerminologyElement.NumberOfObservations => I10nStrings.terminology_NumberOfObservations,
			TerminologyElement.ObservationSpan => I10nStrings.terminology_ObservationSpan,
			TerminologyElement.RmsResidual => I10nStrings.terminology_RmsResidual,
			TerminologyElement.ComputerName => I10nStrings.terminology_ComputerName,
			TerminologyElement.Flags => I10nStrings.terminology_Flags,
			TerminologyElement.DateOfLastObservation => I10nStrings.terminology_DateOfLastObservation,
			TerminologyElement.LinearEccentricity => I10nStrings.terminology_LinearEccentricity,
			TerminologyElement.SemiMinorAxis => I10nStrings.terminology_SemiMinorAxis,
			TerminologyElement.MajorAxis => I10nStrings.terminology_MajorAxis,
			TerminologyElement.MinorAxis => I10nStrings.terminology_MinorAxis,
			TerminologyElement.EccentricAnomaly => I10nStrings.terminology_EccenctricAnomaly,
			TerminologyElement.TrueAnomaly => I10nStrings.terminology_TrueAnomaly,
			TerminologyElement.PerihelionDistance => I10nStrings.terminology_PerihelionDistance,
			TerminologyElement.AphelionDistance => I10nStrings.terminology_AphelionDistance,
			TerminologyElement.LongitudeOfTheDescendingNode => I10nStrings.terminology_LongitudeOfTheDescendingNode,
			TerminologyElement.ArgumentOfTheAphelion => I10nStrings.terminology_ArgumentOfTheAphelion,
			TerminologyElement.FocalParameter => I10nStrings.terminology_FocalParameter,
			TerminologyElement.SemiLatusRectum => I10nStrings.terminology_SemiLatusRectum,
			TerminologyElement.LatusRectum => I10nStrings.terminology_LatusRectum,
			TerminologyElement.OrbitalPeriod => I10nStrings.terminology_OrbitalPeriod,
			TerminologyElement.OrbitalArea => I10nStrings.terminology_OrbitalArea,
			TerminologyElement.OrbitalPerimeter => I10nStrings.terminology_OrbitalPerimeter,
			TerminologyElement.SemiMeanAxis => I10nStrings.terminology_SemiMeanAxis,
			TerminologyElement.MeanAxis => I10nStrings.terminology_MeanAxis,
			TerminologyElement.StandardGravitationalParameter => I10nStrings.terminology_StandardGravitationalParameter,
			// Default case for unrecognized elements
			_ => I10nStrings.terminology_IndexNumber,
		};
	}

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
		// Set the active element based on the selected value
		SetActiveElement();
		// Clear the status bar text
		ClearStatusBar();
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
	/// This method is called when the mouse pointer leaves a control or the control loses focus.
	/// </remarks>
	private void Control_Leave(object sender, EventArgs e) => ClearStatusBar();

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