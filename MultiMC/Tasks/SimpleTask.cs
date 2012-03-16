using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiMC.Tasks
{
	public class SimpleTask : Task
	{
		ThreadStart start;

		public SimpleTask(ThreadStart start, string message = "")
		{
			this.start = start;
			Status = message;
		}

		protected override void TaskStart()
		{
			OnStart();
			start();
			OnComplete();
		}
	}
}
