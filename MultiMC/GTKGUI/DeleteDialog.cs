using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gtk;
using Glade;

using MultiMC.GUI;

namespace MultiMC.GTKGUI
{
	public class DeleteDialog : GTKDialog, IDialog
	{
		[Widget]
		VBox vboxDeleteConf = null;

		public DeleteDialog(Window parent)
			: base("Really delete this instance?", parent)
		{
			XML gxml = new XML("MultiMC.GTKGUI.DeleteDialog.glade",
				"vboxDeleteConf");
			gxml.Autoconnect(this);

			VBox.PackStart(vboxDeleteConf);

			AddButton("_Cancel", ResponseType.Cancel).HasDefault = true;
			AddButton("_OK", ResponseType.Ok);
		}
	}
}
