using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MultiMC.GUI;
using MultiMC.Tasks;

namespace MultiMC
{
	public class TaskStatus : ITaskStatus
	{
		public TaskStatus(int id)
		{
			this.ID = id;
		}

		public int ID
		{
			get;
			protected set;
		}

		public int Progress
		{
			get { return _progress; }
			set { _progress = value; }
		}
		int _progress;

		public string StatusMsg
		{
			get { return _status; }
			set { _status = value; }
		}
		string _status;

		protected virtual void OnProgressChanged(int newValue)
		{
			if (ProgressChanged != null)
				ProgressChanged(this, new Task.ProgressChangeEventArgs(newValue));
		}

		protected virtual void OnStatusChanged(string newValue)
		{
			if (StatusChanged != null)
				StatusChanged(this, new Task.TaskStatusEventArgs(newValue));
		}

		public event EventHandler<Task.ProgressChangeEventArgs> ProgressChanged;

		public event EventHandler<Task.TaskStatusEventArgs> StatusChanged;
	}
}
