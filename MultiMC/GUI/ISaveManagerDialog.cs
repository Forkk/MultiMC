using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	public interface ISaveManagerDialog : IDialog
	{
		event EventHandler BackupSaveClicked;
		event EventHandler RestoreSaveClicked;

		WorldSave SelectedSave
		{
			get;
		}

		void LoadSaveList(Instance inst);
	}
}
