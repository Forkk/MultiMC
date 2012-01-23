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
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

using Gtk;

using MultiMC.Data;
using MultiMC.Tasks;

namespace MultiMC
{
	public partial class MainWindow : Gtk.Window
	{
		private ListStore instList;
		
		public MainWindow() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			Title = "MultiMC " + AppUtils.GetVersion().ToString();
			
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
				instIconView.SelectPath(new TreePath(args.Path));
				if (Instance.NameIsValid(args.NewText))
				{
					SelectedInst.Name = args.NewText;
					LoadInstances();
				}
			};
			
			InitMenu();
//			InitDND();
			
			LoadInstances();
			
			if (!File.Exists(AppSettings.Main.LauncherPath))
			{
				instIconView.Sensitive = false;
				Downloader launchDl = new Downloader(AppSettings.Main.LauncherPath,
				                                     Resources.LauncherURL,
				                                     "Downloading Launcher");
				launchDl.Completed += (sender, e) => instIconView.Sensitive = true;
				StartTask(launchDl);
			}
			
			if (!File.Exists("Ionic.Zip.Reduced.dll"))
			{
				instIconView.Sensitive = false;
				Downloader dnzDl = new Downloader(AppSettings.Main.LauncherPath,
				                                     Resources.LauncherURL,
				                                     "Downloading DotNetZip");
				dnzDl.Completed += (sender, e) => instIconView.Sensitive = true;
				StartTask(dnzDl);
			}
			
			if (AppSettings.Main.AutoUpdate)
			{
				DoUpdateCheck();
			}
		}
		
		#region Buttons
		
		protected void OnDeleteEvent(object o, Gtk.DeleteEventArgs args)
		{
			Application.Quit();
		}
		
		protected void OnNewClicked(object sender, System.EventArgs e)
		{
			NewInstanceDialog newDlg = new NewInstanceDialog();
			newDlg.ParentWindow = this.GdkWindow;
			newDlg.Show();
			newDlg.OKClicked += (sender1, e1) => 
			{
				Console.WriteLine("Adding inst " + newDlg.InstDir);
				Instance inst = new Instance(newDlg.InstName, newDlg.InstDir, true);
				instList.AppendValues(inst.Name, inst);
			};
		}

		protected void OnViewFolderClicked(object sender, System.EventArgs e)
		{
			if (!Directory.Exists(Resources.InstDir))
				Directory.CreateDirectory(Resources.InstDir);
			Process.Start(Resources.InstDir);
		}

		protected void OnSettingsClicked(object sender, System.EventArgs e)
		{
			SettingsDialog settingsDlg = new SettingsDialog();
			settingsDlg.ParentWindow = this.GdkWindow;
			settingsDlg.Show();
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
		
		Task currentTask;
		
		private void StartTask(Task task)
		{
			if (currentTask != null && currentTask.Running)
				throw new TaskAlreadyOccupiedException("A task is already running.");

			this.currentTask = task;

			currentTask.Started += new EventHandler(taskStarted);
			currentTask.Completed += new EventHandler(taskCompleted);
			currentTask.ProgressChange +=
				new Task.ProgressChangeEventHandler(taskProgressChange);
			currentTask.StatusChange +=
				new Task.StatusChangeEventHandler(taskStatusChange);
			currentTask.ErrorMessage += new Task.ErrorMessageEventHandler(taskErrorMessage);

			currentTask.Start();
		}

		void taskErrorMessage(object sender, Task.ErrorMessageEventArgs e)
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
		private void RebuildMCJar(Instance inst)
		{
			if (!File.Exists(SelectedInst.MCJar))
			{
				MessageUtils.ShowMessageBox(MessageType.Warning, 
				                            "You must run the " +
				                            "instance at least " +
				                            "once before installing mods.");
				return;
			}
			Modder modder = new Modder(SelectedInst);
			instIconView.Sensitive = false;
			modder.Completed += (sender2, e2) => instIconView.Sensitive = true;
			StartTask(modder);
		}
		
		#endregion

		#region Updates

		Updater updater;
		Version updateVersion;

		private void DoUpdateCheck()
		{
			updater = new Updater();
			updater.Completed += (sender, e) =>
			{
				updateVersion = updater.NewVersion;
				if (updateVersion != null && 
					updater.NewVersion.CompareTo(AppUtils.GetVersion()) > 0)
				{
					DownloadNewVersion();
				}
			};
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
			updateDL.Completed += new EventHandler(updateDL_Completed);
			StartTask(updateDL);
		}

		void updateDL_Completed(object sender, EventArgs e)
		{
			Application.Invoke(
				(sender1, e1) => 
			{
				MessageDialog updateDlg = new MessageDialog(this,
				                                            DialogFlags.Modal,
				                                            MessageType.Question,
				                                            ButtonsType.YesNo,
				                                            "Version {0} has been " +
				                                            "downloaded. Would you " +
				                                            "like to install it now?",
				                                            updateVersion);
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

		/// <summary>
		/// Thrown when trying to start a new task when one is already running
		/// </summary>
		class TaskAlreadyOccupiedException : Exception
		{
			public TaskAlreadyOccupiedException(string msg)
				: base(msg)
			{
			}
		}

		#endregion

		#region Events for all Tasks

		void taskStarted(object sender, EventArgs e)
		{
			Gtk.Application.Invoke(
				(sender1, e1) =>
			{
				this.statusProgBar.Visible = true;
				this.statusProgBar.Fraction = 0;
			});
		}

		void taskCompleted(object sender, EventArgs e)
		{
			Gtk.Application.Invoke(
				(sender1, e1) =>
			{
				statusProgBar.Visible = false;
			});
		}

		void taskProgressChange(object sender, Task.ProgressChangeEventArgs e)
		{
			Gtk.Application.Invoke(
				(sender1, e1) =>
			{
				Console.WriteLine("Progress: " + e.Progress);
				statusProgBar.Fraction = e.Progress / 100;
			});
		}
		
		void taskStatusChange(object sender, Task.TaskStatusEventArgs e)
		{
			Gtk.Application.Invoke(
				(sender1, e1) =>
			{
				statusProgBar.Text = e.Status;
			});
		}

		#endregion

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
			
			
			imPlay.Activated += (sender, e) => SelectedInst.Launch();
			
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
		}

		void ChangeIconActivated (object sender, EventArgs e)
		{
			// TODO Implement change icon
		}

		void EditNotesActivated (object sender, EventArgs e)
		{
			// TODO Implement notes
		}
		
		void AddModsActivated (object sender, EventArgs e)
		{
			FileChooserDialog fileDlg = new FileChooserDialog("Add mods", 
			                                                  this, 
			                                                  FileChooserAction.Open, 
			                                                  "Cancel", ResponseType.Cancel,
			                                                  "Add", ResponseType.Accept);
			fileDlg.Response += (object o, ResponseArgs args) => 
			{
				if (SelectedInst == null)
					return;
				
				if (args.ResponseId == ResponseType.Accept)
				{
					if (!Directory.Exists(SelectedInst.InstModsDir))
						Directory.CreateDirectory(SelectedInst.InstModsDir);
					
					CopyFiles(fileDlg.Filenames, SelectedInst.InstModsDir);
					RebuildMCJar(SelectedInst);
				}
				fileDlg.Destroy();
			};
			fileDlg.Run();
		}
		
		void EditModsActivated (object sender, EventArgs e)
		{
			// TODO Implement mod removal
		}
		
		void RebuildActivated (object sender, EventArgs e)
		{
			RebuildMCJar(SelectedInst);
		}
		
		void DeleteActivated(object sender, EventArgs e)
		{
			// TODO Implement delete
		}
		
		#endregion
		
		#region Drag and Drop
		
		// FIXME Gtk drag and drop bullshit
//		
//		private void InitDND()
//		{
//			instIconView.EnableModelDragDest(
//				new TargetEntry[]
//			{
//				new TargetEntry("STRING", TargetFlags.OtherApp, 0),
//			}, Gdk.DragAction.Default);
////			instIconView.drag
//			instIconView.DragDrop += IconViewDrop;
//			instIconView.DragLeave += IVDragLeave;
//			instIconView.DragDataReceived += IconViewDataReceived;
//		}
//
//		void IVDragLeave (object o, DragLeaveArgs args)
//		{
//			Console.WriteLine("Drag leave");
//		}
//
//		void IconViewDataReceived (object o, DragDataReceivedArgs args)
//		{
//			Console.WriteLine("Data");
//			Console.WriteLine(args.SelectionData.Text);
//		}
//
//		void IconViewDrop(object o, DragDropArgs args)
//		{
//			Console.WriteLine("drop");
//			args.RetVal = true;
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
//		private string DragDropHint
//		{
//			get { return ddHint; }
//			set
//			{
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
//		} string ddHint;
//		
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
	}
}

