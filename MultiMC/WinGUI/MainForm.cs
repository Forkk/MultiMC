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

			instList.Added += new EventHandler<ItemAddRemoveEventArgs<Instance>>(instList_Added);
			instList.Removed += new EventHandler<ItemAddRemoveEventArgs<Instance>>(instList_Removed);

			// If on windows, set the theme for our instance list.
			if (OSUtils.OS == OSEnum.Windows)
				OSUtils.SetWindowTheme(instView.Handle, "explorer", null);

			EventfulList<Task> tList = new EventfulList<Task>();
			tList.Added += new EventHandler<ItemAddRemoveEventArgs<Task>>(TaskAdded);
			tList.Removed += new EventHandler<ItemAddRemoveEventArgs<Task>>(TaskRemoved);
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
				AnchorStyles.Right | AnchorStyles.Bottom;
			taskStatusStrip.AutoSize = true;
			taskStatusStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;

			// Status label
			ToolStripStatusLabel statusLabel = new ToolStripStatusLabel(task.Status);
			statusLabel.Name = "status";
			statusLabel.AutoSize = true;
			task.StatusChange += (o, args) => UpdateStatusLabel(statusLabel, args.Status);
			statusLabel.Text = task.Status;
			taskStatusStrip.Items.Add(statusLabel);

			// Progress bar
			ToolStripProgressBar progBar = new ToolStripProgressBar("progress");
			progBar.Alignment = ToolStripItemAlignment.Right;
			progBar.Size = new Size(100, 16);
			task.ProgressChange += (o, args) => UpdateProgBar(progBar, args.Progress);
			UpdateProgBar(progBar, task.Progress);
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

		void instList_Removed(object sender, ItemAddRemoveEventArgs<Instance> e)
		{
			foreach (ListViewItem item in instView.Items.Where<ListViewItem>(
				item => item.Tag == e.Item))
			{
				instView.Items.Remove(item);
			}
		}

		void instList_Added(object sender, ItemAddRemoveEventArgs<Instance> e)
		{
			ListViewItem item = new ListViewItem(e.Item.Name);
			item.Tag = e.Item;
			if ((ImageList as WinFormsImageList).ImgList.Images.ContainsKey(e.Item.IconKey))
				item.ImageKey = e.Item.IconKey;
			else
				item.ImageIndex = 0;
			instView.Items.Add(item);
		}

		public event EventHandler NewInstClicked
		{
			add { addInstButton.Click += value; }
			remove { addInstButton.Click -= value; }
		}

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
					//instView.SmallImageList = _imageList.ImgList;
				}
				else if (value != null)
					throw new InvalidOperationException("WinForms needs a WinFormsImageCache.");
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

		private void mainLayoutPanel_Paint(object sender, PaintEventArgs e)
		{

		}
	}
}
