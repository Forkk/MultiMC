using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MultiMC.GUI;
using MultiMC.Tasks;

namespace MultiMC.WinGUI
{
	public partial class TaskDialog : WinFormsDialog, ITaskDialog
	{
		public TaskDialog(Task task = null)
		{
			InitializeComponent();

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
			get { return taskProgressBar.Value; }
			set
			{
				taskProgressBar.Value = value;
				taskProgressBar.Style = (value == 0 ? 
					ProgressBarStyle.Marquee : ProgressBarStyle.Continuous);
			}
		}
	}
}
