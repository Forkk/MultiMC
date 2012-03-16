using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	public interface IConsoleWindow : IWindow
	{
		void Message(string text);

		bool ShowConsole
		{
			get;
			set;
		}

		event EventHandler ConsoleClosed;
	}
}
