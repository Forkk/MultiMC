namespace MultiMC.WinGUI
{
	partial class UpdateDialog
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
			this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.labelMessage = new System.Windows.Forms.Label();
			this.buttonPanel = new System.Windows.Forms.TableLayoutPanel();
			this.buttonYes = new System.Windows.Forms.Button();
			this.buttonChangelog = new System.Windows.Forms.Button();
			this.buttonNo = new System.Windows.Forms.Button();
			this.layoutPanel.SuspendLayout();
			this.buttonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// layoutPanel
			// 
			this.layoutPanel.ColumnCount = 1;
			this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.layoutPanel.Controls.Add(this.labelMessage, 0, 0);
			this.layoutPanel.Controls.Add(this.buttonPanel, 0, 1);
			this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutPanel.Location = new System.Drawing.Point(0, 0);
			this.layoutPanel.Name = "layoutPanel";
			this.layoutPanel.RowCount = 2;
			this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.layoutPanel.Size = new System.Drawing.Size(394, 64);
			this.layoutPanel.TabIndex = 0;
			// 
			// labelMessage
			// 
			this.labelMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.labelMessage.AutoSize = true;
			this.labelMessage.Location = new System.Drawing.Point(4, 6);
			this.labelMessage.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new System.Drawing.Size(386, 13);
			this.labelMessage.TabIndex = 0;
			this.labelMessage.Text = "New update";
			// 
			// buttonPanel
			// 
			this.buttonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.buttonPanel.AutoSize = true;
			this.buttonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonPanel.ColumnCount = 4;
			this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.buttonPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.buttonPanel.Controls.Add(this.buttonYes, 2, 0);
			this.buttonPanel.Controls.Add(this.buttonChangelog, 0, 0);
			this.buttonPanel.Controls.Add(this.buttonNo, 3, 0);
			this.buttonPanel.Location = new System.Drawing.Point(3, 28);
			this.buttonPanel.Name = "buttonPanel";
			this.buttonPanel.Padding = new System.Windows.Forms.Padding(2);
			this.buttonPanel.RowCount = 1;
			this.buttonPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.buttonPanel.Size = new System.Drawing.Size(388, 33);
			this.buttonPanel.TabIndex = 1;
			// 
			// buttonYes
			// 
			this.buttonYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.buttonYes.Location = new System.Drawing.Point(227, 5);
			this.buttonYes.Name = "buttonYes";
			this.buttonYes.Size = new System.Drawing.Size(75, 23);
			this.buttonYes.TabIndex = 0;
			this.buttonYes.Text = "&Yes";
			this.buttonYes.UseVisualStyleBackColor = true;
			this.buttonYes.Click += new System.EventHandler(this.buttonYes_Click);
			// 
			// buttonChangelog
			// 
			this.buttonChangelog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonChangelog.Location = new System.Drawing.Point(5, 5);
			this.buttonChangelog.Name = "buttonChangelog";
			this.buttonChangelog.Size = new System.Drawing.Size(75, 23);
			this.buttonChangelog.TabIndex = 1;
			this.buttonChangelog.Text = "&Changelog";
			this.buttonChangelog.UseVisualStyleBackColor = true;
			this.buttonChangelog.Click += new System.EventHandler(this.buttonChangelog_Click);
			// 
			// buttonNo
			// 
			this.buttonNo.DialogResult = System.Windows.Forms.DialogResult.No;
			this.buttonNo.Location = new System.Drawing.Point(308, 5);
			this.buttonNo.Name = "buttonNo";
			this.buttonNo.Size = new System.Drawing.Size(75, 23);
			this.buttonNo.TabIndex = 2;
			this.buttonNo.Text = "&No";
			this.buttonNo.UseVisualStyleBackColor = true;
			this.buttonNo.Click += new System.EventHandler(this.buttonNo_Click);
			// 
			// UpdateDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(394, 64);
			this.Controls.Add(this.layoutPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UpdateDialog";
			this.Text = "Update available";
			this.Title = "Update available";
			this.layoutPanel.ResumeLayout(false);
			this.layoutPanel.PerformLayout();
			this.buttonPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel layoutPanel;
		private System.Windows.Forms.Label labelMessage;
		private System.Windows.Forms.TableLayoutPanel buttonPanel;
		private System.Windows.Forms.Button buttonYes;
		private System.Windows.Forms.Button buttonChangelog;
		private System.Windows.Forms.Button buttonNo;
	}
}