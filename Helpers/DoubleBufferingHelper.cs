// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using NLog;

using System.Reflection;

namespace Planetoid_DB.Helpers;

/// <summary>Provides helper methods to enable double buffering on controls via compiled delegates for maximum performance.</summary>
/// <remarks>Uses reflection once to create a delegate for the protected <c>SetStyle</c> method on <see cref="Control"/> instances. If enabling double buffering fails, a warning is logged but the application continues to function normally.</remarks>
internal static class DoubleBufferingHelper
{
	/// <summary>NLog logger for logging warnings when double buffering cannot be enabled.</summary>
	/// <remarks>This logger captures warnings that occur during the reflection-based double buffering setup.</remarks>
	private static readonly Logger logger = LogManager.GetCurrentClassLogger();

	/// <summary>Delegate type for the SetStyle method on Control. This delegate allows setting control styles via reflection for performance optimization.</summary>
	/// <param name="control">The control on which to set the style.</param>
	/// <param name="flag">The control style flag to set.</param>
	/// <param name="value">The value to assign to the control style flag.</param>
	/// <remarks>This delegate is used to invoke the protected SetStyle method on Control instances, enabling double buffering and other optimized painting styles.</remarks>
	private delegate void SetStyleDelegate(Control control, ControlStyles flag, bool value);

	/// <summary>A static delegate instance for the SetStyle method on Control. This delegate is initialized via reflection to allow setting control styles for double buffering and optimized painting.</summary>
	/// <remarks>This delegate is created once and reused for all controls, ensuring efficient application of double buffering and painting optimizations.</remarks>
	private static readonly SetStyleDelegate? SetStyle;

	/// <summary>Static constructor for the <see cref="DoubleBufferingHelper"/> class. Initializes the <see cref="SetStyle"/> delegate using reflection to access the protected <c>SetStyle</c> method on <see cref="Control"/> instances.</summary>
	/// <remarks>This constructor is called automatically before any static members are accessed, ensuring that the delegate is ready for use when enabling double buffering on controls.</remarks>
	static DoubleBufferingHelper()
	{
		// Initialize the SetStyle delegate using reflection to access the protected SetStyle method on Control.
		try
		{
			// Use reflection to get the MethodInfo for the protected SetStyle method on Control.
			MethodInfo? methodInfo = typeof(Control).GetMethod(name: "SetStyle", bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance);
			// If the method is found, create a delegate for it to allow high-performance invocation.
			if (methodInfo != null)
			{
				SetStyle = (SetStyleDelegate)Delegate.CreateDelegate(type: typeof(SetStyleDelegate), method: methodInfo);
			}
		}
		// Log an error if the delegate creation fails, indicating that double buffering cannot be applied.
		catch (Exception ex)
		{
			logger.Error(exception: ex, message: "Failed to create high-performance delegate for Control.SetStyle.");
		}
	}

	/// <summary>Enables double buffering and optimized painting styles on the specified control to reduce flickering.</summary>
	/// <param name="control">The control on which to enable double buffering.</param>
	/// <param name="includeChildLabels">If true, also enables double buffering on all child Label and KryptonLabel controls.</param>
	/// <remarks>This method uses reflection to access the protected SetStyle method on Control instances, allowing for high-performance application of double buffering and painting optimizations. If the SetStyle delegate is not initialized, a warning is logged and the method exits without applying styles.</remarks>
	internal static void EnableDoubleBuffering(Control control, bool includeChildLabels = false)
	{
		// Validate that the control argument is not null, throwing an ArgumentNullException if it is.
		ArgumentNullException.ThrowIfNull(argument: control);
		// Check if the SetStyle delegate is initialized. If not, log a warning and exit the method.
		if (SetStyle == null)
		{
			logger.Warn(message: "SetStyle delegate is not initialized. Double buffering cannot be applied.");
			return;
		}
		// Apply double buffering styles to the control, and optionally to its child labels, while handling any exceptions that may occur during the process.
		try
		{
			// Apply double buffering styles to the control based on the includeChildLabels parameter.
			if (includeChildLabels)
			{
				// Apply a comprehensive set of styles to the control to enable double buffering, optimized painting, and user-defined painting.
				ApplyDoubleBufferingStyles(control: control, styles: ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw);
				// Recursively enable double buffering for all child Label and KryptonLabel controls.
				EnableDoubleBufferingForChildLabels(parent: control);
			}
			// If includeChildLabels is false, apply only the essential double buffering styles to the control.
			else
			{
				ApplyDoubleBufferingStyles(control: control, styles: ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint);
			}
		}
		// Log a warning if an exception occurs while enabling double buffering, including the control's name and type for context.
		catch (Exception ex)
		{
			logger.Warn(exception: ex, message: "Could not enable double buffering on {ControlName} (Type: {ControlType}).", control.Name, control.GetType().Name);
		}
	}

	/// <summary>Recursively enables double buffering for all child Label and KryptonLabel controls within the specified parent control.</summary>
	/// <param name="parent">The parent control whose child labels will have double buffering enabled.</param>
	/// <remarks>This method traverses the control hierarchy, applying double buffering styles to all Label and KryptonLabel controls found within the parent control's children.</remarks>
	private static void EnableDoubleBufferingForChildLabels(Control parent)
	{
		// Iterate through each child control of the parent control.
		foreach (Control child in parent.Controls)
		{
			// Check if the child control is a Label or KryptonLabel. If so, apply double buffering styles to it.
			if (child is Label or KryptonLabel)
			{
				// Apply double buffering styles to the child label control to reduce flickering during repainting.
				ApplyDoubleBufferingStyles(control: child, styles: ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint);
			}
			// If the child control has its own children, recursively enable double buffering for those child labels as well.
			if (child.HasChildren)
			{
				EnableDoubleBufferingForChildLabels(parent: child);
			}
		}
	}

	/// <summary>Applies the specified double buffering styles to the given control using the SetStyle delegate.</summary>
	/// <param name="control">The control to which the double buffering styles will be applied.</param>
	/// <param name="styles">The double buffering styles to apply to the control.</param>
	/// <remarks>This method uses the SetStyle delegate to apply the specified double buffering styles to the control, enhancing rendering performance and reducing flicker.</remarks>
	private static void ApplyDoubleBufferingStyles(Control control, ControlStyles styles) =>
		// Apply the specified double buffering styles to the control using the SetStyle delegate.
		SetStyle?.Invoke(control: control, flag: styles, value: true);
}
