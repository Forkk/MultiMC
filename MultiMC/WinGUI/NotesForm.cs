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
	public partial class NotesForm : WinFormsDialog, INotesDialog
	{
		public NotesForm()
		{
			InitializeComponent();
		}

		public string Notes
		{
			get { return textBoxNotes.Text; }
			set { textBoxNotes.Text = value; }
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			OnResponse(DialogResponse.OK);
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			OnResponse(DialogResponse.Cancel);
		}
	}
}
