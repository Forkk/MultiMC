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
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

using Gtk;

namespace MultiMC
{
	class MainClass
	{
		/// <summary>
		/// If true, updates will be installed after the program exits.
		/// </summary>
		public static bool InstallUpdates
		{
			get { return updates; }
			set { updates = value; }
		}

		static bool updates;

		/// <summary>
		/// If true, the program will restart after updating.
		/// </summary>
		public static bool RestartAfterUpdate
		{
			get { return restart; }
			set { restart = value; }
		}

		static bool restart;
		
		public static void Main(string[] args)
		{
			Application.Init();
			
			if (args.Length > 0)
			{
				if (args[0].Equals("-u"))
				{
					Console.WriteLine("Updating MultiMC...");
					if (args.Length < 2)
					{
						Console.WriteLine("Invalid number of arguments for -u.");
						Environment.Exit(-1);
					}
					else
					{
						DoUpdate(args[1]);
					}
					Environment.Exit(0);
				}
				else if (args[0].Equals("-v"))
				{
					Console.WriteLine(AppUtils.GetVersion());
					return;
				}
				else
				{
					Console.WriteLine("Unknown argument: " + args[0]);
				}
			}
			
			AppDomain.CurrentDomain.UnhandledException += (sender, ueArgs) => 
			{
				if ((ueArgs.ExceptionObject as Exception) != null)
					FatalException(ueArgs.ExceptionObject as Exception);
				else
				{
					Console.WriteLine("Fatal error: " + ueArgs.ToString());
					Environment.Exit(1);
				}
			};
			
			GLib.ExceptionManager.UnhandledException += (GLib.UnhandledExceptionArgs ueArgs) => 
			{
				if ((ueArgs.ExceptionObject as Exception) != null)
					FatalException(ueArgs.ExceptionObject as Exception);
				else
				{
					Console.WriteLine("Fatal error: " + ueArgs.ToString());
					Environment.Exit(1);
				}
			};
			
			
			
			if (File.Exists(Resources.NewVersionFileName))
				File.Delete(Resources.NewVersionFileName);
			
			MainWindow win = new MainWindow();
			win.Show();
			
			Application.Run();
			
			if (InstallUpdates)
			{
				string currentFile = 
					new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
				Console.WriteLine(string.Format("-u \"{0}\"", currentFile));
				
				Process.Start(Resources.NewVersionFileName,
				              string.Format("-u \"{0}\"", currentFile));
			}
		}
		
		public static void FatalError(string errorMessage, string title)
		{
			MessageDialog errorDialog = new MessageDialog(null,
					DialogFlags.Modal,
					MessageType.Error,
					ButtonsType.Ok,
					errorMessage);
			errorDialog.Title = title;
			errorDialog.Response += (o, args) => errorDialog.Destroy();
			errorDialog.Show();
			Application.Quit();
			Environment.Exit(1);
		}
		
		public static void FatalException(Exception e)
		{
			string errorMessage = string.Format(
				"MultiMC has encountered a fatal error and needs to close. " +
				"Sorry for the inconvenience.\n" +
				"Technical Details:\n" +
				"Exception type: {0}\n" +
				"Message: {1}\n\n" +
				"ToString: {2}",
				e.GetType().ToString(),
				e.Message,
				e.ToString());
			FatalError(errorMessage, "Unknown Error");
		}
		
		/// <summary>
		/// Updates the target file by replacing it with the current file.
		/// </summary>
		/// <param name='target'>
		/// The file to update
		/// </param>
		public static void DoUpdate(string target)
		{
			try
			{
				if (File.Exists(target))
				{
					bool success = false;
					
					const int timeout = 5000;
					int start = DateTime.Now.Millisecond;
					while (DateTime.Now.Millisecond < start + timeout)
					{
						try
						{
							File.Delete(target);
							if (!File.Exists(target))
							{
								success = true;
								break;
							}
						} catch (IOException e)
						{
							if (e.Message.ToLower().Contains("in use"))
								continue;
							else
							{
								success = false;
								
								string errorStr = "Failed to install updates: " +
									e.Message;
								MessageUtils.ShowMessageBox(MessageType.Error, 
								                            errorStr);
								return;
							}
						}
					}
					if (!success)
					{
						MessageUtils.ShowMessageBox(MessageType.Error, 
						                            "Failed to install updates " +
						                            "because the operation timed " +
						                            "out. This may be due to " +
						                            "MultiMC not closing properly.");
						return;
					}
					else
					{
						string currentFile = 
							new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
						File.Copy(currentFile, target);
					}
				}
				
				Process.Start(target);
			} catch (IOException e)
			{
				MessageUtils.ShowMessageBox(MessageType.Error,
				                            "Failed to install updates: " +
				                            e.Message);
				return;
			}
		}
	}
}
