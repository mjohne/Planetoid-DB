using Krypton.Toolkit;

using NLog;

using System.Reflection;

namespace Planetoid_DB.Helpers;

/// <summary>Provides helper methods to enable double buffering on controls via reflection, reducing UI flickering.</summary>
/// <remarks>Uses reflection to access the protected <c>DoubleBuffered</c> property and <c>SetStyle</c> method on <see cref="Control"/> instances. If enabling double buffering fails, a warning is logged but the application continues to function normally.</remarks>
internal static class DoubleBufferingHelper
{
	/// <summary>NLog logger for logging warnings when double buffering cannot be enabled.</summary>
	/// <remarks>This logger captures warnings that occur during the reflection-based double buffering setup.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Cached reference to the protected <c>DoubleBuffered</c> property of <see cref="Control"/>.</summary>
	/// <remarks>Cached once at class load time to avoid repeated reflection lookups.</remarks>
	private static readonly PropertyInfo? DoubleBufferedProperty = typeof(Control).GetProperty(name: "DoubleBuffered", bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance);

	/// <summary>Cached reference to the protected <c>SetStyle</c> method of <see cref="Control"/>.</summary>
	/// <remarks>Cached once at class load time to avoid repeated reflection lookups.</remarks>
	private static readonly MethodInfo? SetStyleMethod = typeof(Control).GetMethod(name: "SetStyle", bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance);

	/// <summary>Enables double buffering and optimized painting styles on the specified control to reduce flickering.</summary>
	/// <param name="control">The control on which to enable double buffering.</param>
	/// <param name="includeChildLabels">When <see langword="true"/>, also applies double buffering to all child <see cref="Label"/> and <see cref="KryptonLabel"/> controls within the specified control.</param>
	/// <remarks>Sets the <c>DoubleBuffered</c> property to <see langword="true"/> and applies <see cref="ControlStyles.OptimizedDoubleBuffer"/> and <see cref="ControlStyles.AllPaintingInWmPaint"/> control styles. When <paramref name="includeChildLabels"/> is <see langword="true"/>, also applies <see cref="ControlStyles.UserPaint"/> and <see cref="ControlStyles.ResizeRedraw"/> to the main control, and double buffering to all child label controls.</remarks>
	internal static void EnableDoubleBuffering(Control control, bool includeChildLabels = false)
	{
		try
		{
			DoubleBufferedProperty?.SetValue(obj: control, value: true, index: null);
			if (includeChildLabels)
			{
				SetStyleMethod?.Invoke(obj: control, parameters: [
					ControlStyles.OptimizedDoubleBuffer |
					ControlStyles.AllPaintingInWmPaint |
					ControlStyles.UserPaint |
					ControlStyles.ResizeRedraw,
					true
				]);
				foreach (Control child in control.Controls)
				{
					if (child is Label or KryptonLabel)
					{
						DoubleBufferedProperty?.SetValue(obj: child, value: true, index: null);
						SetStyleMethod?.Invoke(obj: child, parameters: [
							ControlStyles.OptimizedDoubleBuffer |
							ControlStyles.AllPaintingInWmPaint,
							true
						]);
					}
				}
			}
			else
			{
				SetStyleMethod?.Invoke(obj: control, parameters: [ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true]);
			}
		}
		catch (Exception ex)
		{
			logger.Warn(exception: ex, message: "Could not enable double buffering on {ControlName}. UI may experience flickering.", args: control.Name);
		}
	}
}
