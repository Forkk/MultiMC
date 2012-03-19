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
			}
		}

		public void LoadModList()
		{
			modView.Items.Clear();
			foreach (string file in inst.InstMods)
			{
				ListViewItem item = new ListViewItem(file);
				item.Checked = true;
				modView.Items.Add(item);
			}
		}

		public void SaveModList()
		{
			int i = 0;
			foreach (ListViewItem item in modView.Items)
			{
				if (item.Checked)
				{
					inst.InstMods[item.Text] = i;
					i++;
				}
				else
				{
					File.Delete(item.Text);
				}
			}
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
			int dragIndex = modView.InsertionMark.Index;

			if (e.Data.GetDataPresent(
				"System.Windows.Forms.ListView+SelectedListViewItemCollection"))
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
			else if (e.Data.GetDataPresent("FileDrop+FolderDrop"))
			{
				string[] files = (string[])e.Data.GetData("FileDrop+FolderDrop");
				e.Effect = DragDropEffects.Copy;
				AddMods(files, dragIndex);
			}
		}

		private void AddMods(string[] files, int index)
		{
			foreach (string file in files)
			{
				File.Copy(file, Path.Combine(inst.InstModsDir, Path.GetFileName(file)), true);
				EventHandler<ModFileChangedEventArgs> addedHandler = null;
				addedHandler = (o, args) =>
				{
					if (args.ModFile == file)
					{
						inst.InstMods[file] = index;
					}
					inst.InstMods.ModFileChanged -= addedHandler;
				};
				inst.InstMods.ModFileChanged += addedHandler;
				LoadModList();
			}
		}

		private void modView_DragOver(object sender, DragEventArgs e)
		{
			// Returns the location of the mouse pointer in the ListView control.
			Point cp = modView.PointToClient(new Point(e.X, e.Y));

			// Obtain the item that is located at the specified location of the mouse pointer.
			if (modView.GetItemAt(cp.X, cp.Y) == null)
			{
				modView.InsertionMark.Index = modView.Items.Count - 1;
				modView.InsertionMark.AppearsAfterItem = true;
			}
			else
			{
				modView.InsertionMark.AppearsAfterItem = false;
				modView.InsertionMark.Index = modView.GetItemAt(cp.X, cp.Y).Index;
			}
		}
	}
}
