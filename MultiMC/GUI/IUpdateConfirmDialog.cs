using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	public interface IUpdateDialog : IDialog
	{
		event EventHandler ViewChangelogClicked;

		string Message
		{
			get;
			set;
		}
	}
}
