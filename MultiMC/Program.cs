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
using System.Reflection;

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
			// Register a callback with AssemblyResolve to load embedded DLLs
			AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
			{
				String resourceName = "AssemblyLoadingAndReflection." +
					new AssemblyName(e.Name).Name + ".dll";

				using (var stream = Assembly.GetExecutingAssembly().
					GetManifestResourceStream(resourceName))
				{
					Byte[] assemblyData = new Byte[stream.Length];
					stream.Read(assemblyData, 0, assemblyData.Length);
					return Assembly.Load(assemblyData);
				}
			};

			if (!args.Contains("-v"))
				Console.WriteLine("Operating System: {0}", OSUtils.OS.ToString());

			if (OSUtils.OS == OSEnum.Linux && !args.Contains("-v"))
			{
				if (Environment.CurrentDirectory.Equals(Environment.GetEnvironmentVariable("HOME")))
				{
					string workingDir = Path.GetDirectoryName(AppUtils.ExecutableFileName);
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

			if (!File.Exists(AppSettings.Main.JavaPath) ||
				AppSettings.Main.JavaPath == "java")
			{
				Console.WriteLine("Detecting Java path.");
				try
				{
					AppSettings.Main.AutoDetectJavaPath();
					AppSettings.Main.Save();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			}

			// If we're not on Windows or not using Mono, use GTK#.
			if (OSUtils.OS != OSEnum.Windows || 
				OSUtils.Runtime == Runtime.Mono)
				Toolkit = WindowToolkit.GtkSharp;
			else
				Toolkit = WindowToolkit.WinForms;

			if (args.Contains("--gtk"))
				Toolkit = WindowToolkit.GtkSharp;
			else if (args.Contains("--winforms"))
				Toolkit = WindowToolkit.WinForms;

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
								MessageDialog.Show(null, errorStr, "Error");
								return;
							}
						}
					}
					if (!success)
					{
						File.WriteAllText("update-error.txt", 
							Properties.Resources.UpdateTimeoutMessage);
						MessageDialog.Show(null, Properties.Resources.UpdateTimeoutMessage, 
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
				MessageDialog.Show(null, string.Format("Failed to install updates: {0}", e.Message));
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
