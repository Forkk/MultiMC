using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gtk;
using Glade;

using MultiMC.GUI;

namespace MultiMC.GTKGUI
{
	public class UpdateDialog : GTKDialog, IUpdateDialog
	{
		[Widget]
		VBox vboxUpdateDialog = null;

		[Widget]
		Label messageLabel = null;

		public UpdateDialog(Window parent = null)
			: base("Update Available", parent)
		{
			XML gxml = new XML(null, "MultiMC.GTKGUI.UpdateDialog.glade",
				"vboxUpdateDialog", null);
			gxml.Autoconnect(this);

			this.VBox.PackStart(vboxUpdateDialog, true, true, 0);

			this.WidthRequest = 380;
			this.HeightRequest = 98;

			AddButton("_Yes", ResponseType.Yes);
			AddButton("_No", ResponseType.No);
		}

		public event EventHandler ViewChangelogClicked;

		public string Message
		{
			get { return messageLabel.Text; }
			set { messageLabel.Text = value; }
		}
	}
}
