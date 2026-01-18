using NLog;

using System.Diagnostics;

namespace Planetoid_DB
{
	/// <summary>
	/// Represents the splash screen form of the application.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public partial class SplashScreenForm : BaseKryptonForm
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger(); // NLog logger instance

		#region constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SplashScreenForm"/> class.
		/// </summary>
		public SplashScreenForm() => InitializeComponent();

		#endregion

		#region helper methods

		/// <summary>
		/// Returns a short debugger display string for this instance.
		/// </summary>
		/// <returns>A string representation of the current instance for use in the debugger.</returns>
		private string GetDebuggerDisplay() => ToString();

		/// <summary>
		/// Sets the splash screen progress bar value.
		/// </summary>
		/// <param name="value">The value to set on the progress bar. Must be between <c>progressBarSplash.Minimum</c> and <c>progressBarSplash.Maximum</c>.</param>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> is outside the valid range of the progress bar.</exception>
		public void SetProgressbar(int value)
		{
			// Validate the value
			// Check if the value is within the range of the progress bar
			// If the value is less than the minimum or greater than the maximum, throw an exception
			if (value < progressBarSplash.Minimum || value > progressBarSplash.Maximum)
			{
				// Log the error and throw an exception
				Logger.Error(message: $"Value {value} is out of range for the progress bar. Minimum: {progressBarSplash.Minimum}, Maximum: {progressBarSplash.Maximum}");
				// Throw an exception indicating that the value is out of range
				throw new ArgumentOutOfRangeException(paramName: nameof(value), message: I10nStrings.IndexOutOfRange);
			}
			// Set the value of the progress bar
			progressBarSplash.Value = value;
		}

		#endregion

		#region form event handlers

		/// <summary>
		/// Fired when the splash screen form loads.
		/// Sets the product title and the formatted version text on the form's labels.
		/// </summary>
		/// <param name="sender">Event source (the form).</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void SplashScreenForm_Load(object sender, EventArgs e)
		{
			// Set the title label text to the product name
			labelTitle.Text = AssemblyInfo.AssemblyProduct;
			// Set the version label text to the assembly version
			labelVersion.Text = string.Format(format: I10nStrings.VersionTemplate, arg0: AssemblyInfo.AssemblyVersion);
		}

		/// <summary>
		/// Fired when the splash screen form is closed.
		/// Disposes managed resources associated with the form.
		/// </summary>
		/// <param name="sender">Event source (the form).</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		private void SplashScreenForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();
		#endregion

		#region DoubleClick event handlers

		/// <summary>
		/// Called when a control is double-clicked. If the <paramref name="sender"/> is a <see cref="Control"/>,
		/// its <see cref="Control.Text"/> value is copied to the clipboard using the shared helper.
		/// </summary>
		/// <param name="sender">Event source — expected to be a <see cref="Control"/>.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void CopyToClipboard_DoubleClick(object sender, EventArgs e)
		{
			// Check if the sender is null
			ArgumentNullException.ThrowIfNull(argument: sender);
			if (sender is Control control)
			{
				// Copy the text to the clipboard
				CopyToClipboard(text: control.Text);
			}
		}

		#endregion
	}
}