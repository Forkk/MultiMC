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
using Gdk;

using MultiMC.Data;

namespace MultiMC
{
	public partial class EditModsDialog : Gtk.Dialog
	{
		ListStore modList;
		TreeView modView;
		ScrolledWindow editModScroll;
		
		public EditModsDialog(Instance inst, Gtk.Window parent = null)
			: base("Edit Mods", parent, DialogFlags.Modal)
		{
			Inst = inst;
			
			this.Build();
			
			#region We have to make the treeview ourselves since monodevelop is absolute shit...
			this.editModScroll = new ScrolledWindow();
			this.editModScroll.HscrollbarPolicy = PolicyType.Never;
			this.modView = new Gtk.TreeView();
			this.modView.CanFocus = true;
			this.modView.Name = "modView";
			this.editModScroll.Add(this.modView);
			this.VBox.Add(this.editModScroll);
			Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.VBox[this.editModScroll]));
			w3.Position = 0;
			this.ShowAll();
			#endregion
			
			modList = new ListStore(typeof(string), typeof(string), typeof(bool), typeof(DateTime));
			modView.Model = modList;
			modView.AppendColumn("File", new CellRendererText(), "text", 0);
			modView.AppendColumn("Install Date", new CellRendererText(), "text", 1);
			
			CellRendererToggle toggleRenderer = new CellRendererToggle();
			toggleRenderer.Activatable = true;
			toggleRenderer.Sensitive = true;
			toggleRenderer.Toggled += (object o, ToggledArgs args) =>
			{
				TreeIter iter;
				if (modList.GetIter(out iter, new TreePath(args.Path)))
					modList.SetValue(iter, 2, !(bool)modList.GetValue(iter, 2));
			};
			modView.AppendColumn("Delete?", toggleRenderer, "active", 2);
			
			modView.Columns[0].Alignment = 0.0f;
			modView.Columns[0].Expand = true;
			modView.Columns[1].Alignment = 1.0f;
			modView.Columns[2].Alignment = 1.0f;
			
			LoadMods();
		}
		
		protected void LoadMods()
		{
			modList.Clear();
			LoadMods(Inst.InstModsDir);
		}
		
		private void LoadMods(string searchdir)
		{
			foreach (string f in Directory.GetFileSystemEntries(searchdir))
			{
				if (Directory.Exists(f))
				{
					LoadMods(f);
				}
				else if (File.Exists(f))
				{
					AddFile(f);
				}
			}
		}
		
		protected void AddFile(string fname)
		{
			modList.AppendValues(fname, 
			                     File.GetCreationTime(fname).ToShortDateString() + " " + 
			                     File.GetCreationTime(fname).ToShortTimeString(), 
			                     false,
			                     File.GetCreationTime(fname));
		}

		protected void OnResponse(object o, Gtk.ResponseArgs args)
		{
			TreeIter iter;
			if (args.ResponseId == ResponseType.Ok && modList.GetIterFirst(out iter))
			{
				do
				{
					if ((bool)modList.GetValue(iter, 2))
					{
						File.Delete((string)modList.GetValue(iter, 0));
					}
				} while (modList.IterNext(ref iter));
			}
			
			Destroy();
		}
		
		public Instance Inst
		{
			get;
			protected set;
		}
	}
}

