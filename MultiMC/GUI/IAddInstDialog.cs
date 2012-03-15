using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	public interface IAddInstDialog : IDialog
	{
		string InstName
		{
			get;
			set;
		}
	}
}
