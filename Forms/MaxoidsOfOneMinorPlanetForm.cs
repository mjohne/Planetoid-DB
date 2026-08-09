// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;
using System.Globalization;

namespace Planetoid_DB;

/// <summary>Form for displaying the Maximum Orbit Intersection Distance (MAXOID) of a minor planet relative to each of the eight solar system planets.</summary>
/// <remarks>This form computes and presents the MAXOID values for a minor planet using a fast, high-precision numerical algorithm equivalent to the approach used by the Minor Planet Center (MPC). The results are shown in a two-column table layout: planet name in the first column, MAXOID in AU in the second column.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class MaxoidsOfOneMinorPlanetForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the table layout panel used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's table layout panel.</remarks>
	protected override TableLayoutPanel? ExportTableLayoutPanel => tableLayoutPanel;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "MAXOIDs of a minor planet";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "MAXOIDs";

	#endregion

	/// <summary>Represents the orbital elements of a minor planet used for MAXOID calculations.</summary>
	/// <param name="SemiMajorAxis">The semi-major axis of the orbit in astronomical units (AU).</param>
	/// <param name="Eccentricity">The orbital eccentricity.</param>
	/// <param name="InclinationDeg">The orbital inclination in degrees.</param>
	/// <param name="LongitudeAscendingNodeDeg">The longitude of the ascending node in degrees.</param>
	/// <param name="ArgumentPerihelionDeg">The argument of perihelion in degrees.</param>
	/// <remarks>This record is used to encapsulate the orbital parameters of a minor planet for MAXOID calculations.</remarks>
	public record OrbitalElements(
		double SemiMajorAxis,
		double Eccentricity,
		double InclinationDeg,
		double LongitudeAscendingNodeDeg,
		double ArgumentPerihelionDeg
	);

	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Represents the orbital elements of the minor planet for which MAXOID values are being calculated.</summary>
	/// <remarks>This field is set via the SetOrbitalElements method and is used during the form's Load event to compute MAXOID values.</remarks>
	private OrbitalElements? _orbitalElements;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="MaxoidsOfOneMinorPlanetForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public MaxoidsOfOneMinorPlanetForm() => InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Sets the orbital elements of the minor planet used for computing MAXOID values.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="eccentricity">The orbital eccentricity.</param>
	/// <param name="inclinationDeg">The inclination to the ecliptic in degrees.</param>
	/// <param name="longitudeAscendingNodeDeg">The longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelionDeg">The argument of perihelion in degrees.</param>
	/// <remarks>Call this method before showing the form so that the MAXOID data is available on load.</remarks>
	public void SetOrbitalElements(
		double semiMajorAxis,
		double eccentricity,
		double inclinationDeg,
		double longitudeAscendingNodeDeg,
		double argumentPerihelionDeg)
	{
		// Create a new instance of OrbitalElements with the provided parameters and assign it to the private field
		_orbitalElements = new OrbitalElements(
			SemiMajorAxis: semiMajorAxis,
			Eccentricity: eccentricity,
			InclinationDeg: inclinationDeg,
			LongitudeAscendingNodeDeg: longitudeAscendingNodeDeg,
			ArgumentPerihelionDeg: argumentPerihelionDeg);
	}

	/// <summary>Sets the orbital elements of the minor planet used for computing MAXOID values.</summary>
	/// <param name="elements">The orbital elements to set.</param>
	/// <remarks>Call this method before showing the form so that the MAXOID data is available on load.</remarks>
	public void SetOrbitalElements(OrbitalElements elements) => _orbitalElements = elements ?? throw new ArgumentNullException(paramName: nameof(elements));

	/// <summary>Updates the UI labels with the calculated data using InvariantCulture.</summary>
	/// <param name="maxoids">The list of MAXOID results for each planet.</param>
	/// <remarks>This method updates the text of the labels for each planet with the corresponding MAXOID value formatted to eight decimal places. It uses InvariantCulture to ensure consistent formatting regardless of the system's locale.</remarks>
	private void UpdatePlanetLabels(List<MaxoidCalculator.MaxoidResult> maxoids)
	{
		// Ensure that we have results for all eight planets before updating the labels
		if (maxoids.Count < 8)
		{
			logger.Warn(message: "Insufficient MAXOID results to update planet labels. Expected 8, but got {0}.", maxoids.Count);
			return;
		}
		// Use InvariantCulture for consistent formatting of the MAXOID values
		CultureInfo culture = CultureInfo.InvariantCulture;
		// Update each label with the corresponding MAXOID value formatted to eight decimal places
		labelMercuryData.Text = maxoids[0].MaxoidAu.ToString(provider: culture);
		labelVenusData.Text = maxoids[1].MaxoidAu.ToString(provider: culture);
		labelEarthData.Text = maxoids[2].MaxoidAu.ToString(provider: culture);
		labelMarsData.Text = maxoids[3].MaxoidAu.ToString(provider: culture);
		labelJupiterData.Text = maxoids[4].MaxoidAu.ToString(provider: culture);
		labelSaturnData.Text = maxoids[5].MaxoidAu.ToString(provider: culture);
		labelUranusData.Text = maxoids[6].MaxoidAu.ToString(provider: culture);
		labelNeptuneData.Text = maxoids[7].MaxoidAu.ToString(provider: culture);
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event. Clears the status bar, computes MAXOID values for all eight planets, and populates the table.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>MAXOID values are calculated using <see cref="MaxoidCalculator.CalculateMaxoids"/> and displayed in the second column of the <see cref="tableLayoutPanel"/>.</remarks>
	private void MaxoidsOfOneMinorPlanetForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		try
		{
			// Ensure that orbital elements have been set before attempting to compute MAXOID values
			if (_orbitalElements == null)
			{
				throw new InvalidOperationException(message: "Orbital elements have not been set.");
			}

			// Calculate MAXOIDs for all 8 planets
			List<MaxoidCalculator.MaxoidResult> maxoids = MaxoidCalculator.CalculateMaxoids(
				semiMajorAxis: _orbitalElements.SemiMajorAxis,
				eccentricity: _orbitalElements.Eccentricity,
				inclinationDeg: _orbitalElements.InclinationDeg,
				longitudeAscendingNodeDeg: _orbitalElements.LongitudeAscendingNodeDeg,
				argumentPerihelionDeg: _orbitalElements.ArgumentPerihelionDeg);
			// Update the UI labels with the calculated MAXOID values
			UpdatePlanetLabels(maxoids: maxoids);
		}
		// Handle any exceptions that may occur during MAXOID calculation and display an error message
		catch (Exception ex)
		{
			logger.Error(message: $"Error computing MAXOID values: {ex}");
			ShowErrorMessage(message: $"Error computing MAXOID values: {ex.Message}");
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the click event for copying the Mercury MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Mercury data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToMercury_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Copying Mercury MAXOID value to clipboard: {0}", labelMercuryData.Text);
		CopyToClipboard(text: labelMercuryData.Text);
	}

	/// <summary>Handles the click event for copying the Venus MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Venus data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToVenus_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Copying Venus MAXOID value to clipboard: {0}", labelVenusData.Text);
		CopyToClipboard(text: labelVenusData.Text);
	}

	/// <summary>Handles the click event for copying the Earth MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Earth data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToEarth_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Copying Earth MAXOID value to clipboard: {0}", labelEarthData.Text);
		CopyToClipboard(text: labelEarthData.Text);
	}

	/// <summary>Handles the click event for copying the Mars MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Mars data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToMars_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Copying Mars MAXOID value to clipboard: {0}", labelMarsData.Text);
		CopyToClipboard(text: labelMarsData.Text);
	}

	/// <summary>Handles the click event for copying the Jupiter MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Jupiter data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToJupiter_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Copying Jupiter MAXOID value to clipboard: {0}", labelJupiterData.Text);
		CopyToClipboard(text: labelJupiterData.Text);
	}

	/// <summary>Handles the click event for copying the Saturn MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Saturn data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToSaturn_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Copying Saturn MAXOID value to clipboard: {0}", labelSaturnData.Text);
		CopyToClipboard(text: labelSaturnData.Text);
	}

	/// <summary>Handles the click event for copying the Uranus MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Uranus data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToUranus_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Copying Uranus MAXOID value to clipboard: {0}", labelUranusData.Text);
		CopyToClipboard(text: labelUranusData.Text);
	}

	/// <summary>Handles the click event for copying the Neptune MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Neptune data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToNeptune_Click(object sender, EventArgs e)
	{
		logger.Info(message: "Copying Neptune MAXOID value to clipboard: {0}", labelNeptuneData.Text);
		CopyToClipboard(text: labelNeptuneData.Text);
	}

	#endregion
}