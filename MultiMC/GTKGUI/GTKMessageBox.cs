using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gtk;

using MultiMC.GUI;

namespace MultiMC.GTKGUI
{
	public class GTKMessageBox
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
			ResponseType response = ResponseType.None;

			Gtk.MessageDialog dialog = new Gtk.MessageDialog(
				(owner is Dialog ? owner as Dialog : owner as Window),
				DialogFlags.Modal, MessageType.Other,
				ConvertButtonsEnum(buttons), text);
			dialog.Title = title;

			dialog.Response += (o, args) =>
				{
					response = args.ResponseId;
					dialog.Destroy();
				};
			dialog.Run();

			return ConvertResultEnum(response);
		}
		/*.Show(
				(owner is Dialog ? owner as Dialog : owner as Window),
				text,
				title,
				ConvertButtonsEnum(buttons))
		 */

		private static ButtonsType ConvertButtonsEnum(MessageButtons buttons)
		{
			switch (buttons)
			{
			case MessageButtons.Ok:
				return ButtonsType.Ok;

			case MessageButtons.OkCancel:
				return ButtonsType.OkCancel;

			case MessageButtons.RetryCancel:
				return ButtonsType.OkCancel;

			case MessageButtons.YesNo:
				return ButtonsType.YesNo;

			case MessageButtons.YesNoCancel:
				return ButtonsType.YesNo;

			default:
				return ButtonsType.Ok;
			}
		}

		private static DialogResponse ConvertResultEnum(ResponseType dialogResult)
		{
			switch (dialogResult)
			{
			case ResponseType.Ok:
				return DialogResponse.OK;

			case ResponseType.Cancel:
				return DialogResponse.Cancel;

			case ResponseType.Yes:
				return DialogResponse.Yes;

			case ResponseType.No:
				return DialogResponse.No;

			default:
				return DialogResponse.Other;
			}
		}
	}
}
