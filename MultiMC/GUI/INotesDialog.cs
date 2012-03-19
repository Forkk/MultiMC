using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	public interface INotesDialog : IDialog
	{
		string Notes
		{
			get;
			set;
		}
	}
}
