using Krypton.Toolkit;

using System.Diagnostics;

namespace Planetoid_DB
{
	/// <summary>
	/// Base form providing common behaviours for application forms.
	/// Currently: enables <c>KeyPreview</c> and closes the form when the Escape key is pressed.
	/// </summary>
	[DebuggerDisplay(value: "{" + nameof(GetDebuggerDisplay) + "(),nq}")]
	public class BaseKryptonForm : KryptonForm
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseKryptonForm"/> class.
		/// </summary>
		protected BaseKryptonForm()
		{
			// Ensure the form receives key events before child controls
			KeyPreview = true;
			KeyDown += BaseKryptonForm_KeyDown;
		}

		/// <summary>
		/// Returns a short debugger display string for this instance.
		/// </summary>
		/// <returns>A string representation for the debugger.</returns>
		private string GetDebuggerDisplay() => ToString();

		/// <summary>
		/// Default KeyDown handler that closes the form when Escape is pressed.
		/// </summary>
		/// <param name="sender">Event source.</param>
		/// <param name="e">Key event args.</param>
		private void BaseKryptonForm_KeyDown(object? sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape && this.InvokeRequired == false)
			{
				Close();
			}
		}
	}
}