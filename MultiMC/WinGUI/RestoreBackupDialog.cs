using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MultiMC.GUI;

using GitSharp;

namespace MultiMC.WinGUI
{
	public partial class RestoreBackupDialog : WinFormsDialog, IRestoreBackupDialog
	{
		public RestoreBackupDialog()
		{
			InitializeComponent();
			backupView.Items.Clear();

			if (OSUtils.OS == OSEnum.Windows)
				OSUtils.SetWindowTheme(backupView.Handle, "explorer", null);
		}

		public void LoadBackupList(WorldSave save)
		{
			backupView.Items.Clear();

			if (!Repository.IsValid(save.Path))
				return;

			using (Repository repo = new Repository(save.Path))
			{
				foreach (Commit commit in repo.CurrentBranch.CurrentCommit.Ancestors)
				{
					DateTime commitTime = commit.CommitDate.LocalDateTime;

					ListViewItem item = new ListViewItem(commit.Message);
					ListViewItem.ListViewSubItem dateItem = item.SubItems.Add(
						commitTime.ToString());
					dateItem.Name = "date";

					ListViewItem.ListViewSubItem hashItem = 
						item.SubItems.Add(commit.ShortHash);
					hashItem.Name = "shorthash";

					ListViewItem.ListViewSubItem longHashItem =
						item.SubItems.Add(commit.Hash);
					longHashItem.Name = "hash";

					backupView.Items.Add(item);
				}
			}
		}

		public string SelectedHash
		{
			get { return backupView.SelectedItems[0].SubItems["hash"].Text; }
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			OnResponse(DialogResponse.OK);
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			OnResponse(DialogResponse.Cancel);
		}

		private void backupView_ItemActivate(object sender, EventArgs e)
		{
			OnResponse(DialogResponse.OK);
		}
	}
}
