using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MultiMC.GUI;

namespace MultiMC.WinGUI
{
	public partial class LoginForm : WinFormsDialog, ILoginDialog
	{
		public LoginForm(string errorMsg = null)
		{
			InitializeComponent();

			buttonLogin.Click += (sender, e) => OnResponse(DialogResponse.OK);
			buttonCancel.Click += (sender, e) => OnResponse(DialogResponse.Cancel);
			if (string.IsNullOrEmpty(errorMsg))
				labelError.Visible = false;
			else
				labelError.Text = errorMsg;

			checkRememberPassword.Enabled = RememberUsername;
		}

		public string Username
		{
			get { return usernameTextBox.Text; }
			set { usernameTextBox.Text = value; }
		}

		public string Password
		{
			get { return passwordTextBox.Text; }
			set { passwordTextBox.Text = value; }
		}

		public bool RememberUsername
		{
			get { return checkRememberUsername.Checked; }
			set { checkRememberUsername.Checked = value; }
		}

		public bool RememberPassword
		{
			get { return checkRememberPassword.Checked && checkRememberPassword.Enabled; }
			set { checkRememberPassword.Checked = value; }
		}

		public bool ForceUpdate
		{
			get { return !buttonForceUpdate.Enabled; }
			set { buttonForceUpdate.Enabled = !value; }
		}

		private void buttonForceUpdate_Click(object sender, EventArgs e)
		{
			buttonForceUpdate.Enabled = false;
			buttonForceUpdate.Text = "Update Forced!";
		}

		private void buttonLogin_Click(object sender, EventArgs e)
		{
			OnResponse(DialogResponse.OK);
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			OnResponse(DialogResponse.Cancel);
		}

		private void checkRememberUsername_CheckedChanged(object sender, EventArgs e)
		{
			checkRememberPassword.Enabled = RememberUsername;
		}
	}
}
