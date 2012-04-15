using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

using Gtk;
using Glade;

using MultiMC.GUI;
using Gdk;

namespace MultiMC.GTKGUI
{
	public class GTKGUIManager : IGUIManager
	{
		GTKWindow mainWindow;

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

		public IImageList LoadInstIcons(bool loadCustomIcons)
		{
			Assembly ass = Assembly.GetExecutingAssembly();

			Dictionary<string, Pixbuf> imgDict =
				   new Dictionary<string, Pixbuf>();
			imgDict.Add("grass", new Pixbuf(ass, "MultiMC.Resources.grass.png"));
			imgDict.Add("brick", new Pixbuf(ass, "MultiMC.Resources.brick.png"));
			imgDict.Add("diamond", new Pixbuf(ass, "MultiMC.Resources.diamond.png"));
			imgDict.Add("dirt", new Pixbuf(ass, "MultiMC.Resources.dirt.png"));
			imgDict.Add("gold", new Pixbuf(ass, "MultiMC.Resources.gold.png"));
			imgDict.Add("iron", new Pixbuf(ass, "MultiMC.Resources.iron.png"));
			imgDict.Add("planks", new Pixbuf(ass, "MultiMC.Resources.planks.png"));
			imgDict.Add("tnt", new Pixbuf(ass, "MultiMC.Resources.tnt.png"));

			return new GTKImageList(Properties.Resources.UserIconDir,
				imgDict, imgDict["grass"], loadCustomIcons);
		}

		public IAboutDialog AboutDialog()
		{
			return new GTKAboutDialog();
		}

		public IConsoleWindow ConsoleWindow(Instance inst)
		{
			return new ConsoleWindow(inst);
		}

		public IAddInstDialog AddInstDialog()
		{
			return new AddInstDialog(mainWindow);
		}

		public ISettingsDialog SettingsWindow()
		{
			return new SettingsDialog(mainWindow);
		}

		public IChangeIconDialog ChangeIconDialog()
		{
			return new ChangeIconDialog(mainWindow);
		}

		public INotesDialog NotesDialog()
		{
			return new EditNotesDialog(mainWindow);
		}

		public IEditModsDialog EditModsDialog(Instance inst)
		{
			return new EditModsDialog(mainWindow, inst);
		}

		public ILoginDialog LoginDialog(string errMsg = null)
		{
			return new LoginDialog(mainWindow, errMsg);
		}

		public IDialog DeleteDialog()
		{
			return new DeleteDialog(mainWindow);
		}

		public void Run(IMainWindow mainWindow)
		{
			this.mainWindow = (mainWindow as GTKWindow);
			mainWindow.Show();

			Application.Run();
		}

		public IDialog ChangelogDialog()
		{
			throw new NotImplementedException();
		}

		public IUpdateDialog UpdateDialog()
		{
			throw new NotImplementedException();
		}


		public ITaskDialog TaskDialog(Tasks.Task task)
		{
			return new TaskDialog(task);
		}
	}
}
