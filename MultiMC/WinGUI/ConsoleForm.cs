using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using MultiMC.GUI;

namespace MultiMC.WinGUI
{
	public partial class ConsoleForm : WinFormsWindow, IConsoleWindow
	{
		Instance inst;

		public ConsoleForm(Instance inst)
		{
			this.ShowConsole = AppSettings.Main.ShowConsole;

			this.inst = inst;
			Text = inst.Name + " is running...";

			InitializeComponent();

			Message("Instance started with command: " + inst.InstProcess.StartInfo.FileName +
				 " " + inst.InstProcess.StartInfo.Arguments.ToString());

			inst.InstQuit += new EventHandler<InstQuitEventArgs>(InstQuit);

			inst.InstProcess.OutputDataReceived += new DataReceivedEventHandler(InstOutput);
			inst.InstProcess.ErrorDataReceived += new DataReceivedEventHandler(InstOutput);

			inst.InstProcess.BeginOutputReadLine();
			inst.InstProcess.BeginErrorReadLine();

			trayIcon.Visible = true;

			showConsoleToolStripMenuItem.Checked = ShowConsole;
		}

		public void Message(string text)
		{
			if (instConsole == null || text == null) return;
			if (InvokeRequired)
			{
				ConsoleMessageCallback d = new ConsoleMessageCallback(Message);
				if (IsHandleCreated && !IsDisposed)
					Invoke(d, text);
			}
			else
			{
				if (!instConsole.IsDisposed)
					instConsole.AppendText(text + "\n");
			}
		}

		private delegate void ConsoleMessageCallback(string text);

		void InstOutput(object sender, DataReceivedEventArgs e)
		{
			Message(e.Data);
		}

		void InstQuit(object sender, InstQuitEventArgs e)
		{
			// If the handle has not been created, wait for it to be created.
			if (!IsHandleCreated)
			{
				HandleCreated += (sender2, e2) => InstQuit(sender, e);
			}
			else
			{
				try
				{
					this.Invoke(new EventHandler(InstClose));
				}
				catch (InvalidOperationException error)
				{
					Console.WriteLine("Invalid operation: " + error.ToString());
					//MessageBox.Show("Invalid operation: " + error.ToString());
				}
			}
		}

		void InstClose(object sender = null, EventArgs e = default(EventArgs))
		{
			buttonClose.Enabled = true;
			if (AppSettings.Main.AutoCloseConsole || !ShowConsole)
			{
				Close();
			}
		}

		public bool ShowConsole
		{
			get { return AppSettings.Main.ShowConsole; }
			set { AppSettings.Main.ShowConsole = value; this.Visible = ShowConsole; }
		}

		public event EventHandler ConsoleClosed;

		private void ConsoleForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (ConsoleClosed != null)
				ConsoleClosed(this, EventArgs.Empty);

			trayIcon.Visible = false;
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			if (!inst.Running)
				Close();
		}

		private void showConsoleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowConsole = !ShowConsole;
			showConsoleToolStripMenuItem.Checked = ShowConsole;
		}

		private void ConsoleForm_Shown(object sender, EventArgs e)
		{
			this.Visible = ShowConsole;
		}

		private void buttonHide_Click(object sender, EventArgs e)
		{
			ShowConsole = false;
		}
	}
}
