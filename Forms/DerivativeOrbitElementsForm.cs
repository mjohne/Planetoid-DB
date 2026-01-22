using NLog;

using System.Diagnostics;

namespace Planetoid_DB
{
	/// <summary>
	/// Form for displaying derived orbit elements.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class DerivativeOrbitElementsForm : BaseKryptonForm
	{
		/// <summary>
		/// Stores the currently selected control for clipboard operations.
		/// </summary>
		private Control currentControl;

		/// <summary>
		/// NLog logger instance.
		/// </summary>
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Stores the current tag text of the control.
		/// </summary>
		private string currentTagText = string.Empty;

		/// <summary>
		/// List of derived orbit elements.
		/// </summary>
		private List<object> derivativeOrbitElements = [];

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="DerivativeOrbitElementsForm"/> class.
		/// </summary>
		public DerivativeOrbitElementsForm()
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

		/// <summary>
		/// Tries to parse an integer from the input string.
		/// </summary>
		/// <param name="input">The input string to parse.</param>
		/// <param name="value">The parsed integer value if successful.</param>
		/// <param name="errorMessage">An error message if parsing fails.</param>
		/// <returns>True if parsing was successful; otherwise, false.</returns>
		public static bool TryParseInt(string input, out int value, out string errorMessage)
		{
			// Initialize output parameters
			value = 0;
			errorMessage = string.Empty;
			// Check if the input is null or whitespace
			if (string.IsNullOrWhiteSpace(value: input))
			{
				// Set the error message and return false
				errorMessage = "Der eingegebene Text ist leer oder besteht nur aus Leerzeichen.";
				return false;
			}
			// Try to parse the integer
			// If parsing fails, set the error message
			if (!int.TryParse(s: input, result: out value))
			{
				// Set the error message and return false
				errorMessage = $"Der Wert \"{input}\" ist keine gültige Ganzzahl.";
				return false;
			}
			// Parsing was successful
			return true;
		}


		/// <summary>
		/// Opens the terminology dialog for a specific derived orbit element.
		/// The <paramref name="index"/> selects which terminology entry to show. Values outside the supported range
		/// are normalized to the default (index 0).
		/// </summary>
		/// <param name="index">Zero-based index selecting the terminology topic (valid range: 0..38).</param>
		private void OpenTerminology(uint index)
		{
			// Check if the index is valid
			// If the index is out of range, set it to 0
			if (index > 38)
			{
				index = 0;
			}
			// Create a new instance of the TerminologyForm and set the active terminology based on the index
			using TerminologyForm formTerminology = new();
			// Set the active terminology based on the index
			switch (index)
			{
				// Set the active terminology based on the index
				// Each case corresponds to a specific terminology
				// and calls the appropriate method in the TerminologyForm
				case 0: formTerminology.SetIndexNumberActive(); break;
				case 1: formTerminology.SetReadableDesignationActive(); break;
				case 2: formTerminology.SetEpochActive(); break;
				case 3: formTerminology.SetMeanAnomalyAtTheEpochActive(); break;
				case 4: formTerminology.SetArgumentOfPerihelionActive(); break;
				case 5: formTerminology.SetLongitudeOfTheAscendingNodeActive(); break;
				case 6: formTerminology.SetInclinationToTheEclipticActive(); break;
				case 7: formTerminology.SetOrbitalEccentricityActive(); break;
				case 8: formTerminology.SetMeanDailyMotionActive(); break;
				case 9: formTerminology.SetSemiMajorAxisActive(); break;
				case 10: formTerminology.SetAbsoluteMagnitudeActive(); break;
				case 11: formTerminology.SetSlopeParamActive(); break;
				case 12: formTerminology.SetReferenceActive(); break;
				case 13: formTerminology.SetNumberOfOppositionsActive(); break;
				case 14: formTerminology.SetNumberOfObservationsActive(); break;
				case 15: formTerminology.SetObservationSpanActive(); break;
				case 16: formTerminology.SetRmsResidualActive(); break;
				case 17: formTerminology.SetComputerNameActive(); break;
				case 18: formTerminology.SetFlagsActive(); break;
				case 19: formTerminology.SetDateOfTheLastObservationActive(); break;
				case 20: formTerminology.SetLinearEccentricityActive(); break;
				case 21: formTerminology.SetSemiMinorAxisActive(); break;
				case 22: formTerminology.SetMajorAxisActive(); break;
				case 23: formTerminology.SetMinorAxisActive(); break;
				case 24: formTerminology.SetEccentricAnomalyActive(); break;
				case 25: formTerminology.SetTrueAnomalyActive(); break;
				case 26: formTerminology.SetPerihelionDistanceActive(); break;
				case 27: formTerminology.SetAphelionDistanceActive(); break;
				case 28: formTerminology.SetLongitudeOfTheDescendingNodeActive(); break;
				case 29: formTerminology.SetArgumentOfTheAphelionActive(); break;
				case 30: formTerminology.SetFocalParameterActive(); break;
				case 31: formTerminology.SetSemiLatusRectumActive(); break;
				case 32: formTerminology.SetLatusRectumActive(); break;
				case 33: formTerminology.SetOrbitalPeriodActive(); break;
				case 34: formTerminology.SetOrbitalAreaActive(); break;
				case 35: formTerminology.SetOrbitalPerimeterActive(); break;
				case 36: formTerminology.SetSemiMeanAxisActive(); break;
				case 37: formTerminology.SetMeanAxisActive(); break;
				case 38: formTerminology.SetStandardGravitationalParameterActive(); break;
				// Default case to handle unexpected values
				default: formTerminology.SetIndexNumberActive(); break;
			}
			// Set the form to be topmost if the main form is topmost
			formTerminology.TopMost = TopMost;
			// Show the terminology form as a dialog
			_ = formTerminology.ShowDialog();
		}

		/// <summary>
		/// Sets the internal list of derived orbit elements used by the form.
		/// </summary>
		/// <param name="list">A list of derived orbit element values. The list is stored by reference and will be used to populate the UI when the form loads.</param>
		public void SetDatabase(List<object> list) => derivativeOrbitElements = list;

		#endregion

		#region form event handlers

		/// <summary>
		/// Fired when the derivative orbit elements form is loaded.
		/// Clears the status area, validates the provided derived-element data and populates the UI labels
		/// with the corresponding values. If the provided data is invalid an error is logged and shown to the user.
		/// </summary>
		/// <param name="sender">Event source (the form).</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void DerivativeOrbitElementsForm_Load(object sender, EventArgs e)
		{
			// Set the status bar text
			ClearStatusBar();
			if (derivativeOrbitElements.Count < 19)
			{
				// Log the error and show an error message
				Logger.Error(message: "Invalid data");
				ShowErrorMessage(message: "Invalid data");
				return;
			}
			// Set the text of the labels with the derived orbit elements
			labelLinearEccentricityData.Text = derivativeOrbitElements[index: 0]?.ToString();
			labelSemiMinorAxisData.Text = derivativeOrbitElements[index: 1]?.ToString();
			labelMajorAxisData.Text = derivativeOrbitElements[index: 2]?.ToString();
			labelMinorAxisData.Text = derivativeOrbitElements[index: 3]?.ToString();
			labelEccentricAnomalyData.Text = derivativeOrbitElements[index: 4]?.ToString();
			labelTrueAnomalyData.Text = derivativeOrbitElements[index: 5]?.ToString();
			labelPerihelionDistanceData.Text = derivativeOrbitElements[index: 6]?.ToString();
			labelAphelionDistanceData.Text = derivativeOrbitElements[index: 7]?.ToString();
			labelLongitudeDescendingNodeData.Text = derivativeOrbitElements[index: 8]?.ToString();
			labelArgumentAphelionData.Text = derivativeOrbitElements[index: 9]?.ToString();
			labelFocalParameterData.Text = derivativeOrbitElements[index: 10]?.ToString();
			labelSemiLatusRectumData.Text = derivativeOrbitElements[index: 11]?.ToString();
			labelLatusRectumData.Text = derivativeOrbitElements[index: 12]?.ToString();
			labelPeriodData.Text = derivativeOrbitElements[index: 13]?.ToString();
			labelOrbitalAreaData.Text = derivativeOrbitElements[index: 14]?.ToString();
			labelOrbitalPerimeterData.Text = derivativeOrbitElements[index: 15]?.ToString();
			labelSemiMeanAxisData.Text = derivativeOrbitElements[index: 16]?.ToString();
			labelMeanAxisData.Text = derivativeOrbitElements[index: 17]?.ToString();
			labelStandardGravitationalParameterData.Text = derivativeOrbitElements[index: 18]?.ToString();
		}

		/// <summary>
		/// Fired when the derivative orbit elements form is closed.
		/// Disposes managed resources associated with the form.
		/// </summary>
		/// <param name="sender">Event source (the form).</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		private void DerivativeOrbitElementsForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

		#endregion

		#region Enter event handlers

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

		#region Leave event handlers

		/// <summary>
		/// Called when the mouse pointer leaves a control or the control loses focus.
		/// Clears the status bar text (delegates to <see cref="ClearStatusBar"/>).
		/// </summary>
		/// <param name="sender">Event source.</param>
		/// <param name="e">Event arguments.</param>
		private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

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
		/// This method checks the type of the sender and copies its text to the clipboard.
		/// </remarks>
		private void CopyToClipboard_DoubleClick(object sender, EventArgs e)
		{
			// Check if the sender is null
			ArgumentNullException.ThrowIfNull(argument: sender);
			// Check the type of the sender and copy the text accordingly
			if (sender is Control control)
			{
				// Copy the text to the clipboard
				CopyToClipboard(text: control.Text);
			}
			// Check if the sender is a ToolStripItem
			else if (sender is ToolStripItem)
			{
				// Copy the text to the clipboard
				CopyToClipboard(text: currentControl.Text);
			}
		}

		/// <summary>
		/// Handles double-click events on the control to open the terminology dialog.
		/// </summary>
		/// <param name="sender">Event source (the control).</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		/// <remarks>
		/// This method attempts to parse the current tag text as an integer and opens the terminology dialog
		/// for the corresponding entry if successful.
		/// </remarks>
		private void OpenTerminology_DoubleClick(object sender, EventArgs e)
		{
			// Try to parse the index from the current tag text
			// If successful, open the terminology dialog for that index
			// If parsing fails, log an error and show an error message
			if (TryParseInt(input: currentTagText, value: out int index, errorMessage: out string errorMessage))
			{
				// Open the terminology dialog for the parsed index
				OpenTerminology(index: (uint)index);
				return;
			}
			// Log the error and show an error message
			Logger.Error(message: $"Failed to parse index from tag text '{currentTagText}': {errorMessage}");
			ShowErrorMessage(message: $"Failed to parse index from tag text '{currentTagText}': {errorMessage}");
		}

		#endregion

		#region MouseDown event handlers

		/// <summary>
		/// Handles the MouseDown event for controls.
		/// Stores the control that triggered the event for future reference.
		/// </summary>
		/// <param name="sender">Event source (the control).</param>
		/// <param name="e">The <see cref="MouseEventArgs"/> instance that contains the event data.</param>
		private void Control_MouseDown(object sender, MouseEventArgs e)
		{
			if (sender is Control control)
			{
				currentControl = control;
				if (control.Tag != null)
				{
					currentTagText = control.Tag.ToString();
				}
			}
		}

		#endregion
	}
}