using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	public interface IAboutDialog : IDialog
	{
		event EventHandler ChangelogClicked;
	}
}
