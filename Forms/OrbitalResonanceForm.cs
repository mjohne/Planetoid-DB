using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>
/// Form for displaying orbital resonances of a planetoid relative to the 8 solar system planets.
/// </summary>
/// <remarks>
/// This form computes and presents the orbital resonance of a planetoid with each planet,
/// including the resonance ratio, deviation, and whether a near-resonance is detected.
/// </remarks>
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class OrbitalResonanceForm : BaseKryptonForm
{
	/// <summary>
	/// NLog logger instance.
	/// </summary>
	/// <remarks>
	/// This logger is used throughout the form to log important events and errors.
	/// </remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>
	/// The deviation threshold in percent below which an orbital ratio is considered a near-resonance.
	/// </summary>
	private const double ResonanceThresholdPercent = 1.0;

	/// <summary>
	/// The semi-major axis of the planetoid in AU, used to calculate orbital resonances.
	/// </summary>
	/// <remarks>
	/// Set this value via <see cref="SetSemiMajorAxis"/> before the form is shown.
	/// </remarks>
	private double semiMajorAxis;

	/// <summary>
	/// Gets the status label used for displaying information in the status bar.
	/// </summary>
	/// <remarks>
	/// Overrides the base class property to return the form-specific status label.
	/// </remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="OrbitalResonanceForm"/> class.
	/// </summary>
	/// <remarks>
	/// This constructor initializes the form components.
	/// </remarks>
	public OrbitalResonanceForm() =>
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
	/// Sets the semi-major axis of the planetoid used for computing orbital resonances.
	/// </summary>
	/// <param name="semiMajorAxis">The semi-major axis in AU.</param>
	/// <remarks>
	/// Call this method before showing the form so that the resonance data is available on load.
	/// </remarks>
	public void SetSemiMajorAxis(double semiMajorAxis) =>
		this.semiMajorAxis = semiMajorAxis;

	/// <summary>
	/// Populates the <see cref="listView"/> with orbital resonance data for the given resonances.
	/// </summary>
	/// <param name="resonances">The list of orbital resonances to display.</param>
	/// <remarks>
	/// Each resonance is shown as one row. The "Is Resonance" column shows "Yes" when the deviation is below 1 %.
	/// </remarks>
	private void PopulateListView(List<DerivedElements.OrbitalResonance> resonances)
	{
		listView.BeginUpdate();
		listView.Items.Clear();
		foreach (DerivedElements.OrbitalResonance resonance in resonances)
		{
			string isResonance = resonance.DeviationPercent < ResonanceThresholdPercent ? "Yes" : "No";
			ListViewItem item = new(text: resonance.PlanetName);
			item.SubItems.AddRange(items: new[]
			{
				resonance.PlanetPeriod.ToString(format: "F6"),
				resonance.PlanetoidPeriod.ToString(format: "F6"),
				resonance.Ratio.ToString(format: "F6"),
				$"{resonance.ResonanceP}:{resonance.ResonanceQ}",
				resonance.DeviationPercent.ToString(format: "F2"),
				isResonance
			});
			listView.Items.Add(value: item);
		}
		listView.EndUpdate();
	}

	#endregion

	#region form event handlers

	/// <summary>
	/// Handles the form Load event.
	/// Clears the status bar, computes orbital resonances for the stored semi-major axis and populates the list view.
	/// </summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// If an error occurs during calculation, it is logged and an error message is shown to the user.
	/// </remarks>
	private void OrbitalResonanceForm_Load(object sender, EventArgs e)
	{
		ClearStatusBar(label: labelInformation);
		try
		{
			List<DerivedElements.OrbitalResonance> resonances = DerivedElements.CalculateOrbitalResonances(semiMajorAxis: semiMajorAxis);
			PopulateListView(resonances: resonances);
		}
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: ex.Message);
			ShowErrorMessage(message: $"Error calculating orbital resonances: {ex.Message}");
		}
	}

	#endregion

	#region Click event handlers

	/// <summary>
	/// Handles the Click event of the copy-to-clipboard menu item.
	/// Copies the text of the currently selected list view row to the clipboard.
	/// </summary>
	/// <param name="sender">Event source (the menu item).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>
	/// All sub-items of the selected row are joined with a tab character before being placed on the clipboard.
	/// If no row is selected the method returns without action.
	/// </remarks>
	private void ToolStripMenuItemCopyToClipboard_Click(object sender, EventArgs e)
	{
		if (listView.SelectedItems.Count == 0)
		{
			return;
		}
		ListViewItem selectedItem = listView.SelectedItems[index: 0];
		IEnumerable<string> subItemTexts = selectedItem.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(selector: static s => s.Text);
		string text = string.Join(separator: "\t", values: subItemTexts);
		CopyToClipboard(text: text);
	}

	#endregion
}
