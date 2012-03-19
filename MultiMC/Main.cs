using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

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

		public IImageList InstIconList
		{
			get;
			protected set;
		}

		public Main()
		{
			GUIManager.Create();
			try
			{
				GUIManager.Main.Initialize();

				MainWindow = GUIManager.Main.MainWindow();
				InstIconList = GUIManager.Main.LoadInstIcons();
			}
			catch (FileNotFoundException e)
			{
				if (e.Message.ToLower().Contains("system.windows.forms"))
				{
					string helpMsg =
						"You're missing the System.Windows.Forms assembly. (version 4.0)";
					string errorFileMsg = string.Format(
@"Oh no!

You seem to be missing one of MultiMC's prerequisites. 
Please make sure you have System.Windows.Forms (version 4.0) installed correctly and then try again.

Details:
{0}
", e.ToString());
					if (OSUtils.OS == OSEnum.Linux)
					{
						helpMsg =
							"Try installing the 'libmono-system-windows-forms4.0-cil' " +
							"package. " +
							"(sudo apt-get install libmono-system-windows-forms4.0-cil)";
						errorFileMsg = string.Format(
@"Oh no!

You seem to be missing one of MultiMC's prerequisites. 
Please make sure you've installed the 'libmono-system-windows-forms4.0-cil' package.

To do this, simply run this command in terminal:
	sudo apt-get install libmono-system-windows-forms4.0-cil

Details:
{0}
", e.ToString());
					}
					string errorMsg = string.Format("Couldn't load WinForms.\n{0}", helpMsg);

					Console.WriteLine(errorMsg);
					DebugUtils.WriteError(errorFileMsg);

					DebugUtils.FatalErrorDialog("MultiMC requires the package " +
						"'libmono-system-windows-forms4.0-cil'");

					Environment.Exit(1);
				}
				else throw;
			}

			MainWindow.Title = string.Format("MultiMC {0} for {1}", 
				AppUtils.GetVersion().ToString(2), OSUtils.OSName);

			MainWindow.DefaultPosition = DefWindowPosition.CenterScreen;

			MainWindow.NewInstClicked += new EventHandler(NewInstClicked);
			MainWindow.RefreshClicked += (o, args) => MainWindow.LoadInstances();
			MainWindow.ViewFolderClicked += new EventHandler(ViewFolderClicked);

			MainWindow.SettingsClicked += new EventHandler(SettingsClicked);
			MainWindow.CheckUpdatesClicked += new EventHandler(UpdateClicked);

			MainWindow.AboutClicked += new EventHandler(AboutClicked);


			MainWindow.InstanceLaunched += new EventHandler<InstActionEventArgs>(LaunchInstance);

			MainWindow.ChangeIconClicked += new EventHandler<InstActionEventArgs>(ChangeIconClicked);
			MainWindow.EditNotesClicked += new EventHandler<InstActionEventArgs>(EditNotesClicked);

			MainWindow.EditModsClicked += new EventHandler<InstActionEventArgs>(EditModsClicked);
			MainWindow.RebuildJarClicked += new EventHandler<InstActionEventArgs>(RebuildClicked);
			MainWindow.ViewInstFolderClicked += new EventHandler<InstActionEventArgs>(ViewInstFolderClicked);

			MainWindow.DeleteInstClicked += new EventHandler<InstActionEventArgs>(DeleteInstClicked);

			MainWindow.ImageList = InstIconList;

			MainWindow.LoadInstances();

			MainWindow.Shown += new EventHandler(MainWindow_Shown);

			// Initialize problem detection
			try
			{
				Console.WriteLine("Initializing problem detection system...");
				ProblemDetection.Problems.InitProblems();
			}
			catch (System.Reflection.ReflectionTypeLoadException e)
			{
				Console.WriteLine("Problem detection failed to initialize:\n{0}", e);
			}
		}

		void MainWindow_Shown(object sender, EventArgs e)
		{
			if (!File.Exists("Ionic.Zip.Reduced.dll"))
			{
				Downloader dotNetZipDL = new Downloader(
					"Ionic.Zip.Reduced.dll",
					Properties.Resources.DotNetZipURL,
					"Downloading DotNetZip");
				MainWindow.Invoke((o, args) => StartTask(dotNetZipDL));
			}

			if (AppSettings.Main.AutoUpdate)
			{
				MainWindow.Invoke((o, args) => DoUpdateCheck());
			}
		}

		#region Menu Bar Events

		void AboutClicked(object sender, EventArgs e)
		{
			IDialog aboutDlg = GUIManager.Main.AboutDialog();
			
			aboutDlg.Parent = MainWindow;
			aboutDlg.DefaultPosition = DefWindowPosition.CenterParent;

			aboutDlg.Run();
		}

		void UpdateClicked(object sender, EventArgs e)
		{
			DoUpdateCheck();
		}

		void updateCheckComplete(object sender, Task.TaskCompleteEventArgs e)
		{
			Updater updater = sender as Updater;
			if (updater == null)
			{
				Console.WriteLine("Update check failed! Sender is null.");
				return;
			}
			DownloadNewVersion();
		}

		void NewInstClicked(object sender, EventArgs e)
		{
			IAddInstDialog addInstDlg = GUIManager.Main.AddInstDialog();

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
			IDialog settingsWindow = GUIManager.Main.SettingsWindow();
			settingsWindow.Parent = MainWindow;
			settingsWindow.DefaultPosition = DefWindowPosition.CenterParent;
			settingsWindow.Response += (o, args) =>
				{
					if (args.Response != DialogResponse.Other)
					{
						settingsWindow.Close();
					}
				};
			settingsWindow.Run();
		}
		#endregion

		#region Instance Menu

		void ChangeIconClicked(object sender, InstActionEventArgs e)
		{
			IChangeIconDialog changeIconDlg = GUIManager.Main.ChangeIconDialog();

			changeIconDlg.ImageList = this.InstIconList;
			changeIconDlg.Response += (o, args) =>
				{
					if (args.Response == DialogResponse.OK)
					{
						SelectedInst.IconKey = changeIconDlg.ChosenIconKey;
					}
					changeIconDlg.Close();
				};
			changeIconDlg.Parent = MainWindow;
			changeIconDlg.DefaultPosition = DefWindowPosition.CenterParent;
			changeIconDlg.Run();
			MainWindow.LoadInstances();
		}

		void EditNotesClicked(object sender, InstActionEventArgs e)
		{
			INotesDialog noteDlg = GUIManager.Main.NotesDialog();

			noteDlg.Notes = SelectedInst.Notes;
			noteDlg.Response += (o, args) =>
				{
					if (args.Response == DialogResponse.OK)
					{
						SelectedInst.Notes = noteDlg.Notes;
					}
					noteDlg.Close();
				};
			noteDlg.Run();
		}

		void EditModsClicked(object sender, InstActionEventArgs e)
		{
			IEditModsDialog editModsDlg = GUIManager.Main.EditModsDialog(SelectedInst);

			editModsDlg.LoadModList();
			editModsDlg.Response += (o, args) =>
				{
					if (args.Response == DialogResponse.OK)
						editModsDlg.SaveModList();
					editModsDlg.Close();
				};
			editModsDlg.Parent = MainWindow;
			editModsDlg.DefaultPosition = DefWindowPosition.CenterParent;
			editModsDlg.Run();
		}

		void RebuildClicked(object sender, InstActionEventArgs e)
		{
			StartTask(new Modder(SelectedInst));
		}

		void ViewInstFolderClicked(object sender, InstActionEventArgs e)
		{
			Process.Start(SelectedInst.RootDir);
		}

		void DeleteInstClicked(object sender, InstActionEventArgs e)
		{
			throw new NotImplementedException();
		}

		#endregion

		public void Run()
		{
			GUIManager.Main.Run(MainWindow);
		}

		#region Task System

		private void StartTask(Task task)
		{
			task.Completed += taskCompleted;
			task.ProgressChange += taskProgressChange;
			task.StatusChange += taskStatusChange;
			task.ErrorMessage += TaskErrorMessage;
			MainWindow.TaskList.Add(task);
			task.Start();
		}

		void TaskErrorMessage(object sender, Task.ErrorMessageEventArgs e)
		{
			MainWindow.Invoke(
				(sender1, e1) =>
				{
					MessageDialog.Show(MainWindow, e.Message, "Error");
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
				MessageDialog.Show(MainWindow,
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
					string updateMsg = "Updates have been downloaded. " +
						"Would you like to install them?";
					if (updateVersion != null)
						updateMsg =  string.Format("Version {0} has been downloaded. " +
							"Would you like to install it now?", updateVersion);


					string updatestr = (updateVersion != null ? updateVersion.ToString() : "");
					if (string.IsNullOrEmpty(updatestr))
					{
						updatestr = "";
						updateMsg = "MultiMC has downloaded updates, would you like to install them?";
					}

					DialogResponse response = MessageDialog.Show(
						MainWindow, updateMsg, "Update MultiMC?", MessageButtons.YesNo);

					if (response == DialogResponse.Yes)
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

		#region Events for all Tasks

		void taskCompleted(object sender, Task.TaskCompleteEventArgs e)
		{
			MainWindow.TaskList.Remove(sender as Task);
		}

		void taskProgressChange(object sender, Task.ProgressChangeEventArgs e)
		{
			
		}

		void taskStatusChange(object sender, Task.TaskStatusEventArgs e)
		{
			
		}

		#endregion

		#endregion

		#region Instances

		void LaunchInstance(object sender, InstActionEventArgs e)
		{
			StartInstance(e.Inst);
		}

		public Instance SelectedInst
		{
			get { return MainWindow.SelectedInst; }
		}

		#endregion

		void ToolkitNotSupported()
		{
			throw new NotImplementedException("This window toolkit is not implemented yet.");
		}

		public void StartInstance(Instance inst)
		{
			string message = "";
			DoLogin(LoginComplete, inst, message, inst.CanPlayOffline);
		}

		/// <summary>
		/// Opens a dialog that allows users to log in.
		/// </summary>
		/// <remarks>
		/// If the user fails to log in, this method will reopen the login dialog
		/// with an error message. The done delegate is not called until the user
		/// either logs in successfully or clicks cancel.
		/// </remarks>
		/// <param name="done">Delegate that is invoked when the user logs in
		/// successfully.</param>
		/// <param name="inst">The instance that is being launched.</param>
		/// <param name="message">An error message to be displayed on the login dialog.</param>
		/// <param name="canplayOffline">True if the user can play this instance offline.</param>
		private void DoLogin(LoginCompleteHandler done,
							 Instance inst,
							 string message = "",
							 bool canplayOffline = false)
		{
			string username = "";
			string password = "";
			ReadUserInfo(out username, out password);

			ILoginDialog loginDlg = null;
			switch (Program.Toolkit)
			{
			case WindowToolkit.WinForms:
				loginDlg = new WinGUI.LoginForm(message);
				break;
			}
			loginDlg.Parent = MainWindow;
			loginDlg.DefaultPosition = DefWindowPosition.CenterParent;

			if (!string.IsNullOrEmpty(username))
			{
				loginDlg.RememberUsername = true;
				loginDlg.Username = username;
			}
			if (!string.IsNullOrEmpty(password))
			{
				loginDlg.RememberPassword = true;
				loginDlg.Password = password;
			}
			loginDlg.Response += (o, args) =>
			{
				if (args.Response == DialogResponse.OK)
				{
					string parameters = string.Format(
						"user={0}&password={1}&version=1337",
						Uri.EscapeDataString(loginDlg.Username),
						Uri.EscapeDataString(loginDlg.Password), 13);

					WriteUserInfo((loginDlg.RememberUsername ? loginDlg.Username : ""),
								  (loginDlg.RememberPassword ? loginDlg.Password : ""));

					// Start a new thread and post the login info to login.minecraft.net
					SimpleTask loginTask = new SimpleTask(() =>
					{
						string reply = "";
						bool postFailed = false;
						try
						{
							if (loggingIn)
								return;
							loggingIn = true;
							reply = AppUtils.ExecutePost("https://login.minecraft.net/",
									parameters);
						}
						catch (System.Net.WebException e)
						{
							postFailed = true;
							reply = e.Message;
						}
						finally
						{
							loggingIn = false;
						}

						// If the login failed
						if (!reply.Contains(":") || postFailed)
						{
							// Translate the error message to a more user friendly wording
							string errorMessage = reply;
							switch (reply.ToLower())
							{
							case "bad login":
								errorMessage = "Invalid username or password.";
								break;
							case "old version":
								errorMessage = "Invalid launcher version.";
								break;
							default:
								errorMessage = "Login failed: " + reply;
								break;
							}

							// Unable to resolve hostname.
							if (reply.ToLower().Contains("name could not be resolved"))
							{
								errorMessage = string.Format(
									"Couldn't connect to login.minecraft.net, " +
									"please connect to the internet or use offline mode.");
							}

							// Error
							MainWindow.Invoke((sender, e) =>
								DoLogin(done, inst, errorMessage));
						}

						// If the login succeeded
						else
						{
							string[] responseValues = reply.Split(':');

							// The response must have 4 values or it's invalid
							if (responseValues.Length != 4)
							{
								// Error
								MainWindow.Invoke((sender, e) =>
									DoLogin(done, inst,
										"Got an invalid response from server", canplayOffline));
							}
							// Now we can finally return our login info.
							else
							{
								LoginInfo info = new LoginInfo(responseValues,
															   loginDlg.ForceUpdate);
								done(info, inst);
							}
						}
					}, "Logging in...");

					if (!loggingIn)
					{
						StartTask(loginTask);
					}
				}
				else if (args.Response == DialogResponse.No)
				{
					// Play offline
					done(new LoginInfo(null, false, false), inst);
				}
				else
				{
					// Login cancelled
					done(new LoginInfo(), inst);
				}
			};
			loginDlg.Run();
		}

		private void LoginComplete(LoginInfo info, Instance inst)
		{
			string mainGameUrl = "minecraft.jar";
			if (!info.Cancelled)
			{
				GameUpdater updater =
						new GameUpdater(inst,
										info.LatestVersion,
										mainGameUrl,
										info.ForceUpdate);

				EventHandler startDelegate = new EventHandler((e, args) =>
					{
						MainWindow.Visible = false;
						inst.Launch(info.Username, info.SessionID);

						IConsoleWindow cwin = GUIManager.Main.ConsoleWindow(inst);

						cwin.Parent = MainWindow;
						cwin.DefaultPosition = DefWindowPosition.CenterParent;

						cwin.ConsoleClosed += (e2, args2) =>
							{
								MainWindow.Visible = true;
							};

						cwin.Show();
					});

				updater.Completed += (sender, e) =>
					{
						if (inst.NeedsRebuild)
						{
							MainWindow.Invoke((sender2, e2) =>
								{
									RebuildMCJar(inst).Completed += (sender3, e3) =>
												MainWindow.Invoke(startDelegate);
								});
						}
						else
							MainWindow.Invoke(startDelegate);
					};

				StartTask(updater);
			}
		}

		private delegate void LoginCompleteHandler(LoginInfo info, Instance inst);

		bool loggingIn;

		private void ReadUserInfo(out string username, out string password)
		{
			try
			{
				if (!File.Exists(Properties.Resources.LastLoginFileName))
				{
					username = password = "";
					return;
				}

				using (SHA384 sha = SHA384.Create())
				{
					byte[] hash = sha.ComputeHash(
						System.Text.ASCIIEncoding.ASCII.GetBytes(Properties.Resources.LastLoginKey));

					byte[] key = new byte[32];
					byte[] IV = new byte[16];

					Array.Copy(hash, key, key.Length);
					Array.ConstrainedCopy(hash, key.Length, IV, 0, IV.Length);

					using (Rijndael rijAlg = Rijndael.Create())
					{
						rijAlg.Key = key;
						rijAlg.IV = IV;

						ICryptoTransform decryptor = rijAlg.CreateDecryptor(key, IV);

						using (FileStream fsDecrypt = File.OpenRead(Properties.Resources.LastLoginFileName))
						{
							CryptoStream csDecrypt =
								new CryptoStream(fsDecrypt, decryptor, CryptoStreamMode.Read);
							StreamReader srDecrypt = new StreamReader(csDecrypt);
							string str = srDecrypt.ReadToEnd();
							string[] data = str.Split(':');
							if (data.Length >= 1)
								username = data[0];
							else
								username = "";

							if (data.Length >= 2)
								password = data[1];
							else
								password = "";
						}
					}
				}
			}
			catch (IndexOutOfRangeException)
			{
				username = "";
				password = "";
				File.Delete(Properties.Resources.LastLoginFileName);
			}
			catch (CryptographicException)
			{
				username = "";
				password = "";
				File.Delete(Properties.Resources.LastLoginFileName);
			}
		}

		private void WriteUserInfo(string username, string password)
		{
			using (SHA384 sha = SHA384.Create())
			{
				byte[] hash = sha.ComputeHash(
					System.Text.ASCIIEncoding.ASCII.GetBytes(Properties.Resources.LastLoginKey));

				byte[] key = new byte[32];
				byte[] IV = new byte[16];

				Array.Copy(hash, key, key.Length);
				Array.ConstrainedCopy(hash, key.Length, IV, 0, IV.Length);

				using (Rijndael rijAlg = Rijndael.Create())
				{
					rijAlg.Key = key;
					rijAlg.IV = IV;

					ICryptoTransform encryptor = rijAlg.CreateEncryptor(key, IV);

					try
					{
						using (FileStream fsEncrypt = File.Open(Properties.Resources.LastLoginFileName,
																FileMode.Create))
						{
							using (CryptoStream csEncrypt =
								new CryptoStream(fsEncrypt, encryptor, CryptoStreamMode.Write))
							{
								using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
								{
									swEncrypt.Write(string.Format("{0}:{1}", username, password));
								}
							}
						}
					}
					catch (IOException)
					{
						Console.WriteLine("Failed to write lastlogin file.");
						return;
					}
				}
			}
		}

		private class LoginInfo
		{
			#region Properties
			public string LatestVersion
			{
				get;
				private set;
			}

			public string DownloadTicket
			{
				get;
				private set;
			}

			public string Username
			{
				get;
				private set;
			}

			public string SessionID
			{
				get;
				private set;
			}

			public bool Cancelled
			{
				get;
				private set;
			}

			public bool Offline
			{
				get;
				private set;
			}

			public bool ForceUpdate
			{
				get;
				private set;
			}
			#endregion

			public LoginInfo(string[] values = null, bool forceUpdate = false, bool cancel = true)
			{
				ForceUpdate = forceUpdate;
				if (values == null)
				{
					values = new[] { "", "", "", "" };
					Cancelled = cancel;
					Offline = !cancel;
				}
				LatestVersion = values[0];
				DownloadTicket = values[1];
				Username = values[2];
				SessionID = values[3];
			}

			//			public LoginInfo(string errorMessage)
			//			{
			//				ErrorMessage = errorMessage;
			//				Failed = true;
			//			}
		}
	}
}
