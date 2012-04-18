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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MultiMC.GUI;
using MultiMC.Tasks;

namespace MultiMC.WinGUI
{
	public partial class MainForm : WinFormsWindow, IMainWindow
	{
		/// <summary>
		/// Dictionary that maps status strips to their corresponding task IDs
		/// </summary>
		Dictionary<int, StatusStrip> statusStrips;

		public MainForm()
		{
			InitializeComponent();

			EventfulList<Instance> instList = new EventfulList<Instance>();
			InstanceList = instList;

			instList.Added += InstAdded;
			instList.Removed += InstRemoved;

			// If on windows, set the theme for our instance list.
			if (OSUtils.OS == OSEnum.Windows)
				OSUtils.SetWindowTheme(instView.Handle, "explorer", null);

			EventfulList<Task> tList = new EventfulList<Task>();
			tList.Added += TaskAdded;
			tList.Removed += TaskRemoved;
			TaskList = tList;

			//mainLayoutPanel.

			statusStrips = new Dictionary<int, StatusStrip>();
		}

		void TaskAdded(object sender, ItemAddRemoveEventArgs<Task> e)
		{
			if (IsTaskIDTaken(e.Item.TaskID))
				e.Item.TaskID = GetAvailableTaskID();
			TaskAdded(e.Item);
		}

		void TaskAdded(Task task)
		{
			if (InvokeRequired)
			{
				Invoke((o, args) => TaskAdded(task));
			}
			else
			{
				if (!task.Running)
				{
					task.Started += (o, args) =>
					{
						AddTaskStatusBar(task);
					};
				}
				else
					AddTaskStatusBar(task);
				task.Completed += (o, args) =>
				{
					Console.WriteLine("Task {0} completed.", task.TaskID);
					RemoveTaskStatusBar(task);
				};
			}
		}

		void TaskRemoved(object sender, ItemAddRemoveEventArgs<Task> e)
		{
			RemoveTaskStatusBar(e.Item);
		}

		void AddTaskStatusBar(Task task)
		{
			if (InvokeRequired)
			{
				Invoke((o, args) => AddTaskStatusBar(task));
				return;
			}

			// Status strip
			StatusStrip taskStatusStrip = new StatusStrip();
			statusStrips[task.TaskID] = taskStatusStrip;
			taskStatusStrip.Anchor = AnchorStyles.Top | AnchorStyles.Left |
				AnchorStyles.Right;
			taskStatusStrip.AutoSize = true;
			taskStatusStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
			taskStatusStrip.SizingGrip = false;

			// Status label
			ToolStripStatusLabel statusLabel = new ToolStripStatusLabel(task.Status);
			statusLabel.Name = "status";
			statusLabel.AutoSize = true;
			task.StatusChange += (o, args) => UpdateStatusLabel(statusLabel, args.Status);
			statusLabel.Text = task.Status;
			statusLabel.Visible = true;
			taskStatusStrip.Items.Add(statusLabel);

			// Progress bar
			ToolStripProgressBar progBar = new ToolStripProgressBar("progress");
			progBar.Alignment = ToolStripItemAlignment.Right;
			progBar.Size = new Size(100, 16);
			task.ProgressChange += (o, args) => UpdateProgBar(progBar, args.Progress);
			UpdateProgBar(progBar, task.Progress);
			progBar.Visible = true;
			taskStatusStrip.Items.Add(progBar);

			// Add the status bar
			toolStripContainer.BottomToolStripPanel.Controls.Add(taskStatusStrip);
			taskStatusStrip.Visible = true;
		}

		void RemoveTaskStatusBar(Task task)
		{
			if (InvokeRequired)
			{
				Invoke((o, args) => RemoveTaskStatusBar(task));
				return;
			}

			if (toolStripContainer.BottomToolStripPanel.Controls.Contains(statusStrips[task.TaskID]))
				toolStripContainer.BottomToolStripPanel.Controls.Remove(statusStrips[task.TaskID]);
		}

		void UpdateProgBar(ToolStripProgressBar progBar, int progress)
		{
			if (InvokeRequired)
			{
				Invoke((o, args) => UpdateProgBar(progBar, progress));
				return;
			}

			if (progress > 0)
			{
				progBar.Style = ProgressBarStyle.Blocks;
				progBar.Value = progress;
			}
			else
				progBar.Style = ProgressBarStyle.Marquee;
		}

		void UpdateStatusLabel(ToolStripStatusLabel label, string status)
		{
			if (InvokeRequired)
			{
				Invoke((o, args) => UpdateStatusLabel(label, status));
				return;
			}

			label.Text = status;
		}

		void UpdateInstViewSize()
		{

		}

		//void UpdateStatus(int taskID)
		//{
		//    if (InvokeRequired)
		//    {
		//        Invoke((o, args) => UpdateStatus(taskID));
		//    }
		//    else
		//    {
		//        Task task = TaskList.FirstOrDefault<Task>(t => t.TaskID == taskID);
		//        if (task != null)
		//        {
		//            StatusStrip statusStrip = null;
		//            ToolStripStatusLabel statusLabel = null;
		//            ToolStripProgressBar statusProgBar = null;
		//            if (!statusStrips.ContainsKey(task.TaskID))
		//            {
		//                TaskAdded(task);
		//            }
		//            else
		//            {
		//                statusStrip = statusStrips[task.TaskID];
		//                statusLabel = statusStrip.Items["status"] as ToolStripStatusLabel;
		//                statusProgBar = statusStrip.Items["progress"] as ToolStripProgressBar;
		//            }

		//            statusLabel.Text = task.Status;

		//            if (task.Progress > 0)
		//            {
		//                statusProgBar.Style = ProgressBarStyle.Blocks;
		//                statusProgBar.Value = task.Progress;
		//            }
		//            else
		//                statusProgBar.Style = ProgressBarStyle.Marquee;
		//        }
		//        else
		//        {
		//            if (statusStrips.ContainsKey(taskID))
		//            {
		//                statusStrips.Remove(taskID);
		//            }
		//        }
		//    }
		//}

		void InstRemoved(object sender, ItemAddRemoveEventArgs<Instance> e)
		{
			foreach (ListViewItem item in instView.Items.Where<ListViewItem>(
				item => item.Tag == e.Item))
			{
				instView.Items.Remove(item);
			}
		}

		void InstAdded(object sender, ItemAddRemoveEventArgs<Instance> e)
		{
			ListViewItem item = new ListViewItem(e.Item.Name);
			item.Tag = e.Item;
			if ((ImageList as WinFormsImageList).ImgList.Images.ContainsKey(e.Item.IconKey))
				item.ImageKey = e.Item.IconKey;
			else
				item.ImageIndex = 0;
			instView.Items.Add(item);
		}

		public event EventHandler<AddInstEventArgs> AddInstClicked;

		public event EventHandler ViewFolderClicked
		{
			add { viewInstanceFolder.Click += value; }
			remove { viewInstanceFolder.Click -= value; }
		}

		public event EventHandler RefreshClicked
		{
			add { refreshButton.Click += value; }
			remove { refreshButton.Click -= value; }
		}

		public event EventHandler SettingsClicked
		{
			add { settingsButton.Click += value; }
			remove { settingsButton.Click -= value; }
		}

		public event EventHandler CheckUpdatesClicked
		{
			add { checkUpdateButton.Click += value; }
			remove { checkUpdateButton.Click -= value; }
		}

		public event EventHandler HelpClicked
		{
			add { helpButton.Click += value; }
			remove { helpButton.Click -= value; }
		}

		public event EventHandler AboutClicked
		{
			add { aboutButton.Click += value; }
			remove { aboutButton.Click -= value; }
		}

		public IList<Task> TaskList
		{
			get;
			protected set;
		}

		public IList<Instance> InstanceList
		{
			get;
			protected set;
		}

		public void LoadInstances()
		{
			instView.Items.Clear();
			InstanceList.Clear();
			foreach (Instance inst in Instance.LoadInstances(AppSettings.Main.InstanceDir))
			{
				InstanceList.Add(inst);
			}
		}

		public IImageList ImageList
		{
			get { return _imageList; }
			set
			{
				if (value is WinFormsImageList)
				{
					_imageList = value as WinFormsImageList;
					instView.LargeImageList = _imageList.ImgList;
				}
				else if (value != null)
					throw new InvalidOperationException("WinForms needs a WinFormsImageList.");
				else
					throw new ArgumentNullException("value");
			}
		}
		WinFormsImageList _imageList;


		public Task GetTaskByID(int taskID)
		{
			return TaskList.First(task => task.TaskID == taskID);
		}

		public bool IsTaskIDTaken(int taskID)
		{
			return TaskList.Any(task => task.TaskID == taskID);
		}

		public int GetAvailableTaskID()
		{
			int i = 0;
			while (IsTaskIDTaken(i))
			{
				i++;
			}
			return i;
		}

		private void instView_ItemActivate(object sender, EventArgs e)
		{
			OnInstanceLaunched(SelectedInst);
		}

		protected virtual void OnInstanceLaunched(Instance inst)
		{
			if (InstanceLaunched != null)
				InstanceLaunched(this, new InstActionEventArgs(inst));
		}

		protected virtual void OnInstanceAction(InstAction action, Instance inst = null)
		{
			if (inst == null)
				inst = SelectedInst;

			InstActionEventArgs args = new InstActionEventArgs(inst);
			switch (action)
			{
			case InstAction.ChangeIcon:
				if (ChangeIconClicked != null)
					ChangeIconClicked(this, args);
				break;

			case InstAction.EditNotes:
				if (EditNotesClicked != null)
					EditNotesClicked(this, args);
				break;

			case InstAction.EditMods:
				if (EditModsClicked != null)
					EditModsClicked(this, args);
				break;

			case InstAction.RebuildJar:
				if (RebuildJarClicked != null)
					RebuildJarClicked(this, args);
				break;

			case InstAction.ViewFolder:
				if (ViewInstFolderClicked != null)
					ViewInstFolderClicked(this, args);
				break;

			case InstAction.Delete:
				if (DeleteInstClicked != null)
					DeleteInstClicked(this, args);
				break;
			}
		}


		public event EventHandler<InstActionEventArgs> InstanceLaunched;

		public event EventHandler<InstActionEventArgs> ChangeIconClicked;

		public event EventHandler<InstActionEventArgs> EditNotesClicked;

		public event EventHandler<InstActionEventArgs> EditModsClicked;

		public event EventHandler<InstActionEventArgs> RebuildJarClicked;

		public event EventHandler<InstActionEventArgs> ViewInstFolderClicked;

		public event EventHandler<InstActionEventArgs> DeleteInstClicked;
		
		public event EventHandler<InstActionEventArgs> RemoveOpenALClicked;

		protected enum InstAction
		{
			ChangeIcon,
			EditNotes,

			EditMods,
			RebuildJar,
			ViewFolder,

			Delete,
		}

		public Instance SelectedInst
		{
			get { return instView.SelectedItems[0].Tag as Instance; }
		}

		private void playToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnInstanceLaunched(SelectedInst);
		}

		private void changeIconToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnInstanceAction(InstAction.ChangeIcon);
		}

		private void editNotesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnInstanceAction(InstAction.EditNotes);
		}

		private void editModsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnInstanceAction(InstAction.EditMods);
		}

		private void rebuildJarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnInstanceAction(InstAction.RebuildJar);
		}

		private void viewFolderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnInstanceAction(InstAction.ViewFolder);
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnInstanceAction(InstAction.Delete);
		}

		private void instanceContextMenu_Opening(object sender, CancelEventArgs e)
		{
			if (instView.SelectedItems.Count == 0)
				e.Cancel = true;
		}

		private void instView_BeforeLabelEdit(object sender, LabelEditEventArgs e)
		{
			// TODO Implement
		}

		#region Drag Drop Code

		private string DragDropHint
		{
			get { return _ddHint; }
			set
			{
				_ddHint = value;
				DragDropHintLabel.Text = _ddHint;
				DragDropHintLabel.Visible = !string.IsNullOrEmpty(_ddHint);
			}
		}

		string _ddHint;

		private void instView_DragDrop(object sender, DragEventArgs e)
		{
			if (!DragDataValid(e.Data, e.X, e.Y))
			{
				return;
			}

			DragDropHint = string.Empty;

			Point p = instView.PointToClient(new Point(e.X, e.Y));
			Instance inst = instView.GetItemAt(p.X, p.Y).Tag as Instance;
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

			DragKeyStates keyStates = new DragKeyStates(e.KeyState);

			string modsFolder = inst.ModLoaderDir;
			if (!Directory.Exists(modsFolder))
				Directory.CreateDirectory(modsFolder);

			string instMods = inst.InstModsDir;
			if (!Directory.Exists(instMods))
				Directory.CreateDirectory(instMods);

			string texturePacksFolder = inst.TexturePackDir;

			if (keyStates.ShiftKey)
			{
				CopyModFiles(files, modsFolder);
			}
			else if (keyStates.AltKey)
			{
				CopyModFiles(files, texturePacksFolder);
			}
			else
			{
				CopyModFiles(files, instMods);
			}
		}

		private void instView_DragLeave(object sender, EventArgs e)
		{
			DragDropHint = string.Empty;
		}

		/// <summary>
		/// Translates drag event key states into a more maintainable format.
		/// </summary>
		struct DragKeyStates
		{
			public DragKeyStates(int keyState)
			{
				this.LeftMouse = (keyState & 1) == 1;
				this.RightMouse = (keyState & 2) == 2;
				this.ShiftKey = (keyState & 4) == 4;
				this.ControlKey = (keyState & 8) == 8;
				this.MiddleMouse = (keyState & 16) == 16;
				this.AltKey = (keyState & 32) == 32;
			}

			public bool LeftMouse;	 // 1
			public bool RightMouse;	 // 2
			public bool ShiftKey;	 // 4
			public bool ControlKey;	 // 8
			public bool MiddleMouse; // 16
			public bool AltKey;		 // 32
		}

		private void instView_DragOver(object sender, DragEventArgs e)
		{
			if (DragDataValid(e.Data, e.X, e.Y))
			{
				DragKeyStates keyState = new DragKeyStates(e.KeyState);

				// Control + Alt
				if (keyState.ControlKey && keyState.AltKey)
				{
					e.Effect = DragDropEffects.Move;
					DragDropHint = "Add files to .minecraft\\bin";
				}

				// Shift
				else if (keyState.ShiftKey)
				{
					e.Effect = DragDropEffects.Copy;
					DragDropHint = "Add files to .minecraft\\mods";

				}

				// Alt
				else if (keyState.AltKey)
				{
					e.Effect = DragDropEffects.Copy;
					DragDropHint = "Add to texture packs";
				}

				// Anything else
				else
				{
					e.Effect = DragDropEffects.Copy;
					DragDropHint = "Add files to minecraft.jar";
				}
			}
			else
			{
				e.Effect = DragDropEffects.None;
				DragDropHint = string.Empty;
			}
		}

		bool DragDataValid(IDataObject dragData, int x, int y)
		{
			Point p = instView.PointToClient(new Point(x, y));

			ListViewItem item = instView.GetItemAt(p.X, p.Y);
			if (item == null)
			{
				return false;
			}

			if (!dragData.GetDataPresent(DataFormats.FileDrop))
			{
				return false;
			}

			string[] files = (string[])dragData.GetData(DataFormats.FileDrop);
			foreach (string file in files)
			{
				if (!(File.Exists(file) || Directory.Exists(file)))
				{
					return false;
				}
			}

			return true;
		}

		#endregion

		#region Other

		/// <summary>
		/// Recursively copies the list of files and folders into the destination
		/// </summary>
		/// <param name="cFiles">list of files and folders to copy</param>
		/// <param name="destination">place to copy the files to</param>
		private void CopyModFiles(IEnumerable<string> cFiles, string destination)
		{
			foreach (string f in cFiles)
			{
				// For files...
				if (File.Exists(f))
				{
					if (!Directory.Exists(destination))
						Directory.CreateDirectory(destination);

					string copyname = Path.Combine(destination, Path.GetFileName(f));
					if (File.Exists(copyname))
					{
						Console.WriteLine("Overwriting " + copyname);
						File.Delete(copyname);
						File.Copy(f, copyname);
					}
					else
					{
						Console.WriteLine("Adding file " + copyname);
						File.Copy(f, copyname);
					}
				}

				// For directories
				else if (Directory.Exists(f))
				{
					CopyModFiles(Directory.EnumerateFileSystemEntries(f),
						Path.Combine(destination, Path.GetFileName(f)));
				}
			}
		}

		#endregion

		private void instView_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			ListViewItem item = instView.Items[e.Item];
			Instance inst = (item.Tag as Instance);

			if (e.Label != null && Instance.NameIsValid(e.Label))
			{
				inst.Name = e.Label;
			}
			else
			{
				e.CancelEdit = true;
			}
		}

		private void instView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.F2)
			{
				if (instView.SelectedItems.Count > 0)
					instView.SelectedItems[0].BeginEdit();
			}
		}

		private void renameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			instView.SelectedItems[0].BeginEdit();
		}


		public event EventHandler<InstActionEventArgs> ManageSavesClicked;

		private void manageSavesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ManageSavesClicked != null)
				ManageSavesClicked(this, new InstActionEventArgs(SelectedInst));
		}

		private void addInstButton_ButtonClick(object sender, EventArgs e)
		{
			OnAddInstClicked(AddInstAction.CreateNew);
		}

		private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnAddInstClicked(AddInstAction.CreateNew);
		}

		private void copyExistingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnAddInstClicked(AddInstAction.CopyExisting);
		}

		private void importToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OnAddInstClicked(AddInstAction.ImportExisting);
		}

		void OnAddInstClicked(AddInstAction action)
		{
			if (AddInstClicked != null)
				AddInstClicked(this, new AddInstEventArgs(action));
		}

		public string ImportInstance()
		{
			FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
			folderBrowser.ShowNewFolderButton = false;
			folderBrowser.Description = "Choose a .minecraft to import.";

			if (OSUtils.OS == OSEnum.Windows)
			{
				string defMCDir = Path.Combine(
					Environment.GetEnvironmentVariable("APPDATA"), ".minecraft");

				if (Directory.Exists(defMCDir))
				{
					folderBrowser.SelectedPath = defMCDir;
				}
			}

			folderBrowser.ShowDialog(this);

			return folderBrowser.SelectedPath;
		}
	}
}
