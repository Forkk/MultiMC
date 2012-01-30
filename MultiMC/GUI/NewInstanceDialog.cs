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
using System.IO;

using MultiMC.Data;

namespace MultiMC
{
	public partial class NewInstanceDialog : Gtk.Dialog
	{
		public NewInstanceDialog(Gtk.Window parent = null)
			: base("", parent, Gtk.DialogFlags.Modal)
		{
			this.Build();
		}

		protected void OnCancelClicked(object sender, System.EventArgs e)
		{
			Hide();
		}

		protected void OnOKClicked(object sender, System.EventArgs e)
		{
			Hide();
			if (OKClicked != null)
				OKClicked(this, EventArgs.Empty);
		}
		
		public string InstName
		{
			get { return instNameEntry.Text; }
		}
		
		public string InstDir
		{
			get
			{
				string instDir = 
					(!string.IsNullOrEmpty(Resources.InstDir) ? Resources.InstDir : "");
				return System.IO.Path.Combine(instDir, 
				                    Instance.GetValidDirName(instNameEntry.Text, instDir));
			}
		}

		protected void OnInstNameEntryActivated(object sender, System.EventArgs e)
		{
		}
		
		public event EventHandler OKClicked;
	}
}

