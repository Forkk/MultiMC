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
		Dictionary<int, ProgressBar> taskProgBars;

		public MainWindow()
		{
			XML gxml = gxml = new XML(null, "MultiMC.GTKGUI.gui.glade", 
				"mainVBox", null);
			gxml.Toplevel = this;
			gxml.Autoconnect(this);

			this.Add(mainVBox);

			ShowAll();

			this.WidthRequest = 620;
			this.HeightRequest = 400;

			DeleteEvent += (o, args) => Application.Quit();

			EventfulList<Task> tList = new EventfulList<Task>();
			tList.Added += new EventHandler<ItemAddRemoveEventArgs<Task>>(TaskAdded);

			TaskList = tList;
			taskProgBars = new Dictionary<int, ProgressBar>();
		}

		void TaskAdded(object sender, ItemAddRemoveEventArgs<Task> e)
		{
			if (IsTaskIDTaken(e.Item.TaskID))
				e.Item.TaskID = GetAvailableTaskID();
			TaskAdded(e.Item);
		}

		void TaskAdded(Task task)
		{
			ProgressBar tProgBar = new ProgressBar();
			tProgBar.Text = task.Status;
			taskProgBars.Add(task.TaskID, tProgBar);
			mainVBox.PackEnd(tProgBar, false, true, 0);

			task.StatusChange += (o, args) =>
				Invoke((o2, args2) => { tProgBar.Text = args.Status; });

			task.ProgressChange += (o, args) =>
				Invoke((o2, args2) => { tProgBar.Fraction = args.Progress / 100; });

			task.Completed += (o, args) => 
				Invoke((o2, args2) =>
			{
				Console.WriteLine("Task {0} complete.", task.TaskID);
				taskProgBars.Remove(task.TaskID);
				mainVBox.Remove(tProgBar);
			});

			tProgBar.Visible = true;
		}

		#region Glade Handlers
		void OnNewInstanceClicked(object sender, EventArgs e)
		{
			if (NewInstClicked != null)
				NewInstClicked(this, EventArgs.Empty);
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
		#endregion

		#region Glade Widgets
		[Widget]
		ToolButton newInstButton;

		[Widget]
		ToolButton viewInstFolderButton;

		[Widget]
		ToolButton refreshButton;

		[Widget]
		ToolButton settingsButton;
		
		[Widget]
		ToolButton updateButton;

		[Widget]
		ToolButton helpButton;
		
		[Widget]
		ToolButton aboutButton;

		[Widget]
		IconView instView;

		[Widget]
		VBox mainVBox;
		#endregion

		public void LoadInstances()
		{
			//throw new NotImplementedException();
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

		public event EventHandler NewInstClicked;

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
			get { throw new NotImplementedException(); }
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
	}
}
