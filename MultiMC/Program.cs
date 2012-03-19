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
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

using MultiMC.GUI;
using MultiMC.ProblemDetection;

namespace MultiMC
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (OSUtils.OS == OSEnum.Linux)
			{
				if (Environment.CurrentDirectory.Equals(Environment.GetEnvironmentVariable("HOME")))
				{
					string workingDir = AppUtils.ExecutableFileName;
					Environment.CurrentDirectory = workingDir;
					Console.WriteLine("Set working directory to {0}", workingDir);
				}
			}

			// Command line arguments
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
						InstallUpdate(args[1]);
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

			if (OSUtils.OS == OSEnum.Windows)
				Toolkit = WindowToolkit.WinForms;
			else
				Toolkit = WindowToolkit.WinForms;

			if (args.Contains("--gtk"))
				Toolkit = WindowToolkit.GtkSharp;
			else if (args.Contains("--winforms"))
				Toolkit = WindowToolkit.WinForms;

			Problems.InitProblems();

			Main main = new Main();
			main.Run();

			if (InstallUpdates)
			{
				string currentFile = AppUtils.ExecutableFileName;
				Console.WriteLine(string.Format("{0} -u \"{1}\"",
												Properties.Resources.NewVersionFileName, 
												currentFile));
				if (OSUtils.OS == OSEnum.Linux)
				{
					Process.Start("gksudo chmod",
								  string.Format("+x \"{0}\"",
								  Properties.Resources.NewVersionFileName));
					ProcessStartInfo info =
						new ProcessStartInfo(Properties.Resources.NewVersionFileName,
											 string.Format("-u \"{0}\"", currentFile));
					info.UseShellExecute = false;
					Process.Start(info);
				}
				else
				{
					Process.Start(Properties.Resources.NewVersionFileName,
								  string.Format("-u \"{0}\"", currentFile));
				}
				Environment.Exit(0);
			}
		}

		private static void InstallUpdate(string target)
		{
			try
			{
				if (File.Exists(target))
				{
					bool success = false;

					const int timeout = 1000 * 15;
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
						}
						catch (IOException e)
						{
							if (e.Message.ToLower().Contains("in use"))
								continue;
							else
							{
								success = false;

								string errorStr = "Failed to install updates: " +
									e.Message;
								MessageBox.Show(null, errorStr, "Error");
								return;
							}
						}
					}
					if (!success)
					{
						File.WriteAllText("update-error.txt", 
							Properties.Resources.UpdateTimeoutMessage);
						MessageBox.Show(null, Properties.Resources.UpdateTimeoutMessage, 
							"Update Timed Out");
						return;
					}
					else
					{
						Console.WriteLine("File: " + Properties.Resources.NewVersionFileName);
						File.Delete(target);
						File.Copy(Properties.Resources.NewVersionFileName, target);
					}
				}
				else
					Console.WriteLine("Couldn't find file to update. '{0}'", target);

				Process.Start(target);
			}
			catch (IOException e)
			{
				MessageBox.Show(null, string.Format("Failed to install updates: {0}", e.Message));
				return;
			}
		}

		public static bool InstallUpdates;
		public static bool RestartAfterUpdate;

		public static WindowToolkit Toolkit
		{
			get;
			private set;
		}
	}

	/// <summary>
	/// An enumeration of window toolkits
	/// </summary>
	public enum WindowToolkit
	{
		WinForms,
		GtkSharp,
	}
}
