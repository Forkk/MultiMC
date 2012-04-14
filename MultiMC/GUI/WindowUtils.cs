using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MultiMC.Tasks;

namespace MultiMC.GUI
{
	public static class WindowUtils
	{
		public static void ConnectTask(this ITaskDialog dlg, Task task)
		{
			task.ProgressChange += (o, args) =>
				dlg.Invoke((o2, args2) =>
					dlg.TaskProgress = args.Progress);

			task.StatusChange += (o, args) =>
				dlg.Invoke((o2, args2) =>
					dlg.TaskStatus = args.Status);

			task.Completed += (o, args) =>
				dlg.Invoke((o2, args2) =>
					dlg.Close());

			dlg.TaskStatus = task.Status;
			dlg.TaskProgress = task.Progress;
		}
	}
}
