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
	public partial class NewInstDialog : WinFormsDialog, IAddInstDialog
	{
		public NewInstDialog()
		{
			InitializeComponent();

			okButton.Click += (o, e) => OnResponse(DialogResponse.OK);
			cancelButton.Click += (o, e) => OnResponse(DialogResponse.Cancel);
		}

		private void textBoxTextChanged(object sender, EventArgs e)
		{

		}

		public string InstName
		{
			get
			{
				return textBox1.Text;
			}
			set
			{
				textBox1.Text = value;
			}
		}
	}
}
