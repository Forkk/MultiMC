using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gtk;
using Glade;

using MultiMC.GUI;
using MultiMC.Tasks;

namespace MultiMC.GTKGUI
{
	public class MainWindow : GTKWindow, IMainWindow
	{
		ListStore instListStore;

		Dictionary<int, Box> taskProgBars;

		public MainWindow()
		{
			XML gxml = new XML(null, "MultiMC.GTKGUI.MainWindow.glade", 
				"mainVBox", null);
			gxml.Toplevel = this;
			gxml.Autoconnect(this);

			XML gxml2 = new XML(null, "MultiMC.GTKGUI.InstContextMenu.glade",
				"instContextMenu", null);
			gxml2.Autoconnect(this);

			this.Add(mainVBox);

			ShowAll();

			this.WidthRequest = 620;
			this.HeightRequest = 380;

			DeleteEvent += (o, args) => Application.Quit();

			// Set up the instance icon view
			instListStore = new ListStore(
				typeof(string), typeof(Gdk.Pixbuf), typeof(Instance));

			instView.Model = instListStore;
			instView.TextColumn = 0;
			instView.PixbufColumn = 1;
			instView.ItemWidth = -1;
			
			Gtk.CellRendererText crt = instView.Cells[0] as CellRendererText;
			crt.Editable = true;
			crt.Edited += (object o, EditedArgs args) =>
			{
				int EditedIndex = int.Parse(args.Path);
				TreeIter iter;
				// don't allow bad names
				if(!Instance.NameIsValid(args.NewText))
					return;
				System.Console.WriteLine("Path: " + args.Path + " New text: " + args.NewText);
				if(instListStore.GetIterFromString(out iter,args.Path))
				{
					instListStore.SetValue(iter, 0, args.NewText);
					Instance inst = instListStore.GetValue(iter, 2) as Instance;
					inst.Name = args.NewText;
				}
				
			};
			
			instView.Orientation = Orientation.Vertical;
			instView.ButtonPressEvent += (o, args) =>
				{
					if (args.Event.Button == 3 &&
						instView.GetPathAtPos((int)args.Event.X,
											  (int)args.Event.Y) != null)
					{
						instView.SelectPath(instView.GetPathAtPos(
							(int)args.Event.X, (int)args.Event.Y));
						instContextMenu.Popup();
					}
				};

			// Set up the task list
			EventfulList<Task> tList = new EventfulList<Task>();
			tList.Added += TaskAdded;
			tList.Removed += TaskRemoved;

			TaskList = tList;
			taskProgBars = new Dictionary<int, Box>();

			// Set up the instance list
			EventfulList<Instance> iList = new EventfulList<Instance>();
			iList.Added += InstAdded;
			iList.Removed += InstRemoved;

			InstanceList = iList;

			helpButton.Sensitive = false;
			if(OSUtils.OS == OSEnum.Linux)
			{
				Gtk.MenuItem openalRemoveItem = gxml2.GetWidget("removeOpenALMenuItem") as Gtk.MenuItem;
				openalRemoveItem.Visible = true;
			}
		}

		void InstAdded(object sender, ItemAddRemoveEventArgs<Instance> e)
		{
			Instance inst = e.Item;
			instListStore.AppendValues(
				inst.Name, _imageList.ImgList[inst.IconKey], inst);
		}

		void InstRemoved(object sender, ItemAddRemoveEventArgs<Instance> e)
		{
			TreeIter iter;
			instListStore.GetIterFirst(out iter);

			do
			{
				if (e.Item == instListStore.GetValue(iter, 2))
				{
					instListStore.Remove(ref iter);
					return;
				}
			} while (instListStore.IterNext(ref iter));
		}

		void TaskAdded(object sender, ItemAddRemoveEventArgs<Task> e)
		{
			Invoke((o, args) =>
				{
					TaskAdded(e.Item);
				});
		}

		void TaskAdded(Task task)
		{
			if (taskProgBars.ContainsKey(task.TaskID))
				task.TaskID = GetAvailableTaskID();

			HBox tHBox = new HBox();

			ProgressBar tProgBar = new ProgressBar();
			tProgBar.Text = task.Status;
			tProgBar.HeightRequest = 22;

			tHBox.PackStart(tProgBar, true, true, 0);

			task.StatusChange += TaskStatusChange;
			task.ProgressChange += TaskProgressChange;

			lock (this)
			{
				taskProgBars.Add(task.TaskID, tHBox);
			}
			progBarVBox.PackEnd(tHBox, false, true, 0);
			tHBox.ShowAll();
		}

		void TaskRemoved(object sender, ItemAddRemoveEventArgs<Task> e)
		{
			Invoke((o, args) => TaskRemoved(e.Item));
		}

		void TaskRemoved(Task task)
		{
			if (!taskProgBars.ContainsKey(task.TaskID))
				return;

			ProgressBar pbar = taskProgBars[task.TaskID].Children[0] as ProgressBar;
			pbar.Fraction = 1;
			pbar.Hide();
			mainVBox.Remove(taskProgBars[task.TaskID]);

			lock (this)
			{
				taskProgBars.Remove(task.TaskID);
			}
		}

		void TaskProgressChange(object sender, Task.ProgressChangeEventArgs e)
		{
			Invoke((o, args) => 
				{
					double pbarFraction = (float)e.Progress / 100f;
					if (pbarFraction > 1.0)
						pbarFraction = 1;

					(taskProgBars[e.TaskID].Children[0] as ProgressBar).
						Fraction = pbarFraction;
				});
			
		}

		void TaskStatusChange(object sender, Task.TaskStatusEventArgs e)
		{
			Invoke((o, args) =>
				{
					(taskProgBars[e.TaskID].Children[0] as ProgressBar).
						Text = e.Status;
				});
		}

		#region Glade Handlers
		// Menu Bar
		void OnNewInstanceClicked(object sender, EventArgs e)
		{
			if (AddInstClicked != null)
				AddInstClicked(this,
					new AddInstEventArgs(AddInstAction.CreateNew));
		}

		void OnViewFolderClicked(object sender, EventArgs e)
		{
			if (ViewFolderClicked != null)
				ViewFolderClicked(this, EventArgs.Empty);
		}

		void OnRefreshClicked(object sender, EventArgs e)
		{
			if (RefreshClicked != null)
				RefreshClicked(this, EventArgs.Empty);
		}


		void OnSettingsClicked(object sender, EventArgs e)
		{
			if (SettingsClicked != null)
				SettingsClicked(this, EventArgs.Empty);
		}

		void OnCheckUpdatesClicked(object sender, EventArgs e)
		{
			if (CheckUpdatesClicked != null)
				CheckUpdatesClicked(this, EventArgs.Empty);
		}


		void OnHelpClicked(object sender, EventArgs e)
		{
			if (HelpClicked != null)
				HelpClicked(this, EventArgs.Empty);
		}

		void OnAboutClicked(object sender, EventArgs e)
		{
			if (AboutClicked != null)
				AboutClicked(this, EventArgs.Empty);
		}


		// Instance Menu
		void OnPlayClicked(object sender, EventArgs e)
		{
			if (InstanceLaunched != null)
				InstanceLaunched(this, new InstActionEventArgs(SelectedInst));
		}

		void OnChangeIconClicked(object sender, EventArgs e)
		{
			if (ChangeIconClicked != null)
				ChangeIconClicked(this, new InstActionEventArgs(SelectedInst));
		}

		void OnEditNotesClicked(object sender, EventArgs e)
		{
			if (EditNotesClicked != null)
				EditNotesClicked(this, new InstActionEventArgs(SelectedInst));
		}

		void OnEditModsClicked(object sender, EventArgs e)
		{
			if (EditModsClicked != null)
				EditModsClicked(this, new InstActionEventArgs(SelectedInst));
		}

		void OnRebuildClicked(object sender, EventArgs e)
		{
			if (RebuildJarClicked != null)
				RebuildJarClicked(this, new InstActionEventArgs(SelectedInst));
		}

		void OnViewInstFolderClicked(object sender, EventArgs e)
		{
			if (ViewInstFolderClicked != null)
				ViewInstFolderClicked(this, new InstActionEventArgs(SelectedInst));
		}

		void OnDeleteClicked(object sender, EventArgs e)
		{
			if (DeleteInstClicked != null)
				DeleteInstClicked(this, new InstActionEventArgs(SelectedInst));
		}
		
		void OnRemoveOpenALClicked(object sender, EventArgs e)
		{
			if (RemoveOpenALClicked != null)
				RemoveOpenALClicked(this, new InstActionEventArgs(SelectedInst));
		}
		
		void OnRenameClicked(object sender, EventArgs e)
		{
			if(instView.SelectedItems.Count() != 0)
				instView.SetCursor(instView.SelectedItems[0], instView.Cells[0], true);
		}

		// Other
		void OnItemActivated(object sender, ItemActivatedArgs e)
		{
			OnPlayClicked(sender, e);
		}
		#endregion

		#region Glade Widgets
		//[Widget]
		//ToolButton newInstButton = null;

		//[Widget]
		//ToolButton viewInstFolderButton = null;

		//[Widget]
		//ToolButton refreshButton = null;

		//[Widget]
		//ToolButton settingsButton = null;
		
		//[Widget]
		//ToolButton updateButton = null;

		[Widget]
		ToolButton helpButton = null;
		
		//[Widget]
		//ToolButton aboutButton = null;

		[Widget]
		IconView instView = null;

		[Widget]
		VBox mainVBox = null;

		[Widget]
		VBox progBarVBox = null;

		[Widget]
		Menu instContextMenu = null;
		#endregion

		public void LoadInstances()
		{
			InstanceList.Clear();
			foreach (Instance inst in Instance.LoadInstances(AppSettings.Main.InstanceDir))
			{
				InstanceList.Add(inst);
			}
		}

		public IList<Task> TaskList
		{
			get;
			protected set;
		}

		public IList<Instance> InstanceList
		{
			get;
			protected set;
		}

		public event EventHandler<AddInstEventArgs> AddInstClicked;

		public event EventHandler ViewFolderClicked;

		public event EventHandler RefreshClicked;

		public event EventHandler SettingsClicked;

		public event EventHandler CheckUpdatesClicked;

		public event EventHandler HelpClicked;

		public event EventHandler AboutClicked;

		public event EventHandler<InstActionEventArgs> InstanceLaunched;

		public event EventHandler<InstActionEventArgs> ChangeIconClicked;

		public event EventHandler<InstActionEventArgs> EditNotesClicked;

		public event EventHandler<InstActionEventArgs> EditModsClicked;

		public event EventHandler<InstActionEventArgs> RebuildJarClicked;

		public event EventHandler<InstActionEventArgs> ViewInstFolderClicked;

		public event EventHandler<InstActionEventArgs> DeleteInstClicked;
		
		public event EventHandler<InstActionEventArgs> RemoveOpenALClicked;
		
		public IImageList ImageList
		{
			get { return _imageList; }
			set
			{
				if (value is GTKImageList)
				{
					_imageList = value as GTKImageList;
				}
				else if (value != null)
					throw new InvalidOperationException("GTK# needs a GTKImageList.");
				else
					throw new ArgumentNullException("value");
			}
		}
		GTKImageList _imageList;

		public Instance SelectedInst
		{
			get
			{
				if (instView.SelectedItems.Length <= 0)
					return null;

				TreeIter iter;
				instListStore.GetIter(out iter, instView.SelectedItems[0]);
				return instListStore.GetValue(iter, 2) as Instance;
			}
		}

		public Tasks.Task GetTaskByID(int taskID)
		{
			return TaskList.First(task => task.TaskID == taskID);
		}

		public bool IsTaskIDTaken(int taskID)
		{
			return TaskList.Any(task => task.TaskID == taskID);
		}

		public int GetAvailableTaskID()
		{
			int i = 0;
			while (IsTaskIDTaken(i))
			{
				i++;
			}
			return i;
		}


		public event EventHandler<InstActionEventArgs> ManageSavesClicked;

		public string ImportInstance()
		{
			FileChooserDialog browserDlg = new FileChooserDialog(
				"Import .minecraft folder", this, FileChooserAction.SelectFolder);
			browserDlg.SelectMultiple = false;

			browserDlg.AddButton("_Cancel", 0);
			browserDlg.AddButton("_Select Folder", 1);

			int response = browserDlg.Run();

			if (response == 1)
			{
				return browserDlg.Filename;
			}
			else
			{
				return null;
			}
		}


		public event EventHandler EscPressed;

		public string StatusText
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}
	}
}
