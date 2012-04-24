using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MultiMC.GUI;

namespace MultiMC.WinGUI
{
	public static class WinFormsMessageBox
	{
		public static DialogResponse Show(
			string text, 
			string title = "",
			MessageButtons buttons = MessageButtons.Ok)
		{
			return Show(null, text, title, buttons);
		}

		public static DialogResponse Show(
			IWindow owner, 
			string text,
			string title = "",
			MessageButtons buttons = MessageButtons.Ok)
		{
			return ConvertResultEnum(System.Windows.Forms.MessageBox.Show(
				owner as IWin32Window, 
				text, 
				title, 
				ConvertButtonsEnum(buttons)));
		}

		static MessageBoxButtons ConvertButtonsEnum(MessageButtons mButtons)
		{
			switch (mButtons)
			{
			case MessageButtons.Ok:
				return MessageBoxButtons.OK;

			case MessageButtons.OkCancel:
				return MessageBoxButtons.OKCancel;

			case MessageButtons.RetryCancel:
				return MessageBoxButtons.RetryCancel;

			case MessageButtons.YesNo:
				return MessageBoxButtons.YesNo;

			case MessageButtons.YesNoCancel:
				return MessageBoxButtons.YesNoCancel;

			default:
				return MessageBoxButtons.OK;
			}
		}

		static DialogResponse ConvertResultEnum(DialogResult result)
		{
			switch (result)
			{
			case DialogResult.Abort:
				return DialogResponse.Abort;
			case DialogResult.Cancel:
				return DialogResponse.Cancel;
			case DialogResult.No:
				return DialogResponse.No;
			case DialogResult.OK:
				return DialogResponse.OK;
			case DialogResult.Yes:
				return DialogResponse.Yes;
			case DialogResult.Retry:
				return DialogResponse.OK;
			default:
				return DialogResponse.Other;
			}
		}
	}
}
