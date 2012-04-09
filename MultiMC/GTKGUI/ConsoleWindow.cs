using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

using Gtk;
using Glade;

using MultiMC.GUI;
using MultiMC.ProblemDetection;

namespace MultiMC.GTKGUI
{
	public class ConsoleWindow : GTKWindow, IConsoleWindow
	{
		public Instance Inst
		{
			get;
			protected set;
		}

		StatusIcon statusIcon;

		[Widget]
		TextView consoleView;

		[Widget]
		Button closeButton;

		public ConsoleWindow(Instance inst) :
			base("MultiMC Console")
		{
			// Build the GUI
			this.Deletable = false;

			// If the user has show console on, show the window
			this.Visible = AppSettings.Main.ShowConsole;

			// Add a listener for when the instance quits
			Inst = inst;
			Inst.InstQuit += OnInstQuit;

			// Add formatting tags to the text buffer
			// Base tag
			using (TextTag baseTag = new TextTag("base"))
			{
				baseTag.Font = "Courier New";
				consoleView.Buffer.TagTable.Add(baseTag);
			}

			// Standard output tag
			using (TextTag stdoutTag = new TextTag("std"))
			{
				consoleView.Buffer.TagTable.Add(stdoutTag);
			}

			// Error message tag
			using (TextTag errorTag = new TextTag("err"))
			{
				errorTag.ForegroundGdk = new Gdk.Color(255, 0, 0);
				consoleView.Buffer.TagTable.Add(errorTag);
			}

			// Misc message tag
			using (TextTag miscTag = new TextTag("etc"))
			{
				miscTag.ForegroundGdk = new Gdk.Color(0, 0, 255);
				consoleView.Buffer.TagTable.Add(miscTag);
			}

			// Listen for output from the instance
			if (Inst.Running)
			{
				AttachOutputListeners();
			}
			else
			{
				Inst.InstLaunch += (sender, e) =>
				{
					AttachOutputListeners();
					Message("Instance started");
				};
			}

			// Add the tray icon
			statusIcon = new StatusIcon(
				Gdk.Pixbuf.LoadFromResource("MultiMC.Resources.MultiMC32.png"));
			statusIcon.Tooltip = "MultiMC Console";
			statusIcon.Activate += (sender, e) => ShowConsole = !ShowConsole;

			// Make a context menu for the icon
			Menu trayMenu = new Menu();

			// Show / hide console
			MenuItem showMenuItem = new MenuItem((ShowConsole ? "Hide Console" : "Show Console"));
			showMenuItem.Activated += (sender, e) =>
			{
				ShowConsole = !ShowConsole;
				(showMenuItem.Child as Label).Text =
					(ShowConsole ? "Hide Console" : "Show Console");
			};
			trayMenu.Add(showMenuItem);

			// Kill Minecraft
			using (MenuItem killMenuItem = new MenuItem("Kill Minecraft"))
			{
				killMenuItem.Activated += (sender, e) =>
				{
					Gtk.MessageDialog confirmDlg = new Gtk.MessageDialog(this,
																	DialogFlags.Modal,
																	MessageType.Warning,
																	ButtonsType.OkCancel,
																	"Killing Minecraft can " +
																	"cause you to lose saves " +
																	"and other things. " +
																	"Are you sure?");
					confirmDlg.Title = "Warning";
					confirmDlg.Response += (object o, ResponseArgs args) =>
					{
						if (args.ResponseId == ResponseType.Ok)
						{
							Inst.InstProcess.Kill();
						}
						confirmDlg.Destroy();
					};
					confirmDlg.Run();
				};
				trayMenu.Add(killMenuItem);
			}

			trayMenu.ShowAll();
			statusIcon.PopupMenu += (object o, PopupMenuArgs args) =>
				statusIcon.PresentMenu(trayMenu, (uint)args.Args[0], (uint)args.Args[1]);

			if (inst.InstProcess != null && inst.InstProcess.StartInfo != null)
				Message("Instance started with command: " +
						inst.InstProcess.StartInfo.FileName +
						" " + inst.InstProcess.StartInfo.Arguments.ToString());
			else
				Message("Instance started");
		}

		void AttachOutputListeners()
		{
			// Attach to the instance's process to listen for output
			Inst.InstProcess.OutputDataReceived += OnInstProcOutput;
			Inst.InstProcess.ErrorDataReceived += OnInstProcError;

			Inst.InstProcess.BeginOutputReadLine();
			Inst.InstProcess.BeginErrorReadLine();
		}

		void OnInstProcOutput(object sender, DataReceivedEventArgs e)
		{
			Message(e.Data, "std");

			Console.WriteLine(e.Data);

			ScanOutput(e.Data);
		}

		// Error detection values
		public const string WrongVersionErrorMsg = "MultiMC detected an error. " +
			"This might mean that " +
				"you're using the wrong version of some of your mods. " +
				"Try downloading the correct version of your mods " +
				"and try again.";

		void OnInstProcError(object sender, DataReceivedEventArgs e)
		{
			if (e == null || e.Data == null)
			{
				Message("", "err");
				return;
			}

			Message(e.Data, "err");

			ScanOutput(e.Data);
		}

		void ScanOutput(string mcOutput)
		{
			if (mcOutput == null)
				return;

			bool killMC = false;
			IMinecraftProblem prob = Problems.GetRelevantProblem(mcOutput);
			if (prob != null)
			{
				Application.Invoke(
					(sender2, e2) =>
					{
						if (!Visible)
							Show();
						else
							GdkWindow.Raise();
						GUI.MessageDialog.Show(this,
							prob.GetErrorMessage(mcOutput),
							"You dun goofed!");
					});
				killMC = prob.ShouldTerminate(mcOutput);
				ErrorOccurred = killMC;
			}

			if (killMC)
			{
				new Thread(() =>
				{
					Thread.Sleep(1000);
					if (Inst.InstProcess != null &&
						!Inst.InstProcess.HasExited)
						Inst.InstProcess.Kill();
				}).Start();
			}
		}

		/// <summary>
		/// Maximum number of characters to be kept in the console at any time.
		/// When the console text is truncated, it will be shortened to this length.
		/// </summary>
		const int ConsoleTruncateLength = 10000;

		/// <summary>
		/// Threshold for console truncation, when the length of the text in the console
		/// is greater than or equal to this value, it will be truncated.
		/// </summary>
		const int ConsoleTruncateThreshold = ConsoleTruncateLength + 1000;

		public void Message(string msg)
		{
			Message(msg);
		}

		public void Message(string msg, string tagName = "etc", bool appendNewLine = true)
		{
			Application.Invoke(
				(sender, e) =>
				{
					TextBuffer buf = consoleView.Buffer;
					TextIter end = buf.EndIter;
					buf.InsertWithTags(ref end,
									   msg + (appendNewLine ? "\n" : ""),
									   buf.TagTable.Lookup("base"),
									   buf.TagTable.Lookup(tagName));

					consoleView.ScrollToIter(buf.EndIter, 0.4, true, 0.0, 1.0);

					if (buf.CharCount > ConsoleTruncateThreshold)
					{
						TruncateConsole(buf, ConsoleTruncateLength);
					}
				});
		}

		void TruncateConsole(TextBuffer buf, int toLength)
		{
			int pre = buf.CharCount;

			TextIter startIter = buf.StartIter;
			TextIter endIter = buf.StartIter;
			endIter.ForwardChars(buf.CharCount - toLength);
			buf.Delete(ref startIter, ref endIter);

			Console.WriteLine("Truncated console. Pre-trunc: {0} Post-trunc: {1}",
				pre, buf.CharCount);
		}

		void OnInstQuit(object sender, InstQuitEventArgs e)
		{
			Application.Invoke(
				(sender2, e2) =>
				{
					Message("Instance quit");

					this.Deletable = true;
					closeButton.Sensitive = true;
					if (AppSettings.Main.AutoCloseConsole || (!ShowConsole && !ErrorOccurred))
					{
						CloseConsole();
					}
				});
		}

		protected void OnCloseButtonClicked(object sender, System.EventArgs e)
		{
			if (!Inst.Running)
				CloseConsole();
			else
				DebugUtils.Print("Can't close console because instance is still running!");
		}

		protected void OnHideButtonClicked(object sender, System.EventArgs e)
		{
			ShowConsole = false;
			if (!Inst.Running)
				this.Destroy();
		}

		void CloseConsole()
		{
			statusIcon.Visible = false;

			Destroy();
			if (ConsoleClosed != null)
				ConsoleClosed(this, EventArgs.Empty);
		}

		public bool ShowConsole
		{
			get
			{
				return AppSettings.Main.ShowConsole;
			}
			set
			{
				Visible = value;
				AppSettings.Main.ShowConsole = value;
			}
		}

		private bool ErrorOccurred
		{
			get;
			set;
		}

		public event EventHandler ConsoleClosed;

		void OnHideClicked(object sender, EventArgs e)
		{

		}

		void OnCloseClicked(object sender, EventArgs args)
		{

		}
	}
}
