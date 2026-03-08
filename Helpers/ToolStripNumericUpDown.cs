// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.ComponentModel;
using System.Windows.Forms.Design;

namespace Planetoid_DB.Helpers;

/// <summary>Represents a numeric up-down control hosted in a ToolStrip.</summary>
/// <remarks>This class provides a convenient way to include a NumericUpDown control within a ToolStrip or StatusStrip, allowing for easy integration of numeric input functionality in these UI components.</remarks>
// You can use this class to add a numeric up-down control to a ToolStrip or StatusStrip, enabling users to input numeric values directly within these UI elements. The class exposes properties and events of the underlying NumericUpDown control, allowing for customization and interaction handling as needed.
[DesignerCategory(category: "code")]
[ToolStripItemDesignerAvailability(visibility: ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
public class ToolStripNumericUpDown : ToolStripControlHost
{
	/// <summary>Initializes a new instance of the ToolStripNumericUpDown class.</summary>
	/// <remarks>This constructor creates a new instance of the ToolStripNumericUpDown class and initializes the hosted NumericUpDown control.</remarks>
	public ToolStripNumericUpDown()
		: base(c: CreateControlInstance())
	{
	}

	/// <summary>Creates a new instance of the NumericUpDown control configured for numeric input within a specified range.</summary>
	/// <remarks>The returned control is suitable for scenarios where a fixed-size numeric selector is required. The
	/// Minimum and Maximum properties are preset to restrict input to values between 0 and 100. The AutoSize property is
	/// disabled to allow custom sizing.</remarks>
	/// <returns>A NumericUpDown control with AutoSize set to false, a size of 60 by 22 pixels, and a value range from 0 to 100.</returns>
	private static NumericUpDown CreateControlInstance()
	{
		// Create a new NumericUpDown control with specific properties for use in the ToolStrip.
		NumericUpDown n = new()
		{
			AutoSize = false,
			Size = new Size(width: 60, height: 22),
			Minimum = 0,
			Maximum = 100
		};
		return n;
	}

	/// <summary>Gets the hosted NumericUpDown control.</summary>
	/// <remarks>This property provides access to the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Browsable(browsable: false)]
	[DesignerSerializationVisibility(visibility: DesignerSerializationVisibility.Hidden)]
	public NumericUpDown NumericUpDownControl => (NumericUpDown)Control;

	/// <summary>Gets or sets the value assigned to the up-down control.</summary>
	/// <remarks>This property provides access to the value of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Data")]
	[DefaultValue(type: typeof(decimal), value: "0")]
	public decimal Value
	{
		get => NumericUpDownControl.Value;
		set => NumericUpDownControl.Value = value;
	}

	/// <summary>Gets or sets the minimum value for the up-down control.</summary>
	/// <remarks>This property provides access to the minimum value of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Data")]
	[DefaultValue(type: typeof(decimal), value: "0")]
	public decimal Minimum
	{
		get => NumericUpDownControl.Minimum;
		set => NumericUpDownControl.Minimum = value;
	}

	/// <summary>Gets or sets the maximum value for the up-down control.</summary>
	/// <remarks>This property provides access to the maximum value of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Data")]
	[DefaultValue(type: typeof(decimal), value: "100")]
	public decimal Maximum
	{
		get => NumericUpDownControl.Maximum;
		set => NumericUpDownControl.Maximum = value;
	}

	/// <summary>Gets or sets the number of decimal places to display in the up-down control.</summary>
	/// <remarks>This property provides access to the number of decimal places of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Data")]
	[DefaultValue(value: 0)]
	public int DecimalPlaces
	{
		get => NumericUpDownControl.DecimalPlaces;
		set => NumericUpDownControl.DecimalPlaces = value;
	}

	/// <summary>Gets or sets the value to increment or decrement the up-down control when the up or down buttons are clicked.</summary>
	/// <remarks>This property provides access to the increment value of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Data")]
	[DefaultValue(type: typeof(decimal), value: "1")]
	public decimal Increment
	{
		get => NumericUpDownControl.Increment;
		set => NumericUpDownControl.Increment = value;
	}

	/// <summary>Gets or sets a value indicating whether a thousands separator is displayed in the up-down control.</summary>
	/// <remarks>This property provides access to the thousands separator setting of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Data")]
	[DefaultValue(value: false)]
	public bool ThousandsSeparator
	{
		get => NumericUpDownControl.ThousandsSeparator;
		set => NumericUpDownControl.ThousandsSeparator = value;
	}

	/// <summary>Gets or sets a value indicating whether the user can use the UP ARROW and DOWN ARROW keys to select values.</summary>
	/// <remarks>This property provides access to the arrow key interception setting of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Behavior")]
	[DefaultValue(value: true)]
	public bool InterceptArrowKeys
	{
		get => NumericUpDownControl.InterceptArrowKeys;
		set => NumericUpDownControl.InterceptArrowKeys = value;
	}

	/// <summary>Gets or sets the tab index of the control.</summary>
	/// <remarks>This property provides access to the tab index of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Behavior")]
	[DesignerSerializationVisibility(visibility: DesignerSerializationVisibility.Hidden)]
	public int TabIndex
	{
		get => NumericUpDownControl.TabIndex;
		set => NumericUpDownControl.TabIndex = value;
	}

	/// <summary>Gets or sets a value indicating whether the user can give the focus to this control using the TAB key.</summary>
	/// <remarks>This property provides access to the tab stop setting of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Behavior")]
	[DefaultValue(value: true)]
	public bool TabStop
	{
		get => NumericUpDownControl.TabStop;
		set => NumericUpDownControl.TabStop = value;
	}

	/// <summary>Gets or sets a value indicating whether the control can accept data that the user drags onto it.</summary>
	/// <remarks>This property provides access to the allow drop setting of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Behavior")]
	[DefaultValue(value: false)]
	public new bool AllowDrop
	{
		get => NumericUpDownControl.AllowDrop;
		set => NumericUpDownControl.AllowDrop = value;
	}

	/// <summary>Gets or sets a value indicating whether the up-down control should display its value in hexadecimal format.</summary>
	/// <remarks>This property provides access to the hexadecimal display setting of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Appearance")]
	[DefaultValue(value: false)]
	public bool Hexadecimal
	{
		get => NumericUpDownControl.Hexadecimal;
		set => NumericUpDownControl.Hexadecimal = value;
	}

	/// <summary>Gets or sets how the up-down control will align the text.</summary>
	/// <remarks>This property provides access to the text alignment setting of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Appearance")]
	[DefaultValue(value: HorizontalAlignment.Left)]
	public new HorizontalAlignment TextAlign
	{
		get => NumericUpDownControl.TextAlign;
		set => NumericUpDownControl.TextAlign = value;
	}

	/// <summary>Gets or sets the alignment of the up-down buttons on the up-down control.</summary>
	/// <remarks>This property provides access to the up-down button alignment setting of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Appearance")]
	[DefaultValue(value: LeftRightAlignment.Right)]
	public LeftRightAlignment UpDownAlign
	{
		get => NumericUpDownControl.UpDownAlign;
		set => NumericUpDownControl.UpDownAlign = value;
	}

	/// <summary>Gets or sets the border style for the up-down control.</summary>
	/// <remarks>This property provides access to the border style setting of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Appearance")]
	[DefaultValue(value: BorderStyle.Fixed3D)]
	public BorderStyle BorderStyle
	{
		get => NumericUpDownControl.BorderStyle;
		set => NumericUpDownControl.BorderStyle = value;
	}

	/// <summary>Gets or sets a value indicating whether to use the wait cursor for the current control and all child controls.</summary>
	/// <remarks>This property provides access to the wait cursor setting of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Appearance")]
	[DefaultValue(value: false)]
	public bool UseWaitCursor
	{
		get => NumericUpDownControl.UseWaitCursor;
		set => NumericUpDownControl.UseWaitCursor = value;
	}

	/// <summary>Gets or sets the shortcut menu associated with the control.</summary>
	/// <remarks>This property provides access to the shortcut menu setting of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Behavior")]
	[DefaultValue(value: null)]
	public ContextMenuStrip? ContextMenuStrip
	{
		get => NumericUpDownControl.ContextMenuStrip;
		set => NumericUpDownControl.ContextMenuStrip = value;
	}

	/// <summary>Gets or sets a value indicating whether the text can be changed by the user.</summary>
	/// <remarks>This property provides access to the read-only setting of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Behavior")]
	[DefaultValue(value: false)]
	public bool ReadOnly
	{
		get => NumericUpDownControl.ReadOnly;
		set => NumericUpDownControl.ReadOnly = value;
	}

	/// <summary> Gets or sets a value indicating whether the control automatically sizes itself to fit its contents.</summary>
	/// <remarks>This property provides access to the auto-size setting of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Layout")]
	[DefaultValue(value: false)]
	public new bool AutoSize
	{
		get => NumericUpDownControl.AutoSize;
		set => NumericUpDownControl.AutoSize = value;
	}

	/// <summary>Gets the collection of accelerations for the up-down control.</summary>
	/// <remarks>This property provides access to the accelerations collection of the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	[Category(category: "Behavior")]
	[DesignerSerializationVisibility(visibility: DesignerSerializationVisibility.Content)]
	public NumericUpDownAccelerationCollection Accelerations => NumericUpDownControl.Accelerations;

	/// <summary>Subscribes to events from the hosted control.</summary>
	/// <param name="control">The hosted control.</param>
	/// <remarks>This method subscribes to events from the underlying NumericUpDown control hosted within the ToolStripNumericUpDown item.</remarks>
	protected override void OnSubscribeControlEvents(Control? control)
	{
		// Call the base method to ensure that any necessary event subscriptions are handled by the base class.
		base.OnSubscribeControlEvents(control);
		// Check if the control is a NumericUpDown and subscribe to its events.
		if (control is NumericUpDown numericUpDown)
		{
			numericUpDown.ValueChanged += OnValueChanged;
			numericUpDown.KeyDown += OnKeyDown;
			numericUpDown.KeyPress += OnKeyPress;
			numericUpDown.KeyUp += OnKeyUp;
			numericUpDown.Validating += OnValidating;
			numericUpDown.Validated += OnValidated;
			numericUpDown.TextChanged += OnTextChanged;
			numericUpDown.Click += OnClick;
			numericUpDown.DoubleClick += OnDoubleClick;
			numericUpDown.MouseClick += OnMouseClick;
			numericUpDown.MouseDoubleClick += OnMouseDoubleClick;
			numericUpDown.MouseDown += OnMouseDown;
			numericUpDown.MouseEnter += OnMouseEnter;
			numericUpDown.MouseLeave += OnMouseLeave;
			numericUpDown.MouseHover += OnMouseHover;
			numericUpDown.MouseMove += OnMouseMove;
			numericUpDown.MouseUp += OnMouseUp;
			numericUpDown.GotFocus += OnGotFocus;
			numericUpDown.LostFocus += OnLostFocus;
			numericUpDown.Enter += OnEnter;
			numericUpDown.Leave += OnLeave;
		}
	}

	/// <summary>
	/// Unsubscribes from events from the hosted control.
	/// </summary>
	/// <param name="control">The hosted control.</param>
	protected override void OnUnsubscribeControlEvents(Control? control)
	{
		// Call the base method to ensure that any necessary event unsubscriptions are handled by the base class.
		base.OnUnsubscribeControlEvents(control);
		// Check if the control is a NumericUpDown and unsubscribe from its events.
		if (control is NumericUpDown numericUpDown)
		{
			numericUpDown.ValueChanged -= OnValueChanged;
			numericUpDown.KeyDown -= OnKeyDown;
			numericUpDown.KeyPress -= OnKeyPress;
			numericUpDown.KeyUp -= OnKeyUp;
			numericUpDown.Validating -= OnValidating;
			numericUpDown.Validated -= OnValidated;
			numericUpDown.TextChanged -= OnTextChanged;
			numericUpDown.Click -= OnClick;
			numericUpDown.DoubleClick -= OnDoubleClick;
			numericUpDown.MouseClick -= OnMouseClick;
			numericUpDown.MouseDoubleClick -= OnMouseDoubleClick;
			numericUpDown.MouseDown -= OnMouseDown;
			numericUpDown.MouseEnter -= OnMouseEnter;
			numericUpDown.MouseLeave -= OnMouseLeave;
			numericUpDown.MouseHover -= OnMouseHover;
			numericUpDown.MouseMove -= OnMouseMove;
			numericUpDown.MouseUp -= OnMouseUp;
			numericUpDown.GotFocus -= OnGotFocus;
			numericUpDown.LostFocus -= OnLostFocus;
			numericUpDown.Enter -= OnEnter;
			numericUpDown.Leave -= OnLeave;
		}
	}

	/// <summary>Occurs when the value of the control changes.</summary>
	/// <remarks>This event is triggered whenever the associated value is modified. Subscribers to this event can
	/// handle the change and perform necessary actions in response.</remarks>
	public event EventHandler? ValueChanged;

	/// <summary>Occurs when a key is pressed while the control has focus.</summary>
	/// <remarks>This event can be used to handle keyboard input for the control. It is important to note that this
	/// event is raised for both key down and key up actions, and can be used to implement custom keyboard shortcuts or
	/// behaviors.</remarks>
	public new event KeyEventHandler? KeyDown;

	/// <summary>Occurs when a key is pressed while the control has focus.</summary>
	/// <remarks>This event allows you to handle key press actions for the control. It is important to note that the
	/// event is raised for each key press, and you can use the event arguments to determine which key was pressed and
	/// whether any special keys (like Shift or Ctrl) were held down.</remarks>
	public new event KeyPressEventHandler? KeyPress;

	/// <summary>Occurs when a key is released while the control has focus.</summary>
	/// <remarks>This event can be used to respond to key release actions, such as validating input or triggering
	/// commands based on the released key. It is important to note that this event is raised after the KeyDown
	/// event.</remarks>
	public new event KeyEventHandler? KeyUp;

	/// <summary>Occurs when the control is being validated.</summary>
	/// <remarks>This event allows you to perform validation before the control's value is accepted. You can use the
	/// event handler to cancel the validation process if the value is not valid.</remarks>
	public new event CancelEventHandler? Validating;

	/// <summary>Occurs when the control has been successfully validated.</summary>
	/// <remarks>This event is raised after the control's validation process completes. Handle this event to perform
	/// actions that depend on the control's validated state, such as updating related UI elements or triggering business
	/// logic. The event is only raised if validation is successful.</remarks>
	public new event EventHandler? Validated;

	/// <summary>Occurs when the text content of the control changes.</summary>
	/// <remarks>This event is triggered whenever the text is modified, allowing subscribers to respond to changes
	/// in the text content. It is important to note that this event may be raised multiple times during a single user
	/// action, such as typing or pasting text.</remarks>
	public new event EventHandler? TextChanged;

	/// <summary>Occurs when the control is clicked.</summary>
	/// <remarks>This event can be used to execute custom logic in response to user interactions with the control.
	/// It is important to note that this event is a new implementation that overrides the base class's Click
	/// event.</remarks>
	public new event EventHandler? Click;

	/// <summary>Occurs when the control is double-clicked.</summary>
	/// <remarks>This event can be used to trigger actions in response to a double-click interaction. It is
	/// important to note that this event is a new implementation that overrides the base class event.</remarks>
	public new event EventHandler? DoubleClick;

	/// <summary>Occurs when the mouse is clicked over the control.</summary>
	/// <remarks>This event can be used to handle mouse click actions, allowing developers to define custom behavior
	/// when a user interacts with the control. The event provides information about the mouse button pressed and the
	/// location of the click.</remarks>
	public event MouseEventHandler? MouseClick;

	/// <summary>Occurs when the mouse pointer is double-clicked over the control.</summary>
	/// <remarks>This event can be used to trigger actions in response to a double-click, such as opening a file or
	/// editing an item. Handlers for this event should be added using the `+=` operator and removed using the `-=`
	/// operator.</remarks>
	public event MouseEventHandler? MouseDoubleClick;

	/// <summary>Occurs when the mouse button is pressed while the mouse pointer is over the control.</summary>
	/// <remarks>This event can be used to handle mouse interactions, such as initiating drag-and-drop operations or
	/// responding to user clicks. It is important to note that this event is raised only when the mouse button is pressed
	/// down, and it does not indicate the completion of a mouse action.</remarks>
	public new event MouseEventHandler? MouseDown;

	/// <summary>Occurs when the mouse pointer enters the bounds of the control.</summary>
	/// <remarks>This event can be used to trigger visual changes or actions when the mouse hovers over the control.
	/// It is important to note that this event will not be raised if the control is not visible or enabled.</remarks>
	public new event EventHandler? MouseEnter;

	/// <summary>Occurs when the mouse pointer leaves the bounds of the control.</summary>
	/// <remarks>This event can be used to trigger actions when the mouse exits the control area, such as changing
	/// visual states or updating UI elements. It is important to note that this event will not be raised if the control is
	/// not visible or if it is disabled.</remarks>
	public new event EventHandler? MouseLeave;

	/// <summary>Occurs when the mouse pointer hovers over the control.</summary>
	/// <remarks>This event can be used to trigger visual feedback or other actions when the user hovers over the
	/// control with the mouse. It is important to note that this event may not be raised if the control is not visible or
	/// enabled.</remarks>
	public new event EventHandler? MouseHover;

	/// <summary>Occurs when the mouse pointer moves over the control.</summary>
	/// <remarks>This event can be used to track mouse movements and respond accordingly, such as updating the UI or
	/// triggering actions based on the mouse position.</remarks>
	public new event MouseEventHandler? MouseMove;

	/// <summary>
	/// Occurs when the mouse button is released over the control.</summary>
	/// <remarks>This event is typically used to handle mouse interactions, allowing developers to execute code in
	/// response to the mouse button being released. It can be combined with other mouse events to create complex user
	/// interactions.</remarks>
	public new event MouseEventHandler? MouseUp;

	/// <summary>Occurs when the control receives focus.</summary>
	/// <remarks>This event can be used to execute code when the control gains focus, such as updating the UI or
	/// validating input. It is important to note that this event is raised after the control has received focus.</remarks>
	public new event EventHandler? GotFocus;

	/// <summary>Occurs when the control loses focus.</summary>
	/// <remarks>This event is typically used to perform actions when the control is no longer active, such as
	/// validating input or saving state. It can be subscribed to by adding an event handler that matches the EventHandler
	/// delegate signature.</remarks>
	public new event EventHandler? LostFocus;

	/// <summary>Occurs when the control receives input focus.</summary>
	/// <remarks>This event hides the base class event and can be used to perform actions when the control is
	/// entered, such as updating UI elements or triggering validation. It is typically raised when the user navigates to
	/// the control using the keyboard or mouse.</remarks>
	public new event EventHandler? Enter;

	/// <summary>Occurs when the control loses focus.</summary>
	/// <remarks>This event can be used to perform actions when the control is no longer active, such as validating
	/// input or saving state. It is important to note that this event is raised after the control has lost focus, allowing
	/// for any necessary cleanup or updates.</remarks>
	public new event EventHandler? Leave;

	/// <summary>Raises the ValueChanged event to notify subscribers when the value has changed.</summary>
	/// <remarks>Call this method when the value changes to allow event handlers to respond accordingly. This is
	/// useful for updating UI elements or triggering additional logic in response to value changes.</remarks>
	/// <param name="sender">The source of the event, typically the object whose value has changed.</param>
	/// <param name="e">An EventArgs instance containing the event data associated with the value change.</param>
	private void OnValueChanged(object? sender, EventArgs e) => ValueChanged?.Invoke(sender: this, e: e);

	/// <summary>Raises the KeyDown event when a key is pressed while the control has focus.</summary>
	/// <remarks>This method invokes the KeyDown event handler, allowing subscribers to respond to key press
	/// events.</remarks>
	/// <param name="sender">The source of the event, typically the control that received the key press.</param>
	/// <param name="e">An instance of KeyEventArgs that contains the event data, including information about which key was pressed.</param>
	private void OnKeyDown(object? sender, KeyEventArgs e) => KeyDown?.Invoke(sender: this, e: e);

	/// <summary>Raises the KeyPress event, allowing subscribers to handle key press actions.</summary>
	/// <remarks>This method invokes the KeyPress event handler if there are any subscribers. It is typically used
	/// to respond to user input in text controls.</remarks>
	/// <param name="sender">The source of the event, typically the control that received the key press.</param>
	/// <param name="e">An instance of KeyPressEventArgs that contains the event data, including the pressed key character.</param>
	private void OnKeyPress(object? sender, KeyPressEventArgs e) => KeyPress?.Invoke(sender: this, e: e);

	/// <summary>Raises the KeyUp event, allowing subscribers to respond when a key is released while the control has focus.</summary>
	/// <remarks>This method is typically called when a key is released. Event handlers attached to the KeyUp event
	/// can use this to implement custom behavior based on key input.</remarks>
	/// <param name="sender">The source of the event, typically the control that received the key input.</param>
	/// <param name="e">An instance of KeyEventArgs that contains information about the released key.</param>
	private void OnKeyUp(object? sender, KeyEventArgs e) => KeyUp?.Invoke(sender: this, e: e);

	/// <summary>Raises the Validating event, allowing subscribers to handle validation logic before the control loses focus.</summary>
	/// <remarks>This method is typically called before the control loses focus. Event handlers attached to the Validating event
	/// can use this to implement custom validation logic and potentially cancel the focus change.</remarks>
	/// <param name="sender">The source of the event, typically the control that is being validated.</param>
	/// <param name="e">An instance of CancelEventArgs that contains the event data, including a flag to cancel the validation.</param>
	private void OnValidating(object? sender, CancelEventArgs e) => Validating?.Invoke(sender: this, e: e);

	/// <summary>Raises the Validated event to indicate that validation has completed for the associated control.</summary>
	/// <remarks>Call this method after validation logic has finished to notify subscribers that validation is
	/// complete. This allows event handlers to perform additional actions in response to validation.</remarks>
	/// <param name="sender">The source of the event, typically the control that has been validated.</param>
	/// <param name="e">An EventArgs instance containing the event data for the validation event.</param>
	private void OnValidated(object? sender, EventArgs e) => Validated?.Invoke(sender: this, e: e);

	/// <summary>Raises the TextChanged event to notify subscribers when the text content has changed.</summary>
	/// <remarks>This handler is attached to the hosted control's <see cref="System.Windows.Forms.Control.TextChanged"/> event.
	/// It delegates to the base <see cref="ToolStripItem.OnTextChanged(EventArgs)"/> method so that the standard
	/// ToolStripItem event pipeline is used.</remarks>
	/// <param name="sender">The source of the event, typically the hosted control whose text was modified.</param>
	/// <param name="e">An <see cref="EventArgs"/> instance containing the event data associated with the text change.</param>
	private void OnTextChanged(object? sender, EventArgs e) => base.OnTextChanged(e: e);

	/// <summary>Raises the Click event, passing the event data to any registered event handlers.</summary>
	/// <remarks>This handler is attached to the hosted control's <see cref="System.Windows.Forms.Control.Click"/> event.
	/// It calls <see cref="ToolStripItem.OnClick(EventArgs)"/> to raise the standard ToolStripItem Click event
	/// and to ensure that any overrides in derived classes are honored.</remarks>
	/// <param name="sender">The source of the event, typically the hosted control that raised the event.</param>
	/// <param name="e">An instance of <see cref="EventArgs"/> that contains the event data.</param>
	private void OnClick(object? sender, EventArgs e) => base.OnClick(e: e);

	/// <summary>Raises the DoubleClick event, allowing subscribers to respond to a double-click action on the control.</summary>
	/// <remarks>This handler is attached to the hosted control's <see cref="System.Windows.Forms.Control.DoubleClick"/> event.
	/// It delegates to <see cref="ToolStripItem.OnDoubleClick(EventArgs)"/> so that the built-in DoubleClick
	/// event pipeline of <see cref="ToolStripItem"/> is used.</remarks>
	/// <param name="sender">The source of the event, typically the hosted control that was double-clicked.</param>
	/// <param name="e">An <see cref="EventArgs"/> instance containing the event data associated with the double-click event.</param>
	private void OnDoubleClick(object? sender, EventArgs e) => base.OnDoubleClick(e: e);

	/// <summary>Raises the MouseClick event, providing the event data to any subscribed event handlers.</summary>
	/// <remarks>This method is typically called in response to mouse click actions on the control. Ensure that any
	/// event handlers are properly subscribed to handle the MouseClick event.</remarks>
	/// <param name="sender">The source of the event, typically the control that raised the event.</param>
	/// <param name="e">An instance of MouseEventArgs that contains the event data, including information about the mouse button pressed
	/// and the mouse cursor position.</param>
	private void OnMouseClick(object? sender, MouseEventArgs e) => MouseClick?.Invoke(sender: this, e: e);

	/// <summary>Raises the MouseDoubleClick event when the mouse is double-clicked over the control.</summary>
	/// <remarks>This method invokes the MouseDoubleClick event handler, allowing subscribers to respond to
	/// double-click actions.</remarks>
	/// <param name="sender">The source of the event, typically the control that raised the event.</param>
	/// <param name="e">An instance of MouseEventArgs that contains the event data, including the position of the mouse and the button that
	/// was pressed.</param>
	private void OnMouseDoubleClick(object? sender, MouseEventArgs e) => MouseDoubleClick?.Invoke(sender: this, e: e);

	/// <summary>Raises the MouseDown event when a mouse button is pressed over the control.</summary>
	/// <remarks>Use this method to provide custom handling for mouse down events by subscribing to the MouseDown
	/// event. Ensure event handlers are attached as needed to respond to user interactions.</remarks>
	/// <param name="sender">The source of the event, typically the control that received the mouse down action.</param>
	/// <param name="e">An instance of MouseEventArgs containing information about the mouse button pressed and the cursor position.</param>
	private void OnMouseDown(object? sender, MouseEventArgs e) => MouseDown?.Invoke(sender: this, e: e);

	/// <summary>Raises the MouseEnter event when the mouse pointer enters the bounds of the control.</summary>
	/// <remarks>Override this method to implement custom logic that executes when the mouse pointer enters the
	/// control area. This method allows derived classes to respond to mouse entry events without attaching a
	/// delegate.</remarks>
	/// <param name="sender">The source of the event, typically the control that the mouse pointer has entered.</param>
	/// <param name="e">The event data associated with the mouse enter event.</param>
	private void OnMouseEnter(object? sender, EventArgs e) => MouseEnter?.Invoke(sender: this, e: e);

	/// <summary>Handles the event when the mouse pointer leaves the bounds of the control.</summary>
	/// <remarks>This method can be used to trigger visual updates or other actions when the mouse pointer exits the
	/// control area. It is commonly used to revert changes made during mouse hover events.</remarks>
	/// <param name="sender">The source of the event, typically the control from which the mouse pointer has departed.</param>
	/// <param name="e">An object containing the event data associated with the mouse leave action.</param>
	private void OnMouseLeave(object? sender, EventArgs e) => MouseLeave?.Invoke(sender: this, e: e);

	/// <summary>Raises the MouseHover event when the mouse pointer hovers over the control.</summary>
	/// <remarks>This method invokes the MouseHover event handler, allowing subscribers to respond to mouse hover
	/// actions.</remarks>
	/// <param name="sender">The source of the event, typically the control that the mouse is hovering over.</param>
	/// <param name="e">An EventArgs that contains the event data for the MouseHover event.</param>
	private void OnMouseHover(object? sender, EventArgs e) => MouseHover?.Invoke(sender: this, e: e);

	/// <summary>Raises the MouseMove event to notify subscribers when the mouse pointer moves over the control.</summary>
	/// <remarks>Override this method in a derived class to provide custom handling for mouse movement events. The
	/// method invokes the MouseMove event if there are any subscribers.</remarks>
	/// <param name="sender">The source of the event, typically the control that received the mouse movement.</param>
	/// <param name="e">An instance of MouseEventArgs containing information about the mouse position and button states.</param>
	private void OnMouseMove(object? sender, MouseEventArgs e) => MouseMove?.Invoke(sender: this, e: e);

	/// <summary>Raises the MouseUp event when a mouse button is released over the control.</summary>
	/// <remarks>Use this method to handle mouse button release actions, such as completing a drag-and-drop
	/// operation or updating control state in response to user input.</remarks>
	/// <param name="sender">The source of the event, typically the control that received the mouse input.</param>
	/// <param name="e">An object containing information about the mouse event, including the mouse button released and the cursor
	/// position.</param>
	private void OnMouseUp(object? sender, MouseEventArgs e) => MouseUp?.Invoke(sender: this, e: e);

	/// <summary>Raises the GotFocus event to notify subscribers that the control has received focus.</summary>
	/// <remarks>Use this method to trigger any logic that should occur when the control gains focus. Subscribers to
	/// the GotFocus event can respond to the focus change as needed.</remarks>
	/// <param name="sender">The source of the event, typically the control that received focus.</param>
	/// <param name="e">An EventArgs instance containing the event data associated with the focus change.</param>
	private void OnGotFocus(object? sender, EventArgs e) => GotFocus?.Invoke(sender: this, e: e);

	/// <summary>Raises the LostFocus event when the control loses input focus.</summary>
	/// <remarks>Use this method to handle custom logic when the control loses focus by subscribing to the LostFocus
	/// event.</remarks>
	/// <param name="sender">The source of the event, typically the control that has lost focus.</param>
	/// <param name="e">An EventArgs instance containing the event data associated with the focus loss.</param>
	private void OnLostFocus(object? sender, EventArgs e) => LostFocus?.Invoke(sender: this, e: e);

	/// <summary>Invokes the Enter event, passing the current instance and event arguments to the event handlers.</summary>
	/// <remarks>This method is typically used to handle the Enter event for controls, allowing for custom behavior
	/// when the control gains focus.</remarks>
	/// <param name="sender">The source of the event, typically the control that raised the event.</param>
	/// <param name="e">The event data associated with the Enter event.</param>
	private void OnEnter(object? sender, EventArgs e) => Enter?.Invoke(sender: this, e: e);

	/// <summary>Raises the Leave event for the control, passing the specified event arguments to all registered handlers.</summary>
	/// <remarks>Call this method when the control loses focus to allow subscribers to perform cleanup or respond to
	/// the focus change.</remarks>
	/// <param name="sender">The source of the event, typically the control that is losing focus.</param>
	/// <param name="e">The event data associated with the Leave event.</param>
	private void OnLeave(object? sender, EventArgs e) => Leave?.Invoke(sender: this, e: e);
}