using NLog;

using System.Diagnostics;

namespace Planetoid_DB
{
	/// <summary>
	/// Represents a form that displays terminology information.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class TerminologyForm : BaseKryptonForm
	{
		// NLog logger instance
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// The currently selected terminology element.
		private TerminologyElement selectedElement = TerminologyElement.IndexNumber;

		/// <summary>
		/// Enumeration of terminology elements used in the application.
		/// </summary>
		public enum TerminologyElement
		{
			/// <summary>
			/// Index number of the element.
			/// </summary>
			IndexNumber,
			/// <summary>
			/// Readable designation of the element.
			/// </summary>
			ReadableDesignation,
			/// <summary>
			/// Epoch of the element.
			/// </summary>
			Epoch,
			/// <summary>
			/// Mean anomaly at the epoch of the element.
			/// </summary>
			MeanAnomalyAtTheEpoch,
			/// <summary>
			/// Argument of perihelion of the element.
			/// </summary>
			ArgumentOfPerihelion,
			/// <summary>
			/// Longitude of the ascending node of the element.
			/// </summary>
			LongitudeOfTheAscendingNode,
			/// <summary>
			/// Inclination to the ecliptic of the element.
			/// </summary>
			InclinationToTheEcliptic,
			/// <summary>
			/// Orbital eccentricity of the element.
			/// </summary>
			OrbitalEccentricity,
			/// <summary>
			/// Mean daily motion of the element.
			/// </summary>
			MeanDailyMotion,
			/// <summary>
			/// Semi-major axis of the element.
			/// </summary>
			SemiMajorAxis,
			/// <summary>
			/// Absolute magnitude of the element.
			/// </summary>
			AbsoluteMagnitude,
			/// <summary>
			/// Slope parameter of the element.
			/// </summary>
			SlopeParameter,
			/// <summary>
			/// Reference of the element.
			/// </summary>
			Reference,
			/// <summary>
			/// Number of oppositions of the element.
			/// </summary>
			NumberOfOppositions,
			/// <summary>
			/// Number of observations of the element.
			/// </summary>
			NumberOfObservations,
			/// <summary>
			/// Observation span of the element.
			/// </summary>
			ObservationSpan,
			/// <summary>
			/// RMS residual of the element.
			/// </summary>
			RmsResidual,
			/// <summary>
			/// Computer name of the element.
			/// </summary>
			ComputerName,
			/// <summary>
			/// Flags of the element.
			/// </summary>
			Flags,
			/// <summary>
			/// Date of last observation of the element.
			/// </summary>
			DateOfLastObservation,
			/// <summary>
			/// Linear eccentricity of the element.
			/// </summary>
			LinearEccentricity,
			/// <summary>
			/// Semi-minor axis of the element.
			/// </summary>
			SemiMinorAxis,
			/// <summary>
			/// Major axis of the element.
			/// </summary>
			MajorAxis,
			/// <summary>
			/// Minor axis of the element.
			/// </summary>
			MinorAxis,
			/// <summary>
			/// Eccentric anomaly of the element.
			/// </summary>
			EccentricAnomaly,
			/// <summary>
			/// True anomaly of the element.
			/// </summary>
			TrueAnomaly,
			/// <summary>
			/// Perihelion distance of the element.
			/// </summary>
			PerihelionDistance,
			/// <summary>
			/// Aphelion distance of the element.
			/// </summary>
			AphelionDistance,
			/// <summary>
			/// Longitude of the descending node of the element.
			/// </summary>
			LongitudeOfTheDescendingNode,
			/// <summary>
			/// Argument of aphelion of the element.
			/// </summary>
			ArgumentOfAphelion,
			/// <summary>
			/// Focal parameter of the element.
			/// </summary>
			FocalParameter,
			/// <summary>
			/// Semi-latus rectum of the element.
			/// </summary>
			SemiLatusRectum,
			/// <summary>
			/// Latus rectum of the element.
			/// </summary>
			LatusRectum,
			/// <summary>
			/// Orbital period of the element.
			/// </summary>
			OrbitalPeriod,
			/// <summary>
			/// Orbital area of the element.
			/// </summary>
			OrbitalArea,
			/// <summary>
			/// Orbital perimeter of the element.
			/// </summary>
			OrbitalPerimeter,
			/// <summary>
			/// Semi-mean axis of the element.
			/// </summary>
			SemiMeanAxis,
			/// <summary>
			/// Mean axis of the element.
			/// </summary>
			MeanAxis,
			/// <summary>
			/// Standard gravitational parameter of the element.
			/// </summary>
			StandardGravitationalParameter
		}

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TerminologyForm"/> class.
		/// </summary>
		public TerminologyForm()
		{
			// Initialize the form components
			InitializeComponent();
		}

		#endregion

		#region local methods

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
		/// Sets the active terminology element and updates the web browser content.
		/// </summary>
		private void SetActiveElement()
		{
			// Set the selected element in the list box
			webBrowser.DocumentText = selectedElement switch
			{
				TerminologyElement.IndexNumber => I10nStrings.terminology_IndexNumber,
				TerminologyElement.ReadableDesignation => I10nStrings.terminology_ReadableDesignaton,
				TerminologyElement.Epoch => I10nStrings.terminology_Epoch,
				TerminologyElement.MeanAnomalyAtTheEpoch => I10nStrings.terminology_MeanAnomalyAtTheEpoch,
				TerminologyElement.ArgumentOfPerihelion => I10nStrings.terminology_ArgumentOfPerihelion,
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
				TerminologyElement.ArgumentOfAphelion => I10nStrings.terminology_ArgumentOfAphelion,
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

		/// <summary>
		/// Sets the selected element to IndexNumber.
		/// </summary>
		public void SetIndexNumberActive() => selectedElement = TerminologyElement.IndexNumber;

		/// <summary>
		/// Sets the selected element to ReadableDesignation.
		/// </summary>
		public void SetReadableDesignationActive() => selectedElement = TerminologyElement.ReadableDesignation;

		/// <summary>
		/// Sets the selected element to Epoch.
		/// </summary>
		public void SetEpochActive() => selectedElement = TerminologyElement.Epoch;

		/// <summary>
		/// Sets the selected element to MeanAnomalyAtTheEpoch.
		/// </summary>
		public void SetMeanAnomalyAtTheEpochActive() => selectedElement = TerminologyElement.MeanAnomalyAtTheEpoch;

		/// <summary>
		/// Sets the selected element to ArgumentOfPerihelion.
		/// </summary>
		public void SetArgumentOfPerihelionActive() => selectedElement = TerminologyElement.ArgumentOfPerihelion;

		/// <summary>
		/// Sets the selected element to LongitudeOfTheAscendingNode.
		/// </summary>
		public void SetLongitudeOfTheAscendingNodeActive() => selectedElement = TerminologyElement.LongitudeOfTheAscendingNode;

		/// <summary>
		/// Sets the selected element to InclinationToTheEcliptic.
		/// </summary>
		public void SetInclinationToTheEclipticActive() => selectedElement = TerminologyElement.InclinationToTheEcliptic;

		/// <summary>
		/// Sets the selected element to OrbitalEccentricity.
		/// </summary>
		public void SetOrbitalEccentricityActive() => selectedElement = TerminologyElement.OrbitalEccentricity;

		/// <summary>
		/// Sets the selected element to MeanDailyMotion.
		/// </summary>
		public void SetMeanDailyMotionActive() => selectedElement = TerminologyElement.MeanDailyMotion;

		/// <summary>
		/// Sets the selected element to SemiMajorAxis.
		/// </summary>
		public void SetSemiMajorAxisActive() => selectedElement = TerminologyElement.SemiMajorAxis;

		/// <summary>
		/// Sets the selected element to AbsoluteMagnitude.
		/// </summary>
		public void SetAbsoluteMagnitudeActive() => selectedElement = TerminologyElement.AbsoluteMagnitude;

		/// <summary>
		/// Sets the selected element to SlopeParameter.
		/// </summary>
		public void SetSlopeParamActive() => selectedElement = TerminologyElement.SlopeParameter;

		/// <summary>
		/// Sets the selected element to Reference.
		/// </summary>
		public void SetReferenceActive() => selectedElement = TerminologyElement.Reference;

		/// <summary>
		/// Sets the selected element to NumberOfOppositions.
		/// </summary>
		public void SetNumberOfOppositionsActive() => selectedElement = TerminologyElement.NumberOfOppositions;

		/// <summary>
		/// Sets the selected element to NumberOfObservations.
		/// </summary>
		public void SetNumberOfObservationsActive() => selectedElement = TerminologyElement.NumberOfObservations;

		/// <summary>
		/// Sets the selected element to ObservationSpan.
		/// </summary>
		public void SetObservationSpanActive() => selectedElement = TerminologyElement.ObservationSpan;

		/// <summary>
		/// Sets the selected element to RmsResidual.
		/// </summary>
		public void SetRmsResidualActive() => selectedElement = TerminologyElement.RmsResidual;

		/// <summary>
		/// Sets the selected element to ComputerName.
		/// </summary>
		public void SetComputerNameActive() => selectedElement = TerminologyElement.ComputerName;

		/// <summary>
		/// Sets the selected element to Flags.
		/// </summary>
		public void SetFlagsActive() => selectedElement = TerminologyElement.Flags;

		/// <summary>
		/// Sets the selected element to DateOfLastObservation.
		/// </summary>
		public void SetDateOfTheLastObservationActive() => selectedElement = TerminologyElement.DateOfLastObservation;

		/// <summary>
		/// Sets the selected element to LinearEccentricity.
		/// </summary>
		public void SetLinearEccentricityActive() => selectedElement = TerminologyElement.LinearEccentricity;

		/// <summary>
		/// Sets the selected element to SemiMinorAxis.
		/// </summary>
		public void SetSemiMinorAxisActive() => selectedElement = TerminologyElement.SemiMinorAxis;

		/// <summary>
		/// Sets the selected element to MajorAxis.
		/// </summary>
		public void SetMajorAxisActive() => selectedElement = TerminologyElement.MajorAxis;

		/// <summary>
		/// Sets the selected element to MinorAxis.
		/// </summary>
		public void SetMinorAxisActive() => selectedElement = TerminologyElement.MinorAxis;

		/// <summary>
		/// Sets the selected element to EccentricAnomaly.
		/// </summary>
		public void SetEccentricAnomalyActive() => selectedElement = TerminologyElement.EccentricAnomaly;

		/// <summary>
		/// Sets the selected element to TrueAnomaly.
		/// </summary>
		public void SetTrueAnomalyActive() => selectedElement = TerminologyElement.TrueAnomaly;

		/// <summary>
		/// Sets the selected element to PerihelionDistance.
		/// </summary>
		public void SetPerihelionDistanceActive() => selectedElement = TerminologyElement.PerihelionDistance;

		/// <summary>
		/// Sets the selected element to AphelionDistance.
		/// </summary>
		public void SetAphelionDistanceActive() => selectedElement = TerminologyElement.AphelionDistance;

		/// <summary>
		/// Sets the selected element to LongitudeOfTheDescendingNode.
		/// </summary>
		public void SetLongitudeOfTheDescendingNodeActive() => selectedElement = TerminologyElement.LongitudeOfTheDescendingNode;

		/// <summary>
		/// Sets the selected element to ArgumentOfAphelion.
		/// </summary>
		public void SetArgumentOfTheAphelionActive() => selectedElement = TerminologyElement.ArgumentOfAphelion;

		/// <summary>
		/// Sets the selected element to FocalParameter.
		/// </summary>
		public void SetFocalParameterActive() => selectedElement = TerminologyElement.FocalParameter;

		/// <summary>
		/// Sets the selected element to SemiLatusRectum.
		/// </summary>
		public void SetSemiLatusRectumActive() => selectedElement = TerminologyElement.SemiLatusRectum;

		/// <summary>
		/// Sets the selected element to LatusRectum.
		/// </summary>
		public void SetLatusRectumActive() => selectedElement = TerminologyElement.LatusRectum;

		/// <summary>
		/// Sets the selected element to OrbitalPeriod.
		/// </summary>
		public void SetOrbitalPeriodActive() => selectedElement = TerminologyElement.OrbitalPeriod;

		/// <summary>
		/// Sets the selected element to OrbitalArea.
		/// </summary>
		public void SetOrbitalAreaActive() => selectedElement = TerminologyElement.OrbitalArea;

		/// <summary>
		/// Sets the selected element to OrbitalPerimeter.
		/// </summary>
		public void SetOrbitalPerimeterActive() => selectedElement = TerminologyElement.OrbitalPerimeter;

		/// <summary>
		/// Sets the selected element to SemiMeanAxis.
		/// </summary>
		public void SetSemiMeanAxisActive() => selectedElement = TerminologyElement.SemiMeanAxis;

		/// <summary>
		/// Sets the selected element to MeanAxis.
		/// </summary>
		public void SetMeanAxisActive() => selectedElement = TerminologyElement.MeanAxis;

		/// <summary>
		/// Sets the selected element to StandardGravitationalParameter.
		/// </summary>
		public void SetStandardGravitationalParameterActive() => selectedElement = TerminologyElement.StandardGravitationalParameter;

		#endregion

		#region Form event handlers

		/// <summary>
		/// Fired when the form loads.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void TerminologyForm_Load(object sender, EventArgs e)
		{
			// Set the active element based on the selected value
			SetActiveElement();
			// Clear the status bar text
			ClearStatusBar();
		}

		/// <summary>
		/// Fired when the form closes.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		private void TerminologyForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

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

		#region Leave-Handler

		/// <summary>
		/// Called when the mouse pointer leaves a control.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

		#endregion

		#region SelectedValueChanged event handler

		/// <summary>
		/// Handles the event when the selected value in the list box changes.
		/// Updates the selected terminology element and sets the active element accordingly.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void ListBox_SelectedValueChanged(object sender, EventArgs e)
		{
			// Get the selected element from the list box
			selectedElement = (TerminologyElement)listBox.SelectedIndex;
			// Set the active element based on the selected value
			SetActiveElement();
		}

		#endregion
	}
}