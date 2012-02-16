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
using MultiMC.Data;

using Gtk;

namespace MultiMC
{
	public partial class EditNotesDialog : Gtk.Dialog
	{
		public EditNotesDialog(Window parent = null)
			: base("", parent, DialogFlags.Modal)
		{
			this.Build();
		}

		public string Notes
		{
			get { return notesTextView.Buffer.Text; }
			set { notesTextView.Buffer.Text = value; }
		}

		protected void OnButtonOkActivated(object sender, System.EventArgs e)
		{
			//Inst.Notes = notesTextView.Buffer.Text;
			//Destroy();
		}

		protected void OnButtonCancelActivated(object sender, System.EventArgs e)
		{
			//Destroy();
		}
	}
}

