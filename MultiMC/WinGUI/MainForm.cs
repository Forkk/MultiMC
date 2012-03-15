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

namespace MultiMC.WinGUI
{
	public partial class MainForm : WinFormsWindow, IMainWindow
	{
		public MainForm()
		{
			InitializeComponent();

			InstanceList = new List<Instance>();
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

		public List<ITaskStatus> TaskList
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
				ListViewItem item = new ListViewItem(inst.Name, inst.IconKey);
				item.Tag = inst;
				instView.Items.Add(item);
			}
		}
	}
}
