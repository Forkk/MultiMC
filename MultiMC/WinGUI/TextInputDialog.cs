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
	public partial class TextInputDialog : WinFormsDialog, ITextInputDialog
	{
		public TextInputDialog(string message = "", string text = "")
		{
			InitializeComponent();

			Message = message;
			Input = text;

			okButton.Click += (o, e) => OnResponse(DialogResponse.OK);
			cancelButton.Click += (o, e) => OnResponse(DialogResponse.Cancel);

			Shown += new EventHandler(TextInputDialog_Shown);
		}

		void TextInputDialog_Shown(object sender, EventArgs e)
		{
			textBoxInput.Focus();
		}

		private void textBoxTextChanged(object sender, EventArgs e)
		{

		}

		public string Message
		{
			get { return labelMessage.Text; }
			set { labelMessage.Text = value; }
		}

		public string Input
		{
			get { return textBoxInput.Text; }
			set { textBoxInput.Text = value; }
		}

		public void HighlightText()
		{
			textBoxInput.SelectAll();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			OnResponse(DialogResponse.OK);
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			OnResponse(DialogResponse.Cancel);
		}
	}
}
