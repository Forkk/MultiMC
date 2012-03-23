using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MultiMC.WinGUI
{
	public partial class DeleteConfirmForm : WinFormsDialog
	{
		public DeleteConfirmForm()
		{
			InitializeComponent();
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			OnResponse(GUI.DialogResponse.OK);
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			OnResponse(GUI.DialogResponse.Cancel);
		}
	}
}
