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
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelHelp = new System.Windows.Forms.Label();
			this.modTabControl = new System.Windows.Forms.TabControl();
			this.tabPageJar = new System.Windows.Forms.TabPage();
			this.modView = new System.Windows.Forms.ListView();
			this.columnModName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tabPageMLMods = new System.Windows.Forms.TabPage();
			this.mlModView = new System.Windows.Forms.ListView();
			this.modColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.flowLayoutPanel1.SuspendLayout();
			this.modTabControl.SuspendLayout();
			this.tabPageJar.SuspendLayout();
			this.tabPageMLMods.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.buttonOk);
			this.flowLayoutPanel1.Controls.Add(this.buttonCancel);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 379);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(2);
			this.flowLayoutPanel1.Size = new System.Drawing.Size(584, 33);
			this.flowLayoutPanel1.TabIndex = 1;
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(502, 5);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 0;
			this.buttonOk.Text = "&OK";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(421, 5);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// labelHelp
			// 
			this.labelHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelHelp.Location = new System.Drawing.Point(12, 9);
			this.labelHelp.Name = "labelHelp";
			this.labelHelp.Size = new System.Drawing.Size(560, 41);
			this.labelHelp.TabIndex = 2;
			this.labelHelp.Text = "Uncheck the box next to a mod to uninstall it. \r\nYou can drag mods up or down in " +
				"the list to change the order in which they will be installed.\r\nDrag mods into th" +
				"is window to install them.";
			// 
			// modTabControl
			// 
			this.modTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.modTabControl.Controls.Add(this.tabPageJar);
			this.modTabControl.Controls.Add(this.tabPageMLMods);
			this.modTabControl.Location = new System.Drawing.Point(12, 53);
			this.modTabControl.Name = "modTabControl";
			this.modTabControl.SelectedIndex = 0;
			this.modTabControl.Size = new System.Drawing.Size(560, 320);
			this.modTabControl.TabIndex = 3;
			// 
			// tabPageJar
			// 
			this.tabPageJar.Controls.Add(this.modView);
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
			this.modView.CheckBoxes = true;
			this.modView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnModName});
			this.modView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.modView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.modView.Location = new System.Drawing.Point(3, 3);
			this.modView.Name = "modView";
			this.modView.Size = new System.Drawing.Size(546, 288);
			this.modView.TabIndex = 1;
			this.modView.UseCompatibleStateImageBehavior = false;
			this.modView.View = System.Windows.Forms.View.Details;
			this.modView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.modView_ItemDrag);
			this.modView.DragDrop += new System.Windows.Forms.DragEventHandler(this.modView_DragDrop);
			this.modView.DragEnter += new System.Windows.Forms.DragEventHandler(this.modView_DragEnter);
			this.modView.DragOver += new System.Windows.Forms.DragEventHandler(this.modView_DragOver);
			this.modView.DragLeave += new System.EventHandler(this.modView_DragLeave);
			this.modView.Resize += new System.EventHandler(this.modView_Resize);
			// 
			// columnModName
			// 
			this.columnModName.Text = "Mod Name";
			this.columnModName.Width = 440;
			// 
			// tabPageMLMods
			// 
			this.tabPageMLMods.Controls.Add(this.mlModView);
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
			this.mlModView.CheckBoxes = true;
			this.mlModView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.modColumn});
			this.mlModView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mlModView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.mlModView.Location = new System.Drawing.Point(3, 3);
			this.mlModView.Name = "mlModView";
			this.mlModView.Size = new System.Drawing.Size(546, 288);
			this.mlModView.TabIndex = 2;
			this.mlModView.UseCompatibleStateImageBehavior = false;
			this.mlModView.View = System.Windows.Forms.View.Details;
			this.mlModView.DragDrop += new System.Windows.Forms.DragEventHandler(this.mlModView_DragDrop);
			this.mlModView.DragOver += new System.Windows.Forms.DragEventHandler(this.mlModView_DragOver);
			this.mlModView.Resize += new System.EventHandler(this.mlModView_Resize);
			// 
			// modColumn
			// 
			this.modColumn.Text = "Mod Name";
			this.modColumn.Width = 440;
			// 
			// EditModsForm
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(584, 412);
			this.Controls.Add(this.modTabControl);
			this.Controls.Add(this.labelHelp);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "EditModsForm";
			this.Text = "Edit mods";
			this.Title = "Edit mods";
			this.flowLayoutPanel1.ResumeLayout(false);
			this.modTabControl.ResumeLayout(false);
			this.tabPageJar.ResumeLayout(false);
			this.tabPageMLMods.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labelHelp;
		private System.Windows.Forms.TabControl modTabControl;
		private System.Windows.Forms.TabPage tabPageJar;
		private System.Windows.Forms.ListView modView;
		private System.Windows.Forms.ColumnHeader columnModName;
		private System.Windows.Forms.TabPage tabPageMLMods;
		private System.Windows.Forms.ListView mlModView;
		private System.Windows.Forms.ColumnHeader modColumn;
	}
}