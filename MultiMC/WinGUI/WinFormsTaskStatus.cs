using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MultiMC.GUI;
using MultiMC.Tasks;

namespace MultiMC.WinGUI
{
	public class WinFormsTaskStatus : ITaskStatus
	{
		public WinFormsTaskStatus(int id)
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
			get;
			set;
		}

		public string StatusMsg
		{
			get;
			set;
		}

		public event EventHandler<Task.ProgressChangeEventArgs> ProgressChanged;

		public event EventHandler<Task.TaskStatusEventArgs> StatusChanged;
	}
}
