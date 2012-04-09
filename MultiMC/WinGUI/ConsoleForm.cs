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
using MultiMC.ProblemDetection;

namespace MultiMC.WinGUI
{
	public partial class ConsoleForm : WinFormsWindow, IConsoleWindow
	{
		bool gameCrashed;

		Instance inst;

		public ConsoleForm(Instance inst)
		{
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

			trayIcon.DoubleClick += (o, args) => ShowConsole = !ShowConsole;
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
			IMinecraftProblem prob = Problems.GetRelevantProblem(e.Data);

			if (prob != null)
			{
				string errorMsg = prob.GetErrorMessage(e.Data);

				MultiMC.GUI.MessageDialog.Show(this, errorMsg, "Error Detected");
				if (prob.ShouldTerminate(e.Data) && AppSettings.Main.QuitIfProblem)
				{
					KillMinecraft(false);
				}
			}

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
				if (InvokeRequired)
				{
					try
					{
						this.Invoke((o, args) => InstQuit(sender, e));
					}
					catch (InvalidOperationException error)
					{
						Console.WriteLine("Invalid operation: " + error.ToString());
					}
				}
				else
				{
					Message(string.Format("Instance exited with code: {0}", e.ExitCode));
					if (e.ExitCode != 0)
					{
						gameCrashed = true;
						this.Visible = true;
						Message("Crash detected!");
					}

					buttonClose.Enabled = true;
					if ((AppSettings.Main.AutoCloseConsole || !this.Visible) && !gameCrashed)
					{
						Close();
					}
				}
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

			buttonClose.Enabled = false;
		}

		private void buttonHide_Click(object sender, EventArgs e)
		{
			ShowConsole = false;
		}

		private void killMinecraftToolStripMenuItem_Click(object sender, EventArgs e)
		{
			KillMinecraft();
		}

		private void KillMinecraft(bool confMessage = true)
		{
			DialogResponse response = confMessage ? DialogResponse.No : DialogResponse.Yes;
			if (confMessage)
				response = MultiMC.GUI.MessageDialog.Show(this, 
					"Are you sure you want to kill Minecraft?",
					"Really?", MessageButtons.YesNo);

			if (response == DialogResponse.Yes)
			{
				if (inst.InstProcess != null)
					inst.InstProcess.Kill();
			}
		}
	}
}
