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

			mlModView.Items.Clear();
			if (Directory.Exists(inst.ModLoaderDir))
			{
				foreach (string file in Directory.GetFiles(inst.ModLoaderDir))
				{
					ListViewItem item = new ListViewItem(file);
					item.Checked = true;
					mlModView.Items.Add(item);
				}
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

			foreach (ListViewItem item in mlModView.Items)
			{
				if (!item.Checked)
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
				inst.InstMods.InsertMod(file, index);
				LoadModList();
			}
		}

		private void modView_DragOver(object sender, DragEventArgs e)
		{
			// Returns the location of the mouse pointer in the ListView control.
			Point cp = modView.PointToClient(new Point(e.X, e.Y));

			// Obtain the item that is located at the specified location of the mouse pointer.
			modView.InsertionMark.Index = modView.InsertionMark.NearestIndex(cp);
			modView.InsertionMark.AppearsAfterItem = 
				modView.Items[modView.Items.Count-1].Bounds.Bottom < cp.Y;
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
				foreach (string file in e.Data.GetData("FileDrop") as string[])
				{
					File.Copy(file, Path.Combine(inst.ModLoaderDir, Path.GetFileName(file)), true);
				}
				LoadModList();
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
	}
}
