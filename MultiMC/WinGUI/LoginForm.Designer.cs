namespace MultiMC.WinGUI
{
	partial class LoginForm
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
			this.tablePanel = new System.Windows.Forms.TableLayoutPanel();
			this.labelUsername = new System.Windows.Forms.Label();
			this.labelPassword = new System.Windows.Forms.Label();
			this.passwordTextBox = new System.Windows.Forms.TextBox();
			this.checkRememberPassword = new System.Windows.Forms.CheckBox();
			this.labelError = new System.Windows.Forms.Label();
			this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.buttonLogin = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOffline = new System.Windows.Forms.Button();
			this.checkRememberUsername = new System.Windows.Forms.CheckBox();
			this.usernameTextBox = new System.Windows.Forms.TextBox();
			this.buttonForceUpdate = new System.Windows.Forms.Button();
			this.tablePanel.SuspendLayout();
			this.buttonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tablePanel
			// 
			this.tablePanel.AutoSize = true;
			this.tablePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tablePanel.ColumnCount = 3;
			this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tablePanel.Controls.Add(this.labelUsername, 0, 0);
			this.tablePanel.Controls.Add(this.labelPassword, 0, 1);
			this.tablePanel.Controls.Add(this.passwordTextBox, 1, 1);
			this.tablePanel.Controls.Add(this.checkRememberPassword, 2, 2);
			this.tablePanel.Controls.Add(this.labelError, 0, 3);
			this.tablePanel.Controls.Add(this.buttonPanel, 0, 3);
			this.tablePanel.Controls.Add(this.checkRememberUsername, 1, 2);
			this.tablePanel.Controls.Add(this.usernameTextBox, 1, 0);
			this.tablePanel.Controls.Add(this.buttonForceUpdate, 0, 2);
			this.tablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tablePanel.Location = new System.Drawing.Point(0, 0);
			this.tablePanel.Margin = new System.Windows.Forms.Padding(0);
			this.tablePanel.Name = "tablePanel";
			this.tablePanel.Padding = new System.Windows.Forms.Padding(4);
			this.tablePanel.RowCount = 5;
			this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tablePanel.Size = new System.Drawing.Size(384, 137);
			this.tablePanel.TabIndex = 0;
			// 
			// labelUsername
			// 
			this.labelUsername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.labelUsername.AutoSize = true;
			this.labelUsername.Location = new System.Drawing.Point(7, 10);
			this.labelUsername.Name = "labelUsername";
			this.labelUsername.Size = new System.Drawing.Size(82, 13);
			this.labelUsername.TabIndex = 0;
			this.labelUsername.Text = "Username:";
			this.labelUsername.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelPassword
			// 
			this.labelPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.labelPassword.AutoSize = true;
			this.labelPassword.Location = new System.Drawing.Point(7, 36);
			this.labelPassword.Name = "labelPassword";
			this.labelPassword.Size = new System.Drawing.Size(82, 13);
			this.labelPassword.TabIndex = 1;
			this.labelPassword.Text = "Password:";
			this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// passwordTextBox
			// 
			this.passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tablePanel.SetColumnSpan(this.passwordTextBox, 2);
			this.passwordTextBox.Location = new System.Drawing.Point(95, 33);
			this.passwordTextBox.Name = "passwordTextBox";
			this.passwordTextBox.Size = new System.Drawing.Size(282, 20);
			this.passwordTextBox.TabIndex = 1;
			this.passwordTextBox.UseSystemPasswordChar = true;
			// 
			// checkRememberPassword
			// 
			this.checkRememberPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.checkRememberPassword.AutoSize = true;
			this.checkRememberPassword.Location = new System.Drawing.Point(229, 62);
			this.checkRememberPassword.Name = "checkRememberPassword";
			this.checkRememberPassword.Size = new System.Drawing.Size(126, 17);
			this.checkRememberPassword.TabIndex = 3;
			this.checkRememberPassword.Text = "R&emember Password";
			this.checkRememberPassword.UseVisualStyleBackColor = true;
			// 
			// labelError
			// 
			this.labelError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.labelError.AutoSize = true;
			this.tablePanel.SetColumnSpan(this.labelError, 3);
			this.labelError.Location = new System.Drawing.Point(7, 88);
			this.labelError.Margin = new System.Windows.Forms.Padding(3);
			this.labelError.Name = "labelError";
			this.labelError.Size = new System.Drawing.Size(370, 13);
			this.labelError.TabIndex = 7;
			this.labelError.Text = "Error message";
			this.labelError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// buttonPanel
			// 
			this.buttonPanel.AutoSize = true;
			this.buttonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tablePanel.SetColumnSpan(this.buttonPanel, 3);
			this.buttonPanel.Controls.Add(this.buttonLogin);
			this.buttonPanel.Controls.Add(this.buttonCancel);
			this.buttonPanel.Controls.Add(this.buttonOffline);
			this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.buttonPanel.Location = new System.Drawing.Point(7, 107);
			this.buttonPanel.Name = "buttonPanel";
			this.buttonPanel.Size = new System.Drawing.Size(370, 29);
			this.buttonPanel.TabIndex = 8;
			// 
			// buttonLogin
			// 
			this.buttonLogin.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonLogin.Location = new System.Drawing.Point(292, 3);
			this.buttonLogin.Name = "buttonLogin";
			this.buttonLogin.Size = new System.Drawing.Size(75, 23);
			this.buttonLogin.TabIndex = 5;
			this.buttonLogin.Text = "&Login";
			this.buttonLogin.UseVisualStyleBackColor = true;
			this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(211, 3);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 6;
			this.buttonCancel.Text = "&Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOffline
			// 
			this.buttonOffline.DialogResult = System.Windows.Forms.DialogResult.No;
			this.buttonOffline.Location = new System.Drawing.Point(130, 3);
			this.buttonOffline.Name = "buttonOffline";
			this.buttonOffline.Size = new System.Drawing.Size(75, 23);
			this.buttonOffline.TabIndex = 7;
			this.buttonOffline.Text = "Play &Offline";
			this.buttonOffline.UseVisualStyleBackColor = true;
			this.buttonOffline.Click += new System.EventHandler(this.buttonOffline_Click);
			// 
			// checkRememberUsername
			// 
			this.checkRememberUsername.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.checkRememberUsername.AutoSize = true;
			this.checkRememberUsername.Location = new System.Drawing.Point(95, 62);
			this.checkRememberUsername.Name = "checkRememberUsername";
			this.checkRememberUsername.Size = new System.Drawing.Size(128, 17);
			this.checkRememberUsername.TabIndex = 2;
			this.checkRememberUsername.Text = "&Remember Username";
			this.checkRememberUsername.UseVisualStyleBackColor = true;
			this.checkRememberUsername.CheckedChanged += new System.EventHandler(this.checkRememberUsername_CheckedChanged);
			// 
			// usernameTextBox
			// 
			this.usernameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tablePanel.SetColumnSpan(this.usernameTextBox, 2);
			this.usernameTextBox.Location = new System.Drawing.Point(95, 7);
			this.usernameTextBox.Name = "usernameTextBox";
			this.usernameTextBox.Size = new System.Drawing.Size(282, 20);
			this.usernameTextBox.TabIndex = 0;
			// 
			// buttonForceUpdate
			// 
			this.buttonForceUpdate.AutoSize = true;
			this.buttonForceUpdate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.buttonForceUpdate.Location = new System.Drawing.Point(7, 59);
			this.buttonForceUpdate.Name = "buttonForceUpdate";
			this.buttonForceUpdate.Size = new System.Drawing.Size(82, 23);
			this.buttonForceUpdate.TabIndex = 4;
			this.buttonForceUpdate.Text = "Force Update";
			this.buttonForceUpdate.UseVisualStyleBackColor = true;
			this.buttonForceUpdate.Click += new System.EventHandler(this.buttonForceUpdate_Click);
			// 
			// LoginForm
			// 
			this.AcceptButton = this.buttonLogin;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(384, 137);
			this.Controls.Add(this.tablePanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoginForm";
			this.Text = "Login";
			this.Title = "Login";
			this.tablePanel.ResumeLayout(false);
			this.tablePanel.PerformLayout();
			this.buttonPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tablePanel;
		private System.Windows.Forms.Label labelUsername;
		private System.Windows.Forms.Label labelPassword;
		private System.Windows.Forms.TextBox usernameTextBox;
		private System.Windows.Forms.TextBox passwordTextBox;
		private System.Windows.Forms.CheckBox checkRememberPassword;
		private System.Windows.Forms.CheckBox checkRememberUsername;
		private System.Windows.Forms.Label labelError;
		private System.Windows.Forms.FlowLayoutPanel buttonPanel;
		private System.Windows.Forms.Button buttonLogin;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOffline;
		private System.Windows.Forms.Button buttonForceUpdate;
	}
}