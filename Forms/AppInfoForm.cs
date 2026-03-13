// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using NLog;

using Planetoid_DB.Forms;
using Planetoid_DB.Helpers;
using Planetoid_DB.Properties;

using System.Diagnostics;

namespace Planetoid_DB;

/// <summary>A form that displays application information.</summary>
/// <remarks>This form is used to present information about the application, such as its version,
/// description, and copyright details.</remarks>
// You can customize the debugger display for this class by providing a method that returns a string representation of the instance, which will be shown in the debugger when you inspect an object of this class. In this case, the GetDebuggerDisplay method is used to return a string representation of the instance, and the DebuggerDisplay attribute is applied to the class to specify that this method should be used for the debugger display.
[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public partial class AppInfoForm : BaseKryptonForm
{
	/// <summary>NLog logger instance.</summary>
	/// <remarks>This logger is used to log messages and errors for the class.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Gets the status label to be used for displaying information.</summary>
	/// <remarks>Derived classes should override this property to provide the specific label.</remarks>
	protected override ToolStripStatusLabel? StatusLabel => labelInformation;

	#region constructor

	/// <summary>Initializes a new instance of the <see cref="AppInfoForm"/> class.</summary>
	/// <remarks>This constructor initializes the form components.</remarks>
	public AppInfoForm() =>
		// Initialize the form components
		InitializeComponent();

	#endregion

	#region helper methods

	/// <summary>Returns a short debugger display string for this instance.</summary>
	/// <returns>A string representation of the current instance for use in the debugger.</returns>
	/// <remarks>This method is used to provide a visual representation of the object in the debugger.</remarks>
	private string GetDebuggerDisplay() => ToString();

	#endregion

	#region form event handlers

	/// <summary>Fired when the application info form loads.
	/// Populates UI labels with product, version, company, description, and copyright information from the assembly,
	/// sets a static author label, and clears the status area.</summary>
	/// <param name="sender">Event source (the form).</param>
	/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
	/// <remarks>This event initializes the form's UI elements with information from the assembly where available and assigns
	/// a predefined author value to the author label.</remarks>
	private void AppInfoForm_Load(object sender, EventArgs e)
	{
		kryptonLabelTitle.Text = AssemblyInfo.AssemblyProduct;
		kryptonLabelVersion.Text = string.Format(format: I18nStrings.VersionTemplate, arg0: AssemblyInfo.AssemblyVersion);
		kryptonLabelCompany.Text = $"Company: {AssemblyInfo.AssemblyCompany}";
		kryptonLabelAuthor.Text = "Author: Michael Johne";
		kryptonLabelDescription.Text = AssemblyInfo.AssemblyDescription;
		kryptonLabelCopyright.Text = AssemblyInfo.AssemblyCopyright;
		ClearStatusBar(label: labelInformation);
	}

	#endregion

	#region Click event handlers

	/// <summary>Handles the click event for the website link label and opens the configured system homepage in the default web
	/// browser.</summary>
	/// <param name="sender">The source of the event, typically the link label control that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void KryptonLinkLabelWebsite_LinkClick(object sender, EventArgs e) => OpenWebsite(fileName: Settings.Default.systemHomepage);

	/// <summary>Handles the LinkClicked event of the Flaticon link label and opens the associated website.</summary>
	/// <remarks>Use this event handler to navigate to the website specified by the link label's text when the label's
	/// LinkClicked event is raised.</remarks>
	/// <param name="sender">The source of the event, typically the link label control that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void KryptonLinkLabelFlaticon_LinkClick(object sender, EventArgs e) => OpenWebsite(fileName: kryptonLinkLabelFlaticon.Text);

	/// <summary>Handles the click event for the Krypton Suite link label and opens the associated website.</summary>
	/// <remarks>Use this event handler to navigate to the website specified by the link label's text when the label is clicked.</remarks>
	/// <param name="sender">The source of the event, typically the link label control that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void KryptonLinkLabelKryptonSuite_LinkClick(object sender, EventArgs e) => OpenWebsite(fileName: kryptonLinkLabelWebsiteKryptonSuite.Text);

	/// <summary>Handles the click event for the NLog website link label and opens the associated website in the default browser.</summary>
	/// <remarks>This event handler is typically attached to a link label representing the NLog website. When the
	/// link is clicked, the corresponding website URL is opened using the default web browser.</remarks>
	/// <param name="sender">The source of the event, typically the link label control that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void KryptonLinkLabelNLog_LinkClick(object sender, EventArgs e) => OpenWebsite(fileName: kryptonLinkLabelWebsiteNlog.Text);

	/// <summary>Handles the click event for the FatCow Icons website link label and opens the associated website in the default browser.</summary>
	/// <remarks>This event handler is typically attached to a link label representing the FatCow Icons website. When the
	/// link is clicked, the corresponding website URL is opened using the default web browser.</remarks>
	/// <param name="sender">The source of the event, typically the link label control that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void KryptonLinkLabelFatCow_LinkClick(object sender, EventArgs e) => OpenWebsite(fileName: kryptonLinkLabelWebsiteFatcow.Text);

	/// <summary>Handles the click event for the email link label and attempts to open the user's default mail client with a new message addressed to the application's support email.</summary>
	/// <remarks>This event handler is typically attached to a link label representing the application's support email. When the
	/// link is clicked, the default mail client is opened with a new message addressed to the specified email.</remarks>
	/// <param name="sender">The source of the event, typically the link label control that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private async void KryptonLinkLabelEmail_LinkClick(object sender, EventArgs e)
	{
		try
		{
			// Open the default email client with a new message to the specified email address
			using Process process = new();
			process.StartInfo = new ProcessStartInfo(fileName: "mailto:info@planetoid-db.de") { UseShellExecute = true };
			// Start the process asynchronously
			_ = await Task.Run(function: process.Start);
		}
		catch (Exception ex)
		{
			// Log the exception and show an error message
			logger.Error(exception: ex, message: ex.Message);
			// Show an error message if the email client cannot be opened
			ShowErrorMessage(message: $"Error opening the email client: {ex.Message}");
		}
	}

	#endregion
}