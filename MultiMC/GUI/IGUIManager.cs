using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	/// <summary>
	/// Defines a GUI manager that creates windows for a certain toolkit.
	/// </summary>
	public interface IGUIManager
	{
		void Initialize();

		IMainWindow MainWindow();

		IImageList LoadInstIcons(bool loadCustomIcons = true);

		IDialog AboutDialog();

		IConsoleWindow ConsoleWindow(Instance inst);

		IAddInstDialog AddInstDialog();

		IDialog SettingsWindow();

		IChangeIconDialog ChangeIconDialog();

		INotesDialog NotesDialog();

		IEditModsDialog EditModsDialog(Instance inst);

		ILoginDialog LoginDialog(string errMsg = null);

		IDialog DeleteDialog();

		void Run(IMainWindow mainWindow);
	}

	public static class GUIManager
	{
		public static IGUIManager Main
		{
			get { return _guiManager; }
		}

		static IGUIManager _guiManager;

		/// <summary>
		/// Creates the GUI manager for the current toolkit.
		/// </summary>
		public static void Create()
		{
			switch (Program.Toolkit)
			{
			case WindowToolkit.WinForms:
				_guiManager = new WinGUI.WinFormsGUIManager();
				break;

			case WindowToolkit.GtkSharp:
				_guiManager = new GTKGUI.GTKGUIManager();
				break;

			default:
				throw new NotImplementedException("No GUI manager found " +
					" for toolkit '" + Program.Toolkit.ToString() + "'");
			}
		}
	}
}
