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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using MultiMC.GUI;

namespace MultiMC.WinGUI
{
	public partial class EditModsForm : WinFormsDialog, IEditModsDialog
	{
		Instance inst;

		public EditModsForm(Instance inst)
		{
			InitializeComponent();

			this.inst = inst;

			if (OSUtils.OS == OSEnum.Windows)
			{
				OSUtils.SetWindowTheme(modView.Handle, "explorer", null);
				OSUtils.SetWindowTheme(mlModView.Handle, "explorer", null);
			}

			inst.InstMods.ModFileChanged += InstMods_ModFileChanged;

			this.FormClosed += (o, args) =>
				inst.InstMods.ModFileChanged -= InstMods_ModFileChanged;
		}

		void InstMods_ModFileChanged(object sender, ModFileChangedEventArgs e)
		{
			LoadModList();
		}

		private Mod GetLinkedMod(ListViewItem item)
		{
			return item.Tag as Mod;
		}

		public void LoadModList()
		{
			if (InvokeRequired)
			{
				this.Invoke((o, args) => LoadModList());
			}
			else
			{
				modView.Items.Clear();
				foreach (Mod mod in inst.InstMods)
				{
					string itemLabel = mod.Name;

					ListViewItem item = new ListViewItem(itemLabel);
					item.Tag = mod;
					//item.Checked = true;

					modView.Items.Add(item);
				}

				mlModView.Items.Clear();
				if (Directory.Exists(inst.ModLoaderDir))
				{
					foreach (string file in Directory.GetFileSystemEntries(inst.ModLoaderDir))
					{
						Mod mod = new Mod(file);
						string itemLabel = Path.GetFileName(file);
						if (mod.Name != mod.FileName)
							itemLabel = mod.Name;

						ListViewItem item = new ListViewItem(itemLabel);
						item.Tag = new Mod(file);
						//item.Checked = true;
						mlModView.Items.Add(item);
					}
				}
			}
		}

		public void SaveModList()
		{
			int i = 0;
			foreach (ListViewItem item in modView.Items)
			{
				//if (item.Checked)
				//{
					inst.InstMods[GetLinkedMod(item).FileName] = i;
					i++;
				//}
				//else
				//{
				//    File.Delete(GetLinkedMod(item).FileName);
				//}
			}

			//foreach (ListViewItem item in mlModView.Items)
			//{
			//    if (!item.Checked)
			//    {
			//        if (File.Exists(GetLinkedMod(item).FileName))
			//            File.Delete(GetLinkedMod(item).FileName);
			//        else if (Directory.Exists(GetLinkedMod(item).FileName))
			//            Directory.Delete(GetLinkedMod(item).FileName, true);
			//    }
			//}
			inst.InstMods.Save();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			OnResponse(DialogResponse.Cancel);
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			OnResponse(DialogResponse.OK);
		}

		private void modView_ItemDrag(object sender, ItemDragEventArgs e)
		{
			DoDragDrop(modView.SelectedItems, DragDropEffects.Move);
		}

		private void modView_DragEnter(object sender, DragEventArgs e)
		{
			int len = e.Data.GetFormats().Length - 1;
			int i;
			for (i = 0; i <= len; i++)
			{
				if (e.Data.GetFormats()[i].Equals(
					"System.Windows.Forms.ListView+SelectedListViewItemCollection"))
				{
					e.Effect = DragDropEffects.Move;
				}
				else if (e.Data.GetFormats()[i] == "FileDrop")
				{
					string[] files = (string[])e.Data.GetData("FileDrop");
					e.Effect = DragDropEffects.Copy;
					foreach (string file in files)
					{
						if (!File.Exists(file))
							e.Effect = DragDropEffects.None;
					}
				}
			}
		}

		private void modView_DragDrop(object sender, DragEventArgs e)
		{
			int dragIndex = modView.InsertionMark.Index + 
				(modView.InsertionMark.AppearsAfterItem ? 1 : 0);
			if (modView.Items.Count <= 0)
				dragIndex = 0;

			if (e.Data.GetDataPresent(
				"System.Windows.Forms.ListView+SelectedListViewItemCollection") &&
				dragIndex > 0)
			{
				// Return if the items are not selected in the ListView control.
				if (modView.SelectedItems.Count == 0)
				{
					return;
				}

				// Returns the location of the mouse pointer in the ListView control.
				Point cp = modView.PointToClient(new Point(e.X, e.Y));


				//if (modView.InsertionMark.AppearsAfterItem)
				//    dragIndex++;
				ListViewItem[] sel = new ListViewItem[modView.SelectedItems.Count];
				for (int i = 0; i <= modView.SelectedItems.Count - 1; i++)
				{
					sel[i] = modView.SelectedItems[i];
				}
				for (int i = 0; i < sel.GetLength(0); i++)
				{
					// Obtain the ListViewItem to be dragged to the target location.
					ListViewItem dragItem = sel[i];
					int itemIndex = dragIndex;
					if (itemIndex == dragItem.Index)
					{
						return;
					}
					if (dragItem.Index < itemIndex)
						itemIndex++;
					else
						itemIndex = dragIndex + i;

					// Insert the item at the mouse pointer.
					ListViewItem insertItem = (ListViewItem)dragItem.Clone();
					modView.Items.Insert(itemIndex, insertItem);

					// Removes the item from the initial location while 
					// the item is moved to the new location.
					modView.Items.Remove(dragItem);
				}
			}
			else if (e.Data.GetDataPresent("FileDrop"))
			{
				string[] files = (string[])e.Data.GetData("FileDrop");
				e.Effect = DragDropEffects.Copy;
				AddMods(files, dragIndex);
			}
			else
			{
				e.Effect = DragDropEffects.None;
				modView.InsertionMark.Index = -1;
			}
		}

		private void AddMods(string[] files, int index)
		{
			foreach (string file in files)
			{
				inst.InstMods.RecursiveCopy(file, index);
			}
		}

		private void modView_DragOver(object sender, DragEventArgs e)
		{
			// Returns the location of the mouse pointer in the ListView control.
			Point cp = modView.PointToClient(new Point(e.X, e.Y));

			// Obtain the item that is located at the specified location of the mouse pointer.
			modView.InsertionMark.Index = modView.InsertionMark.NearestIndex(cp);
			if (modView.Items.Count > 0)
				modView.InsertionMark.AppearsAfterItem =
					modView.Items[modView.Items.Count - 1].Bounds.Bottom < cp.Y;
			else
				modView.InsertionMark.AppearsAfterItem = false;
			//if (modView.GetItemAt(cp.X, cp.Y) == null)
			//{
			//    modView.InsertionMark.Index = modView.Items.Count - 1;
			//    modView.InsertionMark.AppearsAfterItem = true;
			//}
			//else
			//{
			//    modView.InsertionMark.AppearsAfterItem = false;
			//    modView.InsertionMark.Index = modView.GetItemAt(cp.X, cp.Y).Index;
			//}
		}

		private void modView_DragLeave(object sender, EventArgs e)
		{
			modView.InsertionMark.Index = -1;
		}

		private void mlModView_DragOver(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("FileDrop"))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		private void mlModView_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("FileDrop"))
			{
				try
				{
					foreach (string file in e.Data.GetData("FileDrop") as string[])
					{
						if (Directory.Exists(file))
						{
							RecursiveCopy(file, Path.Combine(inst.ModLoaderDir,
								Path.GetFileName(file)), true);
						}
						else if (File.Exists(file))
							File.Copy(file, Path.Combine(inst.ModLoaderDir,
								Path.GetFileName(file)), true);
					}
				}
				catch (UnauthorizedAccessException)
				{
					MessageDialog.Show(this,
						"Can't copy files to mods folder. Access was denied.",
						"Failed to copy files");
				}
				catch (IOException err)
				{
					MessageDialog.Show(this,
						"Can't copy files to mods folder. An unknown error occurred: " + 
						err.Message,
						"Failed to copy files");
				}
			}
		}

		private void RecursiveCopy(string source, string dest, bool overwrite = false)
		{
			if (!Directory.Exists(dest))
				Directory.CreateDirectory(dest);

			if (File.Exists(source))
				File.Copy(source, Path.Combine(dest, Path.GetFileName(source)), overwrite);

			foreach (string file in Directory.GetFiles(source))
			{
				File.Copy(file, Path.Combine(dest, Path.GetFileName(file)), overwrite);
			}
			foreach (string dir in Directory.GetDirectories(source))
			{
				string newdest = Path.Combine(dest, Path.GetFileName(dir));
				RecursiveCopy(dir, newdest, overwrite);
			}
		}

		private void UpdateSizes()
		{
			modView.Columns[0].Width = modView.Width - 2;
			mlModView.Columns[0].Width = mlModView.Width - 2;
		}

		private void modView_Resize(object sender, EventArgs e)
		{
			UpdateSizes();
		}

		private void mlModView_Resize(object sender, EventArgs e)
		{
			UpdateSizes();
		}

		private void buttonExport_Click(object sender, EventArgs e)
		{

		}

		private void modView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Delete)
			{
				List<Mod> removedMods = new List<Mod>();
				foreach (ListViewItem item in modView.SelectedItems)
				{
					removedMods.Add(GetLinkedMod(item));
				}

				foreach (Mod mod in removedMods)
				{
					try
					{
						Console.WriteLine("Removing {0}", mod.FileName);
						if (File.Exists(mod.FileName))
							File.Delete(mod.FileName);
						else if (Directory.Exists(mod.FileName))
							Directory.Delete(mod.FileName);
					}
					catch (IOException err)
					{
						Console.WriteLine("Failed to remove mod '{0}'. {1}", 
							mod.Name, err.ToString());
					}
				}
			}
		}

		private void mlModView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Delete)
			{
				foreach (ListViewItem item in mlModView.SelectedItems)
				{
					if (File.Exists(GetLinkedMod(item).FileName))
						File.Delete(GetLinkedMod(item).FileName);
					else if (Directory.Exists(GetLinkedMod(item).FileName))
						Directory.Delete(GetLinkedMod(item).FileName, true);
				}
			}
		}
	}
}
