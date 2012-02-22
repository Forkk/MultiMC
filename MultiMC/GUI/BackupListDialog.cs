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

using GitSharp;

namespace MultiMC
{
	public partial class BackupListDialog : Gtk.Dialog
	{
		Repository repo;

		ListStore backupStore;

		public BackupListDialog(Window parent, string savePath)
			: base("Backups", parent, DialogFlags.Modal)
		{
			SavePath = savePath;

			this.Build();

			// Date, sha
			backupStore = new ListStore(typeof(string), typeof(string));

			backupView.Model = backupStore;

			backupView.AppendColumn("Date", new CellRendererText(), "text", 0);
			backupView.AppendColumn("Hash", new CellRendererText(), "text", 1);

			backupView.Columns[0].Alignment = 0.0f;
			backupView.Columns[0].Expand = true;
			backupView.Columns[1].Alignment = 1.0f;

			backupView.RowActivated += new RowActivatedHandler(RowActivated);

			repo = new Repository(SavePath);

			LoadBackups();
		}

		void RowActivated(object o, RowActivatedArgs args)
		{
			TreeIter iter;
			if (!backupStore.GetIter(out iter, args.Path))
				return;

			string sha = backupStore.GetValue(iter, 1).ToString();

			ResponseType response = MessageUtils.ShowMessageBox(this, 
				MessageType.Question, ButtonsType.YesNo,
				"Are you sure?", "Do you really want to roll back to this backup? ");

			if (response == ResponseType.Yes)
			{
				RestoreBackup(sha);
			}
		}

		public void LoadBackups()
		{
			repo = new Repository(SavePath);
			foreach (Commit commit in repo.CurrentBranch.CurrentCommit.Ancestors)
			{
				backupStore.AppendValues(commit.Message, commit.Hash);
			}
		}

		public void RestoreBackup(string sha)
		{
			DebugUtils.Print("Reverting to " + sha);
			Commit commit = repo.CurrentBranch.CurrentCommit.Ancestors.
				WhereTrue<Commit>(c => c.Hash == sha)[0];

			commit.Checkout();
		}

		public override void Dispose()
		{
			base.Dispose();
			repo.Dispose();
		}

		public string SavePath
		{
			get;
			protected set;
		}
	}
}

