using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	public interface IRestoreBackupDialog : IDialog
	{
		string SelectedHash
		{
			get;
		}

		void LoadBackupList(WorldSave save);
	}
}
