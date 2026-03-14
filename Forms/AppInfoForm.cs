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

	/// <summary>Applies a pixelation animation effect to the image displayed in the specified <see cref="PictureBox"/> asynchronously.</summary>
	/// <remarks>
	/// The method temporarily replaces the PictureBox image with a series of progressively pixelated versions of
	/// the original image, created by downscaling and then upscaling the bitmap to produce a blocky effect. The
	/// original image is restored after the animation completes. If the PictureBox does not contain an image, the
	/// method returns immediately.
	/// </remarks>
	/// <param name="pictureBox">The <see cref="PictureBox"/> control whose image will be animated. The control must contain a non-null image.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	private static async Task ApplyZoomAndPixelateAsync(PictureBox pictureBox)
	{
		// Check if the PictureBox contains an image; if not, exit the method
		if (pictureBox.Image == null)
		{
			return;
		}
		// Store the original image to restore it later
		Image orig = pictureBox.Image;
		// Track the previously assigned temporary pixelated bitmap to dispose it correctly.
		Bitmap? previousPixelated = null;

		try
		{
			// Loop to create a pixelation effect by resizing the image to smaller dimensions and then scaling it back up
			for (int pixelSize = 1; pixelSize <= 16; pixelSize += 3)
			{
				// Calculate the size of the smaller image based on the pixelation level
				using Bitmap small = new(width: Math.Max(1, orig.Width / pixelSize), height: Math.Max(val1: 1, val2: orig.Height / pixelSize));
				// Draw the original image onto the smaller bitmap using high-quality bicubic interpolation
				using (Graphics g1 = Graphics.FromImage(image: small))
				{
					// Set the interpolation mode to high-quality bicubic for better resizing quality
					g1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
					// Draw the original image onto the smaller bitmap, effectively reducing its resolution
					g1.DrawImage(image: orig, x: 0, y: 0, width: small.Width, height: small.Height);
				}
				// Create a new bitmap to hold the pixelated version of the image
				Bitmap pixelated = new(width: orig.Width, height: orig.Height);
				// Draw the smaller bitmap onto the pixelated bitmap using nearest neighbor interpolation to create a pixelated effect
				using (Graphics g2 = Graphics.FromImage(image: pixelated))
				{
					// Set the interpolation mode to nearest neighbor to maintain the pixelated look when scaling back up
					g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					// Draw the smaller bitmap onto the pixelated bitmap, scaling it back up to the original size
					g2.DrawImage(image: small, x: 0, y: 0, width: pixelated.Width, height: pixelated.Height);
				}
				// Store the previously assigned temporary pixelated bitmap so it can be safely disposed
				// after the PictureBox has been updated to use the new image.
				Bitmap? oldPixelated = previousPixelated;
				// Update the PictureBox image to the new pixelated version.
				pictureBox.Image = pixelated;
				// Remember the current temporary bitmap so it can be disposed on the next iteration.
				previousPixelated = pixelated;
				// Dispose the previously assigned temporary pixelated bitmap to avoid leaking GDI resources,
				// ensuring that the PictureBox no longer references it.
				oldPixelated?.Dispose();
				// Wait briefly to create an animation effect before the next iteration
				await Task.Delay(millisecondsDelay: 5);
			}

		// Wait briefly before starting the zoom-out effect
		await Task.Delay(millisecondsDelay: 20);

		// Track the previously assigned pixelated bitmap so it can be disposed after it is replaced.
		previousPixelated = null;

		// Loop to create a zoom-out effect by resizing the image back to smaller dimensions and then scaling it back up
		for (int pixelSize = 16; pixelSize >= 1; pixelSize -= 3)
		{
			// Calculate the size of the smaller image based on the pixelation level
			int s = Math.Max(1, pixelSize);

			// Create a smaller bitmap based on the current pixelation level and ensure it is disposed even if an exception occurs.
			using (Bitmap small = new(width: Math.Max(val1: 1, val2: orig.Width / s), height: Math.Max(val1: 1, val2: orig.Height / s)))
			{
				// Draw the original image onto the smaller bitmap using high-quality bicubic interpolation
				using (Graphics g1 = Graphics.FromImage(image: small))
				{
					// Set the interpolation mode to high-quality bicubic for better resizing quality
					g1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
					// Draw the original image onto the smaller bitmap, effectively reducing its resolution
					g1.DrawImage(image: orig, x: 0, y: 0, width: small.Width, height: small.Height);
				}

				// Create a new bitmap to hold the pixelated version of the image
				Bitmap pixelated = new(width: orig.Width, height: orig.Height);

				// Draw the smaller bitmap onto the pixelated bitmap using nearest neighbor interpolation to create a pixelated effect
				using (Graphics g2 = Graphics.FromImage(image: pixelated))
				{
					// Set the interpolation mode to nearest neighbor to maintain the pixelated look when scaling back up
					g2.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
					// Draw the smaller bitmap onto the pixelated bitmap, scaling it back up to the original size
					g2.DrawImage(image: small, x: 0, y: 0, width: pixelated.Width, height: pixelated.Height);
				}

				// Dispose the previously used pixelated bitmap, if any, now that it has been replaced.
				if (previousPixelated != null)
				{
					previousPixelated.Dispose();
				}

				// Update the PictureBox image to the pixelated version
				pictureBox.Image = pixelated;
				previousPixelated = pixelated;
			}

			// Wait briefly to create an animation effect before the next iteration
			await Task.Delay(millisecondsDelay: 5);
		}
	}
	finally
	{
		// Always restore the original image, even if an exception occurs during the animation.
		pictureBox.Image = orig;
		// Ensure the last temporary pixelated bitmap is disposed to avoid leaking GDI resources.
		previousPixelated?.Dispose();
	}
		// Restore the original image in the PictureBox after the animation completes
		pictureBox.Image = orig;
		// Dispose the last pixelated bitmap now that it is no longer displayed.
		if (previousPixelated != null)
		{
			previousPixelated.Dispose();
		}
	}

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

	/// <summary>Handles the LinkClicked event for the website link label and opens the configured system homepage in the default web
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

	/// <summary>Handles the LinkClicked event for the Krypton Suite link label and opens the associated website.</summary>
	/// <remarks>Use this event handler to navigate to the website specified by the link label's text when the label's
	/// LinkClicked event is raised.</remarks>
	/// <param name="sender">The source of the event, typically the link label control that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void KryptonLinkLabelKryptonSuite_LinkClick(object sender, EventArgs e) => OpenWebsite(fileName: kryptonLinkLabelWebsiteKryptonSuite.Text);

	/// <summary>Handles the LinkClicked event for the NLog website link label and opens the associated website in the default browser.</summary>
	/// <remarks>This event handler is typically attached to a link label representing the NLog website. When the
	/// link is clicked, the corresponding website URL is opened using the default web browser.</remarks>
	/// <param name="sender">The source of the event, typically the link label control that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void KryptonLinkLabelNLog_LinkClick(object sender, EventArgs e) => OpenWebsite(fileName: kryptonLinkLabelWebsiteNlog.Text);

	/// <summary>Handles the LinkClicked event for the FatCow Icons website link label and opens the associated website in the default browser.</summary>
	/// <remarks>This event handler is typically attached to a link label representing the FatCow Icons website. When the
	/// link is clicked, the corresponding website URL is opened using the default web browser.</remarks>
	/// <param name="sender">The source of the event, typically the link label control that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private void KryptonLinkLabelFatCow_LinkClick(object sender, EventArgs e) => OpenWebsite(fileName: kryptonLinkLabelWebsiteFatcow.Text);

	/// <summary>
	/// Indicates whether the banner animation is currently running to prevent overlapping animations.
	/// </summary>
	private bool _isBannerAnimationRunning;

	/// <summary>Handles the LinkClicked event for the email link label and attempts to open the user's default mail client with a new message addressed to the application's support email.</summary>
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

	/// <summary>Handles the Click event of the banner PictureBox and initiates an asynchronous operation to apply zoom and
	/// pixelation effects.</summary>
	/// <remarks>This event handler triggers an asynchronous image processing operation on the banner PictureBox
	/// when it is clicked, applying zoom and pixelation effects to the displayed image.</remarks>
	/// <param name="sender">The source of the event, typically the PictureBox control that was clicked.</param>
	/// <param name="e">An EventArgs object that contains the event data.</param>
	private async void PictureBoxBanner_Click(object sender, EventArgs e)
	{
		if (_isBannerAnimationRunning)
		{
			return;
		}

		_isBannerAnimationRunning = true;

		try
		{
			await ApplyZoomAndPixelateAsync(pictureBoxBanner);
		}
		catch (Exception ex)
		{
			// Log any exceptions that occur during the banner animation.
			logger.Error(exception: ex, message: ex.Message);
		}
		finally
		{
			_isBannerAnimationRunning = false;
		}
	}

	#endregion
}