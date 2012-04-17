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
//
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
using MultiMC.Mods;

namespace MultiMC
{
	/// <summary>
	/// The main class.
	/// </summary>
	public class Main
	{
		bool customIconLoadError;

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
			GUIManager.Main.Initialize();

			MainWindow = GUIManager.Main.MainWindow();

			MainWindow.Title = string.Format("MultiMC Beta {0} for {1}", 
				AppUtils.GetVersion().ToString(2), OSUtils.OSName);

			MainWindow.DefaultPosition = DefWindowPosition.CenterScreen;

			// Main toolbar
			MainWindow.NewInstClicked += NewInstClicked;
			MainWindow.RefreshClicked += (o, args) => MainWindow.LoadInstances();
			MainWindow.ViewFolderClicked += ViewFolderClicked;

			MainWindow.SettingsClicked += SettingsClicked;
			MainWindow.CheckUpdatesClicked += UpdateClicked;

			MainWindow.AboutClicked += AboutClicked;

			// Instance context menu
			MainWindow.InstanceLaunched += LaunchInstance;

			MainWindow.ChangeIconClicked += ChangeIconClicked;
			MainWindow.EditNotesClicked += EditNotesClicked;

			MainWindow.ManageSavesClicked += ManageSavesClicked;
			MainWindow.EditModsClicked += EditModsClicked;
			MainWindow.RebuildJarClicked += RebuildClicked;
			MainWindow.ViewInstFolderClicked += ViewInstFolderClicked;

			MainWindow.DeleteInstClicked += DeleteInstClicked;
			// on linux, provide the possiblity to nuke OpenAL libs
			if(OSUtils.OS == OSEnum.Linux)
			{
				MainWindow.RemoveOpenALClicked += RemoveOpenALClicked;
			}

			// Try to load the icon list.
			int tries = 0;
			bool keepTryingToLoadIcons = true;
			customIconLoadError = false;
			while (tries < 2 && keepTryingToLoadIcons)
			{
				try
				{
					InstIconList = GUIManager.Main.LoadInstIcons(tries == 0);
					MainWindow.ImageList = InstIconList;

					// If none of the above return false, stop trying to load icons.
					keepTryingToLoadIcons = false;
				}
				catch (ArgumentException)
				{
					keepTryingToLoadIcons = true;
					customIconLoadError = true;
				}
				finally
				{
					tries++;
				}
			}

			MainWindow.LoadInstances();

			MainWindow.Closed += new EventHandler(MainWindow_Closed);

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

		void MainWindow_Closed(object sender, EventArgs e)
		{
			Task[] killTasks = new Task[MainWindow.TaskList.Count];
			MainWindow.TaskList.CopyTo(killTasks, 0);
			foreach (Task task in killTasks)
			{
				task.Cancel();
			}
		}

		void OnStartup()
		{
			Console.WriteLine("Running startup tasks.");

			// No longer needed.
			//if (!File.Exists("Ionic.Zip.Reduced.dll"))
			//{
			//    Downloader dotNetZipDL = new Downloader(
			//        "Ionic.Zip.Reduced.dll",
			//        Properties.Resources.DotNetZipURL,
			//        "Downloading DotNetZip");
			//    MainWindow.Invoke((o, args) => StartTask(dotNetZipDL));
			//}

			Downloader mcVersionsDL = new Downloader(
				Properties.Resources.MCVersionFile,
				Properties.Resources.MCVersionFileDL,
				"Downloading version info file", 30);
			mcVersionsDL.QuietMode = true;
			MainWindow.Invoke((o, args) => StartTask(mcVersionsDL));

			if (AppSettings.Main.AutoUpdate)
			{
				MainWindow.Invoke((o, args) => DoUpdateCheck());
			}
		}

		#region Menu Bar Events

		void AboutClicked(object sender, EventArgs e)
		{
			IAboutDialog aboutDlg = GUIManager.Main.AboutDialog();
			
			aboutDlg.Parent = MainWindow;
			aboutDlg.DefaultPosition = DefWindowPosition.CenterParent;
			aboutDlg.ShowInTaskbar = false;

			aboutDlg.ChangelogClicked += ViewChangelogClicked;
				//(o, args) =>
				//{
				//    IDialog changelogDlg = GUIManager.Main.ChangelogDialog();
				//    changelogDlg.Parent = this.MainWindow;
				//    changelogDlg.DefaultPosition = DefWindowPosition.CenterParent;
				//    changelogDlg.ShowInTaskbar = false;

				//    changelogDlg.Run();
				//};

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

			addInstDlg.Parent = MainWindow;
			addInstDlg.DefaultPosition = DefWindowPosition.CenterParent;
			addInstDlg.ShowInTaskbar = false;
			addInstDlg.MoveToDefPosition();
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
			ISettingsDialog settingsWindow = GUIManager.Main.SettingsWindow();
			settingsWindow.Parent = MainWindow;
			settingsWindow.DefaultPosition = DefWindowPosition.CenterParent;
			settingsWindow.ShowInTaskbar = false;

			settingsWindow.Response += (o, args) =>
				{
					if (args.Response != DialogResponse.Other)
					{
						settingsWindow.Close();
					}
				};
			settingsWindow.Run();

			if (settingsWindow.ForceUpdate)
			{
				DownloadNewVersion();
			}
		}
		#endregion

		#region Instance Menu

		void ChangeIconClicked(object sender, InstActionEventArgs e)
		{
			IChangeIconDialog changeIconDlg = GUIManager.Main.ChangeIconDialog();

			changeIconDlg.ImageList = this.InstIconList;
			changeIconDlg.ShowInTaskbar = false;

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
			noteDlg.ShowInTaskbar = false;

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

		void ManageSavesClicked(object sender, InstActionEventArgs e)
		{
			ISaveManagerDialog manageSaveDialog = GUIManager.Main.SaveManagerDialog();
			manageSaveDialog.LoadSaveList(e.Inst);
			manageSaveDialog.Parent = MainWindow;
			manageSaveDialog.DefaultPosition = DefWindowPosition.CenterParent;

			manageSaveDialog.BackupSaveClicked += BackupSaveClicked;
			manageSaveDialog.RestoreSaveClicked += RestoreSaveClicked;

			manageSaveDialog.Run();
		}

		void BackupSaveClicked(object sender, EventArgs e)
		{
			WorldSave save = (sender as ISaveManagerDialog).SelectedSave;

			if (save == null)
				return;

			ITextInputDialog inputDialog = GUIManager.Main.TextInputDialog(
				"Please enter a name for your backup.", 
				"Unnamed MultiMC Backup");
			inputDialog.Title = "Backup Save";

			inputDialog.Shown += (o, args) => inputDialog.HighlightText();

			inputDialog.Response += (o, args) =>
				{
					if (args.Response == DialogResponse.OK)
						StartModalTask(new BackupTask(save, ""), inputDialog);
				};

			inputDialog.Run();
		}

		void RestoreSaveClicked(object sender, EventArgs e)
		{
			WorldSave save = (sender as ISaveManagerDialog).SelectedSave;

			if (save == null)
				return;

			IRestoreBackupDialog restoreDialog = GUIManager.Main.RestoreBackupDialog();
			restoreDialog.Parent = sender as IWindow;
			restoreDialog.DefaultPosition = DefWindowPosition.CenterParent;

			restoreDialog.Response += (o, args) =>
				{
					if (args.Response == DialogResponse.OK)
					{
						StartModalTask(new RestoreTask(save, restoreDialog.SelectedHash));
					}
				};

			restoreDialog.LoadBackupList(save);
			restoreDialog.Run();
		}

		void EditModsClicked(object sender, InstActionEventArgs e)
		{
			IEditModsDialog editModsDlg = GUIManager.Main.EditModsDialog(SelectedInst);

			editModsDlg.LoadModList();
			editModsDlg.ShowInTaskbar = false;

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
			RebuildMCJar(SelectedInst);
		}

		void ViewInstFolderClicked(object sender, InstActionEventArgs e)
		{
			Process.Start(SelectedInst.RootDir);
		}

		void DeleteInstClicked(object sender, InstActionEventArgs e)
		{
			IDialog deleteDialog = GUIManager.Main.DeleteDialog();
			deleteDialog.ShowInTaskbar = false;

			deleteDialog.Response += (o, args) =>
				{
					if (args.Response == DialogResponse.OK)
					{
						try
						{
							Directory.Delete(SelectedInst.RootDir, true);
						}
						catch (IOException)
						{
							try
							{
								Directory.Delete(SelectedInst.RootDir, true);
							}
							catch (UnauthorizedAccessException)
							{

							}
						}
						catch (UnauthorizedAccessException)
						{

						}
						MainWindow.LoadInstances();
					}
					deleteDialog.Close();
				};
			deleteDialog.Parent = MainWindow;
			deleteDialog.DefaultPosition = DefWindowPosition.CenterParent;
			deleteDialog.Run();
		}
		
		void RemoveOpenALClicked(object sender, InstActionEventArgs e)
		{
			DialogResponse reply = MessageDialog.Show(MainWindow,
			"This will delete the OpenAL libraries distributed with minecraft." +
			"It can fix sound issues, but you should have the OpenAL installed in your system first." +
			"Are you sure you want to do this?",
			"Really delete OpenAL libraries?", MessageButtons.YesNo);

			if (reply == DialogResponse.Yes)
			{
				String openal32 = Path.Combine( ".", SelectedInst.MinecraftDir , "bin", "natives", "libopenal.so" );
				String openal64 = Path.Combine( ".", SelectedInst.MinecraftDir , "bin", "natives", "libopenal64.so" );
				openal32 = Path.GetFullPath(openal32);
				openal64 = Path.GetFullPath(openal64);
				System.Console.WriteLine(openal32);
				System.Console.WriteLine(openal64);
				if(File.Exists(openal32))
	          	{
					File.Delete(openal32);
				}
				if(File.Exists(openal64))
				{
					File.Delete(openal64);
				}
			}
		}
		#endregion

		public void Run()
		{
			MainWindow.Invoke((o, args) =>
				{
					if (DirectLaunch)
					{
						MainWindow.Visible = false;

						Instance launchInst = MainWindow.InstanceList.FirstOrDefault(
							inst => inst.Name == DirectLaunchInstName);

						if (launchInst != null)
						{
							StartInstance(launchInst);
						}
						else
						{
							MessageDialog.Show(null,
								string.Format("No instance with the name {0} was found.",
									DirectLaunchInstName),
								"Direct Launch Failed");
							Environment.Exit(0);
						}
					}
					else
					{
						if (customIconLoadError)
						{
							DialogResponse reply = MessageDialog.Show(MainWindow,
								"Couldn't load your custom icons because one or more " +
								"of them are invalid or corrupt. This can be " +
								"fixed by deleting them, however you will lose " +
								"all custom icons that you've added.\nWould you " +
								"like to delete all custom icons?",
								"Failed to load icons.", MessageButtons.YesNo);

							if (reply == DialogResponse.Yes)
							{
								Directory.Delete("icons", true);
							}
						}

						OnStartup();
					}
				});

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

		private void StartModalTask(Task task, IWindow parentWindow = null)
		{
			task.ErrorMessage += TaskErrorMessage;
			ITaskDialog taskDlg = GUIManager.Main.TaskDialog(task);
			if (!DirectLaunch)
			{
				taskDlg.Parent = parentWindow != null ? parentWindow : MainWindow;
				taskDlg.ShowInTaskbar = false;
			}
			taskDlg.DefaultPosition = DefWindowPosition.CenterScreen;
			task.Start();
			taskDlg.Run();
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
			if (!File.Exists(inst.MCJar))
			{
				MessageDialog.Show(MainWindow,
								"You must run the " +
								"instance at least " +
								"once before installing mods.",
								"Error");
				return null;
			}

			string instVersion = inst.Version;
			Console.WriteLine("Checking mod versions for minecraft {0} instance.",
				instVersion);
			foreach (Mod mod in inst.InstMods)
			{
				string modVersion = null;
				if (mod.MCVersion != null)
					modVersion = MCVersionMap.VersionMap[mod.MCVersion];

				if (string.IsNullOrEmpty(modVersion))
				{
					if (!string.IsNullOrEmpty(mod.MCVersion))
					{
						Console.WriteLine("Unknown Minecraft version: {0}", mod.MCVersion);
					}

					continue;
				}
				else if (modVersion != instVersion)
				{
					DialogResponse response = MessageDialog.Show(MainWindow,
						string.Format("One of your mods ({0} for Minecraft {1}) " + 
							"is not compatible with " +
							"this version of minecraft.\n" + 
							"Continue installing it?",
							mod.Name, mod.MCVersion),
							"Warning", MessageButtons.OkCancel);

					if (response == DialogResponse.Cancel)
					{
						return null;
					}
				}
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

					IUpdateDialog updateDialog = GUIManager.Main.UpdateDialog();
					updateDialog.Parent = MainWindow;
					updateDialog.ShowInTaskbar = false;

					updateDialog.DefaultPosition = DefWindowPosition.CenterParent;

					updateDialog.Message = updateMsg;

					updateDialog.Response += (o, args) =>
						{
							if (args.Response == DialogResponse.Yes)
							{
								MainWindow.Invoke((o2, args2) => CloseForUpdates());
							}
						};

					updateDialog.ViewChangelogClicked += ViewChangelogClicked;

					updateDialog.Run();
				});
		}

		void ViewChangelogClicked(object sender, EventArgs e)
		{
			IDialog changelogDialog = GUIManager.Main.ChangelogDialog();
			changelogDialog.DefaultPosition = DefWindowPosition.CenterParent;
			changelogDialog.ShowInTaskbar = false;

			if (sender is IDialog)
				changelogDialog.Parent = sender as IDialog;

			changelogDialog.Run();
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

		public void StartInstance(Instance inst)
		{
			string message = "";
			DoLogin(
				(LoginInfo info, Instance instance) =>
					MainWindow.Invoke((o, args) => LoginComplete(info, instance)), 
				inst, message, inst.CanPlayOffline);
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

			ILoginDialog loginDlg = GUIManager.Main.LoginDialog(message);
			loginDlg.Parent = (DirectLaunch ? null : MainWindow);
			loginDlg.DefaultPosition = DefWindowPosition.CenterParent;
			loginDlg.ShowInTaskbar = DirectLaunch;

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
						MainWindow.Invoke((o2, args2) =>
							StartModalTask(loginTask));
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
				Console.WriteLine("Version: {0}", info.LatestVersion);
				GameUpdater updater =
						new GameUpdater(inst,
										info.LatestVersion,
										mainGameUrl,
										info.ForceUpdate);

				EventHandler startDelegate = new EventHandler((e, args) =>
					{
						if (!DirectLaunch)
							MainWindow.Visible = false;

						inst.Launch(info.Username, info.SessionID);

						IConsoleWindow cwin = GUIManager.Main.ConsoleWindow(inst);

						cwin.DefaultPosition = DefWindowPosition.CenterScreen;

						cwin.ConsoleClosed += (e2, args2) =>
							{
								if (DirectLaunch)
								{
									Environment.Exit(0);
								}
								else
								{
									MainWindow.Invoke((e3, args3) =>
										MainWindow.Visible = true);
								}
							};

						cwin.Show();
					});

				updater.Completed += (sender, e) =>
					{
						if (inst.NeedsRebuild)
						{
							MainWindow.Invoke((sender2, e2) =>
								{
									Modder modder = RebuildMCJar(inst);

									if (modder == null)
										MainWindow.Invoke(startDelegate);
									else
										modder.Completed += (sender3, e3) =>
											MainWindow.Invoke(startDelegate);
								});
						}
						else
							MainWindow.Invoke(startDelegate);
					};

				MainWindow.Invoke((o2, args2) =>
					StartModalTask(updater));
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

		public bool DirectLaunch
		{
			get;
			private set;
		}

		public string DirectLaunchInstName
		{
			get;
			private set;
		}

		public void Run(string instName)
		{
			DirectLaunch = true;
			DirectLaunchInstName = instName;

			Run();
		}
	}
}
