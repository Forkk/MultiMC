using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace MultiMC
{
	public class AppSettings : ConfigFile
	{
		public AppSettings()
			: base()
		{

		}

		private static AppSettings inst;

		/// <summary>
		/// The main settings provider.
		/// </summary>
		/// <value>
		/// The main settings provider.
		/// </value>
		public static AppSettings Main
		{
			get
			{
				if (inst != null)
				{
					return inst;
				}
				else
				{
					Console.WriteLine("Loading settings...");
					inst = new AppSettings();
					inst.Load();
					return inst;
				}
			}
		}

		public override void Load(string path = null)
		{
			if (path == null)
				path = Properties.Resources.ConfigFileName;

			base.Load(path);
		}

		public override void Save(string path = null)
		{
			if (path == null)
				path = Properties.Resources.ConfigFileName;

			base.Save(path);
		}

		/// <summary>
		/// Automatically finds the path to where java is installed.
		/// </summary>
		/// <returns>
		/// True if java was found. Otherwise, <c>false</c>
		/// </returns>
		public bool AutoDetectJavaPath()
		{
			if (OSUtils.OS == OSEnum.Windows)
			{
				Console.WriteLine("Finding Java on Windows...");
				string[] possiblePaths = new string[]
				{
					@"C:\Program Files\Java\jre6\bin\java.exe",
					@"C:\Program Files\Java\jre7\bin\java.exe",
					@"C:\Program Files (x86)\Java\jre6\bin\java.exe",
					@"C:\Program Files (x86)\Java\jre7\bin\java.exe",
				};

				foreach (string path in possiblePaths)
				{
					if (File.Exists(path))
					{
						Console.WriteLine("Found Java at {0}", path);
						JavaPath = path;
						return true;
					}
				}
				return false;
			}
			else if (OSUtils.OS == OSEnum.Linux)
			{
				Console.WriteLine("Finding Java on Linux...");
				ProcessStartInfo info = new ProcessStartInfo("which", "java");
				info.UseShellExecute = false;
				info.RedirectStandardOutput = true;
				Process findJavaProc = Process.Start(info);
				string path = findJavaProc.StandardOutput.ReadToEnd();
				path = path.Trim();
				if (File.Exists(path))
				{
					Console.WriteLine("Found Java at {0}", path);
					JavaPath = path;
					return true;
				}
				else
					return false;
			}
			return false;
		}

		public int MinMemoryAlloc
		{
			get { return Int32.Parse(this["MinMemoryAlloc", "512"]); }
			set { this["MinMemoryAlloc"] = value.ToString(); }
		}

		public int MaxMemoryAlloc
		{
			get { return Int32.Parse(this["MaxMemoryAlloc", "1024"]); }
			set { this["MaxMemoryAlloc"] = value.ToString(); }
		}

		public string JavaPath
		{
			get { return this["JavaPath", "java"]; }
			set { this["JavaPath"] = value; }
		}

		public string InstanceDir
		{
			get { return this["InstDir", "instances"]; }
			set { this["InstDir"] = value; }
		}

		public bool ShowConsole
		{
			get { return bool.Parse(this["ShowConsole", "false"]); }
			set { this["ShowConsole"] = value.ToString(); }
		}

		public bool AutoCloseConsole
		{
			get { return bool.Parse(this["AutoCloseConsole", "false"]); }
			set { this["AutoCloseConsole"] = value.ToString(); }
		}

		public bool AutoUpdate
		{
			get { return bool.Parse(this["AutoUpdate", "true"]); }
			set { this["AutoUpdate"] = value.ToString(); }
		}

		public bool EnableHints
		{
			get { return bool.Parse(this["EnableHints", "true"]); }
			set { this["EnableHints"] = value.ToString(); }
		}
	}
}
