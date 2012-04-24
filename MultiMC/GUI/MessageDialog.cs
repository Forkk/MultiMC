using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC.GUI
{
	public static class MessageDialog
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

			case WindowToolkit.GtkSharp:
				return GTKGUI.GTKMessageBox.Show(parent, text, title, buttons);

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
		RetryCancel
	}
}
