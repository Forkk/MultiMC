// 
//  Copyright 2012  Andrew Okin
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;

using Gtk;

namespace MultiMC
{
	public partial class LoginDialog : Gtk.Dialog
	{
		public LoginDialog(Gtk.Window parent, 
		                   string errorMessage = "", 
		                   bool canplayOffline = false)
			: base("Login", parent, DialogFlags.Modal)
		{
			this.Build();
			this.ErrorMessage = errorMessage;
			this.buttonOffline.Sensitive = canplayOffline;
		}
		
		public string Username
		{
			get { return userEntry.Text; }
			set { userEntry.Text = value; }
		}
		
		public string Password
		{
			get { return passwordEntry.Text; }
			set { passwordEntry.Text = value; }
		}
		
		public bool ForceUpdate
		{
			get { return forceToggle.Active; }
		}
		
		public bool RememberUsername
		{
			get { return checkRememberUsername.Active; }
			set { checkRememberUsername.Active = value; }
		}
		
		public bool RememberPassword
		{
			get { return checkRememberPwd.Active; }
			set { checkRememberPwd.Active = value; }
		}
		
		public string ErrorMessage
		{
			get { return labelErrorMsg.Text; }
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					labelErrorMsg.Text = "";
					labelErrorMsg.Visible = false;
				}
				else
				{
					labelErrorMsg.Text = value;
					labelErrorMsg.Visible = true;
				}
			}
		}

		protected void OnUserEntryActivated(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(passwordEntry.Text))
				buttonOk.Activate();
			else
				passwordEntry.GrabFocus();
		}

		protected void OnPasswordEntryActivated(object sender, System.EventArgs e)
		{
			buttonOk.Activate();
		}
	}
}

