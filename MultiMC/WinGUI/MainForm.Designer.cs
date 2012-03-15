namespace MultiMC.WinGUI
{
	partial class MainForm
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
			this.menuToolBar = new System.Windows.Forms.ToolStrip();
			this.addInstButton = new System.Windows.Forms.ToolStripButton();
			this.viewInstanceFolder = new System.Windows.Forms.ToolStripButton();
			this.refreshButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.settingsButton = new System.Windows.Forms.ToolStripButton();
			this.checkUpdateButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.helpButton = new System.Windows.Forms.ToolStripButton();
			this.aboutButton = new System.Windows.Forms.ToolStripButton();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.instView = new System.Windows.Forms.ListView();
			this.menuToolBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuToolBar
			// 
			this.menuToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.menuToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addInstButton,
            this.viewInstanceFolder,
            this.refreshButton,
            this.toolStripSeparator1,
            this.settingsButton,
            this.checkUpdateButton,
            this.toolStripSeparator2,
            this.helpButton,
            this.aboutButton});
			this.menuToolBar.Location = new System.Drawing.Point(0, 0);
			this.menuToolBar.Name = "menuToolBar";
			this.menuToolBar.Size = new System.Drawing.Size(604, 25);
			this.menuToolBar.TabIndex = 1;
			this.menuToolBar.Text = "Menu";
			// 
			// addInstButton
			// 
			this.addInstButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.addInstButton.Image = global::MultiMC.Properties.Resources.NewInstIcon;
			this.addInstButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.addInstButton.Name = "addInstButton";
			this.addInstButton.Size = new System.Drawing.Size(23, 22);
			this.addInstButton.Text = "Add a new instance";
			// 
			// viewInstanceFolder
			// 
			this.viewInstanceFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.viewInstanceFolder.Image = global::MultiMC.Properties.Resources.ViewFolderIcon;
			this.viewInstanceFolder.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.viewInstanceFolder.Name = "viewInstanceFolder";
			this.viewInstanceFolder.Size = new System.Drawing.Size(23, 22);
			this.viewInstanceFolder.Text = "View instance folder";
			// 
			// refreshButton
			// 
			this.refreshButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.refreshButton.Image = global::MultiMC.Properties.Resources.RefreshInstIcon;
			this.refreshButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.refreshButton.Name = "refreshButton";
			this.refreshButton.Size = new System.Drawing.Size(23, 22);
			this.refreshButton.Text = "Refresh";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// settingsButton
			// 
			this.settingsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.settingsButton.Image = global::MultiMC.Properties.Resources.SettingsIcon;
			this.settingsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.settingsButton.Name = "settingsButton";
			this.settingsButton.Size = new System.Drawing.Size(23, 22);
			this.settingsButton.Text = "Settings";
			// 
			// checkUpdateButton
			// 
			this.checkUpdateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.checkUpdateButton.Image = global::MultiMC.Properties.Resources.CheckUpdateIcon;
			this.checkUpdateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.checkUpdateButton.Name = "checkUpdateButton";
			this.checkUpdateButton.Size = new System.Drawing.Size(23, 22);
			this.checkUpdateButton.Text = "Check for updates...";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// helpButton
			// 
			this.helpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.helpButton.Image = global::MultiMC.Properties.Resources.HelpIcon;
			this.helpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.helpButton.Name = "helpButton";
			this.helpButton.Size = new System.Drawing.Size(23, 22);
			this.helpButton.Text = "Help";
			// 
			// aboutButton
			// 
			this.aboutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.aboutButton.Image = global::MultiMC.Properties.Resources.AboutIcon;
			this.aboutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.aboutButton.Name = "aboutButton";
			this.aboutButton.Size = new System.Drawing.Size(23, 22);
			this.aboutButton.Text = "About";
			// 
			// statusStrip
			// 
			this.statusStrip.Location = new System.Drawing.Point(0, 339);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(604, 22);
			this.statusStrip.TabIndex = 2;
			this.statusStrip.Text = "Status";
			// 
			// instView
			// 
			this.instView.AllowDrop = true;
			this.instView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.instView.Location = new System.Drawing.Point(0, 28);
			this.instView.MultiSelect = false;
			this.instView.Name = "instView";
			this.instView.Size = new System.Drawing.Size(604, 308);
			this.instView.TabIndex = 0;
			this.instView.UseCompatibleStateImageBehavior = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(604, 361);
			this.Controls.Add(this.instView);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.menuToolBar);
			this.Name = "MainForm";
			this.Text = "MultiMC";
			this.Title = "MultiMC";
			this.menuToolBar.ResumeLayout(false);
			this.menuToolBar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip menuToolBar;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ListView instView;
		private System.Windows.Forms.ToolStripButton addInstButton;
		private System.Windows.Forms.ToolStripButton viewInstanceFolder;
		private System.Windows.Forms.ToolStripButton refreshButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton settingsButton;
		private System.Windows.Forms.ToolStripButton checkUpdateButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton helpButton;
		private System.Windows.Forms.ToolStripButton aboutButton;
	}
}

