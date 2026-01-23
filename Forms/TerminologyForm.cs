using Planetoid_DB.Forms;

using System.Diagnostics;

namespace Planetoid_DB
{
	/// <summary>
	/// Represents a form that displays terminology information.
	/// </summary>
	/// <remarks>
	/// This form provides a user interface for viewing and managing terminology information.
	/// </remarks>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class TerminologyForm : BaseKryptonForm
	{
		/// <summary>
		/// The currently selected terminology element.
		/// </summary>
		/// <remarks>
		/// This field stores the currently selected terminology element.
		/// </remarks>
		private TerminologyElement selectedElement = TerminologyElement.IndexNumber;

		/// <summary>
		/// Enumeration of terminology elements used in the application.
		/// </summary>
		/// <remarks>
		/// This enumeration defines the various terminology elements that can be selected.
		/// </remarks>
		public enum TerminologyElement
		{
			/// <summary>
			/// Index number of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the index number of the element.
			/// </remarks>
			IndexNumber,
			/// <summary>
			/// Readable designation of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the readable designation of the element.
			/// </remarks>
			ReadableDesignation,
			/// <summary>
			/// Epoch of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the epoch of the element.
			/// </remarks>
			Epoch,
			/// <summary>
			/// Mean anomaly at the epoch of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the mean anomaly at the epoch of the element.
			/// </remarks>
			MeanAnomalyAtTheEpoch,
			/// <summary>
			/// Argument of perihelion of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the argument of perihelion of the element.
			/// </remarks>
			ArgumentOfPerihelion,
			/// <summary>
			/// Longitude of the ascending node of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the longitude of the ascending node of the element.
			/// </remarks>
			LongitudeOfTheAscendingNode,
			/// <summary>
			/// Inclination to the ecliptic of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the inclination to the ecliptic of the element.
			/// </remarks>
			InclinationToTheEcliptic,
			/// <summary>
			/// Orbital eccentricity of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the orbital eccentricity of the element.
			/// </remarks>
			OrbitalEccentricity,
			/// <summary>
			/// Mean daily motion of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the mean daily motion of the element.
			/// </remarks>
			MeanDailyMotion,
			/// <summary>
			/// Semi-major axis of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the semi-major axis of the element.
			/// </remarks>
			SemiMajorAxis,
			/// <summary>
			/// Absolute magnitude of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the absolute magnitude of the element.
			/// </remarks>
			AbsoluteMagnitude,
			/// <summary>
			/// Slope parameter of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the slope parameter of the element.
			/// </remarks>
			SlopeParameter,
			/// <summary>
			/// Reference of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the reference of the element.
			/// </remarks>
			Reference,
			/// <summary>
			/// Number of oppositions of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the number of oppositions of the element.
			/// </remarks>
			NumberOfOppositions,
			/// <summary>
			/// Number of observations of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the number of observations of the element.
			/// </remarks>
			NumberOfObservations,
			/// <summary>
			/// Observation span of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the observation span of the element.
			/// </remarks>
			ObservationSpan,
			/// <summary>
			/// RMS residual of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the RMS residual of the element.
			/// </remarks>
			RmsResidual,
			/// <summary>
			/// Computer name of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the computer name of the element.
			/// </remarks>
			ComputerName,
			/// <summary>
			/// Flags of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the flags of the element.
			/// </remarks>
			Flags,
			/// <summary>
			/// Date of last observation of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the date of last observation of the element.
			/// </remarks>
			DateOfLastObservation,
			/// <summary>
			/// Linear eccentricity of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the linear eccentricity of the element.
			/// </remarks>
			LinearEccentricity,
			/// <summary>
			/// Semi-minor axis of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the semi-minor axis of the element.
			/// </remarks>
			SemiMinorAxis,
			/// <summary>
			/// Major axis of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the major axis of the element.
			/// </remarks>
			MajorAxis,
			/// <summary>
			/// Minor axis of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the minor axis of the element.
			/// </remarks>
			MinorAxis,
			/// <summary>
			/// Eccentric anomaly of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the eccentric anomaly of the element.
			/// </remarks>
			EccentricAnomaly,
			/// <summary>
			/// True anomaly of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the true anomaly of the element.
			/// </remarks>
			TrueAnomaly,
			/// <summary>
			/// Perihelion distance of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the perihelion distance of the element.
			/// </remarks>
			PerihelionDistance,
			/// <summary>
			/// Aphelion distance of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the aphelion distance of the element.
			/// </remarks>
			AphelionDistance,
			/// <summary>
			/// Longitude of the descending node of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the longitude of the descending node of the element.
			/// </remarks>
			LongitudeOfTheDescendingNode,
			/// <summary>
			/// Argument of aphelion of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the argument of aphelion of the element.
			/// </remarks>
			ArgumentOfAphelion,
			/// <summary>
			/// Focal parameter of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the focal parameter of the element.
			/// </remarks>
			FocalParameter,
			/// <summary>
			/// Semi-latus rectum of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the semi-latus rectum of the element.
			/// </remarks>
			SemiLatusRectum,
			/// <summary>
			/// Latus rectum of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the latus rectum of the element.
			/// </remarks>
			LatusRectum,
			/// <summary>
			/// Orbital period of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the orbital period of the element.
			/// </remarks>
			OrbitalPeriod,
			/// <summary>
			/// Orbital area of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the orbital area of the element.
			/// </remarks>
			OrbitalArea,
			/// <summary>
			/// Orbital perimeter of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the orbital perimeter of the element.
			/// </remarks>
			OrbitalPerimeter,
			/// <summary>
			/// Semi-mean axis of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the semi-mean axis of the element.
			/// </remarks>
			SemiMeanAxis,
			/// <summary>
			/// Mean axis of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the mean axis of the element.
			/// </remarks>
			MeanAxis,
			/// <summary>
			/// Standard gravitational parameter of the element.
			/// </summary>
			/// <remarks>
			/// This field stores the standard gravitational parameter of the element.
			/// </remarks>
			StandardGravitationalParameter
		}

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
		/// <remarks>
		/// This method sets the selected element to IndexNumber.
		/// </remarks>
		public void SetIndexNumberActive() => selectedElement = TerminologyElement.IndexNumber;

		/// <summary>
		/// Sets the selected element to ReadableDesignation.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to ReadableDesignation.
		/// </remarks>
		public void SetReadableDesignationActive() => selectedElement = TerminologyElement.ReadableDesignation;

		/// <summary>
		/// Sets the selected element to Epoch.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to Epoch.
		/// </remarks>
		public void SetEpochActive() => selectedElement = TerminologyElement.Epoch;

		/// <summary>
		/// Sets the selected element to MeanAnomalyAtTheEpoch.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to MeanAnomalyAtTheEpoch.
		/// </remarks>
		public void SetMeanAnomalyAtTheEpochActive() => selectedElement = TerminologyElement.MeanAnomalyAtTheEpoch;

		/// <summary>
		/// Sets the selected element to ArgumentOfPerihelion.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to ArgumentOfPerihelion.
		/// </remarks>
		public void SetArgumentOfPerihelionActive() => selectedElement = TerminologyElement.ArgumentOfPerihelion;

		/// <summary>
		/// Sets the selected element to LongitudeOfTheAscendingNode.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to LongitudeOfTheAscendingNode.
		/// </remarks>
		public void SetLongitudeOfTheAscendingNodeActive() => selectedElement = TerminologyElement.LongitudeOfTheAscendingNode;

		/// <summary>
		/// Sets the selected element to InclinationToTheEcliptic.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to InclinationToTheEcliptic.
		/// </remarks>
		public void SetInclinationToTheEclipticActive() => selectedElement = TerminologyElement.InclinationToTheEcliptic;

		/// <summary>
		/// Sets the selected element to OrbitalEccentricity.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to OrbitalEccentricity.
		/// </remarks>
		public void SetOrbitalEccentricityActive() => selectedElement = TerminologyElement.OrbitalEccentricity;

		/// <summary>
		/// Sets the selected element to MeanDailyMotion.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to MeanDailyMotion.
		/// </remarks>
		public void SetMeanDailyMotionActive() => selectedElement = TerminologyElement.MeanDailyMotion;

		/// <summary>
		/// Sets the selected element to SemiMajorAxis.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to SemiMajorAxis.
		/// </remarks>
		public void SetSemiMajorAxisActive() => selectedElement = TerminologyElement.SemiMajorAxis;

		/// <summary>
		/// Sets the selected element to AbsoluteMagnitude.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to AbsoluteMagnitude.
		/// </remarks>
		public void SetAbsoluteMagnitudeActive() => selectedElement = TerminologyElement.AbsoluteMagnitude;

		/// <summary>
		/// Sets the selected element to SlopeParameter.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to SlopeParameter.
		/// </remarks>
		public void SetSlopeParamActive() => selectedElement = TerminologyElement.SlopeParameter;

		/// <summary>
		/// Sets the selected element to Reference.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to Reference.
		/// </remarks>
		public void SetReferenceActive() => selectedElement = TerminologyElement.Reference;

		/// <summary>
		/// Sets the selected element to NumberOfOppositions.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to NumberOfOppositions.
		/// </remarks>
		public void SetNumberOfOppositionsActive() => selectedElement = TerminologyElement.NumberOfOppositions;

		/// <summary>
		/// Sets the selected element to NumberOfObservations.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to NumberOfObservations.
		/// </remarks>
		public void SetNumberOfObservationsActive() => selectedElement = TerminologyElement.NumberOfObservations;

		/// <summary>
		/// Sets the selected element to ObservationSpan.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to ObservationSpan.
		/// </remarks>
		public void SetObservationSpanActive() => selectedElement = TerminologyElement.ObservationSpan;

		/// <summary>
		/// Sets the selected element to RmsResidual.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to RmsResidual.
		/// </remarks>
		public void SetRmsResidualActive() => selectedElement = TerminologyElement.RmsResidual;

		/// <summary>
		/// Sets the selected element to ComputerName.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to ComputerName.
		/// </remarks>
		public void SetComputerNameActive() => selectedElement = TerminologyElement.ComputerName;

		/// <summary>
		/// Sets the selected element to Flags.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to Flags.
		/// </remarks>
		public void SetFlagsActive() => selectedElement = TerminologyElement.Flags;

		/// <summary>
		/// Sets the selected element to DateOfLastObservation.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to DateOfLastObservation.
		/// </remarks>
		public void SetDateOfTheLastObservationActive() => selectedElement = TerminologyElement.DateOfLastObservation;

		/// <summary>
		/// Sets the selected element to LinearEccentricity.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to LinearEccentricity.
		/// </remarks>
		public void SetLinearEccentricityActive() => selectedElement = TerminologyElement.LinearEccentricity;

		/// <summary>
		/// Sets the selected element to SemiMinorAxis.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to SemiMinorAxis.
		/// </remarks>
		public void SetSemiMinorAxisActive() => selectedElement = TerminologyElement.SemiMinorAxis;

		/// <summary>
		/// Sets the selected element to MajorAxis.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to MajorAxis.
		/// </remarks>
		public void SetMajorAxisActive() => selectedElement = TerminologyElement.MajorAxis;

		/// <summary>
		/// Sets the selected element to MinorAxis.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to MinorAxis.
		/// </remarks>
		public void SetMinorAxisActive() => selectedElement = TerminologyElement.MinorAxis;

		/// <summary>
		/// Sets the selected element to EccentricAnomaly.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to EccentricAnomaly.
		/// </remarks>
		public void SetEccentricAnomalyActive() => selectedElement = TerminologyElement.EccentricAnomaly;

		/// <summary>
		/// Sets the selected element to TrueAnomaly.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to TrueAnomaly.
		/// </remarks>
		public void SetTrueAnomalyActive() => selectedElement = TerminologyElement.TrueAnomaly;

		/// <summary>
		/// Sets the selected element to PerihelionDistance.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to PerihelionDistance.
		/// </remarks>
		public void SetPerihelionDistanceActive() => selectedElement = TerminologyElement.PerihelionDistance;

		/// <summary>
		/// Sets the selected element to AphelionDistance.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to AphelionDistance.
		/// </remarks>
		public void SetAphelionDistanceActive() => selectedElement = TerminologyElement.AphelionDistance;

		/// <summary>
		/// Sets the selected element to LongitudeOfTheDescendingNode.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to LongitudeOfTheDescendingNode.
		/// </remarks>
		public void SetLongitudeOfTheDescendingNodeActive() => selectedElement = TerminologyElement.LongitudeOfTheDescendingNode;

		/// <summary>
		/// Sets the selected element to ArgumentOfAphelion.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to ArgumentOfAphelion.
		/// </remarks>
		public void SetArgumentOfTheAphelionActive() => selectedElement = TerminologyElement.ArgumentOfAphelion;

		/// <summary>
		/// Sets the selected element to FocalParameter.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to FocalParameter.
		/// </remarks>
		public void SetFocalParameterActive() => selectedElement = TerminologyElement.FocalParameter;

		/// <summary>
		/// Sets the selected element to SemiLatusRectum.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to SemiLatusRectum.
		/// </remarks>
		public void SetSemiLatusRectumActive() => selectedElement = TerminologyElement.SemiLatusRectum;

		/// <summary>
		/// Sets the selected element to LatusRectum.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to LatusRectum.
		/// </remarks>
		public void SetLatusRectumActive() => selectedElement = TerminologyElement.LatusRectum;

		/// <summary>
		/// Sets the selected element to OrbitalPeriod.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to OrbitalPeriod.
		/// </remarks>
		public void SetOrbitalPeriodActive() => selectedElement = TerminologyElement.OrbitalPeriod;

		/// <summary>
		/// Sets the selected element to OrbitalArea.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to OrbitalArea.
		/// </remarks>
		public void SetOrbitalAreaActive() => selectedElement = TerminologyElement.OrbitalArea;

		/// <summary>
		/// Sets the selected element to OrbitalPerimeter.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to OrbitalPerimeter.
		/// </remarks>
		public void SetOrbitalPerimeterActive() => selectedElement = TerminologyElement.OrbitalPerimeter;

		/// <summary>
		/// Sets the selected element to SemiMeanAxis.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to SemiMeanAxis.
		/// </remarks>
		public void SetSemiMeanAxisActive() => selectedElement = TerminologyElement.SemiMeanAxis;

		/// <summary>
		/// Sets the selected element to MeanAxis.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to MeanAxis.
		/// </remarks>
		public void SetMeanAxisActive() => selectedElement = TerminologyElement.MeanAxis;

		/// <summary>
		/// Sets the selected element to StandardGravitationalParameter.
		/// </summary>
		/// <remarks>
		/// This method sets the selected element to StandardGravitationalParameter.
		/// </remarks>
		public void SetStandardGravitationalParameterActive() => selectedElement = TerminologyElement.StandardGravitationalParameter;

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

		/// <summary>
		/// Fired when the form is closed. Disposes managed resources associated with the form.
		/// </summary>
		/// <param name="sender">The event source (the form).</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		/// <remarks>
		/// This event is triggered when the form is closed.
		/// </remarks>
		private void TerminologyForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();
		#endregion

		#region Enter event handlers

		/// <summary>
		/// Handles Enter (mouse over / focus) events for controls and ToolStrip items.
		/// If the sender provides a non-null <c>AccessibleDescription</c>, that text is shown in the status bar.
		/// </summary>
		/// <param name="sender">Event source — expected to be a <see cref="Control"/> or <see cref="ToolStripItem"/>.</param>
		/// <param name="e">Event arguments.</param>
		/// <remarks>
		/// This event is triggered when the mouse pointer enters a control or the control receives focus.
		/// </remarks>
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
		/// <remarks>
		/// This event is triggered when the mouse pointer leaves a control or the control loses focus.
		/// </remarks>
		private void ClearStatusBar_Leave(object sender, EventArgs e) => ClearStatusBar();

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
			selectedElement = (TerminologyElement)listBox.SelectedIndex;
			// Set the active element based on the selected value
			SetActiveElement();
		}

		#endregion
	}
}