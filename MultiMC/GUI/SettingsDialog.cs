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
			
			AppSettings settings = AppSettings.Main;
			
			entryLauncherFilename.Text = settings.LauncherPath;
			
			checkbuttonShowConsole.Active = settings.ShowConsole;
			checkbuttonAutoCloseConsole.Active = settings.AutoCloseConsole;
			
			checkbuttonAutoUpdate.Active = settings.AutoUpdate;
			
			spinbuttonInitialMemory.Value = settings.InitialMemoryAlloc;
			spinbuttonMaxMemory.Value = settings.MaxMemoryAlloc;
			
			entryJavaPath.Text = settings.JavaPath;
		}

		protected void OnCancel(object sender, System.EventArgs e)
		{
			Destroy();
		}

		protected void OnOK(object sender, System.EventArgs e)
		{
			AppSettings settings = AppSettings.Main;
			
			settings.LauncherPath = entryLauncherFilename.Text;
			
			settings.ShowConsole = checkbuttonShowConsole.Active;
			settings.AutoCloseConsole = checkbuttonAutoCloseConsole.Active;
			
			settings.AutoUpdate = checkbuttonAutoUpdate.Active;
			
			settings.InitialMemoryAlloc = (int)spinbuttonInitialMemory.Value;
			settings.MaxMemoryAlloc = (int)spinbuttonMaxMemory.Value;
			
			settings.JavaPath = entryJavaPath.Text;
			
			settings.Save();
			
			Destroy();
		}
	}
}

