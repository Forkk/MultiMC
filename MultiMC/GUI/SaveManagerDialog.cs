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

using Gtk;

using MultiMC.Data;

namespace MultiMC
{
	public partial class SaveManagerDialog : Gtk.Dialog
	{
		ListStore listStoreSaves;
		
		public SaveManagerDialog(Window parent, Instance inst)
			: base("Manage Saves", parent, DialogFlags.Modal)
		{
			this.Inst = inst;

			this.Build();
			
			listStoreSaves = new ListStore(typeof(string), typeof(string), typeof(string));

			saveView.Model = listStoreSaves;

			saveView.AppendColumn("Name", new CellRendererText(), "text", 0);
			saveView.AppendColumn("Last backup", new CellRendererText(), "text", 1);

			saveView.Columns[0].Alignment = 0.0f;
			saveView.Columns[0].Expand = true;
			saveView.Columns[1].Alignment = 1.0f;

			saveView.RowActivated += new RowActivatedHandler(RowActivated);

			LoadList();
		}

		void RowActivated(object o, RowActivatedArgs args)
		{
			TreeIter iter;
			if (!listStoreSaves.GetIter(out iter, args.Path))
				return;

			string savePath = listStoreSaves.GetValue(iter, 2).ToString();

			if (!Inst.InstSaves.SaveHasBackups(savePath))
			{
				DebugUtils.Print(savePath);
				MessageUtils.ShowMessageBox(this, MessageType.Warning, 
					"No backups", "That save has no backups.");
				return;
			}

			using (BackupListDialog bld = new BackupListDialog(this, savePath))
			{
				bld.Response += (o2, args2) =>
					{
						bld.Destroy();
					};
				bld.Run();
			}
		}

		public void LoadList()
		{
			listStoreSaves.Clear();
			foreach (string save in Inst.InstSaves)
			{
				listStoreSaves.AppendValues(System.IO.Path.GetFileName(save), 
											"Not implemented",
											save);
			}
		}

		public Instance Inst
		{
			get;
			protected set;
		}
	}
}

