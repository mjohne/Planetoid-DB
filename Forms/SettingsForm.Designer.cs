// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using Krypton.Toolkit;

using Planetoid_DB.Resources;

using System.ComponentModel;

namespace Planetoid_DB;

	/// <summary>Represents a settings dialog that enables users to configure application settings across four categories: General, Navigator, Database Update, and Appearance.</summary>
	/// <remarks>The form provides a tabbed interface to organize settings. Tabs include General (window behavior and interaction), Navigator (start position and step size), Database Update (startup/background update options and download parameters), and Appearance (theme and icon set). Save and cancel actions are provided via a toolbar. Logic for loading from and persisting to configuration storage is intentionally stubbed with TODO comments and will be implemented in a future iteration.</remarks>
    partial class SettingsForm
    {
        /// <summary>Required designer variable.</summary>
		/// <remarks>This field stores the components used by the form.</remarks>
        private IContainer components = null;

        /// <summary>Clean up any resources being used.</summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		/// <remarks>This method disposes of the resources used by the form.</remarks>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

	#region Windows Form Designer generated code

	/// <summary>Required method for Designer support - do not modify the contents of this method with the code editor.</summary>
	/// <remarks>This method initializes the components of the form.</remarks>
	private void InitializeComponent()
	{
		components = new Container();
		ComponentResourceManager resources = new ComponentResourceManager(typeof(SettingsForm));

		// --- Tab control and pages ---
		tabControlSettings = new TabControl();
		tabPageGeneral = new TabPage();
		tabPageNavigator = new TabPage();
		tabPageDatabaseUpdate = new TabPage();
		tabPageAppearance = new TabPage();

		// --- General tab controls ---
		groupBoxApplicationBehavior = new KryptonGroupBox();
		checkBoxStayAlwaysOnTop = new KryptonCheckBox();
		checkBoxShowSplashScreen = new KryptonCheckBox();
		checkBoxConfirmBeforeExit = new KryptonCheckBox();
		groupBoxInteraction = new KryptonGroupBox();
		checkBoxCopyToClipboardOnDoubleClick = new KryptonCheckBox();
		checkBoxLinkToTerminology = new KryptonCheckBox();

		// --- Navigator tab controls ---
		groupBoxStartingPosition = new KryptonGroupBox();
		radioButtonStartFirst = new KryptonRadioButton();
		radioButtonStartLast = new KryptonRadioButton();
		radioButtonStartLastUsed = new KryptonRadioButton();
		radioButtonStartRandom = new KryptonRadioButton();
		radioButtonStartSpecific = new KryptonRadioButton();
		labelStartSpecificItem = new KryptonLabel();
		numericUpDownStartSpecificItem = new KryptonNumericUpDown();
		groupBoxNavigationStep = new KryptonGroupBox();
		labelStepSize = new KryptonLabel();
		numericUpDownStepSize = new KryptonNumericUpDown();

		// --- Database Update tab controls ---
		groupBoxStartupUpdate = new KryptonGroupBox();
		checkBoxCheckUpdateOnStartup = new KryptonCheckBox();
		checkBoxAutoDownloadOnStartup = new KryptonCheckBox();
		checkBoxAskForRestartOnStartup = new KryptonCheckBox();
		groupBoxBackgroundUpdate = new KryptonGroupBox();
		checkBoxCheckUpdateInBackground = new KryptonCheckBox();
		checkBoxAutoDownloadInBackground = new KryptonCheckBox();
		checkBoxAskForRestartInBackground = new KryptonCheckBox();
		groupBoxDownloadOptions = new KryptonGroupBox();
		labelDownloadTimeout = new KryptonLabel();
		numericUpDownDownloadTimeout = new KryptonNumericUpDown();
		labelMaxRetries = new KryptonLabel();
		numericUpDownMaxRetries = new KryptonNumericUpDown();

		// --- Appearance tab controls ---
		groupBoxTheme = new KryptonGroupBox();
		labelTheme = new KryptonLabel();
		comboBoxTheme = new ComboBox();
		groupBoxIconSet = new KryptonGroupBox();
		radioButtonIconsFatcow = new KryptonRadioButton();
		radioButtonIconsSilk = new KryptonRadioButton();
		radioButtonIconsFugue = new KryptonRadioButton();

		// --- Shared layout controls ---
		kryptonToolStripIcons = new KryptonToolStrip();
		toolStripButtonSave = new ToolStripButton();
		toolStripButtonCancel = new ToolStripButton();
		toolStripSeparator1 = new ToolStripSeparator();
		toolStripButtonLoadDefaultSettings = new ToolStripButton();
		toolStripContainerSettings = new ToolStripContainer();
		kryptonPanelMain = new KryptonPanel();
		kryptonStatusStrip = new KryptonStatusStrip();
		labelInformation = new ToolStripStatusLabel();
		kryptonManager = new KryptonManager(components);

		// --- SuspendLayout / BeginInit ---
		tabControlSettings.SuspendLayout();
		tabPageGeneral.SuspendLayout();
		tabPageNavigator.SuspendLayout();
		tabPageDatabaseUpdate.SuspendLayout();
		tabPageAppearance.SuspendLayout();

		((ISupportInitialize)groupBoxApplicationBehavior).BeginInit();
		((ISupportInitialize)groupBoxApplicationBehavior.Panel).BeginInit();
		groupBoxApplicationBehavior.SuspendLayout();
		groupBoxApplicationBehavior.Panel.SuspendLayout();

		((ISupportInitialize)groupBoxInteraction).BeginInit();
		((ISupportInitialize)groupBoxInteraction.Panel).BeginInit();
		groupBoxInteraction.SuspendLayout();
		groupBoxInteraction.Panel.SuspendLayout();

		((ISupportInitialize)groupBoxStartingPosition).BeginInit();
		((ISupportInitialize)groupBoxStartingPosition.Panel).BeginInit();
		groupBoxStartingPosition.SuspendLayout();
		groupBoxStartingPosition.Panel.SuspendLayout();

		((ISupportInitialize)numericUpDownStartSpecificItem).BeginInit();

		((ISupportInitialize)groupBoxNavigationStep).BeginInit();
		((ISupportInitialize)groupBoxNavigationStep.Panel).BeginInit();
		groupBoxNavigationStep.SuspendLayout();
		groupBoxNavigationStep.Panel.SuspendLayout();

		((ISupportInitialize)numericUpDownStepSize).BeginInit();

		((ISupportInitialize)groupBoxStartupUpdate).BeginInit();
		((ISupportInitialize)groupBoxStartupUpdate.Panel).BeginInit();
		groupBoxStartupUpdate.SuspendLayout();
		groupBoxStartupUpdate.Panel.SuspendLayout();

		((ISupportInitialize)groupBoxBackgroundUpdate).BeginInit();
		((ISupportInitialize)groupBoxBackgroundUpdate.Panel).BeginInit();
		groupBoxBackgroundUpdate.SuspendLayout();
		groupBoxBackgroundUpdate.Panel.SuspendLayout();

		((ISupportInitialize)groupBoxDownloadOptions).BeginInit();
		((ISupportInitialize)groupBoxDownloadOptions.Panel).BeginInit();
		groupBoxDownloadOptions.SuspendLayout();
		groupBoxDownloadOptions.Panel.SuspendLayout();

		((ISupportInitialize)numericUpDownDownloadTimeout).BeginInit();
		((ISupportInitialize)numericUpDownMaxRetries).BeginInit();

		((ISupportInitialize)groupBoxTheme).BeginInit();
		((ISupportInitialize)groupBoxTheme.Panel).BeginInit();
		groupBoxTheme.SuspendLayout();
		groupBoxTheme.Panel.SuspendLayout();

		((ISupportInitialize)groupBoxIconSet).BeginInit();
		((ISupportInitialize)groupBoxIconSet.Panel).BeginInit();
		groupBoxIconSet.SuspendLayout();
		groupBoxIconSet.Panel.SuspendLayout();

		kryptonToolStripIcons.SuspendLayout();
		toolStripContainerSettings.ContentPanel.SuspendLayout();
		toolStripContainerSettings.TopToolStripPanel.SuspendLayout();
		toolStripContainerSettings.SuspendLayout();
		((ISupportInitialize)kryptonPanelMain).BeginInit();
		kryptonPanelMain.SuspendLayout();
		kryptonStatusStrip.SuspendLayout();
		SuspendLayout();

		// ===========================
		// GENERAL TAB
		// ===========================

		// checkBoxStayAlwaysOnTop
		checkBoxStayAlwaysOnTop.AccessibleDescription = "Keep the application window always on top of other windows";
		checkBoxStayAlwaysOnTop.AccessibleName = "Stay always on top";
		checkBoxStayAlwaysOnTop.AccessibleRole = AccessibleRole.CheckButton;
		checkBoxStayAlwaysOnTop.Location = new Point(8, 5);
		checkBoxStayAlwaysOnTop.Name = "checkBoxStayAlwaysOnTop";
		checkBoxStayAlwaysOnTop.Size = new Size(230, 23);
		checkBoxStayAlwaysOnTop.TabIndex = 0;
		checkBoxStayAlwaysOnTop.ToolTipValues.EnableToolTips = true;
		checkBoxStayAlwaysOnTop.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		checkBoxStayAlwaysOnTop.ToolTipValues.Description = "When checked, the application window will always remain visible above all other windows.";
		checkBoxStayAlwaysOnTop.Values.Text = "Stay always on top";
		checkBoxStayAlwaysOnTop.Enter += Control_Enter;
		checkBoxStayAlwaysOnTop.Leave += Control_Leave;

		// checkBoxShowSplashScreen
		checkBoxShowSplashScreen.AccessibleDescription = "Show the splash screen when the application starts";
		checkBoxShowSplashScreen.AccessibleName = "Show splash screen on startup";
		checkBoxShowSplashScreen.AccessibleRole = AccessibleRole.CheckButton;
		checkBoxShowSplashScreen.Location = new Point(8, 32);
		checkBoxShowSplashScreen.Name = "checkBoxShowSplashScreen";
		checkBoxShowSplashScreen.Size = new Size(255, 23);
		checkBoxShowSplashScreen.TabIndex = 1;
		checkBoxShowSplashScreen.ToolTipValues.EnableToolTips = true;
		checkBoxShowSplashScreen.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		checkBoxShowSplashScreen.ToolTipValues.Description = "When checked, the splash screen is displayed each time the application starts.";
		checkBoxShowSplashScreen.Values.Text = "Show splash screen on startup";
		checkBoxShowSplashScreen.Enter += Control_Enter;
		checkBoxShowSplashScreen.Leave += Control_Leave;

		// checkBoxConfirmBeforeExit
		checkBoxConfirmBeforeExit.AccessibleDescription = "Ask for confirmation before closing the application";
		checkBoxConfirmBeforeExit.AccessibleName = "Ask for confirmation before exiting";
		checkBoxConfirmBeforeExit.AccessibleRole = AccessibleRole.CheckButton;
		checkBoxConfirmBeforeExit.Location = new Point(8, 59);
		checkBoxConfirmBeforeExit.Name = "checkBoxConfirmBeforeExit";
		checkBoxConfirmBeforeExit.Size = new Size(280, 23);
		checkBoxConfirmBeforeExit.TabIndex = 2;
		checkBoxConfirmBeforeExit.ToolTipValues.EnableToolTips = true;
		checkBoxConfirmBeforeExit.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		checkBoxConfirmBeforeExit.ToolTipValues.Description = "When checked, a confirmation dialog is shown before the application closes.";
		checkBoxConfirmBeforeExit.Values.Text = "Ask for confirmation before exiting";
		checkBoxConfirmBeforeExit.Enter += Control_Enter;
		checkBoxConfirmBeforeExit.Leave += Control_Leave;

		// groupBoxApplicationBehavior
		groupBoxApplicationBehavior.AccessibleDescription = "Settings for application window behavior";
		groupBoxApplicationBehavior.AccessibleName = "Application behavior";
		groupBoxApplicationBehavior.AccessibleRole = AccessibleRole.Grouping;
		groupBoxApplicationBehavior.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		groupBoxApplicationBehavior.Location = new Point(5, 4);
		groupBoxApplicationBehavior.Name = "groupBoxApplicationBehavior";
		groupBoxApplicationBehavior.Size = new Size(600, 120);
		groupBoxApplicationBehavior.TabIndex = 0;
		groupBoxApplicationBehavior.Values.Heading = "Application behavior";
		groupBoxApplicationBehavior.Panel.Controls.Add(checkBoxStayAlwaysOnTop);
		groupBoxApplicationBehavior.Panel.Controls.Add(checkBoxShowSplashScreen);
		groupBoxApplicationBehavior.Panel.Controls.Add(checkBoxConfirmBeforeExit);

		// checkBoxCopyToClipboardOnDoubleClick
		checkBoxCopyToClipboardOnDoubleClick.AccessibleDescription = "Enable copying field values to the clipboard by double-clicking";
		checkBoxCopyToClipboardOnDoubleClick.AccessibleName = "Enable copying to clipboard by double-clicking";
		checkBoxCopyToClipboardOnDoubleClick.AccessibleRole = AccessibleRole.CheckButton;
		checkBoxCopyToClipboardOnDoubleClick.Location = new Point(8, 5);
		checkBoxCopyToClipboardOnDoubleClick.Name = "checkBoxCopyToClipboardOnDoubleClick";
		checkBoxCopyToClipboardOnDoubleClick.Size = new Size(380, 23);
		checkBoxCopyToClipboardOnDoubleClick.TabIndex = 0;
		checkBoxCopyToClipboardOnDoubleClick.ToolTipValues.EnableToolTips = true;
		checkBoxCopyToClipboardOnDoubleClick.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		checkBoxCopyToClipboardOnDoubleClick.ToolTipValues.Description = "When checked, double-clicking a data field copies its value to the clipboard.";
		checkBoxCopyToClipboardOnDoubleClick.Values.Text = "Enable copying to clipboard by double-clicking";
		checkBoxCopyToClipboardOnDoubleClick.Enter += Control_Enter;
		checkBoxCopyToClipboardOnDoubleClick.Leave += Control_Leave;

		// checkBoxLinkToTerminology
		checkBoxLinkToTerminology.AccessibleDescription = "Enable clickable links from data fields to terminology definitions";
		checkBoxLinkToTerminology.AccessibleName = "Enable linking to terminology";
		checkBoxLinkToTerminology.AccessibleRole = AccessibleRole.CheckButton;
		checkBoxLinkToTerminology.Location = new Point(8, 32);
		checkBoxLinkToTerminology.Name = "checkBoxLinkToTerminology";
		checkBoxLinkToTerminology.Size = new Size(280, 23);
		checkBoxLinkToTerminology.TabIndex = 1;
		checkBoxLinkToTerminology.ToolTipValues.EnableToolTips = true;
		checkBoxLinkToTerminology.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		checkBoxLinkToTerminology.ToolTipValues.Description = "When checked, data field labels act as links that open the corresponding terminology definition.";
		checkBoxLinkToTerminology.Values.Text = "Enable linking to terminology";
		checkBoxLinkToTerminology.Enter += Control_Enter;
		checkBoxLinkToTerminology.Leave += Control_Leave;

		// groupBoxInteraction
		groupBoxInteraction.AccessibleDescription = "Settings for user interaction with data fields";
		groupBoxInteraction.AccessibleName = "Interaction";
		groupBoxInteraction.AccessibleRole = AccessibleRole.Grouping;
		groupBoxInteraction.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		groupBoxInteraction.Location = new Point(5, 130);
		groupBoxInteraction.Name = "groupBoxInteraction";
		groupBoxInteraction.Size = new Size(600, 90);
		groupBoxInteraction.TabIndex = 1;
		groupBoxInteraction.Values.Heading = "Interaction";
		groupBoxInteraction.Panel.Controls.Add(checkBoxCopyToClipboardOnDoubleClick);
		groupBoxInteraction.Panel.Controls.Add(checkBoxLinkToTerminology);

		// tabPageGeneral
		tabPageGeneral.Controls.Add(groupBoxApplicationBehavior);
		tabPageGeneral.Controls.Add(groupBoxInteraction);
		tabPageGeneral.Location = new Point(4, 24);
		tabPageGeneral.Name = "tabPageGeneral";
		tabPageGeneral.Padding = new Padding(3);
		tabPageGeneral.Size = new Size(612, 375);
		tabPageGeneral.TabIndex = 0;
		tabPageGeneral.Text = "General";
		tabPageGeneral.UseVisualStyleBackColor = true;

		// ===========================
		// NAVIGATOR TAB
		// ===========================

		// radioButtonStartFirst
		radioButtonStartFirst.AccessibleDescription = "Start navigation at the first item in the database";
		radioButtonStartFirst.AccessibleName = "Start with the first item";
		radioButtonStartFirst.AccessibleRole = AccessibleRole.RadioButton;
		radioButtonStartFirst.Checked = true;
		radioButtonStartFirst.Location = new Point(8, 5);
		radioButtonStartFirst.Name = "radioButtonStartFirst";
		radioButtonStartFirst.Size = new Size(220, 23);
		radioButtonStartFirst.TabIndex = 0;
		radioButtonStartFirst.ToolTipValues.EnableToolTips = true;
		radioButtonStartFirst.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		radioButtonStartFirst.ToolTipValues.Description = "The navigator will open the first record in the database on startup.";
		radioButtonStartFirst.Values.Text = "Start with the first item";
		radioButtonStartFirst.Enter += Control_Enter;
		radioButtonStartFirst.Leave += Control_Leave;

		// radioButtonStartLast
		radioButtonStartLast.AccessibleDescription = "Start navigation at the last item in the database";
		radioButtonStartLast.AccessibleName = "Start with the last item";
		radioButtonStartLast.AccessibleRole = AccessibleRole.RadioButton;
		radioButtonStartLast.Location = new Point(8, 32);
		radioButtonStartLast.Name = "radioButtonStartLast";
		radioButtonStartLast.Size = new Size(210, 23);
		radioButtonStartLast.TabIndex = 1;
		radioButtonStartLast.ToolTipValues.EnableToolTips = true;
		radioButtonStartLast.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		radioButtonStartLast.ToolTipValues.Description = "The navigator will open the last record in the database on startup.";
		radioButtonStartLast.Values.Text = "Start with the last item";
		radioButtonStartLast.Enter += Control_Enter;
		radioButtonStartLast.Leave += Control_Leave;

		// radioButtonStartLastUsed
		radioButtonStartLastUsed.AccessibleDescription = "Resume at the last item that was open when the application was closed";
		radioButtonStartLastUsed.AccessibleName = "Start with the last used item";
		radioButtonStartLastUsed.AccessibleRole = AccessibleRole.RadioButton;
		radioButtonStartLastUsed.Location = new Point(8, 59);
		radioButtonStartLastUsed.Name = "radioButtonStartLastUsed";
		radioButtonStartLastUsed.Size = new Size(240, 23);
		radioButtonStartLastUsed.TabIndex = 2;
		radioButtonStartLastUsed.ToolTipValues.EnableToolTips = true;
		radioButtonStartLastUsed.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		radioButtonStartLastUsed.ToolTipValues.Description = "The navigator will resume at the record that was last viewed when the application was closed.";
		radioButtonStartLastUsed.Values.Text = "Start with the last used item";
		radioButtonStartLastUsed.Enter += Control_Enter;
		radioButtonStartLastUsed.Leave += Control_Leave;

		// radioButtonStartRandom
		radioButtonStartRandom.AccessibleDescription = "Start navigation at a randomly selected item";
		radioButtonStartRandom.AccessibleName = "Start with a random item";
		radioButtonStartRandom.AccessibleRole = AccessibleRole.RadioButton;
		radioButtonStartRandom.Location = new Point(8, 86);
		radioButtonStartRandom.Name = "radioButtonStartRandom";
		radioButtonStartRandom.Size = new Size(220, 23);
		radioButtonStartRandom.TabIndex = 3;
		radioButtonStartRandom.ToolTipValues.EnableToolTips = true;
		radioButtonStartRandom.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		radioButtonStartRandom.ToolTipValues.Description = "The navigator will open a randomly selected record on startup.";
		radioButtonStartRandom.Values.Text = "Start with a random item";
		radioButtonStartRandom.Enter += Control_Enter;
		radioButtonStartRandom.Leave += Control_Leave;

		// radioButtonStartSpecific
		radioButtonStartSpecific.AccessibleDescription = "Start navigation at a specific item index";
		radioButtonStartSpecific.AccessibleName = "Start with a specific item";
		radioButtonStartSpecific.AccessibleRole = AccessibleRole.RadioButton;
		radioButtonStartSpecific.Location = new Point(8, 113);
		radioButtonStartSpecific.Name = "radioButtonStartSpecific";
		radioButtonStartSpecific.Size = new Size(220, 23);
		radioButtonStartSpecific.TabIndex = 4;
		radioButtonStartSpecific.ToolTipValues.EnableToolTips = true;
		radioButtonStartSpecific.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		radioButtonStartSpecific.ToolTipValues.Description = "The navigator will open the record at the specified index on startup.";
		radioButtonStartSpecific.Values.Text = "Start with a specific item";
		radioButtonStartSpecific.CheckedChanged += RadioButtonStartSpecific_CheckedChanged;
		radioButtonStartSpecific.Enter += Control_Enter;
		radioButtonStartSpecific.Leave += Control_Leave;

		// labelStartSpecificItem
		labelStartSpecificItem.AccessibleDescription = "Item index for specific start position";
		labelStartSpecificItem.AccessibleName = "Item index";
		labelStartSpecificItem.AccessibleRole = AccessibleRole.StaticText;
		labelStartSpecificItem.Location = new Point(28, 141);
		labelStartSpecificItem.Name = "labelStartSpecificItem";
		labelStartSpecificItem.Size = new Size(80, 20);
		labelStartSpecificItem.TabIndex = 5;
		labelStartSpecificItem.Values.Text = "Item index:";

		// numericUpDownStartSpecificItem
		numericUpDownStartSpecificItem.AccessibleDescription = "Item index to start navigation at when 'Start with a specific item' is selected";
		numericUpDownStartSpecificItem.AccessibleName = "Start item index";
		numericUpDownStartSpecificItem.AccessibleRole = AccessibleRole.SpinButton;
		numericUpDownStartSpecificItem.Enabled = false;
		numericUpDownStartSpecificItem.Location = new Point(115, 138);
		numericUpDownStartSpecificItem.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
		numericUpDownStartSpecificItem.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownStartSpecificItem.Name = "numericUpDownStartSpecificItem";
		numericUpDownStartSpecificItem.Size = new Size(120, 22);
		numericUpDownStartSpecificItem.TabIndex = 6;
		numericUpDownStartSpecificItem.Value = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownStartSpecificItem.Enter += Control_Enter;
		numericUpDownStartSpecificItem.Leave += Control_Leave;

		// groupBoxStartingPosition
		groupBoxStartingPosition.AccessibleDescription = "Choose where the navigator starts when the application opens";
		groupBoxStartingPosition.AccessibleName = "Starting position";
		groupBoxStartingPosition.AccessibleRole = AccessibleRole.Grouping;
		groupBoxStartingPosition.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		groupBoxStartingPosition.Location = new Point(5, 4);
		groupBoxStartingPosition.Name = "groupBoxStartingPosition";
		groupBoxStartingPosition.Size = new Size(600, 200);
		groupBoxStartingPosition.TabIndex = 0;
		groupBoxStartingPosition.Values.Heading = "Starting position";
		groupBoxStartingPosition.Panel.Controls.Add(radioButtonStartFirst);
		groupBoxStartingPosition.Panel.Controls.Add(radioButtonStartLast);
		groupBoxStartingPosition.Panel.Controls.Add(radioButtonStartLastUsed);
		groupBoxStartingPosition.Panel.Controls.Add(radioButtonStartRandom);
		groupBoxStartingPosition.Panel.Controls.Add(radioButtonStartSpecific);
		groupBoxStartingPosition.Panel.Controls.Add(labelStartSpecificItem);
		groupBoxStartingPosition.Panel.Controls.Add(numericUpDownStartSpecificItem);

		// labelStepSize
		labelStepSize.AccessibleDescription = "Number of items to skip per navigation step";
		labelStepSize.AccessibleName = "Step size";
		labelStepSize.AccessibleRole = AccessibleRole.StaticText;
		labelStepSize.Location = new Point(8, 6);
		labelStepSize.Name = "labelStepSize";
		labelStepSize.Size = new Size(65, 20);
		labelStepSize.TabIndex = 0;
		labelStepSize.Values.Text = "Step size:";

		// numericUpDownStepSize
		numericUpDownStepSize.AccessibleDescription = "Number of records to skip per navigation step button press";
		numericUpDownStepSize.AccessibleName = "Navigation step size";
		numericUpDownStepSize.AccessibleRole = AccessibleRole.SpinButton;
		numericUpDownStepSize.Location = new Point(115, 3);
		numericUpDownStepSize.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
		numericUpDownStepSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
		numericUpDownStepSize.Name = "numericUpDownStepSize";
		numericUpDownStepSize.Size = new Size(120, 22);
		numericUpDownStepSize.TabIndex = 1;
		numericUpDownStepSize.Value = new decimal(new int[] { 10, 0, 0, 0 });
		numericUpDownStepSize.Enter += Control_Enter;
		numericUpDownStepSize.Leave += Control_Leave;

		// groupBoxNavigationStep
		groupBoxNavigationStep.AccessibleDescription = "Number of records to skip when using step navigation";
		groupBoxNavigationStep.AccessibleName = "Navigation step size";
		groupBoxNavigationStep.AccessibleRole = AccessibleRole.Grouping;
		groupBoxNavigationStep.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		groupBoxNavigationStep.Location = new Point(5, 210);
		groupBoxNavigationStep.Name = "groupBoxNavigationStep";
		groupBoxNavigationStep.Size = new Size(600, 65);
		groupBoxNavigationStep.TabIndex = 1;
		groupBoxNavigationStep.Values.Heading = "Navigation step size";
		groupBoxNavigationStep.Panel.Controls.Add(labelStepSize);
		groupBoxNavigationStep.Panel.Controls.Add(numericUpDownStepSize);

		// tabPageNavigator
		tabPageNavigator.Controls.Add(groupBoxStartingPosition);
		tabPageNavigator.Controls.Add(groupBoxNavigationStep);
		tabPageNavigator.Location = new Point(4, 24);
		tabPageNavigator.Name = "tabPageNavigator";
		tabPageNavigator.Padding = new Padding(3);
		tabPageNavigator.Size = new Size(612, 375);
		tabPageNavigator.TabIndex = 1;
		tabPageNavigator.Text = "Navigator";
		tabPageNavigator.UseVisualStyleBackColor = true;

		// ===========================
		// DATABASE UPDATE TAB
		// ===========================

		// checkBoxCheckUpdateOnStartup
		checkBoxCheckUpdateOnStartup.AccessibleDescription = "Check whether a MPCORB.DAT update is available each time the application starts";
		checkBoxCheckUpdateOnStartup.AccessibleName = "Check MPCORB.DAT update on startup";
		checkBoxCheckUpdateOnStartup.AccessibleRole = AccessibleRole.CheckButton;
		checkBoxCheckUpdateOnStartup.Location = new Point(8, 5);
		checkBoxCheckUpdateOnStartup.Name = "checkBoxCheckUpdateOnStartup";
		checkBoxCheckUpdateOnStartup.Size = new Size(310, 23);
		checkBoxCheckUpdateOnStartup.TabIndex = 0;
		checkBoxCheckUpdateOnStartup.ToolTipValues.EnableToolTips = true;
		checkBoxCheckUpdateOnStartup.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		checkBoxCheckUpdateOnStartup.ToolTipValues.Description = "Checks for a newer MPCORB.DAT file on the MPC server every time the application starts.";
		checkBoxCheckUpdateOnStartup.Values.Text = "Check MPCORB.DAT update on startup";
		checkBoxCheckUpdateOnStartup.Enter += Control_Enter;
		checkBoxCheckUpdateOnStartup.Leave += Control_Leave;

		// checkBoxAutoDownloadOnStartup
		checkBoxAutoDownloadOnStartup.AccessibleDescription = "Automatically download a MPCORB.DAT update if one is available on startup";
		checkBoxAutoDownloadOnStartup.AccessibleName = "Download automatically the MPCORB.DAT update on startup";
		checkBoxAutoDownloadOnStartup.AccessibleRole = AccessibleRole.CheckButton;
		checkBoxAutoDownloadOnStartup.Location = new Point(8, 32);
		checkBoxAutoDownloadOnStartup.Name = "checkBoxAutoDownloadOnStartup";
		checkBoxAutoDownloadOnStartup.Size = new Size(430, 23);
		checkBoxAutoDownloadOnStartup.TabIndex = 1;
		checkBoxAutoDownloadOnStartup.ToolTipValues.EnableToolTips = true;
		checkBoxAutoDownloadOnStartup.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		checkBoxAutoDownloadOnStartup.ToolTipValues.Description = "When checked, a newer MPCORB.DAT is downloaded automatically on startup without user confirmation.";
		checkBoxAutoDownloadOnStartup.Values.Text = "Download automatically the MPCORB.DAT update on startup";
		checkBoxAutoDownloadOnStartup.Enter += Control_Enter;
		checkBoxAutoDownloadOnStartup.Leave += Control_Leave;

		// checkBoxAskForRestartOnStartup
		checkBoxAskForRestartOnStartup.AccessibleDescription = "Ask the user to restart the application after a MPCORB.DAT update on startup";
		checkBoxAskForRestartOnStartup.AccessibleName = "Ask for restart after the MPCORB.DAT update";
		checkBoxAskForRestartOnStartup.AccessibleRole = AccessibleRole.CheckButton;
		checkBoxAskForRestartOnStartup.Location = new Point(8, 59);
		checkBoxAskForRestartOnStartup.Name = "checkBoxAskForRestartOnStartup";
		checkBoxAskForRestartOnStartup.Size = new Size(340, 23);
		checkBoxAskForRestartOnStartup.TabIndex = 2;
		checkBoxAskForRestartOnStartup.ToolTipValues.EnableToolTips = true;
		checkBoxAskForRestartOnStartup.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		checkBoxAskForRestartOnStartup.ToolTipValues.Description = "When checked, the user is prompted to restart the application after a startup update to reload the new database.";
		checkBoxAskForRestartOnStartup.Values.Text = "Ask for restart after the MPCORB.DAT update";
		checkBoxAskForRestartOnStartup.Enter += Control_Enter;
		checkBoxAskForRestartOnStartup.Leave += Control_Leave;

		// groupBoxStartupUpdate
		groupBoxStartupUpdate.AccessibleDescription = "MPCORB.DAT update options applied when the application starts";
		groupBoxStartupUpdate.AccessibleName = "Startup update";
		groupBoxStartupUpdate.AccessibleRole = AccessibleRole.Grouping;
		groupBoxStartupUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		groupBoxStartupUpdate.Location = new Point(5, 4);
		groupBoxStartupUpdate.Name = "groupBoxStartupUpdate";
		groupBoxStartupUpdate.Size = new Size(600, 115);
		groupBoxStartupUpdate.TabIndex = 0;
		groupBoxStartupUpdate.Values.Heading = "Startup update";
		groupBoxStartupUpdate.Panel.Controls.Add(checkBoxCheckUpdateOnStartup);
		groupBoxStartupUpdate.Panel.Controls.Add(checkBoxAutoDownloadOnStartup);
		groupBoxStartupUpdate.Panel.Controls.Add(checkBoxAskForRestartOnStartup);

		// checkBoxCheckUpdateInBackground
		checkBoxCheckUpdateInBackground.AccessibleDescription = "Check for a MPCORB.DAT update every hour while the application is running";
		checkBoxCheckUpdateInBackground.AccessibleName = "Check the MPCORB.DAT update every hour in background";
		checkBoxCheckUpdateInBackground.AccessibleRole = AccessibleRole.CheckButton;
		checkBoxCheckUpdateInBackground.Location = new Point(8, 5);
		checkBoxCheckUpdateInBackground.Name = "checkBoxCheckUpdateInBackground";
		checkBoxCheckUpdateInBackground.Size = new Size(430, 23);
		checkBoxCheckUpdateInBackground.TabIndex = 0;
		checkBoxCheckUpdateInBackground.ToolTipValues.EnableToolTips = true;
		checkBoxCheckUpdateInBackground.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		checkBoxCheckUpdateInBackground.ToolTipValues.Description = "Silently checks for a newer MPCORB.DAT once per hour while the application is open.";
		checkBoxCheckUpdateInBackground.Values.Text = "Check the MPCORB.DAT update every hour in background";
		checkBoxCheckUpdateInBackground.Enter += Control_Enter;
		checkBoxCheckUpdateInBackground.Leave += Control_Leave;

		// checkBoxAutoDownloadInBackground
		checkBoxAutoDownloadInBackground.AccessibleDescription = "Automatically download a MPCORB.DAT update if one is found in the background";
		checkBoxAutoDownloadInBackground.AccessibleName = "Download automatically the MPCORB.DAT update in background";
		checkBoxAutoDownloadInBackground.AccessibleRole = AccessibleRole.CheckButton;
		checkBoxAutoDownloadInBackground.Location = new Point(8, 32);
		checkBoxAutoDownloadInBackground.Name = "checkBoxAutoDownloadInBackground";
		checkBoxAutoDownloadInBackground.Size = new Size(455, 23);
		checkBoxAutoDownloadInBackground.TabIndex = 1;
		checkBoxAutoDownloadInBackground.ToolTipValues.EnableToolTips = true;
		checkBoxAutoDownloadInBackground.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		checkBoxAutoDownloadInBackground.ToolTipValues.Description = "When checked, a newer MPCORB.DAT found in the background is downloaded silently without user confirmation.";
		checkBoxAutoDownloadInBackground.Values.Text = "Download automatically the MPCORB.DAT update in background";
		checkBoxAutoDownloadInBackground.Enter += Control_Enter;
		checkBoxAutoDownloadInBackground.Leave += Control_Leave;

		// checkBoxAskForRestartInBackground
		checkBoxAskForRestartInBackground.AccessibleDescription = "Ask the user to restart after a MPCORB.DAT background update";
		checkBoxAskForRestartInBackground.AccessibleName = "Ask for restart after the MPCORB.DAT background update";
		checkBoxAskForRestartInBackground.AccessibleRole = AccessibleRole.CheckButton;
		checkBoxAskForRestartInBackground.Location = new Point(8, 59);
		checkBoxAskForRestartInBackground.Name = "checkBoxAskForRestartInBackground";
		checkBoxAskForRestartInBackground.Size = new Size(380, 23);
		checkBoxAskForRestartInBackground.TabIndex = 2;
		checkBoxAskForRestartInBackground.ToolTipValues.EnableToolTips = true;
		checkBoxAskForRestartInBackground.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		checkBoxAskForRestartInBackground.ToolTipValues.Description = "When checked, the user is prompted to restart the application after a background update to reload the new database.";
		checkBoxAskForRestartInBackground.Values.Text = "Ask for restart after the MPCORB.DAT background update";
		checkBoxAskForRestartInBackground.Enter += Control_Enter;
		checkBoxAskForRestartInBackground.Leave += Control_Leave;

		// groupBoxBackgroundUpdate
		groupBoxBackgroundUpdate.AccessibleDescription = "MPCORB.DAT update options for the background update check";
		groupBoxBackgroundUpdate.AccessibleName = "Background update";
		groupBoxBackgroundUpdate.AccessibleRole = AccessibleRole.Grouping;
		groupBoxBackgroundUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		groupBoxBackgroundUpdate.Location = new Point(5, 125);
		groupBoxBackgroundUpdate.Name = "groupBoxBackgroundUpdate";
		groupBoxBackgroundUpdate.Size = new Size(600, 115);
		groupBoxBackgroundUpdate.TabIndex = 1;
		groupBoxBackgroundUpdate.Values.Heading = "Background update";
		groupBoxBackgroundUpdate.Panel.Controls.Add(checkBoxCheckUpdateInBackground);
		groupBoxBackgroundUpdate.Panel.Controls.Add(checkBoxAutoDownloadInBackground);
		groupBoxBackgroundUpdate.Panel.Controls.Add(checkBoxAskForRestartInBackground);

		// labelDownloadTimeout
		labelDownloadTimeout.AccessibleDescription = "Maximum time to wait for the database file download to complete";
		labelDownloadTimeout.AccessibleName = "Download timeout in seconds";
		labelDownloadTimeout.AccessibleRole = AccessibleRole.StaticText;
		labelDownloadTimeout.Location = new Point(8, 6);
		labelDownloadTimeout.Name = "labelDownloadTimeout";
		labelDownloadTimeout.Size = new Size(195, 20);
		labelDownloadTimeout.TabIndex = 0;
		labelDownloadTimeout.Values.Text = "Download timeout (seconds):";

		// numericUpDownDownloadTimeout
		numericUpDownDownloadTimeout.AccessibleDescription = "Maximum time in seconds to wait for a database download to complete";
		numericUpDownDownloadTimeout.AccessibleName = "Download timeout";
		numericUpDownDownloadTimeout.AccessibleRole = AccessibleRole.SpinButton;
		numericUpDownDownloadTimeout.Location = new Point(210, 3);
		numericUpDownDownloadTimeout.Maximum = new decimal(new int[] { 3600, 0, 0, 0 });
		numericUpDownDownloadTimeout.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
		numericUpDownDownloadTimeout.Name = "numericUpDownDownloadTimeout";
		numericUpDownDownloadTimeout.Size = new Size(100, 22);
		numericUpDownDownloadTimeout.TabIndex = 1;
		numericUpDownDownloadTimeout.Value = new decimal(new int[] { 300, 0, 0, 0 });
		numericUpDownDownloadTimeout.Enter += Control_Enter;
		numericUpDownDownloadTimeout.Leave += Control_Leave;

		// labelMaxRetries
		labelMaxRetries.AccessibleDescription = "Maximum number of retry attempts if a database download fails";
		labelMaxRetries.AccessibleName = "Maximum retries";
		labelMaxRetries.AccessibleRole = AccessibleRole.StaticText;
		labelMaxRetries.Location = new Point(8, 33);
		labelMaxRetries.Name = "labelMaxRetries";
		labelMaxRetries.Size = new Size(120, 20);
		labelMaxRetries.TabIndex = 2;
		labelMaxRetries.Values.Text = "Maximum retries:";

		// numericUpDownMaxRetries
		numericUpDownMaxRetries.AccessibleDescription = "Maximum number of times a failed database download is retried";
		numericUpDownMaxRetries.AccessibleName = "Maximum download retries";
		numericUpDownMaxRetries.AccessibleRole = AccessibleRole.SpinButton;
		numericUpDownMaxRetries.Location = new Point(210, 30);
		numericUpDownMaxRetries.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
		numericUpDownMaxRetries.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
		numericUpDownMaxRetries.Name = "numericUpDownMaxRetries";
		numericUpDownMaxRetries.Size = new Size(100, 22);
		numericUpDownMaxRetries.TabIndex = 3;
		numericUpDownMaxRetries.Value = new decimal(new int[] { 3, 0, 0, 0 });
		numericUpDownMaxRetries.Enter += Control_Enter;
		numericUpDownMaxRetries.Leave += Control_Leave;

		// groupBoxDownloadOptions
		groupBoxDownloadOptions.AccessibleDescription = "Advanced options for database file downloads";
		groupBoxDownloadOptions.AccessibleName = "Download options";
		groupBoxDownloadOptions.AccessibleRole = AccessibleRole.Grouping;
		groupBoxDownloadOptions.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		groupBoxDownloadOptions.Location = new Point(5, 246);
		groupBoxDownloadOptions.Name = "groupBoxDownloadOptions";
		groupBoxDownloadOptions.Size = new Size(600, 90);
		groupBoxDownloadOptions.TabIndex = 2;
		groupBoxDownloadOptions.Values.Heading = "Download options";
		groupBoxDownloadOptions.Panel.Controls.Add(labelDownloadTimeout);
		groupBoxDownloadOptions.Panel.Controls.Add(numericUpDownDownloadTimeout);
		groupBoxDownloadOptions.Panel.Controls.Add(labelMaxRetries);
		groupBoxDownloadOptions.Panel.Controls.Add(numericUpDownMaxRetries);

		// tabPageDatabaseUpdate
		tabPageDatabaseUpdate.Controls.Add(groupBoxStartupUpdate);
		tabPageDatabaseUpdate.Controls.Add(groupBoxBackgroundUpdate);
		tabPageDatabaseUpdate.Controls.Add(groupBoxDownloadOptions);
		tabPageDatabaseUpdate.Location = new Point(4, 24);
		tabPageDatabaseUpdate.Name = "tabPageDatabaseUpdate";
		tabPageDatabaseUpdate.Padding = new Padding(3);
		tabPageDatabaseUpdate.Size = new Size(612, 375);
		tabPageDatabaseUpdate.TabIndex = 2;
		tabPageDatabaseUpdate.Text = "Database Update";
		tabPageDatabaseUpdate.UseVisualStyleBackColor = true;

		// ===========================
		// APPEARANCE TAB
		// ===========================

		// labelTheme
		labelTheme.AccessibleDescription = "Select the visual theme (palette) for the application";
		labelTheme.AccessibleName = "Theme palette";
		labelTheme.AccessibleRole = AccessibleRole.StaticText;
		labelTheme.Location = new Point(8, 6);
		labelTheme.Name = "labelTheme";
		labelTheme.Size = new Size(60, 20);
		labelTheme.TabIndex = 0;
		labelTheme.Values.Text = "Palette:";

		// comboBoxTheme
		comboBoxTheme.AccessibleDescription = "Select the visual theme palette for the application";
		comboBoxTheme.AccessibleName = "Theme palette";
		comboBoxTheme.AccessibleRole = AccessibleRole.ComboBox;
		comboBoxTheme.DropDownStyle = ComboBoxStyle.DropDownList;
		comboBoxTheme.FormattingEnabled = true;
		// comboBoxTheme items are display strings.
		// When save logic is implemented, map these strings to the corresponding
		// PaletteMode enum values (e.g. "Office 2007 Blue" → PaletteMode.Office2007Blue).
		comboBoxTheme.Items.AddRange(new object[]
		{
			"Professional System",
			"Professional Office 2003",
			"Office 2007 Blue",
			"Office 2007 Black",
			"Office 2007 Silver",
			"Office 2007 White",
			"Office 2010 Blue",
			"Office 2010 Black",
			"Office 2010 Silver",
			"Office 2010 White",
			"Office 2013",
			"Office 2013 White",
			"Microsoft 365 Blue",
			"Microsoft 365 Black",
			"Microsoft 365 Silver",
			"Microsoft 365 White",
			"Sparkle Blue",
			"Sparkle Red",
			"Sparkle Orange",
		});
		comboBoxTheme.Location = new Point(75, 3);
		comboBoxTheme.Name = "comboBoxTheme";
		comboBoxTheme.Size = new Size(450, 23);
		comboBoxTheme.TabIndex = 1;
		comboBoxTheme.Enter += Control_Enter;
		comboBoxTheme.Leave += Control_Leave;

		// groupBoxTheme
		groupBoxTheme.AccessibleDescription = "Visual theme and color scheme for the application";
		groupBoxTheme.AccessibleName = "Theme";
		groupBoxTheme.AccessibleRole = AccessibleRole.Grouping;
		groupBoxTheme.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		groupBoxTheme.Location = new Point(5, 4);
		groupBoxTheme.Name = "groupBoxTheme";
		groupBoxTheme.Size = new Size(600, 65);
		groupBoxTheme.TabIndex = 0;
		groupBoxTheme.Values.Heading = "Theme";
		groupBoxTheme.Panel.Controls.Add(labelTheme);
		groupBoxTheme.Panel.Controls.Add(comboBoxTheme);

		// radioButtonIconsFatcow
		radioButtonIconsFatcow.AccessibleDescription = "Use the FatCow icon set for all toolbar and menu icons";
		radioButtonIconsFatcow.AccessibleName = "FatCow icons";
		radioButtonIconsFatcow.AccessibleRole = AccessibleRole.RadioButton;
		radioButtonIconsFatcow.Checked = true;
		radioButtonIconsFatcow.Location = new Point(8, 5);
		radioButtonIconsFatcow.Name = "radioButtonIconsFatcow";
		radioButtonIconsFatcow.Size = new Size(140, 23);
		radioButtonIconsFatcow.TabIndex = 0;
		radioButtonIconsFatcow.ToolTipValues.EnableToolTips = true;
		radioButtonIconsFatcow.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		radioButtonIconsFatcow.ToolTipValues.Description = "Use the FatCow 16 px icon set for all application icons.";
		radioButtonIconsFatcow.Values.Text = "FatCow Icons";
		radioButtonIconsFatcow.Enter += Control_Enter;
		radioButtonIconsFatcow.Leave += Control_Leave;

		// radioButtonIconsSilk
		radioButtonIconsSilk.AccessibleDescription = "Use the Silk icon set for all toolbar and menu icons";
		radioButtonIconsSilk.AccessibleName = "Silk icons";
		radioButtonIconsSilk.AccessibleRole = AccessibleRole.RadioButton;
		radioButtonIconsSilk.Location = new Point(8, 32);
		radioButtonIconsSilk.Name = "radioButtonIconsSilk";
		radioButtonIconsSilk.Size = new Size(120, 23);
		radioButtonIconsSilk.TabIndex = 1;
		radioButtonIconsSilk.ToolTipValues.EnableToolTips = true;
		radioButtonIconsSilk.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		radioButtonIconsSilk.ToolTipValues.Description = "Use the Silk 16 px icon set for all application icons.";
		radioButtonIconsSilk.Values.Text = "Silk Icons";
		radioButtonIconsSilk.Enter += Control_Enter;
		radioButtonIconsSilk.Leave += Control_Leave;

		// radioButtonIconsFugue
		radioButtonIconsFugue.AccessibleDescription = "Use the Fugue icon set for all toolbar and menu icons";
		radioButtonIconsFugue.AccessibleName = "Fugue icons";
		radioButtonIconsFugue.AccessibleRole = AccessibleRole.RadioButton;
		radioButtonIconsFugue.Location = new Point(8, 59);
		radioButtonIconsFugue.Name = "radioButtonIconsFugue";
		radioButtonIconsFugue.Size = new Size(140, 23);
		radioButtonIconsFugue.TabIndex = 2;
		radioButtonIconsFugue.ToolTipValues.EnableToolTips = true;
		radioButtonIconsFugue.ToolTipValues.Image = FatcowIcons16px.fatcow_information_16px;
		radioButtonIconsFugue.ToolTipValues.Description = "Use the Fugue 16 px icon set for all application icons.";
		radioButtonIconsFugue.Values.Text = "Fugue Icons";
		radioButtonIconsFugue.Enter += Control_Enter;
		radioButtonIconsFugue.Leave += Control_Leave;

		// groupBoxIconSet
		groupBoxIconSet.AccessibleDescription = "Choose the icon set used for toolbar and menu buttons";
		groupBoxIconSet.AccessibleName = "Icon set";
		groupBoxIconSet.AccessibleRole = AccessibleRole.Grouping;
		groupBoxIconSet.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
		groupBoxIconSet.Location = new Point(5, 75);
		groupBoxIconSet.Name = "groupBoxIconSet";
		groupBoxIconSet.Size = new Size(600, 115);
		groupBoxIconSet.TabIndex = 1;
		groupBoxIconSet.Values.Heading = "Icon set";
		groupBoxIconSet.Panel.Controls.Add(radioButtonIconsFatcow);
		groupBoxIconSet.Panel.Controls.Add(radioButtonIconsSilk);
		groupBoxIconSet.Panel.Controls.Add(radioButtonIconsFugue);

		// tabPageAppearance
		tabPageAppearance.Controls.Add(groupBoxTheme);
		tabPageAppearance.Controls.Add(groupBoxIconSet);
		tabPageAppearance.Location = new Point(4, 24);
		tabPageAppearance.Name = "tabPageAppearance";
		tabPageAppearance.Padding = new Padding(3);
		tabPageAppearance.Size = new Size(612, 375);
		tabPageAppearance.TabIndex = 3;
		tabPageAppearance.Text = "Appearance";
		tabPageAppearance.UseVisualStyleBackColor = true;

		// ===========================
		// TAB CONTROL
		// ===========================

		tabControlSettings.Controls.Add(tabPageGeneral);
		tabControlSettings.Controls.Add(tabPageNavigator);
		tabControlSettings.Controls.Add(tabPageDatabaseUpdate);
		tabControlSettings.Controls.Add(tabPageAppearance);
		tabControlSettings.Dock = DockStyle.Fill;
		tabControlSettings.Location = new Point(0, 0);
		tabControlSettings.Name = "tabControlSettings";
		tabControlSettings.SelectedIndex = 0;
		tabControlSettings.ShowToolTips = true;
		tabControlSettings.Size = new Size(620, 403);
		tabControlSettings.TabIndex = 0;

		// ===========================
		// TOOLBAR
		// ===========================

		// toolStripButtonSave
		toolStripButtonSave.AccessibleDescription = "Saves the settings and closes the dialog";
		toolStripButtonSave.AccessibleName = "Save";
		toolStripButtonSave.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonSave.DoubleClickEnabled = true;
		toolStripButtonSave.Image = FatcowIcons16px.fatcow_diskette_16px;
		toolStripButtonSave.ImageTransparentColor = Color.Magenta;
		toolStripButtonSave.Name = "toolStripButtonSave";
		toolStripButtonSave.Size = new Size(51, 22);
		toolStripButtonSave.Text = "Save";
		toolStripButtonSave.ToolTipText = "Save the settings";
		toolStripButtonSave.Click += ToolStripButtonSave_Click;
		toolStripButtonSave.MouseEnter += Control_Enter;
		toolStripButtonSave.MouseLeave += Control_Leave;

		// toolStripButtonCancel
		toolStripButtonCancel.AccessibleDescription = "Discards any changes and closes the dialog";
		toolStripButtonCancel.AccessibleName = "Cancel";
		toolStripButtonCancel.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonCancel.DoubleClickEnabled = true;
		toolStripButtonCancel.Image = FatcowIcons16px.fatcow_cancel_16px;
		toolStripButtonCancel.ImageTransparentColor = Color.Magenta;
		toolStripButtonCancel.Name = "toolStripButtonCancel";
		toolStripButtonCancel.Size = new Size(63, 22);
		toolStripButtonCancel.Text = "Cancel";
		toolStripButtonCancel.ToolTipText = "Cancel the settings";
		toolStripButtonCancel.Click += ToolStripButtonCancel_Click;
		toolStripButtonCancel.MouseEnter += Control_Enter;
		toolStripButtonCancel.MouseLeave += Control_Leave;

		// toolStripSeparator1
		toolStripSeparator1.Name = "toolStripSeparator1";
		toolStripSeparator1.Size = new Size(6, 25);

		// toolStripButtonLoadDefaultSettings
		toolStripButtonLoadDefaultSettings.AccessibleDescription = "Resets all settings to their default values";
		toolStripButtonLoadDefaultSettings.AccessibleName = "Default settings";
		toolStripButtonLoadDefaultSettings.AccessibleRole = AccessibleRole.PushButton;
		toolStripButtonLoadDefaultSettings.DoubleClickEnabled = true;
		toolStripButtonLoadDefaultSettings.Image = FatcowIcons16px.fatcow_cog_16px;
		toolStripButtonLoadDefaultSettings.ImageTransparentColor = Color.Magenta;
		toolStripButtonLoadDefaultSettings.Name = "toolStripButtonLoadDefaultSettings";
		toolStripButtonLoadDefaultSettings.Size = new Size(109, 22);
		toolStripButtonLoadDefaultSettings.Text = "Default settings";
		toolStripButtonLoadDefaultSettings.ToolTipText = "Load default settings";
		toolStripButtonLoadDefaultSettings.Click += ToolStripButtonLoadDefaultSettings_Click;
		toolStripButtonLoadDefaultSettings.MouseEnter += Control_Enter;
		toolStripButtonLoadDefaultSettings.MouseLeave += Control_Leave;

		// kryptonToolStripIcons
		kryptonToolStripIcons.AccessibleDescription = "Toolbar of main functions";
		kryptonToolStripIcons.AccessibleName = "Toolbar of main functions";
		kryptonToolStripIcons.AccessibleRole = AccessibleRole.ToolBar;
		kryptonToolStripIcons.AllowClickThrough = true;
		kryptonToolStripIcons.AllowItemReorder = true;
		kryptonToolStripIcons.Dock = DockStyle.None;
		kryptonToolStripIcons.Font = new Font("Segoe UI", 9F);
		kryptonToolStripIcons.Items.AddRange(new ToolStripItem[] { toolStripButtonSave, toolStripButtonCancel, toolStripSeparator1, toolStripButtonLoadDefaultSettings });
		kryptonToolStripIcons.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
		kryptonToolStripIcons.Location = new Point(0, 0);
		kryptonToolStripIcons.Name = "kryptonToolStripIcons";
		kryptonToolStripIcons.Size = new Size(620, 25);
		kryptonToolStripIcons.Stretch = true;
		kryptonToolStripIcons.TabIndex = 0;
		kryptonToolStripIcons.TabStop = true;
		kryptonToolStripIcons.Text = "Toolbar of main functions";

		// ===========================
		// MAIN PANEL AND CONTAINER
		// ===========================

		// kryptonPanelMain
		kryptonPanelMain.AccessibleDescription = "Groups the settings";
		kryptonPanelMain.AccessibleName = "Settings panel";
		kryptonPanelMain.AccessibleRole = AccessibleRole.Pane;
		kryptonPanelMain.Controls.Add(tabControlSettings);
		kryptonPanelMain.Dock = DockStyle.Fill;
		kryptonPanelMain.Location = new Point(0, 0);
		kryptonPanelMain.Name = "kryptonPanelMain";
		kryptonPanelMain.PanelBackStyle = PaletteBackStyle.FormMain;
		kryptonPanelMain.Size = new Size(620, 403);
		kryptonPanelMain.TabIndex = 0;
		kryptonPanelMain.TabStop = true;

		// toolStripContainerSettings
		toolStripContainerSettings.ContentPanel.Controls.Add(kryptonPanelMain);
		toolStripContainerSettings.ContentPanel.Margin = new Padding(4, 3, 4, 3);
		toolStripContainerSettings.ContentPanel.Size = new Size(620, 403);
		toolStripContainerSettings.Dock = DockStyle.Fill;
		toolStripContainerSettings.Location = new Point(0, 0);
		toolStripContainerSettings.Name = "toolStripContainerSettings";
		toolStripContainerSettings.Size = new Size(620, 428);
		toolStripContainerSettings.TabIndex = 0;
		toolStripContainerSettings.Text = "Settings";
		toolStripContainerSettings.TopToolStripPanel.Controls.Add(kryptonToolStripIcons);

		// ===========================
		// STATUS STRIP
		// ===========================

		// labelInformation
		labelInformation.AccessibleDescription = "Shows information about the focused control";
		labelInformation.AccessibleName = "Information";
		labelInformation.AccessibleRole = AccessibleRole.StaticText;
		labelInformation.AutoToolTip = true;
		labelInformation.Image = FatcowIcons16px.fatcow_lightbulb_16px;
		labelInformation.Name = "labelInformation";
		labelInformation.Size = new Size(160, 17);
		labelInformation.Text = "some information here";
		labelInformation.ToolTipText = "Shows information about the focused control";

		// kryptonStatusStrip
		kryptonStatusStrip.AccessibleDescription = "Shows information about the focused control";
		kryptonStatusStrip.AccessibleName = "Status bar";
		kryptonStatusStrip.AccessibleRole = AccessibleRole.StatusBar;
		kryptonStatusStrip.AllowClickThrough = true;
		kryptonStatusStrip.AllowItemReorder = true;
		kryptonStatusStrip.Font = new Font("Segoe UI", 9F);
		kryptonStatusStrip.Items.AddRange(new ToolStripItem[] { labelInformation });
		kryptonStatusStrip.Location = new Point(0, 428);
		kryptonStatusStrip.Name = "kryptonStatusStrip";
		kryptonStatusStrip.Padding = new Padding(1, 0, 16, 0);
		kryptonStatusStrip.ProgressBars = null;
		kryptonStatusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
		kryptonStatusStrip.ShowItemToolTips = true;
		kryptonStatusStrip.Size = new Size(620, 22);
		kryptonStatusStrip.SizingGrip = false;
		kryptonStatusStrip.TabIndex = 1;
		kryptonStatusStrip.TabStop = true;
		kryptonStatusStrip.Text = "Status bar";

		// kryptonManager
		kryptonManager.GlobalPaletteMode = PaletteMode.Global;
		kryptonManager.ToolkitStrings.MessageBoxStrings.LessDetails = "L&ess Details...";
		kryptonManager.ToolkitStrings.MessageBoxStrings.MoreDetails = "&More Details...";

		// ===========================
		// FORM
		// ===========================

		AccessibleDescription = "Configure application settings including window behavior, navigation preferences, database update options, and visual appearance.";
		AccessibleName = "Settings";
		AccessibleRole = AccessibleRole.Dialog;
		AutoScaleDimensions = new SizeF(7F, 15F);
		AutoScaleMode = AutoScaleMode.Font;
		ClientSize = new Size(620, 450);
		ControlBox = false;
		Controls.Add(kryptonStatusStrip);
		Controls.Add(toolStripContainerSettings);
		FormBorderStyle = FormBorderStyle.FixedToolWindow;
		Icon = (Icon)resources.GetObject("$this.Icon");
		Margin = new Padding(4, 3, 4, 3);
		MaximizeBox = false;
		MinimizeBox = false;
		Name = "SettingsForm";
		StartPosition = FormStartPosition.CenterScreen;
		Text = "Settings";
		Load += SettingsForm_Load;

		// ===========================
		// RESUME LAYOUT (inner to outer)
		// ===========================

		groupBoxApplicationBehavior.Panel.ResumeLayout(false);
		groupBoxApplicationBehavior.Panel.PerformLayout();
		((ISupportInitialize)groupBoxApplicationBehavior.Panel).EndInit();
		groupBoxApplicationBehavior.ResumeLayout(false);
		((ISupportInitialize)groupBoxApplicationBehavior).EndInit();

		groupBoxInteraction.Panel.ResumeLayout(false);
		groupBoxInteraction.Panel.PerformLayout();
		((ISupportInitialize)groupBoxInteraction.Panel).EndInit();
		groupBoxInteraction.ResumeLayout(false);
		((ISupportInitialize)groupBoxInteraction).EndInit();

		((ISupportInitialize)numericUpDownStartSpecificItem).EndInit();

		groupBoxStartingPosition.Panel.ResumeLayout(false);
		groupBoxStartingPosition.Panel.PerformLayout();
		((ISupportInitialize)groupBoxStartingPosition.Panel).EndInit();
		groupBoxStartingPosition.ResumeLayout(false);
		((ISupportInitialize)groupBoxStartingPosition).EndInit();

		((ISupportInitialize)numericUpDownStepSize).EndInit();

		groupBoxNavigationStep.Panel.ResumeLayout(false);
		groupBoxNavigationStep.Panel.PerformLayout();
		((ISupportInitialize)groupBoxNavigationStep.Panel).EndInit();
		groupBoxNavigationStep.ResumeLayout(false);
		((ISupportInitialize)groupBoxNavigationStep).EndInit();

		groupBoxStartupUpdate.Panel.ResumeLayout(false);
		groupBoxStartupUpdate.Panel.PerformLayout();
		((ISupportInitialize)groupBoxStartupUpdate.Panel).EndInit();
		groupBoxStartupUpdate.ResumeLayout(false);
		((ISupportInitialize)groupBoxStartupUpdate).EndInit();

		groupBoxBackgroundUpdate.Panel.ResumeLayout(false);
		groupBoxBackgroundUpdate.Panel.PerformLayout();
		((ISupportInitialize)groupBoxBackgroundUpdate.Panel).EndInit();
		groupBoxBackgroundUpdate.ResumeLayout(false);
		((ISupportInitialize)groupBoxBackgroundUpdate).EndInit();

		((ISupportInitialize)numericUpDownDownloadTimeout).EndInit();
		((ISupportInitialize)numericUpDownMaxRetries).EndInit();

		groupBoxDownloadOptions.Panel.ResumeLayout(false);
		groupBoxDownloadOptions.Panel.PerformLayout();
		((ISupportInitialize)groupBoxDownloadOptions.Panel).EndInit();
		groupBoxDownloadOptions.ResumeLayout(false);
		((ISupportInitialize)groupBoxDownloadOptions).EndInit();

		groupBoxTheme.Panel.ResumeLayout(false);
		groupBoxTheme.Panel.PerformLayout();
		((ISupportInitialize)groupBoxTheme.Panel).EndInit();
		groupBoxTheme.ResumeLayout(false);
		((ISupportInitialize)groupBoxTheme).EndInit();

		groupBoxIconSet.Panel.ResumeLayout(false);
		groupBoxIconSet.Panel.PerformLayout();
		((ISupportInitialize)groupBoxIconSet.Panel).EndInit();
		groupBoxIconSet.ResumeLayout(false);
		((ISupportInitialize)groupBoxIconSet).EndInit();

		tabPageGeneral.ResumeLayout(false);
		tabPageNavigator.ResumeLayout(false);
		tabPageDatabaseUpdate.ResumeLayout(false);
		tabPageAppearance.ResumeLayout(false);
		tabControlSettings.ResumeLayout(false);

		kryptonToolStripIcons.ResumeLayout(false);
		kryptonToolStripIcons.PerformLayout();

		toolStripContainerSettings.ContentPanel.ResumeLayout(false);
		toolStripContainerSettings.TopToolStripPanel.ResumeLayout(false);
		toolStripContainerSettings.TopToolStripPanel.PerformLayout();
		toolStripContainerSettings.ResumeLayout(false);
		toolStripContainerSettings.PerformLayout();

		((ISupportInitialize)kryptonPanelMain).EndInit();
		kryptonPanelMain.ResumeLayout(false);

		kryptonStatusStrip.ResumeLayout(false);
		kryptonStatusStrip.PerformLayout();

		ResumeLayout(false);
		PerformLayout();
	}

	#endregion

	// Tab control and pages
	private TabControl tabControlSettings;
	private TabPage tabPageGeneral;
	private TabPage tabPageNavigator;
	private TabPage tabPageDatabaseUpdate;
	private TabPage tabPageAppearance;

	// General tab controls
	private KryptonGroupBox groupBoxApplicationBehavior;
	private KryptonCheckBox checkBoxStayAlwaysOnTop;
	private KryptonCheckBox checkBoxShowSplashScreen;
	private KryptonCheckBox checkBoxConfirmBeforeExit;
	private KryptonGroupBox groupBoxInteraction;
	private KryptonCheckBox checkBoxCopyToClipboardOnDoubleClick;
	private KryptonCheckBox checkBoxLinkToTerminology;

	// Navigator tab controls
	private KryptonGroupBox groupBoxStartingPosition;
	private KryptonRadioButton radioButtonStartFirst;
	private KryptonRadioButton radioButtonStartLast;
	private KryptonRadioButton radioButtonStartLastUsed;
	private KryptonRadioButton radioButtonStartRandom;
	private KryptonRadioButton radioButtonStartSpecific;
	private KryptonLabel labelStartSpecificItem;
	private KryptonNumericUpDown numericUpDownStartSpecificItem;
	private KryptonGroupBox groupBoxNavigationStep;
	private KryptonLabel labelStepSize;
	private KryptonNumericUpDown numericUpDownStepSize;

	// Database Update tab controls
	private KryptonGroupBox groupBoxStartupUpdate;
	private KryptonCheckBox checkBoxCheckUpdateOnStartup;
	private KryptonCheckBox checkBoxAutoDownloadOnStartup;
	private KryptonCheckBox checkBoxAskForRestartOnStartup;
	private KryptonGroupBox groupBoxBackgroundUpdate;
	private KryptonCheckBox checkBoxCheckUpdateInBackground;
	private KryptonCheckBox checkBoxAutoDownloadInBackground;
	private KryptonCheckBox checkBoxAskForRestartInBackground;
	private KryptonGroupBox groupBoxDownloadOptions;
	private KryptonLabel labelDownloadTimeout;
	private KryptonNumericUpDown numericUpDownDownloadTimeout;
	private KryptonLabel labelMaxRetries;
	private KryptonNumericUpDown numericUpDownMaxRetries;

	// Appearance tab controls
	private KryptonGroupBox groupBoxTheme;
	private KryptonLabel labelTheme;
	private ComboBox comboBoxTheme;
	private KryptonGroupBox groupBoxIconSet;
	private KryptonRadioButton radioButtonIconsFatcow;
	private KryptonRadioButton radioButtonIconsSilk;
	private KryptonRadioButton radioButtonIconsFugue;

	// Shared layout controls
	private KryptonToolStrip kryptonToolStripIcons;
	private ToolStripButton toolStripButtonSave;
	private ToolStripButton toolStripButtonCancel;
	private ToolStripSeparator toolStripSeparator1;
	private ToolStripButton toolStripButtonLoadDefaultSettings;
	private ToolStripContainer toolStripContainerSettings;
	private KryptonPanel kryptonPanelMain;
	private KryptonStatusStrip kryptonStatusStrip;
	private ToolStripStatusLabel labelInformation;
	private KryptonManager kryptonManager;
}
