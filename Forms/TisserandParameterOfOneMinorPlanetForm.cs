// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>Form for displaying the Tisserand parameter of a minor planet relative to each of the eight solar system planets.</summary>
/// <remarks>This form computes and presents the Tisserand parameter values for a minor planet using the standard three-body formula. The results are shown in a two-column table layout: planet name in the first column, Tisserand parameter value in the second column.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class TisserandParameterOfOneMinorPlanetForm : BaseKryptonForm
{
	#region Export override properties

	/// <summary>Gets the table layout panel used for export operations.</summary>
	/// <remarks>Overrides the base export source to use this form's table layout panel.</remarks>
	protected override TableLayoutPanel? ExportTableLayoutPanel => tableLayoutPanel;

	/// <summary>Gets the title used for exported data.</summary>
	/// <remarks>Overrides the base export title for this form's content.</remarks>
	protected override string ExportTitle => "Tisserand parameters of a minor planet";

	/// <summary>Gets the file name prefix used for exported files.</summary>
	/// <remarks>Overrides the default export file prefix for this form.</remarks>
	protected override string ExportFilePrefix => "Tisserand-Parameters";

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

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="TisserandParameterOfOneMinorPlanetForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public TisserandParameterOfOneMinorPlanetForm() => InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	/// <summary>Sets the orbital elements of the minor planet used for computing Tisserand parameter values.</summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <param name="eccentricity">The orbital eccentricity.</param>
	/// <param name="inclinationDeg">The inclination to the ecliptic in degrees.</param>
	/// <remarks>Call this method before showing the form so that the Tisserand parameter data is available on load.</remarks>
	public void SetOrbitalElements(
		double semiMajorAxis,
		double eccentricity,
		double inclinationDeg)
	{
		this.semiMajorAxis = semiMajorAxis;
		this.eccentricity = eccentricity;
		this.inclinationDeg = inclinationDeg;
	}

	#endregion

	#region form event handlers

	/// <summary>Handles the Load event. Clears the status bar, computes Tisserand parameter values for all eight planets, and populates the table.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>Tisserand parameter values are calculated using <see cref="TisserandParameterCalculator.CalculateTisserandParameters"/> and displayed in the second column of the <see cref="tableLayoutPanel"/>.</remarks>
	private void TisserandParameterOfOneMinorPlanetForm_Load(object sender, EventArgs e)
	{
		// Clear the status bar
		ClearStatusBar(label: labelInformation);
		try
		{
			// Calculate Tisserand parameters for all 8 planets
			List<TisserandParameterCalculator.TisserandResult> results = TisserandParameterCalculator.CalculateTisserandParameters(
				semiMajorAxis: semiMajorAxis,
				eccentricity: eccentricity,
				inclinationDeg: inclinationDeg);
			// Populate the data labels (one per planet row, index 0 = Mercury … 7 = Neptune)
			if (results.Count >= 8)
			{
				labelMercuryData.Text = results[index: 0].TisserandValue.ToString(format: "F6");
				labelVenusData.Text = results[index: 1].TisserandValue.ToString(format: "F6");
				labelEarthData.Text = results[index: 2].TisserandValue.ToString(format: "F6");
				labelMarsData.Text = results[index: 3].TisserandValue.ToString(format: "F6");
				labelJupiterData.Text = results[index: 4].TisserandValue.ToString(format: "F6");
				labelSaturnData.Text = results[index: 5].TisserandValue.ToString(format: "F6");
				labelUranusData.Text = results[index: 6].TisserandValue.ToString(format: "F6");
				labelNeptuneData.Text = results[index: 7].TisserandValue.ToString(format: "F6");
			}
		}
		// Log any exceptions that occur during the calculation and show an error message to the user
		catch (Exception ex)
		{
			logger.Error(message: $"Error computing Tisserand parameter values: {ex}");
			ShowErrorMessage(message: $"Error computing Tisserand parameter values: {ex.Message}");
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Mercury to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Mercury from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToMercury_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMercuryData.Text);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Venus to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Venus from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToVenus_Click(object sender, EventArgs e) => CopyToClipboard(text: labelVenusData.Text);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Earth to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Earth from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToEarth_Click(object sender, EventArgs e) => CopyToClipboard(text: labelEarthData.Text);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Mars to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Mars from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToMars_Click(object sender, EventArgs e) => CopyToClipboard(text: labelMarsData.Text);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Jupiter to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Jupiter from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToJupiter_Click(object sender, EventArgs e) => CopyToClipboard(text: labelJupiterData.Text);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Saturn to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Saturn from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToSaturn_Click(object sender, EventArgs e) => CopyToClipboard(text: labelSaturnData.Text);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Uranus to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Uranus from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToUranus_Click(object sender, EventArgs e) => CopyToClipboard(text: labelUranusData.Text);

	/// <summary>Handles the click event for copying the Tisserand parameter relative to Neptune to the clipboard.</summary>
	/// <param name="sender">The source of the event, typically the menu item that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	/// <remarks>This event handler retrieves the Tisserand parameter value relative to Neptune from the corresponding label and copies it to the clipboard using the <see cref="BaseKryptonForm.CopyToClipboard(string)"/> helper method.</remarks>
	private void MenuitemCopyToClipboardTisserandParameterRelativeToNeptune_Click(object sender, EventArgs e) => CopyToClipboard(text: labelNeptuneData.Text);

	#endregion
}