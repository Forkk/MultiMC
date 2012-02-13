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
	public partial class HintDialog : Gtk.Dialog
	{
		private HintDialog(Window parent, Hint hint)
			: base("Hint", parent, DialogFlags.Modal)
		{
			this.Build();
			this.hintAlignment.Remove(textviewHint);
			this.textviewHint.Buffer.Text = HintList.Hints[hint];
			this.hintAlignment.Add(textviewHint);
		}
		
		public bool DontShowAgain
		{
			get { return dontShowCheck.Active; }
		}
		
		public static void ShowHint(Window parent,
		                            Hint hint,
		                            bool forceShowHint = false)
		{
			if (!HintList.Hints.ShouldShowHint(hint) &&
			    !forceShowHint)
				return;
			
			if (!AppSettings.Main.EnableHints)
				return;
			
			HintDialog dialog = new HintDialog(parent, hint);
			dialog.Response += (o, args) => dialog.Destroy();
			dialog.Run();
			HintList.Hints.SetShowHint(hint, !dialog.DontShowAgain);
		}
	}
}

