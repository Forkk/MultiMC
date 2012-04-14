using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;

namespace MultiMC.WinGUI
{
	public partial class ChangelogDialog : WinFormsDialog
	{
		public ChangelogDialog()
		{
			InitializeComponent();
			LoadChangelog();
		}

		private void LoadChangelog()
		{
			buttonRefresh.Enabled = false;
			Thread loadThread = new Thread(() =>
				{
					try
					{
						WebClient webCl = new WebClient();
						string chlog = 
							webCl.DownloadString(Properties.Resources.ChangelogURL);
						Invoke((o, args) => ChangelogText = chlog);
					}
					catch (WebException ex)
					{
						Invoke((o, args) => ChangelogText = ex.Message);
					}
					finally
					{
						Invoke((o, args) => buttonRefresh.Enabled = true);
					}
				});
			ChangelogText = "Loading...";
			loadThread.Start();
		}

		private void buttonRefresh_Click(object sender, EventArgs e)
		{
			LoadChangelog();
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		public string ChangelogText
		{
			get { return textBoxChangelog.Text; }
			set { textBoxChangelog.Text = DataUtils.ConvertLineEndings(value); }
		}
	}
}
