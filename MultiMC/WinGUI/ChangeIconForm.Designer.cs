namespace MultiMC.WinGUI
{
	partial class ChangeIconForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeIconForm));
			this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.buttonOk = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.iconView = new System.Windows.Forms.ListView();
			this.buttonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonPanel
			// 
			this.buttonPanel.AutoSize = true;
			this.buttonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonPanel.Controls.Add(this.buttonOk);
			this.buttonPanel.Controls.Add(this.buttonCancel);
			this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.buttonPanel.Location = new System.Drawing.Point(0, 279);
			this.buttonPanel.Name = "buttonPanel";
			this.buttonPanel.Padding = new System.Windows.Forms.Padding(2);
			this.buttonPanel.Size = new System.Drawing.Size(484, 33);
			this.buttonPanel.TabIndex = 0;
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
			// iconView
			// 
			this.iconView.AllowDrop = true;
			this.iconView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.iconView.Location = new System.Drawing.Point(0, 0);
			this.iconView.MultiSelect = false;
			this.iconView.Name = "iconView";
			this.iconView.Size = new System.Drawing.Size(484, 273);
			this.iconView.TabIndex = 1;
			this.iconView.UseCompatibleStateImageBehavior = false;
			this.iconView.ItemActivate += new System.EventHandler(this.iconView_ItemActivate);
			this.iconView.DragDrop += new System.Windows.Forms.DragEventHandler(this.iconView_DragDrop);
			this.iconView.DragOver += new System.Windows.Forms.DragEventHandler(this.iconView_DragOver);
			this.iconView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.iconView_KeyDown);
			// 
			// ChangeIconForm
			// 
			this.AcceptButton = this.buttonOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(484, 312);
			this.Controls.Add(this.iconView);
			this.Controls.Add(this.buttonPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ChangeIconForm";
			this.Text = "Change icon";
			this.Title = "Change icon";
			this.buttonPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel buttonPanel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.ListView iconView;
	}
}