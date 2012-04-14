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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.instanceContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.changeIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.editModsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rebuildJarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuToolBar = new System.Windows.Forms.ToolStrip();
			this.addInstButton = new System.Windows.Forms.ToolStripButton();
			this.importButton = new System.Windows.Forms.ToolStripButton();
			this.viewInstanceFolder = new System.Windows.Forms.ToolStripButton();
			this.refreshButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.settingsButton = new System.Windows.Forms.ToolStripButton();
			this.checkUpdateButton = new System.Windows.Forms.ToolStripButton();
			this.aboutButton = new System.Windows.Forms.ToolStripButton();
			this.helpButton = new System.Windows.Forms.ToolStripButton();
			this.instView = new System.Windows.Forms.ListView();
			this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.DragDropHintLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.instanceContextMenu.SuspendLayout();
			this.menuToolBar.SuspendLayout();
			this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
			this.toolStripContainer.ContentPanel.SuspendLayout();
			this.toolStripContainer.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// instanceContextMenu
			// 
			this.instanceContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.toolStripSeparator2,
            this.renameToolStripMenuItem,
            this.changeIconToolStripMenuItem,
            this.editNotesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.editModsToolStripMenuItem,
            this.rebuildJarToolStripMenuItem,
            this.viewFolderToolStripMenuItem,
            this.toolStripMenuItem2,
            this.deleteToolStripMenuItem});
			this.instanceContextMenu.Name = "instanceContextMenu";
			this.instanceContextMenu.Size = new System.Drawing.Size(153, 220);
			this.instanceContextMenu.Text = "Instance Menu";
			this.instanceContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.instanceContextMenu_Opening);
			// 
			// playToolStripMenuItem
			// 
			this.playToolStripMenuItem.Name = "playToolStripMenuItem";
			this.playToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.playToolStripMenuItem.Text = "&Play";
			this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
			// 
			// changeIconToolStripMenuItem
			// 
			this.changeIconToolStripMenuItem.Name = "changeIconToolStripMenuItem";
			this.changeIconToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.changeIconToolStripMenuItem.Text = "Change &Icon";
			this.changeIconToolStripMenuItem.Click += new System.EventHandler(this.changeIconToolStripMenuItem_Click);
			// 
			// editNotesToolStripMenuItem
			// 
			this.editNotesToolStripMenuItem.Name = "editNotesToolStripMenuItem";
			this.editNotesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.editNotesToolStripMenuItem.Text = "Edit &Notes";
			this.editNotesToolStripMenuItem.Click += new System.EventHandler(this.editNotesToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
			// 
			// editModsToolStripMenuItem
			// 
			this.editModsToolStripMenuItem.Name = "editModsToolStripMenuItem";
			this.editModsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.editModsToolStripMenuItem.Text = "Edit &Mods";
			this.editModsToolStripMenuItem.Click += new System.EventHandler(this.editModsToolStripMenuItem_Click);
			// 
			// rebuildJarToolStripMenuItem
			// 
			this.rebuildJarToolStripMenuItem.Name = "rebuildJarToolStripMenuItem";
			this.rebuildJarToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.rebuildJarToolStripMenuItem.Text = "Rebuild &Jar";
			this.rebuildJarToolStripMenuItem.Click += new System.EventHandler(this.rebuildJarToolStripMenuItem_Click);
			// 
			// viewFolderToolStripMenuItem
			// 
			this.viewFolderToolStripMenuItem.Name = "viewFolderToolStripMenuItem";
			this.viewFolderToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.viewFolderToolStripMenuItem.Text = "View &Folder";
			this.viewFolderToolStripMenuItem.Click += new System.EventHandler(this.viewFolderToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.deleteToolStripMenuItem.Text = "&Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			// 
			// menuToolBar
			// 
			this.menuToolBar.AutoSize = false;
			this.menuToolBar.Dock = System.Windows.Forms.DockStyle.None;
			this.menuToolBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.menuToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addInstButton,
            this.importButton,
            this.viewInstanceFolder,
            this.refreshButton,
            this.toolStripSeparator1,
            this.settingsButton,
            this.checkUpdateButton,
            this.aboutButton,
            this.helpButton});
			this.menuToolBar.Location = new System.Drawing.Point(0, 0);
			this.menuToolBar.Name = "menuToolBar";
			this.menuToolBar.Size = new System.Drawing.Size(604, 25);
			this.menuToolBar.Stretch = true;
			this.menuToolBar.TabIndex = 2;
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
			// importButton
			// 
			this.importButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.importButton.Image = global::MultiMC.Properties.Resources.document_import;
			this.importButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.importButton.Name = "importButton";
			this.importButton.Size = new System.Drawing.Size(23, 22);
			this.importButton.Text = "Import existing minecraft folder.";
			this.importButton.Visible = false;
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
			// aboutButton
			// 
			this.aboutButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.aboutButton.Image = global::MultiMC.Properties.Resources.AboutIcon;
			this.aboutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.aboutButton.Name = "aboutButton";
			this.aboutButton.Size = new System.Drawing.Size(60, 22);
			this.aboutButton.Text = "About";
			// 
			// helpButton
			// 
			this.helpButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.helpButton.Enabled = false;
			this.helpButton.Image = global::MultiMC.Properties.Resources.HelpIcon;
			this.helpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.helpButton.Name = "helpButton";
			this.helpButton.Size = new System.Drawing.Size(52, 22);
			this.helpButton.Text = "Help";
			// 
			// instView
			// 
			this.instView.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
			this.instView.AllowDrop = true;
			this.instView.ContextMenuStrip = this.instanceContextMenu;
			this.instView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.instView.LabelEdit = true;
			this.instView.Location = new System.Drawing.Point(0, 0);
			this.instView.MultiSelect = false;
			this.instView.Name = "instView";
			this.instView.Size = new System.Drawing.Size(604, 314);
			this.instView.TabIndex = 1;
			this.instView.UseCompatibleStateImageBehavior = false;
			this.instView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.instView_AfterLabelEdit);
			this.instView.ItemActivate += new System.EventHandler(this.instView_ItemActivate);
			this.instView.DragDrop += new System.Windows.Forms.DragEventHandler(this.instView_DragDrop);
			this.instView.DragOver += new System.Windows.Forms.DragEventHandler(this.instView_DragOver);
			this.instView.DragLeave += new System.EventHandler(this.instView_DragLeave);
			this.instView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.instView_KeyDown);
			// 
			// toolStripContainer
			// 
			// 
			// toolStripContainer.BottomToolStripPanel
			// 
			this.toolStripContainer.BottomToolStripPanel.Controls.Add(this.statusStrip1);
			// 
			// toolStripContainer.ContentPanel
			// 
			this.toolStripContainer.ContentPanel.AutoScroll = true;
			this.toolStripContainer.ContentPanel.Controls.Add(this.instView);
			this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(604, 314);
			this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer.Name = "toolStripContainer";
			this.toolStripContainer.Size = new System.Drawing.Size(604, 361);
			this.toolStripContainer.TabIndex = 3;
			this.toolStripContainer.Text = "toolStripContainer1";
			// 
			// toolStripContainer.TopToolStripPanel
			// 
			this.toolStripContainer.TopToolStripPanel.Controls.Add(this.menuToolBar);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DragDropHintLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 0);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(604, 22);
			this.statusStrip1.TabIndex = 5;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// DragDropHintLabel
			// 
			this.DragDropHintLabel.Name = "DragDropHintLabel";
			this.DragDropHintLabel.Size = new System.Drawing.Size(87, 17);
			this.DragDropHintLabel.Text = "Drag Drop Hint";
			this.DragDropHintLabel.Visible = false;
			// 
			// renameToolStripMenuItem
			// 
			this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
			this.renameToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.renameToolStripMenuItem.Text = "&Rename";
			this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(604, 361);
			this.Controls.Add(this.toolStripContainer);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Text = "MultiMC";
			this.Title = "MultiMC";
			this.instanceContextMenu.ResumeLayout(false);
			this.menuToolBar.ResumeLayout(false);
			this.menuToolBar.PerformLayout();
			this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
			this.toolStripContainer.BottomToolStripPanel.PerformLayout();
			this.toolStripContainer.ContentPanel.ResumeLayout(false);
			this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer.ResumeLayout(false);
			this.toolStripContainer.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip instanceContextMenu;
		private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem changeIconToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editNotesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem editModsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rebuildJarToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewFolderToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStrip menuToolBar;
		private System.Windows.Forms.ToolStripButton addInstButton;
		private System.Windows.Forms.ToolStripButton viewInstanceFolder;
		private System.Windows.Forms.ToolStripButton refreshButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton settingsButton;
		private System.Windows.Forms.ToolStripButton checkUpdateButton;
		private System.Windows.Forms.ToolStripButton aboutButton;
		private System.Windows.Forms.ToolStripButton helpButton;
		private System.Windows.Forms.ListView instView;
		private System.Windows.Forms.ToolStripContainer toolStripContainer;
		private System.Windows.Forms.ToolStripButton importButton;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel DragDropHintLabel;
		private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
	}
}

