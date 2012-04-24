namespace MultiMC.WinGUI
{
	partial class EditModsForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditModsForm));
			System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Test Node 1");
			System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Child Node 1");
			System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Child Node 2");
			System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Test Node 2", new System.Windows.Forms.TreeNode[] {
            treeNode22,
            treeNode23});
			System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Root", new System.Windows.Forms.TreeNode[] {
            treeNode21,
            treeNode24});
			System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Test Node 1");
			System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Child Node 1");
			System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Child Node 2");
			System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Test Node 2", new System.Windows.Forms.TreeNode[] {
            treeNode27,
            treeNode28});
			System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("Root", new System.Windows.Forms.TreeNode[] {
            treeNode26,
            treeNode29});
			this.labelHelp = new System.Windows.Forms.Label();
			this.modTabControl = new System.Windows.Forms.TabControl();
			this.tabPageJar = new System.Windows.Forms.TabPage();
			this.modView = new System.Windows.Forms.ListView();
			this.columnModName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnMCVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.jarModButtonPanel = new System.Windows.Forms.TableLayoutPanel();
			this.buttonAddJarMod = new System.Windows.Forms.Button();
			this.buttonRemoveJarMod = new System.Windows.Forms.Button();
			this.buttonMoveUp = new System.Windows.Forms.Button();
			this.buttonMoveDown = new System.Windows.Forms.Button();
			this.tabPageMLMods = new System.Windows.Forms.TabPage();
			this.mlModView = new System.Windows.Forms.ListView();
			this.modColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.buttonAddMLMod = new System.Windows.Forms.Button();
			this.buttonRemoveMLMod = new System.Windows.Forms.Button();
			this.tabPageResources = new System.Windows.Forms.TabPage();
			this.resourceView = new System.Windows.Forms.TreeView();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.buttonExport = new System.Windows.Forms.Button();
			this.rButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonModsFolder = new System.Windows.Forms.Button();
			this.modTabControl.SuspendLayout();
			this.tabPageJar.SuspendLayout();
			this.jarModButtonPanel.SuspendLayout();
			this.tabPageMLMods.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tabPageResources.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.rButtonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// labelHelp
			// 
			this.labelHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelHelp.Location = new System.Drawing.Point(12, 9);
			this.labelHelp.Name = "labelHelp";
			this.labelHelp.Size = new System.Drawing.Size(560, 41);
			this.labelHelp.TabIndex = 2;
			this.labelHelp.Text = resources.GetString("labelHelp.Text");
			// 
			// modTabControl
			// 
			this.modTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.modTabControl.Controls.Add(this.tabPageJar);
			this.modTabControl.Controls.Add(this.tabPageMLMods);
			this.modTabControl.Controls.Add(this.tabPageResources);
			this.modTabControl.Location = new System.Drawing.Point(12, 53);
			this.modTabControl.Name = "modTabControl";
			this.modTabControl.SelectedIndex = 0;
			this.modTabControl.Size = new System.Drawing.Size(560, 320);
			this.modTabControl.TabIndex = 3;
			// 
			// tabPageJar
			// 
			this.tabPageJar.Controls.Add(this.modView);
			this.tabPageJar.Controls.Add(this.jarModButtonPanel);
			this.tabPageJar.Location = new System.Drawing.Point(4, 22);
			this.tabPageJar.Name = "tabPageJar";
			this.tabPageJar.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageJar.Size = new System.Drawing.Size(552, 294);
			this.tabPageJar.TabIndex = 0;
			this.tabPageJar.Text = "minecraft.jar";
			this.tabPageJar.UseVisualStyleBackColor = true;
			// 
			// modView
			// 
			this.modView.AllowDrop = true;
			this.modView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnModName,
            this.columnMCVersion});
			this.modView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.modView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.modView.Location = new System.Drawing.Point(3, 3);
			this.modView.Name = "modView";
			this.modView.Size = new System.Drawing.Size(466, 288);
			this.modView.TabIndex = 3;
			this.modView.UseCompatibleStateImageBehavior = false;
			this.modView.View = System.Windows.Forms.View.Details;
			this.modView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.modView_ItemDrag);
			this.modView.DragDrop += new System.Windows.Forms.DragEventHandler(this.modView_DragDrop);
			this.modView.DragEnter += new System.Windows.Forms.DragEventHandler(this.modView_DragEnter);
			this.modView.DragOver += new System.Windows.Forms.DragEventHandler(this.modView_DragOver);
			this.modView.DragLeave += new System.EventHandler(this.modView_DragLeave);
			this.modView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.modView_KeyDown);
			this.modView.Resize += new System.EventHandler(this.modView_Resize);
			// 
			// columnModName
			// 
			this.columnModName.Text = "Mod Name";
			this.columnModName.Width = 440;
			// 
			// columnMCVersion
			// 
			this.columnMCVersion.Text = "MC Version";
			// 
			// jarModButtonPanel
			// 
			this.jarModButtonPanel.ColumnCount = 1;
			this.jarModButtonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.jarModButtonPanel.Controls.Add(this.buttonAddJarMod, 0, 0);
			this.jarModButtonPanel.Controls.Add(this.buttonRemoveJarMod, 0, 1);
			this.jarModButtonPanel.Controls.Add(this.buttonMoveUp, 0, 3);
			this.jarModButtonPanel.Controls.Add(this.buttonMoveDown, 0, 4);
			this.jarModButtonPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.jarModButtonPanel.Location = new System.Drawing.Point(469, 3);
			this.jarModButtonPanel.Name = "jarModButtonPanel";
			this.jarModButtonPanel.RowCount = 5;
			this.jarModButtonPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.jarModButtonPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.jarModButtonPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.jarModButtonPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.jarModButtonPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.jarModButtonPanel.Size = new System.Drawing.Size(80, 288);
			this.jarModButtonPanel.TabIndex = 2;
			// 
			// buttonAddJarMod
			// 
			this.buttonAddJarMod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAddJarMod.AutoSize = true;
			this.buttonAddJarMod.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonAddJarMod.Location = new System.Drawing.Point(3, 3);
			this.buttonAddJarMod.Name = "buttonAddJarMod";
			this.buttonAddJarMod.Size = new System.Drawing.Size(74, 23);
			this.buttonAddJarMod.TabIndex = 0;
			this.buttonAddJarMod.Text = "&Add";
			this.buttonAddJarMod.UseVisualStyleBackColor = true;
			this.buttonAddJarMod.Click += new System.EventHandler(this.buttonAddJarMod_Click);
			// 
			// buttonRemoveJarMod
			// 
			this.buttonRemoveJarMod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRemoveJarMod.AutoSize = true;
			this.buttonRemoveJarMod.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonRemoveJarMod.Location = new System.Drawing.Point(3, 32);
			this.buttonRemoveJarMod.Name = "buttonRemoveJarMod";
			this.buttonRemoveJarMod.Size = new System.Drawing.Size(74, 23);
			this.buttonRemoveJarMod.TabIndex = 1;
			this.buttonRemoveJarMod.Text = "&Remove";
			this.buttonRemoveJarMod.UseVisualStyleBackColor = true;
			this.buttonRemoveJarMod.Click += new System.EventHandler(this.buttonRemoveJarMod_Click);
			// 
			// buttonMoveUp
			// 
			this.buttonMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.buttonMoveUp.AutoSize = true;
			this.buttonMoveUp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonMoveUp.Location = new System.Drawing.Point(3, 233);
			this.buttonMoveUp.Name = "buttonMoveUp";
			this.buttonMoveUp.Size = new System.Drawing.Size(74, 23);
			this.buttonMoveUp.TabIndex = 2;
			this.buttonMoveUp.Text = "Move &Up";
			this.buttonMoveUp.UseVisualStyleBackColor = true;
			this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
			// 
			// buttonMoveDown
			// 
			this.buttonMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.buttonMoveDown.AutoSize = true;
			this.buttonMoveDown.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonMoveDown.Location = new System.Drawing.Point(3, 262);
			this.buttonMoveDown.Name = "buttonMoveDown";
			this.buttonMoveDown.Size = new System.Drawing.Size(74, 23);
			this.buttonMoveDown.TabIndex = 3;
			this.buttonMoveDown.Text = "Move &Down";
			this.buttonMoveDown.UseVisualStyleBackColor = true;
			this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
			// 
			// tabPageMLMods
			// 
			this.tabPageMLMods.Controls.Add(this.mlModView);
			this.tabPageMLMods.Controls.Add(this.tableLayoutPanel2);
			this.tabPageMLMods.Location = new System.Drawing.Point(4, 22);
			this.tabPageMLMods.Name = "tabPageMLMods";
			this.tabPageMLMods.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageMLMods.Size = new System.Drawing.Size(552, 294);
			this.tabPageMLMods.TabIndex = 1;
			this.tabPageMLMods.Text = "minecraft/mods";
			this.tabPageMLMods.UseVisualStyleBackColor = true;
			// 
			// mlModView
			// 
			this.mlModView.AllowDrop = true;
			this.mlModView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.modColumn});
			this.mlModView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mlModView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.mlModView.Location = new System.Drawing.Point(3, 3);
			this.mlModView.Name = "mlModView";
			this.mlModView.Size = new System.Drawing.Size(466, 288);
			this.mlModView.TabIndex = 4;
			this.mlModView.UseCompatibleStateImageBehavior = false;
			this.mlModView.View = System.Windows.Forms.View.Details;
			this.mlModView.DragDrop += new System.Windows.Forms.DragEventHandler(this.mlModView_DragDrop);
			this.mlModView.DragOver += new System.Windows.Forms.DragEventHandler(this.mlModView_DragOver);
			this.mlModView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mlModView_KeyDown);
			this.mlModView.Resize += new System.EventHandler(this.mlModView_Resize);
			// 
			// modColumn
			// 
			this.modColumn.Text = "Mod Name";
			this.modColumn.Width = 440;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.buttonAddMLMod, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.buttonRemoveMLMod, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(469, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(80, 288);
			this.tableLayoutPanel2.TabIndex = 3;
			// 
			// buttonAddMLMod
			// 
			this.buttonAddMLMod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.buttonAddMLMod.AutoSize = true;
			this.buttonAddMLMod.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonAddMLMod.Location = new System.Drawing.Point(3, 3);
			this.buttonAddMLMod.Name = "buttonAddMLMod";
			this.buttonAddMLMod.Size = new System.Drawing.Size(74, 23);
			this.buttonAddMLMod.TabIndex = 0;
			this.buttonAddMLMod.Text = "&Add";
			this.buttonAddMLMod.UseVisualStyleBackColor = true;
			this.buttonAddMLMod.Click += new System.EventHandler(this.buttonAddMLMod_Click);
			// 
			// buttonRemoveMLMod
			// 
			this.buttonRemoveMLMod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRemoveMLMod.AutoSize = true;
			this.buttonRemoveMLMod.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonRemoveMLMod.Location = new System.Drawing.Point(3, 32);
			this.buttonRemoveMLMod.Name = "buttonRemoveMLMod";
			this.buttonRemoveMLMod.Size = new System.Drawing.Size(74, 23);
			this.buttonRemoveMLMod.TabIndex = 1;
			this.buttonRemoveMLMod.Text = "&Remove";
			this.buttonRemoveMLMod.UseVisualStyleBackColor = true;
			this.buttonRemoveMLMod.Click += new System.EventHandler(this.buttonRemoveMLMod_Click);
			// 
			// tabPageResources
			// 
			this.tabPageResources.Controls.Add(this.resourceView);
			this.tabPageResources.Location = new System.Drawing.Point(4, 22);
			this.tabPageResources.Name = "tabPageResources";
			this.tabPageResources.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageResources.Size = new System.Drawing.Size(552, 294);
			this.tabPageResources.TabIndex = 2;
			this.tabPageResources.Text = "minecraft/resources";
			this.tabPageResources.UseVisualStyleBackColor = true;
			// 
			// resourceView
			// 
			this.resourceView.AllowDrop = true;
			this.resourceView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.resourceView.HideSelection = false;
			this.resourceView.HotTracking = true;
			this.resourceView.Location = new System.Drawing.Point(3, 3);
			this.resourceView.Name = "resourceView";
			treeNode21.Name = "TNode1";
			treeNode21.Text = "Test Node 1";
			treeNode21.ToolTipText = "Test";
			treeNode22.Name = "TN2Child1";
			treeNode22.Text = "Child Node 1";
			treeNode23.Name = "TN2Child2";
			treeNode23.Text = "Child Node 2";
			treeNode24.Name = "TNode2";
			treeNode24.Text = "Test Node 2";
			treeNode25.Name = "TestNode";
			treeNode25.Text = "Root";
			treeNode25.ToolTipText = "Test";
			treeNode26.Name = "TNode1";
			treeNode26.Text = "Test Node 1";
			treeNode26.ToolTipText = "Test";
			treeNode27.Name = "TN2Child1";
			treeNode27.Text = "Child Node 1";
			treeNode28.Name = "TN2Child2";
			treeNode28.Text = "Child Node 2";
			treeNode29.Name = "TNode2";
			treeNode29.Text = "Test Node 2";
			treeNode30.Name = "TestNode";
			treeNode30.Text = "Root";
			treeNode30.ToolTipText = "Test";
			this.resourceView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode25,
            treeNode30});
			this.resourceView.PathSeparator = "/";
			this.resourceView.ShowLines = false;
			this.resourceView.ShowNodeToolTips = true;
			this.resourceView.Size = new System.Drawing.Size(546, 288);
			this.resourceView.TabIndex = 0;
			this.resourceView.DragDrop += new System.Windows.Forms.DragEventHandler(this.resourceView_DragDrop);
			this.resourceView.DragEnter += new System.Windows.Forms.DragEventHandler(this.resourceView_DragEnter);
			this.resourceView.DragOver += new System.Windows.Forms.DragEventHandler(this.resourceView_DragOver);
			this.resourceView.DragLeave += new System.EventHandler(this.resourceView_DragLeave);
			this.resourceView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.resourceView_KeyDown);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.rButtonPanel, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 379);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 33);
			this.tableLayoutPanel1.TabIndex = 4;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.buttonExport);
			this.flowLayoutPanel1.Controls.Add(this.buttonModsFolder);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(2);
			this.flowLayoutPanel1.Size = new System.Drawing.Size(292, 33);
			this.flowLayoutPanel1.TabIndex = 3;
			// 
			// buttonExport
			// 
			this.buttonExport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonExport.Enabled = false;
			this.buttonExport.Location = new System.Drawing.Point(5, 5);
			this.buttonExport.Name = "buttonExport";
			this.buttonExport.Size = new System.Drawing.Size(75, 23);
			this.buttonExport.TabIndex = 1;
			this.buttonExport.Text = "&Export";
			this.buttonExport.UseVisualStyleBackColor = true;
			this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
			// 
			// rButtonPanel
			// 
			this.rButtonPanel.AutoSize = true;
			this.rButtonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.rButtonPanel.Controls.Add(this.buttonOk);
			this.rButtonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.rButtonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.rButtonPanel.Location = new System.Drawing.Point(292, 0);
			this.rButtonPanel.Margin = new System.Windows.Forms.Padding(0);
			this.rButtonPanel.Name = "rButtonPanel";
			this.rButtonPanel.Padding = new System.Windows.Forms.Padding(2);
			this.rButtonPanel.Size = new System.Drawing.Size(292, 33);
			this.rButtonPanel.TabIndex = 2;
			// 
			// buttonOk
			// 
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonOk.Location = new System.Drawing.Point(210, 5);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 0;
			this.buttonOk.Text = "&Close";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonModsFolder
			// 
			this.buttonModsFolder.AutoSize = true;
			this.buttonModsFolder.Location = new System.Drawing.Point(86, 5);
			this.buttonModsFolder.Name = "buttonModsFolder";
			this.buttonModsFolder.Size = new System.Drawing.Size(111, 23);
			this.buttonModsFolder.TabIndex = 2;
			this.buttonModsFolder.Text = "Central &Mods Folder";
			this.buttonModsFolder.UseVisualStyleBackColor = true;
			this.buttonModsFolder.Click += new System.EventHandler(this.buttonModsFolder_Click);
			// 
			// EditModsForm
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 412);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.modTabControl);
			this.Controls.Add(this.labelHelp);
			this.DefaultPosition = MultiMC.GUI.DefWindowPosition.CenterParent;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EditModsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Edit mods";
			this.Title = "Edit mods";
			this.modTabControl.ResumeLayout(false);
			this.tabPageJar.ResumeLayout(false);
			this.jarModButtonPanel.ResumeLayout(false);
			this.jarModButtonPanel.PerformLayout();
			this.tabPageMLMods.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tabPageResources.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.flowLayoutPanel1.PerformLayout();
			this.rButtonPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label labelHelp;
		private System.Windows.Forms.TabControl modTabControl;
		private System.Windows.Forms.TabPage tabPageJar;
		private System.Windows.Forms.TabPage tabPageMLMods;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.FlowLayoutPanel rButtonPanel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button buttonExport;
		private System.Windows.Forms.TabPage tabPageResources;
		private System.Windows.Forms.TreeView resourceView;
		private System.Windows.Forms.TableLayoutPanel jarModButtonPanel;
		private System.Windows.Forms.ListView modView;
		private System.Windows.Forms.ColumnHeader columnModName;
		private System.Windows.Forms.ColumnHeader columnMCVersion;
		private System.Windows.Forms.Button buttonAddJarMod;
		private System.Windows.Forms.Button buttonRemoveJarMod;
		private System.Windows.Forms.Button buttonMoveUp;
		private System.Windows.Forms.Button buttonMoveDown;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button buttonAddMLMod;
		private System.Windows.Forms.Button buttonRemoveMLMod;
		private System.Windows.Forms.ListView mlModView;
		private System.Windows.Forms.ColumnHeader modColumn;
		private System.Windows.Forms.Button buttonModsFolder;
	}
}