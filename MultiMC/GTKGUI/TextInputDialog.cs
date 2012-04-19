using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gtk;
using Glade;

using MultiMC.GUI;

namespace MultiMC.GTKGUI
{
	public class TextInputDialog : GTKDialog, ITextInputDialog
	{
		public TextInputDialog(string message = "", string text = "", Window parentWindow = null)
			: base("", parentWindow)
		{
			XML gxml = new XML(null, "MultiMC.GTKGUI.TextInputDialog.glade",
				"hboxDialogContent", null);
			gxml.Autoconnect(this);

			this.VBox.PackStart(hboxDialogContent, false, true, 8);
			this.VBox.ShowAll();

			base.WidthRequest = 320;

			this.AddButton("_Cancel", ResponseType.Cancel);
			this.AddButton("_OK", ResponseType.Ok);
			
			entryText.Changed += (sender, e) => {
				Text = entryText.Text;
			};
			
			base.SetPosition(WindowPosition.CenterOnParent);
		}
		
		String Text;
		
		[Widget]
		HBox hboxDialogContent = null;

		[Widget]
		Label entryLabel = null;

		[Widget]
		Entry entryText = null;
		
		public void HighlightText()
		{
			entryText.SelectRegion(0,entryText.Text.Count());
		}
		
		public string Message
		{
			get { return entryLabel.Text; }
			set { entryLabel.Text = value; }
		}

		public string Input
		{
			get
			{
				return Text;
			}
			set
			{
				Text = value;
				entryText.Text = value;
			}
		}
	}
}
