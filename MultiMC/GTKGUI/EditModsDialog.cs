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
		
		[Widget]
		Button buttonUp = null;
		
		[Widget]
		Button buttonDown = null;
		
		[Widget]
		Button buttonAdd = null;
		
		[Widget]
		Button buttonRemove = null;
		
		[Widget]
		Button buttonOpenFolder = null;
		
		ListStore modStore;
		ListStore mlModStore;
		enum TargetType
		{
			Text,
			Uri,
			Movement
		};
		static Gtk.TargetEntry[] targetEntries =
		{
			new TargetEntry ("string", 0, (uint)TargetType.Text),
			new TargetEntry ("text/plain",	0, (uint)TargetType.Text),
			new TargetEntry ("text/uri-list", 0, (uint)TargetType.Uri),
			new TargetEntry ("minecraft/mod", 0, (uint)TargetType.Movement)
		};
		static Gtk.TargetEntry[] srcEntries =
		{
			new TargetEntry ("minecraft/mod", 0, (uint)TargetType.Movement)
		};
					
		Instance inst;
		enum Mode
		{
			Jar,
			Modloader,
			Resources
		};
		Mode currentMode;
		bool changed_mod_order = false;
		
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
			// the Jar page is active by default. FIXME: determine dynamically!
			currentMode = Mode.Jar;

			modStore = new ListStore(typeof(string), typeof(Mod));
			jarModList.Model = modStore;
			jarModList.AppendColumn("Mod Name", new CellRendererText(), "text", 0);

			mlModStore = new ListStore(typeof(string), typeof(Mod));
			mlModList.Model = mlModStore;
			mlModList.AppendColumn("Mod Name", new CellRendererText(), "text", 0);

			//mlModList.Selection.Mode = SelectionMode.Multiple;

			inst.InstMods.ModFileChanged += (o, args) => LoadModList();

			// Listen for key presses
			jarModList.KeyPressEvent += new KeyPressEventHandler(jarModList_KeyPressEvent);
			mlModList.KeyPressEvent += new KeyPressEventHandler(mlModList_KeyPressEvent);
			
			// set up drag & drop
			jarModList.EnableModelDragDest(targetEntries,Gdk.DragAction.Default);
			jarModList.EnableModelDragSource(Gdk.ModifierType.Button1Mask,srcEntries,Gdk.DragAction.Move);
			jarModList.DragDataReceived += OnDragDataReceived;
			jarModList.DragDataGet += (object o, DragDataGetArgs args) =>
			{
				TreeIter iter;
				TreeModel model;
				if(!jarModList.Selection.GetSelected (out iter))
					return;
				Gdk.Atom[] targets = args.Context.Targets;
				TreePath tp = modStore.GetPath(iter);
				int idx = tp.Indices[0];
				
				args.SelectionData.Set(targets[0],0,System.Text.Encoding.UTF8.GetBytes( idx.ToString() ));
			};
			
			Drag.DestSet(mlModList, DestDefaults.All, targetEntries, Gdk.DragAction.Default);
			mlModList.DragDataReceived += OnDragDataReceived;
		}
		
		void OnDragDataReceived (object o, DragDataReceivedArgs args)
		{
			Console.WriteLine("OnDragDataReceived: {0}", (TargetType)args.Info);
			Console.WriteLine("Position: {0} {1}", args.X, args.Y);
			switch ((TargetType)args.Info)
			{
				case TargetType.Text:
					// preprocess input string...
					String input = Encoding.UTF8.GetString(args.SelectionData.Data).Trim();
					string[] words = input.Split('\n');
					List<string> nwords = new List<string>();
					foreach (string word in words)
					{
						String nword = word;
						// let's just ignore anything that's not a zip or jar
						if(!word.ToLower().Contains(".jar") && !word.ToLower().Contains(".zip"))
							continue;
						// trim nonsense
						if(word.StartsWith("file://"))
						{
							nword = nword.Substring(7);
						}
						nwords.Add(nword);
					}
					// filtered away everything... just stop.
					if(nwords.Count == 0)
						return;
					// if the drop target is the jar list
					if (o == jarModList)
					{
						TreePath path;
						TreeIter iter;
						TreeViewDropPosition droppos;
						jarModList.GetDestRowAtPos (args.X,args.Y,out path, out droppos);
						jarModList.Model.GetIter (out iter, path);
						if(path != null)
						{
							Console.WriteLine("Got position ;) " + path.Indices[0]);
							switch(droppos)
							{
								case TreeViewDropPosition.Before:
								case TreeViewDropPosition.IntoOrBefore:
									AddJarMods(nwords.ToArray(), path.Indices[0]);
								break;
								case TreeViewDropPosition.After:
								case TreeViewDropPosition.IntoOrAfter:
									AddJarMods(nwords.ToArray(), path.Indices[0]+1);
								break;
							}
						}
						else
						{
							Console.WriteLine("Indeterminate position... ");
							AddJarMods(nwords.ToArray(), -1);
						}
					}
					// if it's the modloader list
					else if (o == mlModList)
						AddMLMods(nwords.ToArray());
					break;
	
				// let's hope this never happens, because it sure doesn't happen here. :whistles:
				case TargetType.Uri:
					foreach (string uri in Encoding.UTF8.GetString(args.SelectionData.Data).Trim().Split('\n'))
						Console.WriteLine("Uri: {0}", Uri.UnescapeDataString(uri));
					break;
				// Handle reorder events inside the list.
				// This is only needed because the Reorderable property stops working
				// when you use custom drag&drop handling
				case TargetType.Movement:
					if (o == jarModList)
					{
						TreePath path;
						TreeIter srcIter;
						TreeIter destIter;
						TreeViewDropPosition droppos;
						jarModList.GetDestRowAtPos (args.X,args.Y,out path, out droppos);
						String sourceStr = System.Text.Encoding.UTF8.GetString (args.SelectionData.Data);
						int sourceInt = int.Parse(sourceStr);
						int destInt = -1;
						modStore.IterNthChild(out srcIter,sourceInt);
						if(path != null)
						{
							jarModList.Model.GetIter (out destIter, path);
							destInt = path.Indices[0];
							switch(droppos)
							{
								case TreeViewDropPosition.Before:
								case TreeViewDropPosition.IntoOrBefore:
									modStore.MoveBefore(srcIter,destIter);
								break;
								case TreeViewDropPosition.After:
								case TreeViewDropPosition.IntoOrAfter:
									modStore.MoveAfter(srcIter,destIter);
								break;
							}
							
							Console.WriteLine("Moving something!: from {0} to {1}", sourceInt, destInt );
						}
						else
						{
							// HACK : this is needed becuase of a GTK# API bug.
							// See: http://mono.1490590.n4.nabble.com/On-a-ListStore-moving-items-on-the-list-some-questions-td2222829.html
							TreeIter stDestIter;
							modStore.GetIterFromString(out stDestIter, (Convert.ToUInt16(modStore.IterNChildren()) - 1).ToString());
							modStore.MoveAfter(srcIter, stDestIter);
							Console.WriteLine("Moving something!: from {0} to End", sourceInt );
						}
						// mod order most probably did change.
						changed_mod_order = true;
					}
					break;
			}
			Console.WriteLine();
			bool success = true;
			Gtk.Drag.Finish (args.Context, success, false, args.Time);
		}
		
		void remove_selected_mods(Gtk.TreeView widget)
		{

			TreeIter iter;
			TreeModel model;
			if(widget.Selection.GetSelected(out model, out iter))
			{
				Mod m = (Mod) model.GetValue (iter, 1);
				string file = m.FileName;
	
				if (File.Exists(file))
					File.Delete(file);
				else if (Directory.Exists(file))
					Directory.Delete(file, true);
				LoadModList();
			}
		}
		void move_selected_jar_mods(bool move_up)
		{
			TreeIter iter;
			TreeModel model;
			ListStore model_real;
			// get selected item, if any
			if(jarModList.Selection.GetSelected(out model, out iter))
			{
				model_real = model as ListStore;
				
				if(move_up)
				{
					TreeIter firstIter;
					// is there at least one row? (better to be sure!)
					if(!model_real.GetIterFirst(out firstIter))
						return;
					// don't move first line up into the void
					if(firstIter.Equals(iter))
						return;
					System.Console.WriteLine("Can move up");
					TreePath path = model.GetPath(iter);
					TreePath prevPath = path;
					if(prevPath.Prev())
					{
						TreeIter prevIter;
						model.GetIter(out prevIter,prevPath);
						model_real.Swap(iter, prevIter);
						changed_mod_order = true;
					}
				}
				else
				{
					TreeIter iternext = iter;
					if(model.IterNext(ref iternext))
					{
						model_real.Swap(iter, iternext);
						changed_mod_order = true;
					}
				}
			}
		}
		
		void jarModList_KeyPressEvent(object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Delete)
			{
				remove_selected_mods(jarModList);
			}
		}
		void mlModList_KeyPressEvent(object o, KeyPressEventArgs args)
		{
			if (args.Event.Key == Gdk.Key.Delete)
			{
				remove_selected_mods(mlModList);
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
		private Mod GetLinkedJarMod(TreeIter item)
		{
			return modStore.GetValue(item, 1) as Mod;
		}

		public void SaveModList()
		{
			// no need to save anything if we didn't move any mods around
			if(!changed_mod_order)
				return;
			int i = 0;
			TreeIter iter;
			// there are no more jar mods...
			if(!modStore.GetIterFirst(out iter))
				return;
			// really a foreach. The GTK people added their own bastardized foreach I refuse to use.
			do
			{
				inst.InstMods[GetLinkedJarMod(iter)] = i;
				i++;
			} while(modStore.IterNext(ref iter));
			inst.InstMods.Save();
			inst.NeedsRebuild = true;
		}
		
		void OnPageChange(object sender, SwitchPageArgs e)
		{
			// it's kinda iffy using constants like this. DO NOT LIKE :(
			if(e.PageNum == 0)
			{
				currentMode = Mode.Jar;
				buttonUp.Sensitive = true;
				buttonDown.Sensitive = true;
			}
			else if(e.PageNum == 1)
			{
				currentMode = Mode.Modloader;
				buttonUp.Sensitive = false;
				buttonDown.Sensitive = false;
			}
			System.Console.WriteLine("Notebook page: " + e.PageNum);
		}

		void OnAddClicked(object sender, EventArgs e)
		{
			switch(currentMode)
			{
			case Mode.Jar:
				AddJarMods(ChooseFiles("Add mods to jar"));
				break;
			case Mode.Modloader:
				AddMLMods(ChooseFiles("Add ModLoader mods"));
				break;
			case Mode.Resources:
				System.Console.WriteLine("Adding resources not implemented yet.");
				break;
			}
		}
		
		void OnRemoveClicked(object sender, EventArgs e)
		{
			switch(currentMode)
			{
			case Mode.Jar:
				remove_selected_mods(jarModList);
				break;
			case Mode.Modloader:
				remove_selected_mods(mlModList);
				break;
			case Mode.Resources:
				System.Console.WriteLine("Removing resources not implemented yet.");
				break;
			}
		}
		
		void OnUpDownClicked(object sender, EventArgs e)
		{
			bool move_up = (sender == buttonUp);
			switch(currentMode)
			{
			case Mode.Jar:
				move_selected_jar_mods(move_up);
				break;
			case Mode.Modloader:
			case Mode.Resources:
				System.Console.WriteLine("Moving resources and modloader mods around is nonsense.");
				break;
			}
		}

		void OnViewFolderClicked(object sender, EventArgs e)
		{
			switch(currentMode)
			{
			case Mode.Jar:
				if (!Directory.Exists(inst.InstModsDir))
					Directory.CreateDirectory(inst.InstModsDir);
				Process.Start(inst.InstModsDir);
				break;
			case Mode.Modloader:
				if (!Directory.Exists(inst.ModLoaderDir))
					Directory.CreateDirectory(inst.ModLoaderDir);
				Process.Start(inst.ModLoaderDir);
				break;
			case Mode.Resources:
				System.Console.WriteLine("Unimplemented :(");
				break;
			}
			
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
			
		// FIXME: this is generic and doesn't belong in the GUI
		void AddJarMods(string[] files, int index = -1)
		{
			foreach (string file in files)
			{
				inst.InstMods.RecursiveCopy(file, index);
			}
			LoadModList();
		}
		// FIXME: this is generic and doesn't belong in the GUI
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
		// FIXME: this is generic and doesn't belong in the GUI
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
