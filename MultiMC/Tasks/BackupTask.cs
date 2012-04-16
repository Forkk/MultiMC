using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitSharp;

namespace MultiMC.Tasks
{
	/// <summary>
	/// Creates a backup by committing changes.
	/// </summary>
	public class BackupTask : Task
	{
		public BackupTask(WorldSave save, string backupName)
		{
			Save = save;
			BackupName = backupName;
		}

		public WorldSave Save
		{
			get;
			private set;
		}

		public string BackupName
		{
			get;
			private set;
		}

		protected override void TaskStart()
		{
			OnStart();
			Status = "Initializing...";
			using (Repository repo = GetRepo())
			{
				if (!repo.Status.AnyDifferences)
				{
					Status = "No changes to back up.";
					Progress = 100;
					Console.WriteLine("No changes to back up.");
					OnComplete();
					return;
				}

				Status = "Staging files...";
				Progress = 33;

				repo.Index.Stage(repo.Status.Modified.
					Where(file => !file.Contains(".git")).ToArray());
				repo.Index.Stage(repo.Status.Untracked.
					Where(file => !file.Contains(".git")).ToArray());
				repo.Index.Remove(repo.Status.Missing.
					Where(file => !file.Contains(".git")).ToArray());

				Status = "Committing...";
				Progress = 66;
				repo.Commit(BackupName, new Author("MultiMC", ""));
			}
			Status = "Done";
			Progress = 100;
			OnComplete();
		}

		private Repository GetRepo()
		{
			if (Repository.IsValid(Save.Path))
			{
				return new Repository(Save.Path);
			}
			else
			{
				return Repository.Init(Save.Path);
			}
		}
	}
}
