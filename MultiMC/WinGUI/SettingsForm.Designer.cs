namespace MultiMC.WinGUI
{
	partial class SettingsForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.settingsTabs = new System.Windows.Forms.TabControl();
			this.generalTab = new System.Windows.Forms.TabPage();
			this.groupBoxUpdates = new System.Windows.Forms.GroupBox();
			this.forceUpdateCheckBox = new System.Windows.Forms.CheckBox();
			this.autoUpdateCheckBox = new System.Windows.Forms.CheckBox();
			this.groupBoxConsole = new System.Windows.Forms.GroupBox();
			this.checkBoxAutoClose = new System.Windows.Forms.CheckBox();
			this.checkBoxShowConsole = new System.Windows.Forms.CheckBox();
			this.advancedTab = new System.Windows.Forms.TabPage();
			this.groupBoxJava = new System.Windows.Forms.GroupBox();
			this.buttonAutoDetect = new System.Windows.Forms.Button();
			this.textBoxJavaPath = new System.Windows.Forms.TextBox();
			this.labelJPath = new System.Windows.Forms.Label();
			this.groupBoxMemory = new System.Windows.Forms.GroupBox();
			this.labelMaxMem = new System.Windows.Forms.Label();
			this.maxMemAllocSpinner = new System.Windows.Forms.NumericUpDown();
			this.minMemAllocSpinner = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.okButton = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.settingsTabs.SuspendLayout();
			this.generalTab.SuspendLayout();
			this.groupBoxUpdates.SuspendLayout();
			this.groupBoxConsole.SuspendLayout();
			this.advancedTab.SuspendLayout();
			this.groupBoxJava.SuspendLayout();
			this.groupBoxMemory.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.maxMemAllocSpinner)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.minMemAllocSpinner)).BeginInit();
			this.buttonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// settingsTabs
			// 
			this.settingsTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.settingsTabs.Controls.Add(this.generalTab);
			this.settingsTabs.Controls.Add(this.advancedTab);
			this.settingsTabs.Location = new System.Drawing.Point(0, 0);
			this.settingsTabs.Name = "settingsTabs";
			this.settingsTabs.SelectedIndex = 0;
			this.settingsTabs.Size = new System.Drawing.Size(434, 319);
			this.settingsTabs.TabIndex = 0;
			// 
			// generalTab
			// 
			this.generalTab.Controls.Add(this.groupBoxUpdates);
			this.generalTab.Controls.Add(this.groupBoxConsole);
			this.generalTab.Location = new System.Drawing.Point(4, 22);
			this.generalTab.Name = "generalTab";
			this.generalTab.Padding = new System.Windows.Forms.Padding(3);
			this.generalTab.Size = new System.Drawing.Size(426, 293);
			this.generalTab.TabIndex = 0;
			this.generalTab.Text = "General";
			this.generalTab.UseVisualStyleBackColor = true;
			// 
			// groupBoxUpdates
			// 
			this.groupBoxUpdates.Controls.Add(this.forceUpdateCheckBox);
			this.groupBoxUpdates.Controls.Add(this.autoUpdateCheckBox);
			this.groupBoxUpdates.Location = new System.Drawing.Point(6, 77);
			this.groupBoxUpdates.Name = "groupBoxUpdates";
			this.groupBoxUpdates.Size = new System.Drawing.Size(414, 65);
			this.groupBoxUpdates.TabIndex = 1;
			this.groupBoxUpdates.TabStop = false;
			this.groupBoxUpdates.Text = "Updates";
			// 
			// forceUpdateCheckBox
			// 
			this.forceUpdateCheckBox.AutoSize = true;
			this.forceUpdateCheckBox.Location = new System.Drawing.Point(6, 42);
			this.forceUpdateCheckBox.Name = "forceUpdateCheckBox";
			this.forceUpdateCheckBox.Size = new System.Drawing.Size(95, 17);
			this.forceUpdateCheckBox.TabIndex = 1;
			this.forceUpdateCheckBox.Text = "Force update?";
			this.forceUpdateCheckBox.UseVisualStyleBackColor = true;
			// 
			// autoUpdateCheckBox
			// 
			this.autoUpdateCheckBox.AutoSize = true;
			this.autoUpdateCheckBox.Location = new System.Drawing.Point(6, 19);
			this.autoUpdateCheckBox.Name = "autoUpdateCheckBox";
			this.autoUpdateCheckBox.Size = new System.Drawing.Size(214, 17);
			this.autoUpdateCheckBox.TabIndex = 0;
			this.autoUpdateCheckBox.Text = "Chech for updates when MultiMC starts.";
			this.autoUpdateCheckBox.UseVisualStyleBackColor = true;
			// 
			// groupBoxConsole
			// 
			this.groupBoxConsole.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBoxConsole.Controls.Add(this.checkBoxAutoClose);
			this.groupBoxConsole.Controls.Add(this.checkBoxShowConsole);
			this.groupBoxConsole.Location = new System.Drawing.Point(6, 6);
			this.groupBoxConsole.Name = "groupBoxConsole";
			this.groupBoxConsole.Size = new System.Drawing.Size(414, 65);
			this.groupBoxConsole.TabIndex = 0;
			this.groupBoxConsole.TabStop = false;
			this.groupBoxConsole.Text = "Console";
			// 
			// checkBoxAutoClose
			// 
			this.checkBoxAutoClose.AutoSize = true;
			this.checkBoxAutoClose.Location = new System.Drawing.Point(6, 42);
			this.checkBoxAutoClose.Name = "checkBoxAutoClose";
			this.checkBoxAutoClose.Size = new System.Drawing.Size(260, 17);
			this.checkBoxAutoClose.TabIndex = 1;
			this.checkBoxAutoClose.Text = "Automatically close console when the game quits.";
			this.checkBoxAutoClose.UseVisualStyleBackColor = true;
			// 
			// checkBoxShowConsole
			// 
			this.checkBoxShowConsole.AutoSize = true;
			this.checkBoxShowConsole.Location = new System.Drawing.Point(6, 19);
			this.checkBoxShowConsole.Name = "checkBoxShowConsole";
			this.checkBoxShowConsole.Size = new System.Drawing.Size(218, 17);
			this.checkBoxShowConsole.TabIndex = 0;
			this.checkBoxShowConsole.Text = "Show console while the game is running.";
			this.checkBoxShowConsole.UseVisualStyleBackColor = true;
			// 
			// advancedTab
			// 
			this.advancedTab.Controls.Add(this.groupBoxJava);
			this.advancedTab.Controls.Add(this.groupBoxMemory);
			this.advancedTab.Location = new System.Drawing.Point(4, 22);
			this.advancedTab.Name = "advancedTab";
			this.advancedTab.Padding = new System.Windows.Forms.Padding(3);
			this.advancedTab.Size = new System.Drawing.Size(426, 293);
			this.advancedTab.TabIndex = 1;
			this.advancedTab.Text = "Advanced";
			this.advancedTab.UseVisualStyleBackColor = true;
			// 
			// groupBoxJava
			// 
			this.groupBoxJava.Controls.Add(this.buttonAutoDetect);
			this.groupBoxJava.Controls.Add(this.textBoxJavaPath);
			this.groupBoxJava.Controls.Add(this.labelJPath);
			this.groupBoxJava.Location = new System.Drawing.Point(6, 84);
			this.groupBoxJava.Name = "groupBoxJava";
			this.groupBoxJava.Size = new System.Drawing.Size(414, 48);
			this.groupBoxJava.TabIndex = 1;
			this.groupBoxJava.TabStop = false;
			this.groupBoxJava.Text = "Java";
			// 
			// buttonAutoDetect
			// 
			this.buttonAutoDetect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAutoDetect.Location = new System.Drawing.Point(332, 19);
			this.buttonAutoDetect.Name = "buttonAutoDetect";
			this.buttonAutoDetect.Size = new System.Drawing.Size(75, 23);
			this.buttonAutoDetect.TabIndex = 2;
			this.buttonAutoDetect.Text = "Auto-detect";
			this.buttonAutoDetect.UseVisualStyleBackColor = true;
			// 
			// textBoxJavaPath
			// 
			this.textBoxJavaPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxJavaPath.Location = new System.Drawing.Point(69, 21);
			this.textBoxJavaPath.Name = "textBoxJavaPath";
			this.textBoxJavaPath.Size = new System.Drawing.Size(258, 20);
			this.textBoxJavaPath.TabIndex = 1;
			// 
			// labelJPath
			// 
			this.labelJPath.AutoSize = true;
			this.labelJPath.Location = new System.Drawing.Point(6, 24);
			this.labelJPath.Name = "labelJPath";
			this.labelJPath.Size = new System.Drawing.Size(57, 13);
			this.labelJPath.TabIndex = 0;
			this.labelJPath.Text = "Java path:";
			// 
			// groupBoxMemory
			// 
			this.groupBoxMemory.Controls.Add(this.labelMaxMem);
			this.groupBoxMemory.Controls.Add(this.maxMemAllocSpinner);
			this.groupBoxMemory.Controls.Add(this.minMemAllocSpinner);
			this.groupBoxMemory.Controls.Add(this.label1);
			this.groupBoxMemory.Location = new System.Drawing.Point(6, 7);
			this.groupBoxMemory.Name = "groupBoxMemory";
			this.groupBoxMemory.Size = new System.Drawing.Size(414, 71);
			this.groupBoxMemory.TabIndex = 0;
			this.groupBoxMemory.TabStop = false;
			this.groupBoxMemory.Text = "Memory";
			// 
			// labelMaxMem
			// 
			this.labelMaxMem.AutoSize = true;
			this.labelMaxMem.Location = new System.Drawing.Point(6, 47);
			this.labelMaxMem.Name = "labelMaxMem";
			this.labelMaxMem.Size = new System.Drawing.Size(138, 13);
			this.labelMaxMem.TabIndex = 3;
			this.labelMaxMem.Text = "Maximum memory allocation";
			// 
			// maxMemAllocSpinner
			// 
			this.maxMemAllocSpinner.Location = new System.Drawing.Point(287, 45);
			this.maxMemAllocSpinner.Name = "maxMemAllocSpinner";
			this.maxMemAllocSpinner.Size = new System.Drawing.Size(120, 20);
			this.maxMemAllocSpinner.TabIndex = 2;
			// 
			// minMemAllocSpinner
			// 
			this.minMemAllocSpinner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.minMemAllocSpinner.Location = new System.Drawing.Point(288, 19);
			this.minMemAllocSpinner.Name = "minMemAllocSpinner";
			this.minMemAllocSpinner.Size = new System.Drawing.Size(120, 20);
			this.minMemAllocSpinner.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(135, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Minimum memory allocation";
			// 
			// buttonPanel
			// 
			this.buttonPanel.AutoSize = true;
			this.buttonPanel.Controls.Add(this.okButton);
			this.buttonPanel.Controls.Add(this.button1);
			this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.buttonPanel.Location = new System.Drawing.Point(0, 321);
			this.buttonPanel.Name = "buttonPanel";
			this.buttonPanel.Padding = new System.Windows.Forms.Padding(2);
			this.buttonPanel.Size = new System.Drawing.Size(434, 33);
			this.buttonPanel.TabIndex = 1;
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(352, 5);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 0;
			this.okButton.Text = "&OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(271, 5);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "&Cancel";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(434, 354);
			this.Controls.Add(this.buttonPanel);
			this.Controls.Add(this.settingsTabs);
			this.Name = "SettingsForm";
			this.Text = "Settings";
			this.Title = "Settings";
			this.settingsTabs.ResumeLayout(false);
			this.generalTab.ResumeLayout(false);
			this.groupBoxUpdates.ResumeLayout(false);
			this.groupBoxUpdates.PerformLayout();
			this.groupBoxConsole.ResumeLayout(false);
			this.groupBoxConsole.PerformLayout();
			this.advancedTab.ResumeLayout(false);
			this.groupBoxJava.ResumeLayout(false);
			this.groupBoxJava.PerformLayout();
			this.groupBoxMemory.ResumeLayout(false);
			this.groupBoxMemory.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.maxMemAllocSpinner)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.minMemAllocSpinner)).EndInit();
			this.buttonPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl settingsTabs;
		private System.Windows.Forms.TabPage generalTab;
		private System.Windows.Forms.TabPage advancedTab;
		private System.Windows.Forms.FlowLayoutPanel buttonPanel;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.GroupBox groupBoxConsole;
		private System.Windows.Forms.CheckBox checkBoxShowConsole;
		private System.Windows.Forms.CheckBox checkBoxAutoClose;
		private System.Windows.Forms.GroupBox groupBoxUpdates;
		private System.Windows.Forms.CheckBox autoUpdateCheckBox;
		private System.Windows.Forms.CheckBox forceUpdateCheckBox;
		private System.Windows.Forms.GroupBox groupBoxMemory;
		private System.Windows.Forms.NumericUpDown minMemAllocSpinner;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown maxMemAllocSpinner;
		private System.Windows.Forms.Label labelMaxMem;
		private System.Windows.Forms.GroupBox groupBoxJava;
		private System.Windows.Forms.Label labelJPath;
		private System.Windows.Forms.TextBox textBoxJavaPath;
		private System.Windows.Forms.Button buttonAutoDetect;
		private System.Windows.Forms.Button button1;
	}
}