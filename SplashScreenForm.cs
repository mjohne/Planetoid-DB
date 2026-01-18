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

		#region local methods

		/// <summary>
		/// Returns a string representation of the object for the debugger.
		/// </summary>
		/// <returns>A string representation of the object.</returns>
		private string GetDebuggerDisplay() => ToString();

		/// <summary>
		/// Sets the value of the progress bar.
		/// </summary>
		/// <param name="value">The progress value.</param>
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

		#region form event handler

		/// <summary>
		/// Fired when the form loads.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance that contains the event data.</param>
		private void SplashScreenForm_Load(object sender, EventArgs e)
		{
			// Set the title label text to the product name
			labelTitle.Text = AssemblyInfo.AssemblyProduct;
			// Set the version label text to the assembly version
			labelVersion.Text = string.Format(format: I10nStrings.VersionTemplate, arg0: AssemblyInfo.AssemblyVersion);
		}

		/// <summary>
		/// Fired when the form closes.
		/// </summary>
		/// <param name="sender">The event source.</param>
		/// <param name="e">The <see cref="FormClosedEventArgs"/> instance that contains the event data.</param>
		private void SplashScreenForm_FormClosed(object sender, FormClosedEventArgs e) => Dispose();

		#endregion

		#region double-click event handler

		/// <summary>
		/// Called when a control is double-clicked to copy its text to the clipboard.
		/// If the sender is a <see cref="Control"/>, the control's <see cref="Control.Text"/> is copied.
		/// </summary>
		/// <param name="sender">Event source (expected to be a <see cref="Control"/>).</param>
		/// <param name="e">Event arguments.</param>
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