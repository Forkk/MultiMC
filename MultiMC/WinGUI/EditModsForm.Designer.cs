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
			this.modView = new System.Windows.Forms.ListView();
			this.columnModName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelHelp = new System.Windows.Forms.Label();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// modView
			// 
			this.modView.AllowDrop = true;
			this.modView.CheckBoxes = true;
			this.modView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnModName});
			this.modView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.modView.Location = new System.Drawing.Point(0, 53);
			this.modView.Name = "modView";
			this.modView.Size = new System.Drawing.Size(484, 270);
			this.modView.TabIndex = 0;
			this.modView.UseCompatibleStateImageBehavior = false;
			this.modView.View = System.Windows.Forms.View.Details;
			this.modView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.modView_ItemDrag);
			this.modView.DragDrop += new System.Windows.Forms.DragEventHandler(this.modView_DragDrop);
			this.modView.DragEnter += new System.Windows.Forms.DragEventHandler(this.modView_DragEnter);
			this.modView.DragOver += new System.Windows.Forms.DragEventHandler(this.modView_DragOver);
			// 
			// columnModName
			// 
			this.columnModName.Text = "Mod Name";
			this.columnModName.Width = 480;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.buttonOk);
			this.flowLayoutPanel1.Controls.Add(this.buttonCancel);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 329);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(2);
			this.flowLayoutPanel1.Size = new System.Drawing.Size(484, 33);
			this.flowLayoutPanel1.TabIndex = 1;
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(402, 5);
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
			this.buttonCancel.Location = new System.Drawing.Point(321, 5);
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
			this.labelHelp.Size = new System.Drawing.Size(460, 41);
			this.labelHelp.TabIndex = 2;
			this.labelHelp.Text = "Uncheck the box next to a mod to uninstall it. \r\nYou can drag mods up or down in " +
				"the list to change the order in which they will be installed.\r\nDrag mods into th" +
				"is window to install them.";
			// 
			// EditModsForm
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(484, 362);
			this.Controls.Add(this.labelHelp);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.modView);
			this.Name = "EditModsForm";
			this.Text = "Edit mods";
			this.Title = "Edit mods";
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView modView;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.ColumnHeader columnModName;
		private System.Windows.Forms.Label labelHelp;
	}
}