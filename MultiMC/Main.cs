using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

using MultiMC.GUI;
using MultiMC.Tasks;

namespace MultiMC
{
	/// <summary>
	/// The main class.
	/// </summary>
	public class Main
	{
		public IMainWindow MainWindow
		{
			get;
			protected set;
		}

		public Main()
		{
			switch (Program.Toolkit)
			{
			case WindowToolkit.WinForms:
				// Initialize WinForms
				System.Windows.Forms.Application.EnableVisualStyles();
				System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

				// Create the main window
				MainWindow = new WinGUI.MainForm();
				break;
			case WindowToolkit.GtkSharp:
				ToolkitNotSupported();
				break;
			}

			MainWindow.Title = string.Format("MultiMC {0} for {1}", 
				AppUtils.GetVersion().ToString(2), OSUtils.OSName);

			MainWindow.DefaultPosition = DefWindowPosition.CenterScreen;

			MainWindow.NewInstClicked += new EventHandler(NewInstClicked);
			MainWindow.RefreshClicked += (o, args) => MainWindow.LoadInstances();
			MainWindow.ViewFolderClicked += new EventHandler(ViewFolderClicked);

			MainWindow.SettingsClicked += new EventHandler(SettingsClicked);

			MainWindow.AboutClicked += new EventHandler(AboutClicked);
		}

		void AboutClicked(object sender, EventArgs e)
		{
			IDialog aboutDlg = null;
			switch (Program.Toolkit)
			{
			case WindowToolkit.WinForms:
				aboutDlg = new WinGUI.AboutForm();
				break;

			default:
				ToolkitNotSupported();
				break;
			}
			aboutDlg.Parent = MainWindow;
			aboutDlg.DefaultPosition = DefWindowPosition.CenterParent;

			aboutDlg.Run();
		}

		#region Menu Bar Events
		void NewInstClicked(object sender, EventArgs e)
		{
			IAddInstDialog addInstDlg = null;
			switch (Program.Toolkit)
			{
			case WindowToolkit.WinForms:
				addInstDlg = new WinGUI.NewInstDialog();
				break;
			}
			addInstDlg.DefaultPosition = DefWindowPosition.CenterParent;
			addInstDlg.Parent = MainWindow;
			addInstDlg.Response += (o, args) =>
			{
				if (args.Response == DialogResponse.OK)
				{
					Instance inst = new Instance(addInstDlg.InstName, 
						Path.Combine(AppSettings.Main.InstanceDir,
							Instance.GetValidDirName(addInstDlg.InstName, 
							AppSettings.Main.InstanceDir)));
					MainWindow.LoadInstances();
				}
				addInstDlg.Close();
			};

			addInstDlg.Run();
		}

		void ViewFolderClicked(object sender, EventArgs e)
		{
			string instDir = Path.GetFullPath(AppSettings.Main.InstanceDir);

			if (!Directory.Exists(instDir))
				Directory.CreateDirectory(instDir);
			Process.Start(instDir);
		}

		void CheckUpdatesClicked(object sender, EventArgs e)
		{
			DoUpdateCheck();
		}

		void SettingsClicked(object sender, EventArgs e)
		{
			switch (Program.Toolkit)
			{
			case WindowToolkit.WinForms:
				WinGUI.SettingsForm settingsForm = new WinGUI.SettingsForm();
				settingsForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
				settingsForm.Parent = MainWindow;
				settingsForm.Response += (o, args) =>
					{
						if (args.Response != DialogResponse.Other)
						{
							settingsForm.Close();
						}
					};
				settingsForm.Run();
				break;
			}
		}
		#endregion

		public void Run()
		{
			switch (Program.Toolkit)
			{
			case WindowToolkit.WinForms:
				System.Windows.Forms.Application.Run(MainWindow as WinGUI.MainForm);
				break;
			default:
				ToolkitNotSupported();
				break;
			}
		}

		#region Task System

		List<Task> currentTasks = new List<Task>();
		//Dictionary<int, ProgressBar> progBars = new Dictionary<int, ProgressBar>();

		private void AddTask(Task newTask)
		{
			currentTasks.Add(newTask);
			int index = currentTasks.IndexOf(newTask);
			newTask.TaskID = index;

			//ProgressBar taskProgBar = new ProgressBar();
			//taskProgBar.HeightRequest = 20;
			//progBars[newTask.TaskID] = taskProgBar;
			//progBarBox.PackEnd(taskProgBar, true, true, 0);
		}

		private void StartTask(Task task)
		{
			AddTask(task);
			task.Started += new EventHandler(taskStarted);
			task.Completed += taskCompleted;
			task.ProgressChange +=
				new Task.ProgressChangeEventHandler(taskProgressChange);
			task.StatusChange +=
				new Task.StatusChangeEventHandler(taskStatusChange);
			task.ErrorMessage += new Task.ErrorMessageEventHandler(TaskErrorMessage);
			task.Start();
		}

		void TaskErrorMessage(object sender, Task.ErrorMessageEventArgs e)
		{
			MainWindow.Invoke(
				(sender1, e1) =>
				{
					MessageBox.Show(MainWindow, e.Message, "Error");
				});
		}

		#region Mod Installation

		/// <summary>
		/// Rebuilds the given instance's minecraft.jar
		/// </summary>
		private Modder RebuildMCJar(Instance inst)
		{
			if (!File.Exists(SelectedInst.MCJar))
			{
				MessageBox.Show(MainWindow,
								"You must run the " +
								"instance at least " +
								"once before installing mods.",
								"Error");
				return null;
			}
			Modder modder = new Modder(inst);
			modder.Completed += (sender, e) => inst.NeedsRebuild = false;
			StartTask(modder);
			return modder;
		}

		#endregion

		#region Updates

		Updater updater;
		Version updateVersion;

		private void DoUpdateCheck()
		{
			if (updater != null &&
				updater.Running)
			{
				return;
			}

			updater = new Updater();
			updater.Completed += (sender, e) =>
			{
				MainWindow.Invoke(
					(sender2, e2) =>
					{
						updateVersion = updater.NewVersion;
						if (updateVersion != null &&
								updater.NewVersion.CompareTo(AppUtils.GetVersion()) > 0)
						{
							DownloadNewVersion();
						}
					});
			};
			Console.WriteLine("Checking for updates...");
			StartTask(updater);
		}

		private void toolStripUpdate_Click(object sender, EventArgs e)
		{
			DoUpdateCheck();
		}

		#endregion

		#region Update Install

		Downloader updateDL;

		private void DownloadNewVersion()
		{
			updateDL = new Downloader(Properties.Resources.NewVersionFileName,
				Properties.Resources.UpdateURL, "Downloading updates...");
			updateDL.Completed += updateDL_Completed;
			StartTask(updateDL);
		}

		void updateDL_Completed(object sender, EventArgs e)
		{
			MainWindow.Invoke(
				(sender1, e1) =>
				{
					string updatemsg = "Version {0} has been downloaded. " +
							"Would you like to install it now?";
					string updatestr = (updateVersion != null ? updateVersion.ToString() : "");
					if (string.IsNullOrEmpty(updatestr))
					{
						updatestr = "";
						updatemsg = "MultiMC has downloaded updates, would you like to install them?";
					}

					DialogResponse response = MessageBox.Show(
						MainWindow, updatemsg, "Update MultiMC?", MessageButtons.YesNo);

					if (response == DialogResponse.OK)
					{
						CloseForUpdates();
					}
				});
		}

		void CloseForUpdates()
		{
			Program.InstallUpdates = true;
			Program.RestartAfterUpdate = true;
			MainWindow.Close();
		}

		#endregion

		#region Exceptions

		#endregion

		#region Events for all Tasks

		void taskStarted(object sender, EventArgs e)
		{
			//MainWindow.Invoke(
			//    (sender1, e1) =>
			//    {
			//        if (progBars[(sender as Task).TaskID] != null)
			//        {
			//            progBars[(sender as Task).TaskID].Show();
			//        }
			//    });
		}

		void taskCompleted(object sender, Task.TaskCompleteEventArgs e)
		{
			//MainWindow.Invoke(
			//    (sender1, e1) =>
			//    {
			//        progBarBox.Remove(progBars[(sender as Task).TaskID]);
			//        progBars[(sender as Task).TaskID].Dispose();
			//    });
		}

		void taskProgressChange(object sender, Task.ProgressChangeEventArgs e)
		{
			//MainWindow.Invoke(
			//    (sender1, e1) =>
			//    {
			//        float progFraction = ((float)e.Progress) / 100;
			//        if (progFraction > 1)
			//        {
			//            Console.WriteLine(string.Format("Warning: Progress fraction " +
			//                    "({0}) is greater than the maximum value (1)", progFraction));
			//            progFraction = 1;
			//        }
					
			//        progBars[(sender as Task).TaskID].Fraction = progFraction;
			//    });
		}

		void taskStatusChange(object sender, Task.TaskStatusEventArgs e)
		{
			//Gtk.Application.Invoke(
			//    (sender1, e1) =>
			//    {
			//        progBars[(sender as Task).TaskID].Text = e.Status;
			//    });
		}

		#endregion

		#endregion

		#region Instances

		public Instance SelectedInst
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		#endregion

		void ToolkitNotSupported()
		{
			throw new NotImplementedException("This window toolkit is not implemented yet.");
		}
	}
}
