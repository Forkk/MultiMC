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
#if !DEBUG && FORCE_MONO
			if (OSUtils.Windows && !OSUtils.IsOnMono())
			{
				Console.WriteLine("Switching to Mono...");
				string pfdir = null;
				if (Directory.Exists(@"C:\Program Files (x86)\"))
					pfdir = @"C:\Program Files (x86)";
				else
					pfdir = @"C:\Program Files";
				string monoPath = Path.Combine(pfdir, "Mono-2.10.8", "bin", "mono.exe");
				if (!File.Exists(monoPath))
				{
					string monodir = null;
					foreach (string dir in Directory.GetFileSystemEntries(pfdir))
						if (Path.GetFileName(dir).ToLower().StartsWith("mono-"))
							monodir = Path.Combine(pfdir, dir);
					monoPath = Path.Combine(monodir, "bin", "mono.exe");
				}
				
				if (!string.IsNullOrEmpty(monoPath))
				{
					try
					{
						ProcessStartInfo info = new ProcessStartInfo(monoPath, 
						                                             Resources.ExecutableFileName);
						info.UseShellExecute = false;
						info.CreateNoWindow = true;
						Process.Start(info);
					} catch (Exception e)
					{
						File.WriteAllText("LaunchError.txt", 
						                  "MultiMC failed to start. Please report this problem." +
						                  "\nPath: " +
						                  monoPath + "\n" + e.ToString());
					}
					Environment.Exit(0);
				}
				else
				{
					Console.WriteLine("Failed to switch to mono.");
				}
			}
#endif
			
			if (OSUtils.Linux)
			{
				if (Environment.CurrentDirectory.Equals(Environment.GetEnvironmentVariable("HOME")))
				{
					string workingDir = Resources.ExecutableFileName;
					Environment.CurrentDirectory = workingDir;
					Console.WriteLine("Set working directory to {0}", workingDir);
				}
			}
			
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
			
			AppDomain.CurrentDomain.UnhandledException += (object sender, 
			                                               UnhandledExceptionEventArgs ueArgs) => 
			{
				OnException(ueArgs.ExceptionObject as Exception);
			};
			
			GLib.ExceptionManager.UnhandledException += (GLib.UnhandledExceptionArgs ueArgs) => 
			{
				OnException(ueArgs.ExceptionObject as Exception);
			};
			
			if (File.Exists(Resources.NewVersionFileName))
				File.Delete(Resources.NewVersionFileName);
			
			MainWindow win = new MainWindow();
			win.Show();
			
			try
			{
				Application.Run();
			} catch (Exception e)
			{
				OnException(e);
			}
			
			if (InstallUpdates)
			{
				string currentFile = Resources.ExecutableFileName;
				Console.WriteLine(string.Format("{0} -u \"{1}\"", 
				                                Resources.NewVersionFileName, currentFile));
				if (OSUtils.Linux)
				{
					Process.Start("chmod",
					              string.Format("+x \"{0}\"", Resources.NewVersionFileName));
					ProcessStartInfo info = 
						new ProcessStartInfo(Resources.NewVersionFileName,
						                     string.Format("-u \"{0}\"", currentFile));
					info.UseShellExecute = false;
					Process.Start(info);
				}
				else
				{
					Process.Start(Resources.NewVersionFileName,
					              string.Format("-u \"{0}\"", currentFile));
				}
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
		
		public static void OnException(Exception e)
		{
			// We should immediately close for these exceptions
			if (e is OutOfMemoryException || 
			    e is StackOverflowException ||
				e is AccessViolationException)
			{
				Console.WriteLine("SEVERE: " + e.GetType().ToString() + "! Aborting.");
				File.WriteAllText("error.txt", e.ToString());
				Environment.Exit(-2);
			}
			
			Console.WriteLine("Caught exception:\n" + e.ToString());
			
			if (e is System.Reflection.TargetInvocationException)
				e = e.InnerException;
			
			ExceptionDialog errDlg = new ExceptionDialog(e);
			
			errDlg.Response += (o, args) => 
			{
				if (args.ResponseId == ResponseType.Yes)
					Environment.Exit(-1);
			};
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
						Console.WriteLine("File: " + Resources.NewVersionFileName);
						File.Delete(target);
						File.Copy(Resources.NewVersionFileName, target);
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
