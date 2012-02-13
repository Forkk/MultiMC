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

namespace MultiMC
{
	public partial class SettingsDialog : Gtk.Dialog
	{
		public SettingsDialog(Gtk.Window parent)
			: base("Settings", parent, Gtk.DialogFlags.Modal)
		{
			this.Build();
			
			LoadSettings();
		}
		
		protected void LoadSettings()
		{
			AppSettings settings = AppSettings.Main;
			
			// **General Section**
			
			// Console settings
			checkbuttonShowConsole.Active = settings.ShowConsole;
			checkbuttonAutoCloseConsole.Active = settings.AutoCloseConsole;
			
			// Update settings
			checkbuttonAutoUpdate.Active = settings.AutoUpdate;
			
			// Hint settings
			checkbuttonEnableHints.Active = settings.EnableHints;
			
			
			// **Advanced Section**
			
			// Memory settings
			spinbuttonInitialMemory.Value = settings.MinMemoryAlloc;
			spinbuttonMaxMemory.Value = settings.MaxMemoryAlloc;
			
			// Java settings
			entryJavaPath.Text = settings.JavaPath;
		}

		protected void OnCancel(object sender, System.EventArgs e)
		{
			Destroy();
		}

		protected void OnOK(object sender, System.EventArgs e)
		{
			AppSettings settings = AppSettings.Main;
			
			// **General Section**
			
			// Console settings
			settings.ShowConsole = checkbuttonShowConsole.Active;
			settings.AutoCloseConsole = checkbuttonAutoCloseConsole.Active;
			
			// Update settings
			settings.AutoUpdate = checkbuttonAutoUpdate.Active;
			
			// Hint settings
			settings.EnableHints = checkbuttonEnableHints.Active;
			
			
			// **Advanced Section**
			
			// Memory settings
			settings.MinMemoryAlloc = (int)spinbuttonInitialMemory.Value;
			settings.MaxMemoryAlloc = (int)spinbuttonMaxMemory.Value;
			
			// Java settings
			settings.JavaPath = entryJavaPath.Text;
			
			settings.Save();
			
			Destroy();
		}

		protected void OnBtnAutoJavaPathClicked(object sender, System.EventArgs e)
		{
			bool success = AppSettings.Main.AutoDetectJavaPath();
			if (!success)
				MessageUtils.ShowMessageBox(this, 
				                            Gtk.MessageType.Warning,
				                            "Error",
				                            "Failed to find Java path.");
			entryJavaPath.Text = AppSettings.Main.JavaPath;
		}

		protected void OnButtonResetHintsClicked(object sender, System.EventArgs e)
		{
			HintList.Hints.ResetAllHints();
			MessageUtils.ShowMessageBox(this, 
			                            Gtk.MessageType.Info, 
			                            "Hints Reset", 
			                            "All hints have been successfully reset.");
		}
	}
}

