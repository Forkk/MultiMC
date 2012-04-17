namespace MultiMC.WinGUI
{
	partial class SaveManagerDialog
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
			this.saveView = new System.Windows.Forms.ListView();
			this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.buttonTable = new System.Windows.Forms.TableLayoutPanel();
			this.buttonCreateBackup = new System.Windows.Forms.Button();
			this.buttonRestore = new System.Windows.Forms.Button();
			this.buttonClose = new System.Windows.Forms.Button();
			this.buttonTable.SuspendLayout();
			this.SuspendLayout();
			// 
			// saveView
			// 
			this.saveView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.saveView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName});
			this.saveView.Location = new System.Drawing.Point(12, 12);
			this.saveView.Name = "saveView";
			this.saveView.Size = new System.Drawing.Size(460, 289);
			this.saveView.TabIndex = 0;
			this.saveView.UseCompatibleStateImageBehavior = false;
			this.saveView.View = System.Windows.Forms.View.Details;
			// 
			// columnHeaderName
			// 
			this.columnHeaderName.Text = "Save Folder";
			this.columnHeaderName.Width = 456;
			// 
			// buttonTable
			// 
			this.buttonTable.AutoSize = true;
			this.buttonTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonTable.ColumnCount = 4;
			this.buttonTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.buttonTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.buttonTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.buttonTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.buttonTable.Controls.Add(this.buttonCreateBackup, 1, 0);
			this.buttonTable.Controls.Add(this.buttonRestore, 0, 0);
			this.buttonTable.Controls.Add(this.buttonClose, 3, 0);
			this.buttonTable.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonTable.Location = new System.Drawing.Point(0, 307);
			this.buttonTable.Name = "buttonTable";
			this.buttonTable.Padding = new System.Windows.Forms.Padding(3);
			this.buttonTable.RowCount = 1;
			this.buttonTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.buttonTable.Size = new System.Drawing.Size(484, 35);
			this.buttonTable.TabIndex = 1;
			// 
			// buttonCreateBackup
			// 
			this.buttonCreateBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCreateBackup.AutoSize = true;
			this.buttonCreateBackup.Location = new System.Drawing.Point(106, 6);
			this.buttonCreateBackup.Name = "buttonCreateBackup";
			this.buttonCreateBackup.Size = new System.Drawing.Size(88, 23);
			this.buttonCreateBackup.TabIndex = 2;
			this.buttonCreateBackup.Text = "Create &Backup";
			this.buttonCreateBackup.UseVisualStyleBackColor = true;
			this.buttonCreateBackup.Click += new System.EventHandler(this.buttonCreateBackup_Click);
			// 
			// buttonRestore
			// 
			this.buttonRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRestore.AutoSize = true;
			this.buttonRestore.Location = new System.Drawing.Point(6, 6);
			this.buttonRestore.Name = "buttonRestore";
			this.buttonRestore.Size = new System.Drawing.Size(94, 23);
			this.buttonRestore.TabIndex = 1;
			this.buttonRestore.Text = "&Restore Backup";
			this.buttonRestore.UseVisualStyleBackColor = true;
			this.buttonRestore.Click += new System.EventHandler(this.buttonRestore_Click);
			// 
			// buttonClose
			// 
			this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonClose.Location = new System.Drawing.Point(403, 6);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(75, 23);
			this.buttonClose.TabIndex = 0;
			this.buttonClose.Text = "&Close";
			this.buttonClose.UseVisualStyleBackColor = true;
			// 
			// SaveManagerDialog
			// 
			this.AcceptButton = this.buttonClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonClose;
			this.ClientSize = new System.Drawing.Size(484, 342);
			this.Controls.Add(this.buttonTable);
			this.Controls.Add(this.saveView);
			this.Name = "SaveManagerDialog";
			this.Text = "Saves";
			this.Title = "Saves";
			this.buttonTable.ResumeLayout(false);
			this.buttonTable.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView saveView;
		private System.Windows.Forms.TableLayoutPanel buttonTable;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Button buttonRestore;
		private System.Windows.Forms.Button buttonCreateBackup;
		private System.Windows.Forms.ColumnHeader columnHeaderName;


	}
}