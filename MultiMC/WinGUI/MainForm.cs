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
		}

		void TaskAdded(object sender, ItemAddRemoveEventArgs<Task> e)
		{
			if (InvokeRequired)
			{
				Invoke(new EventHandler<ItemAddRemoveEventArgs<Task>>(TaskAdded), sender, e);
			}
			else
			{
				Task task = e.Item;

				task.ProgressChange += (o, args) => UpdateStatus();
				task.StatusChange += (o, args) => UpdateStatus();

				task.Started += (o, args) => UpdateStatus();
				task.Completed += (o, args) => UpdateStatus();
			}
		}

		void TaskRemoved(object sender, ItemAddRemoveEventArgs<Task> e)
		{
			UpdateStatus();
		}

		void UpdateStatus(object sender = null, EventArgs e = null)
		{
			if (InvokeRequired)
			{
				Invoke(UpdateStatus);
			}
			else
			{
				if (TaskList.Count >= 1)
				{
					Task task = TaskList[0];
					taskStatusLabel.Visible = true;
					taskStatusLabel.Text = task.Status;

					taskStatusProgBar.Visible = true;

					if (task.Progress > 0)
					{
						taskStatusProgBar.Style = ProgressBarStyle.Blocks;
						taskStatusProgBar.Value = task.Progress;
					}
					else
						taskStatusProgBar.Style = ProgressBarStyle.Marquee;
				}
				else
				{
					taskStatusLabel.Visible = false;
					taskStatusProgBar.Visible = false;
				}
			}
		}

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
			return GetTaskByID(taskID) != null;
		}

		public int GetAvailableTaskID()
		{
			int i = 0;
			while (!IsTaskIDTaken(i))
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

		}
	}
}
