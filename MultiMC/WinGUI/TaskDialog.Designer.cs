namespace MultiMC.WinGUI
{
	partial class TaskDialog
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
			this.statusLabel = new System.Windows.Forms.Label();
			this.taskProgressBar = new System.Windows.Forms.ProgressBar();
			this.layoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// layoutPanel
			// 
			this.layoutPanel.ColumnCount = 1;
			this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.layoutPanel.Controls.Add(this.statusLabel, 0, 0);
			this.layoutPanel.Controls.Add(this.taskProgressBar, 0, 1);
			this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.layoutPanel.Location = new System.Drawing.Point(0, 0);
			this.layoutPanel.Name = "layoutPanel";
			this.layoutPanel.RowCount = 2;
			this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.layoutPanel.Size = new System.Drawing.Size(394, 81);
			this.layoutPanel.TabIndex = 0;
			// 
			// statusLabel
			// 
			this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.statusLabel.AutoSize = true;
			this.statusLabel.Location = new System.Drawing.Point(10, 10);
			this.statusLabel.Margin = new System.Windows.Forms.Padding(10);
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(374, 13);
			this.statusLabel.TabIndex = 1;
			this.statusLabel.Text = "...";
			// 
			// taskProgressBar
			// 
			this.taskProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.taskProgressBar.Location = new System.Drawing.Point(12, 45);
			this.taskProgressBar.Margin = new System.Windows.Forms.Padding(12);
			this.taskProgressBar.Name = "taskProgressBar";
			this.taskProgressBar.Size = new System.Drawing.Size(370, 23);
			this.taskProgressBar.TabIndex = 0;
			// 
			// TaskDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(394, 81);
			this.Controls.Add(this.layoutPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TaskDialog";
			this.layoutPanel.ResumeLayout(false);
			this.layoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel layoutPanel;
		private System.Windows.Forms.ProgressBar taskProgressBar;
		private System.Windows.Forms.Label statusLabel;
	}
}