using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MultiMC.GUI;

namespace MultiMC.WinGUI
{
	public partial class UpdateDialog : WinFormsDialog, IUpdateDialog
	{
		public UpdateDialog()
		{
			InitializeComponent();
		}

		public event EventHandler ViewChangelogClicked;

		public string Message
		{
			get { return labelMessage.Text; }
			set { labelMessage.Text = value; }
		}

		private void buttonChangelog_Click(object sender, EventArgs e)
		{
			if (ViewChangelogClicked != null)
				ViewChangelogClicked(this, EventArgs.Empty);
		}
	}
}
