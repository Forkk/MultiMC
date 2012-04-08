using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

using Gtk;
using Glade;

using MultiMC.GUI;

namespace MultiMC.GTKGUI
{
	public class GTKGUIManager : IGUIManager
	{
		public GTKGUIManager()
		{

		}

		public void Initialize()
		{
			Application.Init();
		}

		public IMainWindow MainWindow()
		{
			return new MainWindow();
		}

		public IImageList LoadInstIcons()
		{
			Assembly ass = Assembly.GetExecutingAssembly();

			Dictionary<string, Image> imgDict =
				   new Dictionary<string, Image>();
			imgDict.Add("grass", new Image(ass, "MultiMC.Resources.grass.png"));
			imgDict.Add("brick", new Image(ass, "MultiMC.Resources.brick.png"));
			imgDict.Add("diamond", new Image(ass, "MultiMC.Resources.diamond.png"));
			imgDict.Add("dirt", new Image(ass, "MultiMC.Resources.dirt.png"));
			imgDict.Add("gold", new Image(ass, "MultiMC.Resources.gold.png"));
			imgDict.Add("iron", new Image(ass, "MultiMC.Resources.iron.png"));
			imgDict.Add("planks", new Image(ass, "MultiMC.Resources.planks.png"));
			imgDict.Add("tnt", new Image(ass, "MultiMC.Resources.tnt.png"));

			return new GTKImageList(Properties.Resources.UserIconDir,
				imgDict, imgDict["grass"]);
		}

		public IDialog AboutDialog()
		{
			throw new NotImplementedException();
		}

		public IConsoleWindow ConsoleWindow(Instance inst)
		{
			throw new NotImplementedException();
		}

		public IAddInstDialog AddInstDialog()
		{
			throw new NotImplementedException();
		}

		public IDialog SettingsWindow()
		{
			throw new NotImplementedException();
		}

		public IChangeIconDialog ChangeIconDialog()
		{
			throw new NotImplementedException();
		}

		public INotesDialog NotesDialog()
		{
			throw new NotImplementedException();
		}

		public IEditModsDialog EditModsDialog(Instance inst)
		{
			throw new NotImplementedException();
		}

		public ILoginDialog LoginDialog(string errMsg = null)
		{
			throw new NotImplementedException();
		}

		public IDialog DeleteDialog()
		{
			throw new NotImplementedException();
		}

		public void Run(IMainWindow mainWindow)
		{
			(mainWindow as GTKWindow).ShowAll();

			Application.Run();
		}
	}
}
