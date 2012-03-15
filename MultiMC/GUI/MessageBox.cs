using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	public static class MessageBox
	{
		public static DialogResponse Show(
			IWindow parent,
			string text,
			string title = "",
			MessageButtons buttons = MessageButtons.Ok)
		{
			switch (Program.Toolkit)
			{
			case WindowToolkit.WinForms:
				return WinGUI.WinFormsMessageBox.Show(parent, text, title, buttons); 

			default:
				throw new NotImplementedException();
			}
		}
	}

	public enum MessageButtons
	{
		Ok,
		OkCancel,
		YesNo,
		YesNoCancel,
		AbortRetryIgnore,
		RetryCancel
	}
}
