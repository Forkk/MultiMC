using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gtk;
using Glade;

using MultiMC.GUI;

namespace MultiMC.GTKGUI
{
	public class EditNotesDialog : GTKDialog, INotesDialog
	{
		[Widget]
		TextView editView = null;

		[Widget]
		VBox vboxEditNotes = null;

		public EditNotesDialog(Window parent)
			: base("Notes", parent)
		{
			XML gxml = new XML("MultiMC.GTKGUI.EditNotesDialog.glade",
				"vboxEditNotes");
			gxml.Autoconnect(this);

			this.VBox.PackStart(vboxEditNotes);

			AddButton("_Cancel", ResponseType.Cancel);
			AddButton("_OK", ResponseType.Ok);

			WidthRequest = 400;
			HeightRequest = 300;
		}

		public string Notes
		{
			get
			{
				return editView.Buffer.Text;
			}
			set
			{
				editView.Buffer.Text = value;
			}
		}
	}
}
