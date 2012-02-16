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
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using Gtk;
using Gdk;

using MultiMC.Data;
using MultiMC.Tasks;

namespace MultiMC
{
	public partial class MainWindow : Gtk.Window
	{
		private ListStore instList;
		private Box progBarBox;

		public MainWindow() :
			base(Gtk.WindowType.Toplevel)
		{
			this.Build();

			string osString = "Unknown OS";

			if (OSUtils.Windows)
				osString = "Windows";
			else if (OSUtils.Linux)
				osString = "Linux";
			else if (OSUtils.MacOSX)
				osString = "Mac OS X";

			if (!Directory.Exists(Resources.InstDir))
				Directory.CreateDirectory(Resources.InstDir);

			if (!File.Exists(AppSettings.Main.JavaPath))
				AppSettings.Main.JavaPath = OSUtils.FindJava();

			Title = string.Format("MultiMC BETA {0} for {1}",
								  Resources.VersionString,
								  osString);
			Icon = Pixbuf.LoadFromResource("MainIcon");

			instList = new ListStore(typeof(string), typeof(Instance), typeof(Gdk.Pixbuf));

			instIconView.Model = instList;
			instIconView.TextColumn = 0;
			instIconView.PixbufColumn = 2;
			instIconView.Cells[0] = new CellRendererText();

			CellRendererText textCell = (instIconView.Cells[0] as CellRendererText);
			textCell.Editable = true;
			textCell.Width = 64;
			textCell.Alignment = Pango.Alignment.Center;
			textCell.Edited += (object o, EditedArgs args) =>
			{
				using (TreePath tp = new TreePath(args.Path))
				{
					instIconView.SelectPath(tp);
					if (Instance.NameIsValid(args.NewText))
					{
						SelectedInst.Name = args.NewText;
						LoadInstances();
					}
				}
			};

			InitMenu();
			//			InitDND();

			progBarBox = new VBox();
			windowbox.PackEnd(progBarBox, false, true, 0);

			progBarBox.ShowAll();

			LoadInstances();

			/* 
			 * Run startup tasks such as downloading DotNetZip 
			 * and checking for updates.
			 */
			RunStartupTasks();

			HintDialog.ShowHint(this, Hint.WelcomeHint);
		}

		#region Startup Tasks

		/// <summary>
		/// Checks for necessary files (such as the minecraft launcher 
		/// and DotNetZip's DLL and downloads them if they don't exist.
		/// </summary>
		private void RunStartupTasks()
		{
			// Get DotNetZip
			CheckDownloadFile("Ionic.Zip.Reduced.dll",
							  Resources.DotNetZipURL,
							  "Downloading DotNetZip...");


			if (AppSettings.Main.AutoUpdate)
				DoUpdateCheck();
		}

		/// <summary>
		/// If the given file doesn't exist, starts a downloader and returns it
		/// Otherwise, returns null
		/// </summary>
		/// <returns>
		/// The downloader or null if the file already exists.
		/// </returns>
		private Downloader CheckDownloadFile(string dest, string url, string message)
		{
			if (!File.Exists(dest))
			{
				Console.WriteLine("Downloading {0}.", dest);
				using (Downloader downloader = new Downloader(dest, url, message))
				{
					StartTask(downloader);
					return downloader;
				}
			}
			else
				return null;
		}

		#endregion

		#region Buttons

		protected void OnDeleteEvent(object o, Gtk.DeleteEventArgs args)
		{
			Application.Quit();
		}

		protected void OnNewClicked(object sender, System.EventArgs e)
		{
			using (NewInstanceDialog newDlg = new NewInstanceDialog(this))
			{
				newDlg.ParentWindow = this.GdkWindow;
				newDlg.Response += (sender1, e1) =>
					{
						if (e1.ResponseId == ResponseType.Ok)
						{
							Console.WriteLine("Adding inst " + newDlg.InstDir);
							using (Instance inst = new Instance(newDlg.InstName, newDlg.InstDir, true))
							{
								instList.AppendValues(inst.Name, inst);
								LoadInstances();
							}
						}

						newDlg.Destroy();
					};
				newDlg.Run();
			}
		}

		protected void OnViewFolderClicked(object sender, System.EventArgs e)
		{
			if (!Directory.Exists(Resources.InstDir))
				Directory.CreateDirectory(Resources.InstDir);
			Process.Start(Resources.InstDir);
		}

		protected void OnSettingsClicked(object sender, System.EventArgs e)
		{
			using (SettingsDialog settingsDlg = new SettingsDialog(this))
			{
				//settingsDlg.ParentWindow = this.GdkWindow;
				settingsDlg.Run();
				if (settingsDlg.ForceUpdate)
					DownloadNewVersion();
			}
		}

		protected void OnRefreshClicked(object sender, System.EventArgs e)
		{
			LoadInstances();
		}

		protected void OnUpdateClicked(object sender, System.EventArgs e)
		{
			DoUpdateCheck();
		}

		/// <summary>
		/// Clears the list of instances and loads it from the instance directory.
		/// </summary>
		protected void LoadInstances()
		{
			instList.Clear();
			foreach (Instance inst in Instance.LoadInstances(Resources.InstDir))
			{
				instList.AppendValues(inst.Name, inst, Resources.GetInstIcon(inst.IconKey));
			}
		}

		#endregion

		#region Task System

		List<Task> currentTasks = new List<Task>();
		Dictionary<int, ProgressBar> progBars = new Dictionary<int, ProgressBar>();

		private void AddTask(Task newTask)
		{
			currentTasks.Add(newTask);
			int index = currentTasks.IndexOf(newTask);
			newTask.TaskID = index;

			ProgressBar taskProgBar = new ProgressBar();
			taskProgBar.HeightRequest = 20;
			progBars[newTask.TaskID] = taskProgBar;
			progBarBox.PackEnd(taskProgBar, true, true, 0);
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
			Gtk.Application.Invoke(
				(sender1, e1) =>
				{
					MessageDialog errDlg = new MessageDialog(this,
															 DialogFlags.Modal,
															 MessageType.Error,
															 ButtonsType.Ok,
															 e.Message);
					errDlg.Response += (o, args) => errDlg.Destroy();
					errDlg.Run();
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
				MessageUtils.ShowMessageBox(MessageType.Warning,
											"You must run the " +
											"instance at least " +
											"once before installing mods.");
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
				Application.Invoke(
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
			updateDL = new Downloader(Resources.NewVersionFileName,
				Resources.LatestVersionURL, "Downloading updates...");
			updateDL.Completed += updateDL_Completed;
			StartTask(updateDL);
		}

		void updateDL_Completed(object sender, EventArgs e)
		{
			Application.Invoke(
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
					MessageDialog updateDlg = new MessageDialog(this,
																DialogFlags.Modal,
																MessageType.Question,
																ButtonsType.YesNo,
																updatemsg,
																updatestr);
					updateDlg.Response += (o, args) =>
					{
						if (args.ResponseId == ResponseType.Yes)
						{
							CloseForUpdates();
						}
						else
						{
							File.Delete(Resources.NewVersionFileName);
						}
						updateDlg.Destroy();
					};
					updateDlg.Run();
				});
		}

		void CloseForUpdates()
		{
			MainClass.InstallUpdates = true;
			MainClass.RestartAfterUpdate = true;
			Destroy();
			Application.Quit();
		}

		#endregion

		#region Exceptions

		#endregion

		#region Events for all Tasks

		void taskStarted(object sender, EventArgs e)
		{
			Gtk.Application.Invoke(
				(sender1, e1) =>
				{
					if (progBars[(sender as Task).TaskID] != null)
					{
						progBars[(sender as Task).TaskID].Show();
					}
				});
		}

		void taskCompleted(object sender, Task.TaskCompleteEventArgs e)
		{
			Gtk.Application.Invoke(
				(sender1, e1) =>
			{
				progBarBox.Remove(progBars[(sender as Task).TaskID]);
				progBars[(sender as Task).TaskID].Dispose();
			});
		}

		void taskProgressChange(object sender, Task.ProgressChangeEventArgs e)
		{
			Gtk.Application.Invoke(
				(sender1, e1) =>
				{
					float progFraction = ((float) e.Progress) / 100;
					if (progFraction > 1)
					{
						Console.WriteLine(string.Format("Warning: Progress fraction " +
							"({0}) is greater than the maximum value (1)", progFraction));
						progFraction = 1;
					}

					progBars[(sender as Task).TaskID].Fraction = progFraction;
				});
		}

		void taskStatusChange(object sender, Task.TaskStatusEventArgs e)
		{
			Gtk.Application.Invoke(
				(sender1, e1) =>
				{
					progBars[(sender as Task).TaskID].Text = e.Status;
				});
		}

		#endregion

		#endregion

		#region Instances

		//		ConsoleWindow consoleWindow;

		public void StartInstance(Instance inst)
		{
			//			inst.InstQuit += (sender, e) =>
			//			{
			//				if (!cwin.Visible)
			//					Visible = true;
			//			};

			//			Console.WriteLine("Offline allowed: " + inst.CanPlayOffline);

			string message = "";
			UIEnabled = false;
			DoLogin(
				(LoginInfo info) =>
				{
					string mainGameUrl = "minecraft.jar";
					if (!info.Cancelled)
					{
						Console.WriteLine(info.ForceUpdate);
						GameUpdater updater =
							new GameUpdater(inst,
											info.LatestVersion,
											mainGameUrl,
											info.ForceUpdate);

						EventHandler startDelegate = new EventHandler(
							(e, args) =>
							{
								Visible = false;
								UIEnabled = true;
								inst.Launch(info.Username, info.SessionID);
								ConsoleWindow cwin = new ConsoleWindow(inst);
								cwin.ConsoleClosed += (sender3, e3) =>
								{
									Visible = true;
									cwin.Dispose();
								};

								cwin.Show();
							});

						updater.Completed += (sender, e) =>
						{
							if (inst.NeedsRebuild)
							{
								Application.Invoke(
									(sender2, e2) =>
									{
										RebuildMCJar(inst).Completed += (sender3, e3) =>
											Application.Invoke(sender3, e3, startDelegate);
									});
							}
							else
								Application.Invoke(sender, e, startDelegate);
						};

						Application.Invoke((sender, e) => StartTask(updater));
					}
					else
						UIEnabled = true;
				}, message, inst.CanPlayOffline);
			//			GameUpdater updater = new GameUpdater(inst, 
			//			                                      loginInfo., 
			//			                                      "minecraft.jar?user="
			//			                                      + username + "&ticket=" + 
			//			                                      downloadTicket,
			//			                                      true);
		}

		private delegate void LoginCompleteHandler(LoginInfo info);

		/// <summary>
		/// Opens a login dialog to allow the user to login.
		/// </summary>
		/// 
		private void DoLogin(LoginCompleteHandler done, string message = "",
							 bool canplayOffline = false)
		{
			string username = "";
			string password = "";
			ReadUserInfo(out username, out password);

			LoginDialog loginDlg = new LoginDialog(this, message, canplayOffline);
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
			loginDlg.Response += (object o, ResponseArgs args) =>
			{
				if (args.ResponseId == ResponseType.Ok)
				{
					Console.WriteLine("OK Clicked");
					string parameters = Uri.EscapeUriString(string.Format(
						"user={0}&password={1}&version={2}",
						loginDlg.Username, loginDlg.Password, 13));

					// Start a new thread and post the login info to login.minecraft.net
					Thread loginThread = new Thread(
						() =>
						{
							WriteUserInfo((loginDlg.RememberUsername ? loginDlg.Username : ""),
										  (loginDlg.RememberPassword ? loginDlg.Password : ""));

							string reply = AppUtils.ExecutePost("https://login.minecraft.net",
																parameters);

							// If the login failed
							if (!reply.Contains(":"))
							{
								// Translate the error message to a more user friendly wording
								string errorMessage = reply;
								switch (reply.ToLower())
								{
								case "bad login":
									errorMessage = "Invalid username or password.";
									break;
								case "old version":
									errorMessage = "Please update.";
									break;
								default:
									errorMessage = "Login failed: " + reply;
									break;
								}

								// Error
								Application.Invoke((sender, e) => DoLogin(done, errorMessage));
							}

							// If the login succeeded
							else
							{
								string[] responseValues = reply.Split(':');

								// The response must have 4 values or it's invalid
								if (responseValues.Length != 4)
								{
									// Error
									Application.Invoke(
										(sender, e) =>
										DoLogin(done, "Got an invalid response from server"));
								}
								// Now we can finally return our login info.
								else
								{
									LoginInfo info = new LoginInfo(responseValues,
																   loginDlg.ForceUpdate);
									done(info);
								}
							}
						});
					loginThread.Start();
				}
				else if (args.ResponseId == ResponseType.Reject)
				{
					// Play offline
					done(new LoginInfo(null, false, false));
				}
				else
				{
					// Login cancelled
					done(new LoginInfo());
				}
				loginDlg.Destroy();
			};
			loginDlg.Run();
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

		private void ReadUserInfo(out string username, out string password)
		{
			try
			{
				if (!File.Exists(Resources.LastLoginFileName))
				{
					username = password = "";
					return;
				}

				using (SHA384 sha = SHA384.Create())
				{
					byte[] hash = sha.ComputeHash(
						System.Text.ASCIIEncoding.ASCII.GetBytes(Resources.LastLoginKey));

					byte[] key = new byte[32];
					byte[] IV = new byte[16];

					Array.Copy(hash, key, key.Length);
					Array.ConstrainedCopy(hash, key.Length, IV, 0, IV.Length);

					using (Rijndael rijAlg = Rijndael.Create())
					{
						rijAlg.Key = key;
						rijAlg.IV = IV;

						ICryptoTransform decryptor = rijAlg.CreateDecryptor(key, IV);

						using (FileStream fsDecrypt = File.OpenRead(Resources.LastLoginFileName))
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
				File.Delete(Resources.LastLoginFileName);
			}
		}

		private void WriteUserInfo(string username, string password)
		{
			using (SHA384 sha = SHA384.Create())
			{
				byte[] hash = sha.ComputeHash(
					System.Text.ASCIIEncoding.ASCII.GetBytes(Resources.LastLoginKey));

				byte[] key = new byte[32];
				byte[] IV = new byte[16];

				Array.Copy(hash, key, key.Length);
				Array.ConstrainedCopy(hash, key.Length, IV, 0, IV.Length);

				using (Rijndael rijAlg = Rijndael.Create())
				{
					rijAlg.Key = key;
					rijAlg.IV = IV;

					ICryptoTransform encryptor = rijAlg.CreateEncryptor(key, IV);

					using (FileStream fsEncrypt = File.Open(Resources.LastLoginFileName,
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
			}
		}

		#endregion

		#region Menu

		private Menu instMenu;
		MenuItem imPlay;
		MenuItem imIcon;
		MenuItem imNotes;
		MenuItem imAddMods;
		MenuItem imEditMods;
		MenuItem imRebuild;
		MenuItem imViewFolder;
		MenuItem imDelete;

		private void InitMenu()
		{
			instMenu = new Menu();

			instMenu.Add(imPlay = new MenuItem("Play"));
			instMenu.Add(new SeparatorMenuItem());
			instMenu.Add(imIcon = new MenuItem("Change Icon"));
			instMenu.Add(imNotes = new MenuItem("Notes"));
			instMenu.Add(new SeparatorMenuItem());
			instMenu.Add(imAddMods = new MenuItem("Add Mods"));
			instMenu.Add(imEditMods = new MenuItem("Edit Mods"));
			instMenu.Add(imRebuild = new MenuItem("Rebuild"));
			instMenu.Add(imViewFolder = new MenuItem("View Folder"));
			instMenu.Add(new SeparatorMenuItem());
			instMenu.Add(imDelete = new MenuItem("Delete"));

			instMenu.ShowAll();

			imPlay.Activated += (sender, e) =>
			{
				if (SelectedInst != null)
					StartInstance(SelectedInst);
			};

			imIcon.Activated += ChangeIconActivated;
			imNotes.Activated += EditNotesActivated;

			imAddMods.Activated += AddModsActivated;
			imEditMods.Activated += EditModsActivated;
			imRebuild.Activated += RebuildActivated;
			imViewFolder.Activated += (sender, e) =>
				Process.Start(SelectedInst.RootDir);

			imDelete.Activated += DeleteActivated;

			instIconView.ButtonPressEvent += (object o, ButtonPressEventArgs args) =>
			{
				if (args.Event.Button == 3 &&
					instIconView.GetPathAtPos((int) args.Event.X,
										  (int) args.Event.Y) != null)
				{
					instIconView.SelectPath(
						instIconView.GetPathAtPos((int) args.Event.X,
											  (int) args.Event.Y));
					instMenu.Popup();
				}
			};

			// If on windows, try to make the menu /not/ look like absolute shit
			if (OSUtils.Windows)
				StyleUtils.DeuglifyMenu(instMenu);
		}

		void ChangeIconActivated(object sender, EventArgs e)
		{
			using (ChangeIconDialog iconDlg = new ChangeIconDialog(SelectedInst, this))
			{
				//iconDlg.ParentWindow = this.GdkWindow;
				iconDlg.Run();
				LoadInstances();
			}
		}

		void EditNotesActivated(object sender, EventArgs e)
		{
			using (EditNotesDialog end = new EditNotesDialog(this))
			{
				end.ParentWindow = this.GdkWindow;
				end.Notes = SelectedInst.Notes;
				end.Response += (o, args) =>
				{
					if (args.ResponseId == ResponseType.Ok)
						SelectedInst.Notes = end.Notes;
					end.Destroy();
				};
				end.Run();
			}
		}

		void AddModsActivated(object sender, EventArgs e)
		{
			HintDialog.ShowHint(this, Hint.AddModsHint);
			if (SelectedInst != null)
				Process.Start(SelectedInst.InstModsDir);
		}

		void EditModsActivated(object sender, EventArgs e)
		{
			using (EditModsDialog emd = new EditModsDialog(SelectedInst, this))
			{
				//emd.ParentWindow = this.GdkWindow;
				emd.Response += (object o, ResponseArgs args) =>
				{
					if (args.ResponseId == ResponseType.Ok)
					{
						RebuildMCJar(SelectedInst);
					}
					emd.Destroy();
				};
				HintDialog.ShowHint(emd, Hint.EditModsHint);
				emd.Run();
			}
		}

		void RebuildActivated(object sender, EventArgs e)
		{
			RebuildMCJar(SelectedInst);
		}

		void DeleteActivated(object sender, EventArgs e)
		{
			DeleteConfirmDialog delConf = new DeleteConfirmDialog(this);
			delConf.Response += (o, args) =>
			{
				delConf.Destroy();
				if (args.ResponseId == ResponseType.Ok &&
					delConf.Text == "DELETE")
				{
					SelectedInst.Dispose();
					try
					{
						Directory.Delete(SelectedInst.RootDir, true);
					}
					catch (IOException err)
					{
						MessageUtils.ShowMessageBox(
							this,
							MessageType.Error,
							"Failed to delete instance",
							"MultiMC failed to delete the instance: " + err.ToString());
					}
					catch (UnauthorizedAccessException)
					{
						MessageUtils.ShowMessageBox(
							this,
							MessageType.Error,
							"Failed to delete instance",
							"MultiMC failed to delete the instance because access was denied.");
					}
					Application.Invoke((sender2, e2) => LoadInstances());
				}
			};
		}

		#endregion

		#region Drag and Drop
		//		
		//		// FIXME Gtk drag and drop bullshit
		////		
		//		private void InitDND()
		//		{
		//			TargetEntry[] targets = new TargetEntry[]
		//			{ 
		//				new TargetEntry("text/plain", 0, 0)
		//			};
		//			
		//			Gtk.Drag.DestSet(instIconView, DestDefaults.All, targets, DragAction.Private);
		//			instIconView.EnableModelDragDest(targets, DragAction.Private);
		//			
		//			instIconView.DragDrop += InstListDragDrop;
		//			instIconView.DragLeave += InstListDragLeave;
		//			instIconView.DragMotion += InstListDragMotion;
		//		}
		//
		//		void InstListDragDrop(object o, DragDropArgs args)
		//		{
		//			Console.WriteLine("drop");
		//			Console.WriteLine(args.Context.DragProtocol.ToString());
		//		}
		//
		//		void InstListDragMotion(object o, DragMotionArgs args)
		//		{
		//			Console.WriteLine("Motion");
		//		}
		//
		//		void InstListDragLeave(object o, DragLeaveArgs args)
		//		{
		//			Console.WriteLine("Leave");
		//		}
		//		
		////		bool DragDataValid(int x, int y)
		////		{
		////			Point p = instanceList.PointToClient(new Point(x, y));
		////
		////			ListViewItem item = instanceList.GetItemAt(p.X, p.Y);
		////			if (item == null)
		////			{
		////				return false;
		////			}
		////
		////			if (currentTask != null && currentTask.Running)
		////			{
		////				return false;
		////			}
		////
		////			if (!dragData.GetDataPresent(DataFormats.FileDrop))
		////			{
		////				return false;
		////			}
		////
		////			string[] files = (string[]) dragData.GetData(DataFormats.FileDrop);
		////			foreach (string file in files)
		////			{
		////				if (!(File.Exists(file) || Directory.Exists(file)))
		////				{
		////					return false;
		////				}
		////			}
		////
		////			return true;
		////		}
		//		
		//		private string DragDropHint {
		//			get { return ddHint; }
		//			set {
		//				ddHint = value;
		//
		//				if (currentTask == null || !currentTask.Running)
		//				{
		//					if (!string.IsNullOrEmpty(ddHint))
		//					{
		//						statusProgBar.Text = ddHint;
		//						statusProgBar.Visible = true;
		//					}
		//					else
		//					{
		//						statusProgBar.Visible = false;
		//					}
		//				}
		//			}
		//		}
		//
		//		string ddHint;

		#endregion

		#region Other

		/// <summary>
		/// Recursively copies the list of files and folders into the destination
		/// </summary>
		/// <param name="cFiles">list of files and folders to copy</param>
		/// <param name="destination">place to copy the files to</param>
		private void CopyFiles(IEnumerable<string> cFiles, string destination)
		{
			foreach (string f in cFiles)
			{
				// For files...
				if (File.Exists(f))
				{
					if (!Directory.Exists(destination))
						Directory.CreateDirectory(destination);

					string copyname = System.IO.Path.Combine(destination,
															 System.IO.Path.GetFileName(f));
					if (File.Exists(copyname))
					{
						Console.WriteLine("Overwriting " + copyname);
						File.Delete(copyname);
						File.Copy(f, copyname);
						File.SetCreationTime(copyname, DateTime.Now);
					}
					else
					{
						Console.WriteLine("Adding file " + copyname);
						File.Copy(f, copyname);
						File.SetCreationTime(copyname, DateTime.Now);
					}
				}

				// For directories
				else if (Directory.Exists(f))
				{
					CopyFiles(Directory.EnumerateFileSystemEntries(f),
						System.IO.Path.Combine(destination, System.IO.Path.GetFileName(f)));
				}
			}
		}

		public bool UIEnabled
		{
			get { return _uiEnabled; }
			set
			{
				_uiEnabled = value;
				foreach (Widget w in AllChildren)
				{
					if (!(w is ProgressBar))
						w.Sensitive = value;
				}
			}
		}

		bool _uiEnabled;

		#endregion

		private Instance SelectedInst
		{
			get
			{
				TreeIter iter;
				if (instList.GetIter(out iter, instIconView.SelectedItems[0]))
				{
					return (Instance) instList.GetValue(iter, 1);
				}
				else
				{
					return null;
				}
			}
		}

		protected void OnInstIconViewItemActivated(object o, Gtk.ItemActivatedArgs args)
		{
			if (SelectedInst != null)
				StartInstance(SelectedInst);
		}
	}
}

