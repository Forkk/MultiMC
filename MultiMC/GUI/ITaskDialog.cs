using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MultiMC.Tasks;

namespace MultiMC.GUI
{
	public interface ITaskDialog : IDialog
	{
		string TaskStatus
		{
			get;
			set;
		}

		int TaskProgress
		{
			get;
			set;
		}
	}
}
