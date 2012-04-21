using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gtk;
using Glade;

using MultiMC.GUI;

namespace MultiMC.GTKGUI
{
	public partial class SaveManagerDialog : GTKDialog, ISaveManagerDialog
	{
		ListStore savesStore;
		
		public event EventHandler BackupSaveClicked;
		public event EventHandler RestoreSaveClicked;
		
		public WorldSave SelectedSave
		{
			get
			{
				return currentSelection;
			}
		}
		
		WorldSave currentSelection;
		
		public void LoadSaveList(Instance inst)
		{
			foreach(WorldSave s in inst.Saves)
			{
				savesStore.AppendValues( s.FolderName, s );
			}
		}

		public SaveManagerDialog (Gtk.Window parent) : base ("Saves", parent)
		{
			this.IconName = "folder";
			
			XML gxml = new XML(null, "MultiMC.GTKGUI.SaveManagerDialog.glade", "saveManagerRoot", null);
			gxml.Toplevel = this;
			gxml.Autoconnect(this);
			
			this.VBox.PackStart(saveManagerRoot);
			
			this.WidthRequest = 620;
			this.HeightRequest = 380;
			
			// nothing selected by default
			btnRestore.Sensitive = false;
			btnBackup.Sensitive = false;
			
			savesStore = new ListStore(typeof(string), typeof(WorldSave));
			savesView.Model = savesStore;
			savesView.AppendColumn("Save Folder", new CellRendererText(), "text", 0);
			savesView.Selection.Mode = SelectionMode.Single;
			savesView.Selection.Changed += (sender, e) =>
			{
				if(savesView.Selection.CountSelectedRows() == 0)
				{
					btnRestore.Sensitive = false;
					btnBackup.Sensitive = false;
					currentSelection = null;
				}
				else
				{
					btnRestore.Sensitive = true;
					btnBackup.Sensitive = true;
					TreeIter iter;
					/*
					savesView.Selection.GetSelected(out iter);
					*/
					Gtk.TreePath[] paths = savesView.Selection.GetSelectedRows();
					TreePath tp = paths[0];
					savesStore.GetIter(out iter,tp);
					
					currentSelection = savesStore.GetValue(iter,1) as WorldSave;
				}
			};
			ShowAll();
		}
		void OnRestoreClicked(object sender, EventArgs e)
		{
			if(RestoreSaveClicked != null)
				RestoreSaveClicked(this, e);
		}
		void OnCreateClicked(object sender, EventArgs e)
		{
			if(BackupSaveClicked != null)
				BackupSaveClicked(this, e);
		}
		void OnCloseClicked(object sender, EventArgs e)
		{
			System.Console.WriteLine("Close!");
			this.Respond(ResponseType.Close);
		}
		#region Glade Widgets
		[Widget]
		VBox saveManagerRoot = null;
		
		[Widget]
		TreeView savesView = null;
		
		[Widget]
		Button btnRestore = null;
		
		[Widget]
		Button btnBackup = null;
		#endregion
	}
}

