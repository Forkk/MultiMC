using System;
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
	public partial class SaveManagerDialog : WinFormsDialog, ISaveManagerDialog
	{
		public SaveManagerDialog()
		{
			InitializeComponent();

			UpdateButtons();

			if (OSUtils.OS == OSEnum.Windows)
				OSUtils.SetWindowTheme(saveView.Handle, "explorer", null);
		}

		public WorldSave GetLinkedSave(ListViewItem item)
		{
			return item.Tag as WorldSave;
		}

		public void SetLinkedSave(ListViewItem item, WorldSave save)
		{
			item.Tag = save;
		}

		public void LoadSaveList(Instance inst)
		{
			saveView.Items.Clear();

			List<WorldSave> saves = inst.Saves;
			foreach (WorldSave save in saves)
			{
				ListViewItem item = new ListViewItem(save.FolderName);
				SetLinkedSave(item, save);
				saveView.Items.Add(item);
			}
		}

		public event EventHandler BackupSaveClicked;

		public event EventHandler RestoreSaveClicked;

		public WorldSave SelectedSave
		{
			get
			{
				if (saveView.SelectedItems.Count == 0)
					return null;
				else
					return GetLinkedSave(saveView.SelectedItems[0]);
			}
		}

		private void buttonRestore_Click(object sender, EventArgs e)
		{
			if (RestoreSaveClicked != null)
				RestoreSaveClicked(this, EventArgs.Empty);
		}

		private void buttonCreateBackup_Click(object sender, EventArgs e)
		{
			if (BackupSaveClicked != null)
				BackupSaveClicked(this, EventArgs.Empty);
		}

		private void saveView_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateButtons();
		}

		void UpdateButtons()
		{
			buttonCreateBackup.Enabled = saveView.SelectedItems.Count > 0;
			buttonRestore.Enabled = saveView.SelectedItems.Count > 0;
		}
	}
}
