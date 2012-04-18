namespace MultiMC.WinGUI
{
	partial class RestoreBackupDialog
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
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Example Item",
            "DD/MM/YYYY HH:MM:SS XX",
            "d4a607a0d"}, -1);
			this.buttonTable = new System.Windows.Forms.TableLayoutPanel();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.backupView = new System.Windows.Forms.ListView();
			this.columnHeaderDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeaderHash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.buttonTable.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonTable
			// 
			this.buttonTable.AutoSize = true;
			this.buttonTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonTable.ColumnCount = 3;
			this.buttonTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.buttonTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.buttonTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.buttonTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.buttonTable.Controls.Add(this.buttonCancel, 1, 0);
			this.buttonTable.Controls.Add(this.buttonOK, 2, 0);
			this.buttonTable.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonTable.Location = new System.Drawing.Point(0, 327);
			this.buttonTable.Name = "buttonTable";
			this.buttonTable.Padding = new System.Windows.Forms.Padding(3);
			this.buttonTable.RowCount = 1;
			this.buttonTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.buttonTable.Size = new System.Drawing.Size(484, 35);
			this.buttonTable.TabIndex = 3;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(322, 6);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 0;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(403, 6);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "&OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// columnHeaderName
			// 
			this.columnHeaderName.Text = "Backup Name";
			this.columnHeaderName.Width = 216;
			// 
			// backupView
			// 
			this.backupView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.backupView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderDate,
            this.columnHeaderHash});
			this.backupView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
			this.backupView.Location = new System.Drawing.Point(12, 12);
			this.backupView.Name = "backupView";
			this.backupView.Size = new System.Drawing.Size(460, 309);
			this.backupView.TabIndex = 2;
			this.backupView.UseCompatibleStateImageBehavior = false;
			this.backupView.View = System.Windows.Forms.View.Details;
			this.backupView.ItemActivate += new System.EventHandler(this.backupView_ItemActivate);
			// 
			// columnHeaderDate
			// 
			this.columnHeaderDate.Text = "Date";
			this.columnHeaderDate.Width = 158;
			// 
			// columnHeaderHash
			// 
			this.columnHeaderHash.Text = "Hash";
			this.columnHeaderHash.Width = 78;
			// 
			// RestoreBackupDialog
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(484, 362);
			this.Controls.Add(this.buttonTable);
			this.Controls.Add(this.backupView);
			this.Name = "RestoreBackupDialog";
			this.Text = "Restore backup";
			this.Title = "Restore backup";
			this.buttonTable.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel buttonTable;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.ColumnHeader columnHeaderName;
		private System.Windows.Forms.ListView backupView;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.ColumnHeader columnHeaderDate;
		private System.Windows.Forms.ColumnHeader columnHeaderHash;

	}
}