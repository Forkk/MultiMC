using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	public interface IChangeIconDialog : IDialog
	{
		string ChosenIconKey
		{
			get;
		}

		IImageList ImageList
		{
			get;
			set;
		}
	}
}
