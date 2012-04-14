namespace MultiMC.WinGUI
{
	partial class ChangelogDialog
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
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.buttonClose = new System.Windows.Forms.Button();
			this.buttonRefresh = new System.Windows.Forms.Button();
			this.textBoxChangelog = new System.Windows.Forms.TextBox();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.buttonClose);
			this.flowLayoutPanel1.Controls.Add(this.buttonRefresh);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 329);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(2);
			this.flowLayoutPanel1.Size = new System.Drawing.Size(484, 33);
			this.flowLayoutPanel1.TabIndex = 0;
			// 
			// buttonClose
			// 
			this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonClose.Location = new System.Drawing.Point(402, 5);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(75, 23);
			this.buttonClose.TabIndex = 0;
			this.buttonClose.Text = "&Close";
			this.buttonClose.UseVisualStyleBackColor = true;
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// buttonRefresh
			// 
			this.buttonRefresh.Location = new System.Drawing.Point(321, 5);
			this.buttonRefresh.Name = "buttonRefresh";
			this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
			this.buttonRefresh.TabIndex = 2;
			this.buttonRefresh.Text = "&Refresh";
			this.buttonRefresh.UseVisualStyleBackColor = true;
			this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
			// 
			// textBoxChangelog
			// 
			this.textBoxChangelog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxChangelog.Location = new System.Drawing.Point(12, 12);
			this.textBoxChangelog.Multiline = true;
			this.textBoxChangelog.Name = "textBoxChangelog";
			this.textBoxChangelog.ReadOnly = true;
			this.textBoxChangelog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxChangelog.Size = new System.Drawing.Size(460, 311);
			this.textBoxChangelog.TabIndex = 1;
			// 
			// ChangelogDialog
			// 
			this.AcceptButton = this.buttonClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonClose;
			this.ClientSize = new System.Drawing.Size(484, 362);
			this.Controls.Add(this.textBoxChangelog);
			this.Controls.Add(this.flowLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChangelogDialog";
			this.Text = "ChangelogDialog";
			this.Title = "ChangelogDialog";
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.TextBox textBoxChangelog;
		private System.Windows.Forms.Button buttonRefresh;
	}
}