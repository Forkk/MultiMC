using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gtk;
using Glade;

using MultiMC.GUI;

namespace MultiMC.GTKGUI
{
	public class LoginDialog : GTKDialog, ILoginDialog
	{
		public LoginDialog(Window parent, string errorMsg)
			: base("Login", parent)
		{
			XML gxml = new XML(null, "MultiMC.GTKGUI.LoginDialog.glade", 
				"loginTable", null);
			gxml.Autoconnect(this);

			labelErrorMsg.Text = errorMsg;

			Alignment loginAlign = new Alignment(0.5f, 0.5f, 1, 1);
			loginAlign.Add(loginTable);
			loginAlign.SetPadding(4, 4, 4, 4);
			this.VBox.Add(loginAlign);
			loginAlign.ShowAll();

			okButton = this.AddButton("_OK", ResponseType.Ok) as Button;
			cancelButton = this.AddButton("_Cancel", ResponseType.Cancel) as Button;

			this.Default = okButton;

			this.WidthRequest = 420;

			labelErrorMsg.Visible = !string.IsNullOrEmpty(labelErrorMsg.Text);

			entryPassword.Visibility = false;
		}

		void OnUsernameFieldActivated(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(Password))
			{
				entryPassword.GrabFocus();
			}
			else
			{
				this.ActivateDefault();
			}
		}

		void OnPasswordFieldActivated(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(Username))
			{
				entryUsername.GrabFocus();
			}
			else
			{
				this.ActivateDefault();
			}
		}

		#region Widgets
		[Widget]
		Table loginTable = null;

		[Widget]
		Label labelErrorMsg = null;

		[Widget]
		Entry entryUsername = null;

		[Widget]
		Entry entryPassword = null;

		[Widget]
		ToggleButton toggleForceUpdate = null;

		[Widget]
		CheckButton checkRememberUsername = null;

		[Widget]
		CheckButton checkRememberPassword = null;

		Button okButton;
		Button cancelButton;
		#endregion

		public string Username
		{
			get { return entryUsername.Text; }
			set { entryUsername.Text = value; }
		}

		public string Password
		{
			get { return entryPassword.Text; }
			set { entryPassword.Text = value; }
		}

		public bool RememberUsername
		{
			get { return checkRememberUsername.Active; }
			set { checkRememberUsername.Active = value; }
		}

		public bool RememberPassword
		{
			get { return checkRememberPassword.Active; }
			set { checkRememberPassword.Active = value; }
		}

		public bool ForceUpdate
		{
			get { return toggleForceUpdate.Active; }
			set { toggleForceUpdate.Active = value; }
		}
	}
}
