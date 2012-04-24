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
using System.Diagnostics;

using MultiMC.GUI;
using MultiMC.Mods;

namespace MultiMC.WinGUI
{
	public partial class EditModsForm : WinFormsDialog, IEditModsDialog
	{
		Instance inst;

		public EditModsForm(Instance inst)
		{
			InitializeComponent();

			this.inst = inst;

			if (OSUtils.OS == OSEnum.Windows && OSUtils.Runtime != Runtime.Mono)
			{
				OSUtils.SetWindowTheme(modView.Handle, "explorer", null);
				OSUtils.SetWindowTheme(mlModView.Handle, "explorer", null);
				OSUtils.SetWindowTheme(resourceView.Handle, "explorer", null);
			}

			inst.InstMods.ModFileChanged += InstMods_ModFileChanged;

			this.FormClosed += (o, args) =>
				inst.InstMods.ModFileChanged -= InstMods_ModFileChanged;
		}

		#region Jar Mod List

		private void modView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Delete)
			{
				List<Mod> removedMods = new List<Mod>();
				foreach (ListViewItem item in modView.SelectedItems)
				{
					removedMods.Add(GetLinkedMod(item));
				}

				DeleteMods(removedMods);
			}
		}

		private void buttonRemoveJarMod_Click(object sender, EventArgs e)
		{
			List<Mod> removed = new List<Mod>();
			foreach (ListViewItem item in modView.SelectedItems)
			{
				removed.Add(GetLinkedMod(item));
			}

			DeleteMods(removed);
		}
		private void DeleteMods(List<Mod> removedMods)
		{
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
				dragIndex >= 0)
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
					//if (dragItem.Index < itemIndex)
					//    itemIndex++;
					//else
					//    itemIndex = dragIndex + i;

					// Insert the item at the mouse pointer.
					ListViewItem insertItem = (ListViewItem)dragItem.Clone();

					if (itemIndex >= modView.Items.Count)
						itemIndex = modView.Items.Count;

					modView.Items.Insert(itemIndex, insertItem);

					// Removes the item from the initial location while 
					// the item is moved to the new location.
					modView.Items.Remove(dragItem);
				}

				SaveModList();
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

		private void AddMods(string[] files, int index = -1)
		{
			if (index == -1)
				index = inst.InstMods.Count();

			foreach (string file in files)
			{
				inst.InstMods.RecursiveCopy(file, index);
			}
		}

		private void modView_Resize(object sender, EventArgs e)
		{
			UpdateSizes();
		}

		private void buttonMoveUp_Click(object sender, EventArgs e)
		{
			if (modView.SelectedItems.Count == 0)
				return;

			int newIndex = modView.SelectedItems[0].Index - 1;
			if (newIndex < 0)
				return;

			List<ListViewItem> moved = new List<ListViewItem>();
			foreach (ListViewItem item in modView.SelectedItems)
			{
				moved.Add(item);
			}

			foreach (ListViewItem item in moved)
			{
				int oldIndex = item.Index;
				modView.Items.Remove(item);
				modView.Items.Insert(oldIndex - 1, item);
			}
			SaveModList();
		}

		private void buttonMoveDown_Click(object sender, EventArgs e)
		{
			if (modView.SelectedItems.Count == 0)
				return;

			int newIndex = 
				modView.SelectedItems[modView.SelectedItems.Count - 1].Index + 1;
			if (newIndex >= modView.Items.Count)
				return;

			List<ListViewItem> moved = new List<ListViewItem>();
			foreach (ListViewItem item in modView.SelectedItems)
			{
				moved.Add(item);
			}

			for (int i = moved.Count - 1; i >= 0; i--)
			{
				ListViewItem item = moved[i];

				int oldIndex = item.Index;
				modView.Items.Remove(item);
				modView.Items.Insert(oldIndex + 1, item);
			}
			SaveModList();
		}

		private void buttonAddJarMod_Click(object sender, EventArgs e)
		{
			AddMods(AddModDialog("Add mod to minecraft.jar"));
		}

		#endregion

		#region Modloader Mod List

		private void mlModView_Resize(object sender, EventArgs e)
		{
			UpdateSizes();
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
					if (!Directory.Exists(inst.ModLoaderDir))
						Directory.CreateDirectory(inst.ModLoaderDir);

					AddMLMods(e.Data.GetData("FileDrop") as string[]);
					
					LoadModList();
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

		private void AddMLMods(IEnumerable<string> files)
		{
			foreach (string file in files)
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

		private void RemoveMLMods(IEnumerable<string> files)
		{
			foreach (string file in files)
			{
				if (File.Exists(file))
					File.Delete(file);
				else if (Directory.Exists(file))
					Directory.Delete(file, true);
			}
		}

		private void mlModView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Delete)
			{
				List<string> removedMods = new List<string>();
				foreach (ListViewItem item in mlModView.SelectedItems)
				{
					removedMods.Add(GetLinkedMod(item).FileName);
				}

				RemoveMLMods(removedMods);
				LoadModList();
			}
		}

		private void buttonAddMLMod_Click(object sender, EventArgs e)
		{
			AddMLMods(AddModDialog("Add mods to minecraft/mods"));
		}

		private void buttonRemoveMLMod_Click(object sender, EventArgs e)
		{
			List<string> removedMods = new List<string>();
			foreach (ListViewItem item in mlModView.SelectedItems)
			{
				removedMods.Add(GetLinkedMod(item).FileName);
			}

			RemoveMLMods(removedMods);
			LoadModList();
		}
		
		#endregion

		#region Resources List

		private void UpdateResourceList()
		{
			List<TreeNode> removed = new List<TreeNode>();
			RemoveRemovedResources(removed);

			foreach (TreeNode node in removed)
			{
				resourceView.Nodes.Remove(node);
			}

			LoadResourceList(inst.ResourceDir);
		}

		private void RemoveRemovedResources(List<TreeNode> removed, 
			TreeNode parent = null)
		{
			if (parent != null &&
				!File.Exists(GetLinkedRPath(parent)) &&
				!Directory.Exists(GetLinkedRPath(parent)))
			{
				parent.Remove();
			}
			else
			{
				TreeNodeCollection nodes = (parent != null? 
					parent.Nodes : 
					resourceView.Nodes);
				foreach (TreeNode node in nodes)
				{
					RemoveRemovedResources(removed, node);
				}
			}
		}

		private void LoadResourceList(string path, TreeNode parent = null)
		{
			TreeNode node = null;
			if (resourceView.Nodes.Find(path, true).Length != 0)
			{
				node = resourceView.Nodes.Find(path, true)[0];
			}
			else
			{
				node = (parent != null ?
					parent.Nodes.Add(path, Path.GetFileName(path)) :
					resourceView.Nodes.Add(path, Path.GetFileName(path)));

				SetLinkedRPath(node, path);
			}

			if (File.Exists(path))
			{
				// Nothing to do here yet.
			}
			else if (Directory.Exists(path))
			{
				foreach (string subPath in Directory.GetFileSystemEntries(path))
				{
					LoadResourceList(subPath, node);
				}
			}
		}

		private string GetLinkedRPath(TreeNode node)
		{
			return node.Name;
		}

		private void SetLinkedRPath(TreeNode node, string path)
		{
			node.Name = path;
			node.ToolTipText = path;
		}

		private void AddResources(IEnumerable<string> files, string dest)
		{
			foreach (string file in files)
			{
				try
				{
					AddResource(file, dest, false);
				}
				catch (IOException ex)
				{
					MessageDialog.Show(this,
						"Failed to copy resource file: " + ex.Message,
						"Error");
				}
				catch (UnauthorizedAccessException ex)
				{
					DialogResponse reply = MessageDialog.Show(this,
						"Failed to copy resource file: " + ex.Message,
						"Error", MessageButtons.OkCancel);

					if (reply == DialogResponse.Cancel)
					{
						return;
					}
				}
			}
			UpdateResourceList();
		}

		private void AddResource(string src, string dest, bool causeReload = true)
		{
			if (File.Exists(src))
			{
				File.Copy(src, Path.Combine(dest, Path.GetFileName(src)), true);
			}
			else if (Directory.Exists(src))
			{
				string newDest = Path.Combine(dest, Path.GetFileName(src));
				if (!Directory.Exists(newDest))
					Directory.CreateDirectory(newDest);
				AddResources(Directory.GetFileSystemEntries(src), newDest);
			}

			if (causeReload)
				UpdateResourceList();
		}

		private void resourceView_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("FileDrop"))
			{
				TreeNode target = resourceView.GetNodeAt(
					resourceView.PointToClient(new Point(e.X, e.Y)));

				if (target != null &&
					Directory.Exists(GetLinkedRPath(target)))
				{
					string dest = GetLinkedRPath(target);

					AddResources(e.Data.GetData("FileDrop") as string[], dest);
				}
			}
		}

		private void resourceView_DragLeave(object sender, EventArgs e)
		{
			resourceView.SelectedNode = null;
		}

		private void resourceView_DragEnter(object sender, DragEventArgs e)
		{

		}

		private void resourceView_DragOver(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent("FileDrop"))
			{
				TreeNode highlight = resourceView.GetNodeAt(
					resourceView.PointToClient(new Point(e.X, e.Y)));

				if (highlight != null && 
					Directory.Exists(GetLinkedRPath(highlight)))
				{
					e.Effect = DragDropEffects.Copy;
					resourceView.SelectedNode = highlight;
				}
				else
				{
					e.Effect = DragDropEffects.None;
					resourceView.SelectedNode = null;
				}
			}
		}

		#endregion

		#region Other

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

				UpdateResourceList();
			}
		}

		public void SaveModList()
		{
			int i = 0;
			foreach (ListViewItem item in modView.Items)
			{
				inst.InstMods[GetLinkedMod(item)] = i;
				i++;
			}
			inst.InstMods.Save();
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			OnResponse(DialogResponse.OK);
		}

		private void buttonExport_Click(object sender, EventArgs e)
		{

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
			modView.Columns[1].Width = 80;
			modView.Columns[0].Width = modView.Width - 
				modView.Columns[1].Width - 10;

			mlModView.Columns[0].Width = mlModView.Width - 10;
		}

		private void resourceView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Delete)
			{
				if (resourceView.SelectedNode != null)
				{
					string path = GetLinkedRPath(resourceView.SelectedNode);

					try
					{
						if (File.Exists(path))
							File.Delete(path);
						else if (Directory.Exists(path))
						{
							DialogResponse response = MessageDialog.Show(this,
								"Really delete this resource folder?",
								"Confirm Deletion", MessageButtons.OkCancel);
							if (response == DialogResponse.OK)
							{
								Directory.Delete(path, true);
							}
						}
					}
					catch (IOException ex)
					{
						MessageDialog.Show(this,
							"Failed to delete resources: " + ex.Message,
							"Error");
					}

					UpdateResourceList();
				}
			}
		}

		private string[] AddModDialog(string title)
		{
			OpenFileDialog fileDlg = new OpenFileDialog();
			fileDlg.Title = title;
			fileDlg.Filter = "Minecraft Mods (*.zip;*.jar)|*.zip;*.jar|All Files|*.*";
			fileDlg.InitialDirectory = Environment.CurrentDirectory;
			fileDlg.RestoreDirectory = true;

			fileDlg.Multiselect = true;

			fileDlg.CheckFileExists = true;
			fileDlg.CheckPathExists = true;

			fileDlg.DereferenceLinks = true;

			fileDlg.ShowDialog(this);

			return fileDlg.FileNames;
		}
		#endregion

		private void buttonModsFolder_Click(object sender, EventArgs e)
		{
			string modFolder = Path.GetFullPath(AppSettings.Main.CentralModsDir);

			if (!Directory.Exists(modFolder))
				Directory.CreateDirectory(modFolder);
			Process.Start(modFolder);
		}
	}
}
