using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitSharp;

namespace MultiMC.Tasks
{
	/// <summary>
	/// Restores a backup by checking out its commit.
	/// </summary>
	public class RestoreTask : Task
	{
		public RestoreTask(WorldSave save, string backupHash)
		{
			Save = save;
			BackupHash = backupHash;
		}

		public WorldSave Save
		{
			get;
			private set;
		}

		public string BackupHash
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
				Status = "Finding backup...";
				Commit restoreCommit =
					repo.CurrentBranch.CurrentCommit.Ancestors.FirstOrDefault(commit =>
						commit.Hash.StartsWith(BackupHash));

				if (restoreCommit == null)
				{
					Status = "No backup found.";
					Progress = 100;
					OnErrorMessage(string.Format("The hash '{0}' did not " + 
						"match any backups.", BackupHash));
					OnComplete(true);
					return;
				}
				else
				{
					Status = "Restoring backup...";
					Progress = 10;
					restoreCommit.Checkout();
				}
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
