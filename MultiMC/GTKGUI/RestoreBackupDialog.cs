using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using MultiMC.GUI;

using GitSharp;

using Gtk;
using Glade;

namespace MultiMC.GTKGUI
{
	public partial class RestoreBackupDialog : GTKDialog, IRestoreBackupDialog
	{
		ListStore backupsStore;
		
		string currentHash;
		
		public string SelectedHash
		{
			get
			{
				return currentHash;
			}
		}

		public void LoadBackupList(WorldSave save)
		{
			backupsStore.Clear();

			if (!Repository.IsValid(save.Path))
				return;

			using (Repository repo = new Repository(save.Path))
			{
				foreach (Commit commit in repo.CurrentBranch.CurrentCommit.Ancestors)
				{
					backupsStore.AppendValues( commit.Message, commit.CommitDate.LocalDateTime, commit.ShortHash, commit.Hash );
				}
			}
			
		}
		
		// feed Date column data to the text cell renderer
        protected void DateTimeCell(TreeViewColumn tree_column, CellRenderer cell, TreeModel tree_model, TreeIter iter)
        {
			if(backupsStore.GetValue(iter,1) == null)
				return;
            DateTime dt = (DateTime) backupsStore.GetValue(iter,1);
			
			CellRendererText crt = cell as CellRendererText;
			if(crt == null)
				return;
			crt.Text = dt.ToString();
			// it's possible to format the rendered text here... maybe it could show more info that way...
        }  
		
        public int DateTimeTreeIterCompareFunc(TreeModel _model, TreeIter a, TreeIter b)
        {
			object obja = backupsStore.GetValue(a,1);
			object objb = backupsStore.GetValue(b,1);
			if(obja == null && objb == null)
				return 0;
			else if(obja == null)
				return -1;
			else if(objb == null)
				return 1;
			DateTime dta = (DateTime) obja;
			DateTime dtb = (DateTime) objb;
			return dta < dtb ? -1 : (dta == dtb ? 0 : 1);
        }
            
		
		public RestoreBackupDialog (Gtk.Window parent) : base ("Saves", parent)
		{
			this.IconName = "document-revert";
			
			XML gxml = new XML(null, "MultiMC.GTKGUI.RestoreBackupDialog.glade", "restoreRoot", null);
			gxml.Toplevel = this;
			gxml.Autoconnect(this);
			
			this.VBox.PackStart(restoreRoot);
			
			this.WidthRequest = 620;
			this.HeightRequest = 380;

			// set default button states
			btnCancel.Sensitive = true;
			btnOK.Sensitive = false;
			
			// FIXME: store date/time properly so ordering works.
			backupsStore = new ListStore(typeof(string), typeof(DateTime), typeof(string), typeof(string));
			restoreView.Model = backupsStore;
			restoreView.AppendColumn("Backup name", new CellRendererText(), "text", 0);
			restoreView.AppendColumn("Date", new CellRendererText(), new TreeCellDataFunc(DateTimeCell));
			restoreView.AppendColumn("Hash", new CellRendererText(), "text", 2);
			restoreView.Selection.Mode = SelectionMode.Single;
			
			// this binds view and model columns together for sorting
			restoreView.Columns[0].SortColumnId = 0;
			restoreView.Columns[1].SortColumnId = 1;
			restoreView.Columns[2].SortColumnId = 2;
			// the date column needs a custom sorting function that can compare DateTime objects
			backupsStore.SetSortFunc(1,new TreeIterCompareFunc(DateTimeTreeIterCompareFunc));
			backupsStore.SetSortColumnId(1,SortType.Ascending); // sort by date
			restoreView.Selection.Changed += (sender, e) =>
			{
				if(restoreView.Selection.CountSelectedRows() != 0)
				{
					btnOK.Sensitive = true;
					TreeIter iter;
					restoreView.Selection.GetSelected(out iter);
					currentHash = backupsStore.GetValue(iter,3) as string;
				}
				else
				{
					btnOK.Sensitive = false;
				}
			};
			ShowAll();
		}
		void OnOKClicked(object sender, EventArgs e)
		{
			this.Respond(ResponseType.Ok);
		}
		void OnCancelClicked(object sender, EventArgs e)
		{
			this.Respond(ResponseType.Cancel);
		}
		#region Glade Widgets
		[Widget]
		VBox restoreRoot = null;
		
		[Widget]
		TreeView restoreView = null;

		[Widget]
		Button btnOK = null;
		
		[Widget]
		Button btnCancel = null;
		#endregion
	}
}

