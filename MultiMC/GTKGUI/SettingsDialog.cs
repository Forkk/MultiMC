using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gtk;
using Glade;

using MultiMC.GUI;

namespace MultiMC.GTKGUI
{
	public class SettingsDialog : GTKDialog
	{
		public SettingsDialog(Window parent)
			: base("Settings", parent)
		{
			XML gxml = new XML(null, "MultiMC.GTKGUI.SettingsDialog.glade",
				"settingsNotebook", null);
			gxml.Autoconnect(this);

			//settingsNotebook.;
			VBox.PackStart(settingsNotebook, true, true, 0);

			AddButton("_OK", ResponseType.Ok);
			AddButton("_Cancel", ResponseType.Cancel);

			WidthRequest = 450;
			HeightRequest = 400;

			Response += OnDialogResponse;

			LoadSettings(AppSettings.Main);
		}

		void OnDialogResponse(object sender, DialogResponseEventArgs e)
		{
			if (e.Response == DialogResponse.OK)
			{
				ApplySettings(AppSettings.Main);
			}
		}

		void ApplySettings(AppSettings settings)
		{
			settings.ShowConsole = checkShowConsole.Active;
			settings.AutoCloseConsole = checkAutoCloseConsole.Active;

			settings.AutoUpdate = checkAutoUpdate.Active;

			settings.MinMemoryAlloc = spinMinMemAlloc.ValueAsInt;
			settings.MaxMemoryAlloc = spinMaxMemAlloc.ValueAsInt;

			settings.JavaPath = entryJavaPath.Text;
		}

		void LoadSettings(AppSettings settings)
		{
			checkShowConsole.Active = settings.ShowConsole;
			checkAutoCloseConsole.Active = settings.AutoCloseConsole;

			checkAutoUpdate.Active = settings.AutoUpdate;

			spinMinMemAlloc.Value = settings.MinMemoryAlloc;
			spinMaxMemAlloc.Value = settings.MaxMemoryAlloc;

			entryJavaPath.Text = settings.JavaPath;
		}

		#region Widgets
		[Widget]
		Notebook settingsNotebook = null;

		[Widget]
		CheckButton checkShowConsole;

		[Widget]
		CheckButton checkAutoCloseConsole;

		[Widget]
		CheckButton checkAutoUpdate;

		[Widget]
		ToggleButton toggleForceUpdate;

		[Widget]
		SpinButton spinMinMemAlloc;

		[Widget]
		SpinButton spinMaxMemAlloc;

		[Widget]
		Entry entryJavaPath;
		#endregion
	}
}
