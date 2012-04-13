using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

using Gtk;
using Glade;

using MultiMC.GUI;
using MultiMC.Mods;

namespace MultiMC.GTKGUI
{
	public class EditModsDialog : GTKDialog, IEditModsDialog
	{
		[Widget]
		VBox vboxEditMods = null;

		[Widget]
		TreeView jarModList = null;

		[Widget]
		TreeView mlModList = null;

		ListStore modStore;
		ListStore mlModStore;

		Instance inst;

		public EditModsDialog(Window parent, Instance inst)
			: base("Edit Mods", parent)
		{
			this.inst = inst;

			XML gxml = new XML(null, "MultiMC.GTKGUI.EditModsDialog.glade",
				"vboxEditMods", null);
			gxml.Autoconnect(this);

			this.VBox.PackStart(vboxEditMods);

			this.AddButton("_Cancel", ResponseType.Cancel);
			this.AddButton("_OK", ResponseType.Ok);

			WidthRequest = 600;
			HeightRequest = 500;

			modStore = new ListStore(typeof(string), typeof(Mod));
			jarModList.Model = modStore;
			jarModList.AppendColumn("Mod Name", new CellRendererText(), "text", 0);

			mlModStore = new ListStore(typeof(string), typeof(Mod));
			mlModList.Model = mlModStore;
			mlModList.AppendColumn("Mod Name", new CellRendererText(), "text", 0);

			mlModList.Selection.Mode = SelectionMode.Multiple;

			inst.InstMods.ModFileChanged += (o, args) => LoadModList();

			// Listen for key presses
			jarModList.KeyPressEvent += new KeyPressEventHandler(jarModList_KeyPressEvent);
			mlModList.KeyPressEvent += new KeyPressEventHandler(mlModList_KeyPressEvent);
			TargetEntry te = new TargetEntry ();
			te.Flags = TargetFlags.Widget;
			te.Target = "STRING"; 
			TargetEntry[] tes = new TargetEntry[1];
			tes[0]=te;
			Gtk.TargetList tel = new Gtk.TargetList(tes);
			Gtk.Drag.SourceSet (jarModList, Gdk.ModifierType.Button1Mask, tes, Gdk.DragAction.Move);
			Gtk.Drag.DestSetTargetList (jarModList, tel);
			jarModList.DragDataGet += new DragDataGetHandler (ddgh);
			jarModList.DragDataReceived += new DragDataReceivedHandler (ddrh);
		}
		void ddgh (object sender, DragDataGetArgs args)
		{
			Console.WriteLine ("DragDataGet..");
			args.SelectionData.Text = "text";
		}
		void ddrh (object sender, DragDataReceivedArgs args)
		{
			Console.WriteLine("DragDataReceived... info: "+
			args.Info+" X: "+args.X+" Y: "+args.Y+" selection Data: "+ 
			args.SelectionData.Data + " text: " +
			args.SelectionData.Text);
		}
		void remove_selected_mods(Gtk.TreeView widget, Gtk.ListStore storage)
		{
			if (widget.Selection.CountSelectedRows() == 0)
				return;

			TreeIter iter;
			widget.Selection.GetSelected(out iter);
			
			Mod m = (storage.GetValue(iter, 1) as Mod);
			string file = m.FileName;

			if (File.Exists(file))
				File.Delete(file);
			else if (Directory.Exists(file))
				Directory.Delete(file, true);
			LoadModList();
		}
		void jarModList_KeyPressEvent(object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Delete)
			{
				remove_selected_mods(jarModList,modStore);
			}
		}
		void mlModList_KeyPressEvent(object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Delete)
			{
				remove_selected_mods(mlModList,mlModStore);
			}
		}

		public void LoadModList()
		{
			modStore.Clear();
			foreach (Mod mod in inst.InstMods)
			{
				string itemLabel = mod.Name;
				modStore.AppendValues(itemLabel, mod);
			}

			mlModStore.Clear();
			if (Directory.Exists(inst.ModLoaderDir))
			{
				foreach (string file in Directory.GetFileSystemEntries(inst.ModLoaderDir))
				{
					Mod mod = new Mod(file);
					string itemLabel = System.IO.Path.GetFileName(file);
					if (mod.Name != mod.FileName)
						itemLabel = mod.Name;
					
					mlModStore.AppendValues(itemLabel, mod);
				}
			}
			
		}

		public void SaveModList()
		{
			
		}

		void OnViewJarModFolderClicked(object sender, EventArgs e)
		{
			if (!Directory.Exists(inst.InstModsDir))
				Directory.CreateDirectory(inst.InstModsDir);
			Process.Start(inst.InstModsDir);
		}

		void OnAddJarModClicked(object sender, EventArgs e)
		{
			AddJarMods(ChooseFiles("Add mods to jar"));
		}
		void OnRmJarModClicked(object sender, EventArgs e)
		{
			remove_selected_mods(jarModList,modStore);
		}

		void OnViewModsFolderClicked(object sender, EventArgs e)
		{
			if (!Directory.Exists(inst.ModLoaderDir))
				Directory.CreateDirectory(inst.ModLoaderDir);
			Process.Start(inst.ModLoaderDir);
		}

		void OnAddMLModClicked(object sender, EventArgs e)
		{
			AddMLMods(ChooseFiles("Add ModLoader mods"));
		}
		void OnRmMLModClicked(object sender, EventArgs e)
		{
			remove_selected_mods(mlModList,mlModStore);
		}
		
		string[] ChooseFiles(string dialogTitle)
		{
			FileChooserDialog fcDialog = new FileChooserDialog(
				dialogTitle, this, FileChooserAction.Open);
			fcDialog.AddButton("_Cancel", ResponseType.Cancel);
			fcDialog.AddButton("_Add", ResponseType.Ok);

			string[] files = new string[0];

			fcDialog.Response += (o, args) =>
				{
					if (args.ResponseId == ResponseType.Ok)
						files = fcDialog.Filenames;

					fcDialog.Destroy();
				};

			fcDialog.Run();
			return files;
		}
			
		void AddJarMods(string[] files, int index = -1)
		{
			foreach (string file in files)
			{
				inst.InstMods.RecursiveCopy(file, index);
			}
			LoadModList();
		}

		void AddMLMods(string[] files)
		{
			try
			{
				if (!Directory.Exists(inst.ModLoaderDir))
					Directory.CreateDirectory(inst.ModLoaderDir);

				foreach (string file in files)
				{
					if (Directory.Exists(file))
					{
						RecursiveCopy(file, System.IO.Path.Combine(inst.ModLoaderDir,
							System.IO.Path.GetFileName(file)), true);
					}
					else if (File.Exists(file))
						File.Copy(file, System.IO.Path.Combine(inst.ModLoaderDir,
							System.IO.Path.GetFileName(file)), true);
				}
				LoadModList();
			}
			catch (UnauthorizedAccessException)
			{
				GUI.MessageDialog.Show(this,
					"Can't copy files to mods folder. Access was denied.",
					"Failed to copy files");
			}
			catch (IOException err)
			{
				GUI.MessageDialog.Show(this,
					"Can't copy files to mods folder. An unknown error occurred: " +
					err.Message,
					"Failed to copy files");
			}
		}

		private void RecursiveCopy(string source, string dest, bool overwrite = false)
		{
			if (!Directory.Exists(dest))
				Directory.CreateDirectory(dest);

			if (File.Exists(source))
				File.Copy(source, System.IO.Path.Combine(dest, 
					System.IO.Path.GetFileName(source)), overwrite);

			foreach (string file in Directory.GetFiles(source))
			{
				File.Copy(file, System.IO.Path.Combine(dest,
					System.IO.Path.GetFileName(file)), overwrite);
			}
			foreach (string dir in Directory.GetDirectories(source))
			{
				string newdest = System.IO.Path.Combine(dest,
					System.IO.Path.GetFileName(dir));
				RecursiveCopy(dir, newdest, overwrite);
			}
		}
	}
}
