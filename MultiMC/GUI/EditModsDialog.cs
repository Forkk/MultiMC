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

			using (Button buttonRefresh = new Button("gtk-refresh"))
			{
				buttonRefresh.Clicked += (sender, e) =>
				{
					LoadMods();
				};
				ActionArea.Homogeneous = false;
				ActionArea.PackEnd(buttonRefresh, false, true, 0);
				buttonRefresh.Visible = true;
			}
			
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
			
			modList = new ListStore(typeof(string), typeof(int), typeof(bool));
			modView.Model = modList;
			using (CellRendererText cr = new CellRendererText())
				modView.AppendColumn("Index", cr, "text", 1);
			using (CellRendererText cr = new CellRendererText())
				modView.AppendColumn("File", cr, "text", 0);

			using (CellRendererToggle toggleRenderer = new CellRendererToggle())
			{
				toggleRenderer.Activatable = true;
				toggleRenderer.Sensitive = true;
				toggleRenderer.Toggled += (object o, ToggledArgs args) =>
				{
					TreeIter iter;
					using (TreePath tp = new TreePath(args.Path))
						if (modList.GetIter(out iter, tp))
							modList.SetValue(iter, 2, !(bool) modList.GetValue(iter, 2));
				};
				modView.AppendColumn("Delete?", toggleRenderer, "active", 2);
			}
			
			modView.Columns[0].Alignment = 0.0f;
			modView.Columns[1].Alignment = 0.0f;
			modView.Columns[1].Expand = true;
			modView.Columns[2].Alignment = 1.0f;
			
			modView.Reorderable = true;
			
			LoadMods();
			
			// Auto-refresh
			Inst.InstMods.ModFileChanged += (sender, e) =>
				Application.Invoke((sender2, e2) => LoadMods());
		}
		
		protected void LoadMods()
		{
			modList.Clear();
			foreach (string instMod in Inst.InstMods)
			{
				modList.AppendValues(instMod, Inst.InstMods[instMod] + 1, false);
			}
//			LoadMods(Inst.InstModsDir);
		}
		
//		private void LoadMods(string searchdir)
//		{
//			try
//			{
//				foreach (string f in Directory.GetFileSystemEntries(searchdir))
//				{
//					if (Directory.Exists(f))
//					{
//						LoadMods(f);
//					}
//					else if (File.Exists(f))
//					{
//						AddFile(f);
//					}
//				}
//			} catch (DirectoryNotFoundException)
//			{
//				return;
//			}
//		}
		
//		protected void AddFile(string fname)
//		{
//			modList.AppendValues(fname, 
//			                     File.GetCreationTime(fname).ToShortDateString() + " " + 
//			                     File.GetCreationTime(fname).ToShortTimeString(), 
//			                     false,
//			                     File.GetCreationTime(fname));
//		}

		protected void OnResponse(object o, Gtk.ResponseArgs args)
		{
			TreeIter iter;
			if (args.ResponseId == ResponseType.Ok && modList.GetIterFirst(out iter))
			{
				int i = 0;
				TreeIter itr;
				modList.GetIterFirst(out itr);
				
				do
				{
					Inst.InstMods[modList.GetValue(itr, 0).ToString()] = i;
					i++;
				} while (modList.IterNext(ref itr));
				
				do
				{
					if ((bool)modList.GetValue(iter, 2))
					{
						File.Delete((string)modList.GetValue(iter, 0));
					}
				} while (modList.IterNext(ref iter));
			}
			Inst.InstMods.Save();
			Inst.InstMods.Update();
			Destroy();
		}
		
		public Instance Inst
		{
			get;
			protected set;
		}

		protected void OnButtonViewFolderClicked(object sender, System.EventArgs e)
		{
			System.Diagnostics.Process.Start(Inst.InstModsDir);
		}
	}
}

