// 
//  Copyright 2012  Andrew Okin
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Diagnostics;

using MultiMC.Data;

using Gtk;
using Gdk;

namespace MultiMC
{
	public partial class ConsoleWindow : Gtk.Window
	{
		public Instance Inst
		{
			get;
			protected set;
		}
		
		StatusIcon statusIcon;
		
		public ConsoleWindow(Instance inst) : 
				base(Gtk.WindowType.Toplevel)
		{
			// Build the GUI
			this.Build();
			this.Deletable = false;
			
			// If the user has show console on, show the window
			this.Visible = AppSettings.Main.ShowConsole;
			
			// Add a listener for when the instance quits
			Inst = inst;
			Inst.InstQuit += OnInstQuit;
			
			// Add formatting tags to the text buffer
			// Base tag
			TextTag baseTag = new TextTag("base");
			baseTag.Font = "Courier New";
			ConsoleView.Buffer.TagTable.Add(baseTag);
			
			// Standard output tag
			TextTag stdoutTag = new TextTag("std");
			ConsoleView.Buffer.TagTable.Add(stdoutTag);
			
			// Error message tag
			TextTag errorTag = new TextTag("err");
			errorTag.ForegroundGdk = new Gdk.Color(255, 0, 0);
			ConsoleView.Buffer.TagTable.Add(errorTag);
			
			// Misc message tag
			TextTag miscTag = new TextTag("etc");
			miscTag.ForegroundGdk = new Gdk.Color(0, 0, 255);
			ConsoleView.Buffer.TagTable.Add(miscTag);
			
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
			statusIcon = new StatusIcon(Pixbuf.LoadFromResource("MainIcon"));
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
			MenuItem killMenuItem = new MenuItem("Kill Minecraft");
			killMenuItem.Activated += (sender, e) => 
			{
				MessageDialog confirmDlg = new MessageDialog(this, 
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
			
			if (OSUtils.Windows)
				StyleUtils.DeuglifyMenu(trayMenu);
			
			trayMenu.ShowAll();
			statusIcon.PopupMenu += (object o, PopupMenuArgs args) => 
				statusIcon.PresentMenu(trayMenu, (uint)args.Args[0], (uint)args.Args[1]);
			
			Message("Instance started with command: " + 
			        inst.InstProcess.StartInfo.FileName +
			        " " + inst.InstProcess.StartInfo.Arguments.ToString());
		}
		
		void AttachOutputListeners()
		{
			// Attach to the instance's process to listen for output
			Inst.InstProcess.OutputDataReceived += OnInstProcOutput;
			Inst.InstProcess.ErrorDataReceived += OnInstProcError;
			
			Inst.InstProcess.BeginOutputReadLine();
			Inst.InstProcess.BeginErrorReadLine();
		}

		void OnInstProcOutput(object sender, System.Diagnostics.DataReceivedEventArgs e)
		{
			Message(e.Data, "std");
		}
		
		void OnInstProcError(object sender, DataReceivedEventArgs e)
		{
			Message(e.Data, "err");
		}
		
		void Message(string msg, string tagName = "etc", bool appendNewLine = true)
		{
			Application.Invoke(
				(sender, e) => 
			{
				TextBuffer buf = ConsoleView.Buffer;
				TextIter end = buf.EndIter;
				buf.InsertWithTags(ref end,
				                   msg + (appendNewLine ? "\n" : ""),
				                   buf.TagTable.Lookup("base"), 
				                   buf.TagTable.Lookup(tagName));
				ConsoleView.ScrollToIter(buf.EndIter, 0.4, true, 0.0, 1.0);
			});
		}
		
		void OnInstQuit(object sender, EventArgs e)
		{
			Application.Invoke(
				(sender2, e2) => 
			{
				Message("Instance quit");
				this.Deletable = true;
				if (AppSettings.Main.AutoCloseConsole || !ShowConsole)
				{
					CloseConsole();
					return;
				}
				CloseButton.Sensitive = true;
			});
		}

		protected void OnCloseButtonClicked(object sender, System.EventArgs e)
		{
			if (!Inst.Running)
				CloseConsole();
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
		
		private bool ShowConsole
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
		
		public event EventHandler ConsoleClosed;
	}
}
