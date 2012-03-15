using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MultiMC.Tasks;

namespace MultiMC.GUI
{
	public interface ITaskStatus
	{
		int ID
		{
			get;
		}

		int Progress
		{
			get;
			set;
		}

		string StatusMsg
		{
			get;
			set;
		}

		event EventHandler<Task.ProgressChangeEventArgs> ProgressChanged;
		event EventHandler<Task.TaskStatusEventArgs> StatusChanged;
	}
}
