using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gtk;
using Glade;

using MultiMC.GUI;

namespace MultiMC.GTKGUI
{
	public class ChangeIconDialog : GTKDialog, IChangeIconDialog
	{
		[Widget]
		VBox vboxIconDialog = null;

		[Widget]
		IconView instIconView = null;

		ListStore iconStore;

		public ChangeIconDialog(Window parent)
			: base("Change Icon", parent)
		{
			XML gxml = new XML(null, "MultiMC.GTKGUI.ChangeIconDialog.glade",
				"vboxIconDialog", null);
			gxml.Autoconnect(this);

			this.VBox.PackStart(vboxIconDialog);

			this.AddButton("_Cancel", ResponseType.Cancel);
			this.AddButton("_OK", ResponseType.Ok);

			iconStore = new ListStore(typeof(string), typeof(Gdk.Pixbuf));
			instIconView.Model = iconStore;

			instIconView.TextColumn = 0;
			instIconView.PixbufColumn = 1;

			WidthRequest = 500;
			HeightRequest = 350;
		}

		public string ChosenIconKey
		{
			get
			{
				if (instIconView.SelectedItems.Length == 0)
					return null;

				TreeIter iter;
				iconStore.GetIter(out iter, instIconView.SelectedItems[0]);
				return iconStore.GetValue(iter, 0) as string;
			}
		}

		public IImageList ImageList
		{
			get { return imgList; }
			set
			{
				if (!(value is GTKImageList))
					throw new InvalidOperationException(
						"Value must be a GTK image list.");

				imgList = value as GTKImageList;
				LoadImageList();
			}
		}
		GTKImageList imgList;

		void LoadImageList()
		{
			iconStore.Clear();
			
			foreach (KeyValuePair<string, Gdk.Pixbuf> kv in imgList.ImgList)
			{
				iconStore.AppendValues(kv.Key, kv.Value);
			}
		}
	}
}
