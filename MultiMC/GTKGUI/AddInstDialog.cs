using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gtk;
using Glade;

using MultiMC.GUI;

namespace MultiMC.GTKGUI
{
	public class AddInstDialog : GTKDialog, IAddInstDialog
	{
		public AddInstDialog(Window parentWindow = null)
			: base("Create new Instance", parentWindow)
		{
			XML gxml = gxml = new XML(null, "MultiMC.GTKGUI.AddInstDialog.glade",
				"hboxDialogContent", null);
			gxml.Autoconnect(this);

			this.VBox.PackStart(hboxDialogContent, false, true, 8);
			this.VBox.ShowAll();

			base.WidthRequest = 320;

			this.AddButton("_Cancel", ResponseType.Cancel);
			this.AddButton("_OK", ResponseType.Ok);

			base.SetPosition(WindowPosition.CenterOnParent);
		}

		[Widget]
		HBox hboxDialogContent = null;

		//[Widget]
		//Label entryLabel;

		[Widget]
		Entry entryInstName = null;

		public string InstName
		{
			get { return entryInstName.Text; }
			set { entryInstName.Text = value; }
		}
	}
}
