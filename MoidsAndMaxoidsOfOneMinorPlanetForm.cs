// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Form for displaying the Minimum Orbit Intersection Distance (MOID) and Maximum Orbit Intersection Distance (MAXOID) of a minor planet relative to each of the eight solar system planets.</summary>
/// <remarks>This form computes and presents the MOID and MAXOID values for a minor planet using a fast, high-precision numerical algorithm equivalent to the approach used by the Minor Planet Center (MPC). The results are shown in a three-column table layout: planet name in the first column, MOID in AU in the second column, and MAXOID in AU in the third column.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class MoidsAndMaxoidsOfOneMinorPlanetForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the table layout panel used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's table layout panel.</remarks>
	protected override TableLayoutPanel? ExportTableLayoutPanel => tableLayoutPanel;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "MOIDs and MAXOIDs of a minor planet";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "MOIDs_AND_MAXOIDs";

	#endregion

	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used throughout the form to log important events and errors.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label used for displaying information in the status bar.</summary>
	/// <remarks>Overrides the base class property to return the form-specific status label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	/// <summary>Semi-major axis of the minor planet in AU.</summary>
	/// <remarks>Set via <see cref="SetOrbitalElements"/> before the form is shown.</remarks>
	private double semiMajorAxis;

	/// <summary>Orbital eccentricity of the minor planet.</summary>
	/// <remarks>Set via <see cref="SetOrbitalElements"/> before the form is shown.</remarks>
	private double eccentricity;

	/// <summary>Orbital inclination of the minor planet in degrees.</summary>
	/// <remarks>Set via <see cref="SetOrbitalElements"/> before the form is shown.</remarks>
	private double inclinationDeg;

	/// <summary>Longitude of the ascending node of the minor planet in degrees.</summary>
	/// <remarks>Set via <see cref="SetOrbitalElements"/> before the form is shown.</remarks>
	private double longitudeAscendingNodeDeg;

	/// <summary>Argument of perihelion of the minor planet in degrees.</summary>
	/// <remarks>Set via <see cref="SetOrbitalElements"/> before the form is shown.</remarks>
	private double argumentPerihelionDeg;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="MoidsAndMaxoidsOfOneMinorPlanetForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public MoidsAndMaxoidsOfOneMinorPlanetForm() => InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Sets the orbital elements of the minor planet used for computing MOID and MAXOID values.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="eccentricity">The orbital eccentricity.</param>
	/// <param name="inclinationDeg">The inclination to the ecliptic in degrees.</param>
	/// <param name="longitudeAscendingNodeDeg">The longitude of the ascending node in degrees.</param>
	/// <param name="argumentPerihelionDeg">The argument of perihelion in degrees.</param>
	/// <remarks>Call this method before showing the form so that the MOID and MAXOID data is available on load.</remarks>
	public void SetOrbitalElements(
		double semiMajorAxis,
		double eccentricity,
		double inclinationDeg,
		double longitudeAscendingNodeDeg,
		double argumentPerihelionDeg)
	{
		this.semiMajorAxis = semiMajorAxis;
		this.eccentricity = eccentricity;
		this.inclinationDeg = inclinationDeg;
		this.longitudeAscendingNodeDeg = longitudeAscendingNodeDeg;
		this.argumentPerihelionDeg = argumentPerihelionDeg;
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event. Clears the status bar, computes MOID and MAXOID values for all eight planets, and populates the table.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>MOID and MAXOID values are calculated using <see cref="MoidCalculator.CalculateMoids"/> and displayed in the second column of the <see cref="tableLayoutPanel"/>.</remarks>
	private void MoidsAndMaxoidsOfOneMinorPlanetForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		try
		{
			// Calculate MOIDs for all 8 planets
			List<MoidCalculator.MoidResult> moids = MoidCalculator.CalculateMoids(
				semiMajorAxis: semiMajorAxis,
				eccentricity: eccentricity,
				inclinationDeg: inclinationDeg,
				longitudeAscendingNodeDeg: longitudeAscendingNodeDeg,
				argumentPerihelionDeg: argumentPerihelionDeg);
			// Populate the data labels (one per planet row, index 0 = Mercury … 7 = Neptune)
			if (moids.Count >= 8)
			{
				labelMercuryMoidData.Text = moids[index: 0].MoidAu.ToString(format: "F8");
				labelVenusMoidData.Text = moids[index: 1].MoidAu.ToString(format: "F8");
				labelEarthMoidData.Text = moids[index: 2].MoidAu.ToString(format: "F8");
				labelMarsMoidData.Text = moids[index: 3].MoidAu.ToString(format: "F8");
				labelJupiterMoidData.Text = moids[index: 4].MoidAu.ToString(format: "F8");
				labelSaturnMoidData.Text = moids[index: 5].MoidAu.ToString(format: "F8");
				labelUranusMoidData.Text = moids[index: 6].MoidAu.ToString(format: "F8");
				labelNeptuneMoidData.Text = moids[index: 7].MoidAu.ToString(format: "F8");
			}
			// Calculate MAXOIDs for all 8 planets
			List<MaxoidCalculator.MaxoidResult> maxoids = MaxoidCalculator.CalculateMaxoids(
				semiMajorAxis: semiMajorAxis,
				eccentricity: eccentricity,
				inclinationDeg: inclinationDeg,
				longitudeAscendingNodeDeg: longitudeAscendingNodeDeg,
				argumentPerihelionDeg: argumentPerihelionDeg);
			// Populate the data labels (one per planet row, index 0 = Mercury … 7 = Neptune)
			if (maxoids.Count >= 8)
			{
				labelMercuryMaxoidData.Text = maxoids[index: 0].MaxoidAu.ToString(format: "F8");
				labelVenusMaxoidData.Text = maxoids[index: 1].MaxoidAu.ToString(format: "F8");
				labelEarthMaxoidData.Text = maxoids[index: 2].MaxoidAu.ToString(format: "F8");
				labelMarsMaxoidData.Text = maxoids[index: 3].MaxoidAu.ToString(format: "F8");
				labelJupiterMaxoidData.Text = maxoids[index: 4].MaxoidAu.ToString(format: "F8");
				labelSaturnMaxoidData.Text = maxoids[index: 5].MaxoidAu.ToString(format: "F8");
				labelUranusMaxoidData.Text = maxoids[index: 6].MaxoidAu.ToString(format: "F8");
				labelNeptuneMaxoidData.Text = maxoids[index: 7].MaxoidAu.ToString(format: "F8");
			}
		}
		// Handle any exceptions that may occur during MOID and MAXOID calculation and display an error message
		catch (Exception ex)
		{
			logger.Error(message: $"Error computing MOID and MAXOID values: {ex}");
			ShowErrorMessage(message: $"Error computing MOID and MAXOID values: {ex.Message}");
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the click event for copying the Mercury MOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Mercury data to the system clipboard. Use this menu item to quickly copy the MOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMoidRelativeToMercury_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMercuryMoidData.Text);

	/// <summary>Handles the click event for copying the Mercury MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Mercury data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToMercury_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMercuryMaxoidData.Text);

	/// <summary>Handles the click event for copying the Venus MOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Venus data to the system clipboard. Use this menu item to quickly copy the MOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMoidRelativeToVenus_Click(object sender, EventArgs e) => CopyToClipboard(text: labelVenusMoidData.Text);

	/// <summary>Handles the click event for copying the Venus MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Venus data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToVenus_Click(object sender, EventArgs e) => CopyToClipboard(text: labelVenusMaxoidData.Text);

	/// <summary>Handles the click event for copying the Earth MOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Earth data to the system clipboard. Use this menu item to quickly copy the MOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMoidRelativeToEarth_Click(object sender, EventArgs e) => CopyToClipboard(text: labelEarthMoidData.Text);

	/// <summary>Handles the click event for copying the Earth MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Earth data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToEarth_Click(object sender, EventArgs e) => CopyToClipboard(text: labelEarthMaxoidData.Text);

	/// <summary>Handles the click event for copying the Mars MOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Mars data to the system clipboard. Use this menu item to quickly copy the MOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMoidRelativeToMars_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMarsMoidData.Text);

	/// <summary>Handles the click event for copying the Mars MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Mars data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToMars_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMarsMaxoidData.Text);

	/// <summary>Handles the click event for copying the Jupiter MOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Jupiter data to the system clipboard. Use this menu item to quickly copy the MOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMoidRelativeToJupiter_Click(object sender, EventArgs e) => CopyToClipboard(text: labelJupiterMoidData.Text);

	/// <summary>Handles the click event for copying the Jupiter MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Jupiter data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToJupiter_Click(object sender, EventArgs e) => CopyToClipboard(text: labelJupiterMaxoidData.Text);

	/// <summary>Handles the click event for copying the Saturn MOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Saturn data to the system clipboard. Use this menu item to quickly copy the MOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMoidRelativeToSaturn_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSaturnMoidData.Text);

	/// <summary>Handles the click event for copying the Saturn MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Saturn data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToSaturn_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSaturnMaxoidData.Text);

	/// <summary>Handles the click event for copying the Uranus MOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Uranus data to the system clipboard. Use this menu item to quickly copy the MOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMoidRelativeToUranus_Click(object sender, EventArgs e) => CopyToClipboard(text: labelUranusMoidData.Text);

	/// <summary>Handles the click event for copying the Uranus MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Uranus data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToUranus_Click(object sender, EventArgs e) => CopyToClipboard(text: labelUranusMaxoidData.Text);

	/// <summary>Handles the click event for copying the Neptune MOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Neptune data to the system clipboard. Use this menu item to quickly copy the MOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMoidRelativeToNeptune_Click(object sender, EventArgs e) => CopyToClipboard(text: labelNeptuneMoidData.Text);

	/// <summary>Handles the click event for copying the Neptune MAXOID value to the clipboard.</summary>
	/// <remarks>This event handler copies the current text value associated with Neptune data to the system clipboard. Use this menu item to quickly copy the MAXOID value for further use.</remarks>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void MenuitemCopyToClipboardMaxoidRelativeToNeptune_Click(object sender, EventArgs e) => CopyToClipboard(text: labelNeptuneMaxoidData.Text);

	#endregion
}
