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
	public class TaskDialog : GTKDialog, ITaskDialog
	{
		[Widget]
		VBox vboxTask = null;

		[Widget]
		Label statusLabel = null;

		[Widget]
		ProgressBar taskProgBar = null;

		public TaskDialog(Task task = null)
		{
			XML gxml = new XML(null, "MultiMC.GTKGUI.TaskDialog.glade",
				"vboxTask", null);
			gxml.Autoconnect(this);

			this.Remove(this.VBox);
			this.Add(vboxTask);

			WidthRequest = 400;
			HeightRequest = 110;

			if (task != null)
				WindowUtils.ConnectTask(this, task);
		}

		public string TaskStatus
		{
			get { return statusLabel.Text; }
			set { statusLabel.Text = value; }
		}

		public int TaskProgress
		{
			get { return (int)(taskProgBar.Fraction * 100); }
			set { taskProgBar.Fraction = value / 100; }
		}
	}
}
