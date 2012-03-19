using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	public interface IEditModsDialog : IDialog
	{
		void LoadModList();
		void SaveModList();
	}
}
