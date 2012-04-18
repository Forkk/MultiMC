using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using MultiMC.GUI;

namespace MultiMC.Tasks
{
	public class DirCopyTask : Task
	{
		public DirCopyTask(string src, string dest)
		{
			this.Source = src;
			this.Destination = dest;
		}

		protected override void TaskStart()
		{
			OnStart();
			Progress = 0;

			try
			{
				int pathLen = Source.Length + 1;
				string destDir = Destination;

				// Create a list of files to copy.
				Status = "Finding files...";
				Progress = 0;
				string[] fileList = Directory.GetFiles(Source,
					"*.*", SearchOption.AllDirectories);

				Status = "Building directory tree...";
				Progress = 10;
				foreach (string srcDir in Directory.GetDirectories(Source,
					"*", SearchOption.AllDirectories))
				{
					string dest = Path.Combine(destDir,
						srcDir.Substring(pathLen));
					Directory.CreateDirectory(dest);
				}

				Status = string.Format("Copying {0} files...", fileList.Length);
				Progress = 20;

				for (int i = 0; i < fileList.Length; i++)
				{
					Progress = (int)
						(((float)(i + 20) / (float)(fileList.Length + 20)) * 100f);

					string src = fileList[i];
					string dest = Path.Combine(destDir,
						src.Substring(pathLen));
					File.Copy(src, dest);
				}
			}
			catch (IOException ex)
			{
				OnErrorMessage("Failed to copy folder.\n" +
					ex.Message);

				OnComplete(false);
			}
			finally
			{
				OnComplete();
			}
		}

		public string Source
		{
			get;
			set;
		}

		public string Destination
		{
			get;
			set;
		}
	}
}
